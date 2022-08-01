using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class CreateDispenseReturnViewModel :MediTechViewModelBase
    {
        #region Properties
        private List<StoreModel> _Stores;
        public List<StoreModel> Stores
        {
            get { return _Stores; }
            set { Set(ref _Stores, value); }
        }

        private StoreModel _SelectStore;
        public StoreModel SelectStore
        {
            get { return _SelectStore; }
            set { Set(ref _SelectStore, value); }
        }

        private List<PrescriptionItemModel> _PrescriptionItem;
        public List<PrescriptionItemModel> PrescriptionItem
        {
            get { return _PrescriptionItem; }
            set { Set(ref _PrescriptionItem, value); }
        }

        private PrescriptionItemModel _SelectPrescriptionItem;
        public PrescriptionItemModel SelectPrescriptionItem
        {
            get { return _SelectPrescriptionItem; }
            set { Set(ref _SelectPrescriptionItem, value); }
        }

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

        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save)); }
        }

        private RelayCommand _CloseCommand;
        public RelayCommand CloseCommand
        {
            get { return _CloseCommand ?? (_CloseCommand = new RelayCommand(Close)); }
        }
        #endregion

        #region Method
        public CreateDispenseReturnViewModel()
        {


        }

        private void Search()
        {

        }

        private void Save()
        {

        }

        private void Close()
        {
            DispenseReturns dispense = new DispenseReturns();
            ChangeViewPermission(dispense);
        }


        private void Clear()
        {

        }
        #endregion
    }
}
