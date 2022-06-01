using System;
using MediTech.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.CustomControl
{
    public class ProblemSearchPopUp
    {
        public ObservableCollection<Column> Columns { get; private set; }

        public ProblemSearchPopUp()
        {
            Columns = new ObservableCollection<Column>() {
                new Column() { FieldName = "Code", Header = "Code" },
                new Column() { FieldName = "Name", Header = "Name" },
            };
        }
    }
}
