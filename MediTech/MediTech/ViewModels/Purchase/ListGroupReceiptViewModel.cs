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
            set { Set(ref _SelectGroupReceipt, value); }
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

        private RelayCommand _AddRecriptCommand;
        public RelayCommand AddRecriptCommand
        {
            get
            {
                return _AddRecriptCommand ?? (_AddRecriptCommand = new RelayCommand(AddReceipt));
            }
        }

        private RelayCommand _ManageRecriptCommand;
        public RelayCommand ManageRecriptCommand
        {
            get
            {
                return _ManageRecriptCommand ?? (_ManageRecriptCommand = new RelayCommand(ManageReceipt));
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

        #endregion

        #region Method
        public ListGroupReceiptViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            Organisations = GetHealthOrganisationIsRoleStock();
            PayorDetails = DataService.MasterData.GetPayorDetail();
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            //GroupReceipt = DataService.Purchaseing.GetGroupReceipt();
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

        private void AddReceipt()
        {
            ManageReceipt receipt = new ManageReceipt();
            ChangeViewPermission(receipt);
        }

        private void ManageReceipt()
        {
            if(SelectGroupReceipt != null)
            {
                ManageReceipt receipt = new ManageReceipt();
                var data = DataService.Purchaseing.GetGroupReceiptByUID(SelectGroupReceipt.GroupReceiptUID);
                (receipt.DataContext as ManageReceiptViewModel).AssignModel(data);
                ChangeViewPermission(receipt);
            }
        }

        #endregion

    }
}
