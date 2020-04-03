using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class DoctorFeeReportModel
    {
        public string CareproviderName { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string BillNumber { get; set; }
        public DateTime BillGeneratedDttm { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        public double DoctorFee { get; set; }
    }
}
