using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PayorAgreementModel
    {
        public int PayorAgreementUID { get; set; }
        public string Name { get; set; }
        public Nullable<int> PBTYPUID { get; set; }
        public string PayorBillType { get; set; }
        public Nullable<int> PAYTRMUID { get; set; }
        public string PaymentTerms { get; set; }
        public int PayorDetailUID { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
