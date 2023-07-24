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
    public partial class Occupational : DevExpress.XtraReports.UI.XtraReport
    {
        public Occupational()
        {
            InitializeComponent();
            BeforePrint += Occupational_BeforePrint;
        }
        void Occupational_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            long PatientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var GroupResult = (new ReportsService()).CheckupGroupResult(PatientUID, PatientVisitUID);
            var patient = (new ReportsService()).PatientInfomationWellness(PatientUID, PatientVisitUID);
            var dataOccupational = (new CheckupService()).GetCheckupMobileResultByVisitUID(PatientUID, PatientVisitUID);
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

            var data = dataOccupational.Where(p => p.ResultItemCode.Contains("TIMUS")).ToList();
            if (data != null && data.Count > 0)
            {
                lbHN.Text = patient.PatientID;
                lbEmployeeID.Text = patient.EmployeeID;
                lbDepartment.Text = patient.Department;
                lbPatientName.Text = patient.PatientName;
                lbAge.Text = patient.Age;
                lbGender.Text = patient.Gender;
                lbBirthDttm.Text = patient.BirthDttmString;
                lbWeight.Text = patient.Weight != null ? patient.Weight + " กก." : "";
                lbHeight.Text = patient.Height != null ? patient.Height + " ซม." : "";
                lbStartDttm.Text = patient.StartDttm.Value.ToString("dd/MM/yyyy");

                lbFarVision.Text = dataOccupational.FirstOrDefault(p => p.ResultItemCode == "TIMUS19")?.ResultValue;
                lbNearVision.Text = dataOccupational.FirstOrDefault(p => p.ResultItemCode == "TIMUS20")?.ResultValue;
                lb3DVision.Text = dataOccupational.FirstOrDefault(p => p.ResultItemCode == "TIMUS21")?.ResultValue;
                lbBalanceEye.Text = dataOccupational.FirstOrDefault(p => p.ResultItemCode == "TIMUS23")?.ResultValue;
                lbVisionColor.Text = dataOccupational.FirstOrDefault(p => p.ResultItemCode == "TIMUS22")?.ResultValue;
                lbFieldVision.Text = dataOccupational.FirstOrDefault(p => p.ResultItemCode == "TIMUS24")?.ResultValue;

                string eyeOccmedConclustion = GroupResult.FirstOrDefault(p => p.GroupCode == "GPRST26")?.Conclusion.ToString();
                if (!string.IsNullOrEmpty(eyeOccmedConclustion))
                {
                    string[] results = eyeOccmedConclustion.Split(',');

                    string description = "";
                    string recommand = "";
                    foreach (var item in results)
                    {
                        if (item.Contains("ควร") || item.Contains("Should"))
                        {
                            int index = item.IndexOf("ควร");
                            description += string.IsNullOrEmpty(description) ? item.Substring(0, index).Trim() : " " + item.Substring(0, index).Trim();
                            recommand = item.Substring(index).Trim();

                        }
                        else if (item != null)
                        {
                            recommand = item.ToString();
                        }


                    }

                    lbVisionOccmedResult.Text = description;
                    lbVisionOccmedRecommend.Text = recommand;
                }
            }
        }
    }
}
