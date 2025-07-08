using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Autodesk.Revit.DB;

namespace T24AddIn.Service.CurtainWall
{
    internal class CurtainWallService
    {
        private readonly Document _doc;


        public CurtainWallService(Document doc)
        {
            _doc = doc;
        }

        public List<IndependentTag> GetAllCurtainWalls()
        {
            var curtainWallCollector = new FilteredElementCollector(_doc)
                .OfCategory(BuiltInCategory.OST_Walls)
                .OfClass(typeof(Wall))
                .WhereElementIsNotElementType()
                .Where(wallElement =>
                {
                    WallType wallType = _doc.GetElement(wallElement.GetTypeId()) as WallType;

                    if (wallType == null) return false;
                    if (wallType.Kind != WallKind.Curtain) return false;

                    var functionParam = wallType.LookupParameter("Function");
                    bool isCurtain = wallType.Name.Contains("Curtain", StringComparison.OrdinalIgnoreCase);

                    if (!isCurtain) return false;

                    bool isExteriorByFunction = functionParam != null &&
                                                functionParam.AsValueString()?.Equals("Exterior", StringComparison.OrdinalIgnoreCase) == true;
                    var exteriorParam = wallType.LookupParameter("Exterior");
                    bool isExteriorByYesNo = exteriorParam is { StorageType: StorageType.Integer } &&
                                             exteriorParam.AsInteger() == 1;

                    return isExteriorByFunction || isExteriorByYesNo || isCurtain;
                })
                .ToList();

            var hostElements = curtainWallCollector.Select(x => x.Id);

            var tags = new FilteredElementCollector(_doc)
                .OfCategory(BuiltInCategory.OST_WallTags)
                .WhereElementIsNotElementType()
                .Where(x => x is IndependentTag tag && hostElements.Contains(tag.GetTaggedElementIds().FirstOrDefault().HostElementId)) // Ensure valid casting
                .Cast<IndependentTag>()
                .ToList();

            return tags;
        }

    }
}
