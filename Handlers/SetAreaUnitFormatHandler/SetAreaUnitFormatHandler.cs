using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


namespace T24AddIn.Handlers.SetAreaUnitFormatHandler
{
    internal class SetAreaUnitFormatHandler: IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            UIDocument uidoc = app.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                using (var transaction = new Transaction(doc, "set Area Unit Format"))
                {
                    transaction.Start();

                    Units units = doc.GetUnits();

                    FormatOptions areaFormat = units.GetFormatOptions(SpecTypeId.Area);

                    double currentAccuracy = areaFormat.Accuracy;

                    if (currentAccuracy > 0.1)
                    {
                        areaFormat.UseDefault = false;
                        areaFormat.SetUnitTypeId(UnitTypeId.SquareFeet);
                        areaFormat.Accuracy = 0.1;
                        areaFormat.RoundingMethod = RoundingMethod.Nearest;

                        units.SetFormatOptions(SpecTypeId.Area, areaFormat);
                        doc.SetUnits(units);
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
            return "SetAreaUnitFormatHandler";
        }
    }
}
