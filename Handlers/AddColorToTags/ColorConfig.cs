using Autodesk.Revit.DB;
using Color = Autodesk.Revit.DB.Color;

namespace T24AddIn.Handlers.AddColorToTags
{
    public class ColorConfig
    {
        public string Group { get; set; } = string.Empty;
        public Color Color { get; set; } = new Color(0, 0, 0);
        public BuiltInCategory Element { get; set; }
        public BuiltInCategory TagType { get; set; }

    }
}
