using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class DoctorFeeReportModel
    {
        public int No { get; set; }
        public string CareproviderName { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string BillNumber { get; set; }
        public DateTime BillGeneratedDttm { get; set; }
        public String VisitType { get; set; } 
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        public double DoctorFee { get; set; }
        public string IsICD10 { get; set; }
        public DateTime StartDttm { set; get; }
        public int OwnerOrganisationUID { get; set; }
        public string OwnerOrganisation { get; set; }
    }
}
