using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediTech.Model;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using MediTech.DataService;
using MediTech.Views;

namespace MediTech.ViewModels
{
    public class CancelPopupViewModel : MediTechViewModelBase
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
