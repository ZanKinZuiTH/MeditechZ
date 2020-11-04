using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class ListCheckupJobViewModel : MediTechViewModelBase
    {
        #region Properties
        private List<CheckupJobContactModel> _ListCheckupJob;

        public List<CheckupJobContactModel> ListCheckupJob
        {
            get { return _ListCheckupJob; }
            set { Set(ref _ListCheckupJob, value); }
        }

        private CheckupJobContactModel _SelectCheckupJob;

        public CheckupJobContactModel SelectCheckupJob
        {
            get { return _SelectCheckupJob; }
            set { Set(ref _SelectCheckupJob, value); }
        }

        #endregion

        #region Command


        private RelayCommand _AddCommand;

        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand
                    ?? (_AddCommand = new RelayCommand(AddJob));
            }
        }


        private RelayCommand _EditCommand;

        public RelayCommand EditCommand
        {
            get
            {
                return _EditCommand
                    ?? (_EditCommand = new RelayCommand(EditJob));
            }
        }


        private RelayCommand _DeleteCommand;

        public RelayCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand
                    ?? (_DeleteCommand = new RelayCommand(DeleteJob));
            }
        }

        #endregion

        #region Method

        public ListCheckupJobViewModel()
        {
            ListCheckupJob = DataService.Checkup.GetCheckupJobContactAll();
        }

        void AddJob()
        {
            ManageCheckupJob pageManage = new ManageCheckupJob();
            ChangeViewPermission(pageManage);
        }

        void EditJob()
        {
            if (SelectCheckupJob != null)
            {
                ManageCheckupJob pageUserManage = new ManageCheckupJob();
                (pageUserManage.DataContext as ManageCheckupJobViewModel).AssingModel(SelectCheckupJob.CheckupJobContactUID);
                ChangeViewPermission(pageUserManage);
            }
        }

        void DeleteJob()
        {
            if (SelectCheckupJob != null)
            {
                try
                {
                    MessageBoxResult result = DeleteDialog();
                    if (result == MessageBoxResult.Yes)
                    {

                        DataService.Checkup.DeleteCheckupJobContact(SelectCheckupJob.CheckupJobContactUID, AppUtil.Current.UserID);
                        DeleteSuccessDialog();
                        ListCheckupJob.Remove(SelectCheckupJob);
                        OnUpdateEvent();
                    }
                }
                catch (Exception er)
                {

                    ErrorDialog(er.Message);
                }

            }
        }

        #endregion
    }
}
