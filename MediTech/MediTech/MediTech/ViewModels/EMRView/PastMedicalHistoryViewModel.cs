using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Reports.Operating.Patient;
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
using System.Windows.Controls;
using System.Windows.Documents;

namespace MediTech.ViewModels
{
    public class PastMedicalHistoryViewModel : MediTechViewModelBase
    {
        #region Properties

        private PatientVisitModel _SelectPatientVisit;

        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set { Set(ref _SelectPatientVisit, value); }
        }

        private List<PatientVisitModel> _PastVisitList;

        public List<PatientVisitModel> PastVisitList
        {
            get { return _PastVisitList; }
            set { Set(ref _PastVisitList, value); }
        }

        private PatientVisitModel _SelectPastVisit;

        public PatientVisitModel SelectPastVisit
        {
            get { return _SelectPastVisit; }
            set
            {
                Set(ref _SelectPastVisit, value);
                if (_SelectPastVisit != null)
                {
                    GetLastVitalSign();
                    LoadCCHPI();
                    LoadPatientAllergy();
                    LoadProgressNote();
                    LoadPhyPE();
                    LoadDianosis();
                    LoadPatientOrder();
                    LoadRaiologyResult();
                    LoadLabResult();
                }
            }
        }

        private string _VitalSignFormat;

        public string VitalSignFormat
        {
            get { return _VitalSignFormat; }
            set { Set(ref _VitalSignFormat, value); }
        }

        private string _ChiftComplain;

        public string ChiftComplain
        {
            get { return _ChiftComplain; }
            set { Set(ref _ChiftComplain, value); }
        }

        private string _Presentillness;

        public string Presentillness
        {
            get { return _Presentillness; }
            set { Set(ref _Presentillness, value); }
        }


        private string _AllergyFormat;

        public string AllergyFormat
        {
            get { return _AllergyFormat; }
            set { Set(ref _AllergyFormat, value); }
        }

        private ObservableCollection<CCHPIModel> _ListCCHPI;

        public ObservableCollection<CCHPIModel> ListCCHPI
        {
            get { return _ListCCHPI; }
            set { Set(ref _ListCCHPI, value); }
        }

        private ObservableCollection<ProgressNoteModel> _ListNote;

        public ObservableCollection<ProgressNoteModel> ListNote
        {
            get { return _ListNote; }
            set { Set(ref _ListNote, value); }
        }

        private List<PatientAllergyModel> _ListAllergy;

        public List<PatientAllergyModel> ListAllergy
        {
            get { return _ListAllergy; }
            set { Set(ref _ListAllergy, value); }
        }

        private string _PhysicalExam;

        public string PhysicalExam
        {
            get { return _PhysicalExam; }
            set { _PhysicalExam = value; }
        }


        private List<PatientProblemModel> _ListDiagnosis;

        public List<PatientProblemModel> ListDiagnosis
        {
            get { return _ListDiagnosis; }
            set { Set(ref _ListDiagnosis, value); }
        }

        private List<PatientOrderDetailModel> _ListOrder;

        public List<PatientOrderDetailModel> ListOrder
        {
            get { return _ListOrder; }
            set { Set(ref _ListOrder, value); }

        }

        private List<PatientOrderDetailModel> _ListDrugProfile;
        public List<PatientOrderDetailModel> ListDrugProfile
        {
            get { return _ListDrugProfile; }
            set { Set(ref _ListDrugProfile, value); }
        }

        private List<ResultRadiologyModel> _ListRadiologyResult;

        public List<ResultRadiologyModel> ListRadiologyResult
        {
            get { return _ListRadiologyResult; }
            set { Set(ref _ListRadiologyResult, value); }
        }


        private List<RequestLabModel> _LabResultDetails;

        public List<RequestLabModel> LabResultDetails
        {
            get { return _LabResultDetails; }
            set { Set(ref _LabResultDetails, value); }
        }

        private bool _IsCheckDiagnosis;

        public bool IsCheckDiagnosis
        {
            get { return _IsCheckDiagnosis; }
            set
            {
                Set(ref _IsCheckDiagnosis, value);
                if (IsCheckDiagnosis || IsCheckDrug)
                {
                    IsEnableRemed = true;
                }
                else
                {
                    IsEnableRemed = false;
                }
            }
        }

        private bool _IsCheckDrug;

        public bool IsCheckDrug
        {
            get { return _IsCheckDrug; }
            set
            {
                Set(ref _IsCheckDrug, value);
                if (IsCheckDiagnosis || IsCheckDrug)
                {
                    IsEnableRemed = true;
                }
                else
                {
                    IsEnableRemed = false;
                }
            }
        }

        private bool _IsEnableDiagnosis;

        public bool IsEnableDiagnosis
        {
            get { return _IsEnableDiagnosis; }
            set { Set(ref _IsEnableDiagnosis, value); }
        }

        private bool _IsEnableDrug;

        public bool IsEnableDrug
        {
            get { return _IsEnableDrug; }
            set { Set(ref _IsEnableDrug, value); }
        }


        private bool _IsEnableRemed = false;

        public bool IsEnableRemed
        {
            get { return _IsEnableRemed; }
            set { Set(ref _IsEnableRemed, value); }
        }


        private ResultComponentModel _SelectResultComponent;

        public ResultComponentModel SelectResultComponent
        {
            get { return _SelectResultComponent; }
            set { Set(ref _SelectResultComponent, value); }
        }

        #endregion

        #region Command

        private RelayCommand _RemedicalCommand;


        public RelayCommand RemedicalCommand
        {
            get
            {
                return _RemedicalCommand
                    ?? (_RemedicalCommand = new RelayCommand(Remedical));
            }
        }



        private RelayCommand _PrintOPDCommand;


        public RelayCommand PrintOPDCommand
        {
            get
            {
                return _PrintOPDCommand
                    ?? (_PrintOPDCommand = new RelayCommand(PrintOPD));
            }
        }


        private RelayCommand _CancelCommand;


        public RelayCommand CancelCommand
        {
            get
            {
                return _CancelCommand
                    ?? (_CancelCommand = new RelayCommand(Cancel));
            }
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

        #region Method
        int REGST = 417;
        int CHKOUT = 418;
        int SNDDOC = 419;
        int FINDIS = 421;
        private void GetLastVitalSign()
        {
            if (SelectPastVisit != null)
            {
                List<PatientVitalSignModel> vitalSign = DataService.PatientHistory.GetPatientVitalSignByVisitUID(SelectPastVisit.PatientVisitUID);
                if (vitalSign != null && vitalSign.Count > 0)
                {
                    var lastVitalSign = vitalSign.OrderByDescending(p => p.CWhen).FirstOrDefault();
                    if (lastVitalSign != null)
                    {
                        VitalSignFormat = "อุณหภูมิ " + (!string.IsNullOrEmpty(lastVitalSign.Temprature.ToString()) ? lastVitalSign.Temprature.ToString() : "N/A") + " องศาเซลเซียส ,";
                        VitalSignFormat += "ชีพจร " + (!string.IsNullOrEmpty(lastVitalSign.Pulse.ToString()) ? lastVitalSign.Pulse.ToString() : "N/A") + " ครั้ง/นาที , ";
                        VitalSignFormat += "อัตราการหายใจ " + (!string.IsNullOrEmpty(lastVitalSign.RespiratoryRate.ToString()) ? lastVitalSign.RespiratoryRate.ToString() : "N/A") + " ครั้ง/นาที  ,";
                        VitalSignFormat += "ความดันบน " + (!string.IsNullOrEmpty(lastVitalSign.BPSys.ToString()) ? lastVitalSign.BPSys.ToString() : "N/A") + " mm Hg , ";
                        VitalSignFormat += "ความดันล่าง " + (!string.IsNullOrEmpty(lastVitalSign.BPDio.ToString()) ? lastVitalSign.BPDio.ToString() : "N/A") + " mm Hg";
                    }
                }
                else
                {
                    VitalSignFormat = "No Record";
                }
            }


        }

        private void LoadCCHPI()
        {
            ListCCHPI = new ObservableCollection<CCHPIModel>();
            List<CCHPIModel> patCCHPIs = DataService.PatientHistory.GetCCHPIByVisit(SelectPastVisit.PatientVisitUID);
            if (patCCHPIs != null)
            {
                foreach (var patCCHPI in patCCHPIs.OrderByDescending(p => p.CWhen))
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

                    ListCCHPI.Add(patCCHPI);
                }
            }

            if (ListCCHPI != null && ListCCHPI.Count > 0)
            {
                var cchpi = ListCCHPI.OrderBy(p => p.CWhen).FirstOrDefault();
                ChiftComplain = !string.IsNullOrEmpty(cchpi.DisplayComplaint) ? cchpi.DisplayComplaint : "No Record";
                Presentillness = !string.IsNullOrEmpty(cchpi.Presentillness) ? cchpi.Presentillness : "No Record";
            }
            else
            {
                ChiftComplain = "No Record";
                Presentillness = "No Record";
            }
        }

        private void LoadProgressNote()
        {
            ListNote = new ObservableCollection<ProgressNoteModel>(DataService.PatientHistory.GetProgressNoteByVisit(SelectPastVisit.PatientVisitUID));
        }

        private void LoadPhyPE()
        {
            List<PhysicalExamModel> patPhyExams = DataService.PatientHistory.GetPhysicalExamByVisit(SelectPastVisit.PatientVisitUID);

            if (patPhyExams != null && patPhyExams.Count > 0)
            {
                if (this.View is PastMedicalHistory)
                {
                    RichTextBox rtb = (this.View as PastMedicalHistory).rtbeditor;
                    if (patPhyExams != null)
                    {
                        MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(patPhyExams.OrderBy(p => p.CWhen).FirstOrDefault().Value));
                        TextRange range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                        range.Load(stream, DataFormats.Rtf);
                    }
                    else
                    {
                        rtb.Document = new FlowDocument();
                    }
                }

            }

        }

        private void LoadPatientAllergy()
        {
            if (ListAllergy == null)
            {
                ListAllergy = DataService.PatientIdentity.GetPatientAllergyByPatientUID(SelectPastVisit.PatientUID);
            }
            AllergyFormat = "";
            if (ListAllergy != null && ListAllergy.Count > 0)
            {
                foreach (var item in ListAllergy)
                {
                    AllergyFormat += item.AllergicTo + ",";
                }
                if (AllergyFormat != "")
                {
                    AllergyFormat = AllergyFormat.Remove(AllergyFormat.LastIndexOf(","), 1);
                }
            }
            else
            {
                AllergyFormat = "No Record";
            }
        }

        private void LoadDianosis()
        {
            ListDiagnosis = DataService.PatientDiagnosis.GetPatientProblemByVisitUID(SelectPastVisit.PatientVisitUID);

            if (ListDiagnosis != null && ListDiagnosis.Count > 0)
                IsEnableDiagnosis = true;
            else
                IsEnableDiagnosis = false;
        }

        private void LoadPatientOrder()
        {
            var orderAll = DataService.OrderProcessing.GetOrderAllByVisitUID(SelectPastVisit.PatientVisitUID);
            orderAll = orderAll.Where(p => p.ORDSTUID != 2848).ToList();
            ListOrder = orderAll.Where(p => p.IdentifyingType.ToUpper() != "DRUG").ToList();
            ListDrugProfile = orderAll.Where(p => p.IdentifyingType.ToUpper() == "DRUG" && p.ORDSTUID != 2848).ToList();

            if (ListDrugProfile != null && ListDrugProfile.Count > 0)
                IsEnableDrug = true;
            else
                IsEnableDrug = false;
        }

        private void LoadRaiologyResult()
        {
            ListRadiologyResult = DataService.Radiology.GetResultRadiologyByVisitUID(SelectPastVisit.PatientVisitUID);
        }

        private void LoadLabResult()
        {
            bool Permission = RoleIsConfidential();
            LabResultDetails = DataService.Lab.GetResultLabGroupRequestNumber(SelectPastVisit.PatientVisitUID);
            foreach (var item in LabResultDetails.ToList())
            {
                if (item.IsConfidential == "Y")
                {
                    if (Permission != true)
                    {
                        LabResultDetails.Remove(item);
                    }
                }
            }
        }
        public void AssingPatientVisit(PatientVisitModel visitModel)
        {
            SelectPatientVisit = visitModel;

            var visitData = DataService.PatientIdentity.GetPatientVisitByPatientUID(SelectPatientVisit.PatientUID);
            PastVisitList = visitData.Where(p => p.VISTSUID == 421 || p.VISTSUID == 418).ToList();
        }

        private void Remedical()
        {
            try
            {
                bool isRemedical = false;
                if (SelectPatientVisit.VISTSUID == CHKOUT || SelectPatientVisit.VISTSUID == FINDIS)
                {
                    WarningDialog("ไม่สามารถดำเนินการได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
                    return;
                }

                if (IsCheckDiagnosis)
                {
                    isRemedical = true;
                }

                if (IsCheckDrug)
                {
                    isRemedical = true;
                }

                if (isRemedical)
                {
                    CloseViewDialog(ActionDialog.Save);
                }
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }
        }

        private void PrintOPD()
        {
            if (SelectPastVisit != null)
            {
                OPDCard report = new OPDCard();
                ReportPrintTool printTool = new ReportPrintTool(report);
                report.Parameters["PatientUID"].Value = SelectPatientVisit.PatientUID;
                report.Parameters["PatientVisitUID"].Value = SelectPatientVisit.PatientVisitUID;
                report.RequestParameters = false;
                report.ShowPrintMarginsWarning = false;
                printTool.ShowPreviewDialog();
            }

        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
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
    }
}
