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
    public class ListItemTransferViewModel : MediTechViewModelBase
    {

        #region Properties
        private bool _IsEnableEdit;

        public bool IsEnableEdit
        {
            get { return _IsEnableEdit; }
            set { Set(ref _IsEnableEdit, value); }
        }

        private bool _IsEnableCancel = false;

        public bool IsEnableCancel
        {
            get { return _IsEnableCancel; }
            set { Set(ref _IsEnableCancel, value); }
        }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public string IssueNo { get; set; }

        public List<LookupReferenceValueModel> IssueStatus { get; set; }

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

        private List<ItemIssueModel> _ItemIssues;

        public List<ItemIssueModel> ItemIssues
        {
            get { return _ItemIssues; }
            set { Set(ref _ItemIssues, value); }
        }

        private ItemIssueModel _SelectItemIssues;

        public ItemIssueModel SelectItemIssues
        {
            get { return _SelectItemIssues; }
            set
            {
                Set(ref _SelectItemIssues, value);
                if (_SelectItemIssues != null)
                {
                    ItemIssueDetails = DataService.Inventory.GetItemIssueDetailByItemIssueUID(SelectItemIssues.ItemIssueUID);
                    if (_SelectItemIssues.ISUSTUID == 2916)
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

        private List<ItemIssueDetailModel> _ItemIssueDetails;

        public List<ItemIssueDetailModel> ItemIssueDetails
        {
            get { return _ItemIssueDetails; }
            set { Set(ref _ItemIssueDetails, value); }
        }

        private ItemIssueDetailModel _SelectItemIssueDetail;

        public ItemIssueDetailModel SelectItemIssueDetail
        {
            get { return _SelectItemIssueDetail; }
            set { Set(ref _SelectItemIssueDetail, value); }
        }

        private LookupReferenceValueModel _SelectIssueStatus;

        public LookupReferenceValueModel SelectIssueStatus
        {
            get { return _SelectIssueStatus; }
            set { Set(ref _SelectIssueStatus, value); }
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

        public ListItemTransferViewModel()
        {
            var Organis = GetHealthOrganisationIsStock();
            IssueStatus = DataService.Technical.GetReferenceValueMany("ISUST");
            OrganisationsFrom = GetHealthOrganisationIsRoleStock();
            OrganisationsTo = Organis;
            DateFrom = DateTime.Now;
            if (OrganisationsFrom != null)
            {
                SelectOrganisationFrom = OrganisationsFrom.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            }

            if (IssueStatus != null)
            {
                SelectIssueStatus = IssueStatus.FirstOrDefault(p => p.ValueCode == "ISSED");
            }
        }

        public override void OnLoaded()
        {
            Search();
        }
        private void Add()
        {
            ManageItemTransfer view = new ManageItemTransfer();
            ChangeViewPermission(view);
        }
        private void Edit()
        {
            try
            {
                if (SelectItemIssues != null)
                {
                    if (SelectItemIssues.ISUSTUID != 2916)
                    {
                        MessageBoxResult resultDiag = QuestionDialog("คุณต้องการ ยกเลิกรายการส่งขนย้ายเดิมแล้วสร้างใหม่ หรือไม่ ?");
                        if (resultDiag != MessageBoxResult.Yes)
                        {
                            return;
                        }
                        CancelPopup cancelPO = new CancelPopup();
                        CancelPopupViewModel result = (CancelPopupViewModel)LaunchViewDialog(cancelPO, "CANISS", true);
                        if (result != null && result.ResultDialog == ActionDialog.Save)
                        {
                            DataService.Inventory.CancelItemTransfer(SelectItemIssues.ItemIssueUID, result.Comments, AppUtil.Current.UserID);
                            SaveSuccessDialog();
                            SelectItemIssues.ISUSTUID = 2916;
                            SelectItemIssues.IssueStatus = IssueStatus.FirstOrDefault(p => p.Key == 2916).Display;

                            ManageItemTransfer managePage = new ManageItemTransfer();
                            var EditData = DataService.Inventory.GetItemIssueByUID(SelectItemIssues.ItemIssueUID);
                            (managePage.DataContext as ManageItemIssueViewModel).AssignModel(EditData);
                            ChangeViewPermission(managePage);
                        }
                    }

                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }
        private void Canecl()
        {
            if (SelectItemIssues != null)
            {
                try
                {
                    if (!DataService.Inventory.CheckCancelTransfer(SelectItemIssues.ItemIssueUID).IsActive)
                    {
                        WarningDialog("ไม่สามารถยกเลิกรายการโอนย้ายนี้ได้เนื่องจากมีการนำคลังสินค้าไปใช้งานแล้ว");
                        return;
                    }
                    CancelPopup cancelPO = new CancelPopup();
                    CancelPopupViewModel result = (CancelPopupViewModel)LaunchViewDialog(cancelPO,"CANISS", true);
                    if (result != null && result.ResultDialog == ActionDialog.Save)
                    {
                        DataService.Inventory.CancelItemTransfer(SelectItemIssues.ItemIssueUID, result.Comments, AppUtil.Current.UserID);
                        SaveSuccessDialog();
                        SelectItemIssues.ISUSTUID = 2916;
                        SelectItemIssues.IssueStatus = IssueStatus.FirstOrDefault(p => p.Key == 2916).Display;
                        OnUpdateEvent();
                        SelectItemIssues = null;

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
            if (SelectItemIssues != null)
            {
                StockIssueReport rpt = new StockIssueReport();
                rpt.Parameters["IssueID"].Value = SelectItemIssues.ItemIssueID;
                ReportPrintTool printTool = new ReportPrintTool(rpt);
                rpt.ShowPrintMarginsWarning = false;
                printTool.ShowPreviewDialog();
            }
        }


        private void Search()
        {
            ItemIssues = null;
            ItemIssueDetails = null;
            int? organisationUIDFrom = SelectOrganisationFrom != null ? SelectOrganisationFrom.HealthOrganisationUID : (int?)null;
            int? organisationUIDTo = SelectOrganisationTo != null ? SelectOrganisationTo.HealthOrganisationUID : (int?)null;
            int? issueStatus = SelectIssueStatus != null ? SelectIssueStatus.Key : (int?)null;
            ItemIssues = DataService.Inventory.SearchItemIssue(DateFrom, DateTo, IssueNo, 2918, issueStatus, organisationUIDFrom, organisationUIDTo);
        }


        private void Clear()
        {
            DateFrom = DateTime.Now;
            DateTo = null;
            SelectOrganisationFrom = null;
            SelectOrganisationTo = null;
            IssueNo = string.Empty ;
        }


        #endregion
    }
}
