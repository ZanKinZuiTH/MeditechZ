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
        List<PrescriptionModel> prescriptModels = new List<PrescriptionModel>();

        public PatientPrescription()
        {
            InitializeComponent();
           // Organisations = (new MasterDataService()).GetHealthOrganisation();
            StaticListLookUpSettings lookupSettings = new StaticListLookUpSettings();
           
                //lookupSettings.LookUpValues.Add(new LookUpValue(item.HealthOrganisationUID, item.Name));
                this.BeforePrint += PatientPrescription_BeforePrint;
         

           // this.LogoType.LookUpSettings = lookupSettings;
                
        }

        private void PatientPrescription_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            prescriptModels = new List<PrescriptionModel>();
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            //long patientVisitUID = int.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            int prescriptionItemUID = int.Parse(this.Parameters["prescriptionUID"].Value.ToString());
            prescriptModels = (new PharmacyService()).GetprescriptionList(prescriptionItemUID);
            var infopaitent = (new PatientIdentityService()).GetPatientVisitByUID(prescriptModels.FirstOrDefault().PatientVisitUID);
            this.DataSource = prescriptModels;


            if (prescriptModels != null && prescriptModels.Count > 0)
            {
                var SelectOrganisation = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                //var SelectOrganisation = (new MasterDataService()).GetHealthOrganisationByUID(logoType);
                this.lbOrganisation.Text = SelectOrganisation.Description?.ToString();
                this.lbOrganisationCopy.Text = lbOrganisation.Text;
                string License = SelectOrganisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + SelectOrganisation.LicenseNo.ToString() : "";
                //lbFooterOrganisation.Text = OrganisationBRXG.Description?.ToString() + " " + License;

                string mobile1 = SelectOrganisation.MobileNo != null ? "โทรศัพท์ " + SelectOrganisation.MobileNo.ToString() : "";
                string mobile2 = SelectOrganisation.MobileNo != null ? "Tel. " + SelectOrganisation.MobileNo.ToString() : "";
                string email = SelectOrganisation.Email != null ? "e-mail:" + SelectOrganisation.Email.ToString() : "";

                //lbAddress1.Text = SelectOrganisation.Address?.ToString() + " " + mobile1 + " " + email;
               // lbAddress2.Text = SelectOrganisation.Address2?.ToString() + " " + mobile2 + " " + email;

                if (SelectOrganisation.LogoImage != null)
                {
                    MemoryStream ms = new MemoryStream(SelectOrganisation.LogoImage);
                    logo1.Image = Image.FromStream(ms);
                    logo2.Image = Image.FromStream(ms);
                }
                else
                {
                    MemoryStream ms = new MemoryStream(SelectOrganisation.LogoImage);
                    logo1.Image = Image.FromStream(ms);
                    logo2.Image = Image.FromStream(ms);
                }
            }

        }
    }
}
