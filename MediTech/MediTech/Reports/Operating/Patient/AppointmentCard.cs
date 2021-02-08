using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Operating.Patient
{
    public partial class AppointmentCard : DevExpress.XtraReports.UI.XtraReport
    {
        public AppointmentCard()
        {
            InitializeComponent();
            this.BeforePrint += AppointmentCard_BeforePrint;
        }

        private void AppointmentCard_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            int bookUID = int.Parse(this.Parameters["BookUID"].Value.ToString());
            var dataSource = (new ReportsService()).PrintPatientBooking(bookUID);

            if (!String.IsNullOrEmpty(OrganisationUID.ToString()))
            {
                var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                if (Organisation != null)
                {
                    string organisation = Organisation.Description?.ToString();
                    string lisence = Organisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + Organisation.LicenseNo.ToString() : "";
                    lbOrganisation.Text = organisation + " " + lisence;

                    string mobile1 = Organisation.MobileNo != null ? "โทรศัพท์ " + Organisation.MobileNo.ToString() : "";
                    string mobile2 = Organisation.MobileNo != null ? "Tel. " + Organisation.MobileNo.ToString() : "";
                    string email = Organisation.Email != null ? "e-mail:" + Organisation.Email.ToString() : "";

                    lbAddress.Text = Organisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2.Text = Organisation.Address2?.ToString() + " " + mobile2 + " " + email;
                }
            }
            else
            {
                var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(AppUtil.Current.OwnerOrganisationUID);
                if (Organisation != null)
                {
                    string organisation = Organisation.Description?.ToString();
                    string lisence = Organisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + Organisation.LicenseNo.ToString() : "";
                    lbOrganisation.Text = organisation + " " + lisence;

                    string mobile1 = Organisation.MobileNo != null ? "โทรศัพท์ " + Organisation.MobileNo.ToString() : "";
                    string mobile2 = Organisation.MobileNo != null ? "Tel. " + Organisation.MobileNo.ToString() : "";
                    string email = Organisation.Email != null ? "e-mail:" + Organisation.Email.ToString() : "";

                    lbAddress.Text = Organisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2.Text = Organisation.Address2?.ToString() + " " + mobile2 + " " + email;
                }

            }

            this.DataSource = dataSource;
      
         }
    }
}
