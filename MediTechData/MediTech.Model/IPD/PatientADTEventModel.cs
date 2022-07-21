using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientADTEventModel
    {
        public long PatientADTEventUID { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public int ENVTYUID { get; set; }
        public string ENVTY { get; set; }
        public DateTime EventOccuredDttm { get; set; }
        public long IdentifyingUID { get; set; }
        public string IdentifyingType { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public byte[] TIMESTAMP { get; set; }
    }
}
