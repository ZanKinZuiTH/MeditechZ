using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ItemIssueDetailModel
    {
        public int ItemIssueDetailUID { get; set; }
        public int ItemIssueUID { get; set; }
        public int ItemMasterUID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double UnitPrice { get; set; }
        public double NetAmount { get; set; }
        public double ItemCost { get; set; }
        public double Quantity { get; set; }
        public int IMUOMUID { get; set; }
        public string Unit { get; set; }
        public string BatchID { get; set; }
        public int StockUID { get; set; }
        public Nullable<System.DateTime> ExpiryDttm { get; set; }
        public string Place { get; set; }
        public int? PLACEUID { get; set; }

        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public int OrganisationUID { get; set; }
        public int StoreUID { get; set; }
        public string StatusFlag { get; set; }
    }
}
