using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientOrderDetailModel
    {
        public long PatientOrderDetailUID { get; set; }
        public long PatientOrderUID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public long? IdentifyingUID { get; set; }

        public string IdentifyingType { get; set; }
        public Nullable<System.DateTime> StartDttm { get; set; }

        public Nullable<System.DateTime> EndDttm { get; set; }
        public int ORDSTUID { get; set; }
        public string OrderDetailStatus { get; set; }
        public string PaymentStatus { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> BalQty { get; set; }
        public Nullable<int> QNUOMUID { get; set; }
        public string QuantityUnit { get; set; }
        public string Comments { get; set; }
        public Nullable<double> UnitPrice { get; set; }

        public Nullable<double> OriginalUnitPrice { get; set; }
        public Nullable<double> DoctorFee { get; set; }
        public Nullable<double> DoctorFeePer { get; set; }
        public Nullable<double> NetAmount { get; set; }
        public Nullable<double> DisplayPrice { get; set; }
        public string IsPriceOverwrite { get; set; }
        public Nullable<double> OverwritePrice { get; set; }
        public Nullable<int> FRQNCUID { get; set; }
        public string DrugFrequency { get; set; }
        public int DrugGenaricUID { get; set; }
        public string GenericName { get; set; }
        public Nullable<int> ROUTEUID { get; set; }
        public Nullable<int> DFORMUID { get; set; }
        public Nullable<int> PRSTYPUID { get; set; }
        public string OrderType { get; set; }
        public string TypeDrug { get; set; }
        public Nullable<int> PDSTSUID { get; set; }
        public string InstructionRoute { get; set; }

        public Nullable<double> Dosage { get; set; }
        public string DosageUnit { get; set; }
        public Nullable<int> DrugDuration { get; set; }
        public string InstructionText { get; set; }
        public string LocalInstructionText { get; set; }
        public string ClinicalComments { get; set; }
        public int BillableItemUID { get; set; }
        public Nullable<int> ItemUID { get; set; }
        public Nullable<int> StoreUID { get; set; }
        public string StoreName { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public String BatchID { get; set; }
        public Nullable<int> StockUID { get; set; }
        public Nullable<int> BillPackageUID { get; set; }

        public Nullable<int> CareproviderUID { get; set; }
        public String CareproviderName { get; set; }
        public Nullable<int> CancelledByUserUID { get; set; }
        public Nullable<System.DateTime> CancelledDttm { get; set; }
        public Nullable<long> PatientPackageUID { get; set; }
        public int BSMDDUID { get; set; }

        public string BillingService { get; set; }

        public string IsDoctorOnly { get; set; }
        public string IsStock { get; set; }
        public Nullable<int> OrderSetUID { get; set; }
        public string OrderSetName { get; set; }
        public Nullable<int> OrderSetBillableItemUID { get; set; }
        public string OrderNumber { get; set; }
        public string OrderBy { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public string OwnerOrganisationName { get; set; }
        public int OwnerOrganisationUID { get; set; }

        public int LocationUID { get; set; }
        public string LocationName { get; set; }

        public System.Nullable<double> Discount { get; set; }

        public List<PatientOrderAlertModel> PatientOrderAlert { get; set; }
        public bool IsSelected { get; set; }

        public bool IsWithoutStock { get; set; }
        public bool IsExpired { get; set; }
        public string IsStandingOrder { get; set; }
        public int? OrderCatagoryUID { get; set; }
        public int? OrderSubCategoryUID { get; set; }
    }
}
