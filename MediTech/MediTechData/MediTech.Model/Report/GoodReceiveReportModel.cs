using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class GoodReceiveReportModel
    {
        public Int64 RowNumber { get; set; }
        public string VendorName { get; set; }

        public string GrnNumber { get; set; }
        public DateTime? ReceiveDttm { get; set; }
        public double? Discount { get; set; }
        public double? OtherCharges { get; set; }
        public double NetAmount { get; set; }
        public string InvoiceNo { get; set; }
        public string ReceivedOrganisation { get; set; }
        public string ReceivedStore { get; set; }
        public string GRNType { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double? FreeQuantity { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public string BatchID { get; set; }
        public DateTime? ExpiryDttm { get; set; }
        public double PurchaseCost { get; set; }
        public double NetAmountCost { get; set; }
        public string Manufacturer { get; set; }
        public string ReceiveBy { get; set; }

    }
}
