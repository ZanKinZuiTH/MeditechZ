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
using DevExpress.XtraReports.Parameters;

namespace MediTech.Reports.Operating.Cashier
{
    public partial class PatientBill : DevExpress.XtraReports.UI.XtraReport
    {
        BillingService service = new BillingService();
        List<PatientBilledItemModel> listbill = new List<PatientBilledItemModel>();
        List<HealthOrganisationModel> Organisations = new List<HealthOrganisationModel>();

        public PatientBill()
        {
            InitializeComponent();
            Organisations = (new MasterDataService()).GetHealthOrganisation();
            
            StaticListLookUpSettings lookupSettings = new StaticListLookUpSettings();
            foreach (var item in Organisations)
            {
                lookupSettings.LookUpValues.Add(new LookUpValue(item.HealthOrganisationUID, item.Name));
            }

            this.LogoType.LookUpSettings = lookupSettings;
            this.BeforePrint += PatientBill_BeforePrint;
        }

        void PatientBill_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            int reportType = Convert.ToInt32(this.Parameters["ReportType"].Value.ToString());
            int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());
            var listStatementBill = service.PrintStatementBill(Convert.ToInt64(this.Parameters["PatientBillUID"].Value.ToString()));
            this.DataSource = listStatementBill;

            var OrganisationBRXG = (new MasterDataService()).GetHealthOrganisationByUID(17);
            if (logoType == 0)
                {
                    var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                    string mobile = Organisation.MobileNo != null ? "โทร " + Organisation.MobileNo?.ToString() : "";
                    string address = Organisation.Address?.ToString();

                    if(Organisation.HealthOrganisationUID == 26)
                    {
                        lbOrganisation.Text = "บริษัท เคเอส อีเคจี รีดดิ้ง จำกัด (สำนักงานใหญ่)";
                        lbOrganisationCopy.Text = "บริษัท เคเอส อีเคจี รีดดิ้ง จำกัด (สำนักงานใหญ่)";
                        lbAddress.Text = "20/47 หมู่ 8 ต.หนองปลาไหล อ.บางละมุง จ.ชลบุรี 20150";
                        lbAddressCopy.Text = "20/47 หมู่ 8 ต.หนองปลาไหล อ.บางละมุง จ.ชลบุรี 20150";
                        lbComment.Text = "(คลินิกหัวใจบูรพารักษ์ 155 / 204 หมู่ที่ 2 ต.ทับมา อ.เมือง จ.ระยอง 21000 โทร 097-4655997)";
                        lbComment2.Text = "(คลินิกหัวใจบูรพารักษ์ 155 / 204 หมู่ที่ 2 ต.ทับมา อ.เมือง จ.ระยอง 21000 โทร 097-4655997)";
                    }
                    else if (Organisation.HealthOrganisationUID == 25)
                    {
                        lbOrganisation.Text = OrganisationBRXG.Description?.ToString();
                        lbOrganisationCopy.Text = OrganisationBRXG.Description?.ToString();
                        string mobileSRC = OrganisationBRXG.MobileNo != null ? "โทร " + OrganisationBRXG.MobileNo?.ToString() : "";
                        string addressSRC = OrganisationBRXG.Address?.ToString();
                        lbAddress.Text = addressSRC + mobileSRC;
                        lbAddressCopy.Text = addressSRC + mobileSRC;
                    }
                    else
                    {
                        lbOrganisation.Text = Organisation.Description?.ToString();
                        lbOrganisationCopy.Text = Organisation.Description?.ToString();
                        lbAddress.Text = address + mobile;
                        lbAddressCopy.Text = address + mobile;
                    }
                    
                    lbTaxNo.Text = Organisation.TINNo != null ? "เลขประจำตัวผู้เสียภาษี : " + Organisation.TINNo.ToString() : "";
                    lbTaxNoCopy.Text = Organisation.TINNo != null ? "เลขประจำตัวผู้เสียภาษี : " + Organisation.TINNo.ToString() : "";

                if (Organisation.LogoImage != null)
                {
                    MemoryStream ms = new MemoryStream(Organisation.LogoImage);
                    logo1.Image = Image.FromStream(ms);
                    logo2.Image = Image.FromStream(ms);
                }
                else
                {
                    MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                    logo1.Image = Image.FromStream(ms);
                    logo2.Image = Image.FromStream(ms);
                }
            }
            else 
            {
                    var SelectOrganisation = (new MasterDataService()).GetHealthOrganisationByUID(logoType);
                    string mobile = SelectOrganisation.MobileNo != null ? "โทร " + SelectOrganisation.MobileNo?.ToString() : "";
                    string address = SelectOrganisation.Address?.ToString();

                    if (SelectOrganisation.HealthOrganisationUID != 26)
                    {
                        lbOrganisation.Text = SelectOrganisation.Description?.ToString();
                        lbAddress.Text = address + mobile;
                        lbOrganisationCopy.Text = SelectOrganisation.Description?.ToString();
                        lbAddressCopy.Text = address + mobile;
                        lbComment.Text = "";
                        lbComment2.Text = "";
                    }
                    else
                        {
                            lbOrganisation.Text = "บริษัท เคเอส อีเคจี รีดดิ้ง จำกัด (สำนักงานใหญ่)";
                            lbOrganisationCopy.Text = "บริษัท เคเอส อีเคจี รีดดิ้ง จำกัด (สำนักงานใหญ่)";
                            lbAddress.Text = "20/47 หมู่ 8 ต.หนองปลาไหล อ.บางละมุง จ.ชลบุรี 20150";
                            lbAddressCopy.Text = "20/47 หมู่ 8 ต.หนองปลาไหล อ.บางละมุง จ.ชลบุรี 20150";
                            lbComment.Text = "(คลินิกหัวใจบูรพารักษ์ 155 / 204 หมู่ที่ 2 ต.ทับมา อ.เมือง จ.ระยอง 21000 โทร 097-4655997)";
                            lbComment2.Text = "(คลินิกหัวใจบูรพารักษ์ 155 / 204 หมู่ที่ 2 ต.ทับมา อ.เมือง จ.ระยอง 21000 โทร 097-4655997)";
                        }

                    lbTaxNo.Text = SelectOrganisation.TINNo != null ? "เลขประจำตัวผู้เสียภาษี : " + SelectOrganisation.TINNo.ToString() : "";
                    lbTaxNoCopy.Text = SelectOrganisation.TINNo != null ? "เลขประจำตัวผู้เสียภาษี : " + SelectOrganisation.TINNo.ToString() : "";

                    if(SelectOrganisation.LogoImage != null)
                    {
                        MemoryStream ms = new MemoryStream(SelectOrganisation.LogoImage);
                        logo1.Image = Image.FromStream(ms);
                        logo2.Image = Image.FromStream(ms);
                    }
                    else if (OrganisationBRXG.LogoImage != null)
                    {
                        MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                        logo1.Image = Image.FromStream(ms);
                        logo2.Image = Image.FromStream(ms);
                    }
                
            }
            
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

            //Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoBRXG.png", UriKind.Absolute);
            //BitmapImage imageSource = new BitmapImage(uri);
            //using (MemoryStream outStream = new MemoryStream())
            //{
            //    BitmapEncoder enc = new BmpBitmapEncoder();
            //    enc.Frames.Add(BitmapFrame.Create(imageSource));
            //    enc.Save(outStream);
            //    //this.logo1.Image = System.Drawing.Image.FromStream(outStream);
            //    //this.logo2.Image = System.Drawing.Image.FromStream(outStream);
            //}


            BillingDetail_supreport.ReportSource = (new PatientBillDetail());
            BillingDetail_supreport2.ReportSource = (new PatientBillDetail());
            if (reportType == 0)
            {
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
                PatientBilledItemModel newModel = new PatientBilledItemModel();
                newModel.BillinsgSubGroup = "ค่าบริการตรวจสุขภาพ";
                newModel.Amount = amountTotal_net;
                newModel.Discount = discountTotal_Net;
                newModel.NetAmount = cashTotal_net;
                listbill = new List<PatientBilledItemModel>();
                listbill.Add(newModel);
            }
            else if (reportType == 3)
            {
                PatientBilledItemModel newModel = new PatientBilledItemModel();
                newModel.BillinsgSubGroup = "ค่าบริการเยี่ยมบ้าน";
                newModel.Amount = amountTotal_net;
                newModel.Discount = discountTotal_Net;
                newModel.NetAmount = cashTotal_net;
                listbill = new List<PatientBilledItemModel>();
                listbill.Add(newModel);
            }

            BillingDetail_supreport.ReportSource.DataSource = listbill;
            BillingDetail_supreport2.ReportSource.DataSource = listbill;

        }

    }
}
