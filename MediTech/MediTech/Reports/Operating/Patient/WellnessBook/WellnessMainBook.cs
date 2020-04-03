using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using System.Collections.Generic;
using MediTech.DataService;
using MediTech.Model.Report;
using MediTech.Model;

namespace MediTech.Reports.Operating.Patient.WellnessBook
{
    public partial class WellnessMainBook : DevExpress.XtraReports.UI.XtraReport
    {

        WellnessBookBack fChartsReport = new WellnessBookBack();
        WellNessBookModel wellNessData = new WellNessBookModel();
        List<RequestLabModel> resultLab = new List<RequestLabModel>();
        Page categoryStartPage;
        int EndpageIndex;
        public WellnessBookBack BookBackReport
        {
            get { return fChartsReport; }
        }
        public WellnessMainBook()
        {
            InitializeComponent();
            BeforePrint += WellnessMainBook_BeforePrint;
            AfterPrint += WellnessMainBook_AfterPrint;
            xrTable2.PrintOnPage += LblPage_PrintOnPage;
        }



        private void LblPage_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            if (IsDisposed || categoryStartPage != null)
                return;
            categoryStartPage = Pages[e.PageIndex];
            EndpageIndex = e.PageCount;
        }

        private void WellnessMainBook_AfterPrint(object sender, EventArgs e)
        {
            if (IsDisposed || categoryStartPage == null)
                return;

            fChartsReport.Parameters["PateintInfomation"].Value = @"ชื่อ : " + wellNessData.PatientName
                + Environment.NewLine
                + "วันเดือนปีเกิด : " + (wellNessData.DOBDttm.HasValue ? wellNessData.DOBDttm.Value.ToString("dd/MM/yyyy") : "")
                + Environment.NewLine
                + "เลขปชช. : " + wellNessData.IDCard;
            fChartsReport.Parameters["PastHistory"].Value = "";
            fChartsReport.Parameters["Investigation"].Value = wellNessData.Investigation;
            fChartsReport.Parameters["Diagnosis"].Value = wellNessData.DiagnosisString;
            fChartsReport.Parameters["ChiefComplain"].Value = wellNessData.ChiefComplain;
            fChartsReport.Parameters["CareproviderName"].Value = wellNessData.CareproviderName;
            fChartsReport.Parameters["PhysicalExam"].Value = wellNessData.PERTFText;
            fChartsReport.Parameters["CareproviderInfomation"].Value = !string.IsNullOrEmpty(wellNessData.CareproviderLicenseNo) ? "เลขที่ใบประกอบวิชา " + wellNessData.CareproviderLicenseNo : "";
            fChartsReport.Parameters["Presentillness"].Value = wellNessData.Presentillness;
            fChartsReport.Parameters["LabResultTextual"].Value = GenerateLabHtml();


            fChartsReport.CreateDocument();
            int categoryStartPageIndex = categoryStartPage.Index;
            categoryStartPage = null;
            Pages.Insert(EndpageIndex, fChartsReport.Pages[0]);

        }

        private void WellnessMainBook_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            long patientuID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long patientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());

            wellNessData = (new ReportsService()).PrintWellNessBook(patientuID, patientVisitUID);
            resultLab = (new LabDataService()).GetResultLabGroupRequestNumber(patientVisitUID);
            if (wellNessData != null)
            {
                lblPatientID.Text = wellNessData.PatientID;
                lblPatientName.Text = wellNessData.PatientName;
                lblArrievedDttm.Text = "วันที่ตรวจ : " + (wellNessData.ArrivedDttm.HasValue ? wellNessData.ArrivedDttm.Value.ToString("dd/MM/yyyy") : "");
                lblPatientAge.Text = !string.IsNullOrEmpty(wellNessData.PatientAge) ? "อายุ " + wellNessData.PatientAge + " ปี" : "";
                lblMedicalSummery.Text = wellNessData.WellnessResult;
            }
        }

        private string GenerateLabHtml()
        {
            string resultLabTextual = string.Empty;
            foreach (var request in resultLab)
            {
                foreach (var requestDetail in request.RequestDetailLabs)
                {

                    if (requestDetail.ResultComponents != null && requestDetail.ResultComponents.Count == 1)
                    {
                        var resultcomponent = requestDetail.ResultComponents.FirstOrDefault();
                        resultLabTextual += (!string.IsNullOrEmpty(resultLabTextual) ? "<br>" : "")
                            + (string.IsNullOrEmpty(resultcomponent.IsAbnormal)
                            ? "<li>" + resultcomponent.ResultItemName + " : " + resultcomponent.ResultValue + " " + resultcomponent.UnitofMeasure + "</li>"
                            : "<li>" + resultcomponent.ResultItemName + " : " + "<i><b><u>" + resultcomponent.ResultValue + " " + resultcomponent.UnitofMeasure + "</u></b></i>" + "</li>");
                    }
                    else
                    {
                        resultLabTextual += (!string.IsNullOrEmpty(resultLabTextual) ? "<br>" : "")
                            + "<li>" + requestDetail.RequestItemName + " : " + "</li>";
                        int i = 0;
                        foreach (var result in requestDetail.ResultComponents)
                        {
                            resultLabTextual += (i == 0 ? "<br><ul style=\"margin-left:15px;\"><li>" : ", ")
                                + (string.IsNullOrEmpty(result.IsAbnormal)
                                ? result.ResultItemName + " " + result.ResultValue + " " + result.UnitofMeasure
                                : "<i><b><u>" + result.ResultItemName + " " + result.ResultValue + " " + result.UnitofMeasure + "</u></b></i>");
                            i++;
                        }

                        resultLabTextual += "</li></ul>";
                    }

                }
            }

            if (!string.IsNullOrEmpty(resultLabTextual))
            {
                resultLabTextual = "<div style=\"font-family:'Tahoma'; font-size:9pt\"><ul style=\"margin-left:25px;\">" + resultLabTextual + "</ul> </div>";
            }

            return resultLabTextual;
        }
    }
}
