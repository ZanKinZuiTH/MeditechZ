using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Model.Report;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class CheckupJobSummeryReportViewModel : MediTechViewModelBase
    {
        #region Properties
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
            set
            {
                Set(ref _SelectPayorDetail, value);
                if (_SelectPayorDetail != null)
                {
                    CheckupJobContactList = DataService.Checkup.GetCheckupJobContactByPayorDetailUID(_SelectPayorDetail.PayorDetailUID);
                    SelectCheckupJobContact = CheckupJobContactList.OrderByDescending(p => p.StartDttm).FirstOrDefault();
                }
            }
        }


        private List<CheckupJobContactModel> _CheckupJobContactList;

        public List<CheckupJobContactModel> CheckupJobContactList
        {
            get { return _CheckupJobContactList; }
            set { Set(ref _CheckupJobContactList, value); }
        }

        private CheckupJobContactModel _SelectCheckupJobContact;

        public CheckupJobContactModel SelectCheckupJobContact
        {
            get { return _SelectCheckupJobContact; }
            set
            {
                Set(ref _SelectCheckupJobContact, value);
                if (_SelectCheckupJobContact != null)
                {
                    DateFrom = _SelectCheckupJobContact.StartDttm;
                    DateTo = _SelectCheckupJobContact.EndDttm;
                }
            }
        }

        private DateTime? _DateFrom;

        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set { Set(ref _DateFrom, value); }
        }

        private DateTime? _DateTo;

        public DateTime? DateTo
        {
            get { return _DateTo; }
            set { Set(ref _DateTo, value); }
        }

        private List<CheckupJobOrderModel> _PivotCheckupJobData;

        public List<CheckupJobOrderModel> PivotCheckupJobData
        {
            get { return _PivotCheckupJobData; }
            set { Set(ref _PivotCheckupJobData, value); }
        }

        #endregion

        #region Command

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(SearchCheckJobSummery)); }
        }


        private RelayCommand _ExportToExcelCommand;
        public RelayCommand ExportToExcelCommand
        {
            get
            {
                return _ExportToExcelCommand
                    ?? (_ExportToExcelCommand = new RelayCommand(ExportPivotGridToExcel));
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
        public CheckupJobSummeryReportViewModel()
        {
            PayorDetails = DataService.Billing.GetPayorDetail();
            DateFrom = DateTime.Now;
            DateTo = null;
        }
        void SearchCheckJobSummery()
        {
            if (SelectCheckupJobContact == null)
            {
                WarningDialog("กรูณาเลือก Job");
                return;
            }
            var checkupJobOrderData = DataService.Reports.CheckupJobOrderSummary(SelectCheckupJobContact.CheckupJobContactUID, DateFrom, DateTo);
            PivotCheckupJobData = checkupJobOrderData?.OrderBy(p => p.OrderSetName).ToList();
        }
        void ExportPivotGridToExcel()
        {
            if (PivotCheckupJobData != null)
            {
                string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xlsx");
                if (fileName != "")
                {
                    CheckupJobSummeryReport view = (CheckupJobSummeryReport)this.View;
                    view.pivotData.ExportToXlsx(fileName);
                    OpenFile(fileName);
                }

            }
        }

        void Cancel()
        {
            ChangeViewPermission(this.BackwardView);
        }


        #endregion
    }
}
