using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Reports.Operating.Pharmacy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class PrintDrugStickerViewModel : MediTechViewModelBase
    {
        #region Properties

        private PrescriptionModel _Prescription;

        public PrescriptionModel Prescription
        {
            get { return _Prescription; }
            set { Set(ref _Prescription, value); }
        }

        private ObservableCollection<PrescriptionItemModel> _SelectPrescriptionItems;

        public ObservableCollection<PrescriptionItemModel> SelectPrescriptionItems
        {
            get
            {
                return _SelectPrescriptionItems
                    ?? (_SelectPrescriptionItems = new ObservableCollection<PrescriptionItemModel>());
            }

            set { Set(ref _SelectPrescriptionItems, value); }
        }

        private List<string> _PrinterLists;

        public List<string> PrinterLists
        {
            get { return _PrinterLists; }
            set { Set(ref _PrinterLists, value); }
        }

        private string _SelectPrinter;

        public string SelectPrinter
        {
            get { return _SelectPrinter; }
            set { Set(ref _SelectPrinter, value); }
        }

        private List<LookupReferenceValueModel> _language;
        public List<LookupReferenceValueModel> language
        {
            get { return _language; }
            set { Set(ref _language, value); }
        }

        private LookupReferenceValueModel _Selectlanguage;

        public LookupReferenceValueModel Selectlanguage
        {
            get { return _Selectlanguage; }
            set { Set(ref _Selectlanguage, value); }
        }
        #endregion

        #region Command

        private RelayCommand _PrintCommand;

        public RelayCommand PrintCommand
        {
            get
            {
                return _PrintCommand
                    ?? (_PrintCommand = new RelayCommand(Print));
            }
        }

        private RelayCommand<DevExpress.Xpf.Grid.RowEventArgs> _RowUpdatedCommand;

        /// <summary>
        /// Gets the RowUpdatedCommand.
        /// </summary>
        public RelayCommand<DevExpress.Xpf.Grid.RowEventArgs> RowUpdatedCommand
        {
            get
            {
                return _RowUpdatedCommand
                    ?? (_RowUpdatedCommand = new RelayCommand<DevExpress.Xpf.Grid.RowEventArgs>(RowUpdated));
            }
        }

        #endregion

        #region Method

        public PrintDrugStickerViewModel()
        {
            PrinterLists = new List<string>();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                PrinterLists.Add(printer);
            }

            SelectPrinter = PrinterLists.FirstOrDefault(p => p.Contains("sticker"));
            language = DataService.Technical.GetReferenceValueMany("SPOKL");
            Selectlanguage = language.FirstOrDefault(p => p.ValueCode == "TH");
        }

        private void RowUpdated(DevExpress.Xpf.Grid.RowEventArgs e)
        {
            try
            {
                if (e.Row is PrescriptionItemModel)
                {

                    PrescriptionItemModel rowData = e.Row as PrescriptionItemModel;
                    DataService.Pharmacy.UpdatePrescriptionLabelSticker(rowData.PrescriptionItemUID, rowData.LocalInstructionText, AppUtil.Current.UserID);
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        public void AssignModel(PrescriptionModel prescription)
        {
            var item = DataService.Pharmacy.GetPrescriptionItemByPrescriptionUID(prescription.PrescriptionUID);
            prescription.PrescriptionItems = new ObservableCollection<PrescriptionItemModel>(item);
            Prescription = prescription;
        }
        void Print()
        {
            if (string.IsNullOrEmpty(SelectPrinter))
            {
                WarningDialog("กรุณาเลือกปริ้นเตอร์");
                return;
            }
            foreach (var item in SelectPrescriptionItems)
            {
                DrugSticker rpt = new DrugSticker();
                ReportPrintTool printTool = new ReportPrintTool(rpt);

                rpt.Parameters["OrganisationUID"].Value = item.OwnerOrganisationUID;
                rpt.Parameters["PrescriptionItemUID"].Value = item.PrescriptionItemUID;
                rpt.Parameters["ExpiryDate"].Value = item.ExpiryDate;
                rpt.Parameters["LangType"].Value = Selectlanguage.ValueCode;
                rpt.RequestParameters = false;
                rpt.ShowPrintMarginsWarning = false;
                printTool.Print(SelectPrinter);
            }
        }

        #endregion
    }
}
