using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientInsuranceDetailModel
    {
        public long PatientInsuranceDetailUID { get; set; }
        public long PatientUID { get; set; }
        public Nullable<long> PatientVisitUID { get; set; }
        public int InsuranceCompanyUID { get; set; }
        public string InsuranceCompanyName { get; set; }
        public string PolicyNumber { get; set; }
        public string CardNumber { get; set; }
        public Nullable<System.DateTime> StartDttm { get; set; }
        public Nullable<System.DateTime> EndDttm { get; set; }
        public Nullable<double> EligibleAmount { get; set; }
        public Nullable<int> PayorDetailUID { get; set; }
        public string PayorName { get; set; }
        public string CCNNumber { get; set; }
        public Nullable<int> PolicyMasterUID { get; set; }
        public string PolicyName { get; set; }
        public Nullable<int> PayorAgreementUID { get; set; }
        public string PayorAgreementName { get; set; }
        public Nullable<int> PAYRTPUID { get; set; }
        public String PayorType { get; set; }
        public Nullable<double> ClaimPercentage { get; set; }
        public Nullable<double> FixedCopayAmount { get; set; }
        public string Comments { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public string Type { get; set; }
    }
}
