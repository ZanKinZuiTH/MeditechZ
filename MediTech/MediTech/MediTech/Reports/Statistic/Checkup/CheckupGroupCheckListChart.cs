using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using MediTech.Model;
using System.IO;
using System.Windows.Media.Imaging;

namespace MediTech.Reports.Statistic.Checkup
{
    public partial class CheckupGroupCheckListChart : DevExpress.XtraReports.UI.XtraReport
    {
        public CheckupGroupCheckListChart()
        {
            InitializeComponent();
            this.BeforePrint += CheckupGroupBase_BeforePrint;
        }

        private void CheckupGroupBase_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());
            HealthOrganisationModel Organisation = new HealthOrganisationModel();

            if (logoType == 0)
            {
                Organisation = (new MasterDataService()).GetHealthOrganisationByUID(30);

                MemoryStream ms = new MemoryStream(Organisation.LogoImage);
                xrPictureBox1.Image = Image.FromStream(ms);
                this.xrPictureBox1.SizeF = new System.Drawing.SizeF(220.4585F, 70.16669F);
                this.xrPictureBox1.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleCenter;
                this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
                //this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(26.17F, 30.67F);

                xrLabel1.Text = Organisation.Description.ToString() + "\r\n" + Organisation.Address.ToString() + "\r\n" +
                   Organisation.Email.ToString() + " Tel " + Organisation.MobileNo.ToString() + "\r\n" + "เลขที่ใบอนุญาต " + Organisation.LicenseNo.ToString();
                //this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(860.17F, 20.67F);
            }

            if (logoType == 1)
            {
                Organisation = (new MasterDataService()).GetHealthOrganisationByUID(17);

                MemoryStream ms = new MemoryStream(Organisation.LogoImage);
                xrPictureBox1.Image = Image.FromStream(ms);
                this.xrPictureBox1.SizeF = new System.Drawing.SizeF(220.4585F, 70.16669F);
                this.xrPictureBox1.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleCenter;
                this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
                //this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(26.17F, 30.67F);

                xrLabel1.Text = Organisation.Description.ToString() + "\r\n" + Organisation.Address.ToString() + "\r\n" +
                   Organisation.Email.ToString() + " Tel " + Organisation.MobileNo.ToString() + "\r\n" + "เลขที่ใบอนุญาต " + Organisation.LicenseNo.ToString();
                //this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(860.17F, 20.67F);
            }

            if (logoType == 2)
            {
                Organisation = (new MasterDataService()).GetHealthOrganisationByUID(30);

                Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoBRXG3.png", UriKind.Absolute);
                BitmapImage imageSource = new BitmapImage(uri);
                using (MemoryStream outStream = new MemoryStream())
                {
                    BitmapEncoder enc = new BmpBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(imageSource));
                    enc.Save(outStream);
                    this.xrPictureBox1.Image = System.Drawing.Image.FromStream(outStream);
                }

                this.xrPictureBox1.SizeF = new System.Drawing.SizeF(220.4585F, 70.16669F);
                this.xrPictureBox1.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleCenter;
                this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
                //this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(26.17F, 30.67F);

                xrLabel1.Text = Organisation.Description.ToString() + "\r\n" + Organisation.Address.ToString() + "\r\n" +
                   Organisation.Email.ToString() + " Tel " + Organisation.MobileNo.ToString() + "\r\n" + "เลขที่ใบอนุญาต " + Organisation.LicenseNo.ToString();
                //this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(860.17F, 20.67F);
            }
        }

    }
}
