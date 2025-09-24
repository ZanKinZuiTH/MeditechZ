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
using MediTech.Model;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using System.IO;

namespace MediTech.Reports.Operating.Patient
{
    public partial class ConfinedSpaceCertificate1 : DevExpress.XtraReports.UI.XtraReport
    {
        ConfinedSpaceCertificate2 page2 = new ConfinedSpaceCertificate2();
        MedicalCertificateModel model = new MedicalCertificateModel();
        List<HealthOrganisationModel> Organisations = new List<HealthOrganisationModel>();

        public ConfinedSpaceCertificate1()
        {
            InitializeComponent();
            Organisations = (new MasterDataService()).GetHealthOrganisation();

            StaticListLookUpSettings lookupSettings = new StaticListLookUpSettings();
            foreach (var item in Organisations)
            {
                lookupSettings.LookUpValues.Add(new LookUpValue(item.HealthOrganisationUID, item.Name));
            }
            this.LogoType.LookUpSettings = lookupSettings;
            BeforePrint += ConfinedSpaceCertificate1_BeforePrint;
            AfterPrint += Page2_AfterPrint;
        }

        private void ConfinedSpaceCertificate1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            model = (new ReportsService()).PrintConfinedSpaceCertificate(PatientVisitUID);
            int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());
            string ReportName = this.Parameters["ReportName"].Value.ToString();

            if (model != null)
            {
                this.lbPatientName.Text = model.PatientName;
                //this.lbIdCard.Text = model.IDCard;
                this.lbIdCard.Text = model.PassportID != null ? model.IDCard + "/" + model.PassportID : model.IDCard;
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

                var exp = model.strVisitData.Value.AddMonths(6);

                if (ReportName == "ใบรับรองแพทย์ที่อับอากาศ (Mobile)")
                {
                    xrLabel44.Text = "หมายเหตุ ใบรับรองแพทย์ฉบับนี้ ให้ใช้ได้ ......................... เดือน นับตั้งแต่วันตรวจร่างกาย";
                    page2.expText.Text = "หมายเหตุ ใบรับรองแพทย์ฉบับนี้ ให้ใช้ได้ ......................... เดือน นับตั้งแต่วันตรวจร่างกาย";
                }
                else
                {
                    xrLabel44.Text = "หมายเหตุ ใบรับรองแพทย์ฉบับนี้ ให้ใช้ได้ 6 เดือน นับตั้งแต่วันที่ตรวจร่างกาย เอกสารนี้ใช้ได้ตั้งแต่ " + model.strVisitData?.ToString("dd'/'MM'/'yyyy") + " วันหมดอายุ " + exp.ToString("dd'/'MM'/'yyyy");
                    page2.expText.Text = "หมายเหตุ ใบรับรองแพทย์ฉบับนี้ ให้ใช้ได้ 6 เดือน นับตั้งแต่วันที่ตรวจร่างกาย เอกสารนี้ใช้ได้ตั้งแต่ " + model.strVisitData?.ToString("dd'/'MM'/'yyyy") + " วันหมดอายุ " + exp.ToString("dd'/'MM'/'yyyy");
                }
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
                    page2.lbOrganisationPlace.Text = OrganisationDefault.Description?.ToString();
                    string lbLicenseNo = OrganisationDefault.LicenseNo != null ? "ใบอนุญาตเลขที่ " + OrganisationDefault.LicenseNo.ToString() : "";

                    string mobile1 = OrganisationDefault.MobileNo != null ? "โทรศัพท์ " + OrganisationDefault.MobileNo.ToString() : "";
                    string mobile2 = OrganisationDefault.MobileNo != null ? "Tel. " + OrganisationDefault.MobileNo.ToString() : "";
                    string email = OrganisationDefault.Email != null ? "e-mail:" + OrganisationDefault.Email.ToString() : "";

                    lbFooterOrganisation.Text = page2.lbOrganisationPlace.Text + " " + lbLicenseNo;
                    page2.lbFooterOrganisation2.Text = page2.lbOrganisationPlace.Text + " " + lbLicenseNo;

                    lbAddressPage1.Text = OrganisationDefault.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2Page1.Text = OrganisationDefault.Address2?.ToString() + " " + mobile2 + " " + email;

                    page2.lbAddressPage2.Text = OrganisationDefault.Address?.ToString() + " " + mobile1 + " " + email;
                    page2.lbAddress2Page2.Text = OrganisationDefault.Address2?.ToString() + " " + mobile2 + " " + email;
                }
                if (OrganisationDefault.LogoImage != null)
                {
                    MemoryStream ms = new MemoryStream(OrganisationDefault.LogoImage);
                    logo1.Image = System.Drawing.Image.FromStream(ms);
                    page2.logo2.Image = System.Drawing.Image.FromStream(ms);
                }
                else
                {
                    MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                    logo1.Image = System.Drawing.Image.FromStream(ms);
                    page2.logo2.Image = System.Drawing.Image.FromStream(ms);
                }
            }
            else
            {
                var SelectOrganisation = (new MasterDataService()).GetHealthOrganisationByUID(logoType);
                if (SelectOrganisation != null)
                {
                    page2.lbOrganisationPlace.Text = SelectOrganisation.Description?.ToString();
                    string lbLicenseNo = SelectOrganisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + SelectOrganisation.LicenseNo.ToString() : "";

                    string mobile1 = SelectOrganisation.MobileNo != null ? "โทรศัพท์ " + SelectOrganisation.MobileNo.ToString() : "";
                    string mobile2 = SelectOrganisation.MobileNo != null ? "Tel. " + SelectOrganisation.MobileNo.ToString() : "";
                    string email = SelectOrganisation.Email != null ? "e-mail:" + SelectOrganisation.Email.ToString() : "";

                    lbFooterOrganisation.Text = page2.lbOrganisationPlace.Text + " " + lbLicenseNo;
                    page2.lbFooterOrganisation2.Text = page2.lbOrganisationPlace.Text + " " + lbLicenseNo;

                    lbAddressPage1.Text = SelectOrganisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2Page1.Text = SelectOrganisation.Address2?.ToString() + " " + mobile2 + " " + email;

                    page2.lbAddressPage2.Text = SelectOrganisation.Address?.ToString() + " " + mobile1 + " " + email;
                    page2.lbAddress2Page2.Text = SelectOrganisation.Address2?.ToString() + " " + mobile2 + " " + email;
                }
                if (SelectOrganisation.LogoImage != null)
                {
                    MemoryStream ms = new MemoryStream(SelectOrganisation.LogoImage);
                    logo1.Image = System.Drawing.Image.FromStream(ms);
                    page2.logo2.Image = System.Drawing.Image.FromStream(ms);
                }
                else
                {
                    MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                    logo1.Image = System.Drawing.Image.FromStream(ms);
                    page2.logo2.Image = System.Drawing.Image.FromStream(ms);
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
