using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Reports.Operating.Cashier;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class PatientBilledViewModel : MediTechViewModelBase
    {

        #region Properties

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
            }
        }
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

        private DateTime? _DateFrom;

        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set { Set(ref _DateFrom , value); }
        }

        private DateTime? _DateTo;

        public DateTime? DateTo
        {
            get { return _DateTo; }
            set { Set(ref _DateTo, value); }
        }

        private string _BillNumber;

        public string BillNumber
        {
            get { return _BillNumber; }
            set { Set(ref _BillNumber, value); }
        }

        private List<PatientBillModel> _PatientBillSource;

        public List<PatientBillModel> PatientBillSource
        {
            get { return _PatientBillSource; }
            set { Set(ref _PatientBillSource, value); }
        }

        private PatientBillModel _SelectPatientBill;

        public PatientBillModel SelectPatientBill
        {
            get { return _SelectPatientBill; }
            set { _SelectPatientBill = value; }
        }


        #endregion

        #region Command

        private RelayCommand _PatientSearchCommand;

        public RelayCommand PatientSearchCommand
        {
            get { return _PatientSearchCommand ?? (_PatientSearchCommand = new RelayCommand(PatientSearch)); }
        }

        private RelayCommand _ViewBillCommand;

        public RelayCommand ViewBillCommand
        {
            get { return _ViewBillCommand ?? (_ViewBillCommand = new RelayCommand(ViewBill)); }
        }

        private RelayCommand _CancelBillCommand;

        public RelayCommand CancelBillCommand
        {
            get { return _CancelBillCommand ?? (_CancelBillCommand = new RelayCommand(CancelBill)); }
        }

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(SearchPatientBill)); }
        }

        private RelayCommand _ClearCommand;

        public RelayCommand ClearCommand
        {
            get { return _ClearCommand ?? (_ClearCommand = new RelayCommand(ClearData)); }
        }

        #endregion

        #region Method

        public PatientBilledViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            Organisations = GetHealthOrganisationRoleMedical();
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
        }

        public override void OnLoaded()
        {
            SearchPatientBill();
        }

        public void SearchPatientBill()
        {
            long? patientUID = null;

            if (SelectedPateintSearch != null && SearchPatientCriteria != "")
            {
                patientUID = SelectedPateintSearch.PatientUID;
            }
            int? ownerOrganisationUID = (SelectOrganisation != null && SelectOrganisation.HealthOrganisationUID != 0) ? SelectOrganisation.HealthOrganisationUID : (int?)null;
            PatientBillSource = DataService.Billing.SearchPatientBill(DateFrom, DateTo, patientUID, BillNumber, ownerOrganisationUID);
        }

        public void ViewBill()
        {
            if (SelectPatientBill != null)
            {
                try
                {

                    XtraReport report;
                    if (SelectPatientBill.VisitType != "Non Medical")
                    {
                        report = new PatientBill();
                    }
                    else
                    {
                        report = new PatientBill2();
                    }
                    report.Parameters["OrganisationUID"].Value = SelectPatientBill.OwnerOrganisationUID;
                    report.Parameters["PatientBillUID"].Value = SelectPatientBill.PatientBillUID;
                    report.RequestParameters = false;

                    ReportPrintTool printTool = new ReportPrintTool(report);
                    report.ShowPrintMarginsWarning = false;
                    printTool.ShowPreviewDialog();
                }
                catch (Exception er)
                {

                    ErrorDialog(er.Message);
                }
            };
        }

        public void CancelBill()
        {
            if (SelectPatientBill != null)
            {
                if (SelectPatientBill.IsCancel)
                {
                    WarningDialog("รายการนี้ถูกยกเลิกไปแล้ว");
                    return;
                }
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
                        DataService.Billing.CancelBill(SelectPatientBill.PatientBillUID, result.Comments, AppUtil.Current.UserID);
                        SaveSuccessDialog();
                        SearchPatientBill();
                    }

                }
                catch (Exception er)
                {

                    ErrorDialog(er.Message);
                }

            }
        }

        public void ClearData()
        {
            BillNumber = string.Empty;
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            SearchPatientCriteria = string.Empty;
            PatientBillSource = null;
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
