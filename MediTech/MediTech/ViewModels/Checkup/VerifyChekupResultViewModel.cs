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
                        ResultComponents = DataService.Checkup.GetResultCumulative(SelectPatientVisit.PatientUID, Convert.ToInt32(SelectGroupResult.Key2));
                    }
                    TextSummeryReslt = string.Empty;
                    IsAbnormal = false;
                    CheckupResult = DataService.Checkup.GetCheckupSummeryResultByVisit(SelectPatientVisit.PatientVisitUID, SelectGroupResult.Key);
                    if (CheckupResult != null)
                    {
                        TextSummeryReslt = CheckupResult.SummeryResult;
                        if (CheckupResult.RABSTSUID == 2882)
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
                    GroupResults = DataService.Checkup.GetCheckupGroupByVisitUID(_SelectPatientVisit.PatientVisitUID);
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

        private bool _IsAbnormal;

        public bool IsAbnormal
        {
            get { return _IsAbnormal; }
            set { Set(ref _IsAbnormal, value); }
        }

        private CheckupSummeryResultModel _CheckupResult;

        public CheckupSummeryResultModel CheckupResult
        {
            get { return _CheckupResult; }
            set { _CheckupResult = value; }
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

        private RelayCommand _SaveCheckupResultCommand;

        public RelayCommand SaveCheckupResultCommand
        {
            get { return _SaveCheckupResultCommand ?? (_SaveCheckupResultCommand = new RelayCommand(SaveCheckupResult)); }
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

        void SaveCheckupResult()
        {

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
