

using System;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Color = Autodesk.Revit.DB.Color;

namespace T24AddIn.Handlers.AddColorToTags
{
    internal class AddColorToTags : IExternalEventHandler
    {
        public ColorConfig Config { get; set; }
        public void Execute(UIApplication app)
        {
            try
            {
                var doc = app.ActiveUIDocument.Document;

                var elements = new FilteredElementCollector(doc)
                    .OfCategory(Config.Element)
                    .WhereElementIsNotElementType()
                    .Where(door =>
                    {

                        var props = door.GetParameters(Config.Group);


                        foreach (var prop in props)
                        {
                            var data = prop.AsValueString();

                            return data == "Yes";
                        }

                        return false;


                    })
                    .Select(x => x.Id);

                var otherTags = new FilteredElementCollector(doc)
                    .OfCategory(Config.TagType)
                    .WhereElementIsNotElementType()
                    .Where(x => x is IndependentTag) // Ensure valid casting
                    .Cast<IndependentTag>()
                    .Where(tag => tag.GetTaggedElementIds().Any(id => elements.Contains(id.HostElementId)));

                var color = Config.Color;
 
                var revitColor = new Color(color.Red, color.Green, color.Blue);

                var overrideSettings = new OverrideGraphicSettings();
                overrideSettings.SetProjectionLineColor(revitColor);

                using (var trans = new Transaction(doc, "Set Tag Colors"))
                {
                    trans.Start();

                    foreach (var tag in otherTags)
                    {
                        doc.ActiveView.SetElementOverrides(tag.Id, overrideSettings);
                    }

                    trans.Commit();
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
            return "AddColorToTags";
        }
    }
}
