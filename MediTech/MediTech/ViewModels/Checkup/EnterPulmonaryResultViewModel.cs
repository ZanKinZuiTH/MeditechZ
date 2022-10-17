using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class EnterPulmonaryResultViewModel : MediTechViewModelBase
    {
        #region Properties

        private string _RequestItemName;

        public string RequestItemName
        {
            get { return _RequestItemName; }
            set { Set(ref _RequestItemName, value); }
        }

        private ObservableCollection<ResultComponentModel> _ResultComponentItems;

        public ObservableCollection<ResultComponentModel> ResultComponentItems
        {
            get { return _ResultComponentItems; }
            set { Set(ref _ResultComponentItems, value); }
        }

        public string OrderStatus { get; set; }

        private RequestListModel RequestModel;
        #endregion

        #region Command
        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                    ?? (_SaveCommand = new RelayCommand(Save));
            }
        }

        private RelayCommand _ViewVitalSignCommand;
        public RelayCommand ViewVitalSignCommand
        {
            get
            {
                return _ViewVitalSignCommand
                    ?? (_ViewVitalSignCommand = new RelayCommand(Viewvitalsign));
            }
        }

        private RelayCommand _CloseCommand;

        public RelayCommand CloseCommand
        {
            get
            {
                return _CloseCommand
                    ?? (_CloseCommand = new RelayCommand(Close));
            }
        }
        #endregion

        #region Method
        List<CheckupRuleModel> dataCheckupRule;
        public override void OnLoaded()
        {
            base.OnLoaded();
            (this.View as EnterPulmonaryResult).patientBanner.SetPatientBanner(RequestModel);
            double height = Convert.ToDouble(this.RequestModel.Height);
            string warningMessage = string.Empty;

            if (string.IsNullOrEmpty(this.RequestModel.Gender))
            {
                warningMessage = "* ผู้ป่วยไม่ได้ระบุเพศ กรุณาตรวจสอบ";
            }
            if (string.IsNullOrEmpty(this.RequestModel.PatientAge))
            {
                warningMessage += string.IsNullOrEmpty(warningMessage) ? "* ผู้ป่วยไม่ได้ระบุอายุ กรุณาตรวจสอบ" : "\r\n* ผู้ป่วยไม่ได้ระบุอายุ กรุณาตรวจสอบ";
            }
            if (this.RequestModel.Height == 0)
            {
                warningMessage += string.IsNullOrEmpty(warningMessage) ? "* ผู้ป่วยไม่ได้ระบุส่วนสูง กรุณาตรวจสอบ" : "\r\n* ผู้ป่วยไม่ได้ระบุส่วนสูง กรุณาตรวจสอบ";
            }

            if (!string.IsNullOrEmpty(warningMessage))
            {
                WarningDialog(warningMessage);
            }
        }

        public void AssignModel(RequestListModel request)
        {
            this.RequestModel = request;
            RequestItemName = this.RequestModel.RequestItemName;
            var dataList = DataService.Checkup.GetResultItemByRequestDetailUID(request.RequestDetailUID);
            if (dataList != null)
            {
                ResultComponentItems = new ObservableCollection<ResultComponentModel>(dataList);
                List<int> gprsts = new List<int>();
                gprsts.Add(3208);// SPIRORULE
                dataCheckupRule = DataService.Checkup.GetCheckupRuleGroupList(gprsts);
            }
        }


        public void Viewvitalsign()
        {
            if ( RequestModel != null)
            {
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(RequestModel.PatientVisitUID);
                PatientVitalSign pageview = new PatientVitalSign();
                (pageview.DataContext as PatientVitalSignViewModel).AssingPatientVisit(patientVisit);
                PatientVitalSignViewModel result = (PatientVitalSignViewModel)LaunchViewDialog(pageview, "PTVAT", false);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    var vitalSign = DataService.PatientHistory.GetPatientVitalSignByVisitUID(RequestModel.PatientVisitUID);
                    if (vitalSign != null)
                    {
                        this.RequestModel.Height = vitalSign.OrderByDescending(p => p.RecordedDttm).FirstOrDefault()?.Height ?? 0;
                        this.RequestModel.Weight = vitalSign.OrderByDescending(p => p.RecordedDttm).FirstOrDefault()?.Weight ?? 0;
                        OnLoaded();
                    }
                    //SaveSuccessDialog();
             
                }
            }
        }

        public void CalculateSpiroValue()
        {
            string gender = this.RequestModel.Gender;
            int age = int.Parse(this.RequestModel.PatientAge);
            double height = Convert.ToDouble(this.RequestModel.Height);
            try
            {
                var fvc_meas = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "SPIRO1" || p.ResultItemName == "FVC (Meas.)");
                var fvc_pred = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "SPIRO2" || p.ResultItemName == "FVC (Pred.)");
                var fvc_perpred = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "SPIRO3" || p.ResultItemName == "FVC (%Pred.)");
                var fev1_meas = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "SPIRO4" || p.ResultItemName == "FEV1 (Meas.)");
                var fev1_pred = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "SPIRO5" || p.ResultItemName == "FEV1 (Pred.)");
                var fev1_perpred = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "SPIRO6" || p.ResultItemName == "FEV1 (%Pred.)");
                var fev1_fvc_meas = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "SPIRO7" || p.ResultItemName == "FEV1/FVC % (Meas.)");
                var fev1_fvc_pred = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "SPIRO8" || p.ResultItemName == "FEV1/FVC % (Pred.)");
                var fev1_fvc_perpread = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "SPIRO9" || p.ResultItemName == "FEV1/FVC % (%Pred.)");
                var resultspiro = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "SPIRORST" || p.ResultItemName == "สรุปผลสมรรถภาพปอด");


                fvc_pred.ResultValue = gender == "ชาย (Male)" ? (Math.Round(-2.601 + (0.122 * age) - (0.00046 * age * age) + (0.00023 * height * height) - (0.00061 * age * height), 2)).ToString()
        : gender == "หญิง (Female)" ? (Math.Round(-5.914 + (0.088 * age) + (0.056 * height) - (0.0003 * age * age) - (0.0005 * age * height), 2)).ToString() : "";

                fev1_pred.ResultValue = gender == "ชาย (Male)" ? (Math.Round(-7.697 + (0.123 * age) + (0.067 * height) - (0.00034 * age * age) - (0.0007 * age * height), 2)).ToString()
    : gender == "หญิง (Female)" ? (Math.Round(-10.603 + (0.085 * age) + (0.12 * height) - (0.00019 * age * age) - (0.00022 * height * height) - (0.00056 * age * height), 2)).ToString() : "";

                fev1_fvc_pred.ResultValue = gender == "ชาย (Male)" ? (Math.Round(19.362 + (0.49 * age) + (0.829 * height) - (0.0023 * height * height) - (0.0041 * age * height), 2)).ToString()
    : gender == "หญิง (Female)" ? (Math.Round(83.126 + (0.243 * age) + (0.08 * height) + (0.002 * age * age) - (0.0036 * age * height), 2)).ToString() : "";

                fvc_perpred.ResultValue = (Math.Round(double.Parse(fvc_meas.ResultValue) / double.Parse(fvc_pred.ResultValue) * 100, 2)).ToString();
                fev1_perpred.ResultValue = (Math.Round(double.Parse(fev1_meas.ResultValue) / double.Parse(fev1_pred.ResultValue) * 100, 2)).ToString();
                fev1_fvc_meas.ResultValue = (Math.Round(double.Parse(fev1_meas.ResultValue) / double.Parse(fvc_meas.ResultValue) * 100, 2)).ToString();
                fev1_fvc_perpread.ResultValue = (Math.Round(double.Parse(fev1_fvc_meas.ResultValue) / double.Parse(fev1_fvc_pred.ResultValue) * 100, 2)).ToString();

                List<ResultComponentModel> resultComponent = new List<ResultComponentModel>();
                resultComponent.Add(new ResultComponentModel { ResultItemUID = 282,ResultValue = fvc_perpred.ResultValue });
                resultComponent.Add(new ResultComponentModel { ResultItemUID = 285, ResultValue = fev1_perpred.ResultValue });
                resultComponent.Add(new ResultComponentModel { ResultItemUID = 288, ResultValue = fev1_fvc_perpread.ResultValue });
                List<CheckupRuleModel> ruleCheckupIsCorrect = new List<CheckupRuleModel>();

                foreach (var ruleCheckup in dataCheckupRule)
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
                            //description += string.IsNullOrEmpty(description) ? content.ThaiDescription.Trim() : " " + content.ThaiDescription.Trim();
                        }
                    }
                    foreach (var content in item.CheckupRuleRecommend)
                    {
                        if (!string.IsNullOrEmpty(content.ThaiRecommend))
                        {
                            conclusion += string.IsNullOrEmpty(conclusion) ? content.ThaiRecommend.Trim() : " " + content.ThaiRecommend.Trim();
                            //recommand += string.IsNullOrEmpty(recommand) ? content.ThaiRecommend.Trim() : " " + content.ThaiRecommend.Trim();
                        }
                    }
                }

                if(!string.IsNullOrEmpty(conclusion))
                {
                    resultspiro.ResultValue = conclusion;
                }
            }
            catch (Exception)
            {

            }

        }


        private void Save()
        {
            try
            {
                RequestDetailItemModel reviewRequestDetail = new RequestDetailItemModel();
                reviewRequestDetail.RequestUID = RequestModel.RequestUID;
                reviewRequestDetail.RequestDetailUID = RequestModel.RequestDetailUID;
                reviewRequestDetail.PatientUID = RequestModel.PatientUID;
                reviewRequestDetail.PatientVisitUID = RequestModel.PatientVisitUID;
                reviewRequestDetail.RequestItemCode = RequestModel.RequestItemCode;
                reviewRequestDetail.RequestItemName = RequestModel.RequestItemName;

                reviewRequestDetail.ResultComponents = new ObservableCollection<ResultComponentModel>(ResultComponentItems.Where(p => !string.IsNullOrEmpty(p.ResultValue)));

                if (reviewRequestDetail.ResultComponents != null && reviewRequestDetail.ResultComponents.Count > 0)
                {
                    DataService.Checkup.SaveOccmedExamination(reviewRequestDetail, AppUtil.Current.UserID);
                    OrderStatus = "Reviewed";
                }
                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void Close()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        #endregion
    }
}
