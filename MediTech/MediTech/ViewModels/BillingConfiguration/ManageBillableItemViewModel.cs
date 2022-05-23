using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Models;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MediTech.ViewModels
{
    public class ManageBillableItemViewModel : MediTechViewModelBase
    {

        #region Properties

        private string _Code;

        public string Code
        {
            get { return _Code; }
            set { Set(ref _Code, value); }
        }

        private string _ReferenceCode;

        public string ReferenceCode
        {
            get { return _ReferenceCode; }
            set { Set(ref _ReferenceCode, value); }
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

        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { Set(ref _Comments, value); }
        }




        private bool _IsShareDoctor;

        public bool IsShareDoctor
        {
            get { return _IsShareDoctor; }
            set
            {
                Set(ref _IsShareDoctor, value);
                if (!_IsShareDoctor)
                {
                    DoctorFee = null;
                }
            }
        }


        private double? _DoctorFee;

        public double? DoctorFee
        {
            get { return _DoctorFee; }
            set
            {
                Set(ref _DoctorFee, value);
                //TotalCost = Cost + (DoctorFee ?? 0);
            }
        }

        private double _Price;

        public double Price
        {
            get { return _Price; }
            set { Set(ref _Price, value); }
        }

        private double _Cost;

        public double Cost
        {
            get { return _Cost; }
            set { Set(ref _Cost, value); }
        }

        private List<LookupReferenceValueModel> _Units;

        public List<LookupReferenceValueModel> Units
        {
            get { return _Units; }
            set { Set(ref _Units, value); }
        }

        private LookupReferenceValueModel _SelectUnit;

        public LookupReferenceValueModel SelectUnit
        {
            get { return _SelectUnit; }
            set { Set(ref _SelectUnit, value); }
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


        private List<LookupReferenceValueModel> _ServiceTypes;

        public List<LookupReferenceValueModel> ServiceTypes
        {
            get { return _ServiceTypes; }
            set { Set(ref _ServiceTypes, value); }
        }

        private LookupReferenceValueModel _SelectServiceType;

        public LookupReferenceValueModel SelectServiceType
        {
            get { return _SelectServiceType; }
            set
            {
                Set(ref _SelectServiceType, value);
                if (_SelectServiceType != null)
                {
                    IsVisibilityAvgCost = Visibility.Collapsed;
                    IsVisibilityRefCode = Visibility.Collapsed;
                    IsVisibilityCost = Visibility.Visible;
                    VisibleCost = true;
                    string serviceType = SelectServiceType.Display;
                    IsVisibilityItem = Visibility.Visible;
                    IsEnableCode = false;
                    SelectedItemService = null;
                    Code = string.Empty;
                    Name = string.Empty;
                    Description = string.Empty;
                    ItemAverageCost = null;
                    if (serviceType == "Order Item")
                    {
                        IsEnableCode = true;
                        IsVisibilityItem = Visibility.Collapsed;
                    }

                    if (serviceType == "Lab Test")
                    {
                        var data = DataService.MasterData.GetRequestItemByCategory("LAB");
                        ItemsServiceSource = data.Select(p => new ItemServiceModel
                        {
                            ItemUID = p.RequestItemUID,
                            Code = p.Code,
                            Name = p.ItemName,
                            ActiveFrom = p.EffectiveFrom,
                            ActiveTo = p.EffectiveTo
                        }).ToList();
                        IsEnableCode = true;
                        IsVisibilityRefCode = Visibility.Visible;
                    }
                    else if (serviceType == "Radiology")
                    {
                        var data = DataService.MasterData.GetRequestItemByCategory("RADIOLOGY");
                        ItemsServiceSource = data.Select(p => new ItemServiceModel
                        {
                            ItemUID = p.RequestItemUID,
                            Code = p.Code,
                            Name = p.ItemName,
                            ActiveFrom = p.EffectiveFrom,
                            ActiveTo = p.EffectiveTo
                        }).ToList();
                        IsEnableCode = true;
                        IsVisibilityRefCode = Visibility.Visible;
                    }
                    else if (serviceType == "Mobile Checkup")
                    {
                        var data = DataService.MasterData.GetRequestItemByCategory("Mobile Checkup");
                        ItemsServiceSource = data.Select(p => new ItemServiceModel
                        {
                            ItemUID = p.RequestItemUID,
                            Code = p.Code,
                            Name = p.ItemName,
                            ActiveFrom = p.EffectiveFrom,
                            ActiveTo = p.EffectiveTo
                        }).ToList();
                        IsEnableCode = true;
                        IsVisibilityRefCode = Visibility.Visible;
                    }
                    else if (serviceType == "Drug")
                    {
                        var data = DataService.Inventory.GetItemMasterByType("Drug");
                        ItemsServiceSource = data.Select(p => new ItemServiceModel
                        {
                            ItemUID = p.ItemMasterUID,
                            Code = p.Code,
                            Name = p.Name,
                            ActiveFrom = p.ActiveFrom,
                            ActiveTo = p.ActiveTo
                        }).ToList();
                        IsVisibilityAvgCost = Visibility.Visible;
                        IsVisibilityCost = Visibility.Hidden;
                        VisibleCost = false;
                    }
                    else if (serviceType == "Medical Supplies")
                    {
                        var data = DataService.Inventory.GetItemMasterByType("Medical Supplies");
                        ItemsServiceSource = data.Select(p => new ItemServiceModel
                        {
                            ItemUID = p.ItemMasterUID,
                            Code = p.Code,
                            Name = p.Name,
                            ActiveFrom = p.ActiveFrom,
                            ActiveTo = p.ActiveTo
                        }).ToList();
                        IsVisibilityAvgCost = Visibility.Visible;
                        IsVisibilityCost = Visibility.Hidden;
                        VisibleCost = false;
                    }
                    else if (serviceType == "Supply")
                    {
                        var data = DataService.Inventory.GetItemMasterByType("Supply");
                        ItemsServiceSource = data.Select(p => new ItemServiceModel
                        {
                            ItemUID = p.ItemMasterUID,
                            Code = p.Code,
                            Name = p.Name,
                            ActiveFrom = p.ActiveFrom,
                            ActiveTo = p.ActiveTo
                        }).ToList();
                        IsVisibilityAvgCost = Visibility.Visible;
                        IsVisibilityCost = Visibility.Hidden;
                        VisibleCost = false;
                    }
                    else
                    {
                        ItemsServiceSource = null;
                    }

                }
            }
        }

        private List<ItemServiceModel> _ItemsServiceSource;

        public List<ItemServiceModel> ItemsServiceSource
        {
            get { return _ItemsServiceSource; }
            set { Set(ref _ItemsServiceSource, value); }
        }

        private ItemServiceModel _SelectItemService;

        public ItemServiceModel SelectedItemService
        {
            get { return _SelectItemService; }
            set
            {
                Set(ref _SelectItemService, value);
                if (_SelectItemService != null)
                {
                    string serviceType = SelectServiceType.Display;

                    if (serviceType == "Lab Test" || serviceType == "Radiology" || serviceType == "Mobile Checkup")
                    {
                        ReferenceCode = _SelectItemService.Code;
                    }
                    else
                    {
                        Code = _SelectItemService.Code;
                    }
                    Name = _SelectItemService.Name;

                    if (serviceType == "Drug" || serviceType == "Medical Supplies" || serviceType == "Supply")
                    {
                        var roleOrgan = Organisations;
                        var itemAverageCost = DataService.Inventory.GetItemAverageCost(SelectedItemService.ItemUID, null);
                        ItemAverageCost = (from i in itemAverageCost
                                           join j in roleOrgan on i.OwnerOrganisationUID equals j.HealthOrganisationUID
                                           select i).ToList();
                    }
                    else
                    {
                        ItemAverageCost = null;
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



        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set { Set(ref _SelectOrganisation, value); }
        }

        private List<BillingGroupModel> _BillingGroup;

        public List<BillingGroupModel> BillingGroup
        {
            get { return _BillingGroup; }
            set { Set(ref _BillingGroup, value); }
        }

        private BillingGroupModel _SelectBillingGroup;

        public BillingGroupModel SelectBillingGroup
        {
            get { return _SelectBillingGroup; }
            set
            {
                Set(ref _SelectBillingGroup, value);
                if (_SelectBillingGroup != null)
                {
                    BillingSubGroup = DataService.Billing.GetBillingSubGroupByGroup(SelectBillingGroup.BillingGroupUID);
                }
            }
        }


        private List<BillingSubGroupModel> _BillingSubGroup;

        public List<BillingSubGroupModel> BillingSubGroup
        {
            get { return _BillingSubGroup; }
            set { Set(ref _BillingSubGroup, value); }
        }

        private BillingSubGroupModel _SelectBillingSubGroup;

        public BillingSubGroupModel SelectBillingSubGroup
        {
            get { return _SelectBillingSubGroup; }
            set { Set(ref _SelectBillingSubGroup, value); }
        }

        private bool _IsEnableCode;

        public bool IsEnableCode
        {
            get { return _IsEnableCode; }
            set { Set(ref _IsEnableCode, value); }
        }


        private DateTime _ActiveFrom2;

        public DateTime ActiveFrom2
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

        private ObservableCollection<BillableItemDetailModel> _BillableItemDetail;

        public ObservableCollection<BillableItemDetailModel> BillableItemDetail
        {
            get { return _BillableItemDetail ?? (_BillableItemDetail = new ObservableCollection<BillableItemDetailModel>()) ; }
            set { Set(ref _BillableItemDetail, value); }
        }

        private BillableItemDetailModel _SelectBillableItemDetail;

        public BillableItemDetailModel SelectBillableItemDetail
        {
            get { return _SelectBillableItemDetail; }
            set { 
                Set(ref _SelectBillableItemDetail, value);
                if (_SelectBillableItemDetail != null)
                {
                    Price = _SelectBillableItemDetail.Price;
                    Cost = _SelectBillableItemDetail.Cost;
                    ActiveFrom2 = _SelectBillableItemDetail.ActiveFrom;
                    ActiveTo2 = _SelectBillableItemDetail.ActiveTo;
                    SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == _SelectBillableItemDetail.OwnerOrganisationUID);
                    SelectUnit = Units.FirstOrDefault(p => p.Key == _SelectBillableItemDetail.CURNCUID);
                }
            }
        }

        private List<ItemAverageCostModel> _ItemAverageCost;

        public List<ItemAverageCostModel> ItemAverageCost
        {
            get { return _ItemAverageCost; }
            set { Set(ref _ItemAverageCost, value); }
        }

        private ItemAverageCostModel _SelectItemAverageCost;

        public ItemAverageCostModel SelectItemAverageCost
        {
            get { return _SelectItemAverageCost; }
            set {
                _SelectItemAverageCost = value;
            }
        }

        private bool _VisibleCost = false;

        public bool VisibleCost
        {
            get { return _VisibleCost; }
            set { Set(ref _VisibleCost, value); }
        }


        private Visibility _IsVisibilityAvgCost = Visibility.Collapsed;

        public Visibility IsVisibilityAvgCost
        {
            get { return _IsVisibilityAvgCost; }
            set { Set(ref _IsVisibilityAvgCost, value); }
        }

        private Visibility _IsVisibilityCost = Visibility.Hidden;

        public Visibility IsVisibilityCost
        {
            get { return _IsVisibilityCost; }
            set { Set(ref _IsVisibilityCost, value); }
        }

        private Visibility _IsVisibilityItem;

        public Visibility IsVisibilityItem
        {
            get { return _IsVisibilityItem; }
            set { Set(ref _IsVisibilityItem, value); }
        }

        private Visibility _IsVisibilityRefCode = Visibility.Collapsed;

        public Visibility IsVisibilityRefCode
        {
            get { return _IsVisibilityRefCode; }
            set { Set(ref _IsVisibilityRefCode, value); }
        }
        #endregion

        #region Command

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

        private RelayCommand _AddCommand;
        public RelayCommand AddCommand
        {
            get { return _AddCommand ?? (_AddCommand = new RelayCommand(AddBillItemDetail)); }
        }



        private RelayCommand _UpdateCommand;
        public RelayCommand UpdateCommand
        {
            get { return _UpdateCommand ?? (_UpdateCommand = new RelayCommand(UpdateBillItemDetail)); }
        }



        private RelayCommand _DeleteCommand;
        public RelayCommand DeleteCommand
        {
            get { return _DeleteCommand ?? (_DeleteCommand = new RelayCommand(DeleteBillItemDetail)); }
        }

        private RelayCommand _ClearCommand;
        public RelayCommand ClearCommand
        {
            get { return _ClearCommand ?? (_ClearCommand = new RelayCommand(ClearBillItmdetail)); }
        }

        #endregion

        #region Method

        public ManageBillableItemViewModel()
        {
            ServiceTypes = DataService.Technical.GetReferenceValueMany("BSMDD");
            BillingGroup = DataService.Billing.GetBillingGroup();
            Organisations = GetHealthOrganisationRole();
            //Organisations.Add(new HealthOrganisationModel { HealthOrganisationUID = 0, Name = "ราคามาตรฐานส่วนกลาง" });
            Organisations = Organisations.OrderBy(p => p.HealthOrganisationUID).ToList();
            Units = DataService.Technical.GetReferenceValueMany("CURNC");
            SelectOrganisation = Organisations.FirstOrDefault();
            SelectUnit = Units.FirstOrDefault();
            ActiveFrom = DateTime.Now;
            ActiveFrom2 = ActiveFrom ?? DateTime.Now;
        }

        public override void OnLoaded()
        {

        }

        private void Save()
        {
            try
            {
                if (SelectServiceType == null)
                {
                    WarningDialog("กรุณาเลือก Service Type");
                    return;
                }
                if (ItemsServiceSource != null)
                {
                    if (SelectedItemService == null)
                    {
                        WarningDialog("กรุณาเลือก Service Item");
                        return;
                    }
                }

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


                if (SelectBillingGroup == null)
                {
                    WarningDialog("กรุณาเลือก Billing Group");
                    return;
                }
                if (SelectBillingSubGroup == null)
                {
                    WarningDialog("กรุณาเลือก Billing Sub-Group");
                    return;
                }


                var billCodes = DataService.MasterData.GetBillableItemByCode(Code);
                billCodes = billCodes?.Where(p => p.BillableItemUID != model?.BillableItemUID).ToList();
                if (billCodes != null && billCodes.Count > 0)
                {
                    WarningDialog("Code ซ้ำ โปรดตรวจสอบ");
                    return;
                }

                if ( BillableItemDetail == null || BillableItemDetail.Count() <= 0)
                {
                    WarningDialog("กรุณาใส่ราคา");
                    return;
                }

                AssingPropertiesToModel();
                DataService.MasterData.ManageBillableItem(model, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ListBillableItem pageList = new ListBillableItem();
                ChangeViewPermission(pageList);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void Cancel()
        {
            ListBillableItem pageList = new ListBillableItem();
            ChangeViewPermission(pageList);
        }

        private void AddBillItemDetail()
        {
            if (SelectOrganisation == null)
            {
                WarningDialog("กรุณาเลือก สถานประกอบการ");
                return;
            }
            if (BillableItemDetail != null)
            {
                if (BillableItemDetail.Count(p => p.OwnerOrganisationUID == SelectOrganisation.HealthOrganisationUID) > 0)
                {
                    WarningDialog("สถานประกอบการซ้ำ กรุณาตรวจสอบ");
                    return;
                }
            }

            BillableItemDetailModel newBillItmDetail = new BillableItemDetailModel();
            newBillItmDetail.ActiveFrom = ActiveFrom2;
            newBillItmDetail.ActiveTo = ActiveTo2;

            newBillItmDetail.OwnerOrganisationUID = SelectOrganisation.HealthOrganisationUID;
            newBillItmDetail.OwnerOrganisationName = SelectOrganisation.Name;

            newBillItmDetail.Price = Price;
            newBillItmDetail.Cost = Cost;
            newBillItmDetail.CURNCUID = SelectUnit.Key.Value;
            newBillItmDetail.Unit = SelectUnit.Display;
            newBillItmDetail.StatusFlag = "A";
            BillableItemDetail.Add(newBillItmDetail);
            OnUpdateEvent();

            ClearBillItmdetail();
        }

        private void UpdateBillItemDetail()
        {
            if (SelectOrganisation == null)
            {
                WarningDialog("กรุณาเลือก สถานประกอบการ");
                return;
            }
            if (BillableItemDetail != null)
            {
                if (BillableItemDetail.Count(p => !p.Equals(SelectBillableItemDetail)
                    && p.OwnerOrganisationUID == SelectOrganisation.HealthOrganisationUID) > 0)
                {
                    WarningDialog("สถานประกอบการซ้ำ กรุณาตรวจสอบ");
                    return;
                }
            }
            if (SelectBillableItemDetail != null)
            {
                SelectBillableItemDetail.ActiveFrom = ActiveFrom2;
                SelectBillableItemDetail.ActiveTo = ActiveTo2;

                SelectBillableItemDetail.OwnerOrganisationUID = SelectOrganisation.HealthOrganisationUID;
                SelectBillableItemDetail.OwnerOrganisationName = SelectOrganisation.Name;

                SelectBillableItemDetail.Price = Price;
                SelectBillableItemDetail.Cost = Cost;
                SelectBillableItemDetail.CURNCUID = SelectUnit.Key.Value;
                SelectBillableItemDetail.Unit = SelectUnit.Display;
                SelectBillableItemDetail.MWhen = DateTime.Now;
                OnUpdateEvent();

                ClearBillItmdetail();
            }
        }


        private void DeleteBillItemDetail()
        {
            if (SelectBillableItemDetail != null)
            {
                SelectBillableItemDetail.StatusFlag = "D";
                //BillableItemDetail.Remove(SelectBillableItemDetail);
                OnUpdateEvent();

                ClearBillItmdetail();
            }
        }

        private void ClearBillItmdetail()
        {
            Price = 0;
            ActiveFrom2 = DateTime.Now;
            ActiveTo2 = null;
            SelectOrganisation = null;
            SelectBillableItemDetail = null;
        }

        BillableItemModel model;

        public void AssingModel(BillableItemModel modelData)
        {
            if (modelData.BillableItemDetails == null)
            {
                modelData = DataService.MasterData.GetBillableItemByUID(modelData.BillableItemUID);
            }
            model = modelData;
            AssingModelToProperties();
        }

        void AssingModelToProperties()
        {
            SelectServiceType = ServiceTypes != null ? ServiceTypes.FirstOrDefault(p => p.Key == model.BSMDDUID) : null;
            SelectedItemService = ItemsServiceSource != null ? ItemsServiceSource.FirstOrDefault(p => p.ItemUID == model.ItemUID) : null;
            SelectBillingGroup = BillingGroup != null ? BillingGroup.FirstOrDefault(p => p.BillingGroupUID == model.BillingGroupUID) : null;
            SelectBillingSubGroup = BillingSubGroup != null ? BillingSubGroup.FirstOrDefault(p => p.BillingSubGroupUID == model.BillingSubGroupUID) : null;

            if (SelectedItemService == null && (model.ItemUID ?? 0) != 0)
            {
                WarningDialog("ServiceItem ปิดการใช้งานแล้ว กรุณาเลือก ServiceItem ใหม่");
            }

            //SelectUnit = Units != null ? Units.FirstOrDefault(p => p.Key == model.CURNCUID) : null;
            IsShareDoctor = model.IsShareDoctor == "Y" ? true : false;
            Code = model.Code;
            Name = model.ItemName;
            Description = model.Description;
            DoctorFee = model.DoctorFee;
            ActiveFrom = model.ActiveFrom;
            Comments = model.Comments;
            //Price = model.Price;
            ActiveTo = model.ActiveTo;
            var roleOrgan = Organisations;
            BillableItemDetail = new ObservableCollection<BillableItemDetailModel>(
                from j in roleOrgan
                join i in model.BillableItemDetails on j.HealthOrganisationUID equals i.OwnerOrganisationUID
                select i
                );
        }

        void AssingPropertiesToModel()
        {
            if (model == null)
            {
                model = new BillableItemModel();
            }
            model.Code = Code;
            model.ItemName = Name;
            model.Description = Description;
            model.BSMDDUID = SelectServiceType.Key.Value;

            if (SelectedItemService != null)
                model.ItemUID = SelectedItemService.ItemUID;

            //if (SelectOrganisation != null)
            //    model.OwnerOrganisationUID = SelectOrganisation.HealthOrganisationUID;
            //else
            //    model.OwnerOrganisationUID = null;

            model.BillingGroupUID = SelectBillingGroup.BillingGroupUID;
            model.BillingSubGroupUID = SelectBillingSubGroup.BillingSubGroupUID;
            //model.CURNCUID = SelectUnit.Key;
            model.DoctorFee = DoctorFee;
            //model.Price = Price;
            model.Comments = Comments;
            model.ActiveFrom = ActiveFrom;
            model.ActiveTo = ActiveTo;
            model.IsShareDoctor = IsShareDoctor == true ? "Y" : "N";
            model.BillableItemDetails = BillableItemDetail.ToList();
        }



        #endregion


    }
}
