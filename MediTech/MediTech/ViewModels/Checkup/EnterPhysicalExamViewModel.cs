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
    public class EnterPhysicalExamViewModel : MediTechViewModelBase
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

        private PatientVitalSignModel _PatientVitalSign;

        public PatientVitalSignModel PatientVitalSign
        {
            get { return _PatientVitalSign ?? (_PatientVitalSign = new PatientVitalSignModel()); }
            set { Set(ref _PatientVitalSign, value); }
        }


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

        public override void OnLoaded()
        {
            base.OnLoaded();
            (this.View as EnterPhysicalExam).patientBanner.SetPatientBanner(RequestModel);
        }

        public void AssignModel(RequestListModel request)
        {
            this.RequestModel = request;
            RequestItemName = this.RequestModel.RequestItemName;
            var RecentVitals = DataService.PatientHistory.GetPatientVitalSignByVisitUID(request.PatientVisitUID);
            var dataList = DataService.Checkup.GetResultItemByRequestDetailUID(request.RequestDetailUID);

            if (RecentVitals != null && RecentVitals.Count > 0)
            {
                PatientVitalSign = RecentVitals.OrderByDescending(p => p.RecordedDttm).FirstOrDefault();
            }
            else
            {
                PatientVitalSign.PatientUID = request.PatientUID;
                PatientVitalSign.PatientVisitUID = request.PatientVisitUID;
            }

            if (dataList != null)
            {
                ResultComponentItems = new ObservableCollection<ResultComponentModel>(dataList);
                foreach (var item in ResultComponentItems)
                {
                    if (!string.IsNullOrEmpty(item.AutoValue))
                        item.AutoValueList = item.AutoValue.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p)).ToList();
                }

                foreach (var item in ResultComponentItems)
                {
                    if (item.ResultUID != null && !string.IsNullOrEmpty(item.ResultValue))
                    {
                        string[] values = item.ResultValue.Split(',');
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (item.AutoValueList != null && item.AutoValueList.Any(p => p == values[i]))
                            {
                                if (item.CheckPhyExamList == null)
                                    item.CheckPhyExamList = new List<object>();

                                item.CheckPhyExamList.Add(values[i]);
                            }
                            else if(values[i] != "ไม่พบความผิดปกติ" && values[i] != "ปฏิเสธ")
                            {
                                if (item.TokenPhyExamList == null)
                                    item.TokenPhyExamList = new List<object>();

                                item.TokenPhyExamList.Add(values[i]);
                            }
                        }
                    }
                }
            }
        }

        private void Save()
        {
            try
            {
                if (PatientVitalSign.BPSys < PatientVitalSign.BPDio)
                {
                    WarningDialog("ความดันบนมากกว่าความดันล่าง โปรดตรวจสอบ");
                    return;
                }

                if (PatientVitalSign.BPSys != null && PatientVitalSign.BPSys <= 60)
                {
                    WarningDialog("ความดันบนน้อยกว่าปกติ โปรดตรวจสอบ");
                    return;
                }

                if (PatientVitalSign.BPDio != null && PatientVitalSign.BPDio <= 20)
                {
                    WarningDialog("ความดันล่างกว่าปกติ โปรดตรวจสอบ");
                    return;
                }

                RequestDetailItemModel reviewRequestDetail = new RequestDetailItemModel();
                reviewRequestDetail.RequestUID = RequestModel.RequestUID;
                reviewRequestDetail.RequestDetailUID = RequestModel.RequestDetailUID;
                reviewRequestDetail.PatientUID = RequestModel.PatientUID;
                reviewRequestDetail.PatientVisitUID = RequestModel.PatientVisitUID;
                reviewRequestDetail.RequestItemCode = RequestModel.RequestItemCode;
                reviewRequestDetail.RequestItemName = RequestModel.RequestItemName;

                if (PatientVitalSign.Weight != null && PatientVitalSign.Height != null)
                {
                    PatientVitalSign.BMIValue = PatientVitalSign.Weight / (PatientVitalSign.Height / 100 * PatientVitalSign.Height / 100);
                    PatientVitalSign.BSAValue = Math.Pow(PatientVitalSign.Weight.Value, 0.425) * Math.Pow(PatientVitalSign.Height.Value, 0.725) * 0.007184;

                    PatientVitalSign.BMIValue = Math.Round(PatientVitalSign.BMIValue.Value, 2);
                    PatientVitalSign.BSAValue = Math.Round(PatientVitalSign.BSAValue.Value, 2);
                }

                foreach (var item in ResultComponentItems)
                {
                    string resultValue = string.Empty;
                    if (item.CheckPhyExamList != null)
                    {
                        foreach (var phyExam in item.CheckPhyExamList)
                        {
                            resultValue += string.IsNullOrEmpty(resultValue) ? phyExam.ToString() : "," + phyExam.ToString();
                        }
                    }

                    if (item.TokenPhyExamList != null)
                    {
                        foreach (var tokenPhyExam in item.TokenPhyExamList)
                        {
                            resultValue += string.IsNullOrEmpty(resultValue) ? tokenPhyExam.ToString() : "," + tokenPhyExam.ToString();
                        }
                    }


                    if (string.IsNullOrEmpty(resultValue))
                    {
                        if (item.ResultItemName == "บุหรี่" || item.ResultItemName == "แอลกอฮอล์" || item.ResultItemName == "แพ้ยา" || item.ResultItemName == "โรคประจำตัว")
                        {
                            resultValue = "ปฏิเสธ";
                        }
                        else
                        {
                            resultValue = "ไม่พบความผิดปกติ";
                        }
                    }

                    item.ResultValue = resultValue;
                }

                reviewRequestDetail.ResultComponents = ResultComponentItems;
                DataService.PatientHistory.ManagePatientVitalSign(PatientVitalSign, AppUtil.Current.UserID);
                DataService.Checkup.SaveOccmedExamination(reviewRequestDetail, AppUtil.Current.UserID);
                OrderStatus = "Reviewed";
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
