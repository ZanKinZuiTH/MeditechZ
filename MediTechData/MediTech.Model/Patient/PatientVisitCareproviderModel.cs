using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientVisitCareproviderModel
    {
        public long PatientVisitCareproviderUID { get; set; }
        public long PatientVisitUID { get; set; }
        public DateTime? StartDttm { get; set; }
        public DateTime? SeenDttm { get; set; }
        public int CareProviderUID { get; set; }
        public string CareProviderName { get; set; }
        public int? ReferralUID { get; set; }
        public int? PACLSUID { get; set; }
        public int? BookingUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public int LocationUID { get; set; }
        public string Location { get; set; }
        public string OwnerOrganisationName { get; set; }
        public string StatusFlag { get; set; }
    }
}
