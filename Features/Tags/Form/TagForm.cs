using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using T24AddIn.Handlers.AddColorToTags;
using T24AddIn.Handlers.AddCurtainWallHandler;
using T24AddIn.Handlers.AddDoorTagHandler;
using T24AddIn.Handlers.AddScheduleHandler;
using T24AddIn.Handlers.AddScheduleToSheetHandler;
using T24AddIn.Handlers.AddTagParamHandler;
using T24AddIn.Handlers.AddWallTagHandler;
using T24AddIn.Handlers.AddWindowTagHandler;
using T24AddIn.Handlers.CalculateAreaHandler;
using T24AddIn.Handlers.CalculateGrossAreaHandler;
using T24AddIn.Handlers.ImportTagHandler;
using T24AddIn.Handlers.MoveCurtainWallTagHandler;
using T24AddIn.Handlers.MoveDoorTagHandler;
using T24AddIn.Handlers.MoveTagFromWallHandler;
using T24AddIn.Handlers.MoveWallTagHandler;
using T24AddIn.Handlers.MoveWindowTagHandler;
using T24AddIn.Handlers.SetAreaUnitFormatHandler;
using Color = Autodesk.Revit.DB.Color;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace T24AddIn.Features.Tags.Form
{
    public partial class TagForm : System.Windows.Forms.Form
    {
        private UIApplication _uiApp;
        private UIDocument _uiDoc;
        private Document _document;

        #region External Events

        private readonly ExternalEvent _externalEventAddTagParam;
        private readonly AddTagParameter _addTagParameter = new AddTagParameter();

        private readonly ExternalEvent _externalEventAddWallTagParam;
        private readonly AddWallTagParameter _addWallTagParameter = new AddWallTagParameter();

        private readonly ExternalEvent _externalImportTag;
        private readonly ImportTagHandler _importTagHandler = new ImportTagHandler();

        private readonly ExternalEvent _externalAddDoorTag;
        private readonly AddDoorTagHandler _addDoorTagHandler = new AddDoorTagHandler();

        private readonly ExternalEvent _externalAddWindowTag;
        private readonly AddWindowTagHandler _addWindowTagHandler = new AddWindowTagHandler();

        private readonly ExternalEvent _externalAddWallTag;
        private readonly AddWallTagHandler _addWallTagHandler = new AddWallTagHandler();

        private readonly ExternalEvent _externalMoveDoorTag;
        private readonly MoveDoorTagHandler _moveDoorTagHandler = new MoveDoorTagHandler();

        private readonly ExternalEvent _externalMoveWindowTag;
        private readonly MoveWindowTagHandler _moveWindowTagHandler = new MoveWindowTagHandler();

        private readonly ExternalEvent _externalMoveWallTag;
        private readonly MoveWallTagHandler _moveWallTagHandler = new MoveWallTagHandler();

        private readonly ExternalEvent _externalAddColor;
        private readonly AddColorToTags _addColorToTags = new AddColorToTags();

        private readonly ExternalEvent _externalAddSchedule;
        private readonly AddScheduleHandler _addScheduleHandler = new AddScheduleHandler();

        private readonly ExternalEvent _externalMoveTagFromWall;
        private readonly MoveTagFromWallHandler _moveTagFromWall = new MoveTagFromWallHandler();

        private readonly ExternalEvent _externalAddScheduleToSheet;
        private readonly AddScheduleToSheetHandler _addScheduleToSheetHandler = new AddScheduleToSheetHandler();

        private readonly ExternalEvent _externalCalculateArea;
        private readonly CalculateAreaHandler _calculateAreaHandler = new CalculateAreaHandler();

        private readonly ExternalEvent _externalAddCurtainWallTag;
        private readonly AddCurtainWallHandler _addCurtainWallHandler = new AddCurtainWallHandler();

        private readonly ExternalEvent _externalMoveCurtainWallTag;
        private readonly MoveCurtainWallTagHandler _moveCurtainWallTagHandler = new MoveCurtainWallTagHandler();

        private readonly ExternalEvent _externalSetAreaUnitFormat;
        private readonly SetAreaUnitFormatHandler _setAreaUnitDecimalHandler = new SetAreaUnitFormatHandler();

        private readonly ExternalEvent _externalCalculateGrossWallArea;
        private readonly CalculateGrossAreaHandler _calculateGrossWallAreaHandler = new CalculateGrossAreaHandler();

        #endregion

        public List<string> Properties =
        [
            "North", "South", "East", "West", "Group 1", "Group 2", "Group 3", "Group 4", "Group 5", "Group 6",
            "Group 7", "Group 8", "All"
        ];

        public TagForm(UIApplication uiApp)
        {
            InitializeComponent();

            _uiApp = uiApp;
            _uiDoc = uiApp.ActiveUIDocument;
            _document = _uiDoc.Document;

            _externalEventAddTagParam = ExternalEvent.Create(_addTagParameter);
            _externalEventAddWallTagParam = ExternalEvent.Create(_addWallTagParameter);
            _externalImportTag = ExternalEvent.Create(_importTagHandler);
            _externalAddDoorTag = ExternalEvent.Create(_addDoorTagHandler);
            _externalAddWindowTag = ExternalEvent.Create(_addWindowTagHandler);
            _externalAddWallTag = ExternalEvent.Create(_addWallTagHandler);

            _externalMoveDoorTag = ExternalEvent.Create(_moveDoorTagHandler);
            _externalMoveWindowTag = ExternalEvent.Create(_moveWindowTagHandler);
            _externalMoveWallTag = ExternalEvent.Create(_moveWallTagHandler);
            _externalAddColor = ExternalEvent.Create(_addColorToTags);
            _externalAddSchedule = ExternalEvent.Create(_addScheduleHandler);
            _externalMoveTagFromWall = ExternalEvent.Create(_moveTagFromWall);
            _externalAddScheduleToSheet = ExternalEvent.Create(_addScheduleToSheetHandler);
            _externalCalculateArea = ExternalEvent.Create(_calculateAreaHandler);
            _externalAddCurtainWallTag = ExternalEvent.Create(_addCurtainWallHandler);
            _externalMoveCurtainWallTag = ExternalEvent.Create(_moveCurtainWallTagHandler);
            _externalSetAreaUnitFormat = ExternalEvent.Create(_setAreaUnitDecimalHandler);
            _externalCalculateGrossWallArea = ExternalEvent.Create(_calculateGrossWallAreaHandler);

            PropSelect.DataSource = Properties.Where(x => x != "All").ToList();

            TagType.DataSource = new List<string> { "Doors", "Walls", "Windows", };
            ScheduleTagTypeComboBox.DataSource = new List<string> { "Doors", "Walls", "Windows" };

            ScheduleGroupComboBox.DataSource = Properties;
            AddViewDataSource();
        }

        private void AddViewDataSource()
        {
            var view = new FilteredElementCollector(_document)
                .OfClass(typeof(ViewSheet)).WhereElementIsNotElementType()
                .Cast<ViewSheet>()
                .Select(x => x.Name)
                .ToList();

            view.Add("None");

            ViewComboBox.DataSource = view;
        }

        private void AddPropertiesBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var elements = new List<Element>();

                _addTagParameter.Document = _document;
                _addTagParameter.Elements = elements;
                _externalEventAddTagParam.Raise();

                //calculate Area
                _externalCalculateArea.Raise();
                _externalSetAreaUnitFormat.Raise();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                TaskDialog.Show("Error", $"An unexpected error occurred: {exception.Message}");
            }
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            _externalImportTag.Raise();
        }

        private void TagDoorBtn_Click(object sender, EventArgs e)
        {
            _externalAddDoorTag.Raise();
            _externalMoveDoorTag.Raise();


            _moveTagFromWall.Category = BuiltInCategory.OST_DoorTags;
            _externalMoveTagFromWall.Raise();
        }

        private void TagWindowBtn_Click(object sender, EventArgs e)
        {
            _externalAddWindowTag.Raise();
            _externalMoveWindowTag.Raise();

            _moveTagFromWall.Category = BuiltInCategory.OST_WindowTags;
            _externalMoveTagFromWall.Raise();
        }

        private void TagWallBtn_Click(object sender, EventArgs e)
        {
            _externalAddWallTag.Raise();
            _externalMoveWallTag.Raise();

            _moveTagFromWall.Category = BuiltInCategory.OST_WallTags;
            _externalMoveTagFromWall.Raise();
        }

        public ColorConfig Config { get; set; } = new ColorConfig();
        private void button1_Click(object sender, EventArgs e)
        {
            _addColorToTags.Config = Config;
            _externalAddColor.Raise();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                var color = colorDialog1.Color;
                Config.Color = new Color(color.R, color.G, color.B);
            }
        }

        private void PropSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.Group = PropSelect.SelectedItem.ToString();
        }

        private void TagType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var element = TagType.SelectedItem.ToString();

            switch (element)
            {
                case "Doors":

                    Config.TagType = BuiltInCategory.OST_DoorTags;
                    Config.Element = BuiltInCategory.OST_Doors;
                    break;
                case "Walls":
                    Config.TagType = BuiltInCategory.OST_WallTags;
                    Config.Element = BuiltInCategory.OST_Walls;
                    break;
                case "Windows":
                    Config.TagType = BuiltInCategory.OST_WindowTags;
                    Config.Element = BuiltInCategory.OST_Windows;
                    break;
            }
        }

        private void ScheduleGroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _addScheduleHandler.Group = ScheduleGroupComboBox.SelectedItem.ToString();
        }

        private void ScheduleTagTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _addScheduleHandler.TagType = ScheduleTagTypeComboBox.SelectedItem.ToString();
        }

        private void ViewComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _addScheduleHandler.ViewName = ViewComboBox.SelectedItem.ToString();
            _addScheduleToSheetHandler.SheetName = ViewComboBox.SelectedItem.ToString();
        }

        private void GenerateScheduleBtn_Click(object sender, EventArgs e)
        {
            var name = $"{_addScheduleHandler.Group} {_addScheduleHandler.TagType} Schedule";
            _addScheduleToSheetHandler.ScheduleName = name;

            _externalAddSchedule.Raise();
            _externalAddScheduleToSheet.Raise();

            //calculate Area
            _externalCalculateArea.Raise();
        }

        private void TagCurtainWallBtn_Click(object sender, EventArgs e)
        {
            _externalAddCurtainWallTag.Raise();
            _externalMoveCurtainWallTag.Raise();

            _moveTagFromWall.Category = BuiltInCategory.OST_CurtainWallPanelTags;
            _externalMoveTagFromWall.Raise();
        }

        private void GrossWallBtn_Click(object sender, EventArgs e)
        {
            _externalCalculateGrossWallArea.Raise();
        }
    }
}
    