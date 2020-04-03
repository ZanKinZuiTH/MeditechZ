using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Reports.Operating.Inventory;
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
    public class ListItemRequestViewModel : MediTechViewModelBase
    {

        #region DataService

        MasterDataService masterData;
        TechnicalService technicalData;
        InventoryService inventoryData;

        #endregion

        #region Properties
        private bool _IsEnableEdit = false;

        public bool IsEnableEdit
        {
            get { return _IsEnableEdit; }
            set { Set(ref _IsEnableEdit , value); }
        }

        private bool _IsEnableCancel = false;

        public bool IsEnableCancel
        {
            get { return _IsEnableCancel; }
            set { Set(ref _IsEnableCancel, value); }
        }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public string RequestNo { get; set; }

        public List<HealthOrganisationModel> Organisations { get; set; }
        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set
            {
                Set(ref _SelectOrganisation, value);
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

        public List<LookupReferenceValueModel> RequestStatus { get; set; }
        private LookupReferenceValueModel _SelectRequestStatus;

        public LookupReferenceValueModel SelectRequestStatus
        {
            get { return _SelectRequestStatus; }
            set { Set(ref _SelectRequestStatus, value); }
        }

        public List<LookupReferenceValueModel> Prioritys { get; set; }
        private LookupReferenceValueModel _SelectPriority;

        public LookupReferenceValueModel SelectPriority
        {
            get { return _SelectPriority; }
            set { Set(ref _SelectPriority, value); }
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
                    ItemRequestDetails = inventoryData.GetItemRequestDetailByItemRequestUID(SelectItemRequest.ItemRequestUID);
                    if (SelectItemRequest.RQSTSUID == 2927 || SelectItemRequest.RQSTSUID == 2929 || SelectItemRequest.RQSTSUID == 2928)
                    {
                        IsEnableCancel = false;
                        IsEnableEdit = false;
                    }
                    else
                    {
                        IsEnableCancel = true;
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



        private RelayCommand _EditCommand;

        public RelayCommand EditCommand
        {
            get
            {
                return _EditCommand
                    ?? (_EditCommand = new RelayCommand(Edit));
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

        public ListItemRequestViewModel()
        {
            masterData = new MasterDataService();
            inventoryData = new InventoryService();
            technicalData = new TechnicalService();
            var refData = technicalData.GetReferenceValueList("RQSTS,IRPRI");
            RequestStatus = refData.Where(p => p.DomainCode == "RQSTS").ToList();
            Prioritys = refData.Where(p => p.DomainCode == "IRPRI").ToList();
            var organ = GetHealthOrganisationIsStock();
            Organisations = GetHealthOrganisationIsRoleStock();
            OrganisationsTo = organ;
            //DateFrom = DateTime.Now;

            if (RequestStatus != null)
            {
                SelectRequestStatus = RequestStatus.FirstOrDefault(p => p.ValueCode == "RAISED");
            }
            //if (Organisations != null)
            //{
            //    SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            //}

        }

        public override void OnLoaded()
        {
            Search();
        }
        private void Add()
        {
            ManageItemRequest view = new ManageItemRequest();
            ChangeViewPermission(view);
        }
        private void Edit()
        {
            if (SelectItemRequest != null)
            {
                ManageItemRequest managePage = new ManageItemRequest();
                var EditData = inventoryData.GetItemRequestByUID(SelectItemRequest.ItemRequestUID);
                (managePage.DataContext as ManageItemRequestViewModel).AssignModel(EditData);
                ChangeViewPermission(managePage);
            }
        }
        private void Canecl()
        {
            if (SelectItemRequest != null)
            {
                try
                {
                    CancelPopup cancelPO = new CancelPopup();
                    CancelPopupViewModel result = (CancelPopupViewModel)LaunchViewDialog(cancelPO,"CANREQ", true);
                    if (result != null && result.ResultDialog == ActionDialog.Save)
                    {
                        inventoryData.CancelItemRequest(SelectItemRequest.ItemRequestUID, result.Comments, AppUtil.Current.UserID);
                        SaveSuccessDialog();
                        SelectItemRequest.RQSTSUID = 2929;
                        SelectItemRequest.RequestStatus = RequestStatus.FirstOrDefault(p => p.Key == 2929).Display;
                        OnUpdateEvent();
                        SelectItemRequest = null;
                    }
                }
                catch (Exception er)
                {

                    ErrorDialog(er.Message);
                }

            }
        }
        private void Print()
        {
            if (SelectItemRequest != null)
            {
                StockRequestReport rpt = new StockRequestReport();
                rpt.Parameters["RequestID"].Value = SelectItemRequest.ItemRequestID;
                ReportPrintTool printTool = new ReportPrintTool(rpt);
                rpt.ShowPrintMarginsWarning = false;
                printTool.ShowPreviewDialog();
            }
        }


        private void Search()
        {
            ItemRequests = null;
            ItemRequestDetails = null;
            int? organisationUID = SelectOrganisation != null ? SelectOrganisation.HealthOrganisationUID : (int?)null;
            int? organisationToUID = SelectOrganisationTo != null ? SelectOrganisationTo.HealthOrganisationUID : (int?)null;
            int? requestStatus = SelectRequestStatus != null ? SelectRequestStatus.Key : (int?)null;
            int? priority = SelectPriority != null ? SelectPriority.Key : (int?)null;
            ItemRequests = inventoryData.SearchItemRequest(DateFrom, DateTo, RequestNo, organisationUID, organisationToUID, requestStatus, priority);
        }


        private void Clear()
        {
            DateFrom = DateTime.Now;
            DateTo = null;
            RequestNo = string.Empty;
            SelectOrganisation = null;
            SelectPriority = null;
            SelectRequestStatus = null;
            SelectOrganisationTo = null;
        }


        #endregion
    }
}
