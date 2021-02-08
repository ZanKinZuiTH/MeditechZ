using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Operating.Patient
{
    public partial class MedicalCertificateEng2Parts : DevExpress.XtraReports.UI.XtraReport
    {
        public MedicalCertificateEng2Parts()
        {
            InitializeComponent();
            this.BeforePrint += MedicalCertificateEng2Parts_BeforePrint;
        }

        private void MedicalCertificateEng2Parts_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var dataSource = (new ReportsService()).PrintConfinedSpaceCertificate(PatientVisitUID);

            if (!String.IsNullOrEmpty(OrganisationUID.ToString()))
            {
                var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                if (Organisation != null)
                {
                    string license = Organisation.LicenseNo != null ? "license no. " + Organisation.LicenseNo.ToString() : "";
                    lbOgenisation.Text = Organisation.Description?.ToString() +" "+ license;
                    lbFooterOrganisation.Text = lbOgenisation.Text + " " + license;

                    string mobile1 = Organisation.MobileNo != null ? "โทรศัพท์ " + Organisation.MobileNo.ToString() : "";
                    string mobile2 = Organisation.MobileNo != null ? "Tel. " + Organisation.MobileNo.ToString() : "";
                    string email = Organisation.Email != null ? "e-mail:" + Organisation.Email.ToString() : "";

                    lbAddress1.Text = Organisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2.Text = Organisation.Address2?.ToString() + " " + mobile2 + " " + email;

                    infoOrganisation1.Text = Organisation.Description?.ToString();
                    infoOrganisation2.Text = Organisation.Description?.ToString();
                    infoAddress.Text = Organisation.Address2?.ToString();
                }
            }
            else
            {
                var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(AppUtil.Current.OwnerOrganisationUID);
                if (Organisation != null)
                {
                    string license = Organisation.LicenseNo != null ? "license no. " + Organisation.LicenseNo.ToString() : "";
                    lbOgenisation.Text = Organisation.Description?.ToString() + " " + license;
                    lbFooterOrganisation.Text = Organisation.Description?.ToString() + " " + license;

                    string mobile1 = Organisation.MobileNo != null ? "โทรศัพท์ " + Organisation.MobileNo.ToString() : "";
                    string mobile2 = Organisation.MobileNo != null ? "Tel. " + Organisation.MobileNo.ToString() : "";
                    string email = Organisation.Email != null ? "e-mail:" + Organisation.Email.ToString() : "";

                    lbAddress1.Text = Organisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2.Text = Organisation.Address2?.ToString() + " " + mobile2 + " " + email;

                    infoOrganisation1.Text = Organisation.Description?.ToString();
                    infoOrganisation2.Text = Organisation.Description?.ToString();
                    infoAddress.Text = Organisation.Address2?.ToString();
                }
            }

            this.DataSource = dataSource;
        }
    }
}
