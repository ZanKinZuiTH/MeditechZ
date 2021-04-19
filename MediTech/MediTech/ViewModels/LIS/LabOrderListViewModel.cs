using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
using MediTech.Model;
using System.Windows;
using ShareLibrary;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using MediTech.Views;
using MediTech.Reports.Operating.Lab;
using DevExpress.XtraReports.UI;
using MediTech.Model.Report;
using MediTech.Models;
using MediTech.Reports.Operating.Patient;

namespace MediTech.ViewModels
{
    public class LabOrderListViewModel : MediTechViewModelBase
    {

        #region Properties

        #region PatientSearch

        private string _SearchPatientCriteria;

        public string SearchPatientCriteria
        {
            get { return _SearchPatientCriteria; }
            set
            {
                Set(ref _SearchPatientCriteria, value);
                PatientsSearchSource = null;
            }
        }


        private List<PatientInformationModel> _PatientsSearchSource;

        public List<PatientInformationModel> PatientsSearchSource
        {
            get { return _PatientsSearchSource; }
            set { Set(ref _PatientsSearchSource, value); }
        }

        private PatientInformationModel _SelectedPateintSearch;

        public PatientInformationModel SelectedPateintSearch
        {
            get { return _SelectedPateintSearch; }
            set
            {
                _SelectedPateintSearch = value;
                if (SelectedPateintSearch != null)
                {
                    SearchLabOrder();
                    SearchPatientCriteria = string.Empty;
                }
            }
        }

        #endregion

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

        private List<LookupReferenceValueModel> _RequestStatus;

        public List<LookupReferenceValueModel> RequestStatus
        {
            get { return _RequestStatus; }
            set { Set(ref _RequestStatus, value); }
        }


        private LookupReferenceValueModel _SelectRequestStatus;

        public LookupReferenceValueModel SelectRequestStatus
        {
            get { return _SelectRequestStatus; }
            set { Set(ref _SelectRequestStatus, value); }
        }

        private List<RequestItemModel> _RequesItems;

        public List<RequestItemModel> RequesItems
        {
            get { return _RequesItems; }
            set { Set(ref _RequesItems, value); }
        }

        private RequestItemModel _SelectRequestItem;

        public RequestItemModel SelectRequestItem
        {
            get { return _SelectRequestItem; }
            set { Set(ref _SelectRequestItem, value); }
        }

        private int _No;
        public int No
        {
            get { return _No; }
            set { Set(ref _No, value); }
        }
        private string _LabNumber;

        public string LabNumber
        {
            get { return _LabNumber; }
            set { Set(ref _LabNumber, value); }
        }

        private List<HealthOrganisationModel> _Organisations;

        public List<HealthOrganisationModel> Organisations
        {
            get { return _Organisations; }
            set { Set(ref _Organisations, value); }
        }

        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set { Set(ref _SelectOrganisation, value); }
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


        private List<RequestLabModel> _RequestLabs;

        public List<RequestLabModel> RequestLabs
        {
            get { return _RequestLabs; }
            set { Set(ref _RequestLabs, value); }
        }

        private RequestLabModel _SelectRequestLab;

        public RequestLabModel SelectRequestLab
        {
            get { return _SelectRequestLab; }
            set
            {
                Set(ref _SelectRequestLab, value);
                if (SelectRequestLab != null)
                {
                    RequestDetailLabs = DataService.Lab.GetRequesDetailLabByRequestUID(SelectRequestLab.RequestUID);
                    if (RequestDetailLabs != null)
                        RequestDetailLabs = RequestDetailLabs.OrderBy(p => p.RequestItemName).ToList();
                }
            }
        }

        private List<RequestDetailItemModel> _RequestDetailLabs;

        public List<RequestDetailItemModel> RequestDetailLabs
        {
            get { return _RequestDetailLabs; }
            set { Set(ref _RequestDetailLabs, value); }
        }

        private RequestDetailItemModel _SelectRequestDetailLab;

        public RequestDetailItemModel SelectRequestDetailLab
        {
            get { return _SelectRequestDetailLab; }
            set { Set(ref _SelectRequestDetailLab, value); }
        }

        private List<RequestDetailItemModel> _SelectRequestDetailLabs;

        public List<RequestDetailItemModel> SelectRequestDetailLabs
        {
            get { return _SelectRequestDetailLabs ?? (_SelectRequestDetailLabs = new List<RequestDetailItemModel>()); }
            set { Set(ref _SelectRequestDetailLabs, value); }
        }

        private LookupItemModel _SelectPrinter;
        public LookupItemModel SelectPrinter
        {
            get { return _SelectPrinter; }
            set { Set(ref _SelectPrinter, value); }
        }

        private List<LookupItemModel> _PrinterLists;
        public List<LookupItemModel> PrinterLists
        {
            get { return _PrinterLists; }
            set { Set(ref _PrinterLists, value); }
        }

        private ObservableCollection<StoreItemList> _StoreOrderList;
        public ObservableCollection<StoreItemList> StoreOrderList
        {
            get { return _StoreOrderList; }
            set { Set(ref _StoreOrderList, value); }
        }

        private bool _SelectAll;

        public bool SelectAll
        {
            get { return _SelectAll; }
            set
            {
                Set(ref _SelectAll, value);
                if (RequestLabs != null && RequestLabs.Count > 0)
                {
                    foreach (var item in RequestLabs)
                    {
                        item.Selected = SelectAll;
                    }
                }
            }
        }


        #endregion

        #region Command

        private RelayCommand _PatientSearchCommand;

        /// <summary>
        /// Gets the PatientSearchCommand.
        /// </summary>
        public RelayCommand PatientSearchCommand
        {
            get
            {
                return _PatientSearchCommand
                    ?? (_PatientSearchCommand = new RelayCommand(PatientSearch));
            }
        }


        private RelayCommand _SearchCommand;

        /// <summary>
        /// Gets the SearchCommand.
        /// </summary>
        public RelayCommand SearchCommand
        {
            get
            {
                return _SearchCommand
                    ?? (_SearchCommand = new RelayCommand(SearchLabOrder));
            }
        }

        private RelayCommand _ClearCommand;

        /// <summary>
        /// Gets the ClearCommand.
        /// </summary>
        public RelayCommand ClearCommand
        {
            get
            {
                return _ClearCommand
                    ?? (_ClearCommand = new RelayCommand(Clear));
            }
        }


        private RelayCommand _EnterResultCommand;

        /// <summary>
        /// Gets the EnterResultCommand.
        /// </summary>
        public RelayCommand EnterResultCommand
        {
            get
            {
                return _EnterResultCommand
                    ?? (_EnterResultCommand = new RelayCommand(EnterResult));
            }
        }

        private RelayCommand _PrintPreviewCommand;

        /// <summary>
        /// Gets the PrintPreviewCommand.
        /// </summary>
        public RelayCommand PrintPreviewCommand
        {
            get
            {
                return _PrintPreviewCommand
                    ?? (_PrintPreviewCommand = new RelayCommand(PrintPreview));
            }
        }


        private RelayCommand _PrintAutoCommand;

        /// <summary>
        /// Gets the EnterResultCommand.
        /// </summary>
        public RelayCommand PrintAutoCommand
        {
            get
            {
                return _PrintAutoCommand
                    ?? (_PrintAutoCommand = new RelayCommand(PrintAuto));
            }
        }

        private RelayCommand _CancelResultCommand;

        /// <summary>
        /// Gets the EnterResultCommand.
        /// </summary>
        public RelayCommand CancelResultCommand
        {
            get
            {
                return _CancelResultCommand
                    ?? (_CancelResultCommand = new RelayCommand(CancelResult));
            }
        }

        private RelayCommand _PrintStickerCommand;
        public RelayCommand PrintStickerCommand
        {
            get { return _PrintStickerCommand ?? (_PrintStickerCommand = new RelayCommand(PrintSticker)); }
        }

        #endregion

        #region Method
        private void PrintSticker()
        {
            if (SelectPrinter == null)
            {
                WarningDialog("กรุณาเลือก Printer");
                return;
            }
            try
            {
                var requestLabSelected = RequestLabs.Where(p => p.Selected);
                if (requestLabSelected != null && requestLabSelected.Count() > 0)
                {
                    //LabOrderList view = (LabOrderList)this.View;
                    foreach (var item in requestLabSelected)
                    {
                        PatientStickerBarcode rpt = new PatientStickerBarcode();
                        ReportPrintTool printTool = new ReportPrintTool(rpt);

                        string gender;
                        switch (item.Gender)
                        {
                            case "หญิง (Female)":
                            case "F":
                                gender = "(F)";
                                break;
                            case "ชาย (Male)":
                            case "M":
                                gender = "(M)";
                                break;
                            default:
                                gender = "(N/A)";
                                break;
                        }

                        rpt.Parameters["No"].Value = item.No;
                        rpt.Parameters["PatientName"].Value = item.PatientName + " " + gender;
                        rpt.Parameters["HN"].Value = item.PatientID;
                        rpt.Parameters["Age"].Value = item.PatientAge;
                        rpt.Parameters["BirthDttm"].Value = item.BirthDate != null ? item.BirthDateString : "";
                        rpt.Parameters["CompanyName"].Value = item.PayorName;
                        rpt.RequestParameters = false;
                        rpt.ShowPrintMarginsWarning = false;

                        printTool.Print(SelectPrinter.Display);
                    }

                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }







            //var printStickers = StoreOrderList.Where(p => p.IsSelected);
            //if (printStickers != null && printStickers.Count() > 0)
            //{
            //    var groupItemStore = printStickers.GroupBy(p => new
            //    {
            //        p.IdentifyingUID,
            //        p.ItemName
            //    })
            //         .Select(
            //         g => new
            //         {
            //             PrescriptionItemUID = g.FirstOrDefault().IdentifyingUID,
            //             ExpiryDate = g.Max(expy => expy.ExpiryDate)
            //         });


            //    foreach (var item in groupItemStore)
            //    {
            //        PatientSticker rpt = new PatientSticker();
            //        ReportPrintTool printTool = new ReportPrintTool(rpt);

            //        rpt.Parameters["PrescriptionItemUID"].Value = item.PrescriptionItemUID;
            //        rpt.Parameters["ExpiryDate"].Value = item.ExpiryDate;
            //        rpt.RequestParameters = false;
            //        rpt.ShowPrintMarginsWarning = false;
            //        printTool.Print(SelectPrinter.Display);
            //    }
            //}
        }

        public LabOrderListViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;

            var refValue = DataService.Technical.GetReferenceValueMany("ORDST");
            RequesItems = DataService.MasterData.GetRequestItemByCategory("LAB");
            Organisations = GetHealthOrganisationRoleMedical();
            PayorDetails = DataService.MasterData.GetPayorDetail();

            RequestStatus = refValue.Where(p => p.ValueCode == "RAISED"
                || p.ValueCode == "REVIW"
                || p.ValueCode == "SAMPLCOL"
                || p.ValueCode == "ACPSMP"
                || p.ValueCode == "PARCOLLEC"
                || p.ValueCode == "PREVI"
                || p.ValueCode == "PRCAN"
                || p.ValueCode == "PARCMP"
                || p.ValueCode == "CANCLD").OrderBy(p => p.Display).ToList();

            PrinterLists = new List<LookupItemModel>();
            int i = 1;
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                LookupItemModel lookupData = new LookupItemModel();
                lookupData.Key = i;
                lookupData.Display = printer;
                PrinterLists.Add(lookupData);
                i++;
            }
            SelectPrinter = PrinterLists.FirstOrDefault(p => p.Display.ToLower().Contains("Sticker"));
        }

        private void SearchLabOrder()
        {
            long? patientUID = null;
            string statusOrder = SelectRequestStatus != null ? SelectRequestStatus.Key.ToString() : "";
            int? requestItemUID = SelectRequestItem != null ? SelectRequestItem.RequestItemUID : (int?)null;
            int? payorDetailUID = SelectPayorDetail != null ? SelectPayorDetail.PayorDetailUID : (int?)null;
            int? organisationUID = SelectOrganisation != null ? SelectOrganisation.HealthOrganisationUID : (int?)null;
            if (!string.IsNullOrEmpty(SearchPatientCriteria))
            {
                if (SelectedPateintSearch != null)
                {
                    patientUID = SelectedPateintSearch.PatientUID;
                }
            }

            RequestLabs = DataService.Lab.SearchRequestLabList(DateFrom, DateTo, statusOrder, patientUID, requestItemUID, LabNumber, payorDetailUID, organisationUID);

            if (RequestLabs != null && RequestLabs.Count > 0)
            {
                int i = 1;
                RequestLabs.ForEach(p => p.No = i++);

            }
            RequestDetailLabs = null;
        }

        public void PatientSearch()
        {
            string patientID = string.Empty;
            string firstName = string.Empty; ;
            string lastName = string.Empty;
            if (SearchPatientCriteria.Length >= 3)
            {
                string[] patientName = SearchPatientCriteria.Split(' ');
                if (patientName.Length >= 2)
                {
                    firstName = patientName[0];
                    lastName = patientName[1];
                }
                else
                {
                    int num = 0;
                    foreach (var ch in SearchPatientCriteria)
                    {
                        if (ShareLibrary.CheckValidate.IsNumber(ch.ToString()))
                        {
                            num++;
                        }
                    }
                    if (num >= 5)
                    {
                        patientID = SearchPatientCriteria;
                    }
                    else if (num <= 2)
                    {
                        firstName = SearchPatientCriteria;
                        lastName = "empty";
                    }

                }
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null);
                PatientsSearchSource = searchResult;
            }
            else
            {
                PatientsSearchSource = null;
            }

        }

        private void EnterResult()
        {
            if (SelectRequestLab != null && SelectRequestLab.OrderStatus != "Cancelled")
            {
                EnterResultsLab enterResult = new EnterResultsLab();
                (enterResult.DataContext as EnterResultsLabViewModel).AssignModel(SelectRequestLab);
                EnterResultsLabViewModel enterResultsLabModel = (EnterResultsLabViewModel)LaunchViewDialog(enterResult, "ENTRE", false, false);

                if (enterResultsLabModel == null)
                {
                    return;
                }

                if (enterResultsLabModel != null && enterResultsLabModel.ResultDialog == ActionDialog.Save)
                {
                    RequestDetailLabs = DataService.Lab.GetRequesDetailLabByRequestUID(SelectRequestLab.RequestUID);
                    if (RequestDetailLabs != null)
                        RequestDetailLabs = RequestDetailLabs.OrderBy(p => p.RequestItemName).ToList();
                    //SearchLabOrder();
                }
            }

        }

        private void PrintPreview()
        {
            if (SelectRequestLab != null)
            {
                LabResultReport rpt = new LabResultReport();
                ReportPrintTool printTool = new ReportPrintTool(rpt);


                rpt.Parameters["PatientVisitUID"].Value = SelectRequestLab.PatientVisitUID;
                rpt.Parameters["RequestNumber"].Value = SelectRequestLab.LabNumber;
                rpt.RequestParameters = false;
                rpt.ShowPrintMarginsWarning = false;
                printTool.ShowPreviewDialog();
            }
        }

        private void PrintAuto()
        {
            if (SelectPrinter == null)
            {
                WarningDialog("กรุณาเลือก Printer");
                return;
            }
            var requestLabSelected = RequestLabs.Where(p => p.Selected);
            if (requestLabSelected != null && requestLabSelected.Count() > 0)
            {
                foreach (var item in requestLabSelected)
                {
                    LabResultReport rpt = new LabResultReport();
                    ReportPrintTool printTool = new ReportPrintTool(rpt);


                    rpt.Parameters["PatientVisitUID"].Value = item.PatientVisitUID;
                    rpt.Parameters["RequestNumber"].Value = item.LabNumber;
                    rpt.PrintAuto = true;
                    rpt.RequestParameters = false;
                    rpt.ShowPrintMarginsWarning = false;
                    printTool.Print(SelectPrinter.Display);
                }
            }


        }

        private void CancelResult()
        {
            try
            {
                if (SelectRequestDetailLabs != null && SelectRequestDetailLabs.Count > 0)
                {
                    MessageBoxResult messResult = QuestionDialog(string.Format("คุณต้องการยกเลิกผลของ {0} ใช้หรือไม่ ?", SelectRequestLab.PatientName));

                    if (messResult == MessageBoxResult.Yes)
                    {
                        foreach (var item in SelectRequestDetailLabs)
                        {
                            if (item.OrderStatus == "Reviewed")
                            {
                                DataService.Lab.CancelLabResult(item.RequestDetailUID, AppUtil.Current.UserID);
                                item.OrderStatus = "Raised";
                                item.ResultEnteredDttm = null;
                            }
                        }
                        SaveSuccessDialog();
                        OnUpdateEvent();
                    }
                }

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void Clear()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            SearchPatientCriteria = "";
            SelectedPateintSearch = null;
            SelectRequestItem = null;
            LabNumber = String.Empty;
            SelectRequestStatus = null;
            SelectOrganisation = null;
            SelectPayorDetail = null;
        }

        #endregion
    }
}
