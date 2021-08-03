using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.Helpers;
using MediTech.Model;
using MediTech.Model.Report;
using MediTech.Reports.Operating.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class RunPatientReportsViewModel : MediTechViewModelBase
    {
        #region Properties

        private PatientVisitModel _SelectPatientVisit;

        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set { Set(ref _SelectPatientVisit, value); }
        }

        private List<ReportsModel> _ListPatientReports;

        public List<ReportsModel> ListPatientReports
        {
            get { return _ListPatientReports; }
            set { Set(ref _ListPatientReports, value); }
        }

        private ReportsModel _SelectReport;

        public ReportsModel SelectReport
        {
            get { return _SelectReport; }
            set { Set(ref _SelectReport, value); }
        }


        private int _Quantity;

        public int Quantity
        {
            get { return _Quantity; }
            set { Set(ref _Quantity, value); }
        }


        private List<LookupReferenceValueModel> _MobileStickerSource;

        public List<LookupReferenceValueModel> MobileStickerSource
        {
            get { return _MobileStickerSource; }
            set { Set(ref _MobileStickerSource, value); }
        }

        private IList<object> _SelectMobileStickers;

        public IList<object> SelectMobileStickers
        {
            get { return _SelectMobileStickers; }
            set { Set(ref _SelectMobileStickers, value); }
        }
        #endregion

        #region Command

        private RelayCommand _PreviewReportCommand;

        /// <summary>
        /// Gets the PrintCommand.
        /// </summary>
        public RelayCommand PreviewReportCommand
        {
            get
            {
                return _PreviewReportCommand
                    ?? (_PreviewReportCommand = new RelayCommand(PreviewReport));
            }
        }


        private RelayCommand _PrintAutoCommand;

        /// <summary>
        /// Gets the PrintAutoCommand.
        /// </summary>
        public RelayCommand PrintAutoCommand
        {
            get
            {
                return _PrintAutoCommand
                    ?? (_PrintAutoCommand = new RelayCommand(PrintAuto));
            }
        }

        private RelayCommand _CancelCommand;

        /// <summary>
        /// Gets the CancelCommand.
        /// </summary>
        public RelayCommand CancelCommand
        {
            get
            {
                return _CancelCommand
                    ?? (_CancelCommand = new RelayCommand(Cancel));
            }
        }

        private RelayCommand _MobileStickerCommand;
        public RelayCommand MobileStickerCommand
        {
            get
            {
                return _MobileStickerCommand
                    ?? (_MobileStickerCommand = new RelayCommand(MobileSticker));
            }
        }
        #endregion

        #region Method

        public RunPatientReportsViewModel()
        {
            ListPatientReports = ConstantData.GetPatientReports();

            Quantity = 1;

            MobileStickerSource = new List<LookupReferenceValueModel>();
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "X-ray" });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "EKG" });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "ปัสสาวะ" });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 3, Display = "เจาะเลือด" });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "พบแพทย์" });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 2, Display = "ใบนำทาง" });
            SelectMobileStickers = new List<object>() { MobileStickerSource[0], MobileStickerSource[1], MobileStickerSource[2]
                , MobileStickerSource[3], MobileStickerSource[4], MobileStickerSource[5]};

        }

        private void PreviewReport()
        {
            if (SelectReport != null)
            {
                var myReport = Activator.CreateInstance(Type.GetType(SelectReport.NamespaceName));
                XtraReport report = (XtraReport)myReport;
                ReportPrintTool printTool = new ReportPrintTool(report);
                if (SelectReport.Name == "ใบรับรองแพทย์โควิดนอกสถานที่")
                    printTool.PrintingSystem.StartPrint += PrintingSystem_StartPrint;

                if (SelectReport.Name == "ปริ้น Sticker" || SelectReport.Name == "ปริ้น Sticker Large")
                {
                    report.Parameters["OrganisationUID"].Value = SelectPatientVisit.OwnerOrganisationUID;
                    report.Parameters["HN"].Value = SelectPatientVisit.PatientID;
                    report.Parameters["PatientName"].Value = SelectPatientVisit.PatientName;
                    report.Parameters["Age"].Value = SelectPatientVisit.Age;
                    report.Parameters["BirthDate"].Value = SelectPatientVisit.BirthDttm != null ? SelectPatientVisit.BirthDttm.Value.ToString("dd/MM/yyyy") : null;
                    report.Parameters["Payor"].Value = SelectPatientVisit.PayorName;
                    report.RequestParameters = false;
                    report.ShowPrintMarginsWarning = false;
                    printTool.ShowPreviewDialog();
                }
                else
                {
                    report.Parameters["OrganisationUID"].Value = SelectPatientVisit.OwnerOrganisationUID;
                    report.Parameters["PatientUID"].Value = SelectPatientVisit.PatientUID;
                    report.Parameters["PatientVisitUID"].Value = SelectPatientVisit.PatientVisitUID;
                    report.RequestParameters = false;
                    report.ShowPrintMarginsWarning = false;
                    printTool.ShowPreviewDialog();
                }

            }
        }


        private void PrintAuto()
        {
            if (SelectReport != null)
            {
                var myReport = Activator.CreateInstance(Type.GetType(SelectReport.NamespaceName));
                XtraReport report = (XtraReport)myReport;
                ReportPrintTool printTool = new ReportPrintTool(report);
                if (SelectReport.Name == "ใบรับรองแพทย์โควิดนอกสถานที่")
                    printTool.PrintingSystem.StartPrint += PrintingSystem_StartPrint;

                for (int i = 1; i <= Quantity; i++)
                {
                    if (SelectReport.Name == "ปริ้น Sticker" || SelectReport.Name == "ปริ้น Sticker Large")
                    {
                        report.Parameters["OrganisationUID"].Value = SelectPatientVisit.OwnerOrganisationUID;
                        report.Parameters["HN"].Value = SelectPatientVisit.PatientID;
                        report.Parameters["PatientName"].Value = SelectPatientVisit.PatientName;
                        report.Parameters["Age"].Value = SelectPatientVisit.Age;
                        report.Parameters["BirthDate"].Value = SelectPatientVisit.BirthDttm != null ? SelectPatientVisit.BirthDttm.Value.ToString("dd/MM/yyyy") : null;
                        report.Parameters["Payor"].Value = SelectPatientVisit.PayorName;
                        report.RequestParameters = false;
                        report.ShowPrintMarginsWarning = false;
                        printTool.Print();
                    }
                    else
                    {
                        report.Parameters["OrganisationUID"].Value = SelectPatientVisit.OwnerOrganisationUID;
                        report.Parameters["PatientUID"].Value = SelectPatientVisit.PatientUID;
                        report.Parameters["PatientVisitUID"].Value = SelectPatientVisit.PatientVisitUID;
                        report.RequestParameters = false;
                        report.ShowPrintMarginsWarning = false;
                        printTool.Print();
                    }
                }


            }
        }

        private void MobileSticker()
        {
            try
            {
                foreach (var Selectitem in SelectMobileStickers)
                {
                    var item = (Selectitem as LookupReferenceValueModel);
                    for (int i = 0; i < item.Key; i++)
                    {
                        MobileSticker rpt = new MobileSticker();
                        ReportPrintTool printTool = new ReportPrintTool(rpt);

                        string gender;
                        switch (SelectPatientVisit.Gender)
                        {
                            case "หญิง":
                            case "F":
                                gender = "(F)";
                                break;
                            case "ชาย":
                            case "M":
                                gender = "(M)";
                                break;
                            default:
                                gender = "(N/A)";
                                break;
                        }

                        rpt.Parameters["PatientName"].Value = SelectPatientVisit.PatientName + " " + gender;

                        rpt.Parameters["HN"].Value = SelectPatientVisit.PatientID;
                        //rpt.Parameters["No"].Value = "";
                        rpt.lblNo.Visible = false;

                        if (SelectPatientVisit.BirthDttm != null)
                        {
                            rpt.Parameters["Age"].Value = ShareLibrary.UtilDate.calAgeFromBirthDate(SelectPatientVisit.BirthDttm.Value);
                        }

                        rpt.Parameters["BirthDttm"].Value = SelectPatientVisit.BirthDttm != null ? SelectPatientVisit.BirthDttm.Value.ToString("dd/MM/yyyy") : "";
                        rpt.Parameters["CheckUp"].Value = item.Display;
                        rpt.RequestParameters = false;
                        rpt.ShowPrintMarginsWarning = false;
                        printTool.Print();

                    }
                }

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }
        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        private void PrintingSystem_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            e.PrintDocument.PrinterSettings.FromPage = 2;
            e.PrintDocument.PrinterSettings.ToPage = 2;
        }

        #endregion
    }
}
