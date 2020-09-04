using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Model;
using MediTech.DataService;
using System.Collections.Generic;

namespace MediTech.Reports.Operating.Patient.CheckupBookReport
{
    public partial class CheckupBook1 : DevExpress.XtraReports.UI.XtraReport
    {
        private MediTechDataService _DataService;

        public MediTechDataService DataService
        {
            get { return _DataService ?? (_DataService = new MediTechDataService()); }
        }

        CheckupBook2 page2 = new CheckupBook2();
        CheckupBook3 page3 = new CheckupBook3();
        CheckupBook4 page4 = new CheckupBook4();
        CheckupBook5 page5 = new CheckupBook5();
        CheckupBook6 page6 = new CheckupBook6();

        public CheckupBook1()
        {
            InitializeComponent();
            BeforePrint += BookPage1_BeforePrint;
            AfterPrint += BookPage1_AfterPrint;
        }

        private void BookPage1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //long patientuID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            //long patientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            //List<CheckupBookModel> data = DataService.Reports.PrintCheckupBook(patientuID, patientVisitUID);
 
        }

        private void BookPage1_AfterPrint(object sender, EventArgs e)
        {
            page2.CreateDocument();
            page3.CreateDocument();
            page4.CreateDocument();
            page5.CreateDocument();
            page6.CreateDocument();
            this.Pages.AddRange(page2.Pages);
            this.Pages.AddRange(page3.Pages);
            this.Pages.AddRange(page4.Pages);
            this.Pages.AddRange(page5.Pages);
            this.Pages.AddRange(page6.Pages);
        }

        private void xrLabel1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}

