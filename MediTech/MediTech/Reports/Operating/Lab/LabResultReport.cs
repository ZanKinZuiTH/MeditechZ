using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using System.Linq;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows;

namespace MediTech.Reports.Operating.Lab
{
    public partial class LabResultReport : DevExpress.XtraReports.UI.XtraReport
    {
        public bool PrintAuto = false;
        public LabResultReport()
        {
            InitializeComponent();
            this.BeforePrint += LabResultReport_BeforePrint;
        }

        private void LabResultReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                long patientVisitUID = Convert.ToInt64(this.Parameters["PatientVisitUID"].Value.ToString());
                string requestNumber = this.Parameters["RequestNumber"].Value.ToString();
                var labResults = (new ReportsService()).GetLabResultByRequestNumber(patientVisitUID, requestNumber);


                if (labResults != null && labResults.Count > 0)
                {
                    var labResultNoImage = labResults.Where(p => p.ResultValueType != "Image");
                    if (labResultNoImage != null && labResultNoImage.Count() > 0)
                    {
                        this.DataSource = labResultNoImage;
                        string healthOrganisationCode = labResults.FirstOrDefault().OrganisationCode;
                        if (healthOrganisationCode.ToUpper().Contains("DRC"))
                        {
                            Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoDRC.png", UriKind.Absolute);
                            BitmapImage imageSource = new BitmapImage(uri);
                            using (MemoryStream outStream = new MemoryStream())
                            {
                                BitmapEncoder enc = new BmpBitmapEncoder();
                                enc.Frames.Add(BitmapFrame.Create(imageSource));
                                enc.Save(outStream);
                                this.logo1.Image = System.Drawing.Image.FromStream(outStream);
                            }
                        }
                        else
                        {
                            Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoBRXG.png", UriKind.Absolute);
                            BitmapImage imageSource = new BitmapImage(uri);
                            using (MemoryStream outStream = new MemoryStream())
                            {
                                BitmapEncoder enc = new BmpBitmapEncoder();
                                enc.Frames.Add(BitmapFrame.Create(imageSource));
                                enc.Save(outStream);
                                this.logo1.Image = System.Drawing.Image.FromStream(outStream);
                            }
                        }
                    }

                    //if (PrintAuto == false)
                    //{
                    var resultImageType = labResults.Where(p => p.ResultValueType == "Image");
                    if (resultImageType != null && resultImageType.Count() > 0)
                    {
                        LabDataService labDataServ = new LabDataService();
                        foreach (var item in resultImageType)
                        {
                            var imageData = labDataServ.GetResultImageByComponentUID(item.ResultComponentUID);
                            if (imageData != null)
                            {
                                string extension = Path.GetExtension(item.ResultValue); // "pdf", etc
                                string filename = System.IO.Path.GetTempFileName() + extension; // Makes something like "C:\Temp\blah.tmp.pdf"

                                File.WriteAllBytes(filename, imageData.ImageContent);


                                var process = new System.Diagnostics.Process();
                                process.StartInfo.FileName = filename;
                                process.StartInfo.Verb = "Open";
                                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                                process.EnableRaisingEvents = true;
                                process.Exited += delegate
                                {
                                    try
                                    {
                                        System.IO.File.Delete(filename);
                                    }
                                    catch (IOException)
                                    {
                                        //file is currently locked
                                    }

                                };
                                process.Start();
                            }

                        }
                        //}
                    }


                    if (labResultNoImage == null || labResultNoImage.Count() <= 0)
                    {
                        e.Cancel = true;
                        this.ClosePreview();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
