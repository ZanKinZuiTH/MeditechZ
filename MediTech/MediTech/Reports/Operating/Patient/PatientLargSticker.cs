using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Operating.Patient
{
    public partial class PatientLargSticker : DevExpress.XtraReports.UI.XtraReport
    {
        public PatientLargSticker()
        {
            InitializeComponent();
            this.BeforePrint += LargSticker_BeforePrint;
        }
        private void LargSticker_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());

            if (!String.IsNullOrEmpty(OrganisationUID.ToString()))
            {
                var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                if (Organisation != null)
                {
                    lbFooterOrganisation.Text = Organisation.Description?.ToString();
                    lbAddress1.Text = Organisation.Address?.ToString();
                    lbAddress2.Text = Organisation.Address2?.ToString();
                }
            }
        }
    }
}
