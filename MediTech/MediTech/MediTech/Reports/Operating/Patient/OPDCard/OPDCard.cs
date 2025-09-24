using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Model;
using System.Collections.Generic;
using MediTech.DataService;
using MediTech.Model.Report;
using System.Linq;
using System.Windows.Media.Imaging;
using System.IO;
using DevExpress.XtraReports.Parameters;

namespace MediTech.Reports.Operating.Patient
{
    public partial class OPDCard : DevExpress.XtraReports.UI.XtraReport
    {
        List<OPDCardModel> listData;
        List<HealthOrganisationModel> Organisations = new List<HealthOrganisationModel>();
        public OPDCard()
        {
            InitializeComponent();
            Organisations = (new MasterDataService()).GetHealthOrganisation();

            StaticListLookUpSettings lookupSettings = new StaticListLookUpSettings();
            foreach (var item in Organisations)
            {
                lookupSettings.LookUpValues.Add(new LookUpValue(item.HealthOrganisationUID, item.Name));
            }

            this.LogoType.LookUpSettings = lookupSettings;

            this.BeforePrint += OPDCard_BeforePrint;
            xrSubreport1.BeforePrint += xrSubreport1_BeforePrint;
            xrSubreport2.BeforePrint += xrSubreport2_BeforePrint;
        }


        void OPDCard_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            listData = new List<OPDCardModel>();
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long patientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long patientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());
            listData = (new ReportsService()).PrintOPDCard(patientUID, patientVisitUID);
            this.DataSource = listData;

            if(OrganisationUID == 24)
            {
                logo.Visible = false;
            }

            if (listData != null && listData.Count > 0)
            {
                var OrganisationBRXG = (new MasterDataService()).GetHealthOrganisationByUID(17);
                if (logoType == 0)
                {
                    var OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                    this.lbOgenisation.Text = OrganisationDefault.Description?.ToString();
                    string License = OrganisationDefault.LicenseNo != null ? "ใบอนุญาตเลขที่ " + OrganisationDefault.LicenseNo.ToString() : "";
                    lbFooterOrganisation.Text = OrganisationDefault.Description?.ToString()+ " " + License;

                    string mobile1 = OrganisationDefault.MobileNo != null ? "โทรศัพท์ " + OrganisationDefault.MobileNo.ToString() : "";
                    string mobile2 = OrganisationDefault.MobileNo != null ? "Tel. " + OrganisationDefault.MobileNo.ToString() : "";
                    string email = OrganisationDefault.Email != null ? "e-mail:" + OrganisationDefault.Email.ToString() : "";

                    lbAddress1.Text = OrganisationDefault.Address?.ToString() + " " + mobile1 + " " + email;
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
                else
                {
                    var SelectOrganisation = (new MasterDataService()).GetHealthOrganisationByUID(logoType);
                    this.lbOgenisation.Text = SelectOrganisation.Description?.ToString();
                    string License = SelectOrganisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + SelectOrganisation.LicenseNo.ToString() : "";
                    lbFooterOrganisation.Text = SelectOrganisation.Description?.ToString() + " " + License;

                    string mobile1 = SelectOrganisation.MobileNo != null ? "โทรศัพท์ " + SelectOrganisation.MobileNo.ToString() : "";
                    string mobile2 = SelectOrganisation.MobileNo != null ? "Tel. " + SelectOrganisation.MobileNo.ToString() : "";
                    string email = SelectOrganisation.Email != null ? "e-mail:" + SelectOrganisation.Email.ToString() : "";

                    lbAddress1.Text = SelectOrganisation.Address?.ToString() + " " + mobile1 + " " + email;
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
        }

        void xrSubreport2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (listData != null)
            {
                foreach (var item in listData)
                {
                    ((XRSubreport)sender).ReportSource.DataSource = item.PatientDrugDetail;
                }

            }
        }

        void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (listData != null)
            {
                foreach (var item in listData)
                {
                    ((XRSubreport)sender).ReportSource.DataSource = item.PateintProblem;
                }

            }

        }
    }
}
