using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MediTech.ViewModels
{
    public class HealthExamListViewModel : MediTechViewModelBase
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
                if (SelectedPateintSearch != null)
                {
                    Search();
                    SearchPatientCriteria = string.Empty;
                }
            }
        }

        #endregion

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

        private List<CheckupJobContactModel> _CheckupJobContactList;

        public List<CheckupJobContactModel> CheckupJobContactList
        {
            get { return _CheckupJobContactList; }
            set { Set(ref _CheckupJobContactList, value); }
        }

        private CheckupJobContactModel _SelectCheckupJobContact;

        public CheckupJobContactModel SelectCheckupJobContact
        {
            get { return _SelectCheckupJobContact; }
            set { Set(ref _SelectCheckupJobContact, value); }
        }


        private List<PayorDetailModel> _PayorDetails;

        public List<PayorDetailModel> PayorDetails
        {
            get { return _PayorDetails; }
            set { Set(ref _PayorDetails, value); }
        }

        private PayorDetailModel _SelectPayorDetail;

        public PayorDetailModel SelectPayorDetail
        {
            get { return _SelectPayorDetail; }
            set {
                Set(ref _SelectPayorDetail, value);
                if (_SelectPayorDetail != null)
                {
                    CheckupJobContactList = DataService.Checkup.GetCheckupJobContactByPayorDetailUID(_SelectPayorDetail.PayorDetailUID);
                }
            }
        }

        private List<LookupReferenceValueModel> _RequestItemTypes;

        public List<LookupReferenceValueModel> RequestItemTypes
        {
            get { return _RequestItemTypes; }
            set { Set(ref _RequestItemTypes, value); }
        }

        private LookupReferenceValueModel _SelectRequestItemType;

        public LookupReferenceValueModel SelectRequestItemType
        {
            get { return _SelectRequestItemType; }
            set { Set(ref _SelectRequestItemType, value); }
        }


        private ObservableCollection<RequestListModel> _CheckupExamList;

        public ObservableCollection<RequestListModel> CheckupExamList
        {
            get { return _CheckupExamList; }
            set { Set(ref _CheckupExamList, value); }
        }

        private RequestListModel _SelectCheckupExam;

        public RequestListModel SelectCheckupExam
        {
            get { return _SelectCheckupExam; }
            set
            {
                RequestListModel oldSource = _SelectCheckupExam;
                RequestListModel newSource = value;
                Set(ref _SelectCheckupExam, value);
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    if (newSource.RowHandle > oldSource.RowHandle)
                    {
                        var rowHandleLength = CheckupExamList.Where(p => p.RowHandle
                        >= oldSource.RowHandle && p.RowHandle <= newSource.RowHandle);

                        if (rowHandleLength.All(p => p.IsSelected))
                        {
                            foreach (var item in rowHandleLength)
                            {
                                item.IsSelected = false;
                            }
                        }
                        else
                        {
                            foreach (var item in rowHandleLength)
                            {
                                item.IsSelected = true;
                            }
                        }
                    }
                    else if (newSource.RowHandle < oldSource.RowHandle)
                    {
                        var rowHandleLength = CheckupExamList.Where(p => p.RowHandle
                        >= newSource.RowHandle && p.RowHandle <= oldSource.RowHandle);

                        if (rowHandleLength.All(p => p.IsSelected))
                        {
                            foreach (var item in rowHandleLength)
                            {
                                item.IsSelected = false;
                            }
                        }
                        else
                        {
                            foreach (var item in rowHandleLength)
                            {
                                item.IsSelected = true;
                            }
                        }

                    }
                }

            }
        }


        private bool _SurpassSelectAll = false;

        public bool SurpassSelectAll
        {
            get { return _SurpassSelectAll; }
            set { _SurpassSelectAll = value; }
        }


        private bool? _IsSelectedAll = false;

        public bool? IsSelectedAll
        {
            get { return _IsSelectedAll; }
            set
            {
                Set(ref _IsSelectedAll, value);
                if (!SurpassSelectAll)
                {
                    foreach (var requestExam in CheckupExamList)
                    {
                        if (IsSelectedAll == true)
                        {
                            requestExam.IsSelected = true;
                        }
                        else if (IsSelectedAll == false)
                        {
                            requestExam.IsSelected = false;
                        }
                    }
                    if (IsSelectedAll == true)
                    {
                        VisibilityCount = System.Windows.Visibility.Visible;
                        CountSelect = "Count : " + CheckupExamList.Count();
                    }
                    else if (IsSelectedAll == false)
                    {
                        VisibilityCount = System.Windows.Visibility.Hidden;
                        CountSelect = "";
                    }
                    OnUpdateEvent();
                }
                SurpassSelectAll = false;
            }
        }

        private System.Windows.Visibility _VisibilityCount = System.Windows.Visibility.Hidden;

        public System.Windows.Visibility VisibilityCount
        {
            get { return _VisibilityCount; }
            set { Set(ref _VisibilityCount, value); }
        }


        private string _CountSelect;

        public string CountSelect
        {
            get { return _CountSelect; }
            set { Set(ref _CountSelect, value); }
        }

        #endregion


        #region Command


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

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get
            {
                return _SearchCommand
                    ?? (_SearchCommand = new RelayCommand(Search));
            }
        }

        private RelayCommand _EnterResultCommand;

        /// <summary>
        /// Gets the EnterResultCommand.
        /// </summary>
        public RelayCommand EnterResultCommand
        {
            get
            {
                return _EnterResultCommand
                    ?? (_EnterResultCommand = new RelayCommand(EnterResult));
            }
        }


        #endregion


        #region Method
        public HealthExamListViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            PayorDetails = DataService.MasterData.GetPayorDetail();
            var refValues = DataService.Technical.GetReferenceValueList("PRTGP");
            if (refValues != null)
                RequestItemTypes = refValues.Where(p => p.NumericValue == 1).ToList();

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
        void Search()
        {
            long? patientUID = null;
            int? payorDetailUID = null;
            int? checkupJobUID = null;
            int? PRTGPUID = null;
            if (!string.IsNullOrEmpty(SearchPatientCriteria))
            {
                if (SelectedPateintSearch != null)
                {
                    patientUID = SelectedPateintSearch.PatientUID;
                }
            }

            if (SelectPayorDetail != null)
            {
                payorDetailUID = SelectPayorDetail.PayorDetailUID;
            }

            if (SelectCheckupJobContact != null)
            {
                checkupJobUID = SelectCheckupJobContact.CheckupJobContactUID;
            }

            if (SelectRequestItemType != null)
            {
                PRTGPUID = SelectRequestItemType.Key;
            }

            var listResult = DataService.Checkup.SearchCheckupExamList(DateFrom, DateTo,patientUID, payorDetailUID, checkupJobUID,PRTGPUID);

            CheckupExamList = new ObservableCollection<RequestListModel>(listResult);
            OnUpdateEvent();
        }

        void EnterResult()
        {
            if (CheckupExamList != null)
            {
                var selectRequestExamlist = CheckupExamList.Where(p => p.IsSelected).OrderBy(p => p.RowHandle);
                if (selectRequestExamlist != null && selectRequestExamlist.Count() > 0)
                {
                    foreach (var item in selectRequestExamlist)
                    {
                        MediTechViewModelBase reviewViewModel = null;
                        switch (item.PrintGroup)
                        {
                            case "Physical examination":
                                EnterPhysicalExam reviewPhyexam = new EnterPhysicalExam();
                                (reviewPhyexam.DataContext as EnterPhysicalExamViewModel).AssignModel(item);
                                reviewViewModel = (EnterPhysicalExamViewModel)LaunchViewDialogNonPermiss(reviewPhyexam, false, true);
                                break;
                            case "Audiogram":
                                EnterAudiogramResult reviewAudioGram = new EnterAudiogramResult();
                                (reviewAudioGram.DataContext as EnterAudiogramResultViewModel).AssignModel(item);
                                reviewViewModel = (EnterAudiogramResultViewModel)LaunchViewDialogNonPermiss(reviewAudioGram, false, true);
                                break;
                            case "Elektrokardiogram":
                                EnterEKGResult reviewEKG = new EnterEKGResult();
                                (reviewEKG.DataContext as EnterEKGResultViewModel).AssignModel(item);
                                reviewViewModel = (EnterEKGResultViewModel)LaunchViewDialogNonPermiss(reviewEKG, false, true);
                                break;
                            case "Occupational Vision Test":
                                EnterOccuVisionTestResult reviewOccu = new EnterOccuVisionTestResult();
                                (reviewOccu.DataContext as EnterOccuVisionTestResultViewModel).AssignModel(item);
                                reviewViewModel = (EnterOccuVisionTestResultViewModel)LaunchViewDialogNonPermiss(reviewOccu, false, true);
                                break;
                            case "Vision Test":
                                EnterVisionTestResult reviewVision = new EnterVisionTestResult();
                                (reviewVision.DataContext as EnterVisionTestResultViewModel).AssignModel(item);
                                reviewViewModel = (EnterVisionTestResultViewModel)LaunchViewDialogNonPermiss(reviewVision, false, true);
                                break;
                            case "Pulmonary Function Test":
                                EnterPulmonaryResult reviewPulmonary = new EnterPulmonaryResult();
                                (reviewPulmonary.DataContext as EnterPulmonaryResultViewModel).AssignModel(item);
                                reviewViewModel = (EnterPulmonaryResultViewModel)LaunchViewDialogNonPermiss(reviewPulmonary, false, true);
                                break;
                            default:

                                break;
                        }



                        if (reviewViewModel == null)
                        {
                            return;
                        }

                        if (reviewViewModel != null && reviewViewModel.ResultDialog == ActionDialog.Cancel)
                        {
                            item.IsSelected = false;
                            break;
                        }

                        if (reviewViewModel != null && reviewViewModel.ResultDialog == ActionDialog.Save)
                        {
                            System.Reflection.PropertyInfo OrderStatus = reviewViewModel.GetType().GetProperty("OrderStatus");
                            item.OrderStatus = (String)(OrderStatus.GetValue(reviewViewModel));
                        }

                        item.IsSelected = false;
                    }

                    OnUpdateEvent();

                }
                else if (SelectCheckupExam != null)
                {
                    MediTechViewModelBase reviewViewModel = null;
                    switch (SelectCheckupExam.PrintGroup)
                    {
                        case "Physical examination":
                            EnterPhysicalExam reviewPhyexam = new EnterPhysicalExam();
                            (reviewPhyexam.DataContext as EnterPhysicalExamViewModel).AssignModel(SelectCheckupExam);
                            reviewViewModel = (EnterPhysicalExamViewModel)LaunchViewDialogNonPermiss(reviewPhyexam, false, true);
                            break;
                        case "Audiogram":
                            EnterAudiogramResult reviewAudioGram = new EnterAudiogramResult();
                            (reviewAudioGram.DataContext as EnterAudiogramResultViewModel).AssignModel(SelectCheckupExam);
                            reviewViewModel = (EnterPhysicalExamViewModel)LaunchViewDialogNonPermiss(reviewAudioGram, false, true);
                            break;
                        case "Elektrokardiogram":
                            EnterEKGResult reviewEKG = new EnterEKGResult();
                            (reviewEKG.DataContext as EnterEKGResultViewModel).AssignModel(SelectCheckupExam);
                            reviewViewModel = (EnterEKGResultViewModel)LaunchViewDialogNonPermiss(reviewEKG, false, true);
                            break;
                        case "Occupational Vision Test":
                            EnterOccuVisionTestResult reviewOccu = new EnterOccuVisionTestResult();
                            (reviewOccu.DataContext as EnterOccuVisionTestResultViewModel).AssignModel(SelectCheckupExam);
                            reviewViewModel = (EnterOccuVisionTestResultViewModel)LaunchViewDialogNonPermiss(reviewOccu, false, true);
                            break;
                        case "Vision Test":
                            EnterVisionTestResult reviewVision = new EnterVisionTestResult();
                            (reviewVision.DataContext as EnterVisionTestResultViewModel).AssignModel(SelectCheckupExam);
                            reviewViewModel = (EnterVisionTestResultViewModel)LaunchViewDialogNonPermiss(reviewVision, false, true);
                            break;
                        case "Pulmonary Function Test":
                            EnterPulmonaryResult reviewPulmonary = new EnterPulmonaryResult();
                            (reviewPulmonary.DataContext as EnterPulmonaryResultViewModel).AssignModel(SelectCheckupExam);
                            reviewViewModel = (EnterPulmonaryResultViewModel)LaunchViewDialogNonPermiss(reviewPulmonary, false, true);
                            break;
                        default:
                            break;
       
                    }

                    if (reviewViewModel == null)
                    {
                        return;
                    }

                    if (reviewViewModel != null && reviewViewModel.ResultDialog == ActionDialog.Save)
                    {
                        System.Reflection.PropertyInfo OrderStatus = reviewViewModel.GetType().GetProperty("OrderStatus");
                        SelectCheckupExam.OrderStatus = (String)(OrderStatus.GetValue(reviewViewModel));
                        OnUpdateEvent();
                    }
                }
            }
        }

        #endregion
    }
}
