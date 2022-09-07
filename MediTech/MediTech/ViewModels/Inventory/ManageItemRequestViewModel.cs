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
namespace MediTech.ViewModels
{
    public class ManageItemRequestViewModel : MediTechViewModelBase
    {

        #region Properites
        public List<LocationModel> LocationFormData { get; set; }
        public List<LocationModel> LocationToData { get; set; }

        public List<HealthOrganisationModel> OrganisationsFrom { get; set; }
        private HealthOrganisationModel _SelectOrganisationFrom;


        public HealthOrganisationModel SelectOrganisationFrom
        {
            get { return _SelectOrganisationFrom; }
            set
            {
                Set(ref _SelectOrganisationFrom, value);
                if (_SelectOrganisationFrom != null)
                {
                    LocationFormData = DataService.MasterData.GetLocationByOrganisationUID(SelectOrganisationFrom.HealthOrganisationUID);

                    LocationFrom = LocationFormData;

                    if (SelectLocationTo != null)
                        LocationFrom = LocationFormData.Where(p => p.LocationUID != SelectLocationTo.LocationUID).ToList();
                }
            }
        }

        private List<LocationModel> _LocationFrom;

        public List<LocationModel> LocationFrom
        {
            get { return _LocationFrom; }
            set { Set(ref _LocationFrom, value); }
        }

        private LocationModel _SelectLocationFrom;

        public LocationModel SelectLocationFrom
        {
            get { return _SelectLocationFrom; }
            set
            {
                Set(ref _SelectLocationFrom, value);
                StoresFrom = null;
                if (_SelectLocationFrom != null)
                {
                    StoresFrom = DataService.Inventory.GetStoreByLocationUID(_SelectLocationFrom.LocationUID);
                    LocationTo = LocationToData.Where(p => p.LocationUID != SelectLocationFrom.LocationUID).ToList();
                }
            }
        }

        private List<StoreModel> _StoresFrom;

        public List<StoreModel> StoresFrom
        {
            get { return _StoresFrom; }
            set { Set(ref _StoresFrom, value); }
        }

        private StoreModel _SelectStoreFrom;

        public StoreModel SelectStoreFrom
        {
            get { return _SelectStoreFrom; }
            set
            {
                Set(ref _SelectStoreFrom, value);

            }
        }

        private List<HealthOrganisationModel> _OrganisationsTo;

        public List<HealthOrganisationModel> OrganisationsTo
        {
            get { return _OrganisationsTo; }
            set { Set(ref _OrganisationsTo, value); }
        }
        private HealthOrganisationModel _SelectOrganisationTo;

        public HealthOrganisationModel SelectOrganisationTo
        {
            get { return _SelectOrganisationTo; }
            set
            {
                Set(ref _SelectOrganisationTo, value);
                if (_SelectOrganisationTo != null)
                {
                    LocationToData = DataService.MasterData.GetLocationByOrganisationUID(_SelectOrganisationTo.HealthOrganisationUID);
                    LocationTo = LocationToData;
                    if (SelectLocationFrom != null)
                        LocationTo = LocationToData.Where(p => p.LocationUID != SelectLocationFrom.LocationUID).ToList();
                }

            }
        }

        private List<LocationModel> _LocationTo;

        public List<LocationModel> LocationTo
        {
            get { return _LocationTo; }
            set { Set(ref _LocationTo, value); }
        }

        private LocationModel _SelectLocationTo;

        public LocationModel SelectLocationTo
        {
            get { return _SelectLocationTo; }
            set
            {
                Set(ref _SelectLocationTo, value);
                StoresTo = null;
                if (_SelectLocationTo != null)
                {
                    StoresTo = DataService.Inventory.GetStoreByLocationUID(_SelectLocationTo.LocationUID);
                    LocationFrom = LocationFormData.Where(p => p.LocationUID != SelectLocationTo.LocationUID).ToList();
                }
            }
        }

        private List<StoreModel> _StoresTo;

        public List<StoreModel> StoresTo
        {
            get { return _StoresTo; }
            set
            {
                Set(ref _StoresTo, value);
            }
        }

        private StoreModel _SelectStoreTo;

        public StoreModel SelectStoreTo
        {
            get { return _SelectStoreTo; }
            set
            {
                Set(ref _SelectStoreTo, value);
                if (_SelectStoreTo != null)
                {
                    ItemMasters = DataService.Inventory.GetItemMasterQtyByStore(SelectStoreTo.StoreUID);
                }
            }
        }

        public List<VendorDetailModel> Vendors { get; set; }
        private VendorDetailModel _SelectVendor;

        public VendorDetailModel SelectVendor
        {
            get { return _SelectVendor; }
            set { Set(ref _SelectVendor, value); }
        }

        private List<ItemMasterModel> _ItemMasters;

        public List<ItemMasterModel> ItemMasters
        {
            get { return _ItemMasters; }
            set { Set(ref _ItemMasters, value); }
        }

        private ObservableCollection<ItemMasterList> _ItemRequestDetail;

        public ObservableCollection<ItemMasterList> ItemRequestDetail
        {
            get { return _ItemRequestDetail; }
            set
            {
                Set(ref _ItemRequestDetail, value);
            }
        }

        private ItemMasterList _SelectItemRequestDetail;

        public ItemMasterList SelectItemRequestDetail
        {
            get { return _SelectItemRequestDetail; }
            set
            {
                Set(ref _SelectItemRequestDetail, value);
            }
        }

        public List<LookupReferenceValueModel> Prioritys { get; set; }
        private LookupReferenceValueModel _SelectPriority;

        public LookupReferenceValueModel SelectPriority
        {
            get { return _SelectPriority; }
            set { Set(ref _SelectPriority, value); }
        }

        public List<LookupReferenceValueModel> RequestStatus { get; set; }
        private LookupReferenceValueModel _SelectRequestStatus;

        public LookupReferenceValueModel SelectRequestStatus
        {
            get { return _SelectRequestStatus; }
            set { Set(ref _SelectRequestStatus, value); }
        }

        private DateTime _RequestDate;

        public DateTime RequestDate
        {
            get { return _RequestDate; }
            set { Set(ref _RequestDate, value); }
        }


        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { Set(ref _Comments, value); }
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



        #endregion

        #region Varible

        List<HealthOrganisationModel> HealthOrganisations;

        #endregion

        #region Method
        ItemRequestModel model;
        public ManageItemRequestViewModel()
        {
            ItemRequestDetail = new ObservableCollection<ItemMasterList>();
            var refData = DataService.Technical.GetReferenceValueList("IRPRI,RQSTS");
            Prioritys = refData.Where(p => p.DomainCode == "IRPRI").ToList();
            SelectPriority = Prioritys.FirstOrDefault(p => p.Key == 2923);
            RequestStatus = refData.Where(p => p.DomainCode == "RQSTS").ToList();
            SelectRequestStatus = RequestStatus.FirstOrDefault(p => p.Key == 2926); ;
            HealthOrganisations = GetHealthOrganisationIsStock();
            OrganisationsFrom = GetHealthOrganisationIsRoleStock();
            OrganisationsTo = HealthOrganisations;
            Vendors = DataService.Purchaseing.GetVendorDetail();
            Vendors = Vendors.Where(p => p.MNFTPUID == 2937).ToList();
            RequestDate = DateTime.Now;

            if (OrganisationsFrom != null)
            {
                SelectOrganisationFrom = OrganisationsFrom.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            }
        }

        private void Save()
        {
            try
            {
                if (SelectOrganisationFrom == null)
                {
                    WarningDialog("กรุณาระบุ สถานประกอบการที่ร้องขอ");
                    return;
                }

                if (SelectOrganisationTo == null)
                {
                    WarningDialog("กรุณาระบุ Store ที่ร้องขอ");
                    return;
                }

                if (SelectLocationFrom == null)
                {
                    WarningDialog("กรุณาระบุ แผนกที่ร้องขอ");
                    return;
                }

                if (SelectLocationTo == null)
                {
                    WarningDialog("กรุณาระบุ แผนกที่ถูกร้องขอ");
                    return;
                }

                if (SelectStoreFrom == null)
                {
                    WarningDialog("กรุณาระบุ สถานประกอบการที่ถูกร้องขอ");
                    return;
                }



                if (SelectStoreTo == null)
                {
                    WarningDialog("กรุณาระบุ Store ที่ต้องการถูกร้องขอ");
                    return;
                }
                if (RequestDate == null)
                {
                    WarningDialog("กรุณาใส่วันที่ ร้องขอ");
                    return;
                }

                if (ItemRequestDetail == null || ItemRequestDetail.Count <= 0)
                {
                    WarningDialog("กรุณาใส่รายการสินค้า");
                    return;
                }

                AssingPropertiesToModel();
                DataService.Inventory.ManageItemRequest(model, AppUtil.Current.UserID);
                SaveSuccessDialog();

                ListItemRequest view = new ListItemRequest();
                ChangeView_CloseViewDialog(view, ActionDialog.Save);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void Cancel()
        {
            ListItemRequest view = new ListItemRequest();
            ChangeView_CloseViewDialog(view, ActionDialog.Cancel);
        }

        public void AssignModel(ItemRequestModel model)
        {
            this.model = model;
            AssingModelToProperties();
        }

        public void AssingModelToProperties()
        {
            SelectOrganisationFrom = OrganisationsFrom.FirstOrDefault(p => p.HealthOrganisationUID == model.OrganisationUID);
            SelectLocationFrom = LocationFrom.FirstOrDefault(p => p.LocationUID == model.LocationUID);
            SelectStoreFrom = StoresFrom.FirstOrDefault(p => p.StoreUID == model.StoreUID);
            SelectOrganisationTo = OrganisationsTo.FirstOrDefault(p => p.HealthOrganisationUID == model.RequestOnOrganistaionUID);
            SelectLocationTo = LocationTo.FirstOrDefault(p => p.LocationUID == model.RequestOnLocationUID);
            SelectStoreTo = StoresTo.FirstOrDefault(p => p.StoreUID == model.RequestOnStoreUID);
            RequestDate = model.RequestedDttm;
            SelectVendor = Vendors.FirstOrDefault(p => p.VendorDetailUID == (model.PreferredVendorUID ?? 0));
            SelectRequestStatus = RequestStatus.FirstOrDefault(p => p.Key == model.RQSTSUID);
            SelectPriority = Prioritys.FirstOrDefault(p => p.Key == model.IRPRIUID);
            Comments = model.Comments;

            foreach (var item in model.ItemRequestDetail)
            {
                ItemMasterList newRow = new ItemMasterList();
                newRow.ItemListUID = item.ItemRequestDetailUID;
                newRow.ItemMasterUID = item.ItemMasterUID;
                newRow.ItemCode = item.ItemCode;
                newRow.ItemName = item.ItemName;
                newRow.Quantity = item.Quantity;
                newRow.IMUOMUID = item.IMUOMUID;
                ItemRequestDetail.Add(newRow);
            }
        }

        public void AssingPropertiesToModel()
        {
            if (model == null)
            {
                model = new ItemRequestModel();
            }
            model.OrganisationUID = SelectOrganisationFrom.HealthOrganisationUID;
            model.LocationUID = SelectLocationFrom.LocationUID;
            model.StoreUID = SelectStoreFrom.StoreUID;
            model.RequestOnOrganistaionUID = SelectOrganisationTo.HealthOrganisationUID;
            model.RequestOnLocationUID = SelectLocationTo.LocationUID;
            model.RequestOnStoreUID = SelectStoreTo.StoreUID;
            model.RequestedDttm = RequestDate;
            model.PreferredVendorUID = SelectVendor != null ? SelectVendor.VendorDetailUID : (int?)null;
            model.RQSTSUID = SelectRequestStatus.Key;
            model.IRPRIUID = SelectPriority.Key;
            model.Comments = Comments;

            model.ItemRequestDetail = new List<ItemRequestDetailModel>();
            foreach (var item in ItemRequestDetail)
            {
                ItemRequestDetailModel newRow = new ItemRequestDetailModel();
                newRow.ItemRequestDetailUID = item.ItemListUID;
                newRow.ItemMasterUID = item.ItemMasterUID;
                newRow.Quantity = item.Quantity ?? 0;
                newRow.IMUOMUID = item.IMUOMUID;
                newRow.ItemCode = item.ItemCode;
                newRow.ItemName = item.ItemName;
                model.ItemRequestDetail.Add(newRow);
            }
        }

        #endregion
    }
}
