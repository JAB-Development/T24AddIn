using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace T24AddIn.Handlers.ImportTagHandler
{
    internal class ImportTagHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            try
            {
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = @"Select a folder to import tags:";
                    folderDialog.ShowNewFolderButton = true;

                    // Show the dialog
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFolderPath = folderDialog.SelectedPath;
                        string[] tagFilePaths = Directory.GetFiles(selectedFolderPath, "*.rfa");

                        if (tagFilePaths.Length == 0)
                        {
                            MessageBox.Show(@"No .rfa files found in the selected folder.", @"Error");
                            return;
                        }

                        UIDocument uidoc = app.ActiveUIDocument;
                        Document doc = uidoc.Document;


                        foreach (string tagFilePath in tagFilePaths)
                        {
                            string[] parts = tagFilePath.Split(new string[] { "\\" }, StringSplitOptions.None);
                            var last = parts.LastOrDefault();

                            switch (last)
                            {
                                case "K2D Door Tag.rfa":
                                    LoadFamilyIntoProject(doc, tagFilePath, BuiltInCategory.OST_Doors);
                                    break;
                                case "K2D Wall Tag.rfa":
                                    LoadFamilyIntoProject(doc, tagFilePath, BuiltInCategory.OST_Walls);
                                    break;
                                case "K2D Curtain Wall Tag.rfa":
                                    LoadFamilyIntoProject(doc, tagFilePath, BuiltInCategory.OST_Walls);
                                    break;
                                case "K2D Window Tag.rfa":
                                    LoadFamilyIntoProject(doc, tagFilePath, BuiltInCategory.OST_Windows);
                                    break;
                                case "K2D Hatch Tag.rfa":
                                    LoadFamilyIntoProject(doc, tagFilePath, BuiltInCategory.OST_DetailComponents);
                                    break;
                            }

                        }

                        MessageBox.Show("Successfully imported tags.", "Import Tags");
                    }
                    else
                    {
                        MessageBox.Show("No folder was selected.", "Import Tags");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error");
            }
        }

        private void LoadFamilyIntoProject(Document doc, string familyPath, BuiltInCategory cat)
        {
            using (var trans = new Transaction(doc, "Load family"))
            {
                trans.Start();

                if (doc.LoadFamily(familyPath, out var family))
                {
                    if (family == null)
                    {
                        MessageBox.Show("No valid tag types found in the loaded families.", "Error");
      
                    }

                    FamilySymbol tagType = GetFirstTagTypeFromFamily(doc, family, cat);

                    if (tagType != null)
                    {
                        // Activate the tag type if it is not active
                        if (!tagType.IsActive)
                        {
                            tagType.Activate();
                            doc.Regenerate();
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Failed to load the family '{Path.GetFileName(familyPath)}'.", "Error");
                }

                trans.Commit();
            }

        }

        private FamilySymbol GetFirstTagTypeFromFamily(Document doc, Family family, BuiltInCategory category)
        {
            // Get the first FamilySymbol (tag type) in the family
            var famSymbol = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .FirstOrDefault(symbol => symbol.Family.Id == family.Id);

            return famSymbol;
        }

        public string GetName()
        {
            return "ImportTagHandler";
        }
    }
}
