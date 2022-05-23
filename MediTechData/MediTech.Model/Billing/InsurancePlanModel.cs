using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class InsurancePlanModel
    {
        public int InsurancePlanUID { get; set; }
        public string PayorAgreement { get; set; }
        public int PayorAgreementUID { get; set; }
        public Nullable<int> InsuranceCompanyUID { get; set; }
        public string PolicyName { get; set; }
        public int PolicyMasterUID { get; set; }
        public string PayorName { get; set; }
        public int PayorDetailUID { get; set; }
        public double? ClaimPercentage { get; set; }
        public double? FixedCopayAmount { get; set; }
        public double? OPDCoverPerDay { get; set; }
        public System.DateTime ActiveFrom { get; set; }
        public Nullable<System.DateTime> ActiveTo { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public string StatusFlag { get; set; }
    }
}
