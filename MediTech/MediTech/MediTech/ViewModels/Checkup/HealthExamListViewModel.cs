using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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


        private List<InsuranceCompanyModel> _InsuranceCompanyDetails;

        public List<InsuranceCompanyModel> InsuranceCompanyDetails
        {
            get { return _InsuranceCompanyDetails; }
            set { Set(ref _InsuranceCompanyDetails, value); }
        }

        private InsuranceCompanyModel _SelectInsuranceCompanyDetails;

        public InsuranceCompanyModel SelectInsuranceCompanyDetails
        {
            get { return _SelectInsuranceCompanyDetails; }
            set
            {
                Set(ref _SelectInsuranceCompanyDetails, value);
                if (_SelectInsuranceCompanyDetails != null)
                {
                    CheckupJobContactList = DataService.Checkup.GetCheckupJobContactByPayorDetailUID(_SelectInsuranceCompanyDetails.InsuranceCompanyUID);
                    SelectCheckupJobContact = CheckupJobContactList.OrderByDescending(p => p.StartDttm).FirstOrDefault();
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

        private List<RequestItemModel> _RequestItems;

        public List<RequestItemModel> RequestItems
        {
            get { return _RequestItems; }
            set { Set(ref _RequestItems, value); }
        }

        private RequestItemModel _SelectRequestItem;

        public RequestItemModel SelectRequestItem
        {
            get { return _SelectRequestItem; }
            set { Set(ref _SelectRequestItem, value); }
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


        private RelayCommand _CancelResultCommand;

        /// <summary>
        /// Gets the CancelResultCommand.
        /// </summary>
        public RelayCommand CancelResultCommand
        {
            get
            {
                return _CancelResultCommand
                    ?? (_CancelResultCommand = new RelayCommand(CancelResult));
            }
        }

        #endregion


        #region Method
        public HealthExamListViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            InsuranceCompanyDetails = DataService.Billing.GetInsuranceCompanyAll();
            var refValues = DataService.Technical.GetReferenceValueList("PRTGP");
            if (refValues != null)
                RequestItemTypes = refValues.Where(p => p.NumericValue == 1).ToList();

            RequestItems = DataService.MasterData.GetRequestItemByCategory("Mobile Checkup");

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
        void Search()
        {

            long? patientUID = null;
            int? insuranceCompanyUID = null;
            int? checkupJobUID = null;
            int? PRTGPUID = null;
            int? requestItemUID = null;
            if (!string.IsNullOrEmpty(SearchPatientCriteria))
            {
                if (SelectedPateintSearch != null)
                {
                    patientUID = SelectedPateintSearch.PatientUID;
                }
            }

            if (SelectInsuranceCompanyDetails != null)
            {
                insuranceCompanyUID = SelectInsuranceCompanyDetails.InsuranceCompanyUID;
            }

            if (SelectCheckupJobContact != null)
            {
                checkupJobUID = SelectCheckupJobContact.CheckupJobContactUID;
            }

            if (SelectRequestItemType != null)
            {
                PRTGPUID = SelectRequestItemType.Key;
            }

            if(SelectRequestItem != null)
            {
                requestItemUID = SelectRequestItem.RequestItemUID;
            }

            var listResult = DataService.Checkup.SearchCheckupExamList(DateFrom, DateTo, patientUID, insuranceCompanyUID, checkupJobUID, PRTGPUID, requestItemUID);

            CheckupExamList = new ObservableCollection<RequestListModel>(listResult);
            OnUpdateEvent();
        }

        void EnterResult()
        {
            try
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
                                case "Pulmonary Function Test":
                                    EnterPulmonaryResult reviewPulmonary = new EnterPulmonaryResult();
                                    (reviewPulmonary.DataContext as EnterPulmonaryResultViewModel).AssignModel(item);
                                    reviewViewModel = (EnterPulmonaryResultViewModel)LaunchViewDialogNonPermiss(reviewPulmonary, false, true);
                                    break;
                                case "Muscle Test":
                                    EnterMuscleTest reviewMuscleTest = new EnterMuscleTest();
                                    (reviewMuscleTest.DataContext as EnterMuscleTestViewModel).AssignModel(item);
                                    reviewViewModel = (EnterMuscleTestViewModel)LaunchViewDialogNonPermiss(reviewMuscleTest, false, true);
                                    break;
                                case "Physical Fitness Test":
                                    EnterFitnessTestResult reviewFitnessTest = new EnterFitnessTestResult();
                                    (reviewFitnessTest.DataContext as EnterFitnessTestResultViewModel).AssignModel(item);
                                    reviewViewModel = (EnterFitnessTestResultViewModel)LaunchViewDialogNonPermiss(reviewFitnessTest, false, true);
                                    break;
                                default:
                                    EnterCheckupTestResult reviewCheckup = new EnterCheckupTestResult();
                                    (reviewCheckup.DataContext as EnterCheckupTestResultViewModel).AssignModel(item);
                                    reviewViewModel = (EnterCheckupTestResultViewModel)LaunchViewDialogNonPermiss(reviewCheckup, false, true);
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
                                if (OrderStatus != null)
                                {
                                    item.OrderStatus = (String)(OrderStatus.GetValue(reviewViewModel));

                                }
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
                                reviewViewModel = (EnterAudiogramResultViewModel)LaunchViewDialogNonPermiss(reviewAudioGram, false, true);
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
                            case "Pulmonary Function Test":
                                EnterPulmonaryResult reviewPulmonary = new EnterPulmonaryResult();
                                (reviewPulmonary.DataContext as EnterPulmonaryResultViewModel).AssignModel(SelectCheckupExam);
                                reviewViewModel = (EnterPulmonaryResultViewModel)LaunchViewDialogNonPermiss(reviewPulmonary, false, true);
                                break;
                            case "Muscle Test":
                                EnterMuscleTest reviewMuscleTest = new EnterMuscleTest();
                                (reviewMuscleTest.DataContext as EnterMuscleTestViewModel).AssignModel(SelectCheckupExam);
                                reviewViewModel = (EnterMuscleTestViewModel)LaunchViewDialogNonPermiss(reviewMuscleTest, false, true);
                                break;
                            case "Physical Fitness Test":
                                EnterFitnessTestResult reviewFitnessTest = new EnterFitnessTestResult();
                                (reviewFitnessTest.DataContext as EnterFitnessTestResultViewModel).AssignModel(SelectCheckupExam);
                                reviewViewModel = (EnterFitnessTestResultViewModel)LaunchViewDialogNonPermiss(reviewFitnessTest, false, true);
                                break;
                            default:
                                EnterCheckupTestResult reviewCheckup = new EnterCheckupTestResult();
                                (reviewCheckup.DataContext as EnterCheckupTestResultViewModel).AssignModel(SelectCheckupExam);
                                reviewViewModel = (EnterCheckupTestResultViewModel)LaunchViewDialogNonPermiss(reviewCheckup, false, true);
                                break;

                        }

                        if (reviewViewModel == null)
                        {
                            return;
                        }

                        if (reviewViewModel != null && reviewViewModel.ResultDialog == ActionDialog.Save)
                        {
                            System.Reflection.PropertyInfo OrderStatus = reviewViewModel.GetType().GetProperty("OrderStatus");
                            if (OrderStatus != null)
                            {
                                SelectCheckupExam.OrderStatus = (String)(OrderStatus.GetValue(reviewViewModel));
                                OnUpdateEvent();

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorDialog(ex.Message);
            }

        }

        void CancelResult()
        {
            if (SelectCheckupExam.OrderStatus == "Reviewed")
            {
                try
                {
                    MessageBoxResult messResult = QuestionDialog(string.Format("คุณต้องการยกเลิกผลของ {0} ใช่หรือไม่ ?", SelectCheckupExam.PatientName));
                    if (messResult == MessageBoxResult.Yes)
                    {
                        DataService.Checkup.CancelOccmedResult(SelectCheckupExam.RequestDetailUID, AppUtil.Current.UserID);
                        SelectCheckupExam.OrderStatus = "Raised";
                        SelectCheckupExam.ResultedDttm = null;
                        SaveSuccessDialog();
                        OnUpdateEvent();
                    }
                }
                catch (Exception er)
                {

                    ErrorDialog(er.Message);
                }

            }

        }

        #endregion
    }
}
