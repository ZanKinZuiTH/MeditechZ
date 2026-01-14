using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Model;
using MediTech.DataService;
using System.Collections.Generic;
using MediTech.Helpers;
using System.Linq;
using DevExpress.XtraPrinting;
using MediTech.Model.Report;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Windows.Media.Imaging;

namespace MediTech.Reports.Operating.Checkup
{
    public partial class CheckupCSR : DevExpress.XtraReports.UI.XtraReport
    {

        private MediTechDataService _DataService;

        public MediTechDataService DataService
        {
            get { return _DataService ?? (_DataService = new MediTechDataService()); }
        }

        public string PreviewWellness { get; set; }

        public CheckupCSR()
        {
            InitializeComponent();
            BeforePrint += CheckupCSR_BeforePrint;
            lbResultWellness.BeforePrint += LbResultWellness2_BeforePrint;
        }

        private void LbResultWellness2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel label = lbResultWellness;
            string text = label.Text;

            this.PrintingSystem.Graph.Font = label.Font;

            float labelWidth = label.WidthF;
            float textHeight = 0;

            switch (this.ReportUnit)
            {
                case DevExpress.XtraReports.UI.ReportUnit.HundredthsOfAnInch:
                    labelWidth = GraphicsUnitConverter.Convert(labelWidth, GraphicsUnit.Inch, GraphicsUnit.Document) / 100;
                    break;
                case DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter:
                    labelWidth = GraphicsUnitConverter.Convert(labelWidth, GraphicsUnit.Millimeter, GraphicsUnit.Document) / 10;
                    break;
            }

            SizeF size = this.PrintingSystem.Graph.MeasureString(text, (int)labelWidth);


            switch (this.ReportUnit)
            {
                case DevExpress.XtraReports.UI.ReportUnit.HundredthsOfAnInch:
                    textHeight = GraphicsUnitConverter.Convert(size.Height, GraphicsUnit.Document, GraphicsUnit.Inch) * 100;
                    break;
                case DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter:
                    textHeight = GraphicsUnitConverter.Convert(size.Height, GraphicsUnit.Document, GraphicsUnit.Millimeter) * 10;
                    break;
            }

            if (textHeight > label.HeightF)
            {
                var newSize = label.Font.Size - 0.5f;
                lbResultWellness.Font = new System.Drawing.Font(lbResultWellness.Font.Name, newSize);
                LbResultWellness2_BeforePrint(null, null);
            }
        }

        private void CheckupCSR_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            long patientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long patientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            int payorDetailUID = int.Parse(this.Parameters["PayorDetailUID"].Value.ToString());
            PatientWellnessModel data = DataService.Reports.PrintWellnessBook(patientUID, patientVisitUID, payorDetailUID);
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
                    xrPictureBox3.Image = Image.FromStream(ms);
                }
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
                    this.xrPictureBox3.LocationFloat = new DevExpress.Utils.PointFloat(559.5F, 10F);
                    this.xrPictureBox3.Image = System.Drawing.Image.FromStream(outStream);
                }
                this.xrPictureBox3.SizeF = new System.Drawing.SizeF(205.4585F, 64.16669F);
            }

            if (data.PatientInfomation != null)
            {
                var patient = data.PatientInfomation;
                var groupResult = data.GroupResult;

                #region Patient information
                lbDateCheckup.Text = patient.StartDttm != null ? patient.StartDttm.Value.ToString("dd/MM/yyyy") : "";
                lbPatientName.Text = patient.PatientName;
                lbHN.Text = patient.PatientID;
                lbEmployee.Text = patient.EmployeeID;
                lbDepartment.Text = patient.Department;
                lbPosition.Text = patient.Position;
                lbCompany.Text = !string.IsNullOrEmpty(patient.CompanyName) ? patient.CompanyName : patient.PayorName;
                //lbChildCompany.Text = patient.CompanyName;
                lbDateOfBirth.Text = patient.BirthDttm != null ? patient.BirthDttm.Value.ToString("dd/MM/yyyy") : "";
                lbAge.Text = patient.Age != null ? patient.Age : "";
                lbGender.Text = patient.Gender;

                lbHeight.Text = patient.Height != null ? patient.Height.ToString() : "";
                lbWeight.Text = patient.Weight != null ? patient.Weight.ToString() : "";
                lbBMI.Text = patient.BMI != null ? patient.BMI.ToString() : "";
                lbBP.Text = (patient.BPSys != null ? patient.BPSys.ToString() : "") + (patient.BPDio != null ? "/" + patient.BPDio.ToString() : "");
                lbPulse.Text = patient.Pulse != null ? patient.Pulse.ToString() : "";
                lbWaist.Text = patient.WaistCircumference != null ? patient.WaistCircumference.ToString() : "";

                #endregion

                #region Result Wellness

                string wellnessResult = string.Empty;
                if (!String.IsNullOrEmpty(PreviewWellness))
                {
                    wellnessResult = PreviewWellness;
                }
                else
                {
                    wellnessResult = patient.WellnessResult;
                }

                if (wellnessResult != null)
                {
                    string[] locResult = Regex.Split(wellnessResult, "[\r\n]+");
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in locResult)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            sb.AppendLine(item);
                        }
                    }
                    lbResultWellness.Text = sb.ToString();


                    // Confirmed pregnancy wins over suspected (mutual exclusive safety)
                    if (wellnessResult.Contains("ตั้งครรภ์") == true)
                    {
                        lbBMI.Text = "";
                        lbObesity.Text = "ตั้งครรภ์";
                    }
                    else if (wellnessResult.Contains("สงสัยตั้งครรภ์") == true)
                    {
                        lbBMI.Text = "";
                        lbObesity.Text = "สงสัยตั้งครรภ์";
                    }
                    else
                    {
                        lbBMI.Text = patient.BMI != null ? patient.BMI.ToString() + " kg/m2" : "";
                        if (patient.BMI != null)
                        {
                            string bmiResult = "";
                            if (patient.BMI < 18.5)
                            {
                                bmiResult = "น้ำหนักน้อย";
                            }
                            else if (patient.BMI >= 18.5 && patient.BMI <= 22.99)
                            {
                                bmiResult = "น้ำหนักปกติ";
                            }
                            else if (patient.BMI >= 23 && patient.BMI <= 24.99)
                            {
                                bmiResult = "น้ำหนักเกินเกณฑ์";
                            }
                            else if (patient.BMI >= 25 && patient.BMI <= 29.99)
                            {
                                bmiResult = "โรคอ้วนระดับที่ 1";
                            }
                            else if (patient.BMI >= 30)
                            {
                                bmiResult = "โรคอ้วนระดับที่ 2";
                            }
                            lbObesity.Text = bmiResult;

                            if (bmiResult != "น้ำหนักปกติ")
                            {
                                lbObesity.Text = bmiResult;
                            }
                        }
                    }
                }

                #endregion
            }
        }
    }
}
