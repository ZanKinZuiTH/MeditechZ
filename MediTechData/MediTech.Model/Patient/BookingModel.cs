using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class BookingModel
    {
        public int BookingUID { get; set; }
        public long PatientUID { get; set; }
        public string PatientName { get; set; }
        public string Phone { get; set; }
        public int BKSTSUID { get; set; }
        public string BookingStatus { get; set; }
        public Nullable<int> PATMSGUID { get; set; }
        public string PatientReminderMessage { get; set; }

        public System.DateTime AppointmentDate
        {
            get { return AppointmentDttm.Date; }
        }
        public System.DateTime AppointmentDttm { get; set; }
        public Nullable<int> CareProviderUID { get; set; }
        public string CareProviderName { get; set; }
        public string Comments { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public int OwnerOrganisationUID { get; set; }

        public string OwnerOrganisationName { get; set; }
        public string StatusFlag { get; set; }
    }
}
