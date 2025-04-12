using System;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace T24AddIn.Handlers.MoveWallTagHandler
{
    internal class MoveWallTagHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            var doc = app.ActiveUIDocument.Document;

            var tagCollector = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_WallTags)
                .WhereElementIsNotElementType()
                .Where(x => x is IndependentTag) // Ensure valid casting
                .Cast<IndependentTag>()
                .ToList();

            foreach (var wallTag in tagCollector)
            {
                using (var trans = new Transaction(doc, "Add Tags to Doors in All Views"))
                {
                    trans.Start();

                    var tagLocation = wallTag.TagHeadPosition;

                    var (movementVector, conflictingTag) = GetNonOverlappingDistance(wallTag, doc);

                    double tolerance = 0.001;
                    if (movementVector.IsAlmostEqualTo(new XYZ(0.2, 0.2, 0), tolerance)) continue;


                    //var newOrigin = new XYZ(tagLocation.X + movementVector.X, tagLocation.Y + movementVector.Y, tagLocation.Z);

                    ElementId taggedElementId = wallTag.GetTaggedElementIds().FirstOrDefault().HostElementId;

                    Element wallElement= doc.GetElement(taggedElementId);
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
                            double directionY = relativeDirection.X >= 0 ? 1 : -1;

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

                            correctedY += -movementVector.Y ;
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
                                correctedX = (end.X + start.X)/2;
                            }

                            if (newOffsetY <= maxOffset) correctedY += movementVector.Y;

                            //correctedX += movementVector.X;
                            //correctedY += movementVector.Y;
                        }


                        var newOrigin = new XYZ(correctedX, correctedY, tagLocation.Z);

                        wallTag.TagHeadPosition = newOrigin;
                    }

                    trans.Commit();
                }
            }
        }

        public string GetName()
        {
            return "MoveWallTagHandler";
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="currentTag"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        private Tuple<XYZ, IndependentTag> GetNonOverlappingDistance(IndependentTag currentTag, Document doc)
        {
            // Collect all door tags except the current tag
            var otherTags = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_WallTags)
                .WhereElementIsNotElementType()
                .Where(x => x is IndependentTag) // Ensure valid casting
                .Cast<IndependentTag>()
                .Where(t => t.Id != currentTag.Id)
                .ToList();

            //var doorTags = new FilteredElementCollector(doc)
            //    .OfCategory(BuiltInCategory.OST_DoorTags)
            //    .WhereElementIsNotElementType()
            //    .Where(x => x is IndependentTag) // Ensure valid casting
            //    .Cast<IndependentTag>()
            //    .Where(t => t.Id != currentTag.Id)
            //    .ToList();

            //otherTags.AddRange(doorTags);

            // Get the bounding box and position of the current tag
            var currentTagHeaderPosition = currentTag.TagHeadPosition;

            BoundingBoxXYZ currentTagBoundingBox = currentTag.get_BoundingBox(doc.ActiveView);

            if (currentTagBoundingBox == null) return Tuple.Create(new XYZ(0, 0, 0), currentTag);

            double currentTagHalfWidth = (currentTagBoundingBox.Max.X - currentTagBoundingBox.Min.X) / 2;
            double currentTagHalfHeight = (currentTagBoundingBox.Max.Y - currentTagBoundingBox.Min.Y) / 2;

            // Initialize the required distance to zero
            double minOffsetX = 0.2;
            double minOffsetY = 0.2;

            ElementId taggedElementId = currentTag.GetTaggedElementIds().FirstOrDefault().HostElementId;

            Element wallEkElement= doc.GetElement(taggedElementId);


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

            foreach (var tag in otherTags)
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
                    if (isVertical)
                    {
                        if (tagHeaderPosition.Y < currentTagHeaderPosition.Y)
                        {
                            minOffsetY = Math.Max(minOffsetY, overlapY);
                        }
                        else
                        {
                            minOffsetY = Math.Min(minOffsetY, -overlapY);
                        }
                    }
                    else
                    {
                        if (tagHeaderPosition.X < currentTagHeaderPosition.X)
                            minOffsetX = Math.Max(minOffsetX, overlapX);
                        else
                            minOffsetX = Math.Min(minOffsetX, -overlapX-0.5);
                    }
                }
            }

            return Tuple.Create( new XYZ(minOffsetX, minOffsetY, 0), tag1);
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
