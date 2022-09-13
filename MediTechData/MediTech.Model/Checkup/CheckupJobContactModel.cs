using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class CheckupJobContactModel
    {
        public int CheckupJobContactUID { get; set; }
        public System.Guid JobContactID { get; set; }
        public string JobNumber { get; set; }
        public int InsuranceCompanyUID { get; set; }
        public string InsuranceCompanyName { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string ServiceName { get; set; }
        public int VisitCount { get; set; }
        public DateTime StartDttm { get; set; }
        public Nullable<System.DateTime> EndDttm { get; set; }
        public Nullable<System.DateTime> CollectDttm { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }

        public List<CheckupJobTaskModel> CheckupJobTasks { get; set; }
    }
}
