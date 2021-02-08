using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Operating.Patient
{
    public partial class MedicalCertificateEng : DevExpress.XtraReports.UI.XtraReport
    {
        public MedicalCertificateEng()
        {
            InitializeComponent();
            this.BeforePrint += MedicalCertificateEng_BeforePrint;
        }

        private void MedicalCertificateEng_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var dataSource = (new ReportsService()).PrintMedicalCertificate(PatientVisitUID);


            if (!String.IsNullOrEmpty(OrganisationUID.ToString()))
            {
                var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                if (Organisation != null)
                {
                    lbAddress.Text = Organisation.Address2?.ToString();
                    string mobile = Organisation.MobileNo != null ? "Tel. " + Organisation.MobileNo.ToString() : "";
                    string email = Organisation.Email != null ? "e-mail:" + Organisation.Email.ToString() : "";

                    lbInfo.Text = mobile + " " + email;
                    
                }
            }
            else
            {
                var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(AppUtil.Current.OwnerOrganisationUID);
                if (Organisation != null)
                {
                    lbAddress.Text = Organisation.Address2?.ToString();
                    string mobile = Organisation.MobileNo != null ? "Tel. " + Organisation.MobileNo.ToString() : "";
                    string email = Organisation.Email != null ? "e-mail:" + Organisation.Email.ToString() : "";

                    lbInfo.Text = mobile + " " + email;
                }
            }
            this.DataSource = dataSource;
        }
    }
}
