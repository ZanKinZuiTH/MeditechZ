using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Model;
using MediTech.DataService;
using System.Collections.Generic;
using System.Linq;

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
            long patientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long patientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            int payorDetailUID = int.Parse(this.Parameters["PayorDetailUID"].Value.ToString());
            List<CheckupBookModel> data = DataService.Reports.PrintCheckupBook(patientUID, patientVisitUID);
            if (data != null && data.Count > 0)
            {
                List<PatientResultLabModel> labCompare = DataService.Reports.CheckupLabCompare(patientUID, payorDetailUID);

                #region Complete Blood Count

                IEnumerable<PatientResultLabModel> cbcTestSet = labCompare
                    .Where(p => p.RequestItemName.Contains("CBC"))
                    .OrderBy(p => p.Year);
                GenerateCompleteBloodCount(cbcTestSet);


                #endregion
            }
        }

        private void GenerateCompleteBloodCount(IEnumerable<PatientResultLabModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                page4.cellHbRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001")?.ReferenceRange;
                if (countYear == 1)
                {
                    page4.cellCBCYear1.Text = "ปี" + " " + Years[0].ToString();
                    page4.cellCBCYear2.Text = "ปี" + " " + (Years[0] + 1).ToString();
                    page4.cellCBCYear3.Text = "ปี" + " " + (Years[0] + 2).ToString();

                    page4.cellHb1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001" && p.Year == Years[0])?.ResultValue;
                }
                else if (countYear == 2)
                {
                    page4.cellCBCYear1.Text = "ปี" + " " + Years[0].ToString();
                    page4.cellCBCYear2.Text = "ปี" + " " + Years[1].ToString();
                    page4.cellCBCYear3.Text = "ปี" + " " + (Years[1] + 1).ToString();

                    page4.cellHb1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001" && p.Year == Years[0])?.ResultValue;
                    page4.cellHb2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001" && p.Year == Years[1])?.ResultValue;
                }
                else if (countYear == 3)
                {
                    page4.cellCBCYear1.Text = "ปี" + " " + Years[0].ToString();
                    page4.cellCBCYear2.Text = "ปี" + " " + Years[1].ToString();
                    page4.cellCBCYear3.Text = "ปี" + " " + Years[2].ToString();

                    page4.cellHb1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001" && p.Year == Years[0])?.ResultValue;
                    page4.cellHb2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001" && p.Year == Years[1])?.ResultValue;
                    page4.cellHb3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001" && p.Year == Years[2])?.ResultValue;
                }
            }
            else
            {
                page4.cellCBCYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page4.cellCBCYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page4.cellCBCYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }

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
    }
}

