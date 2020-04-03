using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class StockReportModel
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public double UnitCost { get; set; }
        public double AvgCost { get; set; }
        public string ItemForm { get; set; }
        public string Store { get; set; }
        public string Unit { get; set; }
        public double Price { get; set; }
        public double NetPrice { get; set; }
        public double StockQty { get; set; }
        public double UsedQty { get; set; }
        public double CurrentQty { get; set; }
        public double TotalCost { get; set; }
        public double TotalAvgCost { get; set; }
        public string BatchID { get; set; }
        public string IsExpired { get; set; }
        public DateTime? ExpiryDttm { get; set; }
        public DateTime? StockDttm { get; set; }
        public string HealthOrganisationName { get; set; }
        public string MouthName { get; set; }
        public int Mouth { get; set; }
        public int Years { get; set; }
        public string OrderStatus { get; set; }
    }
}
