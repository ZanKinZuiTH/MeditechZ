using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.Model;
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
    public class PatientBilledIPViewModel : MediTechViewModelBase
    {
        int? opbill;
        int? ipdbill;

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

        private string _BillNumber;

        public string BillNumber
        {
            get { return _BillNumber; }
            set { Set(ref _BillNumber, value); }
        }

        private ObservableCollection<PatientBillModel> _PatientBillSource;

        public ObservableCollection<PatientBillModel> PatientBillSource
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

        private List<LookupReferenceValueModel> _BillingCategory;

        public List<LookupReferenceValueModel> BillingCategory
        {
            get { return _BillingCategory; }
            set { Set(ref _BillingCategory, value); }
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

        private RelayCommand _ExportToExcelCommand;

        public RelayCommand ExportToExcelCommand
        {
            get { return _ExportToExcelCommand ?? (_ExportToExcelCommand = new RelayCommand(ExportToExcel)); }
        }
        #endregion

        #region Method
        public PatientBilledIPViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            BillingCategory = DataService.Technical.GetReferenceValueMany("BLCAT");
            opbill = BillingCategory.FirstOrDefault(p => p.ValueCode == "OPBILL").Key;
            ipdbill = BillingCategory.FirstOrDefault(p => p.ValueCode == "IPBILL").Key;
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
            int? ownerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
            PatientBillSource = new ObservableCollection<PatientBillModel>(DataService.Billing.SearchPatientBill(DateFrom, DateTo, patientUID, BillNumber,"Y", ownerOrganisationUID));
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

        public void UpdatePatientBill(long patientBillUID, int PAYMDUID)
        {
            try
            {
                DataService.Billing.UpdatePaymentMethod(patientBillUID, PAYMDUID, AppUtil.Current.UserID);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, "");
                PatientsSearchSource = searchResult;
            }
            else
            {
                PatientsSearchSource = null;
            }

        }


        private void ExportToExcel()
        {
            try
            {
                if (PatientBillSource != null)
                {
                    string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xlsx");
                    if (fileName != "")
                    {
                        PatientBilledOP view = (PatientBilledOP)this.View;
                        view.gvPatBill.ExportToXlsx(fileName);
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
