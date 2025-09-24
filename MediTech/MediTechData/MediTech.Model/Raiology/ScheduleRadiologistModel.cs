using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public partial class ScheduleRadiologistModel
    {
        public int ScheduleRadiologistUID { get; set; }
        public int CareproviderUID { get; set; }
        public string CareproviderName { get; set; }
        public DateTime StartWorkTime { get; set; }
        public DateTime EndWorkTime { get; set; }
        public Nullable<int> RIMTYPUID { get; set; }
        public string ImageType { get; set; }
        public bool AllDay { get; set; }
        public int Status { get; set; }
        public int Label { get; set; }
        public int EventType { get; set; }
        public string RecurrenceInfo { get; set; }
        public string ReminderInfo { get; set; }
        public System.DateTime CWhen { get; set; }
        public System.DateTime MWhen { get; set; }
        public int CUser { get; set; }
        public int MUser { get; set; }
        public string StatusFlag { get; set; }
    }
}
