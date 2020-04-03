using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Model;
using MediTech.DataService;
using MediTech.ViewModels;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.IO;
using System.Linq;

namespace MediTech.Reports.Operating.Cashier
{
    public partial class PatientBill2 : DevExpress.XtraReports.UI.XtraReport
    {
        BillingService service = new BillingService();
        List<PatientBilledItemModel> listbill = new List<PatientBilledItemModel>();
        public PatientBill2()
        {
            InitializeComponent();
            this.BeforePrint += PatientBill_BeforePrint;
        }

        void PatientBill_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var listStatementBill = service.PrintStatementBill(Convert.ToInt64(this.Parameters["PatientBillUID"].Value.ToString()));
            this.DataSource = listStatementBill;
            listbill = service.GetPatientBilledItem(Convert.ToInt64(this.Parameters["PatientBillUID"].Value.ToString()));

            double cashTotal_net = 0;
            foreach (var item in listbill)
            {

                cashTotal_net = cashTotal_net + item.NetAmount.Value;

            }
            double vat = cashTotal_net * 7 / 107;
            TBtotalVAT.Text = string.Format("{0:#,#.00}", vat);
            TBtotalVATCopy.Text = TBtotalVAT.Text;

            TBtotalNonVAT.Text = string.Format("{0:#,#.00}", cashTotal_net - vat);
            TBtotalNonVATCopy.Text = TBtotalNonVAT.Text;

            TBtotal_net.Text = string.Format("{0:#,#.00}", cashTotal_net);
            TBtotalCopy_net.Text = TBtotal_net.Text;
            // xrTableCell32.Text = TBtotal_net.Text;
            lblThaiText.Text = "( " + ShareLibrary.NumberToText.ThaiBaht(cashTotal_net.ToString()) + " )";
            lblThaiTextCopy.Text = lblThaiText.Text;
            //xrLabel64.Text = lblThaiText.Text;
            //if (listStatementBill != null && listStatementBill.Count > 0)
            //{
            //    string healthOrganisationCode = listStatementBill.FirstOrDefault().OrganisationCode;
            //    if (healthOrganisationCode.ToUpper().Contains("BRXG"))
            //    {
            //        Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoBRXG.png", UriKind.Absolute);
            //        BitmapImage imageSource = new BitmapImage(uri);
            //        using (MemoryStream outStream = new MemoryStream())
            //        {
            //            BitmapEncoder enc = new BmpBitmapEncoder();
            //            enc.Frames.Add(BitmapFrame.Create(imageSource));
            //            enc.Save(outStream);
            //            this.logo1.Image = System.Drawing.Image.FromStream(outStream);
            //            this.logo2.Image = System.Drawing.Image.FromStream(outStream);
            //        }
            //    }
            //    else if (healthOrganisationCode.ToUpper().Contains("DRC"))
            //    {
            //        Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoDRC.png", UriKind.Absolute);
            //        BitmapImage imageSource = new BitmapImage(uri);
            //        using (MemoryStream outStream = new MemoryStream())
            //        {
            //            BitmapEncoder enc = new BmpBitmapEncoder();
            //            enc.Frames.Add(BitmapFrame.Create(imageSource));
            //            enc.Save(outStream);
            //            this.logo1.Image = System.Drawing.Image.FromStream(outStream);
            //            this.logo2.Image = System.Drawing.Image.FromStream(outStream);
            //        }
            //    }
            //}
            //else
            //{
            //    Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoBRXG.png", UriKind.Absolute);
            //    BitmapImage imageSource = new BitmapImage(uri);
            //    using (MemoryStream outStream = new MemoryStream())
            //    {
            //        BitmapEncoder enc = new BmpBitmapEncoder();
            //        enc.Frames.Add(BitmapFrame.Create(imageSource));
            //        enc.Save(outStream);
            //        this.logo1.Image = System.Drawing.Image.FromStream(outStream);
            //        this.logo2.Image = System.Drawing.Image.FromStream(outStream);
            //    }
            //}

            Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoBRXG.png", UriKind.Absolute);
            BitmapImage imageSource = new BitmapImage(uri);
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(imageSource));
                enc.Save(outStream);
                this.logo1.Image = System.Drawing.Image.FromStream(outStream);
                this.logo2.Image = System.Drawing.Image.FromStream(outStream);
            }

        }

        private void BillingDetail_supreport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            ((XRSubreport)sender).ReportSource.DataSource = listbill;

        }

        private void xrSubreport2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRSubreport)sender).ReportSource.DataSource = listbill;
        }

    }
}
