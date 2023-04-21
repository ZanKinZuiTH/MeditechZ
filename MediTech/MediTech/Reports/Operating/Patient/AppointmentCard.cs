using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using MediTech.Model;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using System.IO;

namespace MediTech.Reports.Operating.Patient
{
    public partial class AppointmentCard : DevExpress.XtraReports.UI.XtraReport
    {
        List<HealthOrganisationModel> Organisations = new List<HealthOrganisationModel>();
        public AppointmentCard()
        {
            InitializeComponent();
            Organisations = (new MasterDataService()).GetHealthOrganisation();

            StaticListLookUpSettings lookupSettings = new StaticListLookUpSettings();
            foreach (var item in Organisations)
            {
                lookupSettings.LookUpValues.Add(new LookUpValue(item.HealthOrganisationUID, item.Name));
            }
            this.LogoType.LookUpSettings = lookupSettings;
            this.BeforePrint += AppointmentCard_BeforePrint;
        }

        private void AppointmentCard_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            int bookUID = int.Parse(this.Parameters["BookUID"].Value.ToString());
            var dataSource = (new ReportsService()).PrintPatientBooking(bookUID);
            int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());

            var OrganisationBRXG = (new MasterDataService()).GetHealthOrganisationByUID(17);
            if (logoType == 0)
            {
                var OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                if (OrganisationDefault != null)
                {
                    string organisation = OrganisationDefault.Description?.ToString();
                    string lisence = OrganisationDefault.LicenseNo != null ? "ใบอนุญาตเลขที่ " + OrganisationDefault.LicenseNo.ToString() : "";
                    lbOrganisation.Text = organisation + " " + lisence;

                    string mobile1 = OrganisationDefault.MobileNo != null ? "โทรศัพท์ " + OrganisationDefault.MobileNo.ToString() : "";
                    string mobile2 = OrganisationDefault.MobileNo != null ? "Tel. " + OrganisationDefault.MobileNo.ToString() : "";
                    string email = OrganisationDefault.Email != null ? "e-mail:" + OrganisationDefault.Email.ToString() : "";

                    lbAddress.Text = OrganisationDefault.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2.Text = OrganisationDefault.Address2?.ToString() + " " + mobile2 + " " + email;

                    if (OrganisationDefault.LogoImage != null)
                    {
                        MemoryStream ms = new MemoryStream(OrganisationDefault.LogoImage);
                        logo.Image = Image.FromStream(ms);
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                        logo.Image = Image.FromStream(ms);
                    }
                }
                if (OrganisationUID == 26)
                {
                    textFooter.Text = "หากท่านมีความผิดปกติ หรือต้องการเปลี่ยนแปลงนัดหมาย กรุณาติดต่อ 097-4655997\r\nIf you have any problem or change appointment, please contact 097-4655997\r\nเปิดทำการทุกวัน  หยุดวันจันทร์ และวันศุกร์ (8.00-19.00 น.)\r\nOpen every day, except Monday and Friday (8 AM – 7 PM) \r\n";
                }
                else
                {
                    textFooter.Text = "ผู้ป่วยที่ใช้สิทธิต่างๆกรุณาแจ้งที่เคาร์เตอร์ลงทะเบียน ก่อนเข้ารับบริการ กรณีมีนัดหมายตรวจเลือด หรือเอกซเรย์ กรุณามาก่อนเวลานัด 1 ชั่วโมง\r\nPlease contact registration department and reach the hospital 1 prior 1 hour of appointment time in case of blood test or x-ray.\r\nหากท่านมีความผิดปกติกรุณาติดต่อกลับมาที่คลินิกก่อนเวลานัดหมาย Please return earlier if worsening symptom.\r\nกรณีเปลี่ยนแปลงนัดหมาย กรุณาติดต่อ 033 060 399 If you enable to keep appointment. Please call 033 060 399 เปิดทุกวัน เวลา 7:00 น. ถึง 18:00 น.  \r\n";
                }
            }
            else
            {
                var SelectOrganisation = (new MasterDataService()).GetHealthOrganisationByUID(logoType);
                if (SelectOrganisation != null)
                {
                    string organisation = SelectOrganisation.Description?.ToString();
                    string lisence = SelectOrganisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + SelectOrganisation.LicenseNo.ToString() : "";
                    lbOrganisation.Text = organisation + " " + lisence;

                    string mobile1 = SelectOrganisation.MobileNo != null ? "โทรศัพท์ " + SelectOrganisation.MobileNo.ToString() : "";
                    string mobile2 = SelectOrganisation.MobileNo != null ? "Tel. " + SelectOrganisation.MobileNo.ToString() : "";
                    string email = SelectOrganisation.Email != null ? "e-mail:" + SelectOrganisation.Email.ToString() : "";

                    lbAddress.Text = SelectOrganisation.Address?.ToString() + " " + mobile1 + " " + email;
                    lbAddress2.Text = SelectOrganisation.Address2?.ToString() + " " + mobile2 + " " + email;

                    if (SelectOrganisation.LogoImage != null)
                    {
                        MemoryStream ms = new MemoryStream(SelectOrganisation.LogoImage);
                        logo.Image = Image.FromStream(ms);
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                        logo.Image = Image.FromStream(ms);
                    }
                }
                if (logoType == 26)
                {
                    textFooter.Text = "หากท่านมีความผิดปกติ หรือต้องการเปลี่ยนแปลงนัดหมาย กรุณาติดต่อ 097-4655997\r\nIf you have any problem or change appointment, please contact 097-4655997\r\nเปิดทำการทุกวัน  หยุดวันจันทร์ และวันศุกร์ (8.00-19.00 น.)\r\nOpen every day, except Monday and Friday (8 AM – 7 PM) \r\n";
                }
                else
                {
                    textFooter.Text = "ผู้ป่วยที่ใช้สิทธิต่างๆกรุณาแจ้งที่เคาร์เตอร์ลงทะเบียน ก่อนเข้ารับบริการ กรณีมีนัดหมายตรวจเลือด หรือเอกซเรย์ กรุณามาก่อนเวลานัด 1 ชั่วโมง\r\nPlease contact registration department and reach the hospital 1 prior 1 hour of appointment time in case of blood test or x-ray.\r\nหากท่านมีความผิดปกติกรุณาติดต่อกลับมาที่คลินิกก่อนเวลานัดหมาย Please return earlier if worsening symptom.\r\nกรณีเปลี่ยนแปลงนัดหมาย กรุณาติดต่อ 033 060 399 If you enable to keep appointment. Please call 033 060 399 เปิดทุกวัน เวลา 7:00 น. ถึง 18:00 น.  \r\n";
                }

            }

            this.DataSource = dataSource;
      
         }
    }
}
