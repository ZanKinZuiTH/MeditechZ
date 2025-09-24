using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Model;
using MediTech.Model.Report;
using System.Linq;
using System.Collections.Generic;
using MediTech.DataService;
using System.IO;
using System.Windows.Media.Imaging;

namespace MediTech.Reports.Statistic.Checkup
{
    public partial class CheckupGroupCheckList : DevExpress.XtraReports.UI.XtraReport
    {
        MediTech.DataService.MediTechDataService dbService = new DataService.MediTechDataService();


        public CheckupGroupCheckList()
        {
            InitializeComponent();
            this.AfterPrint += CheckupSummary_AfterPrint;
            this.BeforePrint += CheckupSummary_BeforePrint;
        }

        private void CheckupSummary_AfterPrint(object sender, EventArgs e)
        {
            var dataList = (this.DataSource as List<CheckupSummaryModel>);

            if (dataList != null && dataList.Count > 0)
            {
                if (dataList.Count <= 24)
                {
                    CheckupGroupCheckListChart fChartSummary = new CheckupGroupCheckListChart();
                    string title = this.Parameters["Title"].Value.ToString();
                    string year = this.Parameters["Year"].Value.ToString();
                    fChartSummary.Parameters["LogoType"].Value = this.Parameters["LogoType"].Value;
                    fChartSummary.Parameters["Header"].Value = "กราฟแสดงผู้ที่มีผลการตรวจปกติ และผลผิดปกติประจำปี " + year
                        + Environment.NewLine + title;
                    fChartSummary.checkupChart.DataSource = dataList;
                    fChartSummary.CreateDocument();
                    this.Pages.AddRange(fChartSummary.Pages);
                }
                else
                {
                    int number = dataList.Count / 2;
                    for (int i = 0; i <= 1; i++)
                    {                      
                        List<CheckupSummaryModel> newDatList = new List<CheckupSummaryModel>();
                        if (i == 0)
                        {
                            for (int j = 0; j < number; j++)
                            {
                                CheckupSummaryModel dataRow = new CheckupSummaryModel();
                                dataRow = dataList[j];
                                newDatList.Add(dataRow);
                            }
                        }
                        else if (i == 1)
                        {
                            for (int j = number; j < dataList.Count; j++)
                            {
                                CheckupSummaryModel dataRow = new CheckupSummaryModel();
                                dataRow = dataList[j];
                                newDatList.Add(dataRow);
                            }

                        }
                        CheckupGroupCheckListChart fChartSummary = new CheckupGroupCheckListChart();
                        string title = this.Parameters["Title"].Value.ToString();
                        string year = this.Parameters["Year"].Value.ToString();
                        fChartSummary.Parameters["Header"].Value = "กราฟแสดงผู้ที่มีผลการตรวจปกติ และผลผิดปกติประจำปี " + year
                            + Environment.NewLine + title;
                        fChartSummary.checkupChart.DataSource = newDatList;
                        fChartSummary.CreateDocument();
                        this.Pages.AddRange(fChartSummary.Pages);
                    }
                }
            }




        }

        private void CheckupSummary_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
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


            int checkupJobUID = Convert.ToInt32(this.Parameters["CheckupJobUID"].Value);
            string companyName = this.Parameters["CompanyName"].Value.ToString();
            string GPRSTUIDs = this.Parameters["GPRSTUIDs"].Value.ToString();
            string Year = this.Parameters["Year"].Value.ToString();
            string title = this.Parameters["Title"].Value.ToString();
            lbTitle.Text = title + Environment.NewLine + " โปรแกรมตรวจสุขภาพประจำปี " + Year;
            CheckupCompanyModel branchModel = new CheckupCompanyModel();
            branchModel.CheckupJobUID = checkupJobUID;
            branchModel.GPRSTUIDs = GPRSTUIDs;
            branchModel.CompanyName = companyName;
            if (this.Parameters["DateFrom"].Value != null)
            {
                branchModel.DateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value); ;
            }
            if (this.Parameters["DateTo"].Value != null)
            {
                branchModel.DateTo= Convert.ToDateTime(this.Parameters["DateTo"].Value); ;
            }
            var dataSummary = dbService.Reports.CheckupSummary(branchModel);
            this.DataSource = dataSummary;
        }
    }
}
