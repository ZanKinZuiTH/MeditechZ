using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class BillingGroupModel
    {
        public int BillingGroupUID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public int? ContactAgreementAccountUID { get; set; }
    }
}
