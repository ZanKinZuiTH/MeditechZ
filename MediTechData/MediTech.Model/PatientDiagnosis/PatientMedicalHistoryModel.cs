using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientMedicalHistoryModel
    {
        public int PatientMedicalHistoryUID { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public string CC { get; set; }
        public string PI { get; set; }
        public string PE { get; set; }
        public string Note { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public Nullable<int> OwnerOrganisationUID { get; set; }
    }
}
