using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class StockReceiveReportModel
    {
        public string ReceivedOrganisation { get; set; }
        public string ReceivedStore { get; set; }
        public string IssuedOrganisation { get; set; }
        public string IssuedStore { get; set; }
        public string ReceiveID { get; set; }
        public string ReceiveStatus { get; set; }
        public string ReceivedBy { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double Itemcost { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public DateTime? ExpiryDttm { get; set; }
        public DateTime? ReceivedDttm { get; set; }
        public string Comments { get; set; }
    }
}
