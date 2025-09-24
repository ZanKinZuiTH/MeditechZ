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
    public class SessionWithdrawnViewModel : MediTechViewModelBase
    {
        #region Properites

        public bool SuppressEvent { get; set; }

        private ObservableCollection<SessionWithdrawnDetailModel> _WithDrawSessions;

        public ObservableCollection<SessionWithdrawnDetailModel> WithDrawSessions
        {
            get { return _WithDrawSessions; }
            set { Set(ref _WithDrawSessions, value); }
        }

        private SessionWithdrawnDetailModel _SelectWithDrawSession;

        public SessionWithdrawnDetailModel SelectWithDrawSession
        {
            get { return _SelectWithDrawSession; }
            set
            {
                _SelectWithDrawSession = value;
                if (SelectWithDrawSession != null)
                {
                    SuppressEvent = true;
                    SelectReason = Reasons.FirstOrDefault(p => p.Key == SelectWithDrawSession.WTHRSUID);
                    Comment = SelectWithDrawSession.Comments;
                    SuppressEvent = false;
                }
            }
        }

        private DateTime _SelectedDate;

        public DateTime SelectedDate
        {
            get { return _SelectedDate; }
            set { Set(ref _SelectedDate, value); }
        }

        private DateTime _StartTime;

        public DateTime StartTime
        {
            get { return _StartTime; }
            set { Set(ref _StartTime, value); }
        }

        private DateTime _EndTime;

        public DateTime EndTime
        {
            get { return _EndTime; }
            set { Set(ref _EndTime, value); }
        }

        private string _Comment;

        public string Comment
        {
            get { return _Comment; }
            set
            {
                Set(ref _Comment, value);
                if (SelectWithDrawSession != null)
                {
                    if (!SuppressEvent)
                    {
                        SelectWithDrawSession.Comments = Comment;
                        OnUpdateEvent();
                    }
                }
            }
        }

        private List<LookupReferenceValueModel> _Reasons;

        public List<LookupReferenceValueModel> Reasons
        {
            get { return _Reasons; }
            set { Set(ref _Reasons, value); }
        }

        private LookupReferenceValueModel _SelectReason;

        public LookupReferenceValueModel SelectReason
        {
            get { return _SelectReason; }
            set
            {
                Set(ref _SelectReason, value);
                if (SelectWithDrawSession != null)
                {
                    if (!SuppressEvent)
                    {
                        SelectWithDrawSession.WithDrawReason = SelectReason == null ? null : SelectReason.Display;
                        SelectWithDrawSession.WTHRSUID = SelectReason == null ? 0 : SelectReason.Key.Value;
                        OnUpdateEvent();
                    }


                }
            }
        }

        private string _DoctorName;

        public string DoctorName
        {
            get { return _DoctorName; }
            set { Set(ref _DoctorName, value); }
        }


        #endregion

        #region Command

        private RelayCommand _AddCommand;

        /// <summary>
        /// Gets the AddCommand.
        /// </summary>
        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand
                    ?? (_AddCommand = new RelayCommand(AddWithDrawSession));
            }
        }

        private RelayCommand _RemoveCommand;

        /// <summary>
        /// Gets the RemoveCommand.
        /// </summary>
        public RelayCommand RemoveCommand
        {
            get
            {
                return _RemoveCommand
                    ?? (_RemoveCommand = new RelayCommand(RemoveWithDrawSession));
            }
        }

        private RelayCommand _SaveCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                    ?? (_SaveCommand = new RelayCommand(SaveWithDrawSession));
            }
        }

        private RelayCommand _CancelCommand;

        /// <summary>
        /// Gets the CancelCommand.
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

        List<SessionWithdrawnDetailModel> withDrawModel;
        int sessionDefinitionUID;
        public SessionWithdrawnViewModel()
        {
            Reasons = DataService.Technical.GetReferenceValueMany("WTHRS");
            SelectedDate = DateTime.Now;
            StartTime = DateTime.Now.Date;
            EndTime = DateTime.Now.Date.AddHours(23);
        }
        public void AddWithDrawSession()
        {
            DateTime startDttm = SelectedDate.Add(StartTime.TimeOfDay);
            DateTime endDttm = SelectedDate.Add(EndTime.TimeOfDay);

            foreach (var item in WithDrawSessions)
            {
                if (item.StartDttm == startDttm && item.EndDttm == endDttm)
                {
                    WarningDialog("ช่วงเวลานี้มีอยู่แล้ว โปรดตรวจสอบ");
                    return;
                }
            }

            SessionWithdrawnDetailModel withDrawDeailModel = new SessionWithdrawnDetailModel();

            withDrawDeailModel.SessionDefinitionUID = sessionDefinitionUID;


            withDrawDeailModel.Comments = Comment;
            withDrawDeailModel.StartDttm = startDttm;
            withDrawDeailModel.EndDttm = endDttm;
            WithDrawSessions.Add(withDrawDeailModel);
            OnUpdateEvent();
        }

        private void RemoveWithDrawSession()
        {
            if (SelectWithDrawSession != null)
            {
                WithDrawSessions.Remove(SelectWithDrawSession);
                OnUpdateEvent();
            }

            if (SelectWithDrawSession == null)
            {
                SelectReason = null;
                Comment = null;
            }
        }
        private void SaveWithDrawSession()
        {
            try
            {

                DataService.Radiology.WithDrawSession(WithDrawSessions.ToList(),sessionDefinitionUID, AppUtil.Current.UserID);
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

        public void AssignModel(SessionDefinitionModel sessionDefinition)
        {
            sessionDefinitionUID = sessionDefinition.SessionDefinitionUID;
            DoctorName = sessionDefinition.CareproviderName;
            withDrawModel = DataService.Radiology.GetWithDrawSessionBySessionUID(sessionDefinition.SessionDefinitionUID);
            WithDrawSessions = new ObservableCollection<SessionWithdrawnDetailModel>(withDrawModel);
        }

        #endregion
    }
}
