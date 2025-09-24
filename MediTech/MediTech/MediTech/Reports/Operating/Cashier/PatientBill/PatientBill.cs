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
        List<LookupReferenceValueModel> Language = new List<LookupReferenceValueModel>();

        public PatientBill()
        {
            InitializeComponent();
            Organisations = (new MasterDataService()).GetHealthOrganisation();
            Language = (new TechnicalService()).GetReferenceValueMany("SPOKL");

            StaticListLookUpSettings lookupSettings = new StaticListLookUpSettings();
            StaticListLookUpSettings Languagelookup = new StaticListLookUpSettings();
            foreach (var item in Organisations)
            {
                lookupSettings.LookUpValues.Add(new LookUpValue(item.HealthOrganisationUID, item.Name));
            }

            foreach (var item in Language)
            {
                Languagelookup.LookUpValues.Add(new LookUpValue(item.ValueCode, item.Display));
            }

            this.LogoBillType.LookUpSettings = lookupSettings;
            this.LangType.LookUpSettings = Languagelookup;
            this.BeforePrint += PatientBill_BeforePrint;
        }

        void PatientBill_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            int reportType = Convert.ToInt32(this.Parameters["ReportType"].Value.ToString());
            int logoType = Convert.ToInt32(this.Parameters["LogoBillType"].Value.ToString());
            string langType = this.Parameters["LangType"].Value.ToString();
            var listStatementBill = service.PrintStatementBill(Convert.ToInt64(this.Parameters["PatientBillUID"].Value.ToString()));
            this.DataSource = listStatementBill;

            var OrganisationBRXG = (new MasterDataService()).GetHealthOrganisationByUID(30);
            if (langType == "EN")
            {
                NameLabel.Text = "Name :";
                NameLabelCopy.Text = "Name :";
                BillNoLabel.Text = "No :";
                BillNoLabelCopy.Text = "No :";
                DateLabel.Text = "Date :";
                DateLabelCopy.Text = "Date :";
                VisitLabel.Text = "Date of Treatment :";
                VisitLabelCopy.Text = "Date of Treatment :";

                ListLabel.Text = "Lists";
                ListLabelCopy.Text = "Lists";
                AmountLabel.Text = "Amount";
                AmountLabelCopy.Text = "Amount";
                DiscountLabel.Text = "Discount";
                DiscountLabelCopy.Text = "Discount";
                TotalLabel.Text = "Total (THB)";
                TotalLabelCopy.Text = "Total (THB)";
                TotalBottom.Text = "Total (Thai Baht)";
                TotalBottomCopy.Text = "Total (Thai Baht)";

                PayType.Text = "Paid By ";
                PayTypeCopy.Text = "Paid By ";
                Cash.Text = "Cash";
                CashCopy.Text = "Cash";
                Transfer.Text = "Bank Transfer";
                TransferCopy.Text = "Bank Transfer";
                Credit.Text = "Credit Card";
                CreditCopy.Text = "Credit Card";
                Cheque.Text = "Cheque";
                ChequeCopy.Text = "Cheque";
                Invoice.Text = "Billing";
                InvoiceCopy.Text = "Billing";

                SignPatient.Text = "Patient";
                SignPatientCopy.Text = "Patient";
                SignStaff.Text = "Financial Staff";
                SignStaffCopy.Text = "Financial Staff";

                BottomLabel.Text = "This receipt will be valid only after money has been transfer to the Bank";
                BottomLabelCopy.Text = "This receipt will be valid only after money has been transfer to the Bank";
            }
            else
            {
                NameLabel.Text = "ชื่อ-นามสกุล :";
                NameLabelCopy.Text = "ชื่อ-นามสกุล :";
                BillNoLabel.Text = "เลขที่ :";
                BillNoLabelCopy.Text = "เลขที่ :";
                DateLabel.Text = "วันที่ :";
                DateLabelCopy.Text = "วันที่ :";
                VisitLabel.Text = "วันที่ตรวจ :";
                VisitLabelCopy.Text = "วันที่ตรวจ :";

                ListLabel.Text = "รายการ";
                ListLabelCopy.Text = "รายการ";
                AmountLabel.Text = "จำนวนเงิน";
                AmountLabelCopy.Text = "จำนวนเงิน";
                DiscountLabel.Text = "ส่วนลด";
                DiscountLabelCopy.Text = "ส่วนลด";
                TotalLabel.Text = "ยอดรวม";
                TotalLabelCopy.Text = "ยอดรวม";
                TotalBottom.Text = "ยอดรวม";
                TotalBottomCopy.Text = "ยอดรวม";

                PayType.Text = "ชำระด้วย";
                PayTypeCopy.Text = "ชำระด้วย";
                Cash.Text = "เงินสด";
                CashCopy.Text = "เงินสด";
                Transfer.Text = "โอนเงิน";
                TransferCopy.Text = "โอนเงิน";
                Credit.Text = "บัตรเครดิต";
                CreditCopy.Text = "บัตรเครดิต";
                Cheque.Text = "เช็ค";
                ChequeCopy.Text = "เช็ค";
                Invoice.Text = "วางบิล";
                InvoiceCopy.Text = "วางบิล";

                SignPatient.Text = "ลายเซ็นผู้ป่วย";
                SignPatientCopy.Text = "ลายเซ็นผู้ป่วย";
                SignStaff.Text = "เจ้าหน้าที่";
                SignStaffCopy.Text = "เจ้าหน้าที่";

                BottomLabel.Text = "การชำระเงินจะสมบูรณ์เมื่อบริษัทได้รับเงินเรียบร้อยแล้ว";
                BottomLabelCopy.Text = "การชำระเงินจะสมบูรณ์เมื่อบริษัทได้รับเงินเรียบร้อยแล้ว";
            }

            string telLabel = langType == "EN" ? "Tel. " : "โทร";
            string taxLabel = langType == "EN" ? "Tax ID. " : "เลขประจำตัวผู้เสียภาษี ";

            if (logoType == 0)
            {
                var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                string mobile = Organisation.MobileNo != null ? " "+ telLabel +" " + Organisation.MobileNo?.ToString() : "";
                string address = langType == "EN" ? Organisation.Address2?.ToString() : Organisation.Address?.ToString();

                if (Organisation.HealthOrganisationUID == 26)
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
                    lbOrganisation.Text = "บริษัทบีอาร์เอ็กซ์จีจำกัด (คลินิกบูรพารักษ์สาขาศรีราชา)";
                    lbOrganisationCopy.Text = "บริษัทบีอาร์เอ็กซ์จีจำกัด (คลินิกบูรพารักษ์สาขาศรีราชา)";
                    string mobileSRC = Organisation.MobileNo != null ? " " + telLabel + " " + Organisation.MobileNo?.ToString() : "";
                    string addressBrxg = langType == "EN" ? OrganisationBRXG.Address2?.ToString() : OrganisationBRXG.Address?.ToString();
                    lbAddress.Text = addressBrxg + mobile;
                    lbAddressCopy.Text = addressBrxg + mobile;
                }
                else if (Organisation.HealthOrganisationUID == 17)
                {
                    lbOrganisation.Text = Organisation.Description?.ToString();
                    lbOrganisationCopy.Text = Organisation.Description?.ToString();
                    string mobileBrxg = Organisation.MobileNo != null ? " " + telLabel + " " + Organisation.MobileNo?.ToString() : "";
                    string addressBrxg = langType == "EN" ? OrganisationBRXG.Address2?.ToString() : OrganisationBRXG.Address?.ToString();
                    lbAddress.Text = addressBrxg + mobileBrxg;
                    lbAddressCopy.Text = addressBrxg + mobileBrxg;
                }
                else
                {
                    lbOrganisation.Text = Organisation.Description?.ToString();
                    lbOrganisationCopy.Text = Organisation.Description?.ToString();
                    lbAddress.Text = address + mobile;
                    lbAddressCopy.Text = address + mobile;
                }

                lbTaxNo.Text = Organisation.TINNo != null ? taxLabel + Organisation.TINNo.ToString() : "";
                lbTaxNoCopy.Text = Organisation.TINNo != null ? taxLabel + Organisation.TINNo.ToString() : "";


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
                string mobile = SelectOrganisation.MobileNo != null ? " " + telLabel + " " + SelectOrganisation.MobileNo?.ToString() : "";
                string address = langType == "EN" ? SelectOrganisation.Address2?.ToString() : SelectOrganisation.Address?.ToString();

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
                    if(langType == "EN")
                    {
                        lbOrganisation.Text = "KS EKG READING COMPANY LIMITED";
                        lbOrganisationCopy.Text = "KS EKG READING COMPANY LIMITED";
                        lbAddress.Text = "20/47 moo 8, Nong Pla Lai, Bang Lamung, Chonburi 20150";
                        lbAddressCopy.Text = "20/47 moo 8, Nong Pla Lai, Bang Lamung, Chonburi 20150";
                        lbComment.Text = "(Burapharux Cardiac 155/204 moo 2, Thapma, Mueang, Rayong 21000 Tel 097-4655997)";
                        lbComment2.Text = "(Burapharux Cardiac 155/204 moo 2, Thapma, Mueang, Rayong 21000 Tel 097-4655997)";
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
                }

                lbTaxNo.Text = SelectOrganisation.TINNo != null ? taxLabel + SelectOrganisation.TINNo.ToString() : "";
                lbTaxNoCopy.Text = SelectOrganisation.TINNo != null ? taxLabel + SelectOrganisation.TINNo.ToString() : "";

                if (SelectOrganisation.LogoImage != null)
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

                if(langType == "EN")
                {
                    txtheader1.Text = "Receipt" + System.Environment.NewLine + "Original";
                    txtheader2.Text = "Receipt" + System.Environment.NewLine + "Copy";
                }
            }
            else
            {
                txtheader1.Text = "ใบแจ้งหนี้/วางบิล" + System.Environment.NewLine + "ต้นฉบับ";
                txtheader2.Text = "ใบแจ้งหนี้/วางบิล" + System.Environment.NewLine + "สำเนา";

                if (langType == "EN")
                {
                    txtheader1.Text = "Invoice/Billing" + System.Environment.NewLine + "Original";
                    txtheader2.Text = "Invoice/Billing" + System.Environment.NewLine + "Copy";
                }
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
                //cashTotal_net = cashTotal_net + item.NetAmount.Value;

            }

            discountTotal_Net = Math.Round(discountTotal_Net);
            cashTotal_net = amountTotal_net - discountTotal_Net;

            TBtotal_net.Text = string.Format("{0:#,#.00}", cashTotal_net);
            Tb_Total_copy.Text = TBtotal_net.Text;
            //xrTableCell32.Text = TBtotal_net.Text;
            lblThaiText.Text = "( " + ShareLibrary.NumberToText.ThaiBaht(cashTotal_net.ToString()) + " )";
            lblThaiTextCopy.Text = lblThaiText.Text;

            if (langType == "EN")
            {
                lblThaiText.Text = "( " + ShareLibrary.NumberToText.NumberToWords(cashTotal_net) + " )";
            lblThaiTextCopy.Text = lblThaiText.Text;
            }


            if (langType == "EN")
            {
                foreach (var item in listbill)
                {
                    item.BillingGroup = item.BillingGroupEN;
                    item.BillinsgSubGroup = item.BillingSubGroupEN;
                }
            }

            BillingDetail_supreport.ReportSource = (new PatientBillDetail());
            BillingDetail_supreport2.ReportSource = (new PatientBillDetail());
            if (reportType == 0)
            {
                PatientBilledItemModel newModel = new PatientBilledItemModel();
                newModel.BillinsgSubGroup = langType == "EN" ? "Medical Supplies" : "ค่าบริการยา และ เวชภัณฑ์ทางการแพทย์";
                newModel.Amount = amountTotal_net;
                newModel.Discount = discountTotal_Net;
                newModel.NetAmount = cashTotal_net;
                listbill = new List<PatientBilledItemModel>();
                listbill.Add(newModel);
            }
            else if(reportType == 2)
            {
                PatientBilledItemModel newModel = new PatientBilledItemModel();
                newModel.BillinsgSubGroup = langType == "EN" ? "Checkup service fee" : "ค่าบริการตรวจสุขภาพ";
                newModel.Amount = amountTotal_net;
                newModel.Discount = discountTotal_Net;
                newModel.NetAmount = cashTotal_net;
                listbill = new List<PatientBilledItemModel>();
                listbill.Add(newModel);
            }
            else if (reportType == 3)
            {
                PatientBilledItemModel newModel = new PatientBilledItemModel();
                newModel.BillinsgSubGroup = langType == "EN" ? "Homecare service fee" : "ค่าบริการเยี่ยมบ้าน";
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
