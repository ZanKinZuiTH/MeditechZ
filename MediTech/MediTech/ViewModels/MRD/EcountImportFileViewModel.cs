using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Models;
using MediTech.Reports.Operating.Patient;
using MediTech.Views;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace MediTech.ViewModels
{
 public class EcountImportFileViewModel : MediTechViewModelBase
    {
        #region Properties


        private List<HealthOrganisationModel> _HealthOrganisations;

        public List<HealthOrganisationModel> HealthOrganisations
        {
            get { return _HealthOrganisations; }
            set { Set(ref _HealthOrganisations, value); }
        }

        private HealthOrganisationModel _SelectHealthOrganisation;

        public HealthOrganisationModel SelectHealthOrganisation
        {
            get { return _SelectHealthOrganisation; }
            set
            {
                Set(ref _SelectHealthOrganisation, value);
                if (SelectHealthOrganisation == null)
                {
                    //EnableSearchItem = false;
                }
                else
                {
                    //EnableSearchItem = true;
                }
            }
        }



        private ObservableCollection<StockAdjustmentModel> _AdjustStock;

        public ObservableCollection<StockAdjustmentModel> AdjustStock
        {
            get { return _AdjustStock; }
            set { Set(ref _AdjustStock, value); }
        }

        private StockAdjustmentModel _SelectAdjustStock;

        public StockAdjustmentModel SelectAdjustStock
        {
            get { return _SelectAdjustStock; }
            set { Set(ref _SelectAdjustStock, value); }
        }


        private StoreModel _SelectStore;

        public StoreModel SelectStore
        {
            get { return _SelectStore; }
            set { Set(ref _SelectStore, value); }
        }



        private StockModel _SelectCurrentStock;

        public StockModel SelectCurrentStock
        {
            get { return _SelectCurrentStock; }
            set
            {
                Set(ref _SelectCurrentStock, value);
                if (SelectCurrentStock != null)
                {
                   
                }
            }
        }


        private List<HealthOrganisationModel> _Organisations;

        public List<HealthOrganisationModel> Organisations
        {
            get { return _Organisations; }
            set { Set(ref _Organisations, value); }


        }

        private List<StoreModel> _Stores;

        public List<StoreModel> Stores
        {
            get { return _Stores; }
            set { Set(ref _Stores, value); }
        }


        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set
            {
                Set(ref _SelectOrganisation, value);
                if (_SelectOrganisation != null)
                {
                    Stores = DataService.Inventory.GetStoreByOrganisationUID(SelectOrganisation.HealthOrganisationUID);
                }
            }
        }


        private string _FileLocation;

        public string FileLocation
        {
            get { return _FileLocation; }
            set { Set(ref _FileLocation, value); }
        }

        private int _TotalRecord;

        public int TotalRecord
        {
            get { return _TotalRecord; }
            set { Set(ref _TotalRecord, value); }
        }

        private List<StockModel> _CurrentStock;

        public List<StockModel> CurrentStock
        {
            get { return _CurrentStock; }
            set { Set(ref _CurrentStock, value); }
        }


        private ObservableCollection<StockAdjustmentModel> _EcountDataList;
        public ObservableCollection<StockAdjustmentModel> EcountDataList
        {
            get
            {
                return _EcountDataList;
            }
            set
            {
                Set(ref _EcountDataList, value);
            }
        }

        private EcountImportModel _SelectEcountData;
        public EcountImportModel SelectEcountData
        {
            get
            {
                return _SelectEcountData;
            }
            set
            {
                Set(ref _SelectEcountData, value);
            }
        }


        #endregion

        #region Command

        private RelayCommand _ChooseCommand;

        public RelayCommand ChooseCommand
        {
            get
            {
                return _ChooseCommand
                    ?? (_ChooseCommand = new RelayCommand(ChooseFile));
            }
        }


        private RelayCommand _ImportCommand;

        public RelayCommand ImportCommand
        {
            get
            {
                return _ImportCommand
                    ?? (_ImportCommand = new RelayCommand(ImportFile));
            }
        }


        private RelayCommand _SavedataCommand;

        public RelayCommand SavedataCommand
        {
            get
            {
                return _SavedataCommand
                    ?? (_SavedataCommand = new RelayCommand(Save));
            }
        }




        #endregion

        #region Method

        public EcountImportFileViewModel()
        {
            Organisations = GetHealthOrganisationRoleMedical();
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);

            HealthOrganisations = Organisations;
            SelectHealthOrganisation = HealthOrganisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);

        }


      




        private void ChooseFile()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Excel 2007 (*.xlsx)|*.xlsx|Excel 1997 - 2003 (*.xls)|*.xls"; ;
            openDialog.InitialDirectory = @"c:\";
            openDialog.ShowDialog();
            if (openDialog.FileName.Trim() != "")
            {
                try
                {
                    FileLocation = openDialog.FileName.Trim();
                }
                catch (Exception ex)
                {
                    ErrorDialog(ex.Message);
                }
            }
        }

        private void ImportFile()
        {
            if (SelectStore == null)
            {
                WarningDialog("กรุณาเลือก STOCK");
                return;
            }
     

            OleDbConnection conn;
            DataSet objDataset1;
            OleDbCommand cmd;
            DataTable dt;
            DataTable ImportData = new DataTable();
            string connectionString = string.Empty;
            int pgBarCounter = 0;
            TotalRecord = 0;
            //DateTime birthdttm;
            EcountImportFile view = (EcountImportFile)this.View;
            try
            {
                if (FileLocation.Trim() != string.Empty)
                {
                    //EcountDataList = new ObservableCollection<EcountImportModel>();
                    //SelectEcountData = null;
                    if (FileLocation.Trim().EndsWith(".xls"))
                    {
                        connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + FileLocation.Trim() +
                            "; Extended Properties=\"Excel 8.0; HDR=Yes; IMEX=1\"";
                    }
                    else
                    {
                        connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + FileLocation.Trim() +
                            "; Extended Properties=\"Excel 12.0 Xml; HDR=YES; IMEX=1\"";
                    }
                    using (conn = new OleDbConnection(connectionString))
                    {
                        conn.Open();
                        objDataset1 = new DataSet();
                        dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int row = 0; row < dt.Rows.Count;)
                            {
                                string FileName = Convert.ToString(dt.Rows[row]["Table_Name"]);
                                cmd = conn.CreateCommand();
                                OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [" + FileName + "] Where ([ItemCode] <> '' OR [NO] IS NOT NULL)", conn);
                                OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();
                                objAdapter1.SelectCommand = objCmdSelect;
                                objAdapter1.Fill(objDataset1);
                                //Break after reading the first sheet
                                break;
                            }
                            ImportData = objDataset1.Tables[0];
                            conn.Close();
                        }
                    }

                    int upperlimit = ImportData.Rows.Count;
                    view.SetProgressBarLimits(0, upperlimit);
                    view.SetProgressBarValue(upperlimit);
                    OnUpdateEvent();
                    AdjustStock = new ObservableCollection<StockAdjustmentModel>();
                    foreach (DataRow drow in ImportData.Rows)
                    {
                        // List<StockModel> CurrentStock = DataService.Inventory.GetStockRemainByItemMasterUID(int.Parse(drow["ItemMasterUID"].ToString().Trim()),SelectStore.StoreUID);

                        List<StockModel> CurrentStock = DataService.Inventory.GetStoreEcounByItemMaster(int.Parse(drow["ItemMasterUID"].ToString().Trim()), SelectStore.StoreUID);

                        StockAdjustmentModel adjustStock = new StockAdjustmentModel();
                        adjustStock.ItemMasterUID = int.Parse(drow["ItemMasterUID"].ToString().Trim());
                        adjustStock.ItemCode = drow["ItemCode"].ToString().Trim();
                        adjustStock.ItemName = drow["ItemName"].ToString().Trim();
                        adjustStock.OwnerOrganisationUID = SelectOrganisation.HealthOrganisationUID;
                        adjustStock.StoreUID = SelectStore.StoreUID;
                        //adjustStock.StockUID = CurrentStock.StockUID;
                        //adjustStock.ItemCost = CurrentStock.ItemCost;
                        //adjustStock.OwnerOrganisationUID = CurrentStock.OrganisationUID;
                        //adjustStock.StoreUID = CurrentStock.StoreUID;
                        //adjustStock.StoreName = CurrentStock.StoreName;
                        adjustStock.ActualQuantity = GetSumQTY(CurrentStock);
                        adjustStock.QuantityAdjusted = int.Parse(drow["QuantityAdjusted"].ToString().Trim());
                        adjustStock.AdjustedQuantity = adjustStock.ActualQuantity + int.Parse(drow["QuantityAdjusted"].ToString().Trim());
                        //adjustStock.ActualUOM = CurrentStock.IMUOMUID;
                        //adjustStock.AdjustedUOM = CurrentStock.IMUOMUID;
                        //adjustStock.AdjustedUnit = CurrentStock.Unit;
                         adjustStock.ExpiryDate = DateTime.Parse(drow["ExpiryDate"].ToString().Trim());
                        //adjustStock.BatchID = CurrentStock.Where .BatchID;
                        //adjustStock.ItemCost = ItemCost;
                        //adjustStock.NewBatchID = CurrentStock.Where(p=>p.)
                     //  adjustStock.Comments = Comments;

                        AdjustStock.Add(adjustStock);
                        pgBarCounter = pgBarCounter + 1;
                        TotalRecord = pgBarCounter;
                        view.SetProgressBarValue(pgBarCounter);
                    }

                               }
            }

            catch (Exception er)
            {

                System.Windows.Forms.MessageBox.Show(er.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private double GetSumQTY(List<StockModel> curryQTY)
        {
            try
            {
                double sumQTY = 0;
                foreach (var item in curryQTY)
                {
                    sumQTY = sumQTY + item.Quantity; 
                }

                return sumQTY;
            }
            catch (Exception)
            {

                throw;
            }
        
        
        }

        private void Save()
        {
            try
            {
                if (AdjustStock == null)
                {
                    WarningDialog("กรุณาทำรายการอย่างน้อย 1 รายการ");
                    return;
                }

                foreach (var item in AdjustStock)
                {
                    DataService.Inventory.AdjustStock(item, AppUtil.Current.UserID);
                }

                SaveSuccessDialog();
               // AdjustStock = null;
            }
            catch (Exception er)
            {
                ErrorDialog(er.Message);
            }

        }


        #endregion

    }
}
