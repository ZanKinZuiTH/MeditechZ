using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Statistic.Cashier
{
    public partial class StockToEcount : DevExpress.XtraReports.UI.XtraReport
    {
        public StockToEcount()
        {
            InitializeComponent();
            this.BeforePrint += StockToEcount_BeforePrint;
            xrPivotGrid1.CustomCellValue += XrPivotGrid1_CustomCellValue;
        }

        private void XrPivotGrid1_CustomCellValue(object sender, DevExpress.XtraReports.UI.PivotGrid.PivotCellValueEventArgs e)
        {
            //if (e.RowValueType == DevExpress.XtraPivotGrid.PivotGridValueType.Total || e.RowValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal)
            //{
            //    if (e.DataField.FieldName == "Qty" || e.DataField.FieldName == "UnitCost" || e.DataField.FieldName == "UnitPrice")
            //    {
            //        e.Value = "";
            //    }
            //}
        }

        private void StockToEcount_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string organisationList = this.Parameters["OrganisationList"].Value.ToString();
            DateTime dateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["DateTo"].Value);
            int? vistyuid = this.Parameters["VISTYUID"].Value.ToString() != "0" ? Convert.ToInt32(this.Parameters["VISTYUID"].Value) : (int?)null;
            xrPivotGrid1.DataSource = (new ReportsService()).GetStockToEcount(dateFrom, dateTo, vistyuid, organisationList);
        }
    }
}
