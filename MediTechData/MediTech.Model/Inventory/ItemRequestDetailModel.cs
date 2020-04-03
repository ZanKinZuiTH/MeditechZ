using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ItemRequestDetailModel
    {
        public int ItemRequestDetailUID { get; set; }
        public int ItemRequestUID { get; set; }
        public int ItemMasterUID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public int IMUOMUID { get; set; }
        public string Unit { get; set; }
        public double CurrentQuantity { get; set; }
        public double StockQuantity { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public string MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
