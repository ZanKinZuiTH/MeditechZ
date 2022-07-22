using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.DataService
{
    public class MediTechDataService
    {
        private BillingService _Billing;
        public BillingService Billing
        {
            get { return _Billing ?? (_Billing = new BillingService()); }
        }


        private IcheckupService _Icheckup;
        public IcheckupService Icheckup
        {
            get { return _Icheckup ?? (_Icheckup = new IcheckupService()); }
        }



        private InventoryService _Inventory;
        public InventoryService Inventory
        {
            get { return _Inventory ?? (_Inventory = new InventoryService()); }
        }

        private MainManageService _MainManage;
        public MainManageService MainManage
        {
            get { return _MainManage ?? (_MainManage = new MainManageService()); }
        }

        private MasterDataService _MasterData;
        public MasterDataService MasterData
        {
            get { return _MasterData ?? (_MasterData = new MasterDataService()); }
        }

        private OrderProcessingService _OrderProcessing;
        public OrderProcessingService OrderProcessing
        {
            get { return _OrderProcessing ?? (_OrderProcessing = new OrderProcessingService()); }
        }

        private PatientDiagnosticsService _PatientDiagnosis;
        public PatientDiagnosticsService PatientDiagnosis
        {
            get { return _PatientDiagnosis ?? (_PatientDiagnosis = new PatientDiagnosticsService()); }
        }

        private PatientHistoryService _PatientHistory;
        public PatientHistoryService PatientHistory
        {
            get { return _PatientHistory ?? (_PatientHistory = new PatientHistoryService()); }
        }

        private PatientIdentityService _PatientIdentity;
        public PatientIdentityService PatientIdentity
        {
            get { return _PatientIdentity ?? (_PatientIdentity = new PatientIdentityService()); }
        }

        private PharmacyService _Pharmacy;
        public PharmacyService Pharmacy
        {
            get { return _Pharmacy ?? (_Pharmacy = new PharmacyService()); }
        }

        private PurchaseingService _Purchaseing;
        public PurchaseingService Purchaseing
        {
            get { return _Purchaseing?? (_Purchaseing = new PurchaseingService()); }
        }

        private RadiologyService _Radiology;
        public RadiologyService Radiology
        {
            get { return _Radiology ?? (_Radiology = new RadiologyService()); }
        }

        private LabDataService _Lab;
        public LabDataService Lab
        {
            get { return _Lab ?? (_Lab = new LabDataService()); }
        }

        private ReportsService _Reports;
        public ReportsService Reports
        {
            get { return _Reports ?? (_Reports = new ReportsService()); }
        }

        private RoleManageService _RoleManage;
        public RoleManageService RoleManage
        {
            get { return _RoleManage ?? (_RoleManage = new RoleManageService()); }
        }

        private TechnicalService _Technical;
        public TechnicalService Technical
        {
            get { return _Technical ?? (_Technical = new TechnicalService()); }
        }

        private UserManageService _UserManage;
        public UserManageService UserManage
        {
            get { return _UserManage ?? (_UserManage = new UserManageService()); }
        }

        private CheckupService _Checkup;
        public CheckupService Checkup
        {
            get { return _Checkup ?? (_Checkup = new CheckupService()); }
        }

        private IPDService _IPDService;
        public IPDService IPDService
        {
            get { return _IPDService ?? (_IPDService = new IPDService()); }
        }



        private InPatientService _InPatientService;
        public InPatientService InPatientService
        {
            get { return _InPatientService ?? (_InPatientService = new InPatientService()); }
        }


        private PACSService _PACS;
        public PACSService PACS
        {
            get { return _PACS ?? (_PACS = new PACSService()); }
        }

    }
}
