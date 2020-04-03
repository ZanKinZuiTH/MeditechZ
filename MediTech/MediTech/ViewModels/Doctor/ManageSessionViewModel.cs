using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ManageSessionViewModel : MediTechViewModelBase
    {
        #region Properties

        private List<CareproviderModel> _Doctors;

        public List<CareproviderModel> Doctors
        {
            get { return _Doctors; }
            set { _Doctors = value; }
        }

        private CareproviderModel _SelectDoctor;

        public CareproviderModel SelectDoctor
        {
            get { return _SelectDoctor; }
            set { Set(ref _SelectDoctor, value); }
        }

        private DateTime? _StartDate;

        public DateTime? StartDate
        {
            get { return _StartDate; }
            set { Set(ref _StartDate, value); }
        }

        private DateTime? _EndDate;

        public DateTime? EndDate
        {
            get { return _EndDate; }
            set { Set(ref _EndDate, value); }
        }
        private DateTime _StartTime;

        public DateTime StartTime
        {
            get { return _StartTime; }
            set { Set(ref _StartTime ,value); }
        }

        private DateTime _EndTime;

        public DateTime EndTime
        {
            get { return _EndTime; }
            set { Set(ref _EndTime, value); }
        }

        private bool _IsDay1;

        public bool IsDay1
        {
            get { return _IsDay1; }
            set { Set(ref _IsDay1, value); }
        }

        private bool _IsDay2;

        public bool IsDay2
        {
            get { return _IsDay2; }
            set { Set(ref _IsDay2, value); }
        }

        private bool _IsDay3;

        public bool IsDay3
        {
            get { return _IsDay3; }
            set { Set(ref _IsDay3, value); }
        }

        private bool _IsDay4;

        public bool IsDay4
        {
            get { return _IsDay4; }
            set { Set(ref _IsDay4, value); }
        }

        private bool _IsDay5;

        public bool IsDay5
        {
            get { return _IsDay5; }
            set { Set(ref _IsDay5, value); }
        }

        private bool _IsDay6;

        public bool IsDay6
        {
            get { return _IsDay6; }
            set { Set(ref _IsDay6, value); }
        }

        private bool _IsDay7;

        public bool IsDay7
        {
            get { return _IsDay7; }
            set { Set(ref _IsDay7, value); }
        }
        #endregion

        #region Command

        private RelayCommand _SaveCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                    ?? (_SaveCommand = new RelayCommand(SaveSession));
            }
        }

        private RelayCommand _CancelCommand;

        /// <summary>
        /// Gets the SaveCommand.
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

        SessionDefinitionModel sessionModel;

        public ManageSessionViewModel()
        {
            Doctors = DataService.UserManage.GetCareproviderDoctor();
            StartDate = DateTime.Now;
            EndDate = null;
        }
        private void SaveSession()
        {
            try
            {
                if (SelectDoctor == null)
                {
                    WarningDialog("กรุณาระบุแพทย์");
                    return;
                }
                if (StartDate == null)
                {
                    WarningDialog("กรุณาระบุวันเริ่ม");
                    return;
                }
                if (EndDate == null)
                {
                    WarningDialog("กรุณาระบุวันสุดท้าย");
                    return;
                }
                AssingProperitesToModel();
                DataService.Radiology.ManageSession(sessionModel, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ListSession listSession = new ListSession();
                ChangeViewPermission(listSession);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void Cancel()
        {
            ListSession listSession = new ListSession();
            ChangeViewPermission(listSession);
        }

        public void AssingModel(SessionDefinitionModel model)
        {
            this.sessionModel = model;

            SelectDoctor = Doctors.FirstOrDefault(p => p.CareproviderUID == sessionModel.CareproviderUID);
            StartDate = sessionModel.StartDttm;
            EndDate = sessionModel.EndDttm;
            StartTime = sessionModel.SessionStartDttm;
            EndTime = sessionModel.SessionEndDttm;
            IsDay1 = sessionModel.Day1;
            IsDay2 = sessionModel.Day2;
            IsDay3 = sessionModel.Day3;
            IsDay4 = sessionModel.Day4;
            IsDay5 = sessionModel.Day5;
            IsDay6 = sessionModel.Day6;
            IsDay7 = sessionModel.Day7;
        }
        public void AssingProperitesToModel()
        {
            if (sessionModel == null)
            {
                sessionModel = new SessionDefinitionModel();
            }

            sessionModel.CareproviderUID = SelectDoctor.CareproviderUID;
            sessionModel.StartDttm = StartDate.Value;
            sessionModel.EndDttm = EndDate.Value;
            sessionModel.SessionStartDttm = DateTime.Now.Date.Add(StartTime.TimeOfDay);
            sessionModel.SessionEndDttm = DateTime.Now.Date.Add(EndTime.TimeOfDay);
            sessionModel.Day1 = IsDay1;
            sessionModel.Day2 = IsDay2;
            sessionModel.Day3 = IsDay3;
            sessionModel.Day4 = IsDay4;
            sessionModel.Day5 = IsDay5;
            sessionModel.Day6 = IsDay6;
            sessionModel.Day7 = IsDay7;
        }
        #endregion
    }
}
