using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Windows.Controls;
using DevExpress.XtraPrinting;
using MediTech.DataService;
using MediTech.Model.Report;
using MediTech.Reports.Operating.Patient.MedicalCertification;

namespace MediTech.Reports.Operating.Patient
{
    public partial class ConfinedSpaceCertificate1 : DevExpress.XtraReports.UI.XtraReport
    {
        ConfinedSpaceCertificate2 page2 = new ConfinedSpaceCertificate2();
        MedicalCertificateModel model = new MedicalCertificateModel();


        public ConfinedSpaceCertificate1()
        {
            InitializeComponent();
            BeforePrint += ConfinedSpaceCertificate1_BeforePrint;
            AfterPrint += Page2_AfterPrint;
        }

        private void ConfinedSpaceCertificate1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            model = (new ReportsService()).PrintConfinedSpaceCertificate(PatientVisitUID);
            
            if(model != null)
            {
                this.lbPatientName.Text = model.PatientName;
                this.lbIdCard.Text = model.IDCard;

                page2.lbDoctor.Text = model.Doctor;
                page2.lbDoctorNo.Text = model.DoctorLicenseNo;
                page2.lbPatientName.Text = model.PatientName;
                page2.lbDateNow.Text = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                page2.lbDate.Text = model.strVisitData?.ToString("dd'/'MM'/'yyyy");
                page2.lbWeight.Text = model.Weight.ToString();
                page2.lbHeight.Text = model.Height.ToString();
                page2.lbBMI.Text = model.BMI.ToString();
                page2.lbBP.Text = model.BPSys != null ? model.BPSys.ToString() + "/" + model.BPDio.ToString() : "";
                page2.lbPuls.Text = model.Pulse.ToString();
            }

            if (!String.IsNullOrEmpty(OrganisationUID.ToString()))
            {
                var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                if (Organisation != null)
                {
                    page2.lbOrganisationPlace.Text = Organisation.Description?.ToString();
                    string lbLicenseNo = Organisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + Organisation.LicenseNo.ToString() : "";

                    string mobile1 = Organisation.MobileNo != null ? "โทรศัพท์ " + Organisation.MobileNo.ToString() : "";
                    string mobile2 = Organisation.MobileNo != null ? "Tel. " + Organisation.MobileNo.ToString() : "";
                    string email = Organisation.Email != null ? "e-mail:" + Organisation.Email.ToString() : "";

                    lbFooterOrganisation.Text = page2.lbOrganisationPlace.Text + " " + lbLicenseNo;
                    page2.lbFooterOrganisation2.Text = page2.lbOrganisationPlace.Text + " " + lbLicenseNo;

                    lbAddressPage1.Text = Organisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2Page1.Text = Organisation.Address2?.ToString() + " " + mobile2 + " " + email;

                    page2.lbAddressPage2.Text = Organisation.Address?.ToString() + " " + mobile1 + " " + email;
                    page2.lbAddress2Page2.Text = Organisation.Address2?.ToString() + " " + mobile2 + " " + email;
                }
            }
            else
            {
                var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(AppUtil.Current.OwnerOrganisationUID);
                if (Organisation != null)
                {
                    page2.lbOrganisationPlace.Text = Organisation.Description?.ToString();
                    string lbLicenseNo = Organisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + Organisation.LicenseNo.ToString() : "";

                    string mobile1 = Organisation.MobileNo != null ? "โทรศัพท์ " + Organisation.MobileNo.ToString() : "";
                    string mobile2 = Organisation.MobileNo != null ? "Tel. " + Organisation.MobileNo.ToString() : "";
                    string email = Organisation.Email != null ? "e-mail:" + Organisation.Email.ToString() : "";

                    lbFooterOrganisation.Text = page2.lbOrganisationPlace.Text + " " + lbLicenseNo;
                    page2.lbFooterOrganisation2.Text = page2.lbOrganisationPlace.Text + " " + lbLicenseNo;

                    lbAddressPage1.Text = Organisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2Page1.Text = Organisation.Address2?.ToString() + " " + mobile2 + " " + email;

                    page2.lbAddressPage2.Text = Organisation.Address?.ToString() + " " + mobile1 + " " + email;
                    page2.lbAddress2Page2.Text = Organisation.Address2?.ToString() + " " + mobile2 + " " + email;
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
