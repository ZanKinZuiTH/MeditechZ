using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Operating.Patient
{
    public partial class MedicalCertificate : DevExpress.XtraReports.UI.XtraReport
    {
        public MedicalCertificate()
        {
            InitializeComponent();
            this.BeforePrint += MedicalCertificate_BeforePrint;
        }

        private void MedicalCertificate_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var dataSource = (new ReportsService()).PrintConfinedSpaceCertificate(PatientVisitUID);

            if (!String.IsNullOrEmpty(OrganisationUID.ToString()))
            {
                var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                if (Organisation != null)
                {
                    lbOgenisation.Text = Organisation.Description?.ToString();
                    lbOrganisationPlace.Text = Organisation.Description.ToString() != null ? "สถานที่ตรวจ " + Organisation.Description.ToString() : "";
                    lbLicenseNo.Text = Organisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + Organisation.LicenseNo.ToString() : "";
                    lbFooterOrganisation.Text = lbOgenisation.Text + " " + lbLicenseNo.Text;

                    string mobile1 = Organisation.MobileNo != null ? "โทรศัพท์ " + Organisation.MobileNo.ToString() : "";
                    string mobile2 = Organisation.MobileNo != null ? "Tel. " + Organisation.MobileNo.ToString() : "";
                    string email = Organisation.Email != null ? "e-mail:" + Organisation.Email.ToString() : "";

                    lbAddress1.Text = Organisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2.Text = Organisation.Address2?.ToString() + " " + mobile2 + " " + email;

                    infoOrganisation.Text = Organisation.Description?.ToString();
                }
            }
            else
            {
                var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(AppUtil.Current.OwnerOrganisationUID);
                if (Organisation != null)
                {
                    lbOgenisation.Text = Organisation.Description?.ToString();
                    lbOrganisationPlace.Text = Organisation.Description.ToString() != null ? "สถานที่ตรวจ " + Organisation.Description.ToString() : "";
                    lbLicenseNo.Text = Organisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + Organisation.LicenseNo.ToString() : "";
                    lbFooterOrganisation.Text = lbOgenisation.Text + " " + lbLicenseNo.Text;

                    string mobile1 = Organisation.MobileNo != null ? "โทรศัพท์ " + Organisation.MobileNo.ToString() : "";
                    string mobile2 = Organisation.MobileNo != null ? "Tel. " + Organisation.MobileNo.ToString() : "";
                    string email = Organisation.Email != null ? "e-mail:" + Organisation.Email.ToString() : "";

                    lbAddress1.Text = Organisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2.Text = Organisation.Address2?.ToString() + " " + mobile2 + " " + email;

                    infoOrganisation.Text = Organisation.Description?.ToString();
                }
            }
            
            this.DataSource = dataSource;
         
        }
    }
}
