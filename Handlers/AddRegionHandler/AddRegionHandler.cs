using System.Windows.Controls;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Color = Autodesk.Revit.DB.Color;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace T24AddIn.Handlers.AddRegionHandler
{
    public class AddRegionHandler : IExternalEventHandler
    {
        public  List<XYZ> IntersectionPoints { get; set; }
        public  Document Document { get; set; }
        public List<XYZ> SelectedIntersectionPoints { get; set; }
        public Color Color { get; set; } = new Color(0, 0, 0);
        public void Execute(UIApplication app)
        {
            try
            {
                if (IntersectionPoints == null || IntersectionPoints.Count < 3)
                {
                    TaskDialog.Show("Error", "At least 3 intersection points are required to form a closed loop.");
                    return;
                }

                double tolerance = 0.09;


                List<XYZ> filteredIntersectionPoints = new List<XYZ>();
                CurveLoop loop = new CurveLoop();
                var cleanedPoints = new List<XYZ>();

                if (SelectedIntersectionPoints != null && SelectedIntersectionPoints.Count > 0)
                {
                    // Use SelectedIntersctionPints to find the closest matching points from IntersectionPoints
                    var points = IntersectionPoints
                        .Where(p => SelectedIntersectionPoints.Any(sel => sel.DistanceTo(p) <= tolerance))
                        .ToList();
                        
                    if (points.Count < 3)
                    {
                        TaskDialog.Show("Error", "Not enough matching intersection points found to create a loop.");
                        return;
                    }


                    foreach (var selected in SelectedIntersectionPoints)
                    {
                        // Snap selected point to nearest known intersection point
                        var match = IntersectionPoints.FirstOrDefault(p => p.DistanceTo(selected) <= tolerance);
                        if (match != null && !filteredIntersectionPoints.Contains(match))
                        {
                            filteredIntersectionPoints.Add(match);
                        }
                        else
                        {
                            TaskDialog.Show("Warning", "A selected point did not match any known intersection point.");
                            return;
                        }
                    }
                }
                else
                {
                    //XYZ center = new XYZ(
                    //    IntersectionPoints.Average(p => p.X),
                    //    IntersectionPoints.Average(p => p.Y),
                    //    IntersectionPoints.Average(p => p.Z)
                    //);

                    //IntersectionPoints = IntersectionPoints
                    //    .OrderBy(p =>
                    //        Math.Atan2(p.Y - center.Y, p.X - center.X)) // Sort around center
                    //    .ToList();

                    double toleranceDoc = 0.01;



                    foreach (var pt in IntersectionPoints)
                    {
                        if (cleanedPoints.Count == 0 || !pt.IsAlmostEqualTo(cleanedPoints.Last(), toleranceDoc))
                        {
                            cleanedPoints.Add(pt);
                        }
                    }
                }

                for (int i = 0; i < cleanedPoints.Count; i++)
                {
                    XYZ p1 = cleanedPoints[i];
                    XYZ p2 = IntersectionPoints[(i + 1) % cleanedPoints.Count]; 
                    loop.Append(Line.CreateBound(p1, p2));
                }

                FilledRegionType regionType = new FilteredElementCollector(Document)
                    .OfClass(typeof(FilledRegionType))
                    .Cast<FilledRegionType>()
                    .FirstOrDefault();

                FillPatternElement diagonalPattern = new FilteredElementCollector(Document)
                    .OfClass(typeof(FillPatternElement))
                    .Cast<FillPatternElement>()
                    .FirstOrDefault(p => p.GetFillPattern().Name.ToLower().Contains("diagonal up"));

                var tag = FindDefaultTagTypeId(Document);

                if (tag == null)
                {
                    TaskDialog.Show("Warning", "No Default Tag");
                }
                ;




                var view = app.ActiveUIDocument.ActiveGraphicalView;
                XYZ tagLocation = GetLoopCenter(loop);

                if (regionType != null)
                {
                    using (Transaction tx = new Transaction(Document, "Create Closed Loop Region"))
                    {
                        tx.Start();

                        IList<CurveLoop> loops = new List<CurveLoop> { loop };

                        var detailComponentSymbol = FindDefaultTagType(Document); // Load or find the correct family symbol
                        var activeView = Document.ActiveView;
      

                        if (activeView != null)
                        {
                            var region = FilledRegion.Create(Document, regionType.Id, Document.ActiveView.Id, loops);


                            if (diagonalPattern != null)
                            {
                                regionType = Document.GetElement(region.GetTypeId()) as FilledRegionType;
                                regionType.ForegroundPatternId = diagonalPattern.Id;
                                regionType.ForegroundPatternColor = new Color(0, 0, 0);

                                regionType.IsMasking = false;
                            }

                            OverrideGraphicSettings ogs = new OverrideGraphicSettings();
                            ogs.SetProjectionLineColor(Color);
                            ogs.SetSurfaceForegroundPatternId(diagonalPattern.Id); 
                            ogs.SetSurfaceForegroundPatternColor(Color);

                            activeView.SetElementOverrides(region.Id, ogs);

                            var independentTag = IndependentTag.Create(Document, activeView.Id, new Reference(region), false, TagMode.TM_ADDBY_CATEGORY, TagOrientation.Horizontal, tagLocation);
                        }

                        tx.Commit();
                    }
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
            return "AddRegionHandler";
        }

        private XYZ GetLoopCenter(CurveLoop loop)
        {
            XYZ sum = XYZ.Zero;
            int count = 0;

            foreach (Curve curve in loop)
            {
                sum += curve.GetEndPoint(0);
                sum += curve.GetEndPoint(1);
                count += 2;
            }

            return sum / count;
        }

        private FamilySymbol FindDefaultTagType(Document doc)
        {
            var famSymbol = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .Where(x => x.FamilyName == "K2D Hatch Tag")
                .ToList();


            return famSymbol.FirstOrDefault();
        }

        private ElementId FindDefaultTagTypeId(Document doc)
        {
            var famSymbol = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .Where(x => x.FamilyName == "K2D Hatch Tag")
                .ToList();


            return famSymbol?.FirstOrDefault().Id;
        }
    }
}
