using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientAlertModel
    {
        public long PatientAlertUID { get; set; }
        public string AlertDescription { get; set; }
        public int? ALRTYUID { get; set; }
        public string AlertType { get; set; }
        public int? ALTSTUID { get; set; }
        public string Alert { get; set; }
        public int? SEVTYUID { get; set; }
        public string Severity { get; set; }
        public DateTime? ClosureDttm { get; set; }
        public DateTime? OnsetDttm { get; set; }
        public long PatientUID { get; set; }
        public long? PatientVisitUID { get; set; }
        public int? ALRPRTUID { get; set; }
        public string Priority { get; set; }
        public int? LocationUID { get; set; }
        public string Location { get; set; }
        public string IsVisitSpecific { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public string OwnerOrganisation { get; set; }
    }
}
