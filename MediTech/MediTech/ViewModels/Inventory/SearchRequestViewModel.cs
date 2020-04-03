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
            int? organisationToUID = SelectOrganisationTo != null ? SelectOrganisationTo.HealthOrganisationUID : (int?)null;
            int? requestStatus = SelectRequestStatus != null ? SelectRequestStatus.Key : (int?)null;
            int? priority = null;
            ItemRequests = DataService.Inventory.SearchItemRequest(DateFrom, DateTo, RequestNo, organisationUID, organisationToUID, requestStatus, priority);
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
