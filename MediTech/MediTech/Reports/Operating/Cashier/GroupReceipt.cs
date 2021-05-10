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
            var data = (new PurchaseingService()).GetGroupReceiptByUID(GroupReceiptUID);

            if(data != null)
            {
                lbBillNunber.Text = data.ReceiptNo;
                lbDate.Text = data.StartDttm?.ToString("dd'/'MM'/'yyyy");
                lbSell.Text = data.Seller;
                lbCompany.Text = data.PayorName;
                lbAddress.Text = data.PayerAddress;
                var tex = (new MasterDataService()).GetPayorDetailByUID(data.PayorDetailUID).TINNo?.ToString();
                lbTexNo.Text = tex != null ? "เลขประจำตัวผู้เลียภาษี " + tex : "";
                

                if (data.GroupReceiptDetailModel.Count > 0)
                {
                    int i = 1;
                    double priceTotal = 0;

                    foreach (var item in data.GroupReceiptDetailModel)
                    {
                        item.No = i;
                        i++;
                        priceTotal = priceTotal + item.TotalPrice.Value;
                    }
                    lbThaiPrice.Text = ShareLibrary.NumberToText.ThaiBaht(priceTotal.ToString());
                }
                
            }
            this.DataSource = data;
        }
    }
}
