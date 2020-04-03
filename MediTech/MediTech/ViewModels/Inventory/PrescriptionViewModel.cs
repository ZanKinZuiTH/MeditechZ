using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;
using MediTech.Model;
using MediTech.Reports.Operating.Pharmacy;
using DevExpress.XtraReports.UI;

namespace MediTech.ViewModels
{
    public class PrescriptionViewModel : MediTechViewModelBase
    {

        #region Properties
        private DateTime? _DateFrom;
        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set
            {
                Set(ref _DateFrom, value);
            }
        }

        private DateTime? _DateTo;
        public DateTime? DateTo
        {
            get { return _DateTo; }
            set
            {
                Set(ref _DateTo, value);
            }
        }

        private List<LookupReferenceValueModel> _PrescritionStatus;

        public List<LookupReferenceValueModel> PrescritionStatus
        {
            get { return _PrescritionStatus; }
            set { Set(ref _PrescritionStatus, value); }
        }

        private LookupReferenceValueModel _SelectPrescritionStatus;

        public LookupReferenceValueModel SelectPrescritionStatus
        {
            get { return _SelectPrescritionStatus; }
            set { Set(ref _SelectPrescritionStatus, value); }
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

        private List<PrescriptionModel> _Prescriptons;

        public List<PrescriptionModel> Prescriptons
        {
            get { return _Prescriptons; }
            set { Set(ref _Prescriptons, value); }
        }

        private PrescriptionModel _SelectPrescription;

        public PrescriptionModel SelectPrescription
        {
            get { return _SelectPrescription; }
            set
            {
                Set(ref _SelectPrescription, value);
                if (_SelectPrescription != null)
                {
                    PrescriptionItems = DataService.Inventory.GetPrescriptionItemByPrescriptionUID(SelectPrescription.PrescriptionUID);
                }
            }
        }

        private List<PrescriptionItemModel> _PrescriptionItems;

        public List<PrescriptionItemModel> PrescriptionItems
        {
            get { return _PrescriptionItems; }
            set { Set(ref _PrescriptionItems, value); }
        }

        private List<PrescriptionItemModel> _SelectPrescriptionItems;

        public List<PrescriptionItemModel> SelectPrescriptionItems
        {
            get { return _SelectPrescriptionItems ?? (_SelectPrescriptionItems = new List<PrescriptionItemModel>()); }
            set { Set(ref _SelectPrescriptionItems, value); }
        }

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
                if (_SelectedPateintSearch != null)
                {
                    SearchPrescrition();
                    SearchPatientCriteria = string.Empty;
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


        private string _PrescriptionNumber;

        public string PrescriptionNumber
        {
            get { return _PrescriptionNumber; }
            set { _PrescriptionNumber = value; }
        }


        #endregion

        #region Command

        private RelayCommand _PatientSearchCommand;

        public RelayCommand PatientSearchCommand
        {
            get { return _PatientSearchCommand ?? (_PatientSearchCommand = new RelayCommand(PatientSearch)); }
        }

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(SearchPrescrition)); }
        }

        private RelayCommand _ClearCommand;

        public RelayCommand ClearCommand
        {
            get { return _ClearCommand ?? (_ClearCommand = new RelayCommand(DefaultControl)); }
        }

        private RelayCommand _PrintDrugStickerCommand;

        public RelayCommand PrintDrugStickerCommand
        {
            get { return _PrintDrugStickerCommand ?? (_PrintDrugStickerCommand = new RelayCommand(PrintDrugSticker)); }
        }

        #endregion

        #region Method

        public PrescriptionViewModel()
        {
            Organisations = GetHealthOrganisationIsStock();
            var refValues = DataService.Technical.GetReferenceValueMany("ORDST");
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
            PrescritionStatus = refValues.Where(p => p.ValueCode == "RAISED" || p.ValueCode == "DISPE" || p.ValueCode == "CANCLD" 
            || p.ValueCode == "DISPCANCL" || p.ValueCode == "OPDISP"
            || p.ValueCode == "OPCANDISP" || p.ValueCode == "PRCAN").ToList();
            DefaultControl();
        }


        void SearchPrescrition()
        {
            int? ORDSTUID = null;
            long? patientUID = null;
            int? organisationUID = null;

            if (SelectPrescritionStatus != null)
            {
                ORDSTUID = SelectPrescritionStatus.Key;
            }

            if (!string.IsNullOrEmpty(SearchPatientCriteria))
            {
                if (SelectedPateintSearch != null)
                {
                    patientUID = SelectedPateintSearch.PatientUID;
                }
            }

            if (SelectOrganisation != null)
            {
                organisationUID = SelectOrganisation.HealthOrganisationUID;
            }

            Prescriptons = DataService.Inventory.Searchprescription(DateFrom,DateTo, ORDSTUID, patientUID, PrescriptionNumber, organisationUID);
        }

        void PrintDrugSticker()
        {
            if (SelectPrinter == null)
            {
                WarningDialog("กรุณาเลือก Printer");
                return;
            }


            foreach (var item in SelectPrescriptionItems)
            {
                DrugSticker rpt = new DrugSticker();
                ReportPrintTool printTool = new ReportPrintTool(rpt);

                rpt.Parameters["PrescriptionItemUID"].Value = item.PrescriptionItemUID;
                rpt.Parameters["ExpiryDate"].Value = item.ExpiryDate;
                rpt.RequestParameters = false;
                rpt.ShowPrintMarginsWarning = false;
                printTool.Print(SelectPrinter.Display);
            }
        }
        void DefaultControl()
        {
            DateFrom = DateTime.Now;
            DateTo = null;
            SelectPrescritionStatus = null;
            SelectedPateintSearch = null;
            SearchPatientCriteria = "";
            PrescriptionNumber = "";
            SelectOrganisation = null;
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
        #endregion
    }
}
