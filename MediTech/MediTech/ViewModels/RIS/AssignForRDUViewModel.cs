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

namespace MediTech.ViewModels
{
    public class AssignForRDUViewModel : MediTechViewModelBase
    {
        #region Properties

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

        private string _RequestItemName;

        public string RequestItemName
        {
            get { return _RequestItemName; }
            set { Set(ref _RequestItemName, value); }
        }

        private List<LookupReferenceValueModel> _Modality;

        public List<LookupReferenceValueModel> Modality
        {
            get { return _Modality; }
            set { Set(ref _Modality, value); }
        }

        private LookupReferenceValueModel _SelectModality;

        public LookupReferenceValueModel SelectModality
        {
            get { return _SelectModality; }
            set { Set(ref _SelectModality, value); }
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
            set { Set(ref _SelectPayorDetail, value); }
        }

        private List<LookupReferenceValueModel> _RequestStatusSource;

        public List<LookupReferenceValueModel> RequestStatusSource
        {
            get { return _RequestStatusSource; }
            set { Set(ref _RequestStatusSource, value); }
        }

        private LookupReferenceValueModel _SelectRequestStatus;

        public LookupReferenceValueModel SelectRequestStatus
        {
            get { return _SelectRequestStatus; }
            set { Set(ref _SelectRequestStatus, value); }
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
                    foreach (var requestExam in RequestExamList)
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
                    OnUpdateEvent();
                }
                SurpassSelectAll = false;
            }
        }

        #endregion

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
                    SearchExamListForAssign();
                    SearchPatientCriteria = string.Empty;
                }
            }
        }

        private ObservableCollection<RequestListModel> _RequestExamList;

        public ObservableCollection<RequestListModel> RequestExamList
        {
            get { return _RequestExamList; }
            set { Set(ref _RequestExamList, value); }
        }


        private ObservableCollection<RequestListModel> _SelectRequestExams;

        public ObservableCollection<RequestListModel> SelectRequestExams
        {
            get { return _SelectRequestExams ?? (_SelectRequestExams = new ObservableCollection<RequestListModel>()); }
            set { Set(ref _SelectRequestExams, value); }
        }

        private List<CareproviderModel> _Careproviders;

        public List<CareproviderModel> Careproviders
        {
            get { return _Careproviders; }
            set { _Careproviders = value; }
        }

        private CareproviderModel _SelectCareprovider;

        public CareproviderModel SelectCareprovider
        {
            get { return _SelectCareprovider; }
            set { Set(ref _SelectCareprovider, value); }
        }

        private List<CareproviderModel> _ListAssignCareprovider;

        public List<CareproviderModel> ListAssignCareprovider
        {
            get { return _ListAssignCareprovider ?? (_ListAssignCareprovider = new List<CareproviderModel>()); }
            set { Set(ref _ListAssignCareprovider, value); }
        }

        private CareproviderModel _SelectListAssingCareprovider;

        public CareproviderModel SelectListAssingCareprovider
        {
            get { return _SelectListAssingCareprovider; }
            set { Set(ref _SelectListAssingCareprovider, value); }
        }

        private string _CountSelect;

        public string CountSelect
        {
            get { return _CountSelect; }
            set { Set(ref _CountSelect, value); }
        }

        private Visibility _VisibilityCount = Visibility.Hidden;

        public Visibility VisibilityCount
        {
            get { return _VisibilityCount; }
            set { Set(ref _VisibilityCount, value); }
        }


        #endregion

        #region Command

        private RelayCommand _SearchCommand;

        /// <summary>
        /// Gets the SearchCommand.
        /// </summary>
        public RelayCommand SearchCommand
        {
            get
            {
                return _SearchCommand
                    ?? (_SearchCommand = new RelayCommand(SearchExamListForAssign));
            }
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

        private RelayCommand _ClearCommand;

        /// <summary>
        /// Gets the ClearCommand.
        /// </summary>
        public RelayCommand ClearCommand
        {
            get
            {
                return _ClearCommand
                    ?? (_ClearCommand = new RelayCommand(Clear));
            }
        }

        private RelayCommand _AddCareproviderCommand;

        /// <summary>
        /// Gets the AddCareproviderCommand.
        /// </summary>
        public RelayCommand AddCareproviderCommand
        {
            get
            {
                return _AddCareproviderCommand
                    ?? (_AddCareproviderCommand = new RelayCommand(AddCareprovider));
            }
        }

        private RelayCommand _DeleteCareproviderCommand;

        /// <summary>
        /// Gets the DeleteCareproviderCommand.
        /// </summary>
        public RelayCommand DeleteCareproviderCommand
        {
            get
            {
                return _DeleteCareproviderCommand
                    ?? (_DeleteCareproviderCommand = new RelayCommand(DeleteCareprovider));
            }
        }

        private RelayCommand _AssignCommand;

        /// <summary>
        /// Gets the AssignCommand.
        /// </summary>
        public RelayCommand AssignCommand
        {
            get
            {
                return _AssignCommand
                    ?? (_AssignCommand = new RelayCommand(Assign));
            }
        }

        private RelayCommand _ExecuteCommand;

        /// <summary>
        /// Gets the ExecuteCommand.
        /// </summary>
        public RelayCommand ExecuteCommand
        {
            get
            {
                return _ExecuteCommand
                    ?? (_ExecuteCommand = new RelayCommand(Execute));
            }
        }
        #endregion

        #region Method

        public AssignForRDUViewModel()
        {
            SelectRequestExams.CollectionChanged += SelectRequestExams_CollectionChanged;
            DateFrom = DateTime.Now;
            PayorDetails = DataService.Billing.GetPayorDetail();
            Careproviders = DataService.UserManage.GetCareproviderAll();
            var refValue = DataService.Technical.GetReferenceValueList("ORDST,RIMTYP");
            Modality = refValue.Where(p => p.DomainCode == "RIMTYP").ToList();
            var ordstData = refValue.Where(p => p.DomainCode == "ORDST").ToList();

            if (ordstData != null)
            {
                RequestStatusSource = ordstData.Where(p => p.ValueCode == "OALLC" || p.ValueCode == "REGIST" || p.ValueCode == "EXCUT").ToList();
            }

            if (RequestStatusSource != null)
            {
                SelectRequestStatus = RequestStatusSource.FirstOrDefault(p => p.ValueCode == "REGIST");
            }

            if (Careproviders != null)
            {
                Careproviders = Careproviders.Where(p => p.IsRDUStaff).ToList();
            }
            Organisations = GetHealthOrganisationRoleMedical();
            Modality = Modality.OrderBy(p => p.DisplayOrder).ToList();
        }

        private void SelectRequestExams_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (SelectRequestExams != null && SelectRequestExams.Count > 1)
            {
                VisibilityCount = Visibility.Visible;
                CountSelect = "Count : " + SelectRequestExams.Count.ToString();
            }
            else
            {
                VisibilityCount = Visibility.Hidden;
                CountSelect = "";
            }
        }

        void SearchExamListForAssign()
        {
            long? patientUID = null;
            int? rimtyp = null;
            int? organisationUID = null;
            int? payorDetailUID = null;
            int? ORDSTUID = null;
            if (!string.IsNullOrEmpty(SearchPatientCriteria))
            {
                if (SelectedPateintSearch != null)
                {
                    patientUID = SelectedPateintSearch.PatientUID;
                }
            }

            if (SelectModality != null)
            {
                rimtyp = SelectModality.Key;
            }

            if (SelectOrganisation != null)
            {
                organisationUID = SelectOrganisation.HealthOrganisationUID;
            }

            if (SelectPayorDetail != null)
            {
                payorDetailUID = SelectPayorDetail.PayorDetailUID;
            }
            if (SelectRequestStatus != null)
            {
                ORDSTUID = SelectRequestStatus.Key;
            }

            var tempExamList = DataService.Radiology.SearchRequestExamListForAssign(DateFrom, DateTo, organisationUID, patientUID, RequestItemName, rimtyp, null,payorDetailUID, ORDSTUID);
            RequestExamList = new ObservableCollection<RequestListModel>(tempExamList);
        }

        void AddCareprovider()
        {
            if (SelectCareprovider != null)
            {
                if (ListAssignCareprovider.FirstOrDefault(p => p.CareproviderUID == SelectCareprovider.CareproviderUID) != null)
                {
                    WarningDialog("มีรายชื่อเจ้าหน้าที่นี้อยู่ในลิสต์แล้ว");
                    return;
                }
                CareproviderModel newCare = new CareproviderModel();
                newCare.CareproviderUID = SelectCareprovider.CareproviderUID;
                newCare.FirstName = SelectCareprovider.FirstName;
                newCare.LastName = SelectCareprovider.LastName;
                newCare.FullName = SelectCareprovider.FullName;

                ListAssignCareprovider.Add(newCare);
                SelectCareprovider = null;
                OnUpdateEvent();
            }
        }

        void DeleteCareprovider()
        {
            if (SelectListAssingCareprovider != null)
            {
                ListAssignCareprovider.Remove(SelectListAssingCareprovider);
                OnUpdateEvent();
            }
        }

        void Assign()
        {
            try
            {
                //var selectExam = SelectRequestExam;
                if (SelectRequestExams != null && SelectRequestExams.Count() <= 0)
                {
                    WarningDialog("กรุณาเลือกรายการอย่างน้อย 1 รายการ");
                    return;
                }

                if (ListAssignCareprovider == null || ListAssignCareprovider.Count <= 0)
                {
                    WarningDialog("กรุณาเลือกเจ้าหน้าที่อย่างน้อย 1 คน");
                    return;
                }

                int countStaff = ListAssignCareprovider.Count();
                int indexStaff = 0;
                int countPerCase = (SelectRequestExams.Count() / countStaff) == 0 ? 1 : (SelectRequestExams.Count() / countStaff);
                int countloop = 0;


                foreach (var examItem in SelectRequestExams)
                {
                    if (countloop >= countPerCase)
                    {
                        countloop = 0;
                        indexStaff++;
                    }
                    if (examItem.Equals(SelectRequestExams.Last()) && SelectRequestExams.Count > 1)
                    {
                        indexStaff = countStaff - 1;
                    }
                    examItem.AssignedToUID = ListAssignCareprovider[indexStaff].CareproviderUID;

                    examItem.AssignedByUID = AppUtil.Current.UserID;
                    examItem.AssignedDttm = DateTime.Now;

                    countloop++;
                }
                DataService.Radiology.AssignToRDUStaff(SelectRequestExams.ToList(), AppUtil.Current.UserID);
                SaveSuccessDialog();
                SearchExamListForAssign();
                ListAssignCareprovider = null;
                IsSelectedAll = false;
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }


        private void Execute()
        {
            if (SelectRequestExams != null && SelectRequestExams.Count() <= 0)
            {
                WarningDialog("กรุณาเลือกรายการอย่างน้อย 1 รายการ");
                return;
            }

            ExecutePopUp executePopUp = new ExecutePopUp(SelectRequestExams.ToList());
            ExecutePopUpViewModel executePopUpViewModel = (ExecutePopUpViewModel)LaunchViewDialog(executePopUp, "EXEDET", false);
            if (executePopUpViewModel != null && executePopUpViewModel.ResultDialog == ActionDialog.Save)
            {
                SearchExamListForAssign();
            }

        }
        public void CheckSelectAll()
        {
            int allRow = RequestExamList.Count();
            int selectRow = RequestExamList.Count(p => p.IsSelected);
            int unSelectRow = RequestExamList.Count(p => p.IsSelected == false);
            SurpassSelectAll = true;
            if (allRow == selectRow)
            {
                IsSelectedAll = true;
            }
            else if (allRow == unSelectRow)
            {
                IsSelectedAll = false;
            }
            else if (selectRow > 0)
            {
                IsSelectedAll = null;
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, "");
                PatientsSearchSource = searchResult;
            }
            else
            {
                PatientsSearchSource = null;
            }

        }

        private void Clear()
        {
            DateFrom = DateTime.Now;
            DateTo = null;
            SearchPatientCriteria = "";
            SelectedPateintSearch = null;
            RequestItemName = "";
            SelectOrganisation = null;
            SelectModality = null;
        }
        #endregion
    }
}
