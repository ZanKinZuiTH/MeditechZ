using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientVisitModel : PatientInformationModel
    {
        public long PatientVisitUID { get; set; }
        public Nullable<int> CareProviderUID { get; set; }
        public string CareProviderName { get; set; }
        public Nullable<int> VISTSUID { get; set; }
        public Nullable<int> CheckupJobUID { get; set; }
        public string CompanyName { get; set; }
        public string VisitType { get; set; }
        public string VisitStatus { get; set; }
        public Nullable<int> VISTYUID { get; set; }
        public Nullable<int> PRITYUID { get; set; }
        public Nullable<System.DateTime> StartDttm { get; set; }
        public Nullable<System.DateTime> EndDttm { get; set; }
        public Nullable<System.DateTime> ArrivedDttm { get; set; }
        public Nullable<System.DateTime> DischargeDttm { get; set; }
        
        public string VisitID { get; set; }
        public string IsBillFinalized { get; set; }
        public string Comments { get; set; }
        public int? RefNo { get; set; }
        public int? BookingUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public string OwnerOrganisation { get; set; }
        public int PayorDetailUID { get; set; }
        public string PayorName { get; set; }
        public int PayorAgreementUID { get; set; }
        public bool Select { get; set; }
    }
}
