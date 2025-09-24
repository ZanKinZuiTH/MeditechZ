using GalaSoft.MvvmLight.Command;
using MediTech.Interface;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class SendConsultViewModel : MediTechViewModelBase,IPatientVisitViewModel
    {
        #region Properties

        #region Varible
        public PatientVisitModel patientVisitModel;
        public AppointmentRequestModel requestDetail { get; set; }
        public List<AppointmentRequestModel> requestDelete;

        #endregion

        private PatientVisitModel _SelectedPatientVisit;
        public PatientVisitModel SelectedPatientVisit
        {
            get { return _SelectedPatientVisit; }
            set { Set(ref _SelectedPatientVisit, value); }
        }

        private DateTime _AppointmentDate;
        public DateTime AppointmentDate
        {
            get { return _AppointmentDate; }
            set { Set(ref _AppointmentDate, value); }
        }

        private DateTime? _AppointmentTime;
        public DateTime? AppointmentTime
        {
            get { return _AppointmentTime; }
            set { Set(ref _AppointmentTime, value); }
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
        
        private string _Comments;
        public string Comments
        {
            get { return _Comments; }
            set { Set(ref _Comments, value); }
        }
        private ObservableCollection<AppointmentRequestModel> _AppointmentRequest;
        public ObservableCollection<AppointmentRequestModel> AppointmentRequest
        {
            get { return _AppointmentRequest; }
            set { Set(ref _AppointmentRequest, value); }
        }

        private AppointmentRequestModel _SelectAppointmentRequest;
        public AppointmentRequestModel SelectAppointmentRequest
        {
            get { return _SelectAppointmentRequest; }
            set { Set(ref _SelectAppointmentRequest, value); }
        }

        private LookupReferenceValueModel _BookingStatus;
        public LookupReferenceValueModel BookingStatus
        {
            get { return _BookingStatus; }
            set { Set(ref _BookingStatus, value); }
        }


        #endregion

        #region Command
        private RelayCommand _AddCommand;

        public RelayCommand AddCommand
        {
            get { return _AddCommand ?? (_AddCommand = new RelayCommand(Add)); }
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

        public SendConsultViewModel()
        {
            BookingStatus = DataService.Technical.GetReferenceValueByCode("BKSTS", "REQTED");
            Doctors = DataService.UserManage.GetCareproviderDoctor();
            var org = GetLocatioinRole(AppUtil.Current.OwnerOrganisationUID);
            Locations = org.Where(p => p.IsRegistrationAllowed == "Y").ToList();
            AppointmentDate = DateTime.Now;
            AppointmentTime = DateTime.Now;
        }
        private void Add()
        {
            if (AppointmentRequest.Count > 0)
            {
                WarningDialog("มีรายการร้องขอแล้ว ไม่สามารถทำรายการได้อีก");
                return;
            }

            
            if (SelectLocations == null)
            {
                WarningDialog("กรุณาเลือกแผนก");
                return;
            }
            
            if (SelectLocations.LocationUID == SelectedPatientVisit.LocationUID)
            {
                WarningDialog("ปัจุบันผู้ป่วยอยู่แผนก "+ SelectLocations.Name + "\r\nกรุณาเลือกแผนกใหม่");
                return;
            }

            if (AppointmentDate == DateTime.MinValue)
            {
                WarningDialog("กรุณาเลือกวันที่ทำนัด");
                return;
            }

            if (AppointmentTime == null)
            {
                WarningDialog("กรุณาเลือกเวลาทำนัด");
                return;
            }

            //var time = AppointmentTime?.TimeOfDay;
            //if (AppointmentDate.Date == DateTime.Now.Date && time <= DateTime.Now.TimeOfDay)
            //{
            //    WarningDialog("กรุณาเลือกวันเวลาทำนัดใหม่");
            //    return;
            // }
            
            AssignToGrid();

            if(requestDetail != null)
            {
                AppointmentRequest.Add(requestDetail);
            }
        }


        private void AssignToGrid()
        {
            requestDetail = new AppointmentRequestModel();
            var time = AppointmentTime.Value.TimeOfDay;

            requestDetail.AppointmentDttm = AppointmentDate;
            requestDetail.AppointmentDttm.Date.Add(AppointmentTime.Value.TimeOfDay);
            requestDetail.BKSTSUID = BookingStatus.Key ?? 0;
            requestDetail.RequestStatus = BookingStatus.Display;
            requestDetail.PatientUID = patientVisitModel.PatientUID;
            requestDetail.PatientVisitUID = patientVisitModel.PatientVisitUID;
            requestDetail.CareProviderUID = SelectDoctor != null ? SelectDoctor.CareproviderUID : (int?)null;
            requestDetail.CareProviderName = SelectDoctor != null ? Doctors.FirstOrDefault(p => p.CareproviderUID == SelectDoctor.CareproviderUID).FullName : null;
            requestDetail.Comments = Comments;
            requestDetail.CUser = AppUtil.Current.UserID;
            requestDetail.MUser = AppUtil.Current.UserID;
            requestDetail.StatusFlag = "A";
            requestDetail.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
            requestDetail.LocationUID = SelectLocations != null ? SelectLocations.LocationUID : 0;
            requestDetail.LocationName = Locations.FirstOrDefault(p => p.LocationUID == SelectLocations.LocationUID).Name;

        }

        private void Clear()
        {
            SelectDoctor = null;
            SelectLocations = null;
            AppointmentDate = DateTime.Now;
            Comments = null;
        }

        private void Delete()
        {
            if (SelectAppointmentRequest != null)
            {
                if (SelectAppointmentRequest.AppointmentRequestUID != 0)
                {
                    AppointmentRequestModel item = new AppointmentRequestModel();
                    item = SelectAppointmentRequest;
                    item.StatusFlag = "D";
                    item.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
                    item.CUser = AppUtil.Current.UserID;

                    if (requestDelete == null)
                        requestDelete = new List<AppointmentRequestModel>();

                    requestDelete.Add(item);
                }
                AppointmentRequest.Remove(SelectAppointmentRequest);
                Clear();
            }
        }
    

        private void Save()
        {
            if(AppointmentRequest.Count <= 0 && requestDelete.Count <= 0)
            {
                WarningDialog("กรุณาเพิ่มรายการร้องขอ");
                return;
            }
           
            if(AppointmentRequest.Count > 0 || requestDelete.Count > 0)
            {
                var data = AssignToModel();
                if (requestDelete != null)
                {
                    data.AddRange(requestDelete);
                }

                DataService.PatientIdentity.ManageAppointmentRequest(data, AppUtil.Current.UserID);
                CloseViewDialog(ActionDialog.Save);
            }
        }

        private List<AppointmentRequestModel> AssignToModel()
        {
            
                List<AppointmentRequestModel> returnData = new List<AppointmentRequestModel>();

                if (AppointmentRequest != null)
                {
                    foreach (var item in AppointmentRequest)
                    {
                        //PatientInsuranceDetailModel data = new PatientInsuranceDetailModel();
                        //data = item;
                        item.StatusFlag = "A";
                        item.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
                        item.RequestedBy = AppUtil.Current.UserID;
                        item.RequestedDate = DateTime.Now;
                        returnData.Add(item);
                    }
                }
                return returnData;
            
        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        public void AssignPatientVisit(PatientVisitModel patVisitData)
        {
            SelectedPatientVisit = patVisitData;
            patientVisitModel = patVisitData;
            int patientUID = Convert.ToInt32(patVisitData.PatientUID);
            int patientVisitUID = Convert.ToInt32(patVisitData.PatientVisitUID);
            //int bkstsUID = DataService.Technical.GetReferenceValueByCode("VISTS", "REGST").Key ?? 0;

            var requestData = DataService.PatientIdentity.GetAppointmentRequestbyUID(patientUID, patientVisitUID);
            AppointmentRequest = new ObservableCollection<AppointmentRequestModel>(requestData);
        }

        #endregion
    }
}
