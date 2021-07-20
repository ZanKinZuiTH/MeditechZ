using GalaSoft.MvvmLight.Command;
using MediTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class BulkAlertDialogViewModel : MediTechViewModelBase
    {
        #region Propeties
        private PatientRegistrationBulkData _PatientHNDupicate;
        public PatientRegistrationBulkData PatientHNDupicate
        {
            get
            {
                return _PatientHNDupicate;
            }
            set
            {
                if (_PatientHNDupicate != value)
                {
                    _PatientHNDupicate = value;
                    Set(ref _PatientHNDupicate, value);
                }
            }
        }

        #endregion

        #region Command


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

        #region Method

        void Cancel()
        {
            this.CloseViewDialog(ActionDialog.Cancel);
        }

        #endregion
    }
}
