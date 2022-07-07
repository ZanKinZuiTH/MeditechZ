using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class AgreementAccountDiscountModel
    {
        public long AgreementAccountDiscountUID { get; set; }
        public int PayorAgreementUID { get; set; }
        public int ServiceUID { get; set; }
        public string ServiceName { get; set; }
        public double Discount { get; set; }
        public string IsPercentage { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public int PBLCTUID { get; set; }
        public int ALLDIUID { get; set; }
        public string AllowDiscount { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
