using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;
using View = Autodesk.Revit.DB.View;

namespace T24AddIn.Features.Ungroup
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class UngroupCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            try
            {
                UIApplication uiApp = commandData.Application;
                UIDocument uiDoc = uiApp.ActiveUIDocument;
                Document doc = uiDoc.Document;
                var activeView = doc.ActiveView;

                Reference pickedRef = uiDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, "Select a group to ungroup");

                if (pickedRef == null)
                {
                    TaskDialog.Show("Selection", "No element selected.");
                    return Result.Failed;
                }

                Element selectedElement = doc.GetElement(pickedRef.ElementId);


                if (selectedElement is Group selectedGroup)
                {
                    var selectedCollection = new FilteredElementCollector(doc, activeView.Id)
                        .OfClass(typeof(Group))
                        .Cast<Group>()
                        .Where(g =>
                            g.Name == selectedGroup.Name
                            )
                        .ToList();

                    foreach (var group1 in selectedCollection)
                    {
                        using (var transaction = new Transaction(doc, "Ungroup Selected Data"))
                        {
                            transaction.Start();

                            var element = doc.GetElement(group1.UniqueId);

                            if (element.Id == ElementId.InvalidElementId) continue;

                            var group = doc.GetElement(element.Id) as Group;

                            if (group == null) continue;

                            group.UngroupMembers();

                            transaction.Commit();
                        }
                    }
                }
                else
                {
                    TaskDialog.Show("Selection", "Please Select a group");
                    return Result.Failed;
                }
            }
            catch (Exception e)
            {
                return Result.Failed;
            }

            return Result.Succeeded;
        }

        private bool IsElementVisibleInView(Element element, View view)
        {
            // Get the element's bounding box
            BoundingBoxXYZ boundingBox = element.get_BoundingBox(view);

            return boundingBox != null;
        }
    }
}
