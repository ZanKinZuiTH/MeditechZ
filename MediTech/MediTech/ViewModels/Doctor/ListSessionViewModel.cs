using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediTech.ViewModels
{
    public class ListSessionViewModel : MediTechViewModelBase
    {
        #region Properites

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

        private List<SessionDefinitionModel> _Sessions;

        public List<SessionDefinitionModel> Sessions
        {
            get { return _Sessions; }
            set { Set(ref _Sessions, value); }
        }

        private SessionDefinitionModel _SelectSession;

        public SessionDefinitionModel SelectSession
        {
            get { return _SelectSession; }
            set { Set(ref _SelectSession, value); }
        }

        #endregion

        #region Command

        private RelayCommand _SearchCommand;

        /// <summary>
        /// Gets the SearchCommand.
        /// </summary>
        public RelayCommand SearchCommand
        {
            get
            {
                return _SearchCommand
                    ?? (_SearchCommand = new RelayCommand(SearchSession));
            }
        }

        private RelayCommand _ClearCommand;

        /// <summary>
        /// Gets the ClearCommand.
        /// </summary>
        public RelayCommand ClearCommand
        {
            get
            {
                return _ClearCommand
                    ?? (_ClearCommand = new RelayCommand(Clear));
            }
        }


        private RelayCommand _CreateCommand;

        /// <summary>
        /// Gets the CreateCommand.
        /// </summary>
        public RelayCommand CreateCommand
        {
            get
            {
                return _CreateCommand
                    ?? (_CreateCommand = new RelayCommand(CreateSession));
            }
        }

        private RelayCommand _EditCommand;

        /// <summary>
        /// Gets the EditCommand.
        /// </summary>
        public RelayCommand EditCommand
        {
            get
            {
                return _EditCommand
                    ?? (_EditCommand = new RelayCommand(EditSession));
            }
        }

        private RelayCommand _WithDrawCommand;

        /// <summary>
        /// Gets the WithDrawCommand.
        /// </summary>
        public RelayCommand WithDrawCommand
        {
            get
            {
                return _WithDrawCommand
                    ?? (_WithDrawCommand = new RelayCommand(WithDrawSession));
            }
        }

        private RelayCommand _DeleteCommand;

        /// <summary>
        /// Gets the WithDrawCommand.
        /// </summary>
        public RelayCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand
                    ?? (_DeleteCommand = new RelayCommand(DeleteSession));
            }
        }



        #endregion

        #region Method

        public ListSessionViewModel()
        {
            Doctors = DataService.UserManage.GetCareproviderDoctor();
            StartDate = DateTime.Now;
            EndDate = StartDate.Value.AddMonths(3);
            SearchSession();
        }

        private void SearchSession()
        {
            int? careproviderUID = null;

            if (SelectDoctor != null)
                careproviderUID = SelectDoctor.CareproviderUID;
            Sessions = DataService.Radiology.SearchSession(StartDate, EndDate, careproviderUID);
        }

        private void CreateSession()
        {
            ManageSession manageSession = new ManageSession();
            ChangeViewPermission(manageSession);
        }
        private void WithDrawSession()
        {
            if (SelectSession != null)
            {
                SessionWithdrawn withDrawSession = new SessionWithdrawn();
                (withDrawSession.DataContext as SessionWithdrawnViewModel).AssignModel(SelectSession);
                ChangeViewPermission(withDrawSession);
            }

        }

        private void EditSession()
        {
            if (SelectSession != null)
            {
                ManageSession manageSession = new ManageSession();
                (manageSession.DataContext as ManageSessionViewModel).AssingModel(SelectSession);
                ChangeViewPermission(manageSession);
            }

        }

        private void DeleteSession()
        {
            try
            {
                if (SelectSession != null)
                {
                    MessageBoxResult result = QuestionDialog("คุณต้องการลบ Session นี้ใช่หรือไม่ ?");
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.Radiology.DeleteSession(SelectSession.SessionDefinitionUID, AppUtil.Current.UserID);
                        DeleteSuccessDialog();
                        SearchSession();
                    }
     
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void Clear()
        {
            StartDate = DateTime.Now;
            EndDate = StartDate.Value.AddMonths(3);
            SelectDoctor = null;
        }
        #endregion
    }
}
