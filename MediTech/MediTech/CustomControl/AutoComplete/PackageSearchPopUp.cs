using MediTech.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.CustomControl
{
    public class PackageSearchPopUp
    {
        public ObservableCollection<Column> Columns { get; private set; }
        public PackageSearchPopUp()
        {
            Columns = new ObservableCollection<Column>() {
                new Column() { FieldName = "PackageName", Header = "Name" },
                new Column() { FieldName = "Code", Header = "Code" }

            };
        }
    }
}
