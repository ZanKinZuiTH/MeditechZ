using DevExpress.XtraReports.UI;
using MediTech.DataService;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace MediTech.Reports.Operating.Patient
{
    public partial class CovidRapidTestCertificationMobile : DevExpress.XtraReports.UI.XtraReport
    {
        public CovidRapidTestCertificationMobile()
        {
            InitializeComponent();
            this.BeforePrint += CovidRapidTest_BeforePrint;
        }
        private void CovidRapidTest_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var model = (new ReportsService()).PrintMedicalCertificate(PatientVisitUID);

            if (model != null)
            {
                lbStartDate1.Text = model.strVisitData?.ToString("dd'/'MM'/'yyyy");
                lbDoctor.Text = model.Doctor;
                lbDoctorLicense.Text = model.DoctorLicenseNo;
                lbSignDoctor.Text = model.Doctor;
                lbSignPatient.Text = model.PatientName;
                lbPatientName.Text = model.PatientName;
            }
        }
    }
}
