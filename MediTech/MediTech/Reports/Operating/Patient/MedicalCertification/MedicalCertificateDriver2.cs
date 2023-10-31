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
    public partial class MedicalCertificateDriver2 : DevExpress.XtraReports.UI.XtraReport
    {
        List<HealthOrganisationModel> Organisations = new List<HealthOrganisationModel>();
        public MedicalCertificateDriver2()
        {
            InitializeComponent();
            Organisations = (new MasterDataService()).GetHealthOrganisation();

            StaticListLookUpSettings lookupSettings = new StaticListLookUpSettings();
            foreach (var item in Organisations)
            {
                lookupSettings.LookUpValues.Add(new LookUpValue(item.HealthOrganisationUID, item.Name));
            }

            this.LogoType.LookUpSettings = lookupSettings;
            this.BeforePrint += MedicalCertification_BeforePrint;
        }

        private void MedicalCertification_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            long PatientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            var medicalData = (new ReportsService()).PrintConfinedSpaceCertificate(PatientVisitUID);
            int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());
            var OrganisationBRXG = (new MasterDataService()).GetHealthOrganisationByUID(17);

            if (logoType == 0)
            {
                HealthOrganisationModel OrganisationDefault = new HealthOrganisationModel();
                OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(30);

                hospitalName.Text = OrganisationDefault.Description?.ToString() + " ใบอนุญาตเลขที่ " + OrganisationDefault.LicenseNo.ToString() ;
                lbHospital.Text = OrganisationDefault.Description?.ToString();
                lbLicenseNo.Text = OrganisationDefault.LicenseNo != null ? "ใบอนุญาตเลขที่ " + OrganisationDefault.LicenseNo.ToString() : "";
                 
                string mobile1 = OrganisationDefault.MobileNo != null ? " โทรศัพท์ " + OrganisationDefault.MobileNo.ToString() : "";
                string mobile2 = OrganisationDefault.MobileNo != null ? " Tel. " + OrganisationDefault.MobileNo.ToString() : "";
                string email = OrganisationDefault.Email != null ? " e-mail:" + OrganisationDefault.Email.ToString() : "";

                lbOgenisation.Text = OrganisationDefault.Address?.ToString() + " " + mobile1 + " " + email;
                //lbOgenisation2.Text = OrganisationDefault.Address2?.ToString() + " " + mobile2 + " " + email;


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
                    hospitalName.Text = SelectOrganisation.Description?.ToString() + " ใบอนุญาตเลขที่ " + SelectOrganisation.LicenseNo.ToString() ;
                    lbHospital.Text = SelectOrganisation.Description?.ToString();
                    lbLicenseNo.Text = SelectOrganisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + SelectOrganisation.LicenseNo.ToString() : "";

                    string mobile1 = SelectOrganisation.MobileNo != null ? " โทรศัพท์ " + SelectOrganisation.MobileNo.ToString() : "";
                    string mobile2 = SelectOrganisation.MobileNo != null ? " Tel. " + SelectOrganisation.MobileNo.ToString() : "";
                    string email = SelectOrganisation.Email != null ? " e-mail:" + SelectOrganisation.Email.ToString() : "";

                    lbOgenisation.Text = SelectOrganisation.Address?.ToString() + " " + mobile1 + " " + email;
                    //lbOgenisation2.Text = SelectOrganisation.Address2?.ToString() + " " + mobile2 + " " + email;

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

            if (medicalData != null)
            {
                if (medicalData.BPSys != null && medicalData.BPDio != null)
                {
                    lbBP.Text = medicalData.BPSys + "/" + medicalData.BPDio;
                }
                
                if (medicalData.IDCard != "")
                {
                    NationalID.Text = medicalData.IDCard;
                }
                else if(medicalData.PassportID != "")
                {
                    NationalID.Text = medicalData.PassportID;
                }

                if(medicalData.Gender == "หญิง (Female)")
                {
                    checkBoxFemale.Checked = true;
                }

                if (medicalData.Gender == "ชาย (Male)")
                {
                    checkBoxMale.Checked = true;
                }
            }

            this.DataSource = medicalData;
        }
    }
}
