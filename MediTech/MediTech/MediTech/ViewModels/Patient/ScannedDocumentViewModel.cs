using GalaSoft.MvvmLight.Command;
using MediTech.Helpers;
using MediTech.Model;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using WIA;

namespace MediTech.ViewModels
{
    public class ScannedDocumentViewModel : MediTechViewModelBase
    {

        #region Preperties
        private PatientVisitModel _SelectPatientVisit;

        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set { Set(ref _SelectPatientVisit, value); }
        }


        private bool _UploadAttachment;

        public bool UploadAttachment
        {
            get { return _UploadAttachment; }
            set
            {
                Set(ref _UploadAttachment, value);
                if (UploadAttachment)
                {
                    labelDocumentName = "Choose Document";
                    btnChooseDocument = "Choose";
                }
            }
        }

        private bool _ScanDocument;

        public bool ScanDocument
        {
            get { return _ScanDocument; }
            set
            {
                Set(ref _ScanDocument, value);
                if (ScanDocument)
                {
                    labelDocumentName = "Document Name";
                    btnChooseDocument = "Scan";
                }
            }
        }

        private string _labelDocumentName;

        public string labelDocumentName
        {
            get { return _labelDocumentName; }
            set { Set(ref _labelDocumentName, value); }
        }

        private string _btnChooseDocument;

        public string btnChooseDocument
        {
            get { return _btnChooseDocument; }
            set { Set(ref _btnChooseDocument, value); }
        }

        private string _DocumentName;

        public string DocumentName
        {
            get { return _DocumentName; }
            set { Set(ref _DocumentName, value); }
        }

        private List<LookupReferenceValueModel> _DocumentTypes;

        public List<LookupReferenceValueModel> DocumentTypes
        {
            get { return _DocumentTypes; }
            set { Set(ref _DocumentTypes, value); }
        }

        private LookupReferenceValueModel _SelectDocumentType;

        public LookupReferenceValueModel SelectDocumentType
        {
            get { return _SelectDocumentType; }
            set { Set(ref _SelectDocumentType, value); }
        }

        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { Set(ref _Comments, value); }
        }

        private ObservableCollection<PatientScannedDocumentModel> _ScannedDocuments;

        public ObservableCollection<PatientScannedDocumentModel> ScannedDocuments
        {
            get { return _ScannedDocuments; }
            set { Set(ref _ScannedDocuments, value); }
        }

        private PatientScannedDocumentModel _SelectScannedDocuments;

        public PatientScannedDocumentModel SelectScannedDocuments
        {
            get { return _SelectScannedDocuments; }
            set
            {
                Set(ref _SelectScannedDocuments, value);
            }
        }


        private byte[] _FileUploedCotent;

        public byte[] FileUploedCotent
        {
            get { return _FileUploedCotent; }
            set { Set(ref _FileUploedCotent, value); }
        }

        private BitmapImage _ImagePreview;
        public BitmapImage ImagePreview
        {
            get
            {
                return _ImagePreview;
            }
            set
            {
                Set(ref _ImagePreview, value);
            }
        }

        #endregion

        #region Command
        private RelayCommand _UploadDocumentCommand;

        public RelayCommand UploadDocumentCommand
        {
            get { return _UploadDocumentCommand ?? (_UploadDocumentCommand = new RelayCommand(UploadDocument)); }
        }

        private RelayCommand _ViewDocumentCommand;

        public RelayCommand ViewDocumentCommand
        {
            get { return _ViewDocumentCommand ?? (_ViewDocumentCommand = new RelayCommand(ViewDocument)); }
        }


        private RelayCommand _DeleteDocumentCommand;

        public RelayCommand DeleteDocumentCommand
        {
            get { return _DeleteDocumentCommand ?? (_DeleteDocumentCommand = new RelayCommand(DeleteDocument)); }
        }

        private RelayCommand _CloseCommand;

        public RelayCommand CloseCommand
        {
            get { return _CloseCommand ?? (_CloseCommand = new RelayCommand(Close)); }
        }

        private RelayCommand _ChooseScanCommand;

        public RelayCommand ChooseScanCommand
        {
            get { return _ChooseScanCommand ?? (_ChooseScanCommand = new RelayCommand(ChooseScan)); }
        }


        #endregion

        #region Method

        public ScannedDocumentViewModel()
        {
            UploadAttachment = true;
            DocumentTypes = DataService.Technical.GetReferenceValueMany("SCDTY");
            SelectDocumentType = DocumentTypes.FirstOrDefault();
        }

        void ChooseScan()
        {
            try
            {
                if (UploadAttachment)
                {
                    OpenFileDialog openDialog = new OpenFileDialog();
                    openDialog.Filter = @"Image files (*.bmp, *.gif, *.jpeg, *.jpg, *.png)|*.bmp; *.gif; *.jpeg; *.jpg; *.png
|PDF (*.pdf)|*.pdf|Html (*.htm,*.html)|*.htm;*.html|Documents (*.doc,*.docx)|*.doc;*.docx";
                    openDialog.ShowDialog();
                    if (openDialog.FileName.Trim() != "")
                    {
                        DocumentName = openDialog.SafeFileName.Trim();
                        FileUploedCotent = File.ReadAllBytes(openDialog.FileName);
                    }
                }
                else if (ScanDocument)
                {
                    if (String.IsNullOrEmpty(DocumentName))
                    {
                        WarningDialog("กรุณาระบุ DocumentName");
                        return;
                    }

                    var scanner = new ScannerService();

                    ImageFile file = scanner.Scan();
                    if (file != null)
                    {
                        var converter = new ScannerImageConverter();

                        WIA.Vector vector = file.FileData;

                        if (vector != null)
                        {
                            byte[] bytes = vector.get_BinaryData() as byte[];

                            if (bytes != null)
                            {
                                var msImage = new MemoryStream(bytes);
                                PdfDocument document = new PdfDocument();
                                PdfPage page = document.AddPage();
                                XGraphics gfx = XGraphics.FromPdfPage(page);
                                XImage image = XImage.FromStream(msImage);
                                gfx.DrawImage(image, 0, 0, (int)page.Width, (int)page.Height);
                                if (document.PageCount > 0)
                                {
                                    using (MemoryStream msPDF = new MemoryStream())
                                    {
                                        document.Save(msPDF);
                                        FileUploedCotent = msPDF.ToArray();
                                    }

                                }

                            }
                        }


                    }
                }

            }
            catch (ScannerException ex)
            {
                // yeah, I know. Showing UI from the VM. Shoot me now.
                ErrorDialog(ex.Message);
            }
            catch (Exception ex)
            {
                ErrorDialog(ex.Message);
            }

        }
        void UploadDocument()
        {
            try
            {
                if (String.IsNullOrEmpty(DocumentName))
                {
                    WarningDialog("กรุณาระบุ DocumentName");
                    return;
                }

                if (SelectDocumentType == null)
                {
                    WarningDialog("กรุณาเลือก Document Type");
                    return;
                }

                if (FileUploedCotent == null)
                {
                    WarningDialog("ไม่มีข้อมูลสำหรับ Uploaded");
                    return;
                }

                if (ScanDocument && !DocumentName.Contains("pdf"))
                {
                    DocumentName = DocumentName.Trim();
                    DocumentName = DocumentName + ".pdf";
                }

                PatientScannedDocumentModel patScanDoc = new PatientScannedDocumentModel();
                patScanDoc.PatientUID = SelectPatientVisit.PatientUID;
                patScanDoc.PatientVisitUID = SelectPatientVisit.PatientVisitUID;
                patScanDoc.DocumentName = DocumentName;
                patScanDoc.ScannedDocument = FileUploedCotent;
                patScanDoc.Comments = Comments;
                patScanDoc.DocUploadedDttm = DateTime.Now;
                patScanDoc.SCDTYUID = SelectDocumentType.Key.Value;
                DataService.PatientIdentity.UploadedScannedDocument(patScanDoc, AppUtil.Current.UserID);
                GetPatientScannedDocument();
                DocumentName = "";
                FileUploedCotent = null;
                Comments = "";
                SelectDocumentType = DocumentTypes.FirstOrDefault();
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }
        }

        void ViewDocument()
        {
            try
            {
                if (SelectScannedDocuments != null)
                {
                    string extension = Path.GetExtension(SelectScannedDocuments.DocumentName); // "pdf", etc
                    string filename = System.IO.Path.GetTempFileName() + extension; // Makes something like "C:\Temp\blah.tmp.pdf"
                    byte[] fileContent = DataService.PatientIdentity.GetPatientScannedDocumentContent(SelectScannedDocuments.PatientScannedDocumentUID);
                    if (fileContent != null)
                    {
                        File.WriteAllBytes(filename, fileContent);


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

                }

            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }
        }

        void DeleteDocument()
        {
            try
            {
                if (SelectScannedDocuments != null)
                {
                    MessageBoxResult result = QuestionDialog("คุณต้องการลบเอกสารนี้ ใช้หรือไม่ ?");
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.PatientIdentity.DeletePatientScannedDocument(SelectScannedDocuments.PatientScannedDocumentUID, AppUtil.Current.UserID);
                        GetPatientScannedDocument();
                    }
                }
            }
            catch (Exception ex)
            {


            }

        }

        void Close()
        {
            try
            {
                this.CloseViewDialog(ActionDialog.Cancel);
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }

        public void AssingPatientVisit(PatientVisitModel visitModel)
        {
            SelectPatientVisit = visitModel;
            GetPatientScannedDocument();
        }

        void GetPatientScannedDocument()
        {
            try
            {
                ScannedDocuments = new ObservableCollection<PatientScannedDocumentModel>
                    (DataService.PatientIdentity.GetPatientScannedDocumentByPatientUID(SelectPatientVisit.PatientUID));
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }

        #endregion
    }
}
