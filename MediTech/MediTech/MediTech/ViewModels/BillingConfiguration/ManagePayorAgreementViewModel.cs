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
    public class ManagePayorAgreementViewModel : MediTechViewModelBase
    {
        #region Properties

        #region Detail
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

        private List<LookupReferenceValueModel> _BillType;
        public List<LookupReferenceValueModel> BillType
        {
            get { return _BillType; }
            set { Set(ref _BillType, value); }
        }

        private LookupReferenceValueModel _SelectBillType;
        public LookupReferenceValueModel SelectBillType
        {
            get { return _SelectBillType; }
            set { Set(ref _SelectBillType, value); }
        }

        private List<LookupReferenceValueModel> _PayorBillType;
        public List<LookupReferenceValueModel> PayorBillType
        {
            get { return _PayorBillType; }
            set { Set(ref _PayorBillType, value); }
        }

        private LookupReferenceValueModel _SelectPayorBillType;
        public LookupReferenceValueModel SelectPayorBillType
        {
            get { return _SelectPayorBillType; }
            set { Set(ref _SelectPayorBillType, value); }
        }

        private List<LookupReferenceValueModel> _CreditTerm;
        public List<LookupReferenceValueModel> CreditTerm
        {
            get { return _CreditTerm; }
            set { Set(ref _CreditTerm, value); }
        }

        private LookupReferenceValueModel _SelectCreditTerm;
        public LookupReferenceValueModel SelectCreditTerm
        {
            get { return _SelectCreditTerm; }
            set { Set(ref _SelectCreditTerm, value); }
        }

        private bool _IsLimitAfterDiscount;
        public bool IsLimitAfterDiscount
        {
            get { return _IsLimitAfterDiscount; }
            set { Set(ref _IsLimitAfterDiscount, value); }
        }

        private string _CoverPerDay;
        public string CoverPerDay
        {
            get { return _CoverPerDay; }
            set { Set(ref _CoverPerDay, value); }
        }

        private string _ClaimPercentage;
        public string ClaimPercentage
        {
            get { return _ClaimPercentage; }
            set { Set(ref _ClaimPercentage, value); }
        }

        private List<LookupReferenceValueModel> _PrimaryTariff;
        public List<LookupReferenceValueModel> PrimaryTariff
        {
            get { return _PrimaryTariff; }
            set { Set(ref _PrimaryTariff, value); }
        }

        private LookupReferenceValueModel _SelectPrimaryTariff;
        public LookupReferenceValueModel SelectPrimaryTariff
        {
            get { return _SelectPrimaryTariff; }
            set { Set(ref _SelectPrimaryTariff, value); 
            }
        }

        private List<LookupReferenceValueModel> _SecondaryTariff;
        public List<LookupReferenceValueModel> SecondaryTariff
        {
            get { return _SecondaryTariff; }
            set { Set(ref _SecondaryTariff, value); }
        }

        private LookupReferenceValueModel _SelectSecondaryTariff;
        public LookupReferenceValueModel SelectSecondaryTariff
        {
            get { return _SelectSecondaryTariff; }
            set { Set(ref _SelectSecondaryTariff, value); }
        }

        private List<LookupReferenceValueModel> _TertiaryTariff;
        public List<LookupReferenceValueModel> TertiaryTariff
        {
            get { return _TertiaryTariff; }
            set { Set(ref _TertiaryTariff, value); }
        }

        private LookupReferenceValueModel _SelectTertiaryTariff;
        public LookupReferenceValueModel SelectTertiaryTariff
        {
            get { return _SelectTertiaryTariff; }
            set { Set(ref _SelectTertiaryTariff, value); }
        }

        private List<PolicyMasterModel> _PolicyMaster;
        public List<PolicyMasterModel> PolicyMaster
        {
            get { return _PolicyMaster; }
            set { Set(ref _PolicyMaster, value); }
        }

        private PolicyMasterModel _SelectPolicyMaster;
        public PolicyMasterModel SelectPolicyMaster
        {
            get { return _SelectPolicyMaster; }
            set { Set(ref _SelectPolicyMaster, value); 
                if(SelectPolicyMaster != null)
                {
                    SelectBillingGroupSource = null;
                    SelectBillingSubGroupSource = null;
                    SelectItemSource = null;
                    BillingGroupSource = null;
                    BillingSubGroupSource = null;
                    ItemSource = null;


                    AgreementType = SelectPolicyMaster.AgreementType;

                    var policy = DataService.Billing.GetPolicyForPayorAgreement(SelectPolicyMaster.PolicyMasterUID);

                    if (policy.OrderGroup != null && policy.OrderGroup.Count != 0)
                    {
                        if (billingGroups == null)
                            billingGroups = new List<BillingGroupModel>();

                            billingGroups = policy.OrderGroup;

                        if (policy.OrderSubGroup != null)
                        {
                            if(billingSubGroups == null)
                            billingSubGroups = new List<BillingSubGroupModel>();

                            billingSubGroups = policy.OrderSubGroup;
                        }

                        if (policy.OrderItem != null)
                        {
                            if (billableItems == null)
                                billableItems = new List<BillableItemModel>();

                            billableItems = policy.OrderItem;
                        }

                        BillingGroupSource = new ObservableCollection<BillingGroupModel>(billingGroups);
                    }
                }
                else
                {
                    AgreementType = null;
                }
            }
        }

        private string _AgreementType;
        public string AgreementType
        {
            get { return _AgreementType; }
            set { Set(ref _AgreementType, value); }
        }

        private int _InsuranceCompanyID;
        public int InsuranceCompanyID
        {
            get { return _InsuranceCompanyID; }
            set
            {
                Set(ref _InsuranceCompanyID, value);
            }
        }
        private int _SelectContactIndex;
        public int SelectContactIndex
        {
            get { return _SelectContactIndex; }
            set { Set(ref _SelectContactIndex, value); }
        }

        private ObservableCollection<BillingGroupModel> _BillingGroupSource;
        public ObservableCollection<BillingGroupModel> BillingGroupSource
        {
            get { return _BillingGroupSource ?? (_BillingGroupSource = new ObservableCollection<BillingGroupModel>()); }
            set { Set(ref _BillingGroupSource, value); }
        }

        private BillingGroupModel _SelectBillingGroupSource;
        public BillingGroupModel SelectBillingGroupSource
        {
            get { return _SelectBillingGroupSource; }
            set { Set(ref _SelectBillingGroupSource, value); 
            if(SelectBillingGroupSource != null)
                {
                    ItemSource = null;
                    BillingSubGroupSource = new ObservableCollection<BillingSubGroupModel>(billingSubGroups.Where(p => p.BillingGroupUID == SelectBillingGroupSource.BillingGroupUID));
                }
            }
        }

        private ObservableCollection<BillingSubGroupModel> _BillingSubGroupSource;
        public ObservableCollection<BillingSubGroupModel> BillingSubGroupSource
        {
            get { return _BillingSubGroupSource ?? (_BillingSubGroupSource = new ObservableCollection<BillingSubGroupModel>()); }
            set { Set(ref _BillingSubGroupSource, value); }
        }

        private BillingSubGroupModel _SelectBillingSubGroupSource;
        public BillingSubGroupModel SelectBillingSubGroupSource
        {
            get { return _SelectBillingSubGroupSource; }
            set { Set(ref _SelectBillingSubGroupSource, value); 
            if(SelectBillingSubGroupSource != null)

                ItemSource = new ObservableCollection<BillableItemModel>(billableItems.Where(p => p.BillingSubGroupUID == SelectBillingSubGroupSource.BillingSubGroupUID));
 
            }
        }

        private ObservableCollection<BillableItemModel> _ItemSource;
        public ObservableCollection<BillableItemModel> ItemSource
        {
            get { return _ItemSource ?? (ItemSource = new ObservableCollection<BillableItemModel>()); }
            set { Set(ref _ItemSource, value); }
        }

        private BillableItemModel _SelectItemSource;
        public BillableItemModel SelectItemSource
        {
            get { return _SelectItemSource; }
            set { Set(ref _SelectItemSource, value); }
        }

        #endregion

        #region Discount

        private int _SelectTabIndex;
        public int SelectTabIndex
        {
            get { return _SelectTabIndex; }
            set { Set(ref _SelectTabIndex, value); }
        }

        private List<LookupReferenceValueModel> _Tariff;
        public List<LookupReferenceValueModel> Tariff
        {
            get { return _Tariff; }
            set { Set(ref _Tariff, value); }
        }

        private LookupReferenceValueModel _SelectTariff;
        public LookupReferenceValueModel SelectTariff
        {
            get { return _SelectTariff; }
            set
            {
                Set(ref _SelectTariff, value);
                if (SelectTariff != null)
                {
                    GroupName = DataService.Billing.GetBillingGroup();

                    if (agreementAccounts != null)
                    {
                        SelectDiscountItemSource = null;
                        SelectDiscountSubGroupSource = null;
                        DiscountItemSource = null;
                        DiscountSubGroupSource = null;
                        SelectDiscountGroupSource = null;

                        ClearGroup();
                        ClearSubGroup();
                        ClearItem();

                        ItemSource = null;
                        ItemName = null;

                        DiscountGroupSource = new ObservableCollection<AgreementAccountDiscountModel>(agreementAccounts.Where(p => p.PBLCTUID == SelectTariff.Key));
                    }
                }
            }
        }


        #region Group
        private bool? _IsSelectGroup;
        public bool? IsSelectGroup
        {
            get { return _IsSelectGroup ?? (IsSelectGroup = false); }
            set { Set(ref _IsSelectGroup, value); }
        }

        private List<BillingGroupModel> _GroupName;
        public List<BillingGroupModel> GroupName
        {
            get { return _GroupName; }
            set { Set(ref _GroupName, value); }
        }

        private BillingGroupModel _SelectGroupName;
        public BillingGroupModel SelectGroupName
        { 
            get { return _SelectGroupName; }
            set { Set(ref _SelectGroupName, value);
            }
        }
        
        private double? _GroupDiscount;
        public double? GroupDiscount
        {
            get { return _GroupDiscount; }
            set { Set(ref _GroupDiscount, value); }
        }

        private bool _GroupIsPer;
        public bool GroupIsPer
        {
            get { return _GroupIsPer; }
            set { Set(ref _GroupIsPer, value); }
        }

        private List<LookupReferenceValueModel> _GroupAllowDiscount;
        public List<LookupReferenceValueModel> GroupAllowDiscount
        {
            get { return _GroupAllowDiscount; }
            set { Set(ref _GroupAllowDiscount, value); }
        }

        private LookupReferenceValueModel _SelectGroupAllowDiscount;
        public LookupReferenceValueModel SelectGroupAllowDiscount
        {
            get { return _SelectGroupAllowDiscount; }
            set { Set(ref _SelectGroupAllowDiscount, value); }
        }

        private DateTime? _GroupDateFrom;
        public DateTime? GroupDateFrom
        {
            get { return _GroupDateFrom; }
            set { Set(ref _GroupDateFrom, value); }
        }

        private DateTime? _GroupDateTo;
        public DateTime? GroupDateTo
        {
            get { return _GroupDateTo; }
            set { Set(ref _GroupDateTo, value); }
        }

        private ObservableCollection<AgreementAccountDiscountModel> _DiscountGroupSource;
        public ObservableCollection<AgreementAccountDiscountModel> DiscountGroupSource
        {
            get { return _DiscountGroupSource ?? (DiscountGroupSource = new ObservableCollection<AgreementAccountDiscountModel>()); }
            set { Set(ref _DiscountGroupSource, value); }
        }

        private AgreementAccountDiscountModel _SelectDiscountGroupSource;
        public AgreementAccountDiscountModel SelectDiscountGroupSource
        {
            get { return _SelectDiscountGroupSource; }
            set
            {
                Set(ref _SelectDiscountGroupSource, value);
                if (SelectDiscountGroupSource != null)
                {
                    IsSelectGroup = true;

                    SubGroupName = DataService.Billing.GetBillingSubGroupByGroup(SelectDiscountGroupSource.ServiceUID);
                    
                    SelectDiscountSubGroupSource = null;
                    SelectDiscountItemSource = null;
                    DiscountItemSource = null;
                    DiscountSubGroupSource = null;

                    if (agreementDetails != null)
                    {
                        DiscountSubGroupSource = new ObservableCollection<AgreementDetailDiscountModel>(agreementDetails.Where(p => p.BillgGroupUID == SelectDiscountGroupSource.ServiceUID));
                    }

                    SelectGroupName = GroupName.FirstOrDefault(p => p.BillingGroupUID == SelectDiscountGroupSource.ServiceUID);
                    GroupDiscount = SelectDiscountGroupSource.Discount;
                    GroupIsPer = SelectDiscountGroupSource.IsPercentage == "Y" ? true : false;
                    SelectGroupAllowDiscount = GroupAllowDiscount.FirstOrDefault(p => p.Key == SelectDiscountGroupSource.ALLDIUID);
                    GroupDateFrom = SelectDiscountGroupSource.DateFrom;
                    GroupDateTo = SelectDiscountGroupSource.DateTo;
                }
                else
                {
                    IsSelectGroup = false;
                }
            }
        }

        #endregion

        #region SubGroup
        private bool? _IsSelectSubGroup;
        public bool? IsSelectSubGroup
        {
            get { return _IsSelectSubGroup ?? (IsSelectSubGroup = false); }
            set { Set(ref _IsSelectSubGroup, value); }
        }

        private List<BillingSubGroupModel> _SubGroupName;
        public List<BillingSubGroupModel> SubGroupName
        {
            get { return _SubGroupName; }
            set { Set(ref _SubGroupName, value); }
        }

        private BillingSubGroupModel _SelectSubGroupName;
        public BillingSubGroupModel SelectSubGroupName
        {
            get { return _SelectSubGroupName; }
            set
            {
                Set(ref _SelectSubGroupName, value);
            }
        }

        private double? _SubGroupDiscount;
        public double? SubGroupDiscount
        {
            get { return _SubGroupDiscount; }
            set { Set(ref _SubGroupDiscount, value); }
        }

        private bool _SubGroupIsPer;
        public bool SubGroupIsPer
        {
            get { return _SubGroupIsPer; }
            set { Set(ref _SubGroupIsPer, value); }
        }

        private List<LookupReferenceValueModel> _SubGroupAllowDiscount;
        public List<LookupReferenceValueModel> SubGroupAllowDiscount
        {
            get { return _SubGroupAllowDiscount; }
            set { Set(ref _SubGroupAllowDiscount, value); }
        }

        private LookupReferenceValueModel _SelectSubGroupAllowDiscount;
        public LookupReferenceValueModel SelectSubGroupAllowDiscount
        {
            get { return _SelectSubGroupAllowDiscount; }
            set { Set(ref _SelectSubGroupAllowDiscount, value); }
        }

        private DateTime? _SubGroupDateFrom;
        public DateTime? SubGroupDateFrom
        {
            get { return _SubGroupDateFrom; }
            set { Set(ref _SubGroupDateFrom, value); }
        }

        private DateTime? _SubGroupDateTo;
        public DateTime? SubGroupDateTo
        {
            get { return _SubGroupDateTo; }
            set { Set(ref _SubGroupDateTo, value); }
        }

        private ObservableCollection<AgreementDetailDiscountModel> _DiscountSubGroupSource;
        public ObservableCollection<AgreementDetailDiscountModel> DiscountSubGroupSource
        {
            get { return _DiscountSubGroupSource ?? (DiscountSubGroupSource = new ObservableCollection<AgreementDetailDiscountModel>()); }
            set { Set(ref _DiscountSubGroupSource, value); }
        }

        private AgreementDetailDiscountModel _SelectDiscountSubGroupSource;
        public AgreementDetailDiscountModel SelectDiscountSubGroupSource
        {
            get { return _SelectDiscountSubGroupSource; }
            set { Set(ref _SelectDiscountSubGroupSource, value);
                if (SelectDiscountSubGroupSource != null)
                {
                    IsSelectSubGroup = true;
                    ItemName = DataService.MasterData.GetBillableItemByBillingSubGroupUID(SelectDiscountSubGroupSource.ServiceUID);

                    if(agreementItems != null)
                    {
                        DiscountItemSource = new ObservableCollection<AgreementItemDiscountModel>(agreementItems.Where(p => p.BillingSubGroupUID == SelectDiscountSubGroupSource.ServiceUID));
                    }

                    SelectSubGroupName = SubGroupName.FirstOrDefault(p => p.BillingSubGroupUID == SelectDiscountSubGroupSource.ServiceUID);
                    SubGroupDiscount = SelectDiscountSubGroupSource.Discount;
                    SubGroupIsPer = SelectDiscountSubGroupSource.IsPercentage == "Y" ? true : false;
                    SelectSubGroupAllowDiscount = SubGroupAllowDiscount.FirstOrDefault(p => p.Key == SelectDiscountSubGroupSource.ALLDIUID);
                    SubGroupDateFrom = SelectDiscountSubGroupSource.DateFrom;
                    SubGroupDateTo = SelectDiscountSubGroupSource.DateTo;
                }
                else
                {
                    IsSelectSubGroup = false;
                }
            }
        }
        #endregion 

        #region Item
        private bool? _IsSelectItem;
        public bool? IsSelectItem
        {
            get { return _IsSelectItem ?? (IsSelectItem = false); }
            set { Set(ref _IsSelectItem, value); }
        }

        private List<BillableItemModel> _ItemName;
        public List<BillableItemModel> ItemName
        {
            get { return _ItemName; }
            set { Set(ref _ItemName, value); }
        }

        private BillableItemModel _SelectItemName;
        public BillableItemModel SelectItemName
        {
            get { return _SelectItemName; }
            set { Set(ref _SelectItemName, value); }
        }

        private double? _ItemDiscount;
        public double? ItemDiscount
        {
            get { return _ItemDiscount; }
            set { Set(ref _ItemDiscount, value); }
        }

        private bool _ItemIsPer;
        public bool ItemIsPer
        {
            get { return _ItemIsPer; }
            set { Set(ref _ItemIsPer, value); }
        }

        private List<LookupReferenceValueModel> _ItemAllowDiscount;
        public List<LookupReferenceValueModel> ItemAllowDiscount
        {
            get { return _ItemAllowDiscount; }
            set { Set(ref _ItemAllowDiscount, value); }
        }

        private LookupReferenceValueModel _SelectItemAllowDiscount;
        public LookupReferenceValueModel SelectItemAllowDiscount
        {
            get { return _SelectItemAllowDiscount; }
            set { Set(ref _SelectItemAllowDiscount, value); }
        }

        private DateTime? _ItemDateFrom;
        public DateTime? ItemDateFrom
        {
            get { return _ItemDateFrom; }
            set { Set(ref _ItemDateFrom, value); }
        }

        private DateTime? _ItemDateTo;
        public DateTime? ItemDateTo
        {
            get { return _ItemDateTo; }
            set { Set(ref _ItemDateTo, value); }
        }

        private ObservableCollection<AgreementItemDiscountModel> _DiscountItemSource;
        public ObservableCollection<AgreementItemDiscountModel> DiscountItemSource
        {
            get { return _DiscountItemSource ?? (DiscountItemSource = new ObservableCollection<AgreementItemDiscountModel>()); }
            set { Set(ref _DiscountItemSource, value); }
        }

        private AgreementItemDiscountModel _SelectDiscountItemSource;
        public AgreementItemDiscountModel SelectDiscountItemSource
        {
            get { return _SelectDiscountItemSource; }
            set { Set(ref _SelectDiscountItemSource, value); 
                if(SelectDiscountItemSource != null)
                {
                    IsSelectItem = true;

                    SelectItemName = ItemName.FirstOrDefault(p => p.BillableItemUID == SelectDiscountItemSource.BillableItemUID);
                    ItemDiscount = SelectDiscountItemSource.Discount;
                    ItemIsPer = SelectDiscountItemSource.IsPercentage == "Y" ? true : false;
                    SelectItemAllowDiscount = ItemAllowDiscount.FirstOrDefault(p => p.Key == SelectDiscountItemSource.ALLDIUID);
                    ItemDateFrom = SelectDiscountItemSource.DateFrom;
                    ItemDateTo = SelectDiscountItemSource.DateTo;
                }
                else
                {
                    IsSelectItem = false;
                }
            }
        }
        #endregion

        #endregion

        #endregion

        #region Command
        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get { return _SaveCommand?? (_SaveCommand = new RelayCommand(Save)); }
        }

        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get { return _CancelCommand?? (_CancelCommand = new RelayCommand(Cancel)); }
        }

        private RelayCommand _AddGroupCommand;
        public RelayCommand AddGroupCommand
        {
            get { return _AddGroupCommand ?? (_AddGroupCommand = new RelayCommand(AddGroup)); }
        }

        private RelayCommand _UpdateGroupCommand;
        public RelayCommand UpdateGroupCommand
        {
            get { return _UpdateGroupCommand ?? (_UpdateGroupCommand = new RelayCommand(UpdateGroup)); }
        }

        private RelayCommand _DeleteGroupCommand;
        public RelayCommand DeleteGroupCommand
        {
            get { return _DeleteGroupCommand ?? (_DeleteGroupCommand = new RelayCommand(DeleteGroup)); }
        }

        private RelayCommand _ClearGroupCommand;
        public RelayCommand ClearGroupCommand
        {
            get { return _ClearGroupCommand ?? (_ClearGroupCommand = new RelayCommand(ClearGroup)); }
        }

        private RelayCommand _AddSubGroupCommand;
        public RelayCommand AddSubGroupCommand
        {
            get { return _AddSubGroupCommand ?? (_AddSubGroupCommand = new RelayCommand(AddSubGroup)); }
        }

        private RelayCommand _UpdateSubGroupCommand;
        public RelayCommand UpdateSubGroupCommand
        {
            get { return _UpdateSubGroupCommand ?? (_UpdateSubGroupCommand = new RelayCommand(UpdateSubGroup)); }
        }

        private RelayCommand _DeleteSubGroupCommand;
        public RelayCommand DeleteSubGroupCommand
        {
            get { return _DeleteSubGroupCommand ?? (_DeleteSubGroupCommand = new RelayCommand(DeleteSubGroup)); }
        }

        private RelayCommand _ClearSubGroupCommand;
        public RelayCommand ClearSubGroupCommand
        {
            get { return _ClearSubGroupCommand ?? (_ClearSubGroupCommand = new RelayCommand(ClearSubGroup)); }
        }

        private RelayCommand _AddItemCommand;
        public RelayCommand AddItemCommand
        {
            get { return _AddItemCommand ?? (_AddItemCommand = new RelayCommand(AddItem)); }
        }

        private RelayCommand _UpdateItemCommand;
        public RelayCommand UpdateItemCommand
        {
            get { return _UpdateItemCommand ?? (_UpdateItemCommand = new RelayCommand(UpdateItem)); }
        }

        private RelayCommand _DeleteItemCommand;
        public RelayCommand DeleteItemCommand
        {
            get { return _DeleteItemCommand ?? (_DeleteItemCommand = new RelayCommand(DeleteItem)); }
        }

        private RelayCommand _ClearItemCommand;
        public RelayCommand ClearItemCommand
        {
            get { return _ClearItemCommand ?? (_ClearItemCommand = new RelayCommand(ClearItem)); }
        }
        #endregion

        #region Methoh
        PayorAgreementModel payorAgreement;

        List<BillingGroupModel> billingGroups;
        List<BillingSubGroupModel> billingSubGroups;
        List<BillableItemModel> billableItems;

        List<AgreementAccountDiscountModel> agreementAccounts;
        List<AgreementDetailDiscountModel> agreementDetails;
        List<AgreementItemDiscountModel> agreementItems;

        List<AgreementAccountDiscountModel> deleteAgreementAccounts;
        List<AgreementDetailDiscountModel> deleteAgreementDetails;
        List<AgreementItemDiscountModel> deleteAgreementItems;

        public ManagePayorAgreementViewModel()
        {
            DateTime now = DateTime.Now;
            var refValue = DataService.Technical.GetReferenceValueList("PAYTRM,PYRACAT,PBTYP,PBLCT,BLTYP");
            //ProvinceSource = DataService.Technical.GetProvince();
            CreditTerm = refValue.Where(p => p.DomainCode == "PAYTRM").ToList();
            //PayorCategory = refValue.Where(p => p.DomainCode == "PYRACAT").ToList();
            PayorBillType = refValue.Where(p => p.DomainCode == "PBTYP").ToList();
            BillType = refValue.Where(p => p.DomainCode == "BLTYP").ToList();

            PrimaryTariff = refValue.Where(p => p.DomainCode == "PBLCT").ToList();
            SecondaryTariff = refValue.Where(p => p.DomainCode == "PBLCT").ToList();
            TertiaryTariff = refValue.Where(p => p.DomainCode == "PBLCT").ToList();

            PolicyMaster = DataService.Billing.GetPolicyMasterAll();

            Tariff = refValue.Where(p => p.DomainCode == "PBLCT").ToList();
            GroupAllowDiscount = DataService.Technical.GetReferenceValueMany("ALLDI");
            GroupDateFrom = now;

            SubGroupAllowDiscount = DataService.Technical.GetReferenceValueMany("ALLDI");
            SubGroupDateFrom = now;

            ItemAllowDiscount = DataService.Technical.GetReferenceValueMany("ALLDI");
            ItemDateFrom = now;
        }
        private void Save()
        {
            if (String.IsNullOrEmpty(Name))
            {
                WarningDialog("กรุณาระบุ Name");
                return;
            }
            if (String.IsNullOrEmpty(Code))
            {
                WarningDialog("กรุณาระบุ Code");
                return;
            }
            if(SelectBillType == null)
            {
                WarningDialog("กรุณาเลือก Bill Type");
                return;
            }
            if (SelectPayorBillType == null)
            {
                WarningDialog("กรุณาเลือก Payor Bill Type");
                return;
            }
            if (SelectPolicyMaster == null)
            {
                WarningDialog("กรุณาเลือก Contact");
                return;
            }
            if(SelectPrimaryTariff == null)
            {
                WarningDialog("กรุณาเลือก Primary Tariff");
                return;
            }

            AssignProprotiesToModel();

            if (deleteAgreementAccounts != null && deleteAgreementAccounts.Count != 0)
                payorAgreement.AgreementDiscount.AddRange(deleteAgreementAccounts);

            if(deleteAgreementDetails != null && deleteAgreementDetails.Count != 0)
                payorAgreement.AgreementDiscountDetails.AddRange(deleteAgreementDetails);

            if (deleteAgreementItems != null && deleteAgreementItems.Count != 0)
                payorAgreement.AgreementItems.AddRange(deleteAgreementItems);

            DataService.Billing.ManagePayorAgreement(payorAgreement, AppUtil.Current.UserID);
            CloseViewDialog(ActionDialog.Save);
        }

        public void AssignModel(int? insuranceCompanyUID, PayorAgreementModel agreementModel = null)
        {
            InsuranceCompanyID = insuranceCompanyUID ?? 0;
            if (agreementModel != null)
            {
                payorAgreement = agreementModel;
                AssignModelToProperties(agreementModel);

                var data = DataService.Billing.GetAgreementAccountByAgreementUID(agreementModel.PayorAgreementUID);
                if(data.Count != 0)
                {
                    int? opd = Tariff.Where(p => p.ValueCode == "OPDTRF").Select(p => p.Key).FirstOrDefault();
                    int? employee = Tariff.Where(p => p.ValueCode == "EPYTRF").Select(p => p.Key).FirstOrDefault();
                    int? foreigner = Tariff.Where(p => p.ValueCode == "FRNTRF").Select(p => p.Key).FirstOrDefault();
                    int? ipd = Tariff.Where(p => p.ValueCode == "IPDTRF").Select(p => p.Key).FirstOrDefault();

                    var IsOpd = data.FirstOrDefault(p => p.PBLCTUID == opd);
                    if(IsOpd != null)
                    {
                        int index = Tariff.FindIndex(p => p.Key == opd);
                        Tariff[index].Display = Tariff[index].Display + " (ใช้)";
                    }

                    var IsEmployee = data.FirstOrDefault(p => p.PBLCTUID == employee);
                    if (IsEmployee != null)
                    {
                        int index = Tariff.FindIndex(p => p.Key == employee);
                        Tariff[index].Display = Tariff[index].Display + " (ใช้)";
                    }

                    var IsForeigner = data.FirstOrDefault(p => p.PBLCTUID == foreigner);
                    if (IsForeigner != null)
                    {
                        int index = Tariff.FindIndex(p => p.Key == foreigner);
                        Tariff[index].Display = Tariff[index].Display + " (ใช้)";
                    }

                    var IsIpd = data.FirstOrDefault(p => p.PBLCTUID == ipd);
                    if (IsIpd != null)
                    {
                        int index = Tariff.FindIndex(p => p.Key == ipd);
                        Tariff[index].Display = Tariff[index].Display + " (ใช้)";
                    }

                    //DiscountGroupSource = new ObservableCollection<AgreementAccountDiscountModel>(data);
                    agreementAccounts = data;
                } 

                var data2 = DataService.Billing.GetAgreementAccountDetailByAgreementUID(agreementModel.PayorAgreementUID);
                //DiscountSubGroupSource = data.Count != 0 ? new ObservableCollection<AgreementDetailDiscountModel>(data2) : null;
                agreementDetails = data2;

                var data3 = DataService.Billing.GetAgreementItemByAgreementUID(agreementModel.PayorAgreementUID);
                //DiscountItemSource = data.Count != 0 ? new ObservableCollection<AgreementItemDiscountModel>(data3) : null;
                agreementItems = data3;
            }
        }

        private void AssignModelToProperties(PayorAgreementModel model)
        {
            Name = model.Name;
            Code = model.Code;
            
            if (model.BLTYPUID != null)
                SelectBillType = BillType.FirstOrDefault(p => p.Key == model.BLTYPUID);
            if (model.PBTYPUID != null)
                SelectPayorBillType = PayorBillType.FirstOrDefault(p => p.Key == model.PBTYPUID);
            if (model.CRDTRMUID != null)
                SelectCreditTerm = CreditTerm.FirstOrDefault(p => p.Key == model.CRDTRMUID);
            if (model.PrimaryPBLCTUID != null)
                SelectPrimaryTariff = PrimaryTariff.FirstOrDefault(p => p.Key == model.PrimaryPBLCTUID);
            if (model.SecondaryPBLCTUID != null)
                SelectSecondaryTariff = SecondaryTariff.FirstOrDefault(p => p.Key == model.SecondaryPBLCTUID);
            if (model.TertiaryPBLCTUID != null)
                SelectTertiaryTariff = TertiaryTariff.FirstOrDefault(p => p.Key == model.TertiaryPBLCTUID);
            //if (model.AGTYPUID != null)
            //    SelectPolicyMaster = PolicyMaster.FirstOrDefault(p => p.PolicyMasterUID == model.AGTYPUID);
            ClaimPercentage = model.ClaimPercentage.ToString();
            CoverPerDay = model.OPDCoverPerDay.ToString();
            IsLimitAfterDiscount = model.IsLimitAfterDiscount == "Y" ? true : false;

            ActiveFrom = model.ActiveFrom;
            ActiveTo = model.ActiveTo;

            if(model.PolicyMasterUID != 0)
            SelectPolicyMaster = PolicyMaster.FirstOrDefault(p => p.PolicyMasterUID == model.PolicyMasterUID);
        }


        private void AssignProprotiesToModel()
        {
            if (payorAgreement == null)
            {
                payorAgreement = new PayorAgreementModel();
            }

            payorAgreement.InsuranceCompanyUID = InsuranceCompanyID;
            payorAgreement.Name = Name;
            payorAgreement.Code = Code;
            //payorAgreement.Description = 
            payorAgreement.BLTYPUID = SelectBillType != null ? SelectBillType.Key : (int?)null;
            payorAgreement.CRDTRMUID = SelectCreditTerm != null ? SelectCreditTerm.Key : (int?)null;
            payorAgreement.PBTYPUID = SelectPayorBillType != null ? SelectPayorBillType.Key : (int?)null;

            payorAgreement.ClaimPercentage = !String.IsNullOrEmpty(ClaimPercentage) ? double.Parse(ClaimPercentage) : 0;
            payorAgreement.OPDCoverPerDay = !String.IsNullOrEmpty(CoverPerDay) ? double.Parse(CoverPerDay) : (double?)null;

            payorAgreement.PrimaryPBLCTUID = SelectPrimaryTariff != null ? SelectPrimaryTariff.Key : (int?)null;
            payorAgreement.SecondaryPBLCTUID = SelectSecondaryTariff != null ? SelectSecondaryTariff.Key : (int?)null;
            payorAgreement.TertiaryPBLCTUID = SelectTertiaryTariff != null ? SelectTertiaryTariff.Key : (int?)null;
            payorAgreement.AGTYPUID = SelectPolicyMaster != null ? SelectPolicyMaster.AGTYPUID : (int?)null;
            payorAgreement.IsLimitAfterDiscount = IsLimitAfterDiscount == true ? "Y" : "N";
            payorAgreement.PolicyMasterUID = SelectPolicyMaster != null ? SelectPolicyMaster.PolicyMasterUID : (int?)null;
            payorAgreement.ActiveFrom = ActiveFrom;
            payorAgreement.ActiveTo = ActiveTo;

            payorAgreement.AgreementDiscount = agreementAccounts;
            payorAgreement.AgreementDiscountDetails = agreementDetails;
            payorAgreement.AgreementItems = agreementItems;
        }

        private void AddGroup()
        {
            if (SelectTariff != null)
            {
                if (SelectGroupName != null)
                {

                    if (SelectGroupAllowDiscount == null)
                    {
                        WarningDialog("กรุณาเลือก Allow Discount");
                        return;
                    }
                    var Isdata = DiscountGroupSource.FirstOrDefault(p => p.ServiceUID == SelectGroupName.BillingGroupUID);
                    if (Isdata != null)
                    {
                        WarningDialog("มีรายการนี้อยู่แล้ว กรุณาเลือกรายการใหม่");
                        return;
                    }

                    AgreementAccountDiscountModel data = new AgreementAccountDiscountModel();
                    data.ServiceUID = SelectGroupName.BillingGroupUID;
                    data.ServiceName = SelectGroupName.Description;
                    data.Discount = GroupDiscount ?? 0;
                    data.IsPercentage = GroupIsPer == true ? "Y" : "N";
                    data.PBLCTUID = SelectTariff.Key ?? 0;
                    data.ALLDIUID = SelectGroupAllowDiscount.Key ?? 0;
                    data.AllowDiscount = SelectGroupAllowDiscount.Display;
                    data.DateFrom = GroupDateFrom;
                    data.DateTo = GroupDateTo;

                    if (agreementAccounts == null)
                        agreementAccounts = new List<AgreementAccountDiscountModel>();

                    agreementAccounts.Add(data);
                    DiscountGroupSource.Add(data);
                    ClearGroup();
                }
            }
        }

        private void UpdateGroup()
        {
            if (SelectDiscountGroupSource != null)
            {
                var index = DiscountGroupSource.IndexOf(SelectDiscountGroupSource);
                var indexTemp = agreementAccounts.IndexOf(SelectDiscountGroupSource);

                AgreementAccountDiscountModel data = new AgreementAccountDiscountModel();
                data = SelectDiscountGroupSource;
                data.ServiceUID = SelectGroupName.BillingGroupUID;
                data.ServiceName = SelectGroupName.Description;
                data.Discount = GroupDiscount ?? 0;
                data.IsPercentage = GroupIsPer == true ? "Y" : "N";
                data.PBLCTUID = SelectTariff.Key ?? 0;
                data.ALLDIUID = SelectGroupAllowDiscount.Key ?? 0;
                data.AllowDiscount = SelectGroupAllowDiscount.Display;
                data.DateFrom = GroupDateFrom;
                data.DateTo = GroupDateTo;

                DiscountGroupSource[index] = data;
                agreementAccounts[indexTemp] = data;
            }
        }

        private void DeleteGroup()
        {
            if (SelectDiscountGroupSource != null)
            {
                var d = SelectDiscountGroupSource;
                var value = DiscountSubGroupSource.Where(p => p.BillgGroupUID == SelectDiscountGroupSource.ServiceUID).ToList();
                if (value != null && value.Count != 0)
                {
                    foreach (var item in value)
                    {
                        var value2 = DiscountItemSource.Where(p => p.BillingSubGroupUID == item.ServiceUID).ToList();
                        if (value2 != null && value2.Count != 0)
                        {
                            foreach (var itemDetail in value2)
                            {
                                if (itemDetail.AgreementItemDiscountUID != 0)
                                {
                                    itemDetail.StatusFlag = "D";
                                    if (deleteAgreementItems == null)
                                        deleteAgreementItems = new List<AgreementItemDiscountModel>();

                                    deleteAgreementItems.Add(itemDetail);
                                }
                                agreementItems.Remove(itemDetail);
                                DiscountItemSource.Remove(itemDetail);
                                
                            }
                        }


                        if (item.AgreementDetailDiscountUID != 0)
                        {
                            item.StatusFlag = "D";
                            if (deleteAgreementDetails == null)
                                deleteAgreementDetails = new List<AgreementDetailDiscountModel>();

                            deleteAgreementDetails.Add(item);
                        }
                        agreementDetails.Remove(item);
                        DiscountSubGroupSource.Remove(item);
                        
                    }
                }

                if(SelectDiscountGroupSource.AgreementAccountDiscountUID != 0)
                {
                    d.StatusFlag = "D";
                    if (deleteAgreementAccounts == null)
                        deleteAgreementAccounts = new List<AgreementAccountDiscountModel>();

                    deleteAgreementAccounts.Add(d);
                }

                agreementAccounts.Remove(SelectDiscountGroupSource);
                DiscountGroupSource.Remove(SelectDiscountGroupSource);
                
                ClearGroup();
            }
        }

        private void ClearGroup()
        {
            SelectGroupName = null;
            GroupDiscount = null;
            GroupIsPer = false;
            SelectGroupAllowDiscount = null;
            GroupDateTo = null;
            SelectDiscountGroupSource = null;
        }

        private void AddSubGroup()
        {
            if (SelectTariff != null)
            {
                if (SelectSubGroupName != null)
                {
                    if (SelectSubGroupAllowDiscount == null)
                    {
                        WarningDialog("กรุณาเลือก Allow Discount");
                        return;
                    }
                    var Isdata = DiscountSubGroupSource.FirstOrDefault(p => p.ServiceUID == SelectSubGroupName.BillingSubGroupUID);
                    if (Isdata != null)
                    {
                        WarningDialog("มีรายการนี้อยู่แล้ว กรุณาเลือกรายการใหม่");
                        return;
                    }

                    AgreementDetailDiscountModel data = new AgreementDetailDiscountModel();
                    data.BillgGroupUID = SelectSubGroupName.BillingGroupUID;
                    data.ServiceUID = SelectSubGroupName.BillingSubGroupUID;
                    data.ServiceName = SelectSubGroupName.Description;
                    data.Discount = SubGroupDiscount ?? 0;
                    data.IsPercentage = SubGroupIsPer == true ? "Y" : "N";
                    data.PBLCTUID = SelectTariff.Key ?? 0;
                    data.ALLDIUID = SelectSubGroupAllowDiscount.Key ?? 0;
                    data.AllowDiscount = SelectSubGroupAllowDiscount.Display;
                    data.DateFrom = SubGroupDateFrom;
                    data.DateTo = SubGroupDateTo;

                    if (agreementDetails == null)
                        agreementDetails = new List<AgreementDetailDiscountModel>();

                    agreementDetails.Add(data);
                    DiscountSubGroupSource.Add(data);
                    ClearSubGroup();
                }

            }
        }

        private void UpdateSubGroup()
        {
            if (SelectDiscountSubGroupSource != null)
            {
                var index = DiscountSubGroupSource.IndexOf(SelectDiscountSubGroupSource);
                var indexTemp = agreementDetails.IndexOf(SelectDiscountSubGroupSource);

                AgreementDetailDiscountModel data = new AgreementDetailDiscountModel();
                data = SelectDiscountSubGroupSource;
                data.BillgGroupUID = SelectSubGroupName.BillingGroupUID;
                data.ServiceUID = SelectSubGroupName.BillingSubGroupUID;
                data.ServiceName = SelectSubGroupName.Description;
                data.Discount = SubGroupDiscount ?? 0;
                data.IsPercentage = SubGroupIsPer == true ? "Y" : "N";
                data.PBLCTUID = SelectTariff.Key ?? 0;
                data.ALLDIUID = SelectSubGroupAllowDiscount.Key ?? 0;
                data.AllowDiscount = SelectSubGroupAllowDiscount.Display;
                data.DateFrom = SubGroupDateFrom;
                data.DateTo = SubGroupDateTo;

                DiscountSubGroupSource[index] = data;
                agreementDetails[indexTemp] = data;
            }
        }

        private void DeleteSubGroup()
        {
            if (SelectDiscountSubGroupSource != null)
            {
                var d = SelectDiscountSubGroupSource;
                var value = DiscountItemSource.Where(p => p.BillingSubGroupUID == SelectDiscountSubGroupSource.ServiceUID).ToList();
                if (value != null && value.Count != 0)
                {
                    foreach (var item in value)
                    {
                        if(item.AgreementItemDiscountUID != 0)
                        {
                            item.StatusFlag = "D";
                            if (deleteAgreementItems == null)
                                deleteAgreementItems = new List<AgreementItemDiscountModel>();

                            deleteAgreementItems.Add(item);
                        }
                        agreementItems.Remove(item);
                        DiscountItemSource.Remove(item);
                        
                    }
                }
                if (SelectDiscountSubGroupSource.AgreementDetailDiscountUID != 0)
                {
                    d.StatusFlag = "D";
                    if (deleteAgreementDetails == null)
                        deleteAgreementDetails = new List<AgreementDetailDiscountModel>();

                    deleteAgreementDetails.Add(d);
                }

                agreementDetails.Remove(SelectDiscountSubGroupSource);
                DiscountSubGroupSource.Remove(SelectDiscountSubGroupSource);
               
                ClearSubGroup();
            }
        }

        private void ClearSubGroup()
        {
            SelectSubGroupName = null;
            SubGroupDiscount = null;
            SubGroupIsPer = false;
            SelectSubGroupAllowDiscount = null;
            SubGroupDateTo = null;
            SelectDiscountSubGroupSource = null;
        }

        private void AddItem()
        {
            if (SelectTariff != null)
            {
                if (SelectItemName != null)
                {
                    if (SelectItemAllowDiscount == null)
                    {
                        WarningDialog("กรุณาเลือก Allow Discount");
                        return;
                    }
                    var Isdata = DiscountItemSource.FirstOrDefault(p => p.BillableItemUID == SelectItemName.BillableItemUID);
                    if (Isdata != null)
                    {
                        WarningDialog("มีรายการนี้อยู่แล้ว กรุณาเลือกรายการใหม่");
                        return;
                    }

                    AgreementItemDiscountModel data = new AgreementItemDiscountModel();
                    data.BillableItemUID = SelectItemName.BillableItemUID;
                    data.BillingSubGroupUID = SelectItemName.BillingSubGroupUID ?? 0;
                    data.BillableItemName = SelectItemName.ItemName;
                    data.Discount = ItemDiscount ?? 0;
                    data.IsPercentage = ItemIsPer == true ? "Y" : "N";
                    data.PBLCTUID = SelectTariff.Key ?? 0;
                    data.ALLDIUID = SelectItemAllowDiscount.Key ?? 0;
                    data.AllowDiscount = SelectItemAllowDiscount.Display;
                    data.DateFrom = ItemDateFrom;
                    data.DateTo = ItemDateTo;

                    if (agreementItems == null)
                        agreementItems = new List<AgreementItemDiscountModel>();

                    agreementItems.Add(data);
                    DiscountItemSource.Add(data);
                    ClearItem();
                }
            }
        }
        private void UpdateItem()
        {
            if (SelectDiscountItemSource != null)
            {
                var index = DiscountItemSource.IndexOf(SelectDiscountItemSource);
                var indexTemp = agreementItems.IndexOf(SelectDiscountItemSource);

                AgreementItemDiscountModel data = new AgreementItemDiscountModel();
                data = SelectDiscountItemSource;
                data.BillableItemUID = SelectItemName.BillableItemUID;
                data.BillingSubGroupUID = SelectItemName.BillingSubGroupUID ?? 0;
                data.BillableItemName = SelectItemName.ItemName;
                data.Discount = ItemDiscount ?? 0;
                data.IsPercentage = ItemIsPer == true ? "Y" : "N";
                data.PBLCTUID = SelectTariff.Key ?? 0;
                data.ALLDIUID = SelectItemAllowDiscount.Key ?? 0;
                data.AllowDiscount = SelectItemAllowDiscount.Display;
                data.DateFrom = ItemDateFrom;
                data.DateTo = ItemDateTo;

                DiscountItemSource[index] = data;
                agreementItems[indexTemp] = data;
            }
        }

        private void DeleteItem()
        {
            if (SelectDiscountItemSource != null)
            {
                var d = SelectDiscountItemSource;
                if (SelectDiscountItemSource.AgreementItemDiscountUID != 0)
                {
                    d.StatusFlag = "D";
                    if (deleteAgreementItems == null)
                        deleteAgreementItems = new List<AgreementItemDiscountModel>();

                    deleteAgreementItems.Add(d);
                }

                DiscountItemSource.Remove(SelectDiscountItemSource);
                agreementItems.Remove(SelectDiscountItemSource);
                ClearItem();
            }
        }

        private void ClearItem()
        {
            SelectItemName = null;
            ItemDiscount = null;
            ItemIsPer = false;
            SelectItemAllowDiscount = null;
            ItemDateTo = null;
            SelectDiscountItemSource = null;
        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }
        #endregion
    }
}
