using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class NotificationTaskResultModel
    {
        public long UID { get; set; }
        public long ResultUID { get; set; }
        public string NetworkPartnerID { get; set; }
        public System.DateTime ResultEnteredDttm { get; set; }
        public System.DateTime CWhen { get; set; }
        public System.DateTime MWhen { get; set; }
        public int OwnerOrganisationUID { get; set; }
    }
}
