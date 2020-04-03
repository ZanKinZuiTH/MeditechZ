using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediTech.Model;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using MediTech.DataService;
using MediTech.Views;
using System.Windows.Forms;
using MediTech.Reports.Operating.Inventory;
using DevExpress.XtraReports.UI;

namespace MediTech.ViewModels
{
    public class ListGRNViewModel : MediTechViewModelBase
    {
        #region Properties

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        private string _InvoinceNo;

        public string InvoinceNo
        {
            get { return _InvoinceNo; }
            set { Set(ref _InvoinceNo, value); }
        }

        private string _GRNNumber;

        public string GRNNumber
        {
            get { return _GRNNumber; }
            set { Set(ref _GRNNumber, value); }
        }
        

        public List<HealthOrganisationModel> Organisations { get; set; }
        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set
            {
                Set(ref _SelectOrganisation, value);
                if (_SelectOrganisation != null)
                {
                    Stores = DataService.Inventory.GetStoreByOrganisationUID(_SelectOrganisation.HealthOrganisationUID);
                }
                else
                {
                    Stores = null;
                }
            }
        }


        private List<StoreModel> _Stores;

        public List<StoreModel> Stores
        {
            get { return _Stores; }
            set { Set(ref _Stores, value); }
        }
        private StoreModel _SelectStore;

        public StoreModel SelectStore
        {
            get { return _SelectStore; }
            set { Set(ref _SelectStore, value); }
        }

        public List<VendorDetailModel> Vendors { get; set; }
        private VendorDetailModel _SelectVendor;

        public VendorDetailModel SelectVendor
        {
            get { return _SelectVendor; }
            set { Set(ref _SelectVendor, value); }
        }


        private List<GRNDetailModel> _GRNDetails;

        public List<GRNDetailModel> GRNDetails
        {
            get { return _GRNDetails; }
            set { Set(ref _GRNDetails, value); }
        }

        private GRNDetailModel _SelectedGRNDetail;

        public GRNDetailModel SelectGRNDetail
        {
            get { return _SelectedGRNDetail; }
            set
            {
                Set(ref _SelectedGRNDetail, value);
                if (_SelectedGRNDetail != null)
                {
                    GRNItemLists = DataService.Purchaseing.GetGoodReceiveItemByGRNDetailUID(SelectGRNDetail.GRNDetailUID);
                    if (SelectGRNDetail.GRNStatus == "ยกเลิก")
                    {
                        IsEditGRN = false;
                        IsCancelGRN = false;
                    }
                    else
                    {
                        IsEditGRN = true;
                        IsCancelGRN = true;
                    }
                }
                else
                {
                    GRNItemLists = null;
                }
            }
        }

        private List<GRNItemListModel> _GRNItemLists;

        public List<GRNItemListModel> GRNItemLists
        {
            get { return _GRNItemLists; }
            set { Set(ref _GRNItemLists, value); }
        }

        private GRNItemListModel _SelectGRNItemList;

        public GRNItemListModel SelectGRNItemList
        {
            get { return _SelectGRNItemList; }
            set { Set(ref _SelectGRNItemList, value); }
        }

        private bool _IsEditGRN = false;

        public bool IsEditGRN
        {
            get { return _IsEditGRN; }
            set {Set(ref _IsEditGRN , value); }
        }

        private bool _IsCancelGRN = false;

        public bool IsCancelGRN
        {
            get { return _IsCancelGRN; }
            set { Set(ref _IsCancelGRN, value); }
        }

        private List<LookupReferenceValueModel> _GRNStatus;

        public List<LookupReferenceValueModel> GRNStatus
        {
            get { return _GRNStatus; }
            set { _GRNStatus = value; }
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

        private RelayCommand _DeleteCommand;

        public RelayCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand
                    ?? (_DeleteCommand = new RelayCommand(Cancel));
            }

        }

        #endregion

        #region Method

        public ListGRNViewModel()
        {
            DateFrom = DateTime.Now.Date;
            Organisations = GetHealthOrganisationIsRoleStock();
            Vendors = DataService.Purchaseing.GetVendorDetail();
            Vendors = Vendors.Where(p => p.MNFTPUID == 2937).ToList();
            GRNStatus = DataService.Technical.GetReferenceValueMany("GRNSTS");

            if (Organisations != null)
            {
                SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            }

           
        }

        public override void OnLoaded()
        {
            Search();
        }
        private void Search()
        {
            GRNDetails = null;
            GRNItemLists = null;
            SelectGRNDetail = null;
            SelectGRNItemList = null;
            int? organisationUID = SelectOrganisation != null ? SelectOrganisation.HealthOrganisationUID : (int?)null;
            int? storeUID = SelectStore != null ? SelectStore.StoreUID : (int?)null;
            int? vendorDetailUID = SelectVendor != null ? SelectVendor.VendorDetailUID : (int?)null;
            GRNDetails = DataService.Purchaseing.SearchGoodReceive(DateFrom, DateTo, organisationUID, storeUID, vendorDetailUID, InvoinceNo, GRNNumber);

        }

        private void Clear()
        {
            DateFrom = DateTime.Now;
            DateTo = null;
            SelectVendor = null;
            SelectOrganisation = null;
            SelectStore = null;
            InvoinceNo = string.Empty;
            GRNNumber = string.Empty;
        }

        private void Print()
        {
            if (SelectGRNDetail != null)
            {
                GoodReceiveReport rpt = new GoodReceiveReport();
                rpt.Parameters["GRNNumber"].Value = SelectGRNDetail.GRNNumber;
                ReportPrintTool printTool = new ReportPrintTool(rpt);
                rpt.ShowPrintMarginsWarning = false;
                printTool.ShowPreviewDialog();
            }
        }
        private void Add()
        {
            ManageGRN managePage = new ManageGRN();
            ChangeViewPermission(managePage);
        }

        private void Edit()
        {
            if (SelectGRNDetail != null)
            {
                if (!DataService.Purchaseing.CheckCancelGoodReceive(SelectGRNDetail.GRNDetailUID).IsActive)
                {
                    WarningDialog("ไม่สามารถแก้ไขรายการรับสินค้านี้ได้เนื่องจากมีการนำคลังสินค้าไปใช้งานแล้ว");
                    return;
                }

                CancelPopup cancelPO = new CancelPopup();
                CancelPopupViewModel result = (CancelPopupViewModel)LaunchViewDialog(cancelPO, "CANGRN", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    DataService.Purchaseing.CancelGoodReceive(SelectGRNDetail.GRNDetailUID, result.Comments, AppUtil.Current.UserID);
                    SaveSuccessDialog();
                    SelectGRNDetail.GRNSTSUID = 2912;
                    SelectGRNDetail.GRNStatus = GRNStatus.FirstOrDefault(p => p.Key == 2912).Display;
                }

                ManageGRN managePage = new ManageGRN();
                var grnItemList = DataService.Purchaseing.GetGoodReceiveItemByGRNDetailUID(SelectGRNDetail.GRNDetailUID);
                SelectGRNDetail.GRNItemLists = grnItemList;
                (managePage.DataContext as ManageGRNViewModel).AssignModel(SelectGRNDetail,true);
                ChangeViewPermission(managePage);
            }

        }

        private void Cancel()
        {

            if (SelectGRNDetail != null)
            {
                try
                {
                    if (!DataService.Purchaseing.CheckCancelGoodReceive(SelectGRNDetail.GRNDetailUID).IsActive)
                    {
                        WarningDialog("ไม่สามารถยกเลิกรายการรับสินค้านี้ได้เนื่องจากมีการนำคลังสินค้าไปใช้งานแล้ว");
                        return;
                    }
                    CancelPopup cancelPO = new CancelPopup();
                    CancelPopupViewModel result = (CancelPopupViewModel)LaunchViewDialog(cancelPO,"CANGRN", true);
                    if (result != null && result.ResultDialog == ActionDialog.Save)
                    {
                        DataService.Purchaseing.CancelGoodReceive(SelectGRNDetail.GRNDetailUID, result.Comments, AppUtil.Current.UserID);
                        SaveSuccessDialog();
                        SelectGRNDetail.GRNSTSUID = 2912;
                        SelectGRNDetail.GRNStatus = GRNStatus.FirstOrDefault(p => p.Key == 2912).Display;
                        OnUpdateEvent();
                        SelectGRNDetail = null;
                    }
                }
                catch (Exception er)
                {

                    ErrorDialog(er.Message);
                }
            }
        }




        #endregion
    }
}
