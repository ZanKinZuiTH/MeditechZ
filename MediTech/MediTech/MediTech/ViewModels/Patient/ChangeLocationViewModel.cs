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

        private DateTime? _DateFrom;
        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set
            {
                Set(ref _DateFrom, value);
            }
        }


        private DateTime? _DateTo;
        public DateTime? DateTo
        {
            get { return _DateTo; }
            set
            {
                Set(ref _DateTo, value);
            }
        }

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
        private string _PatientID;
        public string PatientID
        {
            get { return _PatientID; }
            set { Set(ref _PatientID, value); }
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
      
        #endregion

        #region Command

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
            DateFrom = DateTime.Now;
            var org = GetLocatioinRole(AppUtil.Current.OwnerOrganisationUID);
            Locations = org.Where(p => p.IsRegistrationAllowed == "Y").ToList();
        }

        private void Search()
        {
            string visitID = VisitID;
            long? patientUID = (PatientID != "" && PatientID != null) ? long.Parse(PatientID) : (long?)null;

            PatientVisitData = DataService.PatientIdentity.GetPatientVisitToChangeLocation(patientUID, visitID, DateFrom, DateTo);
        }
        private void Clear()
        {
            SelectLocations = null;
            PatientVisitData = null;
            CurrentLocation = null;
            DateFrom = DateTime.Now;
            DateTo = null;
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
