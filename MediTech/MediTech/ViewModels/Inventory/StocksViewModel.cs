using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class StocksViewModel : MediTechViewModelBase
    {
        #region Properties

        private int _SelectTabIndex;

        public int SelectTabIndex
        {
            get { return _SelectTabIndex; }
            set
            {
                Set(ref _SelectTabIndex, value);
                if (SelectTabIndex == 1)
                {
                    if (SelectOrganisationOnHand != null)
                    {
                        SelectOrganisationMovement = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == SelectOrganisationOnHand.HealthOrganisationUID);
                    }
                    if (SelectStoreOnHand != null)
                    {
                        SelectStoreMovement = StoresMovement.FirstOrDefault(p => p.StoreUID ==  SelectStoreOnHand.StoreUID);
                    }
                 
                    if (SelectStockOnHand != null)
                    {
                        ItemCodeMovement = SelectStockOnHand.ItemCode;
                        ItemNameMovement = SelectStockOnHand.ItemName;
                        SearchMovement();
                    }
                    else
                    {
                        ItemCodeMovement = string.Empty;
                        ItemNameMovement = string.Empty;
                        StockMovements = null;
                    }
                }
                else if (SelectTabIndex == 2)
                {
                    if (SelectOrganisationOnHand != null)
                    {
                        SelectOrganisationBalance = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == SelectOrganisationOnHand.HealthOrganisationUID);
                    }
                    if (SelectStoreOnHand != null)
                    {
                        SelectStoreBalance = StoresBalance.FirstOrDefault(p => p.StoreUID == SelectStoreOnHand.StoreUID);
                    }
                    if (SelectStockOnHand != null)
                    {
                        ItemCodeBalance = SelectStockOnHand.ItemCode;
                        ItemNameBalance = SelectStockOnHand.ItemName;
                        SearchBalance();
                    }
                    else
                    {
                        ItemCodeBalance = string.Empty;
                        ItemNameBalance = string.Empty;
                        StockBalances = null;
                    }
                }
            }
        }


        private List<HealthOrganisationModel> _Organisations;

        public List<HealthOrganisationModel> Organisations
        {
            get { return _Organisations; }
            set { Set(ref _Organisations, value); }
        }



        #region StockOnHand

        private HealthOrganisationModel _SelectOrganisationOnHand;

        public HealthOrganisationModel SelectOrganisationOnHand
        {
            get { return _SelectOrganisationOnHand; }
            set
            {
                Set(ref _SelectOrganisationOnHand, value);
                if (_SelectOrganisationOnHand != null)
                {
                    LocationOnHands = DataService.MasterData.GetLocationByOrganisationUID(SelectOrganisationOnHand.HealthOrganisationUID);
                   
                }
            }
        }

        private List<LocationModel> _LocationOnHands;

        public List<LocationModel> LocationOnHands
        {
            get { return _LocationOnHands; }
            set { Set(ref _LocationOnHands, value); }
        }

        private LocationModel _SelectLocationOnHand;

        public LocationModel SelectLocationOnHand
        {
            get { return _SelectLocationOnHand; }
            set { Set(ref _SelectLocationOnHand, value);
                if (_SelectLocationOnHand != null)
                {
                    StoresOnHand = DataService.Inventory.GetStoreByLocationUID(_SelectLocationOnHand.LocationUID);
                }
            }
        }


        private List<StoreModel> _StoresOnHand;

        public List<StoreModel> StoresOnHand
        {
            get { return _StoresOnHand; }
            set { Set(ref _StoresOnHand, value); }
        }

        private StoreModel _SelectStoreOnHand;

        public StoreModel SelectStoreOnHand
        {
            get { return _SelectStoreOnHand; }
            set
            {
                Set(ref _SelectStoreOnHand, value);
            }
        }

        private List<LookupReferenceValueModel> _ItemTypes;

        public List<LookupReferenceValueModel> ItemTypes
        {
            get { return _ItemTypes; }
            set { Set(ref _ItemTypes, value); }
        }
        private LookupReferenceValueModel _SelectItemTypeOnHand;

        public LookupReferenceValueModel SelectItemTypeOnHand
        {
            get { return _SelectItemTypeOnHand; }
            set { Set(ref _SelectItemTypeOnHand, value); }
        }

        private string _ItemCodeOnHand;

        public string ItemCodeOnHand
        {
            get { return _ItemCodeOnHand; }
            set { Set(ref _ItemCodeOnHand, value); }
        }

        private string _ItemNameOnHand;

        public string ItemNameOnHand
        {
            get { return _ItemNameOnHand; }
            set { Set(ref _ItemNameOnHand, value); }
        }

        private List<StockOnHandModel> _StockOnHand;

        public List<StockOnHandModel> StockOnHand
        {
            get { return _StockOnHand; }
            set { Set(ref _StockOnHand, value); }
        }

        private StockOnHandModel _SelectStockOnHand;

        public StockOnHandModel SelectStockOnHand
        {
            get { return _SelectStockOnHand; }
            set
            {
                Set(ref _SelectStockOnHand, value);
                if (_SelectStockOnHand != null)
                {
                    StockOnHandBatch = DataService.Inventory.SearchStockBatchByStoreUID(SelectStockOnHand.StoreUID, SelectStockOnHand.ItemMasterUID);
                }
            }
        }

        private List<StockOnHandModel> _StockOnHandBatch;

        public List<StockOnHandModel> StockOnHandBatch
        {
            get { return _StockOnHandBatch; }
            set { Set(ref _StockOnHandBatch, value); }
        }

        private StockOnHandModel _SelectStockOnHandBatch;

        public StockOnHandModel SelectStockOnHandBatch
        {
            get { return _SelectStockOnHandBatch; }
            set { Set(ref _SelectStockOnHandBatch, value); }
        }


        #endregion

        #region Movement

        private HealthOrganisationModel _SelectOrganisationMovement;

        public HealthOrganisationModel SelectOrganisationMovement
        {
            get { return _SelectOrganisationMovement; }
            set
            {
                Set(ref _SelectOrganisationMovement, value);
                if (_SelectOrganisationMovement != null)
                {
                    LocationMovements = DataService.MasterData.GetLocationByOrganisationUID(SelectOrganisationMovement != null ? SelectOrganisationMovement.HealthOrganisationUID : 0);
                }
            }
        }

        private List<LocationModel> _LocationMovements;

        public List<LocationModel> LocationMovements
        {
            get { return _LocationMovements; }
            set { Set(ref _LocationMovements, value); }
        }

        private LocationModel _SelectLocationMovement;

        public LocationModel SelectLocationMovement
        {
            get { return _SelectLocationMovement; }
            set
            {
                Set(ref _SelectLocationMovement, value);
                if (_SelectLocationMovement != null)
                {
                    StoresMovement = DataService.Inventory.GetStoreByLocationUID(_SelectLocationMovement.LocationUID);
                }
            }
        }

        private List<StoreModel> _StoresMovement;

        public List<StoreModel> StoresMovement
        {
            get { return _StoresMovement; }
            set { Set(ref _StoresMovement, value); }
        }

        private StoreModel _SelectStoreMovement;

        public StoreModel SelectStoreMovement
        {
            get { return _SelectStoreMovement; }
            set { Set(ref _SelectStoreMovement, value); }
        }


        private LookupReferenceValueModel _SelectItemTypeMovement;

        public LookupReferenceValueModel SelectItemTypeMovement
        {
            get { return _SelectItemTypeMovement; }
            set { Set(ref _SelectItemTypeMovement, value); }
        }


        private string _ItemCodeMovement;

        public string ItemCodeMovement
        {
            get { return _ItemCodeMovement; }
            set { Set(ref _ItemCodeMovement, value); }
        }


        private string _ItemNameMovement;

        public string ItemNameMovement
        {
            get { return _ItemNameMovement; }
            set { Set(ref _ItemNameMovement, value); }
        }

        private DateTime? _DateFromMovement;

        public DateTime? DateFromMovement
        {
            get { return _DateFromMovement; }
            set { Set(ref _DateFromMovement, value); }
        }
        private DateTime? _DateToMovement;

        public DateTime? DateToMovement
        {
            get { return _DateToMovement; }
            set { Set(ref _DateToMovement, value); }
        }

        private List<StockMovementModel> _StockMovements;

        public List<StockMovementModel> StockMovements
        {
            get { return _StockMovements; }
            set { Set(ref _StockMovements, value); }
        }

        private List<LookupReferenceValueModel> _TransactionTypes;

        public List<LookupReferenceValueModel> TransactionTypes
        {
            get { return _TransactionTypes; }
            set { Set(ref _TransactionTypes, value); }
        }


        private LookupReferenceValueModel _SelectTransactionType;

        public LookupReferenceValueModel SelectTransactionType
        {
            get { return _SelectTransactionType; }
            set { Set(ref _SelectTransactionType, value); }
        }
        #endregion

        #region Balance

        private HealthOrganisationModel _SelectOrganisationBalance;

        public HealthOrganisationModel SelectOrganisationBalance
        {
            get { return _SelectOrganisationBalance; }
            set
            {
                Set(ref _SelectOrganisationBalance, value);
                if (_SelectOrganisationOnHand != null)
                {
                    LocationBalances = DataService.MasterData.GetLocationByOrganisationUID(SelectOrganisationBalance !=null ? SelectOrganisationBalance.HealthOrganisationUID : 0);
                }
            }
        }

        private List<LocationModel> _LocationBalances;

        public List<LocationModel> LocationBalances
        {
            get { return _LocationBalances; }
            set { Set(ref _LocationBalances, value); }
        }

        private LocationModel _SelectLocationBalance;

        public LocationModel SelectLocationBalance
        {
            get { return _SelectLocationBalance; }
            set
            {
                Set(ref _SelectLocationBalance, value);
                if (_SelectLocationBalance != null)
                {
                    StoresBalance = DataService.Inventory.GetStoreByLocationUID(_SelectLocationBalance.LocationUID);
                }
            }
        }

        private List<StoreModel> _StoresBalance;

        public List<StoreModel> StoresBalance
        {
            get { return _StoresBalance; }
            set { Set(ref _StoresBalance, value); }
        }

        private StoreModel _SelectStoreBalance;

        public StoreModel SelectStoreBalance
        {
            get { return _SelectStoreBalance; }
            set { Set(ref _SelectStoreBalance, value); }
        }


        private LookupReferenceValueModel _SelectItemTypeBalance;

        public LookupReferenceValueModel SelectItemTypeBalance
        {
            get { return _SelectItemTypeBalance; }
            set { Set(ref _SelectItemTypeBalance, value); }
        }

        private string _ItemCodeBalance;

        public string ItemCodeBalance
        {
            get { return _ItemCodeBalance; }
            set { Set(ref _ItemCodeBalance, value); }
        }


        private string _ItemNameBalance;

        public string ItemNameBalance
        {
            get { return _ItemNameBalance; }
            set { Set(ref _ItemNameBalance, value); }
        }

        private DateTime? _DateFromBalance;

        public DateTime? DateFromBalance
        {
            get { return _DateFromBalance; }
            set { Set(ref _DateFromBalance, value); }
        }

        private DateTime? _DateToBalance;

        public DateTime? DateToBalance
        {
            get { return _DateToBalance; }
            set { Set(ref _DateToBalance, value); }
        }


        private List<StockBalanceModel> _StockBalances;

        public List<StockBalanceModel> StockBalances
        {
            get { return _StockBalances; }
            set { Set(ref _StockBalances, value); }
        }

        #endregion

        #endregion

        #region Command

        #region StockOnHand

        private RelayCommand _SearchStockOnHandCommand;

        public RelayCommand SearchStockOnHandCommand
        {
            get
            {
                return _SearchStockOnHandCommand
                    ?? (_SearchStockOnHandCommand = new RelayCommand(SearchStockOnHand));
            }

        }

        private RelayCommand _ClearStockOnHandCommand;

        public RelayCommand ClearStockOnHandCommand
        {
            get
            {
                return _ClearStockOnHandCommand
                    ?? (_ClearStockOnHandCommand = new RelayCommand(ClearStockOnHand));
            }

        }

        #endregion

        #region Movement


        private RelayCommand _SearchMovementCommand;

        public RelayCommand SearchMovementCommand
        {
            get
            {
                return _SearchMovementCommand
                    ?? (_SearchMovementCommand = new RelayCommand(SearchMovement));
            }

        }

        private RelayCommand _ClearMovementCommand;

        public RelayCommand ClearMovementCommand
        {
            get
            {
                return _ClearMovementCommand
                    ?? (_ClearMovementCommand = new RelayCommand(ClearMovement));
            }

        }

        #endregion


        #region Balance


        private RelayCommand _SearchStockBalanceCommand;

        public RelayCommand SearchStockBalanceCommand
        {
            get
            {
                return _SearchStockBalanceCommand
                    ?? (_SearchStockBalanceCommand = new RelayCommand(SearchBalance));
            }

        }

        private RelayCommand _ClearStockBalanceCommand;

        public RelayCommand ClearStockBalanceCommand
        {
            get
            {
                return _ClearStockBalanceCommand
                    ?? (_ClearStockBalanceCommand = new RelayCommand(ClearBalance));
            }

        }

        #endregion

        #endregion

        #region Method

        public StocksViewModel()
        {
            Organisations = GetHealthOrganisationIsRoleStock();
            ItemTypes = DataService.Technical.GetReferenceValueMany("ITMTYP");

            TransactionTypes = new List<LookupReferenceValueModel>();
            TransactionTypes.Add(new LookupReferenceValueModel { Key = 0, Display = "ItemIssueDetail" });
            TransactionTypes.Add(new LookupReferenceValueModel { Key = 1, Display = "ItemReceiveDetail" });
            TransactionTypes.Add(new LookupReferenceValueModel { Key = 2, Display = "GRNItemList" });
            TransactionTypes.Add(new LookupReferenceValueModel { Key = 3, Display = "StockAdjustment" });
            TransactionTypes.Add(new LookupReferenceValueModel { Key = 4, Display = "DispensedItem" });
            DateFromMovement = DateTime.Now;
            DateFromBalance = DateTime.Now;

            SelectOrganisationOnHand = Organisations != null ? Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID) : null;
        }

        #region StockOnHand


        private void SearchStockOnHand()
        {
            int? ownerUID = SelectOrganisationOnHand != null ? SelectOrganisationOnHand.HealthOrganisationUID : (int?)null;
            int? locationUID = SelectLocationOnHand != null ? SelectLocationOnHand.LocationUID : (int?)null;
            int? storeUID = SelectStoreOnHand != null ? SelectStoreOnHand.StoreUID : (int?)null;
            int? itemType = SelectItemTypeOnHand != null ? SelectItemTypeOnHand.Key : (int?)null;
            StockOnHand = DataService.Inventory.SearchStockOnHand(ownerUID, locationUID, storeUID, itemType, ItemCodeOnHand, ItemNameOnHand);
            StockOnHandBatch = null;
        }

        private void ClearStockOnHand()
        {
            SelectItemTypeOnHand = null;
            SelectOrganisationOnHand = null;
            SelectStoreOnHand = null;
            ItemCodeOnHand = string.Empty;
            ItemNameOnHand = string.Empty;

        }

        #endregion

        #region Movemment

        private void SearchMovement()
        {
            int? ownerUID = SelectOrganisationMovement != null ? SelectOrganisationMovement.HealthOrganisationUID : (int?)null;
            int? locationUID = SelectLocationMovement != null ? SelectLocationMovement.LocationUID : (int?)null;
            int? storeUID = SelectStoreMovement != null ? SelectStoreMovement.StoreUID : (int?)null;
            int? itemType = SelectItemTypeMovement != null ? SelectItemTypeMovement.Key : (int?)null;
            string tranType = SelectTransactionType != null ? SelectTransactionType.Display : null;
            //int itemMasterUID = SelectStockOnHand != null ? SelectStockOnHand.ItemMasterUID : 0;

            StockMovements = DataService.Inventory.SearchStockMovement(ownerUID, locationUID, storeUID, ItemCodeMovement,ItemNameMovement, tranType, DateFromMovement, DateToMovement);
            if (StockMovements != null)
            {
                StockMovements = StockMovements.OrderBy(p => p.StockDttm).ToList();
            }
        }

        private void ClearMovement()
        {
            SelectOrganisationMovement = null;
            SelectStoreMovement = null;
            SelectTransactionType = null;
            SelectItemTypeMovement = null;
            ItemCodeMovement = string.Empty;
            ItemNameMovement = string.Empty;
            DateFromMovement = DateTime.Now;
            DateToMovement = null;
        }

        #endregion

        #region Balance

        private void SearchBalance()
        {
            int? ownerUID = SelectOrganisationBalance != null ? SelectOrganisationBalance.HealthOrganisationUID : (int?)null;
            int? locationUID = SelectLocationBalance != null ? SelectLocationBalance.LocationUID : (int?)null;
            int? storeUID = SelectStoreBalance != null ? SelectStoreBalance.StoreUID : (int?)null;
            int? itemType = SelectItemTypeBalance != null ? SelectItemTypeBalance.Key : (int?)null;
            //int itemMasterUID = SelectStockOnHand != null ? SelectStockOnHand.ItemMasterUID : 0;
            StockBalances = DataService.Inventory.SearchStockBalance(ownerUID, locationUID, storeUID, ItemCodeBalance,ItemNameBalance, DateFromBalance, DateToBalance);
        }

        private void ClearBalance()
        {
            SelectOrganisationBalance = null;
            SelectStoreBalance = null;
            SelectItemTypeBalance = null;
            ItemCodeBalance = string.Empty;
            ItemNameBalance = string.Empty;
            DateFromBalance = DateTime.Now;
            DateToBalance = null;
        }

        #endregion

        #endregion
    }
}
