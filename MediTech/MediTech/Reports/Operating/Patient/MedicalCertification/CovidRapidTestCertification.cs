using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using MediTech.Reports.Operating.Patient.MedicalCertification;

namespace MediTech.Reports.Operating.Patient
{
    public partial class CovidRapidTestCertification : DevExpress.XtraReports.UI.XtraReport
    {
        CovidRapidTestCertification2 page2 = new CovidRapidTestCertification2();
        public CovidRapidTestCertification()
        {
            InitializeComponent();
            this.BeforePrint += CovidRapidTest_BeforePrint;
            AfterPrint += Page2_AfterPrint;
        }
        private void CovidRapidTest_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var model = (new ReportsService()).PrintMedicalCertificate(PatientVisitUID);

            if (model != null)
            {
                page2.lbStartDate1.Text = model.strVisitData?.ToString("dd'/'MM'/'yyyy");
                page2.lbDoctor.Text = model.Doctor;
                page2.lbDoctorLicense.Text = model.DoctorLicenseNo;
                page2.lbSignDoctor.Text = model.Doctor;
                page2.lbSignPatient.Text = model.PatientName;
                page2.lbPatientName.Text = model.PatientName;
            }
            this.DataSource = model;
        }
        private void Page2_AfterPrint(object sender, EventArgs e)
        {

            page2.CreateDocument();
            this.Pages.AddRange(page2.Pages);
        }
    }
}
