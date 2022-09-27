using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class PatientAlertViewModel : MediTechViewModelBase
    {
        #region Properties

        private List<LookupReferenceValueModel> _AlertType;
        public List<LookupReferenceValueModel> AlertType
        {
            get { return _AlertType; }
            set
            {
                Set(ref _AlertType, value);
            }
        }

        private LookupReferenceValueModel _SelectedAlertType;
        public LookupReferenceValueModel SelectedAlertType
        {
            get { return _SelectedAlertType; }
            set
            {
                Set(ref _SelectedAlertType, value);
            }
        }

        private List<LookupReferenceValueModel> _Alert;
        public List<LookupReferenceValueModel> Alert
        {
            get { return _Alert; }
            set
            {
                Set(ref _Alert, value);
            }
        }

        private LookupReferenceValueModel _SelectedAlert;
        public LookupReferenceValueModel SelectedAlert
        {
            get { return _SelectedAlert; }
            set
            {
                Set(ref _SelectedAlert, value);
            }
        }

        private List<LookupReferenceValueModel> _Severity;
        public List<LookupReferenceValueModel> Severity
        {
            get { return _Severity; }
            set
            {
                Set(ref _Severity, value);
            }
        }

        private LookupReferenceValueModel _SelectedSeverity;
        public LookupReferenceValueModel SelectedSeverity
        {
            get { return _SelectedSeverity; }
            set
            {
                Set(ref _SelectedSeverity, value);
            }
        }

        private List<LookupReferenceValueModel> _Priority;
        public List<LookupReferenceValueModel> Priority
        {
            get { return _Priority; }
            set
            {
                Set(ref _Priority, value);
            }
        }

        private LookupReferenceValueModel _SelectedPriority;
        public LookupReferenceValueModel SelectedPriority
        {
            get { return _SelectedPriority; }
            set
            {
                Set(ref _SelectedPriority, value);
            }
        }

        private List<LocationModel> _Locations;
        public List<LocationModel> Locations
        {
            get { return _Locations; }
            set { Set(ref _Locations, value); }
        }

        private LocationModel _SelectLocation;
        public LocationModel SelectLocation
        {
            get { return _SelectLocation; }
            set
            {
                Set(ref _SelectLocation, value);
            }
        }

        private DateTime? _OnsetDate;
        public DateTime? OnsetDate
        {
            get { return _OnsetDate; }
            set { Set(ref _OnsetDate, value); }
        }

        private DateTime? _ClosureDate;
        public DateTime? ClosureDate
        {
            get { return _ClosureDate; }
            set { Set(ref _ClosureDate, value); }
        }

        private string _AlertDescription;
        public string AlertDescription
        {
            get { return _AlertDescription; }
            set { Set(ref _AlertDescription, value); }
        }
        
        private bool _IsCurrentVisit;
        public bool IsCurrentVisit
        {
            get { return _IsCurrentVisit; }
            set { Set(ref _IsCurrentVisit, value); }
        }

        private bool _IsAcrossVisit;
        public bool IsAcrossVisit
        {
            get { return _IsAcrossVisit; }
            set { Set(ref _IsAcrossVisit, value); }
        }

        private ObservableCollection<PatientAlertModel> _PatientAlertSource;
        public ObservableCollection<PatientAlertModel> PatientAlertSource
        {
            get { return _PatientAlertSource ?? (_PatientAlertSource = new ObservableCollection<PatientAlertModel>()); }
            set { Set(ref _PatientAlertSource, value); }
        }

        private PatientAlertModel _SelectPatientAlert;
        public PatientAlertModel SelectPatientAlert
        {
            get { return _SelectPatientAlert; }
            set { Set(ref _SelectPatientAlert, value); 
                if(SelectPatientAlert != null)
                {
                    AssignModelToPropertie();
                }
            }
        }

        public PatientAlertModel alertModel { get; set; }
        public List<PatientAlertModel> alerttDelete;

        #region PatientSearch

        private string _SearchPatientCriteria;
        public string SearchPatientCriteria
        {
            get { return _SearchPatientCriteria; }
            set
            {
                Set(ref _SearchPatientCriteria, value);
                PatientsSearchSource = null;
            }
        }

        private List<PatientInformationModel> _PatientsSearchSource;
        public List<PatientInformationModel> PatientsSearchSource
        {
            get { return _PatientsSearchSource; }
            set { Set(ref _PatientsSearchSource, value); }
        }

        private PatientInformationModel _SelectedPateintSearch;
        public PatientInformationModel SelectedPateintSearch
        {
            get { return _SelectedPateintSearch; }
            set
            {
                _SelectedPateintSearch = value;
                if (_SelectedPateintSearch != null)
                {
                    PatientVisitModel visitInfoNonClose = DataService.PatientIdentity.GetLatestPatientVisitToConvert(_SelectedPateintSearch.PatientUID);
                    
                    if (visitInfoNonClose != null)
                    {
                        SelectPatientVisit = visitInfoNonClose;
                    }
                    else
                    {
                        PatientVisitModel patientVisit = new PatientVisitModel();
                        patientVisit.PatientID = SelectedPateintSearch != null ? SelectedPateintSearch.PatientID : null;
                        patientVisit.PatientUID = SelectedPateintSearch.PatientUID;

                        SelectPatientVisit = patientVisit;
                    }
                }
            }
        }

        private bool _IsSearchAll;
        public bool IsSearchAll
        {
            get { return _IsSearchAll; }
            set { _IsSearchAll = value; }
        }

        private PatientVisitModel _SelectPatientVisit;
        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set
            {
                Set(ref _SelectPatientVisit, value);
                if(SelectPatientVisit != null)
                {
                    var patientVisit = SelectPatientVisit.PatientVisitUID != 0 ? SelectPatientVisit.PatientVisitUID : (long?)null;
                    var listData = DataService.PatientIdentity.GetPatientAlertByPatientUID(SelectPatientVisit.PatientUID, patientVisit);
                    
                    if(listData.Count != 0)
                    {
                        PatientAlertSource = new ObservableCollection<PatientAlertModel>(listData);
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Command

        private RelayCommand _PatientSearchCommand;
        public RelayCommand PatientSearchCommand
        {
            get { return _PatientSearchCommand ?? (_PatientSearchCommand = new RelayCommand(PatientSearch)); }
        }

        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save)); }
        }

        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }

        private RelayCommand _AddCommand;
        public RelayCommand AddCommand
        {
            get { return _AddCommand ?? (_AddCommand = new RelayCommand(Add)); }
        }

        private RelayCommand _UpdateCommand;
        public RelayCommand UpdateCommand
        {
            get { return _UpdateCommand ?? (_UpdateCommand = new RelayCommand(Update)); }
        }

        private RelayCommand _DeleteCommand;
        public RelayCommand DeleteCommand
        {
            get { return _DeleteCommand ?? (_DeleteCommand = new RelayCommand(Delete)); }
        }

        private RelayCommand _ClearCommand;
        public RelayCommand ClearCommand
        {
            get { return _ClearCommand ?? (_ClearCommand = new RelayCommand(Clear)); }
        }

        #endregion

        #region Method
        public PatientAlertViewModel()
        {
            OnsetDate = DateTime.Now;
            AlertType = DataService.Technical.GetReferenceValueMany("RISCT");
            SelectedAlertType = AlertType.FirstOrDefault();
            Alert = DataService.Technical.GetReferenceValueMany("RISSK");
            Severity = DataService.Technical.GetReferenceValueMany("SEVRT");
            SelectedSeverity = Severity.FirstOrDefault();
            Priority = DataService.Technical.GetReferenceValueMany("RQARPRT");
            var LocationSource = GetLocatioinRole(AppUtil.Current.OwnerOrganisationUID);
            Locations = LocationSource.Where(p => p.IsRegistrationAllowed == "Y").ToList();
        }

        private void Save()
        {
            if(PatientAlertSource != null)
            {
                List<PatientAlertModel> models = new List<PatientAlertModel>(PatientAlertSource);

                if (alerttDelete != null)
                {
                    models.AddRange(alerttDelete);
                }
                DataService.PatientIdentity.ManagePatientAlert(models, AppUtil.Current.UserID);
                SaveSuccessDialog("บันทึกสำเร็จ");
                Clear();
            }
        }

        public void AssignModel()
        {

            if(alertModel == null)
            alertModel = new PatientAlertModel();

            alertModel.PatientUID = SelectPatientVisit.PatientUID;
            alertModel.PatientVisitUID = SelectPatientVisit.PatientVisitUID;
            alertModel.AlertDescription = AlertDescription;
            alertModel.ALRTYUID = SelectedAlertType != null ? SelectedAlertType.Key : (int?)null;
            alertModel.AlertType = SelectedAlertType?.Display;
            alertModel.ALTSTUID = SelectedAlert != null ? SelectedAlert.Key : (int?)null;
            alertModel.Alert = SelectedAlert?.Display;
            alertModel.SEVTYUID = SelectedSeverity != null ? SelectedSeverity.Key : (int?)null;
            alertModel.Severity = SelectedSeverity?.Display;
            alertModel.OnsetDttm = OnsetDate;
            alertModel.ClosureDttm = ClosureDate;
            alertModel.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
            alertModel.ALRPRTUID = SelectedPriority != null ? SelectedPriority.Key : (int?)null;
            alertModel.Priority = SelectedPriority?.Display;
            alertModel.LocationUID = SelectLocation != null ? SelectLocation.LocationUID : (int?)null;
            alertModel.Location = SelectLocation?.Name;
            alertModel.StatusFlag = "A";

            if(IsCurrentVisit == true)
            {
                alertModel.IsVisitSpecific = "Y";
            }

            if(IsAcrossVisit == true)
            {
                alertModel.IsVisitSpecific = "N";
            }
        }

        private void Add()
        {
            if (SelectPatientVisit == null)
            {
                WarningDialog("กรุณาค้นหาผู้ป่วย");
                return;
            }

            if (SelectedAlertType == null)
            {
                WarningDialog("กรุณาเลือก Alert Type");
                return;
            }

            if (SelectedSeverity == null)
            {
                WarningDialog("กรุณาเลือก Severity");
                return;
            }

            AssignModel();
            PatientAlertSource.Add(alertModel);
            Clear();
        }

        public void AssignModelToPropertie()
        {
            AlertDescription = SelectPatientAlert.AlertDescription;
            SelectedAlertType = AlertType.FirstOrDefault(p => p.Key == SelectPatientAlert.ALRTYUID);
            SelectedAlert = Alert.FirstOrDefault(p => p.Key == SelectPatientAlert.ALTSTUID);
            SelectedSeverity = Severity.FirstOrDefault(p => p.Key == SelectPatientAlert.SEVTYUID);
            OnsetDate = SelectPatientAlert.OnsetDttm;
            ClosureDate = SelectPatientAlert.ClosureDttm;
            SelectLocation = Locations.FirstOrDefault(p => p.LocationUID == SelectPatientAlert.LocationUID);
            SelectedPriority = Priority.FirstOrDefault(p => p.Key == SelectPatientAlert.ALRPRTUID);

            IsCurrentVisit = SelectPatientAlert.IsVisitSpecific == "Y" ? true : false;
            IsAcrossVisit = SelectPatientAlert.IsVisitSpecific == "N" ? true : false;
        }

        private void Update()
        {
            if (SelectPatientAlert != null)
            {
                alertModel = SelectPatientAlert;
                AssignModel();
                PatientAlertSource.Remove(SelectPatientAlert);
                PatientAlertSource.Add(alertModel);

                Clear();
            }
        }

        private void Clear()
        {
            AlertDescription = null;
            SelectedAlert = null;
            SelectedAlertType = AlertType.FirstOrDefault();
            SelectedSeverity = Severity.FirstOrDefault();
            SelectLocation = null;
            SelectedPriority = null;
            OnsetDate = DateTime.Now;
            ClosureDate = null;
            IsCurrentVisit = false;
            IsAcrossVisit = false;
            SelectPatientAlert = null;
        }

        private void Delete()
        {
            if(SelectPatientAlert != null)
            {
                if (SelectPatientAlert.PatientAlertUID != 0)
                {
                    PatientAlertModel item = new PatientAlertModel();
                    item = SelectPatientAlert;
                    item.StatusFlag = "D";
                    item.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
                    item.CUser = AppUtil.Current.UserID;

                    if (alerttDelete == null)
                        alerttDelete = new List<PatientAlertModel>();

                    alerttDelete.Add(item);
                }

                PatientAlertSource.Remove(SelectPatientAlert);
                Clear();
            }
        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        public void PatientSearch()
        {
            string patientID = string.Empty;
            string firstName = string.Empty; ;
            string lastName = string.Empty;
            if (SearchPatientCriteria.Length >= 3)
            {
                string[] patientName = SearchPatientCriteria.Split(' ');
                if (patientName.Length >= 2)
                {
                    firstName = patientName[0];
                    lastName = patientName[1];
                }
                else
                {
                    int num = 0;
                    foreach (var ch in SearchPatientCriteria)
                    {
                        if (ShareLibrary.CheckValidate.IsNumber(ch.ToString()))
                        {
                            num++;
                        }
                    }
                    if (num >= 5)
                    {
                        patientID = SearchPatientCriteria;
                    }
                    else if (num <= 2)
                    {
                        firstName = SearchPatientCriteria;
                        lastName = "empty";
                    }
                }
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, null, null);
                PatientsSearchSource = searchResult;
            }
            else
            {
                PatientsSearchSource = null;
            }
        }

        #endregion
    }
}
