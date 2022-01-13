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

        #region PatientSearch

        #endregion

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

    

        private ObservableCollection<EcountImportModel> _EcountDataList;
        public ObservableCollection<EcountImportModel> EcountDataList
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


 

        #endregion

        #region Method


 

     
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
                                OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [" + FileName + "] Where ([NO] <> '' OR [NO] IS NOT NULL)", conn);
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
                }

                }
            catch (Exception er)
            {

                System.Windows.Forms.MessageBox.Show(er.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        #endregion

    }
}
