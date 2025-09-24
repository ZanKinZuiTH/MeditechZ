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
    public class ManageOrderSetViewModel : MediTechViewModelBase
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

        //private List<HealthOrganisationModel> _Organisations;

        //public List<HealthOrganisationModel> Organisations
        //{
        //    get { return _Organisations; }
        //    set { Set(ref _Organisations, value); }
        //}


        //private HealthOrganisationModel _SelectOrganisation;

        //public HealthOrganisationModel SelectOrganisation
        //{
        //    get { return _SelectOrganisation; }
        //    set
        //    {
        //        Set(ref _SelectOrganisation, value);

        //        if (OrderSetBillableItems != null && OrderSetBillableItems.Count > 0)
        //        {
        //            foreach (var item in OrderSetBillableItems)
        //            {
        //                int ownerOrganisationUID = SelectOrganisation == null ? 0 : SelectOrganisation.HealthOrganisationUID;
        //                string ownerOrganisationName = SelectOrganisation == null ? "" : SelectOrganisation.Name;

        //                var priceStandard = item.BillableItemDetails.FirstOrDefault(p => p.OwnerOrganisationUID == 0);
        //                if (priceStandard != null)
        //                {
        //                    item.Price = priceStandard.Price;
        //                }

        //                var TmpPrice = item.BillableItemDetails.FirstOrDefault(p => p.OwnerOrganisationUID == ownerOrganisationUID);
        //                if (TmpPrice != null)
        //                {
        //                    item.Price = TmpPrice.Price;
        //                }

        //                if (priceStandard == null && TmpPrice == null)
        //                {
        //                    item.Price = 0;
        //                    WarningDialog("รายการ " + item.OrderCatalogName + " ไม่มีการตั้งค่าราคาขายสำหรับ " + ownerOrganisationName);
        //                }

        //                item.NetPrice = item.Price * item.Quantity;
        //            }
        //            bool tempEnableAdd = EnableAdd;
        //            OnUpdateEvent();
        //            if (tempEnableAdd)
        //            {
        //                SelectOrderSetBillableItem = null;
        //            }
        //        }
        //    }
        //}

        public List<BillableItemModel> BillableItems { get; set; }
        private BillableItemModel _SelectBillableItem;

        public BillableItemModel SelectBillableItem
        {
            get { return _SelectBillableItem; }
            set
            {
                Set(ref _SelectBillableItem, value);
                if (_SelectBillableItem != null)
                {
                    if (_SelectBillableItem.BillingServiceMetaData == "Drug")
                    {
                        EnableFrequency = true;
                        EnableDose = true;
                    }
                    else
                    {
                        EnableFrequency = false;
                        EnableDose = false;
                    }

                    if (_SelectBillableItem.DoctorFee != null && _SelectBillableItem.DoctorFee > 0)
                    {
                        DoctorFee = SelectBillableItem.DoctorFee;
                    }
                }
            }
        }

        private DateTime? _ActiveFrom2;

        public DateTime? ActiveFrom2
        {
            get { return _ActiveFrom2; }
            set { Set(ref _ActiveFrom2, value); }
        }

        private DateTime? _ActiveTo2;

        public DateTime? ActiveTo2
        {
            get { return _ActiveTo2; }
            set { Set(ref _ActiveTo2, value); }
        }


        public List<FrequencyDefinitionModel> Frequency { get; set; }
        private FrequencyDefinitionModel _SelectFrequency;

        public FrequencyDefinitionModel SelectFrequency
        {
            get { return _SelectFrequency; }
            set { Set(ref _SelectFrequency, value); }
        }

        private double _Quantity;

        public double Quantity
        {
            get { return _Quantity; }
            set { Set(ref _Quantity, value); }
        }

        private double? _DoseQuantity;

        public double? DoseQuantity
        {
            get { return _DoseQuantity; }
            set { Set(ref _DoseQuantity, value); }
        }


        private double? _Price;

        public double? Price
        {
            get { return _Price; }
            set { Set(ref _Price, value); }
        }


        private double? _DoctorFee;

        public double? DoctorFee
        {
            get { return _DoctorFee; }
            set { Set(ref _DoctorFee, value); }
        }

        public List<CareproviderModel> Careproviders { get; set; }
        private CareproviderModel _SelectCareprovider;

        public CareproviderModel SelectCareprovider
        {
            get { return _SelectCareprovider; }
            set { Set(ref _SelectCareprovider, value); }
        }

        private string _NoteProcessing;

        public string NoteProcessing
        {
            get { return _NoteProcessing; }
            set { Set(ref _NoteProcessing, value); }
        }


        private ObservableCollection<OrderSetBillableItemModel> _OrderSetBillableItems;

        public ObservableCollection<OrderSetBillableItemModel> OrderSetBillableItems
        {
            get { return _OrderSetBillableItems ?? (_OrderSetBillableItems = new ObservableCollection<OrderSetBillableItemModel>()); }
            set { Set(ref _OrderSetBillableItems, value); }
        }

        private OrderSetBillableItemModel _SelectOrderSetBillableItem;

        public OrderSetBillableItemModel SelectOrderSetBillableItem
        {
            get { return _SelectOrderSetBillableItem; }
            set
            {
                Set(ref _SelectOrderSetBillableItem, value);
                if (_SelectOrderSetBillableItem != null)
                {
                    EnableAdd = false;
                    EnableBillItemName = false;
                    EnableUpdate = true;


                    SelectBillableItem = BillableItems.FirstOrDefault(p => p.BillableItemUID == SelectOrderSetBillableItem.BillableItemUID);
                    SelectFrequency = Frequency.FirstOrDefault(p => p.FrequencyUID == SelectOrderSetBillableItem.FRQNCUID);
                    Quantity = SelectOrderSetBillableItem.Quantity;
                    DoseQuantity = SelectOrderSetBillableItem.DoseQty;
                    ActiveFrom2 = SelectOrderSetBillableItem.ActiveFrom;
                    ActiveTo2 = SelectOrderSetBillableItem.ActiveTo;
                    NoteProcessing = SelectOrderSetBillableItem.ProcessingNotes;
                    Price = SelectOrderSetBillableItem.Price;
                    DoctorFee = SelectOrderSetBillableItem.DoctorFee;
                    SelectCareprovider = Careproviders.FirstOrDefault(p => p.CareproviderUID == SelectOrderSetBillableItem.CareproviderUID);
                }
                else
                {
                    EnableAdd = true;
                    EnableBillItemName = true;
                    EnableUpdate = false;


                    SelectBillableItem = null;
                    SelectFrequency = null;
                    Price = null;
                    DoctorFee = null;
                    Quantity = 1;
                    DoseQuantity = null;
                    ActiveFrom2 = DateTime.Now;
                    ActiveTo2 = null;
                    NoteProcessing = string.Empty;
                }


            }
        }

        private bool _EnableFrequency;

        public bool EnableFrequency
        {
            get { return _EnableFrequency; }
            set { Set(ref _EnableFrequency, value); }
        }

        private bool _EnableDose;

        public bool EnableDose
        {
            get { return _EnableDose; }
            set { Set(ref _EnableDose, value); }
        }

        private bool _EnableAdd = true;

        public bool EnableAdd
        {
            get { return _EnableAdd; }
            set { Set(ref _EnableAdd, value); }
        }

        private bool _EnableUpdate;

        public bool EnableUpdate
        {
            get { return _EnableUpdate; }
            set { Set(ref _EnableUpdate, value); }
        }


        private bool _EnableBillItemName = true;

        public bool EnableBillItemName
        {
            get { return _EnableBillItemName; }
            set { Set(ref _EnableBillItemName, value); }
        }

        private List<OrderCategoryModel> _Category;
        public List<OrderCategoryModel> Category
        {
            get { return _Category; }
            set { Set(ref _Category, value); }
        }

        private OrderCategoryModel _SelectCategory;
        public OrderCategoryModel SelectCategory
        {
            get { return _SelectCategory; }
            set
            {
                Set(ref _SelectCategory, value);
                if (SelectCategory != null)
                {
                    OrderSubCategory = DataService.MasterData.GetOrderSubCategoryByUID(SelectCategory.OrderCategoryUID);
                }
            }
        }

        private List<OrderSubCategoryModel> _OrderSubCategory;
        public List<OrderSubCategoryModel> OrderSubCategory
        {
            get { return _OrderSubCategory; }
            set
            {
                Set(ref _OrderSubCategory, value);

            }
        }

        private OrderSubCategoryModel _SelectOrderSubCategory;
        public OrderSubCategoryModel SelectOrderSubCategory
        {
            get { return _SelectOrderSubCategory; }
            set { Set(ref _SelectOrderSubCategory, value); }
        }

        #endregion

        #region Command

        private RelayCommand _SaveCommand;


        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                    ?? (_SaveCommand = new RelayCommand(SaveOrderSet));
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


        private RelayCommand _AddCommand;


        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand
                    ?? (_AddCommand = new RelayCommand(AddBillableItem));
            }
        }

        private RelayCommand _UpdateCommand;


        public RelayCommand UpdateCommand
        {
            get
            {
                return _UpdateCommand
                    ?? (_UpdateCommand = new RelayCommand(UpdateBillableItem));
            }
        }

        private RelayCommand _DeleteCommand;


        public RelayCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand
                    ?? (_DeleteCommand = new RelayCommand(DeleteBillableItem));
            }
        }

        private RelayCommand _ResetCommand;


        public RelayCommand ResetCommand
        {
            get
            {
                return _ResetCommand
                    ?? (_ResetCommand = new RelayCommand(Reset));
            }
        }

        #endregion

        #region Method

        OrderSetModel model;
        public ManageOrderSetViewModel()
        {
            Frequency = DataService.Pharmacy.GetDrugFrequency();
            BillableItems = DataService.MasterData.GetBillableItemAll();
            Careproviders = DataService.UserManage.GetCareproviderDoctor();
            //Organisations = GetHealthOrganisationMedical();
            //Organisations.Add(new HealthOrganisationModel { HealthOrganisationUID = 0, Name = "ราคามาตรฐานส่วนกลาง" });
            //Organisations = Organisations.OrderBy(p => p.HealthOrganisationUID).ToList();
            //SelectOrganisation = Organisations.FirstOrDefault();
            Quantity = 1;
            ActiveFrom = DateTime.Now;
            ActiveFrom2 = ActiveFrom;
            Category = DataService.MasterData.GetOrderCategory();

        }
        private void SaveOrderSet()
        {
            try
            {
                if (string.IsNullOrEmpty(Code))
                {
                    WarningDialog("กรุณาระบุ Code");
                    return;
                }
                if (string.IsNullOrEmpty(Name))
                {
                    WarningDialog("กรุณาระบุ ชื่อ");
                    return;
                }
                if (OrderSetBillableItems == null || OrderSetBillableItems.Count(p => p.StatusFlag != "D") <= 0)
                {
                    WarningDialog("ต้องมีรายการ Item อย่างน้อย 1 รายการ");
                    return;
                }

                AssingPropertiesToModel();
                DataService.MasterData.ManageOrderSet(model, AppUtil.Current.UserID);
                SaveSuccessDialog();

                ListOrderSet pageList = new ListOrderSet();
                ChangeViewPermission(pageList);
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }
        private void Cancel()
        {
            ListOrderSet pageList = new ListOrderSet();
            ChangeViewPermission(pageList);
        }
        private void AddBillableItem()
        {
            if (SelectBillableItem != null)
            {
                if (OrderSetBillableItems.FirstOrDefault(p => p.BillableItemUID == SelectBillableItem.BillableItemUID) != null)
                {
                    WarningDialog("รายการ Item นี้มีอยู่แล้ว");
                    return;
                }

                //if (DoctorFee != 0 && SelectCareprovider == null)
                //{
                //    WarningDialog("กรุณาเลือกแพทย์");
                //    return;
                //}

                //if ((Price ?? 0) == 0)
                //{
                //    WarningDialog("กรุณาระบุราคาขายต่อหน่วย");
                //    return;
                //}

                OrderSetBillableItemModel newBillItem = new OrderSetBillableItemModel();
                newBillItem.BillableItemUID = SelectBillableItem.BillableItemUID;
                newBillItem.Code = SelectBillableItem.Code;
                newBillItem.OrderCatalogName = SelectBillableItem.ItemName;
                newBillItem.Quantity = Quantity;
                newBillItem.DoseQty = DoseQuantity;
                newBillItem.Price = Price ?? 0;
                newBillItem.DoctorFee = DoctorFee ?? 0;
                newBillItem.CareproviderUID = SelectCareprovider != null ? SelectCareprovider.CareproviderUID : (int?)null;
                newBillItem.CareproviderName = SelectCareprovider != null ? SelectCareprovider.FullName : null;
                newBillItem.ProcessingNotes = NoteProcessing;
                newBillItem.ActiveFrom = ActiveFrom2;
                newBillItem.ActiveTo = ActiveTo2;
                newBillItem.StatusFlag = "A";
                //newBillItem.BillableItemDetails = DataService.MasterData.GetBillableItemDetailByBillableItemUID(SelectBillableItem.BillableItemUID);

                //int ownerOrganisationUID = SelectOrganisation == null ? 0 : SelectOrganisation.HealthOrganisationUID;
                //string ownerOrganisationName = SelectOrganisation == null ? "" : SelectOrganisation.Name;

                //var priceStandard = newBillItem.BillableItemDetails.FirstOrDefault(p => p.OwnerOrganisationUID == 0);
                //if (priceStandard != null)
                //{
                //    newBillItem.Price = priceStandard.Price;
                //}

                //var TmpPrice = newBillItem.BillableItemDetails.FirstOrDefault(p => p.OwnerOrganisationUID == ownerOrganisationUID);
                //if (TmpPrice != null)
                //{
                //    newBillItem.Price = TmpPrice.Price;
                //}

                //if (priceStandard == null && TmpPrice == null)
                //{
                //    DialogResult result = QuestionDialog("รายการ " + SelectBillableItem.ItemName
                //        + " ไม่มีการตั้งค่าราคาขายสำหรับ " + ownerOrganisationName + " ต้องการดำเนินการต่อหรือไม่");

                //    if (result != DialogResult.Yes)
                //    {
                //        return;
                //    }
                //}
                newBillItem.NetPrice = (newBillItem.Price ?? 0) * newBillItem.Quantity;

                if (SelectFrequency != null && EnableFrequency)
                {
                    newBillItem.FRQNCUID = SelectFrequency.FrequencyUID;
                }
                OrderSetBillableItems.Add(newBillItem);
                OnUpdateEvent();

                Reset();
            }
        }

        private void UpdateBillableItem()
        {
            if ((Price ?? 0) == 0)
            {
                WarningDialog("กรุณาระบุราคาขายต่อหน่วย");
                return;
            }

            if (SelectOrderSetBillableItem != null)
            {
                SelectOrderSetBillableItem.Quantity = Quantity;
                SelectOrderSetBillableItem.DoseQty = DoseQuantity;
                SelectOrderSetBillableItem.Price = Price;
                SelectOrderSetBillableItem.NetPrice = (Price ?? 0) * Quantity;
                SelectOrderSetBillableItem.DoctorFee = DoctorFee;
                SelectOrderSetBillableItem.CareproviderUID = SelectCareprovider != null ? SelectCareprovider.CareproviderUID : (int?)null;
                SelectOrderSetBillableItem.CareproviderName = SelectCareprovider != null ? SelectCareprovider.FullName : null;
                SelectOrderSetBillableItem.ProcessingNotes = NoteProcessing;
                SelectOrderSetBillableItem.ActiveFrom = ActiveFrom2;
                SelectOrderSetBillableItem.ActiveTo = ActiveTo2;
                SelectOrderSetBillableItem.MWhen = DateTime.Now;
                if (EnableFrequency)
                {
                    SelectOrderSetBillableItem.FRQNCUID = SelectFrequency != null ? SelectFrequency.FrequencyUID : (int?)null;
                }
                OnUpdateEvent();
                Reset();
            }
        }

        private void DeleteBillableItem()
        {
            if (SelectOrderSetBillableItem != null)
            {
                SelectOrderSetBillableItem.StatusFlag = "D";
                OnUpdateEvent();
                Reset();
            }
        }

        private void Reset()
        {
            SelectOrderSetBillableItem = null;
            ActiveFrom2 = DateTime.Now;
        }
        public void AssingModel(OrderSetModel model)
        {
            this.model = DataService.MasterData.GetOrderSetByUID(model.OrderSetUID);
            AssingModelToProperties(this.model);
        }

        public void CopyOrderSet(OrderSetModel modelCopy)
        {
            modelCopy = DataService.MasterData.GetOrderSetByUID(modelCopy.OrderSetUID);
            modelCopy.Name = "";
            modelCopy.Code = "";
            modelCopy.ActiveFrom = DateTime.Now;
            modelCopy.ActiveTo = null;
            ActiveFrom2 = modelCopy.ActiveFrom;
            modelCopy.OrderSetBillableItems.ForEach(p => { p.OrderSetUID = 0; p.OrderSetBillableItemUID = 0; });
            AssingModelToProperties(modelCopy);
        }

        public void AssingModelToProperties(OrderSetModel model)
        {
            Name = model.Name;
            Code = model.Code;
            Description = model.Description;
            ActiveFrom = model.ActiveFrom;
            ActiveTo = model.ActiveTo;
            OrderSetBillableItems = new ObservableCollection<OrderSetBillableItemModel>(model.OrderSetBillableItems);
            //if (OrderSetBillableItems != null && OrderSetBillableItems.Count > 0)
            //{
            //    foreach (var item in OrderSetBillableItems)
            //    {
            //        int ownerOrganisationUID = SelectOrganisation == null ? 0 : SelectOrganisation.HealthOrganisationUID;
            //        item.Price = item.BillableItemDetails.FirstOrDefault(p => p.OwnerOrganisationUID == ownerOrganisationUID).Price;
            //        item.NetPrice = item.Price * item.Quantity;
            //    }
            //    OnUpdateEvent();
            //}
            SelectOrderSetBillableItem = null;
            SelectCategory = model.OrderCategoryUID != null ? Category.FirstOrDefault(p => p.OrderCategoryUID == model.OrderCategoryUID) : null;
            SelectOrderSubCategory = model.OrderSubCategoryUID != null ? OrderSubCategory.FirstOrDefault(p => p.OrderSubCategoryUID == model.OrderSubCategoryUID) : null;

        }
        public void AssingPropertiesToModel()
        {
            if (model == null)
            {
                model = new OrderSetModel();
            }
            model.Name = Name;
            model.Code = Code;
            model.Description = Description;
            model.ActiveFrom = ActiveFrom;
            model.ActiveTo = ActiveTo;
            model.OrderSetBillableItems = OrderSetBillableItems.ToList();
            model.OrderCategoryUID = SelectCategory.OrderCategoryUID;
            model.OrderSubCategoryUID = SelectOrderSubCategory.OrderSubCategoryUID;
        }

        #endregion
    }
}
