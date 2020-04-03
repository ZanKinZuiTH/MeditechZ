using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PrescriptionModel
    {
        public long PrescriptionUID { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public string PrescriptionNumber { get; set; }
        public string PrescriptionStatus { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string OrganisationName { get; set; }
        public int PrescribedBy { get; set; }
        public System.DateTime PrescribedDttm { get; set; }
        public string Comments { get; set; }
        public int? ORDSTUID { get; set; }
        public Nullable<int> BSMDDUID { get; set; }
        public Nullable<System.DateTime> DispensedDttm { get; set; }
        public long PatientOrderUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public Nullable<int> OwnerOrganisationUID { get; set; }
    }
}
