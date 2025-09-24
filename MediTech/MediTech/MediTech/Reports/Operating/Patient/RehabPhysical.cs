using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using MediTech.Model;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using System.IO;
using DevExpress.XtraPrinting;

namespace MediTech.Reports.Operating.Patient
{
    public partial class RehabPhysical : DevExpress.XtraReports.UI.XtraReport
    {
        List<HealthOrganisationModel> Organisations = new List<HealthOrganisationModel>();
        public RehabPhysical()
        {
            InitializeComponent();
            Organisations = (new MasterDataService()).GetHealthOrganisation();

            StaticListLookUpSettings lookupSettings = new StaticListLookUpSettings();
            foreach (var item in Organisations)
            {
                lookupSettings.LookUpValues.Add(new LookUpValue(item.HealthOrganisationUID, item.Name));
            }

            this.LogoType.LookUpSettings = lookupSettings;
            this.BeforePrint += MedicalCertificate_BeforePrint;
        }
        private void MedicalCertificate_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var dataSource = (new ReportsService()).PrintConfinedSpaceCertificate(PatientVisitUID);
            int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());

            var OrganisationRehab = (new MasterDataService()).GetHealthOrganisationByUID(27);
            if (logoType == 0)
            {
                var OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                if (OrganisationDefault.LogoImage != null)
                {
                    MemoryStream ms = new MemoryStream(OrganisationDefault.LogoImage);
                    logo.Image = Image.FromStream(ms);
                }
                else
                {
                    MemoryStream ms = new MemoryStream(OrganisationRehab.LogoImage);
                    logo.Image = Image.FromStream(ms);
                }
            }
            else
            {
                var SelectOrganisation = (new MasterDataService()).GetHealthOrganisationByUID(logoType);
                if (SelectOrganisation != null)
                {
                    if(SelectOrganisation.LogoImage != null)
                    {
                            MemoryStream ms = new MemoryStream(SelectOrganisation.LogoImage);
                            logo.Image = Image.FromStream(ms);
                    }
                    else
                        {
                            MemoryStream ms = new MemoryStream(OrganisationRehab.LogoImage);
                            logo.Image = Image.FromStream(ms);
                    }
                }
            }

                this.DataSource = dataSource;
        }

        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

    }
}
