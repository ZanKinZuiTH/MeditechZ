using MediTech.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.CustomControl
{
    public class GeneralSearchPopUp
    {
        public ObservableCollection<Column> Columns { get; private set; }
        public GeneralSearchPopUp()
        {
            Columns = new ObservableCollection<Column>() {
                new Column() { FieldName = "Code", Header = "รหัส" },
                new Column() { FieldName = "Name", Header = "ชื่อ" },
                new Column() { FieldName = "Description", Header = "รายละเอียด" }
            };
        }
    }
}
