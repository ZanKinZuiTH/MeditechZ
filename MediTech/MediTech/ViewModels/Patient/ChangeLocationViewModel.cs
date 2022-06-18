using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ChangeLocationViewModel : MediTechViewModelBase
    {
        #region Properties

        private List<PatientVisitModel> _PatientVisitData;
        public List<PatientVisitModel> PatientVisitData
        {
            get { return _PatientVisitData; }
            set { Set(ref _PatientVisitData, value); }
        }

        private PatientVisitModel _SelectPatientVisit;
        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set { Set(ref _SelectPatientVisit, value); 
            if(SelectPatientVisit != null)
                {
                    SelectLocations = null;
                    CurrentLocation = SelectPatientVisit.LocationName;
                }
            }
        }

        private string _VisitID;
        public string VisitID
        {
            get { return _VisitID; }
            set { Set(ref _VisitID, value); }
        }
        
        private string _CurrentLocation;
        public string CurrentLocation
        {
            get { return _CurrentLocation; }
            set { Set(ref _CurrentLocation, value); }
        }

        private List<LocationModel> _Locations;
        public List<LocationModel> Locations
        {
            get { return _Locations; }
            set { Set(ref _Locations, value); }
        }

        private LocationModel _SelectLocations;
        public LocationModel SelectLocations
        {
            get { return _SelectLocations; }
            set { Set(ref _SelectLocations, value); }
        }

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
                //if (_SelectedPateintSearch != null)
                //{
                //    AssignToModel(SelectedPateintSearch);
                //}
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

        private RelayCommand _SearchCommand;
        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(Search)); }
        }

        private RelayCommand _ClearCommand;
        public RelayCommand ClearCommand
        {
            get { return _ClearCommand ?? (_ClearCommand = new RelayCommand(Clear)); }
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
        #endregion

        #region Method
        public ChangeLocationViewModel()
        {
            var org = GetLocatioinRole(AppUtil.Current.OwnerOrganisationUID);
            Locations = org.Where(p => p.IsRegistrationAllowed == "Y").ToList();
        }

        public void PatientSearch()
        {
            string patientID = string.Empty;
            string firstName = string.Empty;
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatientEmergency(patientID, firstName, "", lastName, "", null, null, "", null, "");
                PatientsSearchSource = searchResult;
            }
            else
            {
                PatientsSearchSource = null;
            }

        }

        private void Search()
        {
            string visitID = VisitID;
            long? patientUID = SelectedPateintSearch != null ? SelectedPateintSearch.PatientUID : (long?)null;

            PatientVisitData = DataService.PatientIdentity.GetPatientVisitToChangeLocation(patientUID, visitID);
        }
        private void Clear()
        {
            SelectedPateintSearch = null;
            SelectLocations = null;
            PatientVisitData = null;
            CurrentLocation = null;
        }

        private void Save()
        {
            if(SelectLocations.Name == CurrentLocation)
            {
                WarningDialog("แผนกซ้ำ กรุณาเลือกแผนกใหม่");
                return;
            }

            if (SelectPatientVisit == null && SelectLocations == null)
            {
                WarningDialog("ไม่มีข้อมูลสำหรับทำรายการ");
                return;
            }
            long patientVisitUID = SelectPatientVisit.PatientVisitUID;
            int locatinUID = SelectLocations.LocationUID;

            DataService.PatientIdentity.ChangeVisitLocation(patientVisitUID, locatinUID, AppUtil.Current.UserID);
            SaveSuccessDialog();
            SelectLocations = null;
            SelectPatientVisit = null;
            CurrentLocation = null;
            Search();
        }
        private void Cancel()
        {
            
        }

        #endregion
    }
}
