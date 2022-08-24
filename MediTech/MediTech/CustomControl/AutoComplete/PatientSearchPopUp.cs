using MediTech.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.CustomControl
{
    public class PatientSearchPopUp
    {
        public ObservableCollection<Column> Columns { get; private set; }
        public PatientSearchPopUp()
        {
            Columns = new ObservableCollection<Column>() {
                new Column() { FieldName = "PatientID", Header = "LN" },
                new Column() { FieldName = "Title", Header = "คำนำหน้า" },
                new Column() { FieldName = "FirstName", Header = "ชื่อ" },
                new Column() { FieldName = "LastName", Header = "นามสกุล" },
                new Column() { FieldName = "Gender", Header = "เพศ" },
                new Column() { FieldName = "BirthDttmString", Header = "วัน เดือน ปี เกิด" }
            };
        }
    }
}
