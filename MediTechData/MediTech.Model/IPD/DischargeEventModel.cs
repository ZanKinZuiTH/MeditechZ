using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class DischargeEventModel
    {
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public long DischargeEventUID { get; set; }
        public long AdmissionEventUID { get; set; }
        public int DSCSTUID { get; set; }
        public DateTime? MedicalDischargeDttm { get; set; }
        public DateTime ActualDischargeDttm { get; set; }
        public int RecordedBy { get; set; }
        public string DischargeComments { get; set; }
        public int MDTRNUID { get; set; }
        public int? DSGDSUID { get; set; }
        public int DSCTYUID { get; set; }
        public int? INFCTUID { get; set; }
        public int? AdvicedBy { get; set; }
        public DateTime? DischargeAdviceDttm { get; set; }
        public int? DSHTYPUID { get; set; }
        public int? DSOCMUID { get; set; }
        public int? CARNSUID { get; set; }
        public string CancelledBy { get; set; }
        public DateTime? CancelledDttm { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public byte[] TIMESTAMP { get; set; }
        public int? VISTSUID { get; set; }
        public int? ENSTAUID { get; set; }
    }
}
