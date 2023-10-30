using DevExpress.Xpf.Core;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Model.Report;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MediTech.Reports.Operating.Radiology
{
    public partial class ImagingReportA5 : DevExpress.XtraReports.UI.XtraReport
    {
        private string _LogoType;

        public string LogoType
        {
            get { return _LogoType; }
            set { _LogoType = value; }
        }

        List<HealthOrganisationModel> Organisations = new List<HealthOrganisationModel>();

        public ImagingReportA5()
        {
            InitializeComponent();
            Organisations = (new MasterDataService()).GetHealthOrganisation();
            StaticListLookUpSettings lookupSettings = new StaticListLookUpSettings();
            foreach (var item in Organisations)
            {
                lookupSettings.LookUpValues.Add(new LookUpValue(item.HealthOrganisationUID, item.Name));
            }

            this.logoHead.LookUpSettings = lookupSettings;
            this.BeforePrint += XrayReport_BeforePrint;
        }

        void XrayReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = AppUtil.Current.OwnerOrganisationUID;
            int logoType = Convert.ToInt32(this.Parameters["logoHead"].Value.ToString());
            var OrganisationBRXG = (new MasterDataService()).GetHealthOrganisationByUID(17);
            var OrganisationHospital = (new MasterDataService()).GetHealthOrganisationByUID(30);
            var OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);

            CultureInfo culture = new CultureInfo("en-US");
            PatientResultRadiology dataReport = (new ReportsService()).GetPatientResultRadiology(Convert.ToInt64(this.Parameters["ResultUID"].Value));
            if (dataReport != null)
            {
                if (dataReport.OrderStatus?.ToLower() == "completed")
                {
                    DXMessageBox.Show("ผล " + dataReport.PatientName + " ยังมีสถานะเป็น Completed ไม่สามารถพิมพ์ได้", "Warining", MessageBoxButton.OK, MessageBoxImage.Warning);
                    e.Cancel = true;
                }
                this.lblOrderName.Text = dataReport.RequestItemName;
                this.lblPatientName.Text = dataReport.PatientName;
                this.lblAge.Text = dataReport.Age != "" ? dataReport.Age + " Y" : "";
                this.lblSex.Text = dataReport.Gender;
                this.lblRegisterNo.Text = dataReport.HN;
                this.lblRequestDate.Text = dataReport.RequestedDttm.ToString("dd MMM yyyy HH:mm:ss", culture);
                this.lblReportDate.Text = dataReport.ResultEnteredDttm.Value.ToString("dd MMM yyyy HH:mm:ss", culture);
                this.lblDoctor.Text = dataReport.Doctor + "  " + dataReport.DoctorLicense;
                this.lblPatientOrder.Text = dataReport.RequestItemName;
                this.lblFREportDoc.Text = dataReport.Doctor;
                this.lblFReportDate.Text = dataReport.ResultEnteredDttm.Value.ToString("dd MMM yyyy HH:mm:ss", culture);

                if (logoType != 0)
                {
                    var data = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == logoType);

                    if (data.LogoImage != null)
                    {
                        MemoryStream ms = new MemoryStream(data.LogoImage);
                        logo.Image = Image.FromStream(ms);
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                        logo.Image = Image.FromStream(ms);
                    }

                    this.logo.LocationFloat = new DevExpress.Utils.PointFloat(38.54167F, 33.41668F);
                    this.logo.SizeF = new System.Drawing.SizeF(205.4585F, 64.16669F);
                    this.logo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;

                    this.lblOrganisationAddress.Text = data.Address;
                }
                else
                {
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
                        this.logo.LocationFloat = new DevExpress.Utils.PointFloat(75.4584F, 20.41668F);
                        this.logo.SizeF = new System.Drawing.SizeF(125.5F, 49.5F);
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
                        this.logo.LocationFloat = new DevExpress.Utils.PointFloat(75.4584F, 20.41668F);
                        this.logo.SizeF = new System.Drawing.SizeF(125.5F, 49.5F);
                        this.logo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
                    }
                    else if (LogoType.ToUpper() == "BRXG HOSPITAL")
                    {
                        Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoBRXGHospital.jpg", UriKind.Absolute);
                        BitmapImage imageSource = new BitmapImage(uri);
                        using (MemoryStream outStream = new MemoryStream())
                        {
                            BitmapEncoder enc = new BmpBitmapEncoder();
                            enc.Frames.Add(BitmapFrame.Create(imageSource));
                            enc.Save(outStream);
                            this.logo.Image = System.Drawing.Image.FromStream(outStream);
                        }
                        this.logo.LocationFloat = new DevExpress.Utils.PointFloat(75.54167F, 20.41668F);
                        this.logo.SizeF = new System.Drawing.SizeF(130.5F, 50.5F);
                        this.logo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;

                        this.lblOrganisationAddress.Text = OrganisationHospital.Address;
                    }
                    else if (LogoType.ToUpper() == "โรงพยาบาลพระยุพราช")
                    {
                        Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoHospital.jpg", UriKind.Absolute);
                        BitmapImage imageSource = new BitmapImage(uri);
                        using (MemoryStream outStream = new MemoryStream())
                        {
                            BitmapEncoder enc = new BmpBitmapEncoder();
                            enc.Frames.Add(BitmapFrame.Create(imageSource));
                            enc.Save(outStream);
                            this.logo.Image = System.Drawing.Image.FromStream(outStream);
                        }
                        this.logo.LocationFloat = new DevExpress.Utils.PointFloat(75.54F, 23F);
                        this.logo.SizeF = new System.Drawing.SizeF(64.83336F, 83.7084F);
                        this.logo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;

                        this.lblOrganisationAddress.Text = "155/196 หมู่ 2 ต.ทับมา อ.เมือง จ.ระยอง 21000 โทร. 033060399";
                        this.lblRegisterNo.Text = dataReport.OtherID;
                    }
                    else if (LogoType.ToUpper() == "แม่ฟ้าหลวง")
                    {
                        Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/logoMFU.jpg", UriKind.Absolute);
                        BitmapImage imageSource = new BitmapImage(uri);
                        using (MemoryStream outStream = new MemoryStream())
                        {
                            BitmapEncoder enc = new BmpBitmapEncoder();
                            enc.Frames.Add(BitmapFrame.Create(imageSource));
                            enc.Save(outStream);
                            this.logo.Image = System.Drawing.Image.FromStream(outStream);
                        }
                        this.logo.LocationFloat = new DevExpress.Utils.PointFloat(70.54F, 23F);
                        this.logo.SizeF = new System.Drawing.SizeF(64.83336F, 83.7084F);
                        this.logo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;

                        this.lblOrganisationAddress.Text = "ศูนย์บริการสุขภาพแบบครบวงจรแห่งภาคเหนือและอนุภูมิภาคลุ่มแม่น้ำโขง 333 ม.1 ต.ท่าสุด อ.เมือง จ.เชียงราย 57100 โทรศัพท์ 053 - 917556";

                    }
                    else if (LogoType.ToUpper() == "โรงพยาบาลบ้านแพ้ว")
                    {
                        Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/logoBGH.png", UriKind.Absolute);
                        BitmapImage imageSource = new BitmapImage(uri);
                        using (MemoryStream outStream = new MemoryStream())
                        {
                            BitmapEncoder enc = new BmpBitmapEncoder();
                            enc.Frames.Add(BitmapFrame.Create(imageSource));
                            enc.Save(outStream);
                            this.logo.Image = System.Drawing.Image.FromStream(outStream);
                        }
                        this.logo.LocationFloat = new DevExpress.Utils.PointFloat(65.54F, 33F);
                        this.logo.SizeF = new System.Drawing.SizeF(73.83336F, 70.7084F);
                        this.logo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;

                        this.lblOrganisationAddress.Text = "198 หมู่ 1 ถ.บ้านแพ้ว-พระประโทน ต.บ้านแพ้ว อ.บ้านแพ้ว จ.สมุทรสาคร 74120 โทร.034-419555 โทรสาร.034-419567 Email:bghhosp @gmail.com";
                    }
                    else
                    {
                        this.logo.Image = null;
                        this.lblOrganisationAddress.Text = "";
                    }
                }

                RichEditDocumentServer docServer = new RichEditDocumentServer();
                docServer.HtmlText = dataReport.ResultHtml;
                docServer.Document.DefaultParagraphProperties.LineSpacingType = ParagraphLineSpacing.Multiple;
                docServer.Document.DefaultParagraphProperties.LineSpacingMultiplier = 0.8f;
                this.RichTXT.Html = docServer.HtmlText;

                this.RichTXT.Font = new Font(RichTXT.Font.FontFamily, 12, System.Drawing.FontStyle.Bold);
                //this.RichTXT.Html = dataReport.ResultHtml;
                //this.lblThaiReult.Text = dataReport.ThaiResult;
            }

        }
    }
}
