using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class AEDischargeEventModel: PatientVisitModel
    {
        public long AEDischarge { get; set; }
        public long PatientAEAdmissionUID { get; set; }
        public DateTime CheckoutDttm { get; set; }
        public int? DSCCNDUID { get; set; }
        public int? DSCTYPUID { get; set; }
        public int? DESTINUID { get; set; }
        public DateTime? DeceasedDttm { get; set; }
        public int? ATSTYPUID { get; set; }
        public string DischargeEvents { get; set; }
        public int RecordedBy { get; set; }
        public string Comments { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public byte[] TIMESTAMP { get; set; }
        public int OwnerOrganisationUID { get; set; }
    }
}
