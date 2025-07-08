using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using T24AddIn.Features.Region.Form;

namespace T24AddIn.Features.Region
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class RegionCommand : IExternalCommand
    {
        private static RegionForm _form;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;

            if (_form == null || _form.IsDisposed)
            {
                _form = new RegionForm(uiApp);
                _form.Show();
            }
            else
            {
                _form.BringToFront();
            }

            return Result.Succeeded;
        }
    }
}
