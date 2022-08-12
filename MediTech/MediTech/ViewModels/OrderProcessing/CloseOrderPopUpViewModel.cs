using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class CloseOrderPopUpViewModel : MediTechViewModelBase
    {

        #region Properties

        private DateTime? _CloseDate;

        public DateTime? CloseDate
        {
            get { return _CloseDate; }
            set { _CloseDate = value; }
        }

        private DateTime? _CloseTime;

        public DateTime? CloseTime
        {
            get { return _CloseTime; }
            set { _CloseTime = value; }
        }


        #endregion

        #region Command


        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save));
            }
        }


        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel));
            }
        }

        #endregion

        #region Method

        void Save()
        {
            CloseViewDialog(ActionDialog.Save);
        }

        void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }
        #endregion
    }
}
