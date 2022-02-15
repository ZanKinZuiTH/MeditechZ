using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using MediTech.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.Data;
using System.Windows.Forms;
using MediTech.Helpers;

namespace MediTech.ViewModels
{
    public class TranslateCheckupResultViewModel : MediTechViewModelBase
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
                    SearchPatientVisit();
                    SearchPatientCriteria = string.Empty;
                }
            }
        }

        #endregion

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
                    foreach (var jobTask in CheckupJobTasks)
                    {
                        jobTask.IsSelected = IsSelectedAll ?? false;
                    }
                    OnUpdateEvent();
                }
                SurpassSelectAll = false;
            }
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
            set
            {
                Set(ref _SelectPayorDetail, value);
                if (_SelectPayorDetail != null)
                {
                    CheckupJobContactList = DataService.Checkup.GetCheckupJobContactByPayorDetailUID(_SelectPayorDetail.PayorDetailUID);
                    SelectCheckupJobContact = CheckupJobContactList.OrderByDescending(p => p.StartDttm).FirstOrDefault();
                }
            }
        }

        private PayorDetailModel _SelectPayorDetail2;

        public PayorDetailModel SelectPayorDetail2
        {
            get { return _SelectPayorDetail2; }
            set
            {
                Set(ref _SelectPayorDetail2, value);
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
                if (SelectCheckupJobContact != null)
                {
                    CheckupJobTasks = new ObservableCollection<CheckupJobTaskModel>(DataService.Checkup.GetCheckupJobTaskByJobUID(SelectCheckupJobContact.CheckupJobContactUID));
                }
            }
        }


        private ObservableCollection<CheckupJobTaskModel> _CheckupJobTasks;

        public ObservableCollection<CheckupJobTaskModel> CheckupJobTasks
        {
            get { return _CheckupJobTasks; }
            set { Set(ref _CheckupJobTasks, value); }
        }

        private CheckupJobTaskModel _SelectCheckupJobTask;

        public CheckupJobTaskModel SelectCheckupJobTask
        {
            get { return _SelectCheckupJobTask; }
            set
            {
                Set(ref _SelectCheckupJobTask, value);
            }
        }


        private ObservableCollection<Column> _ColumnsResultItems;

        public ObservableCollection<Column> ColumnsResultItems
        {
            get { return _ColumnsResultItems; }
            set { Set(ref _ColumnsResultItems, value); }
        }

        List<XrayTranslateMappingModel> dtResultMapping;

        private DateTime _JobDateFrom;

        public DateTime JobDateFrom
        {
            get { return _JobDateFrom; }
            set { Set(ref _JobDateFrom, value); }
        }

        private DateTime _JobDateTo;

        public DateTime JobDateTo
        {
            get { return _JobDateTo; }
            set { Set(ref _JobDateTo, value); }
        }

        private DateTime _DateFrom;

        public DateTime DateFrom
        {
            get { return _DateFrom; }
            set { Set(ref _DateFrom, value); }
        }

        private DateTime _DateTo;

        public DateTime DateTo
        {
            get { return _DateTo; }
            set { Set(ref _DateTo, value); }
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
            set { Set(ref _SelectPatientVisit, value); }
        }

        private ObservableCollection<PatientVisitModel> _SelectPatientVisits;

        public ObservableCollection<PatientVisitModel> SelectPatientVisits
        {
            get { return _SelectPatientVisits ?? (_SelectPatientVisits = new ObservableCollection<PatientVisitModel>()); }
            set { Set(ref _SelectPatientVisits, value); }
        }


        #endregion

        #region Command

        private RelayCommand _TranslateSpecificCommmand;

        public RelayCommand TranslateSpecificCommmand
        {
            get
            {
                return _TranslateSpecificCommmand
                    ?? (_TranslateSpecificCommmand = new RelayCommand(TranslateSpecific));
            }
        }


        private RelayCommand _TranslateAllCommand;

        public RelayCommand TranslateAllCommand
        {
            get
            {
                return _TranslateAllCommand
                    ?? (_TranslateAllCommand = new RelayCommand(TranslateAll));
            }
        }

        private RelayCommand _TranslateNonConfirmCommand;

        public RelayCommand TranslateNonConfirmCommand
        {
            get
            {
                return _TranslateNonConfirmCommand
                    ?? (_TranslateNonConfirmCommand = new RelayCommand(TranslateNonConfirm));
            }
        }



        private RelayCommand _TranslatePatientCommand;

        public RelayCommand TranslatePatientCommand
        {
            get
            {
                return _TranslatePatientCommand
                    ?? (_TranslatePatientCommand = new RelayCommand(TranslatePatient));
            }
        }



        private RelayCommand _LoadDataCommand;

        public RelayCommand LoadDataCommand
        {
            get
            {
                return _LoadDataCommand
                    ?? (_LoadDataCommand = new RelayCommand(LoadData));
            }
        }

        private RelayCommand _ExportDataCommand;

        public RelayCommand ExportDataCommand
        {
            get
            {
                return _ExportDataCommand
                    ?? (_ExportDataCommand = new RelayCommand(ExportData));
            }
        }


        private RelayCommand _SearchPatientVisitCommand;

        public RelayCommand SearchPatientVisitCommand
        {
            get
            {
                return _SearchPatientVisitCommand
                    ?? (_SearchPatientVisitCommand = new RelayCommand(SearchPatientVisit));
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

        #endregion

        #region Method
        public TranslateCheckupResultViewModel()
        {
            DateTime now = DateTime.Now;
            PayorDetails = DataService.MasterData.GetPayorDetail();
            DateFrom = now;
            DateTo = now;
            JobDateFrom = now;
            JobDateTo = now;
        }

        void TranslateSpecific()
        {
            try
            {
                if (SelectCheckupJobContact != null)
                {
                    TranslateCheckupResult view = (TranslateCheckupResult)this.View;
                    List<int> GPRSTUIDs = CheckupJobTasks.Where(p => p.IsSelected).Select(p => p.GPRSTUID).ToList();
                    if (GPRSTUIDs != null && GPRSTUIDs.Count > 0)
                    {
                        if (GPRSTUIDs.Any(p => p == 3179 || p == 3180 || p == 3181 || p == 4258))
                        {
                            dtResultMapping = DataService.Radiology.GetXrayTranslateMapping();
                        }
                        List<PatientVisitModel> visitData = DataService.Checkup.GetVisitCheckupGroupNonTran(SelectCheckupJobContact.CheckupJobContactUID, GPRSTUIDs);
                        List<CheckupRuleModel> dataCheckupRule = DataService.Checkup.GetCheckupRuleGroupList(GPRSTUIDs);
                        view.SetProgressBarLimits(0, visitData.Count());
                        int loopCount = 0;
                        foreach (var patientVisit in visitData)
                        {
                            TranslateProcess(patientVisit, GPRSTUIDs, dataCheckupRule);

                            loopCount++;
                            view.SetProgressBarValue(loopCount);
                        }
                    }
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }
        void TranslateAll()
        {
            try
            {
                if (SelectCheckupJobContact != null)
                {
                    TranslateCheckupResult view = (TranslateCheckupResult)this.View;
                    List<int> GPRSTUIDs = CheckupJobTasks.Where(p => p.IsSelected).Select(p => p.GPRSTUID).ToList();
                    if (GPRSTUIDs != null && GPRSTUIDs.Count > 0)
                    {
                        if (GPRSTUIDs.Any(p => p == 3179 || p == 3180 || p == 3181 || p == 4258))
                        {
                            dtResultMapping = DataService.Radiology.GetXrayTranslateMapping();
                        }
                        List<PatientVisitModel> visitData = DataService.Checkup.GetVisitCheckupGroup(SelectCheckupJobContact.CheckupJobContactUID, GPRSTUIDs);
                        List<CheckupRuleModel> dataCheckupRule = DataService.Checkup.GetCheckupRuleGroupList(GPRSTUIDs);
                        view.SetProgressBarLimits(0, visitData.Count());
                        int loopCount = 0;
                        foreach (var patientVisit in visitData)
                        {
                            TranslateProcess(patientVisit, GPRSTUIDs, dataCheckupRule);

                            loopCount++;
                            view.SetProgressBarValue(loopCount);
                        }
                    }
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        void TranslateNonConfirm()
        {
            try
            {
                if (SelectCheckupJobContact != null)
                {
                    TranslateCheckupResult view = (TranslateCheckupResult)this.View;
                    List<int> GPRSTUIDs = CheckupJobTasks.Where(p => p.IsSelected).Select(p => p.GPRSTUID).ToList();
                    if (GPRSTUIDs != null && GPRSTUIDs.Count > 0)
                    {
                        if (GPRSTUIDs.Any(p => p == 3179 || p == 3180 || p == 3181 || p == 4258))
                        {
                            dtResultMapping = DataService.Radiology.GetXrayTranslateMapping();
                        }
                        List<PatientVisitModel> visitData = DataService.Checkup.GetVisitCheckupGroupNonConfirm(SelectCheckupJobContact.CheckupJobContactUID, GPRSTUIDs);
                        List<CheckupRuleModel> dataCheckupRule = DataService.Checkup.GetCheckupRuleGroupList(GPRSTUIDs);
                        view.SetProgressBarLimits(0, visitData.Count());
                        int loopCount = 0;
                        foreach (var patientVisit in visitData)
                        {
                            TranslateProcess(patientVisit, GPRSTUIDs, dataCheckupRule);

                            loopCount++;
                            view.SetProgressBarValue(loopCount);
                        }
                    }
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        void TranslatePatient()
        {
            try
            {
                if (SelectPatientVisits != null && SelectPatientVisits.Count > 0)
                {
                    foreach (var visit in SelectPatientVisits.ToList())
                    {
                        var groupResults = DataService.Checkup.GetCheckupGroupByVisitUID(visit.PatientVisitUID);
                        if (groupResults != null)
                        {
                            List<int> GPRSTUIDs = groupResults.Select(p => p.Key.Value).ToList();
                            List<CheckupRuleModel> dataCheckupRule = DataService.Checkup.GetCheckupRuleGroupList(GPRSTUIDs);
                            if (GPRSTUIDs.Any(p => p == 3179 || p == 3180 || p == 3181 || p == 4258))
                            {
                                dtResultMapping = DataService.Radiology.GetXrayTranslateMapping();
                            }
                            TranslateProcess(visit, GPRSTUIDs, dataCheckupRule);
                        }
                        SelectPatientVisits.Remove(visit);
                    }
                    SaveSuccessDialog();
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        void TranslateProcess(PatientVisitModel patientVisit, List<int> GPRSTUIDs, List<CheckupRuleModel> dataCheckupRule)
        {
            try
            {
                int? ageInt = !string.IsNullOrEmpty(patientVisit.Age) ? int.Parse(patientVisit.Age) : (int?)null;
                List<ResultComponentModel> resultComponent = new List<ResultComponentModel>();
                var dataVital = DataService.PatientHistory.GetPatientVitalSignByVisitUID(patientVisit.PatientVisitUID);
                var radiology = DataService.Radiology.GetResultRadiologyByVisitUID(patientVisit.PatientVisitUID);
                var resultLab_Occ = DataService.Checkup.GetResultComponentByVisitUID(patientVisit.PatientVisitUID);
                if (dataVital != null && dataVital.Count > 0)
                {
                    var vitalSign = dataVital.OrderByDescending(p => p.RecordedDttm).FirstOrDefault();
                    if (vitalSign.BMIValue != null)
                    {
                        ResultComponentModel bmiComponent = new ResultComponentModel() { ResultItemUID = 328, GPRSTUID = 3177, ResultItemCode = "PEBMI", ResultItemName = "BMI (ดัชนีมวลกาย)", ResultValue = vitalSign.BMIValue.ToString() };
                        ResultComponentModel commentComponent = new ResultComponentModel() { ResultItemUID = 116, GPRSTUID = 3177, ResultItemCode = "PAR67", ResultItemName = "Comment", ResultValue = (vitalSign.IsPregnant ?? false) ? "ตั้งครรภ์" : "" };
                        resultComponent.Add(bmiComponent);
                        resultComponent.Add(commentComponent);
                    }

                    if (vitalSign.BPSys != null)
                    {
                        ResultComponentModel sdpComponent = new ResultComponentModel() { ResultItemUID = 329, GPRSTUID = 3178, ResultItemCode = "PESBP", ResultItemName = "ความดันโลหิต (SBP)", ResultValue = vitalSign.BPSys.ToString() };
                        resultComponent.Add(sdpComponent);
                    }
                    if (vitalSign.BPDio != null)
                    {
                        ResultComponentModel dbpComponent = new ResultComponentModel() { ResultItemUID = 330, GPRSTUID = 3178, ResultItemCode = "PEDBP", ResultItemName = "ความดันโลหิต (DBP)", ResultValue = vitalSign.BPDio.ToString() };
                        resultComponent.Add(dbpComponent);
                    }
                    if (vitalSign.Pulse != null)
                    {
                        ResultComponentModel pluseComponent = new ResultComponentModel() { ResultItemUID = 331, GPRSTUID = 3178, ResultItemCode = "PEPLUSE", ResultItemName = "ชีพจร(Pulse)", ResultValue = vitalSign.Pulse.ToString() };
                        resultComponent.Add(pluseComponent);
                    }

                }

                if (radiology != null && radiology.Count > 0)
                {
                    foreach (var item in radiology)
                    {
                        if (item.RequestItemName.ToLower().Contains("chest"))
                        {
                            ResultComponentModel newResultCom = new ResultComponentModel();
                            newResultCom.ResultItemUID = 342;
                            newResultCom.GPRSTUID = 3179;
                            newResultCom.ResultValue = item.ResultStatus;
                            resultComponent.Add(newResultCom);
                        }
                        else if (item.RequestItemName.ToLower().Contains("mammo"))
                        {
                            ResultComponentModel newResultCom = new ResultComponentModel();
                            newResultCom.ResultItemUID = 343;
                            newResultCom.GPRSTUID = 3180;
                            newResultCom.ResultValue = item.ResultStatus;
                            resultComponent.Add(newResultCom);
                        }
                        else if (item.RequestItemName.ToLower().Contains("ultrasound") && !item.RequestItemName.ToLower().Contains("thyroid"))
                        {
                            ResultComponentModel newResultCom = new ResultComponentModel();
                            newResultCom.ResultItemUID = 344;
                            newResultCom.GPRSTUID = 3181;
                            newResultCom.ResultValue = item.ResultStatus;
                            resultComponent.Add(newResultCom);
                        }
                        else if (item.RequestItemName.ToLower().Contains("thyroid"))
                        {
                            ResultComponentModel newResultCom = new ResultComponentModel();
                            newResultCom.ResultItemUID = 356;
                            newResultCom.GPRSTUID = 4258;
                            newResultCom.ResultValue = item.ResultStatus;
                            resultComponent.Add(newResultCom);
                        }
                    }
                }

                if (resultLab_Occ != null && resultLab_Occ.Count > 0)
                {
                    foreach (var result in resultLab_Occ)
                    {
                        resultComponent.Add(result);
                    }
                }

                if (resultComponent != null && resultComponent.Count > 0)
                {
                    foreach (var grpstUID in GPRSTUIDs)
                    {
                        if (resultComponent.Any(p => p.GPRSTUID == grpstUID))
                        {
                            List<CheckupRuleModel> ruleCheckupIsCorrect = new List<CheckupRuleModel>();
                            string wellNessResult = string.Empty;

                            var ruleCheckups = dataCheckupRule
                                .Where(p => p.GPRSTUID == grpstUID
                                && (p.SEXXXUID == 3 || p.SEXXXUID == patientVisit.SEXXXUID)
                                && ((p.AgeFrom == null && p.AgeTo == null) || (ageInt >= p.AgeFrom && ageInt <= p.AgeTo)
                                || (ageInt >= p.AgeFrom && p.AgeTo == null) || (p.AgeFrom == null && ageInt <= p.AgeTo))
                                && (p.RABSTSUID != 2883 || (p.RABSTSUID == 2883)
                                )).Select(p => new CheckupRuleModel
                                {
                                    CheckupRuleUID = p.CheckupRuleUID,
                                    Name = p.Name,
                                    SEXXXUID = p.SEXXXUID,
                                    AgeFrom = p.AgeFrom,
                                    AgeTo = p.AgeTo,
                                    RABSTSUID = p.RABSTSUID,
                                    GPRSTUID = p.GPRSTUID,
                                    CheckupRuleRecommend = p.CheckupRuleRecommend,
                                    CheckupRuleItem = p.CheckupRuleItem,
                                    CheckupRuleDescription = p.CheckupRuleDescription
                                    .Select(s => new CheckupRuleDescriptionModel
                                    {
                                        CheckupRuleUID = s.CheckupRuleUID,
                                        CheckupTextMasterUID = s.CheckupTextMasterUID,
                                        CheckupRuleDescriptionUID = s.CheckupRuleDescriptionUID,
                                        ThaiDescription = s.ThaiDescription,
                                        EngDescription = s.EngDescription
                                    }).ToList()
                                }).ToList();


                            foreach (var ruleCheckup in ruleCheckups)
                            {
                                bool isConrrect = false;
                                foreach (var ruleItem in ruleCheckup.CheckupRuleItem.OrderBy(p => p.Operator))
                                {
                                    var resultItemValue = resultComponent.FirstOrDefault(p => p.ResultItemUID == ruleItem.ResultItemUID);

                                    if (resultItemValue != null)
                                    {
                                        if (ruleItem.NonCheckup == true)
                                        {
                                            isConrrect = false;
                                            if (ruleItem.Operator == "And")
                                            {
                                                break;
                                            }
                                        }
                                        else if (!string.IsNullOrEmpty(ruleItem.Text))
                                        {
                                            string[] values = ruleItem.Text.Split(',');
                                            string[] resultValues = resultItemValue.ResultValue?.Split(',');
                                            bool flagCondition = false;

                                            if (!(ruleItem.NotEqual ?? false) && values.Any(p => resultValues.Any(x => x.ToLower().Trim() == p.ToLower().Trim())))
                                            {
                                                flagCondition = true;
                                            }
                                            else if ((ruleItem.NotEqual ?? false) && !values.Any(p => resultValues.Any(x => x.ToLower().Trim() == p.ToLower().Trim())))
                                            {
                                                flagCondition = true;
                                            }

                                            if (flagCondition)
                                            {
                                                isConrrect = true;
                                                if (ruleItem.Operator == "Or")
                                                {
                                                    var ruleDescription = ruleCheckup.CheckupRuleDescription.FirstOrDefault();
                                                    if (ruleDescription != null && ruleDescription.ThaiDescription.Contains("{0}"))
                                                    {
                                                        string thaiDescription = "";
                                                        for (int i = 0; i < resultValues.Count(); i++)
                                                        {
                                                            if (ruleCheckup.CheckupRuleItem.FirstOrDefault(p => p.Text.Trim() == resultValues[i].Trim()) != null)
                                                            {
                                                                thaiDescription += thaiDescription == "" ? resultValues[i].Trim() : "," + resultValues[i].Trim();
                                                            }
                                                        }
                                                        ruleDescription.ThaiDescription = ruleDescription.ThaiDescription.Replace("{0}", thaiDescription);
                                                    }
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                isConrrect = false;
                                                if (ruleItem.Operator == "And")
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            #region  CriteriaNumber
                                            double resultValueNumber;
                                            if (double.TryParse(resultItemValue.ResultValue.Trim(), out resultValueNumber))
                                            {
                                                if ((resultValueNumber >= ruleItem.Low && resultValueNumber <= ruleItem.Hight)
                                                    || (resultValueNumber >= ruleItem.Low && ruleItem.Hight == null)
                                                    || (ruleItem.Low == null && resultValueNumber <= ruleItem.Hight))
                                                {
                                                    isConrrect = true;
                                                    if (ruleItem.Operator == "Or")
                                                    {
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    isConrrect = false;
                                                    if (ruleItem.Operator == "And")
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (resultItemValue.ResultValue.Contains("-"))
                                                {
                                                    string[] values = resultItemValue.ResultValue.Split('-');
                                                    if (values.Count() == 2)
                                                    {
                                                        if (double.TryParse(values[1].Trim(), out resultValueNumber))
                                                        {
                                                            if ((resultValueNumber >= ruleItem.Low && resultValueNumber <= ruleItem.Hight)
                                                                || (resultValueNumber >= ruleItem.Low && ruleItem.Hight == null)
                                                                || (ruleItem.Low == null && resultValueNumber <= ruleItem.Hight))
                                                            {
                                                                isConrrect = true;
                                                                if (ruleItem.Operator == "Or")
                                                                {
                                                                    break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                isConrrect = false;
                                                                if (ruleItem.Operator == "And")
                                                                {
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }

                                                }
                                                else if (resultItemValue.ResultValue.Contains("<")
                                                    || resultItemValue.ResultValue.Contains(">")
                                                    || resultItemValue.ResultValue.Trim().EndsWith("R"))
                                                {
                                                    string value = resultItemValue.ResultValue.Replace("<", "").Replace(">", "").Replace("R", "");
                                                    if (double.TryParse(value.Trim(), out resultValueNumber))
                                                    {

                                                        if (resultItemValue.ResultValue.Contains("<"))
                                                        {
                                                            resultValueNumber = resultValueNumber - 0.01;
                                                        }
                                                        else if (resultItemValue.ResultValue.Contains(">"))
                                                        {
                                                            resultValueNumber = resultValueNumber + 0.01;
                                                        }

                                                        if ((resultValueNumber >= ruleItem.Low && resultValueNumber <= ruleItem.Hight)
                                                            || (resultValueNumber >= ruleItem.Low && ruleItem.Hight == null)
                                                            || (ruleItem.Low == null && resultValueNumber <= ruleItem.Hight))
                                                        {
                                                            isConrrect = true;
                                                            if (ruleItem.Operator == "Or")
                                                            {
                                                                break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            isConrrect = false;
                                                            if (ruleItem.Operator == "And")
                                                            {
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                            #endregion

                                        }
                                    }
                                    else
                                    {
                                        if (ruleItem.NonCheckup == true)
                                        {
                                            isConrrect = true;
                                            if (ruleItem.Operator == "Or")
                                            {
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            isConrrect = false;
                                            if (ruleItem.Operator == "And")
                                            {
                                                break;
                                            }
                                        }

                                    }

                                }
                                if (isConrrect == true)
                                {
                                    ruleCheckupIsCorrect.Add(ruleCheckup);
                                }
                            }


                            string conclusion = string.Empty;

                            int RABSTSUID = ruleCheckupIsCorrect.Any(p => p.RABSTSUID == 2882) ? 2882
                                : ruleCheckupIsCorrect.Any(p => p.RABSTSUID == 2885) ? 2885
                                : ruleCheckupIsCorrect.Any(p => p.RABSTSUID == 3124) ? 3124 : 2883;
                            foreach (var item in ruleCheckupIsCorrect)
                            {
                                if (!string.IsNullOrEmpty(conclusion))
                                {
                                    conclusion += ", ";
                                }
                                foreach (var content in item.CheckupRuleDescription)
                                {
                                    if (!string.IsNullOrEmpty(content.ThaiDescription))
                                    {
                                        conclusion += string.IsNullOrEmpty(conclusion) ? content.ThaiDescription.Trim() : " " + content.ThaiDescription.Trim();
                                    }
                                }
                                foreach (var content in item.CheckupRuleRecommend)
                                {
                                    if (!string.IsNullOrEmpty(content.ThaiRecommend))
                                    {
                                        conclusion += string.IsNullOrEmpty(conclusion) ? content.ThaiRecommend.Trim() : " " + content.ThaiRecommend.Trim();
                                    }
                                }
                            }

                            if (grpstUID == 3182 || grpstUID == 3183 || grpstUID == 3184 || grpstUID == 3190 || grpstUID == 3193) //แปล LAB,CBC,UA,ไขมัน
                            {
                                if (RABSTSUID == 2883 && !ruleCheckupIsCorrect.Any(p => p.RABSTSUID == 2883))
                                {
                                    conclusion = "อยู่ในเกณฑ์ปกติ";
                                }

                            }
                            else if (grpstUID == 3179 || grpstUID == 3180 || grpstUID == 3181 || grpstUID == 4258) //แปล Chest,mammo,Ultrasound
                            {
                                if (RABSTSUID == 2883)
                                {
                                    conclusion = "ปกติ";
                                }
                                else
                                {
                                    List<string> listNoMapResult = new List<string>();
                                    var resultRadiology = grpstUID == 3179 ? radiology.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("chest"))
                                        : grpstUID == 3180 ? radiology.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("mammo")) :
                                        grpstUID == 3181 ? radiology.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("ultrasound") && !p.RequestItemName.ToLower().Contains("thyroid")) :
                                        grpstUID == 4258 ? radiology.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("thyroid")) : null;
                                    if (resultRadiology != null)
                                    {
                                        string thairesult = TranslateResult.TranslateResultXray(resultRadiology.PlainText, resultRadiology.ResultStatus, resultRadiology.RequestItemName, " ", dtResultMapping, ref listNoMapResult);
                                        if (!string.IsNullOrEmpty(thairesult))
                                        {
                                            conclusion = thairesult;
                                        }
                                        else
                                        {
                                            thairesult = "ผิดปกติ";
                                        }
                                    }

                                }
                            }
                            else if (grpstUID == 3176) //พบแพทย์ PE
                            {
                                if (RABSTSUID == 2883)
                                {
                                    conclusion = "ไม่พบความผิดปกติ";
                                }
                            }
                            else if (grpstUID == 3200) //แปลหู
                            {
                                if (RABSTSUID == 2882 || RABSTSUID == 2885)
                                {
                                    conclusion = string.Empty;
                                    var ruleCheckup = ruleCheckupIsCorrect.FirstOrDefault(p => p.RABSTSUID == RABSTSUID);
                                    foreach (var content in ruleCheckup.CheckupRuleDescription)
                                    {
                                        if (!string.IsNullOrEmpty(content.ThaiDescription))
                                        {
                                            conclusion += string.IsNullOrEmpty(conclusion) ? content.ThaiDescription.Trim() : " " + content.ThaiDescription.Trim();
                                        }
                                    }
                                    foreach (var content in ruleCheckup.CheckupRuleRecommend)
                                    {
                                        if (!string.IsNullOrEmpty(content.ThaiRecommend))
                                        {
                                            conclusion += string.IsNullOrEmpty(conclusion) ? content.ThaiRecommend.Trim() : " " + content.ThaiRecommend.Trim();
                                        }
                                    }
                                }

                            }
                            else if (grpstUID == 3201) //แปลตาอาชีวะ
                            {
                                if (RABSTSUID == 2883 && string.IsNullOrEmpty(conclusion))
                                {
                                    conclusion = "ผลการตรวจปกติ ควรตรวจสมรรถภาพการมองเห็นปีละ 1 ครั้ง";
                                }
                                else if (RABSTSUID == 2882 || RABSTSUID == 2885)
                                {
                                    conclusion = string.Empty;
                                    var ruleNonColorBlindness = ruleCheckupIsCorrect.Where(p => !p.Name.StartsWith("ตาบอดสี")).ToList();
                                    if (ruleNonColorBlindness != null && ruleNonColorBlindness.Count > 0)
                                    {
                                        foreach (var ruleData in ruleNonColorBlindness)
                                        {
                                            foreach (var content in ruleData.CheckupRuleDescription)
                                            {
                                                if (!string.IsNullOrEmpty(content.ThaiDescription))
                                                {
                                                    conclusion += string.IsNullOrEmpty(conclusion) ? content.ThaiDescription.Trim() : " " + content.ThaiDescription.Trim();
                                                }
                                            }

                                        }

                                        if (ruleNonColorBlindness != null)
                                        {
                                            string thaiRecommend = ruleNonColorBlindness.FirstOrDefault().CheckupRuleRecommend.Where(p => !String.IsNullOrEmpty(p.ThaiRecommend)).FirstOrDefault().ThaiRecommend;
                                            conclusion += string.IsNullOrEmpty(conclusion) ? thaiRecommend.Trim() : " " + thaiRecommend.Trim();
                                        }
                                    }


                                    var ruleColorBlindness = ruleCheckupIsCorrect.Where(p => p.Name.StartsWith("ตาบอดสี")).ToList();
                                    if (ruleColorBlindness != null && ruleColorBlindness.Count > 0)
                                    {
                                        CheckupRuleModel ruleCheckup = null;
                                        if (ruleColorBlindness.Count() > 1)
                                        {
                                            ruleCheckup = ruleColorBlindness.Where(p => p.Name != "ตาบอดสี").FirstOrDefault();
                                        }
                                        else
                                        {
                                            ruleCheckup = ruleColorBlindness.FirstOrDefault();
                                        }
                                        if (!string.IsNullOrEmpty(conclusion))
                                        {
                                            conclusion += ", ";
                                        }
                                        foreach (var content in ruleCheckup.CheckupRuleDescription)
                                        {
                                            if (!string.IsNullOrEmpty(content.ThaiDescription))
                                            {
                                                conclusion += string.IsNullOrEmpty(conclusion) ? content.ThaiDescription.Trim() : " " + content.ThaiDescription.Trim();
                                            }
                                        }
                                        foreach (var content in ruleCheckup.CheckupRuleRecommend)
                                        {
                                            if (!string.IsNullOrEmpty(content.ThaiRecommend))
                                            {
                                                conclusion += string.IsNullOrEmpty(conclusion) ? content.ThaiRecommend.Trim() : " " + content.ThaiRecommend.Trim();
                                            }
                                        }
                                    }
                                }
                                //else
                                //{
                                //    conclusion = "ผลการตรวจปกติ ควรตรวจสมรรถภาพการมองเห็นปีละ 1 ครั้ง";
                                //    description = "ผลการตรวจปกติ";
                                //    recommand = "ควรตรวจสมรรถภาพการมองเห็นปีละ 1 ครั้ง";
                                //}

                                var timus1 = resultComponent.FirstOrDefault(p => p.ResultItemCode == "TIMUS1");
                                var timus2 = resultComponent.FirstOrDefault(p => p.ResultItemCode == "TIMUS2");
                                var timus3 = resultComponent.FirstOrDefault(p => p.ResultItemCode == "TIMUS3");
                                string far = "";
                                string near = "";
                                if (timus2 != null && timus3 != null)
                                {
                                    far = timus2.ResultItemName + " " + timus2.ResultValue;
                                    near = timus3.ResultItemName + " " + timus3.ResultValue;
                                }
                                else if (timus2 != null && timus3 == null)
                                {
                                    far = timus2.ResultItemName + " " + timus2.ResultValue;
                                    near = "ตรวจมองใกล้ (Near)" + " " + timus2.ResultValue;
                                }
                                else if (timus3 != null && timus2 == null)
                                {
                                    far = "ตรวจมองไกล (Far)" + " " + timus3.ResultValue;
                                    near = timus3.ResultItemName + " " + timus3.ResultValue;
                                }


                                conclusion = "กลุ่มอาชีพ : " + timus1?.ResultValue + ", ตรวจขณะ : " + far + " " + near + ", " + Environment.NewLine + conclusion;
                            }


                            if (!string.IsNullOrEmpty(conclusion))
                            {
                                CheckupGroupResultModel checkupResult = new CheckupGroupResultModel();
                                checkupResult.PatientUID = patientVisit.PatientUID;
                                checkupResult.PatientVisitUID = patientVisit.PatientVisitUID;
                                checkupResult.GPRSTUID = grpstUID;
                                checkupResult.RABSTSUID = RABSTSUID;
                                checkupResult.Conclusion = conclusion.Trim();
                                DataService.Checkup.SaveCheckupGroupResult(checkupResult, AppUtil.Current.UserID);
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }


        }

        void LoadData()
        {
            try
            {

                if (SelectCheckupJobTask != null)
                {
                    TranslateCheckupResult view = (TranslateCheckupResult)this.View;
                    DataTable dtResult = new DataTable();
                    dtResult.Clear();
                    dtResult.Columns.Add("RowHandle");
                    dtResult.Columns.Add("EmployeeID");
                    dtResult.Columns.Add("PatientID");
                    dtResult.Columns.Add("Title");
                    dtResult.Columns.Add("FirstName");
                    dtResult.Columns.Add("LastName");
                    dtResult.Columns.Add("Department");
                    dtResult.Columns.Add("CompanyName");
                    dtResult.Columns.Add("Age");
                    dtResult.Columns.Add("Gender");
                    dtResult.Columns.Add("StartDttm");

                    ColumnsResultItems = new ObservableCollection<Column>();
                    ColumnsResultItems.Add(new Column() { Header = "No", FieldName = "RowHandle", VisibleIndex = 1 });
                    ColumnsResultItems.Add(new Column() { Header = "PatientID", FieldName = "PatientID", VisibleIndex = 2 });
                    ColumnsResultItems.Add(new Column() { Header = "EmployeeID", FieldName = "EmployeeID", VisibleIndex = 3 });
                    ColumnsResultItems.Add(new Column() { Header = "Title", FieldName = "Title", VisibleIndex = 4 });
                    ColumnsResultItems.Add(new Column() { Header = "FirstName", FieldName = "FirstName", VisibleIndex = 5 });
                    ColumnsResultItems.Add(new Column() { Header = "LastName", FieldName = "LastName", VisibleIndex = 6 });
                    ColumnsResultItems.Add(new Column() { Header = "Age", FieldName = "Age", VisibleIndex = 7 });
                    ColumnsResultItems.Add(new Column() { Header = "Gender", FieldName = "Gender", VisibleIndex = 8 });
                    ColumnsResultItems.Add(new Column() { Header = "Department", FieldName = "Department", VisibleIndex = 9 });
                    ColumnsResultItems.Add(new Column() { Header = "CompanyName", FieldName = "CompanyName", VisibleIndex = 10 });
                    ColumnsResultItems.Add(new Column() { Header = "StartDttm", FieldName = "StartDttm", VisibleIndex = 11 });
                    int visibleIndex = 12;

                    CheckupCompanyModel chkCompanyModel = new CheckupCompanyModel();
                    chkCompanyModel.CheckupJobUID = SelectCheckupJobContact.CheckupJobContactUID;
                    chkCompanyModel.GPRSTUID = SelectCheckupJobTask.GPRSTUID;
                    chkCompanyModel.DateFrom = JobDateFrom;
                    chkCompanyModel.DateTo = JobDateTo;
                    List<PatientResultCheckupModel> resultData = DataService.Checkup.GetCheckupGroupResultByJob(chkCompanyModel);
                    if (resultData != null && resultData.Count > 0)
                    {
                        var resultItemList = resultData.Select(p => p.ResultItemName).Distinct();
                        foreach (var item in resultItemList)
                        {
                            ColumnsResultItems.Add(new Column() { Header = item, FieldName = item, VisibleIndex = visibleIndex++ });
                            dtResult.Columns.Add(item);
                        }

                        ColumnsResultItems.Add(new Column() { Header = "แปลผล", FieldName = "Conclusion", VisibleIndex = ColumnsResultItems.Count() });
                        ColumnsResultItems.Add(new Column() { Header = "สรุปผลการตรวจสุขภาพ", FieldName = "CheckupResultStatus", VisibleIndex = ColumnsResultItems.Count() + 2 });

                        dtResult.Columns.Add("Conclusion");
                        dtResult.Columns.Add("CheckupResultStatus");
                    }
                    var patientData = resultData.GroupBy(p => new
                    {
                        p.RowNumber,
                        p.PatientID,
                        p.EmployeeID,
                        p.Title,
                        p.FirstName,
                        p.LastName,
                        p.Department,
                        p.CompanyName,
                        p.Age,
                        p.Gender,
                        p.Conclusion,
                        p.CheckupResultStatus,
                        p.StartDttm
                    })
                    .Select(g => new
                    {
                        RowNumber = g.FirstOrDefault().RowNumber,
                        EmployeeID = g.FirstOrDefault().EmployeeID,
                        PatientID = g.FirstOrDefault().PatientID,
                        Title = g.FirstOrDefault().Title,
                        FirstName = g.FirstOrDefault().FirstName,
                        LastName = g.FirstOrDefault().LastName,
                        Department = g.FirstOrDefault().Department,
                        CompanyName = g.FirstOrDefault().CompanyName,
                        Age = g.FirstOrDefault().Age,
                        Gender = g.FirstOrDefault().Gender,
                        Conclusion = g.FirstOrDefault().Conclusion,
                        CheckupResultStatus = g.FirstOrDefault().CheckupResultStatus,
                        StartDttm = g.FirstOrDefault().StartDttm
                    });

                    foreach (var patient in patientData)
                    {
                        DataRow newRow = dtResult.NewRow();
                        newRow["RowHandle"] = patient.RowNumber;
                        newRow["EmployeeID"] = patient.EmployeeID;
                        newRow["PatientID"] = patient.PatientID;
                        newRow["Title"] = patient.Title;
                        newRow["FirstName"] = patient.FirstName;
                        newRow["LastName"] = patient.LastName;
                        newRow["Department"] = patient.Department;
                        newRow["CompanyName"] = patient.CompanyName;
                        newRow["Age"] = patient.Age;
                        newRow["Gender"] = patient.Gender;
                        newRow["Conclusion"] = patient.Conclusion;
                        newRow["CheckupResultStatus"] = patient.CheckupResultStatus;
                        newRow["StartDttm"] = patient.StartDttm;
                        dtResult.Rows.Add(newRow);
                    }
                    foreach (var result in resultData)
                    {
                        var rowData = dtResult.AsEnumerable().Where(dr => dr.Field<string>("PatientID") == result.PatientID
                        && dr.Field<string>("FirstName") == result.FirstName).FirstOrDefault();
                        if (rowData != null)
                        {
                            rowData[result.ResultItemName] = !string.IsNullOrEmpty(result.IsAbnormal) ? result.ResultValue + " " + result.IsAbnormal : result.ResultValue;
                        }
                    }
                    view.gcResultList.ItemsSource = dtResult;
                }

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void ExportData()
        {
            TranslateCheckupResult view = (TranslateCheckupResult)this.View;
            if (view.gcResultList.ItemsSource != null)
            {
                string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xlsx");
                if (fileName != "")
                {

                    view.gvResultList.ExportToXlsx(fileName);
                    OpenFile(fileName);
                }

            }
        }

        private void SearchPatientVisit()
        {
            string patientID = "";
            int? payorDetailUID;
            if (SearchPatientCriteria != "" && SelectedPateintSearch != null)
            {
                patientID = SelectedPateintSearch.PatientID;
            }

            payorDetailUID = SelectPayorDetail2 != null ? SelectPayorDetail2.PayorDetailUID : (int?)null;

            PatientVisits = DataService.PatientIdentity.SearchPatientVisit(patientID, null, null, null, null, DateFrom, DateTo, null, null, payorDetailUID, null);
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
        #endregion
    }
}
