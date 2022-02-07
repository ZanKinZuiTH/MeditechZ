using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Model;
using MediTech.DataService;
using System.Collections.Generic;
using MediTech.Helpers;
using System.Linq;
using DevExpress.XtraPrinting;
using MediTech.Model.Report;
using System.Text;
using System.Text.RegularExpressions;


namespace MediTech.Reports.Operating.Checkup
{
    public partial class Papsmear : DevExpress.XtraReports.UI.XtraReport
    {


        private MediTechDataService _DataService;

        public MediTechDataService DataService
        {
            get { return _DataService ?? (_DataService = new MediTechDataService()); }
        }
        List<XrayTranslateMappingModel> dtResultMapping;
        public string PreviewWellness { get; set; }

        public Papsmear()
        {
            InitializeComponent();
            BeforePrint += Papsmear_BeforePrint;
        }

        private void Papsmear_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            long patientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long patientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            int payorDetailUID = int.Parse(this.Parameters["PayorDetailUID"].Value.ToString());
            PatientWellnessModel data = DataService.Reports.PrintWellnessBook(patientUID, patientVisitUID, payorDetailUID);


            if (data.PatientInfomation != null)
            {
                var patient = data.PatientInfomation;
                lbHN.Text = patient.StartDttm != null ? patient.StartDttm.Value.ToString("dd/MM/yyyy") : "";
                lbPatientName.Text = patient.PatientName;
                lbHN.Text = patient.PatientID;
                lbAGE.Text = patient.Age != null ? patient.Age : "";
                lbDateCheckup.Text = patient.StartDttm != null ? patient.StartDttm.Value.ToString("dd/MM/yyyy") : "";
                lbcompany.Text = !string.IsNullOrEmpty(patient.CompanyName) ? patient.CompanyName : patient.PayorName;
               

            }

        }
    }
}
