using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Reports.Operating.Patient;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
namespace MediTech.ViewModels
{
    public class ManageAppointmentViewModel : MediTechViewModelBase
    {

        #region Properties

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
                    AssingPatientBannner();
                }
            }
        }

        #endregion

        private PatientVisitModel _SelectPatientVisit;

        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set { Set(ref _SelectPatientVisit, value); }
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

        private List<LookupReferenceValueModel> _AppointmentStatus;

        public List<LookupReferenceValueModel> AppointmentStatus
        {
            get { return _AppointmentStatus; }
            set { Set(ref _AppointmentStatus, value); }
        }

        private LookupReferenceValueModel _SelectAppointmentStatus;

        public LookupReferenceValueModel SelectAppointmentStatus
        {
            get { return _SelectAppointmentStatus; }
            set { Set(ref _SelectAppointmentStatus, value); }
        }

        private List<LookupReferenceValueModel> _AppointmentMassage;

        public List<LookupReferenceValueModel> AppointmentMassage
        {
            get { return _AppointmentMassage; }
            set { Set(ref _AppointmentMassage, value); }
        }

        private LookupReferenceValueModel _SelectAppointmentMassage;

        public LookupReferenceValueModel SelectAppointmentMassage
        {
            get { return _SelectAppointmentMassage; }
            set { Set(ref _SelectAppointmentMassage, value); }

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

        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { Set(ref _Comments, value); }
        }

        private bool _IsCheckPrint;

        public bool IsCheckPrint
        {
            get { return _IsCheckPrint; }
            set { Set(ref _IsCheckPrint, value); }
        }

        private Visibility _VisibilityPatientSearch = Visibility.Visible;

        public Visibility VisibilityPatientSearch
        {
            get { return _VisibilityPatientSearch; }
            set { Set(ref _VisibilityPatientSearch, value); }
        }

        #endregion

        #region Command


        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(SaveData)); }
        }

        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }


        private RelayCommand _PatientSearchCommand;

        /// <summary>
        /// Gets the PatientSearchCommand.
        /// </summary>
        public RelayCommand PatientSearchCommand
        {
            get
            {
                return _PatientSearchCommand
                    ?? (_PatientSearchCommand = new RelayCommand(PatientSearch));
            }
        }
        #endregion

        #region Method

        BookingModel model;

        public ManageAppointmentViewModel()
        {
            var refDAta = DataService.Technical.GetReferenceValueList("BKSTS,PATMSG");
            Doctors = DataService.UserManage.GetCareproviderDoctor();
            AppointmentStatus = refDAta.Where(p => p.DomainCode == "BKSTS").ToList();
            AppointmentMassage = refDAta.Where(p => p.DomainCode == "PATMSG").ToList();
            SelectAppointmentStatus = AppointmentStatus.FirstOrDefault(p => p.Key == 2944);
            var org = GetLocatioinRole(AppUtil.Current.OwnerOrganisationUID);
            Locations = org.Where(p => p.IsRegistrationAllowed == "Y").ToList();
            SelectLocations = Locations.FirstOrDefault(p => p.LocationUID == AppUtil.Current.LocationUID);
            AppointmentDate = DateTime.Now;
        }

        private void SaveData()
        {
            try
            {
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

                if (SelectLocations == null)
                {
                    WarningDialog("กรุณาเลือกแผนก");
                    return;
                }

                if (SelectPatientVisit != null || SelectedPateintSearch != null)
                {
                    long patientUID = 0;
                    if (SelectedPateintSearch != null)
                    {
                        patientUID = SelectedPateintSearch.PatientUID;
                    }
                    else if (SelectPatientVisit != null)
                    {
                        patientUID = SelectPatientVisit.PatientUID;
                    }

                    AssingPropertiesToModel(patientUID);
                    BookingModel bookingModel = DataService.PatientIdentity.ManageBooking(model);

                    if (IsCheckPrint)
                    {
                        AppointmentCard rpt = new AppointmentCard();
                        ReportPrintTool printTool = new ReportPrintTool(rpt);

                        rpt.Parameters["BookUID"].Value = bookingModel.BookingUID;
                        rpt.RequestParameters = false;
                        rpt.ShowPrintMarginsWarning = false;
                        printTool.ShowPreviewDialog();
                    }

                    CloseViewDialog(ActionDialog.Save);
                }
                else
                {
                    WarningDialog("กรุณาเลือกคนไข้ทำนัด");
                    return;
                }

            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, "", "");
                PatientsSearchSource = searchResult;
            }
            else
            {
                PatientsSearchSource = null;
            }

        }

        public void AssingPatientBannner()
        {
            (this.View as ManageAppointment).patientBanner.SetPatientBanner(SelectedPateintSearch.PatientUID);
        }

        public void AssingPatientVisit(PatientVisitModel visitModel)
        {
            SelectPatientVisit = visitModel;
            VisibilityPatientSearch = Visibility.Collapsed;

            SelectLocations = Locations.FirstOrDefault(p => p.LocationUID == SelectPatientVisit.LocationUID);
        }

        public void AssignModel(BookingModel model)
        {
            this.model = model;
            AssignModelToProperties();

        }

        public void AssignModelToProperties()
        {
            if (model != null)
            {
                AppointmentDate = model.AppointmentDttm;
                AppointmentTime = model.AppointmentDttm;
                Comments = model.Comments;
                if (AppointmentMassage != null && model.PATMSGUID != null)
                {
                    SelectAppointmentMassage = AppointmentMassage.FirstOrDefault(p => p.Key == model.PATMSGUID);
                }
                if (AppointmentStatus != null && model.BKSTSUID != 0)
                {
                    SelectAppointmentStatus = AppointmentStatus.FirstOrDefault(p => p.Key == model.BKSTSUID);
                }
                if (Doctors != null && model.CareProviderUID != null)
                {
                    SelectDoctor = Doctors.FirstOrDefault(p => p.CareproviderUID == model.CareProviderUID);
                }
                if(model.LocationUID != 0)
                {
                    SelectLocations = Locations.FirstOrDefault(p => p.LocationUID == model.LocationUID);
                }
               
            }
        }
        public void AssingPropertiesToModel(long patientUID)
        {
            if (model == null)
            {
                model = new BookingModel();
            }
            model.AppointmentDttm = AppointmentDate.Add(AppointmentTime.Value.TimeOfDay);
            model.BKSTSUID = SelectAppointmentStatus != null ? SelectAppointmentStatus.Key.Value : 0;
            model.PatientUID = patientUID;
            model.PATMSGUID = SelectAppointmentMassage != null ? SelectAppointmentMassage.Key : (int?)null;
            model.CareProviderUID = SelectDoctor != null ? SelectDoctor.CareproviderUID : (int?)null;
            model.Comments = Comments;
            model.CUser = AppUtil.Current.UserID;
            model.MUser = AppUtil.Current.UserID;
            model.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
            model.LocationUID = SelectLocations.LocationUID;
        }

        #endregion
    }
}
