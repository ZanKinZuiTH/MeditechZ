using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class GeneratePatientBillModel
    {
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public long PatientBillUID { get; set; }     
        public DateTime BillGenerateDttm { get; set; }
        public double? AdvanceAmount { get; set; }

        public double? TotalAmount { get; set; }

        public double? DiscountAmount { get; set; }

        public double? NetAmount { get; set; }

        public int? PBTYPUID { get; set; }
        public int? BLTYPUID { get; set; }

        public int? BLCATUID { get; set; }

        public long? PatientVisitPayorUID { get; set; }
        public int? PayorDetailUID { get; set; }
        public int? PayorAgreementUID { get; set; }
        public int UserUID { get; set; }

        public int? OwnerOrganisationUID { get; set; } 
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
        public string Comments { get; set; }
        public List<AllocatedPatBillableItemsAccountResultModel> PatientBillableItemsAccounts { get; set; }

        public List<PatientPaymentDetailModel> PatientPaymentDetails { get; set; }
    }
}
