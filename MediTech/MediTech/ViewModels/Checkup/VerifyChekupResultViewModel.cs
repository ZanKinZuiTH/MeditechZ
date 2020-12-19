using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                if (_SelectGroupResult != null)
                {
                    if (SelectGroupResult.Key == 3177 | SelectGroupResult.Key == 3178)
                    {
                        ResultComponents = DataService.Checkup.GetVitalSignCumulative(SelectPatientVisit.PatientUID);
                    }
                    else
                    {
                        ResultComponents = DataService.Checkup.GetGroupResultCumulative(SelectPatientVisit.PatientUID, SelectGroupResult.Key);
                    }
                    TextSummeryReslt = string.Empty;
                    IsAbnormal = false;
                    CheckupGroupResult = DataService.Checkup.GetCheckupGroupResultByVisit(SelectPatientVisit.PatientVisitUID, SelectGroupResult.Key);
                    if (CheckupGroupResult != null)
                    {
                        TextSummeryReslt = CheckupGroupResult.Conclusion;
                        if (CheckupGroupResult.RABSTSUID == 2882)
                        {
                            IsAbnormal = true;
                        }
                    }
                }
                else
                {
                    ResultComponents = null;
                    TextSummeryReslt = string.Empty;
                    IsAbnormal = false;
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
                if (_SelectPatientVisit != null)
                {
                    WellnessData = null;
                    WellnessResult = null;
                    ListGroupResult = null;
                    GroupResults = DataService.Checkup.GetCheckupGroupByVisitUID(_SelectPatientVisit.PatientVisitUID);
                    var result = DataService.PatientHistory.GetWellnessDataByVisit(_SelectPatientVisit.PatientVisitUID);
                    if (result != null)
                    {
                        WellnessData = result.FirstOrDefault();
                        WellnessResult = WellnessData.WellnessResult;
                        string[] locResult = WellnessResult.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        ListGroupResult = locResult.ToList();
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

        private WellnessDataModel _WellnessData;

        public WellnessDataModel WellnessData
        {
            get { return _WellnessData; }
            set { _WellnessData = value; }
        }


        private bool _IsAbnormal;

        public bool IsAbnormal
        {
            get { return _IsAbnormal; }
            set { Set(ref _IsAbnormal, value); }
        }

        private CheckupGroupResultModel _CheckupGroupResult;

        public CheckupGroupResultModel CheckupGroupResult
        {
            get { return _CheckupGroupResult; }
            set { _CheckupGroupResult = value; }
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
            DateFrom = DateTime.Now;
            DateTo = null;
        }

        public void SearchPatientVisit()
        {
            string patientID = "";
            if (SearchPatientCriteria != "" && SelectedPateintSearch != null)
            {
                patientID = SelectedPateintSearch.PatientID;
            }

            int? payorDetailUID = SelectPayorDetail != null ? SelectPayorDetail.PayorDetailUID : (int?)null;
            int? chekcupJobContactUID = SelectCheckupJobContact != null ? SelectCheckupJobContact.CheckupJobContactUID : (int?)null;
            PatientVisits = DataService.PatientIdentity.SearchPatientVisit(patientID, "", "", null, null, DateFrom, DateTo, null, null, payorDetailUID, chekcupJobContactUID);
        }

        void SaveCheckupGroupResult()
        {
            try
            {
                if (ListGroupResult == null)
                {
                    ListGroupResult = new List<string>();
                }
                CheckupGroupResult.Conclusion = TextSummeryReslt.Trim();
                CheckupGroupResult.RABSTSUID = IsAbnormal ? 2882 : 2883;
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
                if (string.IsNullOrEmpty(WellnessResult))
                {
                    WarningDialog("กรุณากรอกข้อมูลในสรุปเล่มรายบุุคล");
                }

                if (WellnessData == null)
                {
                    WellnessData = new WellnessDataModel();
                    WellnessData.PatientUID = SelectPatientVisit.PatientUID;
                    WellnessData.PatientVisitUID = SelectPatientVisit.PatientVisitUID;
                }
                WellnessData.WellnessResult = WellnessResult;
                DataService.PatientHistory.ManageWellnessData(WellnessData, AppUtil.Current.UserID);
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
