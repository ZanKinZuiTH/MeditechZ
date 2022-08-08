using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientVisitPayorModel
    {
        public long PatientVisitPayorUID { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public int PayorDetailUID { get; set; }
        public string PayorName { get; set; }
        public int PayorAgreementUID { get; set; }
        public string AgreementName { get; set; }
        public Nullable<int> PolicyMasterUID { get; set; }
        public string PolicyName { get; set; }
        public Nullable<int> InsuranceCompanyUID { get; set; }
        public string InsuranceName { get; set; }
        public Nullable<double> EligibleAmount { get; set; }
        public Nullable<int> PAYRTPUID { get; set; }

        public int? PBTYPUID { get; set; }
        public int? BLTYPUID { get; set; }
        public string PayorType { get; set; }
        public Nullable<System.DateTime> ActiveFrom { get; set; }
        public Nullable<System.DateTime> ActiveTo { get; set; }
        public Nullable<double> ClaimPercentage { get; set; }
        public Nullable<double> FixedCopayAmount { get; set; }
        public Nullable<double> CoveredAmount { get; set; }
        public Nullable<double> ClaimAmount { get; set; }
        public string Comment { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }

        public int OwnerOrganisationUID { get; set; }

        private bool _IsExpired;

        public bool IsExpired
        {
            get
            {
                if (ActiveTo.HasValue && ActiveTo.Value.Date < DateTime.Now.Date)
                {
                    return true;
                }
                return false;
            }
            set { _IsExpired = value; }
        }

        public string Description { get; set; }

        public string CreatedBy { get; set; }

    }
}
