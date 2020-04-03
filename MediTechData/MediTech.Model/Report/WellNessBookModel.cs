using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class WellNessBookModel
    {
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string PatientAge { get; set; }
        public DateTime? ArrivedDttm { get; set; }
        public DateTime? DOBDttm { get; set; }
        public string IDCard { get; set; }
        public string CareproviderName { get; set; }
        public string CareproviderLicenseNo { get; set; }
        public string ChiefComplain { get; set; }

        public string Presentillness { get; set; }
        public string PEPlainText { get; set; }
        public string PERTFText { get; set; }
        //public string PERTFText { get; set; }
        public string DiagnosisString { get; set; }
        public string Investigation { get; set; }
        public string WellnessResult { get; set; }
    }
}
