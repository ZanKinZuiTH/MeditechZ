using GalaSoft.MvvmLight;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Models
{
    public class ItemMasterList : ObservableObject
    {
        public string Comments { get; set; }
        public int CUser { get; set; }
        public DateTime CWhen { get; set; }

        private int _IMUOMUID;

        public int IMUOMUID
        {
            get { return _IMUOMUID; }
            set { Set(ref _IMUOMUID, value); }
        }

        private string _ItemCode;

        public string ItemCode
        {
            get { return _ItemCode; }
            set { Set(ref _ItemCode, value); }
        }


        private int _ItemMasterUID;

        public int ItemMasterUID
        {
            get { return _ItemMasterUID; }
            set
            {
                Set(ref _ItemMasterUID, value);
                if (_ItemMasterUID != 0)
                {
                    //MediTech.DataService.InventoryService inventoryData = new DataService.InventoryService();
                    //ItemMasterModel itemModel = inventoryData.GetItemMasterByUID(_ItemMasterUID);
                    //ItemCode = SelectItemMaster.Code;
                    //ItemName = SelectItemMaster.Name;

                    //StoreUOMConverts = inventoryData.GetStoreConvertUOM(_ItemMasterUID);
                    //if (StoreUOMConverts != null)
                    //{
                    //    IMUOMUID = StoreUOMConverts.FirstOrDefault().ConversionUOMUID;
                    //}
                }
            }
        }

        public string ItemName { get; set; }

        private string _BatchID;

        public string BatchID
        {
            get { return _BatchID; }
            set { Set(ref _BatchID, value); }
        }

        private int _StockUID;

        public int StockUID
        {
            get { return _StockUID; }
            set
            {
                Set(ref _StockUID, value);
            }
        }

        private DateTime? _ExpiryDttm;

        public DateTime? ExpiryDttm
        {
            get { return _ExpiryDttm; }
            set { Set(ref _ExpiryDttm, value); }
        }

        public int MUser { get; set; }
        public DateTime MWhen { get; set; }

        private double _NetAmount;

        public double NetAmount
        {
            get { return _NetAmount; }
            set { Set(ref _NetAmount, value); }
        }

        private double _NetCost;

        public double NetCost
        {
            get { return _NetCost; }
            set
            {
                Set(ref _NetCost, value);
            }
        }

        public int ItemListUID { get; set; }
        public int? PurchaseOrderUID { get; set; }
        public int? ManufacturerUID { get; set; }

        private double? _ItemCost;

        public double? ItemCost
        {
            get { return _ItemCost; }
            set
            {
                Set(ref _ItemCost, value);
            }
        }

        private double _StockQuantity;

        public double StockQuantity
        {
            get { return _StockQuantity; }
            set { Set(ref _StockQuantity, value); }
        }


        private double _BatchQuantity;

        public double BatchQuantity
        {
            get { return _BatchQuantity; }
            set { Set(ref _BatchQuantity, value); }
        }

        private double? _ShowBatchQuantity;

        public double? ShowBatchQuantity
        {
            get { return _ShowBatchQuantity; }
            set { Set(ref _ShowBatchQuantity, value); }
        }

        private double? _Quantity;

        public double? Quantity
        {
            get { return _Quantity; }
            set
            {
                Set(ref _Quantity, value);
                NetAmount = CalculateNetAmount();
                NetCost = CalculateNetCost();
            }
        }

        private double? _FreeQuantity;

        public double? FreeQuantity
        {
            get { return _FreeQuantity; }
            set { Set(ref _FreeQuantity, value); }
        }

        private double? _Discount;

        public double? Discount
        {
            get { return _Discount; }
            set
            {
                Set(ref _Discount, value);
                NetAmount = CalculateNetAmount();
            }
        }

        private double? _TaxPercentage;

        public double? TaxPercentage
        {
            get { return _TaxPercentage; }
            set
            {
                _TaxPercentage = value;
                NetAmount = CalculateNetAmount();
            }
        }

        public string StatusFlag { get; set; }


        private double? _UnitPrice;

        public double? UnitPrice
        {
            get { return _UnitPrice; }
            set
            {
                Set(ref _UnitPrice, value);
                NetAmount = CalculateNetAmount();
            }
        }

        public string Unit { get; set; }

        private ItemMasterModel _SelectItemMaster;

        public ItemMasterModel SelectItemMaster
        {
            get { return _SelectItemMaster; }
            set
            {
                Set(ref _SelectItemMaster, value);
                if (SelectItemMaster != null)
                {
                    ItemMasterUID = SelectItemMaster.ItemMasterUID;
                    ItemCode = SelectItemMaster.Code;
                    ItemName = SelectItemMaster.Name;

                    if (SelectItemMaster.UnitPrice != null)
                    {
                        UnitPrice = SelectItemMaster.UnitPrice;
                    }

                    if (string.IsNullOrEmpty(BatchID))
                        BatchID = SelectItemMaster.BatchID;

                    if (ExpiryDttm == null)
                        ExpiryDttm = SelectItemMaster.ExpiryDttm;


                    BatchQuantity = SelectItemMaster.BatchQty;

                    ShowBatchQuantity = SelectItemMaster.BatchQty - (Quantity ?? 0);


                    if (StockUID == 0)
                        StockUID = SelectItemMaster.StockUID;

                    StockQuantity = SelectItemMaster.StockQty;
                    if (ItemCost == null)
                        ItemCost = SelectItemMaster.ItemCost;

                    MediTech.DataService.InventoryService inventoryData = new DataService.InventoryService();
                    //ItemMasterModel itemModel = inventoryData.GetItemMasterByUID(_ItemMasterUID);
                    //ItemCode = SelectItemMaster.Code;
                    //ItemName = SelectItemMaster.Name;

                    StoreUOMConverts = inventoryData.GetStoreConvertUOM(_ItemMasterUID);
                    if (StoreUOMConverts != null)
                    {
                        IMUOMUID = StoreUOMConverts.FirstOrDefault().ConversionUOMUID;
                    }

                }
            }
        }



        private List<StoreUOMConversionModel> _StoreUOMConverts;

        public List<StoreUOMConversionModel> StoreUOMConverts
        {
            get
            {
                return _StoreUOMConverts;
            }
            set
            {
                Set(ref _StoreUOMConverts, value);
            }
        }

        double CalculateNetAmount()
        {
            double NetAmount = 0;
            //NetAmount = (((Quantity ?? 0) * (UnitPrice ?? 0)) + ((UnitPrice ?? 0) * ((TaxPercentage ?? 0) / 100))) - (Discount ?? 0);
            NetAmount = ((Quantity ?? 0) * (UnitPrice ?? 0)) - (Discount ?? 0);
            return NetAmount;
        }

        double CalculateNetCost()
        {
            double NetCost = 0;
            NetCost = (((Quantity ?? 0) * (ItemCost ?? 0)));
            return NetCost;
        }

        public string SerialNumber { get; set; }
    }
}
