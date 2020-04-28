using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MediTech.Model;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using MediTech.DataService;
using MediTech.Views;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class OrderMedicalItemViewModel : MediTechViewModelBase
    {
        #region Properites

        public int? OwnerOrgansitaion { get; set; }
        public BillableItemModel BillableItem { get; set; }
        public ItemMasterModel ItemMaster { get; set; }
        public PatientOrderDetailModel PatientOrderDetail { get; set; }

        private string _TypeOrder;

        public string TypeOrder
        {
            get { return _TypeOrder; }
            set { Set(ref _TypeOrder, value); }
        }


        private string _OrderName;

        public string OrderName
        {
            get { return _OrderName; }
            set { Set(ref _OrderName, value); }
        }

        private string _OrderCode;

        public string OrderCode
        {
            get { return _OrderCode; }
            set { Set(ref _OrderCode, value); }
        }
        private string _UnitPrice;

        public string UnitPrice
        {
            get { return _UnitPrice; }
            set { Set(ref _UnitPrice, value); }
        }


        private DateTime _StartDate;

        public DateTime StartDate
        {
            get { return _StartDate; }
            set { Set(ref _StartDate, value); }
        }

        private DateTime _StartTime;
        public DateTime StartTime
        {
            get { return _StartTime; }
            set { Set(ref _StartTime, value); }
        }

        private List<StockModel> _Stores;

        public List<StockModel> Stores
        {
            get { return _Stores; }
            set { Set(ref _Stores, value); }
        }

        private StockModel _SelectStore;

        public StockModel SelectStore
        {
            get { return _SelectStore; }
            set
            {
                Set(ref _SelectStore, value);
                if (_SelectStore != null)
                {
                    StockQuantity = _SelectStore.Quantity;
                }
            }
        }

        private double? _StockQuantity;

        public double? StockQuantity
        {
            get { return _StockQuantity; }
            set { Set(ref _StockQuantity, value); }
        }
        private List<ItemUOMConversionModel> _Units;

        public List<ItemUOMConversionModel> Units
        {
            get { return _Units; }
            set { Set(ref _Units, value); }
        }

        private ItemUOMConversionModel _SelectUnit;

        public ItemUOMConversionModel SelectUnit
        {
            get { return _SelectUnit; }
            set
            {
                var oldVale = _SelectUnit;
                Set(ref _SelectUnit, value);
                if (oldVale != null)
                {
                    Quantity = (Quantity / oldVale.ConversionValue) * SelectUnit.ConversionValue;
                }
                else
                {
                    Quantity = Quantity * SelectUnit.ConversionValue;
                }
            }
        }


        private double _Quantity;

        public double Quantity
        {
            get { return _Quantity; }
            set { Set(ref _Quantity, value); }
        }

        private double? _OverwritePrice;

        public double? OverwritePrice
        {
            get { return _OverwritePrice; }
            set { Set(ref _OverwritePrice, value); }
        }

        private string _LabelSticker;

        public string LabelSticker
        {
            get { return _LabelSticker; }
            set { Set(ref _LabelSticker, value); }
        }

        private string _NoteToPharmacy;

        public string NoteToPharmacy
        {
            get { return _NoteToPharmacy; }
            set { Set(ref _NoteToPharmacy, value); }
        }

        private string _PatientInstruction;

        public string PatientInstruction
        {
            get { return _PatientInstruction; }
            set { Set(ref _PatientInstruction, value); }
        }


        private string _OrderInstruction;

        public string OrderInstruction
        {
            get { return _OrderInstruction; }
            set { Set(ref _OrderInstruction, value); }
        }

        private string _Comment;

        public string Comment
        {
            get { return _Comment; }
            set { Set(ref _Comment, value); }
        }

        #endregion

        #region Command




        private RelayCommand _AddCommand;

        public RelayCommand AddCommand
        {
            get { return _AddCommand ?? (_AddCommand = new RelayCommand(Add)); }
        }


        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }


        #endregion

        #region Method

        private void BindingData()
        {
            Units = DataService.Inventory.GetItemConvertUOM(ItemMaster.ItemMasterUID);
        }

        public void BindingFromBillableItem()
        {
            DateTime now = DateTime.Now;
            ItemMaster = DataService.Inventory.GetItemMasterByUID(BillableItem.ItemUID.Value);
            int? ownerOrganisatinUID = (OwnerOrgansitaion != null && OwnerOrgansitaion != 0) ? OwnerOrgansitaion : AppUtil.Current.OwnerOrganisationUID;
            Stores = DataService.Inventory.GetStockRemainByItemMasterUID(ItemMaster.ItemMasterUID, ownerOrganisatinUID ?? 0);
            BindingData();

            TypeOrder = BillableItem.BillingServiceMetaData;
            OrderName = BillableItem.ItemName;
            OrderCode = "Code : " + BillableItem.Code;
            UnitPrice = BillableItem.Price.ToString("#,#.00");
            SelectStore = Stores != null ? Stores.FirstOrDefault() : null;
            SelectUnit = Units.FirstOrDefault(p => p.ConversionUOMUID == ItemMaster.BaseUOM);
            Quantity = ItemMaster.MinSalesQty ?? 1;

            if (!string.IsNullOrEmpty(ItemMaster.DispenseEnglish) && !string.IsNullOrEmpty(ItemMaster.DispenseLocal))
            {
                PatientInstruction = ItemMaster.DispenseEnglish + "/" + ItemMaster.DispenseLocal;
            }
            else if (!string.IsNullOrEmpty(ItemMaster.DispenseLocal))
            {
                PatientInstruction = ItemMaster.DispenseLocal;
            }
            else if (!string.IsNullOrEmpty(ItemMaster.DispenseEnglish))
            {
                PatientInstruction = ItemMaster.DispenseEnglish;
            }
            
            OrderInstruction = ItemMaster.OrderInstruction;
            Comment = ItemMaster.Comments;
            StartDate = now.Date;
            StartTime = now;
        }
        public void BindingFromPatientOrderDetail()
        {
            ItemMaster = DataService.Inventory.GetItemMasterByUID(PatientOrderDetail.ItemUID.Value);
            Stores = DataService.Inventory.GetStockRemainByItemMasterUID(ItemMaster.ItemMasterUID, PatientOrderDetail.OwnerOrganisationUID);
            BindingData();

            TypeOrder = PatientOrderDetail.BillingService;
            OrderName = PatientOrderDetail.ItemName;
            OrderCode = "Code : " + PatientOrderDetail.ItemCode;
            UnitPrice = PatientOrderDetail.UnitPrice.Value.ToString("#,#.00");
            StartDate = PatientOrderDetail.StartDttm.Value.Date;
            StartTime = PatientOrderDetail.StartDttm.Value;
            OverwritePrice = PatientOrderDetail.OverwritePrice;

            SelectStore = Stores.FirstOrDefault(p => p.StoreUID == PatientOrderDetail.StoreUID);
            SelectUnit = Units.FirstOrDefault(p => p.ConversionUOMUID == PatientOrderDetail.QNUOMUID);
            Quantity = PatientOrderDetail.Quantity ?? 1;

            if (!string.IsNullOrEmpty(ItemMaster.DispenseEnglish) && !string.IsNullOrEmpty(ItemMaster.DispenseLocal))
            {
                PatientInstruction = ItemMaster.DispenseEnglish + "/" + ItemMaster.DispenseLocal;
            }
            else if (!string.IsNullOrEmpty(ItemMaster.DispenseLocal))
            {
                PatientInstruction = ItemMaster.DispenseLocal;
            }
            else if (!string.IsNullOrEmpty(ItemMaster.DispenseEnglish))
            {
                PatientInstruction = ItemMaster.DispenseEnglish;
            }

            OrderInstruction = ItemMaster.OrderInstruction;
            Comment = ItemMaster.Comments;
            LabelSticker = PatientOrderDetail.LocalInstructionText;
            NoteToPharmacy = PatientOrderDetail.ClinicalComments;
        }

        private void Add()
        {
            try
            {
                if (SelectStore == null)
                {
                    WarningDialog("ไม่มีของในคลัง โปรดตรวจสอบ");
                    return;
                }
                else
                {
                    if (Quantity > SelectStore.Quantity)
                    {
                        if (ItemMaster.CanDispenseWithOutStock != "Y")
                        {
                            WarningDialog("มีของในคลังไม่พอสำหรับจ่ายยา โปรดตรวจสอบ");
                            return;

                        }
                        else if (ItemMaster.CanDispenseWithOutStock == "Y")
                        {
                            DialogResult result = QuestionDialog("มีของในคลังไม่พอ คุณต้องการดำเนินการต่อหรือไม่ ?");
                            if (result == DialogResult.No || result == DialogResult.Cancel)
                            {
                                return;
                            }
                        }
                    }
                }

                if (Quantity <= 0)
                {
                    WarningDialog("ไม่อนุญาติให้คีย์ จำนวน < 0");
                    return;
                }

                if ((Quantity % SelectUnit.ConversionValue) > 0)
                {
                    WarningDialog("คุณคีย์จำนวนไม่ถูกต้อง โปรดตรวจสอบ");
                    return;
                }

                if (ItemMaster.MinSalesQty != null && Quantity < ItemMaster.MinSalesQty)
                {
                    WarningDialog("คีย์จำนวนที่ใช้น้อยกว่าจำนวนขั้นต่ำที่คีย์ได้ โปรดตรวจสอบ");
                    return;
                }

                if (PatientOrderDetail == null)
                {
                    PatientOrderDetail = new PatientOrderDetailModel();
                    PatientOrderDetail.BillableItemUID = BillableItem.BillableItemUID;
                    PatientOrderDetail.BSMDDUID = BillableItem.BSMDDUID;
                    PatientOrderDetail.ItemUID = BillableItem.ItemUID;
                    PatientOrderDetail.ItemCode = BillableItem.Code;
                    PatientOrderDetail.ItemName = BillableItem.ItemName;
                    PatientOrderDetail.BillingService = BillableItem.BillingServiceMetaData;                   
                    PatientOrderDetail.UnitPrice = BillableItem.Price;
                    PatientOrderDetail.DoctorFee = (BillableItem.DoctorFee / 100) * BillableItem.Price;

                }

                PatientOrderDetail.Comments = Comment;
                PatientOrderDetail.StartDttm = StartDate.Add(StartTime.TimeOfDay);


                PatientOrderDetail.Quantity = Quantity;
                if (SelectUnit != null)
                {
                    PatientOrderDetail.QNUOMUID = SelectUnit.ConversionUOMUID;
                    PatientOrderDetail.QuantityUnit = SelectUnit.ConversionUnit;
                }




                PatientOrderDetail.LocalInstructionText = LabelSticker;
                PatientOrderDetail.ClinicalComments = NoteToPharmacy;
                PatientOrderDetail.IsStock = ItemMaster.IsStock;
                PatientOrderDetail.StoreUID = SelectStore.StoreUID;

                if (OverwritePrice != null)
                {
                    PatientOrderDetail.OverwritePrice = OverwritePrice;
                    PatientOrderDetail.IsPriceOverwrite = "Y";
                    PatientOrderDetail.DisplayPrice = PatientOrderDetail.OverwritePrice;

                    if (SelectUnit.ConversionUOMUID == ItemMaster.BaseUOM)
                    {
                        //PatientOrderDetail.NetAmount = (OverwritePrice * PatientOrderDetail.Quantity) + (PatientOrderDetail.DoctorFee ?? 0);
                        PatientOrderDetail.NetAmount = (OverwritePrice * PatientOrderDetail.Quantity);
                    }
                    else
                    {
                        var Qty = (Quantity / SelectUnit.ConversionValue);
                        //PatientOrderDetail.NetAmount = (OverwritePrice * Qty) + (PatientOrderDetail.DoctorFee ?? 0);
                        PatientOrderDetail.NetAmount = (OverwritePrice * Qty);
                    }

                }
                else
                {
                    PatientOrderDetail.OverwritePrice = OverwritePrice;
                    PatientOrderDetail.IsPriceOverwrite = "N";
                    PatientOrderDetail.DisplayPrice = PatientOrderDetail.UnitPrice;

                    if (SelectUnit.ConversionUOMUID == ItemMaster.BaseUOM)
                    {
                        //PatientOrderDetail.NetAmount = ((PatientOrderDetail.UnitPrice ?? 0) * PatientOrderDetail.Quantity) + (PatientOrderDetail.DoctorFee ?? 0);
                        PatientOrderDetail.NetAmount = ((PatientOrderDetail.UnitPrice ?? 0) * PatientOrderDetail.Quantity);
                    }
                    else
                    {
                        var Qty = (Quantity / SelectUnit.ConversionValue);
                        //PatientOrderDetail.NetAmount = ((PatientOrderDetail.UnitPrice ?? 0) * Qty) + (PatientOrderDetail.DoctorFee ?? 0);
                        PatientOrderDetail.NetAmount = ((PatientOrderDetail.UnitPrice ?? 0) * Qty);
                    }

                }

                if (PatientOrderDetail.OwnerOrganisationUID == 0)
                {
                    PatientOrderDetail.OwnerOrganisationUID = (OwnerOrgansitaion != null && OwnerOrgansitaion != 0) ? OwnerOrgansitaion.Value : AppUtil.Current.OwnerOrganisationUID;
                }

                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }


        }

        public void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }
        #endregion

    }
}
