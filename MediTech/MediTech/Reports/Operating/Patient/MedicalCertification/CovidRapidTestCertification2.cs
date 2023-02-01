using DevExpress.XtraReports.UI;
using MediTech.DataService;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace MediTech.Reports.Operating.Patient
{
    public partial class CovidRapidTestCertification2 : DevExpress.XtraReports.UI.XtraReport
    {
        public CovidRapidTestCertification2()
        {
            InitializeComponent();
            this.BeforePrint += CovidRapidTestCertification2_BeforePrint;
        }

        private void CovidRapidTestCertification2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());
            var OrganisationBRXG = (new MasterDataService()).GetHealthOrganisationByUID(30);
            var OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
            string textType = "ไม่ค้างคืน";

            if (this.Parameters["PatientVisitUID"].Value != null)
            {
                long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
                var model = (new ReportsService()).PrintMedicalCertificate(PatientVisitUID);

                if (model != null)
                {
                    lbStartDate1.Text = model.strVisitData?.ToString("dd'/'MM'/'yyyy");
                    lbDoctor.Text = model.Doctor;
                    lbDoctorLicense.Text = model.DoctorLicenseNo;
                    lbSignDoctor.Text = model.Doctor;
                    lbSignPatient.Text = model.PatientName;
                    lbPatientName.Text = model.PatientName;
                }
            }

            if (logoType == 0)
            {
                lbOgenisation.Text = OrganisationDefault.Description?.ToString();
                //lbOrganisationPlace.Text = OrganisationDefault.Description.ToString() != null ? "สถานที่ตรวจ " + OrganisationDefault.Description.ToString() : "";
                lbLicenseNo.Text = OrganisationDefault.LicenseNo != null ? "ใบอนุญาตเลขที่ " + OrganisationDefault.LicenseNo.ToString() : "";
                lbFooterOrganisation.Text = lbOgenisation.Text + " " + lbLicenseNo.Text;

                string mobile1 = OrganisationDefault.MobileNo != null ? "โทรศัพท์ " + OrganisationDefault.MobileNo.ToString() : "";
                string mobile2 = OrganisationDefault.MobileNo != null ? "Tel. " + OrganisationDefault.MobileNo.ToString() : "";
                string email = OrganisationDefault.Email != null ? "e-mail:" + OrganisationDefault.Email.ToString() : "";

                lbHeadAddress1.Text = OrganisationDefault.Address?.ToString() + " " + mobile1 + " " + email;
                lbHeadAddress2.Text = OrganisationDefault.Address2?.ToString() + " " + mobile2 + " " + email;
                lbAddress1.Text = OrganisationDefault.Address?.ToString() + " " + mobile1 + " " + email;
                lbAddress2.Text = OrganisationDefault.Address2?.ToString() + " " + mobile2 + " " + email;
                // infoOrganisation.Text = OrganisationDefault.Description?.ToString();

                if (OrganisationDefault.LogoImage != null && OrganisationDefault.HealthOrganisationUID != 27)
                {
                    MemoryStream ms = new MemoryStream(OrganisationDefault.LogoImage);
                    logo.Image = Image.FromStream(ms);
                    logoFooter.Image = Image.FromStream(ms);

                }
                else if (OrganisationDefault.LogoImage != null && OrganisationDefault.HealthOrganisationUID == 27)
                {
                    MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                    logo.Image = Image.FromStream(ms);
                    logoFooter.Image = Image.FromStream(ms);

                }
                else
                {
                    MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                    logo.Image = Image.FromStream(ms);
                    logoFooter.Image = Image.FromStream(ms);

                }

                string name = OrganisationDefault.HealthOrganisationUID == 17 ? "บีอาร์เอ็กซ์จีสหคลินิก" : OrganisationDefault.Name?.ToString();

                if (OrganisationDefault.HOTYPUID == 3079)
                    textType = "ค้างคืน";

                text1.Text = "ประกอบวิชาชีพเวชกรรมอยู่ที่ " + name + " ได้รับอนุญาตประกอบกิจการสถานพยาบาลประเภท"+ textType +" เลขที่ใบอนุญาต";
                text2.Text = OrganisationDefault.LicenseNo.ToString() + " ตั้งอยู่เลขที่ " + OrganisationDefault.Address?.ToString() + " เบอร์โทรติดต่อ " + OrganisationDefault.MobileNo;

            }
            else
            {
                var SelectOrganisation = (new MasterDataService()).GetHealthOrganisationByUID(logoType);
                if (SelectOrganisation != null)
                {
                    lbOgenisation.Text = SelectOrganisation.Description?.ToString();
                    //lbOrganisationPlace.Text = SelectOrganisation.Description.ToString() != null ? "สถานที่ตรวจ " + SelectOrganisation.Description.ToString() : "";
                    lbLicenseNo.Text = SelectOrganisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + SelectOrganisation.LicenseNo.ToString() : "";
                    lbFooterOrganisation.Text = lbOgenisation.Text + " " + lbLicenseNo.Text;



                    string mobile1 = SelectOrganisation.MobileNo != null ? "โทรศัพท์ " + SelectOrganisation.MobileNo.ToString() : "";
                    string mobile2 = SelectOrganisation.MobileNo != null ? "Tel. " + SelectOrganisation.MobileNo.ToString() : "";
                    string email = SelectOrganisation.Email != null ? "e-mail:" + SelectOrganisation.Email.ToString() : "";


                    lbHeadAddress1.Text = OrganisationDefault.Address?.ToString() + " " + mobile1 + " " + email;
                    lbHeadAddress2.Text = OrganisationDefault.Address2?.ToString() + " " + mobile2 + " " + email;
                    lbAddress1.Text = OrganisationDefault.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2.Text = OrganisationDefault.Address2?.ToString() + " " + mobile2 + " " + email;


                    //infoOrganisation.Text = SelectOrganisation.Description?.ToString();
                }
                if (SelectOrganisation.LogoImage != null && SelectOrganisation.HealthOrganisationUID != 27)
                {
                    MemoryStream ms = new MemoryStream(SelectOrganisation.LogoImage);
                    logo.Image = Image.FromStream(ms);
                    logoFooter.Image = Image.FromStream(ms);

                }
                else if (SelectOrganisation.LogoImage != null && SelectOrganisation.HealthOrganisationUID == 27)
                {
                    MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                    logo.Image = Image.FromStream(ms);
                    logoFooter.Image = Image.FromStream(ms);
                }
                else
                {
                    MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                    logo.Image = Image.FromStream(ms);
                    logoFooter.Image = Image.FromStream(ms);

                }
                string name = SelectOrganisation.HealthOrganisationUID == 17 ? "บีอาร์เอ็กซ์จีสหคลินิก" : SelectOrganisation.Name?.ToString();
                if (SelectOrganisation.HOTYPUID == 3079)
                    textType = "ค้างคืน";

                text1.Text = "ประกอบวิชาชีพเวชกรรมอยู่ที่ "+ name + " ได้รับอนุญาตประกอบกิจการสถานพยาบาลประเภท" + textType + " เลขที่ใบอนุญาต" ;
                text2.Text = SelectOrganisation.LicenseNo.ToString() + " ตั้งอยู่เลขที่ " + SelectOrganisation.Address?.ToString() + " เบอร์โทรติดต่อ " + SelectOrganisation.MobileNo;
            }

        }
    }
}
