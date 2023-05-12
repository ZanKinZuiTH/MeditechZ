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
    public class ExecutePopUpViewModel : MediTechViewModelBase
    {
        #region Properties

        private ObservableCollection<CareproviderModel> _Radiologists;

        public ObservableCollection<CareproviderModel> Radiologists
        {
            get { return _Radiologists; }
            set { Set(ref _Radiologists, value); }
        }

        private List<CareproviderModel> _ExecuteBy;

        public List<CareproviderModel> ExecuteBy
        {
            get { return _ExecuteBy; }
            set { Set(ref _ExecuteBy, value); }
        }

        private List<CareproviderModel> _UltrasoundBy;

        public List<CareproviderModel> UltrasoundBy
        {
            get { return _UltrasoundBy; }
            set { Set(ref _UltrasoundBy, value); }
        }

        private CareproviderModel _SelectRaiologist;

        public CareproviderModel SelectRaiologist
        {
            get { return _SelectRaiologist; }
            set
            {
                Set(ref _SelectRaiologist, value);
                if (_SelectRaiologist != null)
                {
                    foreach (var item in ExecuteList)
                    {
                        item.RadiologistUID = SelectRaiologist.CareproviderUID;
                        OnUpdateEvent();
                    }
                }
            }
        }

        private CareproviderModel _SelectExecuteBy;

        public CareproviderModel SelectExecuteBy
        {
            get { return _SelectExecuteBy; }
            set { Set(ref _SelectExecuteBy, value); }
        }

        private CareproviderModel _SelectUltrasoundBy;

        public CareproviderModel SelectUltrasoundBy
        {
            get { return _SelectUltrasoundBy; }
            set { Set(ref _SelectUltrasoundBy, value); }
        }

        private List<RequestListModel> _ExecuteList;

        public List<RequestListModel> ExecuteList
        {
            get { return _ExecuteList; }
            set
            {
                Set(ref _ExecuteList, value);
            }
        }


        private DateTime _StartDate;

        public DateTime StartDate
        {
            get { return _StartDate; }
            set { Set(ref _StartDate, value); }
        }

        private DateTime _StartTime;
        public DateTime StartTime
        {
            get { return _StartTime; }
            set { Set(ref _StartTime, value); }
        }

        private string _Note;

        public string Note
        {
            get { return _Note; }
            set { Set(ref _Note, value); }
        }


        #endregion

        #region Command

        private RelayCommand _ExecuteCommand;

        /// <summary>
        /// Gets the ExecuteCommand.
        /// </summary>
        public RelayCommand ExecuteCommand
        {
            get
            {
                return _ExecuteCommand
                    ?? (_ExecuteCommand = new RelayCommand(Execute));
            }
        }

        private RelayCommand _CancelCommand;

        /// <summary>
        /// Gets the ExecuteCommand.
        /// </summary>
        public RelayCommand CancelCommand
        {
            get
            {
                return _CancelCommand
                    ?? (_CancelCommand = new RelayCommand(Cancel));
            }
        }

        #endregion

        #region Method

        public ExecutePopUpViewModel()
        {
            var Careprovider = DataService.UserManage.GetCareproviderAll();
            ExecuteBy = Careprovider;
            UltrasoundBy = Careprovider;
            Radiologists = new ObservableCollection<CareproviderModel>(Careprovider.Where(p => p.IsRadiologist).ToList());
            Radiologists.Insert(0, new CareproviderModel { CareproviderUID = 0, FirstName = "Assign", LastName = "Auto", FullName = "Assign Auto" });

            SelectExecuteBy = ExecuteBy.FirstOrDefault(p => p.CareproviderUID == AppUtil.Current.UserID);

            DateTime now = DateTime.Now;
            StartDate = now.Date;
            StartTime = now;
        }


        public void Execute()
        {
            try
            {
                if (SelectExecuteBy == null)
                {
                    WarningDialog("กรุณาเลือกคน Execute");
                    return;
                }
                DateTime executeDttm = StartDate.Add(StartTime.TimeOfDay);
                foreach (var item in ExecuteList)
                {
                    if (item.RadiologistUID == null)
                    {
                        WarningDialog("กรุณา Assign รังสีแพทย์ให้ครบ");
                        return;
                    }
                    item.PreparedDttm = executeDttm;
                    item.ExecuteByUID = SelectExecuteBy.CareproviderUID;
                    item.UltrasoundByUID = SelectUltrasoundBy != null ? SelectUltrasoundBy.CareproviderUID : (int?)null;

                }
                DataService.Radiology.AssignRadiologist(ExecuteList.ToList(), AppUtil.Current.UserID);
                SaveSuccessDialog();
                CloseViewDialog(ActionDialog.Save);

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        public void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        #endregion
    }
}
