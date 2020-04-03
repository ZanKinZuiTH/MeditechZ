using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientOrderModel
    {
        public long PatientOrderUID { get; set; }
        public string OrderNumber { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public Nullable<System.DateTime> StartDttm { get; set; }
        public string Comments { get; set; }
        public int BSMDDUID { get; set; }
        public long? IdentifyingUID { get; set; }
        public string IdentifyingType { get; set; }
        public int OrderRaisedBy { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public List<PatientOrderDetailModel> PatientOrderDetail { get; set; }
    }
}
