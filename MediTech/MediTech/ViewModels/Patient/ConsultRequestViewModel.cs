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
    public class ConsultRequestViewModel : MediTechViewModelBase
    {
        #region Properties
        private bool _IsRequest;
        public bool IsRequest
        {
            get { return _IsRequest; }
            set { Set(ref _IsRequest, value); }
        }

        private DateTime? _DateFrom;
        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set { Set(ref _DateFrom, value); }
        }

        private DateTime? _DateTo;
        public DateTime? DateTo
        {
            get { return _DateTo; }
            set { Set(ref _DateTo, value); }
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

        private List<LookupReferenceValueModel> _BookingStatus;

        public List<LookupReferenceValueModel> BookingStatus
        {
            get { return _BookingStatus; }
            set { Set(ref _BookingStatus, value); }
        }

        private LookupReferenceValueModel _SelectBookingStatus;
        public LookupReferenceValueModel SelectBookingStatus
        {
            get { return _SelectBookingStatus; }
            set { Set(ref _SelectBookingStatus, value); }
        }

        private AppointmentRequestModel _SelectAppointmentRequest;
        public AppointmentRequestModel SelectAppointmentRequest
        {
            get { return _SelectAppointmentRequest; }
            set { Set(ref _SelectAppointmentRequest, value);
                if (SelectAppointmentRequest != null)
                {
                    IsRequest = SelectAppointmentRequest.IsCheckin;
                }
            }
        }

        private ObservableCollection<AppointmentRequestModel> _AppointmentRequest;
        public ObservableCollection<AppointmentRequestModel> AppointmentRequest
        {
            get { return _AppointmentRequest; }
            set { Set(ref _AppointmentRequest, value); }
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
        private RelayCommand _CheckinCommand;
        public RelayCommand CheckinCommand
        {
            get { return _CheckinCommand ?? (_CheckinCommand = new RelayCommand(Checkin)); }
        }

        private RelayCommand _DropCommand;
        public RelayCommand DropCommand
        {
            get { return _DropCommand ?? (_DropCommand = new RelayCommand(Drop)); }
        }

        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
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
        #endregion

        #region Method

        public ConsultRequestViewModel()
        {
            DateFrom = DateTime.Today;
            DateTo = DateTime.Today;
            Doctors = DataService.UserManage.GetCareproviderDoctor();
            BookingStatus = DataService.Technical.GetReferenceValueMany("BKSTS");
            var org = GetLocatioinRole(AppUtil.Current.OwnerOrganisationUID);
            Locations = org.Where(p => p.IsRegistrationAllowed == "Y").ToList();
            SelectLocations = Locations.FirstOrDefault(p => p.LocationUID == AppUtil.Current.LocationUID);

        }

        private void Search()
        {
            int? careproviderUID = SelectDoctor != null ? SelectDoctor.CareproviderUID : (int?)null;
            int? locationUID = SelectLocations != null ? SelectLocations.LocationUID : (int?)null;
            int? wnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
            int? bookingStatus = SelectBookingStatus != null ? SelectBookingStatus.Key : (int?)null;
            DateTime dateto = DateTo.Value.AddDays(1);
            AppointmentRequest = new ObservableCollection<AppointmentRequestModel>(DataService.PatientIdentity.SearchAppointmentRequest(DateFrom, dateto, locationUID, bookingStatus, wnerOrganisationUID, careproviderUID));
            AppointmentRequest = new ObservableCollection<AppointmentRequestModel>(AppointmentRequest.OrderBy(p => p.AppointmentDttm).ToList());
        }

        private void Drop()
        {
            if (SelectAppointmentRequest != null)
            {
                if(SelectAppointmentRequest.RequestStatus == "Requested")
                {
                    int status = BookingStatus.FirstOrDefault(p => p.ValueCode == "CANCD").Key ?? 0;
                    DataService.PatientIdentity.ChangeAppointmentRequest(SelectAppointmentRequest.AppointmentRequestUID, status, AppUtil.Current.UserID);
                    SaveSuccessDialog();
                    Search();
                }
                else
                {
                    WarningDialog("สถานะ"+ SelectAppointmentRequest.RequestStatus + "ไม่สามารถทำรายการได้");
                    return;
                }
            }
        }

        private void Checkin()
        {
            if (SelectAppointmentRequest != null)
            {
                //if (SelectAppointmentRequest.RequestStatus == "Requested")
                //{
                    PatientArrived pageview = new PatientArrived();
                    (pageview.DataContext as PatientArrivedViewModel).AssignData(SelectAppointmentRequest);
                    PatientArrivedViewModel result = (PatientArrivedViewModel)LaunchViewDialog(pageview, "PTARRD", true);
                    if (result != null && result.ResultDialog == ActionDialog.Save)
                    {
                        //SelectAppointmentRequest.IsCheckin = true;
                        SaveSuccessDialog();
                        Search();
                    }
                //}
                //else
                //{
                //    WarningDialog("สถานะ" + SelectAppointmentRequest.RequestStatus + "ไม่สามารถทำรายการได้");
                //    return;
                //}
            }
           
        }

        private void Cancel()
        {

        }

        private void Clear()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            SelectBookingStatus = null;
            SelectDoctor = null;
            //SearchPatientCriteria = "";
            //SelectedPateintSearch = null;
        }

        #endregion
    }
}
