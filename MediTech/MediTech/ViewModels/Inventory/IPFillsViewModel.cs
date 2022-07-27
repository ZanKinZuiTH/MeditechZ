using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediTech.ViewModels
{
    public class IPFillsViewModel : MediTechViewModelBase
    {
        #region Properties

        private DateTime? _DateFrom;
        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set { Set(ref _DateFrom, value); }
        }

        private DateTime? _DateTo;
        public DateTime? DateTo
        {
            get { return _DateTo; }
            set { Set(ref _DateTo, value); }
        }

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

        private List<IPFillProcessModel> _FillProcess;
        public List<IPFillProcessModel> FillProcess
        {
            get { return _FillProcess; }
            set { Set(ref _FillProcess, value); }
        }

        private IPFillProcessModel _SelectFillProcess;
        public IPFillProcessModel SelectFillProcess
        {
            get { return _SelectFillProcess; }
            set { Set(ref _SelectFillProcess, value); }
        }

        #endregion

        #region Command

        private RelayCommand _SearchCommand;
        public RelayCommand SearchCommand
        {
            get{ return _SearchCommand ?? (_SearchCommand = new RelayCommand(Search));}
        }

        private RelayCommand _ClearCommand;

        public RelayCommand ClearCommand
        {
            get { return _ClearCommand ?? (_ClearCommand = new RelayCommand(Clear)); }
        }

        private RelayCommand _NewFillsCommand;
        public RelayCommand NewFillsCommand
        {
            get { return _NewFillsCommand ?? (_NewFillsCommand = new RelayCommand(NewFill)); }
        }

        private RelayCommand _ViewDetailCommand;
        public RelayCommand ViewDetailCommand
        {
            get { return _ViewDetailCommand ?? (_ViewDetailCommand = new RelayCommand(ViewDetail)); }
        }

        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }
        #endregion

        #region Method
        public IPFillsViewModel()
        {
            DateFrom = DateTime.Now;
            Stores = DataService.Inventory.GetStore();
            SelectStore = Stores.FirstOrDefault();
        }

        private void Search()
        {
            FillProcess = DataService.Inventory.SearchIPFills(DateFrom, DateTo, SelectStore.StoreUID);
        }

        private void NewFill()
        {
            NewIPFills newIP = new NewIPFills();
            ChangeViewPermission(newIP);
        }

        private void ViewDetail()
        {
            if (SelectFillProcess != null)
            {
                IPFillsDetail page = new IPFillsDetail();
                if(page.DataContext is IPFillsDetailViewModel)
                {
                    (page.DataContext as IPFillsDetailViewModel).AssignModel(SelectFillProcess);
                    ChangeViewPermission(page);
                }
            }
        }

        private void Clear()
        {
            DateFrom = null;
            DateTo = null;
            FillProcess = null;
        }

        private void Cancel()
        {
            if (SelectFillProcess != null)
            {
                MessageBoxResult diagResult = DeleteDialog();
                if (diagResult == MessageBoxResult.Yes)
                {
                    int fillId = Convert.ToInt32(SelectFillProcess.IPFillProcessUID);
                    DataService.Inventory.CancelDispenseIPFills(fillId, AppUtil.Current.UserID);
                    Search();
                }
            }
        }

        #endregion
    }
}
