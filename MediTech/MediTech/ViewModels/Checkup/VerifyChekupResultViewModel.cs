using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediTech.ViewModels
{
    public class VerifyChekupResultViewModel : MediTechViewModelBase
    {
        #region Properties
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
            set
            {
                Set(ref _SelectPayorDetail, value);
                if (_SelectPayorDetail != null)
                {
                    CheckupJobContactList = DataService.Checkup.GetCheckupJobContactByPayorDetailUID(_SelectPayorDetail.PayorDetailUID);
                }
            }
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
            set
            {
                Set(ref _SelectCheckupJobContact, value);
                if (_SelectCheckupJobContact != null)
                {
                    DateFrom = _SelectCheckupJobContact.StartDttm;
                    DateTo = _SelectCheckupJobContact.EndDttm;
                }
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
                    SearchPatientVisit();
                }
            }
        }

        #endregion

        private List<LookupReferenceValueModel> _GroupResults;

        public List<LookupReferenceValueModel> GroupResults
        {
            get { return _GroupResults; }
            set { Set(ref _GroupResults, value); }
        }

        private LookupReferenceValueModel _SelectGroupResult;

        public LookupReferenceValueModel SelectGroupResult
        {
            get { return _SelectGroupResult; }
            set
            {
                Set(ref _SelectGroupResult, value);
                ResultComponents = null;
                TextSummeryReslt = string.Empty;
                SelectResultStatus = null;
                if (_SelectGroupResult != null)
                {
                    if (SelectGroupResult.Key == 3177 || SelectGroupResult.Key == 3178)
                    {
                        ResultComponents = DataService.Checkup.GetVitalSignCumulative(SelectPatientVisit.PatientUID);

                        VisibilityLab = Visibility.Visible;
                        VisibilityRadiology = Visibility.Collapsed;
                    }
                    else if (SelectGroupResult.Key == 3179 || SelectGroupResult.Key == 3180 || SelectGroupResult.Key == 3181)
                    {
                        ResultRadiologys = DataService.Radiology.GetResultRadiologyByVisitUID(SelectPatientVisit.PatientVisitUID);
                        VisibilityLab = Visibility.Collapsed;
                        VisibilityRadiology = Visibility.Visible;

                        ResultRadiologys = SelectGroupResult.Key == 3179 ? ResultRadiologys.Where(p => p.RequestItemName.ToLower().Contains("chest")).ToList()
            : SelectGroupResult.Key == 3179 ? ResultRadiologys.Where(p => p.RequestItemName.ToLower().Contains("chest")).ToList() :
            SelectGroupResult.Key == 3179 ? ResultRadiologys.Where(p => p.RequestItemName.ToLower().Contains("chest")).ToList() : null;
                    }
                    else
                    {
                        ResultComponents = DataService.Checkup.GetGroupResultCumulative(SelectPatientVisit.PatientUID, SelectGroupResult.Key);
                        VisibilityLab = Visibility.Visible;
                        VisibilityRadiology = Visibility.Collapsed;
                    }
                    TextSummeryReslt = string.Empty;
                    CheckupGroupResult = DataService.Checkup.GetCheckupGroupResultByVisit(SelectPatientVisit.PatientVisitUID, SelectGroupResult.Key);
                    if (CheckupGroupResult != null)
                    {
                        TextSummeryReslt = CheckupGroupResult.Conclusion;
                        SelectResultStatus = ResultStatus.FirstOrDefault(p => p.Key == CheckupGroupResult.RABSTSUID);
                    }
                }
            }
        }

        private List<PatientVisitModel> _PatientVisits;

        public List<PatientVisitModel> PatientVisits
        {
            get { return _PatientVisits; }
            set { Set(ref _PatientVisits, value); }
        }

        private PatientVisitModel _SelectPatientVisit;

        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set
            {
                Set(ref _SelectPatientVisit, value);
                WellnessData = null;
                WellnessResult = null;
                ListGroupResult = null;
                SelectResultStatus = null;
                ResultComponents = null;
                ResultRadiologys = null;
                TextSummeryReslt = null;
                if (_SelectPatientVisit != null)
                {
                    GroupResults = DataService.Checkup.GetCheckupGroupByVisitUID(_SelectPatientVisit.PatientVisitUID);
                    var result = DataService.PatientHistory.GetWellnessDataByVisit(_SelectPatientVisit.PatientVisitUID);
                    if (result != null && result.Count > 0)
                    {
                        WellnessData = result.FirstOrDefault();
                        WellnessResult = WellnessData.WellnessResult;
                        string[] locResult = WellnessResult.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        ListGroupResult = locResult.ToList();
                    }
                    else
                    {
                        var groupResults = DataService.Checkup.GetCheckupGroupResultListByVisit(_SelectPatientVisit.PatientUID, _SelectPatientVisit.PatientVisitUID);

                        if (groupResults != null)
                        {
                            foreach (var groupResult in groupResults)
                            {
                                var data = GroupResults.FirstOrDefault(p => p.Key == groupResult.GPRSTUID);
                                if (data != null)
                                {
                                    groupResult.DisplayOrder = data.DisplayOrder;
                                }
                            }
                            groupResults = groupResults.OrderBy(p => p.DisplayOrder).ToList();

                            foreach (var groupResult in groupResults)
                            {
                                if (ListGroupResult == null)
                                {
                                    ListGroupResult = new List<string>();
                                }
                                ListGroupResult.Add("O " + groupResult.GroupResult + " : " + groupResult.Conclusion);
                            }

                            StringBuilder sb = new StringBuilder();
                            foreach (var resultvalue in ListGroupResult)
                            {
                                if (!string.IsNullOrEmpty(resultvalue))
                                    sb.AppendLine(resultvalue);
                            }

                            WellnessResult = sb.ToString();
                        }
                    }
                }
            }
        }

        private List<PatientResultComponentModel> _ResultComponents;

        public List<PatientResultComponentModel> ResultComponents
        {
            get { return _ResultComponents; }
            set { Set(ref _ResultComponents, value); }
        }

        private PatientResultComponentModel _SelectResultComponent;

        public PatientResultComponentModel SelectResultComponent
        {
            get { return _SelectResultComponent; }
            set
            {
                Set(ref _SelectResultComponent, value);
            }
        }

        private List<ResultRadiologyModel> _ResultRadiologys;

        public List<ResultRadiologyModel> ResultRadiologys
        {
            get { return _ResultRadiologys; }
            set { Set(ref _ResultRadiologys, value); }
        }

        private string _WellnessResult;

        public string WellnessResult
        {
            get { return _WellnessResult; }
            set { Set(ref _WellnessResult, value); }
        }

        private string _TextSummeryReslt;

        public string TextSummeryReslt
        {
            get { return _TextSummeryReslt; }
            set { Set(ref _TextSummeryReslt, value); }
        }

        private List<string> _ListGroupResult;

        public List<string> ListGroupResult
        {
            get { return _ListGroupResult; }
            set { _ListGroupResult = value; }
        }

        private List<LookupReferenceValueModel> _ResultStatus;

        public List<LookupReferenceValueModel> ResultStatus
        {
            get { return _ResultStatus; }
            set { Set(ref _ResultStatus, value); }
        }

        private LookupReferenceValueModel _SelectResultStatus;

        public LookupReferenceValueModel SelectResultStatus
        {
            get { return _SelectResultStatus; }
            set
            {
                Set(ref _SelectResultStatus, value);
            }
        }

        private WellnessDataModel _WellnessData;

        public WellnessDataModel WellnessData
        {
            get { return _WellnessData; }
            set { _WellnessData = value; }
        }



        private CheckupGroupResultModel _CheckupGroupResult;

        public CheckupGroupResultModel CheckupGroupResult
        {
            get { return _CheckupGroupResult; }
            set { _CheckupGroupResult = value; }
        }


        private Visibility _VisibilityLab = Visibility.Visible;

        public Visibility VisibilityLab
        {
            get { return _VisibilityLab; }
            set { Set(ref _VisibilityLab, value); }
        }

        private Visibility _VisibilityRadiology = Visibility.Collapsed;

        public Visibility VisibilityRadiology
        {
            get { return _VisibilityRadiology; }
            set { Set(ref _VisibilityRadiology, value); }
        }

        #endregion

        #region Command


        private RelayCommand _PatientSearchCommand;

        public RelayCommand PatientSearchCommand
        {
            get { return _PatientSearchCommand ?? (_PatientSearchCommand = new RelayCommand(PatientSearch)); }
        }

        private RelayCommand _SearchPatientVisitCommand;

        public RelayCommand SearchPatientVisitCommand
        {
            get { return _SearchPatientVisitCommand ?? (_SearchPatientVisitCommand = new RelayCommand(SearchPatientVisit)); }
        }

        private RelayCommand _SaveCheckupGroupResultCommand;

        public RelayCommand SaveCheckupGroupResultCommand
        {
            get { return _SaveCheckupGroupResultCommand ?? (_SaveCheckupGroupResultCommand = new RelayCommand(SaveCheckupGroupResult)); }
        }

        private RelayCommand _SaveWellNessResultCommand;

        public RelayCommand SaveWellNessResultCommand
        {
            get { return _SaveWellNessResultCommand ?? (_SaveWellNessResultCommand = new RelayCommand(SaveWellNessResult)); }
        }
        #endregion

        #region Method
        public VerifyChekupResultViewModel()
        {
            PayorDetails = DataService.MasterData.GetPayorDetail();
            ResultStatus = DataService.Technical.GetReferenceValueMany("RABSTS");
            DateFrom = DateTime.Now;
            DateTo = null;
        }

        public void SearchPatientVisit()
        {
            if (SelectPayorDetail == null)
            {
                WarningDialog("กรุณาเลือก Payor");
                return;
            }

            long? patientUID = null;
            if (SearchPatientCriteria != "" && SelectedPateintSearch != null)
            {
                patientUID = SelectedPateintSearch.PatientUID;
            }

            int? payorDetailUID = SelectPayorDetail != null ? SelectPayorDetail.PayorDetailUID : (int?)null;
            int? chekcupJobContactUID = SelectCheckupJobContact != null ? SelectCheckupJobContact.CheckupJobContactUID : (int?)null;
            PatientVisits = DataService.Checkup.SearchPatientCheckup(DateFrom, DateTo, patientUID, payorDetailUID, chekcupJobContactUID);

        }

        void SaveCheckupGroupResult()
        {
            try
            {
                if (SelectPatientVisit == null)
                {
                    WarningDialog("กรุณาเลือกคนไข้");
                    return;
                }
                if (string.IsNullOrEmpty(TextSummeryReslt.Trim()))
                {
                    WarningDialog("กรุณากรอกข้อมูลในช่องสรุปผล");
                    return;
                }

                if (SelectResultStatus == null)
                {
                    WarningDialog("กรุณาเลือก Status");
                    return;
                }

                if (ListGroupResult == null)
                {
                    ListGroupResult = new List<string>();
                }
                CheckupGroupResult.PatientUID = SelectPatientVisit.PatientUID;
                CheckupGroupResult.PatientVisitUID = SelectPatientVisit.PatientVisitUID;
                CheckupGroupResult.Conclusion = TextSummeryReslt.Trim();
                CheckupGroupResult.RABSTSUID = SelectResultStatus.Key;
                DataService.Checkup.SaveCheckupGroupResult(CheckupGroupResult, AppUtil.Current.UserID);

                StringBuilder sb = new StringBuilder();
                int index = ListGroupResult.FindIndex(n => n.Contains(SelectGroupResult.Display));
                if (index >= 0)
                {
                    index = ListGroupResult.FindIndex(n => n.Contains(SelectGroupResult.Display));
                    ListGroupResult[index] = "O " + SelectGroupResult.Display + " : " + TextSummeryReslt.Trim();
                }
                else
                {
                    ListGroupResult.Add("O " + SelectGroupResult.Display + " : " + TextSummeryReslt.Trim());
                }
                foreach (var result in ListGroupResult)
                {
                    if (!string.IsNullOrEmpty(result))
                        sb.AppendLine(result);
                }

                WellnessResult = sb.ToString();

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        void SaveWellNessResult()
        {
            try
            {
                if (SelectPatientVisit == null)
                {
                    WarningDialog("กรุณาเลือกคนไข้");
                    return;
                }
                if (string.IsNullOrEmpty(WellnessResult))
                {
                    WarningDialog("กรุณากรอกข้อมูลในช่องสรุปเล่มรายบุุคล");
                    return;
                }

                if (WellnessData == null)
                {
                    WellnessData = new WellnessDataModel();
                    WellnessData.PatientUID = SelectPatientVisit.PatientUID;
                    WellnessData.PatientVisitUID = SelectPatientVisit.PatientVisitUID;
                }
                WellnessData.WellnessResult = WellnessResult;
                DataService.PatientHistory.ManageWellnessData(WellnessData, AppUtil.Current.UserID);
                SelectPatientVisit.IsWellnessResult = true;
                SaveSuccessDialog();
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
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
