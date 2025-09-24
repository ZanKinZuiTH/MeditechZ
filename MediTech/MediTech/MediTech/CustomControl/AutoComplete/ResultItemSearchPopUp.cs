using MediTech.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.CustomControl
{
    public class ResultItemSearchPopUp
    {
        public ObservableCollection<Column> Columns { get; private set; }
        public ResultItemSearchPopUp()
        {
            Columns = new ObservableCollection<Column>() {
                new Column() { FieldName = "DisplyName", Header = "DisplyName" },
                new Column() { FieldName = "Code", Header = "Code" },
                new Column() { FieldName = "UOM", Header = "Unit" },
                new Column() { FieldName = "ResultType", Header = "Type" }
            };
        }
    }
}
