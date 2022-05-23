using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Model.Report;
using MediTech.Reports.Operating.Cashier;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MediTech.ViewModels
{ 
    public class ListGroupReceiptViewModel : MediTechViewModelBase
    {
        #region Propotie
        public List<HealthOrganisationModel> Organisations { get; set; }

        private HealthOrganisationModel _SelectOrganisation;
        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set { Set(ref _SelectOrganisation, value); }
        }

        private List<GroupReceiptModel> _GroupReceipt;
        public List<GroupReceiptModel> GroupReceipt
        {
            get { return _GroupReceipt ?? (_GroupReceipt = new List<GroupReceiptModel>()); }
            set { Set(ref _GroupReceipt, value); }
        }

        private GroupReceiptModel _SelectGroupReceipt;
        public GroupReceiptModel SelectGroupReceipt
        {
            get { return _SelectGroupReceipt; }
            set { 
                Set(ref _SelectGroupReceipt, value);
                if (SelectGroupReceipt != null && SelectGroupReceipt.CancelledDttm == null)
                {
                    IsEnableEditReceipt = true;
                    IsEnableCancel = true;
                }
                else
                {
                    IsEnableEditReceipt = false;
                    IsEnableCancel = false;
                }
            }
        }
        private List<PayorDetailModel> _PayorDetails;
        public List<PayorDetailModel> PayorDetails
        {
            get { return _PayorDetails; }
            set { Set(ref _PayorDetails, value); }
        }

        private PayorDetailModel _SelectPayorDetail;
        public PayorDetailModel SelectPayorDetail
        {
            get { return _SelectPayorDetail; }
            set { Set(ref _SelectPayorDetail, value); }
        }

        private string _ReceiptNumber;
        public string ReceiptNumber
        {
            get { return _ReceiptNumber; }
            set { Set(ref _ReceiptNumber, value); }
        }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }


        private bool _IsEnableEditReceipt = false;

        public bool IsEnableEditReceipt
        {
            get { return _IsEnableEditReceipt; }
            set { Set(ref _IsEnableEditReceipt, value); }
        }

        private bool _IsEnableCancel = false;

        public bool IsEnableCancel
        {
            get { return _IsEnableCancel; }
            set { Set(ref _IsEnableCancel, value); }
        }

        #endregion

        #region Command
        private RelayCommand _SearchCommand;
        public RelayCommand SearchCommand
        {
            get
            {
                return _SearchCommand ?? (_SearchCommand = new RelayCommand(Search));
            }
        }

        private RelayCommand _CreateRecriptCommand;
        public RelayCommand CreateRecriptCommand
        {
            get
            {
                return _CreateRecriptCommand ?? (_CreateRecriptCommand = new RelayCommand(CreateReceipt));
            }
        }

        private RelayCommand _ModifyReceiptCommand;
        public RelayCommand ModifyReceiptCommand
        {
            get
            {
                return _ModifyReceiptCommand ?? (_ModifyReceiptCommand = new RelayCommand(ModifyReceipt));
            }
        }

        private RelayCommand _CancelReceiptCommand;
        public RelayCommand CancelReceiptCommand
        {
            get
            {
                return _CancelReceiptCommand ?? (_CancelReceiptCommand = new RelayCommand(CancelReceipt));
            }
        }

        private RelayCommand _PrintRecriptCommand;
        public RelayCommand PrintRecriptCommand
        {
            get
            {
                return _PrintRecriptCommand ?? (_PrintRecriptCommand = new RelayCommand(PrintReceipt));
            }
        }


        private RelayCommand _ExportToExcelCommand;

        public RelayCommand ExportToExcelCommand
        {
            get { return _ExportToExcelCommand ?? (_ExportToExcelCommand = new RelayCommand(ExportToExcel)); }
        }

        #endregion

        #region Method
        public ListGroupReceiptViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            Organisations = GetHealthOrganisationIsRoleStock();
            PayorDetails = DataService.Billing.GetPayorDetail();
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            //GroupReceipt = DataService.Purchaseing.GetGroupReceipt();
            Search();
        }
        public override void OnLoaded()
        {
            Search();
        }

        void Search()
        {
            GroupReceipt = null;
            //SelectOrganisation = null;
            int? organisationUID = SelectOrganisation != null ? SelectOrganisation.HealthOrganisationUID : (int?)null;
            int? payorDetailUID = SelectPayorDetail != null ? SelectPayorDetail.PayorDetailUID : (int?)null;

            GroupReceipt = DataService.Purchaseing.SearchGroupReceipt(DateFrom, DateTo, organisationUID, payorDetailUID, ReceiptNumber);
        }

        void PrintReceipt()
        {
            if(SelectGroupReceipt != null)
            {
                GroupReceipt rpt = new GroupReceipt();
                rpt.Parameters["GroupReceiptUID"].Value = SelectGroupReceipt.GroupReceiptUID;
                //rpt.DataSource = SelectGroupReceipt;
                ReportPrintTool printTool = new ReportPrintTool(rpt);
                rpt.RequestParameters = false;
                rpt.ShowPrintMarginsWarning = false;
                printTool.ShowPreviewDialog();
            }
        }

        private void CreateReceipt()
        {
            ManageReceipt receipt = new ManageReceipt();
            ChangeViewPermission(receipt);
        }

        private void ModifyReceipt()
        {
            if(SelectGroupReceipt != null && SelectGroupReceipt.CancelledDttm == null)
            {
                ManageReceipt receipt = new ManageReceipt();
                var data = DataService.Purchaseing.GetGroupReceiptByUID(SelectGroupReceipt.GroupReceiptUID);
                (receipt.DataContext as ManageReceiptViewModel).AssignModel(data);
                ChangeViewPermission(receipt);
            }
        }

        private void CancelReceipt()
        {
            if (SelectGroupReceipt != null && SelectGroupReceipt.CancelledDttm == null)
            {
                try
                {
                    CancelPopup cancelPopup = new CancelPopup();
                    CancelPopupViewModel result = (CancelPopupViewModel)LaunchViewDialog(cancelPopup, "CANBILL", true);
                    if (result != null && result.ResultDialog == ActionDialog.Save)
                    {
                        if (String.IsNullOrEmpty(result.Comments))
                        {
                            WarningDialog("ไม่สามารถยกเลิกได้ กรุณาใส่เหตุผล");
                            return;
                        }
                        DataService.Purchaseing.CancelReceipt(SelectGroupReceipt.GroupReceiptUID, result.Comments, AppUtil.Current.UserID);
                        SaveSuccessDialog();
                        Search();
                    }

                }
                catch (Exception er)
                {

                    ErrorDialog(er.Message);
                }
            }
        }

        private void ExportToExcel()
        {
            try
            {
                if (GroupReceipt != null)
                {
                    string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xlsx");
                    if (fileName != "")
                    {
                        ListGroupReceipt view = (ListGroupReceipt)this.View;
                        view.viewGroupReceipt.ExportToXlsx(fileName);
                        OpenFile(fileName);
                    }

                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        #endregion

    }
}
