using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.Interface;
using MediTech.Model;
using MediTech.Reports.Operating.Checkup.CheckupBookReport;
using MediTech.Reports.Operating.Lab;
using MediTech.Reports.Operating.Radiology;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class SummeryViewViewModel : MediTechViewModelBase, IPatientVisitViewModel
    {
        #region Properties

        private ObservableCollection<CCHPIModel> _ListCCHPI;

        public ObservableCollection<CCHPIModel> ListCCHPI
        {
            get { return _ListCCHPI; }
            set { Set(ref _ListCCHPI, value); }
        }

        private CCHPIModel _SelectCCHPI;

        public CCHPIModel SelectCCHPI
        {
            get { return _SelectCCHPI; }
            set { Set(ref _SelectCCHPI, value); }
        }


        private ObservableCollection<ProgressNoteModel> _ListNote;

        public ObservableCollection<ProgressNoteModel> ListNote
        {
            get { return _ListNote; }
            set { Set(ref _ListNote, value); }
        }

        private ObservableCollection<PhysicalExamModel> _ListPhysicalExam;

        public ObservableCollection<PhysicalExamModel> ListPhysicalExam
        {
            get { return _ListPhysicalExam; }
            set { Set(ref _ListPhysicalExam, value); }
        }

        private PhysicalExamModel _SelectPhysicalExam;

        public PhysicalExamModel SelectPhysicalExam
        {
            get { return _SelectPhysicalExam; }
            set { Set(ref _SelectPhysicalExam, value); }
        }


        private ObservableCollection<PatientProblemModel> _ListDiagnosis;

        public ObservableCollection<PatientProblemModel> ListDiagnosis
        {
            get { return _ListDiagnosis; }
            set { Set(ref _ListDiagnosis, value); }
        }

        private bool _Permission;

        public bool Permission
        {
            get { return _Permission; }
            set { Set(ref _Permission, value); }
        }

        #region PatientOrder
        private ObservableCollection<PatientOrderDetailModel> _ListOrder;

        public ObservableCollection<PatientOrderDetailModel> ListOrder
        {
            get { return _ListOrder; }
            set { Set(ref _ListOrder, value); }
        }

        private ObservableCollection<PatientOrderDetailModel> _ListOrderMaximized;

        public ObservableCollection<PatientOrderDetailModel> ListOrderMaximized
        {
            get { return _ListOrderMaximized; }
            set { Set(ref _ListOrderMaximized, value); }
        }

        private DateTime? _DateFromOrderDetail;

        public DateTime? DateFromOrderDetail
        {
            get { return _DateFromOrderDetail; }
            set { Set(ref _DateFromOrderDetail, value); }
        }


        private DateTime? _DateToOrderDetail;

        public DateTime? DateToOrderDetail
        {
            get { return _DateToOrderDetail; }
            set { Set(ref _DateToOrderDetail, value); }
        }
        #endregion

        #region DrugProfile

        private ObservableCollection<PatientOrderDetailModel> _ListDrugProfile;

        public ObservableCollection<PatientOrderDetailModel> ListDrugProfile
        {
            get { return _ListDrugProfile; }
            set { Set(ref _ListDrugProfile, value); }
        }


        private ObservableCollection<PatientOrderDetailModel> _ListDrugProfileMaximized;

        public ObservableCollection<PatientOrderDetailModel> ListDrugProfileMaximized
        {
            get { return _ListDrugProfileMaximized; }
            set { Set(ref _ListDrugProfileMaximized, value); }
        }

        private DateTime? _DateFromDrugProfile;

        public DateTime? DateFromDrugProfile
        {
            get { return _DateFromDrugProfile; }
            set { Set(ref _DateFromDrugProfile, value); }
        }


        private DateTime? _DateToDrugProfile;

        public DateTime? DateToDrugProfile
        {
            get { return _DateToDrugProfile; }
            set { Set(ref _DateToDrugProfile, value); }
        }
        #endregion

        #region Radiology

        private ObservableCollection<ResultRadiologyModel> _ListRadiologyResult;

        public ObservableCollection<ResultRadiologyModel> ListRadiologyResult
        {
            get { return _ListRadiologyResult; }
            set { Set(ref _ListRadiologyResult, value); }
        }

        private ObservableCollection<ResultRadiologyModel> _ListRadiologyResultMaximized;

        public ObservableCollection<ResultRadiologyModel> ListRadiologyResultMaximized
        {
            get { return _ListRadiologyResultMaximized; }
            set { Set(ref _ListRadiologyResultMaximized, value); }
        }

        private DateTime? _DateFromRadiologyResult;

        public DateTime? DateFromRadiologyResult
        {
            get { return _DateFromRadiologyResult; }
            set { Set(ref _DateFromRadiologyResult, value); }
        }


        private DateTime? _DateToRadiologyResult;

        public DateTime? DateToRadiologyResult
        {
            get { return _DateToRadiologyResult; }
            set { Set(ref _DateToRadiologyResult, value); }
        }



        #endregion



        private ObservableCollection<ResultModel> _ListLabsResult;

        public ObservableCollection<ResultModel> ListLabsResult
        {
            get { return _ListLabsResult; }
            set { Set(ref _ListLabsResult, value); }
        }

        private ObservableCollection<WellnessDataModel> _ListWellnessData;

        public ObservableCollection<WellnessDataModel> ListWellnessData
        {
            get { return _ListWellnessData; }
            set { Set(ref _ListWellnessData, value); }
        }

        private ResultRadiologyModel _SelectRadiologyResult;

        public ResultRadiologyModel SelectRadiologyResult
        {
            get { return _SelectRadiologyResult; }
            set { Set(ref _SelectRadiologyResult, value); }
        }

        private PatientVisitModel _SelectedPatientVisit;

        public PatientVisitModel SelectedPatientVisit
        {
            get { return _SelectedPatientVisit; }
            set
            {
                Set(ref _SelectedPatientVisit, value);
                if (SelectedPatientVisit != null)
                {
                    LoadCCHPI();
                    LoadPhyPE();
                    LoadDianosis();
                    LoadPatientOrder();
                    LoadProgressNote();
                    LoadRaiologyResult();
                    LoadLabResult();
                    LoadWellnessData();
                }
                else
                {
                    ListCCHPI = null;
                    ListPhysicalExam = null;
                    ListDiagnosis = null;
                    ListOrder = null;
                    ListDrugProfile = null;
                    ListNote = null;
                    ListRadiologyResult = null;
                    ListLabsResult = null;
                    ListWellnessData = null;
                }
            }
        }

        private ResultModel _SelectLabResult;

        public ResultModel SelectLabResult
        {
            get { return _SelectLabResult; }
            set { Set(ref _SelectLabResult, value); }
        }

        private ProgressNoteModel _SelectProgressNote;

        public ProgressNoteModel SelectProgressNote
        {
            get { return _SelectProgressNote; }
            set { Set(ref _SelectProgressNote, value); }
        }

        private WellnessDataModel _SelectWellnessData;

        public WellnessDataModel SelectWellnessData
        {
            get { return _SelectWellnessData; }
            set { Set(ref _SelectWellnessData, value); }
        }

        #region LabResultDetail

        private DateTime? _DateFromLabResultDetail;

        public DateTime? DateFromLabResultDetail
        {
            get { return _DateFromLabResultDetail; }
            set { Set(ref _DateFromLabResultDetail, value); }
        }


        private DateTime? _DateToLabResultDetail;

        public DateTime? DateToLabResultDetail
        {
            get { return _DateToLabResultDetail; }
            set { Set(ref _DateToLabResultDetail, value); }
        }



        private ObservableCollection<RequestLabModel> _LabResultDetails;

        public ObservableCollection<RequestLabModel> LabResultDetails
        {
            get { return _LabResultDetails; }
            set { Set(ref _LabResultDetails, value); }
        }


        private RequestLabModel _SelectLabResultDetail;

        public RequestLabModel SelectLabResultDetail
        {
            get { return _SelectLabResultDetail; }
            set { Set(ref _SelectLabResultDetail, value); }
        }


        private ResultComponentModel _SelectResultComponent;

        public ResultComponentModel SelectResultComponent
        {
            get { return _SelectResultComponent; }
            set { Set(ref _SelectResultComponent, value); }
        }
        #endregion

        #endregion

        #region Command

        #region PatientOrder

        private RelayCommand _OrderCommand;

        public RelayCommand OrderCommand
        {
            get { return _OrderCommand ?? (_OrderCommand = new RelayCommand(OpenOrder)); }
        }

        private RelayCommand _SearchOrderDetailsCommand;

        public RelayCommand SearchOrderDetailsCommand
        {
            get { return _SearchOrderDetailsCommand ?? (_SearchOrderDetailsCommand = new RelayCommand(SearchOrderDetails)); }
        }

        private RelayCommand _LoadOrderMaximizedCommand;

        public RelayCommand LoadOrderMaximizedCommand
        {
            get { return _LoadOrderMaximizedCommand ?? (_LoadOrderMaximizedCommand = new RelayCommand(LoadOrderMaximized)); }
        }
        #endregion

        #region DrguProfile

        private RelayCommand _SearchDrugProfileCommand;

        public RelayCommand SearchDrugProfileCommand
        {
            get { return _SearchDrugProfileCommand ?? (_SearchDrugProfileCommand = new RelayCommand(SearchDrugProfile)); }
        }

        private RelayCommand _LoadDrugProfileMaximizedCommand;

        public RelayCommand LoadDrugProfileMaximizedCommand
        {
            get { return _LoadDrugProfileMaximizedCommand ?? (_LoadDrugProfileMaximizedCommand = new RelayCommand(LoadDrugProfileMaximized)); }
        }

        #endregion

        private RelayCommand _DiagnosisCommand;

        public RelayCommand DiagnosisCommand
        {
            get { return _DiagnosisCommand ?? (_DiagnosisCommand = new RelayCommand(OpenDiagnosis)); }
        }

        private RelayCommand _PhysicalCommand;

        public RelayCommand PhysicalCommand
        {
            get { return _PhysicalCommand ?? (_PhysicalCommand = new RelayCommand(OpenPhysical)); }
        }

        private RelayCommand _EditPhysicalCommand;

        public RelayCommand EditPhysicalCommand
        {
            get { return _EditPhysicalCommand ?? (_EditPhysicalCommand = new RelayCommand(EditPhysical)); }
        }

        private RelayCommand _CCHPICommand;

        public RelayCommand CCHPICommand
        {
            get { return _CCHPICommand ?? (_CCHPICommand = new RelayCommand(OpenCCHPI)); }
        }

        private RelayCommand _EditCCHPICommand;

        public RelayCommand EditCCHPICommand
        {
            get { return _EditCCHPICommand ?? (_EditCCHPICommand = new RelayCommand(EditCCHPI)); }
        }

        private RelayCommand _ProgressNoteCommand;

        public RelayCommand ProgressNoteCommand
        {
            get { return _ProgressNoteCommand ?? (_ProgressNoteCommand = new RelayCommand(ProgressNote)); }
        }

        private RelayCommand _EditProgressNoteCommand;

        public RelayCommand EditProgressNoteCommand
        {
            get { return _EditProgressNoteCommand ?? (_EditProgressNoteCommand = new RelayCommand(EditProgressNote)); }
        }


        private RelayCommand<int> _DeleteProgressNoteCommand;

        public RelayCommand<int> DeleteProgressNoteCommand
        {
            get { return _DeleteProgressNoteCommand ?? (_DeleteProgressNoteCommand = new RelayCommand<int>(DeleteProgressNote)); }
        }


        private RelayCommand _WellnessDataCommand;

        public RelayCommand WellnessDataCommand
        {
            get { return _WellnessDataCommand ?? (_WellnessDataCommand = new RelayCommand(OpenWellnessData)); }
        }

        private RelayCommand _EditWellnessDataCommand;

        public RelayCommand EditWellnessDataCommand
        {
            get { return _EditWellnessDataCommand ?? (_EditWellnessDataCommand = new RelayCommand(EditWellnessData)); }
        }

        private RelayCommand<int> _PreviewWellnessBookCommand;

        public RelayCommand<int> PreviewWellnessBookCommand
        {
            get { return _PreviewWellnessBookCommand ?? (_PreviewWellnessBookCommand = new RelayCommand<int>(PreviewWellnessBook)); }
        }

        private RelayCommand<int> _DeleteWellnessCommand;

        public RelayCommand<int> DeleteWellnessCommand
        {
            get { return _DeleteWellnessCommand ?? (_DeleteWellnessCommand = new RelayCommand<int>(DeleteWellness)); }
        }


        #region Radiology

        private RelayCommand _RadiologyViewCommand;

        public RelayCommand RadiologyViewCommand
        {
            get { return _RadiologyViewCommand ?? (_RadiologyViewCommand = new RelayCommand(RadiologyView)); }
        }

        private RelayCommand _RadiologyImageCommand;

        public RelayCommand RadiologyImageCommand
        {
            get { return _RadiologyImageCommand ?? (_RadiologyImageCommand = new RelayCommand(RadiologyImage)); }
        }

        private RelayCommand _LoadRadiologyResultMaximizedCommand;

        public RelayCommand LoadRadiologyResultMaximizedCommand
        {
            get { return _LoadRadiologyResultMaximizedCommand ?? (_LoadRadiologyResultMaximizedCommand = new RelayCommand(LoadRadiologyResultMaximized)); }
        }


        private RelayCommand _SearchRadiologyResultCommand;

        public RelayCommand SearchRadiologyResultCommand
        {
            get { return _SearchRadiologyResultCommand ?? (_SearchRadiologyResultCommand = new RelayCommand(SearchRadiologyResult)); }
        }

        #endregion

        #region LabResultDetail

        private RelayCommand _SearchLabResultDetailsCommand;

        public RelayCommand SearchLabResultDetailsCommand
        {
            get { return _SearchLabResultDetailsCommand ?? (_SearchLabResultDetailsCommand = new RelayCommand(SearchLabResultDetails)); }
        }

        private RelayCommand<string> _LabViewCommand;

        public RelayCommand<string> LabViewCommand
        {
            get { return _LabViewCommand ?? (_LabViewCommand = new RelayCommand<string>(LabView)); }
        }


        private RelayCommand _LoadLabResultDetailsCommand;

        public RelayCommand LoadLabResultDetailsCommand
        {
            get { return _LoadLabResultDetailsCommand ?? (_LoadLabResultDetailsCommand = new RelayCommand(LoadLabResultDetails)); }
        }


        private RelayCommand<long> _OpenLabImageCommand;
        public RelayCommand<long> OpenLabImageCommand
        {
            get
            {
                return _OpenLabImageCommand
                    ?? (_OpenLabImageCommand = new RelayCommand<long>(OpenLabImage));
            }

        }

        #endregion

        #endregion

        #region Method

        int REGST = 417;
        int CHKOUT = 418;
        int SNDDOC = 419;
        int BLINP = 423;
        int FINDIS = 421;



        public void LoadCCHPI()
        {
            if (SelectedPatientVisit.PatientVisitUID == 0)
            {
                ListCCHPI = new ObservableCollection<CCHPIModel>(DataService.PatientHistory.GetCCHPIByPatientUID(SelectedPatientVisit.PatientUID));
            }
            else
            {
                ListCCHPI = new ObservableCollection<CCHPIModel>(DataService.PatientHistory.GetCCHPIByVisit(SelectedPatientVisit.PatientVisitUID));
            }

            if (ListCCHPI != null)
            {
                ListCCHPI = new ObservableCollection<CCHPIModel>(ListCCHPI.OrderByDescending(p => p.CWhen));
            }

            if (ListCCHPI != null && ListCCHPI.Count > 0)
            {
                foreach (var patCCHPI in ListCCHPI)
                {
                    patCCHPI.DisplayComplaint = patCCHPI.Complaint;

                    if ((patCCHPI.Period != null && patCCHPI.Period != 0) && patCCHPI.AGUOMUID != null)
                    {
                        patCCHPI.DisplayComplaint += " ( " + patCCHPI.Period.ToString() + " " + patCCHPI.DateUnit + " ) ";
                    }
                    else if ((patCCHPI.Period != null && patCCHPI.Period != 0) && patCCHPI.AGUOMUID == null)
                    {
                        patCCHPI.DisplayComplaint += " ( " + patCCHPI.Period.ToString() + " ) ";
                    }
                }
            }
        }

        public void LoadProgressNote()
        {
            if (SelectedPatientVisit.PatientVisitUID == 0)
            {
                ListNote = new ObservableCollection<ProgressNoteModel>(DataService.PatientHistory.GetProgressNoteByPatientUID(SelectedPatientVisit.PatientUID));
            }
            else
            {
                ListNote = new ObservableCollection<ProgressNoteModel>(DataService.PatientHistory.GetProgressNoteByVisit(SelectedPatientVisit.PatientVisitUID));
            }

            if (ListNote != null)
            {
                ListNote = new ObservableCollection<ProgressNoteModel>(ListNote.OrderByDescending(p => p.CWhen));
            }
        }

        private void DeleteProgressNote(int progressNoteUID)
        {
            MessageBoxResult result = QuestionDialog("คุณต้องการลบ Note ใช้หรือไม่ ?");
            if (result == MessageBoxResult.Yes)
            {
                DataService.PatientHistory.DeleteProgressNote(progressNoteUID, AppUtil.Current.UserID);
                LoadProgressNote();
            }
        }

        public void LoadPhyPE()
        {
            if (SelectedPatientVisit.PatientVisitUID == 0)
            {
                ListPhysicalExam = new ObservableCollection<PhysicalExamModel>(DataService.PatientHistory.GetPhysicalExamByPatientUID(SelectedPatientVisit.PatientUID));
            }
            else
            {
                ListPhysicalExam = new ObservableCollection<PhysicalExamModel>(DataService.PatientHistory.GetPhysicalExamByVisit(SelectedPatientVisit.PatientVisitUID));
            }

            if (ListPhysicalExam != null)
            {
                ListPhysicalExam = new ObservableCollection<PhysicalExamModel>(ListPhysicalExam.OrderByDescending(p => p.CWhen));
            }
        }

        public void LoadWellnessData()
        {
            if (SelectedPatientVisit.PatientVisitUID == 0)
            {
                ListWellnessData = new ObservableCollection<WellnessDataModel>(DataService.PatientHistory.GetWellnessDataByPatient(SelectedPatientVisit.PatientUID));
            }
            else
            {
                ListWellnessData = new ObservableCollection<WellnessDataModel>(DataService.PatientHistory.GetWellnessDataByVisit(SelectedPatientVisit.PatientVisitUID));
            }
        }

        private void PreviewWellnessBook(int wellNessDataUID)
        {
            var previewWellness = ListWellnessData.FirstOrDefault(p => p.WellnessDataUID == wellNessDataUID);
            CheckupPage1 report = new CheckupPage1();
            ReportPrintTool printTool = new ReportPrintTool(report);
            report.Parameters["PatientUID"].Value = previewWellness.PatientUID;
            report.Parameters["PatientVisitUID"].Value = previewWellness.PatientVisitUID;
            report.Parameters["PayorDetailUID"].Value = previewWellness.PayorDetailUID;
            report.RequestParameters = false;
            report.ShowPrintMarginsWarning = false;
            printTool.ShowPreviewDialog();
        }
        private void DeleteWellness(int wellNessDataUID)
        {
            MessageBoxResult result = QuestionDialog("คุณต้องการลบข้อมูล Wellness ใช่หรือไม่ ?");
            if (result == MessageBoxResult.Yes)
            {
                DataService.PatientHistory.DeleteWellnessData(wellNessDataUID, AppUtil.Current.UserID);
                LoadWellnessData();
            }
        }

        public void LoadDianosis()
        {
            if (SelectedPatientVisit.PatientVisitUID == 0)
            {
                ListDiagnosis = new ObservableCollection<PatientProblemModel>(DataService.PatientDiagnosis.GetPatientProblemByPatientUID(SelectedPatientVisit.PatientUID));
            }
            else
            {
                ListDiagnosis = new ObservableCollection<PatientProblemModel>(DataService.PatientDiagnosis.GetPatientProblemByVisitUID(SelectedPatientVisit.PatientVisitUID));
            }

            if (ListDiagnosis != null)
            {
                ListDiagnosis = new ObservableCollection<PatientProblemModel>(ListDiagnosis.OrderByDescending(p => p.RecordedDttm));
            }
        }
        public void LoadPatientOrder()
        {
            if (SelectedPatientVisit.PatientVisitUID == 0)
            {
                ListOrder = new ObservableCollection<PatientOrderDetailModel>(DataService.OrderProcessing.GetOrderAllByPatientUID(SelectedPatientVisit.PatientUID));
            }
            else
            {
                ListOrder = new ObservableCollection<PatientOrderDetailModel>(DataService.OrderProcessing.GetOrderAllByVisitUID(SelectedPatientVisit.PatientVisitUID));
            }

            if (ListOrder != null)
            {

                ListOrder = new ObservableCollection<PatientOrderDetailModel>(ListOrder.OrderByDescending(p => p.StartDttm));
                ListDrugProfile = new ObservableCollection<PatientOrderDetailModel>(ListOrder.Where(p => p.BSMDDUID == 2826 && p.ORDSTUID != 2848));
            }

            DateFromOrderDetail = SelectedPatientVisit.StartDttm;
            DateToOrderDetail = null;

            DateFromDrugProfile = SelectedPatientVisit.StartDttm;
            DateToDrugProfile = null;

            ListOrderMaximized = ListOrder;
            ListDrugProfileMaximized = ListDrugProfile;
        }

        private void LoadOrderMaximized()
        {
            if (ListOrderMaximized == null || ListOrderMaximized.Count <= 0)
            {
                DateFromOrderDetail = SelectedPatientVisit.StartDttm;
                DateToOrderDetail = null;
                ListOrderMaximized = ListOrder;
            }
        }

        private void SearchOrderDetails()
        {
            ListOrderMaximized = new ObservableCollection<PatientOrderDetailModel>(DataService.OrderProcessing.GetOrderAllByPatientUID(SelectedPatientVisit.PatientUID, DateFromOrderDetail, DateToOrderDetail));
            if (ListOrderMaximized != null && ListOrderMaximized.Count > 0)
            {
                ListOrderMaximized = new ObservableCollection<PatientOrderDetailModel>(ListOrderMaximized.OrderByDescending(p => p.StartDttm));
            }
        }

        private void LoadDrugProfileMaximized()
        {
            if (ListDrugProfileMaximized == null || ListDrugProfileMaximized.Count <= 0)
            {
                DateFromDrugProfile = SelectedPatientVisit.StartDttm;
                DateToDrugProfile = null;
                ListDrugProfileMaximized = ListDrugProfile;
            }
        }

        private void SearchDrugProfile()
        {
            ListDrugProfileMaximized = null;
            var drugData = DataService.OrderProcessing.GetOrderDrugByPatientUID(SelectedPatientVisit.PatientUID, DateFromDrugProfile, DateToDrugProfile);
            if (drugData != null && drugData.PatientOrderDetail != null && drugData.PatientOrderDetail.Count > 0)
            {
                ListDrugProfileMaximized = new ObservableCollection<PatientOrderDetailModel>(drugData.PatientOrderDetail);
            }
            if (ListDrugProfileMaximized != null && ListDrugProfileMaximized.Count > 0)
            {
                ListDrugProfileMaximized = new ObservableCollection<PatientOrderDetailModel>(ListDrugProfileMaximized.Where(p => p.ORDSTUID != 2848).OrderByDescending(p => p.StartDttm));
            }
        }

        public void LoadRaiologyResult()
        {
            if (SelectedPatientVisit.PatientVisitUID == 0)
            {
                ListRadiologyResult = new ObservableCollection<ResultRadiologyModel>(DataService.Radiology.GetResultRadiologyByPatientUID(SelectedPatientVisit.PatientUID));
            }
            else
            {
                ListRadiologyResult = new ObservableCollection<ResultRadiologyModel>(DataService.Radiology.GetResultRadiologyByVisitUID(SelectedPatientVisit.PatientVisitUID));
            }

            if (ListRadiologyResult != null)
            {
                ListRadiologyResult = new ObservableCollection<ResultRadiologyModel>(ListRadiologyResult.OrderByDescending(p => p.ResultEnteredDttm));
            }

            DateFromRadiologyResult = SelectedPatientVisit.StartDttm;
            DateToRadiologyResult = null;
            ListRadiologyResultMaximized = ListRadiologyResult;
        }

        private void LoadRadiologyResultMaximized()
        {
            if (ListRadiologyResultMaximized == null || ListRadiologyResultMaximized.Count <= 0)
            {
                DateFromRadiologyResult = SelectedPatientVisit.StartDttm;
                DateToRadiologyResult = null;
                ListRadiologyResultMaximized = ListRadiologyResult;
            }
        }
        private void SearchRadiologyResult()
        {
            ListRadiologyResultMaximized = new ObservableCollection<ResultRadiologyModel>(DataService.Radiology.GetResultRadiologyByPatientUID(SelectedPatientVisit.PatientUID, DateFromRadiologyResult, DateToRadiologyResult));
            if (ListRadiologyResultMaximized != null && ListRadiologyResultMaximized.Count > 0)
            {
                ListRadiologyResultMaximized = new ObservableCollection<ResultRadiologyModel>(ListRadiologyResultMaximized.OrderByDescending(p => p.ResultEnteredDttm));
            }
        }

        public void LoadLabResult()
        {
            Permission = RoleIsConfidential();
            ListLabsResult = new ObservableCollection<ResultModel>();
            if (SelectedPatientVisit.PatientVisitUID == 0)
            {
                LabResultDetails = new ObservableCollection<RequestLabModel>(DataService.Lab.GetResultLabGroupRequestNumberByPatient(SelectedPatientVisit.PatientUID));
            }
            else
            {
                LabResultDetails = new ObservableCollection<RequestLabModel>(DataService.Lab.GetResultLabGroupRequestNumberByVisit(SelectedPatientVisit.PatientVisitUID));
            }

            if (LabResultDetails != null)
            {
                foreach (var resultDetail in LabResultDetails.OrderBy(p => p.LabNumber))
                {
                    foreach(var item in resultDetail.RequestDetailLabs.ToList())
                    {
                        if(item.IsConfidential == "Y")
                        {
                            if(Permission != true)
                            {
                                resultDetail.RequestDetailLabs.Remove(item);
                            }
                        }
                    }

                    var LabResult = resultDetail.RequestDetailLabs.Select(p => new ResultModel
                    {
                        ResultUID = p.ResultUID ?? 0,
                        PatientUID = p.PatientUID,
                        PatientVisitUID = p.PatientVisitUID,
                        RequestItemName = p.RequestItemName,
                        ResultEnteredDttm = p.ResultEnteredDttm,
                        LabNumber = resultDetail.LabNumber
                    }).OrderBy(p => p.RequestItemName).OrderByDescending(p => p.ResultEnteredDttm).ToList();

                    foreach (var item in LabResult)
                    {
                        ListLabsResult.Add(item);
                    }

                }
            }

            DateFromLabResultDetail = SelectedPatientVisit.StartDttm;
            DateToLabResultDetail = null;
            //ListLabsResult = DataService.Lab.GetResultLabByPatientVisitUID(SelectPatientVisit.PatientVisitUID);
        }

        #region LabResultDetail

        private void LoadLabResultDetails()
        {
            if (LabResultDetails == null || LabResultDetails.Count <= 0)
            {
                DateFromLabResultDetail = SelectedPatientVisit.StartDttm;
                DateToLabResultDetail = null;
                SearchLabResultDetails();
            }
        }


        private void SearchLabResultDetails()
        {
            LabResultDetails = new ObservableCollection<RequestLabModel>(DataService.Lab.GetResultLabGroupRequestNumberByPatient(SelectedPatientVisit.PatientUID, DateFromLabResultDetail, DateToLabResultDetail));
            if (LabResultDetails != null && LabResultDetails.Count > 0)
            {
                LabResultDetails = new ObservableCollection<RequestLabModel>(LabResultDetails.OrderByDescending(p => p.RequestedDttm));
            }
        }

        private void OpenLabImage(long resultCompentUID)
        {
            try
            {
                var imageData = DataService.Lab.GetResultImageByComponentUID(resultCompentUID);

                string extension = Path.GetExtension(SelectResultComponent.ResultValue); // "pdf", etc
                string filename = System.IO.Path.GetTempFileName() + extension; // Makes something like "C:\Temp\blah.tmp.pdf"

                File.WriteAllBytes(filename, imageData.ImageContent);


                var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = filename;
                process.StartInfo.Verb = "Open";
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                process.EnableRaisingEvents = true;
                //process.Exited += delegate
                //{
                //    System.IO.File.Delete(filename);
                //};
                process.Start();

                // Clean up our temporary file...


            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }

        #endregion

        private void OpenOrder()
        {
            if (SelectedPatientVisit == null || SelectedPatientVisit.PatientVisitUID == 0)
            {
                WarningDialog("กรุณาเลือก Visit");
                return;
            }
            PatientOrderEntry pageview = new PatientOrderEntry();
            (pageview.DataContext as PatientOrderEntryViewModel).AssingPatientVisit(SelectedPatientVisit);
            PatientOrderEntryViewModel result = (PatientOrderEntryViewModel)LaunchViewDialog(pageview, "ORDITM", false, true);

            if (result != null)
            {
                if (result.ResultDialog == ActionDialog.Save)
                    LoadPatientOrder();
            }

        }

        private void OpenDiagnosis()
        {
            if (SelectedPatientVisit == null || SelectedPatientVisit.PatientVisitUID == 0)
            {
                WarningDialog("กรุณาเลือก Visit");
                return;
            }
            PatientDiagnosis pageview = new PatientDiagnosis();
            (pageview.DataContext as PatientDiagnosisViewModel).AssignPatientVisit(SelectedPatientVisit);
            PatientDiagnosisViewModel result = (PatientDiagnosisViewModel)LaunchViewDialog(pageview, "PATDIAG", false);

            if (result != null)
            {
                if (result.ResultDialog == ActionDialog.Save)
                    LoadDianosis();
            }

        }

        private void OpenPhysical()
        {
            if (SelectedPatientVisit == null || SelectedPatientVisit.PatientVisitUID == 0)
            {
                WarningDialog("กรุณาเลือก Visit");
                return;
            }
            PhysicalExam pageview = new PhysicalExam();
            (pageview.DataContext as PhysicalExamViewModel).AssingPatientVisit(SelectedPatientVisit);
            PhysicalExamViewModel result = (PhysicalExamViewModel)LaunchViewDialog(pageview, "PHYEXAM", false);

            if (result != null)
            {
                if (result.ResultDialog == ActionDialog.Save)
                    LoadPhyPE();
            }

        }

        private void EditPhysical()
        {
            PhysicalExam pageview = new PhysicalExam();
            if (SelectPhysicalExam != null)
            {
                (pageview.DataContext as PhysicalExamViewModel).AssingPatientVisit(SelectedPatientVisit, SelectPhysicalExam);
            }
            else
            {
                if (SelectedPatientVisit == null || SelectedPatientVisit.PatientVisitUID == 0)
                {
                    WarningDialog("กรุณาเลือก Visit");
                    return;
                }
                (pageview.DataContext as PhysicalExamViewModel).AssingPatientVisit(SelectedPatientVisit);
            }
            PhysicalExamViewModel result = (PhysicalExamViewModel)LaunchViewDialog(pageview, "PHYEXAM", false);

            if (result != null)
            {
                if (result.ResultDialog == ActionDialog.Save)
                    LoadPhyPE();
            }
        }

        private void OpenCCHPI()
        {
            if (SelectedPatientVisit == null || SelectedPatientVisit.PatientVisitUID == 0)
            {
                WarningDialog("กรุณาเลือก Visit");
                return;
            }
            CCHPI pageview = new CCHPI();
            (pageview.DataContext as CCHPIViewModel).AssingPatientVisit(SelectedPatientVisit);
            CCHPIViewModel result = (CCHPIViewModel)LaunchViewDialog(pageview, "CCHPI", true);

            if (result != null)
            {
                if (result.ResultDialog == ActionDialog.Save)
                    LoadCCHPI();
            }
        }

        private void EditCCHPI()
        {
            CCHPI pageview = new CCHPI();
            if (SelectCCHPI != null)
            {
                (pageview.DataContext as CCHPIViewModel).AssingPatientVisit(SelectedPatientVisit, SelectCCHPI);
            }
            else
            {
                if (SelectedPatientVisit == null || SelectedPatientVisit.PatientVisitUID == 0)
                {
                    WarningDialog("กรุณาเลือก Visit");
                    return;
                }

                (pageview.DataContext as CCHPIViewModel).AssingPatientVisit(SelectedPatientVisit);
            }
            CCHPIViewModel result = (CCHPIViewModel)LaunchViewDialog(pageview, "CCHPI", true);
            if (result != null)
            {
                if (result.ResultDialog == ActionDialog.Save)
                    LoadCCHPI();
            }
        }

        private void OpenWellnessData()
        {
            if (SelectedPatientVisit == null || SelectedPatientVisit.PatientVisitUID == 0)
            {
                WarningDialog("กรุณาเลือก Visit");
                return;
            }
            WellnessData pageview = new WellnessData();
            (pageview.DataContext as WellnessDataViewModel).AssingPatientVisit(SelectedPatientVisit);
            WellnessDataViewModel result = (WellnessDataViewModel)LaunchViewDialog(pageview, "WELLNE", true);

            if (result != null)
            {
                if (result.ResultDialog == ActionDialog.Save)
                    LoadWellnessData();
            }
        }

        private void EditWellnessData()
        {
            WellnessData pageview = new WellnessData();
            if (SelectWellnessData != null)
            {
                (pageview.DataContext as WellnessDataViewModel).AssingPatientVisit(SelectedPatientVisit, SelectWellnessData);
            }
            else
            {
                if (SelectedPatientVisit == null || SelectedPatientVisit.PatientVisitUID == 0)
                {
                    WarningDialog("กรุณาเลือก Visit");
                    return;
                }
                (pageview.DataContext as WellnessDataViewModel).AssingPatientVisit(SelectedPatientVisit);
            }

            WellnessDataViewModel result = (WellnessDataViewModel)LaunchViewDialog(pageview, "WELLNE", true);

            if (result != null)
            {
                if (result.ResultDialog == ActionDialog.Save)
                    LoadWellnessData();
            }
        }
        private void ProgressNote()
        {
            if (SelectedPatientVisit == null || SelectedPatientVisit.PatientVisitUID == 0)
            {
                WarningDialog("กรุณาเลือก Visit");
                return;
            }
            ProgressNote pageview = new ProgressNote();
            (pageview.DataContext as ProgressNoteViewModel).AssignModelToProperties(SelectedPatientVisit);
            ProgressNoteViewModel result = (ProgressNoteViewModel)LaunchViewDialogNonPermiss(pageview, true);

            if (result != null)
            {
                if (result.ResultDialog == ActionDialog.Save)
                    LoadProgressNote();
            }
        }


        private void EditProgressNote()
        {
            ProgressNote pageview = new ProgressNote();
            if (SelectProgressNote != null)
            {
                (pageview.DataContext as ProgressNoteViewModel).AssignModelToProperties(SelectedPatientVisit, SelectProgressNote);
                ProgressNoteViewModel result = (ProgressNoteViewModel)LaunchViewDialogNonPermiss(pageview, true);
                if (result != null)
                {
                    if (result.ResultDialog == ActionDialog.Save)
                        LoadProgressNote();
                }
            }
            //else
            //{
            //    if (SelectPatientVisit == null || SelectPatientVisit.PatientVisitUID == 0)
            //    {
            //        WarningDialog("กรุณาเลือก Visit");
            //        return;
            //    }
            //    (pageview.DataContext as ProgressNoteViewModel).AssignModelToProperties(SelectPatientVisit);
            //}





        }

        private void RadiologyView()
        {
            if (SelectRadiologyResult != null)
            {
                ImagingReport rpt = new ImagingReport();
                //string organistion = SelectPatientVisit.OwnerOrganisation;
                string logo = "";
                //if (organistion.ToUpper().Contains("BRXG"))
                //{
                //    logo = "BRXG";
                //}
                //else if (organistion.ToUpper().Contains("DRC"))
                //{
                //    logo = "DRC";
                //}
                logo = "BRXG Polyclinic";
                rpt.LogoType = logo;
                ReportPrintTool printTool = new ReportPrintTool(rpt);


                rpt.Parameters["ResultUID"].Value = SelectRadiologyResult.ResultUID;
                rpt.RequestParameters = false;
                rpt.ShowPrintMarginsWarning = false;
                printTool.ShowPreviewDialog();
            }
        }

        private void RadiologyImage()
        {
            if (SelectRadiologyResult != null)
            {
                //ProcessStartInfo infro = new ProcessStartInfo();
                //string pacURL = SelectRadiologyResult.NavigationImage;
                //Process.Start(@"iexplore.exe", pacURL);
                PACSWorkList pacs = new PACSWorkList();
                PACSWorkListViewModel pacsViewModel = (pacs.DataContext as PACSWorkListViewModel);
                pacsViewModel.PatientID = SelectRadiologyResult.PatientID;
                pacsViewModel.IsCheckedPeriod = true;
                pacsViewModel.DateFrom = SelectRadiologyResult.ResultEnteredDttm.Value.AddDays(-1);
                pacsViewModel.DateTo = SelectRadiologyResult.ResultEnteredDttm;
                pacsViewModel.Modality = SelectRadiologyResult.Modality;
                pacsViewModel.IsOpenFromExam = true;
                LaunchViewDialog(pacs, "PACS", false, true);
            }

        }

        private void LabView(string labNumber)
        {
            LabResultReport rpt = new LabResultReport();
            ReportPrintTool printTool = new ReportPrintTool(rpt);

            var resultLab = LabResultDetails.FirstOrDefault(p => p.LabNumber == labNumber);

            rpt.Parameters["OrganisationUID"].Value = resultLab.OwnerOrganisationUID;
            rpt.Parameters["PatientVisitUID"].Value = resultLab.PatientVisitUID;
            rpt.Parameters["RequestNumber"].Value = resultLab.LabNumber;
            rpt.RequestParameters = false;
            rpt.ShowPrintMarginsWarning = false;
            printTool.ShowPreviewDialog();
        }

        public void AssignPatientVisit(PatientVisitModel patVisitData)
        {
            SelectedPatientVisit = patVisitData;
        }


        #endregion
    }
}
