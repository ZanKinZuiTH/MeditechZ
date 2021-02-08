using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Operating.Patient
{
    public partial class CovidRapidTestCertification : DevExpress.XtraReports.UI.XtraReport
    {
        public CovidRapidTestCertification()
        {
            InitializeComponent();
            this.BeforePrint += CovidRapidTest_BeforePrint;
        }
        private void CovidRapidTest_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var dataSource = (new ReportsService()).PrintConfinedSpaceCertificate(PatientVisitUID);

            if (!String.IsNullOrEmpty(OrganisationUID.ToString()))
            {
                var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                if (Organisation != null)
                {
                    lbOrganisation.Text = Organisation.Description?.ToString();
                    string mobile = Organisation.MobileNo != null ? "Tel. " + Organisation.MobileNo.ToString() : "";
                    string email = Organisation.Email != null ? "E-mail:" + Organisation.Email.ToString() : "";
                    lbAddress.Text = Organisation.Address?.ToString() + " " + mobile +" "+ email;
                    lbAddress2.Text = Organisation.Address2?.ToString() + " " + mobile + " " + email;
                    lbLicense.Text = Organisation.LicenseNo != null ? "เลขที่ใบอนุญาตให้ประกอบกิจการสถานพยาบาล " + Organisation.LicenseNo.ToString() : "";

                    footerOrganisation.Text = Organisation.Description?.ToString();
                    footerAddress.Text = Organisation.Address?.ToString() + " " + mobile + " " + email;
                    footerAddress2.Text = Organisation.Address2?.ToString() + " " + mobile + " " + email;
                    footerLicense.Text = Organisation.LicenseNo != null ? "เลขที่ใบอนุญาตให้ประกอบกิจการสถานพยาบาล " + Organisation.LicenseNo.ToString() : "";
                }
            }
            else
            {
                var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(AppUtil.Current.OwnerOrganisationUID);
                if (Organisation != null)
                {
                    lbOrganisation.Text = Organisation.Description?.ToString();
                    string mobile = Organisation.MobileNo != null ? "Tel. " + Organisation.MobileNo.ToString() : "";
                    string email = Organisation.Email != null ? "E-mail:" + Organisation.Email.ToString() : "";
                    lbAddress.Text = Organisation.Address?.ToString() + " " + mobile + " " + email;
                    lbAddress2.Text = Organisation.Address2?.ToString() + " " + mobile + " " + email;
                    lbLicense.Text = Organisation.LicenseNo != null ? "เลขที่ใบอนุญาตให้ประกอบกิจการสถานพยาบาล " + Organisation.LicenseNo.ToString() : "";

                    footerOrganisation.Text = Organisation.Description?.ToString();
                    footerAddress.Text = Organisation.Address?.ToString() + " " + mobile + " " + email;
                    footerAddress2.Text = Organisation.Address2?.ToString() + " " + mobile + " " + email;
                    footerLicense.Text = Organisation.LicenseNo != null ? "เลขที่ใบอนุญาตให้ประกอบกิจการสถานพยาบาล " + Organisation.LicenseNo.ToString() : "";
                }
            }
            this.DataSource = dataSource;
        }
    }
}
