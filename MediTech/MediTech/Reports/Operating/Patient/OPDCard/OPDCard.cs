using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Model;
using System.Collections.Generic;
using MediTech.DataService;
using MediTech.Model.Report;
using System.Linq;
using System.Windows.Media.Imaging;
using System.IO;

namespace MediTech.Reports.Operating.Patient
{
    public partial class OPDCard : DevExpress.XtraReports.UI.XtraReport
    {
        List<OPDCardModel> listData;
        public OPDCard()
        {
            InitializeComponent();
            this.BeforePrint += OPDCard_BeforePrint;
            xrSubreport1.BeforePrint += xrSubreport1_BeforePrint;
            xrSubreport2.BeforePrint += xrSubreport2_BeforePrint;
        }


        void OPDCard_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            listData = new List<OPDCardModel>();
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long patientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long patientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            listData = (new ReportsService()).PrintOPDCard(patientUID, patientVisitUID);
            this.DataSource = listData;

            if(OrganisationUID == 24)
            {
                logo.Visible = false;
            }

            if (listData != null && listData.Count > 0)
            {
                string healthOrganisationCode = listData.FirstOrDefault().OrganisationCode;
                if (healthOrganisationCode.ToUpper().Contains("BRXG"))
                {
                    Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoBRXG.png", UriKind.Absolute);
                    BitmapImage imageSource = new BitmapImage(uri);
                    using (MemoryStream outStream = new MemoryStream())
                    {
                        BitmapEncoder enc = new BmpBitmapEncoder();
                        enc.Frames.Add(BitmapFrame.Create(imageSource));
                        enc.Save(outStream);
                        this.logo.Image = System.Drawing.Image.FromStream(outStream);
                        //this.lbOgenisation.Text = "บีอาร์เอ็กซ์จีสหคลินิค (BRXG Polyclinic)";

                    }
                }

                var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                if (Organisation != null)
                {
                    this.lbOgenisation.Text = Organisation.Description?.ToString();
                    string License = Organisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + Organisation.LicenseNo.ToString() : "";
                    lbFooterOrganisation.Text = Organisation.Description?.ToString()+ " " + License;

                    string mobile1 = Organisation.MobileNo != null ? "โทรศัพท์ " + Organisation.MobileNo.ToString() : "";
                    string mobile2 = Organisation.MobileNo != null ? "Tel. " + Organisation.MobileNo.ToString() : "";
                    string email = Organisation.Email != null ? "e-mail:" + Organisation.Email.ToString() : "";

                    lbAddress1.Text = Organisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2.Text = Organisation.Address2?.ToString() + " " + mobile2 + " " + email;
                    
                }
                else if (healthOrganisationCode.ToUpper().Contains("DRC"))
                {
                    Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoDRC.png", UriKind.Absolute);
                    BitmapImage imageSource = new BitmapImage(uri);
                    using (MemoryStream outStream = new MemoryStream())
                    {
                        BitmapEncoder enc = new BmpBitmapEncoder();
                        enc.Frames.Add(BitmapFrame.Create(imageSource));
                        enc.Save(outStream);
                        this.logo.Image = System.Drawing.Image.FromStream(outStream);
                        this.lbOgenisation.Text = "ดี อาร์ ซี คลินิกการพยาบาลและการผดุงครรภ์ เลขที่ 125/17 หมู่ 1 ต.สำนักทอง อ.เมื่อง จ.ระยอง 21000 โทร. 098-746-3622";
                    }
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
                    Image image = System.Drawing.Image.FromStream(outStream);
                    //this.lbOgenisation.Text = "บีอาร์เอ็กซ์จีสหคลินิค (BRXG Polyclinic)";
                    var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(AppUtil.Current.OwnerOrganisationUID);
                    this.lbOgenisation.Text = Organisation.Description?.ToString();
                    logo.Visible = false;

                    string License = Organisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + Organisation.LicenseNo.ToString() : "";
                    lbFooterOrganisation.Text = Organisation.Description?.ToString() + " " + License;

                    string mobile1 = Organisation.MobileNo != null ? "โทรศัพท์ " + Organisation.MobileNo.ToString() : "";
                    string mobile2 = Organisation.MobileNo != null ? "Tel. " + Organisation.MobileNo.ToString() : "";
                    string email = Organisation.Email != null ? "e-mail:" + Organisation.Email.ToString() : "";

                    lbAddress1.Text = Organisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2.Text = Organisation.Address2?.ToString() + " " + mobile2 + " " + email;
                }
            }

            //if (!String.IsNullOrEmpty(OrganisationUID.ToString()))
            //{
            //    var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
            //    if (Organisation != null)
            //    {
            //        lbOgenisation.Text = Organisation.Description.ToString();
            //    }
            //}
            //else
            //{
            //    var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(AppUtil.Current.OwnerOrganisationUID);
            //    if (Organisation != null)
            //    {
            //        lbOgenisation.Text = Organisation.Description.ToString();
                    
            //    }
            //}
        }
        void xrSubreport2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (listData != null)
            {
                foreach (var item in listData)
                {
                    ((XRSubreport)sender).ReportSource.DataSource = item.PatientDrugDetail;
                }

            }
        }

        void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (listData != null)
            {
                foreach (var item in listData)
                {
                    ((XRSubreport)sender).ReportSource.DataSource = item.PateintProblem;
                }

            }

        }
    }
}
