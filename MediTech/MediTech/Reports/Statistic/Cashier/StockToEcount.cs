using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using MediTech.Model.Report;
using System.Collections.Generic;

namespace MediTech.Reports.Statistic.Cashier
{
    public partial class StockToEcount : DevExpress.XtraReports.UI.XtraReport
    {
        public StockToEcount()
        {
            InitializeComponent();
            this.BeforePrint += StockToEcount_BeforePrint;
           
                
         
        }

  

        private void StockToEcount_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string organisationList = this.Parameters["OrganisationList"].Value.ToString();
            DateTime dateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["DateTo"].Value);
            int? vistyuid = this.Parameters["VISTYUID"].Value.ToString() != "0" ? Convert.ToInt32(this.Parameters["VISTYUID"].Value) : (int?)null;
            string storefromInRow = this.Parameters["StoreFrom"].Value.ToString();
            List<EcountExportModel>  data  = (new ReportsService()).GetStockToEcount(dateFrom, dateTo, vistyuid, organisationList);
            this.DataSource = data;
        }

        private void cStore_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = sender as XRTableCell;
            if (cell.DataBindings.Count > 0)
            {
             
                string value = (string)cell.Report.GetCurrentColumnValue(cell.DataBindings[0].DataMember);
                if (string.IsNullOrEmpty(value))
                {
                    cell.Text = this.Parameters["StoreFrom"].Value.ToString();
                }
  
            }

        }

        private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void xrTableRow2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //XRTableRow row = (XRTableRow)sender;
            //if (row != null)
            //{
            //    foreach (XRTableCell cellData in row.Cells)
            //    {
            //        if (cellData.Name == "payBy")
            //        {
            //            if (string.IsNullOrEmpty(cellData.Value.ToString()))
            //            {
            //                
            //            }
            //        }
            //    }
            //}
        }

        private void xrTableCell35_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = sender as XRTableCell;
            if (cell.DataBindings.Count > 0)
            {

                int?value = (int?)cell.Report.GetCurrentColumnValue(cell.DataBindings[0].DataMember);
                if ( value  != null && value != 0 )
                {
                    switch (value.ToString())
                    {
                        case "2933":
                            cell.Text = "13";
                            cell.Value = "13";
                            break;
                        case "2934":
                            cell.Text = "15";
                            cell.Value = "15";
                            break;
                        case "2935":
                            cell.Text = "17";
                            cell.Value = "17";
                            break;
                        case "2936":
                            cell.Text = "19";
                            cell.Value = "19";
                            break;
                        case "4280":
                            cell.Text = "1B";
                            cell.Value = "1B";
                            break;
                        case "4305":
                            cell.Text = "1D";
                            cell.Value = "1D";
                            break;
                    }
                }

                }
        }

        private void xrTableCell35_AfterPrint(object sender, EventArgs e)
        {
            XRTableCell cell = sender as XRTableCell;
            if (cell.DataBindings.Count > 0)
            {

                int? value = (int?)cell.Report.GetCurrentColumnValue(cell.DataBindings[0].DataMember);
                if (value != null && value != 0)
                {
                    switch (value.ToString())
                    {
                        case "2933":
                            cell.Text = "13";
                            cell.Value = "13";
                            break;
                        case "2934":
                            cell.Text = "15";
                            cell.Value = "15";
                            break;
                        case "2935":
                            cell.Text = "17";
                            cell.Value = "17";
                            break;
                        case "2936":
                            cell.Text = "19";
                            cell.Value = "19";
                            break;
                        case "4280":
                            cell.Text = "1B";
                            cell.Value = "1B";
                            break;
                        case "4305":
                            cell.Text = "1D";
                            cell.Value = "1D";
                            break;
                    }
                }

            }
        }
    }
}
