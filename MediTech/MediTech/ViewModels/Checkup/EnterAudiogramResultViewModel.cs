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
    public class EnterAudiogramResultViewModel : MediTechViewModelBase
    {
        #region Properties

        private string _RequestItemName;

        public string RequestItemName
        {
            get { return _RequestItemName; }
            set { Set(ref _RequestItemName, value); }
        }

        private ObservableCollection<ResultComponentModel> _LeftEarResultComponentItems;

        public ObservableCollection<ResultComponentModel> LeftEarResultComponentItems
        {
            get { return _LeftEarResultComponentItems; }
            set { Set(ref _LeftEarResultComponentItems, value); }
        }

        private ObservableCollection<ResultComponentModel> _RightEarResultComponentItems;

        public ObservableCollection<ResultComponentModel> RightEarResultComponentItems
        {
            get { return _RightEarResultComponentItems; }
            set { Set(ref _RightEarResultComponentItems, value); }
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
            (this.View as EnterAudiogramResult).patientBanner.SetPatientBanner(RequestModel);
        }

        public void AssignModel(RequestListModel request)
        {
            this.RequestModel = request;
            RequestItemName = this.RequestModel.RequestItemName;
            var dataList = DataService.Checkup.GetResultItemByRequestDetailUID(request.RequestDetailUID);
            if (dataList != null)
            {
                LeftEarResultComponentItems = new ObservableCollection<ResultComponentModel>(dataList.Where(p => p.ResultItemName.EndsWith("L")));
                RightEarResultComponentItems = new ObservableCollection<ResultComponentModel>(dataList.Where(p => p.ResultItemName.EndsWith("R")));

                RightEarResultComponentItems.Add(dataList.FirstOrDefault(p => p.ResultItemName == "แปลผลหูขวา"));
                RightEarResultComponentItems.Add(dataList.FirstOrDefault(p => p.ResultItemName == "สรุปผลหูขวา"));
                LeftEarResultComponentItems.Add(dataList.FirstOrDefault(p => p.ResultItemName == "แปลผลหูซ้าย"));
                LeftEarResultComponentItems.Add(dataList.FirstOrDefault(p => p.ResultItemName == "สรุปผลหูซ้าย"));
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

                reviewRequestDetail.ResultComponents = new ObservableCollection<ResultComponentModel>();
                foreach (var item in LeftEarResultComponentItems)
                {
                    if (!string.IsNullOrEmpty(item.ResultValue))
                        reviewRequestDetail.ResultComponents.Add(item);
                }

                foreach (var item in RightEarResultComponentItems)
                {
                    if (!string.IsNullOrEmpty(item.ResultValue))
                        reviewRequestDetail.ResultComponents.Add(item);
                }

                if (reviewRequestDetail.ResultComponents == null || reviewRequestDetail.ResultComponents.Count <= 0)
                {
                    WarningDialog("กรุณากรอกข้อมูล");
                    return;
                }

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

        public void CalculateRightResult()
        {
            try
            {
                string result = string.Empty;
                string translate = string.Empty;
                bool abnormal = false;
                var hz500 = RightEarResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "AUDIO1" || p.ResultItemName == "500 R");
                var hz1000 = RightEarResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "AUDIO2" || p.ResultItemName == "1000 R");
                var hz2000 = RightEarResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "AUDIO3" || p.ResultItemName == "2000 R");
                var hz3000 = RightEarResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "AUDIO4" || p.ResultItemName == "3000 R");
                var hz4000 = RightEarResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "AUDIO5" || p.ResultItemName == "4000 R");
                var hz6000 = RightEarResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "AUDIO6" || p.ResultItemName == "6000 R");
                var hz8000 = RightEarResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "AUDIO7" || p.ResultItemName == "8000 R");
                var rightTran = RightEarResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "AUDIO8" || p.ResultItemName == "แปลผลหูขวา");
                var rightResult = RightEarResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "AUDIO17" || p.ResultItemName == "สรุปผลหูขวา");

                if (int.Parse(hz6000.ResultValue) > 25 && int.Parse(hz4000.ResultValue) > 25
                    || (int.Parse(hz500.ResultValue) > 25 && int.Parse(hz1000.ResultValue) > 25 && int.Parse(hz2000.ResultValue) > 25
                    && int.Parse(hz3000.ResultValue) > 25 && int.Parse(hz4000.ResultValue) > 25))
                {
                    result = "ผิดปกติที่ความถี่สูง";
                    abnormal = true;
                }
                else if (int.Parse(hz6000.ResultValue) > 25 || int.Parse(hz8000.ResultValue) > 25 || int.Parse(hz500.ResultValue) > 25
                    || int.Parse(hz1000.ResultValue) > 25 || int.Parse(hz2000.ResultValue) > 25 || int.Parse(hz3000.ResultValue) > 25
                    || int.Parse(hz4000.ResultValue) > 25)
                {
                    result = "ระดับการได้ยินลดลงเล็กน้อย";
                    abnormal = true;
                }
                else if (int.Parse(hz6000.ResultValue) <= 25 && int.Parse(hz8000.ResultValue) <= 25 && int.Parse(hz500.ResultValue) <= 25
                    && int.Parse(hz1000.ResultValue) <= 25 && int.Parse(hz2000.ResultValue) <= 25 && int.Parse(hz3000.ResultValue) <= 25
                    && int.Parse(hz4000.ResultValue) <= 25)
                {
                    result = "ไม่พบความผิดปกติ";
                }
                translate = result;
                if (abnormal == true)
                {
                    string db = string.Empty;
                    string hz = string.Empty;
                    foreach (var item in RightEarResultComponentItems.Where(p => p.ResultItemName.EndsWith("R")))
                    {
                        if (int.Parse(item.ResultValue) > 25)
                        {
                            db += " " + item.ResultValue;
                            hz += string.IsNullOrEmpty(hz) ? "dB ที่ " + item.ResultItemName.Replace("R", "").Trim() : " " + item.ResultItemName.Replace("R", "").Trim();
                        }
                    }

                    translate = result + db + " " + hz + " Hz";
                }

                rightTran.ResultValue = translate;
                rightResult.ResultValue = result;
            }
            catch (Exception)
            {

            }

        }

        public void CalculateLeftResult()
        {
            try
            {
                string result = string.Empty;
                string translate = string.Empty;
                bool abnormal = false;
                var hz500 = LeftEarResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "AUDIO9" || p.ResultItemName == "500 L");
                var hz1000 = LeftEarResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "AUDIO10" || p.ResultItemName == "1000 L");
                var hz2000 = LeftEarResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "AUDIO11" || p.ResultItemName == "2000 L");
                var hz3000 = LeftEarResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "AUDIO12" || p.ResultItemName == "3000 L");
                var hz4000 = LeftEarResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "AUDIO13" || p.ResultItemName == "4000 L");
                var hz6000 = LeftEarResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "AUDIO14" || p.ResultItemName == "6000 L");
                var hz8000 = LeftEarResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "AUDIO15" || p.ResultItemName == "8000 L");
                var leftTran = LeftEarResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "AUDIO16" || p.ResultItemName == "แปลผลหูซ้าย");
                var leftResult = LeftEarResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "AUDIO18" || p.ResultItemName == "สรุปผลหูซ้าย");

                if (int.Parse(hz6000.ResultValue) > 25 && int.Parse(hz4000.ResultValue) > 25
    || (int.Parse(hz500.ResultValue) > 25 && int.Parse(hz1000.ResultValue) > 25 && int.Parse(hz2000.ResultValue) > 25
    && int.Parse(hz3000.ResultValue) > 25 && int.Parse(hz4000.ResultValue) > 25))
                {
                    result = "ผิดปกติที่ความถี่สูง";
                    abnormal = true;
                }
                else if (int.Parse(hz6000.ResultValue) > 25 || int.Parse(hz8000.ResultValue) > 25 || int.Parse(hz500.ResultValue) > 25
                    || int.Parse(hz1000.ResultValue) > 25 || int.Parse(hz2000.ResultValue) > 25 || int.Parse(hz3000.ResultValue) > 25
                    || int.Parse(hz4000.ResultValue) > 25)
                {
                    result = "ระดับการได้ยินลดลงเล็กน้อย";
                    abnormal = true;
                }
                else if (int.Parse(hz6000.ResultValue) <= 25 && int.Parse(hz8000.ResultValue) <= 25 && int.Parse(hz500.ResultValue) <= 25
                    && int.Parse(hz1000.ResultValue) <= 25 && int.Parse(hz2000.ResultValue) <= 25 && int.Parse(hz3000.ResultValue) <= 25
                    && int.Parse(hz4000.ResultValue) <= 25)
                {
                    result = "ไม่พบความผิดปกติ";
                }
                translate = result;
                if (abnormal == true)
                {
                    string db = string.Empty;
                    string hz = string.Empty;
                    foreach (var item in LeftEarResultComponentItems.Where(p => p.ResultItemName.EndsWith("L")))
                    {
                        if (int.Parse(item.ResultValue) > 25)
                        {
                            db += " " + item.ResultValue;
                            hz += string.IsNullOrEmpty(hz) ? "dB ที่ " + item.ResultItemName.Replace("L", "").Trim() : " " + item.ResultItemName.Replace("L", "").Trim();
                        }
                    }

                    translate = result + db + " " + hz + " Hz";
                }

                leftTran.ResultValue = translate;
                leftResult.ResultValue = result;
            }
            catch (Exception)
            {

            }

        }

        #endregion
    }
}
