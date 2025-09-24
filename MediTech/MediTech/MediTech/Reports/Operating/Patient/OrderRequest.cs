using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using System.IO;

namespace MediTech.Reports.Operating.Patient
{
    public partial class OrderRequest : DevExpress.XtraReports.UI.XtraReport
    {
        public OrderRequest()
        {
            InitializeComponent();
            BeforePrint += OrderRequest_BeforePrint;
        }

        private void OrderRequest_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var dataSource = (new ReportsService()).PrintOrderRequestCard(PatientUID, PatientVisitUID);

            if (!String.IsNullOrEmpty(OrganisationUID.ToString()))
            {
                var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                if (Organisation != null)
                {
                    string organisation = Organisation.Description?.ToString();
                    string license = Organisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + Organisation.LicenseNo.ToString() : "";
                    lbOrganisation.Text = organisation + " " + license;

                    string mobile1 = Organisation.MobileNo != null ? "โทรศัพท์ " + Organisation.MobileNo.ToString() : "";
                    string mobile2 = Organisation.MobileNo != null ? "Tel. " + Organisation.MobileNo.ToString() : "";
                    string email = Organisation.Email != null ? "e-mail:" + Organisation.Email.ToString() : "";

                    lbAddress1.Text = Organisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2.Text = Organisation.Address2?.ToString() + " " + mobile2 + " " + email;
                    
                }
                var OrganisationBRXG = (new MasterDataService()).GetHealthOrganisationByUID(17);
                if (Organisation.LogoImage != null && Organisation.HealthOrganisationUID != 27)
                {
                    MemoryStream ms = new MemoryStream(Organisation.LogoImage);
                    xrPictureBox1.Image = Image.FromStream(ms);
                }
                else if (Organisation.LogoImage != null && Organisation.HealthOrganisationUID == 27)
                {
                    MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                    xrPictureBox1.Image = Image.FromStream(ms);
                }
                else
                {
                    MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                    xrPictureBox1.Image = Image.FromStream(ms);
                }
            }
            else
            {
                var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(AppUtil.Current.OwnerOrganisationUID);
                if (Organisation != null)
                {
                    string organisation = Organisation.Description?.ToString();
                    string license = Organisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + Organisation.LicenseNo.ToString() : "";
                    lbOrganisation.Text = organisation + " " + license;

                    string mobile1 = Organisation.MobileNo != null ? "โทรศัพท์ " + Organisation.MobileNo.ToString() : "";
                    string mobile2 = Organisation.MobileNo != null ? "Tel. " + Organisation.MobileNo.ToString() : "";
                    string email = Organisation.Email != null ? "e-mail:" + Organisation.Email.ToString() : "";

                    lbAddress1.Text = Organisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2.Text = Organisation.Address2?.ToString() + " " + mobile2 + " " + email;
                }
            }

            this.DataSource = dataSource;
          
        }
    }
}
