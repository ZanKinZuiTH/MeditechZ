 /*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:MediTech"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MediTech.ViewModels.Doctor;
using MediTech.ViewModels.Inventory;
using Microsoft.Practices.ServiceLocation;

namespace MediTech.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in thec
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            //////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
            var culture = new System.Globalization.CultureInfo("en-us");
            culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            culture.DateTimeFormat.LongTimePattern = "HH:mm:ss";
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;

            //SimpleIoc.Default.Register<LoginScreenViewModel>();
            //SimpleIoc.Default.Register<MainViewModel>();

            //SimpleIoc.Default.Register<ManagePatientViewModel>();
            //SimpleIoc.Default.Register<SearchPatientViewModel>();
            //SimpleIoc.Default.Register<RegisterPatientViewModel>();
        }

        public MainViewModel MainViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<MainViewModel>())
                    SimpleIoc.Default.Register<MainViewModel>();

                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        #region Home

        public ChangePasswordViewModel ChangePasswordViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ChangePasswordViewModel>())
                    SimpleIoc.Default.Register<ChangePasswordViewModel>();

                return ServiceLocator.Current.GetInstance<ChangePasswordViewModel>();
            }
        }

        #endregion

        #region Billing

        public CashierWorklistViewModel CashierWorklistViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<CashierWorklistViewModel>())
                    SimpleIoc.Default.Register<CashierWorklistViewModel>();

                return ServiceLocator.Current.GetInstance<CashierWorklistViewModel>();
            }
        }

        public PatientBilledOPViewModel PatientBilledOPViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PatientBilledOPViewModel>())
                    SimpleIoc.Default.Register<PatientBilledOPViewModel>();

                return ServiceLocator.Current.GetInstance<PatientBilledOPViewModel>();
            }
        }

        public BillSettlementOPViewModel BillSettlementOPViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<BillSettlementOPViewModel>())
                    SimpleIoc.Default.Register<BillSettlementOPViewModel>();

                return ServiceLocator.Current.GetInstance<BillSettlementOPViewModel>();
            }
        }

        public ListQueryBillsOPViewModel ListQueryBillsOPViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListQueryBillsOPViewModel>())
                    SimpleIoc.Default.Register<ListQueryBillsOPViewModel>();

                return ServiceLocator.Current.GetInstance<ListQueryBillsOPViewModel>();
            }
        }

        public GenerateBillViewModel GenerateBillViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<GenerateBillViewModel>())
                    SimpleIoc.Default.Register<GenerateBillViewModel>();

                return ServiceLocator.Current.GetInstance<GenerateBillViewModel>();
            }
        }

        public AllocateBillPopupViewModel AllocateBillPopupViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<AllocateBillPopupViewModel>())
                    SimpleIoc.Default.Register<AllocateBillPopupViewModel>();

                return ServiceLocator.Current.GetInstance<AllocateBillPopupViewModel>();
            }
        }

        public ListQueryBillsIPViewModel ListQueryBillsIPViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListQueryBillsIPViewModel>())
                    SimpleIoc.Default.Register<ListQueryBillsIPViewModel>();

                return ServiceLocator.Current.GetInstance<ListQueryBillsIPViewModel>();
            }
        }

        public BillSettlementIPViewModel BillSettlementIPViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<BillSettlementIPViewModel>())
                    SimpleIoc.Default.Register<BillSettlementIPViewModel>();

                return ServiceLocator.Current.GetInstance<BillSettlementIPViewModel>();
            }
        }

        public PatientBilledIPViewModel PatientBilledIPViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PatientBilledIPViewModel>())
                    SimpleIoc.Default.Register<PatientBilledIPViewModel>();

                return ServiceLocator.Current.GetInstance<PatientBilledIPViewModel>();
            }
        }

        public ListBillPaymentViewModel ListBillPaymentViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListBillPaymentViewModel>())
                    SimpleIoc.Default.Register<ListBillPaymentViewModel>();

                return ServiceLocator.Current.GetInstance<ListBillPaymentViewModel>();
            }
        }

        public ListPaymentDetailsViewModel ListPaymentDetailsViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListPaymentDetailsViewModel>())
                    SimpleIoc.Default.Register<ListPaymentDetailsViewModel>();

                return ServiceLocator.Current.GetInstance<ListPaymentDetailsViewModel>();
            }
        }

        public BillingPaymentModePopUpViewModel BillingPaymentModePopUpViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<BillingPaymentModePopUpViewModel>())
                    SimpleIoc.Default.Register<BillingPaymentModePopUpViewModel>();

                return ServiceLocator.Current.GetInstance<BillingPaymentModePopUpViewModel>();
            }                                                                                                                   
        }

        public MergeBillRecipetPopupViewModel MergeBillRecipetPopupViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<MergeBillRecipetPopupViewModel>())
                    SimpleIoc.Default.Register<MergeBillRecipetPopupViewModel>();

                return ServiceLocator.Current.GetInstance<MergeBillRecipetPopupViewModel>();
            }
        }

        public CancelBillPopupViewModel CancelBillPopupViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<CancelBillPopupViewModel>())
                    SimpleIoc.Default.Register<CancelBillPopupViewModel>();

                return ServiceLocator.Current.GetInstance<CancelBillPopupViewModel>();
            }
        }

        public SearchItemViewModel SearchItemViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<SearchItemViewModel>())
                    SimpleIoc.Default.Register<SearchItemViewModel>();

                return ServiceLocator.Current.GetInstance<SearchItemViewModel>();
            }
        }

        public ListQueryBillsMassForInvoiceViewModel ListQueryBillsMobileViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListQueryBillsMassForInvoiceViewModel>())
                    SimpleIoc.Default.Register<ListQueryBillsMassForInvoiceViewModel>();

                return ServiceLocator.Current.GetInstance<ListQueryBillsMassForInvoiceViewModel>();
            }
        }

        #endregion

        #region BillingConfiguration

        public ListBillableItemViewModel ListBillableItemViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListBillableItemViewModel>())
                    SimpleIoc.Default.Register<ListBillableItemViewModel>();

                return ServiceLocator.Current.GetInstance<ListBillableItemViewModel>();
            }
        }

        public ManageBillableItemViewModel ManageBillableItemViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageBillableItemViewModel>())
                    SimpleIoc.Default.Register<ManageBillableItemViewModel>();

                return ServiceLocator.Current.GetInstance<ManageBillableItemViewModel>();
            }
        }

        public ListOrderSetViewModel ListOrderSetViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListOrderSetViewModel>())
                    SimpleIoc.Default.Register<ListOrderSetViewModel>();

                return ServiceLocator.Current.GetInstance<ListOrderSetViewModel>();
            }
        }

        public ManageOrderSetViewModel ManageOrderSetViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageOrderSetViewModel>())
                    SimpleIoc.Default.Register<ManageOrderSetViewModel>();

                return ServiceLocator.Current.GetInstance<ManageOrderSetViewModel>();
            }
        }

        public ListPayorDetailViewModel ListPayorDetailViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListPayorDetailViewModel>())
                    SimpleIoc.Default.Register<ListPayorDetailViewModel>();

                return ServiceLocator.Current.GetInstance<ListPayorDetailViewModel>();
            }
        }

        public ManagePayorDetailViewModel ManagePayorDetailViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManagePayorDetailViewModel>())
                    SimpleIoc.Default.Register<ManagePayorDetailViewModel>();

                return ServiceLocator.Current.GetInstance<ManagePayorDetailViewModel>();
            }
        }

        public InsuranceCompanyViewModel InsuranceCompanyViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<InsuranceCompanyViewModel>())
                    SimpleIoc.Default.Register<InsuranceCompanyViewModel>();

                return ServiceLocator.Current.GetInstance<InsuranceCompanyViewModel>();
            }
        }

        public ManageInsuranceCompanyViewModel ManageInsuranceCompanyViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageInsuranceCompanyViewModel>())
                    SimpleIoc.Default.Register<ManageInsuranceCompanyViewModel>();

                return ServiceLocator.Current.GetInstance<ManageInsuranceCompanyViewModel>();
            }
        }

        public ManagePayorAgreementViewModel ManagePayorAgreementViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManagePayorAgreementViewModel>())
                    SimpleIoc.Default.Register<ManagePayorAgreementViewModel>();

                return ServiceLocator.Current.GetInstance<ManagePayorAgreementViewModel>();
            }
        }

        public ManageInsurancePlanViewModel ManageInsurancePlanViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageInsurancePlanViewModel>())
                    SimpleIoc.Default.Register<ManageInsurancePlanViewModel>();

                return ServiceLocator.Current.GetInstance<ManageInsurancePlanViewModel>();
            }
        }

        public ListPolicyMasterViewModel ListPolicyMasterViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListPolicyMasterViewModel>())
                    SimpleIoc.Default.Register<ListPolicyMasterViewModel>();

                return ServiceLocator.Current.GetInstance<ListPolicyMasterViewModel>();
            }
        }

        public ManagePolicyMasterViewModel ManagePolicyMasterViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManagePolicyMasterViewModel>())
                    SimpleIoc.Default.Register<ManagePolicyMasterViewModel>();

                return ServiceLocator.Current.GetInstance<ManagePolicyMasterViewModel>();
            }
        }

        public ListPackageViewModel ListPackageViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListPackageViewModel>())
                    SimpleIoc.Default.Register<ListPackageViewModel>();

                return ServiceLocator.Current.GetInstance<ListPackageViewModel>();
            }
        }

        public ManagePackageViewModel ManagePackageViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManagePackageViewModel>())
                    SimpleIoc.Default.Register<ManagePackageViewModel>();

                return ServiceLocator.Current.GetInstance<ManagePackageViewModel>();
            }
        }

        #endregion

        #region Documents

        public PatientReportViewModel PatientReportViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PatientReportViewModel>())
                    SimpleIoc.Default.Register<PatientReportViewModel>();

                return ServiceLocator.Current.GetInstance<PatientReportViewModel>();
            }
        }

        public RegistrationReportViewModel RegistrationReportViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<RegistrationReportViewModel>())
                    SimpleIoc.Default.Register<RegistrationReportViewModel>();

                return ServiceLocator.Current.GetInstance<RegistrationReportViewModel>();
            }
        }

        public CashierReportViewModel CashierReportViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<CashierReportViewModel>())
                    SimpleIoc.Default.Register<CashierReportViewModel>();

                return ServiceLocator.Current.GetInstance<CashierReportViewModel>();
            }
        }

        public EcountReportViewModel EcountReportViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EcountReportViewModel>())
                    SimpleIoc.Default.Register<EcountReportViewModel>();

                return ServiceLocator.Current.GetInstance<EcountReportViewModel>();
            }
        }


        public CheckupJobSummeryReportViewModel CheckupJobSummeryReportViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<CheckupJobSummeryReportViewModel>())
                    SimpleIoc.Default.Register<CheckupJobSummeryReportViewModel>();

                return ServiceLocator.Current.GetInstance<CheckupJobSummeryReportViewModel>();
            }
        }

        public InventoryReportViewModel InventoryReportViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<InventoryReportViewModel>())
                    SimpleIoc.Default.Register<InventoryReportViewModel>();

                return ServiceLocator.Current.GetInstance<InventoryReportViewModel>();
            }
        }

        public RadiologyReportViewModel RadiologyReportViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<RadiologyReportViewModel>())
                    SimpleIoc.Default.Register<RadiologyReportViewModel>();

                return ServiceLocator.Current.GetInstance<RadiologyReportViewModel>();
            }
        }


        public ReportParameter1ViewModel ReportParameter1ViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ReportParameter1ViewModel>())
                    SimpleIoc.Default.Register<ReportParameter1ViewModel>();

                return ServiceLocator.Current.GetInstance<ReportParameter1ViewModel>();
            }
        }

        public ReportParameter2ViewModel ReportParameter2ViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ReportParameter2ViewModel>())
                    SimpleIoc.Default.Register<ReportParameter2ViewModel>();

                return ServiceLocator.Current.GetInstance<ReportParameter2ViewModel>();
            }
        }

        public ReportParameter3ViewModel ReportParameter3ViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ReportParameter3ViewModel>())
                    SimpleIoc.Default.Register<ReportParameter3ViewModel>();

                return ServiceLocator.Current.GetInstance<ReportParameter3ViewModel>();
            }
        }

        public ReportParameter4ViewModel ReportParameter4ViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ReportParameter4ViewModel>())
                    SimpleIoc.Default.Register<ReportParameter4ViewModel>();

                return ServiceLocator.Current.GetInstance<ReportParameter4ViewModel>();
            }
        }

        public ReportParameter5ViewModel ReportParameter5ViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ReportParameter5ViewModel>())
                    SimpleIoc.Default.Register<ReportParameter5ViewModel>();

                return ServiceLocator.Current.GetInstance<ReportParameter5ViewModel>();
            }
        }

        public ReportParameter7ViewModel ReportParameter7ViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ReportParameter7ViewModel>())
                    SimpleIoc.Default.Register<ReportParameter7ViewModel>();

                return ServiceLocator.Current.GetInstance<ReportParameter7ViewModel>();
            }
        }

        public ReportParameter8ViewModel ReportParameter8ViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ReportParameter8ViewModel>())
                    SimpleIoc.Default.Register<ReportParameter8ViewModel>();

                return ServiceLocator.Current.GetInstance<ReportParameter8ViewModel>();
            }
        }


        public PatientSummeryReportViewModel PatientSummeryReportViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PatientSummeryReportViewModel>())
                    SimpleIoc.Default.Register<PatientSummeryReportViewModel>();

                return ServiceLocator.Current.GetInstance<PatientSummeryReportViewModel>();
            }
        }

        #endregion


        #region Doctor 

        public DoctorRoomViewModel DoctorRoomViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<DoctorRoomViewModel>())
                    SimpleIoc.Default.Register<DoctorRoomViewModel>();

                return ServiceLocator.Current.GetInstance<DoctorRoomViewModel>();
            }
        }

        public ListSessionViewModel ListSessionViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListSessionViewModel>())
                    SimpleIoc.Default.Register<ListSessionViewModel>();

                return ServiceLocator.Current.GetInstance<ListSessionViewModel>();
            }
        }

        public ManageSessionViewModel ManageSessionViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageSessionViewModel>())
                    SimpleIoc.Default.Register<ManageSessionViewModel>();

                return ServiceLocator.Current.GetInstance<ManageSessionViewModel>();
            }
        }

        public SessionWithdrawnViewModel SessionWithdrawnViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<SessionWithdrawnViewModel>())
                    SimpleIoc.Default.Register<SessionWithdrawnViewModel>();

                return ServiceLocator.Current.GetInstance<SessionWithdrawnViewModel>();
            }
        }

        public PatientListForDoctorViewModel PatientListForDoctorViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PatientListForDoctorViewModel>())
                    SimpleIoc.Default.Register<PatientListForDoctorViewModel>();

                return ServiceLocator.Current.GetInstance<PatientListForDoctorViewModel>();
            }
        }

        #endregion

        #region EMRView

        public SummeryViewViewModel SummeryViewViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<SummeryViewViewModel>())
                    SimpleIoc.Default.Register<SummeryViewViewModel>();

                return ServiceLocator.Current.GetInstance<SummeryViewViewModel>();
            }
        }

        public EMRViewViewModel EMRViewViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EMRViewViewModel>())
                    SimpleIoc.Default.Register<EMRViewViewModel>();

                return ServiceLocator.Current.GetInstance<EMRViewViewModel>();
            }
        }

        public CCHPIViewModel CCHPIViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<CCHPIViewModel>())
                    SimpleIoc.Default.Register<CCHPIViewModel>();

                return ServiceLocator.Current.GetInstance<CCHPIViewModel>();
            }
        }

        public PhysicalExamViewModel PhysicalExamViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PhysicalExamViewModel>())
                    SimpleIoc.Default.Register<PhysicalExamViewModel>();

                return ServiceLocator.Current.GetInstance<PhysicalExamViewModel>();
            }
        }

        public ProgressNoteViewModel ProgressNoteViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ProgressNoteViewModel>())
                    SimpleIoc.Default.Register<ProgressNoteViewModel>();

                return ServiceLocator.Current.GetInstance<ProgressNoteViewModel>();
            }
        }

        public WellnessDataViewModel WellnessDataViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<WellnessDataViewModel>())
                    SimpleIoc.Default.Register<WellnessDataViewModel>();

                return ServiceLocator.Current.GetInstance<WellnessDataViewModel>();
            }
        }

        public PatientDiagnosisViewModel PatientDiagnosisViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PatientDiagnosisViewModel>())
                    SimpleIoc.Default.Register<PatientDiagnosisViewModel>();

                return ServiceLocator.Current.GetInstance<PatientDiagnosisViewModel>();
            }
        }

        public PastMedicalHistoryViewModel PastMedicalHistoryViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PastMedicalHistoryViewModel>())
                    SimpleIoc.Default.Register<PastMedicalHistoryViewModel>();

                return ServiceLocator.Current.GetInstance<PastMedicalHistoryViewModel>();
            }
        }

        public VitalSignsChartViewModel VitalSignsChartViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<VitalSignsChartViewModel>())
                    SimpleIoc.Default.Register<VitalSignsChartViewModel>();

                return ServiceLocator.Current.GetInstance<VitalSignsChartViewModel>();
            }
        }

        #endregion

        #region Inventory

        public PrescriptionViewModel PrescriptionViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PrescriptionViewModel>())
                    SimpleIoc.Default.Register<PrescriptionViewModel>();

                return ServiceLocator.Current.GetInstance<PrescriptionViewModel>();
            }
        }

        public ListItemMasterViewModel ListItemMasterViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListItemMasterViewModel>())
                    SimpleIoc.Default.Register<ListItemMasterViewModel>();

                return ServiceLocator.Current.GetInstance<ListItemMasterViewModel>();
            }
        }

        public ManageItemMasterViewModel ManageItemMasterViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageItemMasterViewModel>())
                    SimpleIoc.Default.Register<ManageItemMasterViewModel>();

                return ServiceLocator.Current.GetInstance<ManageItemMasterViewModel>();
            }
        }

        public ListStoresViewModel ListStoresViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListStoresViewModel>())
                    SimpleIoc.Default.Register<ListStoresViewModel>();

                return ServiceLocator.Current.GetInstance<ListStoresViewModel>();
            }
        }

        public ManageStoreViewModel ManageStoreViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageStoreViewModel>())
                    SimpleIoc.Default.Register<ManageStoreViewModel>();

                return ServiceLocator.Current.GetInstance<ManageStoreViewModel>();
            }
        }
        public ListStoreUOMConversionViewModel ListStoreUOMConversionViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListStoreUOMConversionViewModel>())
                    SimpleIoc.Default.Register<ListStoreUOMConversionViewModel>();

                return ServiceLocator.Current.GetInstance<ListStoreUOMConversionViewModel>();
            }
        }

        public ManageStoreUOMConversionViewModel ManageStoreUOMConversionViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageStoreUOMConversionViewModel>())
                    SimpleIoc.Default.Register<ManageStoreUOMConversionViewModel>();

                return ServiceLocator.Current.GetInstance<ManageStoreUOMConversionViewModel>();
            }
        }

        public AdjustStockViewModel AdjustStockViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<AdjustStockViewModel>())
                    SimpleIoc.Default.Register<AdjustStockViewModel>();

                return ServiceLocator.Current.GetInstance<AdjustStockViewModel>();
            }
        }

        public ListItemIssueViewModel ListItemIssueViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListItemIssueViewModel>())
                    SimpleIoc.Default.Register<ListItemIssueViewModel>();

                return ServiceLocator.Current.GetInstance<ListItemIssueViewModel>();
            }
        }

        public ListItemTransferViewModel ListItemTransferViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListItemTransferViewModel>())
                    SimpleIoc.Default.Register<ListItemTransferViewModel>();

                return ServiceLocator.Current.GetInstance<ListItemTransferViewModel>();
            }
        }

        public ListItemReceiveViewModel ListItemReceiveViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListItemReceiveViewModel>())
                    SimpleIoc.Default.Register<ListItemReceiveViewModel>();

                return ServiceLocator.Current.GetInstance<ListItemReceiveViewModel>();
            }
        }

        public ListItemRequestViewModel ListItemRequestViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListItemRequestViewModel>())
                    SimpleIoc.Default.Register<ListItemRequestViewModel>();

                return ServiceLocator.Current.GetInstance<ListItemRequestViewModel>();
            }
        }

        public ListStockWorkListViewModel ListStockWorkListViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListStockWorkListViewModel>())
                    SimpleIoc.Default.Register<ListStockWorkListViewModel>();

                return ServiceLocator.Current.GetInstance<ListStockWorkListViewModel>();
            }
        }

        public ManageItemIssueViewModel ManageItemIssueViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageItemIssueViewModel>())
                    SimpleIoc.Default.Register<ManageItemIssueViewModel>();

                return ServiceLocator.Current.GetInstance<ManageItemIssueViewModel>();
            }
        }

        public ManageItemTransferViewModel ManageItemTransferViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageItemTransferViewModel>())
                    SimpleIoc.Default.Register<ManageItemTransferViewModel>();

                return ServiceLocator.Current.GetInstance<ManageItemTransferViewModel>();
            }
        }

        public ManageItemReceiveViewModel ManageItemReceiveViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageItemReceiveViewModel>())
                    SimpleIoc.Default.Register<ManageItemReceiveViewModel>();

                return ServiceLocator.Current.GetInstance<ManageItemReceiveViewModel>();
            }
        }

        public ManageItemRequestViewModel ManageItemRequestViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageItemRequestViewModel>())
                    SimpleIoc.Default.Register<ManageItemRequestViewModel>();

                return ServiceLocator.Current.GetInstance<ManageItemRequestViewModel>();
            }
        }

        public StocksViewModel StocksViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<StocksViewModel>())
                    SimpleIoc.Default.Register<StocksViewModel>();

                return ServiceLocator.Current.GetInstance<StocksViewModel>();
            }
        }

        public ConsumptionStockViewModel ConsumptionStockViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ConsumptionStockViewModel>())
                    SimpleIoc.Default.Register<ConsumptionStockViewModel>();

                return ServiceLocator.Current.GetInstance<ConsumptionStockViewModel>();
            }
        }

        public DisposeStockViewModel DisposeStockViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<DisposeStockViewModel>())
                    SimpleIoc.Default.Register<DisposeStockViewModel>();

                return ServiceLocator.Current.GetInstance<DisposeStockViewModel>();
            }
        }

        public CancelPopupViewModel CancelPopupViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<CancelPopupViewModel>())
                    SimpleIoc.Default.Register<CancelPopupViewModel>();

                return ServiceLocator.Current.GetInstance<CancelPopupViewModel>();
            }
        }

        public SearchRequestViewModel SearchRequestViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<SearchRequestViewModel>())
                    SimpleIoc.Default.Register<SearchRequestViewModel>();

                return ServiceLocator.Current.GetInstance<SearchRequestViewModel>();
            }
        }

        public SearchIssueViewModel SearchIssueViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<SearchIssueViewModel>())
                    SimpleIoc.Default.Register<SearchIssueViewModel>();

                return ServiceLocator.Current.GetInstance<SearchIssueViewModel>();
            }
        }

        public DispenseDrugViewModel DispenseDrugViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<DispenseDrugViewModel>())
                    SimpleIoc.Default.Register<DispenseDrugViewModel>();

                return ServiceLocator.Current.GetInstance<DispenseDrugViewModel>();
            }
        }

        public CancelDispenseViewModel CancelDispenseViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<CancelDispenseViewModel>())
                    SimpleIoc.Default.Register<CancelDispenseViewModel>();

                return ServiceLocator.Current.GetInstance<CancelDispenseViewModel>();
            }
        }

        public PrintDrugStickerViewModel PrintDrugStickerViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PrintDrugStickerViewModel>())
                    SimpleIoc.Default.Register<PrintDrugStickerViewModel>();

                return ServiceLocator.Current.GetInstance<PrintDrugStickerViewModel>();
            }
        }

        public IPFillsViewModel IPFillsViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<IPFillsViewModel>())
                    SimpleIoc.Default.Register<IPFillsViewModel>();

                return ServiceLocator.Current.GetInstance<IPFillsViewModel>();
            }
        }

        public NewIPFillsViewModel NewIPFillsViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<NewIPFillsViewModel>())
                    SimpleIoc.Default.Register<NewIPFillsViewModel>();

                return ServiceLocator.Current.GetInstance<NewIPFillsViewModel>();
            }
        }

        public IPFillsDetailViewModel IPFillsDetailViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<IPFillsDetailViewModel>())
                    SimpleIoc.Default.Register<IPFillsDetailViewModel>();

                return ServiceLocator.Current.GetInstance<IPFillsDetailViewModel>();
            }
        }

        public DispenseReturnsViewModel DispenseReturnsViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<DispenseReturnsViewModel>())
                    SimpleIoc.Default.Register<DispenseReturnsViewModel>();

                return ServiceLocator.Current.GetInstance<DispenseReturnsViewModel>();
            }
        }

        public CreateDispenseReturnViewModel createDispenseReturnViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<CreateDispenseReturnViewModel>())
                    SimpleIoc.Default.Register<CreateDispenseReturnViewModel>();

                return ServiceLocator.Current.GetInstance<CreateDispenseReturnViewModel>();
            }
        }

        public ChangeStorePopupViewmodel ChangeStorePopupViewmodel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ChangeStorePopupViewmodel>())
                    SimpleIoc.Default.Register<ChangeStorePopupViewmodel>();

                return ServiceLocator.Current.GetInstance<ChangeStorePopupViewmodel>();
            }
        }

        #endregion

        #region LIS_RIS Setting
        public ListRequestItemViewModel ListRequestItemViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListRequestItemViewModel>())
                    SimpleIoc.Default.Register<ListRequestItemViewModel>();

                return ServiceLocator.Current.GetInstance<ListRequestItemViewModel>();
            }
        }

        public ManageRequestItemViewModel ManageRequestItemViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageRequestItemViewModel>())
                    SimpleIoc.Default.Register<ManageRequestItemViewModel>();

                return ServiceLocator.Current.GetInstance<ManageRequestItemViewModel>();
            }
        }

        public ListTestParameterViewModel ListTestParameterViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListTestParameterViewModel>())
                    SimpleIoc.Default.Register<ListTestParameterViewModel>();

                return ServiceLocator.Current.GetInstance<ListTestParameterViewModel>();
            }
        }


        public ManageTestParameterViewModel ManageParameterViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageTestParameterViewModel>())
                    SimpleIoc.Default.Register<ManageTestParameterViewModel>();

                return ServiceLocator.Current.GetInstance<ManageTestParameterViewModel>();
            }
        }


        public ListSpecimenViewModel ListSpecimenViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListSpecimenViewModel>())
                    SimpleIoc.Default.Register<ListSpecimenViewModel>();

                return ServiceLocator.Current.GetInstance<ListSpecimenViewModel>();
            }
        }

        public ManageSpecimenViewModel ManageSpecimenViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageSpecimenViewModel>())
                    SimpleIoc.Default.Register<ManageSpecimenViewModel>();

                return ServiceLocator.Current.GetInstance<ManageSpecimenViewModel>();
            }
        }

        #endregion

        #region RIS

        public PACSWorkListViewModel PACSWorkListViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PACSWorkListViewModel>())
                    SimpleIoc.Default.Register<PACSWorkListViewModel>();

                return ServiceLocator.Current.GetInstance<PACSWorkListViewModel>();
            }
        }

        public RadiologyExamListViewModel RadiologyExamListViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<RadiologyExamListViewModel>())
                    SimpleIoc.Default.Register<RadiologyExamListViewModel>();

                return ServiceLocator.Current.GetInstance<RadiologyExamListViewModel>();
            }
        }

        public ExecutePopUpViewModel ExecutePopUpViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ExecutePopUpViewModel>())
                    SimpleIoc.Default.Register<ExecutePopUpViewModel>();

                return ServiceLocator.Current.GetInstance<ExecutePopUpViewModel>();
            }
        }

        public ReviewRISResultViewModel ReviewRISResultViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ReviewRISResultViewModel>())
                    SimpleIoc.Default.Register<ReviewRISResultViewModel>();

                return ServiceLocator.Current.GetInstance<ReviewRISResultViewModel>();
            }
        }

        public ReviewHistoryViewModel ReviewHistoryViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ReviewHistoryViewModel>())
                    SimpleIoc.Default.Register<ReviewHistoryViewModel>();

                return ServiceLocator.Current.GetInstance<ReviewHistoryViewModel>();
            }
        }

        public ListRadiologistTemplateViewModel ListRadiologistTemplateViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListRadiologistTemplateViewModel>())
                    SimpleIoc.Default.Register<ListRadiologistTemplateViewModel>();

                return ServiceLocator.Current.GetInstance<ListRadiologistTemplateViewModel>();
            }
        }

        public ManageRadiologistTemplateViewModel ManageRadiologistTemplateViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageRadiologistTemplateViewModel>())
                    SimpleIoc.Default.Register<ManageRadiologistTemplateViewModel>();

                return ServiceLocator.Current.GetInstance<ManageRadiologistTemplateViewModel>();
            }
        }

        public ListDoctorFeesRISViewModel ListDoctorFeesRISViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListDoctorFeesRISViewModel>())
                    SimpleIoc.Default.Register<ListDoctorFeesRISViewModel>();

                return ServiceLocator.Current.GetInstance<ListDoctorFeesRISViewModel>();
            }
        }


        public RDUReviewResultViewModel RDUReviewResultViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<RDUReviewResultViewModel>())
                    SimpleIoc.Default.Register<RDUReviewResultViewModel>();

                return ServiceLocator.Current.GetInstance<RDUReviewResultViewModel>();
            }
        }

        public RDUPositivePopupViewModel RDUPositivePopupViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<RDUPositivePopupViewModel>())
                    SimpleIoc.Default.Register<RDUPositivePopupViewModel>();

                return ServiceLocator.Current.GetInstance<RDUPositivePopupViewModel>();
            }
        }
        public AssignForRDUViewModel AssignForRDUViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<AssignForRDUViewModel>())
                    SimpleIoc.Default.Register<AssignForRDUViewModel>();

                return ServiceLocator.Current.GetInstance<AssignForRDUViewModel>();
            }
        }

        public BurnImageViewModel BurnImageViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<BurnImageViewModel>())
                    SimpleIoc.Default.Register<BurnImageViewModel>();

                return ServiceLocator.Current.GetInstance<BurnImageViewModel>();
            }
        }

        public EditStudyViewModel EditStudyViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EditStudyViewModel>())
                    SimpleIoc.Default.Register<EditStudyViewModel>();

                return ServiceLocator.Current.GetInstance<EditStudyViewModel>();
            }
        }

        public RadiologistReportViewModel RadiologistReportViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<RadiologistReportViewModel>())
                    SimpleIoc.Default.Register<RadiologistReportViewModel>();

                return ServiceLocator.Current.GetInstance<RadiologistReportViewModel>();
            }
        }

        public ScheduleRadiologistViewModel ScheduleRadiologistViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ScheduleRadiologistViewModel>())
                    SimpleIoc.Default.Register<ScheduleRadiologistViewModel>();

                return ServiceLocator.Current.GetInstance<ScheduleRadiologistViewModel>();
            }
        }

        public CheckRISResultViewModel CheckRISResultViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<CheckRISResultViewModel>())
                    SimpleIoc.Default.Register<CheckRISResultViewModel>();

                return ServiceLocator.Current.GetInstance<CheckRISResultViewModel>();
            }
        }
        #endregion

        #region LIS

        public SpecimenCollectionViewModel SpecimenCollectionViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<SpecimenCollectionViewModel>())
                    SimpleIoc.Default.Register<SpecimenCollectionViewModel>();

                return ServiceLocator.Current.GetInstance<SpecimenCollectionViewModel>();
            }
        }

        public SpecimenAcceptViewModel SpecimenAcceptViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<SpecimenAcceptViewModel>())
                    SimpleIoc.Default.Register<SpecimenAcceptViewModel>();

                return ServiceLocator.Current.GetInstance<SpecimenAcceptViewModel>();
            }
        }

        public LabOrderListViewModel LabOrderListViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<LabOrderListViewModel>())
                    SimpleIoc.Default.Register<LabOrderListViewModel>();

                return ServiceLocator.Current.GetInstance<LabOrderListViewModel>();
            }
        }

        public EnterResultsLabViewModel EnterResultsLabViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EnterResultsLabViewModel>())
                    SimpleIoc.Default.Register<EnterResultsLabViewModel>();

                return ServiceLocator.Current.GetInstance<EnterResultsLabViewModel>();
            }
        }

        public ImportLabResultViewModel ImportLabResultViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ImportLabResultViewModel>())
                    SimpleIoc.Default.Register<ImportLabResultViewModel>();

                return ServiceLocator.Current.GetInstance<ImportLabResultViewModel>();
            }
        }

        #endregion

        #region MRD

        public TranslateXrayViewModel TranslateXrayViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<TranslateXrayViewModel>())
                    SimpleIoc.Default.Register<TranslateXrayViewModel>();

                return ServiceLocator.Current.GetInstance<TranslateXrayViewModel>();
            }
        }

        public MappingTranslateXrayViewModel MappingTranslateXrayViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<MappingTranslateXrayViewModel>())
                    SimpleIoc.Default.Register<MappingTranslateXrayViewModel>();

                return ServiceLocator.Current.GetInstance<MappingTranslateXrayViewModel>();
            }
        }

        public ReportResultMassViewModel ReportResultMassViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ReportResultMassViewModel>())
                    SimpleIoc.Default.Register<ReportResultMassViewModel>();

                return ServiceLocator.Current.GetInstance<ReportResultMassViewModel>();
            }
        }

        public RegistrationBulkImportViewModel RegistrationBulkImportViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<RegistrationBulkImportViewModel>())
                    SimpleIoc.Default.Register<RegistrationBulkImportViewModel>();

                return ServiceLocator.Current.GetInstance<RegistrationBulkImportViewModel>();
            }
        }

        public PatientVisitMassViewModel PatientVisitMassViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PatientVisitMassViewModel>())
                    SimpleIoc.Default.Register<PatientVisitMassViewModel>();

                return ServiceLocator.Current.GetInstance<PatientVisitMassViewModel>();
            }
        }

        public LabResultMassViewModel CheckupReportViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<LabResultMassViewModel>())
                    SimpleIoc.Default.Register<LabResultMassViewModel>();

                return ServiceLocator.Current.GetInstance<LabResultMassViewModel>();
            }
        }

        public BulkAlertDialogViewModel BulkAlertDialogViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<BulkAlertDialogViewModel>())
                    SimpleIoc.Default.Register<BulkAlertDialogViewModel>();

                return ServiceLocator.Current.GetInstance<BulkAlertDialogViewModel>();
            }
        }

        public EcountImportFileViewModel EcountImportFileViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EcountImportFileViewModel>())
                    SimpleIoc.Default.Register<EcountImportFileViewModel>();

                return ServiceLocator.Current.GetInstance<EcountImportFileViewModel>();
            }
        }



        public EcountTranferViewModel EcountTranferViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EcountTranferViewModel>())
                    SimpleIoc.Default.Register<EcountTranferViewModel>();

                return ServiceLocator.Current.GetInstance<EcountTranferViewModel>();
            }
        }

        public EcountItemIssueViewModel EcountItemIssueViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EcountItemIssueViewModel>())
                    SimpleIoc.Default.Register<EcountItemIssueViewModel>();

                return ServiceLocator.Current.GetInstance<EcountItemIssueViewModel>();
            }
        }



        #endregion

        #region Checkup

        public HealthExamListViewModel HealthExamListViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<HealthExamListViewModel>())
                    SimpleIoc.Default.Register<HealthExamListViewModel>();

                return ServiceLocator.Current.GetInstance<HealthExamListViewModel>();
            }
        }

        public EnterPhysicalExamViewModel EnterPhysicalExamViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EnterPhysicalExamViewModel>())
                    SimpleIoc.Default.Register<EnterPhysicalExamViewModel>();

                return ServiceLocator.Current.GetInstance<EnterPhysicalExamViewModel>();
            }
        }

        public EnterMuscleTestViewModel EnterMuscleTestViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EnterMuscleTestViewModel>())
                    SimpleIoc.Default.Register<EnterMuscleTestViewModel>();

                return ServiceLocator.Current.GetInstance<EnterMuscleTestViewModel>();
            }
        }

        public EnterFitnessTestResultViewModel EnterFitnessTestResultViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EnterFitnessTestResultViewModel>())
                    SimpleIoc.Default.Register<EnterFitnessTestResultViewModel>();

                return ServiceLocator.Current.GetInstance<EnterFitnessTestResultViewModel>();
            }
        }

        public EnterAudiogramResultViewModel EnterAudiogramResultViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EnterAudiogramResultViewModel>())
                    SimpleIoc.Default.Register<EnterAudiogramResultViewModel>();

                return ServiceLocator.Current.GetInstance<EnterAudiogramResultViewModel>();
            }
        }

        public EnterPulmonaryResultViewModel EnterPulmonaryResultViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EnterPulmonaryResultViewModel>())
                    SimpleIoc.Default.Register<EnterPulmonaryResultViewModel>();

                return ServiceLocator.Current.GetInstance<EnterPulmonaryResultViewModel>();
            }
        }

        public EnterEKGResultViewModel EnterEKGResultViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EnterEKGResultViewModel>())
                    SimpleIoc.Default.Register<EnterEKGResultViewModel>();

                return ServiceLocator.Current.GetInstance<EnterEKGResultViewModel>();
            }
        }

        public EnterOccuVisionTestResultViewModel EnterOccuVisionTestResultViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EnterOccuVisionTestResultViewModel>())
                    SimpleIoc.Default.Register<EnterOccuVisionTestResultViewModel>();

                return ServiceLocator.Current.GetInstance<EnterOccuVisionTestResultViewModel>();
            }
        }

        public EnterCheckupTestResultViewModel EnterCheckupTestResultViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EnterCheckupTestResultViewModel>())
                    SimpleIoc.Default.Register<EnterCheckupTestResultViewModel>();

                return ServiceLocator.Current.GetInstance<EnterCheckupTestResultViewModel>();
            }
        }

        public ListCheckupJobViewModel ListCheckupJobViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListCheckupJobViewModel>())
                    SimpleIoc.Default.Register<ListCheckupJobViewModel>();

                return ServiceLocator.Current.GetInstance<ListCheckupJobViewModel>();
            }
        }

        public ManageCheckupJobViewModel ManageCheckupJobViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageCheckupJobViewModel>())
                    SimpleIoc.Default.Register<ManageCheckupJobViewModel>();

                return ServiceLocator.Current.GetInstance<ManageCheckupJobViewModel>();
            }
        }

        public CheckupRuleViewModel CheckupRuleViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<CheckupRuleViewModel>())
                    SimpleIoc.Default.Register<CheckupRuleViewModel>();

                return ServiceLocator.Current.GetInstance<CheckupRuleViewModel>();
            }
        }

        public CheckupSummaryViewModel CheckupSummaryViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<CheckupSummaryViewModel>())
                    SimpleIoc.Default.Register<CheckupSummaryViewModel>();

                return ServiceLocator.Current.GetInstance<CheckupSummaryViewModel>();
            }
        }

        public VerifyChekupResultViewModel VerifyChekupResultViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<VerifyChekupResultViewModel>())
                    SimpleIoc.Default.Register<VerifyChekupResultViewModel>();

                return ServiceLocator.Current.GetInstance<VerifyChekupResultViewModel>();
            }
        }

        public TranslateCheckupResultViewModel TranslateCheckupResultViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<TranslateCheckupResultViewModel>())
                    SimpleIoc.Default.Register<TranslateCheckupResultViewModel>();

                return ServiceLocator.Current.GetInstance<TranslateCheckupResultViewModel>();
            }
        }

        public ImportOccMedResultViewModel ImportOccMedResultViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ImportOccMedResultViewModel>())
                    SimpleIoc.Default.Register<ImportOccMedResultViewModel>();

                return ServiceLocator.Current.GetInstance<ImportOccMedResultViewModel>();
            }
        }

        public CheckupListReportViewModel CheckupListReportViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<CheckupListReportViewModel>())
                    SimpleIoc.Default.Register<CheckupListReportViewModel>();

                return ServiceLocator.Current.GetInstance<CheckupListReportViewModel>();
            }
        }

        public ImportOldResultViewModel ImportOldResultViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ImportOldResultViewModel>())
                    SimpleIoc.Default.Register<ImportOldResultViewModel>();

                return ServiceLocator.Current.GetInstance<ImportOldResultViewModel>();
            }
        }

        public EnterPapSmearViewModel EnterPapSmearViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EnterPapSmearViewModel>())
                    SimpleIoc.Default.Register<EnterPapSmearViewModel>();

                return ServiceLocator.Current.GetInstance<EnterPapSmearViewModel>();
            }
        }

        #endregion

        #region OrderProcessing

        public OrderWithOutStockItemViewModel OrderLabItemViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<OrderWithOutStockItemViewModel>())
                    SimpleIoc.Default.Register<OrderWithOutStockItemViewModel>();

                return ServiceLocator.Current.GetInstance<OrderWithOutStockItemViewModel>();
            }
        }

        public OrderDrugItemViewModel OrderDrugItemViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<OrderDrugItemViewModel>())
                    SimpleIoc.Default.Register<OrderDrugItemViewModel>();

                return ServiceLocator.Current.GetInstance<OrderDrugItemViewModel>();
            }
        }
        public OrderMedicalItemViewModel OrderMedicalItemViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<OrderMedicalItemViewModel>())
                    SimpleIoc.Default.Register<OrderMedicalItemViewModel>();

                return ServiceLocator.Current.GetInstance<OrderMedicalItemViewModel>();
            }
        }

        public PatientOrderEntryViewModel PatientOrderEntryViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PatientOrderEntryViewModel>())
                    SimpleIoc.Default.Register<PatientOrderEntryViewModel>();

                return ServiceLocator.Current.GetInstance<PatientOrderEntryViewModel>();
            }
        }

        public CancelOrderViewModel CancelOrderViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<CancelOrderViewModel>())
                    SimpleIoc.Default.Register<CancelOrderViewModel>();

                return ServiceLocator.Current.GetInstance<CancelOrderViewModel>();
            }
        }


        public OrderGroupReceiptViewModel OrderGroupReceiptViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<OrderGroupReceiptViewModel>())
                    SimpleIoc.Default.Register<OrderGroupReceiptViewModel>();

                return ServiceLocator.Current.GetInstance<OrderGroupReceiptViewModel>();
            }
        }

        public OrderOtherTypeViewModel OrderOtherTypeViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<OrderOtherTypeViewModel>())
                    SimpleIoc.Default.Register<OrderOtherTypeViewModel>();

                return ServiceLocator.Current.GetInstance<OrderOtherTypeViewModel>();
            }
        }

        public CloseOrderPopUpViewModel CloseOrderPopUpViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<CloseOrderPopUpViewModel>())
                    SimpleIoc.Default.Register<CloseOrderPopUpViewModel>();

                return ServiceLocator.Current.GetInstance<CloseOrderPopUpViewModel>();
            }
        }

        public AdjustOrderDetailForPackageViewModel AdjustOrderDetailForPackageViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<AdjustOrderDetailForPackageViewModel>())
                    SimpleIoc.Default.Register<AdjustOrderDetailForPackageViewModel>();

                return ServiceLocator.Current.GetInstance<AdjustOrderDetailForPackageViewModel>();
            }
        }

        #endregion

        #region Patient

        public SearchPatientViewModel SearchPatientViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<SearchPatientViewModel>())
                    SimpleIoc.Default.Register<SearchPatientViewModel>();

                return ServiceLocator.Current.GetInstance<SearchPatientViewModel>();
            }
        }

        public RegisterPatientViewModel RegisterPatientViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<RegisterPatientViewModel>())
                    SimpleIoc.Default.Register<RegisterPatientViewModel>();

                return ServiceLocator.Current.GetInstance<RegisterPatientViewModel>();
            }
        }

        public ManagePatientViewModel ManagePatientViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManagePatientViewModel>())
                    SimpleIoc.Default.Register<ManagePatientViewModel>();

                return ServiceLocator.Current.GetInstance<ManagePatientViewModel>();
            }
        }

        public PatientListViewModel PatientListViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PatientListViewModel>())
                    SimpleIoc.Default.Register<PatientListViewModel>();

                return ServiceLocator.Current.GetInstance<PatientListViewModel>();
            }
        }
        public PatientVitalSignViewModel PatientVitalSignModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PatientVitalSignViewModel>())
                    SimpleIoc.Default.Register<PatientVitalSignViewModel>();

                return ServiceLocator.Current.GetInstance<PatientVitalSignViewModel>();
            }
        }

        public PatientAllergyViewModel PatientAllergyViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PatientAllergyViewModel>())
                    SimpleIoc.Default.Register<PatientAllergyViewModel>();

                return ServiceLocator.Current.GetInstance<PatientAllergyViewModel>();
            }
        }

        public ManageAppointmentViewModel ManageAppointmentViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageAppointmentViewModel>())
                    SimpleIoc.Default.Register<ManageAppointmentViewModel>();

                return ServiceLocator.Current.GetInstance<ManageAppointmentViewModel>();
            }
        }

        public ListAppointmentViewModel ListAppointmentViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListAppointmentViewModel>())
                    SimpleIoc.Default.Register<ListAppointmentViewModel>();

                return ServiceLocator.Current.GetInstance<ListAppointmentViewModel>();
            }
        }


        public CreateVisitViewModel CreateVisitViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<CreateVisitViewModel>())
                    SimpleIoc.Default.Register<CreateVisitViewModel>();

                return ServiceLocator.Current.GetInstance<CreateVisitViewModel>();
            }
        }

        public ModifyVisitPayorViewModel ModifyVisitPayorViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ModifyVisitPayorViewModel>())
                    SimpleIoc.Default.Register<ModifyVisitPayorViewModel>();

                return ServiceLocator.Current.GetInstance<ModifyVisitPayorViewModel>();
            }
        }

        public ModifyVisitViewModel ModifyVisitViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ModifyVisitViewModel>())
                    SimpleIoc.Default.Register<ModifyVisitViewModel>();

                return ServiceLocator.Current.GetInstance<ModifyVisitViewModel>();
            }
        }

        public ScannedDocumentViewModel ScannedDocumentViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ScannedDocumentViewModel>())
                    SimpleIoc.Default.Register<ScannedDocumentViewModel>();

                return ServiceLocator.Current.GetInstance<ScannedDocumentViewModel>();
            }
        }

        public ListPatMergeViewModel ListPatMergeViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListPatMergeViewModel>())
                    SimpleIoc.Default.Register<ListPatMergeViewModel>();

                return ServiceLocator.Current.GetInstance<ListPatMergeViewModel>();
            }
        }

        public ManageEncounterMergeViewModel ManageEncounterMergeViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageEncounterMergeViewModel>())
                    SimpleIoc.Default.Register<ManageEncounterMergeViewModel>();

                return ServiceLocator.Current.GetInstance<ManageEncounterMergeViewModel>();
            }
        }

        public ManagePatientMergeViewModel ManagePatientMergeViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManagePatientMergeViewModel>())
                    SimpleIoc.Default.Register<ManagePatientMergeViewModel>();

                return ServiceLocator.Current.GetInstance<ManagePatientMergeViewModel>();
            }
        }

        public PatientTrackingViewModel PatientTrackingViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PatientTrackingViewModel>())
                    SimpleIoc.Default.Register<PatientTrackingViewModel>();

                return ServiceLocator.Current.GetInstance<PatientTrackingViewModel>();
            }
        }

        public ConsultRequestViewModel ConsultRequestViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ConsultRequestViewModel>())
                    SimpleIoc.Default.Register<ConsultRequestViewModel>();

                return ServiceLocator.Current.GetInstance<ConsultRequestViewModel>();
            }
        }

        public SendConsultViewModel SendConsultViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<SendConsultViewModel>())
                    SimpleIoc.Default.Register<SendConsultViewModel>();

                return ServiceLocator.Current.GetInstance<SendConsultViewModel>();
            }
        }

        public PatientArrivedViewModel PatientArrivedViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PatientArrivedViewModel>())
                    SimpleIoc.Default.Register<PatientArrivedViewModel>();

                return ServiceLocator.Current.GetInstance<PatientArrivedViewModel>();
            }
        }

        public ChangeLocationViewModel ChangeLocationViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ChangeLocationViewModel>())
                    SimpleIoc.Default.Register<ChangeLocationViewModel>();

                return ServiceLocator.Current.GetInstance<ChangeLocationViewModel>();
            }
        }

        public PatientAlertViewModel PatientAlertViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PatientAlertViewModel>())
                    SimpleIoc.Default.Register<PatientAlertViewModel>();

                return ServiceLocator.Current.GetInstance<PatientAlertViewModel>();
            }
        }

        public PatientAlertPopupViewModel PatientAlertPopupViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PatientAlertPopupViewModel>())
                    SimpleIoc.Default.Register<PatientAlertPopupViewModel>();

                return ServiceLocator.Current.GetInstance<PatientAlertPopupViewModel>();
            }
        }


        #endregion

        #region Phamacy

        public ListDrugGenericViewModel ListDrugGenericViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListDrugGenericViewModel>())
                    SimpleIoc.Default.Register<ListDrugGenericViewModel>();

                return ServiceLocator.Current.GetInstance<ListDrugGenericViewModel>();
            }
        }

        public ManageDrugGenericViewModel ManageDrugGenericViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageDrugGenericViewModel>())
                    SimpleIoc.Default.Register<ManageDrugGenericViewModel>();

                return ServiceLocator.Current.GetInstance<ManageDrugGenericViewModel>();
            }
        }

        public ListDrugFrequencyViewModel ListDrugFrequencyViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListDrugFrequencyViewModel>())
                    SimpleIoc.Default.Register<ListDrugFrequencyViewModel>();

                return ServiceLocator.Current.GetInstance<ListDrugFrequencyViewModel>();
            }
        }

        public ManageDrugFrequencyViewModel ManageDrugFrequencyViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageDrugFrequencyViewModel>())
                    SimpleIoc.Default.Register<ManageDrugFrequencyViewModel>();

                return ServiceLocator.Current.GetInstance<ManageDrugFrequencyViewModel>();
            }
        }

        public ListBilledItemSaleViewModel ListBilledItemSaleViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListBilledItemSaleViewModel>())
                    SimpleIoc.Default.Register<ListBilledItemSaleViewModel>();

                return ServiceLocator.Current.GetInstance<ListBilledItemSaleViewModel>();
            }
        }

        public ManageBilledItemSaleViewModel ManageBilledItemSaleViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageBilledItemSaleViewModel>())
                    SimpleIoc.Default.Register<ManageBilledItemSaleViewModel>();

                return ServiceLocator.Current.GetInstance<ManageBilledItemSaleViewModel>();
            }
        }


        #endregion

        #region Purchase

        public ListVendorViewModel ListVendorViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListVendorViewModel>())
                    SimpleIoc.Default.Register<ListVendorViewModel>();

                return ServiceLocator.Current.GetInstance<ListVendorViewModel>();
            }
        }

        public ManageVendorDetailViewModel ManageVendorDetailViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageVendorDetailViewModel>())
                    SimpleIoc.Default.Register<ManageVendorDetailViewModel>();

                return ServiceLocator.Current.GetInstance<ManageVendorDetailViewModel>();
            }
        }

        public ListPurchaseOrderViewModel ListPurchaseOrderViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListPurchaseOrderViewModel>())
                    SimpleIoc.Default.Register<ListPurchaseOrderViewModel>();

                return ServiceLocator.Current.GetInstance<ListPurchaseOrderViewModel>();
            }
        }

        public ManagePurchaseOrderViewModel ManagePurchaseOrderViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManagePurchaseOrderViewModel>())
                    SimpleIoc.Default.Register<ManagePurchaseOrderViewModel>();

                return ServiceLocator.Current.GetInstance<ManagePurchaseOrderViewModel>();
            }
        }

        public ListGRNViewModel ListGRNViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListGRNViewModel>())
                    SimpleIoc.Default.Register<ListGRNViewModel>();

                return ServiceLocator.Current.GetInstance<ListGRNViewModel>();
            }
        }

        public ManageGRNViewModel ManageGRNViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageGRNViewModel>())
                    SimpleIoc.Default.Register<ManageGRNViewModel>();

                return ServiceLocator.Current.GetInstance<ManageGRNViewModel>();
            }
        }

        public ApprovePOViewModel ApprovePOViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ApprovePOViewModel>())
                    SimpleIoc.Default.Register<ApprovePOViewModel>();

                return ServiceLocator.Current.GetInstance<ApprovePOViewModel>();
            }
        }

        public ManageReceiptViewModel ManageReceiptViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageReceiptViewModel>())
                    SimpleIoc.Default.Register<ManageReceiptViewModel>();

                return ServiceLocator.Current.GetInstance<ManageReceiptViewModel>();
            }
        }


        public ListGroupReceiptViewModel ListGroupReceiptViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListGroupReceiptViewModel>())
                    SimpleIoc.Default.Register<ListGroupReceiptViewModel>();

                return ServiceLocator.Current.GetInstance<ListGroupReceiptViewModel>();
            }
        }

        public SearchPatientBillViewModel SearchPatientBillViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<SearchPatientBillViewModel>())
                    SimpleIoc.Default.Register<SearchPatientBillViewModel>();

                return ServiceLocator.Current.GetInstance<SearchPatientBillViewModel>();
            }
        }

        //public ListBilledItemSaleViewModel ListBilledItemSaleViewModel
        //{
        //    get
        //    {
        //        if (!SimpleIoc.Default.ContainsCreated<ListBilledItemSaleViewModel>())
        //            SimpleIoc.Default.Register<ListBilledItemSaleViewModel>();

        //        return ServiceLocator.Current.GetInstance<ListBilledItemSaleViewModel>();
        //    }
        //}

        //public ManageSaleViewModel ManageSaleViewModel
        //{
        //    get
        //    {
        //        if (!SimpleIoc.Default.ContainsCreated<ManageSaleViewModel>())
        //            SimpleIoc.Default.Register<ManageSaleViewModel>();

        //        return ServiceLocator.Current.GetInstance<ManageSaleViewModel>();
        //    }
        //}

        #endregion

        #region Setting

        public ListReferenceDomainViewModel ListReferenceDomainViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListReferenceDomainViewModel>())
                    SimpleIoc.Default.Register<ListReferenceDomainViewModel>();

                return ServiceLocator.Current.GetInstance<ListReferenceDomainViewModel>();
            }
        }

        public ManageReferenceDomainViewModel ManageReferenceDomainViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageReferenceDomainViewModel>())
                    SimpleIoc.Default.Register<ManageReferenceDomainViewModel>();

                return ServiceLocator.Current.GetInstance<ManageReferenceDomainViewModel>();
            }
        }


        public ListUsersViewModel ListUsersViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListUsersViewModel>())
                    SimpleIoc.Default.Register<ListUsersViewModel>();

                return ServiceLocator.Current.GetInstance<ListUsersViewModel>();
            }
        }

        public ManageUserViewModel ManageUserViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageUserViewModel>())
                    SimpleIoc.Default.Register<ManageUserViewModel>();

                return ServiceLocator.Current.GetInstance<ManageUserViewModel>();
            }
        }

        public ListRolesViewModel ListRolesViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListRolesViewModel>())
                    SimpleIoc.Default.Register<ListRolesViewModel>();

                return ServiceLocator.Current.GetInstance<ListRolesViewModel>();
            }
        }

        public ManageRoleViewModel ManageRoleViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageRoleViewModel>())
                    SimpleIoc.Default.Register<ManageRoleViewModel>();

                return ServiceLocator.Current.GetInstance<ManageRoleViewModel>();
            }
        }

        public ListOrganisationViewModel ListOrganisationViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListOrganisationViewModel>())
                    SimpleIoc.Default.Register<ListOrganisationViewModel>();

                return ServiceLocator.Current.GetInstance<ListOrganisationViewModel>();
            }
        }

        public ManageOrganisationViewModel ManageOrganisationViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageOrganisationViewModel>())
                    SimpleIoc.Default.Register<ManageOrganisationViewModel>();

                return ServiceLocator.Current.GetInstance<ManageOrganisationViewModel>();
            }
        }

        public ManageUserOrganisationViewModel ManageUserOrganisationViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageUserOrganisationViewModel>())
                    SimpleIoc.Default.Register<ManageUserOrganisationViewModel>();

                return ServiceLocator.Current.GetInstance<ManageUserOrganisationViewModel>();
            }
        }

        public ListLocationViewModel ListLocationViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ListLocationViewModel>())
                    SimpleIoc.Default.Register<ListLocationViewModel>();

                return ServiceLocator.Current.GetInstance<ListLocationViewModel>();
            }
        }


        public ManageLocationViewModel ManageLocationViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageLocationViewModel>())
                    SimpleIoc.Default.Register<ManageLocationViewModel>();

                return ServiceLocator.Current.GetInstance<ManageLocationViewModel>();
            }
        }

        public ManageUserLocationViewModel ManageUserLocationViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ManageUserLocationViewModel>())
                    SimpleIoc.Default.Register<ManageUserLocationViewModel>();

                return ServiceLocator.Current.GetInstance<ManageUserLocationViewModel>();
            }
        }

        #endregion

        #region Emergency
        public EmergencyRegisterViewModel EmergencyRegisterViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EmergencyRegisterViewModel>())
                    SimpleIoc.Default.Register<EmergencyRegisterViewModel>();

                return ServiceLocator.Current.GetInstance<EmergencyRegisterViewModel>();
            }
        }

        public EmergencyBedStatusViewModel EmergencyBedStatusViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EmergencyBedStatusViewModel>())
                    SimpleIoc.Default.Register<EmergencyBedStatusViewModel>();

                return ServiceLocator.Current.GetInstance<EmergencyBedStatusViewModel>();
            }
        }

        public EmergencyListViewModel EmergencyListViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EmergencyListViewModel>())
                    SimpleIoc.Default.Register<EmergencyListViewModel>();

                return ServiceLocator.Current.GetInstance<EmergencyListViewModel>();
            }
        }

        public AECheckoutViewModel AECheckoutViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<AECheckoutViewModel>())
                    SimpleIoc.Default.Register<AECheckoutViewModel>();

                return ServiceLocator.Current.GetInstance<AECheckoutViewModel>();
            }
        }
        #endregion

        #region IPD

        public IPDSearchViewModel IPDSearchViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<IPDSearchViewModel>())
                    SimpleIoc.Default.Register<IPDSearchViewModel>();

                return ServiceLocator.Current.GetInstance<IPDSearchViewModel>();
            }
        }

        public BedChangedStatusViewModel BedChangedStatusViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<BedChangedStatusViewModel>())
                    SimpleIoc.Default.Register<BedChangedStatusViewModel>();

                return ServiceLocator.Current.GetInstance<BedChangedStatusViewModel>();
            }
        }

        public IPDMedicalDischargeViewModel IPDMedicalDischargeViewModel
        {
           
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<IPDMedicalDischargeViewModel>())
                    SimpleIoc.Default.Register<IPDMedicalDischargeViewModel>();

                return ServiceLocator.Current.GetInstance<IPDMedicalDischargeViewModel>();
            }
        }


        public AdmissionDetailViewModel AdmissionDetailViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<AdmissionDetailViewModel>())
                    SimpleIoc.Default.Register<AdmissionDetailViewModel>();

                return ServiceLocator.Current.GetInstance<AdmissionDetailViewModel>();
            }
        }


        public AdmissionRequestListViewModel AdmissionRequestListViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<AdmissionRequestListViewModel>())
                    SimpleIoc.Default.Register<AdmissionRequestListViewModel>();

                return ServiceLocator.Current.GetInstance<AdmissionRequestListViewModel>();
            }
        }

        public WardViewModel WardViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<WardViewModel>())
                    SimpleIoc.Default.Register<WardViewModel>();

                return ServiceLocator.Current.GetInstance<WardViewModel>();
            }
        }

        public AdmissionRequestViewModel AdmissionRequestViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<AdmissionRequestViewModel>())
                    SimpleIoc.Default.Register<AdmissionRequestViewModel>();

                return ServiceLocator.Current.GetInstance<AdmissionRequestViewModel>();
            }
        }

        public TranferBedViewModel TranferBedViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<TranferBedViewModel>())
                    SimpleIoc.Default.Register<TranferBedViewModel>();

                return ServiceLocator.Current.GetInstance<TranferBedViewModel>();
            }
        }

        public CancelDischargeViewModel CancelDischargeViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<CancelDischargeViewModel>())
                    SimpleIoc.Default.Register<CancelDischargeViewModel>();

                return ServiceLocator.Current.GetInstance<CancelDischargeViewModel>();
            }
        }

        public BedBookingViewModel BedBookingViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<BedBookingViewModel>())
                    SimpleIoc.Default.Register<BedBookingViewModel>();

                return ServiceLocator.Current.GetInstance<BedBookingViewModel>();
            }
        }
        public TranferBedListViewModel TranferBedListViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<TranferBedListViewModel>())
                    SimpleIoc.Default.Register<TranferBedListViewModel>();

                return ServiceLocator.Current.GetInstance<TranferBedListViewModel>();
            }
        }
        public EditExpDischargeDateViewModel EditExpDischargeDateViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<EditExpDischargeDateViewModel>())
                    SimpleIoc.Default.Register<EditExpDischargeDateViewModel>();

                return ServiceLocator.Current.GetInstance<EditExpDischargeDateViewModel>();
            }
        }


        public IPDConsultViewModel IPDConsultViewModel
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<IPDConsultViewModel>())
                    SimpleIoc.Default.Register<IPDConsultViewModel>();
                return ServiceLocator.Current.GetInstance<IPDConsultViewModel>();
            }
        }

        #endregion

        public static void Cleanup()
        {
            //TODO Clear the ViewMode
            SimpleIoc.Default.Reset();
            
        }
    }
}