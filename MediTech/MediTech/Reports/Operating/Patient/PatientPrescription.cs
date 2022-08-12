using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Model;
using MediTech.DataService;
using MediTech.ViewModels;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.IO;
using System.Linq;
using DevExpress.XtraReports.Parameters;
using MediTech.Model.Report;

namespace MediTech.Reports.Operating.Patient
{
    public partial class PatientPrescription : DevExpress.XtraReports.UI.XtraReport
    {
        List<HealthOrganisationModel> Organisations = new List<HealthOrganisationModel>();
        List<PrescriptionItemModel> ListPrescriptions = new List<PrescriptionItemModel>();

        public PatientPrescription()
        {
            InitializeComponent();
            Organisations = (new MasterDataService()).GetHealthOrganisation();

            StaticListLookUpSettings lookupSettings = new StaticListLookUpSettings();
            foreach (var item in Organisations)
            {
                lookupSettings.LookUpValues.Add(new LookUpValue(item.HealthOrganisationUID, item.Name));
            }

            this.LogoType.LookUpSettings = lookupSettings;
            this.BeforePrint += PatientPrescription_BeforePrint;
         

        }

        private void PatientPrescription_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());
            int prescriptionUID = Convert.ToInt32(this.Parameters["PrescriptionUID"].Value.ToString());
            var prescription = (new PharmacyService()).GetprescriptionList(prescriptionUID);

            ListPrescriptions = prescription.FirstOrDefault().PrescriptionItems.ToList();

            if (ListPrescriptions != null && ListPrescriptions.Count != 0)
            {
                int i = 1;
                foreach (var item in ListPrescriptions)
                {
                    item.No = i;
                    i++;
                }
            }

            var dianosis = (new PatientDiagnosticsService()).GetPatientProblemByVisitUID(prescription.FirstOrDefault().PatientVisitUID);
            this.DataSource = prescription;
            prescription_supreport.ReportSource.DataSource = ListPrescriptions;
            prescription_supreport2.ReportSource.DataSource = ListPrescriptions;
            diagnosis_supreport.ReportSource.DataSource = dianosis;
            diagnosis_supreport2.ReportSource.DataSource = dianosis;

            var OrganisationBRXG = (new MasterDataService()).GetHealthOrganisationByUID(17);

            if (logoType == 0)
            {
                var OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                lbLicenseNo.Text = OrganisationDefault.Description?.ToString();
                if (OrganisationDefault.LicenseNo != null)
                {
                    lbLicenseNo.Text = lbLicenseNo.Text + " ใบอนุญาตเลขที่ " + OrganisationDefault.LicenseNo.ToString();
                    lbLicenseNo2.Text = lbLicenseNo.Text + " ใบอนุญาตเลขที่ " + OrganisationDefault.LicenseNo.ToString();
                }

                string mobile1 = OrganisationDefault.MobileNo != null ? "Tel. " + OrganisationDefault.MobileNo.ToString() : "";
                string email = OrganisationDefault.Email != null ? " e-mail :" + OrganisationDefault.Email.ToString() : "";

                Address.Text = OrganisationDefault.Address?.ToString();
                Address2.Text = OrganisationDefault.Address?.ToString();
                Tel.Text = mobile1 + email;
                Tel2.Text = mobile1 + email;
                if (OrganisationDefault.LogoImage != null)
                {
                    MemoryStream ms = new MemoryStream(OrganisationDefault.LogoImage);
                    logo1.Image = Image.FromStream(ms);
                    logo2.Image = Image.FromStream(ms);
                }
                else
                {
                    MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                    logo1.Image = Image.FromStream(ms);
                    logo2.Image = Image.FromStream(ms);
                }
            }
            else
            {
                var SelectOrganisation = (new MasterDataService()).GetHealthOrganisationByUID(logoType);
                if (SelectOrganisation != null)
                {
                    lbLicenseNo.Text = SelectOrganisation.Description?.ToString();
                    if (SelectOrganisation.LicenseNo != null)
                    {
                        lbLicenseNo.Text = lbLicenseNo.Text + " ใบอนุญาตเลขที่ " + SelectOrganisation.LicenseNo.ToString();
                        lbLicenseNo2.Text = lbLicenseNo.Text + " ใบอนุญาตเลขที่ " + SelectOrganisation.LicenseNo.ToString();
                    }
                    string mobile1 = SelectOrganisation.MobileNo != null ? "Tel. " + SelectOrganisation.MobileNo.ToString() : "";
                    string email = SelectOrganisation.Email != null ? " e-mail:" + SelectOrganisation.Email.ToString() : "";

                    Address.Text = SelectOrganisation.Address?.ToString();
                    Address2.Text = SelectOrganisation.Address?.ToString();
                    Tel.Text = mobile1 + email;
                    Tel2.Text = mobile1 + email;
                }

                if (SelectOrganisation.LogoImage != null)
                {
                    MemoryStream ms = new MemoryStream(SelectOrganisation.LogoImage);
                    logo1.Image = Image.FromStream(ms);
                    logo2.Image = Image.FromStream(ms);
                }

                else
                {
                    MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                    logo1.Image = Image.FromStream(ms);
                    logo2.Image = Image.FromStream(ms);
                }

            }
        }
    }
}
