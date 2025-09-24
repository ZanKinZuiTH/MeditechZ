using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using MediTech.Reports.Operating.Patient.MedicalCertification;
using System.IO;
using MediTech.Model;

namespace MediTech.Reports.Operating.Patient
{
    public partial class CovidRapidTestCertification : DevExpress.XtraReports.UI.XtraReport
    {
        CovidRapidTestCertification2 page2 = new CovidRapidTestCertification2();
        public CovidRapidTestCertification()
        {
            InitializeComponent();
            this.BeforePrint += CovidRapidTest_BeforePrint;
            AfterPrint += Page2_AfterPrint;
        }
        private void CovidRapidTest_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var model = (new ReportsService()).PrintMedicalCertificate(PatientVisitUID);
            int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());
            var OrganisationBRXG = (new MasterDataService()).GetHealthOrganisationByUID(17);
           // var OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
            HealthOrganisationModel OrganisationDefault = new HealthOrganisationModel();
            if (OrganisationUID == 17)
            {
                var visitType = new PatientIdentityService().GetPatientVisitByUID(PatientVisitUID);
                if (visitType.VISTYUID == 4867)
                {
                    OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(30);
                }
                else
                {
                    OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                }
            }
            else
            {
                OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
            }

            if (model != null)
            {
                this.lbIdCard.Text = model.PassportID != null ? model.IDCard + "/" + model.PassportID : model.IDCard;
                page2.lbStartDate1.Text = model.strVisitData?.ToString("dd'/'MM'/'yyyy");
                page2.lbDoctor.Text = model.Doctor;
                page2.lbDoctorLicense.Text = model.DoctorLicenseNo;
                page2.lbSignDoctor.Text = model.Doctor;
                page2.lbSignPatient.Text = model.PatientName;
                page2.lbPatientName.Text = model.PatientName;
            }

            if (logoType == 0)
            {
                lbOgenisation.Text = OrganisationDefault.Description?.ToString();
                //lbOrganisationPlace.Text = OrganisationDefault.Description.ToString() != null ? "สถานที่ตรวจ " + OrganisationDefault.Description.ToString() : "";
                lbLicenseNo.Text = OrganisationDefault.LicenseNo != null ? "ใบอนุญาตเลขที่ " + OrganisationDefault.LicenseNo.ToString() : "";
               
                string mobile1 = OrganisationDefault.MobileNo != null ? "โทรศัพท์ " + OrganisationDefault.MobileNo.ToString() : "";
                string mobile2 = OrganisationDefault.MobileNo != null ? "Tel. " + OrganisationDefault.MobileNo.ToString() : "";
                string email = OrganisationDefault.Email != null ? "e-mail:" + OrganisationDefault.Email.ToString() : "";
                lbHeadAddress1.Text = OrganisationDefault.Address?.ToString() + " " + mobile1 + " " + email;

               // infoOrganisation.Text = OrganisationDefault.Description?.ToString();

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
            }
            else
            {
                var SelectOrganisation = (new MasterDataService()).GetHealthOrganisationByUID(logoType);
                if (SelectOrganisation != null)
                {
                    lbOgenisation.Text = SelectOrganisation.Description?.ToString();
                    //lbOrganisationPlace.Text = SelectOrganisation.Description.ToString() != null ? "สถานที่ตรวจ " + SelectOrganisation.Description.ToString() : "";
                    lbLicenseNo.Text = SelectOrganisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + SelectOrganisation.LicenseNo.ToString() : "";
                 
                 

                    string mobile1 = SelectOrganisation.MobileNo != null ? "โทรศัพท์ " + SelectOrganisation.MobileNo.ToString() : "";
                    string mobile2 = SelectOrganisation.MobileNo != null ? "Tel. " + SelectOrganisation.MobileNo.ToString() : "";
                    string email = SelectOrganisation.Email != null ? "e-mail:" + SelectOrganisation.Email.ToString() : "";


                    lbHeadAddress1.Text = SelectOrganisation.Address?.ToString() + " " + mobile1 + " " + email;
               

                    //infoOrganisation.Text = SelectOrganisation.Description?.ToString();
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
            }

            this.DataSource = model;
        }
        private void Page2_AfterPrint(object sender, EventArgs e)
        {
            page2.RequestParameters = false;
            page2.Parameters["LogoType"].Value = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());
            page2.Parameters["OrganisationUID"].Value = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            page2.CreateDocument();

            this.Pages.AddRange(page2.Pages);
        }
    }
}
