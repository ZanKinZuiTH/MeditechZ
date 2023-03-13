using DevExpress.XtraReports.UI;
using MediTech.DataService;
using MediTech.Model;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace MediTech.Reports.Operating.Checkup
{
    public partial class Muscle : DevExpress.XtraReports.UI.XtraReport
    {
        public Muscle()
        {
            InitializeComponent();
            BeforePrint += Muscle_BeforePrint;
        }
        void Muscle_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            long PatientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var GroupResult = (new ReportsService()).CheckupGroupResult(PatientUID, PatientVisitUID);
            var patient = (new ReportsService()).PatientInfomationWellness(PatientUID, PatientVisitUID);
            var dataMuscle = (new CheckupService()).GetCheckupMobileResultByVisitUID(PatientUID, PatientVisitUID);
            int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());

            HealthOrganisationModel Organisation = null;
            if (logoType == 2)
            {
                Organisation = (new MasterDataService()).GetHealthOrganisationByUID(30);
            }

            if (logoType == 3)
            {
                Organisation = (new MasterDataService()).GetHealthOrganisationByUID(17);
            }

            if (Organisation != null)
            {
                if (Organisation.LogoImage != null)
                {
                    MemoryStream ms = new MemoryStream(Organisation.LogoImage);
                    xrPictureBox1.Image = Image.FromStream(ms);
                }

                xrLabel1.Text = Organisation.Description.ToString() + "\r\n" + Organisation.Address.ToString() + "\r\n" +
                    Organisation.Email.ToString() + " Tel " + Organisation.MobileNo.ToString() + "\r\n" + "เลขที่ใบอนุญาต " + Organisation.LicenseNo.ToString();
            }

            if (logoType == 1)
            {
                Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoBRXG3.png", UriKind.Absolute);
                BitmapImage imageSource = new BitmapImage(uri);
                using (MemoryStream outStream = new MemoryStream())
                {
                    BitmapEncoder enc = new BmpBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(imageSource));
                    enc.Save(outStream);
                    //this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(109.17F, 60.67F);
                    this.xrPictureBox1.Image = System.Drawing.Image.FromStream(outStream);
                }
                this.xrPictureBox1.SizeF = new System.Drawing.SizeF(170.4585F, 50.16669F);
            }

            string MyofascialTop = dataMuscle.FirstOrDefault(p => p.ResultItemCode == "PAR1301")?.ResultValue;
            string MyofascialBottom = dataMuscle.FirstOrDefault(p => p.ResultItemCode == "PAR1302")?.ResultValue;
            string NeckROM = dataMuscle.FirstOrDefault(p => p.ResultItemCode == "PAR1303")?.ResultValue;
            string RTShoulderROM = dataMuscle.FirstOrDefault(p => p.ResultItemCode == "PAR1304")?.ResultValue;
            string LTShoulderROM = dataMuscle.FirstOrDefault(p => p.ResultItemCode == "PAR1305")?.ResultValue;
            string LumbarROM = dataMuscle.FirstOrDefault(p => p.ResultItemCode == "PAR1306")?.ResultValue;
            string Conclusion;

            if (MyofascialTop != "" && MyofascialBottom != "" && NeckROM != "" && RTShoulderROM != "" && LTShoulderROM != "" && LumbarROM != "")
            {

                if (MyofascialTop == "มีความเสี่ยง" || MyofascialBottom == "มีความเสี่ยง" || NeckROM == "ผิดปกติ" || RTShoulderROM == "ผิดปกติ" || LTShoulderROM == "ผิดปกติ" || LumbarROM == "ผิดปกติ")
                {
                    Conclusion = "โครงสร้างและกล้ามเนื้ออยู่ในเกณฑ์มีความเสี่ยง หากอาการปวดหรืออาการชากระทบกับการดำเนินชีวิตประจำวัน ควรตรวจวินิจฉัยเพิ่มเติมโดยละเอียด และเข้ารับการรักษาที่เหมาะสม ร่วมกับการปรับพฤติกรรม เพื่อลดโอกาสการบาดเจ็บเรื้อรัง";
                }
                else
                {
                    Conclusion = "โครงสร้างและกล้ามเนื้ออยู่ในเกณฑ์ปกติ ควรยืดเหยียดกล้ามเนื้อและออกกำลังกายสม่ำเสมออย่างเหมาะสม เพื่อลดความเสี่ยงการบาดเจ็บของโครงสร้างและกล้ามเนื้อ";
                }
            }
        }

    }
}
