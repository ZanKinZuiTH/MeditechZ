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
    public class SearchRequestViewModel : MediTechViewModelBase
    {
        #region Properties

        public List<LocationModel> LocationFormData { get; set; }
        public List<LocationModel> LocationToData { get; set; }

        private bool _IsEnableEdit = false;

        public bool IsEnableEdit
        {
            get { return _IsEnableEdit; }
            set { Set(ref _IsEnableEdit, value); }
        }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public string RequestNo { get; set; }

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

        private List<ItemRequestModel> _ItemRequests;

        public List<ItemRequestModel> ItemRequests
        {
            get { return _ItemRequests; }
            set { Set(ref _ItemRequests, value); }
        }

        private ItemRequestModel _SelectItemRequest;

        public ItemRequestModel SelectItemRequest
        {
            get { return _SelectItemRequest; }
            set
            {
                Set(ref _SelectItemRequest, value);
                if (_SelectItemRequest != null)
                {

                    ItemRequestDetails = DataService.Inventory.GetItemRequestDetailByItemRequestUID(SelectItemRequest.ItemRequestUID);
                    if (_SelectItemRequest.RQSTSUID == 2927 || _SelectItemRequest.RQSTSUID == 2928|| _SelectItemRequest.RQSTSUID == 2929)
                    {
                        IsEnableEdit = false;
                    }
                    else
                    {
                        IsEnableEdit = true;
                    }
                }
            }
        }

        private List<ItemRequestDetailModel> _ItemRequestDetails;

        public List<ItemRequestDetailModel> ItemRequestDetails
        {
            get { return _ItemRequestDetails; }
            set { Set(ref _ItemRequestDetails, value); }
        }

        private ItemRequestDetailModel _SelectItemRequestDetail;

        public ItemRequestDetailModel SelectItemRequestDetail
        {
            get { return _SelectItemRequestDetail; }
            set { Set(ref _SelectItemRequestDetail, value); }
        }

        public List<LookupReferenceValueModel> RequestStatus { get; set; }
        private LookupReferenceValueModel _SelectRequestStatus;

        public LookupReferenceValueModel SelectRequestStatus
        {
            get { return _SelectRequestStatus; }
            set { Set(ref _SelectRequestStatus, value); }
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


        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                    ?? (_SaveCommand = new RelayCommand(Save));
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

        #endregion

        #region Method

        public SearchRequestViewModel()
        {
            var refData = DataService.Technical.GetReferenceValueList("RQSTS,IRPRI");
            RequestStatus = refData.Where(p => p.DomainCode == "RQSTS").ToList();
            var organ = GetHealthOrganisationIsStock();
            OrganisationsFrom = organ;
            OrganisationsTo = GetHealthOrganisationIsRoleStock();
            //DateFrom = DateTime.Now;

            if (RequestStatus != null)
            {
                SelectRequestStatus = RequestStatus.FirstOrDefault(p => p.ValueCode == "RAISED");
            }

        }

        public override void OnLoaded()
        {
            Search();
        }
        private void Save()
        {
            CloseViewDialog(ActionDialog.Save);
        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }
        private void Search()
        {
            ItemRequests = null;
            ItemRequestDetails = null;
            int? organisationUID = SelectOrganisationFrom != null ? SelectOrganisationFrom.HealthOrganisationUID : (int?)null;
            int? locationUID = SelectLocationFrom != null ? SelectLocationFrom.LocationUID : (int?)null;
            int? organisationToUID = SelectOrganisationTo != null ? SelectOrganisationTo.HealthOrganisationUID : (int?)null;
            int? locationToUID = SelectLocationTo != null ? SelectLocationTo.LocationUID : (int?)null;
            int? requestStatus = SelectRequestStatus != null ? SelectRequestStatus.Key : (int?)null;
            int? priority = null;
            ItemRequests = DataService.Inventory.SearchItemRequest(DateFrom, DateTo, RequestNo, organisationUID, locationUID, organisationToUID, locationToUID, requestStatus, priority);
        }


        private void Clear()
        {
            DateFrom = DateTime.Now;
            DateTo = null;
            RequestNo = string.Empty;
            SelectOrganisationFrom = null;
            SelectOrganisationTo = null;
        }

        #endregion
    }
}
