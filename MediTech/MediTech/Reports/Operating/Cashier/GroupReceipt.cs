using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Model.Report;
using MediTech.DataService;

namespace MediTech.Reports.Operating.Cashier
{
    public partial class GroupReceipt : DevExpress.XtraReports.UI.XtraReport
    {
        public GroupReceipt()
        {
            InitializeComponent();
            this.BeforePrint += GroupReceipt_BeforePrint;
        }
        private void GroupReceipt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            int GroupReceiptUID = int.Parse(this.Parameters["GroupReceiptUID"].Value.ToString());
            int ReceiptCopy = int.Parse(this.Parameters["ReceiptCopy"].Value.ToString());
            var data = (new PurchaseingService()).GetGroupReceiptByUID(GroupReceiptUID);

            if(data != null)
            {
                if (data.ReceiptNo.Contains("NM"))
                {
                    TitleReceipt.Text = "ใบเสร็จรับเงิน/ใบกำกับภาษี";
                }
                lbReceiptCopy.Text = "ต้นฉบับ";
                lbBillNunber.Text = data.ReceiptNo;
                lbDate.Text = data.StartDttm?.ToString("dd'/'MM'/'yyyy");
                lbSell.Text = data.Seller;
                lbCompany.Text = data.PayorName;
                lbCompany2.Text = data.PayorName;
                lbAddress.Text = data.PayerAddress;
                var tax = (new BillingService()).GetPayorDetailByUID(data.PayorDetailUID).TINNo?.ToString();
                lbTexNo.Text = tax != null ? "เลขประจำตัวผู้เสียภาษี " + tax : "";

                if (ReceiptCopy == 0)
                {
                    lbReceiptCopy.Text = "ต้นฉบับ";
                }
                if (ReceiptCopy == 1)
                {
                    lbReceiptCopy.Text = "สำเนา";
                }

                if (data.GroupReceiptDetails.Count > 0)
                {
                    int i = 1;
                    double priceTotal = 0;

                    foreach (var item in data.GroupReceiptDetails)
                    {
                        item.No = i;
                        i++;

                        priceTotal = priceTotal + item.TotalPrice.Value;
                    }
                    lbThaiPrice.Text = ShareLibrary.NumberToText.ThaiBaht(priceTotal.ToString());
                }

                lbBeforeTaxAmount.Text = data.BfTaxAmount != 0 ? string.Format("{0:#,#.00}", data.BfTaxAmount): "0.00";
                lbTaxAmount.Text = data.TaxAmount != 0 ? string.Format("{0:#,#.00}", data.TaxAmount) : "0.00";
                lbNetAmount.Text = string.Format("{0:#,#.00}", data.NetAmount);
            }
            this.DataSource = data;
        }
    }
}
