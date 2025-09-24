using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Statistic.Cashier
{
    public partial class UsedReport : DevExpress.XtraReports.UI.XtraReport
    {
        public UsedReport()
        {
            InitializeComponent();
            this.BeforePrint += UsedReport_BeforePrint;
            xrPivotGrid1.CustomCellValue += xrPivotGrid1_CustomCellValue;
        }



        void xrPivotGrid1_CustomCellValue(object sender, DevExpress.XtraReports.UI.PivotGrid.PivotCellValueEventArgs e)
        {
            if (e.RowValueType == DevExpress.XtraPivotGrid.PivotGridValueType.Total || e.RowValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal)
            {
                if (e.DataField.FieldName == "Qty" || e.DataField.FieldName == "UnitCost" || e.DataField.FieldName == "UnitPrice" )
                {
                    e.Value = "";
                }
            }
        }

        void UsedReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string organisationList = this.Parameters["OrganisationList"].Value.ToString();
            DateTime dateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["DateTo"].Value);
            xrPivotGrid1.DataSource = (new ReportsService()).GetUsedReport(dateFrom, dateTo, organisationList);
        }

    }
}
