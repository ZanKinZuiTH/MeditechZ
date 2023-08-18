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
    public partial class WorkPermitCertificate : DevExpress.XtraReports.UI.XtraReport
    {
        List<HealthOrganisationModel> Organisations = new List<HealthOrganisationModel>();

        public WorkPermitCertificate()
        {
            InitializeComponent();
            Organisations = (new MasterDataService()).GetHealthOrganisation();
            StaticListLookUpSettings lookupSettings = new StaticListLookUpSettings();
            foreach (var item in Organisations)
            {
                lookupSettings.LookUpValues.Add(new LookUpValue(item.HealthOrganisationUID, item.Name));
            }
            this.LogoType.LookUpSettings = lookupSettings;
            this.BeforePrint += MedicalCouncil5_BeforePrint;
        }

        private void MedicalCouncil5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var dataSource = (new ReportsService()).PrintConfinedSpaceCertificate(PatientVisitUID);
            int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());

            var OrganisationBRXG = (new MasterDataService()).GetHealthOrganisationByUID(30);

            if (logoType == 0)
            {
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

                lbOgenisation.Text = OrganisationDefault.Description?.ToString();
                lbOrganisationPlace.Text = OrganisationDefault.Description.ToString();
                lbLicenseNo.Text = OrganisationDefault.LicenseNo != null ? "ใบอนุญาตเลขที่ " + OrganisationDefault.LicenseNo.ToString() : "";
                lbFooterOrganisation.Text = lbOgenisation.Text + " " + lbLicenseNo.Text;

                string mobile1 = OrganisationDefault.MobileNo != null ? "โทรศัพท์ " + OrganisationDefault.MobileNo.ToString() : "";
                string mobile2 = OrganisationDefault.MobileNo != null ? "Tel. " + OrganisationDefault.MobileNo.ToString() : "";
                string email = OrganisationDefault.Email != null ? "e-mail:" + OrganisationDefault.Email.ToString() : "";

                lbAddress1.Text = OrganisationDefault.Address?.ToString() + " " + mobile1 + " " + email;
                lbAddress2.Text = OrganisationDefault.Address2?.ToString() + " " + mobile2 + " " + email;

                if (OrganisationDefault.LogoImage != null)
                {
                    MemoryStream ms = new MemoryStream(OrganisationDefault.LogoImage);
                    logo.Image = Image.FromStream(ms);
                    logoFooter.Image = Image.FromStream(ms);
                }
                else
                {
                    MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                    logo.Image = Image.FromStream(ms);
                    logoFooter.Image = Image.FromStream(ms);
                }
            }
            else
            {
                var SelectOrganisation = (new MasterDataService()).GetHealthOrganisationByUID(logoType);
                if (SelectOrganisation != null)
                {
                    lbOgenisation.Text = SelectOrganisation.Description?.ToString();
                    lbOrganisationPlace.Text = SelectOrganisation.Description.ToString();
                    lbLicenseNo.Text = SelectOrganisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + SelectOrganisation.LicenseNo.ToString() : "";
                    lbFooterOrganisation.Text = lbOgenisation.Text + " " + lbLicenseNo.Text;

                    string mobile1 = SelectOrganisation.MobileNo != null ? "โทรศัพท์ " + SelectOrganisation.MobileNo.ToString() : "";
                    string mobile2 = SelectOrganisation.MobileNo != null ? "Tel. " + SelectOrganisation.MobileNo.ToString() : "";
                    string email = SelectOrganisation.Email != null ? "e-mail:" + SelectOrganisation.Email.ToString() : "";

                    lbAddress1.Text = SelectOrganisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2.Text = SelectOrganisation.Address2?.ToString() + " " + mobile2 + " " + email;
                }

                if (SelectOrganisation.LogoImage != null)
                {
                    MemoryStream ms = new MemoryStream(SelectOrganisation.LogoImage);
                    logo.Image = Image.FromStream(ms);
                    logoFooter.Image = Image.FromStream(ms);
                }
                else
                {
                    MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                    logo.Image = Image.FromStream(ms);
                    logoFooter.Image = Image.FromStream(ms);
                }
            }

            this.DataSource = dataSource;
        }

    }
}
