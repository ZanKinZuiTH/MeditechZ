using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class BillConfigurationModel
    {
        public int BillConfigurationUID { get; set; }
        public int CURNCUID { get; set; }
        public int HealthOrganisationUID { get; set; }
        public Nullable<double> ServiceTax { get; set; }
        public Nullable<double> VAT { get; set; }
        public Nullable<int> StoreUID { get; set; }
        public string IsDisAuthorizationReqd { get; set; }
        public Nullable<double> RoundOff { get; set; }
        public Nullable<double> MaxDiscountPercentage { get; set; }
        public string IsAdvanceWarningReqd { get; set; }
        public string IsBillSettlmentReqd { get; set; }
        public Nullable<double> EducationCess { get; set; }
        public Nullable<double> HigherEducationCess { get; set; }
        public Nullable<double> TDSPercentage { get; set; }
        public Nullable<int> BDTRFUID { get; set; }
        public string IsServiceTaxApplicable { get; set; }
        public string IsServiceTaxOPApplicable { get; set; }
        public Nullable<double> ServiceTaxOP { get; set; }
        public Nullable<int> EditIPBill { get; set; }
        public Nullable<int> WeekHolidaySpecialRate { get; set; }
        public string HolidaySpecialRate { get; set; }
        public Nullable<int> NightHourStart { get; set; }
        public Nullable<int> NightHourEnd { get; set; }
        public string IsPayRollIntegrated { get; set; }
        public Nullable<int> BillRefundHour { get; set; }
        public Nullable<int> CancelBill { get; set; }
        public string OPBillAccountSplit { get; set; }
        public string IPBillAccountSplit { get; set; }
        public Nullable<double> BillRoundOff { get; set; }
        public Nullable<int> PayorUID { get; set; }
        public Nullable<int> InsuranceCompanyUID { get; set; }
        public Nullable<int> PayorAgreementUID { get; set; }
        public Nullable<int> AssociatedServiceUID { get; set; }
        public Nullable<int> ROFLEUID { get; set; }
        public Nullable<int> ROFTYUID { get; set; }
        public string PackageQtyMax { get; set; }
        public string OrderSetOverWrite { get; set; }
        public Nullable<int> IPDAMOUNTBILL { get; set; }
        public Nullable<int> SearchMaxDayInOrderList { get; set; }
        public string ShowDFByDoctor { get; set; }
        public string IsBRHHideXRay { get; set; }
        public string IsAutoMeargeReceipt { get; set; }
        public string IsAutoRegis { get; set; }
        public string ISPatientForeigner { get; set; }
        public string IsAlterDoctor { get; set; }
        public string IspopupMedreconcileInOrder { get; set; }
        public string IsCompleteDF { get; set; }
        public string IsAlertMeddischarge { get; set; }
        public Nullable<int> IsOrderLock { get; set; }
        public Nullable<int> IsOrderStartDate { get; set; }
        public string IsBillDefaultPrintCheckedDoctor { get; set; }
        public string CanLookPriceOrder { get; set; }
        public string IsUseQtyInTemplateOrderFood { get; set; }
        public string IsShowQtyAllInStock { get; set; }
        public string IsUseSearchOrderOrderbyRecent { get; set; }
        public string IsUseFixPrice { get; set; }
        public string IsFixPriceUseDateTime { get; set; }
        public string IsAutoServiceChargeAtCashier { get; set; }
        public string IsBlockDFFixPriceOver { get; set; }
        public string IspopupMedreconcileInOrder_Type { get; set; }
        public string IsOrderPackageNotOrderPermission { get; set; }
        public string IsOrderStandingInOPD { get; set; }
        public string IsUseWithoutstock { get; set; }
        public string IsUseAlterDoctorComplex { get; set; }
        public string IsBlockOrderAllergy { get; set; }
        public string IsAutoPrintOrderSheet { get; set; }
        public string IsDrugDuplicateInVisit { get; set; }
        public string IsOrderDFCheckDiagInVisitByDoctor { get; set; }
        public string IsUseTemplateMedOrderOldVersion { get; set; }
        public string IsUseSSO { get; set; }
        public string IsCheckDiagForMed { get; set; }
        public string IsAutoCheckCompleteDF { get; set; }
        public Nullable<int> PayoragreementSpecialDiscountUID { get; set; }
        public string IsUseControlFrequencyNew { get; set; }
        public string IsPrintOwnmed { get; set; }
        public string IsUseModeLockOrderDF { get; set; }
        public string LockOrderDFIorO { get; set; }
        public string LockOrderBlockOrWarning { get; set; }
        public Nullable<int> CancelAdmissionDuration { get; set; }
        public string IsUseXRayRequiNote { get; set; }
        public string TypeVisitForXRayRequiNote { get; set; }
        public string IsPrintQdetailAfterSettleBill { get; set; }
        public string IsUseCheckHasAllergy { get; set; }
        public string IsNotBlockDuplicateOrderInGrid { get; set; }
        public string IsUseFunc_AgreementVisitIPD { get; set; }
        public string IsPrintPrescriptionWhenSaveOrder { get; set; }
        public Nullable<int> PlanForNewBorn { get; set; }
        public string ChangeStandingOwnmed { get; set; }
        public string IsDefaultQtyInSTAT { get; set; }
        public string IsUseAllergyIsConfirm { get; set; }
        public string IsTypeStatandFreqStat { get; set; }
        public Nullable<int> FreqStatUID { get; set; }
        public string IsUseGenStat { get; set; }
        public string UseTPRNormal { get; set; }
        public string UseTPRNBNH { get; set; }
        public Nullable<int> DefaultFreqStatForGenStatUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public int OwnerOrganisationUID { get; set; }
    }
}
