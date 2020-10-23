using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraReports.UI;
using MediTech.Reports.Operating.Radiology;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using MediTech.Helpers;
using System.IO;
using DevExpress.XtraRichEdit;

namespace MediTech.ViewModels
{
    public class ReviewRISResultViewModel : MediTechViewModelBase
    {

        #region Properties

        private PatientRequestReportModel _PatientRequest;

        public PatientRequestReportModel PatientRequest
        {
            get { return _PatientRequest; }
            set { Set(ref _PatientRequest, value); }
        }


        private List<CareproviderModel> _Radioligists;

        public List<CareproviderModel> Radioligists
        {
            get { return _Radioligists; }
            set { Set(ref _Radioligists, value); }
        }
        private CareproviderModel _SelectRaiologist;

        public CareproviderModel SelectRaiologist
        {
            get { return _SelectRaiologist; }
            set
            {
                Set(ref _SelectRaiologist, value);
            }
        }

        private List<LookupReferenceValueModel> _ResultStatus;

        public List<LookupReferenceValueModel> ResultStatus
        {
            get { return _ResultStatus; }
            set { Set(ref _ResultStatus, value); }
        }

        private LookupReferenceValueModel _SelectResultStatus;

        public LookupReferenceValueModel SelectResultStatus
        {
            get { return _SelectResultStatus; }
            set
            {
                Set(ref _SelectResultStatus, value);
                if (_SelectResultStatus.Display == "Abnormal")
                {
                    ReviewFontWeight = FontWeights.Bold;
                    ReviewColor = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    ReviewFontWeight = FontWeights.Normal;
                    ReviewColor = new SolidColorBrush(Colors.Black);
                }
            }
        }

        private PreviousResult _SelectPreviousResult;

        public PreviousResult SelectPreviousResult
        {
            get { return _SelectPreviousResult; }
            set { Set(ref _SelectPreviousResult, value); }
        }


        private List<ResultRadiologyTemplateModel> _ResultTemplate;

        public List<ResultRadiologyTemplateModel> ResultTemplate
        {
            get { return _ResultTemplate; }
            set
            {
                Set(ref _ResultTemplate, value);
                ResultPrimaryTemplate = ResultTemplate.Where(p => p.IsPrimary).OrderBy(p => p.ResultStatus).ToList();
            }
        }

        private ResultRadiologyTemplateModel _SelectResultTemplate;

        public ResultRadiologyTemplateModel SelectResultTemplate
        {
            get { return _SelectResultTemplate; }
            set { Set(ref _SelectResultTemplate, value); }
        }

        private List<ResultRadiologyTemplateModel> _ResultPrimaryTemplate;

        public List<ResultRadiologyTemplateModel> ResultPrimaryTemplate
        {
            get { return _ResultPrimaryTemplate; }
            set { Set(ref _ResultPrimaryTemplate, value); }
        }

        private bool _IsCheckPrint;

        public bool IsCheckPrint
        {
            get { return _IsCheckPrint; }
            set { Set(ref _IsCheckPrint, value); }
        }

        private bool? _IsCaseStudy;

        public bool? IsCaseStudy
        {
            get { return _IsCaseStudy; }
            set { Set(ref _IsCaseStudy, value); }
        }

        public Document Document { get; set; }

        public string ResultOrderStatus { get; set; }

        public string DoctorName { get; set; }

        public string ResultedStatus { get; set; }

        private string _TemplateName;

        public string TemplateName
        {
            get { return _TemplateName; }
            set { Set(ref _TemplateName, value); }
        }

        private SolidColorBrush _ReviewColor = new SolidColorBrush(Colors.Black);

        public SolidColorBrush ReviewColor
        {
            get { return _ReviewColor; }
            set { Set(ref _ReviewColor, value); }
        }

        private FontWeight _ReviewFontWeight;

        public FontWeight ReviewFontWeight
        {
            get { return _ReviewFontWeight; }
            set { Set(ref _ReviewFontWeight, value); }
        }

        private bool _FromRDUReview = false;

        public bool FromRDUReview
        {
            get { return _FromRDUReview; }
            set { _FromRDUReview = value; }
        }

        private bool _IsEnabledRadiologist = true;

        public bool IsEnabledRadiologist
        {
            get { return _IsEnabledRadiologist; }
            set { Set(ref _IsEnabledRadiologist, value); }
        }

        #endregion

        #region Command

        private RelayCommand _ReviewHistoryCommand;

        /// <summary>
        /// Gets the ReviewHistoryCommand.
        /// </summary>
        public RelayCommand ReviewHistoryCommand
        {
            get
            {
                return _ReviewHistoryCommand
                    ?? (_ReviewHistoryCommand = new RelayCommand(OpenReviewHistory));
            }
        }

        private RelayCommand _SaveCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                    ?? (_SaveCommand = new RelayCommand(SaveResult));
            }
        }

        private RelayCommand _ReviewCommand;

        /// <summary>
        /// Gets the ReviewCommand.
        /// </summary>
        public RelayCommand ReviewCommand
        {
            get
            {
                return _ReviewCommand
                    ?? (_ReviewCommand = new RelayCommand(ReviewResult));
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

        private RelayCommand _CopyTemplateCommand;

        /// <summary>
        /// Gets the CopyTemplateCommand.
        /// </summary>
        public RelayCommand CopyTemplateCommand
        {
            get
            {
                return _CopyTemplateCommand
                    ?? (_CopyTemplateCommand = new RelayCommand(CopyTemplate));
            }
        }

        private RelayCommand _EditTemplateCommand;

        /// <summary>
        /// Gets the EditTemplateCommand.
        /// </summary>
        public RelayCommand EditTemplateCommand
        {
            get
            {
                return _EditTemplateCommand
                    ?? (_EditTemplateCommand = new RelayCommand(EditTemplate));
            }
        }

        private RelayCommand _DeleteTemplateCommand;

        /// <summary>
        /// Gets the DeleteTemplateCommand.
        /// </summary>
        public RelayCommand DeleteTemplateCommand
        {
            get
            {
                return _DeleteTemplateCommand
                    ?? (_DeleteTemplateCommand = new RelayCommand(DeleteTemplate));
            }
        }

        private RelayCommand<bool> _AddTemplateCommand;

        /// <summary>
        /// Gets the AddTemplateCommand.
        /// </summary>
        public RelayCommand<bool> AddTemplateCommand
        {
            get
            {
                return _AddTemplateCommand
                    ?? (_AddTemplateCommand = new RelayCommand<bool>(AddTemplate));
            }
        }

        private RelayCommand _ViewOldResultCommand;

        /// <summary>
        /// Gets the ViewOldResultCommand.
        /// </summary>
        public RelayCommand ViewOldResultCommand
        {
            get
            {
                return _ViewOldResultCommand
                    ?? (_ViewOldResultCommand = new RelayCommand(ViewOldResult));
            }
        }

        private RelayCommand _CopyOldResultCommand;

        /// <summary>
        /// Gets the ViewOldResultCommand.
        /// </summary>
        public RelayCommand CopyOldResultCommand
        {
            get
            {
                return _CopyOldResultCommand
                    ?? (_CopyOldResultCommand = new RelayCommand(CopyOldResult));
            }
        }


        private RelayCommand _ViewPACSCommand;

        /// <summary>
        /// Gets the ViewPatientImageCommand.
        /// </summary>
        public RelayCommand ViewPACSCommand
        {
            get
            {
                return _ViewPACSCommand
                    ?? (_ViewPACSCommand = new RelayCommand(ViewPACS));
            }
        }

        private RelayCommand _ViewPreviousFilmCommand;

        /// <summary>
        /// Gets the ViewDicomImageCommand.
        /// </summary>
        public RelayCommand ViewPreviousFilmCommand
        {
            get
            {
                return _ViewPreviousFilmCommand
                    ?? (_ViewPreviousFilmCommand = new RelayCommand(ViewPreviousFilm));
            }
        }


        private RelayCommand _ViewRequestFromCommand;

        /// <summary>
        /// Gets the ViewRequestFromCommand.
        /// </summary>
        public RelayCommand ViewRequestFromCommand
        {
            get
            {
                return _ViewRequestFromCommand
                    ?? (_ViewRequestFromCommand = new RelayCommand(ViewRequestFrom));
            }
        }

        private RelayCommand _OpenPastPACSCommand;

        /// <summary>
        /// Gets the OpenPastPACSCommand.
        /// </summary>
        public RelayCommand OpenPastPACSCommand
        {
            get
            {
                return _OpenPastPACSCommand
                    ?? (_OpenPastPACSCommand = new RelayCommand(OpenPastPACS));
            }
        }

        private RelayCommand<int> _EnterPrimaryTemplateCommand;

        /// <summary>
        /// Gets the EnterPrimaryTemplate.
        /// </summary>
        public RelayCommand<int> EnterPrimaryTemplateCommand
        {
            get
            {
                return _EnterPrimaryTemplateCommand
                    ?? (_EnterPrimaryTemplateCommand = new RelayCommand<int>(EnterPrimaryTemplate));
            }
        }


        #endregion

        #region Method

        long patientUID;
        long patientVisitUID;
        long requestUID;
        long requestDetailUID;
        string comments;
        public ReviewRISResultViewModel()
        {

            var careproviderAll = DataService.UserManage.GetCareproviderAll();
            ResultStatus = DataService.Technical.GetReferenceValueList("RABSTS");
            Radioligists = careproviderAll.Where(p => p.IsRadiologist).ToList();

            //SelectResultStatus = ResultStatus.FirstOrDefault();

            SelectRaiologist = Radioligists.FirstOrDefault(p => p.CareproviderUID == AppUtil.Current.UserID);

            if ((AppUtil.Current.IsRadiologist ?? false) && (!AppUtil.Current.IsAdminRadiologist ?? false))
            {
                IsEnabledRadiologist = false;
            }
        }

        public override void OnLoaded()
        {
            (this.View as ReviewRISResult).PatientBanner.SetPatientBanner(patientUID, patientVisitUID);

            DataService.Radiology.UpdateRequestDetailToReviewing(PatientRequest.RequestDetailUID, AppUtil.Current.UserID);
            ResultTemplate = DataService.Radiology.GetResultRadiologyTemplateByReportEditor(PatientRequest.RIMTYPUID, PatientRequest.SEXXXUID, AppUtil.Current.UserID);


            //Document.Unit = DevExpress.Office.DocumentUnit.Centimeter;
            //Document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            //Document.Sections[0].Page.Height = 20f;
            //Document.Sections[0].Margins.Left = 0.5f;
            //Document.Sections[0].Margins.Top = 0.5f;
            //Document.Sections[0].Margins.Bottom = 0.5f;

            if (!FromRDUReview)
            {
                ViewPACS();
            }

        }

        private void OpenReviewHistory()
        {
            ReviewHistory reviewHis = new ReviewHistory();
            (reviewHis.DataContext as ReviewHistoryViewModel).RequestDetailUID = requestDetailUID;
            LaunchViewDialogNonPermiss(reviewHis, false);
        }

        public void SaveResult()
        {
            try
            {
                if (ValidateSave())
                {
                    return;
                }
                int userID = AppUtil.Current.UserID;
                int completStatus = 2845;
                ResultRadiologyModel resultradiology = PatientRequest.Result;
                AssingPropertiesToResultModel(ref resultradiology, completStatus);
                long resultUID = DataService.Radiology.SaveReviewResult(resultradiology, userID);
                //SaveSuccessDialog();

                if (IsCheckPrint)
                {
                    ImagingReport rpt = new ImagingReport();
                    ReportPrintTool printTool = new ReportPrintTool(rpt);


                    rpt.Parameters["ResultUID"].Value = resultUID;
                    rpt.RequestParameters = false;
                    rpt.ShowPrintMarginsWarning = false;
                    printTool.ShowPreviewDialog();
                }
                ResultOrderStatus = "Complete";
                DoctorName = SelectRaiologist.FullName;
                ResultedStatus = SelectResultStatus.Display;
                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        public void ReviewResult()
        {
            try
            {
                if (ValidateSave())
                {
                    return;
                }
                int userID = AppUtil.Current.UserID;
                int reviewStatus = 2863;
                ResultRadiologyModel resultradiology = PatientRequest.Result;
                AssingPropertiesToResultModel(ref resultradiology, reviewStatus);
                long resultUID = DataService.Radiology.SaveReviewResult(resultradiology, userID);
                //SaveSuccessDialog();

                if (IsCheckPrint)
                {
                    ImagingReport rpt = new ImagingReport();
                    ReportPrintTool printTool = new ReportPrintTool(rpt);


                    rpt.Parameters["ResultUID"].Value = resultUID;
                    rpt.RequestParameters = false;
                    rpt.ShowPrintMarginsWarning = false;
                    printTool.ShowPreviewDialog();
                }
                ResultOrderStatus = "Reviewed";
                DoctorName = SelectRaiologist.FullName;
                ResultedStatus = SelectResultStatus.Display;
                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void EnterPrimaryTemplate(int resultTemplateUID)
        {
            try
            {
                ResultRadiologyTemplateModel resultTemplate = DataService.Radiology.GetResultRaiologyTemplateByUID(resultTemplateUID);
                ResultRadiologyModel resultradiology = PatientRequest.Result;
                int userID = AppUtil.Current.UserID;
                DateTime now = DateTime.Now;
                int reviewStatus = 2863;
                ResultOrderStatus = "Reviewed";
                if (resultradiology == null)
                {
                    resultradiology = new ResultRadiologyModel();
                }

                resultradiology.ORDSTUID = reviewStatus;
                resultradiology.RequestDetailUID = PatientRequest.RequestDetailUID;
                resultradiology.ResultEnteredDttm = now;
                resultradiology.RABSTSUID = resultTemplate.RABSTSUID ?? SelectResultStatus.Key;
                resultradiology.RadiologistUID = SelectRaiologist.CareproviderUID;
                resultradiology.PatientUID = patientUID;
                resultradiology.PatientVisitUID = patientVisitUID;
                resultradiology.Value = resultTemplate.Value;
                resultradiology.PlainText = resultTemplate.PlainText;
                long resultUID = DataService.Radiology.SaveReviewResult(resultradiology, userID);
                SaveSuccessDialog();

                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        public bool ValidateSave()
        {
            bool flag = false;

            if (SelectRaiologist == null)
            {
                WarningDialog("กรุณาเลือก รังสีแพทย์");
                flag = true;
            }

            if (SelectResultStatus == null)
            {
                WarningDialog("กรุณาเลือก สถานะ");
                flag = true;
            }

            return flag;
        }
        private void CopyTemplate()
        {
            if (SelectResultTemplate != null)
            {
                ResultRadiologyTemplateModel resTemp = DataService.Radiology.GetResultRaiologyTemplateByUID(SelectResultTemplate.ResultRadiologyTemplateUID);
                if (resTemp != null)
                {
                    Document.HtmlText = resTemp.Value;

                    if (resTemp.RABSTSUID != null && resTemp.RABSTSUID != 0)
                        SelectResultStatus = ResultStatus.FirstOrDefault(p => p.Key == resTemp.RABSTSUID);

                    AdjustDocumentPage();
                }
            }
        }

        private void EditTemplate()
        {
            try
            {
                if (SelectResultStatus == null)
                {
                    WarningDialog("กรุณาเลือกสถานะ");
                    return;
                }

                if (SelectResultTemplate != null)
                {
                    MessageBoxResult result = QuestionDialog("ต้องการบันทึกการแก้ไข Template " + SelectResultTemplate.Name + " ใช้หรือไม่ ?");
                    if (result == MessageBoxResult.Yes)
                    {
                        string htmlText = Document.GetHtmlText(Document.Range, null);
                        string plainText = Document.GetText(Document.Range);
                        ResultRadiologyTemplateModel resultRadTemp = new ResultRadiologyTemplateModel();
                        resultRadTemp.ResultRadiologyTemplateUID = SelectResultTemplate.ResultRadiologyTemplateUID;
                        resultRadTemp.RABSTSUID = SelectResultStatus.Key;
                        resultRadTemp.Value = htmlText;
                        resultRadTemp.PlainText = plainText;
                        DataService.Radiology.UpdateResultRadiologyTemplate(resultRadTemp, AppUtil.Current.UserID);
                        SaveSuccessDialog();
                    }
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }


        }

        private void DeleteTemplate()
        {
            try
            {
                if (SelectResultTemplate != null)
                {
                    MessageBoxResult result = QuestionDialog("ต้องการลบ Template " + SelectResultTemplate.Name + " ใช้หรือไม่ ?");
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.Radiology.DeleteResultRadiologyTemplate(SelectResultTemplate.ResultRadiologyTemplateUID, AppUtil.Current.UserID);
                        DeleteSuccessDialog();
                        ResultTemplate.Remove(SelectResultTemplate);
                        SelectResultTemplate = null;
                        OnUpdateEvent();
                    }
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void AddTemplate(bool IsPublic)
        {
            try
            {
                if (string.IsNullOrEmpty(TemplateName))
                {
                    WarningDialog("กรุณาใส่ชื่อ Template");
                    return;
                }
                if (SelectResultStatus == null)
                {
                    WarningDialog("กรุณาเลือกสถานะให้ Template");
                    return;
                }

                int userID = AppUtil.Current.UserID;
                List<ResultRadiologyTemplateModel> ResultCurrent = DataService.Radiology.GetResultRadiologyTemplateByDoctor(userID);
                int DisplayOrder = (ResultCurrent.Max(p => p.DisplayOrder) ?? 0) + 1;
                int countPrimary = ResultCurrent.Where(p => p.IsPrimary == true).Count();
                string htmlText = Document.GetHtmlText(Document.Range, null);
                string plainText = Document.GetText(Document.Range);

                ResultRadiologyTemplateModel resultRadTemp = new ResultRadiologyTemplateModel();

                resultRadTemp.Name = TemplateName;
                resultRadTemp.Description = TemplateName;
                resultRadTemp.Value = htmlText;
                resultRadTemp.RIMTYPUID = PatientRequest.RIMTYPUID;
                resultRadTemp.RABSTSUID = SelectResultStatus.Key;
                resultRadTemp.PlainText = plainText;
                resultRadTemp.DisplayOrder = DisplayOrder;
                resultRadTemp.IsPublic = IsPublic;

                if (PatientRequest.RequestItemName.ToLower().Contains("ultrasound upper")
                    || PatientRequest.RequestItemName.ToLower().Contains("ultrasound whole"))
                {
                    resultRadTemp.SEXXXUID = PatientRequest.SEXXXUID;
                }

                if (countPrimary < 4)
                {
                    resultRadTemp.IsPrimary = true;
                }
                else
                {
                    resultRadTemp.IsPrimary = false;
                }

                int resultTemplateUID = DataService.Radiology.SaveResultRadiologyTemplate(resultRadTemp, userID);
                SaveSuccessDialog();
                resultRadTemp.ResultRadiologyTemplateUID = resultTemplateUID;
                ResultTemplate.Add(resultRadTemp);
                SelectResultTemplate = null;
                TemplateName = "";
                OnUpdateEvent();
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void ViewOldResult()
        {
            try
            {
                if (SelectPreviousResult != null)
                {
                    ImagingReport rpt = new ImagingReport();
                    ReportPrintTool printTool = new ReportPrintTool(rpt);


                    rpt.Parameters["ResultUID"].Value = SelectPreviousResult.ResultUID;
                    rpt.RequestParameters = false;
                    rpt.ShowPrintMarginsWarning = false;
                    printTool.ShowPreviewDialog();
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void CopyOldResult()
        {
            try
            {
                if (SelectPreviousResult != null)
                {
                    ResultRadiologyModel oldResult = DataService.Radiology.GetResultRadiologyByResultUID(SelectPreviousResult.ResultUID);
                    Document.HtmlText = oldResult.Value;

                    AdjustDocumentPage();
                }

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void OpenPastPACS()
        {
            if (SelectPreviousResult != null)
            {
                PACSWorkList pacs = new PACSWorkList();
                PACSWorkListViewModel pacsViewModel = (pacs.DataContext as PACSWorkListViewModel);
                pacsViewModel.PatientID = PatientRequest.PatientID;
                pacsViewModel.IsCheckedPeriod = true;
                pacsViewModel.DateFrom = null;
                pacsViewModel.DateTo = SelectPreviousResult.RequestedDttm;
                pacsViewModel.Modality = SelectPreviousResult.Modality;
                pacsViewModel.IsOpenFromExam = true;
                System.Windows.Window owner = (System.Windows.Window)(this.View as ReviewRISResult).Parent;
                LaunchViewShow(pacs, owner, "PACS", false, true);
            }

        }

        internal void ViewPACS()
        {
            string modlity;
            if (PatientRequest.Modality == "MG")
            {
                modlity = "'MG','US'";
            }
            else if (PatientRequest.Modality == "DX")
            {
                modlity = "'DX','CR'";
            }
            else
            {
                modlity = "'" + PatientRequest.Modality + "'";
            }

            var StudiesList = DataService.PACS.SearchPACSWorkList(null, PatientRequest.RequestedDttm,
                modlity, null, PatientRequest.PatientID, null, null, null, null, null);

            if (StudiesList != null && StudiesList.Count == 1)
            {
                string url = PACSHelper.GetPACSViewerStudyUrl(StudiesList.FirstOrDefault().StudyInstanceUID);
                PACSHelper.OpenPACSViewer(url);
            }
            else
            {

                PACSWorkList pacs = new PACSWorkList();
                PACSWorkListViewModel pacsViewModel = (pacs.DataContext as PACSWorkListViewModel);
                pacsViewModel.PatientID = PatientRequest.PatientID;
                pacsViewModel.DateFrom = null;
                pacsViewModel.DateTo = PatientRequest.RequestedDttm;
                pacsViewModel.IsCheckedPeriod = true;
                pacsViewModel.Modality = PatientRequest.Modality;
                pacsViewModel.StudiesList = StudiesList;
                System.Windows.Window owner = (System.Windows.Window)(this.View as ReviewRISResult).Parent;
                LaunchViewShow(pacs, owner, "PACS", false, true);
            }
        }

        internal void ViewPreviousFilm()
        {
            try
            {
                string url = PACSHelper.GetPACSViewerPatientUrl(PatientRequest.PatientID);
                PACSHelper.OpenPACSViewer(url);

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        internal void ViewRequestFrom()
        {
            RequestDetailDocumentModel requestFrom = DataService.Radiology.GetRequestDetailDocument(requestDetailUID);
            if (requestFrom != null)
            {
                string tempImage = Path.GetTempPath() + "\\Requestfrom";
                if (!Directory.Exists(tempImage))
                {
                    Directory.CreateDirectory(tempImage);
                }

                DirectoryInfo diTempMedia = new DirectoryInfo(tempImage);
                foreach (FileInfo file in diTempMedia.GetFiles())
                {
                    file.Delete();
                }
                string filePath = tempImage + "\\" + requestFrom.DocumentName + ".jpg";
                BitmapEncoder encoder = new PngBitmapEncoder();
                BitmapImage image = ImageHelpers.ConvertByteToBitmap(requestFrom.DocumentContent);
                encoder.Frames.Add(BitmapFrame.Create(image));

                using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                {
                    encoder.Save(fileStream);
                }

                Process.Start(filePath);
            }
        }

        public void AssignModel(long patientUID, long patientVisitUID, long requestUID, long requestDetailUID, string comments = null)
        {
            this.patientUID = patientUID;
            this.patientVisitUID = patientVisitUID;
            this.requestUID = requestUID;
            this.requestDetailUID = requestDetailUID;
            this.comments = comments;
            PatientRequest = DataService.Radiology.GetRequestForReview(patientUID, requestUID, requestDetailUID);

            if (PatientRequest.Result != null)
            {
                Document.HtmlText = PatientRequest.Result.Value;
                SelectResultStatus = ResultStatus.FirstOrDefault(p => p.Key == PatientRequest.Result.RABSTSUID);
                SelectRaiologist = Radioligists.FirstOrDefault(p => p.CareproviderUID == PatientRequest.Result.RadiologistUID);
                IsCaseStudy = PatientRequest.Result.IsCaseStudy;
            }
        }

        public void AssingPropertiesToResultModel(ref ResultRadiologyModel resultModel, int orderStatus)
        {
            DateTime now = DateTime.Now;
            string htmlText = Document.GetHtmlText(Document.Range, null);
            string plainText = Document.GetText(Document.Range);
            if (resultModel == null)
            {
                resultModel = new ResultRadiologyModel();
            }

            resultModel.ORDSTUID = orderStatus;
            resultModel.RequestDetailUID = PatientRequest.RequestDetailUID;
            resultModel.ResultEnteredDttm = now;
            resultModel.RABSTSUID = SelectResultStatus.Key;
            resultModel.RadiologistUID = SelectRaiologist.CareproviderUID;
            resultModel.Comments = comments;
            resultModel.PatientUID = patientUID;
            resultModel.PatientVisitUID = patientVisitUID;
            resultModel.Value = htmlText;
            resultModel.PlainText = plainText;
            resultModel.IsCaseStudy = IsCaseStudy;
        }

        public void AdjustDocumentPage()
        {

            Document.Unit = DevExpress.Office.DocumentUnit.Centimeter;
            Document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            Document.Sections[0].Page.Height = 20f;
            Document.Sections[0].Margins.Left = 0.5f;
            Document.Sections[0].Margins.Top = 0.5f;
            Document.Sections[0].Margins.Bottom = 0.5f;
        }

        public void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }
        #endregion
    }
}
