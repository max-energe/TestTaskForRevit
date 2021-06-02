using Autodesk.Revit.DB.Architecture;
using System;
using System.Text.RegularExpressions;

namespace TestTaskForRevit
{
    /// <summary>
    /// Содержит информацию о помещении с дополнительными открытыми свойствами
    /// </summary>
    public class RoomInfo
    {
        public Room Room { get; set; }
        public string Block { get; set; }
        private int roomnumber = -1;
        public int RoomNumber {
            get
            {
                if (roomnumber == -1)
                    return GetRoomNumber();
                else
                    return roomnumber;
            }
        }
        public string Subzone { get; set; }
        public string Zone { get; set; }
        public string CalculatedSubzoneID { get; set; }

        public override string ToString()
        {
            return $"{Room.Level.Name} / {Zone} / {Block}"; 
        }


        private int GetRoomNumber()
        {
            var numberAsString = Regex.Replace(this.Zone.Trim(), @"^(Квартира)\s*(?<var1>\d{1,3}).*", "${var1}");

            if (int.TryParse(numberAsString, out int result))
                return result;
            else
                throw new Exception(string.Format("Номер квартиры не распознан: {0}", this.ToString()));
        }
    }
}
