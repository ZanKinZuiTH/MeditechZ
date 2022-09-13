using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;


namespace MediTech.ViewModels
{
    public class ListItemReceiveViewModel : MediTechViewModelBase
    {

        #region Properties
        public List<LocationModel> LocationFormData { get; set; }
        public List<LocationModel> LocationToData { get; set; }

        private bool _IsEnableCancel = false;

        public bool IsEnableCancel
        {
            get { return _IsEnableCancel; }
            set { Set(ref _IsEnableCancel, value); }
        }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public string ReceiveNo { get; set; }

        public List<LookupReferenceValueModel> ReceiveStatus { get; set; }

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
                if (_SelectLocationFrom != null)
                {
                    LocationTo = LocationToData.Where(p => p.LocationUID != SelectLocationFrom.LocationUID).ToList();
                }
            }
        }

        public List<HealthOrganisationModel> OrganisationsTo { get; set; }
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
                if (_SelectLocationTo != null)
                {
                    LocationFrom = LocationFormData.Where(p => p.LocationUID != SelectLocationTo.LocationUID).ToList();
                }
            }
        }

        private List<ItemReceiveModel> _ItemReceives;

        public List<ItemReceiveModel> ItemReceives
        {
            get { return _ItemReceives; }
            set { Set(ref _ItemReceives, value); }
        }

        private ItemReceiveModel _SelectItemReceive;

        public ItemReceiveModel SelectItemReceive
        {
            get { return _SelectItemReceive; }
            set
            {
                Set(ref _SelectItemReceive, value);
                if (_SelectItemReceive != null)
                {
                    ItemReceiveDetails = DataService.Inventory.GetItemReceiveDetailByItemReceiveUID(SelectItemReceive.ItemRecieveUID);
                    if (SelectItemReceive.RCSTSUID == 2932)
                    {
                        IsEnableCancel = false;
                    }
                    else
                    {
                        IsEnableCancel = true;
                    }
                }
            }
        }

        private List<ItemReceiveDetailModel> _ItemReceiveDetails;

        public List<ItemReceiveDetailModel> ItemReceiveDetails
        {
            get { return _ItemReceiveDetails; }
            set { Set(ref _ItemReceiveDetails, value); }
        }

        private ItemReceiveDetailModel _SelectItemReceiveDetail;

        public ItemReceiveDetailModel SelectItemReceiveDetail
        {
            get { return _SelectItemReceiveDetail; }
            set { Set(ref _SelectItemReceiveDetail, value); }
        }

        #endregion

        #region Command

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get
            {
                return _SearchCommand
                    ?? (_SearchCommand = new RelayCommand(Search));
            }

        }

        private RelayCommand _ClearCommand;

        public RelayCommand ClearCommand
        {
            get
            {
                return _ClearCommand
                    ?? (_ClearCommand = new RelayCommand(Clear));
            }

        }



        private RelayCommand _PrintCommand;

        public RelayCommand PrintCommand
        {
            get
            {
                return _PrintCommand
                    ?? (_PrintCommand = new RelayCommand(Print));
            }

        }


        private RelayCommand _AddCommand;

        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand
                    ?? (_AddCommand = new RelayCommand(Add));
            }

        }





        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get
            {
                return _CancelCommand
                    ?? (_CancelCommand = new RelayCommand(Canecl));
            }

        }

        #endregion

        #region Method

        public ListItemReceiveViewModel()
        {
            var Organis = GetHealthOrganisationIsStock();
            ReceiveStatus = DataService.Technical.GetReferenceValueMany("RCSTS");
            OrganisationsTo = GetHealthOrganisationIsRoleStock();
            OrganisationsFrom = Organis;
            DateFrom = DateTime.Now;

            if (OrganisationsTo != null)
            {
                SelectOrganisationTo= OrganisationsTo.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            }

        }

        public override void OnLoaded()
        {
            Search();
        }
        private void Add()
        {
            ManageItemReceive view = new ManageItemReceive();
            ChangeViewPermission(view);
        }

        private void Print()
        {

        }
        private void Edit()
        {
            if (SelectItemReceive != null)
            {
                ManageItemReceive managePage = new ManageItemReceive();
                var EditData = DataService.Inventory.GetItemReceiveByUID(SelectItemReceive.ItemRecieveUID);
                (managePage.DataContext as ManageItemReceiveViewModel).AssignModel(EditData);
                ChangeViewPermission(managePage);
            }
        }

        private void Canecl()
        {
            if (SelectItemReceive != null)
            {
                try
                {
                    if (!DataService.Inventory.CheckCancelReceive(SelectItemReceive.ItemRecieveUID).IsActive)
                    {
                        WarningDialog("ไม่สามารถยกเลิกรายการรับสินค้านี้ได้เนื่องจากมีการนำคลังสินค้าไปใช้งานแล้ว");
                        return;
                    }
                    CancelPopup cancelPO = new CancelPopup();
                    CancelPopupViewModel result = (CancelPopupViewModel)LaunchViewDialog(cancelPO, "CANREC", true);
                    if (result != null && result.ResultDialog == ActionDialog.Save)
                    {
                        DataService.Inventory.CancelItemReceive(SelectItemReceive.ItemRecieveUID, result.Comments, AppUtil.Current.UserID);
                        SaveSuccessDialog();
                        SelectItemReceive.RCSTSUID = 2932;
                        SelectItemReceive.ReceiveStatus = ReceiveStatus.FirstOrDefault(p => p.Key == 2932).Display;
                        OnUpdateEvent();
                        SelectItemReceive = null;
                    }
                }
                catch (Exception er)
                {

                    ErrorDialog(er.Message);
                }

            }
        }

        private void Search()
        {
            ItemReceives = null;
            ItemReceiveDetails = null;
            int? organisationUIDFrom = SelectOrganisationFrom != null ? SelectOrganisationFrom.HealthOrganisationUID : (int?)null;
            int? locationUIDFrom = SelectLocationFrom != null ? SelectLocationFrom.LocationUID : (int?)null;
            int? organisationUIDTo = SelectOrganisationTo != null ? SelectOrganisationTo.HealthOrganisationUID : (int?)null;
            int? locationUIDTo = SelectLocationTo != null ? SelectLocationTo.LocationUID : (int?)null;
            ItemReceives = DataService.Inventory.SearchItemReceive(DateFrom, DateTo, ReceiveNo, organisationUIDFrom, locationUIDFrom, organisationUIDTo, locationUIDTo);
        }


        private void Clear()
        {
            DateFrom = DateTime.Now;
            DateTo = null;
            SelectOrganisationFrom = null;
            SelectOrganisationTo = null;
            ReceiveNo = string.Empty;
        }

        #endregion
    }
}
