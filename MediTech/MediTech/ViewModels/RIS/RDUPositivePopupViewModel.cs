using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class RDUPositivePopupViewModel : MediTechViewModelBase
    {
        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }


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

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        private void Save()
        {
            CloseViewDialog(ActionDialog.Save);
        }
    }
}
