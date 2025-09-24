using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class EmergencyListViewModel: MediTechViewModelBase
    {
        #region Properties
        public string LN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsEditDate { get; set; }

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

        private List<LocationModel> _Location;
        public List<LocationModel> Location
        {
            get { return _Location; }
            set { Set(ref _Location, value); }
        }

        private LocationModel _SelectLocation;
        public LocationModel SelectLocation
        {
            get { return _SelectLocation; }
            set { Set(ref _SelectLocation, value); }
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

        private List<LookupReferenceValueModel> _EncounterType;
        public List<LookupReferenceValueModel> EncounterType
        {
            get { return _EncounterType; }
            set { Set(ref _EncounterType, value); }
        }

        private LookupReferenceValueModel _SelectEncounterType;
        public LookupReferenceValueModel SelectEncounterType
        {
            get { return _SelectEncounterType; }
            set { Set(ref _SelectEncounterType, value); }
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

        private RelayCommand _VitalSignCommand;
        public RelayCommand VitalSignCommand
        {
            get { return _VitalSignCommand ?? (_VitalSignCommand = new RelayCommand(VitalSign)); }
        }

        private RelayCommand _AllergyCommand;
        public RelayCommand AllergyCommand
        {
            get { return _AllergyCommand ?? (_AllergyCommand = new RelayCommand(Allergy)); }
        }

        private RelayCommand _CreateOrderCommand;
        public RelayCommand CreateOrderCommand
        {
            get { return _CreateOrderCommand ?? (_CreateOrderCommand = new RelayCommand(CreateOrder)); }
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

        private RelayCommand _ModifyVisitCommand;
        public RelayCommand ModifyVisitCommand
        {
            get { return _ModifyVisitCommand ?? (_ModifyVisitCommand = new RelayCommand(OpenModifyVisit)); }
        }

        private RelayCommand _PatientRecordsCommand;
        public RelayCommand PatientRecordsCommand
        {
            get { return _PatientRecordsCommand ?? (_PatientRecordsCommand = new RelayCommand(PatientRecords)); }
        }

        private RelayCommand _ManageAEAdmissionCommand;
        public RelayCommand ManageAEAdmissionCommand
        {
            get { return _ManageAEAdmissionCommand ?? (_ManageAEAdmissionCommand = new RelayCommand(ManageAEAdmission)); }
        }

        private RelayCommand _ArrivedCommand;
        public RelayCommand ArrivedCommand
        {
            get { return _ArrivedCommand ?? (_ArrivedCommand = new RelayCommand(Arrived)); }
        }

        #endregion

        #region Medtod

        int REGST = 417;
        int CHKOUT = 418;
        int SNDDOC = 419;
        int FINDIS = 421;
        int CANCEL = 410;
        int BILLING = 423;

        public EmergencyListViewModel()
        {
            EncounterType = DataService.Technical.GetReferenceValueMany("ENTYP");
            SelectEncounterType = EncounterType.FirstOrDefault(p => p.ValueCode == "AEPAT");
            Doctors = DataService.UserManage.GetCareproviderDoctor();
            VisitStatus = new ObservableCollection<LookupReferenceValueModel>(DataService.Technical.GetReferenceValueMany("VISTS"));
            SelectVisitStatusList.Add(VisitStatus.FirstOrDefault(p => p.ValueCode == "REGST").Key);
            SelectVisitStatusList.Add(VisitStatus.FirstOrDefault(p => p.ValueCode == "SNDDOC").Key) ;
            DateTypes = new List<LookupItemModel>();
            DateTypes.Add(new LookupItemModel { Key = 1, Display = "วันนี้" });
            DateTypes.Add(new LookupItemModel { Key = 2, Display = "อาทิตย์ล่าสุด" });
            DateTypes.Add(new LookupItemModel { Key = 3, Display = "เดือนนี้" });
            SelectDateType = DateTypes.FirstOrDefault();

            var org = GetLocatioinRole(AppUtil.Current.OwnerOrganisationUID);
            Location = org.Where(p => p.IsRegistrationAllowed == "Y").ToList();
            SelectLocation = Location.FirstOrDefault(p => p.LocationUID == AppUtil.Current.LocationUID);
            InsuranceCompany = DataService.Billing.GetInsuranceCompanyAll();
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

            int? insuranceCompanyUID = SelectInsuranceCompany != null ? SelectInsuranceCompany.InsuranceCompanyUID : (int?)null;
            int? careproviderUID = SelectDoctor != null ? SelectDoctor.CareproviderUID : (int?)null;
            int? locationUID = SelectLocation != null ? SelectLocation.LocationUID : (int?)null;
            int? ownerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
            int? encounterUID = SelectEncounterType.Key ?? 0;
            PatientVisits = new ObservableCollection<PatientVisitModel>(DataService.PatientIdentity.SearchERPatientVisit(LN, FirstName, LastName, careproviderUID, statusList, DateFrom, DateTo, null, ownerOrganisationUID, locationUID, insuranceCompanyUID, null, encounterUID));
        }
        private void Arrived()
        {
            if (SelectPatientVisit != null)
            {
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectPatientVisit.PatientVisitUID);
                if (patientVisit.VISTSUID == FINDIS || patientVisit.VISTSUID == CANCEL || patientVisit.VISTSUID == BILLING)
                {
                    WarningDialog("ไม่สามารถดำเนินการได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
                    SelectPatientVisit.VISTSUID = patientVisit.VISTSUID;
                    SelectPatientVisit.VisitStatus = patientVisit.VisitStatus;
                    OnUpdateEvent();
                    return;
                }
                PatientStatus arrived = new PatientStatus(SelectPatientVisit, PatientStatusType.Arrive,null);
                arrived.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                arrived.Owner = MainWindow;
                arrived.ShowDialog();
                ActionDialog result = arrived.ResultDialog;
                if (result == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    SearchPatientVisit();
                }
            }
        }

        private void SendToDoctor()
        {
            if (SelectPatientVisit != null)
            {
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectPatientVisit.PatientVisitUID);
                if (patientVisit.VISTSUID == CHKOUT || patientVisit.VISTSUID == FINDIS || patientVisit.VISTSUID == CANCEL || patientVisit.VISTSUID == BILLING)
                {
                    WarningDialog("ไม่สามารถดำเนินการได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
                    SelectPatientVisit.VISTSUID = patientVisit.VISTSUID;
                    SelectPatientVisit.VisitStatus = patientVisit.VisitStatus;
                    OnUpdateEvent();
                    return;
                }
                PatientStatus sendToDoctor = new PatientStatus(SelectPatientVisit, PatientStatusType.SendToDoctor,null);
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

        private void ManageAEAdmission()
        {
            
            if (SelectPatientVisit != null)
            {
                PatientAEAdmissionModel erVisit = DataService.PatientIdentity.GetPatientAEAdmissionByUID(SelectPatientVisit.PatientVisitUID);

                EmergencyRegister pageview = new EmergencyRegister();
                (pageview.DataContext as EmergencyRegisterViewModel).AssingModel(new PatientInformationModel(),erVisit);
                EmergencyRegisterViewModel result = (EmergencyRegisterViewModel)LaunchViewDialog(pageview, "ERREG", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    SearchPatientVisit();
                }
            }
        }


        private void VitalSign()
        {
            if (SelectPatientVisit != null)
            {
                PatientVitalSign pageview = new PatientVitalSign();
                (pageview.DataContext as PatientVitalSignViewModel).AssingPatientVisit(SelectPatientVisit);
                PatientVitalSignViewModel result = (PatientVitalSignViewModel)LaunchViewDialog(pageview, "PTVAT",true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    //SearchPatientVisit();
                }
            }

        }

        private void Allergy()
        {
            if (SelectPatientVisit != null)
            {
                PatientAllergy pageview = new PatientAllergy();
                (pageview.DataContext as PatientAllergyViewModel).AssignPatientVisit(SelectPatientVisit);
                PatientAllergyViewModel result = (PatientAllergyViewModel)LaunchViewDialog(pageview, "LIARGY", false);
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
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectPatientVisit.PatientVisitUID);
                if (patientVisit.VISTSUID == CHKOUT || patientVisit.VISTSUID == FINDIS || patientVisit.VISTSUID == CANCEL || patientVisit.VISTSUID == BILLING)
                {
                    WarningDialog("ไม่สามารถดำเนินการได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
                    SelectPatientVisit.VISTSUID = patientVisit.VISTSUID;
                    SelectPatientVisit.VisitStatus = patientVisit.VisitStatus;
                    OnUpdateEvent();
                    return;
                }
                PatientOrderEntry pageview = new PatientOrderEntry();
                (pageview.DataContext as PatientOrderEntryViewModel).AssingPatientVisit(SelectPatientVisit);
                PatientOrderEntryViewModel result = (PatientOrderEntryViewModel)LaunchViewDialog(pageview, "ORDITM", false, true);
            }
        }
        private void MedicalDischarge()
        {
            if (SelectPatientVisit != null)
            {
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectPatientVisit.PatientVisitUID);
                if (patientVisit.VISTSUID == BILLING || patientVisit.VISTSUID == FINDIS || patientVisit.VISTSUID == CANCEL)
                {
                    WarningDialog("ไม่สามารถดำเนินการได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
                    SelectPatientVisit.VISTSUID = patientVisit.VISTSUID;
                    SelectPatientVisit.VisitStatus = patientVisit.VisitStatus;
                    OnUpdateEvent();
                    return;
                }
                PatientStatus medicalDischarge = new PatientStatus(SelectPatientVisit, PatientStatusType.MedicalDischarge,null);
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



        private void RunPatientReport()
        {
            if (SelectPatientVisit != null)
            {
                ShowModalDialogUsingViewModel(new RunPatientReports(), new RunPatientReportsViewModel() { SelectPatientVisit = SelectPatientVisit }, true);
            }
        }

        private void OpenModifyVisit()
        {
            if (SelectPatientVisit != null)
            {
                ModifyVisitPayor pageview = new ModifyVisitPayor();
                (pageview.DataContext as ModifyVisitPayorViewModel).AssingPatientVisit(SelectPatientVisit);
                ModifyVisitPayorViewModel result = (ModifyVisitPayorViewModel)LaunchViewDialog(pageview, "MODPAY", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    SearchPatientVisit();
                }

                //ModifyVisit pageview = new ModifyVisit();
                //(pageview.DataContext as ModifyVisitViewModel).AssingPatientVisit(SelectPatientVisit);
                //ModifyVisitViewModel result = (ModifyVisitViewModel)LaunchViewDialog(pageview, "MDVIS", true);
                //if (result != null && result.ResultDialog == ActionDialog.Save)
                //{
                //    SaveSuccessDialog();
                //    SearchPatientVisit();
                //}
            }
        }

        private void PatientRecords()
        {
            if (SelectPatientVisit != null)
            {
                EMRView pageview = new EMRView();
                (pageview.DataContext as EMRViewViewModel).AssignPatientVisit(SelectPatientVisit);
                EMRViewViewModel result = (EMRViewViewModel)LaunchViewDialog(pageview, "EMRVE", false, true);
            }
        }

        #endregion
    }
}
