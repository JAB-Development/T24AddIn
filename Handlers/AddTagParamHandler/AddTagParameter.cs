﻿using System.IO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace T24AddIn.Handlers.AddTagParamHandler
{
    internal class AddTagParameter : IExternalEventHandler
    {
        public List<Element> Elements { get; set; }
        public Document Document { get; set; }

        public List<string> Properties =
        [
            "Area1", "North", "South", "East", "West", "Group 1", "Group 2", "Group 3", "Group 4", "Group 5", "Group 6",
            "Group 7", "Group 8", "Exterior", "Curtain Wall", "Gross Area"
        ];

        public void Execute(UIApplication app)
        {
            //try
            //{
            //    foreach (var element in Elements)
            //    {
            //        if (element is FamilySymbol familySymbol)
            //        {
            //            var familyDoc = Document.EditFamily(familySymbol.Family);

            //            using (var transaction = new Transaction(familyDoc, "Add Tag Parameter"))
            //            {
            //                transaction.Start();

            //                FamilyManager familyManager = familyDoc.FamilyManager;

            //                if (familyManager == null) return;

            //                foreach (var propName in Properties)
            //                {
            //                    var existingParam = familyManager.get_Parameter(propName);

            //                    if (existingParam == null)
            //                    {
            //                        familyManager.AddParameter(propName, BuiltInParameterGroup.PG_IDENTITY_DATA, ParameterType.YesNo, true);

            //                        var newParam = familyManager.get_Parameter(propName);

            //                        if (newParam != null)
            //                        {
            //                            familyManager.Set(newParam, 0);
            //                        }
            //                    }
            //                }

            //                transaction.Commit();
            //            }

            //            familyDoc.LoadFamily(Document, new FamilyOptions());

            //            familyDoc.Close(false);
            //        }
            //    }

            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}

            try
            {
                // Open shared parameter file if using shared parameters
                DefinitionFile sharedParamFile = Document.Application.OpenSharedParameterFile();

                if (sharedParamFile == null)
                {
                    string sharedParamFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SharedParameters.txt");

                    // Create a new shared parameter file if it doesn't exist
                    if (!File.Exists(sharedParamFilePath))
                    {
                        File.Create(sharedParamFilePath).Dispose(); // Create and release the file
                    }

                    // Assign the newly created shared parameter file to Revit
                    Document.Application.SharedParametersFilename = sharedParamFilePath;

                    // Re-open the shared parameter file
                    sharedParamFile = Document.Application.OpenSharedParameterFile();

                    if (sharedParamFile == null)
                    {
                        Autodesk.Revit.UI.TaskDialog.Show("Error", "Failed to create or open the shared parameter file.");
                        return;
                    }
                }

                // Get or create the shared parameter group for the walls
                DefinitionGroup definitionGroup = sharedParamFile.Groups.get_Item("Tag Parameters");
                if (definitionGroup == null)
                {
                    definitionGroup = sharedParamFile.Groups.Create("Tag Parameters");
                }

                // Get the Wall category (OST_Walls)
                var doc = app.ActiveUIDocument.Document;
                Category windowCategory = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Windows);
                Category doorsCategory = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Doors);
                Category wallCategory = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Walls);


                // Begin a transaction for the document to add parameters
                using (var transaction = new Transaction(doc, "Add Wall Tag Parameter"))
                {
                    transaction.Start();

                    // Check and create the shared parameters
                    foreach (var propName in Properties)
                    {
                        // Check if the shared parameter already exists
                        Definition definition = definitionGroup.Definitions.get_Item(propName);
                        if (definition == null)
                        {
                            // Create the shared parameter if it does not exist


                            if (propName is "Area1" or "Gross Area")
                            {
                                var option = new ExternalDefinitionCreationOptions(propName, SpecTypeId.Area)
                                {
                                    UserModifiable = true,
                                    Visible = propName == "Gross Area" // 👈 Only "Gross Area" is visible
                                };

                                ExternalDefinitionCreationOptions options1 =
                                    option;

                                definition = definitionGroup.Definitions.Create(options1);
                            }
                            else
                            {
                                ExternalDefinitionCreationOptions options = new ExternalDefinitionCreationOptions(propName, SpecTypeId.Boolean.YesNo);
                                definition = definitionGroup.Definitions.Create(options);
                            }
                        }

                        // Bind the shared parameter to the Wall category
                        CategorySet categorySet = new CategorySet();

                        //if (propName != "Curtain Wall")
                        //{
                        //    categorySet.Insert(doorsCategory);
                        //    categorySet.Insert(windowCategory);
                        //}


                        //if(propName != "Area1") categorySet.Insert(wallCategory);

                        //if (propName == "Gross Area")
                        //{
                        //    categorySet.Insert(wallCategory);
                        //}

                        if (propName == "Gross Area")
                        {
                            // Only insert wall category for Gross Area
                            categorySet.Insert(wallCategory);
                        }
                        else
                        {
                            if (propName != "Curtain Wall")
                            {
                                categorySet.Insert(doorsCategory);
                                categorySet.Insert(windowCategory);
                            }

                            if (propName != "Area1")
                            {
                                categorySet.Insert(wallCategory);
                            }
                        }


                        InstanceBinding binding = doc.Application.Create.NewInstanceBinding(categorySet);

                        // Add the shared parameter to the project
                        if (!doc.ParameterBindings.Contains(definition))
                        {
                            doc.ParameterBindings.Insert(definition, binding, GroupTypeId.IdentityData);
                        }
                    }

                    // Commit the transaction
                    transaction.Commit();
                }   

                TaskDialog.Show("Success", "Tag parameters added to Windows, wall and Doors successfully.");
            }
            catch (Exception e)
            {
                TaskDialog.Show("Error", e.Message);
            }
        }

        public string GetName()
        {
            return "AddTagParameter";
        }
    }
}
