using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediTech.ViewModels
{
    public class AECheckoutViewModel : MediTechViewModelBase
    {
        #region properties
        private List<LookupReferenceValueModel> _OutcameSource;

        public List<LookupReferenceValueModel> OutcameSource
        {
            get { return _OutcameSource; }
            set { Set(ref _OutcameSource, value); }
        }

        private LookupReferenceValueModel _SelectOutcame;
        public LookupReferenceValueModel SelectOutcame
        {
            get { return _SelectOutcame; }
            set
            {
                Set(ref _SelectOutcame, value);
                if (_SelectOutcame != null)
                {
                    //VisibiltyOP = Visibility.Collapsed;
                    if (SelectOutcame.ValueCode == "TRASOP")
                    {
                        VisibiltyOP = Visibility.Visible;
                        VisibiltyCareprovider = Visibility.Collapsed;
                        //VisibiltyDepartment = Visibility.Collapsed;
                        VisibiltySpeciality = Visibility.Collapsed;
                    }
                    else
                    {
                        VisibiltyOP = Visibility.Collapsed;
                        VisibiltyCareprovider = Visibility.Visible;
                        //VisibiltyDepartment = Visibility.Visible;
                        VisibiltySpeciality = Visibility.Visible;
                    }
                }
            }
        }

        private DateTime _CheckoutDate;
        public DateTime CheckoutDate
        {
            get { return _CheckoutDate; }
            set { Set(ref _CheckoutDate, value); }
        }

        private DateTime _CheckoutTime;
        public DateTime CheckoutTime
        {
            get { return _CheckoutTime; }
            set { Set(ref _CheckoutTime, value); }
        }

        private PatientVisitModel _visitmodel;
        public PatientVisitModel visitmodel
        {
            get { return _visitmodel; }
            set { Set(ref _visitmodel, value); }
        }

        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set { Set(ref _Comment, value); }
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


        private List<CareproviderModel> _CareproviderSource;

        public List<CareproviderModel> CareproviderSource
        {
            get { return _CareproviderSource; }
            set { Set(ref _CareproviderSource, value); }
        }

        private CareproviderModel _SelectCareprovider;
        public CareproviderModel SelectCareprovider
        {
            get { return _SelectCareprovider; }
            set
            {
                Set(ref _SelectCareprovider, value);
            }
        }

        private List<LookupReferenceValueModel> _DepartmentSource;

        public List<LookupReferenceValueModel> DepartmentSource
        {
            get { return _DepartmentSource; }
            set { Set(ref _DepartmentSource, value); }
        }

        private LookupReferenceValueModel _SelectDepartment;
        public LookupReferenceValueModel SelectDepartment
        {
            get { return _SelectDepartment; }
            set
            {
                Set(ref _SelectDepartment, value);
            }
        }

        private List<LookupReferenceValueModel> _DischargeSource;
        public List<LookupReferenceValueModel> DischargeSource
        {
            get { return _DischargeSource; }
            set { Set(ref _DischargeSource, value); }
        }

        private LookupReferenceValueModel _SelectDischarge;
        public LookupReferenceValueModel SelectDischarge
        {
            get { return _SelectDischarge; }
            set
            {
                Set(ref _SelectDischarge, value);
            }
        }

        private List<LookupReferenceValueModel> _DestinationSource;
        public List<LookupReferenceValueModel> DestinationSource
        {
            get { return _DestinationSource; }
            set { Set(ref _DestinationSource, value); }
        }

        private LookupReferenceValueModel _SelectDestination;
        public LookupReferenceValueModel SelectDestination
        {
            get { return _SelectDestination; }
            set
            {
                Set(ref _SelectDestination, value);
                if (_SelectDestination != null && SelectDestination.ValueCode == "DESTIN3")
                {
                    DateTime now = DateTime.Now;
                    DestinationDate = now.Date;
                    DestinationTime = now;
                }
            }
        }

        private List<LookupReferenceValueModel> _AutopsySource;
        public List<LookupReferenceValueModel> AutopsySource
        {
            get { return _AutopsySource; }
            set { Set(ref _AutopsySource, value); }
        }

        private LookupReferenceValueModel _SelectAutopsy;
        public LookupReferenceValueModel SelectAutopsy
        {
            get { return _SelectAutopsy; }
            set
            {
                Set(ref _SelectAutopsy, value);
            }
        }
        

        private ObservableCollection<LookupReferenceValueModel> _AEDischargeOutCome;
        public ObservableCollection<LookupReferenceValueModel> AEDischargeOutCome
        {
            get { return _AEDischargeOutCome; }
            set
            {
                _AEDischargeOutCome = value;
                //OnPropertyChanged("AEDischargeOutCome");
            }
        }



        private DateTime? _DestinationDate;
        public DateTime? DestinationDate
        {
            get { return _DestinationDate; }
            set { Set(ref _DestinationDate, value); }
        }

        private DateTime? _DestinationTime;
        public DateTime? DestinationTime
        {
            get { return _DestinationTime; }
            set { Set(ref _DestinationTime, value); }
        }

        private Visibility _VisibiltyOP = Visibility.Collapsed;

        public Visibility VisibiltyOP
        {
            get { return _VisibiltyOP; }
            set { Set(ref _VisibiltyOP, value); }
        }

        private Visibility _VisibiltySpeciality = Visibility.Collapsed;
        public Visibility VisibiltySpeciality
        {
            get { return _VisibiltySpeciality; }
            set { Set(ref _VisibiltySpeciality, value); }
        }

        private Visibility _VisibiltyCareprovider = Visibility.Collapsed;
        public Visibility VisibiltyCareprovider
        {
            get { return _VisibiltyCareprovider; }
            set { Set(ref _VisibiltyCareprovider, value); }
        }

        //private Visibility _VisibiltyDepartment = Visibility.Collapsed;
        //public Visibility VisibiltyDepartment
        //{
        //    get { return _VisibiltyDepartment; }
        //    set { Set(ref _VisibiltyDepartment, value); }
        //}
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
        public AECheckoutViewModel()
        {
            DateTime now = DateTime.Now;
            CheckoutDate = now.Date;
            CheckoutTime = now;

            List<LookupReferenceValueModel> dataLookupSource = DataService.Technical.GetReferenceValueList("OUTEMER,ENTYP,DSCTYP,DESTIN,ATSTYP,AEDSOCM");
            OutcameSource = dataLookupSource.Where(p => p.DomainCode == "OUTEMER").ToList();
            SelectOutcame = OutcameSource.FirstOrDefault(p => p.ValueCode == "REIPADM");
            //DepartmentSource = dataLookupSource.Where(p => p.DomainCode == "ENTYP").ToList();
            DischargeSource = dataLookupSource.Where(p => p.DomainCode == "DSCTYP").ToList();
            DestinationSource = dataLookupSource.Where(p => p.DomainCode == "DESTIN").ToList();
            AutopsySource = dataLookupSource.Where(p => p.DomainCode == "ATSTYP").ToList();
            var AEDSOCM = dataLookupSource.Where(p => p.DomainCode == "AEDSOCM").ToList();

            AEDischargeOutCome = new ObservableCollection<LookupReferenceValueModel>(AEDSOCM);
            SpecialitySource = DataService.MasterData.GetSpecialityAll();
            CareproviderSource = DataService.UserManage.GetCareproviderDoctor();
        }
        private void Save()
        {
            
            if (SelectOutcame.ValueCode == "TRASOP")
            {
                if (ValidateOPDData())
                {
                    return;
                }

                DateTime now = DateTime.Now;
                AEDischargeEventModel model = new AEDischargeEventModel();
                model.LocationUID = visitmodel.LocationUID ?? 0;
                model.PatientUID = visitmodel.PatientUID;
                model.PatientVisitUID = visitmodel.PatientVisitUID;
                model.PatientAEAdmissionUID = visitmodel.AEAdmissionUID ?? 0;

                model.CheckoutDttm = DateTime.Parse(CheckoutDate.ToString("dd/MM/yyyy") + " " + CheckoutTime.ToString("HH:mm"));
                if (SelectDischarge != null)
                {
                    model.DSCTYPUID = SelectDischarge.Key;

                    if (SelectDischarge.ValueCode == "DSCTYP6" || SelectDischarge.ValueCode == "DSCTYP7") 
                    { 
                        model.DeceasedDttm = DateTime.Parse(DestinationDate?.ToString("dd/MM/yyyy") + " " + DestinationDate?.ToString("HH:mm"));
                    }
                }
                if (SelectDestination != null)
                {
                    model.DESTINUID = SelectDestination.Key;
                }
                string[] DCEvents = AEDischargeOutCome.Where(p => p.IsSelected == true).Select(p => p.Key.ToString()).ToArray();
                string DCEventsID;
                if (DCEvents != null)
                {
                    DCEventsID = String.Join(",", DCEvents);
                    model.DischargeEvents = DCEventsID;
                }

              
                if(SelectAutopsy != null)
                {
                    model.ATSTYPUID = SelectAutopsy.Key;
                }
                model.RecordedBy =  AppUtil.Current.UserID;
                model.Comments = Comment;
                model.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;

                // ถ้าตาย(opd) => edit  patientvisit,
                // insert patientDeceaseddetail,
                // Patient column isDecced,
                // patient AEAdmission IsDaed,
                // save AEDischargeEvent

                model = DataService.PatientIdentity.SaveAEDischargeEvent(model, AppUtil.Current.UserID);
                CloseViewDialog(ActionDialog.Save);
            }
            else
            {
                if (ValidateIpBookingData())
                {
                    return;
                }
                //(ipd) edit  patientvist, insert IPbooking  
                IPBookingModel model = new IPBookingModel();
                model.PatientUID = visitmodel.PatientUID;
                model.PatientVisitUID = visitmodel.PatientVisitUID;
                model.BedUID = visitmodel.BedUID;
                //model.LocationUID = visitmodel.LocationUID ?? 0;
                model.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;

                if (SelectSpeciality != null)
                {
                    model.SpecialityUID = SelectSpeciality.SpecialityUID;
                }

                //if(SelectDepartment != null)
                //{
                //    model.RequestedLocationUID = SelectDepartment.Key ?? 0;
                //}

                if(SelectCareprovider != null)
                {
                    model.CareproviderUID = SelectCareprovider.CareproviderUID;
                }
                model.AdmissionDttm = DateTime.Parse(CheckoutDate.ToString("dd/MM/yyyy") + " " + CheckoutTime.ToString("HH:mm"));
                model.ExpectedDischargeDttm = DateTime.Parse(CheckoutDate.ToString("dd/MM/yyyy") + " " + CheckoutTime.ToString("HH:mm"));
                model.BookedDttm = DateTime.Parse(CheckoutDate.ToString("dd/MM/yyyy") + " " + CheckoutTime.ToString("HH:mm"));
                model.ReferredByUID = AppUtil.Current.UserID;
                model.VISTYUID = DataService.Technical.GetReferenceValueByCode("VISTY", "EMR").Key.Value;
                model.RequestedByLocationUID = AppUtil.Current.LocationUID;
                model.BKTYPUID = DataService.Technical.GetReferenceValueByCode("BKTYP", "REQTD").Key ?? 0;

                model = DataService.PatientIdentity.SaveIPBooking(model, AppUtil.Current.UserID);
                CloseViewDialog(ActionDialog.Save);
            }
        }
        public void AssingAEAdmission(PatientVisitModel visitModel)
        {
            visitmodel = visitModel;
        }
        public bool ValidateOPDData()
        {
            if (CheckoutDate == null)
            {
                WarningDialog("กรุณาเลือก วันที่เช็คเอ้าท์");
                return true;
            }
            if (CheckoutDate == null)
            {
                WarningDialog("กรุณาเลือก เวลาเช็คเอ้าท์");
                return true;
            }

            if (SelectDischarge == null)
            {
                WarningDialog("กรุณาเลือก Discharge Type");
                return true;
            }

            return false;
        }

        public bool ValidateIpBookingData()
        {
            if (CheckoutDate == null)
            {
                WarningDialog("กรุณาเลือก วันที่เช็คเอ้าท์");
                return true;
            }
            if (CheckoutDate == null)
            {
                WarningDialog("กรุณาเลือก เวลาเช็คเอ้าท์");
                return true;
            }
            if (SelectCareprovider == null)
            {
                WarningDialog("กรุณาเลือก แพทย์");
                return true;
            }

            //if (SelectSpeciality == null)
            //{
            //    WarningDialog("กรุณาเลือก Speciality");
            //    return true;
            //}

            //if (SelectDepartment == null)
            //{
            //    WarningDialog("กรุณาเลือก Department");
            //    return true;
            //}

            return false;
        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
           
        }

        #endregion
    }
}
