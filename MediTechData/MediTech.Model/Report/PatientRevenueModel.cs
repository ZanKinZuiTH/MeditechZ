using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class PatientRevenueModel
    {
        public Int64 RowNumber { get; set; }
        public string PatientName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string VisitID { get; set; }
        public string PatientID { get; set; }
        public string NationalID { get; set; }
        public string PatientOtherID { get; set; }
        public DateTime VisitDttm { get; set; }
        public double Qty { get; set; }
        public string Store { get; set; }
        public string BatchID { get; set; }
        public string Unit { get; set; }
        public double UnitCost { get; set; }
        public double UnitPrice { get; set; }
        public double NetCost { get; set; }
        public double Discount { get; set; }
        public double NetPrice { get; set; }
        public double Profit { get; set; }
        public string Status { get; set; }
        public string BillNumber { get; set; }
        public string HealthOrganisationName { get; set; }
    }
}
