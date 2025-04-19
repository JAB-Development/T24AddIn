using System.Diagnostics;
using System.IO;
using System.Reflection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using T24AddIn.Features.Tags;
using System.Windows.Media.Imaging;
using T24AddIn.Features.Ungroup;


namespace T24AddIn
{

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Application : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            var panel = RibbonPanel(application);
            var assemblyPath = Assembly.GetExecutingAssembly().Location;

            if (panel.AddItem(new PushButtonData("T24 Tagging", "T24 Tagging", assemblyPath, typeof(TagsCommand).ToString()))
                is PushButton button)
            {
                var iconPath = Path.Combine(Path.GetDirectoryName(assemblyPath) ?? string.Empty, "Resources", "Tag.ico");

                if (File.Exists(iconPath))
                {
                    var uri = new Uri(iconPath);
                    var bitmap = new BitmapImage(uri);

                    button.LargeImage = bitmap;
                    
                    button.ItemText = "Tags";

                }
            }

            if (panel.AddItem(new PushButtonData("Ungroup Components", "Ungroup Components", assemblyPath, typeof(UngroupCommand).ToString()))
                is PushButton ungroupButton)
            {
                var iconPathGroup = Path.Combine(Path.GetDirectoryName(assemblyPath) ?? string.Empty, "Resources", "ungroup2.ico");

                if (File.Exists(iconPathGroup))
                {
                    var uri = new Uri(iconPathGroup);
                    var bitmap = new BitmapImage(uri);

                    ungroupButton.LargeImage = bitmap;

                    ungroupButton.ItemText = "Ungroup";

                }
            }

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public RibbonPanel RibbonPanel(UIControlledApplication application)
        {
            const string tabName = "T24";
            const string panelName = "T24 Tool";

            try
            {
                application.CreateRibbonTab(tabName);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            try
            {
                application.CreateRibbonPanel(tabName, panelName);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            var ribbonPanel = application
                .GetRibbonPanels(tabName)
                .FirstOrDefault(x => x.Name == panelName);

            return ribbonPanel;
        }
    }
}
