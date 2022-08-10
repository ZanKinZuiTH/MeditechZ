using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ManagePolicyMasterViewModel : MediTechViewModelBase
    {
        #region Variable
        int? INCLU, EXCLU;

        private List<BillableItemModel> _deleteBillableItem;

        public List<BillableItemModel> deleteBillableItem
        {
            get { return _deleteBillableItem ?? (_deleteBillableItem = new List<BillableItemModel>()); }
            set { _deleteBillableItem = value; }
        }

        private List<BillingGroupModel> _deletebillingGroup;

        public List<BillingGroupModel> deletebillingGroup
        {
            get { return _deletebillingGroup ?? (_deletebillingGroup = new List<BillingGroupModel>()); }
            set { _deletebillingGroup = value; }
        }

        private List<BillingSubGroupModel> _deletebillingSubGroup;

        public List<BillingSubGroupModel> deletebillingSubGroup
        {
            get { return _deletebillingSubGroup ?? (_deletebillingSubGroup = new List<BillingSubGroupModel>()); }
            set { _deletebillingSubGroup = value; }
        }

        private List<OrderCategoryModel> _deleteOrderSetGroup;

        public List<OrderCategoryModel> deleteOrderSetGroup
        {
            get { return _deleteOrderSetGroup ?? (_deleteOrderSetGroup = new List<OrderCategoryModel>()); }
            set { _deleteOrderSetGroup = value; }
        }

        private List<OrderSubCategoryModel> _deleteOrderSetSubGroup;

        public List<OrderSubCategoryModel> deleteOrderSetSubGroup
        {
            get { return _deleteOrderSetSubGroup ?? (_deleteOrderSetSubGroup = new List<OrderSubCategoryModel>()); }
            set { _deleteOrderSetSubGroup = value; }
        }

        private List<OrderSetModel> _deleteOrderSetItem;

        public List<OrderSetModel> deleteOrderSetItem
        {
            get { return _deleteOrderSetItem ?? (_deleteOrderSetItem = new List<OrderSetModel>()); }
            set { _deleteOrderSetItem = value; }
        }


        private List<OrderCategoryModel> _deletePackageGroup;

        public List<OrderCategoryModel> deletePackageGroup
        {
            get { return _deletePackageGroup ?? (_deletePackageGroup = new List<OrderCategoryModel>()); }
            set { _deletePackageGroup = value; }
        }


        private List<OrderSubCategoryModel> _deletePackageSubGroup;

        public List<OrderSubCategoryModel> deletePackageSubGroup
        {
            get { return _deletePackageSubGroup ?? (_deletePackageSubGroup = new List<OrderSubCategoryModel>()); }
            set { _deletePackageSubGroup = value; }
        }

        private List<BillPackageModel> _deletePackage;

        public List<BillPackageModel> deletePackage
        {
            get { return _deletePackage ?? (_deletePackage = new List<BillPackageModel>()); }
            set { _deletePackage = value; }
        }

        #endregion

        #region Properties

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { Set(ref _Name, value); }
        }

        private string _Code;
        public string Code
        {
            get { return _Code; }
            set { Set(ref _Code, value); }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { Set(ref _Description, value); }
        }
        private bool _IsOrder;
        public bool IsOrder
        {
            get { return _IsOrder; }
            set { Set(ref _IsOrder, value);
                if (IsOrder == true)
                {
                    TabGroup = "Group";
                    TabSubGroup = "SubGroup";
                    TabHeader = "Item";

                    SelectOrderSetCategorySource = null;
                    SelectOrderSetSubCategorySource = null;
                    SelectOrderSetItemSource = null;

                    SelectPackageCategorySource = null;
                    SelectPackageSubCategorySource = null;
                    SelectPackageSource = null;

                    SelectTabIndex = 0;
                }
            }
        }

        private bool _IsOrderSet;
        public bool IsOrderSet
        {
            get { return _IsOrderSet; }
            set { Set(ref _IsOrderSet, value); 
            if(IsOrderSet == true)
                {
                    TabGroup = "Order Category";
                    TabSubGroup = "Order SubCategory";
                    TabHeader = "Order Set";

                    SelectOrderBillGroupSource = null;
                    SelectOrderBillSubGroupSource = null;
                    SelectOrderItemSource = null;

                    SelectPackageCategorySource = null;
                    SelectPackageSubCategorySource = null;
                    SelectPackageSource = null;
                    SelectTabIndex = 0;
                }
            }
        }

        private bool _IsPackage;
        public bool IsPackage
        {
            get { return _IsPackage; }
            set { Set(ref _IsPackage, value);
                if (IsPackage == true)
                {
                    TabGroup = "Order Category";
                    TabSubGroup = "Order SubCategory";
                    TabHeader = "Package";

                    SelectOrderBillGroupSource = null;
                    SelectOrderBillSubGroupSource = null;
                    SelectOrderItemSource = null;

                    SelectOrderSetCategorySource = null;
                    SelectOrderSetSubCategorySource = null;
                    SelectOrderSetItemSource = null;

                    SelectTabIndex = 0;
                }
            }
        }

        private bool _IsExclude = false;
        public bool IsExclude
        {
            get { return _IsExclude; }
            set { Set(ref _IsExclude, value); 
                if(IsExclude == true)
                {
                    IsExcludeText = "ไม่คุ้มครอง";
                }
                else
                {
                    IsExcludeText = "คุ้มครอง";
                }
            }
        }

        private string _IsExcludeText;
        public string IsExcludeText
        {
            get { return _IsExcludeText; }
            set { Set(ref _IsExcludeText, value); }
        }

        private int _SelectTabIndex;
        public int SelectTabIndex
        {
            get { return _SelectTabIndex; }
            set { Set(ref _SelectTabIndex, value); 
            }
        }
        private string _TabGroup;
        public string TabGroup
        {
            get { return _TabGroup; }
            set { Set(ref _TabGroup, value); }
        }

        private string _TabSubGroup;
        public string TabSubGroup
        {
            get { return _TabSubGroup; }
            set { Set(ref _TabSubGroup, value); }
        }

        private string _TabHeader;
        public string TabHeader
        {
            get { return _TabHeader; }
            set { Set(ref _TabHeader, value); }
        }

        #region order 

        private List<BillingGroupModel> _OrderBillGroup;
        public List<BillingGroupModel> OrderBillGroup
        {
            get { return _OrderBillGroup; }
            set { Set(ref _OrderBillGroup, value); }
        }

        private BillingGroupModel _SelectOrderBillGroup;
        public BillingGroupModel SelectOrderBillGroup
        {
            get { return _SelectOrderBillGroup; }
            set
            {
                Set(ref _SelectOrderBillGroup, value);
            }
        }

        private ObservableCollection<BillingGroupModel> _OrderBillGroupSource;
        public ObservableCollection<BillingGroupModel> OrderBillGroupSource
        {
            get { return _OrderBillGroupSource ?? (_OrderBillGroupSource = new ObservableCollection<BillingGroupModel>()); }
            set { Set(ref _OrderBillGroupSource, value); }
        }

        private BillingGroupModel _SelectOrderBillGroupSource;
        public BillingGroupModel SelectOrderBillGroupSource
        {
            get { return _SelectOrderBillGroupSource; }
            set
            {
                Set(ref _SelectOrderBillGroupSource, value);
                if(SelectOrderBillGroupSource != null)
                {
                    OrderBillSubGroup = DataService.Billing.GetBillingSubGroupByGroup(SelectOrderBillGroupSource.BillingGroupUID);
                    
                    OrderItemSource = null;
                    SelectOrderItemSource = null;
                    SelectOrderBillSubGroupSource = null;

                    if (policyMasterModel != null && policyMasterModel.OrderSubGroup != null)
                    {
                        //var d = new ObservableCollection<BillingSubGroupModel>(policyMasterModel.OrderSubGroup);
                        OrderBillSubGroupSource = new ObservableCollection<BillingSubGroupModel>(policyMasterModel.OrderSubGroup.Where(p => p.BillingGroupUID == SelectOrderBillGroupSource.BillingGroupUID));
                    }
                    else
                    {
                        OrderBillSubGroupSource = new ObservableCollection<BillingSubGroupModel>(billingSubGroup.Where(p => p.BillingGroupUID == SelectOrderBillGroupSource.BillingGroupUID));
                    }
                }
            }
        }

        private List<BillingSubGroupModel> _OrderBillSubGroup;
        public List<BillingSubGroupModel> OrderBillSubGroup
        {
            get { return _OrderBillSubGroup; }
            set { Set(ref _OrderBillSubGroup, value); }
        }

        private BillingSubGroupModel _SelectOrderBillSubGroup;
        public BillingSubGroupModel SelectOrderBillSubGroup
        {
            get { return _SelectOrderBillSubGroup; }
            set
            {
                Set(ref _SelectOrderBillSubGroup, value);
            }
        }

        private ObservableCollection<BillingSubGroupModel> _OrderBillSubGroupSource;
        public ObservableCollection<BillingSubGroupModel> OrderBillSubGroupSource
        {
            get { return _OrderBillSubGroupSource ?? (_OrderBillSubGroupSource = new ObservableCollection<BillingSubGroupModel>()); }
            set { Set(ref _OrderBillSubGroupSource, value); }
        }

        private BillingSubGroupModel _SelectOrderBillSubGroupSource;
        public BillingSubGroupModel SelectOrderBillSubGroupSource
        {
            get { return _SelectOrderBillSubGroupSource; }
            set
            {
                Set(ref _SelectOrderBillSubGroupSource, value);
                if (SelectOrderBillSubGroupSource != null)
                {
                    OrderItem = DataService.MasterData.GetBillableItemByBillingSubGroupUID(SelectOrderBillSubGroupSource.BillingSubGroupUID);
                    SelectOrderItemSource = null;

                    if (policyMasterModel != null && policyMasterModel.OrderItem != null)
                    {
                        //var d = new ObservableCollection<BillingSubGroupModel>(policyMasterModel.OrderSubGroup);
                        OrderItemSource = new ObservableCollection<BillableItemModel>(policyMasterModel.OrderItem.Where(p => p.BillingSubGroupUID == SelectOrderBillSubGroupSource.BillingSubGroupUID));
                    }
                    else
                    {
                        OrderItemSource = new ObservableCollection<BillableItemModel>(billableItem.Where(p => p.BillingSubGroupUID == SelectOrderBillSubGroupSource.BillingSubGroupUID));
                    }
                    
                }
            }
        }

        private List<BillableItemModel> _OrderItem;
        public List<BillableItemModel> OrderItem
        {
            get { return _OrderItem; }
            set { Set(ref _OrderItem, value); }
        }

        private BillableItemModel _SelectOrderItem;
        public BillableItemModel SelectOrderItem
        {
            get { return _SelectOrderItem; }
            set
            {
                Set(ref _SelectOrderItem, value);
            }
        }

        private ObservableCollection<BillableItemModel> _OrderItemSource;
        public ObservableCollection<BillableItemModel> OrderItemSource
        {
            get { return _OrderItemSource ?? (_OrderItemSource = new ObservableCollection<BillableItemModel>()); }
            set { Set(ref _OrderItemSource, value); }
        }

        private BillableItemModel _SelectOrderItemSource;
        public BillableItemModel SelectOrderItemSource
        {
            get { return _SelectOrderItemSource; }
            set
            {
                Set(ref _SelectOrderItemSource, value);
                if (_SelectOrderItemSource != null)
                {

                }
            }
        }
        #endregion

        #region order set

        private List<OrderCategoryModel> _OrderSetCategory;
        public List<OrderCategoryModel> OrderSetCategory
        {
            get { return _OrderSetCategory; }
            set { Set(ref _OrderSetCategory, value); }
        }

        private OrderCategoryModel _SelectOrderSetCategory;
        public OrderCategoryModel SelectOrderSetCategory
        {
            get { return _SelectOrderSetCategory; }
            set { Set(ref _SelectOrderSetCategory, value);}
        }

        private ObservableCollection<OrderCategoryModel> _OrderSetCategorySource;
        public ObservableCollection<OrderCategoryModel> OrderSetCategorySource
        {
            get { return _OrderSetCategorySource ?? (_OrderSetCategorySource = new ObservableCollection<OrderCategoryModel>()); }
            set { Set(ref _OrderSetCategorySource, value); }
        }

        private OrderCategoryModel _SelectOrderSetCategorySource;
        public OrderCategoryModel SelectOrderSetCategorySource
        {
            get { return _SelectOrderSetCategorySource; }
            set { Set(ref _SelectOrderSetCategorySource, value); 
            if (SelectOrderSetCategorySource != null)
                {
                    OrderSetSubCategory = DataService.MasterData.GetOrderSubCategoryByUID(SelectOrderSetCategorySource.OrderCategoryUID);
                    OrderSetItemSource = null;
                    SelectOrderSetItemSource = null;
                    SelectOrderSetSubCategorySource = null;

                    if (policyMasterModel != null && policyMasterModel.OrderSetSubGroup != null)
                    {
                        OrderSetSubCategorySource = new ObservableCollection<OrderSubCategoryModel>(policyMasterModel.OrderSetSubGroup.Where(p => p.OrderCategoryUID == SelectOrderSetCategorySource.OrderCategoryUID));
                    }
                    else
                    {
                        OrderSetSubCategorySource = new ObservableCollection<OrderSubCategoryModel>(orderSetSubGroup.Where(p => p.OrderCategoryUID == SelectOrderSetCategorySource.OrderCategoryUID));

                    }

                }
            }
        }

        private List<OrderSubCategoryModel> _OrderSetSubCategory;
        public List<OrderSubCategoryModel> OrderSetSubCategory
        {
            get { return _OrderSetSubCategory; }
            set
            {
                Set(ref _OrderSetSubCategory, value);
            }
        }

        private OrderSubCategoryModel _SelectOrderSetSubCategory;
        public OrderSubCategoryModel SelectOrderSetSubCategory
        {
            get { return _SelectOrderSetSubCategory; }
            set { Set(ref _SelectOrderSetSubCategory, value); }
        }


        private ObservableCollection<OrderSubCategoryModel> _OrderSetSubCategorySource;
        public ObservableCollection<OrderSubCategoryModel> OrderSetSubCategorySource
        {
            get { return _OrderSetSubCategorySource ?? (_OrderSetSubCategorySource = new ObservableCollection<OrderSubCategoryModel>()); }
            set { Set(ref _OrderSetSubCategorySource, value); }
        }

        private OrderSubCategoryModel _SelectOrderSetSubCategorySource;
        public OrderSubCategoryModel SelectOrderSetSubCategorySource
        {
            get { return _SelectOrderSetSubCategorySource; }
            set { Set(ref _SelectOrderSetSubCategorySource, value); 
            if (SelectOrderSetSubCategorySource != null)
                {
                    OrderSetItem = DataService.MasterData.GetOrderSetByOrderSubCategoryUID(SelectOrderSetSubCategorySource.OrderSubCategoryUID);
                    SelectOrderSetItemSource = null;

                    if (policyMasterModel != null && policyMasterModel.OrderSetItem != null)
                    {
                        OrderSetItemSource = new ObservableCollection<OrderSetModel>(policyMasterModel.OrderSetItem.Where(p => p.OrderSubCategoryUID == SelectOrderSetSubCategorySource.OrderSubCategoryUID));
                    }
                    else
                    {
                        OrderSetItemSource = new ObservableCollection<OrderSetModel>(orderSetItem.Where(p => p.OrderSubCategoryUID == SelectOrderSetSubCategorySource.OrderSubCategoryUID));
                    }
                }
            }
        }

        private List<OrderSetModel> _OrderSetItem;
        public List<OrderSetModel> OrderSetItem
        {
            get { return _OrderSetItem; }
            set
            {
                Set(ref _OrderSetItem, value);
            }
        }

        private OrderSetModel _SelectOrderSetItem;
        public OrderSetModel SelectOrderSetItem
        {
            get { return _SelectOrderSetItem; }
            set { Set(ref _SelectOrderSetItem, value); }
        }

        private ObservableCollection<OrderSetModel> _OrderSetItemSource;
        public ObservableCollection<OrderSetModel> OrderSetItemSource
        {
            get { return _OrderSetItemSource ?? (_OrderSetItemSource = new ObservableCollection<OrderSetModel>()); }
            set { Set(ref _OrderSetItemSource, value); }
        }

        private OrderSetModel _SelectOrderSetItemSource;
        public OrderSetModel SelectOrderSetItemSource
        {
            get { return _SelectOrderSetItemSource; }
            set { Set(ref _SelectOrderSetItemSource, value); }
        }
        #endregion

        #region Package
        private List<OrderCategoryModel> _PackageCategory;
        public List<OrderCategoryModel> PackageCategory
        {
            get { return _PackageCategory; }
            set { Set(ref _PackageCategory, value); }
        }

        private OrderCategoryModel _SelectPackageCategory;
        public OrderCategoryModel SelectPackageCategory
        {
            get { return _SelectPackageCategory; }
            set { Set(ref _SelectPackageCategory, value); }
        }

        private ObservableCollection<OrderCategoryModel> _PackageCategorySource;
        public ObservableCollection<OrderCategoryModel> PackageCategorySource
        {
            get { return _PackageCategorySource ?? (_PackageCategorySource = new ObservableCollection<OrderCategoryModel>()); }
            set { Set(ref _PackageCategorySource, value); }
        }

        private OrderCategoryModel _SelectPackageCategorySource;
        public OrderCategoryModel SelectPackageCategorySource
        {
            get { return _SelectPackageCategorySource; }
            set
            {
                Set(ref _SelectPackageCategorySource, value);
                if (SelectPackageCategorySource != null)
                {
                    PackageSubCategory = DataService.MasterData.GetOrderSubCategoryByUID(SelectPackageCategorySource.OrderCategoryUID);
                    PackageSource = null;
                    SelectPackageSource = null;
                    SelectPackageSubCategorySource = null;

                    if (policyMasterModel != null)
                    {
                        if(policyMasterModel.PackageSubGroup != null && policyMasterModel.PackageSubGroup.Count != 0)
                        PackageSubCategorySource = new ObservableCollection<OrderSubCategoryModel>(policyMasterModel.PackageSubGroup.Where(p => p.OrderCategoryUID == SelectPackageCategorySource.OrderCategoryUID));
                        else
                         PackageSubCategorySource = new ObservableCollection<OrderSubCategoryModel>(packageSubGroup.Where(p => p.OrderCategoryUID == SelectPackageCategorySource.OrderCategoryUID));
                    }
                    else
                    {
                        PackageSubCategorySource = new ObservableCollection<OrderSubCategoryModel>(packageSubGroup.Where(p => p.OrderCategoryUID == SelectPackageCategorySource.OrderCategoryUID));
                    }
                }
            }
        }

        private List<OrderSubCategoryModel> _PackageSubCategory;
        public List<OrderSubCategoryModel> PackageSubCategory
        {
            get { return _PackageSubCategory; }
            set { Set(ref _PackageSubCategory, value); }
        }

        private OrderSubCategoryModel _SelectPackageSubCategory;
        public OrderSubCategoryModel SelectPackageSubCategory
        {
            get { return _SelectPackageSubCategory; }
            set { Set(ref _SelectPackageSubCategory, value); }
        }

        private ObservableCollection<OrderSubCategoryModel> _PackageSubCategorySource;
        public ObservableCollection<OrderSubCategoryModel> PackageSubCategorySource
        {
            get { return _PackageSubCategorySource ?? (_PackageSubCategorySource = new ObservableCollection<OrderSubCategoryModel>()); }
            set { Set(ref _PackageSubCategorySource, value); }
        }

        private OrderSubCategoryModel _SelectPackageSubCategorySource;
        public OrderSubCategoryModel SelectPackageSubCategorySource
        {
            get { return _SelectPackageSubCategorySource; }
            set
            {
                Set(ref _SelectPackageSubCategorySource, value);
                if (SelectPackageSubCategorySource != null)
                {
                    Package = DataService.Billing.GetBillPackageByOrderSubCategoryUID(SelectPackageSubCategorySource.OrderSubCategoryUID);
                    SelectPackageSource = null;

                    if (policyMasterModel != null )
                    {
                        if (policyMasterModel.Package != null && policyMasterModel.Package.Count != 0)
                            PackageSource = new ObservableCollection<BillPackageModel>(policyMasterModel.Package.Where(p => p.OrderSubCategoryUID == SelectPackageSubCategorySource.OrderSubCategoryUID));   
                        else
                            PackageSource = new ObservableCollection<BillPackageModel>(package.Where(p => p.OrderSubCategoryUID == SelectPackageSubCategorySource.OrderSubCategoryUID));

                    }
                    else
                    {
                        PackageSource = new ObservableCollection<BillPackageModel>(package.Where(p => p.OrderSubCategoryUID == SelectPackageSubCategorySource.OrderSubCategoryUID));
                    }
                }
            }
        }

        private List<BillPackageModel> _Package;
        public List<BillPackageModel> Package
        {
            get { return _Package; }
            set { Set(ref _Package, value); }
        }

        private BillPackageModel _SelectPackage;
        public BillPackageModel SelectPackage
        {
            get { return _SelectPackage; }
            set { Set(ref _SelectPackage, value); }
        }

        private ObservableCollection<BillPackageModel> _PackageSource;
        public ObservableCollection<BillPackageModel> PackageSource
        {
            get { return _PackageSource ?? (_PackageSource = new ObservableCollection<BillPackageModel>()); }
            set { Set(ref _PackageSource, value); }
        }

        private BillPackageModel _SelectPackageSource;
        public BillPackageModel SelectPackageSource
        {
            get { return _SelectPackageSource; }
            set
            {
                Set(ref _SelectPackageSource, value);
            }
        }

        #endregion

        #endregion

        #region Command
        private RelayCommand _AddCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand ?? (_AddCommand = new RelayCommand(Add));
            }
        }

        private RelayCommand _AddAllCommand;
        public RelayCommand AddAllCommand
        {
            get
            {
                return _AddAllCommand ?? (_AddAllCommand = new RelayCommand(AddAll));
            }
        }
        
       private RelayCommand _DeleteOrderCommand;
        public RelayCommand DeleteOrderCommand
        {
            get
            {
                return _DeleteOrderCommand ?? (_DeleteOrderCommand = new RelayCommand(Delete));
            }
        }


        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save));
            }
        }

        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel));
            }
        }
        #endregion

        #region Method
        PolicyMasterModel policyMasterModel;
        List<BillingSubGroupModel> billingSubGroup = new List<BillingSubGroupModel>();
        List<BillableItemModel> billableItem = new List<BillableItemModel>();

        List<OrderSubCategoryModel> orderSetSubGroup = new List<OrderSubCategoryModel>();
        List<OrderSetModel> orderSetItem = new List<OrderSetModel>();

        List<OrderSubCategoryModel> packageSubGroup = new List<OrderSubCategoryModel>();
        List<BillPackageModel> package = new List<BillPackageModel>();



        public ManagePolicyMasterViewModel()
        {
            IsOrder = true;
            IsExclude = false;
            var agrementType = DataService.Technical.GetReferenceValueMany("AGTYP");
            if (agrementType != null)
            {
                INCLU = agrementType.FirstOrDefault(p => p.ValueCode == "INCLU").Key;
                EXCLU = agrementType.FirstOrDefault(p => p.ValueCode == "EXCLU").Key;
            }
            OrderBillGroup = DataService.Billing.GetBillingGroup();
            OrderSetCategory = DataService.MasterData.GetOrderCategory();
            PackageCategory = DataService.MasterData.GetOrderCategory();
        }

        private void Save()
        {
            if (String.IsNullOrEmpty(Code))
            {
                WarningDialog("กรุณาระบุ Code");
                return;
            }

            if (string.IsNullOrEmpty(Name))
            {
                WarningDialog("กรุณาระบุ Company Name");
                return;
            }

            AssingPropertiesToModel();

            if(deletebillingGroup != null && deletebillingGroup.Count > 0)
            {
                policyMasterModel.OrderGroup.AddRange(deletebillingGroup);
            }

            if (deletebillingSubGroup != null && deletebillingSubGroup.Count > 0)
                policyMasterModel.OrderSubGroup.AddRange(deletebillingSubGroup);
            

            if (deleteBillableItem != null && deleteBillableItem.Count > 0)
                policyMasterModel.OrderItem.AddRange(deleteBillableItem);
            

            if (deleteOrderSetGroup != null && deleteOrderSetGroup.Count > 0)
                policyMasterModel.OrderSetGroup.AddRange(deleteOrderSetGroup);
            

            if (deleteOrderSetSubGroup != null && deleteOrderSetSubGroup.Count > 0)
                policyMasterModel.OrderSetSubGroup.AddRange(deleteOrderSetSubGroup);
            

            if (deleteOrderSetItem != null && deleteOrderSetItem.Count > 0)
                policyMasterModel.OrderSetItem.AddRange(deleteOrderSetItem);
            

            if (deletePackageGroup != null && deletePackageGroup.Count > 0)
                policyMasterModel.PackageGroup.AddRange(deletePackageGroup);
            

            if (deletePackageSubGroup != null && deletePackageSubGroup.Count > 0)
                policyMasterModel.PackageSubGroup.AddRange(deletePackageSubGroup);
            

            if (package != null && package.Count > 0)
                policyMasterModel.Package.AddRange(package);
            

            DataService.Billing.ManagePolicyMaster(policyMasterModel, AppUtil.Current.UserID);
            CloseViewDialog(ActionDialog.Save);

        }

        public void AssingModel(int policyMasterUID)
        {
            policyMasterModel = DataService.Billing.GetPolicyMasterByUID(policyMasterUID);
            AssingModelToProperties(policyMasterModel);
        }
        private void AssingPropertiesToModel()
        {
            if (policyMasterModel == null)
            {
                policyMasterModel = new PolicyMasterModel();

                policyMasterModel.OrderGroup = OrderBillGroupSource != null ? new List<BillingGroupModel>(OrderBillGroupSource) : new List<BillingGroupModel>();
                policyMasterModel.OrderSubGroup = billingSubGroup.Count != 0 ? billingSubGroup : new List<BillingSubGroupModel>();
                policyMasterModel.OrderItem = billableItem.Count != 0 ? billableItem : new List<BillableItemModel>();


                policyMasterModel.OrderSetGroup = OrderSetCategorySource != null ? new List<OrderCategoryModel>(OrderSetCategorySource) : new List<OrderCategoryModel>();
                policyMasterModel.OrderSetSubGroup = orderSetSubGroup.Count != 0 ? orderSetSubGroup : new List<OrderSubCategoryModel>();
                policyMasterModel.OrderSetItem = orderSetItem.Count != 0 ? orderSetItem : new List<OrderSetModel>();

                policyMasterModel.PackageGroup = PackageCategorySource != null ? new List<OrderCategoryModel>(PackageCategorySource) : new List<OrderCategoryModel>();
                policyMasterModel.PackageSubGroup = packageSubGroup.Count != 0 ? packageSubGroup : new List<OrderSubCategoryModel>();
                policyMasterModel.Package = package.Count != 0 ? package : new List<BillPackageModel>();
            }
            else
            {
                policyMasterModel.OrderGroup = OrderBillGroupSource.Count != 0 ? new List<BillingGroupModel>(OrderBillGroupSource) : new List<BillingGroupModel>();
                policyMasterModel.OrderSubGroup = policyMasterModel.OrderSubGroup != null ? policyMasterModel.OrderSubGroup : new List<BillingSubGroupModel>();
                policyMasterModel.OrderItem = policyMasterModel.OrderItem != null ? policyMasterModel.OrderItem : new List<BillableItemModel>();


                policyMasterModel.OrderSetGroup = OrderSetCategorySource.Count != 0 ? new List<OrderCategoryModel>(OrderSetCategorySource) : new List<OrderCategoryModel>();
                policyMasterModel.OrderSetSubGroup = policyMasterModel.OrderSetSubGroup != null ? policyMasterModel.OrderSetSubGroup : new List<OrderSubCategoryModel>();
                policyMasterModel.OrderSetItem = policyMasterModel.OrderSetItem != null ? policyMasterModel.OrderSetItem : new List<OrderSetModel>();

                policyMasterModel.PackageGroup = PackageCategorySource.Count != 0 ? new List<OrderCategoryModel>(PackageCategorySource) : new List<OrderCategoryModel>();
                policyMasterModel.PackageSubGroup = policyMasterModel.PackageSubGroup != null ? policyMasterModel.PackageSubGroup : new List<OrderSubCategoryModel>();
                policyMasterModel.Package = policyMasterModel.Package != null ? policyMasterModel.Package : new List<BillPackageModel>();

            }

            policyMasterModel.Code = Code;
            policyMasterModel.PolicyName = Name;
            policyMasterModel.Description = Description;
            policyMasterModel.OwnweOwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
            policyMasterModel.AGTYPUID = IsExclude == true ? EXCLU : INCLU;

        }

        private void AssingModelToProperties(PolicyMasterModel model)
        {
            Code = model.Code;
            Name = model.PolicyName;
            Description = model.Description;
            IsExclude = (model.AGTYPUID == EXCLU)  ? true : false;
            
            var order = DataService.Billing.GetPolicyOrder(model.PolicyMasterUID);
            
            if (order.OrderGroup != null && order.OrderGroup.Count > 0 )
            {
                policyMasterModel.OrderGroup = order.OrderGroup;
                policyMasterModel.OrderSubGroup = order.OrderSubGroup;
                policyMasterModel.OrderItem = order.OrderItem;

                OrderBillGroupSource = new ObservableCollection<BillingGroupModel>(order.OrderGroup);
            }

            if (order.OrderSetGroup != null && order.OrderSetGroup.Count > 0)
            {
                policyMasterModel.OrderSetGroup = order.OrderSetGroup;
                policyMasterModel.OrderSetSubGroup = order.OrderSetSubGroup;
                policyMasterModel.OrderSetItem = order.OrderSetItem;

                OrderSetCategorySource = new ObservableCollection<OrderCategoryModel>(policyMasterModel.OrderSetGroup);
            }

            if (order.PackageGroup != null && order.PackageGroup.Count > 0)
            {
                policyMasterModel.PackageGroup = order.PackageGroup;
                policyMasterModel.PackageSubGroup = order.PackageSubGroup;
                policyMasterModel.Package = order.Package;

                PackageCategorySource = new ObservableCollection<OrderCategoryModel>(policyMasterModel.PackageGroup);
            }
        }

        private void Add()
        {
            if(IsOrder == true)
            {
                if (SelectTabIndex == 0)
                {
                    if (SelectOrderBillGroup != null)
                    {
                        var Isdata = OrderBillGroupSource.FirstOrDefault(p => p.BillingGroupUID == SelectOrderBillGroup.BillingGroupUID);
                        if(Isdata != null)
                        {
                            WarningDialog("มีรายการนี้อยู่แล้ว กรุณาเลือกรายการใหม่");
                            return;
                        }

                        var data = OrderBillGroup.FirstOrDefault(p => p.BillingGroupUID == SelectOrderBillGroup.BillingGroupUID);
                        if(policyMasterModel != null )
                        {
                            if (policyMasterModel.OrderGroup == null)
                                policyMasterModel.OrderGroup = new List<BillingGroupModel>();

                            policyMasterModel.OrderGroup.Add(data);
                        }
                        OrderBillGroupSource.Add(data);
                        //billingGroup.Add(data);
                        SelectOrderBillGroup = null;
                    }
                }
                else if (SelectTabIndex == 1)
                {
                    if (SelectOrderBillSubGroup != null)
                    {
                        var Isdata = OrderBillSubGroupSource.FirstOrDefault(p => p.BillingSubGroupUID == SelectOrderBillSubGroup.BillingSubGroupUID);
                        if (Isdata != null)
                        {
                            WarningDialog("มีรายการนี้อยู่แล้ว กรุณาเลือกรายการใหม่");
                            return;
                        }

                        var data = OrderBillSubGroup.FirstOrDefault(p => p.BillingSubGroupUID == SelectOrderBillSubGroup.BillingSubGroupUID);
                        if (policyMasterModel != null)
                        {
                            if (policyMasterModel.OrderSubGroup == null)
                                policyMasterModel.OrderSubGroup = new List<BillingSubGroupModel>();

                            policyMasterModel.OrderSubGroup.Add(data);
                        }
                            
                        OrderBillSubGroupSource.Add(data);
                        billingSubGroup.Add(data);
                        SelectOrderBillSubGroup = null;
                    }
                }
                else if (SelectTabIndex == 2)
                {
                    if(SelectOrderItem != null)
                    {
                        var Isdata = OrderItemSource.FirstOrDefault(p => p.BillableItemUID == SelectOrderItem.BillableItemUID);
                        if (Isdata != null)
                        {
                            WarningDialog("มีรายการนี้อยู่แล้ว กรุณาเลือกรายการใหม่");
                            return;
                        }

                        var data = OrderItem.FirstOrDefault(p => p.BillableItemUID == SelectOrderItem.BillableItemUID);
                        if (policyMasterModel != null)
                        {
                            if (policyMasterModel.OrderItem == null)
                                policyMasterModel.OrderItem = new List<BillableItemModel>();

                            policyMasterModel.OrderItem.Add(data);
                        }
                        
                        OrderItemSource.Add(data);
                        billableItem.Add(data);
                        SelectOrderItem = null;
                    }
                }
            }

            if (IsOrderSet == true)
            {
                if (SelectTabIndex == 0)
                {
                    if (SelectOrderSetCategory != null)
                    {
                        var Isdata = OrderSetCategorySource.FirstOrDefault(p => p.OrderCategoryUID == SelectOrderSetCategory.OrderCategoryUID);
                        if (Isdata != null)
                        {
                            WarningDialog("มีรายการนี้อยู่แล้ว กรุณาเลือกรายการใหม่");
                            return;
                        }

                        var data = OrderSetCategory.FirstOrDefault(p => p.OrderCategoryUID == SelectOrderSetCategory.OrderCategoryUID);
                        if (policyMasterModel != null)
                        {
                            if (policyMasterModel.OrderSetGroup == null)
                                policyMasterModel.OrderSetGroup = new List<OrderCategoryModel>();

                                policyMasterModel.OrderSetGroup.Add(data);
                        }
                        OrderSetCategorySource.Add(data);
                        SelectOrderSetCategory = null;
                    }
                }
                else if (SelectTabIndex == 1)
                {
                    if(SelectOrderSetSubCategory != null)
                    {
                        var Isdata = OrderSetSubCategorySource.FirstOrDefault(p => p.OrderSubCategoryUID == SelectOrderSetSubCategory.OrderSubCategoryUID);
                        if (Isdata != null)
                        {
                            WarningDialog("มีรายการนี้อยู่แล้ว กรุณาเลือกรายการใหม่");
                            return;
                        }

                        var data = OrderSetSubCategory.FirstOrDefault(p => p.OrderSubCategoryUID == SelectOrderSetSubCategory.OrderSubCategoryUID);
                        if (policyMasterModel != null )
                        {
                            if (policyMasterModel.OrderSetSubGroup == null)
                                policyMasterModel.OrderSetSubGroup = new List<OrderSubCategoryModel>();

                            policyMasterModel.OrderSetSubGroup.Add(data);
                        }
                        OrderSetSubCategorySource.Add(data);
                        orderSetSubGroup.Add(data);
                        SelectOrderSetSubCategory = null;
                    }

                }
                else if (SelectTabIndex == 2)
                {
                    if(SelectOrderSetItem != null)
                    {
                        var Isdata = OrderSetItemSource.FirstOrDefault(p => p.OrderSetUID == SelectOrderSetItem.OrderSetUID);
                        if (Isdata != null)
                        {
                            WarningDialog("มีรายการนี้อยู่แล้ว กรุณาเลือกรายการใหม่");
                            return;
                        }

                        var data = OrderSetItem.FirstOrDefault(p => p.OrderSetUID == SelectOrderSetItem.OrderSetUID);
                        if (policyMasterModel != null )
                        {
                            if (policyMasterModel.OrderSetItem == null)
                                policyMasterModel.OrderSetItem = new List<OrderSetModel>();

                            policyMasterModel.OrderSetItem.Add(data);
                        }
                        OrderSetItemSource.Add(data);
                        orderSetItem.Add(data);
                        SelectOrderSetItem = null;
                    }
                }
            }

            if (IsPackage == true)
            {
                if (SelectTabIndex == 0)
                {
                    if (SelectPackageCategory != null)
                    {
                        var Isdata = PackageCategorySource.FirstOrDefault(p => p.OrderCategoryUID == SelectPackageCategory.OrderCategoryUID);
                        if (Isdata != null)
                        {
                            WarningDialog("มีรายการนี้อยู่แล้ว กรุณาเลือกรายการใหม่");
                            return;
                        }
                        var data = PackageCategory.FirstOrDefault(p => p.OrderCategoryUID == SelectPackageCategory.OrderCategoryUID);
                        if (policyMasterModel != null)
                        {
                            if (policyMasterModel.PackageGroup == null)
                                policyMasterModel.PackageGroup = new List<OrderCategoryModel>();

                            policyMasterModel.PackageGroup.Add(data);
                        }
                        PackageCategorySource.Add(data);
                        SelectPackageCategory = null;
                    }
                }
                else if (SelectTabIndex == 1)
                {
                    if (SelectPackageSubCategory != null)
                    {
                        var Isdata = PackageSubCategorySource.FirstOrDefault(p => p.OrderSubCategoryUID == SelectPackageSubCategory.OrderSubCategoryUID);
                        if (Isdata != null)
                        {
                            WarningDialog("มีรายการนี้อยู่แล้ว กรุณาเลือกรายการใหม่");
                            return;
                        }
                        var data = PackageSubCategory.FirstOrDefault(p => p.OrderSubCategoryUID == SelectPackageSubCategory.OrderSubCategoryUID);
                        if (policyMasterModel != null)
                        {
                            if (policyMasterModel.PackageSubGroup == null)
                                policyMasterModel.PackageSubGroup = new List<OrderSubCategoryModel>();

                            policyMasterModel.PackageSubGroup.Add(data);
                        }
                        PackageSubCategorySource.Add(data);
                        packageSubGroup.Add(data);
                        SelectPackageSubCategory = null;
                    }
                }
                else if (SelectTabIndex == 2)
                {
                    if (SelectPackage != null)
                    {
                        var Isdata = PackageSource.FirstOrDefault(p => p.BillPackageUID == SelectPackage.BillPackageUID);
                        if (Isdata != null)
                        {
                            WarningDialog("มีรายการนี้อยู่แล้ว กรุณาเลือกรายการใหม่");
                            return;
                        }

                        var data = Package.FirstOrDefault(p => p.BillPackageUID == SelectPackage.BillPackageUID);
                        if (policyMasterModel != null)
                        {
                            if (policyMasterModel.Package == null)
                                policyMasterModel.Package = new List<BillPackageModel>();
                            policyMasterModel.Package.Add(data);
                        }
                        PackageSource.Add(data);
                        package.Add(data);
                        SelectPackage = null;
                    }
                }
            }
        }

        private void Delete()
        {
            if (IsOrder == true)
            {
                if (SelectTabIndex == 0)
                {
                    if (SelectOrderBillGroupSource != null)
                    {
                        var d = SelectOrderBillGroupSource;
                        var value = OrderBillSubGroupSource.Where(p => p.BillingGroupUID == SelectOrderBillGroupSource.BillingGroupUID).ToList();
                        if (value != null)
                        {
                            foreach (var item in value)
                            {
                                var valueDetail = OrderItemSource.Where(p => p.BillingSubGroupUID == item.BillingSubGroupUID).ToList();
                                if (valueDetail != null)
                                {
                                    foreach (var itemDetail in valueDetail)
                                    {
                                        if (policyMasterModel.OrderItem != null && policyMasterModel.OrderItem.Count != 0)
                                        {
                                            if (itemDetail.ContactAgreementAccountItemUID != null)
                                            {
                                                itemDetail.StatusFlag = "D";

                                                deleteBillableItem.Add(itemDetail);
                                            }
                                            policyMasterModel.OrderItem.Remove(itemDetail);
                                        }
                                        OrderItemSource.Remove(itemDetail);
                                    }
                                }

                                if (policyMasterModel.OrderSubGroup != null && policyMasterModel.OrderSubGroup.Count != 0)
                                {
                                    if (item.ContactAgreementAccountDetailUID != null)
                                    {
                                        item.StatusFlag = "D";


                                        deletebillingSubGroup.Add(item);
                                    }
                                    policyMasterModel.OrderSubGroup.Remove(item);
                                }
                                OrderBillSubGroupSource.Remove(item);
                            }
                        }
                        OrderBillGroupSource.Remove(SelectOrderBillGroupSource);

                        if (policyMasterModel.OrderGroup != null && policyMasterModel.OrderGroup.Count != 0)
                        {
                            policyMasterModel.OrderGroup.Remove(SelectOrderBillGroupSource);
                            if (d.ContactAgreementAccountUID != null)
                            {
                                d.StatusFlag = "D";
   

                                deletebillingGroup.Add(d);
                            }
                        }
                    }
                }
                else if (SelectTabIndex == 1)
                {
                    if (SelectOrderBillSubGroupSource != null)
                    {
                        var d = SelectOrderBillSubGroupSource;
                        var value = OrderItemSource.Where(p => p.BillingSubGroupUID == SelectOrderBillSubGroupSource.BillingSubGroupUID).ToList();
                        if(value != null) 
                        {
                            foreach (var item in value)
                            {
                                if (policyMasterModel.OrderItem != null && policyMasterModel.OrderItem.Count != 0)
                                {
                                    if (item.ContactAgreementAccountItemUID != null)
                                    {
                                        item.StatusFlag = "D";

                                        deleteBillableItem.Add(item);
                                    }
                                    policyMasterModel.OrderItem.Remove(item);
                                }
                                OrderItemSource.Remove(item);
                            }
                        }
                        OrderBillSubGroupSource.Remove(SelectOrderBillSubGroupSource);

                        if (policyMasterModel.OrderSubGroup != null && policyMasterModel.OrderSubGroup.Count != 0)
                        {
                            policyMasterModel.OrderSubGroup.Remove(SelectOrderBillSubGroupSource);
                            if (d.ContactAgreementAccountDetailUID != null)
                            {
                                d.StatusFlag = "D";
                                deletebillingSubGroup.Add(d);
                            }
                        }
                    }
                }
                else if (SelectTabIndex == 2)
                {
                    if (SelectOrderItemSource != null)
                    {
                        var d = SelectOrderItemSource;
                        OrderItemSource.Remove(SelectOrderItemSource);
                        if (policyMasterModel.OrderItem != null && policyMasterModel.OrderItem.Count != 0)
                        {
                            policyMasterModel.OrderItem.Remove(d);
                            if (d.ContactAgreementAccountItemUID != null)
                            {
                                d.StatusFlag = "D";

                                deleteBillableItem.Add(d);
                            }
                        }
                    }
                }
            }
            if (IsOrderSet == true)
                {
                    if (SelectTabIndex == 0)
                    {
                        if (SelectOrderSetCategorySource != null)
                        {
                        var d = SelectOrderSetCategorySource;
                            var value = OrderSetSubCategorySource.Where(p => p.OrderCategoryUID == SelectOrderSetCategorySource.OrderCategoryUID).ToList();
                            if (value != null)
                            {
                                foreach (var item in value)
                                {
                                    var valueDetail = orderSetItem.Where(p => p.OrderSubCategoryUID == item.OrderSubCategoryUID).ToList();
                                    if (valueDetail != null)
                                    {
                                        foreach (var itemDetail in valueDetail)
                                        {
                                            if (policyMasterModel.OrderSetItem != null && policyMasterModel.OrderSetItem.Count != 0)
                                            {
                                                if (itemDetail.ContactAgreementAccountItemUID != null)
                                                {
                                                    itemDetail.StatusFlag = "D";

                                                    deleteOrderSetItem.Add(itemDetail);
                                                }
                                                policyMasterModel.OrderSetItem.Remove(itemDetail);
                                            }
                                            OrderSetItemSource.Remove(itemDetail);
                                        }
                                    }

                                    if (policyMasterModel.OrderSetSubGroup != null && policyMasterModel.OrderSetSubGroup.Count != 0)
                                    {
                                        if (item.ContactAgreementAccountDetailUID != null)
                                        {
                                            item.StatusFlag = "D";

                                            deleteOrderSetSubGroup.Add(item);
                                        }
                                        policyMasterModel.OrderSetSubGroup.Remove(item);
                                    }
                                    OrderSetSubCategorySource.Remove(item);
                                }
                            }
                            OrderSetCategorySource.Remove(SelectOrderSetCategorySource);

                            if (policyMasterModel.OrderSetGroup != null && policyMasterModel.OrderSetGroup.Count != 0)
                            {
                                policyMasterModel.OrderSetGroup.Remove(SelectOrderSetCategorySource);
                                if (d.ContactAgreementAccountUID != null)
                                {
                                    d.StatusFlag = "D";

                                    deleteOrderSetGroup.Add(d);
                                }
                            }
                        }
                    }
                    else if (SelectTabIndex == 1)
                    {
                        if (SelectOrderSetSubCategorySource != null)
                        {
                        var d = SelectOrderSetSubCategorySource;

                        var valueDetail = orderSetItem.Where(p => p.OrderSubCategoryUID == SelectOrderSetSubCategorySource.OrderSubCategoryUID).ToList();
                        if (valueDetail != null)
                        {
                            foreach (var itemDetail in valueDetail)
                            {
                                if (policyMasterModel.OrderSetItem != null && policyMasterModel.OrderSetItem.Count != 0)
                                {
                                    if (itemDetail.ContactAgreementAccountItemUID != null)
                                    {
                                        itemDetail.StatusFlag = "D";

                                        deleteOrderSetItem.Add(itemDetail);
                                    }
                                    policyMasterModel.OrderSetItem.Remove(itemDetail);
                                }
                                OrderSetItemSource.Remove(itemDetail);
                            }
                        }

                        OrderSetSubCategorySource.Remove(SelectOrderSetSubCategorySource);

                            if (policyMasterModel.OrderSetSubGroup != null && policyMasterModel.OrderSetSubGroup.Count != 0)
                            {
                                policyMasterModel.OrderSetSubGroup.Remove(SelectOrderSetSubCategorySource);
                                if (d.ContactAgreementAccountDetailUID != null)
                                {
                                    d.StatusFlag = "D";

                                    deleteOrderSetSubGroup.Add(d);
                                }
                            }
                        }
                    }
                    else if (SelectTabIndex == 2)
                    {
                        if (SelectOrderSetItemSource != null)
                        {
                        var d = SelectOrderSetItemSource;
                        OrderSetItemSource.Remove(SelectOrderSetItemSource);

                            if (policyMasterModel.OrderSetItem != null && policyMasterModel.OrderSetItem.Count != 0)
                            {
                                policyMasterModel.OrderSetItem.Remove(SelectOrderSetItemSource);
                                if (d.ContactAgreementAccountItemUID != null)
                                {
                                    d.StatusFlag = "D";

                                    deleteOrderSetItem.Add(d);
                                }
                            }
                        }
                    }
                }

            if (IsPackage == true)
                {
                    if (SelectTabIndex == 0)
                    {
                        if (SelectPackageCategorySource != null)
                        {
                            var d = SelectPackageCategorySource;
                            var value = packageSubGroup.Where(p => p.OrderCategoryUID == SelectPackageCategorySource.OrderCategoryUID).ToList();
                            if (value != null)
                            {
                                foreach (var item in value)
                                {
                                    var valueDetail = PackageSource.Where(p => p.OrderSubCategoryUID == SelectPackageSubCategorySource.OrderSubCategoryUID).ToList();
                                    if (valueDetail != null)
                                    {
                                        foreach (var itemDetail in valueDetail)
                                        {
                                            if (policyMasterModel.Package != null && policyMasterModel.Package.Count != 0)
                                            {
                                                if (itemDetail.ContactAgreementAccountItemUID != null)
                                                {
                                                    itemDetail.StatusFlag = "D";

                                                    deletePackage.Add(itemDetail);
                                                }
                                                policyMasterModel.Package.Remove(itemDetail);
                                            }
                                            PackageSource.Remove(itemDetail);
                                        }
                                    }

                                if (policyMasterModel.OrderSetSubGroup != null && policyMasterModel.OrderSetSubGroup.Count != 0)
                                    {
                                        if (item.ContactAgreementAccountDetailUID != null)
                                        {
                                            item.StatusFlag = "D";

                                            deleteOrderSetSubGroup.Add(item);
                                        }
                                        policyMasterModel.OrderSetSubGroup.Remove(item);
                                    }
                                    OrderSetSubCategorySource.Remove(item);
                                }
                            }
                            PackageCategorySource.Remove(SelectPackageCategorySource);
                            if (policyMasterModel.PackageGroup != null && policyMasterModel.PackageGroup.Count != 0)
                            {
                                policyMasterModel.PackageGroup.Remove(d);
                                if (d.ContactAgreementAccountUID != null)
                                {
                                    d.StatusFlag = "D";

                                    deletePackageGroup.Add(d);
                                }
                            }
                        }
                    }
                    else if (SelectTabIndex == 1)
                    {
                        if (SelectPackageSubCategorySource != null)
                        {
                            var d = SelectPackageSubCategorySource;
                            var valueDetail = PackageSource.Where(p => p.OrderSubCategoryUID == SelectPackageSubCategorySource.OrderSubCategoryUID).ToList();
                            if (valueDetail != null)
                            {
                                foreach (var itemDetail in valueDetail)
                                {
                                    if (policyMasterModel.Package != null && policyMasterModel.Package.Count != 0)
                                    {
                                        if (itemDetail.ContactAgreementAccountItemUID != null)
                                        {
                                            itemDetail.StatusFlag = "D";

                                            deletePackage.Add(itemDetail);
                                        }
                                        policyMasterModel.Package.Remove(itemDetail);
                                    }
                                    PackageSource.Remove(itemDetail);
                                }
                            }

                            PackageSubCategorySource.Remove(SelectPackageSubCategorySource);
                            if (policyMasterModel.PackageSubGroup != null && policyMasterModel.PackageSubGroup.Count != 0)
                            {
                                policyMasterModel.PackageSubGroup.Remove(d);
                                if (d.ContactAgreementAccountDetailUID != null)
                                {
                                    d.StatusFlag = "D";

                                    deletePackageSubGroup.Add(d);
                                }
                            }
                        }
                    }
                    else if (SelectTabIndex == 2)
                    {
                        if (SelectPackageSource != null)
                        {
                            var d = SelectPackageSource;
                            PackageSource.Remove(SelectPackageSource);
                            if (policyMasterModel.Package != null && policyMasterModel.Package.Count != 0)
                            {
                                policyMasterModel.Package.Remove(d);
                                if (d.ContactAgreementAccountItemUID != null)
                                {
                                    d.StatusFlag = "D";

                                    deletePackage.Add(d);
                                }
                            }
                        }
                    }
                }
        }

        private void AddAll()
        {
            
        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        #endregion
    }
}
