using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientBannerModel : PatientInformationModel
    {
        public long PatientVisitUID { get; set; }
        public string VisitID { get; set; }
        public string Note { get; set; }
        public string VisitStatus { get; set; }
        public DateTime VisitDate { get; set; }
        public string CareproviderName { get; set; }
        public bool IsAllergy { get; set; }
        public int? VisitCount { get; set; }
    }
}
