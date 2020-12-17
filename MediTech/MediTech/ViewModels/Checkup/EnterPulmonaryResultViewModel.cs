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
            (this.View as EnterPulmonaryResult).patientBanner.SetPatientBanner(RequestModel);
            double height = Convert.ToDouble(this.RequestModel.Height);
            string warningMessage = string.Empty;

            if (string.IsNullOrEmpty(this.RequestModel.Gender))
            {
                warningMessage = "* คนไข้ไม่ได้ระบุเพศ กรุณาตรวจสอบ";
            }
            if (string.IsNullOrEmpty(this.RequestModel.PatientAge))
            {
                warningMessage += string.IsNullOrEmpty(warningMessage) ? "* คนไข้ไม่ได้ระบุอายุ กรุณาตรวจสอบ" : "\r\n* คนไข้ไม่ได้ระบุอายุ กรุณาตรวจสอบ";
            }
            if (this.RequestModel.Height == 0)
            {
                warningMessage += string.IsNullOrEmpty(warningMessage) ? "* คนไข้ไม่ได้ระบุส่วนสูง กรุณาตรวจสอบ" : "\r\n* คนไข้ไม่ได้ระบุส่วนสูง กรุณาตรวจสอบ";
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
