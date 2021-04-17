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
    public class EnterFitnessTestResultViewModel : MediTechViewModelBase
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
            (this.View as EnterFitnessTestResult).patientBanner.SetPatientBanner(RequestModel);
        }

        public void AssignModel(RequestListModel request)
        {
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

                foreach (var item in ResultComponentItems)
                {
                    if (item.ResultUID != null && !string.IsNullOrEmpty(item.ResultValue))
                    {
                        string[] values = item.ResultValue.Split(',');
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (item.AutoValueList != null && item.AutoValueList.Any(p => p == values[i]))
                            {
                                if (item.CheckDataList == null)
                                    item.CheckDataList = new List<object>();

                                item.CheckDataList.Add(values[i]);
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
                RequestDetailItemModel reviewRequestDetail = new RequestDetailItemModel();
                reviewRequestDetail.RequestUID = RequestModel.RequestUID;
                reviewRequestDetail.RequestDetailUID = RequestModel.RequestDetailUID;
                reviewRequestDetail.PatientUID = RequestModel.PatientUID;
                reviewRequestDetail.PatientVisitUID = RequestModel.PatientVisitUID;
                reviewRequestDetail.RequestItemCode = RequestModel.RequestItemCode;
                reviewRequestDetail.RequestItemName = RequestModel.RequestItemName;

                foreach (var item in ResultComponentItems)
                {
                    if (item.ResultItemCode == "PAR1221")
                    {
                        string resultValue = string.Empty;
                        if (item.CheckDataList != null)
                        {
                            foreach (var phyExam in item.CheckDataList)
                            {
                                resultValue += string.IsNullOrEmpty(resultValue) ? phyExam.ToString() : "," + phyExam.ToString();
                            }
                        }
                        item.ResultValue = resultValue;
                    }

                }

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
