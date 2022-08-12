using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;
using MediTech.Model;
using MediTech.Reports.Operating.Pharmacy;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using System.Collections.ObjectModel;
using MediTech.Views;
using MediTech.Reports.Operating.Patient;

namespace MediTech.ViewModels
{
    public class PrescriptionViewModel : MediTechViewModelBase
    {

        #region Properties
        public ReportsModel ReportTemplate { get; set; }


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

        private List<object> _SelectPrescritionStatus;

        public List<object> SelectPrescritionStatus
        {
            get { return _SelectPrescritionStatus ?? (_SelectPrescritionStatus = new List<object>()); }
            set { Set(ref _SelectPrescritionStatus, value); }
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
                if(_SelectPrescription != null)
                {
                    IsEnableCancelDispense = false;
                    IsEnableEditDispense = false;
                    IsEnableDispense = false;
                    if (_SelectPrescription.IsBilled == "N")
                    {
                        if (_SelectPrescription.PrescriptionStatus == "Dispensed" || _SelectPrescription.PrescriptionStatus == "Partially Dispensed")
                        {
                            IsEnableCancelDispense = true;
                        }
                    }

                    if (_SelectPrescription.PrescriptionStatus == "Partially Dispensed")
                    {
                        IsEnableEditDispense = true;
                    }

                    if (_SelectPrescription.PrescriptionStatus == "Raised")
                    {
                        IsEnableDispense = true;
                    }
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



        #endregion


        private string _PrescriptionNumber;

        public string PrescriptionNumber
        {
            get { return _PrescriptionNumber; }
            set { Set(ref _PrescriptionNumber, value); }
        }


        private bool _IsEnableCancelDispense = false;

        public bool IsEnableCancelDispense
        {
            get { return _IsEnableCancelDispense; }
            set { Set(ref _IsEnableCancelDispense, value); }
        }

        private bool _IsEnableEditDispense = false;

        public bool IsEnableEditDispense
        {
            get { return _IsEnableEditDispense; }
            set { Set(ref _IsEnableEditDispense, value); }
        }


        private bool _IsEnableDispense = false;

        public bool IsEnableDispense
        {
            get { return _IsEnableDispense; }
            set { Set(ref _IsEnableDispense, value); }
        }

        #endregion

        #region Command

        private RelayCommand _CreateOrderCommand;

        public RelayCommand CreateOrderCommand
        {
            get { return _CreateOrderCommand ?? (_CreateOrderCommand = new RelayCommand(CreateOrder)); }
        }

        private RelayCommand _PatientRecordsCommand;

        public RelayCommand PatientRecordsCommand
        {
            get { return _PatientRecordsCommand ?? (_PatientRecordsCommand = new RelayCommand(PatientRecords)); }
        }

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

        private RelayCommand _DispenseCommand;

        public RelayCommand DispenseCommand
        {
            get { return _DispenseCommand ?? (_DispenseCommand = new RelayCommand(Dispense)); }
        }

        private RelayCommand _EditDispenseCommand;

        public RelayCommand EditDispenseCommand
        {
            get { return _EditDispenseCommand ?? (_EditDispenseCommand = new RelayCommand(EditDispense)); }
        }


        private RelayCommand _CancelDispenseCommand;

        public RelayCommand CancelDispenseCommand
        {
            get { return _CancelDispenseCommand ?? (_CancelDispenseCommand = new RelayCommand(CancelDispense)); }
        }

        private RelayCommand _PrintStickerCommand;

        public RelayCommand PrintStickerCommand
        {
            get { return _PrintStickerCommand ?? (_PrintStickerCommand = new RelayCommand(PrintSticker)); }
        }

        private RelayCommand _PrintPrescriptionCommand;
        public RelayCommand PrintPrescriptionCommand
        {
            get { return _PrintPrescriptionCommand ?? (_PrintPrescriptionCommand = new RelayCommand(PrintPrescription)); }
        }

        private RelayCommand _PrintPrescriptionOPDCommand;
        public RelayCommand PrintPrescriptionOPDCommand
        {
            get { return _PrintPrescriptionOPDCommand ?? (_PrintPrescriptionOPDCommand = new RelayCommand(PrintPrescriptionOPD)); }
        }


        #endregion

        #region Method

        public PrescriptionViewModel()
        {
            
            var refValues = DataService.Technical.GetReferenceValueMany("ORDST");
            PrescritionStatus = refValues.Where(p => p.ValueCode == "RAISED" || p.ValueCode == "DISPE" || p.ValueCode == "CANCLD"
            || p.ValueCode == "DISPCANCL" || p.ValueCode == "OPDISP"
            || p.ValueCode == "OPCANDISP" || p.ValueCode == "PRCAN").ToList();

            DefaultControl();
            SearchPrescrition();
        }




        void SearchPrescrition()
        {
            long? patientUID = null;

            string statusList = string.Empty;
            if (SelectPrescritionStatus != null)
            {
                foreach (object item in SelectPrescritionStatus)
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

            if (!string.IsNullOrEmpty(SearchPatientCriteria))
            {
                if (SelectedPateintSearch != null)
                {
                    patientUID = SelectedPateintSearch.PatientUID;
                }
            }

            Prescriptons = DataService.Pharmacy.Searchprescription(DateFrom, DateTo, statusList, patientUID, PrescriptionNumber, AppUtil.Current.OwnerOrganisationUID);
           
        }

        private void PrintSticker()
        {
            if (SelectPrescription != null)
            {
                PrintDrugSticker pageview = new PrintDrugSticker();
                (pageview.DataContext as PrintDrugStickerViewModel).AssignModel(SelectPrescription);
                LaunchViewDialogNonPermiss(pageview, false, false);
            }
        }

        private void CreateOrder()
        {
            if (SelectPrescription != null)
            {
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectPrescription.PatientVisitUID);
                PatientOrderEntry pageview = new PatientOrderEntry();
                (pageview.DataContext as PatientOrderEntryViewModel).AssingPatientVisit(patientVisit);
                LaunchViewDialog(pageview, "ORDITM", false, true);
            }
        }

        private void PatientRecords()
        {
            if (SelectPrescription != null)
            {
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectPrescription.PatientVisitUID);
                EMRView pageview = new EMRView();
                (pageview.DataContext as EMRViewViewModel).AssignPatientVisit(patientVisit);
                LaunchViewDialog(pageview, "EMRVE", false, true);
            }
        }



        void DefaultControl()
        {
            DateFrom = DateTime.Now;
            DateTo = null;
            SelectPrescritionStatus.Add(PrescritionStatus.FirstOrDefault(p => p.ValueCode == "RAISED").Key);
            SelectPrescritionStatus.Add(PrescritionStatus.FirstOrDefault(p => p.ValueCode == "OPDISP").Key);
            SelectedPateintSearch = null;
            SearchPatientCriteria = "";
            PrescriptionNumber = "";
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, "", "");
                PatientsSearchSource = searchResult;
            }
            else
            {
                PatientsSearchSource = null;
            }

        }


        public void CancelDispense()
        {
            if (SelectPrescription != null && SelectPrescription.IsBilled == "N")
            {
                if (SelectPrescription.PrescriptionStatus == "Dispensed" || SelectPrescription.PrescriptionStatus == "Partially Dispensed")
                {
                    var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectPrescription.PatientVisitUID);
                    CancelDispense cancelDispense = new CancelDispense();
                    (cancelDispense.DataContext as CancelDispenseViewModel).AssignModel(SelectPrescription.PrescriptionItems, patientVisit);
                    ChangeViewPermission(cancelDispense);
                }
            }
        }

        public void EditDispense()
        {
            if (SelectPrescription != null)
            {
                if (SelectPrescription.PrescriptionStatus == "Partially Dispensed")
                {
                    var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectPrescription.PatientVisitUID);
                    DispenseDrug dispense = new DispenseDrug();
                    var dataContext = (dispense.DataContext as DispenseDrugViewModel);
                    dataContext.IsEditDispense = true;
                    dataContext.AssingModel(SelectPrescription, patientVisit);
                    ChangeViewPermission(dispense);
                }
            }
        }


        private void PrintPrescription()
        {
            if (SelectPrescription != null)
            {

                PatientPrescription rpt = new PatientPrescription();
                ReportPrintTool printTool = new ReportPrintTool(rpt);
                rpt.Parameters["prescriptionUID"].Value = SelectPrescription.PrescriptionUID;
                rpt.Parameters["OrganisationUID"].Value = AppUtil.Current.OwnerOrganisationUID;
                rpt.RequestParameters = false;
                rpt.ShowPrintMarginsWarning = false;
                printTool.ShowPreviewDialog();
            }
        }

        private void PrintPrescriptionOPD()
        {
            if (SelectPrescription != null)
            {
                OPPrescription rpt = new OPPrescription();
                ReportPrintTool printTool = new ReportPrintTool(rpt);
                rpt.Parameters["PrescriptionUID"].Value = SelectPrescription.PrescriptionUID;
                rpt.Parameters["OrganisationUID"].Value = SelectPrescription.OwnerOrganisationUID;
                rpt.Parameters["LogoType"].Value = SelectPrescription.OwnerOrganisationUID;
                rpt.RequestParameters = false;
                rpt.ShowPrintMarginsWarning = false;
                printTool.ShowPreviewDialog();
            }
        }


        public void Dispense()
        {
            if (SelectPrescription != null)
            {
                if (SelectPrescription.PrescriptionStatus == "Raised")
                {
                    var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectPrescription.PatientVisitUID);
                    DispenseDrug dispense = new DispenseDrug();
                    (dispense.DataContext as DispenseDrugViewModel).AssingModel(SelectPrescription,patientVisit);
                    ChangeViewPermission(dispense);
                }
            }
        }

        #endregion
    }
}
