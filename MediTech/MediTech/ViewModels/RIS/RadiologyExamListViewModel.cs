using GalaSoft.MvvmLight.Command;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediTech.Model;
using System.Collections.ObjectModel;
using DevExpress.XtraReports.UI;
using MediTech.Helpers;
using MediTech.Reports.Operating.Radiology;
using System.Windows.Input;
using System.Windows;

namespace MediTech.ViewModels
{
    public class RadiologyExamListViewModel : MediTechViewModelBase
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
                    SearchExamList();
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


        private List<LookupReferenceValueModel> _RequestStatus;

        public List<LookupReferenceValueModel> RequestStatus
        {
            get { return _RequestStatus; }
            set { Set(ref _RequestStatus, value); }
        }


        private string _RequestItemName;

        public string RequestItemName
        {
            get { return _RequestItemName; }
            set { Set(ref _RequestItemName, value); }
        }

        private List<HealthOrganisationModel> _Organisations;

        public List<HealthOrganisationModel> Organisations
        {
            get { return _Organisations; }
            set { Set(ref _Organisations, value); }
        }

        private List<LookupReferenceValueModel> _Modality;

        public List<LookupReferenceValueModel> Modality
        {
            get { return _Modality; }
            set { Set(ref _Modality, value); }
        }


        private List<LookupReferenceValueModel> _Priority;

        public List<LookupReferenceValueModel> Priority
        {
            get { return _Priority; }
            set { Set(ref _Priority, value); }
        }

        private List<LookupItemModel> _Reports;

        public List<LookupItemModel> Reports
        {
            get { return _Reports; }
            set { Set(ref _Reports, value); }
        }


        private ObservableCollection<RequestListModel> _RequestExamList;

        public ObservableCollection<RequestListModel> RequestExamList
        {
            get { return _RequestExamList; }
            set { Set(ref _RequestExamList, value); }
        }

        private List<CareproviderModel> _Radiologist;

        public List<CareproviderModel> Radiologist
        {
            get { return _Radiologist; }
            set { Set(ref _Radiologist, value); }
        }


        private List<object> _SelectRequestStatusList;

        public List<object> SelectRequestStatusList
        {
            get { return _SelectRequestStatusList ?? (_SelectRequestStatusList = new List<object>()); }
            set { Set(ref _SelectRequestStatusList, value); }
        }

        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set { Set(ref _SelectOrganisation, value); }
        }

        private CareproviderModel _SelectRadiologist;

        public CareproviderModel SelectRadiologist
        {
            get { return _SelectRadiologist; }
            set { Set(ref _SelectRadiologist, value); }
        }

        private LookupReferenceValueModel _SelectModality;

        public LookupReferenceValueModel SelectModality
        {
            get { return _SelectModality; }
            set { Set(ref _SelectModality, value); }
        }

        private LookupReferenceValueModel _SelectPriority;

        public LookupReferenceValueModel SelectPriority
        {
            get { return _SelectPriority; }
            set { Set(ref _SelectPriority, value); }
        }

        private RequestListModel _SelectRequestExam;

        public RequestListModel SelectRequestExam
        {
            get { return _SelectRequestExam; }
            set
            {
                RequestListModel oldSource = _SelectRequestExam;
                RequestListModel newSource = value;
                Set(ref _SelectRequestExam, value);
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {

                    if (newSource.RowHandle > oldSource.RowHandle)
                    {
                        var rowHandleLength = RequestExamList.Where(p => p.RowHandle
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
                        var rowHandleLength = RequestExamList.Where(p => p.RowHandle
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


        private LookupItemModel _SelectReport;

        public LookupItemModel SelectReport
        {
            get { return _SelectReport; }
            set { Set(ref _SelectReport, value); }
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
                    if (IsSelectedAll == true)
                    {
                        VisibilityCount = System.Windows.Visibility.Visible;
                        CountSelect = "Count : " + RequestExamList.Count();
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


        private List<CareproviderModel> _RDUStaffList;

        public List<CareproviderModel> RDUStaffList
        {
            get { return _RDUStaffList; }
            set { _RDUStaffList = value; }
        }

        private CareproviderModel _SelectRDUStaff;

        public CareproviderModel SelectRDUStaff
        {
            get { return _SelectRDUStaff; }
            set { Set(ref _SelectRDUStaff, value); }
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


        private string _CountSelect;

        public string CountSelect
        {
            get { return _CountSelect; }
            set { Set(ref _CountSelect, value); }
        }

        private List<LookupItemModel> _Logos;

        public List<LookupItemModel> Logos
        {
            get { return _Logos; }
            set { Set(ref _Logos, value); }
        }

        private LookupItemModel _SelectLogo;

        public LookupItemModel SelectLogo
        {
            get { return _SelectLogo; }
            set { Set(ref _SelectLogo, value); }
        }


        private System.Windows.Visibility _VisibilityCount = System.Windows.Visibility.Hidden;

        public System.Windows.Visibility VisibilityCount
        {
            get { return _VisibilityCount; }
            set { Set(ref _VisibilityCount, value); }
        }

        private System.Windows.Visibility _VisibilityAutuRefersh = System.Windows.Visibility.Hidden;

        public System.Windows.Visibility VisibilityAutuRefersh
        {
            get { return _VisibilityAutuRefersh; }
            set { Set(ref _VisibilityAutuRefersh, value); }
        }


        private System.Windows.Visibility _VisibilityMassResult = System.Windows.Visibility.Visible;

        public System.Windows.Visibility VisibilityMassResult
        {
            get { return _VisibilityMassResult; }
            set { Set(ref _VisibilityMassResult, value); }
        }

        System.Windows.Threading.DispatcherTimer dispatcherTimer;



        private bool _IsCheckedAutoRefersh;

        public bool IsCheckedAutoRefersh
        {
            get
            {
                    return _IsCheckedAutoRefersh;
            }
            set
            {
                if (SelectRadiologist == null && value)
                {
                    WarningDialog("กรุณาเลือก รังสีแพทย์");
                    Set(ref _IsCheckedAutoRefersh, false);
                }
                else
                {
                    Set(ref _IsCheckedAutoRefersh, value);
                }


                if (_IsCheckedAutoRefersh)
                    dispatcherTimer.Start();
                else
                    dispatcherTimer.Stop();
            }
        }

        private bool _IsEnabledRadiologist = true;

        public bool IsEnabledRadiologist
        {
            get { return _IsEnabledRadiologist; }
            set { Set(ref _IsEnabledRadiologist, value); }
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
                    ?? (_SearchCommand = new RelayCommand(SearchExamList));
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


        private RelayCommand _ViewPACSCommand;

        /// <summary>
        /// Gets the OpenPACSCommand.
        /// </summary>
        public RelayCommand ViewPACSCommand
        {
            get
            {
                return _ViewPACSCommand
                    ?? (_ViewPACSCommand = new RelayCommand(ViewPACS));
            }
        }


        private RelayCommand _OpenPACSWorkListCommand;

        /// <summary>
        /// Gets the OpenPACSCommand.
        /// </summary>
        public RelayCommand OpenPACSWorkListCommand
        {
            get
            {
                return _OpenPACSWorkListCommand
                    ?? (_OpenPACSWorkListCommand = new RelayCommand(OpenWorkList));
            }
        }

        private RelayCommand _RDUReviewCommand;

        /// <summary>
        /// Gets the BulkResultCommand.
        /// </summary>
        public RelayCommand RDUReviewCommand
        {
            get
            {
                return _RDUReviewCommand
                    ?? (_RDUReviewCommand = new RelayCommand(RDUReview));
            }
        }

        private RelayCommand<string> _PrintPreviewAndAutoCommand;

        /// <summary>
        /// Gets the PrintPreviewAndAutoCommand.
        /// </summary>
        public RelayCommand<string> PrintPreviewAndAutoCommand
        {
            get
            {
                return _PrintPreviewAndAutoCommand
                    ?? (_PrintPreviewAndAutoCommand = new RelayCommand<string>(PrintPreviewAndAuto));
            }
        }

        //private RelayCommand _PrintAutoCommand;

        ///// <summary>
        ///// Gets the PrintAutoCommand.
        ///// </summary>
        //public RelayCommand PrintAutoCommand
        //{
        //    get
        //    {
        //        return _PrintAutoCommand
        //            ?? (_PrintAutoCommand = new RelayCommand(PrintAutoResult));
        //    }
        //}


        #endregion

        #region Method

        public RadiologyExamListViewModel()
        {
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick; ;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);

            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;

            if ((AppUtil.Current.IsRadiologist ?? false) && (!AppUtil.Current.IsAdminRadiologist ?? false))
            {
                IsEnabledRadiologist = false;
            }

            Reports = new List<LookupItemModel>();
            Reports.Add(new LookupItemModel { Key = 0, Display = "รายงานภาษาอังกฤษ" });
            Reports.Add(new LookupItemModel { Key = 1, Display = "รายงานสองภาษา" });
            SelectReport = Reports.FirstOrDefault();

            var refValue = DataService.Technical.GetReferenceValueList("ORDST,RQPRT,RIMTYP");
            var careprovider = DataService.UserManage.GetCareproviderAll();
            Radiologist = careprovider.Where(p => p.IsRadiologist).ToList();
            RDUStaffList = careprovider.Where(p => p.IsRDUStaff).ToList();
            Organisations = GetHealthOrganisationRoleMedical();
            PayorDetails = DataService.MasterData.GetPayorDetail();

            CareproviderModel newCareprovider = new CareproviderModel();
            newCareprovider.CareproviderUID = 0;
            newCareprovider.FullName = "สำหรับแพทย์";
            RDUStaffList.Add(newCareprovider);
            RDUStaffList = RDUStaffList.OrderBy(p => p.CareproviderUID).ToList();

            RequestStatus = refValue.Where(p => p.ValueCode == "CMPLT"
                || p.ValueCode == "REGIST"
                || p.ValueCode == "EXCUT"
                || p.ValueCode == "OALLC"
                || p.ValueCode == "REVIW"
                || p.ValueCode == "REVIN").ToList();

            Priority = refValue.Where(p => p.DomainCode == "RQPRT").ToList();
            Modality = refValue.Where(p => p.DomainCode == "RIMTYP").ToList();



            Priority = Priority.OrderBy(p => p.DisplayOrder).ToList();
            Modality = Modality.OrderBy(p => p.DisplayOrder).ToList();
            Radiologist = Radiologist.OrderBy(p => p.FullName).ToList();

            Logos = new List<LookupItemModel>();
            Logos.Add(new LookupItemModel { Key = 0, Display = "BRXG Polyclinic" });
            Logos.Add(new LookupItemModel { Key = 1, Display = "BRXG" });
            Logos.Add(new LookupItemModel { Key = 2, Display = "DRC" });
            SelectLogo = Logos.FirstOrDefault();


            if (AppUtil.Current.IsRDUStaff ?? false)
            {
                var allocated = RequestStatus.FirstOrDefault(p => p.ValueCode == "OALLC");
                var executed = RequestStatus.FirstOrDefault(p => p.ValueCode == "EXCUT");
                if (allocated != null)
                {
                    SelectRequestStatusList.Add(allocated.Key);
                }
                if (executed != null)
                {
                    SelectRequestStatusList.Add(executed.Key);
                }
                SelectRDUStaff = RDUStaffList.FirstOrDefault(p => p.CareproviderUID == AppUtil.Current.UserID);
            }
            else
            {
                foreach (var item in RequestStatus)
                {
                    SelectRequestStatusList.Add(item.Key);
                }
            }


            if (AppUtil.Current.IsRadiologist ?? false)
            {
                SelectRadiologist = Radiologist.FirstOrDefault(p => p.CareproviderUID == AppUtil.Current.UserID);
                VisibilityAutuRefersh = System.Windows.Visibility.Visible;
            }

            SearchExamList();

            if (AppUtil.Current.ApplicationId == "TERC")
            {
                IsCheckedAutoRefersh = true;
                VisibilityMassResult = System.Windows.Visibility.Hidden;
            }

        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();

            if (SelectRadiologist != null)
            {
                DateTime now = DateTime.Now;
                List<RequestListModel> data = DataService.Radiology.GetRequestExecuteRadiologist(now.AddDays(-1), now, SelectRadiologist.CareproviderUID);

                if (data != null && data.Count > 0)
                {
                    foreach (var newRequest in data)
                    {
                        if (!RequestExamList.Any(p => p.RequestDetailUID == newRequest.RequestDetailUID))
                        {
                            RequestExamList.Add(newRequest);
                            string textLine1 = newRequest.PatientName + " (" + newRequest.PatientID + ")";
                            string textLine2 = "Item : " + newRequest.RequestItemName + " (" + newRequest.RequestItemCode + " )";
                            string textLine3 = "RequestedDttm : " + newRequest.RequestedDttm.Value.ToString("dd MMM yyyy (hh:mm:ss)");
                            string textLine4 = newRequest.OrganisationName;
                            (this.View as RadiologyExamList).ShowNotification(textLine1, textLine2, textLine3, textLine4);
                        }
                    }

                }
                MediTech.Helpers.MemoryManagement.FlushMemory();
            }


            dispatcherTimer.Start();
        }

        public override void UnLoaded()
        {
            dispatcherTimer.Stop();
            dispatcherTimer = null;
        }


        private void EnterResult()
        {
            if (RequestExamList != null)
            {
                var selectRequestExamlist = RequestExamList.Where(p => p.IsSelected).OrderBy(p => p.RowHandle);
                if (selectRequestExamlist != null && selectRequestExamlist.Count() > 0)
                {
                    foreach (var item in selectRequestExamlist)
                    {
                        //item.IsSelected = false;
                        ////CheckSelectAll();
                        //OnUpdateEvent();
                        ReviewRISResult review = new ReviewRISResult();
                        (review.DataContext as ReviewRISResultViewModel).AssignModel(item.PatientUID, item.PatientVisitUID, item.RequestUID, item.RequestDetailUID);
                        ReviewRISResultViewModel reviewViewModel = (ReviewRISResultViewModel)LaunchViewDialog(review, "RESTREV", false, true);

                        if (reviewViewModel == null)
                        {
                            return;
                        }

                        if (reviewViewModel != null && reviewViewModel.ResultDialog == ActionDialog.Cancel)
                        {
                            item.IsSelected = false;
                            //CheckSelectAll();
                            OnUpdateEvent();
                            break;
                        }

                        if (reviewViewModel != null && reviewViewModel.ResultDialog == ActionDialog.Save)
                        {
                            item.OrderStatus = reviewViewModel.ResultOrderStatus;
                            item.DoctorName = reviewViewModel.DoctorName;
                            item.ResultStatus = reviewViewModel.ResultedStatus;
                        }

                        item.IsSelected = false;
                        // CheckSelectAll();
                        OnUpdateEvent();

                    }

                }
                else if (SelectRequestExam != null)
                {
                    ReviewRISResult review = new ReviewRISResult();
                    (review.DataContext as ReviewRISResultViewModel).AssignModel(SelectRequestExam.PatientUID
                        , SelectRequestExam.PatientVisitUID, SelectRequestExam.RequestUID, SelectRequestExam.RequestDetailUID);
                    ReviewRISResultViewModel reviewViewModel = (ReviewRISResultViewModel)LaunchViewDialog(review, "RESTREV", false, true);

                    if (reviewViewModel == null)
                    {
                        return;
                    }

                    if (reviewViewModel != null && reviewViewModel.ResultDialog == ActionDialog.Save)
                    {
                        SelectRequestExam.OrderStatus = reviewViewModel.ResultOrderStatus;
                        SelectRequestExam.DoctorName = reviewViewModel.DoctorName;
                        SelectRequestExam.ResultStatus = reviewViewModel.ResultedStatus;
                        OnUpdateEvent();
                    }
                }
            }
        }

        private void RDUReview()
        {
            if (RequestExamList != null)
            {
                var selectRequestExamlist = RequestExamList.Where(p => p.IsSelected).OrderBy(p => p.RowHandle);
                if (selectRequestExamlist != null && selectRequestExamlist.Count() > 0)
                {
                    var registList = selectRequestExamlist.Where(p => p.OrderStatus == "Registered"
                    || p.OrderStatus == "Executed" || p.OrderStatus == "Allocated"
                    || (AppUtil.Current.IsRadiologist == true));
                    if (registList != null && registList.Count() > 0)
                    {
                        foreach (var regist in registList)
                        {
                            if (regist.RDUResultStatus == "Abnormal" && (AppUtil.Current.IsRadiologist ?? false))
                            {
                                #region OpenPACS

                                PACSWorkList pacs = new PACSWorkList();
                                PACSWorkListViewModel pacsViewModel = (pacs.DataContext as PACSWorkListViewModel);
                                pacsViewModel.PatientID = regist.PatientID;
                                pacsViewModel.DateFrom = null;
                                pacsViewModel.DateTo = regist.PreparedDttm != null ? regist.PreparedDttm : regist.RequestedDttm;
                                pacsViewModel.IsCheckedPeriod = true;
                                pacsViewModel.Modality = regist.Modality;
                                pacsViewModel.IsOpenFromExam = true;
                                LaunchViewShow(pacs, null, "PACS", false, true);

                                #endregion

                                ReviewRISResult review = new ReviewRISResult();
                                (review.DataContext as ReviewRISResultViewModel).AssignModel(regist.PatientUID, regist.PatientVisitUID, regist.RequestUID, regist.RequestDetailUID);
                                ReviewRISResultViewModel reviewViewModel = (ReviewRISResultViewModel)LaunchViewDialog(review, "RESTREV", false, true);



                                if (reviewViewModel == null)
                                {
                                    return;
                                }

                                if (reviewViewModel != null && reviewViewModel.ResultDialog == ActionDialog.Cancel)
                                {
                                    regist.IsSelected = false;
                                    //CheckSelectAll();
                                    OnUpdateEvent();
                                    break;
                                }

                                if (reviewViewModel != null && reviewViewModel.ResultDialog == ActionDialog.Save)
                                {
                                    regist.OrderStatus = reviewViewModel.ResultOrderStatus;
                                    regist.DoctorName = reviewViewModel.DoctorName;
                                    regist.ResultStatus = reviewViewModel.ResultedStatus;
                                }
                            }
                            else
                            {
                                RDUReviewResult rduResult = new RDUReviewResult();
                                (rduResult.DataContext as RDUReviewResultViewModel).AssignModel(regist);
                                RDUReviewResultViewModel rduViewModel = (RDUReviewResultViewModel)LaunchViewDialog(rduResult, "RDUREV", true);

                                if (rduViewModel != null && rduViewModel.IsStop)
                                {
                                    break;
                                }

                                if (rduViewModel != null && rduViewModel.ResultDialog == ActionDialog.Save)
                                {
                                    regist.OrderStatus = rduViewModel.OrderStatus;
                                    regist.DoctorName = rduViewModel.DoctorName;
                                    regist.ResultStatus = rduViewModel.ResultedStatus;
                                }

                            }

                            regist.IsSelected = false;
                            //CheckSelectAll();
                            OnUpdateEvent();
                        }

                    }
                }
                else if (SelectRequestExam != null)
                {
                    if (SelectRequestExam.OrderStatus == "Registered"
                    || SelectRequestExam.OrderStatus == "Executed" || SelectRequestExam.OrderStatus == "Allocated"
                    || (AppUtil.Current.IsRadiologist == true))
                    {
                        if (SelectRequestExam.RDUResultStatus == "Abnormal" && (AppUtil.Current.IsRadiologist ?? false))
                        {
                            #region OpenPACS

                            PACSWorkList pacs = new PACSWorkList();
                            PACSWorkListViewModel pacsViewModel = (pacs.DataContext as PACSWorkListViewModel);
                            pacsViewModel.PatientID = SelectRequestExam.PatientID;
                            pacsViewModel.DateFrom = null;
                            pacsViewModel.DateTo = SelectRequestExam.PreparedDttm != null ? SelectRequestExam.PreparedDttm : SelectRequestExam.RequestedDttm;
                            pacsViewModel.IsCheckedPeriod = true;
                            pacsViewModel.Modality = SelectRequestExam.Modality;
                            pacsViewModel.IsOpenFromExam = true;
                            LaunchViewShow(pacs, null, "PACS", false, true);

                            #endregion

                            ReviewRISResult review = new ReviewRISResult();
                            (review.DataContext as ReviewRISResultViewModel)
                                .AssignModel(SelectRequestExam.PatientUID, SelectRequestExam.PatientVisitUID
                                , SelectRequestExam.RequestUID, SelectRequestExam.RequestDetailUID);
                            ReviewRISResultViewModel reviewViewModel = (ReviewRISResultViewModel)LaunchViewDialog(review, "RESTREV", false, true);



                            if (reviewViewModel == null)
                            {
                                return;
                            }

                            if (reviewViewModel != null && reviewViewModel.ResultDialog == ActionDialog.Save)
                            {
                                SelectRequestExam.OrderStatus = reviewViewModel.ResultOrderStatus;
                                SelectRequestExam.DoctorName = reviewViewModel.DoctorName;
                                SelectRequestExam.ResultStatus = reviewViewModel.ResultedStatus;
                            }
                        }
                        else
                        {
                            RDUReviewResult rduResult = new RDUReviewResult();
                            (rduResult.DataContext as RDUReviewResultViewModel).AssignModel(SelectRequestExam);
                            RDUReviewResultViewModel rduViewModel = (RDUReviewResultViewModel)LaunchViewDialog(rduResult, "RDUREV", true);

                            if (rduViewModel != null && rduViewModel.ResultDialog == ActionDialog.Save)
                            {
                                SelectRequestExam.OrderStatus = rduViewModel.OrderStatus;
                                SelectRequestExam.DoctorName = rduViewModel.DoctorName;
                                SelectRequestExam.ResultStatus = rduViewModel.ResultedStatus;
                                OnUpdateEvent();
                            }

                        }
                    }
                }
            }
        }

        private void Execute()
        {
            if (RequestExamList != null)
            {
                var selectRequestExamlist = RequestExamList.Where(p => p.IsSelected);
                if (selectRequestExamlist != null && selectRequestExamlist.Count() > 0)
                {
                    var registList = selectRequestExamlist.Where(p => p.OrderStatus == "Registered" || p.OrderStatus == "Executed");
                    if (registList != null && registList.Count() > 0)
                    {
                        ExecutePopUp executePopUp = new ExecutePopUp(registList.ToList());
                        ExecutePopUpViewModel executePopUpViewModel = (ExecutePopUpViewModel)LaunchViewDialog(executePopUp, "EXEDET", false);
                        if (executePopUpViewModel != null && executePopUpViewModel.ResultDialog == ActionDialog.Save)
                        {
                            SearchExamList();
                            //CheckSelectAll();
                        }
                    }

                }
                else if (SelectRequestExam != null)
                {
                    if (SelectRequestExam.OrderStatus == "Registered" || SelectRequestExam.OrderStatus == "Executed")
                    {
                        List<RequestListModel> executeList = new List<RequestListModel>();
                        executeList.Add(SelectRequestExam);
                        ExecutePopUp executePopUp = new ExecutePopUp(executeList);
                        ExecutePopUpViewModel executePopUpViewModel = (ExecutePopUpViewModel)LaunchViewDialog(executePopUp, "EXEDET", false);
                        if (executePopUpViewModel != null && executePopUpViewModel.ResultDialog == ActionDialog.Save)
                        {
                            SearchExamList();
                            //CheckSelectAll();
                        }
                    }
                }
            }


        }

        private void SearchExamList()
        {
            string statusList = string.Empty;
            int? rqprtuid = null;
            long? patientUID = null;
            int? radiologistUID = null;
            int? rduStaffUID = null;
            int? rimtyp = null;
            int? payorDetailUID = null;
            if (SelectRequestStatusList != null)
            {
                foreach (object item in SelectRequestStatusList)
                {
                    if (item.ToString() != "0")
                    {
                        if (statusList == "")
                        {
                            statusList = item.ToString();
                        }
                        else
                        {
                            statusList += "," + item.ToString();
                        }
                    }

                }
            }
            int? organisationUID = null;

            if (SelectPriority != null)
            {
                rqprtuid = SelectPriority.Key;
            }


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

            if (SelectRadiologist != null)
            {
                radiologistUID = SelectRadiologist.CareproviderUID;
            }

            if (SelectRDUStaff != null && SelectRDUStaff.CareproviderUID != 0)
            {
                rduStaffUID = SelectRDUStaff.CareproviderUID;
            }

            if (SelectOrganisation != null)
            {
                organisationUID = SelectOrganisation.HealthOrganisationUID;
            }

            if (SelectPayorDetail != null)
            {
                payorDetailUID = SelectPayorDetail.PayorDetailUID;
            }

            var listResult = DataService.Radiology.SearchRequestExamList(DateFrom, DateTo, statusList, rqprtuid, patientUID, RequestItemName, rimtyp, radiologistUID, rduStaffUID, payorDetailUID, organisationUID);

            if (SelectRDUStaff != null && SelectRDUStaff.CareproviderUID == 0)
                listResult = listResult.Where(p => string.IsNullOrEmpty(p.RDUStaff)).ToList();

            RequestExamList = new ObservableCollection<RequestListModel>(listResult);
            OnUpdateEvent();
        }

        private void Clear()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            SearchPatientCriteria = "";
            SelectedPateintSearch = null;
            RequestItemName = "";

            foreach (var item in RequestStatus)
            {
                SelectRequestStatusList.Add(item.Key);
            }
            (this.View as RadiologyExamList).cmbStatus.RefreshData();
            if (AppUtil.Current.IsRadiologist.HasValue && AppUtil.Current.IsRadiologist.Value)
            {
                SelectRadiologist = Radiologist.FirstOrDefault(p => p.CareproviderUID == AppUtil.Current.UserID);
            }

            SelectOrganisation = null;
            SelectPayorDetail = null;
            SelectModality = null;
            SelectPriority = null;
        }

        private void ViewPACS()
        {
            if (SelectRequestExam != null)
            {
                PACSWorkList pacs = new PACSWorkList();
                PACSWorkListViewModel pacsViewModel = (pacs.DataContext as PACSWorkListViewModel);
                pacsViewModel.PatientID = SelectRequestExam.PatientID;
                pacsViewModel.IsCheckedPeriod = true;
                pacsViewModel.DateFrom = null;
                pacsViewModel.DateTo = SelectRequestExam.PreparedDttm != null ? SelectRequestExam.PreparedDttm : SelectRequestExam.RequestedDttm;
                pacsViewModel.IsOpenFromExam = true;
                pacsViewModel.Modality = SelectRequestExam.Modality;
                LaunchViewDialog(pacs, "PACS", false, true);
            }
        }

        private void OpenWorkList()
        {
            PACSWorkList pacs = new PACSWorkList();
            LaunchViewShow(pacs, "PACS", false, true);
        }

        private void PrintPreviewAndAuto(string printMode)
        {
            if (SelectReport == null)
            {
                WarningDialog("กรุณาเลือกประเภทรายงานผล");
            }
            if (RequestExamList != null)
            {
                var selectRequestExamlist = RequestExamList.Where(p => p.IsSelected).OrderBy(p => p.RowHandle);
                if (selectRequestExamlist != null && selectRequestExamlist.Count() > 0)
                {
                    foreach (var item in selectRequestExamlist)
                    {
                        if (SelectReport.Key == 0)
                        {
                            if (item.ResultUID != 0)
                            {
                                ImagingReport rpt = new ImagingReport();
                                rpt.LogoType = SelectLogo != null ? SelectLogo.Display : "";
                                ReportPrintTool printTool = new ReportPrintTool(rpt);

                                rpt.Parameters["ResultUID"].Value = item.ResultUID;
                                rpt.RequestParameters = false;
                                rpt.ShowPrintMarginsWarning = false;

                                if (printMode == "preview")
                                {
                                    printTool.ShowPreviewDialog();
                                }
                                else
                                {
                                    printTool.Print();
                                }
                            }
                        }
                        else if (SelectReport.Key == 1)
                        {
                            List<XrayTranslateMappingModel> dtResultMapping = DataService.Radiology.GetXrayTranslateMapping();
                            if (item.ResultUID != 0)
                            {
                                ImagingReportThai rpt = new ImagingReportThai();
                                rpt.LogoType = SelectLogo != null ? SelectLogo.Display : "";
                                ReportPrintTool printTool = new ReportPrintTool(rpt);

                                List<string> listNoMapResult = new List<string>();
                                ResultRadiologyModel result = DataService.Radiology.GetResultRadiologyByResultUID(item.ResultUID);
                                string thairesult = TranslateResult.TranslateResultXray(result.PlainText, item.ResultStatus,item.RequestItemName, " ", dtResultMapping, ref listNoMapResult);

                                rpt.Parameters["ResultUID"].Value = item.ResultUID;
                                rpt.Parameters["ResultThai"].Value = thairesult;

                                rpt.RequestParameters = false;
                                rpt.ShowPrintMarginsWarning = false;
                                if (printMode == "preview")
                                {
                                    printTool.ShowPreviewDialog();
                                }
                                else
                                {
                                    printTool.Print();
                                }
                            }
                        }
                        item.IsSelected = false;
                        //CheckSelectAll();
                        OnUpdateEvent();
                    }

                }
                else if (SelectRequestExam != null)
                {
                    if (SelectReport.Key == 0)
                    {
                        if (SelectRequestExam.ResultUID != 0)
                        {
                            ImagingReport rpt = new ImagingReport();
                            rpt.LogoType = SelectLogo != null ? SelectLogo.Display : "";
                            ReportPrintTool printTool = new ReportPrintTool(rpt);

                            rpt.Parameters["ResultUID"].Value = SelectRequestExam.ResultUID;
                            rpt.RequestParameters = false;
                            rpt.ShowPrintMarginsWarning = false;
                            if (printMode == "preview")
                            {
                                printTool.ShowPreviewDialog();
                            }
                            else
                            {
                                printTool.Print();
                            }
                        }
                    }
                    else if (SelectReport.Key == 1)
                    {
                        List<XrayTranslateMappingModel> dtResultMapping = DataService.Radiology.GetXrayTranslateMapping();
                        if (SelectRequestExam.ResultUID != 0)
                        {
                            ImagingReportThai rpt = new ImagingReportThai();
                            rpt.LogoType = SelectLogo != null ? SelectLogo.Display : "";
                            ReportPrintTool printTool = new ReportPrintTool(rpt);

                            List<string> listNoMapResult = new List<string>();
                            ResultRadiologyModel result = DataService.Radiology.GetResultRadiologyByResultUID(SelectRequestExam.ResultUID);
                            string thairesult = TranslateResult.TranslateResultXray(result.PlainText
                                , SelectRequestExam.ResultStatus,SelectRequestExam.RequestItemName, " ", dtResultMapping, ref listNoMapResult);

                            rpt.Parameters["ResultUID"].Value = SelectRequestExam.ResultUID;
                            rpt.Parameters["ResultThai"].Value = thairesult;

                            rpt.RequestParameters = false;
                            rpt.ShowPrintMarginsWarning = false;
                            if (printMode == "preview")
                            {
                                printTool.ShowPreviewDialog();
                            }
                            else
                            {
                                printTool.Print();
                            }
                        }
                    }
                }
            }

        }

        //private void PrintAutoResult()
        //{
        //    if (SelectReport == null)
        //    {
        //        WarningDialog("กรุณาเลือกประเภทรายงานผล");
        //    }
        //    if (RequestExamList != null)
        //    {
        //        var selectRequestExamlist = RequestExamList.Where(p => p.IsSelected).OrderBy(p => p.RowHandle);
        //        if (selectRequestExamlist != null && selectRequestExamlist.Count() > 0)
        //        {
        //            foreach (var item in selectRequestExamlist)
        //            {
        //                if (SelectReport.Key == 0)
        //                {
        //                    if (item.ResultUID != 0)
        //                    {
        //                        ImagingReport rpt = new ImagingReport();
        //                        ReportPrintTool printTool = new ReportPrintTool(rpt);


        //                        rpt.Parameters["ResultUID"].Value = item.ResultUID;
        //                        rpt.RequestParameters = false;
        //                        rpt.ShowPrintMarginsWarning = false;
        //                        printTool.Print();
        //                    }
        //                }
        //                else if (SelectReport.Key == 1)
        //                {
        //                    List<XrayTranslateMappingModel> dtResultMapping = DataService.Radiology.GetXrayTranslateMapping();
        //                    if (item.ResultUID != 0)
        //                    {
        //                        ImagingReportThai rpt = new ImagingReportThai();
        //                        ReportPrintTool printTool = new ReportPrintTool(rpt);

        //                        List<string> listNoMapResult = new List<string>();
        //                        ResultRadiologyModel result = DataService.Radiology.GetResultRadiologyByResultUID(item.ResultUID);
        //                        string thairesult = TranslateResult.TranslateResultXray(result.PlainText, item.ResultStatus, dtResultMapping, ref listNoMapResult);

        //                        rpt.Parameters["ResultUID"].Value = item.ResultUID;
        //                        rpt.Parameters["ResultThai"].Value = thairesult;

        //                        rpt.RequestParameters = false;
        //                        rpt.ShowPrintMarginsWarning = false;
        //                        printTool.Print();
        //                    }
        //                }
        //                item.IsSelected = false;
        //                //CheckSelectAll();
        //                OnUpdateEvent();
        //            }

        //        }
        //        else if (SelectRequestExam != null)
        //        {
        //            if (SelectReport.Key == 0)
        //            {
        //                if (SelectRequestExam.ResultUID != 0)
        //                {
        //                    ImagingReport rpt = new ImagingReport();
        //                    ReportPrintTool printTool = new ReportPrintTool(rpt);


        //                    rpt.Parameters["ResultUID"].Value = SelectRequestExam.ResultUID;
        //                    rpt.RequestParameters = false;
        //                    rpt.ShowPrintMarginsWarning = false;
        //                    printTool.Print();
        //                }
        //            }
        //            else if (SelectReport.Key == 1)
        //            {
        //                List<XrayTranslateMappingModel> dtResultMapping = DataService.Radiology.GetXrayTranslateMapping();
        //                if (SelectRequestExam.ResultUID != 0)
        //                {
        //                    ImagingReportThai rpt = new ImagingReportThai();
        //                    ReportPrintTool printTool = new ReportPrintTool(rpt);

        //                    List<string> listNoMapResult = new List<string>();
        //                    ResultRadiologyModel result = DataService.Radiology.GetResultRadiologyByResultUID(SelectRequestExam.ResultUID);
        //                    string thairesult = TranslateResult.TranslateResultXray(result.PlainText
        //                        , SelectRequestExam.ResultStatus, dtResultMapping, ref listNoMapResult);

        //                    rpt.Parameters["ResultUID"].Value = SelectRequestExam.ResultUID;
        //                    rpt.Parameters["ResultThai"].Value = thairesult;

        //                    rpt.RequestParameters = false;
        //                    rpt.ShowPrintMarginsWarning = false;
        //                    printTool.Print();
        //                }
        //            }
        //        }
        //    }

        //}
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

        public void CheckSelectAll()
        {
            int allRow = RequestExamList.Count();
            int selectRow = RequestExamList.Count(p => p.IsSelected);
            int unSelectRow = RequestExamList.Count(p => p.IsSelected == false);
            SurpassSelectAll = true;
            if (allRow == selectRow)
            {
                IsSelectedAll = true;
                VisibilityCount = System.Windows.Visibility.Visible;
                CountSelect = "Count : " + allRow;
            }
            else if (allRow == unSelectRow)
            {
                IsSelectedAll = false;
                VisibilityCount = System.Windows.Visibility.Hidden;
                CountSelect = "";
            }
            else if (selectRow > 0)
            {
                IsSelectedAll = null;
                if (selectRow > 1)
                {
                    VisibilityCount = System.Windows.Visibility.Visible;
                    CountSelect = "Count : " + selectRow;
                }

            }

        }

        #endregion
    }
}
