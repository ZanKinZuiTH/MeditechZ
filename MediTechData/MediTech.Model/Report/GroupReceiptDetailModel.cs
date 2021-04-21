using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class GroupReceiptDetailModel
    {
        public int GroupReceiptDetailUID { get; set; }
        public int GroupReceiptUID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public int ItemUID { get; set; }
        public Nullable<double> Quantity { get; set; }
        public int No { get; set; }
        public string UnitItem { get; set; }
        public Nullable<double> PriceUnit { get; set; }
        public Nullable<double> TotalPrice { get; set; }
        public Nullable<double> Discount { get; set; }
    }
}
