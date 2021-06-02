using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TestTaskForRevit
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]

    public class ApartmentLayoutVisibilityPlagin : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            FilteredElementCollector roomFilter = new FilteredElementCollector(doc);

            ICollection<Element> allRooms = roomFilter.OfCategory(BuiltInCategory.OST_Rooms).WhereElementIsNotElementType().ToElements();

            var rooms = allRooms
                .Select(x => new RoomInfo
                {
                    Room = (Room)x,
                    Block = x.GetParameters("BS_Блок")[0].AsString(),
                    Subzone = x.GetParameters("ROM_Подзона")[0].AsString(),
                    Zone = x.GetParameters("ROM_Зона")[0].AsString(),
                    CalculatedSubzoneID = x.GetParameters("ROM_Расчетная_подзона_ID")[0].AsString()
                })
                .Where(name => name.Zone.Contains("Квартира"));

            // Группируем помещения по их свойствам
            // Фильтруем группы, оставляя те помещения, которые не являются уникальными в группе
            var roomGroups = rooms
                .GroupBy(x => new { x.Room.Level.Name, x.Block, x.Subzone })
                .Where(x => !x.All(r => r.Zone == x.First().Zone));

            // Выделяем группы помещений с четным номером квартиры,
            // и имеющим смежную рядомстоящую квартиру (+/- к номеру квартиры)
            var evenRoomGroups = roomGroups.Select(x => x.Where(t => t.RoomNumber % 2 == 0 &&
                                                                     x.Count(r => (r.RoomNumber + 1 == t.RoomNumber) || 
                                                                                  (r.RoomNumber - 1 == t.RoomNumber)) > 0)
                                                         .Select(r => r));

            using (Transaction t = new Transaction(doc))
            {
                t.Start("SetNumber");
                foreach (var group in evenRoomGroups)
                {
                    foreach (var room in group)
                    {
                        room.Room.GetParameters("ROM_Подзона_Index")[0].Set($"{room.CalculatedSubzoneID}.Полутон");
                    }
                }
                t.Commit();
            }

            return Result.Succeeded;
        }

      
    }
}
