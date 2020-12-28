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
    public partial class CheckupOccMedSummary : DevExpress.XtraReports.UI.XtraReport
    {
        MediTech.DataService.MediTechDataService dbService = new DataService.MediTechDataService();


        public CheckupOccMedSummary()
        {
            InitializeComponent();
            this.AfterPrint += CheckupSummary_AfterPrint;
            this.BeforePrint += CheckupSummary_BeforePrint;
        }

        private void CheckupSummary_AfterPrint(object sender, EventArgs e)
        {
            var dataList = (this.DataSource as List<CheckupSummaryModel>);

            if (dataList != null && dataList.Count > 0)
            {
                if (dataList.Count <= 24)
                {
                    CheckupOccmedSummaryChart fChartSummary = new CheckupOccmedSummaryChart();
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
                    int number = dataList.Count / 2;
                    for (int i = 0; i <= 1; i++)
                    {
                        List<CheckupSummaryModel> newDatList = new List<CheckupSummaryModel>();
                        if (i == 0)
                        {
                            for (int j = 0; j < number; j++)
                            {
                                CheckupSummaryModel dataRow = new CheckupSummaryModel();
                                dataRow = dataList[j];
                                newDatList.Add(dataRow);
                            }
                        }
                        else if (i == 1)
                        {
                            for (int j = number; j < dataList.Count; j++)
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
            foreach (var dataRow in dataSummary)
            {
                string name1 = dataRow.GroupName.Substring(0, dataRow.GroupName.IndexOf(' '));
                string name2 = dataRow.GroupName.Substring(dataRow.GroupName.IndexOf(' '), dataRow.GroupName.Length - name1.Length);
                dataRow.GroupName = name1 + Environment.NewLine + name2;
            }
            this.DataSource = dataSummary;
        }
    }
}
