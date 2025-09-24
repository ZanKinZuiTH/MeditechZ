using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class AdmissionEventModel
    {
        public long AdmissionEventUID { get; set; }
        public long PatientVisitUID { get; set; }
        public long PatientUID { get; set; }
        public int CarepoviderUID { get; set; }
        public int? ExpectedLengthOfStay { get; set; }
        public DateTime AdmissionDttm { get; set; }
        public DateTime? ExpectedDischargeDttm { get; set; }
        public int? IPBookingUID { get; set; }
        public int? PreviousEventUID { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public string Comments { get; set; }
        public DateTime? ValidFromDttm { get; set; }
        public DateTime? ValidToDttm { get; set; }
        public string IsNoVisitorAllowed { get; set; }
        public int? RequestingLocationUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public byte[] TIMESTAMP { get; set; }
    }
}
