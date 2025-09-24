using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
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
    public class ListUsersViewModel : MediTechViewModelBase
    {

        #region Properties

        private List<CareproviderModel> _Careproviders;

        public List<CareproviderModel> Careproviders
        {
            get { return _Careproviders; }
            set { Set(ref _Careproviders, value); }
        }

        public CareproviderModel SelectCareprovider { get; set; }
        #endregion

        #region Command

        private RelayCommand _AddCommand;
        public RelayCommand AddCommand
        {
            get { return _AddCommand ?? (_AddCommand = new RelayCommand(AddUser)); }
        }

        private RelayCommand _EditCommand;
        public RelayCommand EditCommand
        {
            get { return _EditCommand ?? (_EditCommand = new RelayCommand(EditUser)); }
        }

        private RelayCommand _DeleteCommand;
        public RelayCommand DeleteCommand
        {
            get { return _DeleteCommand ?? (_DeleteCommand = new RelayCommand(DeleteUser)); }
        }

        #endregion

        #region Method

        public ListUsersViewModel()
        {
            Careproviders = DataService.UserManage.GetCareproviderAll();
        }

        private void AddUser()
        {
            ManageUser pageUserManage = new ManageUser();
            ChangeViewPermission(pageUserManage);
        }

        private void EditUser()
        {
            if (SelectCareprovider != null)
            {
                ManageUser pageUserManage = new ManageUser();
                (pageUserManage.DataContext as ManageUserViewModel).AssingModel(SelectCareprovider.CareproviderUID);
                ChangeViewPermission(pageUserManage);
            }

        }

        private void DeleteUser()
        {
            if (SelectCareprovider != null)
            {
                try
                {
                    MessageBoxResult result = DeleteDialog();
                    if (result == MessageBoxResult.Yes)
                    {
                        int? loginUID = null;
                        if (SelectCareprovider.loginModel != null)
                        {
                            loginUID = SelectCareprovider.loginModel.LoginUID;
                        }
                        DataService.UserManage.DeleteCareProvider(SelectCareprovider.CareproviderUID, loginUID, AppUtil.Current.UserID);
                        DeleteSuccessDialog();
                        Careproviders.Remove(SelectCareprovider);
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
