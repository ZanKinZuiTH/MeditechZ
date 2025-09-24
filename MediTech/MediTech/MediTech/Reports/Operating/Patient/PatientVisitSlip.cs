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
    public partial class PatientVisitSlip : DevExpress.XtraReports.UI.XtraReport
    {
        List<HealthOrganisationModel> Organisations = new List<HealthOrganisationModel>();
        public PatientVisitSlip()
        {
            InitializeComponent();
            Organisations = (new MasterDataService()).GetHealthOrganisation();
            StaticListLookUpSettings lookupSettings = new StaticListLookUpSettings();
            foreach (var item in Organisations)
            {
                lookupSettings.LookUpValues.Add(new LookUpValue(item.HealthOrganisationUID, item.Name));
            }
            this.LogoType.LookUpSettings = lookupSettings;
            this.BeforePrint += PatientVisitSlip_BeforePrint;
        }
        private void PatientVisitSlip_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            int PatientVisitUID = int.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            int PatientUID = int.Parse(this.Parameters["PatientUID"].Value.ToString());
            var dataSource = (new PatientIdentityService()).GetPatientVisitByUID(PatientVisitUID);
            var patient = (new PatientIdentityService()).GetPatientByUID(PatientUID);

            patientId.Text = patient.PatientID;
            age.Text = patient.AgeString;
            gender.Text = patient.Gender;
            birthDate.Text = patient.BirthDttmString;
            var data = (new PatientIdentityService()).GetPatientAllergyByPatientUID(PatientUID);
            if (data != null)
            {
                var myList = new List<string>();

                foreach (var item in data)
                {
                    myList.Add(item.AllergyDescription);
                }
                var result = string.Join(",", myList);

                Allergytxt.Text = result;
            }

            int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());
            var OrganisationBRXG = (new MasterDataService()).GetHealthOrganisationByUID(17);
            if (logoType == 0)
            {
                var OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                if (OrganisationDefault != null)
                {
                    string organisation = OrganisationDefault.Description?.ToString();
                    string lisence = OrganisationDefault.LicenseNo != null ? "ใบอนุญาตเลขที่ " + OrganisationDefault.LicenseNo.ToString() : "";
                    lbOrganisation.Text = organisation + " " + lisence;

                    string mobile1 = OrganisationDefault.MobileNo != null ? "โทรศัพท์ " + OrganisationDefault.MobileNo.ToString() : "";
                    string mobile2 = OrganisationDefault.MobileNo != null ? "Tel. " + OrganisationDefault.MobileNo.ToString() : "";
                    string email = OrganisationDefault.Email != null ? "e-mail:" + OrganisationDefault.Email.ToString() : "";

                    lbAddress.Text = OrganisationDefault.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2.Text = OrganisationDefault.Address2?.ToString() + " " + mobile2 + " " + email;

                    if (OrganisationDefault.LogoImage != null)
                    {
                        MemoryStream ms = new MemoryStream(OrganisationDefault.LogoImage);
                        logo.Image = Image.FromStream(ms);
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                        logo.Image = Image.FromStream(ms);
                    }
                }
            }
            else
            {
                var SelectOrganisation = (new MasterDataService()).GetHealthOrganisationByUID(logoType);
                if (SelectOrganisation != null)
                {
                    string organisation = SelectOrganisation.Description?.ToString();
                    string lisence = SelectOrganisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + SelectOrganisation.LicenseNo.ToString() : "";
                    lbOrganisation.Text = organisation + " " + lisence;

                    string mobile1 = SelectOrganisation.MobileNo != null ? "โทรศัพท์ " + SelectOrganisation.MobileNo.ToString() : "";
                    string mobile2 = SelectOrganisation.MobileNo != null ? "Tel. " + SelectOrganisation.MobileNo.ToString() : "";
                    string email = SelectOrganisation.Email != null ? "e-mail:" + SelectOrganisation.Email.ToString() : "";

                    lbAddress.Text = SelectOrganisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2.Text = SelectOrganisation.Address2?.ToString() + " " + mobile2 + " " + email;

                    if (SelectOrganisation.LogoImage != null)
                    {
                        MemoryStream ms = new MemoryStream(SelectOrganisation.LogoImage);
                        logo.Image = Image.FromStream(ms);
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                        logo.Image = Image.FromStream(ms);
                    }
                }
                
            }
            this.DataSource = dataSource;
        }
    }
}
