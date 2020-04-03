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
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class ListAppointmentViewModel : MediTechViewModelBase
    {

        #region Properties

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
            }
        }
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

        private List<BookingModel> _BookingSource;

        public List<BookingModel> BookingSource
        {
            get { return _BookingSource; }
            set { Set(ref _BookingSource, value); }
        }

        private BookingModel _SelectBooking;

        public BookingModel SelectBooking
        {
            get { return _SelectBooking; }
            set
            {
                Set(ref _SelectBooking, value);
                if (SelectBooking != null)
                {
                    switch (SelectBooking.BKSTSUID)
                    {
                        case 2943:
                            IsEnableCancel = false;
                            IsEnableEdit = false;
                            IsEnableRegister = false;
                            break;
                        case 2945:
                            IsEnableCancel = false;
                            IsEnableEdit = false;
                            IsEnableRegister = false;
                            break;
                        case 2944:
                            IsEnableCancel = true;
                            IsEnableEdit = true;
                            IsEnableRegister = true;
                            break;
                    }
                }
            }
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
            set { Set(ref _SelectOrganisation, value); }
        }

        private bool _IsEnableRegister = false;

        public bool IsEnableRegister
        {
            get { return _IsEnableRegister; }
            set { Set(ref _IsEnableRegister, value); }
        }

        private bool _IsEnableEdit = false;

        public bool IsEnableEdit
        {
            get { return _IsEnableEdit; }
            set { Set(ref _IsEnableEdit, value); }
        }

        private bool _IsEnableCancel = false;

        public bool IsEnableCancel
        {
            get { return _IsEnableCancel; }
            set { Set(ref _IsEnableCancel, value); }
        }

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

        private RelayCommand _RegisterCommand;

        public RelayCommand RegisterCommand
        {
            get { return _RegisterCommand ?? (_RegisterCommand = new RelayCommand(Register)); }
        }

        private RelayCommand _AddCommand;

        public RelayCommand AddCommand
        {
            get { return _AddCommand ?? (_AddCommand = new RelayCommand(AddBooking)); }
        }

        private RelayCommand _EditCommand;

        public RelayCommand EditCommand
        {
            get { return _EditCommand ?? (_EditCommand = new RelayCommand(EditBooking)); }
        }

        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(CancelBooking)); }
        }

        private RelayCommand _PrintCommand;

        public RelayCommand PrintCommand
        {
            get { return _PrintCommand ?? (_PrintCommand = new RelayCommand(Print)); }
        }


        #endregion

        #region Method
        public ListAppointmentViewModel()
        {
            var refDAta = DataService.Technical.GetReferenceValueList("BKSTS,PATMSG");
            DateFrom = DateTime.Now;
            Doctors = DataService.UserManage.GetCareproviderDoctor();
            BookingStatus = refDAta.Where(p => p.DomainCode == "BKSTS").ToList();
            AppointmentMassage = refDAta.Where(p => p.DomainCode == "PATMSG").ToList();
            SelectBookingStatus = BookingStatus.FirstOrDefault(p => p.Key == 2944);

            Organisations = GetHealthOrganisationRoleMedical();
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);

            Search();
        }


        private void Search()
        {
            int? careproviderUID = SelectDoctor != null ? SelectDoctor.CareproviderUID : (int?)null;
            long? patientUID = null;
            int? healthOrganisationUID = null;
            if (!string.IsNullOrEmpty(SearchPatientCriteria) && SelectedPateintSearch != null)
            {
                patientUID = SelectedPateintSearch.PatientUID;
            }
            int? bookingStatus = SelectBookingStatus != null ? SelectBookingStatus.Key : (int?)null;
            int? appointmentMessage = SelectAppointmentMassage != null ? SelectAppointmentMassage.Key : (int?)null;
            if (SelectOrganisation != null)
            {
                healthOrganisationUID = SelectOrganisation.HealthOrganisationUID;
            }

            BookingSource = DataService.PatientIdentity.SearchBooking(DateFrom, DateTo, careproviderUID, patientUID, bookingStatus, appointmentMessage, healthOrganisationUID);
            BookingSource = BookingSource.OrderBy(p => p.AppointmentDttm).ToList();
        }

        private void Clear()
        {
            DateFrom = DateTime.Now;
            DateTo = null;
            SelectBookingStatus = null;
            SelectDoctor = null;
            SearchPatientCriteria = "";
            SelectedPateintSearch = null;
        }


        private void Register()
        {
            if (SelectBooking != null)
            {
                var patient = DataService.PatientIdentity.GetPatientByUID(SelectBooking.PatientUID);
                //MediTech.Models.RegisterPatientMassage message = new Models.RegisterPatientMassage();

                RegisterPatient registerPage = new RegisterPatient();
                RegisterPatientViewModel viewModel = (registerPage.DataContext as RegisterPatientViewModel);
                
                viewModel.IsManageRegister = true;
                ChangeViewPermission(registerPage, this.View);
                viewModel.OpenPage(PageRegister.Manage, patient, SelectBooking);

            }
        }


        private void AddBooking()
        {
            ManageAppointment pageview = new ManageAppointment();
            ManageAppointmentViewModel result = (ManageAppointmentViewModel)LaunchViewDialog(pageview, "MAPOME", true);
            if (result != null && result.ResultDialog == ActionDialog.Save)
            {
                SaveSuccessDialog();
                Search();
            }
        }
        private void EditBooking()
        {
            if (SelectBooking != null)
            {
                var editData = DataService.PatientIdentity.GetBookingByUID(SelectBooking.BookingUID);
                PatientVisitModel visit = new PatientVisitModel();
                visit.PatientUID = SelectBooking.PatientUID;
                visit.CareProviderName = SelectBooking.CareProviderName;
                ManageAppointment pageview = new ManageAppointment();
                ManageAppointmentViewModel viewModel = (pageview.DataContext as ManageAppointmentViewModel);
                viewModel.AssingPatientVisit(visit);
                viewModel.AssignModel(editData);
                ManageAppointmentViewModel result = (ManageAppointmentViewModel)LaunchViewDialog(pageview,"MAPOME", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    Search();
                }
            }

        }

        private void CancelBooking()
        {
            if (SelectBooking != null)
            {
                DialogResult result = QuestionDialog("คุณต้องการยกเลิกนัดนี้ ใช้หรือไม่");
                if (result == DialogResult.Yes)
                {
                    DataService.PatientIdentity.CancelBooking(SelectBooking.BookingUID, AppUtil.Current.UserID);
                    SelectBooking.BKSTSUID = 2945;
                    SelectBooking.BookingStatus = BookingStatus.FirstOrDefault(p => p.Key == 2945).Display;
                    SaveSuccessDialog();
                    OnUpdateEvent();
                }

            }

        }


        private void Print()
        {
            if (SelectBooking != null)
            {
                AppointmentCard rpt = new AppointmentCard();
                ReportPrintTool printTool = new ReportPrintTool(rpt);

                rpt.Parameters["BookUID"].Value = SelectBooking.BookingUID;
                rpt.RequestParameters = false;
                rpt.ShowPrintMarginsWarning = false;
                printTool.ShowPreviewDialog();
            }
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null);
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
