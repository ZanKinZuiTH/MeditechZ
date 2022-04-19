using DevExpress.XtraReports.UI;
using MediTech.DataService;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace MediTech.Reports.Operating.Patient
{
    public partial class CovidRapidTestCertification2 : DevExpress.XtraReports.UI.XtraReport
    {
        public CovidRapidTestCertification2()
        {
            InitializeComponent();
            this.BeforePrint += CovidRapidTestCertification2_BeforePrint;
        }

        private void CovidRapidTestCertification2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (this.Parameters["PatientVisitUID"].Value != null)
            {
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
}
