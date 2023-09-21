using DevExpress.DataAccess.Native.ObjectBinding;
using DevExpress.DataAccess.Native.Sql.MasterDetail;
using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels.Doctor
{
    public class PatientListForDoctorViewModel : MediTechViewModelBase
    {
        #region Properties

        public string LN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool IsEditDate { get; set; }

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

        private List<PatientVisitModel> _SelectPatientVisits;

        public List<PatientVisitModel> SelectPatientVisits
        {
            get { return _SelectPatientVisits ?? (_SelectPatientVisits = new List<PatientVisitModel>()); }
            set { Set(ref _SelectPatientVisits,value); }
        }

        private List<InsuranceCompanyModel> _InsuranceCompany;
        public  List<InsuranceCompanyModel> InsuranceCompany
        {
            get { return _InsuranceCompany; }
            set { Set(ref _InsuranceCompany, value); }
        }

        private InsuranceCompanyModel _SelectInsuranceCompany;
        public InsuranceCompanyModel SelectInsuranceCompany
        {
            get { return _SelectInsuranceCompany; }
            set
            {
                Set(ref _SelectInsuranceCompany, value);
                if (_SelectInsuranceCompany != null)
                {
                    CheckupJobContactList = DataService.Checkup.GetCheckupJobContactByPayorDetailUID(_SelectInsuranceCompany.InsuranceCompanyUID);
                    SelectCheckupJobContact = CheckupJobContactList.OrderByDescending(p => p.StartDttm).FirstOrDefault();
                }
            }
        }

        private List<CheckupJobContactModel> _CheckupJobContactList;
        public List<CheckupJobContactModel> CheckupJobContactList
        {
            get { return _CheckupJobContactList; }
            set { Set(ref _CheckupJobContactList, value); }
        }

        private CheckupJobContactModel _SelectCheckupJobContact;
        public CheckupJobContactModel SelectCheckupJobContact
        {
            get { return _SelectCheckupJobContact; }
            set
            {
                Set(ref _SelectCheckupJobContact, value);
                if (_SelectCheckupJobContact != null)
                {
                    DateFrom = _SelectCheckupJobContact.StartDttm;
                    //DateTo = _SelectCheckupJobContact.EndDttm;
                }
            }
        }

        private List<HealthOrganisationModel> _Organisations;

        public List<HealthOrganisationModel> Organisations
        {
            get { return _Organisations; }
            set { Set(ref _Organisations, value); }
        }

        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set
            {
                Set(ref _SelectOrganisation, value);
                Locations = null;
                if (SelectOrganisation != null)
                {
                    var loct = DataService.MasterData.GetLocationRoleByOrganisationUID(SelectOrganisation.HealthOrganisationUID, AppUtil.Current.UserID);
                    Locations = loct.Where(p => p.IsRegistrationAllowed == "Y").ToList();
                }
            }
        }

        private List<LocationModel> _Locations;
        public List<LocationModel> Locations
        {
            get { return _Locations; }
            set
            {
                Set(ref _Locations, value);
                if (Locations != null)
                {
                    var data = Locations.FirstOrDefault(p => p.LocationUID == AppUtil.Current.LocationUID);
                    if (data != null)
                    {
                        SelectLocation = Locations.FirstOrDefault(p => p.LocationUID == data.LocationUID);
                    }
                }
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

        private RelayCommand _DiagnosisCommand;

        public RelayCommand DiagnosisCommand
        {
            get { return _DiagnosisCommand ?? (_DiagnosisCommand = new RelayCommand(Diagnosis)); }
        }

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(SearchPatientVisit)); }
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

        private RelayCommand _MedicalDischargeCommand;

        public RelayCommand MedicalDischargeCommand
        {
            get { return _MedicalDischargeCommand ?? (_MedicalDischargeCommand = new RelayCommand(MedicalDischarge)); }
        }


        private RelayCommand _PatientTrackingCommand;
        public RelayCommand PatientTrackingCommand
        {
            get { return _PatientTrackingCommand ?? (_PatientTrackingCommand = new RelayCommand(PatientTracking)); }
        }

        private RelayCommand _AssignDoctorGPCommand;
        public RelayCommand AssignDoctorGPCommand
        {
            get { return _AssignDoctorGPCommand ?? (_AssignDoctorGPCommand = new RelayCommand(AssignDoctorGP)); }
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

        public PatientListForDoctorViewModel()
        {
            Doctors = DataService.UserManage.GetCareproviderDoctor();
            VisitStatus = new ObservableCollection<LookupReferenceValueModel>(DataService.Technical.GetReferenceValueMany("VISTS"));

            SelectVisitStatusList.AddRange(VisitStatus?.Where(p => p.ValueCode != "FINDIS").Select(p => (object)p.Key.Value).ToList());
            DateTypes = new List<LookupItemModel>();
            DateTypes.Add(new LookupItemModel { Key = 1, Display = "วันนี้" });
            DateTypes.Add(new LookupItemModel { Key = 2, Display = "อาทิตย์ล่าสุด" });
            DateTypes.Add(new LookupItemModel { Key = 3, Display = "เดือนนี้" });
            SelectDateType = DateTypes.FirstOrDefault();
            Organisations = GetHealthOrganisationRole();
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);

            InsuranceCompany = DataService.Billing.GetInsuranceCompanyAll();
        }

        void Diagnosis()
        {
            try
            {
                if (SelectPatientVisits != null && SelectPatientVisits.Count() > 0)
                {
                    PatientDiagnosis pageview = new PatientDiagnosis();
                    (pageview.DataContext as PatientDiagnosisViewModel).IsVisitMass = true;
                    PatientDiagnosisViewModel result = (PatientDiagnosisViewModel)LaunchViewDialog(pageview, "PATDIAG", false);

                    if (result != null)
                    {
                        if (result.ResultDialog == ActionDialog.Save)
                        {
                            List<PatientProblemModel> listProblem = new List<PatientProblemModel>();
                            foreach (var patVisit in SelectPatientVisits)
                            {
                                foreach (var patPromblem in result.PatientProblemList)
                                {
                                    PatientProblemModel newProblem = new PatientProblemModel();
                                    newProblem.PatientUID = patVisit.PatientUID;
                                    newProblem.PatientVisitUID = patVisit.PatientVisitUID;
                                    newProblem.ProblemUID = patPromblem.ProblemUID;
                                    newProblem.ProblemCode = patPromblem.ProblemCode;
                                    newProblem.ProblemName = patPromblem.ProblemName;
                                    newProblem.ProblemDescription = patPromblem.ProblemDescription;
                                    newProblem.DIAGTYPUID = patPromblem.DIAGTYPUID;
                                    newProblem.DiagnosisType = patPromblem.DiagnosisType;
                                    newProblem.IsUnderline = patPromblem.IsUnderline;
                                    newProblem.SEVRTUID = patPromblem.SEVRTUID;
                                    newProblem.CERNTUID = patPromblem.CERNTUID;
                                    newProblem.PBMTYUID = patPromblem.PBMTYUID;
                                    newProblem.BDLOCUID = patPromblem.BDLOCUID;
                                    newProblem.Severity = patPromblem.Severity;
                                    newProblem.Certanity = patPromblem.Certanity;
                                    newProblem.ProblemType = patPromblem.ProblemType;
                                    newProblem.BodyLocation = patPromblem.BodyLocation;
                                    newProblem.OnsetDttm = patPromblem.OnsetDttm;
                                    newProblem.ClosureDttm = patPromblem.ClosureDttm;
                                    newProblem.ClosureComments = patPromblem.ClosureComments;
                                    newProblem.RecordedDttm = patPromblem.RecordedDttm;
                                    newProblem.RecordedName = AppUtil.Current.UserName;
                                    listProblem.Add(newProblem);
                                }
                            }
                            if (listProblem.Count() > 0)
                            {
                                DataService.PatientDiagnosis.ManagePatientProblemMass(listProblem, AppUtil.Current.UserID);
                            }
                            SaveSuccessDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }

        private void AssignDoctorGP()
        {
            try
            {
                if (SelectPatientVisits == null)
                {
                    WarningDialog("กรุณาเลือก Visit อย่างน้อย 1 Visit");
                    return;
                }
                AssignDoctorGPPopup assignDocPopup = new AssignDoctorGPPopup();
                AssignDoctorGPPopupViewModel result = (AssignDoctorGPPopupViewModel)LaunchViewDialogNonPermiss(assignDocPopup, true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    int countStaff = result.CareproviderLists.Count();
                    int indexStaff = 0;
                    int countPerCase = (SelectPatientVisits.Count() / countStaff) == 0 ? 1 : (SelectPatientVisits.Count() / countStaff);
                    int countloop = 0;


                    foreach (var visitItem in SelectPatientVisits)
                    {
                        if (countloop >= countPerCase)
                        {
                            countloop = 0;
                            indexStaff++;
                        }
                        if (visitItem.Equals(SelectPatientVisits.Last()) && SelectPatientVisits.Count > 1)
                        {
                            indexStaff = countStaff - 1;
                        }
                        visitItem.CareProvider2UID = result.CareproviderLists[indexStaff].CareproviderUID;
                        countloop++;
                    }

                    DataService.PatientIdentity.AssignDoctorGPList(SelectPatientVisits, AppUtil.Current.UserID);
                    SaveSuccessDialog();
                    SearchPatientVisit();
                }
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }
        
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

        private void MedicalDischarge()
        {
            if (SelectPatientVisit != null)
            {
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectPatientVisit.PatientVisitUID);
                if (patientVisit.VISTSUID == BLINP || patientVisit.VISTSUID == FINDIS || patientVisit.VISTSUID == CANCEL)
                {
                    WarningDialog("ไม่สามารถดำเนินการได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
                    SelectPatientVisit.VISTSUID = patientVisit.VISTSUID;
                    SelectPatientVisit.VisitStatus = patientVisit.VisitStatus;
                    OnUpdateEvent();
                    return;
                }
                PatientStatus medicalDischarge = new PatientStatus(SelectPatientVisit, PatientStatusType.MedicalDischarge, null);
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
                (pageview.DataContext as EMRViewViewModel).AssignPatientVisit(SelectPatientVisit);
                EMRViewViewModel result = (EMRViewViewModel)LaunchViewDialog(pageview, "EMRVE", false, true);
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
            int? organisationUID = SelectOrganisation != null ? SelectOrganisation.HealthOrganisationUID : (int?)null;
            int? locationUID = SelectLocation != null ? SelectLocation.LocationUID : (int?)null;
            int userUID = AppUtil.Current.UserID;
            int? checkupJobUID = SelectCheckupJobContact != null ? SelectCheckupJobContact.CheckupJobContactUID : (int?)null;
            PatientVisits = new ObservableCollection<PatientVisitModel>(DataService.PatientIdentity.SearchPatientVisit(LN, FirstName, LastName, careproviderUID, statusList, DateFrom, DateTo, null, organisationUID, locationUID, insuranceCompanyUID, checkupJobUID, null, userUID));
        }


        #endregion
    }
}
