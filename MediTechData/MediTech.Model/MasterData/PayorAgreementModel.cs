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
        public string Description { get; set; }
        public string Code { get; set; }
        public Nullable<int> PBTYPUID { get; set; }
        public string PayorBillType { get; set; }
        public Nullable<int> PAYTRMUID { get; set; }
        public string PaymentTerms { get; set; }
        public int PayorDetailUID { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public string AgentName { get; set; }
        public int? CRDTRMUID { get; set; }
        public string IsForeign { get; set; }
        public int? BLTYPUID { get; set; }
        public int? AGTYPUID { get; set; }
        public int? PrimaryPBLCTUID { get; set; }
        public int? SecondaryPBLCTUID { get; set; }
        public int? TertiaryPBLCTUID { get; set; }
        public double? OPDCoverPerDay { get; set; }
        public double? ClaimPercentage { get; set; }
        public double? FixedCopayAmount { get; set; }
        public string IsPackageDiscountAllowed { get; set; }
        public string IsLimitAfterDiscount { get; set; }
        public int? DisplayOrder { get; set; }
        public int? InsuranceCompanyUID { get; set; }
        public int? PolicyMasterUID { get; set; }
        public int? OldAgreemntUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
