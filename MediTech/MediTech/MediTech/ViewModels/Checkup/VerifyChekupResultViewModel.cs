using DevExpress.XtraReports.UI;
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
        private List<InsuranceCompanyModel> _InsuranceCompany;

        public List<InsuranceCompanyModel> InsuranceCompany
        {
            get { return _InsuranceCompany; }
            set { Set(ref _InsuranceCompany, value); }
        }

        private InsuranceCompanyModel _SelectInsuranceCompany;

        public InsuranceCompanyModel SelectInsuranceCompany
        {
            get { return _SelectInsuranceCompany; }
            set
            {
                Set(ref _SelectInsuranceCompany, value);
                if (_SelectInsuranceCompany != null)
                {
                    CheckupJobContactList = DataService.Checkup.GetCheckupJobContactByPayorDetailUID(_SelectInsuranceCompany.InsuranceCompanyUID);
                    SelectCheckupJobContact = CheckupJobContactList.OrderByDescending(p => p.StartDttm).FirstOrDefault();
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
                        ResultComponents = DataService.Checkup.GetVitalSignCumulative(SelectPatientVisit.PatientUID, SelectPatientVisit.PatientVisitUID);

                        VisibilityLab = Visibility.Visible;
                        VisibilityRadiology = Visibility.Collapsed;
                    }
                    else if (SelectGroupResult.Key == 3179 || SelectGroupResult.Key == 3180 || SelectGroupResult.Key == 3181 || SelectGroupResult.Key == 4258)
                    {
                        ResultRadiologys = DataService.Radiology.GetResultRadiologyByVisitUID(SelectPatientVisit.PatientVisitUID);
                        VisibilityLab = Visibility.Collapsed;
                        VisibilityRadiology = Visibility.Visible;

                        ResultRadiologys = SelectGroupResult.Key == 3179 ? ResultRadiologys.Where(p => p.RequestItemName.ToLower().Contains("chest")).ToList()
            : SelectGroupResult.Key == 3180 ? ResultRadiologys.Where(p => p.RequestItemName.ToLower().Contains("mammo")).ToList() :
            SelectGroupResult.Key == 3181 ? ResultRadiologys.Where(p => p.RequestItemName.ToLower().Contains("ultrasound") && !p.RequestItemName.ToLower().Contains("thyroid")).ToList() :
            SelectGroupResult.Key == 4258 ? ResultRadiologys.Where(p => p.RequestItemName.ToLower().Contains("thyroid")).ToList() : null;
                    }
                    else
                    {
                        ResultComponents = DataService.Checkup.GetGroupResultCumulative(SelectPatientVisit.PatientUID, SelectPatientVisit.PatientVisitUID, SelectGroupResult.Key.Value, SelectPatientVisit.PayorDetailUID);
                        VisibilityLab = Visibility.Visible;
                        VisibilityRadiology = Visibility.Collapsed;
                    }
                    TextSummeryReslt = string.Empty;
                    CheckupGroupResult = DataService.Checkup.GetCheckupGroupResultByVisit(SelectPatientVisit.PatientVisitUID, SelectGroupResult.Key.Value);
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
                ListFitnessTestResult = null;
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

                        if (groupResults != null && groupResults.Count > 0)
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

                            var groupFitnessUID = groupResults.Where(p => p.GPRSTUID == 4238).FirstOrDefault();

                            List<CheckupGroupResultModel> groupFitnessResult = new List<CheckupGroupResultModel>();
                            List<CheckupGroupResultModel> groupResultOther = groupResults;
                            
                            if (groupFitnessUID != null)
                            {
                                var data = groupResults.Where(p => groupFitnessTest.Any(n => n == p.GPRSTUID));
                                groupFitnessResult = data.ToList();

                                var resultOther = groupResults.Where(p => !groupFitnessTest.Any(n => n == p.GPRSTUID));
                                groupResultOther = resultOther.ToList();
                            }

                            //var groupResultOther = groupResults.Where(p => !groupFitnessTest.Any(n => n == p.GPRSTUID));
                            //var groupFitnessResult = groupResults.Where(p => groupFitnessTest.Any(n => n == p.GPRSTUID));

                            foreach (var groupResult in groupResultOther)
                            {
                                if (ListGroupResult == null)
                                {
                                    ListGroupResult = new List<string>();
                                }
                                ListGroupResult.Add("O " + groupResult.GroupResult + " : " + groupResult.Conclusion);
                            }
                            
                            foreach (var groupResult in groupFitnessResult)
                            {
                                if (ListFitnessTestResult == null)
                                {
                                    ListFitnessTestResult = new List<string>();
                                    ListFitnessTestResult.Add("O " + "Fitness test :");
                                }
                                ListFitnessTestResult.Add("- " + groupResult.GroupResult + " : " + groupResult.Conclusion);
                            }

                            StringBuilder sb = new StringBuilder();

                            if (ListGroupResult != null)
                                foreach (var resultvalue in ListGroupResult)
                                {
                                    if (!string.IsNullOrEmpty(resultvalue))
                                        sb.AppendLine(resultvalue);
                                }

                            if (ListFitnessTestResult != null)
                            {
                                foreach (var resultvalue in ListFitnessTestResult)
                                {
                                    if (!string.IsNullOrEmpty(resultvalue))
                                        sb.AppendLine(resultvalue);
                                }
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

        private List<string> _ListFitnessTestResult;
        public List<string> ListFitnessTestResult
        {
            get { return _ListFitnessTestResult; }
            set { _ListFitnessTestResult = value; }
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

        private RelayCommand _PreviewWellnessCommand;
        public RelayCommand PreviewWellnessCommand
        {
            get { return _PreviewWellnessCommand ?? (_PreviewWellnessCommand = new RelayCommand(PreviewWellness)); }
        }
        #endregion

        #region Method


        int[] groupFitnessTest = new int[] { 4265, 4269, 4270, 4271, 4272, 4273 };

        public VerifyChekupResultViewModel()
        {
            InsuranceCompany = DataService.Billing.GetInsuranceCompanyAll();

            ResultStatus = DataService.Technical.GetReferenceValueMany("RABSTS");
            DateFrom = DateTime.Now;
            DateTo = null;
        }

        public void SearchPatientVisit()
        {
            if (SelectInsuranceCompany == null)
            {
                WarningDialog("กรุณาเลือก Payor");
                return;
            }

            long? patientUID = null;
            if (SearchPatientCriteria != "" && SelectedPateintSearch != null)
            {
                patientUID = SelectedPateintSearch.PatientUID;
            }

            int? insuranceCompanyUID = SelectInsuranceCompany != null ? SelectInsuranceCompany.InsuranceCompanyUID : (int?)null;
            int? chekcupJobContactUID = SelectCheckupJobContact != null ? SelectCheckupJobContact.CheckupJobContactUID : (int?)null;
            PatientVisits = DataService.Checkup.SearchPatientCheckup(DateFrom, DateTo, patientUID, insuranceCompanyUID, chekcupJobContactUID);

        }

        void SaveCheckupGroupResult()
        {
            try
            {
                if (SelectPatientVisit == null)
                {
                    WarningDialog("กรุณาเลือกผู้ป่วย");
                    return;
                }
                if (SelectGroupResult == null)
                {
                    WarningDialog("กรุณาเลือก Group Result ที่จะทำการแก้ไขข้อมูล");
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
                if (ListFitnessTestResult == null)
                {
                    ListFitnessTestResult = new List<string>();
                }
                if (CheckupGroupResult == null)
                {
                    CheckupGroupResult = new CheckupGroupResultModel();
                }
                CheckupGroupResult.PatientUID = SelectPatientVisit.PatientUID;
                CheckupGroupResult.PatientVisitUID = SelectPatientVisit.PatientVisitUID;
                CheckupGroupResult.GPRSTUID = SelectGroupResult.Key.Value;
                CheckupGroupResult.Conclusion = TextSummeryReslt.Trim();
                CheckupGroupResult.RABSTSUID = SelectResultStatus.Key.Value;
                DataService.Checkup.SaveCheckupGroupResult(CheckupGroupResult, AppUtil.Current.UserID);

                StringBuilder sb = new StringBuilder();

                if (!groupFitnessTest.Any(n => n == SelectGroupResult.Key))
                {
                    int index = ListGroupResult.FindIndex(n => n.Contains(SelectGroupResult.Display));


                    if (index >= 0)
                    {
                        ListGroupResult[index] = "O " + SelectGroupResult.Display + " : " + TextSummeryReslt.Trim();
                        List<int> indexRemove = new List<int>();
                        for (int i = index + 1; i < ListGroupResult.Count; i++)
                        {
                            if (!ListGroupResult[i].StartsWith("O"))
                                indexRemove.Add(i);
                            else
                                break;
                        }
                        if (indexRemove.Count > 0)
                        {
                            ListGroupResult.RemoveRange(indexRemove.Min(), indexRemove.Count);
                        }
                    }
                    else
                    {
                        ListGroupResult.Add("O " + SelectGroupResult.Display + " : " + TextSummeryReslt.Trim());
                    }
                }

                if (groupFitnessTest.Any(n => n == SelectGroupResult.Key))
                {
                    int indexFitness = ListFitnessTestResult.FindIndex(n => n.Contains(SelectGroupResult.Display));

                    if (indexFitness >= 0)
                    {
                        ListFitnessTestResult[indexFitness] = "- " + SelectGroupResult.Display + " : " + TextSummeryReslt.Trim();
                        List<int> indexRemove = new List<int>();
                        for (int i = indexFitness + 1; i < ListFitnessTestResult.Count; i++)
                        {
                            if (!ListFitnessTestResult[i].StartsWith("-"))
                                indexRemove.Add(i);
                            else
                                break;
                        }
                        if (indexRemove.Count > 0)
                        {
                            ListFitnessTestResult.RemoveRange(indexRemove.Min(), indexRemove.Count);
                        }
                    }
                    else
                    {
                        ListFitnessTestResult.Add("- " + SelectGroupResult.Display + " : " + TextSummeryReslt.Trim());
                    }
                }




                foreach (var result in ListGroupResult)
                {
                    if (!string.IsNullOrEmpty(result))
                        sb.AppendLine(result);
                }

                foreach (var resultvalue in ListFitnessTestResult)
                {
                    if (!string.IsNullOrEmpty(resultvalue))
                        sb.AppendLine(resultvalue);
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
                    WarningDialog("กรุณาเลือกผู้ป่วย");
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

        void PreviewWellness()
        {
            if (SelectPatientVisit != null)
            {
                //Reports.Operating.Checkup.CheckupBookReport.CheckupPage1 rpt = new Reports.Operating.Checkup.CheckupBookReport.CheckupPage1();
                Reports.Operating.Checkup.CheckupBookA5.CheckupPage1 rpt = new Reports.Operating.Checkup.CheckupBookA5.CheckupPage1();
                rpt.Parameters["PatientUID"].Value = SelectPatientVisit.PatientUID;
                rpt.Parameters["PatientVisitUID"].Value = SelectPatientVisit.PatientVisitUID;
                rpt.Parameters["PayorDetailUID"].Value = SelectPatientVisit.PayorDetailUID;
                rpt.PreviewWellness = WellnessResult;
                ReportPrintTool printTool = new ReportPrintTool(rpt);
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, null, "");
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
