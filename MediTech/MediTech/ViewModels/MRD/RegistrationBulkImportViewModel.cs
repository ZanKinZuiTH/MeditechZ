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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace MediTech.ViewModels
{
    public class RegistrationBulkImportViewModel : MediTechViewModelBase
    {

        #region Properties

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

        private bool _SelectAll;

        public bool SelectAll
        {
            get { return _SelectAll; }
            set
            {
                if (_SelectAll != value)
                {
                    _SelectAll = value;
                    if (SelectAll == true)
                    {
                        if (PatientDataList != null && PatientDataList.Count > 0)
                        {
                            foreach (PatientRegistrationBulkData item in PatientDataList)
                            {
                                item.Register = true;
                            }
                        }
                        if (ImportedDataList != null && ImportedDataList.Count > 0)
                        {
                            foreach (PatientRegistrationBulkData item in ImportedDataList)
                            {
                                item.Register = true;
                            }
                        }
                    }
                    else if (SelectAll == false)
                    {
                        if (PatientDataList != null && PatientDataList.Count > 0)
                        {
                            foreach (PatientRegistrationBulkData item in PatientDataList)
                            {
                                item.Register = false;
                            }
                        }
                        if (ImportedDataList != null && ImportedDataList.Count > 0)
                        {
                            foreach (PatientRegistrationBulkData item in ImportedDataList)
                            {
                                item.Register = false;
                            }
                        }
                    }
                    RegistrationBulkImport view = (RegistrationBulkImport)this.View;
                    view.PatientGrid.RefreshData();
                }
                Set(ref _SelectAll, value);
            }
        }

        private bool _NonCheckHNDuplicate;

        public bool NonCheckHNDuplicate
        {
            get { return _NonCheckHNDuplicate; }
            set { Set(ref _NonCheckHNDuplicate, value); }
        }


        private int? _RegisteredCount;

        public int? RegisteredCount
        {
            get { return _RegisteredCount; }
            set { Set(ref _RegisteredCount, value); }
        }

        private int? _UpdateCount;

        public int? UpdateCount
        {
            get { return _UpdateCount; }
            set { Set(ref _UpdateCount, value); }
        }



        private int? _CreateVisitCount;

        public int? CreateVisitCount
        {
            get { return _CreateVisitCount; }
            set { Set(ref _CreateVisitCount, value); }
        }

        #region ImportedDataList
        private ObservableCollection<PatientRegistrationBulkData> _importedDataList;
        public ObservableCollection<PatientRegistrationBulkData> ImportedDataList
        {
            get
            {
                return _importedDataList;
            }
            set
            {
                Set(ref _importedDataList, value);
            }
        }
        #endregion
        //
        #region PagedDatalist
        private ObservableCollection<PatientRegistrationBulkData> _PatientDataList;
        public ObservableCollection<PatientRegistrationBulkData> PatientDataList
        {
            get
            {
                return _PatientDataList;
            }
            set
            {
                Set(ref _PatientDataList, value);
            }
        }


        private PatientRegistrationBulkData _SelectPatientDataList;
        public PatientRegistrationBulkData SelectPatientDataList
        {
            get
            {
                return _SelectPatientDataList;
            }
            set
            {
                Set(ref _SelectPatientDataList, value);
            }
        }
        #endregion

        #region CurrentImportedData
        private PatientRegistrationBulkData _currentImportedData;
        public PatientRegistrationBulkData CurrentImportedData
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

        private int _StickerQuantity = 1;

        public int StickerQuantity
        {
            get { return _StickerQuantity; }
            set { Set(ref _StickerQuantity, value); }
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
                    ?? (_ImportCommand = new RelayCommand(Import));
            }
        }


        private RelayCommand _ExportCommand;

        public RelayCommand ExportCommand
        {
            get
            {
                return _ExportCommand
                    ?? (_ExportCommand = new RelayCommand(Export));
            }
        }

        private RelayCommand _RegisterCommand;

        public RelayCommand RegisterCommand
        {
            get
            {
                return _RegisterCommand
                    ?? (_RegisterCommand = new RelayCommand(Register));
            }
        }



        private RelayCommand _CreateVisitCommand;

        public RelayCommand CreateVisitCommand
        {
            get
            {
                return _CreateVisitCommand
                    ?? (_CreateVisitCommand = new RelayCommand(CreateVisit));
            }
        }


        private RelayCommand _PrintStickerCommand;
        public RelayCommand PrintStickerCommand
        {
            get
            {
                return _PrintStickerCommand
                    ?? (_PrintStickerCommand = new RelayCommand(PrintSticker));
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

        private RelayCommand _AppointmentCommand;
        public RelayCommand AppointmentCommand
        {
            get
            {
                return _AppointmentCommand
                    ?? (_AppointmentCommand = new RelayCommand(PrintAppointment));
            }
        }

        #endregion

        #region Method
        public RegistrationBulkImportViewModel()
        {
            MobileStickerSource = new List<LookupReferenceValueModel>();
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 2, Display = "ใบนำทาง", DisplayOrder = 1 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "พบแพทย์", DisplayOrder = 2 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 3, Display = "เจาะเลือด", DisplayOrder = 3 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "ปัสสาวะ", DisplayOrder = 4 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "ปัสสาวะ(Toxi)", DisplayOrder = 5 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "อุจจาระ", DisplayOrder = 6 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "สายตาทั่วไป", DisplayOrder = 7 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "สายตาอาชีวะ", DisplayOrder = 8 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "ตรวจการได้ยิน", DisplayOrder = 9 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "ตรววจเป่าปอด", DisplayOrder = 10 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "X-ray", DisplayOrder = 11 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "EKG", DisplayOrder = 12 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "อัลตราซาวด์ช่องท้อง(Whole)", DisplayOrder = 13 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "อัลตราซาวด์ช่องท้อง(Upper)", DisplayOrder = 14 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "อัลตราซาวด์ช่องท้อง(Lower)", DisplayOrder = 15 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "แมมโมแกรม(MMG)", DisplayOrder = 16 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "ตรวจกล้ามเนื้อหลัง", DisplayOrder = 17 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "ตรวจกล้ามเนื้อมือ", DisplayOrder = 18 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "ตรวจกล้ามเนื้อขา", DisplayOrder = 19 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "ตรวจกล้ามเนื้อหลัง+ขา", DisplayOrder = 20 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "ตรวจกล้ามเนื้อ3ส่วน", DisplayOrder = 21 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "PAP Smear", DisplayOrder = 22 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "Covid (Rapid test Antigen)", DisplayOrder = 23 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "Covid (Rapid test Antibody)", DisplayOrder = 24 });
            MobileStickerSource.Add(new LookupReferenceValueModel { Key = 1, Display = "Covid (RT-PCR)", DisplayOrder = 25 });


            SelectMobileStickers = new List<object>() { MobileStickerSource[0],MobileStickerSource[1], MobileStickerSource[2], MobileStickerSource[3]
                , MobileStickerSource[4], MobileStickerSource[5], MobileStickerSource[6],MobileStickerSource[7],MobileStickerSource[8],MobileStickerSource[9]
                , MobileStickerSource[10],MobileStickerSource[11],MobileStickerSource[12],MobileStickerSource[13],MobileStickerSource[14],MobileStickerSource[15]
                ,MobileStickerSource[16],MobileStickerSource[17],MobileStickerSource[18],MobileStickerSource[19],MobileStickerSource[20],MobileStickerSource[21],MobileStickerSource[22],MobileStickerSource[23]};
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

        private void Import()
        {
            try
            {
                OleDbConnection conn;
                DataSet objDataset1;
                OleDbCommand cmd;
                DataTable dt;
                DataTable ImportData = new DataTable();
                string connectionString = string.Empty;
                bool IsRecordDoubleinExcel = false;
                SelectAll = false;
                bool IsIncompleteRecordPresent = false;
                bool isFutureBirthDatePresent = false;
                PatientInformationModel patient;
                List<PatientRegistrationBulkData> patientHNDupicate = null;
                Double doublebirthdttm;
                int pgBarCounter = 0;
                TotalRecord = 0;
                DateTime birthdttm;
                RegistrationBulkImport view = (RegistrationBulkImport)this.View;
                if (!string.IsNullOrEmpty(FileLocation))
                {

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

                    int upperlimit = 0;
                    foreach (DataRow drow in ImportData.Rows)
                    {
                        if (drow["FirstName"].ToString().Trim() != "")
                        {
                            upperlimit += 1;
                        }
                    }

                    view.SetProgressBarLimits(0, upperlimit);

                    ImportedDataList = new ObservableCollection<PatientRegistrationBulkData>();
                    var titleDataSource = DataService.Technical.GetReferenceValueList("TITLE");
                    foreach (DataRow drow in ImportData.Rows)
                    {
                        CurrentImportedData = new PatientRegistrationBulkData();
                        CurrentImportedData.No = int.Parse(drow["No"].ToString().Trim());
                        if (ImportData.Columns.Contains("HN"))
                        {
                            CurrentImportedData.BN = drow["HN"].ToString();
                        }

                        CurrentImportedData.PatientOtherID = drow["Other ID"].ToString().Trim();
                        CurrentImportedData.EmployeeID = drow["EmployeeID"].ToString().Trim();
                        CurrentImportedData.PreName = drow["PreName"].ToString().Trim();
                        CurrentImportedData.FirstName = drow["FirstName"].ToString().Trim();
                        CurrentImportedData.LastName = drow["LastName"].ToString().Trim();
                        CurrentImportedData.DateOfBirth = drow["Birth Date"].ToString().Trim();
                        CurrentImportedData.Age = drow["Age"].ToString().Trim();
                        CurrentImportedData.Gender = drow["Sex"].ToString().Trim();
                        CurrentImportedData.Company = drow["Company"].ToString().Trim();
                        CurrentImportedData.Program = drow["Program"].ToString().Trim();

                        DateTime checkupDttm;
                        if (DateTime.TryParse(drow["CheckupDttm"].ToString().Trim(), out checkupDttm))
                            CurrentImportedData.CheckupDttm = checkupDttm;

                        if (ImportData.Columns.Contains("Group"))
                        {
                            CurrentImportedData.Group = drow["Group"].ToString();
                        }

                        CurrentImportedData.MobilePhone = drow["MobilePhone"].ToString().Trim();
                        CurrentImportedData.IsDuplicate = "N";
                        CurrentImportedData.IsJustRegistered = false;
                        CurrentImportedData.IsInComplete = "N";

                        CurrentImportedData.Gender = drow["Sex"].ToString();


                        if (drow["Sex"].ToString().Trim().ToLower() == "f" || drow["Sex"].ToString().Trim().ToLower() == "female" || drow["Sex"].ToString().Trim().ToLower() == "หญิง")
                        {
                            CurrentImportedData.SEXXXUID = 2; //Female
                        }
                        else if (drow["Sex"].ToString().Trim().ToLower() == "m" || drow["Sex"].ToString().Trim().ToLower() == "male" || drow["Sex"].ToString().Trim().ToLower() == "ชาย")
                        {
                            CurrentImportedData.SEXXXUID = 1; //Male
                        }
                        else if (drow["Sex"].ToString().Trim().ToLower() == "u")
                        {
                            CurrentImportedData.SEXXXUID = 3; //Unknown
                        }
                        //else
                        //{
                        //    CurrentImportedData.SEXXXUID = 1; //Male
                        //}

                        string preName = drow["PreName"].ToString().Trim().ToUpper();
                        switch (preName)
                        {
                            case "เด็กชาย":
                                CurrentImportedData.TITLEUID = titleDataSource.FirstOrDefault(p => p.ValueCode == "00008TH").Key;
                                break;
                            case "เด็กหญิง":
                                CurrentImportedData.TITLEUID = titleDataSource.FirstOrDefault(p => p.ValueCode == "00010TH").Key;
                                break;
                            case "นาง":
                                CurrentImportedData.TITLEUID = titleDataSource.FirstOrDefault(p => p.ValueCode == "00116TH").Key;
                                break;
                            case "นางสาว":
                                CurrentImportedData.TITLEUID = titleDataSource.FirstOrDefault(p => p.ValueCode == "00117TH").Key;
                                break;
                            case "น.ส.":
                                CurrentImportedData.TITLEUID = titleDataSource.FirstOrDefault(p => p.ValueCode == "00117TH").Key;
                                break;
                            case "นาย":
                                CurrentImportedData.TITLEUID = titleDataSource.FirstOrDefault(p => p.ValueCode == "00118TH").Key;
                                break;
                            case "พระภิกษุ":
                                CurrentImportedData.TITLEUID = titleDataSource.FirstOrDefault(p => p.ValueCode == "00245TH").Key;
                                break;
                            case "MR.":
                                CurrentImportedData.TITLEUID = titleDataSource.FirstOrDefault(p => p.ValueCode == "00118EN").Key;
                                break;
                            case "MS.":
                                CurrentImportedData.TITLEUID = titleDataSource.FirstOrDefault(p => p.ValueCode == "01226EN").Key;
                                break;

                            default:
                                var refTitle = titleDataSource.FirstOrDefault(p => p.Display == preName);
                                if (refTitle != null)
                                {
                                    CurrentImportedData.TITLEUID = refTitle.Key;
                                }
                                break;
                        }

                        bool valid = DateTime.TryParse(CurrentImportedData.DateOfBirth, out birthdttm);

                        if (!valid)
                        {
                            DateTime.TryParse(CurrentImportedData.DateOfBirth, new CultureInfo("th-TH"), System.Globalization.DateTimeStyles.None, out birthdttm);
                        }


                        if (!string.IsNullOrEmpty(CurrentImportedData.DateOfBirth))
                        {

                            if (birthdttm == DateTime.MinValue)
                            {
                                Double.TryParse(CurrentImportedData.DateOfBirth, out doublebirthdttm);
                                birthdttm = DateTime.FromOADate(doublebirthdttm);
                            }


                            if (int.Parse(birthdttm.ToString("yyyy")) > 2400)
                            {
                                birthdttm = birthdttm.AddYears(-543);
                            }

                            DateTime Now = DateTime.Now;
                            int age = Now.Year - birthdttm.Year;
                            if ((birthdttm.Month > Now.Month) || (birthdttm.Month == Now.Month && birthdttm.Day > Now.Day))
                            {
                                --age;
                            }
                            CurrentImportedData.Age = age.ToString();

                            if (birthdttm > System.DateTime.Now)
                            {
                                isFutureBirthDatePresent = true;
                            }

                            CurrentImportedData.BirthDttm = birthdttm;
                        }
                        else if (!string.IsNullOrEmpty(CurrentImportedData.Age) && (CheckValidate.IsNumber(CurrentImportedData.Age)))
                        {

                            CurrentImportedData.BirthDttm = DateTime.Parse("01/01/" + Convert.ToString(int.Parse(DateTime.Now.ToString("yyyy")) - int.Parse(CurrentImportedData.Age)));
                            CurrentImportedData.DOBComputed = true;
                            CurrentImportedData.DateOfBirth = CurrentImportedData.BirthDttm?.ToString("dd/MM/yyyy");
                        }

                        CurrentImportedData.IDCard = drow["IDCard"].ToString().Replace("-", "");
                        CurrentImportedData.IDPassport = drow["IDPassport"].ToString().Replace("-", "");

                        if (drow.Table.Columns.Contains("Company"))
                            CurrentImportedData.Company = drow["Company"].ToString();

                        if (drow.Table.Columns.Contains("Position"))
                            CurrentImportedData.Position = drow["Position"].ToString();

                        if (drow.Table.Columns.Contains("Department"))
                            CurrentImportedData.Department = drow["Department"].ToString();


                        if (!string.IsNullOrEmpty(drow["HN"].ToString()))
                        {
                            patient = DataService.PatientIdentity.GetPatientByHN(drow["HN"].ToString().Trim());
                            if (patient != null)
                            {
                                if (!NonCheckHNDuplicate)
                                {
                                    if (drow["FirstName"].ToString().Trim() != patient.FirstName?.ToString() && drow["LastName"].ToString().Trim() != patient.LastName?.ToString())
                                    {
                                        if (patientHNDupicate == null)
                                        {
                                            patientHNDupicate = new List<PatientRegistrationBulkData>();
                                        }
                                        patientHNDupicate.Add(CurrentImportedData);
                                    }
                                    //((RegistrationBulkImport)View).ShowMessageBox("HN: " + drow["HN"].ToString().Trim() + " ซ้ำกับในระบบ โปรดตรวจสอบ", "", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(drow["IDCard"].ToString()))
                        {
                            patient = DataService.PatientIdentity.GetPatientByIDCard(drow["IDCard"].ToString().Replace("-", "").Trim());
                            if (patient != null)
                            {
                                if (drow["FirstName"].ToString().Trim() != patient.FirstName?.ToString() && drow["LastName"].ToString().Trim() != patient.LastName?.ToString())
                                {
                                    CurrentImportedData.IsNationalIDDuplicate = "Y";
                                    CurrentImportedData.Register = false;
                                }
                            }

                        }
                        else
                        {
                            patient = DataService.PatientIdentity.CheckDupicatePatient(CurrentImportedData.FirstName
                                , CurrentImportedData.LastName
                                , CurrentImportedData.BirthDttm != null ? CurrentImportedData.BirthDttm.Value : (DateTime?)null
                                , CurrentImportedData.SEXXXUID.Value);
                        }

                        if (patient != null)
                        {
                            //SimilarPatientsExist
                            CurrentImportedData.AlreadyExists = "Y";
                            CurrentImportedData.Register = false;
                            CurrentImportedData.BN = patient.PatientID;

                            CurrentImportedData.PatientUID = patient.PatientUID;

                            if (string.IsNullOrEmpty(CurrentImportedData.DateOfBirth) && patient.BirthDttm != null)
                            {
                                CurrentImportedData.BirthDttm = patient.BirthDttm;
                                CurrentImportedData.DateOfBirth = patient.BirthDttm?.ToString("dd/MM/yyyy");
                            }
                            if (CurrentImportedData.SEXXXUID == null && patient.SEXXXUID != null)
                            {
                                CurrentImportedData.SEXXXUID = patient.SEXXXUID;
                            }

                            if (CurrentImportedData.TITLEUID == null && patient.TITLEUID != null)
                            {
                                CurrentImportedData.TITLEUID = patient.TITLEUID;
                            }
                            PatientVisitModel visitData = DataService.PatientIdentity.GetLatestPatientVisit(patient.PatientUID);

                            if (visitData != null)
                            {
                                if (visitData.StartDttm.Value.Date == System.DateTime.Today)
                                {
                                    CurrentImportedData.HasVisitToday = true;
                                }
                            }

                        }
                        else
                        {
                            CurrentImportedData.AlreadyExists = "N";
                            CurrentImportedData.Register = true;
                        }




                        var duplicateList = (from p in ImportedDataList
                                             where (p.FirstName == CurrentImportedData.FirstName
                                                 && p.LastName == CurrentImportedData.LastName
                                                 && p.DateOfBirth == CurrentImportedData.DateOfBirth
                                                 && p.Gender == CurrentImportedData.Gender)
                                                 || (CurrentImportedData.BN != "" && p.BN == CurrentImportedData.BN)
                                                 || (CurrentImportedData.IDCard != "" && p.IDCard == CurrentImportedData.IDCard)
                                             select p);

                        if (string.IsNullOrEmpty(CurrentImportedData.FirstName) ||
   string.IsNullOrEmpty(CurrentImportedData.Gender) ||
   string.IsNullOrEmpty(CurrentImportedData.LastName) ||
   string.IsNullOrEmpty(CurrentImportedData.DateOfBirth) || (birthdttm > System.DateTime.Now))
                        {
                            CurrentImportedData.IsInComplete = "Y";
                            IsIncompleteRecordPresent = true;
                            CurrentImportedData.Register = false;

                        }

                        else if (duplicateList != null && duplicateList.Count() > 0)
                        {
                            foreach (PatientRegistrationBulkData item in duplicateList)
                            {
                                item.IsDuplicate = "Y";
                                item.RowColor = Colors.Yellow.ToString();
                            }
                            IsRecordDoubleinExcel = true;
                            CurrentImportedData.IsDuplicate = "Y";
                        }

                        AdjustRowColor(CurrentImportedData);


                        ImportedDataList.Add(CurrentImportedData);

                        pgBarCounter = pgBarCounter + 1;
                        TotalRecord = pgBarCounter;
                        view.SetProgressBarValue(pgBarCounter);
                    }

                    view.SetProgressBarValue(upperlimit);

                    TotalRecord = ImportedDataList.Count;
                    string msg = "Total No. of Patient read from Excel: " + ImportedDataList.Count.ToString();

                    if (isFutureBirthDatePresent)
                    {
                        msg = msg + "\r\n* Some records have future Date of Birth and are marked as Incomplete";
                    }
                    else if (IsIncompleteRecordPresent)
                    {
                        msg = msg + "\r\n* Some records are Incomplete";
                    }

                    if (IsRecordDoubleinExcel)
                    {
                        msg = msg + "\r\n* Some records are Duplicate in Excel";
                    }
                    ((RegistrationBulkImport)View).ShowMessageBox(msg, "", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

                    if (patientHNDupicate != null && patientHNDupicate.Count > 0)
                    {
                        ShowModalDialogUsingViewModel(new BulkAlertDialog(), new BulkAlertDialogViewModel(patientHNDupicate), false);
                    }

                    PatientDataList = new ObservableCollection<PatientRegistrationBulkData>(ImportedDataList);
                }
            }
            catch (Exception ex)
            {
                ErrorDialog(ex.Message);
            }
        }

        private void Register()
        {
            try
            {
                if (ImportedDataList != null)
                {
                    RegistrationBulkImport view = (RegistrationBulkImport)this.View;
                    int upperlimit = 0;
                    int loopCounter = 0;
                    int regisPatient = 0;
                    int updatePatient = 0;
                    foreach (var currentData in ImportedDataList)
                    {
                        if (currentData.Register == true)
                        {
                            upperlimit++;
                        }
                    }
                    view.SetProgressBarLimits(0, upperlimit);

                    foreach (var currentData in ImportedDataList)
                    {
                        if (currentData.Register == true)
                        {
                            PatientInformationModel patientModel = new PatientInformationModel();
                            patientModel.PatientUID = currentData.PatientUID;
                            patientModel.PatientID = currentData.BN;


                            patientModel.PatientOtherID = currentData.PatientOtherID;
                            patientModel.EmployeeID = currentData.EmployeeID;
                            patientModel.SEXXXUID = currentData.SEXXXUID;
                            patientModel.TITLEUID = currentData.TITLEUID;
                            patientModel.BirthDttm = currentData.BirthDttm;
                            patientModel.DOBComputed = currentData.DOBComputed;
                            patientModel.FirstName = currentData.FirstName;
                            patientModel.LastName = currentData.LastName;
                            patientModel.NationalID = currentData.IDCard.Replace("-", "");
                            patientModel.IDPassport = currentData.IDPassport.Replace("-", "");
                            patientModel.MobilePhone = currentData.MobilePhone;
                            patientModel.Department = currentData.Department?.Trim();
                            patientModel.Position = currentData.Position?.Trim();
                            var patregistered = DataService.PatientIdentity.RegisterPatient(patientModel, AppUtil.Current.UserID, 5);
                            if (currentData.PatientUID == 0)
                            {

                                currentData.BN = patregistered.PatientID;
                                currentData.PatientUID = patregistered.PatientUID;
                                currentData.RowColor = Colors.DarkBlue.ToString();
                                regisPatient++;
                                RegisteredCount = regisPatient;

                            }
                            else
                            {
                                updatePatient++;
                                UpdateCount = updatePatient;
                            }



                            currentData.Register = false;
                            loopCounter = loopCounter + 1;
                            view.SetProgressBarValue(loopCounter);
                        }
                    }

                    PatientDataList = new ObservableCollection<PatientRegistrationBulkData>(ImportedDataList);
                    view.SetProgressBarValue(upperlimit);
                    RegisteredCount = regisPatient;
                    UpdateCount = updatePatient;
                    InformationDialog("Total No. of Patient Registered: " + regisPatient + Environment.NewLine + "Total No. of Patient Update: " + updatePatient);
                    view.progressBar1.Value = 0;
                    view.PatientGrid.RefreshData();
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void CreateVisit()
        {
            try
            {
                int loopCounter = 0;
                int createVisitCount = 0;
                int toBeCreateVisitCount = 0;
                if (PatientDataList != null)
                {
                    RegistrationBulkImport view = (RegistrationBulkImport)this.View;
                    foreach (var item in PatientDataList)
                    {
                        if (item.Register && !string.IsNullOrEmpty(item.BN) && !item.HasVisitToday)
                        {
                            toBeCreateVisitCount += 1;
                        }
                    }
                    view.SetProgressBarLimits(0, toBeCreateVisitCount);
                    if (toBeCreateVisitCount > 0)
                    {
                        CreateVisit createVisitPopUp = new Views.CreateVisit();
                        var datacontext = (createVisitPopUp.DataContext as CreateVisitViewModel);
                        datacontext.ShowSaveButton = System.Windows.Visibility.Visible;
                        datacontext.IsMassRegister = true;
                        datacontext.Patient = null;

                        CreateVisitViewModel result = (CreateVisitViewModel)LaunchViewDialogNonPermiss(createVisitPopUp, true);
                        if (result != null && result.ResultDialog == ActionDialog.Save)
                        {
                            foreach (PatientRegistrationBulkData currentData in ImportedDataList)
                            {
                                if (currentData.Register && !string.IsNullOrEmpty(currentData.BN) && !currentData.HasVisitToday)
                                {
                                    PatientVisitModel visitInfo = result.PatientVisitInfo;
                                    visitInfo.PatientUID = currentData.PatientUID;
                                    visitInfo.VISTSUID = 418; //Medical Discharge
                                    visitInfo.CompanyName = currentData.Company;
                                    visitInfo.RefNo = currentData.No;
                                    visitInfo.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
                                    PatientVisitModel returnData = DataService.PatientIdentity.SavePatientVisit(visitInfo, AppUtil.Current.UserID);

                                    currentData.VisitID = returnData.VisitID;
                                    currentData.HasVisitToday = true;
                                    createVisitCount = createVisitCount + 1;
                                    CreateVisitCount = createVisitCount;
                                }
                                currentData.Register = false;
                                loopCounter = loopCounter + 1;
                                view.SetProgressBarValue(loopCounter);
                            }
                            view.SetProgressBarValue(toBeCreateVisitCount);

                            //Show Message 
                            if (CreateVisitCount > 0)
                            {
                                InformationDialog("Total No. of Patient Visit Created: " + CreateVisitCount);
                            }
                            else if (toBeCreateVisitCount == 0)
                            {
                                InformationDialog("Visit Already exists for the patients or None of the Registered patient are selected.");
                            }

                            view.progressBar1.Value = 0;
                            view.PatientGrid.RefreshData();
                        }

                    }
                }
            }
            catch (Exception er)
            {
                ErrorDialog(er.Message);
            }

        }

        private void Export()
        {
            try
            {
                if (ImportedDataList != null)
                {
                    string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xlsx");
                    if (fileName != "")
                    {
                        RegistrationBulkImport view = (RegistrationBulkImport)this.View;
                        view.tableView.ExportToXlsx(fileName);
                        OpenFile(fileName);
                    }

                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void AdjustRowColor(PatientRegistrationBulkData CurrentData)
        {
            #region RowColor
            if (CurrentData.BN != "")
            {
                CurrentData.RowColor = Colors.DarkBlue.ToString();
            }
            else
            {
                CurrentData.RowColor = Colors.BlueViolet.ToString();
            }

            if (CurrentImportedData.HasVisitToday)
            {
                CurrentData.RowColor = Colors.Orange.ToString();
            }
            else if (CurrentData.IsInComplete == "Y")
            {
                CurrentData.RowColor = Colors.Green.ToString();
            }
            else if (CurrentImportedData.IsDuplicate == "Y")
            {
                CurrentData.RowColor = Colors.Yellow.ToString();
            }
            else if (CurrentImportedData.IsNationalIDDuplicate == "Y")
            {
                CurrentData.RowColor = Colors.Red.ToString();
            }

            #endregion
        }

        private void PrintSticker()
        {
            int upperlimit = 0;
            int pgBarCounter = 0;
            try
            {

                if (PatientDataList != null && PatientDataList.Count > 0)
                {
                    RegistrationBulkImport view = (RegistrationBulkImport)this.View;
                    foreach (var currentData in PatientDataList)
                    {
                        if (currentData.Register == true)
                        {
                            upperlimit++;
                        }
                    }
                    view.SetProgressBarLimits(0, upperlimit);
                    //int No = 1;
                    var patientASC = PatientDataList.OrderBy(p => p.No);
                    foreach (var patient in patientASC.ToList())
                    {
                        if (patient.Register)
                        {
                            PatientStickerBarcode rpt = new PatientStickerBarcode();
                            ReportPrintTool printTool = new ReportPrintTool(rpt);

                            string gender;
                            switch (patient.Gender)
                            {
                                case "หญิง (Female)":
                                case "F":
                                    gender = "(F)";
                                    break;
                                case "ชาย (Male)":
                                case "M":
                                    gender = "(M)";
                                    break;
                                default:
                                    gender = "(N/A)";
                                    break;
                            }

                            rpt.Parameters["PatientName"].Value = patient.PreName + " " + patient.FirstName + " " + patient.LastName + " " + gender;
                            rpt.Parameters["HN"].Value = patient.BN;

                            rpt.Parameters["No"].Value = patient.No;

                            rpt.Parameters["Age"].Value = patient.Age;
                            rpt.Parameters["BirthDttm"].Value = patient.BirthDttm != null ? patient.BirthDttm.Value.ToString("dd/MM/yyyy") : "";
                            rpt.Parameters["Department"].Value = patient.Department;
                            rpt.Parameters["EmployeeID"].Value = patient.EmployeeID;
                            rpt.Parameters["CompanyName"].Value = patient.Company;
                            rpt.RequestParameters = false;
                            rpt.ShowPrintMarginsWarning = false;
                            for (int i = 0; i < StickerQuantity; i++)
                            {
                                printTool.Print();
                            }

                            patient.Register = false;
                            pgBarCounter = pgBarCounter + 1;
                            view.SetProgressBarValue(pgBarCounter);
                        }
                        //SelectPatientXrays.Remove(item);



                        //OnUpdateEvent();

                        //No++;
                    }
                    view.SetProgressBarValue(upperlimit);
                    view.PatientGrid.RefreshData();
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void MobileSticker()
        {
            int upperlimit = 0;
            int pgBarCounter = 0;
            try
            {

                if (PatientDataList != null && PatientDataList.Count > 0)
                {
                    RegistrationBulkImport view = (RegistrationBulkImport)this.View;
                    foreach (var currentData in PatientDataList)
                    {
                        if (currentData.Register == true)
                        {
                            upperlimit++;
                        }
                    }
                    view.SetProgressBarLimits(0, upperlimit);
                    //int No = 1;
                    var patientASC = PatientDataList.OrderBy(p => p.No);
                    foreach (var patient in patientASC.ToList())
                    {
                        if (patient.Register)
                        {
                            if (SelectMobileStickers != null)
                            {
                                List<LookupReferenceValueModel> stickers = new List<LookupReferenceValueModel>();
                                foreach (var item in SelectMobileStickers)
                                {
                                    var newItem = (item as LookupReferenceValueModel);
                                    stickers.Add(newItem);
                                }
                                foreach (var SelectSticker in stickers.OrderByDescending(p => p.DisplayOrder))
                                {
                                    var item = (SelectSticker as LookupReferenceValueModel);
                                    for (int i = 0; i < item.Key; i++)
                                    {
                                        MobileSticker rpt = new MobileSticker();
                                        ReportPrintTool printTool = new ReportPrintTool(rpt);

                                        string gender;
                                        switch (patient.Gender)
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

                                        rpt.Parameters["PatientName"].Value = patient.PreName + " " + patient.FirstName + " " + patient.LastName + " " + gender;

                                        rpt.Parameters["HN"].Value = patient.BN;
                                        rpt.Parameters["No"].Value = patient.No;

                                        if (!string.IsNullOrEmpty(patient.Age))
                                        {
                                            rpt.Parameters["Age"].Value = patient.Age;
                                        }
                                        else if (patient.BirthDttm != null)
                                        {
                                            rpt.Parameters["Age"].Value = ShareLibrary.UtilDate.calAgeFromBirthDate(patient.BirthDttm.Value);
                                        }

                                        rpt.Parameters["BirthDttm"].Value = patient.BirthDttm != null ? patient.BirthDttm.Value.ToString("dd/MM/yyyy") : "";
                                        rpt.Parameters["CheckUp"].Value = item.Display;
                                        rpt.Parameters["Company"].Value = patient.Company;
                                        rpt.Parameters["Program"].Value = patient.Program;
                                        rpt.RequestParameters = false;
                                        rpt.ShowPrintMarginsWarning = false;
                                        printTool.Print();

                                    }

                                }
                            }

                            patient.Register = false;
                            pgBarCounter = pgBarCounter + 1;
                            view.SetProgressBarValue(pgBarCounter);
                        }


                    }
                    view.SetProgressBarValue(upperlimit);
                    view.PatientGrid.RefreshData();
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void PrintAppointment()
        {
            int upperlimit = 0;
            int pgBarCounter = 0;
            try
            {
                if (PatientDataList != null && PatientDataList.Count > 0)
                {
                    RegistrationBulkImport view = (RegistrationBulkImport)this.View;
                    foreach (var currentData in PatientDataList)
                    {
                        if (currentData.Register == true)
                        {
                            upperlimit++;
                        }
                    }
                    view.SetProgressBarLimits(0, upperlimit);
                    var patientASC = PatientDataList.OrderBy(p => p.No);
                    foreach (var patient in patientASC.ToList())
                    {
                        if (patient.Register)
                        {
                            AppointmentMobile rpt = new AppointmentMobile();
                            ReportPrintTool printTool = new ReportPrintTool(rpt);

                            rpt.Parameters["HN"].Value = patient.BN;
                            rpt.Parameters["PatientName"].Value = patient.PreName + " " + patient.FirstName + " " + patient.LastName;
                            rpt.Parameters["StartDate"].Value = patient.CheckupDttm != null ? patient.CheckupDttm.Value.ToString("dd/MM/yyyy") : "";
                            rpt.Parameters["DepartmentEmployee"].Value = patient.Department;
                            rpt.Parameters["EmployeeID"].Value = patient.EmployeeID;

                            if (!string.IsNullOrEmpty(patient.Age))
                            {
                                rpt.Parameters["Age"].Value = patient.Age;
                            }
                            else if (patient.BirthDttm != null)
                            {
                                rpt.Parameters["Age"].Value = ShareLibrary.UtilDate.calAgeFromBirthDate(patient.BirthDttm.Value);
                            }
                            rpt.Parameters["Group"].Value = patient.Group;
                            rpt.Parameters["CompanyName"].Value = patient.Company;
                            rpt.Parameters["PositionEmployee"].Value = patient.Position;
                            rpt.ShowPrintMarginsWarning = false;
                            printTool.Print();
                        }
                        patient.Register = false;
                        pgBarCounter = pgBarCounter + 1;
                        view.SetProgressBarValue(pgBarCounter);
                    }
                    view.SetProgressBarValue(upperlimit);
                    view.PatientGrid.RefreshData();
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
