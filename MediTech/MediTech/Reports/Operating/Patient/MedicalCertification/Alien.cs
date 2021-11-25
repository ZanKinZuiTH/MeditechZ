using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Windows.Media.Imaging;
using System.IO;
using MediTech.DataService;
using MediTech.Model;
using System.Collections.Generic;
using System.Linq;

namespace MediTech.Reports.Operating.Patient
{
    public partial class Alien : DevExpress.XtraReports.UI.XtraReport
    {
        public Alien()
        {
            InitializeComponent();
            this.BeforePrint += MedicalCertification_BeforePrint;
        }

        private void MedicalCertification_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            // Image imageCheckbox = null;
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var medicalData = (new ReportsService()).PrintMedicalCertificate(PatientVisitUID);
            this.DataSource = medicalData;

            //Uri uri = new Uri(@"pack://application:,,,/AlienRegister;component/Resources/Icon/Checkbox.png", UriKind.Absolute);
            //BitmapImage imageSource = new BitmapImage(uri);
            //using (MemoryStream outStream = new MemoryStream())
            //{
            //    BitmapEncoder enc = new BmpBitmapEncoder();
            //    enc.Frames.Add(BitmapFrame.Create(imageSource));
            //    enc.Save(outStream);
            //    imageCheckbox = System.Drawing.Image.FromStream(outStream);
            //}

            //if (medicalData != null )
            //{
            //    if (this.Parameters["CheckupDate"].Value == null || (DateTime)this.Parameters["CheckupDate"].Value == DateTime.MinValue)
            //    {
            //       // this.Parameters["CheckupDate"].Value = medicalData.FirstOrDefault().ArrivedDttm;

            //        this.Parameters["CheckupDate"].Value = medicalData.strVisitData;
            //    }

            //switch (medicalData.FirstOrDefault().Tuberculosis)
            //{
            //    case "ปกติ":
            //        imgTuberculosis1.Image = imageCheckbox;
            //        break;
            //    case "ผิดปกติ/ให้รักษา":
            //        imgTuberculosis2.Image = imageCheckbox;
            //        break;
            //    case "ระยะอันตราย":
            //        imgTuberculosis3.Image = imageCheckbox;
            //        break;
            //}

            //switch (medicalData.FirstOrDefault().Leprosy)
            //{
            //    case "ปกติ":
            //        imgLeprosy1.Image = imageCheckbox;
            //        break;
            //    case "ผิดปกติ/ให้รักษา":
            //        imgLeprosy2.Image = imageCheckbox;
            //        break;
            //    case "ระยะติดต่อ/อาการเป็นที่รังเกียจ":
            //        imgLeprosy3.Image = imageCheckbox;
            //        break;
            //}

            //switch (medicalData.FirstOrDefault().Elephantiasis)
            //{
            //    case "ปกติ":
            //        imgElephantiasis1.Image = imageCheckbox;
            //        break;
            //    case "ผิดปกติ/ให้รักษา":
            //        imgElephantiasis2.Image = imageCheckbox;
            //        break;
            //    case "อาการเป็นที่รังเกียจ":
            //        imgElephantiasis3.Image = imageCheckbox;
            //        break;
            //}

            //switch (medicalData.FirstOrDefault().Syphilis)
            //{
            //    case "ปกติ":
            //        imgSyphilis1.Image = imageCheckbox;
            //        break;
            //    case "ผิดปกติ/ให้รักษา":
            //        imgSyphilis2.Image = imageCheckbox;
            //        break;
            //    case "ระยะที่ 3":
            //        imgSyphilis3.Image = imageCheckbox;
            //        break;
            //}

            //switch (medicalData.FirstOrDefault().Narcotic)
            //{
            //    case "ปกติ":
            //        imgNarcotic1.Image = imageCheckbox;
            //        break;
            //    case "ผิดปกติ/ให้รักษา":
            //        imgNarcotic2.Image = imageCheckbox;
            //        break;
            //    case "ให้ตรวจยืนยัน":
            //        imgNarcotic3.Image = imageCheckbox;
            //        break;
            //}

            //switch (medicalData.FirstOrDefault().Alcohol)
            //{
            //    case "ปกติ":
            //        imgAlcohol1.Image = imageCheckbox;
            //        break;
            //    case "ปรากฏอาการ":
            //        imgAlcohol2.Image = imageCheckbox;
            //        break;
            //}

            //switch (medicalData.FirstOrDefault().Pregnant)
            //{
            //    case "ไม่ตรวจ":
            //        imgPregnant1.Image = imageCheckbox;
            //        break;
            //    case "ไม่ตั้งครรภ์":
            //        imgPregnant2.Image = imageCheckbox;
            //        break;
            //    case "ตั้งครรภ์":
            //        imgPregnant3.Image = imageCheckbox;
            //        break;
            //}

            //if (medicalData.FirstOrDefault().Conclusion == "สุขภาพสมบูรณ์ดี")
            //{
            //    imgGoodhealth.Image = imageCheckbox;
            //}
            //else if (medicalData.FirstOrDefault().Conclusion == "ไม่ผ่านการตรวจสุขภาพ")
            //{
            //    imgNotGood.Image = imageCheckbox;
            //}
            //else if (medicalData.FirstOrDefault().Conclusion != null && (medicalData.FirstOrDefault().Conclusion.Contains("วัณโรค") || medicalData.FirstOrDefault().Conclusion.Contains("โรคเรื้อน")
            //    || medicalData.FirstOrDefault().Conclusion.Contains("โรคเท้าช้าง") || medicalData.FirstOrDefault().Conclusion.Contains("โรคซิฟิลิส")))
            //{
            //    imgGoodBut.Image = imageCheckbox;
            //    if (medicalData.FirstOrDefault().Conclusion.Contains("วัณโรค"))
            //    {
            //        imgGoodButTuberculosis.Image = imageCheckbox;
            //    }

            //    if (medicalData.FirstOrDefault().Conclusion.Contains("โรคเรื้อน"))
            //    {
            //        imgGoodButLeprosy.Image = imageCheckbox;
            //    }

            //    if (medicalData.FirstOrDefault().Conclusion.Contains("โรคเท้าช้าง"))
            //    {
            //        imgGoodButElephantiasis.Image = imageCheckbox;
            //    }

            //    if (medicalData.FirstOrDefault().Conclusion.Contains("โรคซิฟิลิส"))
            //    {
            //        imgGoodButSyphilis.Image = imageCheckbox;
            //    }
            //}

        }
    }
}
