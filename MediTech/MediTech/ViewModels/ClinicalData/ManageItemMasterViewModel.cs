using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediTech.Model;
using MediTech.DataService;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using MediTech.Views;
using System.Windows;

namespace MediTech.ViewModels
{
    public class ManageItemMasterViewModel : MediTechViewModelBase
    {
        #region Properties

        private string _Code;

        public string Code
        {
            get { return _Code; }
            set { Set(ref _Code, value); }
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { Set(ref _Name, value); }
        }

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { Set(ref _Description, value); }
        }

        private double? _VATPercentage;

        public double? VATPercentage
        {
            get { return _VATPercentage; }
            set { Set(ref _VATPercentage, value); }
        }


        private DateTime? _ActiveFrom;

        public DateTime? ActiveFrom
        {
            get { return _ActiveFrom; }
            set { Set(ref _ActiveFrom, value); }
        }

        private DateTime? _ActiveTo;

        public DateTime? ActiveTo
        {
            get { return _ActiveTo; }
            set { Set(ref _ActiveTo, value); }
        }

        private double? _ItemCost;

        public double? ItemCost
        {
            get { return _ItemCost; }
            set { Set(ref _ItemCost, value); }
        }

        private LookupReferenceValueModel _SelectBaseUnit;

        public LookupReferenceValueModel SelectBaseUnit
        {
            get { return _SelectBaseUnit; }
            set { Set(ref _SelectBaseUnit, value); }
        }

        private LookupReferenceValueModel _SelectPresUnit;

        public LookupReferenceValueModel SelectPresUnit
        {
            get { return _SelectPresUnit; }
            set { Set(ref _SelectPresUnit, value); }
        }

        public List<VendorDetailModel> Manufacturers { get; set; }
        public List<VendorDetailModel> Vendors { get; set; }
        private VendorDetailModel _SelectManufacturer;

        public VendorDetailModel SelectManufacturer
        {
            get { return _SelectManufacturer; }
            set { Set(ref _SelectManufacturer, value); }
        }

        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { Set(ref _Comments, value); }
        }

        private double? _MinSalesQty;

        public double? MinSalesQty
        {
            get { return _MinSalesQty; }
            set
            {
                Set(ref _MinSalesQty, value);
            }
        }

        private double? _DoseQuantity;

        public double? DoseQuantity
        {
            get { return _DoseQuantity; }
            set { Set(ref _DoseQuantity, value); }
        }



        private bool? _IsStock = true;

        public bool? IsStock
        {
            get { return _IsStock; }
            set
            {
                Set(ref _IsStock, value);
                if (_IsStock != null)
                {
                    if (IsStock == false)
                    {
                        IsBatch = false;
                    }
                }
            }
        }

        private bool? _IsBatch = true;

        public bool? IsBatch
        {
            get { return _IsBatch; }
            set
            {
                Set(ref _IsBatch, value);
                if (_IsBatch != null)
                {
                    if (IsBatch == true)
                    {
                        IsStock = true;
                    }
                }
            }
        }

        private bool? _CanDispenseWithOutStock;

        public bool? CanDispenseWithOutStock
        {
            get { return _CanDispenseWithOutStock; }
            set
            {
                Set(ref _CanDispenseWithOutStock, value);
            }
        }


        public List<LookupReferenceValueModel> ItemTypes { get; set; }
        private LookupReferenceValueModel _SelectItemType;

        public LookupReferenceValueModel SelectItemType
        {
            get { return _SelectItemType; }
            set
            {
                Set(ref _SelectItemType, value);
                if (SelectItemType != null)
                {
                    if (SelectItemType.Key == 632)
                    {
                        IsPresVisibility = Visibility.Visible;
                    }
                    else
                    {
                        IsPresVisibility = Visibility.Hidden;
                    }
                }
            }
        }

        public List<DrugGenericModel> DrugGenarics { get; set; }
        private DrugGenericModel _SelectDrugGenaric;

        public DrugGenericModel SelectDrugGenaric
        {
            get { return _SelectDrugGenaric; }
            set { Set(ref _SelectDrugGenaric, value); }
        }

        private string _DispenseEnglish;

        public string DispenseEnglish
        {
            get { return _DispenseEnglish; }
            set { Set(ref _DispenseEnglish, value); }
        }

        private string _DispenseLocal;

        public string DispenseLocal
        {
            get { return _DispenseLocal; }
            set { Set(ref _DispenseLocal, value); }
        }
        private string _OrderInstruction;

        public string OrderInstruction
        {
            get { return _OrderInstruction; }
            set { Set(ref _OrderInstruction, value); }
        }

        public List<LookupReferenceValueModel> DrugInstructions { get; set; }
        private LookupReferenceValueModel _SelectDrugInstruction;

        public LookupReferenceValueModel SelectDrugInstruction
        {
            get { return _SelectDrugInstruction; }
            set { Set(ref _SelectDrugInstruction, value); }
        }

        public List<LookupReferenceValueModel> DrugForms { get; set; }
        private LookupReferenceValueModel _SelectDrugFrom;

        public LookupReferenceValueModel SelectDrugFrom
        {
            get { return _SelectDrugFrom; }
            set { Set(ref _SelectDrugFrom, value); }
        }

        public List<FrequencyDefinitionModel> DrugFrequencys { get; set; }
        private FrequencyDefinitionModel _SelectDrugFrequency;

        public FrequencyDefinitionModel SelectDrugFrequency
        {
            get { return _SelectDrugFrequency; }
            set { Set(ref _SelectDrugFrequency, value); }
        }

        private bool _IsNarcotic;

        public bool IsNarcotic
        {
            get { return _IsNarcotic; }
            set { Set(ref _IsNarcotic, value); }
        }

        public List<LookupReferenceValueModel> NarcoticTypes { get; set; }

        private LookupReferenceValueModel _SelectNarcotic;

        public LookupReferenceValueModel SelectNarcotic
        {
            get { return _SelectNarcotic; }
            set { Set(ref _SelectNarcotic, value); }
        }


        public List<LookupReferenceValueModel> ItemMasterUnits { get; set; }

        private ObservableCollection<ItemUOMConversionModel> _ItemUOMConversions;

        public ObservableCollection<ItemUOMConversionModel> ItemUOMConversions
        {
            get { return _ItemUOMConversions; }
            set { Set(ref _ItemUOMConversions, value); }
        }

        public ItemUOMConversionModel SelectItemUOMConversion { get; set; }


        private ObservableCollection<ItemVendorDetailModel> _ItemVendorDetails;

        public ObservableCollection<ItemVendorDetailModel> ItemVendorDetails
        {
            get { return _ItemVendorDetails; }
            set { Set(ref _ItemVendorDetails, value); }
        }

        public ItemVendorDetailModel SelectItemVendorDetail { get; set; }

        private Visibility _IsPresVisibility;

        public Visibility IsPresVisibility
        {
            get { return _IsPresVisibility; }
            set { Set(ref _IsPresVisibility, value); }
        }

        private Visibility _VisibilityAutoCode = Visibility.Visible;

        public Visibility VisibilityAutoCode
        {
            get { return _VisibilityAutoCode; }
            set { Set(ref _VisibilityAutoCode, value); }
        }

        private bool _IsEnabledEdit = true;

        public bool IsEnabledEdit
        {
            get { return _IsEnabledEdit; }
            set { Set(ref _IsEnabledEdit, value); }
        }

        #endregion

        #region Command

        private RelayCommand _GenerateCodeCommand;
        public RelayCommand GenerateCodeCommand
        {
            get { return _GenerateCodeCommand ?? (_GenerateCodeCommand = new RelayCommand(GenerateCode)); }
        }


        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save)); }
        }


        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }

        private RelayCommand _DeleteConvertUOMCommand;
        public RelayCommand DeleteConvertUOMCommand
        {
            get { return _DeleteConvertUOMCommand ?? (_DeleteConvertUOMCommand = new RelayCommand(DeleteConvertUOM)); }
        }

        private RelayCommand _DeleteVendorCommand;
        public RelayCommand DeleteVendorCommand
        {
            get { return _DeleteVendorCommand ?? (_DeleteVendorCommand = new RelayCommand(DeleteVendor)); }
        }



        #endregion

        #region Method

        #region Variable

        ItemMasterModel model;

        #endregion

        public ManageItemMasterViewModel()
        {
            ItemUOMConversions = new ObservableCollection<ItemUOMConversionModel>();
            ItemVendorDetails = new ObservableCollection<ItemVendorDetailModel>();
            var refValue = DataService.Technical.GetReferenceValueList("IMUOM,NRCTP,ITMTYP,PDSTS,FORMM");
            DrugGenarics = DataService.Pharmacy.GetDrugGeneric();
            DrugFrequencys = DataService.Pharmacy.GetDrugFrequency();
            ItemMasterUnits = refValue.Where(p => p.DomainCode == "IMUOM").ToList();
            NarcoticTypes = refValue.Where(p => p.DomainCode == "NRCTP").ToList();
            ItemTypes = refValue.Where(p => p.DomainCode == "ITMTYP").ToList();
            DrugForms = refValue.Where(p => p.DomainCode == "FORMM").ToList();
            DrugInstructions = refValue.Where(p => p.DomainCode == "PDSTS").ToList();

            var vendors = DataService.Purchaseing.GetVendorDetail();
            if (vendors != null)
            {
                Manufacturers = vendors.Where(p => p.MNFTPUID == 2938).ToList();
                Vendors = vendors.Where(p => p.MNFTPUID == 2937).ToList();
            }
            ActiveFrom = DateTime.Now.Date;
        }

        private void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(Code))
                {
                    WarningDialog("กรุณาใส่ รหัส");
                    return;
                }
                if (string.IsNullOrEmpty(Name))
                {
                    WarningDialog("กรุณาใส่ ชื่อ");
                    return;
                }
                if (SelectItemType == null)
                {
                    WarningDialog("กรุณาเลือก ประเภท");
                    return;
                }
                if (SelectBaseUnit == null)
                {
                    WarningDialog("กรุณาเลือก Base Unit");
                    return;
                }

                if (SelectItemType.Key == 632)
                {
                    if (SelectPresUnit == null)
                    {
                        WarningDialog("กรุณาเลือก Prescription Unit");
                        return;
                    }
                }

                if (model == null)
                {
                    var dupicateCode = DataService.Inventory.GetItemMasterByCode(Code);
                    if (dupicateCode != null)
                    {
                        WarningDialog("Code ซ้ำ โปรดตรวจสอบ");
                        return;
                    }
                }


                AssingPropertiesToModel();
                DataService.Inventory.ManageItemMaster(model, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ListItemMaster pageListItem = new ListItemMaster();
                ChangeViewPermission(pageListItem);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void Cancel()
        {
            ListItemMaster pageListItem = new ListItemMaster();
            ChangeViewPermission(pageListItem);
        }

        private void GenerateCode()
        {
            try
            {
                string ItemCode = DataService.Inventory.GenerateCodeItem();
                if (!string.IsNullOrEmpty(Name))
                {
                    ItemCode = Name.Substring(0, 1) + ItemCode;
                }
                Code = ItemCode;
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void DeleteConvertUOM()
        {
            if (SelectItemUOMConversion != null)
            {
                ItemUOMConversions.Remove(SelectItemUOMConversion);
                OnUpdateEvent();
            }
        }

        private void DeleteVendor()
        {
            if (SelectItemVendorDetail != null)
            {
                ItemVendorDetails.Remove(SelectItemVendorDetail);
                OnUpdateEvent();
            }
        }
        public void AssingModelData(ItemMasterModel modelData)
        {
            model = modelData;
            AssingModelToProperties();
        }
        public void AssingModelToProperties()
        {
            Code = model.Code;
            Name = model.Name;
            Description = model.Description;
            ActiveFrom = model.ActiveFrom;
            ActiveTo = model.ActiveTo;
            ItemCost = model.ItemCost;
            SelectBaseUnit = ItemMasterUnits != null ? ItemMasterUnits.FirstOrDefault(p => p.Key == model.BaseUOM) : null;
            SelectPresUnit = ItemMasterUnits != null ? ItemMasterUnits.FirstOrDefault(p => p.Key == model.PrescriptionUOM) : null;
            SelectManufacturer = Manufacturers != null ? Manufacturers.FirstOrDefault(p => p.VendorDetailUID == model.ManufacturerByUID) : null;
            Comments = model.Comments;


            if (model.IsStock == "Y")
            {
                IsStock = true;
            }
            else
            {
                IsStock = false;
            }

            if (model.IsBatchIDMandatory == "Y")
            {
                IsBatch = true;
            }
            else
            {
                IsBatch = false;
            }

            if (model.CanDispenseWithOutStock == "Y")
            {
                CanDispenseWithOutStock = true;
            }

            MinSalesQty = model.MinSalesQty;
            DoseQuantity = model.DoseQuantity;
            VATPercentage = model.VATPercentage;
            SelectItemType = ItemTypes != null ? ItemTypes.FirstOrDefault(p => p.Key == model.ITMTYPUID) : null;
            SelectDrugGenaric = DrugGenarics != null ? DrugGenarics.FirstOrDefault(p => p.DrugGenericUID == model.DrugGenaricUID) : null;
            SelectDrugFrom = DrugForms != null ? DrugForms.FirstOrDefault(p => p.Key == model.FORMMUID) : null;
            SelectDrugInstruction = DrugInstructions != null ? DrugInstructions.FirstOrDefault(p => p.Key == model.PDSTSUID) : null;
            DispenseEnglish = model.DispenseEnglish;
            DispenseLocal = model.DispenseLocal;
            OrderInstruction = model.OrderInstruction;
            SelectDrugFrequency = DrugFrequencys != null ? DrugFrequencys.FirstOrDefault(p => p.FrequencyUID == model.FRQNCUID) : null;

            if (model.IsNarcotic == "Y")
            {
                IsNarcotic = true;
            }

            SelectNarcotic = NarcoticTypes != null ? NarcoticTypes.FirstOrDefault(p => p.Key == model.NRCTPUID) : null;
            if (model != null)
            {
                ItemUOMConversions = new ObservableCollection<ItemUOMConversionModel>(DataService.Inventory.GetItemUOMConversionByItemMasterUID(model.ItemMasterUID));
                ItemVendorDetails = new ObservableCollection<ItemVendorDetailModel>(DataService.Inventory.GetItemVendorDetailByItemMasterUID(model.ItemMasterUID));
            }

            VisibilityAutoCode = Visibility.Collapsed;
            IsEnabledEdit = false;
        }

        public void AssingPropertiesToModel()
        {
            if (model == null)
            {
                model = new ItemMasterModel();
            }

            model.Code = Code;
            model.Name = Name;
            model.Description = Description;
            model.ActiveFrom = ActiveFrom;
            model.ActiveTo = ActiveTo;
            model.ItemCost = ItemCost;
            model.BaseUOM = SelectBaseUnit != null ? SelectBaseUnit.Key : (int?)null;
            model.SalesUOM = model.BaseUOM;
            model.PrescriptionUOM = SelectPresUnit != null ? SelectPresUnit.Key : (int?)null;
            model.ManufacturerByUID = SelectManufacturer != null ? SelectManufacturer.VendorDetailUID : (int?)null;
            model.Comments = Comments;
            model.IsStock = IsStock == true ? "Y" : "N";
            model.IsBatchIDMandatory = IsBatch == true ? "Y" : "N";
            model.CanDispenseWithOutStock = CanDispenseWithOutStock == true ? "Y" : "N";
            model.MinSalesQty = MinSalesQty;
            model.DoseQuantity = DoseQuantity;
            model.ITMTYPUID = SelectItemType != null ? SelectItemType.Key.Value : 0;
            model.DrugGenaricUID = SelectDrugGenaric != null ? SelectDrugGenaric.DrugGenericUID : (int?)null;
            model.GenaricName = SelectDrugGenaric != null ? SelectDrugGenaric.Name : "";
            model.PDSTSUID = SelectDrugInstruction != null ? SelectDrugInstruction.Key : (int?)null;
            model.FORMMUID = SelectDrugFrom != null ? SelectDrugFrom.Key : (int?)null;
            model.DispenseEnglish = DispenseEnglish;
            model.DispenseLocal = DispenseLocal;
            model.OrderInstruction = OrderInstruction;
            model.FRQNCUID = SelectDrugFrequency != null ? SelectDrugFrequency.FrequencyUID : (int?)null;
            model.VATPercentage = VATPercentage;
            model.IsNarcotic = IsNarcotic == true ? "Y" : "N";
            if (IsNarcotic == true)
            {
                model.NRCTPUID = SelectNarcotic != null ? SelectNarcotic.Key : (int?)null;
            }
            else
            {
                model.NRCTPUID = null;
            }


            if (ItemUOMConversions != null)
            {
                model.ItemUOMConversions = new List<ItemUOMConversionModel>();
                model.ItemUOMConversions.AddRange(ItemUOMConversions);
            }

            if (ItemVendorDetails != null)
            {
                model.ItemVendorDetails = new List<ItemVendorDetailModel>();
                model.ItemVendorDetails.AddRange(ItemVendorDetails);
            }

        }

        #endregion
    }
}
