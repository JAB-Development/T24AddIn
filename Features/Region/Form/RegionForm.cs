using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using T24AddIn.Handlers.AddRegionHandler;
using Color = Autodesk.Revit.DB.Color;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace T24AddIn.Features.Region.Form
{
    public partial class RegionForm : System.Windows.Forms.Form
    {
        private readonly UIDocument _uidoc;
        private readonly Document _doc;

        private readonly ExternalEvent _externalEventAddRegion;
        private readonly AddRegionHandler _addRegionHandler = new AddRegionHandler();

        public RegionForm(UIApplication uiApp)
        {
            InitializeComponent();
            _uidoc = uiApp.ActiveUIDocument;
            _doc = _uidoc.Document;

            _externalEventAddRegion = ExternalEvent.Create(_addRegionHandler);
        }

        private void SelectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                List<XYZ> clickedPoints = new List<XYZ>();

                while (true)
                {
                    try
                    {
                        XYZ point = _uidoc.Selection.PickPoint("Click an endpoint (Esc to finish)");
                        clickedPoints.Add(point);
                    }
                    catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                    {
                        break;
                    }
                }

                IList<Reference> pickedFaces = _uidoc.Selection.PickObjects(
                    ObjectType.Face,
                    "Select exterior wall faces");


                if (clickedPoints == null || clickedPoints.Count < 3)
                {
                    TaskDialog.Show("Error", "You need at least 3 lines to form a closed loop.");
                    return; // Exit early if not enough edges are selected
                }

                _addRegionHandler.Document = _doc;
                _addRegionHandler.IntersectionPoints = clickedPoints;
                if (_addRegionHandler.Color == null)
                {
                    _addRegionHandler.Color = new Color(255, 255, 255);
                }
                
                _externalEventAddRegion.Raise();
            }
            catch (Exception exception)
            {
                TaskDialog.Show("Error", "Something went wrong: " + exception.Message);
            }
        }

        public XYZ GetIntersectionPoint(Curve curve1, Curve curve2, double tolerance = 0.01)
        {
            if (curve1 == null || curve2 == null)
                throw new ArgumentNullException("Curves cannot be null");

            if (!(curve1 is Line line1) || !(curve2 is Line line2))
                throw new ArgumentException("Only Line curves are supported");

            XYZ p1 = line1.GetEndPoint(0);
            XYZ p2 = line1.GetEndPoint(1);
            XYZ q1 = line2.GetEndPoint(0);
            XYZ q2 = line2.GetEndPoint(1);

            XYZ u = p2 - p1;
            XYZ v = q2 - q1;
            XYZ w = p1 - q1;

            double a = u.DotProduct(u);         // always >= 0
            double b = u.DotProduct(v);
            double c = v.DotProduct(v);         // always >= 0
            double d = u.DotProduct(w);
            double e = v.DotProduct(w);
            double D = a * c - b * b;           // always >= 0

            if (Math.Abs(D) < 1e-9)
                return null; // Lines are parallel or coincident

            double sc = (b * e - c * d) / D;
            double tc = (a * e - b * d) / D;

            XYZ pointOnLine1 = p1 + sc * u;
            XYZ pointOnLine2 = q1 + tc * v;

            if (pointOnLine1.DistanceTo(pointOnLine2) > tolerance)
                return null; // Lines don’t intersect close enough

            // Now check if point lies within both line segments (not just infinite lines)
            if (
                IsPointOnLineSegment(pointOnLine1, p1, p2, tolerance) &&
                IsPointOnLineSegment(pointOnLine2, q1, q2, tolerance)
            )
            {
                return pointOnLine1; // or average of both if you want: (pointOnLine1 + pointOnLine2) / 2
            }

            return null;
        }

        private bool IsPointOnLineSegment(XYZ pt, XYZ start, XYZ end, double tolerance)
        {
            double total = start.DistanceTo(end);
            double d1 = start.DistanceTo(pt);
            double d2 = pt.DistanceTo(end);
            return Math.Abs((d1 + d2) - total) <= tolerance;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ColorRegionOverride.ShowDialog() == DialogResult.OK)
            {
                var color = ColorRegionOverride.Color;

                _addRegionHandler.Color = new Color(color.R, color.G, color.B);
                ColorPickerPannel.BackColor = System.Drawing.Color.FromArgb(color.R, color.G, color.B);
            }
        }
    }
}
