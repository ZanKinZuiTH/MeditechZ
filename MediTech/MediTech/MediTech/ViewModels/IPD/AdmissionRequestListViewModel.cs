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
    public class AdmissionRequestListViewModel : MediTechViewModelBase
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
        
        private List<PatientVisitModel> _PatientSerach;
        public List<PatientVisitModel> PatientSerach
        {
            get { return _PatientSerach; }
            set { Set(ref _PatientSerach, value); }
        }

        private List<IPBookingModel> _IPBookingSource;
        public List<IPBookingModel> IPBookingSource
        {
            get { return _IPBookingSource; }
            set { Set(ref _IPBookingSource, value); }
        }

        private IPBookingModel _SelectedIPBooking;
        public IPBookingModel SelectedIPBooking
        {
            get { return _SelectedIPBooking; }
            set
            {
                _SelectedIPBooking = value;
                if(SelectedIPBooking != null)
                {
                    IsRequest = SelectedIPBooking.BedBookingRequest == "Requested" ? true : false;
                }
            }
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
                if (SelectedPateintSearch != null)
                {
                    Search();
                    SearchPatientCriteria = string.Empty;
                }
            }
        }

        public List<LookupReferenceValueModel> StatusSource { get; set; }

        private LookupReferenceValueModel _SelectedStatus;
        public LookupReferenceValueModel SelectedStatus
        {
            get { return _SelectedStatus; }
            set { Set(ref _SelectedStatus, value); }
        }

        public List<LocationModel> WardSource { get; set; }

        private LocationModel _SelectedWard;
        public LocationModel SelectedWard
        {
            get { return _SelectedWard; }
            set { Set(ref _SelectedWard, value); }
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
            get
            {
                return _SearchCommand ?? (_SearchCommand = new RelayCommand(Search));
            }
        }

        private RelayCommand _ClearCommand;
        public RelayCommand ClearCommand
        {
            get
            {
                return _ClearCommand ?? (_ClearCommand = new RelayCommand(Clear));
            }
        }


        private RelayCommand _PrintDocumentCommand;
        public RelayCommand PrintDocumentCommand
        {
            get
            {
                return _PrintDocumentCommand ?? (_PrintDocumentCommand = new RelayCommand(PrintDocument));
            }
        }

        private RelayCommand _AdmitCommand;
        public RelayCommand AdmitCommand
        {
            get
            {
                return _AdmitCommand ?? (_AdmitCommand = new RelayCommand(Admit));
            }
        }

        private RelayCommand _DropCommand;
        public RelayCommand DropCommand
        {
            get
            {
                return _DropCommand ?? (_DropCommand = new RelayCommand(Drop));
            }
        }
        #endregion

        #region Method
        public AdmissionRequestListViewModel()
        {
            DateFrom = DateTime.Now;
            WardSource = DataService.Technical.GetLocationByTypeUID(3152,AppUtil.Current.OwnerOrganisationUID);
            StatusSource = DataService.Technical.GetReferenceValueList("BKTYP");
            Search();
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
            string patientID = SelectedPateintSearch != null ? SelectedPateintSearch.PatientID : "";
            int? bktyUID = SelectedStatus != null ? SelectedStatus.Key : (int?)null;
            int? wardUID = SelectedWard != null ? SelectedWard.LocationUID : (int?)null;

            IPBookingSource = DataService.PatientIdentity.SearchIPBooking(patientID, DateFrom, DateTo, bktyUID, wardUID);
        }
        private void Clear()
        {
            SelectedWard = null;
            SelectedStatus = null;
            DateTo = null;
            DateFrom = DateTime.Now;
        }

        private void Admit()
        {
            if (SelectedIPBooking != null)
            { 
                if(SelectedIPBooking.BedBookingRequest != "Requested")
                {
                    WarningDialog("สถานะเป็น"+ SelectedIPBooking.BedBookingRequest+ " ไม่สามารถ Admit ได้");
                    return;
                }

                if (SelectedIPBooking.BedBookingRequest != null)
                {
                    AdmissionDetail pageview = new AdmissionDetail();
                    (pageview.DataContext as AdmissionDetailViewModel).ConfirmFromRequestAdmission(SelectedIPBooking);
                    AdmissionDetailViewModel result = (AdmissionDetailViewModel)LaunchViewDialogNonPermiss(pageview, false);

                    if (result != null && result.ResultDialog == ActionDialog.Save)
                    {
                        SaveSuccessDialog();
                    }
                    Search();
                }
            }
        }
        private void Drop()
        {
            if(SelectedIPBooking != null)
            {
                int status = DataService.Technical.GetReferenceValueByCode("BKTYP", "ADMIT").Key ?? 0;
                DataService.PatientIdentity.ChangeStatusIPBooking(SelectedIPBooking.IPBookingUID, status, AppUtil.Current.UserID);

                // DataService.PatientIdentity.DropIPBooking(SelectedIPBooking.IPBookingUID, AppUtil.Current.UserID);
            }

            Search();
        }

        private void PrintDocument()
        {
            if (SelectedIPBooking != null)
            {
                PatientVisitModel visitModel = new PatientVisitModel();
                visitModel.PatientID = SelectedIPBooking.PatientID;
                visitModel.PatientUID = SelectedIPBooking.PatientUID;
                visitModel.PatientVisitUID = SelectedIPBooking.PatientVisitUID ?? 0;
                visitModel.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
                ShowModalDialogUsingViewModel(new RunPatientReports(), new RunPatientReportsViewModel() { SelectPatientVisit = visitModel }, true);
            }
        }

        #endregion
    }
}
