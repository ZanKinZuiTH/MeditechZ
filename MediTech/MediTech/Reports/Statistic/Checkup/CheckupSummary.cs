using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Model;
using MediTech.Model.Report;
using System.Linq;
using System.Collections.Generic;

namespace MediTech.Reports.Statistic.Checkup
{
    public partial class CheckupSummary : DevExpress.XtraReports.UI.XtraReport
    {
        MediTech.DataService.MediTechDataService dbService = new DataService.MediTechDataService();


        public CheckupSummary()
        {
            InitializeComponent();
            this.AfterPrint += CheckupSummary_AfterPrint;
            this.BeforePrint += CheckupSummary_BeforePrint;
        }

        private void CheckupSummary_AfterPrint(object sender, EventArgs e)
        {
            var dataList = (this.DataSource as List<CheckupSummaryModel>);

            if (dataList != null && dataList.Count <= 24)
            {
                CheckupSummaryChart fChartSummary = new CheckupSummaryChart();
                string title = this.Parameters["Title"].Value.ToString();
                string year = this.Parameters["Year"].Value.ToString();
                fChartSummary.Parameters["Header"].Value = "กราฟแสดงผู้ที่มีผลการตรวจปกติ และผลผิดปกติประจำปี " + year
                    + Environment.NewLine + title;
                fChartSummary.checkupChart.DataSource = dataList;
                fChartSummary.CreateDocument();
                this.Pages.AddRange(fChartSummary.Pages);
            }
            else
            {
                for (int i = 0; i <= 1; i++)
                {
                    List<CheckupSummaryModel> newDatList = new List<CheckupSummaryModel>();
                    if (i == 0)
                    {
                        for (int j = 0; j < 15; j++)
                        {
                            CheckupSummaryModel dataRow = new CheckupSummaryModel();
                            dataRow = dataList[j];
                            newDatList.Add(dataRow);
                        }
                    }
                    else if(i == 1)
                    {
                        for (int j = 15; j < dataList.Count - 1; j++)
                        {
                            CheckupSummaryModel dataRow = new CheckupSummaryModel();
                            dataRow = dataList[j];
                            newDatList.Add(dataRow);
                        }

                    }
                    CheckupSummaryChart fChartSummary = new CheckupSummaryChart();
                    string title = this.Parameters["Title"].Value.ToString();
                    string year = this.Parameters["Year"].Value.ToString();
                    fChartSummary.Parameters["Header"].Value = "กราฟแสดงผู้ที่มีผลการตรวจปกติ และผลผิดปกติประจำปี " + year
                        + Environment.NewLine + title;
                    fChartSummary.checkupChart.DataSource = newDatList;
                    fChartSummary.CreateDocument();
                    this.Pages.AddRange(fChartSummary.Pages);
                }
            }



        }

        private void CheckupSummary_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            int checkupJobUID = Convert.ToInt32(this.Parameters["CheckupJobUID"].Value);
            string companyName = this.Parameters["CompanyName"].Value.ToString();
            string GPRSTUIDs = this.Parameters["GPRSTUIDs"].Value.ToString();
            string Year = this.Parameters["Year"].Value.ToString();
            string title = this.Parameters["Title"].Value.ToString();
            lbTitle.Text = title + Environment.NewLine + " โปรแกรมตรวจสุขภาพประจำปี " + Year;
            CheckupCompanyModel branchModel = new CheckupCompanyModel();
            branchModel.CheckupJobUID = checkupJobUID;
            branchModel.GPRSTUIDs = GPRSTUIDs;
            branchModel.CompanyName = companyName;
            var dataSummary = dbService.Reports.CheckupSummary(branchModel);
            this.DataSource = dataSummary;
        }
    }
}
