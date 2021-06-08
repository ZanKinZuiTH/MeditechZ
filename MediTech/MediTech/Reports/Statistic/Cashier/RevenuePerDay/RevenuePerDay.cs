using DevExpress.XtraReports.UI;
using MediTech.DataService;
using MediTech.Model.Report;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using DevExpress.Xpf.Core;
using System.Windows;
using DevExpress.XtraPrinting;

namespace MediTech.Reports.Statistic.Cashier
{
    public partial class RevenuePerDay : DevExpress.XtraReports.UI.XtraReport
    {

        List<RevenuePerDayModel> dataSourceBinding;

        List<List<RevenuePerDayModel>> listDataGroup = new List<List<RevenuePerDayModel>>();
        public RevenuePerDay()
        {
            InitializeComponent();
            this.BeforePrint += RevenuePerDay_BeforePrint;
            this.AfterPrint += RevenuePerDay_AfterPrint;
        }


        private void RevenuePerDay_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string organisationList = this.Parameters["OrganisationList"].Value.ToString();
            DateTime date = Convert.ToDateTime(this.Parameters["Date"].Value);
            dataSourceBinding = (new ReportsService()).GetRevenuePerDay(date, organisationList);
            if (dataSourceBinding == null)
            {
                DXMessageBox.Show("ไม่พบข้อมูล", "", MessageBoxButton.OK, MessageBoxImage.Information);
                this.ClosePreview();
            }

            if (dataSourceBinding != null)
            {
                List<int> organisationDistinct = dataSourceBinding.Select(p => p.OwnerOrganisationUID).Distinct().ToList();
                foreach (var organisationUID in organisationDistinct)
                {
                    var dataGroupOrg = dataSourceBinding.Where(p => p.OwnerOrganisationUID == organisationUID).ToList();
                    if (dataGroupOrg != null && dataGroupOrg.Count > 0)
                    {
                        if (!(this.DataSource is List<RevenuePerDayModel>))
                        {
                            this.DataSource = dataGroupOrg;
                            xrCrossTab1.DataSource = this.DataSource;
                            lblReportHeader.Text = "รายรับ " + dataGroupOrg.FirstOrDefault().HealthOrganisationName;
                        }
                        else
                        {
                            listDataGroup.Add(dataGroupOrg);

                        }

                    }
                }
            }


        }

        private void RevenuePerDay_AfterPrint(object sender, EventArgs e)
        {
            foreach (var dataGroupOrg in listDataGroup)
            {
                RevenuePerDayNewPage newPage = new RevenuePerDayNewPage();
                newPage.Parameters["Date"].Value = this.Parameters["Date"].Value;
                newPage.DataSource = dataGroupOrg;

                newPage.CreateDocument();
                this.Pages.AddRange(newPage.Pages);
            }
        }

    }
}
