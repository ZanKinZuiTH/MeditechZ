using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Reports.Operating.Pharmacy;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class DispenseDrugViewModel : MediTechViewModelBase
    {
        #region Properties

        private PatientVisitModel _PatientVisit;

        public PatientVisitModel PatientVisit
        {
            get { return _PatientVisit; }
            set { Set(ref _PatientVisit, value); }
        }

        private PrescriptionModel _Prescription;

        public PrescriptionModel Prescription
        {
            get { return _Prescription; }
            set { _Prescription = value; }
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

        private bool _IsPrintSticker = true;

        public bool IsPrintSticker
        {
            get { return _IsPrintSticker; }
            set { Set(ref _IsPrintSticker, value); }
        }

        private ObservableCollection<PrescriptionItemModel> _PrescriptionItems;

        public ObservableCollection<PrescriptionItemModel> PrescriptionItems
        {
            get { return _PrescriptionItems; }
            set { Set(ref _PrescriptionItems, value); }
        }


        #endregion

        #region Command

        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save)); }
        }

        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
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
        public DispenseDrugViewModel()
        {

            PrinterLists = new List<string>();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                PrinterLists.Add(printer);
            }

            SelectPrinter = PrinterLists.FirstOrDefault(p => p.Contains("sticker"));
        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            PrescriptionItems = new ObservableCollection<PrescriptionItemModel>(DataService.Pharmacy.GetPrescriptionItemByPrescriptionUID(Prescription.PrescriptionUID));
            foreach (var item in PrescriptionItems)
            {
                var storeUsed = DataService.Pharmacy.GetDrugStoreDispense(item.ItemMasterUID ?? 0, item.Quantity ?? 0, item.IMUOMUID ?? 0, item.StoreUID ?? 0);
                foreach (var stock in storeUsed)
                {
                    if (stock.Quantity > stock.BalQty)
                    {
                        stock.IsWithoutStock = true;
                    }

                    if (stock.ExpiryDate?.Date <= DateTime.Now.Date)
                    {
                        stock.IsExpired = true;
                    }
                }
                item.StockUID = storeUsed.FirstOrDefault() != null ? storeUsed.FirstOrDefault().StockUID : null;
                item.StoreStockItem = new ObservableCollection<PatientOrderDetailModel>(storeUsed);
                item.BalQty = storeUsed.Sum(p => p.BalQty);
                item.ExpiryDate = storeUsed.FirstOrDefault() != null ? storeUsed.FirstOrDefault().ExpiryDate : null;
                item.IsWithoutStock = storeUsed.FirstOrDefault(p => p.IsWithoutStock == true) != null ? true : false;
            }
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

        public void AssingModel(PrescriptionModel prescriptionModel,PatientVisitModel visitModel)
        {
            PatientVisit = visitModel;
            Prescription = prescriptionModel;

        }

        public void Save()
        {
            try
            {
                if (IsPrintSticker == true)
                {
                    if (string.IsNullOrEmpty(SelectPrinter))
                    {
                        WarningDialog("กรุณาเลือกปริ้นเตอร์");
                        return;
                    }
                }

                //DataService.Pharmacy.DispensePrescription(Prescription,AppUtil.Current.UserID);
                foreach (var item in PrescriptionItems)
                {
                    if (item.BalQty > item.Quantity && item.ORDSTUID == 2847)
                    {
                        DataService.Pharmacy.DispensePrescriptionItem(item, AppUtil.Current.UserID);


                        if (IsPrintSticker == true)
                        {
                            DrugSticker rpt = new DrugSticker();
                            ReportPrintTool printTool = new ReportPrintTool(rpt);

                            rpt.Parameters["OrganisationUID"].Value = item.OwnerOrganisationUID;
                            rpt.Parameters["PrescriptionItemUID"].Value = item.PrescriptionItemUID;
                            rpt.Parameters["ExpiryDate"].Value = item.ExpiryDate;
                            rpt.RequestParameters = false;
                            rpt.ShowPrintMarginsWarning = false;
                            printTool.Print(SelectPrinter);
                        }
                    }
                }



                Prescription prescription = new Prescription();
                ChangeViewPermission(prescription);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        public void Cancel()
        {
            Prescription prescription = new Prescription();
            ChangeViewPermission(prescription);
        }
        #endregion
    }
}
