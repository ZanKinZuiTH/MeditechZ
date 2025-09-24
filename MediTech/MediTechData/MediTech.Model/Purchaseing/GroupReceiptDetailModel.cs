using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class GroupReceiptDetailModel
    {
        public int GroupReceiptDetailUID { get; set; }
        public int GroupReceiptUID { get; set; }
        public string ItemName { get; set; }
        public int? BillableItemUID { get; set; }
        public int? OrderSetUID { get; set; }
        public int ItemUID { get; set; }
        public Nullable<double> Quantity { get; set; }
        public int No { get; set; }
        public string UnitItem { get; set; }
        public Nullable<double> PriceUnit { get; set; }
        public Nullable<double> TotalPrice { get; set; }
        public Nullable<double> Discount { get; set; }
        public double? PTaxPercentage { get; set; }
        public int TypeOrderUID { get; set; }
        public string TypeOrder { get; set; }
        public string Unit { get; set; }
        public DateTime MWhen { get; set; }
        public DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public int CUser { get; set; }

    }
}
