using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class NewIPFillsViewModel : MediTechViewModelBase
    {
        #region Properties
        #endregion

        #region Command
        private RelayCommand _SearchCommand;
        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(Search)); }
        }

        private RelayCommand _ClearCommand;

        public RelayCommand ClearCommand
        {
            get { return _ClearCommand ?? (_ClearCommand = new RelayCommand(Clear)); }
        }

        private RelayCommand _DispenseCommand;
        public RelayCommand DispenseCommand
        {
            get { return _DispenseCommand ?? (_DispenseCommand = new RelayCommand(Dispense)); }
        }

        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }

        #endregion

        #region Method
        private void Search()
        {

        }

        private void Dispense()
        {

        }

        private void Clear()
        {
            
        }

        private void Cancel()
        {

        }


        #endregion
    }
}
