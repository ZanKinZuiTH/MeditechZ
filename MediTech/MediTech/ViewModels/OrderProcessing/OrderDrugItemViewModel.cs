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
    public class OrderDrugItemViewModel : MediTechViewModelBase
    {

        #region Properites

        public bool SuppressQuantityEvent { get; set; }
        public bool SuppressDurationEvent { get; set; }
        public int OwnerOrgansitaionUID { get; set; }
        public BillableItemModel BillableItem { get; set; }

        public ItemMasterModel ItemMaster { get; set; }

        private PatientOrderDetailModel _PatientOrderDetail;

        public PatientOrderDetailModel PatientOrderDetail
        {
            get { return _PatientOrderDetail; }
            set
            {
                _PatientOrderDetail = value;
            }
        }



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


        private DateTime? _EndDate;

        public DateTime? EndDate
        {
            get { return _EndDate; }
            set
            {
                Set(ref _EndDate, value);
                if (_EndDate != null)
                {
                    var DurationTemp = (EndDate - StartDate).Value.Days;
                    if (Duration != DurationTemp)
                    {
                        Duration = DurationTemp;
                    }
                }
            }
        }

        private DateTime? _EndTime;
        public DateTime? EndTime
        {
            get { return _EndTime; }
            set { Set(ref _EndTime, value); }
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

        private double? _StockQuantity = 0;

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
                if (SelectUnit != null)
                {
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
        }

        private List<LookupReferenceValueModel> _PrescriptionTypes;

        public List<LookupReferenceValueModel> PrescriptionTypes
        {
            get { return _PrescriptionTypes; }
            set { Set(ref _PrescriptionTypes, value); }
        }

        private LookupReferenceValueModel _SelectPrescriptionType;

        public LookupReferenceValueModel SelectPrescriptionType
        {
            get { return _SelectPrescriptionType; }
            set { Set(ref _SelectPrescriptionType, value); }
        }

        private List<LookupReferenceValueModel> _DrugLabel;

        public List<LookupReferenceValueModel> DrugLabel
        {
            get { return _DrugLabel; }
            set { Set(ref _DrugLabel, value); }
        }

        private LookupReferenceValueModel _SelectDrugLabel;

        public LookupReferenceValueModel SelectDrugLabel
        {
            get { return _SelectDrugLabel; }
            set { Set(ref _SelectDrugLabel, value); }
        }

        private List<LookupReferenceValueModel> _DrugFORM;

        public List<LookupReferenceValueModel> DrugFORM
        {
            get { return _DrugFORM; }
            set { Set(ref _DrugFORM, value); }
        }


        private LookupReferenceValueModel _SelectDrugFORM;

        public LookupReferenceValueModel SelectDrugFORM
        {
            get { return _SelectDrugFORM; }
            set { Set(ref _SelectDrugFORM, value); }
        }

        private List<LookupReferenceValueModel> _DrugRoute;

        public List<LookupReferenceValueModel> DrugRoute
        {
            get { return _DrugRoute; }
            set { Set(ref _DrugRoute, value); }
        }


        private LookupReferenceValueModel _SelectDrugRoute;

        public LookupReferenceValueModel SelectDrugRoute
        {
            get { return _SelectDrugRoute; }
            set { Set(ref _SelectDrugRoute, value); }
        }



        private List<FrequencyDefinitionModel> _DrugFrequency;

        public List<FrequencyDefinitionModel> DrugFrequency
        {
            get { return _DrugFrequency; }
            set { Set(ref _DrugFrequency, value); }
        }

        private FrequencyDefinitionModel _SelectDrugFrequency;

        public FrequencyDefinitionModel SelectDrugFrequency
        {
            get { return _SelectDrugFrequency; }
            set
            {
                Set(ref _SelectDrugFrequency, value);
                if (SelectDrugFrequency != null)
                {
                    DrugFrequencyText = SelectDrugFrequency.Comments;
                    CalculateQuantity();
                }
                else
                {
                    DrugFrequencyText = "";
                }
            }
        }

        private string _DrugFrequencyText;

        public string DrugFrequencyText
        {
            get { return _DrugFrequencyText; }
            set { Set(ref _DrugFrequencyText, value); }
        }

        private string _DosageUnit;

        public string DosageUnit
        {
            get { return _DosageUnit; }
            set { Set(ref _DosageUnit, value); }
        }

        private int? _Duration;

        public int? Duration
        {
            get { return _Duration; }
            set
            {
                Set(ref _Duration, value);
                if (_Duration != null)
                {
                    CalculateQuantity();
                    if (SelectPrescriptionType != null && SelectPrescriptionType.ValueCode != "STORD")
                    {
                        DateTime? EndDateTemp = StartDate.AddDays(Duration.Value);
                        if (EndDate?.Date != EndDateTemp?.Date)
                        {
                            EndDate = EndDateTemp;
                            EndTime = EndDate?.Add(StartTime.TimeOfDay);
                        }
                    }
                }
            }
        }

        private double? _DosageQuantity;

        public double? DosageQuantity
        {
            get { return _DosageQuantity; }
            set
            {
                Set(ref _DosageQuantity, value);
                CalculateQuantity();
            }
        }

        private double _Quantity;

        public double Quantity
        {
            get { return _Quantity; }
            set
            {
                Set(ref _Quantity, value);
                if (Quantity != 0)
                {
                    CalculateDuration();
                }

            }
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

        private RelayCommand _CalculateQuantityCommand;

        public RelayCommand CalculateQuantityCommand
        {
            get { return _CalculateQuantityCommand ?? (_CalculateQuantityCommand = new RelayCommand(CalculateQuantity)); }
        }

        private RelayCommand _CalculateDurationCommand;

        public RelayCommand CalculateDurationCommand
        {
            get { return _CalculateDurationCommand ?? (_CalculateDurationCommand = new RelayCommand(CalculateDuration)); }
        }
        #endregion

        #region Method


        public OrderDrugItemViewModel()
        {

        }

        void CalculateQuantity()
        {

            if (SelectUnit != null && SelectUnit.NumericValue == 1)
            {
                if (!SuppressQuantityEvent)
                {
                    var tempQuantity = (DosageQuantity ?? 0) * (Duration ?? 0) * (SelectDrugFrequency != null ? SelectDrugFrequency.AmountPerTimes ?? 1 : 1);

                    SuppressDurationEvent = true;
                    Quantity = Math.Round(tempQuantity);
                    if (SelectUnit.ConversionValue != 1)
                    {
                        SuppressDurationEvent = true;
                        Quantity = Quantity * SelectUnit.ConversionValue;
                    }
                }

            }
            SuppressQuantityEvent = false;
        }

        void CalculateDuration()
        {
            if (SelectUnit != null && SelectUnit.NumericValue == 1)
            {
                if (!SuppressDurationEvent)
                {
                    var qty = Quantity;
                    if (SelectUnit.ConversionValue != 1)
                    {
                        qty = (Quantity / SelectUnit.ConversionValue);
                    }
                    var tempDuration = qty / ((DosageQuantity ?? 0) * (SelectDrugFrequency != null ? SelectDrugFrequency.AmountPerTimes ?? 1 : 1));

                    SuppressQuantityEvent = true;
                    Duration = Convert.ToInt32(Math.Ceiling(tempDuration));
                }

            }
            SuppressDurationEvent = false;
        }
        private void BindingData()
        {

            var refVale = DataService.Technical.GetReferenceValueList("PDSTS,FORMM,ROUTE,PRSTYP");
            PrescriptionTypes = refVale.Where(p => p.DomainCode == "PRSTYP").ToList();
            DrugFORM = refVale.Where(p => p.DomainCode == "FORMM").ToList();
            DrugLabel = refVale.Where(p => p.DomainCode == "PDSTS").ToList();
            DrugRoute = refVale.Where(p => p.DomainCode == "ROUTE").ToList();
            DrugFrequency = DataService.Pharmacy.GetDrugFrequency();
            Units = DataService.Inventory.GetItemConvertUOM(ItemMaster.ItemMasterUID);
        }
        public void BindingFromBillableItem()
        {
            DateTime now = DateTime.Now;
            ItemMaster = DataService.Inventory.GetItemMasterByUID(BillableItem.ItemUID.Value);
            Stores = DataService.Inventory.GetStockRemainForDispensedByItemMasterUID(ItemMaster.ItemMasterUID, OwnerOrgansitaionUID);
            BindingData();

            TypeOrder = BillableItem.BillingServiceMetaData;
            OrderName = BillableItem.ItemName;
            OrderCode = "Code : " + BillableItem.Code;
            UnitPrice = BillableItem.Price.ToString("#,#.00");

            SelectStore = Stores != null ? Stores.FirstOrDefault() : null;
            SelectDrugFORM = DrugFORM.FirstOrDefault(p => p.Key == ItemMaster.FORMMUID);
            SelectDrugLabel = DrugLabel.FirstOrDefault(p => p.Key == ItemMaster.PDSTSUID);
            SelectUnit = Units.FirstOrDefault(p => p.ConversionUOMUID == ItemMaster.BaseUOM);
            SelectDrugFrequency = DrugFrequency.FirstOrDefault(p => p.FrequencyUID == ItemMaster.FRQNCUID);

            SelectPrescriptionType = PrescriptionTypes.FirstOrDefault(p => p.ValueCode == "ROMED");

            DosageQuantity = ItemMaster.DoseQuantity ?? 1;
            Quantity = ItemMaster.MinSalesQty ?? 1;
            Duration = 1;
            DosageUnit = ItemMaster.PrescriptionUnit;

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

            EndDate = StartDate.AddDays(1).Date;
            EndTime = EndDate?.Add(StartTime.TimeOfDay);
        }

        public void BindingFromPatientOrderDetail()
        {
            ItemMaster = DataService.Inventory.GetItemMasterByUID(PatientOrderDetail.ItemUID.Value);
            Stores = DataService.Inventory.GetStockRemainForDispensedByItemMasterUID(ItemMaster.ItemMasterUID, PatientOrderDetail.OwnerOrganisationUID);
            BindingData();

            TypeOrder = PatientOrderDetail.BillingService;
            OrderName = PatientOrderDetail.ItemName;
            OrderCode = "Code : " + PatientOrderDetail.ItemCode;
            UnitPrice = PatientOrderDetail.OriginalUnitPrice.Value.ToString("#,#.00");
            StartDate = PatientOrderDetail.StartDttm.Value.Date;
            StartTime = PatientOrderDetail.StartDttm.Value;

            EndDate = PatientOrderDetail.EndDttm?.Date;
            EndTime = PatientOrderDetail.EndDttm;

            OverwritePrice = PatientOrderDetail.OverwritePrice;

            SuppressDurationEvent = true;
            Duration = PatientOrderDetail.DrugDuration;
            SelectStore = Stores.FirstOrDefault(p => p.StoreUID == PatientOrderDetail.StoreUID);
            SelectDrugFORM = DrugFORM.FirstOrDefault(p => p.Key == PatientOrderDetail.DFORMUID);
            SelectDrugLabel = DrugLabel.FirstOrDefault(p => p.Key == PatientOrderDetail.PDSTSUID);
            SelectUnit = Units.FirstOrDefault(p => p.ConversionUOMUID == PatientOrderDetail.QNUOMUID);

            SelectPrescriptionType = PrescriptionTypes.FirstOrDefault(p => p.Key == PatientOrderDetail.PRSTYPUID);


            SuppressQuantityEvent = true;
            SelectDrugFrequency = DrugFrequency.FirstOrDefault(p => p.FrequencyUID == PatientOrderDetail.FRQNCUID);
            SuppressQuantityEvent = true;
            DosageQuantity = PatientOrderDetail.Dosage;
            SuppressQuantityEvent = true;
            Quantity = PatientOrderDetail.Quantity ?? 1;

            DosageUnit = ItemMaster.PrescriptionUnit;

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

            Comment = ItemMaster.Comments;
            OrderInstruction = ItemMaster.OrderInstruction;
            LabelSticker = PatientOrderDetail.LocalInstructionText;
            NoteToPharmacy = PatientOrderDetail.ClinicalComments;
        }
        private void Add()
        {
            try
            {
                if (SelectStore == null)
                {
                    WarningDialog("ไม่มียาในคลัง โปรดตรวจสอบ");
                    return;
                }
                else
                {
                    if (Quantity > SelectStore.Quantity)
                    {
                        if (ItemMaster.CanDispenseWithOutStock != "Y")
                        {
                            WarningDialog("มียาในคลังไม่พอสำหรับจ่ายยา โปรดตรวจสอบ");
                            return;

                        }
                        else if (ItemMaster.CanDispenseWithOutStock == "Y")
                        {
                            MessageBoxResult result = QuestionDialog("มียาในคลังไม่พอสำหรับจ่ายยา คุณต้องการดำเนินการต่อหรือไม่ ?");
                            if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
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
                    PatientOrderDetail.OriginalUnitPrice = BillableItem.Price;
                    PatientOrderDetail.DoctorFee = (BillableItem.DoctorFee / 100) * BillableItem.Price;
                }

                PatientOrderDetail.Comments = Comment;
                PatientOrderDetail.StartDttm = StartDate.Add(StartTime.TimeOfDay);


                PatientOrderDetail.EndDttm = EndDate != null ? EndDate?.Add(EndTime.Value.TimeOfDay) : null;

                PatientOrderDetail.Dosage = DosageQuantity;
                PatientOrderDetail.DosageUnit = DosageUnit;
                PatientOrderDetail.Quantity = Quantity;
                if (SelectUnit != null)
                {
                    PatientOrderDetail.QNUOMUID = SelectUnit.ConversionUOMUID;
                    PatientOrderDetail.QuantityUnit = SelectUnit.ConversionUnit;
                }

                if (SelectDrugFrequency != null)
                {
                    PatientOrderDetail.FRQNCUID = SelectDrugFrequency.FrequencyUID;
                    PatientOrderDetail.DrugFrequency = SelectDrugFrequency.Name;
                }
                else
                {
                    PatientOrderDetail.FRQNCUID = null;
                    PatientOrderDetail.DrugFrequency = null;
                }

                if (SelectDrugLabel != null)
                {
                    PatientOrderDetail.PDSTSUID = SelectDrugLabel.Key;
                }
                else
                {
                    PatientOrderDetail.PDSTSUID = null;
                }
                //PatientOrderDetail.InstructionText = LabelSticker;
                PatientOrderDetail.LocalInstructionText = LabelSticker;
                PatientOrderDetail.ClinicalComments = NoteToPharmacy;
                PatientOrderDetail.IsStock = ItemMaster.IsStock;
                PatientOrderDetail.DrugDuration = Duration ?? 1;
                PatientOrderDetail.DFORMUID = SelectDrugFORM != null ? SelectDrugFORM.Key : (int?)null;
                PatientOrderDetail.StoreUID = SelectStore.StoreUID;
                PatientOrderDetail.PRSTYPUID = SelectPrescriptionType.Key;
                PatientOrderDetail.OrderType = SelectPrescriptionType.Display;
                if (OverwritePrice != null)
                {
                    PatientOrderDetail.UnitPrice = OverwritePrice;
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
                    PatientOrderDetail.UnitPrice = PatientOrderDetail.OriginalUnitPrice;
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
                    PatientOrderDetail.OwnerOrganisationUID = OwnerOrgansitaionUID;
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
