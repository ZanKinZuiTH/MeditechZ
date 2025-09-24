using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class AppointmentRequestModel
    {
        public int AppointmentRequestUID { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public string PatientName { get; set; }
        public DateTime AppointmentDttm { get; set; }
        public int? CareProviderUID { get; set; }
        public string CareProviderName { get; set; }
        public DateTime? RequestedDate { get; set; }
        public int? RequestedBy { get; set; }
        public int BKSTSUID { get; set; }
        public string Comments { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public int LocationUID { get; set; }
        public string LocationName { get; set; }
        public string OwnerOrganisationName { get; set; }
        public string StatusFlag { get; set; }
        public string RequestStatus { get; set; }
        public bool IsCheckin { get; set; }
    }
}
