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
    public class EnterMuscleTestViewModel : MediTechViewModelBase
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
            (this.View as EnterMuscleTest).patientBanner.SetPatientBanner(RequestModel);
        }

        public void AssignModel(RequestListModel request)
        {
            var dt = DataService.PatientHistory.GetPatientVitalSignByVisitUID(request.PatientVisitUID).OrderByDescending(p => p.MWhen).FirstOrDefault();
            if(dt != null)
            {
                request.Weight = dt.Weight ?? 0;
            }
            this.RequestModel = request;
            RequestItemName = this.RequestModel.RequestItemName;
            var dataList = DataService.Checkup.GetResultItemByRequestDetailUID(request.RequestDetailUID);
            if (dataList != null)
            {
                ResultComponentItems = new ObservableCollection<ResultComponentModel>(dataList);
                foreach (var item in ResultComponentItems)
                {
                    if (!string.IsNullOrEmpty(item.AutoValue))
                        item.AutoValueList = item.AutoValue.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p)).ToList();
                }
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
        public void CalculateMuscleValue()
        {
            double weight = Convert.ToDouble(this.RequestModel.Weight);
            try
            {
                var val_back_Strength = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "MUCS1" || p.ResultItemName == "Value Back Strength");
                var back_Strength = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "MUCS2" || p.ResultItemName == "Back Strength");

                var val_grip_Strength = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "MUCS3" || p.ResultItemName == "Value Grip Strength");
                var grip_Strength = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "MUCS4" || p.ResultItemName == "Grip Strength");

                var val_leg_Strength = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "MUCS5" || p.ResultItemName == "Value Leg Strength");
                var leg_Strength = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "MUCS6" || p.ResultItemName == "Leg Strength");

                if (val_back_Strength != null)
                    back_Strength.ResultValue = (Math.Round(double.Parse(val_back_Strength.ResultValue) / weight, 2)).ToString();

                if (val_grip_Strength != null)
                    grip_Strength.ResultValue = (Math.Round(double.Parse(val_grip_Strength.ResultValue) / weight, 2)).ToString();

                if (val_leg_Strength != null)
                    leg_Strength.ResultValue = (Math.Round(double.Parse(val_leg_Strength.ResultValue) / weight, 2)).ToString();
            }
            catch (Exception)
            {

            }
        }
        #endregion
    }
}
