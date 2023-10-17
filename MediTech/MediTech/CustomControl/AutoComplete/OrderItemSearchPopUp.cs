using MediTech.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.CustomControl
{
    public class OrderItemSearchPopUp
    {
        public ObservableCollection<Column> Columns { get; private set; }
        public OrderItemSearchPopUp()
        {
            Columns = new ObservableCollection<Column>() {
                new Column() { FieldName = "No", Header = "No" },
                new Column() { FieldName = "Code", Header = "Code" },
                new Column() { FieldName = "ItemName", Header = "ItemName" },
                new Column() { FieldName = "TypeOrder", Header = "TypeOrder" },
                new Column() { FieldName = "TradeName", Header = "TradeName" }
            };
        }
    }
}
