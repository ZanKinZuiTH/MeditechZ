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
    public partial class MedicalCertificateEng2 : DevExpress.XtraReports.UI.XtraReport
    {
        List<HealthOrganisationModel> Organisations = new List<HealthOrganisationModel>();
        public MedicalCertificateEng2()
        {
            InitializeComponent();
            Organisations = (new MasterDataService()).GetHealthOrganisation();
            StaticListLookUpSettings lookupSettings = new StaticListLookUpSettings();
            foreach (var item in Organisations)
            {
                lookupSettings.LookUpValues.Add(new LookUpValue(item.HealthOrganisationUID, item.Name));
            }

            this.LogoType.LookUpSettings = lookupSettings;
            this.BeforePrint += MedicalCertificateEng2_BeforePrint;
        }

        private void MedicalCertificateEng2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var model = (new ReportsService()).PrintConfinedSpaceCertificate(PatientVisitUID);
            int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());
            var OrganisationBRXG = (new MasterDataService()).GetHealthOrganisationByUID(30);
            //var OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
            HealthOrganisationModel OrganisationDefault = new HealthOrganisationModel();
            if (OrganisationUID == 17)
            {
                OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(30);
            }
            else
            {
                OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
            }

            if ( model.BPSys != null && model.BPDio != null)
            {
                Bloodpressure.Text = model.BPSys.ToString() + "/" + model.BPDio.ToString();
            }
            
            model.DoctorLicenseNo = model.DoctorLicenseNo.Replace("ว.", string.Empty);

            if (logoType == 0)
            {
                lbOgenisation.Text = OrganisationDefault.Description?.ToString();
                lbLicenseNo.Text = OrganisationDefault.LicenseNo != null ? "License No " + OrganisationDefault.LicenseNo.ToString() : "";

                string mobile2 = OrganisationDefault.MobileNo != null ? "Tel. " + OrganisationDefault.MobileNo.ToString() : "";
                string email = OrganisationDefault.Email != null ? "E-mail:" + OrganisationDefault.Email.ToString() : "";
                lbHeadAddress2.Text = OrganisationDefault.Address2?.ToString() + " " + mobile2 + " " + email;
                ResidentialAddress.Text = OrganisationDefault.Address2?.ToString();
                HospitalPlace.Text = OrganisationDefault.HealthOrganisationUID == 17 ? OrganisationDefault.Name : OrganisationDefault.Description?.ToString();
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
                    lbLicenseNo.Text = SelectOrganisation.LicenseNo != null ? "License No " + SelectOrganisation.LicenseNo.ToString() : "";

                    string mobile2 = SelectOrganisation.MobileNo != null ? "Tel. " + SelectOrganisation.MobileNo.ToString() : "";
                    string email = SelectOrganisation.Email != null ? "E-mail:" + SelectOrganisation.Email.ToString() : "";
                    lbHeadAddress2.Text = SelectOrganisation.Address2?.ToString() + " " + mobile2 + " " + email;
                    ResidentialAddress.Text = SelectOrganisation.Address2?.ToString();
                    HospitalPlace.Text = SelectOrganisation.HealthOrganisationUID == 17 ? SelectOrganisation.Name : SelectOrganisation.Description?.ToString();
                }
                if (SelectOrganisation.LogoImage != null && SelectOrganisation.HealthOrganisationUID != 26)
                {
                    MemoryStream ms = new MemoryStream(SelectOrganisation.LogoImage);
                    logo.Image = Image.FromStream(ms);

                }
                else if (SelectOrganisation.LogoImage != null && SelectOrganisation.HealthOrganisationUID == 26)
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
       
    }
}
