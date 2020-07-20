using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Statistic.Cashier
{
    public partial class StockTransferredOut : DevExpress.XtraReports.UI.XtraReport
    {
        public StockTransferredOut()
        {
            InitializeComponent();
            this.BeforePrint += DrugStoreNetProfit_BeforePrint;
            //xrPivotGrid1.CustomCellValue += xrPivotGrid1_CustomCellValue;
        }

        //void xrPivotGrid1_CustomCellValue(object sender, DevExpress.XtraReports.UI.PivotGrid.PivotCellValueEventArgs e)
        //{
        //    if (e.RowValueType == DevExpress.XtraPivotGrid.PivotGridValueType.Total || e.RowValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal)
        //    {
        //        if (e.DataField.FieldName == "Quantity" || e.DataField.FieldName == "Itemcost" )
        //        {
        //            e.Value = "";
        //        }
        //    }
        //}

        void DrugStoreNetProfit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int? organisationUID = this.Parameters["OrganisationUID"].Value.ToString() != "0" ? Convert.ToInt32(this.Parameters["OrganisationUID"].Value) : (int?)null;
            DateTime dateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["DateTo"].Value);
            var dataReport = (new ReportsService()).StockTransferredOutReport(dateFrom, dateTo, organisationUID);
            this.DataSource = dataReport;
        }

    }
}
