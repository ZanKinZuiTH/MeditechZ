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
    public partial class PatientBill : DevExpress.XtraReports.UI.XtraReport
    {
        BillingService service = new BillingService();
        List<PatientBilledItemModel> listbill = new List<PatientBilledItemModel>();
        public PatientBill()
        {
            InitializeComponent();
            this.BeforePrint += PatientBill_BeforePrint;
        }

        void PatientBill_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int reportType = Convert.ToInt32(this.Parameters["ReportType"].Value.ToString());
            var listStatementBill = service.PrintStatementBill(Convert.ToInt64(this.Parameters["PatientBillUID"].Value.ToString()));
            this.DataSource = listStatementBill;

            string billType = listStatementBill.Select(p => p.BillType).FirstOrDefault();
            if (billType != "Invoice")
            {
                txtheader1.Text = "ใบเสร็จรับเงิน"+ System.Environment.NewLine + "ต้นฉบับ";
                txtheader2.Text = "ใบเสร็จรับเงิน"+ System.Environment.NewLine + "สำเนา";
            }
            else
            {
                txtheader1.Text = "ใบแจ้งหนี้/วางบิล" + System.Environment.NewLine + "ต้นฉบับ";
                txtheader2.Text = "ใบแจ้งหนี้/วางบิล" + System.Environment.NewLine + "สำเนา";
            }

            if (reportType == 0)
            {
                listbill = service.GetPatientBilledItem(Convert.ToInt64(this.Parameters["PatientBillUID"].Value.ToString()));
            }
            else
            {
                listbill = service.GetPatientBillingGroup(Convert.ToInt64(this.Parameters["PatientBillUID"].Value.ToString()));
            }



            double amountTotal_net = 0;
            double discountTotal_Net = 0;
            double cashTotal_net = 0;

            foreach (var item in listbill)
            {
                amountTotal_net = amountTotal_net + (item.Amount ?? 0);
                discountTotal_Net = discountTotal_Net + (item.Discount ?? 0);
                cashTotal_net = cashTotal_net + item.NetAmount.Value;

            }

            TBtotal_net.Text = string.Format("{0:#,#.00}", cashTotal_net);
            Tb_Total_copy.Text = TBtotal_net.Text;
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

            if (reportType == 0)
            {
                BillingDetail_supreport.ReportSource = (new PatientBillDetail());
                BillingDetail_supreport2.ReportSource = (new PatientBillDetail());
                PatientBilledItemModel newModel = new PatientBilledItemModel();
                newModel.BillinsgSubGroup = "ค่าบริการยา และ เวชภัณฑ์ทางการแพทย์";
                newModel.Amount = amountTotal_net;
                newModel.Discount = discountTotal_Net;
                newModel.NetAmount = cashTotal_net;
                listbill = new List<PatientBilledItemModel>();
                listbill.Add(newModel);
            }
            else if(reportType == 2)
            {
                BillingDetail_supreport.ReportSource = (new PatientBillDetail());
                BillingDetail_supreport2.ReportSource = (new PatientBillDetail());
                PatientBilledItemModel newModel = new PatientBilledItemModel();
                newModel.BillinsgSubGroup = "ค่าบริการตรวจสุขภาพ";
                newModel.Amount = amountTotal_net;
                newModel.Discount = discountTotal_Net;
                newModel.NetAmount = cashTotal_net;
                listbill = new List<PatientBilledItemModel>();
                listbill.Add(newModel);
            }
            else
            {
                BillingDetail_supreport.ReportSource = (new PatientBillDetail());
                BillingDetail_supreport2.ReportSource = (new PatientBillDetail());
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
