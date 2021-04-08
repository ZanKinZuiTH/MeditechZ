using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Globalization;
using MediTech.Model;
using MediTech.DataService;
using MediTech.Model.Report;
using System.Windows.Media.Imaging;
using System.IO;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;

namespace MediTech.Reports.Operating.Radiology
{
    public partial class ImagingReportThai : DevExpress.XtraReports.UI.XtraReport
    {
        private string _LogoType = "DRC";

        public string LogoType
        {
            get { return _LogoType; }
            set { _LogoType = value; }
        }
        public ImagingReportThai()
        {
            InitializeComponent();
            this.BeforePrint += XrayReport_BeforePrint;
        }



        void XrayReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            CultureInfo culture = new CultureInfo("en-US");
            PatientResultRadiology dataReport = (new ReportsService()).GetPatientResultRadiology(Convert.ToInt64(this.Parameters["ResultUID"].Value));
            if (dataReport != null)
            {
                this.lblOrderName.Text = dataReport.RequestItemName;
                this.lblPatientName.Text = dataReport.PatientName;
                this.lblAge.Text = dataReport.Age;
                this.lblSex.Text = dataReport.Gender;
                this.lblRegisterNo.Text = dataReport.HN;
                this.lblRequestDate.Text = dataReport.RequestedDttm.ToString("dd MMM yyyy HH:mm:ss", culture);
                this.lblReportDate.Text = dataReport.ResultEnteredDttm.Value.ToString("dd MMM yyyy HH:mm:ss", culture);
                this.lblDoctor.Text = dataReport.Doctor;
                this.lblPatientOrder.Text = dataReport.RequestItemName;
                this.lblFREportDoc.Text = dataReport.Doctor;
                this.lblFReportDate.Text = dataReport.ResultEnteredDttm.Value.ToString("dd MMM yyyy HH:mm:ss", culture);
                
                if (LogoType.ToUpper() == "BRXG POLYCLINIC")
                {
                    Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoBRXG.png", UriKind.Absolute);
                    BitmapImage imageSource = new BitmapImage(uri);
                    using (MemoryStream outStream = new MemoryStream())
                    {
                        BitmapEncoder enc = new BmpBitmapEncoder();
                        enc.Frames.Add(BitmapFrame.Create(imageSource));
                        enc.Save(outStream);
                        this.logo.Image = System.Drawing.Image.FromStream(outStream);
                    }
                    this.logo.LocationFloat = new DevExpress.Utils.PointFloat(36.4584F, 33.41668F);
                    this.logo.SizeF = new System.Drawing.SizeF(205.4585F, 64.16669F);
                    this.logo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;

                    this.lblOrganisationAddress.Text = "155/196 หมู่ 2 ต.ทับมา อ.เมือง จ.ระยอง 21000 โทร. 033060399";
                }
                else if (LogoType.ToUpper() == "BRXG")
                {
                    Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoBRXG4.jpg", UriKind.Absolute);
                    BitmapImage imageSource = new BitmapImage(uri);
                    using (MemoryStream outStream = new MemoryStream())
                    {
                        BitmapEncoder enc = new BmpBitmapEncoder();
                        enc.Frames.Add(BitmapFrame.Create(imageSource));
                        enc.Save(outStream);
                        this.logo.Image = System.Drawing.Image.FromStream(outStream);
                    }
                    this.logo.LocationFloat = new DevExpress.Utils.PointFloat(36.4584F, 33.41668F);
                    this.logo.SizeF = new System.Drawing.SizeF(205.4585F, 64.16669F);
                    this.logo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
                }
                else if (LogoType.ToUpper() == "DRC")
                {
                    Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoDRC.png", UriKind.Absolute);
                    BitmapImage imageSource = new BitmapImage(uri);
                    using (MemoryStream outStream = new MemoryStream())
                    {
                        BitmapEncoder enc = new BmpBitmapEncoder();
                        enc.Frames.Add(BitmapFrame.Create(imageSource));
                        enc.Save(outStream);
                        this.logo.Image = System.Drawing.Image.FromStream(outStream);
                    }
                    this.logo.LocationFloat = new DevExpress.Utils.PointFloat(38.54167F, 33.41668F);
                    this.logo.SizeF = new System.Drawing.SizeF(119.7916F, 96.20834F);
                    this.logo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;

                    this.lblOrganisationAddress.Text = dataReport.OgranastionAddress;
                }
                else if (LogoType.ToUpper() == "โรงพยาบาลพระยุพราช")
                {
                    Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoHospital.png", UriKind.Absolute);
                    BitmapImage imageSource = new BitmapImage(uri);
                    using (MemoryStream outStream = new MemoryStream())
                    {
                        BitmapEncoder enc = new BmpBitmapEncoder();
                        enc.Frames.Add(BitmapFrame.Create(imageSource));
                        enc.Save(outStream);
                        this.logo.Image = System.Drawing.Image.FromStream(outStream);
                    }
                    this.logo.LocationFloat = new DevExpress.Utils.PointFloat(38.54F, 23F);
                    this.logo.SizeF = new System.Drawing.SizeF(95.83336F, 108.7084F);
                    this.logo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;

                    this.lblOrganisationAddress.Text = dataReport.OgranastionAddress;
                    this.lblRegisterNo.Text = dataReport.OtherID;
                }
                else
                {
                    this.logo.Image = null;
                    this.lblOrganisationAddress.Text = "";
                }

                RichEditDocumentServer docServer = new RichEditDocumentServer();
                docServer.HtmlText = dataReport.ResultHtml;
                docServer.Document.DefaultParagraphProperties.LineSpacingType = ParagraphLineSpacing.Multiple;
                docServer.Document.DefaultParagraphProperties.LineSpacingMultiplier = 0.8f;
                this.RichTXT.Html = docServer.HtmlText;

                //this.RichTXT.Html = dataReport.ResultHtml;
                //this.lblThaiReult.Text = dataReport.ThaiResult;
            }

        }

    }
}
