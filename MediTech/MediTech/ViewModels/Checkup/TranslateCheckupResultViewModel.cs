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

        #endregion

        #region Method
        public TranslateCheckupResultViewModel()
        {
            PayorDetails = DataService.MasterData.GetPayorDetail();
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
                        if (GPRSTUIDs.Any(p => p == 3179 || p == 3180 || p == 3181))
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
                        if (GPRSTUIDs.Any(p => p == 3179 || p == 3180 || p == 3181))
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
                        if (GPRSTUIDs.Any(p => p == 3179 || p == 3180 || p == 3181))
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

        void TranslateProcess(PatientVisitModel patientVisit, List<int> GPRSTUIDs, List<CheckupRuleModel> dataCheckupRule)
        {
            int? ageInt = !string.IsNullOrEmpty(patientVisit.Age) ? int.Parse(patientVisit.Age) : (int?)null;
            PatientVitalSignModel vitalSign = null;
            List<ResultRadiologyModel> radiology = null;
            foreach (var grpstUID in GPRSTUIDs)
            {
                List<CheckupRuleModel> ruleCheckupIsCorrect = new List<CheckupRuleModel>();
                string wellNessResult = string.Empty;
                List<ResultComponentModel> resultComponent = null;

                if (grpstUID == 3177 || grpstUID == 3178)
                {
                    if (vitalSign == null)
                    {
                        var dataVital = DataService.PatientHistory.GetPatientVitalSignByVisitUID(patientVisit.PatientVisitUID);
                        if (dataVital != null)
                        {
                            vitalSign = dataVital.OrderByDescending(p => p.RecordedDttm).FirstOrDefault();
                        }
                    }

                    if (vitalSign != null)
                    {
                        resultComponent = new List<ResultComponentModel>();
                        double? bmiValue = null;
                        if (vitalSign.BMIValue.HasValue)
                            bmiValue = Math.Round(vitalSign.BMIValue.Value, 1);

                        if (grpstUID == 3177)
                        {
                            if (vitalSign.BMIValue != null)
                            {
                                ResultComponentModel bmiComponent = new ResultComponentModel() { ResultItemUID = 328, ResultItemCode = "PEBMI", ResultItemName = "BMI (ดัชนีมวลกาย)", ResultValue = bmiValue.ToString() };
                                resultComponent.Add(bmiComponent);
                            }
                        }

                        if (grpstUID == 3178)
                        {
                            if (vitalSign.BPSys != null)
                            {
                                ResultComponentModel sdpComponent = new ResultComponentModel() { ResultItemUID = 329, ResultItemCode = "PESBP", ResultItemName = "ความดันโลหิต (SBP)", ResultValue = vitalSign.BPSys.ToString() };
                                resultComponent.Add(sdpComponent);
                            }
                            if (vitalSign.BPDio != null)
                            {
                                ResultComponentModel dbpComponent = new ResultComponentModel() { ResultItemUID = 330, ResultItemCode = "PEDBP", ResultItemName = "ความดันโลหิต (DBP)", ResultValue = vitalSign.BPDio.ToString() };
                                resultComponent.Add(dbpComponent);
                            }
                            if (vitalSign.Pulse != null)
                            {
                                ResultComponentModel pluseComponent = new ResultComponentModel() { ResultItemUID = 331, ResultItemCode = "PEPLUSE", ResultItemName = "ชีพจร(Pulse)", ResultValue = vitalSign.Pulse.ToString() };
                                resultComponent.Add(pluseComponent);
                            }
                        }

                    }

                }
                else if (grpstUID == 3179 || grpstUID == 3180 || grpstUID == 3181)
                {
                    if (radiology == null)
                    {
                        radiology = DataService.Radiology.GetResultRadiologyByVisitUID(patientVisit.PatientVisitUID);
                    }

                    if (radiology != null)
                    {
                        resultComponent = new List<ResultComponentModel>();
                        foreach (var item in radiology)
                        {
                            ResultComponentModel newResultCom = new ResultComponentModel();
                            newResultCom.ResultItemUID = item.RequestItemName.ToLower().Contains("chest") ? 342
                                : item.RequestItemName.ToLower().Contains("mammo") ? 343
                                : item.RequestItemName.ToLower().Contains("ultrasound") ? 344 : 0;
                            newResultCom.ResultValue = item.ResultStatus;

                            resultComponent.Add(newResultCom);
                        }
                    }
                }
                else
                {
                    resultComponent = DataService.Checkup.GetGroupResultComponentByVisitUID(patientVisit.PatientVisitUID, grpstUID);
                }

                if (patientVisit.PatientUID == 58822)
                {
                    if (true)
                    {

                    }
                }
                if (resultComponent != null && resultComponent.Count > 0)
                {

                    //var ruleCheckups = dataCheckupRule
                    //    .Where(p => p.GPRSTUID == grpstUID
                    //    && (p.SEXXXUID == 3 || p.SEXXXUID == patientVisit.SEXXXUID)
                    //    && ((p.AgeFrom == null && p.AgeTo == null) || (ageInt > p.AgeFrom && ageInt < p.AgeTo)
                    //    || (ageInt > p.AgeFrom && p.AgeTo == null) || (p.AgeFrom == null && ageInt < p.AgeTo))
                    //    && (p.RABSTSUID != 2883 || (p.RABSTSUID == 2883 && testType == "LAB" && resultComponent.All(x => p.CheckupRuleItem.Any(y => x.ResultItemUID == y.ResultItemUID)))
                    //    )).ToList();


                    var ruleCheckups = dataCheckupRule
                        .Where(p => p.GPRSTUID == grpstUID
                        && (p.SEXXXUID == 3 || p.SEXXXUID == patientVisit.SEXXXUID)
                        && ((p.AgeFrom == null && p.AgeTo == null) || (ageInt >= p.AgeFrom && ageInt <= p.AgeTo)
                        || (ageInt >= p.AgeFrom && p.AgeTo == null) || (p.AgeFrom == null && ageInt <= p.AgeTo))
                        && (p.RABSTSUID != 2883 || (p.RABSTSUID == 2883)
                        )).ToList();


                    foreach (var ruleCheckup in ruleCheckups)
                    {
                        bool isConrrect = false;
                        foreach (var ruleItem in ruleCheckup.CheckupRuleItem.OrderBy(p => p.Operator))
                        {
                            var resultItemValue = resultComponent.FirstOrDefault(p => p.ResultItemUID == ruleItem.ResultItemUID);

                            if (resultItemValue != null)
                            {
                                if (!string.IsNullOrEmpty(ruleItem.Text))
                                {
                                    string[] values = ruleItem.Text.Split(',');
                                    string[] resultValues = resultItemValue.ResultValue.Split(',');
                                    //if (values.Any(p => p.ToLower().Trim() == resultItemValue.ResultValue.ToLower().Trim()))
                                    //{
                                    //    isConrrect = true;
                                    //    if (ruleItem.Operator == "Or")
                                    //    {
                                    //        break;
                                    //    }
                                    //}
                                    if (values.Any(p => resultValues.Any(x => x.ToLower().Trim() == p.ToLower().Trim())))
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
                                        else if(resultItemValue.ResultValue.Contains("<") 
                                            || resultItemValue.ResultValue.Contains(">"))
                                        {
                                            string value = resultItemValue.ResultValue.Replace("<", "").Replace(">","");
                                            if (double.TryParse(value.Trim(), out resultValueNumber))
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
                    string description = string.Empty;
                    string recommand = string.Empty;

                    int RABSTSUID = ruleCheckupIsCorrect.Any(p => p.RABSTSUID == 2882) ? 2882 : ruleCheckupIsCorrect.Any(p => p.RABSTSUID == 2885) ? 2885 : 2883;
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
                                description += string.IsNullOrEmpty(description) ? content.ThaiDescription.Trim() : " " + content.ThaiDescription.Trim();
                            }
                        }
                        foreach (var content in item.CheckupRuleRecommend)
                        {
                            if (!string.IsNullOrEmpty(content.ThaiRecommend))
                            {
                                conclusion += string.IsNullOrEmpty(conclusion) ? content.ThaiRecommend.Trim() : " " + content.ThaiRecommend.Trim();
                                recommand += string.IsNullOrEmpty(recommand) ? content.ThaiRecommend.Trim() : " " + content.ThaiRecommend.Trim();
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
                    else if (grpstUID == 3179 || grpstUID == 3180 || grpstUID == 3181) //แปล Chest,mammo,Ultrasound
                    {
                        if (RABSTSUID == 2883)
                        {
                            conclusion = "ปกติ";
                        }
                        else
                        {
                            List<string> listNoMapResult = new List<string>();
                            var resultRadiology = grpstUID == 3179 ? radiology.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("chest"))
                                : grpstUID == 3179 ? radiology.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("chest")) :
                                grpstUID == 3179 ? radiology.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("chest")) : null;
                            if (resultRadiology != null)
                            {
                                string thairesult = TranslateResult.TranslateResultXray(resultRadiology.PlainText, resultRadiology.ResultStatus, resultRadiology.RequestItemName, ",", dtResultMapping, ref listNoMapResult);
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
                            description = string.Empty;
                            recommand = string.Empty;
                            var ruleCheckup = ruleCheckupIsCorrect.FirstOrDefault(p => p.RABSTSUID == RABSTSUID);
                            foreach (var content in ruleCheckup.CheckupRuleDescription)
                            {
                                if (!string.IsNullOrEmpty(content.ThaiDescription))
                                {
                                    conclusion += string.IsNullOrEmpty(conclusion) ? content.ThaiDescription.Trim() : " " + content.ThaiDescription.Trim();
                                    description += string.IsNullOrEmpty(description) ? content.ThaiDescription.Trim() : " " + content.ThaiDescription.Trim();
                                }
                            }
                            foreach (var content in ruleCheckup.CheckupRuleRecommend)
                            {
                                if (!string.IsNullOrEmpty(content.ThaiRecommend))
                                {
                                    conclusion += string.IsNullOrEmpty(conclusion) ? content.ThaiRecommend.Trim() : " " + content.ThaiRecommend.Trim();
                                    recommand += string.IsNullOrEmpty(recommand) ? content.ThaiRecommend.Trim() : " " + content.ThaiRecommend.Trim();
                                }
                            }
                        }

                    }
                    else if (grpstUID == 3201) //แปลตาอาชีวะ
                    {
                        if (RABSTSUID == 2882 || RABSTSUID == 2885)
                        {
                            conclusion = string.Empty;
                            description = string.Empty;
                            recommand = string.Empty;
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
                                            description += string.IsNullOrEmpty(description) ? content.ThaiDescription.Trim() : " " + content.ThaiDescription.Trim();
                                        }
                                    }

                                }

                                if (ruleNonColorBlindness != null)
                                {
                                    string thaiRecommend = ruleNonColorBlindness.FirstOrDefault().CheckupRuleRecommend.Where(p => !String.IsNullOrEmpty(p.ThaiRecommend)).FirstOrDefault().ThaiRecommend;
                                    conclusion += string.IsNullOrEmpty(conclusion) ? thaiRecommend.Trim() : " " + thaiRecommend.Trim();
                                    recommand += string.IsNullOrEmpty(recommand) ? thaiRecommend.Trim() : " " + thaiRecommend.Trim();
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
                                        description += string.IsNullOrEmpty(description) ? content.ThaiDescription.Trim() : " " + content.ThaiDescription.Trim();
                                    }
                                }
                                foreach (var content in ruleCheckup.CheckupRuleRecommend)
                                {
                                    if (!string.IsNullOrEmpty(content.ThaiRecommend))
                                    {
                                        conclusion += string.IsNullOrEmpty(conclusion) ? content.ThaiRecommend.Trim() : " " + content.ThaiRecommend.Trim();
                                        recommand += string.IsNullOrEmpty(recommand) ? content.ThaiRecommend.Trim() : " " + content.ThaiRecommend.Trim();
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
                        if (timus1 != null && timus2 != null && timus3 != null)
                        {
                            string far = timus2.ResultItemName + " " + timus2.ResultValue;
                            string near = timus3.ResultItemName + " " + timus3.ResultValue;
                            conclusion = "กลุ่มอาชีพ : " + timus1.ResultValue + ", ตรวจขณะ : " + far + " " + near + ", " + Environment.NewLine + conclusion;
                        }
                    }

                    CheckupGroupResultModel checkupResult = new CheckupGroupResultModel();
                    checkupResult.PatientUID = patientVisit.PatientUID;
                    checkupResult.PatientVisitUID = patientVisit.PatientVisitUID;
                    checkupResult.GPRSTUID = grpstUID;
                    checkupResult.RABSTSUID = RABSTSUID;
                    checkupResult.Description = description;
                    checkupResult.Recommend = recommand;
                    checkupResult.Conclusion = conclusion;
                    checkupResult.Conclusion = checkupResult.Conclusion.Trim();
                    DataService.Checkup.SaveCheckupGroupResult(checkupResult, AppUtil.Current.UserID);


                }
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

                    ColumnsResultItems = new ObservableCollection<Column>();
                    ColumnsResultItems.Add(new Column() { Header = "No", FieldName = "RowHandle", VisibleIndex = 1 });
                    ColumnsResultItems.Add(new Column() { Header = "EmployeeID", FieldName = "EmployeeID", VisibleIndex = 2 });
                    ColumnsResultItems.Add(new Column() { Header = "Title", FieldName = "Title", VisibleIndex = 3 });
                    ColumnsResultItems.Add(new Column() { Header = "PatientID", FieldName = "PatientID", VisibleIndex = 4 });
                    ColumnsResultItems.Add(new Column() { Header = "FirstName", FieldName = "FirstName", VisibleIndex = 5 });
                    ColumnsResultItems.Add(new Column() { Header = "LastName", FieldName = "LastName", VisibleIndex = 6 });
                    ColumnsResultItems.Add(new Column() { Header = "Department", FieldName = "Department", VisibleIndex = 7 });
                    ColumnsResultItems.Add(new Column() { Header = "CompanyName", FieldName = "CompanyName", VisibleIndex = 8 });
                    ColumnsResultItems.Add(new Column() { Header = "Age", FieldName = "Age", VisibleIndex = 9 });
                    ColumnsResultItems.Add(new Column() { Header = "Gender", FieldName = "Gender", VisibleIndex = 10 });
                    int visibleIndex = 11;
                    List<PatientResultCheckupModel> resultData = DataService.Checkup
                        .GetCheckupGroupResultByJob(SelectCheckupJobContact.CheckupJobContactUID, SelectCheckupJobTask.GPRSTUID);
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
                        p.CheckupResultStatus
                    })
                    .Select(g => new
                    {
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
                        CheckupResultStatus = g.FirstOrDefault().CheckupResultStatus
                    });
                    int i = 1;
                    foreach (var patient in patientData)
                    {
                        DataRow newRow = dtResult.NewRow();
                        newRow["RowHandle"] = i++;
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

        private string ShowSaveFileDialog(string title, string filter)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            string name = System.Windows.Forms.Application.ProductName;
            int n = name.LastIndexOf(".") + 1;
            if (n > 0) name = name.Substring(n, name.Length - n);
            dlg.Title = "Export To " + title;
            dlg.FileName = name;
            dlg.Filter = filter;
            if (dlg.ShowDialog() == DialogResult.OK) return dlg.FileName;
            return "";
        }

        private void OpenFile(string fileName)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you want to open this file?", "Export To...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = fileName;
                    process.StartInfo.Verb = "Open";
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    process.Start();
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Cannot find an application on your system suitable for openning the file with exported data.", System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion
    }
}
