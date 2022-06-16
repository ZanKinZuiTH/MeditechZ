using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
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
    public class AdjustStockViewModel : MediTechViewModelBase
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
            set { 
                Set(ref _SelectOrganisation, value);
                if (_SelectOrganisation != null)
                {
                    Locations = GetLocatioinRole(_SelectOrganisation.HealthOrganisationUID);
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

        private double? _ItemCost;

        public double? ItemCost
        {
            get { return _ItemCost; }
            set { Set(ref _ItemCost, value); }
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
                    ItemCost = SelectCurrentStock.ItemCost;
                }
            }
        }

        private ObservableCollection<StockAdjustmentModel> _AdjustStock;

        public ObservableCollection<StockAdjustmentModel> AdjustStock
        {
            get { return _AdjustStock; }
            set { Set(ref _AdjustStock, value); }
        }

        private StockAdjustmentModel _SelectAdjustStock;

        public StockAdjustmentModel SelectAdjustStock
        {
            get { return _SelectAdjustStock; }
            set { Set(ref _SelectAdjustStock, value); }
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

        public AdjustStockViewModel()
        {
            Organisations = GetHealthOrganisationIsRoleStock();
            ItemTypes = DataService.Technical.GetReferenceValueMany("ITMTYP");


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
            CurrentStock = DataService.Inventory.SearchStockBatch(SelectOrganisation.HealthOrganisationUID,SelectLocation.LocationUID, SelectStore.StoreUID, itemType,ItemCode,ItemName);
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
            Comments = string.Empty;
            ItemCost = null;
        }

        private void Add()
        {
            if (SelectCurrentStock == null)
            {
                WarningDialog("กรุณาเลือก Batch");
                return;
            }
            if (string.IsNullOrEmpty(Comments))
            {
                WarningDialog("กรุณาระบุเหตุผล");
                return;
            }

            if (AdjustStock != null)
            {
                if (AdjustStock.FirstOrDefault(p => p.StockUID == SelectCurrentStock.StockUID) != null)
                {
                    WarningDialog("มีรายการที่เลือกแล้ว");
                    return;
                }
            }

            if (AdjustStock == null)
            {
                AdjustStock = new ObservableCollection<StockAdjustmentModel>();
            }

            StockAdjustmentModel adjustStock = new StockAdjustmentModel();
            adjustStock.ItemMasterUID = SelectCurrentStock.ItemMasterUID;
            adjustStock.ItemCode = SelectCurrentStock.ItemCode;
            adjustStock.ItemName = SelectCurrentStock.ItemName;
            adjustStock.OwnerOrganisationUID = SelectCurrentStock.OrganisationUID;
            adjustStock.StoreUID = SelectCurrentStock.StoreUID;
            adjustStock.StockUID = SelectCurrentStock.StockUID;
            adjustStock.ItemCost = SelectCurrentStock.ItemCost;
            adjustStock.OwnerOrganisationUID = SelectCurrentStock.OrganisationUID;
            adjustStock.StoreUID = SelectCurrentStock.StoreUID;
            adjustStock.StoreName = SelectCurrentStock.StoreName;
            adjustStock.ActualQuantity = SelectCurrentStock.Quantity;
            adjustStock.QuantityAdjusted = Quantity;
            adjustStock.AdjustedQuantity = adjustStock.ActualQuantity + adjustStock.QuantityAdjusted;
            adjustStock.ActualUOM = SelectCurrentStock.IMUOMUID;
            adjustStock.AdjustedUOM = SelectCurrentStock.IMUOMUID;
            adjustStock.AdjustedUnit = SelectCurrentStock.Unit;
            adjustStock.ExpiryDate = ExpiryDate;
            adjustStock.BatchID = SelectCurrentStock.BatchID;
            adjustStock.ItemCost = ItemCost;
            adjustStock.NewBatchID = BatchID;
            adjustStock.Comments = Comments;
            AdjustStock.Add(adjustStock);

            ClearInput();
        }

        private void Delete()
        {
            if (SelectAdjustStock != null)
            {
                AdjustStock.Remove(SelectAdjustStock);
                ClearInput();
            }

        }

        private void Save()
        {
            try
            {
                if (AdjustStock == null)
                {
                    WarningDialog("กรุณาทำรายการอย่างน้อย 1 รายการ");
                    return;
                }

                foreach (var item in AdjustStock)
                {
                    DataService.Inventory.AdjustStock(item, AppUtil.Current.UserID);
                }

                SaveSuccessDialog();
                ClearInput();
                AdjustStock = null;
                Search();
            }
            catch (Exception er)
            {
                ErrorDialog(er.Message);
            }

        }

        private void Cancel()
        {
            ChangeViewPermission(null);
        }


        #endregion
    }
}
