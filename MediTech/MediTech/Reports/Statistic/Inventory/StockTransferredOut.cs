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
            string organisationList = this.Parameters["OrganisationList"].Value.ToString() ;
            DateTime dateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["DateTo"].Value);
            var dataReport = (new ReportsService()).StockTransferredOutReport(dateFrom, dateTo, organisationList);
            this.DataSource = dataReport;
        }

    }
}
