using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ConsumptionStockViewModel : MediTechViewModelBase
    {

        #region Properties

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
                    Locations = DataService.MasterData.GetLocationByOrganisationUID(SelectOrganisation.HealthOrganisationUID);
                    LocationUseds = Locations;
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

        private List<LookupReferenceValueModel> _ItemTypes;

        public List<LookupReferenceValueModel> ItemTypes
        {
            get { return _ItemTypes; }
            set { Set(ref _ItemTypes, value); }
        }


        private LookupReferenceValueModel _SelectItemType;

        public LookupReferenceValueModel SelectItemType
        {
            get { return _SelectItemType; }
            set { Set(ref _SelectItemType, value); }
        }

        private string _ItemCode;

        public string ItemCode
        {
            get { return _ItemCode; }
            set { Set(ref _ItemCode, value); }
        }


        private string _ItemName;

        public string ItemName
        {
            get { return _ItemName; }
            set { Set(ref _ItemName, value); }
        }

        private double _Quantity;

        public double Quantity
        {
            get { return _Quantity; }
            set { Set(ref _Quantity, value); }
        }

        private DateTime? _ExpiryDate;

        public DateTime? ExpiryDate
        {
            get { return _ExpiryDate; }
            set { Set(ref _ExpiryDate, value); }
        }

        private double? _BatchQty;

        public double? BatchQty
        {
            get { return _BatchQty; }
            set { Set(ref _BatchQty, value); }
        }

        private List<LocationModel> _LocationUseds;

        public List<LocationModel> LocationUseds
        {
            get { return _LocationUseds; }
            set { Set(ref _LocationUseds, value); }
        }

        private LocationModel _SelectLocationUsed;

        public LocationModel SelectLocationUsed
        {
            get { return _SelectLocationUsed; }
            set { Set(ref _SelectLocationUsed, value); }
        }

        private string _BatchID;

        public string BatchID
        {
            get { return _BatchID; }
            set { Set(ref _BatchID, value); }
        }

        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { Set(ref _Comments, value); }
        }


        private List<StockModel> _CurrentStock;

        public List<StockModel> CurrentStock
        {
            get { return _CurrentStock; }
            set { Set(ref _CurrentStock, value); }
        }


        private StockModel _SelectCurrentStock;

        public StockModel SelectCurrentStock
        {
            get { return _SelectCurrentStock; }
            set
            {
                Set(ref _SelectCurrentStock, value);
                if (SelectCurrentStock != null)
                {
                    BatchID = SelectCurrentStock.BatchID;
                    ExpiryDate = SelectCurrentStock.ExpiryDttm;
                    BatchQty = SelectCurrentStock.Quantity;
                }
            }
        }


        private ObservableCollection<StockAdjustmentModel> _IssueStocks;

        public ObservableCollection<StockAdjustmentModel> IssueStocks
        {
            get { return _IssueStocks; }
            set { Set(ref _IssueStocks, value); }
        }

        private StockAdjustmentModel _SelectIssueStock;

        public StockAdjustmentModel SelectIssueStock
        {
            get { return _SelectIssueStock; }
            set { Set(ref _SelectIssueStock, value); }
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

        private RelayCommand _AddCommand;

        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand
                    ?? (_AddCommand = new RelayCommand(Add));
            }

        }

        private RelayCommand _DeleteCommand;

        public RelayCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand
                    ?? (_DeleteCommand = new RelayCommand(Delete));
            }

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

        #endregion

        #region Method

        public ConsumptionStockViewModel()
        {
            Organisations = GetHealthOrganisationIsRoleStock();
            ItemTypes = DataService.Technical.GetReferenceValueMany("ITMTYP");
            //Location = DataService.MasterData.GetLocationAll(AppUtil.Current.OwnerOrganisationUID);



            if (Organisations != null)
            {
                SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            }
        }
        private void Search()
        {
            if (SelectStore == null)
            {
                WarningDialog("กรุณาระบุ คลัง");
                return;
            }
            int? itemType = SelectItemType != null ? SelectItemType.Key : (int?)null;
            CurrentStock = DataService.Inventory.SearchStockBatch(SelectOrganisation.HealthOrganisationUID, SelectStore.StoreUID, itemType, ItemCode, ItemName);
        }

        private void Save()
        {
            if (IssueStocks == null || IssueStocks.Count <= 0)
            {
                WarningDialog("กรุณาใส่รายการของที่จะใช้");
                return;
            }

            List<ItemIssueDetailModel> listIssued = new List<ItemIssueDetailModel>();
            foreach (var item in IssueStocks)
            {
                ItemIssueDetailModel newRow = new ItemIssueDetailModel();
                //newRow.ItemIssueDetailUID = item.ItemListUID;
                newRow.OrganisationUID = item.OwnerOrganisationUID;
                newRow.StoreUID = item.StoreUID;
                newRow.StockUID = item.StockUID;
                newRow.ItemMasterUID = item.ItemMasterUID;
                newRow.ItemCode = item.ItemCode;
                newRow.ItemName = item.ItemName;
                newRow.BatchID = item.BatchID;
                newRow.UnitPrice = item.ItemCost ?? 0;
                newRow.NetAmount = (item.ItemCost ?? 0 * item.QuantityAdjusted);
                newRow.ItemCost = item.ItemCost ?? 0;
                newRow.Quantity = item.QuantityAdjusted;
                newRow.IMUOMUID = item.AdjustedUOM;
                newRow.ExpiryDttm = item.ExpiryDate;
                newRow.Location = item.Location;
                newRow.LocationUID = item.LocationUID;
                listIssued.Add(newRow);
            }

            DataService.Inventory.ConsumptionItem(listIssued, Comments, AppUtil.Current.UserID);
            SaveSuccessDialog();
            ListItemIssue view = new ListItemIssue();
            ChangeView_CloseViewDialog(view, ActionDialog.Save);
        }

        private void Add()
        {
            if (SelectCurrentStock == null)
            {
                WarningDialog("กรุณาเลือก Batch");
                return;
            }

            if (SelectLocation == null)
            {
                WarningDialog("กรุณาเลือกสถานที่ใช้");
                return;
            }


            if (IssueStocks != null)
            {
                if ((IssueStocks.FirstOrDefault(p => p.StockUID == SelectCurrentStock.StockUID) != null)
                    && (IssueStocks.FirstOrDefault(p => p.Location == SelectLocation.Name) != null))
                {
                    WarningDialog("มีรายการที่เลือกแล้ว");
                    return;
                }
            }

            if ((SelectCurrentStock.Quantity - Quantity) < 0)
            {
                WarningDialog("คีย์จำนวนที่ใช้เกิน จำนวนในคลัง โปรดตรวจสอบ");
                return;
            }

            if (IssueStocks == null)
            {
                IssueStocks = new ObservableCollection<StockAdjustmentModel>();
            }

            StockAdjustmentModel adjustStock = new StockAdjustmentModel();
            adjustStock.ItemMasterUID = SelectCurrentStock.ItemMasterUID;
            adjustStock.ItemCode = SelectCurrentStock.ItemCode;
            adjustStock.ItemName = SelectCurrentStock.ItemName;
            adjustStock.StockUID = SelectCurrentStock.StockUID;
            adjustStock.OwnerOrganisationUID = SelectCurrentStock.OrganisationUID;
            adjustStock.StoreUID = SelectCurrentStock.StoreUID;
            adjustStock.StoreName = SelectCurrentStock.StoreName;
            adjustStock.ActualQuantity = SelectCurrentStock.Quantity;
            adjustStock.QuantityAdjusted = Quantity;
            adjustStock.ItemCost = SelectCurrentStock.ItemCost;
            adjustStock.AdjustedQuantity = adjustStock.ActualQuantity - adjustStock.QuantityAdjusted;
            adjustStock.ActualUOM = SelectCurrentStock.IMUOMUID;
            adjustStock.AdjustedUOM = SelectCurrentStock.IMUOMUID;
            adjustStock.AdjustedUnit = SelectCurrentStock.Unit;
            adjustStock.ExpiryDate = ExpiryDate;
            adjustStock.BatchID = SelectCurrentStock.BatchID;
            adjustStock.Location = SelectLocation.Name;
            adjustStock.LocationUID = SelectLocation.LocationUID;
            IssueStocks.Add(adjustStock);

            ClearInput();
        }

        private void Delete()
        {
            if (SelectIssueStock != null)
            {
                IssueStocks.Remove(SelectIssueStock);
                ClearInput();
            }

        }

        private void Clear()
        {
            SelectOrganisation = null;
            SelectStore = null;
            SelectItemType = null;
            ItemName = string.Empty;
            ItemCode = string.Empty;
        }

        private void ClearInput()
        {
            SelectCurrentStock = null;
            Quantity = 0;
            ExpiryDate = null;
            BatchID = string.Empty;
            BatchQty = null;
        }

        private void Cancel()
        {
            ListItemIssue view = new ListItemIssue();
            ChangeView_CloseViewDialog(view, ActionDialog.Cancel);
        }

        #endregion

    }
}
