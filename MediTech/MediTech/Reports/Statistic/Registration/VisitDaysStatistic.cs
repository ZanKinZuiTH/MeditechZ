using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using MediTech.Model.Report;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevExpress.XtraCharts;

namespace MediTech.Reports.Statistic.Registration
{
    public partial class VisitDaysStatistic : DevExpress.XtraReports.UI.XtraReport
    {
        public VisitDaysStatistic()
        {
            InitializeComponent();
            this.BeforePrint += VisitDaysStatistic_BeforePrint;
        }

        private void VisitDaysStatistic_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int organisationUID = this.Parameters["OrganisationUID"].Value.ToString() != "" ? Convert.ToInt32(this.Parameters["OrganisationUID"].Value) : 0;
            int? vistyuid = this.Parameters["VISTYUID"].Value.ToString() != "" ? Convert.ToInt32(this.Parameters["VISTYUID"].Value) : 0;
            DateTime dateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["DateTo"].Value);
            List<ChartStatisticModel> dataStatistic = (new ReportsService()).VisitDaysStatistic(dateFrom, dateTo, vistyuid != 0 ? vistyuid : (int?)null, organisationUID != 0 ? organisationUID : (int?)null);

            this.xrChart1.BeginInit();
            Series series = new Series("Day", ViewType.Bar);
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            DevExpress.XtraCharts.ChartTitle chartTitle2 = new DevExpress.XtraCharts.ChartTitle();
            xyDiagram1.AxisY.Title.Text = "จำนวน (คน)";
            xyDiagram1.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.DefaultPane.EnableAxisXScrolling = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisXZooming = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisYScrolling = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisYZooming = DevExpress.Utils.DefaultBoolean.False;
            this.xrChart1.Diagram = xyDiagram1;
            chartTitle1.Font = new Font("Times New Roman", 13, FontStyle.Bold);
            chartTitle2.Font = new Font("Times New Roman", 13, FontStyle.Bold);
            chartTitle1.Text = "แผนภูมิแท่ง แสดงจำนวนผู้เข้ารับบริการในแต่ละวัน";
            chartTitle2.Text = "ตั้งแต่วันที่ " + dateFrom.ToString("dd/MM/yyyy") + "-" + dateTo.ToString("dd/MM/yyyy");
            this.xrChart1.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1,chartTitle2});
            xrChart1.Series.Add(series);
            xrChart1.Diagram = xyDiagram1;
            series.DataSource = dataStatistic;
            series.ShowInLegend = false;
            series.ArgumentScaleType = ScaleType.Auto;
            series.ArgumentDataMember = "Argument";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "Value" });
            this.xrChart1.EndInit();
            XRTable xrTable1 = new XRTable();
            xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
            | DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));

            if (dataStatistic != null)
            {
                XRTableRow headRow = new XRTableRow();
                XRTableCell cellhead1 = new XRTableCell();
                XRTableCell cellhead2 = new XRTableCell();
                cellhead1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                cellhead2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                cellhead1.Font = new Font("Times New Roman", 10, FontStyle.Bold);
                cellhead2.Font = new Font("Times New Roman", 10, FontStyle.Bold);
                cellhead1.Text = "วัน";
                cellhead2.Text = "จำนวนคน";
                headRow.Cells.Add(cellhead1);
                headRow.Cells.Add(cellhead2);
                xrTable1.Rows.Add(headRow);

                foreach (var item in dataStatistic)
                {
                    XRTableRow newRow = new XRTableRow();
                    XRTableCell cell1 = new XRTableCell();
                    XRTableCell cell2 = new XRTableCell();
                    cell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    cell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    cell1.Text = item.Argument;
                    cell2.Text = item.Value.ToString();
                    newRow.Cells.Add(cell1);
                    newRow.Cells.Add(cell2);
                    xrTable1.Rows.Add(newRow);
                }

                XRTableRow footRow = new XRTableRow();
                XRTableCell cellFoot1 = new XRTableCell();
                XRTableCell cellFoot2 = new XRTableCell();
                cellFoot1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                cellFoot2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                cellFoot1.Font = new Font("Times New Roman", 10, FontStyle.Bold);
                cellFoot2.Font = new Font("Times New Roman", 10, FontStyle.Bold);
                cellFoot1.Text = "รวม";
                cellFoot2.Text = dataStatistic.Sum(p => p.Value).ToString();
                footRow.Cells.Add(cellFoot1);
                footRow.Cells.Add(cellFoot2);
                xrTable1.Rows.Add(footRow);

                xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(22.5f, 66.25f);
                xrTable1.SizeF = new SizeF(246.88f, 25);
                this.PageHeader.Controls.Add(xrTable1);

            }

        }

        private DataTable CreateChartData(List<ChartStatisticModel> dataStatistic)
        {
            // Create an empty table.
            DataTable table = new DataTable("Table1");

            // Add two columns to the table.
            table.Columns.Add("Argument", typeof(String));
            table.Columns.Add("Value", typeof(Int32));

            // Add data rows to the table.
            DataRow row = null;
            foreach (var item in dataStatistic)
            {
                row = table.NewRow();
                row["Argument"] = item.Argument;
                row["Value"] = item.Value;
                table.Rows.Add(row);
            }

            return table;
        }
    }
}
