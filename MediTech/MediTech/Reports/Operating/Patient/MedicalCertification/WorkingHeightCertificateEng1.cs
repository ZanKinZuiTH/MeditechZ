using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Reports.Operating.Patient.MedicalCertification;
using MediTech.Model.Report;
using MediTech.DataService;
using MediTech.Model;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using System.IO;

namespace MediTech.Reports.Operating.Patient
{
    public partial class WorkingHeightCertificateEng1 : DevExpress.XtraReports.UI.XtraReport
    {
        WorkingHeightCertificateEng2 page2 = new WorkingHeightCertificateEng2();
        MedicalCertificateModel model = new MedicalCertificateModel();
        List<HealthOrganisationModel> Organisations = new List<HealthOrganisationModel>();
        public WorkingHeightCertificateEng1()
        {
            InitializeComponent();
            Organisations = (new MasterDataService()).GetHealthOrganisation();

            StaticListLookUpSettings lookupSettings = new StaticListLookUpSettings();
            foreach (var item in Organisations)
            {
                lookupSettings.LookUpValues.Add(new LookUpValue(item.HealthOrganisationUID, item.Name));
            }
            this.LogoType.LookUpSettings = lookupSettings;
            BeforePrint += WorkingHeightCertificateEng1_BeforePrint;
            AfterPrint += Page2_AfterPrint;
        }
        private void WorkingHeightCertificateEng1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            model = (new ReportsService()).PrintConfinedSpaceCertificate(PatientVisitUID);
            int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());

            if (model != null)
            {
                this.lbPatientName.Text = model.PatientName.ToString();
                this.lbIdCard.Text = model.IDCard.ToString();

                page2.lbDoctor.Text = model.DoctorEngName;
                page2.lbDoctorNo.Text = model.DoctorLicenseNo;
                page2.lbDateNow.Text = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                page2.lbDate.Text = model.strVisitData?.ToString("dd'/'MM'/'yyyy");
                page2.lbWeight.Text = model.Weight.ToString();
                page2.lbHeight.Text = model.Height.ToString();
                page2.lbBMI.Text = model.BMI.ToString();
                page2.lbBP.Text = model.BPSys.ToString() + "/" + model.BPDio.ToString();
                page2.lbPulse.Text = model.Pulse.ToString();
            }

            var OrganisationBRXG = (new MasterDataService()).GetHealthOrganisationByUID(17);
            if (logoType == 0)
            {
                //var OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
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

                if (OrganisationDefault != null)
                {
                    string license = OrganisationDefault.LicenseNo != null ? "ใบอนุญาตเลขที่ " + OrganisationDefault.LicenseNo.ToString() : "";
                    string organisation = OrganisationDefault.Description?.ToString();

                    lbFooterOrganisation.Text = organisation + " " + license;
                    page2.lbFooterOrganisationPage2.Text = organisation + " " + license;

                    string mobile1 = OrganisationDefault.MobileNo != null ? "โทรศัพท์ " + OrganisationDefault.MobileNo.ToString() : "";
                    string mobile2 = OrganisationDefault.MobileNo != null ? "Tel. " + OrganisationDefault.MobileNo.ToString() : "";
                    string email = OrganisationDefault.Email != null ? "e-mail:" + OrganisationDefault.Email.ToString() : "";

                    lbAddressPage1.Text = OrganisationDefault.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2Page1.Text = OrganisationDefault.Address2?.ToString() + " " + mobile2 + " " + email;
                    page2.lbAddressPage2.Text = OrganisationDefault.Address?.ToString() + " " + mobile1 + " " + email;
                    page2.lbAddress2Page2.Text = OrganisationDefault.Address2?.ToString() + " " + mobile2 + " " + email;

                    if (OrganisationDefault.LogoImage != null)
                    {
                        MemoryStream ms = new MemoryStream(OrganisationDefault.LogoImage);
                        logo.Image = Image.FromStream(ms);
                        page2.logo2.Image = Image.FromStream(ms);
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                        logo.Image = Image.FromStream(ms);
                        page2.logo2.Image = Image.FromStream(ms);
                    }
                }
            }
            else
            {
                var SelectOrganisation = (new MasterDataService()).GetHealthOrganisationByUID(logoType);
                if (SelectOrganisation != null)
                {
                    string license = SelectOrganisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + SelectOrganisation.LicenseNo.ToString() : "";
                    string organisation = SelectOrganisation.Description?.ToString();

                    lbFooterOrganisation.Text = organisation + " " + license;
                    page2.lbFooterOrganisationPage2.Text = organisation + " " + license;

                    string mobile1 = SelectOrganisation.MobileNo != null ? "โทรศัพท์ " + SelectOrganisation.MobileNo.ToString() : "";
                    string mobile2 = SelectOrganisation.MobileNo != null ? "Tel. " + SelectOrganisation.MobileNo.ToString() : "";
                    string email = SelectOrganisation.Email != null ? "e-mail:" + SelectOrganisation.Email.ToString() : "";

                    lbAddressPage1.Text = SelectOrganisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2Page1.Text = SelectOrganisation.Address2?.ToString() + " " + mobile2 + " " + email;
                    page2.lbAddressPage2.Text = SelectOrganisation.Address?.ToString() + " " + mobile1 + " " + email;
                    page2.lbAddress2Page2.Text = SelectOrganisation.Address2?.ToString() + " " + mobile2 + " " + email;

                    if (SelectOrganisation.LogoImage != null)
                    {
                        MemoryStream ms = new MemoryStream(SelectOrganisation.LogoImage);
                        logo.Image = Image.FromStream(ms);
                        page2.logo2.Image = Image.FromStream(ms);
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                        logo.Image = Image.FromStream(ms);
                        page2.logo2.Image = Image.FromStream(ms);
                    }
                }
            }

        }
        private void Page2_AfterPrint(object sender, EventArgs e)
        {
            page2.CreateDocument();
            this.Pages.AddRange(page2.Pages);
        }
    }
}
