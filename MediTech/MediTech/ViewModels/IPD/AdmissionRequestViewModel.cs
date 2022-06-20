using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class AdmissionRequestViewModel : MediTechViewModelBase
    {
        #region Properties

        public IPBookingModel iPBooking;

        private PatientVisitModel _SelectPatientVisit;
        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set { Set(ref _SelectPatientVisit, value); }
        }

        private List<LocationModel> _ListWard;
        public List<LocationModel> ListWard 
        {
            get { return _ListWard; }
            set { Set(ref _ListWard, value); }
        }

        private LocationModel _SelectedListWard;
        public LocationModel SelectedListWard
        {
            get { return _SelectedListWard; }
            set { Set(ref _SelectedListWard, value); }
        }
        public List<LocationModel> WardSource { get; set; }

        private LocationModel _SelectedWard;
        public LocationModel SelectedWard
        {
            get { return _SelectedWard; }
            set { Set(ref _SelectedWard, value);
                if (_SelectedWard != null)
                {
                    ListWard = DataService.PatientIdentity.GetBedLocation(SelectedWard.LocationUID, null).Where(p => p.BedIsUse == "N").ToList();
                }
            }
        }

        public List<LocationModel> LocationDepartment { get; set; }

        private LocationModel _SelectedLocationDepartment;
        public LocationModel SelectedLocationDepartment
        {
            get { return _SelectedLocationDepartment; }
            set { Set(ref _SelectedLocationDepartment, value); }
        }


        private DateTime? _ExpectedAdmission;
        public DateTime? ExpectedAdmission
        {
            get { return _ExpectedAdmission; }
            set
            {
                Set(ref _ExpectedAdmission, value);
                
            }
        }

        private DateTime? _DischargeDate;
        public DateTime? DischargeDate
        {
            get { return _DischargeDate; }
            set
            {
                Set(ref _DischargeDate, value);
                if(DischargeDate != null)
                {
                    //if(ExpectedAdmission != null)
                    //{
                    //    int length = (DischargeDate.Value.Date - ExpectedAdmission.Value.Date).Days;
                    //    LenghtofDay = length.ToString();
                    //}
                }

            }
        }

        public bool Changed { get; set; }

        private string _LenghtofDay;
        public string LenghtofDay
        {
            get { return _LenghtofDay; }
            set { Set(ref _LenghtofDay, value);

                double OutVal;
                double.TryParse(LenghtofDay, out OutVal);
                DischargeDate = ExpectedAdmission?.AddDays(OutVal);

                //if (LenghtofDay != null && ExpectedAdmission != null) {
                //    if (DischargeDate == null)
                //    {
                //        double OutVal;
                //        double.TryParse(LenghtofDay, out OutVal);
                //        DischargeDate = ExpectedAdmission?.AddDays(OutVal);
                //    }
                //}
            }
        }

        public List<CareproviderModel> AdmissionCareprovider { get; set; }

        private CareproviderModel _SelectAdmissionCareprovider;
        public CareproviderModel SelectAdmissionCareprovider
        {
            get { return _SelectAdmissionCareprovider; }
            set { Set(ref _SelectAdmissionCareprovider, value); }
        }

        public List<CareproviderModel> RequestedDoctor { get; set; }

        private CareproviderModel _SelectRequestedDoctor;
        public CareproviderModel SelectRequestedDoctor
        {
            get { return _SelectRequestedDoctor; }
            set { Set(ref _SelectRequestedDoctor, value); }
        }

        private List<SpecialityModel> _SpecialitySource;
        public List<SpecialityModel> SpecialitySource
        {
            get { return _SpecialitySource; }
            set { Set(ref _SpecialitySource, value); }
        }

        private SpecialityModel _SelectSpeciality;
        public SpecialityModel SelectSpeciality
        {
            get { return _SelectSpeciality; }
            set
            {
                Set(ref _SelectSpeciality, value);
            }
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
        public AdmissionRequestViewModel()
        {
            ExpectedAdmission = DateTime.Now;
            WardSource = DataService.Technical.GetLocationByTypeUID(3152);
            AdmissionCareprovider = DataService.UserManage.GetCareproviderAll();
            SelectAdmissionCareprovider = AdmissionCareprovider.Find(p => p.CareproviderUID == AppUtil.Current.UserID);
            SpecialitySource = DataService.MasterData.GetSpecialityAll();
            RequestedDoctor = DataService.UserManage.GetCareproviderDoctor();
            LocationDepartment = DataService.Technical.GetLocationByTypeUID(3159);
            LenghtofDay = "1";
        }

        private void Save()
        {
            try
            {
                if(ExpectedAdmission == null)
                {
                    WarningDialog("กรุณาใส่ Expected Admission");
                    return;
                }

                if (!String.IsNullOrEmpty(LenghtofDay))
                {
                    var isNum = IsDigitsOnly(LenghtofDay);

                    if (isNum == false)
                    {
                        WarningDialog("กรุณาใส่ Lenght of Stay เป็นตัวเลขเท่านั้น");
                        return;
                    }
                }

                if(SelectRequestedDoctor == null)
                {
                    WarningDialog("กรุณาใส่ Requested Docto");
                    return;
                }

                if(SelectedLocationDepartment == null)
                {
                    WarningDialog("กรุณาใส่ Requested Location");
                    return;
                }

                //AssingPropertiesToModel();
                AssingPropertiesToModel();
                DataService.PatientIdentity.SaveIPBooking(iPBooking, AppUtil.Current.UserID);
                //SaveSuccessDialog();

                CloseViewDialog(ActionDialog.Save);
                //ListLocation pageList = new ListLocation();
                //ChangeViewPermission(pageList);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        public void AssingModel(PatientVisitModel patientVisit)
        {
            iPBooking = new IPBookingModel();
            iPBooking.PatientUID = patientVisit.PatientUID;
            iPBooking.PatientVisitUID = patientVisit.PatientVisitUID;
            iPBooking.VISTYUID = patientVisit.VISTYUID ?? 0;
            //AssingModelProperties();
            SelectPatientVisit = patientVisit;
        }

        public void AssingPropertiesToModel()
        {
            DateTime date = DateTime.Now;

            if (iPBooking == null)
            {
                iPBooking = new IPBookingModel();
            }

            iPBooking.BedUID = SelectedListWard != null ? SelectedListWard.LocationUID : (int?)null;
            iPBooking.AdmissionDttm = (DateTime)(ExpectedAdmission != null ? ExpectedAdmission : date);
            iPBooking.ExpectedDischargeDttm = (DateTime)(DischargeDate != null ? DischargeDate : date);
            iPBooking.ExpectedLengthofStay = LenghtofDay != null ? Int32.Parse(LenghtofDay) : (int?)null;
            iPBooking.CareproviderUID = SelectRequestedDoctor.CareproviderUID;
            iPBooking.SpecialityUID = SelectSpeciality != null ? SelectSpeciality.SpecialityUID : (int?)null;
            iPBooking.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
            iPBooking.LocationUID = SelectedLocationDepartment.LocationUID;
            iPBooking.RequestedByUID = AppUtil.Current.UserID;
            iPBooking.RequestedByLocationUID = AppUtil.Current.OwnerOrganisationUID;
        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }

        #endregion
    }
}
