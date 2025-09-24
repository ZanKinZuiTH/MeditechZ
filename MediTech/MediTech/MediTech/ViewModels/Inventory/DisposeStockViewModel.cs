using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Models;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class DisposeStockViewModel : MediTechViewModelBase
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

        private string _BatchID;

        public string BatchID
        {
            get { return _BatchID; }
            set { Set(ref _BatchID, value); }
        }


        private List<HealthOrganisationModel> _Organisations;

        public List<HealthOrganisationModel> Organisations
        {
            get { return _Organisations; }
            set { Set(ref _Organisations, value); }
        }

        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set
            {
                Set(ref _SelectOrganisation, value);
                if (_SelectOrganisation != null)
                {
                    Locations = DataService.MasterData.GetLocationByOrganisationUID(_SelectOrganisation.HealthOrganisationUID);
                }
            }
        }

        private List<LocationModel> _Locations;
        public List<LocationModel> Locations
        {
            get { return _Locations; }
            set { Set(ref _Locations, value); }
        }

        private LocationModel _SelectLocation;

        public LocationModel SelectLocation
        {
            get { return _SelectLocation; }
            set
            {
                Set(ref _SelectLocation, value);
                if (_SelectLocation != null)
                {
                    Stores = DataService.Inventory.GetStoreByLocationUID(_SelectLocation.LocationUID);
                }
            }
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

        private string _ItemName;

        public string ItemName
        {
            get { return _ItemName; }
            set { Set(ref _ItemName, value); }
        }

        private ObservableCollection<StockModel> _StockList;

        public ObservableCollection<StockModel> StockList
        {
            get { return _StockList; }
            set { Set(ref _StockList, value); }
        }

        private ObservableCollection<StockModel> _SelectStockList;

        public ObservableCollection<StockModel> SelectStockList
        {
            get { return _SelectStockList ?? (_SelectStockList = new ObservableCollection<StockModel>()); }
            set { Set(ref _SelectStockList, value); }
        }

        #endregion

        #region Command

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get
            {
                return _SearchCommand
                    ?? (_SearchCommand = new RelayCommand(Search));
            }

        }

        private RelayCommand _ClearCommand;

        public RelayCommand ClearCommand
        {
            get
            {
                return _ClearCommand
                    ?? (_ClearCommand = new RelayCommand(Clear));
            }

        }

        private RelayCommand _DisposeCommand;

        public RelayCommand DisposeCommand
        {
            get
            {
                return _DisposeCommand
                    ?? (_DisposeCommand = new RelayCommand(Dispose));
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

        #region Method

        public DisposeStockViewModel()
        {
            Organisations = GetHealthOrganisationIsRoleStock();

            if (Organisations != null)
            {
                SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            }
        }

        private void Search()
        {
            if (SelectStore == null)
            {
                WarningDialog("กรุณาเลือก Store");
                return;
            }
            var tempStock = DataService.Inventory.SearchStockForDispose(DateFrom, DateTo, SelectStore.StoreUID, BatchID, ItemName);
            StockList = new ObservableCollection<StockModel>(tempStock);
        }

        private void Clear()
        {
            DateFrom = null;
            DateTo = null;
            BatchID = string.Empty;
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            ItemName = string.Empty;
        }

        private void Dispose()
        {
            try
            {
                if (SelectStockList != null)
                {
                    DisposeStockReasonViewModel dsrvm = (DisposeStockReasonViewModel)ShowModalDialogUsingViewModel
                        (new DisposeStockReason(), new DisposeStockReasonViewModel(SelectStockList.ToList()), true);
                    if (dsrvm.ResultDialog == ActionDialog.Save)
                    {
                        DataService.Inventory.DisposeStock(dsrvm.StockForDispose, SelectStore.StoreUID, dsrvm.SelectDisposeReason.Key.Value, dsrvm.Comments, AppUtil.Current.UserID);
                        SaveSuccessDialog();
                        Search();
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorDialog(ex.Message);
            }
        }


        private void Cancel()
        {
            ChangeViewPermission(null);
        }
        #endregion
    }
}
