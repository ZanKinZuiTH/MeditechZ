using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
  public class EcountMassFileModel
    {

        public int ItemMasterUID { get; set; }
        public string BatchID { get; set; }
        public string NewBatchID { get; set; }
        public string ItemCode { get; set; }
        public int StockUID { get; set; }
        public string ItemName { get; set; }

        public string SerialNumber { get; set; }

        public int ItemIssueDetailUID { get; set; }
        public int ItemIssueUID { get; set; }
        public double UnitPrice { get; set; }
        public double NetAmount { get; set; }
        public double ItemCost { get; set; }
        public double Quantity { get; set; }
        public double BatchQTY { get; set; }
        public int IMUOMUID { get; set; }
        public string Unit { get; set; }

        public Nullable<System.DateTime> ExpiryDttm { get; set; }
        public string Location { get; set; }
        public int? LocationUID { get; set; }



    }
}
