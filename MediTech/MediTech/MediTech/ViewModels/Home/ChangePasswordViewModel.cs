using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ChangePasswordViewModel : MediTechViewModelBase
    {


        #region Properties

        private string _OldPassword;

        public string OldPassword
        {
            get { return _OldPassword; }
            set { Set(ref _OldPassword, value); }
        }


        private string _NewPassword;

        public string NewPassword
        {
            get { return _NewPassword; }
            set { Set(ref _NewPassword, value); }
        }


        private string _ConfirmPassword;

        public string ConfirmPassword
        {
            get { return _ConfirmPassword; }
            set { Set(ref _ConfirmPassword, value); }
        }

        private string _WarningMessage;

        public string WarningMessage
        {
            get { return _WarningMessage; }
            set { Set(ref _WarningMessage, value); }
        }


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
        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get
            {
                return _CancelCommand
                    ?? (_CancelCommand = new RelayCommand(Cancel));
            }

        }




        #endregion

        public ChangePasswordViewModel()
        {
        }

        private void Save()
        {
            try
            {
                WarningMessage = "";
                if (string.IsNullOrEmpty(OldPassword))
                {
                    WarningMessage = "กรุณาระบุรหัสเก่า";
                    return;
                }
                if (string.IsNullOrEmpty(NewPassword))
                {
                    WarningMessage = "กรุณาระบุรหัสใหม่";
                    return;
                }
                if (string.IsNullOrEmpty(ConfirmPassword))
                {
                    WarningMessage = "กรุณายืนยันรหัส";
                    return;
                }

                if (NewPassword != ConfirmPassword)
                {
                    WarningMessage = "กรุณายืนยันรหัสให้ถูกต้อง";
                    return;
                }

                string oldPassword = Encryption.Encrypt(OldPassword);

                LoginModel checkLoging = new LoginModel();
                checkLoging.LoginName = AppUtil.Current.LoginName;
                checkLoging.Password = oldPassword;
                var user = DataService.UserManage.CheckUserByLogin(checkLoging);

                if (user == null || (user != null && user.Count == 0))
                {
                    WarningMessage = "รหัสเก่าไม่ถูกต้อง";
                    return;
                }
                string newPassword = Encryption.Encrypt(NewPassword);
                LoginModel loginModel = new LoginModel();
                loginModel.LoginUID = AppUtil.Current.LoginID;
                loginModel.Password = newPassword;
                DataService.UserManage.ChangePassword(loginModel);
                SaveSuccessDialog();
                ClearData();
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }
        }


        public void ClearData()
        {
            WarningMessage = "";
            OldPassword = "";
            NewPassword = "";
            ConfirmPassword = "";
        }

        private void Cancel()
        {
            ChangeView(null, "");
        }

    }
}
