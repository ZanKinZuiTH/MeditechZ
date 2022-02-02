using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class GRNItemListModel
    {
        public int GRNItemListUID { get; set; }
        public int GRNDetailUID { get; set; }
        public int ItemMasterUID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public double Quantity { get; set; }
        public double? FreeQuantity { get; set; }
        public int IMUOMUID { get; set; }
        public string Unit { get; set; }
        public string BatchID { get; set; }
        public Nullable<System.DateTime> ExpiryDttm { get; set; }
        public double PurchaseCost { get; set; }
        public double? TaxPercentage { get; set; }
        public double? Discount { get; set; }
        public Nullable<double> NetAmount { get; set; }
        public int? ManufacturerUID { get; set; }
        public string ManufacturerName { get; set; }
        public Nullable<int> ITCATUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public string SerialNumber { get; set; }
    }
}
