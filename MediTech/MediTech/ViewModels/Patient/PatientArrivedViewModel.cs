using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class PatientArrivedViewModel : MediTechViewModelBase
    {
        #region Properties

        public AppointmentRequestModel appointmentRequest;
        public PatientVisitCareproviderModel patientVisitCareprovider;

        private List<LookupReferenceValueModel> _StatusSource;
        public List<LookupReferenceValueModel> StatusSource
        {
            get { return _StatusSource; }
            set { Set(ref _StatusSource, value); }
        }

        private LookupReferenceValueModel _SelectStatus;
        public LookupReferenceValueModel SelectStatus
        {
            get { return _SelectStatus; }
            set { Set(ref _SelectStatus, value); }
        }

        private DateTime? _StartTime;
        public DateTime? StartTime
        {
            get { return _StartTime; }
            set { Set(ref _StartTime, value); }
        }

        private string _PatientName;
        public string PatientName
        {
            get { return _PatientName; }
            set { Set(ref _PatientName, value); }
        }

        private List<CareproviderModel> _Doctors;
        public List<CareproviderModel> Doctors
        {
            get { return _Doctors; }
            set { Set(ref _Doctors, value); }
        }

        private CareproviderModel _SelectDoctor;
        public CareproviderModel SelectDoctor
        {
            get { return _SelectDoctor; }
            set { Set(ref _SelectDoctor, value); }
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
        public PatientArrivedViewModel()
        {
            StatusSource = DataService.Technical.GetReferenceValueMany("VISTS");

            StatusSource = StatusSource.Where(p => p.ValueCode == "REGST" || p.ValueCode == "ARRVD" 
            || p.ValueCode == "ARRWI" || p.ValueCode == "SNDDOC" || p.ValueCode == "AWRES" || p.ValueCode == "RFLSTS").ToList();

            SelectStatus = StatusSource.FirstOrDefault(p => p.ValueCode == "ARRVD");
            Doctors = DataService.UserManage.GetCareproviderDoctor();
            Doctors = Doctors.Where(p => p.IsDoctor == true).ToList();
            var org = GetLocatioinRole(AppUtil.Current.OwnerOrganisationUID);
            Locations = org.Where(p => p.IsRegistrationAllowed == "Y").ToList();
            StartTime = DateTime.Now;
        }

        public void AssignData(AppointmentRequestModel model)
        {
            PatientName = model.PatientName;
            appointmentRequest = model;
            
            SelectDoctor = Doctors.FirstOrDefault(p => p.CareproviderUID == model.CareProviderUID);
            SelectLocations = Locations.FirstOrDefault(p => p.LocationUID == model.LocationUID);
        }

        private void Save()
        {
            if (SelectStatus != null)
            {
                if (SelectStatus.ValueCode == "SNDDOC" && SelectDoctor == null)
                {
                    WarningDialog("กรุณาเลือกแพทย์");
                    return;
                }

                AssignToModel();
                DataService.PatientIdentity.SavePatientVisitCareprovider(patientVisitCareprovider, appointmentRequest.AppointmentRequestUID);
                CloseViewDialog(ActionDialog.Save);
            }
        }

        private void AssignToModel()
        {
            patientVisitCareprovider = new PatientVisitCareproviderModel();

            patientVisitCareprovider.PatientVisitUID = appointmentRequest.PatientVisitUID;
            patientVisitCareprovider.StartDttm = StartTime;
            patientVisitCareprovider.CareProviderUID = SelectDoctor != null ? SelectDoctor.CareproviderUID : 0;
            patientVisitCareprovider.CareProviderName = SelectDoctor != null ? SelectDoctor.FullName : null;
            patientVisitCareprovider.PACLSUID = SelectStatus.Key;
            patientVisitCareprovider.LocationUID = SelectLocations.LocationUID;
            patientVisitCareprovider.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
            patientVisitCareprovider.CUser = AppUtil.Current.UserID;
        }
        

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        #endregion
    }
}
