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
using System.Windows.Data;

namespace MediTech.ViewModels
{
    public class ManagePackageViewModel : MediTechViewModelBase
    {
        #region Properties
        private BillPackageModel _model;
        public BillPackageModel model
        {
            get { return _model; }
            set { _model = value; }
        }

        private List<OrderCategoryModel> _Catagory;
        public List<OrderCategoryModel> Catagory
        {
            get { return _Catagory; }
            set { Set(ref _Catagory, value); }
        }

        private OrderCategoryModel _SelectCatagory;
        public OrderCategoryModel SelectCatagory
        {
            get { return _SelectCatagory; }
            set { Set(ref _SelectCatagory, value); 
            if(SelectCatagory != null)
                {
                    OrderSubCatagory = DataService.MasterData.GetOrderSubCategoryByUID(SelectCatagory.OrderCategoryUID);
                }
            }
        }

        private List<OrderSubCategoryModel> _OrderSubCatagory;
        public List<OrderSubCategoryModel> OrderSubCatagory
        {
            get { return _OrderSubCatagory; }
            set { Set(ref _OrderSubCatagory, value); 
                
            }
        }

        private OrderSubCategoryModel _SelectOrderSubCatagory;
        public OrderSubCategoryModel SelectOrderSubCatagory
        {
            get { return _SelectOrderSubCatagory; }
            set { Set(ref _SelectOrderSubCatagory, value); }
        }

        private string _PackageCode;
        public string PackageCode
        {
            get { return _PackageCode; }
            set { Set(ref _PackageCode, value); }
        }

        private string _PackageName;
        public string PackageName
        {
            get { return _PackageName; }
            set { Set(ref _PackageName, value); }
        }

        private string _TotalAmount;
        public string TotalAmount
        {
            get { return _TotalAmount; }
            set { Set(ref _TotalAmount, value); }
        }

        private DateTime? _ActiveTo;
        public DateTime? ActiveTo
        {
            get { return _ActiveTo; }
            set { Set(ref _ActiveTo, value); }
        }

        private DateTime _ActiveFrom;
        public DateTime ActiveFrom
        {
            get { return _ActiveFrom; }
            set { Set(ref _ActiveFrom, value); }
        }

        #region billpackageitem

        private List<SearchOrderItem> _OrderItems;
        public List<SearchOrderItem> OrderItems
        {
            get { return _OrderItems; }
            set { Set(ref _OrderItems, value); }
        }

        private bool _EnableSearchItem = false;
        public bool EnableSearchItem
        {
            get { return _EnableSearchItem; ; }
            set { Set(ref _EnableSearchItem, value); }
        }

        private string _SearchOrderCriteria;
        public string SearchOrderCriteria
        {
            get { return _SearchOrderCriteria; }
            set
            {
                Set(ref _SearchOrderCriteria, value);
                if (!string.IsNullOrEmpty(_SearchOrderCriteria) && _SearchOrderCriteria.Length >= 3)
                {
                    int ownerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
                    //if (SelectOrganisation != null)
                    //{
                    //    ownerOrganisationUID = SelectOrganisation.HealthOrganisationUID;
                    //}
                    OrderItems = DataService.OrderProcessing.SearchOrderItem(_SearchOrderCriteria, ownerOrganisationUID);
                   
                }
                else
                {
                    OrderItems = null;
                }
            }
        }

        private SearchOrderItem _SelectOrderItem;
        public SearchOrderItem SelectOrderItem
        {
            get { return _SelectOrderItem; }
            set
            {
                _SelectOrderItem = value;
                if (_SelectOrderItem != null)
                {
                    ApplyOrderItem(_SelectOrderItem);
                }
            }
        }
        private string _ItemName;
        public string ItemName
        {
            get { return _ItemName; }
            set { Set(ref _ItemName, value); }
        }

        private string _ItemQuantity;
        public string ItemQuantity
        {
            get { return _ItemQuantity; }
            set { Set(ref _ItemQuantity, value); }
        }

        private string _ItemAmount;
        public string ItemAmount
        {
            get { return _ItemAmount; }
            set { Set(ref _ItemAmount, value); 
            
            }
        }

        private DateTime _ItemActiveFrom;
        public DateTime ItemActiveFrom
        {
            get { return _ItemActiveFrom; }
            set { Set(ref _ItemActiveFrom, value); }
        }

        private ObservableCollection<BillPackageDetailModel> _BillPackageDetail;
        public ObservableCollection<BillPackageDetailModel> BillPackageDetail
        {
            get { return _BillPackageDetail ?? (_BillPackageDetail = new ObservableCollection<BillPackageDetailModel>()); }
            //set
            //{
            //    _BillPackageDetail = value;
            //}
            set { Set(ref _BillPackageDetail, value); }
        }

        private BillPackageDetailModel _SelectBillPackageDetail;
        public BillPackageDetailModel SelectBillPackageDetail
        {
            get { return _SelectBillPackageDetail; }
            set { Set(ref _SelectBillPackageDetail, value); 
            if(SelectBillPackageDetail != null)
                {
                    AssignModelToPackagePropertie();
                }
            }
        }

        #endregion

        #region varible
        private BillPackageDetailModel _Item;
        public BillPackageDetailModel Item
        {
            get { return _Item; }
            set { Set(ref _Item, value); }
        }
        #endregion

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

        private RelayCommand _AddBillPackageCommand;
        public RelayCommand AddBillPackageCommand
        {
            get { return _AddBillPackageCommand ?? (_AddBillPackageCommand = new RelayCommand(AddBillPackageDetail)); }
        }
        
        private RelayCommand _EditBillPackageCommand;
        public RelayCommand EditBillPackageCommand
        {
            get { return _EditBillPackageCommand ?? (_EditBillPackageCommand = new RelayCommand(EditBillPackageDetail)); }
        }


        private RelayCommand _DeleteBillPackageCommand;
        public RelayCommand DeleteBillPackageCommand
        {
            get { return _DeleteBillPackageCommand ?? (_DeleteBillPackageCommand = new RelayCommand(DeleteBillPackageDetail)); }
        }

        private RelayCommand _ClearCommand;
        public RelayCommand ClearCommand
        {
            get { return _ClearCommand ?? (_ClearCommand = new RelayCommand(ClearBillPackageDetail)); }
        }

        #endregion

        #region Method
        public ManagePackageViewModel()
        {
            ActiveFrom = DateTime.Now.Date;
            ItemActiveFrom = DateTime.Now.Date;
            Catagory = DataService.MasterData.GetOrderCategory();
            
        }

        void ApplyOrderItem(SearchOrderItem orderItem)
        {
            try
            {
                Item = new BillPackageDetailModel();
                Item.ItemCode = orderItem.Code;
                Item.ItemName = orderItem.ItemName;
                Item.BillableItemUID = orderItem.BillableItemUID;
                Item.ItemUID = orderItem.ItemUID;
                Item.BSMDDUID = orderItem.BillingServiceUID;
                Item.Quantity = 1;
                ItemName = orderItem.ItemName;

                SearchOrderCriteria = "";
                AssingBillPackageDetailToProperties(Item);
            }
            catch (Exception ex)
            {
                ErrorDialog(ex.Message);
            }

        }

        private void AssingBillPackageDetailToProperties(BillPackageDetailModel item)
        {
            ItemName = item.ItemName;
            ItemAmount = item.Amount.ToString();
            ItemQuantity = item.Quantity.ToString();
        }

        private void AddBillPackageDetail()
        {
            if(ItemName == null)
            {
                WarningDialog("กรุณาเลือก item");
                return;
            }
            if (ItemQuantity == null)
            {
                WarningDialog("กรุณาใส่ Quantity");
                return;
            }

            if (ItemAmount == null)
            {
                WarningDialog("กรุณาใส่ Amount");
                return;
            }
            var quantity = int.Parse(ItemQuantity);
            var amount = double.Parse(ItemAmount);
            var total = quantity * amount;

            Item.CURNCUID = 2811;
            Item.Quantity = int.Parse(ItemQuantity);
            Item.Amount = double.Parse(ItemAmount);
            Item.ActiveFrom = ItemActiveFrom;
            Item.TotalAmount = total;

            if (BillPackageDetail != null)
            {
                BillPackageDetailModel data = new BillPackageDetailModel();
                data = Item;
                BillPackageDetail.Add(data);
            }

            CalculateNetAmount();
            ClearBillPackageDetail();
        }

        private void EditBillPackageDetail()
        {
            if (SelectBillPackageDetail == null)
            {
                WarningDialog("กรุณาเลือก item");
            }

            if (SelectBillPackageDetail != null)
            {
                var index = BillPackageDetail.Where(p => p.ItemName == SelectBillPackageDetail.ItemName);
                Item = SelectBillPackageDetail;
                var total = int.Parse(ItemQuantity) * double.Parse(ItemAmount);
                Item.Amount = double.Parse(ItemAmount);
                Item.Quantity = int.Parse(ItemQuantity);
                Item.TotalAmount = total;
                Item.ActiveFrom = ItemActiveFrom;
                //BillPackageDetail[index] = Item;
                BillPackageDetail.Remove(index.FirstOrDefault());
                BillPackageDetail.Add(Item);
                ClearBillPackageDetail();
            }

            CalculateNetAmount();
        }

        private void DeleteBillPackageDetail()
        {
            if (SelectBillPackageDetail == null)
            {
                WarningDialog("กรุณาเลือก item");
                return;
            }

            if(SelectBillPackageDetail != null)
            {
                MessageBoxResult result = QuestionDialog("คุณต้องการลบ item ใช่หรือไม่ ?");
                if (result == MessageBoxResult.Yes)
                {
                    BillPackageDetail.Remove(SelectBillPackageDetail);
                    CollectionViewSource.GetDefaultView(BillPackageDetail).Refresh();
                    CalculateNetAmount();
                }
            }
        }

        private void ClearBillPackageDetail()
        {
            SearchOrderCriteria = "";
            SelectOrderItem = null;
            ItemName = null;
            ItemAmount = null;
            ItemQuantity = null;
            Item = null;
        }

        private void Add()
        {
            if(PackageCode == null)
            {
                WarningDialog("กรุณาใส่ Code");
                return;
            }
            if (PackageName == null)
            {
                WarningDialog("กรุณาใส่ Name");
                return;
            }
            if (BillPackageDetail == null)
            {
                WarningDialog("กรุณาเพิ่มรายการ item");
                return;
            }

            CalculateNetAmount();
            AssingPropertiesToModel();

            DataService.Billing.ManageBillPackage(model, AppUtil.Current.UserID);
            SaveSuccessDialog();

            ListPackage listPage = new ListPackage();
            ChangeView_CloseViewDialog(listPage, ActionDialog.Cancel);
        }

        private void AssignModelToPackagePropertie()
        {
            if (SelectBillPackageDetail != null)
            {
                ItemName = SelectBillPackageDetail.ItemName;
                ItemAmount = SelectBillPackageDetail.Amount.ToString();
                ItemQuantity = SelectBillPackageDetail.Quantity.ToString();
            }
        }

        private void Cancel()
        {
            ListPackage listPage = new ListPackage();
            ChangeView_CloseViewDialog(listPage, ActionDialog.Cancel);
        }

        private void CalculateNetAmount()
        {
            if(BillPackageDetail != null)
            {
                double amount = 0;
                amount = BillPackageDetail.Sum(item => item.TotalAmount);

                TotalAmount = amount.ToString();
            }
        }
        public void AssignModel(BillPackageModel modelData)
        {
            model = modelData;
            AssingModelToProperties();
        }

        public void AssingModelToProperties()
        {
            PackageCode = model.Code;
            PackageName = model.PackageName;
            ActiveFrom = (DateTime)model.ActiveFrom;
            ActiveTo = model.ActiveTo;
            SelectCatagory = model.OrderCategoryUID != null ? Catagory.FirstOrDefault(p => p.OrderCategoryUID == model.OrderCategoryUID) : null;
            SelectOrderSubCatagory = model.OrderSubCategoryUID != null ? OrderSubCatagory.FirstOrDefault(p => p.OrderSubCategoryUID == model.OrderSubCategoryUID) : null;
            TotalAmount = model.TotalAmount.ToString();

            var data = DataService.Billing.GetBillPackageItemByUID(model.BillPackageUID);
           
            if (data != null)
            {

                foreach (var item in data)
                {
                    BillPackageDetail.Add(item);
                }
                CalculateNetAmount();
            }
        }

        public void AssingPropertiesToModel()
        {
            if (model == null)
            {
                model = new BillPackageModel();
            }
            model.BillableItemDetails = new List<BillPackageDetailModel>();

            if (BillPackageDetail != null)
            {
                double? allPrice = BillPackageDetail.Sum(item => item.Amount);
                //model.ReceiptNo = ReceiptNumber;
                model.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
                model.Code = PackageCode;
                model.PackageName = PackageName;
                model.Description = PackageName;
                model.ActiveFrom = ActiveFrom;
                model.ActiveTo = null;
                model.TotalAmount = double.Parse(TotalAmount);
                model.NoofDays = 0;
                model.CURNCUID = 2811;
                //model.OrderCategoryUID = 
                model.BillableItemDetails.AddRange(BillPackageDetail);
            }
        }

        #endregion
    }
}
