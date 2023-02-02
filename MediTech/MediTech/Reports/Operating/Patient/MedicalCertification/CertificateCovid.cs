using DevExpress.Data.Filtering.Helpers;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using MediTech.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace MediTech.Reports.Operating.Patient
{
    public partial class CertificateCovid : DevExpress.XtraReports.UI.XtraReport
    {
        List<HealthOrganisationModel> Organisations = new List<HealthOrganisationModel>();

        public CertificateCovid()
        {
            InitializeComponent();
            Organisations = (new MasterDataService()).GetHealthOrganisation();
            StaticListLookUpSettings lookupSettings = new StaticListLookUpSettings();
            foreach (var item in Organisations)
            {
                lookupSettings.LookUpValues.Add(new LookUpValue(item.HealthOrganisationUID, item.Name));
            }

            this.LogoType.LookUpSettings = lookupSettings;
            this.BeforePrint += Covid_BeforePrint;
        }
        private void Covid_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var model = (new ReportsService()).PrintMedicalCertificate(PatientVisitUID);
            int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());
            var OrganisationBRXG = (new MasterDataService()).GetHealthOrganisationByUID(17);
            var OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
            int selectCovidTest = int.Parse(this.Parameters["CovidTest"].Value.ToString());
            string textType = "ไม่ค้างคืน";

            if (selectCovidTest == 0)
            {
                testResult.Text = "หาเชื้อ SARS-CoV-2 (COVID-19) ด้วยวิธี Rapid Test Antigen โดยทำการ Nasal Swab";
            }

            if (selectCovidTest == 1)
            {
                testResult.Text = "หาเชื้อ SARS-CoV-2 (COVID-19) ด้วยวิธี Rapid Test Antibody IgG/IgM โดยการเจาะเลือด";
            }

            if (selectCovidTest == 2)
            {
                testResult.Text = "หาเชื้อ SARS-CoV-2 (COVID-19) ด้วยวิธี RT-PCR โดยทำการ Nasopharyngeal and Throat Swab (NS/TS";
            }



            if (logoType == 0)
            {
                lbOgenisation.Text = OrganisationDefault.Description?.ToString();
                //lbOrganisationPlace.Text = OrganisationDefault.Description.ToString() != null ? "สถานที่ตรวจ " + OrganisationDefault.Description.ToString() : "";
                lbLicenseNo.Text = OrganisationDefault.LicenseNo != null ? "ใบอนุญาตเลขที่ " + OrganisationDefault.LicenseNo.ToString() : "";

                string mobile1 = OrganisationDefault.MobileNo != null ? "โทรศัพท์ " + OrganisationDefault.MobileNo.ToString() : "";
                string mobile2 = OrganisationDefault.MobileNo != null ? "Tel. " + OrganisationDefault.MobileNo.ToString() : "";
                string email = OrganisationDefault.Email != null ? "E-mail:" + OrganisationDefault.Email.ToString() : "";
                lbHeadAddress1.Text = OrganisationDefault.Address?.ToString() + " " + mobile1 + " " + email;
                lbHeadAddress2.Text = OrganisationDefault.Address2?.ToString() + " " + mobile2 + " " + email;

                lbFooterOrganisation.Text = lbOgenisation.Text + " " + lbLicenseNo.Text;
                // infoOrganisation.Text = OrganisationDefault.Description?.ToString();
                lbAddress1.Text = OrganisationDefault.Address?.ToString() + " " + mobile1 + " " + email;
                lbAddress2.Text = OrganisationDefault.Address2?.ToString() + " " + mobile2 + " " + email;
                if (OrganisationDefault.LogoImage != null && OrganisationDefault.HealthOrganisationUID != 27)
                {
                    MemoryStream ms = new MemoryStream(OrganisationDefault.LogoImage);
                    logo.Image = Image.FromStream(ms);

                }
                else if (OrganisationDefault.LogoImage != null && OrganisationDefault.HealthOrganisationUID == 27)
                {
                    MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                    logo.Image = Image.FromStream(ms);

                }
                else
                {
                    MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                    logo.Image = Image.FromStream(ms);

                }
                string name = OrganisationDefault.HealthOrganisationUID == 17 ? "บีอาร์เอ็กซ์จีสหคลินิก" : OrganisationDefault.Name?.ToString();
                TextboxPlace.Text = name + " ที่อยู่ " + OrganisationBRXG.Address?.ToString() + "\nเลขที่ใบอนุญาต " + OrganisationDefault.LicenseNo.ToString() + " เบอร์โทรศัพท์ " + OrganisationBRXG.MobileNo + " E-mail " + OrganisationBRXG.Email.ToString();

                if (OrganisationDefault.HOTYPUID == 3079)
                    textType = "ค้างคืน";

                text1.Text = name + " ได้รับอนุญาตประกอบกิจการสถานพยาบาลประเภท"+ textType +" เลขที่ใบอนุญาต " + OrganisationDefault.LicenseNo.ToString();
                text2.Text = "ตั้งอยู่ เลขที่ " + OrganisationDefault.Address?.ToString() + " เบอร์โทรติดต่อ " + OrganisationDefault.MobileNo;
            }
            else
            {
                var SelectOrganisation = (new MasterDataService()).GetHealthOrganisationByUID(logoType);
                if (SelectOrganisation != null)
                {
                    lbOgenisation.Text = SelectOrganisation.Description?.ToString();
                    lbLicenseNo.Text = SelectOrganisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + SelectOrganisation.LicenseNo.ToString() : "";
                    lbFooterOrganisation.Text = lbOgenisation.Text + " " + lbLicenseNo.Text;


                    string mobile1 = SelectOrganisation.MobileNo != null ? "โทรศัพท์ " + SelectOrganisation.MobileNo.ToString() : "";
                    string mobile2 = SelectOrganisation.MobileNo != null ? "Tel. " + SelectOrganisation.MobileNo.ToString() : "";
                    string email = SelectOrganisation.Email != null ? "E-mail:" + SelectOrganisation.Email.ToString() : "";


                    lbHeadAddress1.Text = SelectOrganisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbHeadAddress2.Text = SelectOrganisation.Address2?.ToString() + " " + mobile2 + " " + email;
                    lbAddress1.Text = SelectOrganisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2.Text = SelectOrganisation.Address2?.ToString() + " " + mobile2 + " " + email;

                }
                if (SelectOrganisation.LogoImage != null && SelectOrganisation.HealthOrganisationUID != 27)
                {
                    MemoryStream ms = new MemoryStream(SelectOrganisation.LogoImage);
                    logo.Image = Image.FromStream(ms);

                }
                else if (SelectOrganisation.LogoImage != null && SelectOrganisation.HealthOrganisationUID == 27)
                {
                    MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                    logo.Image = Image.FromStream(ms);

                }
                else
                {
                    MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                    logo.Image = Image.FromStream(ms);

                }
                string name = SelectOrganisation.HealthOrganisationUID == 17 ? "บีอาร์เอ็กซ์จีสหคลินิก" : SelectOrganisation.Name?.ToString();
                TextboxPlace.Text = name + " ที่อยู่ " + SelectOrganisation.Address?.ToString() + "\nเลขที่ใบอนุญาต " + SelectOrganisation.LicenseNo.ToString() + " เบอร์โทรศัพท์ " + SelectOrganisation.MobileNo + " E-mail " + SelectOrganisation.Email.ToString();
                
                if (SelectOrganisation.HOTYPUID == 3079)
                    textType = "ค้างคืน";

                text1.Text = name + " ได้รับอนุญาตประกอบกิจการสถานพยาบาลประเภท"+ textType +"  เลขที่ใบอนุญาต " + SelectOrganisation.LicenseNo.ToString();
                text2.Text = "ตั้งอยู่ เลขที่ " + SelectOrganisation.Address?.ToString() + " เบอร์โทรติดต่อ " + SelectOrganisation.MobileNo;
            }

            this.DataSource = model;
        }

        }
}
