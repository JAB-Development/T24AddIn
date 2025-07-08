using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace T24AddIn.Handlers.CalculateGrossAreaHandler
{
    internal class CalculateGrossAreaHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            try
            {
                var doc = app.ActiveUIDocument.Document;
                var wallCollector = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_Walls)
                    .OfClass(typeof(Wall))
                    .WhereElementIsNotElementType()
                    .ToList();

                foreach (var wall in wallCollector)
                {
                    //Parameter mark = wall.LookupParameter("Mark");

                    //if (mark != null && mark.HasValue)
                    //{
                    //    string markValue = mark.AsString();

                    //    if (markValue == "shit")
                    //    {
                    //        var test = 1;
                    //    }
                    //}

                    //Options options = new Options();
                    //options.ComputeReferences = false;
                    //options.DetailLevel = ViewDetailLevel.Coarse;
                    //options.IncludeNonVisibleObjects = false;

                    //GeometryElement geomElem = wall.get_Geometry(options);

                    //double exteriorArea = 0;


                    //LocationCurve locCurve = wall.Location as LocationCurve;
                    //if (locCurve == null) return;

                    //XYZ wallDirection = (locCurve.Curve.GetEndPoint(1) - locCurve.Curve.GetEndPoint(0)).Normalize();

                    //// Rotate direction by 90° to get wall exterior orientation (cross with Z)
                    //XYZ exteriorDirection = new XYZ(-wallDirection.Y, wallDirection.X, 0); // 2D perpendicular vector

                    //foreach (GeometryObject geomObj in geomElem)
                    //{
                    //    Solid solid = geomObj as Solid;
                    //    if (solid == null || solid.Faces.IsEmpty) continue;

                    //    foreach (Face face in solid.Faces)
                    //    {
                    //        XYZ normal = face.ComputeNormal(new UV(0.5, 0.5)).Normalize();
                    //        double dot = normal.DotProduct(exteriorDirection);

                    //        if (dot > 0.9) // Adjust threshold as needed
                    //        {
                    //            exteriorArea += face.Area;
                    //        }
                    //    }
                    //}
                    // Get wall instance length (in feet)
                    double length = wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH)?.AsDouble() ?? 0;

                    // Get wall type (from Symbol)
                    WallType wallType = wall.Document.GetElement(wall.GetTypeId()) as WallType;

                    if (wallType != null)
                    {
                        // Get wall width (in feet)
                        double width = wallType.get_Parameter(BuiltInParameter.WALL_ATTR_WIDTH_PARAM)?.AsDouble() ?? 0;

                        // Get unconnected height (in feet)
                        double height = wall.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM)?.AsDouble() ?? 0;

                        // Compute area (length × width)
                        double totalLength = length + width;
                        double areaSqM = totalLength * height;


                        using (Transaction tx = new Transaction(doc, "Set Gross Area"))
                        {
                            tx.Start();

                            Parameter areaParam = wall.LookupParameter("Gross Area");
                            if (areaParam != null && !areaParam.IsReadOnly)
                            {
                                areaParam.Set(areaSqM);
                            }

                            tx.Commit();
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GetName() => "CalculateGrossAreaHandler";
    }
}
