using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using MediTech.Model;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using System.IO;

namespace MediTech.Reports.Operating.Patient
{
    public partial class MedicalCertificateEng2Parts : DevExpress.XtraReports.UI.XtraReport
    {
        List<HealthOrganisationModel> Organisations = new List<HealthOrganisationModel>();
        public MedicalCertificateEng2Parts()
        {
            InitializeComponent();
            Organisations = (new MasterDataService()).GetHealthOrganisation();

            StaticListLookUpSettings lookupSettings = new StaticListLookUpSettings();
            foreach (var item in Organisations)
            {
                lookupSettings.LookUpValues.Add(new LookUpValue(item.HealthOrganisationUID, item.Name));
            }
            this.LogoType.LookUpSettings = lookupSettings;
            this.BeforePrint += MedicalCertificateEng2Parts_BeforePrint;
        }

        private void MedicalCertificateEng2Parts_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var dataSource = (new ReportsService()).PrintConfinedSpaceCertificate(PatientVisitUID);
            int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());

            var OrganisationBRXG = (new MasterDataService()).GetHealthOrganisationByUID(17);
            if (logoType == 0)
            {
                var OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                if (OrganisationDefault != null)
                {
                    string license = OrganisationDefault.LicenseNo != null ? "license no. " + OrganisationDefault.LicenseNo.ToString() : "";
                    lbOgenisation.Text = OrganisationDefault.Description?.ToString() +" "+ license;
                    lbFooterOrganisation.Text = lbOgenisation.Text + " " + license;

                    string mobile1 = OrganisationDefault.MobileNo != null ? "โทรศัพท์ " + OrganisationDefault.MobileNo.ToString() : "";
                    string mobile2 = OrganisationDefault.MobileNo != null ? "Tel. " + OrganisationDefault.MobileNo.ToString() : "";
                    string email = OrganisationDefault.Email != null ? "e-mail:" + OrganisationDefault.Email.ToString() : "";

                    lbAddress1.Text = OrganisationDefault.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2.Text = OrganisationDefault.Address2?.ToString() + " " + mobile2 + " " + email;

                    infoOrganisation1.Text = OrganisationDefault.Description?.ToString();
                    infoOrganisation2.Text = OrganisationDefault.Description?.ToString();
                    infoAddress.Text = OrganisationDefault.Address2?.ToString();
                    
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
            }
            else
            {
                var SelectOrganisation = (new MasterDataService()).GetHealthOrganisationByUID(logoType);
                if (SelectOrganisation != null)
                {
                    string license = SelectOrganisation.LicenseNo != null ? "license no. " + SelectOrganisation.LicenseNo.ToString() : "";
                    lbOgenisation.Text = SelectOrganisation.Description?.ToString() + " " + license;
                    lbFooterOrganisation.Text = SelectOrganisation.Description?.ToString() + " " + license;

                    string mobile1 = SelectOrganisation.MobileNo != null ? "โทรศัพท์ " + SelectOrganisation.MobileNo.ToString() : "";
                    string mobile2 = SelectOrganisation.MobileNo != null ? "Tel. " + SelectOrganisation.MobileNo.ToString() : "";
                    string email = SelectOrganisation.Email != null ? "e-mail:" + SelectOrganisation.Email.ToString() : "";

                    lbAddress1.Text = SelectOrganisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2.Text = SelectOrganisation.Address2?.ToString() + " " + mobile2 + " " + email;

                    infoOrganisation1.Text = SelectOrganisation.Description?.ToString();
                    infoOrganisation2.Text = SelectOrganisation.Description?.ToString();
                    infoAddress.Text = SelectOrganisation.Address2?.ToString();
                    
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
            }

            this.DataSource = dataSource;
        }
    }
}
