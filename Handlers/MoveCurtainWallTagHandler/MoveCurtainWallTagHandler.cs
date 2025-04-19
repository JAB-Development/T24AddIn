using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using T24AddIn.Service.CurtainWall;

namespace T24AddIn.Handlers.MoveCurtainWallTagHandler
{
    internal class MoveCurtainWallTagHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            try
            {
                var doc = app.ActiveUIDocument.Document;

                var curtainWallCollector = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_Walls)
                    .OfClass(typeof(Wall))
                    .WhereElementIsNotElementType()
                    .Where(wallElement =>
                    {
                        WallType wallType = doc.GetElement(wallElement.GetTypeId()) as WallType;

                        if (wallType == null) return false;
                        if (wallType.Kind != WallKind.Curtain) return false;

                        var functionParam = wallType.LookupParameter("Function");
                        bool isCurtain = wallType.Name.Contains("Curtain", StringComparison.OrdinalIgnoreCase);

                        if (!isCurtain) return false;

                        bool isExteriorByFunction = functionParam != null &&
                                                    functionParam.AsValueString()?.Equals("Exterior", StringComparison.OrdinalIgnoreCase) == true;
                        var exteriorParam = wallElement.LookupParameter("Exterior");
                        bool isExteriorByYesNo = exteriorParam is { StorageType: StorageType.Integer } &&
                                                 exteriorParam.AsInteger() == 1;

                        return isExteriorByFunction || isExteriorByYesNo || isCurtain;
                    })
                    .ToList();

                var hostElements = curtainWallCollector.Select(x => x.Id);

                var tags = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_WallTags)
                    .WhereElementIsNotElementType()
                    .Where(x => x is IndependentTag tag // Ensure valid casting
                                && hostElements.Contains(tag.GetTaggedElementIds().FirstOrDefault().HostElementId))
                    .Cast<IndependentTag>()
                    .ToList();

                foreach (var wallTag in tags)
                {
                    using var transaction = new Transaction(doc, "Move Curtain Tag");
                    transaction.Start();

                    var tagLocation = wallTag.TagHeadPosition;

                    var (movementVector, conflictingTag) = GetNonOverlappingDistance(wallTag, doc);

                    double tolerance = 0.001;
                    if (movementVector.IsAlmostEqualTo(new XYZ(0.2, 0.2, 0), tolerance)) continue;

                    ElementId taggedElementId = wallTag.GetTaggedElementIds().FirstOrDefault().HostElementId;

                    Element wallElement = doc.GetElement(taggedElementId);
                    LocationCurve location = wallElement.Location as LocationCurve;
                    Curve curve = location.Curve;

                    XYZ start = curve.GetEndPoint(0);
                    XYZ end = curve.GetEndPoint(1);
                    if (location != null)
                    {
                        bool isVertical = false;
                        if (wallElement is Wall wall)
                        {
                            isVertical = IsWallVertical(wall);
                        }
                        XYZ projectedPoint = curve.Project(tagLocation).XYZPoint;

                        double maxOffset = 1; // Define your max offset limit

                        double correctedX = tagLocation.X;
                        double correctedY = tagLocation.Y;


                        if (conflictingTag != null)
                        {
                            XYZ currentPosition = wallTag.TagHeadPosition;
                            XYZ conflictPosition = conflictingTag.TagHeadPosition;

                            XYZ relativeDirection = currentPosition - conflictPosition;

                            // Flip movement vector to push in the opposite direction
                            double directionX = relativeDirection.X >= 0 ? 1 : -1;
                            double directionY = relativeDirection.Y >= 0 ? 1 : -1;

                            movementVector = new XYZ(
                                Math.Abs(movementVector.X) * directionX,
                                Math.Abs(movementVector.Y) * directionY,
                                movementVector.Z
                            );
                        }


                        if (isVertical)
                        {
                            double newOffsetY = Math.Abs((tagLocation.Y + movementVector.Y) - projectedPoint.Y);

                            //if (newOffsetY <= maxOffset)
                            //{
                            //    correctedY += movementVector.Y;

                            //}

                            correctedY += movementVector.Y;
                        }
                        else
                        {
                            double newOffsetX = Math.Abs((tagLocation.X + movementVector.X));
                            double newOffsetY = Math.Abs((tagLocation.Y + movementVector.Y) - projectedPoint.Y);

                            //double newOffsetX = Math.Abs((tagLocation.X + movementVector.X));
                            //double newOffsetY = Math.Abs((tagLocation.Y + movementVector.Y));

                            if (newOffsetX < tagLocation.X)
                            {
                                correctedX += movementVector.X;
                            }
                            else
                            {
                                correctedX = (end.X + start.X) / 2;
                            }

                            if (newOffsetY <= maxOffset) correctedY += movementVector.Y;

                            //correctedX += movementVector.X;
                            //correctedY += movementVector.Y;
                        }


                        var newOrigin = new XYZ(correctedX, correctedY, tagLocation.Z);

                        wallTag.TagHeadPosition = newOrigin;
                    }

                    transaction.Commit();

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GetName()
        {
            return "MoveCurtainWallTagHandler";
        }

        private Tuple<XYZ, IndependentTag> GetNonOverlappingDistance(IndependentTag currentTag, Document doc)
        {
            try
            {
                var service = new CurtainWallService(doc);
                //var otherTags = service.GetAllCurtainWalls();

                var otherTags = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_WallTags)
                    .WhereElementIsNotElementType()
                    .Where(x => x is IndependentTag) // Ensure valid casting
                    .Cast<IndependentTag>()
                    .ToList();

                var currentTagHeaderPosition = currentTag.TagHeadPosition;

                BoundingBoxXYZ currentTagBoundingBox = currentTag.get_BoundingBox(doc.ActiveView);

                if (currentTagBoundingBox == null) return Tuple.Create(new XYZ(0, 0, 0), currentTag);

                double currentTagHalfWidth = (currentTagBoundingBox.Max.X - currentTagBoundingBox.Min.X) / 2;
                double currentTagHalfHeight = (currentTagBoundingBox.Max.Y - currentTagBoundingBox.Min.Y) / 2;


                double minOffsetX = 0.2;
                double minOffsetY = 0.2;

                double rightOffset = 0.2;
                double leftOffset = 0.2;
                double topOffset = 0.2;
                double bottomOffset = 0.2;

                ElementId taggedElementId = currentTag.GetTaggedElementIds().FirstOrDefault().HostElementId;

                Element wallEkElement = doc.GetElement(taggedElementId);

                Parameter exteriorParam = wallEkElement.LookupParameter("Exterior");


                if (exteriorParam is { StorageType: StorageType.Integer })
                {
                    var isExterior = exteriorParam.AsInteger() == 1;

                    if (isExterior)
                    {
                        var shit = 1;
                    }
                }

                bool isVertical = false;
                if (wallEkElement is Wall wall)
                {
                    isVertical = IsWallVertical(wall);
                }

                var wallLocation = wallEkElement.Location as LocationCurve;
                if (wallLocation == null) return Tuple.Create(new XYZ(0, 0, 0), currentTag);

                IndependentTag tag1 = null;

                foreach (var tag in otherTags.Where(t => t.Id != currentTag.Id))
                {
                    var tagHeaderPosition = tag.TagHeadPosition;
                    BoundingBoxXYZ tagBoundingBox = tag.get_BoundingBox(doc.ActiveView);

                    if (tagBoundingBox == null) continue;

                    double tagHalfWidth = (tagBoundingBox.Max.X - tagBoundingBox.Min.X) / 2;
                    double tagHalfHeight = (tagBoundingBox.Max.Y - tagBoundingBox.Min.Y) / 2;

                    double overlapX = (currentTagHalfWidth + tagHalfWidth) - Math.Abs(currentTagHeaderPosition.X - tagHeaderPosition.X);
                    double overlapY = (currentTagHalfHeight + tagHalfHeight) - Math.Abs(currentTagHeaderPosition.Y - tagHeaderPosition.Y);

                    if (overlapX > 0 && overlapY > 0)
                    {
                        tag1 = tag;
                        //if (isVertical)
                        //{
                        //    if (tagHeaderPosition.Y < currentTagHeaderPosition.Y)
                        //    {
                        //        minOffsetY = Math.Max(minOffsetY, overlapY);
                        //    }
                        //    else
                        //    {
                        //        minOffsetY = Math.Min(minOffsetY, -overlapY);
                        //    }
                        //}
                        //else
                        //{
                        //    if (tagHeaderPosition.X < currentTagHeaderPosition.X)
                        //        minOffsetX = Math.Max(minOffsetX, overlapX);
                        //    else
                        //        minOffsetX = Math.Min(minOffsetX, -overlapX - 0.5);
                        //}

                        if (isVertical)
                        {
                            if (tagHeaderPosition.Y < currentTagHeaderPosition.Y)
                            {
                                // tag is below -> we move current tag up
                                bottomOffset = Math.Max(bottomOffset, overlapY);
                            }
                            else
                            {
                                // tag is above -> we move current tag down
                                topOffset = Math.Max(topOffset, overlapY);
                            }
                        }
                        else
                        {
                            if (tagHeaderPosition.X < currentTagHeaderPosition.X)
                            {
                                // tag is to the left -> move current tag right
                                leftOffset = Math.Max(leftOffset, overlapX);
                            }
                            else
                            {
                                // tag is to the right -> move current tag left
                                rightOffset = Math.Max(rightOffset, overlapX + 0.5);
                            }
                        }
                    }


                }

                double finalOffsetX = rightOffset; // or use logic to prefer left/right
                double finalOffsetY = bottomOffset; // or use logic to prefer up/down

                return Tuple.Create(new XYZ(finalOffsetX, finalOffsetY, 0), tag1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private bool IsWallVertical(Wall wall)
        {
            var locationCurve = wall.Location as LocationCurve;
            if (locationCurve == null) return false;

            var curve = locationCurve.Curve;
            var startPoint = curve.GetEndPoint(0);
            var endPoint = curve.GetEndPoint(1);

            // Calculate the differences in X and Y
            double deltaX = Math.Abs(endPoint.X - startPoint.X);
            double deltaY = Math.Abs(endPoint.Y - startPoint.Y);

            // A wall is vertical if the Y difference is significantly larger than the X difference
            return deltaY > deltaX;
        }


    }
}
