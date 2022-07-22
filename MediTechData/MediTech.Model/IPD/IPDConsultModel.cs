using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class IPDConsultModel
    {

        public long PatientConsultionUID { get; set; }
        public long PatientUID { get; set; }
        public long? PatientVisitUID { get; set; }
        public string PatientName { get; set; }
        public string PatientID { get; set; }

        public string VisitID { get; set; }
        public Nullable<int> CareProviderUID { get; set; }
        public Nullable<int> VISTSUID { get; set; }
        public Nullable<int> CONSTSUID { get; set; }
        public String ConultTypeStr { get; set; }
        public String ConultStatusStr { get; set; }

        public Nullable<int> CONTYPUID { get; set; }
  
        public string CareProviderName { get; set; }

        public DateTime? StartConsultDate { get; set; }
        public DateTime? EndConsultDate { get; set; }

        public string Note { get; set; }
        public Nullable<int> OwnerOrganisationUID { get; set; }
        public string OwnerOrganisation { get; set; }

        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
