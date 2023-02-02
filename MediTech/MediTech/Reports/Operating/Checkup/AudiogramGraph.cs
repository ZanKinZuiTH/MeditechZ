using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Model;
using System.Collections.Generic;
using MediTech.DataService;
using System.Linq;
using DevExpress.DataProcessing;
using DevExpress.Xpf.CodeView;
using DevExpress.XtraCharts;
using System.IO;
using System.Windows.Media.Imaging;

namespace MediTech.Reports.Operating.Checkup
{
    public partial class AudiogramGraph : DevExpress.XtraReports.UI.XtraReport
    {

        public AudiogramGraph()
        {
            InitializeComponent();
            BeforePrint += AudiogramGraph_BeforePrint;
            
        }

        void AudiogramGraph_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            long PatientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var dataAudio = (new ReportsService()).AudiogramResult(PatientUID, PatientVisitUID);
           var GroupResult = (new ReportsService()).CheckupGroupResult(PatientUID,PatientVisitUID);
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

            //var GroupResult = (new CheckupController()).GetCheckupGroupResultListByVisit(patientUID, patientVisitUID);
            // PatientWellnessModel data = DataService.Reports.PrintWellnessBook(patientUID, patientVisitUID, payorDetailUID);


            if (dataAudio != null && dataAudio.Count > 0)
            {
                lbHN.Text = dataAudio.FirstOrDefault().PatientID;
                lbEmployeeID.Text = dataAudio.FirstOrDefault().EmployeeID;
                lbDepartment.Text = dataAudio.FirstOrDefault().Department;
                lbPatientName.Text = dataAudio.FirstOrDefault().PatientName;
                lbAge.Text = dataAudio.FirstOrDefault().Age;
                lbGender.Text = dataAudio.FirstOrDefault().Gender;
                lbBirthDttm.Text = dataAudio.FirstOrDefault().BirthDttmString;
                lbWeight.Text = dataAudio.FirstOrDefault().Weight != null ? dataAudio.FirstOrDefault().Weight + " กก." : "";
                lbHeight.Text = dataAudio.FirstOrDefault().Height != null ? dataAudio.FirstOrDefault().Height + " ซม." : "";
                lbStartDttm.Text = dataAudio.FirstOrDefault().StartDttm.Value.ToString("dd/MM/yyyy");

                foreach (Series series in audioChartLine.Series)
                {
                    if (series.Name == "ขวา")
                    {
                        foreach (var item in dataAudio.Where(p => p.ResultItemName.EndsWith("R")).OrderBy(p => int.Parse(p.ResultItemName.Replace("R", "").Trim())))
                        {
                            SeriesPoint seriesPoint1 = new SeriesPoint(int.Parse(item.ResultItemName.Replace("R","").Trim()),item.ResultValue);
                            series.Points.Add(seriesPoint1);
                        }
                    }
                    else if(series.Name == "ซ้าย")
                    {
                        foreach (var item in dataAudio.Where(p => p.ResultItemName.EndsWith("L")).OrderBy(p => int.Parse(p.ResultItemName.Replace("L", "").Trim())))
                        {
                            SeriesPoint seriesPoint1 = new SeriesPoint(int.Parse(item.ResultItemName.Replace("L", "").Trim()), item.ResultValue);
                            series.Points.Add(seriesPoint1);
                        }
                    }
                }
                //    DevExpress.XtraCharts.SeriesPoint seriesPoint1 = new DevExpress.XtraCharts.SeriesPoint(0D, new object[] {
                //((object)(50D))});

                //    series1.Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] {
                //seriesPoint1});


                TitleAudiogram.Text = "Audiogram";
                TitleAudioListResult.Text = "Result";
                TitleAudioRight.Text = "Right ear";
                TitleAudioLeft.Text = "Left ear";
                TitleAudioResult.Text = "Summary";
                TitleAudioRecommend.Text = "Suggestion";

                if (lbAudioLeft.Text == "ไม่พบความผิดปกติ")
                {
                    lbAudioLeft.Text = "Normal";
                }
                if (lbAudioRight.Text == "ไม่พบความผิดปกติ")
                {
                    lbAudioRight.Text = "Normal";
                }
                if (lbAudioResult.Text == "ปกติ")
                {
                   lbAudioResult.Text = "Normal";
                    lbAudioRecommend.Text = "Annualy check up";
                }
                if (lbAudioResult.Text == "เฝ้าระวัง")
                {
                    lbAudioResult.Text = "Mild abnormality";
                }
                if (lbAudioResult.Text == "ผิดปกตื")
                {
                   lbAudioResult.Text = "Abnormal";
                }

                lbAudioRight.Text = dataAudio.FirstOrDefault(p => p.ResultItemCode == "AUDIO8")?.ResultValue;
                lbAudioLeft.Text = dataAudio.FirstOrDefault(p => p.ResultItemCode == "AUDIO16")?.ResultValue;
          

                 lbAudioResult.Text = GroupResult.FirstOrDefault(p => p.GroupCode == "GPRST25")?.ResultStatus.ToString();
                lbAudioRecommend.Text = GroupResult.FirstOrDefault(p => p.GroupCode == "GPRST25")?.Conclusion.ToString();





            }
        }
    }
}


