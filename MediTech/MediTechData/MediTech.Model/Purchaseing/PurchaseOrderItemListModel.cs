using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PurchaseOrderItemListModel
    {
        public int PurchaseOrderItemListUID { get; set; }
        public Nullable<int> PurchaseOrderUID { get; set; }
        public int ItemMasterUID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public int IMUOMUID { get; set; }
        public string Unit { get; set; }
        public double UnitPrice { get; set; }
        public double NetAmount { get; set; }
        public string Comments { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
