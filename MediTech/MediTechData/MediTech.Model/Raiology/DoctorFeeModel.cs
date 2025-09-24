using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class DoctorFeeModel
    {
        public long DoctorFeeUID { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public string PatientName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }

        public string CareproviderName { get; set; }
        public int CareproviderUID { get; set; }
        public long PatientOrderDetailUID { get; set; }
        public System.DateTime StartDttm { get; set; }

        public System.DateTime? ResultEnteredDate { get; set; }
        public System.DateTime? ResultEnteredTime { get; set; }
        public double NetAmount { get; set; }
        public double DoctorFee { get; set; }
        public double Vat { get; set; }
        public double Radread { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
