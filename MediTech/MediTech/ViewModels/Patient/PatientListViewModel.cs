using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace MediTech.ViewModels
{
    public class PatientListViewModel : MediTechViewModelBase
    {

        #region Properties

        public string LN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool IsEditDate { get; set; }

        public ObservableCollection<LookupReferenceValueModel> EncounterType { get; set; }

        private List<object> _SelectEncounterType;
        public List<object> SelectEncounterType
        {
            get { return _SelectEncounterType ?? (_SelectEncounterType = new List<object>()); }
            set { Set(ref _SelectEncounterType, value); }
        }

        private List<InsuranceCompanyModel> _InsuranceCompany;

        public List<InsuranceCompanyModel> InsuranceCompany
        {
            get { return _InsuranceCompany; }
            set { Set(ref _InsuranceCompany, value); }
        }

        private InsuranceCompanyModel _SelectInsuranceCompany;

        public InsuranceCompanyModel SelectInsuranceCompany
        {
            get { return _SelectInsuranceCompany; }
            set { Set(ref _SelectInsuranceCompany, value); }
        }

        private ObservableCollection<PatientVisitModel> _PatientVisits;

        public ObservableCollection<PatientVisitModel> PatientVisits
        {
            get { return _PatientVisits; }
            set { Set(ref _PatientVisits, value); }
        }

        private PatientVisitModel _SelectPatientVisit;

        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set { Set(ref _SelectPatientVisit, value); }
        }

        public List<CareproviderModel> Doctors { get; set; }
        private CareproviderModel _SelectDoctor;

        public CareproviderModel SelectDoctor
        {
            get { return _SelectDoctor; }
            set { _SelectDoctor = value; }
        }

        public ObservableCollection<LookupReferenceValueModel> VisitStatus { get; set; }

        private List<object> _SelectVisitStatusList;

        public List<object> SelectVisitStatusList
        {
            get { return _SelectVisitStatusList ?? (_SelectVisitStatusList = new List<object>()); }
            set { Set(ref _SelectVisitStatusList, value); }
        }




        private DateTime? _DateFrom;

        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set
            {
                Set(ref _DateFrom, value);
                if (IsEditDate)
                {
                    SelectDateType = null;
                }

            }
        }


        private DateTime? _DateTo;

        public DateTime? DateTo
        {
            get { return _DateTo; }
            set
            {
                Set(ref _DateTo, value);
                if (IsEditDate)
                {
                    SelectDateType = null;
                }
            }
        }
        public List<LookupItemModel> DateTypes { get; set; }
        private LookupItemModel _SelectDateType;

        public LookupItemModel SelectDateType
        {
            get { return _SelectDateType; }
            set
            {
                Set(ref _SelectDateType, value);

                if (SelectDateType != null)
                {
                    if (SelectDateType.Key == 1)
                    {
                        IsEditDate = false;
                        DateFrom = DateTime.Now;
                        DateTo = DateTime.Now;
                        IsEditDate = true;
                    }
                    else if (SelectDateType.Key == 2)
                    {
                        IsEditDate = false;
                        DateFrom = DateTime.Now.AddDays(-7);
                        DateTo = DateTime.Now;
                        IsEditDate = true;
                    }
                    else if (SelectDateType.Key == 3)
                    {
                        IsEditDate = false;
                        DateFrom = DateTime.Parse("01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year);
                        DateTo = DateTime.Now;
                        IsEditDate = true;
                    }
                }
            }
        }

        //private List<HealthOrganisationModel> _Organisations;

        //public List<HealthOrganisationModel> Organisations
        //{
        //    get { return _Organisations; }
        //    set { Set(ref _Organisations, value); }
        //}

        //private HealthOrganisationModel _SelectOrganisation;

        //public HealthOrganisationModel SelectOrganisation
        //{
        //    get { return _SelectOrganisation; }
        //    set { Set(ref _SelectOrganisation, value);
        //        if (SelectOrganisation != null)
        //        {
        //            Location = DataService.MasterData.GetLocationIsRegister(SelectOrganisation.HealthOrganisationUID);

        //        }
        //        else
        //        {
        //            Location = null;
        //            SelectLocation = null;
        //        }
        //    }
        //}

        private List<LocationModel> _Location;
        public List<LocationModel> Location
        {
            get { return _Location; }
            set { Set(ref _Location, value);
            //if(Location != null)
            //    {
            //        var data = Location.FirstOrDefault(p => p.LocationUID == AppUtil.Current.LocationUID);
            //        if(data != null)
            //        {
            //            SelectLocation = Location.FirstOrDefault(p => p.LocationUID == data.LocationUID);
            //        }
            //    }
            }
        }

        private LocationModel _SelectLocation;
        public LocationModel SelectLocation
        {
            get { return _SelectLocation; }
            set { Set(ref _SelectLocation, value); }
        }

        #endregion

        #region Command

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(SearchPatientVisit)); }
        }


        private RelayCommand _SendToDoctorCommand;

        public RelayCommand SendToDoctorCommand
        {
            get { return _SendToDoctorCommand ?? (_SendToDoctorCommand = new RelayCommand(SendToDoctor)); }
        }

        private RelayCommand _MedicalDischargeCommand;

        public RelayCommand MedicalDischargeCommand
        {
            get { return _MedicalDischargeCommand ?? (_MedicalDischargeCommand = new RelayCommand(MedicalDischarge)); }
        }

        private RelayCommand _RunPatientReportCommand;

        public RelayCommand RunPatientReportCommand
        {
            get { return _RunPatientReportCommand ?? (_RunPatientReportCommand = new RelayCommand(RunPatientReport)); }
        }


        private RelayCommand _VitalSignCommand;

        public RelayCommand VitalSignCommand
        {
            get { return _VitalSignCommand ?? (_VitalSignCommand = new RelayCommand(VitalSign)); }
        }

        private RelayCommand _CreateOrderCommand;

        public RelayCommand CreateOrderCommand
        {
            get { return _CreateOrderCommand ?? (_CreateOrderCommand = new RelayCommand(CreateOrder)); }
        }

        private RelayCommand _AllergyCommand;

        public RelayCommand AllergyCommand
        {
            get { return _AllergyCommand ?? (_AllergyCommand = new RelayCommand(Allergy)); }
        }


        private RelayCommand _PatientRecordsCommand;

        public RelayCommand PatientRecordsCommand
        {
            get { return _PatientRecordsCommand ?? (_PatientRecordsCommand = new RelayCommand(PatientRecords)); }
        }

        private RelayCommand _ModifyVisitCommand;

        public RelayCommand ModifyVisitCommand
        {
            get { return _ModifyVisitCommand ?? (_ModifyVisitCommand = new RelayCommand(OpenModifyVisit)); }
        }

        private RelayCommand _ScannedDocumentCommand;

        public RelayCommand ScannedDocumentCommand
        {
            get { return _ScannedDocumentCommand ?? (_ScannedDocumentCommand = new RelayCommand(ScannedDocument)); }
        }


        private RelayCommand _SalesItemCommand;

        public RelayCommand SalesItemCommand
        {
            get { return _SalesItemCommand ?? (_SalesItemCommand = new RelayCommand(SalesItem)); }
        }

        private RelayCommand _ExportToExcelCommand;

        public RelayCommand ExportToExcelCommand
        {
            get { return _ExportToExcelCommand ?? (_ExportToExcelCommand = new RelayCommand(ExportToExcel)); }
        }

        private RelayCommand _AdmissionRequestCommand;
        public RelayCommand AdmissionRequestCommand
        {
            get { return _AdmissionRequestCommand ?? (_AdmissionRequestCommand = new RelayCommand(AdmissionRequest)); }
        }
        
        private RelayCommand _PatientTrackingCommand;
        public RelayCommand PatientTrackingCommand
        {
            get { return _PatientTrackingCommand ?? (_PatientTrackingCommand = new RelayCommand(PatientTracking)); }
        }

        private RelayCommand _ModifyVisitPayorCommand;
        public RelayCommand ModifyVisitPayorCommand
        {
            get { return _ModifyVisitPayorCommand ?? (_ModifyVisitPayorCommand = new RelayCommand(ModifyVisitPayor)); }
        }

        

        #endregion

        #region Method

        #region Variable

        int REGST = 417;
        int CHKOUT = 418;
        int SNDDOC = 419;
        int BLINP = 423;
        int FINDIS = 421;
        int CANCEL = 410;

        #endregion

        public PatientListViewModel()
        {
            EncounterType = new ObservableCollection<LookupReferenceValueModel>(DataService.Technical.GetReferenceValueMany("ENTYP"));
            SelectEncounterType.Add(EncounterType.FirstOrDefault(p => p.ValueCode == "OUPAT").Key);
            Doctors = DataService.UserManage.GetCareproviderDoctor();
            VisitStatus = new ObservableCollection<LookupReferenceValueModel>(DataService.Technical.GetReferenceValueMany("VISTS"));
            SelectVisitStatusList.Add(VisitStatus.FirstOrDefault(p => p.ValueCode == "REGST").Key);
            SelectVisitStatusList.Add(VisitStatus.FirstOrDefault(p => p.ValueCode == "SNDDOC").Key);
            DateTypes = new List<LookupItemModel>();
            DateTypes.Add(new LookupItemModel { Key = 1, Display = "วันนี้" });
            DateTypes.Add(new LookupItemModel { Key = 2, Display = "อาทิตย์ล่าสุด" });
            DateTypes.Add(new LookupItemModel { Key = 3, Display = "เดือนนี้" });
            SelectDateType = DateTypes.FirstOrDefault();
            var org = GetLocatioinRole(AppUtil.Current.OwnerOrganisationUID);
            Location = org.Where(p => p.IsRegistrationAllowed == "Y").ToList();
            SelectLocation = Location.FirstOrDefault(p => p.LocationUID == AppUtil.Current.LocationUID);
            //Organisations = GetHealthOrganisationMedical();
            //SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            InsuranceCompany = DataService.Billing.GetInsuranceCompanyAll();
            SelectInsuranceCompany = InsuranceCompany.FirstOrDefault(p => p.Code == "000001");
        }

        public override void OnLoaded()
        {
            SearchPatientVisit();
        }
        private void SearchPatientVisit()
        {
            string statusList = string.Empty;
            if (SelectVisitStatusList != null)
            {
                foreach (object item in SelectVisitStatusList)
                {
                    if (statusList == "")
                    {
                        statusList = item.ToString();
                    }
                    else
                    {
                        statusList += "," + item.ToString();
                        
                    }
                }
            }

            string encounterList = string.Empty;
            if (SelectEncounterType != null)
            {
                foreach (object item in SelectEncounterType)
                {
                    if (encounterList == "")
                    {
                        encounterList = item.ToString();
                    }
                    else
                    {
                        encounterList += "," + item.ToString();

                    }
                }
            }

            int? insuranceCompanyUID = SelectInsuranceCompany != null ? SelectInsuranceCompany.InsuranceCompanyUID : (int?)null;
            int? careproviderUID = SelectDoctor != null ? SelectDoctor.CareproviderUID : (int?)null;
            int? locationUID = SelectLocation != null ? SelectLocation.LocationUID : (int?)null;
            int? ownerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
            PatientVisits = new ObservableCollection<PatientVisitModel>(DataService.PatientIdentity.SearchPatientVisit(LN, FirstName, LastName, careproviderUID, statusList, DateFrom, DateTo, null, ownerOrganisationUID,locationUID, insuranceCompanyUID, null, encounterList));
        }

        private void VitalSign()
        {
            if (SelectPatientVisit != null)
            {
                PatientVitalSign pageview = new PatientVitalSign();
                (pageview.DataContext as PatientVitalSignViewModel).AssingPatientVisit(SelectPatientVisit);
                PatientVitalSignViewModel result = (PatientVitalSignViewModel)LaunchViewDialog(pageview, "PTVAT", false);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    //SearchPatientVisit();
                }
            }

        }

        private void CreateOrder()
        {
            if (SelectPatientVisit != null)
            {
                PatientOrderEntry pageview = new PatientOrderEntry();
                (pageview.DataContext as PatientOrderEntryViewModel).AssingPatientVisit(SelectPatientVisit);
                PatientOrderEntryViewModel result = (PatientOrderEntryViewModel)LaunchViewDialog(pageview, "ORDITM", false, true);
            }
        }

        private void OpenModifyVisit()
        {
            if (SelectPatientVisit != null)
            {

                ModifyVisit pageview = new ModifyVisit();
                (pageview.DataContext as ModifyVisitViewModel).AssingPatientVisit(SelectPatientVisit);
                ModifyVisitViewModel result = (ModifyVisitViewModel)LaunchViewDialog(pageview, "MDVIS", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    SearchPatientVisit();
                }
            }
        }

        private void ModifyVisitPayor()
        {
            if (SelectPatientVisit != null)
            {
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectPatientVisit.PatientVisitUID);
                if (patientVisit.VISTSUID == FINDIS || patientVisit.VISTSUID == CANCEL)
                {
                    WarningDialog("ไม่สามารถดำเนินการได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
                    SelectPatientVisit.VISTSUID = patientVisit.VISTSUID;
                    SelectPatientVisit.VisitStatus = patientVisit.VisitStatus;
                    OnUpdateEvent();
                    return;
                }
                ModifyVisitPayor pageview = new ModifyVisitPayor();
                (pageview.DataContext as ModifyVisitPayorViewModel).AssingPatientVisit(SelectPatientVisit);
                ModifyVisitPayorViewModel result = (ModifyVisitPayorViewModel)LaunchViewDialog(pageview, "MODPAY", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                }
            }
        }

        private void ScannedDocument()
        {
            if (SelectPatientVisit != null)
            {

                ScannedDocument pageview = new ScannedDocument();
                (pageview.DataContext as ScannedDocumentViewModel).AssingPatientVisit(SelectPatientVisit);
                ScannedDocumentViewModel result = (ScannedDocumentViewModel)LaunchViewDialog(pageview, "PATSCD",false);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    SearchPatientVisit();
                }
            }
        }

        private void Allergy()
        {
            if (SelectPatientVisit != null)
            {
                PatientAllergy pageview = new PatientAllergy();
                (pageview.DataContext as PatientAllergyViewModel).AssingPatientVisit(SelectPatientVisit);
                PatientAllergyViewModel result = (PatientAllergyViewModel)LaunchViewDialog(pageview, "LIARGY", false);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    //SearchPatientVisit();
                }
            }

        }

        private void SendToDoctor()
        {
            if (SelectPatientVisit != null)
            {
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectPatientVisit.PatientVisitUID);
                if (patientVisit.VISTSUID == CHKOUT || patientVisit.VISTSUID == BLINP || patientVisit.VISTSUID == FINDIS || patientVisit.VISTSUID == CANCEL)
                {
                    WarningDialog("ไม่สามารถดำเนินการได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
                    SelectPatientVisit.VISTSUID = patientVisit.VISTSUID;
                    SelectPatientVisit.VisitStatus = patientVisit.VisitStatus;
                    OnUpdateEvent();
                    return;
                }
                PatientStatus sendToDoctor = new PatientStatus(SelectPatientVisit, PatientStatusType.SendToDoctor);
                sendToDoctor.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                sendToDoctor.Owner = MainWindow;
                sendToDoctor.ShowDialog();
                ActionDialog result = sendToDoctor.ResultDialog;
                if (result == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    SearchPatientVisit();
                }
            }
        }

        private void MedicalDischarge()
        {
            if (SelectPatientVisit != null)
            {
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectPatientVisit.PatientVisitUID);
                if (patientVisit.VISTSUID == CHKOUT || patientVisit.VISTSUID == BLINP || patientVisit.VISTSUID == FINDIS || patientVisit.VISTSUID == CANCEL)
                {
                    WarningDialog("ไม่สามารถดำเนินการได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
                    SelectPatientVisit.VISTSUID = patientVisit.VISTSUID;
                    SelectPatientVisit.VisitStatus = patientVisit.VisitStatus;
                    OnUpdateEvent();
                    return;
                }
                PatientStatus medicalDischarge = new PatientStatus(SelectPatientVisit, PatientStatusType.MedicalDischarge);
                medicalDischarge.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                medicalDischarge.Owner = MainWindow;
                medicalDischarge.ShowDialog();
                ActionDialog result = medicalDischarge.ResultDialog;
                if (result == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    SearchPatientVisit();
                }
            }
        }


        private void PatientRecords()
        {
            if (SelectPatientVisit != null)
            {
                EMRView pageview = new EMRView();
                (pageview.DataContext as EMRViewViewModel).AssingPatientVisit(SelectPatientVisit);
                EMRViewViewModel result = (EMRViewViewModel)LaunchViewDialog(pageview, "EMRVE", false, true);
            }
        }
        private void SalesItem()
        {
            if (SelectPatientVisit != null)
            {
                //if (SelectPatientVisit.VISTSUID == CHKOUT || SelectPatientVisit.VISTSUID == FINDIS)
                //{
                //    WarningDialog("ไม่สามารถดำเนินการได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
                //    return;
                //}
                ManageBilledItemSale pageview = new ManageBilledItemSale();
                //(pageview.DataContext as ManageBilledItemSaleViewModel).AssingPatientVisit(SelectPatientVisit);
                ManageBilledItemSaleViewModel result = (ManageBilledItemSaleViewModel)LaunchViewDialog(pageview, "SALESAPP", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    SearchPatientVisit();
                }
            }
        }
        private void AdmissionRequest()
        {
            if(SelectPatientVisit != null)
            {
                AdmissionRequest pageview = new AdmissionRequest();
                (pageview.DataContext as AdmissionRequestViewModel).AssingModel(SelectPatientVisit);
                AdmissionRequestViewModel result = (AdmissionRequestViewModel)LaunchViewDialog(pageview, "ADRQST", false);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    SearchPatientVisit();
                }
            }
            
        }

        private void PatientTracking()
        {
            if (SelectPatientVisit != null)
            {
                PatientTracking pageview = new PatientTracking();
                (pageview.DataContext as PatientTrackingViewModel).AssingModel(SelectPatientVisit);
                PatientTrackingViewModel result = (PatientTrackingViewModel)LaunchViewDialog(pageview, "PATRCK", false);  
            }

        }

        private void ExportToExcel()
        {
            try
            {
                if (PatientVisits != null)
                {
                    string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xlsx");
                    if (fileName != "")
                    {
                        PatientList view = (PatientList)this.View;
                        view.tableViewVisitList.ExportToXlsx(fileName);
                        OpenFile(fileName);
                    }

                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void RunPatientReport()
        {
            if (SelectPatientVisit != null)
            {
                ShowModalDialogUsingViewModel(new RunPatientReports(), new RunPatientReportsViewModel() { SelectPatientVisit = SelectPatientVisit }, true);
            }
        }
        #endregion
    }
}
