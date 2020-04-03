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

namespace MediTech.ViewModels
{
    public class SpecimenCollectionViewModel : MediTechViewModelBase
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
                    SearchRequestDetailSpecimen();
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

        private List<object> _SelectRequestStatusList;

        public List<object> SelectRequestStatusList
        {
            get { return _SelectRequestStatusList ?? (_SelectRequestStatusList = new List<object>()); }
            set { Set(ref _SelectRequestStatusList, value); }
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

        private string _LabNumber;

        public string LabNumber
        {
            get { return _LabNumber; }
            set { Set(ref _LabNumber, value); }
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
                if (_SelectRequestLab != null)
                {
                    GetRequestDetailSpecimen(SelectRequestLab.RequestUID);
                    (this.View as SpecimenCollection).PatientBanner.SetPatientBanner(SelectRequestLab.PatientUID, SelectRequestLab.PatientVisitUID);
                }
                else
                {
                    (this.View as SpecimenCollection).PatientBanner.ClearData();
                    RequestDetailSpecimens = null;
                }
            }
        }

        private ObservableCollection<RequestDetailSpecimenModel> _RequestDetailSpecimens;

        public ObservableCollection<RequestDetailSpecimenModel> RequestDetailSpecimens
        {
            get { return _RequestDetailSpecimens; }
            set { Set(ref _RequestDetailSpecimens, value); }
        }

        private List<RequestDetailSpecimenModel> _SelectRequestDetailSpecimens;

        public List<RequestDetailSpecimenModel> SelectRequestDetailSpecimens
        {
            get { return _SelectRequestDetailSpecimens ?? (_SelectRequestDetailSpecimens = new List<RequestDetailSpecimenModel>()); }
            set
            {
                Set(ref _SelectRequestDetailSpecimens, value);
            }
        }

        private bool _SurpassEventChecked = false;

        public bool SurpassEventChecked
        {
            get { return _SurpassEventChecked; }
            set { _SurpassEventChecked = value; }
        }


        private bool _SelectedAll;

        public bool SelectedAll
        {
            get { return _SelectedAll; }
            set
            {
                Set(ref _SelectedAll, value);
                if (RequestDetailSpecimens != null)
                {
                    SurpassEventChecked = true;
                    foreach (var item in RequestDetailSpecimens)
                    {
                        if (item.EnableSelect)
                        {
                            item.Selected = SelectedAll;
                        }

                    }
                    SurpassEventChecked = false;
                }
            }
        }


        private List<LookupItemModel> _PrinterLists;

        public List<LookupItemModel> PrinterLists
        {
            get { return _PrinterLists; }
            set { Set(ref _PrinterLists, value); }
        }

        private LookupItemModel _SelectPrinter;

        public LookupItemModel SelectPrinter
        {
            get { return _SelectPrinter; }
            set { Set(ref _SelectPrinter, value); }
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
                    ?? (_SearchCommand = new RelayCommand(SearchRequestDetailSpecimen));
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
                    ?? (_ClearCommand = new RelayCommand(DefaultControl));
            }
        }


        private RelayCommand _PrintBarcodeCommand;

        /// <summary>
        /// Gets the PrintBarcodeCommand.
        /// </summary>
        public RelayCommand PrintBarcodeCommand
        {
            get
            {
                return _PrintBarcodeCommand
                    ?? (_PrintBarcodeCommand = new RelayCommand(PrintBarcode));
            }
        }


        private RelayCommand _CollectAndPrintCommand;

        /// <summary>
        /// Gets the CollectAndPrintCommand.
        /// </summary>
        public RelayCommand CollectAndPrintCommand
        {
            get
            {
                return _CollectAndPrintCommand
                    ?? (_CollectAndPrintCommand = new RelayCommand(CollectAndPrint));
            }
        }

        private RelayCommand _CollectCommand;

        /// <summary>
        /// Gets the CollectAndPrintCommand.
        /// </summary>
        public RelayCommand CollectCommand
        {
            get
            {
                return _CollectCommand
                    ?? (_CollectCommand = new RelayCommand(Collect));
            }
        }

        private RelayCommand<int> _SelectedSpecimenCommand;

        /// <summary>
        /// Gets the SelectedSpecimenCommand.
        /// </summary>
        public RelayCommand<int> SelectedSpecimenCommand
        {
            get
            {
                return _SelectedSpecimenCommand
                    ?? (_SelectedSpecimenCommand = new RelayCommand<int>(SelectedSpecimen));
            }
        }

        private RelayCommand _CreateOrderCommand;

        public RelayCommand CreateOrderCommand
        {
            get { return _CreateOrderCommand ?? (_CreateOrderCommand = new RelayCommand(CreateOrder)); }
        }

        #endregion

        #region Method

        public SpecimenCollectionViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = null;
            var refValue = DataService.Technical.GetReferenceValueMany("ORDST");
            RequestStatus = refValue.Where(p => p.ValueCode == "RAISED"
                || p.ValueCode == "REVIW"
    || p.ValueCode == "SAMPLCOL"
    || p.ValueCode == "ACPSMP"
    || p.ValueCode == "PARCOLLEC"
    || p.ValueCode == "PREVI"
    || p.ValueCode == "PRCAN"
    || p.ValueCode == "PARCMP"
    || p.ValueCode == "CANCLD").OrderBy(p => p.Display).ToList();

            SelectRequestStatusList.Add(RequestStatus.FirstOrDefault(p => p.ValueCode == "RAISED").Key);
            RequesItems = DataService.MasterData.GetRequestItemByCategory("LAB");

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

            SelectPrinter = PrinterLists.FirstOrDefault(p => p.Display.ToLower().Contains("sticker"));

            SearchRequestDetailSpecimen();
        }

        void SearchRequestDetailSpecimen()
        {
            long? patientUID = null;
            string statusList = string.Empty;

            if (!string.IsNullOrEmpty(SearchPatientCriteria))
            {
                if (SelectedPateintSearch != null)
                {
                    patientUID = SelectedPateintSearch.PatientUID;
                }
            }

            if (SelectRequestStatusList != null)
            {
                foreach (object item in SelectRequestStatusList)
                {
                    if (item.ToString() != "0")
                    {
                        if (statusList == "")
                        {
                            statusList = item.ToString();
                        }
                        else
                        {
                            statusList += "," + item.ToString();
                        }
                    }

                }
            }
            int? requestItemUID = SelectRequestItem != null ? SelectRequestItem.RequestItemUID : (int?)null;

            RequestLabs = DataService.Lab.SearchRequestLabList(DateFrom, DateTo, statusList, patientUID, requestItemUID, LabNumber, null, null);
            if (SelectRequestLab != null)
                SelectRequestLab = null;
        }

        void CollectAndPrint()
        {
            if (SelectPrinter == null)
            {
                WarningDialog("กรุณาเลือก Printer");
                return;
            }

            Collect();
            GenareateBarcode(RequestDetailSpecimens);
        }
        
        void PrintBarcode()
        {
            if (SelectRequestLab != null)
            {
                var requestDetailSpecimens = DataService.Lab.GetRequestDetailSpecimenByRequestUID(SelectRequestLab.RequestUID);
                GenareateBarcode(requestDetailSpecimens);
            }

        }
        void GenareateBarcode(IEnumerable<RequestDetailSpecimenModel> requestDetailSpecimens)
        {
            try
            {
                if (SelectPrinter == null)
                {
                    WarningDialog("กรุณาเลือก Printer");
                    return;
                }

                if (requestDetailSpecimens != null && requestDetailSpecimens.Count() > 0 && SelectRequestLab != null)
                {
                    var specimenSticker = requestDetailSpecimens.GroupBy(p => new { p.SpecimenName, p.CollectionDttm,p.Suffix })
                        .Select(g => new
                        {
                            SpecimenName = g.FirstOrDefault().SpecimenName,
                            CollectionDttm = g.FirstOrDefault().CollectionDttm,
                            Suffix = g.FirstOrDefault().Suffix
                        }).ToList();

                    if (specimenSticker != null)
                    {
                        string patientName = SelectRequestLab.PatientName;
                        string patientID = SelectRequestLab.PatientID;
                        string birthDate = SelectRequestLab.BirthDate != DateTime.MinValue ? SelectRequestLab.BirthDate.ToString("dd/MM/yyyy") : "";
                        string age = SelectRequestLab.BirthDate != DateTime.MinValue ? ShareLibrary.UtilDate.calAgeFromBirthDate(SelectRequestLab.BirthDate) : "";
                        string requestNumber = SelectRequestLab.LabNumber;
                        foreach (var specimen in specimenSticker)
                        {
                            SpecimenBarcode rpt = new SpecimenBarcode();
                            ReportPrintTool printTool = new ReportPrintTool(rpt);

                            rpt.Parameters["PatientName"].Value = patientName;
                            rpt.Parameters["PatientID"].Value = patientID;
                            rpt.Parameters["BirthDate"].Value = birthDate;
                            rpt.Parameters["Age"].Value = age;
                            rpt.Parameters["RequestNumber"].Value = requestNumber + specimen.Suffix;
                            rpt.Parameters["SpecimenName"].Value = specimen.SpecimenName;
                            rpt.Parameters["CollectionDttm"].Value = specimen.CollectionDttm.HasValue ? specimen.CollectionDttm.Value.ToString("dd/MM/yyyy HH:mm") : "";
                            rpt.RequestParameters = false;
                            rpt.ShowPrintMarginsWarning = false;
                            printTool.Print(SelectPrinter.Display);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }


        }

        void SelectedSpecimen(int specimenUID)
        {
            if (!SurpassEventChecked)
            {
                SurpassEventChecked = true;
                var specimens = RequestDetailSpecimens.Where(p => p.SpecimenUID == specimenUID && specimenUID != 0 && p.Selected == false);
                foreach (var item in specimens)
                {
                    if (item.EnableSelect)
                        item.Selected = true;
                }
                SurpassEventChecked = false;
            }
        }

        void Collect()
        {
            try
            {
                if (RequestDetailSpecimens != null && RequestDetailSpecimens.Count() > 0)
                {
                    int SAMPLCOL = 2862;
                    int RAISED = 2847;
                    foreach (var item in RequestDetailSpecimens)
                    {
                        if (item.EnableSelect)
                        {
                            if (item.Selected)
                            {
                                item.SPSTSUID = SAMPLCOL;
                            }
                            else
                            {
                                item.SPSTSUID = RAISED;
                            }
                        }
                    }
                    DataService.Lab.UpdateRequestDetailSpecimens(RequestDetailSpecimens.ToList(), AppUtil.Current.UserID);
                    GetRequestDetailSpecimen(SelectRequestLab.RequestUID);
                }
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }

        void GetRequestDetailSpecimen(long requestUID)
        {
            RequestDetailSpecimens = new ObservableCollection<RequestDetailSpecimenModel>(DataService.Lab.GetRequestDetailSpecimenByRequestUID(requestUID));
            if (RequestDetailSpecimens != null)
            {
                foreach (var item in RequestDetailSpecimens)
                {
                    if (item.SpecimenStatus == "Specimen Collected" || item.SpecimenStatus == "Raised")
                    {
                        if (item.SpecimenStatus == "Specimen Collected")
                        {
                            item.Selected = true;
                        }
                        item.EnableSelect = true;
                    }
                    else
                    {
                        item.EnableSelect = false;
                    }
                }
            }
        }

        void DefaultControl()
        {
            DateFrom = DateTime.Now;
            DateTo = null;
            SelectRequestStatusList.Clear();
            LabNumber = "";
            SelectRequestStatusList.Add(RequestStatus.FirstOrDefault(p => p.ValueCode == "RAISED").Key);
            (this.View as SpecimenCollection).cmbStatus.RefreshData();
            SelectRequestItem = null;
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

        private void CreateOrder()
        {
            if (SelectRequestLab != null)
            {
                int FINDIS = 421;
                if (SelectRequestLab.VISTSUID == FINDIS)
                {
                    WarningDialog("ไม่สามารถดำเนินการได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
                    return;
                }
                PatientOrderEntry pageview = new PatientOrderEntry();
                PatientVisitModel visitModel = new PatientVisitModel();
                visitModel.PatientUID = SelectRequestLab.PatientUID;
                visitModel.PatientVisitUID = SelectRequestLab.PatientVisitUID;
                visitModel.OwnerOrganisationUID = SelectRequestLab.OwnerOrganisationUID;
                visitModel.OwnerOrganisation = SelectRequestLab.OrganisationName;
                (pageview.DataContext as PatientOrderEntryViewModel).AssingPatientVisit(visitModel);
                PatientOrderEntryViewModel result = (PatientOrderEntryViewModel)LaunchViewDialog(pageview, "ORDITM", false, true);

                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SearchRequestDetailSpecimen();
                }
            }
        }

        #endregion
    }
}
