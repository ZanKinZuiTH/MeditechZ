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
                //Set(ref _SelectHealthOrganisation, value);
                //if (SelectHealthOrganisation == null)
                //{
                //    EnableSearchItem = false;
                //}
                //else
                //{
                //    EnableSearchItem = true;
                //}
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

        #region CurrentImportedData
        private EcountImportModel _currentImportedData;
        public EcountImportModel CurrentImportedData
        {
            get
            {
                return _currentImportedData;
            }
            set
            {
                if (_currentImportedData != value)
                {
                    _currentImportedData = value;
                    Set(ref _currentImportedData, value);
                }
            }
        }
        #endregion

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


        private RelayCommand _SavedataCommand;

        public RelayCommand SavedataCommand
        {
            get
            {
                return _SavedataCommand
                    ?? (_SavedataCommand = new RelayCommand(SaveData));
            }
        }




        #endregion

        #region Method

        public EcountImportFileViewModel()
        {
            Organisations = GetHealthOrganisationRoleMedical();
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);

           // PayorDetails = DataService.MasterData.GetPayorDetail();
            //Careproviders = DataService.UserManage.GetCareproviderAll();
           // SelectCareprovider = Careproviders.FirstOrDefault(p => p.CareproviderUID == AppUtil.Current.UserID);

            HealthOrganisations = Organisations;
            SelectHealthOrganisation = HealthOrganisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);

        }






        private void SaveData()
        {
        
        
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
                    EcountDataList = new ObservableCollection<EcountImportModel>();
                    foreach (DataRow drow in ImportData.Rows)
                    {
                        CurrentImportedData = new EcountImportModel();
                        CurrentImportedData.ActiveDate = drow["ActiveDate"].ToString().Trim();
                        CurrentImportedData.NO = int.Parse(drow["NO"].ToString().Trim());
                        CurrentImportedData.PIC = drow["PIC"].ToString().Trim();
                        CurrentImportedData.DepartmentGoodsStock = drow["DepartmentGoodsStock"].ToString().Trim();
                        CurrentImportedData.DepartmentPickStock = drow["DepartmentPickStock"].ToString().Trim();
                        CurrentImportedData.ItemCode = drow["ItemCode"].ToString().Trim();
                        CurrentImportedData.ItemName = drow["ItemName"].ToString().Trim();

                        EcountDataList.Add(CurrentImportedData);

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


        #endregion

    }
}
