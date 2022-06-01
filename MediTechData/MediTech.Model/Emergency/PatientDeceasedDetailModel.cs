using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientDeceasedDetailModel
    {
        public long UID { get; set; }
        public long PatientVisitUID { get; set; }
        public int PatientUID { get; set; }
        public DateTime DeathDttm { get; set; }
        public DateTime? DeathTime { get; set; }
        public int? ConfirmedBy { get; set; }
        public int? DCSRNUID { get; set; }
        public string Comments { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public byte[] TIMESTAMP { get; set; }
    }
}
