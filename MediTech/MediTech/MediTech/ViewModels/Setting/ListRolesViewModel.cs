using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Models;
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
    public class ListRolesViewModel : MediTechViewModelBase
    {

        #region Properties

        private List<RoleModel> _RolesSource;

        public List<RoleModel> RolesSource
        {
            get { return _RolesSource; }
            set { _RolesSource = value; }
        }

        private RoleModel _SelectedRole;

        public RoleModel SelectedRole
        {
            get { return _SelectedRole; }
            set { _SelectedRole = value; }
        }


        #endregion

        #region Command

        private RelayCommand _AddRoleCommand;
        public RelayCommand AddRoleCommand
        {
            get { return _AddRoleCommand ?? (_AddRoleCommand = new RelayCommand(AddRole)); }
        }

        private RelayCommand _EditRoleCommand;
        public RelayCommand EditRoleCommand
        {
            get { return _EditRoleCommand ?? (_EditRoleCommand = new RelayCommand(EditRole)); }
        }


        private RelayCommand _DeleteCommand;
        public RelayCommand DeleteCommand
        {
            get { return _DeleteCommand ?? (_DeleteCommand = new RelayCommand(DeleteRole)); }
        }
        #endregion

        #region Method

        public ListRolesViewModel()
        {
            RolesSource = DataService.RoleManage.GetRole();
        }
        public void AddRole()
        {
            ManageRole roleManageView = new ManageRole();
            ChangeViewPermission(roleManageView);
        }

        public void EditRole()
        {
            if (_SelectedRole != null)
            {
                ManageRole roleManageView = new ManageRole();
                ChangeViewPermission(roleManageView);
                roleManageView.AssignModel(_SelectedRole.RoleUID);
            }
        }

        public void DeleteRole()
        {
            if (_SelectedRole != null)
            {
                MessageBoxResult result = DeleteDialog();
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {

                        DeleteSuccessDialog();
                        DataService.RoleManage.DeleteRole(_SelectedRole.RoleUID, AppUtil.Current.UserID);
                        RolesSource.Remove(_SelectedRole);
                        OnUpdateEvent();
                    }
                    catch (Exception er)
                    {

                        ErrorDialog(er.Message);
                    }

                }
            }
        }


        #endregion

    }
}
