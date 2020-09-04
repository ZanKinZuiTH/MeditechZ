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
            set {
                Set(ref _SelectRequestLab, value);
                if (SelectRequestLab != null)
                {
                    RequestDetailLabs = DataService.Lab.GetRequesDetailLabByRequestUID(SelectRequestLab.RequestUID);
                }
            }
        }

        private List<RequestDetailLabModel> _RequestDetailLabs;

        public List<RequestDetailLabModel> RequestDetailLabs
        {
            get { return _RequestDetailLabs; }
            set { Set(ref _RequestDetailLabs, value); }
        }

        private RequestDetailLabModel _SelectRequestDetailLab;

        public RequestDetailLabModel SelectRequestDetailLab
        {
            get { return _SelectRequestDetailLab; }
            set { Set(ref _SelectRequestDetailLab, value); }
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

        #endregion

        #region Method

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

            RequestLabs =  DataService.Lab.SearchRequestLabList(DateFrom, DateTo, statusOrder, patientUID, requestItemUID, LabNumber, payorDetailUID, organisationUID);
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
                EnterResultsLabViewModel enterResultsLabModel = (EnterResultsLabViewModel)LaunchViewDialog(enterResult, "ENTRE", false,false);

                if (enterResultsLabModel == null)
                {
                    return;
                }

                if (enterResultsLabModel != null && enterResultsLabModel.ResultDialog == ActionDialog.Save)
                {
                    RequestDetailLabs = DataService.Lab.GetRequesDetailLabByRequestUID(SelectRequestLab.RequestUID);
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
            LabResultReport rpt = new LabResultReport();
            ReportPrintTool printTool = new ReportPrintTool(rpt);


            rpt.Parameters["PatientVisitUID"].Value = SelectRequestLab.PatientVisitUID;
            rpt.Parameters["RequestNumber"].Value = SelectRequestLab.LabNumber;
            rpt.PrintAuto = true;
            rpt.RequestParameters = false;
            rpt.ShowPrintMarginsWarning = false;
            printTool.Print();

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
