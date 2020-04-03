using MediTech.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.CustomControl
{
    public class ZipCodeSearchPopUp
    {
        public ObservableCollection<Column> Columns { get; private set; }
        public ZipCodeSearchPopUp()
        {
            Columns = new ObservableCollection<Column>() {
                new Column() { FieldName = "ZipCode", Header = "รหัสไปรษณีย์ย์" },
                new Column() { FieldName = "District", Header = "ตำบล" },
                new Column() { FieldName = "Amphur", Header = "อำเภอ" },
                new Column() { FieldName = "Province", Header = "จังหวัด" }
            };
        }
    }
}
