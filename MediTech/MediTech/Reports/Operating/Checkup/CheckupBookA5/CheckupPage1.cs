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
using System.Windows.Media.Imaging;
using System.IO;

namespace MediTech.Reports.Operating.Checkup.CheckupBookA5
{
    public partial class CheckupPage1 : DevExpress.XtraReports.UI.XtraReport
    {

        private MediTechDataService _DataService;
        public MediTechDataService DataService
        {
            get { return _DataService ?? (_DataService = new MediTechDataService()); }
        }
        List<XrayTranslateMappingModel> dtResultMapping;
        public string PreviewWellness { get; set; }

        CheckupPage2 page2 = new CheckupPage2();
        CheckupPage3 page3 = new CheckupPage3();
        CheckupPage4 page4 = new CheckupPage4();
        CheckupPage5 page5 = new CheckupPage5();
        CheckupPage6 page6 = new CheckupPage6();
        CheckupPage7 page7 = new CheckupPage7();
        CheckupPage8 page8 = new CheckupPage8();
        CheckupPage9 page9 = new CheckupPage9();
        CheckupPage10 page10 = new CheckupPage10();
        CheckupPage11 page11 = new CheckupPage11();
        CheckupPage12 page12 = new CheckupPage12();
        LabA labA = new LabA();
        LabB labB = new LabB();
        LabC labC = new LabC();
        LabD labD = new LabD();
        LabE labE = new LabE();
        LabF labF = new LabF();
        LabG labG = new LabG();
        LabH labH = new LabH();


        public CheckupPage1()
        {
            InitializeComponent();
            BeforePrint += CheckupPage1_BeforePrint;
            AfterPrint += CheckupPage1_AfterPrint;
            page12.lbResultWellness2.BeforePrint += LbResultWellness2_BeforePrint;
        }

        private void LbResultWellness2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel label = page11.lbResultWellness;
            string text = label.Text;

            this.PrintingSystem.Graph.Font = label.Font;

            float labelWidth = label.WidthF;
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

            float textHeight = 0;

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
                float diffHeight = textHeight - label.HeightF;
                var result1 = page11.lbResultWellness.Lines.Where(p => !string.IsNullOrEmpty(p)).ToList();
                var result2 = page12.lbResultWellness2.Lines.Where(p => !string.IsNullOrEmpty(p)).ToList();
                //List<string> result1List = new List<string>();
                //List<string> result2List = new List<string>();

                string lastResult = result1[result1.Count - 1];
                result2.Insert(0, lastResult);
                result1.Remove(lastResult);
                //if (diffHeight > 35)
                //{
                //    string lastSecondResult = result1[result1.Count - 2];
                //    result2List.Add(lastSecondResult);
                //    result1.Remove(lastSecondResult);
                //}

                //result1List.AddRange(result1);

                page11.lbResultWellness.Lines = result1.ToArray();
                page12.lbResultWellness2.Lines = result2.ToArray();
                LbResultWellness2_BeforePrint(null, null);
            }
        }

        private void CheckupPage1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            long patientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long patientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            int payorDetailUID = int.Parse(this.Parameters["PayorDetailUID"].Value.ToString());
            PatientWellnessModel data = DataService.Reports.PrintWellnessBook(patientUID, patientVisitUID, payorDetailUID);
            int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());

            //int logoType = int.Parse(this.Parameters["LogoType"].Value.ToString());
         
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
                    this.xrPictureBox3.LocationFloat = new DevExpress.Utils.PointFloat(320.61F, 20F);
                    this.xrPictureBox3.Image = System.Drawing.Image.FromStream(outStream);
                }
                this.xrPictureBox3.SizeF = new System.Drawing.SizeF(205.4585F, 64.16669F);
            }


            page10.RowRisk1.Visible = false;
            page10.RowRisk2.Visible = false;

            if (data.PatientInfomation != null)
            {
                var patient = data.PatientInfomation;
                var groupResult = data.GroupResult;

                RN.Text = patient.PatientOtherID;

                #region Show HN/Name
                page2.lbHN2.Text = patient.PatientID;
                page2.lbName2.Text = patient.PatientName;
                page11.lbHN11.Text = patient.PatientID;
                page11.lbName11.Text = patient.PatientName;

                page3.lbHN3.Text = patient.PatientID;
                page3.lbName3.Text = patient.PatientName;
                page10.lbHN10.Text = patient.PatientID;
                page10.lbName10.Text = patient.PatientName;

                page4.lbHN4.Text = patient.PatientID;
                page4.lbName4.Text = patient.PatientName;
                page9.lbHN9.Text = patient.PatientID;
                page9.lbName9.Text = patient.PatientName;

                page5.lbHN5.Text = patient.PatientID;
                page5.lbName5.Text = patient.PatientName;
                page8.lbHN8.Text = patient.PatientID;
                page8.lbName8.Text = patient.PatientName;

                page6.lbHN6.Text = patient.PatientID;
                page6.lbName6.Text = patient.PatientName;
                page7.lbHN7.Text = patient.PatientID;
                page7.lbName7.Text = patient.PatientName;

                #endregion

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
                lbAddress.Text = patient.PatientAddress != null ? patient.PatientAddress : "";
                lbPhoneNumber.Text = patient.MobilePhone != null ? patient.MobilePhone : "";

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
                    //StringBuilder sb2 = new StringBuilder();
                    int line = 0;
                    foreach (var item in locResult)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            //if (line < 14)
                            //{
                            //    sb.AppendLine(item);
                            //}
                            //else
                            //{
                            //    sb2.AppendLine(item);
                            //}
                            sb.AppendLine(item);
                            line++;
                        }
                    }

                    page11.lbResultWellness.Text = sb.ToString();
                    //lbResultWellness2.Text = sb2.ToString();


                    if (wellnessResult.Contains("สงสัยตั้งครรภ์") == true)
                    {
                        lbBMI.Text = "";
                        lbObesity.Text = "สงสัยตั้งครรภ์";
                    }
                    else if (wellnessResult.Contains("ตั้งครรภ์") == true)
                    {
                        lbBMI.Text = "";
                        lbObesity.Text = "ตั้งครรภ์";
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

                #region Radiology

                string chestPA = groupResult.FirstOrDefault(p => p.GroupCode == "GPRST4")?.Conclusion;
                string mammogram = groupResult.FirstOrDefault(p => p.GroupCode == "GPRST5")?.Conclusion;
                string ultrasound = groupResult.FirstOrDefault(p => p.GroupCode == "GPRST6")?.Conclusion;
                string ultrasound_thyroid = groupResult.FirstOrDefault(p => p.GroupCode == "GPRST62")?.Conclusion;

                if (!string.IsNullOrEmpty(chestPA))
                {
                    page8.lbChest.Text = chestPA;
                }
                else
                {
                    page8.lbChest.Text = "-";
                }

                if (!string.IsNullOrEmpty(mammogram))
                {
                    page8.lbMam.Text = mammogram;
                }
                else
                {
                    page8.lbMam.Text = "-";
                }

                if (!string.IsNullOrEmpty(ultrasound))
                {
                    page8.lbUlt.Text = ultrasound;
                }
                else
                {
                    page8.lbUlt.Text = "-";
                }


                if (!string.IsNullOrEmpty(ultrasound_thyroid))
                {
                    page8.lbThyroid.Text = ultrasound_thyroid;
                }
                else
                {
                    page8.lbThyroid.Text = "-";
                }

                var radilogy = data.Radiology;
                if (radilogy.FirstOrDefault(p => !string.IsNullOrEmpty(p.RequestItemName)) != null)
                {
                    if (radilogy.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("chest")) != null)
                    {
                        ResultRadiologyModel chestXray = radilogy.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("chest"));
                        if (!string.IsNullOrEmpty(chestXray.PlainText))
                        {
                            if (patient.SPOKLUID == 4240)
                            {
                                string chestEN = chestXray.PlainText;
                                string[] ChestResult = chestEN.Split(new string[] { "IMPRESSION", "Impression", "impression" }, StringSplitOptions.None);
                                if (ChestResult.Length > 1)
                                {
                                    string ResultChestEn = ChestResult[1].Replace(":", "");
                                    ResultChestEn = ResultChestEn.Trim();
                                    if (ResultChestEn.ToLower().Contains("negative study"))
                                    {
                                        page8.lbChest.Text = "Negative";
                                    }
                                    else
                                    {
                                        page8.lbChest.Text = ResultChestEn;
                                    }
                                }
                                else if (ChestResult[0].ToLower().Contains("old fracture right clavicle"))
                                {
                                    page8.lbChest.Text = "Calcification in aorta Old fracture of right clavicle";
                                }
                            }
                            //else
                            //{
                            //    string resultChestThai = TranslateXray(chestXray.PlainText, chestXray.ResultStatus, chestXray.RequestItemName);
                            //    if (!string.IsNullOrEmpty(resultChestThai))
                            //    {
                            //        page6.lbChest.Text = resultChestThai;
                            //        if (page6.lbChest.Text != null && page6.lbChest.Text.Length > 400)
                            //        {
                            //            page6.lbChest.Font = new Font("Angsana New", 9);
                            //        }
                            //    }
                            //    else
                            //    {
                            //        page6.lbChest.Text = "ยังไม่ได้แปลไทย";
                            //    }
                            //}
                        }
                    }
                    //else
                    //{
                    //    page6.lbChest.Text = "-";
                    //}

                    if (radilogy.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("mammo")) != null)
                    {
                        ResultRadiologyModel mammoGram = radilogy.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("mammo"));
                        //page6.lbMam.Text = mammoGram.ResultStatus;
                        if (!string.IsNullOrEmpty(mammoGram.PlainText))
                        {
                            if (patient.SPOKLUID == 4240)
                            {
                                string mamEN = mammoGram.PlainText;
                                string[] MamResult = mamEN.Split(new string[] { "IMPRESSION", "Impression", "impression" }, StringSplitOptions.None);
                                string MamResultEn;
                                if (MamResult.Length > 1)
                                {
                                    MamResultEn = MamResult[1].Replace("\n", "").Replace("\r", "").Replace("\r\n", "");
                                }
                                else
                                {
                                    MamResultEn = MamResult[1].Replace("\n", "").Replace("\r", "").Replace("\r\n", "");
                                }
                                //string MamResultEn = MamResult[1].Replace("\n", "").Replace("\r", "").Replace("\r\n", "");
                                MamResultEn = MamResultEn.Trim();
                                page8.lbMam.Text = MamResultEn;
                            }
                            //else
                            //{
                            //    string resultChestThai = TranslateXray(mammoGram.PlainText, mammoGram.ResultStatus, mammoGram.RequestItemName);
                            //    if (!string.IsNullOrEmpty(resultChestThai))
                            //    {
                            //        page6.lbMam.Text = resultChestThai;
                            //        if (page6.lbMam.Text != null && page6.lbMam.Text.Length > 400)
                            //        {
                            //            page6.lbMam.Font = new Font("Angsana New", 9);
                            //        }
                            //    }
                            //    else
                            //    {
                            //        page6.lbMam.Text = "ยังไม่ได้แปลไทย";
                            //    }
                            //}
                        }
                    }
                    //else
                    //{
                    //    page6.lbMam.Text = "-";
                    //}

                    if (radilogy.FirstOrDefault(p => (p.RequestItemName.ToLower().Contains("ultrasound") || p.RequestItemName.ToLower().Contains("US"))) != null)
                    {
                        ResultRadiologyModel ultrsound = radilogy.FirstOrDefault(p => (p.RequestItemName.ToLower().Contains("ultrasound") || p.RequestItemName.ToLower().Contains("US")));
                        //page6.lbUlt.Text = ultrsound.ResultStatus;
                        if (!string.IsNullOrEmpty(ultrsound.PlainText))
                        {
                            if (patient.SPOKLUID == 4240)
                            {
                                string UltEN = ultrsound.PlainText;
                                string[] UltResult = UltEN.Split(new string[] { "IMPRESSION", "Impression", "impression" }, StringSplitOptions.None);
                                string UltResultEn;
                                if (UltResult.Length > 1)
                                {
                                    UltResultEn = UltResult[1].Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(":", "");
                                }
                                else
                                {
                                    UltResultEn = UltResult[0].Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(":", "");
                                }
                                //string UltResultEn = UltResult[1].Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(":", "");
                                
                                UltResultEn = UltResultEn.Trim();
                                page8.lbUlt.Text = UltResultEn;
                            }
                            //else
                            //{
                            //    string resultChestThai = TranslateXray(ultrsound.PlainText, ultrsound.ResultStatus, ultrsound.RequestItemName);
                            //    if (!string.IsNullOrEmpty(resultChestThai))
                            //    {
                            //        page6.lbUlt.Text = resultChestThai;
                            //        if (page6.lbUlt.Text != null && page6.lbUlt.Text.Length > 400)
                            //        {
                            //            page6.lbUlt.Font = new Font("Angsana New", 9);
                            //        }
                            //    }
                            //    else
                            //    {
                            //        page6.lbUlt.Text = "ยังไม่ได้แปลไทย";
                            //    }
                            //}
                        }
                    }
                    //else
                    //{
                    //    page6.lbUlt.Text = "-";
                    //}
                }

                #endregion

                page8.lbEKGRecommend.Text = groupResult.FirstOrDefault(p => p.GroupCode == "GPRST23")?.Conclusion;

                #region Hide Toxico/ other lab

                page7.RowIsopropanol.Visible = false;
                page7.RowStyreneUrine.Visible = false;
                page7.RowAluminiumBlood.Visible = false;
                page7.RowArsenic.Visible = false;
                page7.CyclohexanoneRow.Visible = false;
                page7.RowPhenol.Visible = false;
                page7.RowMibkUrine.Visible = false;
                page7.RowCadmiumUrine.Visible = false;
                page7.RowEthylbenzeneUrine.Visible = false;
                page7.RowMercuryUrine.Visible = false;
                page7.RowMEK.Visible = false;
                page7.RowCarboxy.Visible = false;
                page7.RowBenzene.Visible = false;
                page7.RowMethanol.Visible = false;
                page7.RowMethyrene.Visible = false;
                page7.RowHexane.Visible = false;
                page7.RowNickelUrine.Visible = false;
                page7.RowMibkUrine.Visible = false;
                page7.RowEthylbenzeneUrine.Visible = false;
                page7.RowMercuryUrine.Visible = false;
                page7.RowMethyreneUrine.Visible = false;
                page7.RowBenzenettUrine.Visible = false;
                page7.RowMercuryBlood.Visible = false;
                page7.Rowfluoride.Visible = false;
                page7.RowFormadehyde.Visible = false;
                page7.Row25Hexan.Visible = false;
                page7.RowManganese.Visible = false;
                page7.RowZinc.Visible = false;
                page7.RowIron.Visible = false;
                page7.RowCadInb.Visible = false;
                page7.RowChroinB.Visible = false;
                page7.RowChroinS.Visible = false;
                page7.RowAmmo.Visible = false;
                page7.RowLeadInu.Visible = false;
                page7.RowCholinesteraseBlood.Visible = false;
                page7.RowThinnerUrine.Visible = false;
                page7.RowCopperBlood.Visible = false;
                page7.RowTrichloroUrine.Visible = false;
                page7.rowDirectToluene.Visible = false;
                page7.RowEthanolBlood.Visible = false;
                page6.RowHpylori.Visible = false;
                page6.RowPhosphorus.Visible = false;
                page6.RowVDRL.Visible = false;
                #endregion

                #region Hide Liver function

                page5.RowAlbumin.Visible = false;
                page5.RowGlobulin.Visible = false;
                page5.RowGGT.Visible = false;

                #endregion

                IOrderedEnumerable<PatientResultComponentModel> labCompare;
                if (data.LabCompare != null)
                {
                    labCompare = data.LabCompare.OrderByDescending(p => p.Year);

                    if(labCompare != null)
                    {
                        #region Complete Blood Count

                        IEnumerable<PatientResultComponentModel> cbcTestSet = labCompare
                        .Where(p => p.RequestItemName.Contains("CBC"))
                        .OrderByDescending(p => p.Year);
                        GenerateCompleteBloodCount(cbcTestSet);
                        #endregion

                        #region Urinalysis
                        IEnumerable<PatientResultComponentModel> uaTestSet = labCompare
                            .Where(p => p.RequestItemName.Contains("UA"))
                            .OrderByDescending(p => p.Year);
                        GenerateUrinalysis(uaTestSet);

                        #endregion

                        #region Renal function
                        IEnumerable<PatientResultComponentModel> RenalTestSet = labCompare
                            .Where(p => p.RequestItemCode.Contains("LAB212")
                            || p.RequestItemCode.Contains("LAB211")
                            || p.RequestItemCode.Contains("LAB213"))
                            .OrderByDescending(p => p.Year);
                        GenerateRenalFunction(RenalTestSet);

                        #endregion

                        #region Fasting Blood Sugar
                        IEnumerable<PatientResultComponentModel> FbsTestSet = labCompare
                            .Where(p => p.RequestItemCode.Contains("LAB231")
                            || p.RequestItemCode.Contains("LAB232"))
                             .OrderByDescending(p => p.Year);
                        GenerateFastingBloodSugar(FbsTestSet);

                        #endregion

                        #region Uric acid
                        IEnumerable<PatientResultComponentModel> UricTestSet = labCompare
                            .Where(p => p.RequestItemCode.Contains("LAB261"))
                            .OrderByDescending(p => p.Year);
                        GenerateUricAcid(UricTestSet);

                        #endregion

                        #region Lipid Profiles 
                        IEnumerable<PatientResultComponentModel> LipidTestSet = labCompare
                            .Where(p => p.RequestItemCode.Contains("LAB241")
                            || p.RequestItemCode.Contains("LAB242")
                            || p.RequestItemCode.Contains("LAB243")
                            || p.RequestItemCode.Contains("LAB244"))
                             .OrderByDescending(p => p.Year);
                        GenerateLipidProfiles(LipidTestSet);

                        #endregion

                        #region Liver Function
                        IEnumerable<PatientResultComponentModel> LiverTestSet = labCompare
                            .Where(p => p.RequestItemCode.Contains("LAB221")
                            || p.RequestItemCode.Contains("LAB222")
                            || p.RequestItemCode.Contains("LAB223")
                            || p.RequestItemCode.Contains("LAB474")
                            || p.RequestItemCode.Contains("LAB475")
                            || p.RequestItemCode.Contains("LAB503")
                            || p.RequestItemCode.Contains("LAB225")
                            || p.RequestItemCode.Contains("LAB226")
                            || p.RequestItemCode.Contains("LAB227")
                            )

                             .OrderByDescending(p => p.Year);
                        GenerateLiverFunction(LiverTestSet);
                        #endregion

                        #region Immunology and Virology
                        IEnumerable<PatientResultComponentModel> ImmunologyTestSet = labCompare
                            .Where(p => p.RequestItemCode.Contains("LAB451")
                            || p.RequestItemCode.Contains("LAB441")
                            || p.RequestItemCode.Contains("LAB512")
                            || p.RequestItemCode.Contains("LAB554")
                            || p.RequestItemCode.Contains("LAB595")
                            || p.RequestItemCode.Contains("LAB596")
                            || p.RequestItemCode.Contains("LAB582")
                            || p.RequestItemCode.Contains("LAB452")
                            )
                             .OrderByDescending(p => p.Year);
                        GenerateImmunology(ImmunologyTestSet);
                        #endregion

                        #region Stool Exam
                        IEnumerable<PatientResultComponentModel> StoolTestSet = labCompare
                            .Where(p => p.RequestItemName.Contains("Stool Examination"))
                             .OrderByDescending(p => p.Year);
                        GenerateStool(StoolTestSet);
                        #endregion

                        #region Stool Culture
                        IEnumerable<PatientResultComponentModel> StoolCultureTestSet = labCompare
                            .Where(p => p.RequestItemCode.Contains("LAB322"))
                             .OrderByDescending(p => p.Year);
                        GenerateStoolCulture(StoolCultureTestSet);
                        #endregion

                        #region tumor marker
                        IEnumerable<PatientResultComponentModel> TumorMarker = labCompare
                            .Where(p => p.RequestItemCode.Contains("LAB281") //afp
                            || p.RequestItemCode.Contains("LAB282") //ca19-9
                            || p.RequestItemCode.Contains("LAB283") //cea
                            || p.RequestItemCode.Contains("LAB284") //psa
                            || p.RequestItemCode.Contains("LAB285") //ca125
                            || p.RequestItemCode.Contains("LAB286")) //ca153
                         .OrderByDescending(p => p.Year);
                        GenerateTumorMarker(TumorMarker);
                        #endregion

                        #region Toxicology
                        IEnumerable<PatientResultComponentModel> ToxicoTestSet = labCompare
                            .Where(p => p.RequestItemCode.Contains("LAB508")
                            || p.RequestItemCode.Contains("LAB517")
                            || p.RequestItemCode.Contains("LAB516")
                            || p.RequestItemCode.Contains("LAB314")
                            || p.RequestItemCode.Contains("LAB319")
                            || p.RequestItemCode.Contains("LAB414")
                            || p.RequestItemCode.Contains("LAB510")
                            || p.RequestItemCode.Contains("LAB477")
                            || p.RequestItemCode.Contains("LAB510")
                            || p.RequestItemCode.Contains("LAB315")
                            || p.RequestItemCode.Contains("LAB317")
                            || p.RequestItemCode.Contains("LAB325")
                            || p.RequestItemCode.Contains("LAB323")
                            || p.RequestItemCode.Contains("LAB324")
                            || p.RequestItemCode.Contains("LAB519")
                            || p.RequestItemCode.Contains("LAB558")
                            || p.RequestItemCode.Contains("LAB518")
                            || p.RequestItemCode.Contains("LAB560")
                            || p.RequestItemCode.Contains("LAB561") //Arsenic 
                            || p.RequestItemCode.Contains("LAB562") //Cyclohexanone
                            || p.RequestItemCode.Contains("LAB316") //Phenol
                            || p.RequestItemCode.Contains("LAB570") //MIBK
                            || p.RequestItemCode.Contains("LAB571") //Cadmium in Urine
                            || p.RequestItemCode.Contains("LAB572") //Ethylbenzene
                            || p.RequestItemCode.Contains("LAB489") //Mercury
                            || p.RequestItemCode.Contains("LAB573") //Methyrene chloride in Urine
                            || p.RequestItemCode.Contains("LAB568") //Benzene (t,t-Muconic acid) in Urine
                            || p.RequestItemCode.Contains("LAB488") //Mercury in blood
                            || p.RequestItemCode.Contains("LAB584") //fluoride  in Urine
                            || p.RequestItemCode.Contains("LAB513") //formadehyde in Urine
                            || p.RequestItemCode.Contains("LAB276") //25hex
                            || p.RequestItemCode.Contains("LAB588") //Manganes in blood
                            || p.RequestItemCode.Contains("LAB587") //Cadmium in Blood
                            || p.RequestItemCode.Contains("LAB547") // Zinc in zerum
                            || p.RequestItemCode.Contains("LAB463") // Iron zerum
                            || p.RequestItemCode.Contains("LAB542") // chro zerum
                            || p.RequestItemCode.Contains("LAB575")// Ammo
                            || p.RequestItemCode.Contains("LAB487") //Lead in Urin 
                            || p.RequestItemCode.Contains("LAB511")
                            || p.RequestItemCode.Contains("LAB620") //direct Toluene 
                            || p.RequestItemCode.Contains("LAB619")
                            || p.RequestItemCode.Contains("LAB589")
                            || p.RequestItemCode.Contains("LAB543")//copper blood
                            || p.RequestItemCode.Contains("LAB606"))
                             .OrderByDescending(p => p.Year);
                        GenerateToxicology(ToxicoTestSet);
                        #endregion

                        #region Other Lab Teat
                        IEnumerable<PatientResultComponentModel> OtherTestSet = labCompare
                            .Where(p => p.RequestItemCode.Contains("LAB411") //blood 
                            || p.RequestItemCode.Contains("LAB251") //calcium
                            || p.RequestItemCode.Contains("LAB271") //tsh
                            || p.RequestItemCode.Contains("LAB272") //T3
                            || p.RequestItemCode.Contains("LAB273") //T4
                            || p.RequestItemCode.Contains("LAB274") //FreeT3
                            || p.RequestItemCode.Contains("LAB275") //FreeT4
                            || p.RequestItemCode.Contains("LAB618")
                            || p.RequestItemCode.Contains("LAB431")
                            || p.RequestItemCode.Contains("LAB565")
                             ).OrderByDescending(p => p.Year);
                        GenerateOther(OtherTestSet);
                        #endregion

                    }
                }

                var occmed = data.MobileResult;
                if (occmed != null)
                {
                    IEnumerable<PatientResultComponentModel> timusResult = occmed
                        .Where(p => p.RequestItemCode.Contains("TIMUS"));
                    GenerateTimus(timusResult);

                    IEnumerable<PatientResultComponentModel> AudioResult = occmed
                        .Where(p => p.RequestItemCode.Contains("AUDIO"));
                    GenerateAudio(AudioResult);

                    IEnumerable<PatientResultComponentModel> SpiroResult = occmed
                        .Where(p => p.RequestItemCode.Contains("SPIRO"));
                    GenerateSpiro(SpiroResult);
                    
                    IEnumerable<PatientResultComponentModel> PhysicalExam = occmed
                        .Where(p => p.RequestItemCode.Contains("PEXAM"));
                    GeneratePhysicalExam(PhysicalExam);

                    IEnumerable<PatientResultComponentModel> PhysicalExamRisk = occmed
                        .Where(p => p.RequestItemCode.Contains("PEAX01"));
                    GeneratePhysicalExamRisk(PhysicalExamRisk);

                    IEnumerable<PatientResultComponentModel> BackStrength = occmed
                        .Where(p => p.RequestItemCode.Contains("MUSCLEBA")
                        || p.RequestItemCode.Contains("MUSCLEGR")
                        || p.RequestItemCode.Contains("MUSCLELEG"));
                    GenerateBackStrength(BackStrength);

                    IEnumerable<PatientResultComponentModel> VisualAcuityTest = occmed
                        .Where(p => p.RequestItemCode.Contains("VISAY"));
                    GenerateVisualAcuityTest(VisualAcuityTest);
                }

                if (groupResult != null)
                {
                    IEnumerable<CheckupGroupResultModel> occmedGroupResult = groupResult
                        .Where(p => p.GroupCode.Contains("GPRST33")  //spiro
                        || p.GroupCode.Contains("GPRST26") //timus
                        || p.GroupCode.Contains("GPRST25") //audio
                        || p.GroupCode.Contains("GPRST32")); //BackStrength
                    GenerateOccmedGroup(occmedGroupResult);
                }
                if (patient.SPOKLUID == 4240)
                {
                    page12.TitleResultWellness2.Text = "Summary";
                    page11.TitleResultWellness.Text = "Summary";
                    TitleObesity.Text = "BMI Interpretation";
                    lbPulse.Text = patient.Pulse != null ? patient.Pulse.ToString() + " times/min" : "";

                    if (patient.BMI != null)
                    {
                        string bmiResult = "";
                        if (patient.BMI < 18.5)
                        {
                            bmiResult = "Less weight";
                        }
                        else if (patient.BMI >= 18.5 && patient.BMI <= 22.99)
                        {
                            bmiResult = "Normal";
                        }
                        else if (patient.BMI >= 23 && patient.BMI <= 24.99)
                        {
                            bmiResult = "Overweight";
                        }
                        else if (patient.BMI >= 25 && patient.BMI <= 29.99)
                        {
                            bmiResult = "Obesity Class 1";
                        }
                        else if (patient.BMI >= 30)
                        {
                            bmiResult = "Obesity class 2";
                        }
                        lbObesity.Text = bmiResult;

                        if (bmiResult != "Normal weight")
                        {
                            lbObesity.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }
                    }

                    if (page2.lbEye.Text.Trim().Contains("ไม่พบความผิดปกติ"))
                    {
                        page2.lbEye.Text = "Normal";
                    }
                    if (page2.lbEars.Text.Trim().Contains("ไม่พบความผิดปกติ"))
                    {
                        page2.lbEars.Text = "Normal";
                    }
                    if (page2.lbThroat.Text.Trim().Contains("ไม่พบความผิดปกติ"))
                    {
                        page2.lbThroat.Text = "Normal";
                    }
                    if (page2.lbNose.Text.Trim().Contains("ไม่พบความผิดปกติ"))
                    {
                        page2.lbNose.Text = "Normal";
                    }
                    if (page2.lbTeeth.Text.Trim().Contains("ไม่พบความผิดปกติ"))
                    {
                        page2.lbTeeth.Text = "Normal";
                    }
                    if (page2.lbLung.Text.Trim().Contains("ไม่พบความผิดปกติ"))
                    {
                        page2.lbLung.Text = "Normal";
                    }
                    if (page2.lbHeart.Text.Trim().Contains("ไม่พบความผิดปกติ"))
                    {
                        page2.lbHeart.Text = "Normal";
                    }
                    if (page2.lbSkin.Text.Trim().Contains("ไม่พบความผิดปกติ"))
                    {
                        page2.lbSkin.Text = "Normal";
                    }
                    if (page2.lbThyroid.Text.Trim().Contains("ไม่พบความผิดปกติ"))
                    {
                        page2.lbThyroid.Text = "Normal";
                    }
                    if (page2.lbLymphNode.Text.Trim().Contains("ไม่พบความผิดปกติ"))
                    {
                        page2.lbLymphNode.Text = "Normal";
                    }

                    if (page2.lbSmoke.Text.Trim().Contains("ปฏิเสธ"))
                    {
                        page2.lbSmoke.Text = "Denial";
                    }
                    if (page2.lbSmoke.Text.Trim().Contains("10-20มวน/วัน"))
                    {
                        page2.lbSmoke.Text = "0.5-1 pack/day";
                    }


                    if (page2.lbDrugAllergy.Text.Trim().Contains("ปฏิเสธ"))
                    {
                        page2.lbDrugAllergy.Text = "Denial";
                    }


                    if (page2.lbAlcohol.Text.Trim().Contains("ปฏิเสธ"))
                    {
                        page2.lbAlcohol.Text = "Denial";
                    }
                    if (page2.lbAlcohol.Text.Trim().Contains("ดื่มตามโอกาส"))
                    {
                        page2.lbAlcohol.Text = "Social drinking";
                    }
                    if (page2.lbAlcohol.Text.Trim().Contains("6-7ครั้ง/สัปดาห์"))
                    {
                        page2.lbAlcohol.Text = "6-7 times/week";
                    }
                    if (page2.lbAlcohol.Text.Trim().Contains("2 ครั้ง/สัปดาห์"))
                    {
                        page2.lbAlcohol.Text = "2 times/week";
                    }
                    if (page2.lbAlcohol.Text.Trim().Contains("ดื่มทุกวัน"))
                    {
                        page2.lbAlcohol.Text = "everyday";
                    }


                    if (page2.lbUnderlying.Text.Trim().Contains("ปฏิเสธ"))
                    {
                        page2.lbUnderlying.Text = "Denial";
                    }
                    if (page2.lbUnderlying.Text.Trim().Contains("ความดันโลหิตสูง"))
                    {
                        page2.lbUnderlying.Text = "Hypertension";
                    }

                    page9.TitlePulmonaryResult.Text = "Summary";
                    page9.TitlePulmonaryRecommend.Text = "Suggestion";

                    if (page9.lbLungResult.Text == "ปกติ")
                    {
                        page9.lbLungResult.Text = "Normal";
                        page9.lbLungRecommend.Text = "Annualy check up";
                    }
                    if (page9.lbLungResult.Text == "ผิดปกติ")
                    {
                        page9.lbLungResult.Text = "Abnormal";
                    }

                    page9.TitleFarVision.Text = "Far Test";
                    page9.TitleNearVision.Text = "Near Test";
                    page9.Title3DVision.Text = "3D Test";
                    page9.TitleBalanceEye.Text = "Eye Balance";
                    page9.TitleColor.Text = "Color";
                    page9.TitleFieldVision.Text = "Visual Field";
                    page9.TitleVisionOccmedResult.Text = "Summary";
                    page9.TitleVisionOccmedRecommend.Text = "Suggestion";

                    if (page9.lbVisionOccmedResult.Text == "ปกติ")
                    {
                        page9.lbVisionOccmedResult.Text = "Normal";
                    }
                    if (page9.lbVisionOccmedResult.Text == "ผิดปกติ")
                    {
                        page9.lbVisionOccmedResult.Text = "Abnormal";
                    }

                    page9.TitleAudiogram.Text = "Audiogram";
                    page9.TitleAudioListResult.Text = "Result";
                    page9.TitleAudioRight.Text = "Right ear";
                    page9.TitleAudioLeft.Text = "Left ear";
                    page9.TitleAudioResult.Text = "Summary";
                    page9.TitleAudioRecommend.Text = "Suggestion";

                    if (page9.lbAudioLeft.Text == "ไม่พบความผิดปกติ")
                    {
                        page9.lbAudioLeft.Text = "Normal";
                    }
                    if (page9.lbAudioRight.Text == "ไม่พบความผิดปกติ")
                    {
                        page9.lbAudioRight.Text = "Normal";
                    }
                    if (page9.lbAudioResult.Text == "ปกติ")
                    {
                        page9.lbAudioResult.Text = "Normal";
                        page9.lbAudioRecommend.Text = "Annualy check up";
                    }
                    if (page9.lbAudioResult.Text == "เฝ้าระวัง")
                    {
                        page9.lbAudioResult.Text = "Mild abnormality";
                    }
                    if (page9.lbAudioResult.Text == "ผิดปกตื")
                    {
                        page9.lbAudioResult.Text = "Abnormal";
                    }

                    page10.TitleMyopiaRight.Text = "Shortsighted Rt.";
                    page10.TitleMyopiaLeft.Text = "Shortsighted Lt.";
                    page10.TitleAstigmaticRight.Text = "Astigmatic Rt.";
                    page10.TitleAstigmaticLeft.Text = "Astigmatic Lt.";
                    page10.TitleViewRight.Text = "Degree Rt.";
                    page10.TitleViewLeft.Text = "Degree Lt.";
                    page10.TitleHyperopiaRight.Text = "Longsighted Rt.";
                    page10.TitleHyperopiaLeft.Text = "Longsighted Lt.";
                    page10.TitleVARight.Text = "VA Rt.";
                    page10.TitleVALeft.Text = "VA Lt.";
                    page10.TitleBlindColor.Text = "Color Blindness";
                    page10.TitleViewResult.Text = "Result";
                    page10.TitleViewRecommend.Text = "Suggestion";
                }
            }
        }


        #region Lab Result
        private void GenerateCompleteBloodCount(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            page3.rowCBC_RBCMor.Visible = false;
            if (labTestSet != null && labTestSet.Count() > 0)
            {

                List<int?> oneYears = labTestSet.Select(p => p.Year).Distinct().ToList();
                if(oneYears.Count == 1)
                {
                    List<DateTime?> repeatYears = labTestSet.Select(p => p.StartDttm).Distinct().ToList();
                    repeatYears.Sort();

                    if (repeatYears.Count > 1)
                    {
                        DateTime? dateMax = DateTime.Now;
                        DateTime? dateMin = repeatYears[0];
                        foreach (DateTime dt in repeatYears)
                        {

                            if (dt > dateMin)
                            {
                                dateMax = dt;
                            }
                        }
                        int yearMax = dateMax.Value.Year;
                        int yearMin = yearMax - 1;

                        labTestSet.Where(w => w.StartDttm == dateMin).ToList().ForEach(i => i.Year = yearMin);
                    }
                }

                int thisYear = DateTime.Now.Year;
                int? compare = thisYear - 2;
                List<int?> Years = labTestSet.Select(p => p.Year).Where(p => p.Value >= compare).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();

                if (countYear != 0)
                {


                    int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                    int? year2 = countYear >= 2 ? (Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1) : year1 - 1;
                    int? year3 = countYear >= 3 ? (Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1) : year2 - 1;
                    page3.cellCBCYear1.Text = "ปี" + " " + year1.ToString();
                    page3.cellCBCYear2.Text = "ปี" + " " + year2.ToString();
                    page3.cellCBCYear3.Text = "ปี" + " " + year3.ToString();

                    page3.cellHbRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001")?.ReferenceRange;
                    page3.cellHb1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001" && p.Year == year1)?.ResultValue;
                    page3.cellHb2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001" && p.Year == year2)?.ResultValue;

                    page3.cellHb3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001" && p.Year == year3)?.ResultValue;

                    page3.cellHctRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020")?.ReferenceRange;
                    page3.cellHct1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020" && p.Year == year1)?.ResultValue;
                    page3.cellHct2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020" && p.Year == year2)?.ResultValue;
                    page3.cellHct3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020" && p.Year == year3)?.ResultValue;

                    page3.cellMcvRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025")?.ReferenceRange;
                    page3.cellMcv1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025" && p.Year == year1)?.ResultValue;
                    page3.cellMcv2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025" && p.Year == year2)?.ResultValue;
                    page3.cellMcv3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025" && p.Year == year3)?.ResultValue;

                    page3.cellMchRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030")?.ReferenceRange;
                    page3.cellMch1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030" && p.Year == year1)?.ResultValue;
                    page3.cellMch2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030" && p.Year == year2)?.ResultValue;
                    page3.cellMch3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030" && p.Year == year3)?.ResultValue;

                    page3.cellMchcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035")?.ReferenceRange;
                    page3.cellMchc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035" && p.Year == year1)?.ResultValue;
                    page3.cellMchc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035" && p.Year == year2)?.ResultValue;
                    page3.cellMchc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035" && p.Year == year3)?.ResultValue;

                    page3.cellRdwRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1")?.ReferenceRange;
                    page3.cellRdw1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1" && p.Year == year1)?.ResultValue;
                    page3.cellRdw2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1" && p.Year == year2)?.ResultValue;
                    page3.cellRdw3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1" && p.Year == year3)?.ResultValue;


                    page3.cellRbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428")?.ReferenceRange;
                    page3.cellRbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428" && p.Year == year1)?.ResultValue;
                    page3.cellRbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428" && p.Year == year2)?.ResultValue;
                    page3.cellRbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428" && p.Year == year3)?.ResultValue;

                    page3.cellRbcMorRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13")?.ReferenceRange;
                    page3.cellRbcMor1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13" && p.Year == year1)?.ResultValue;
                    page3.cellRbcMor2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13" && p.Year == year2)?.ResultValue;
                    page3.cellRbcMor3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13" && p.Year == year3)?.ResultValue;

                    page3.cellWbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006")?.ReferenceRange;
                    page3.cellWbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006" && p.Year == year1)?.ResultValue;
                    page3.cellWbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006" && p.Year == year2)?.ResultValue;
                    page3.cellWbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006" && p.Year == year3)?.ResultValue;

                    page3.cellNectophilRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040")?.ReferenceRange;
                    page3.cellNectophil1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040" && p.Year == year1)?.ResultValue;
                    page3.cellNectophil2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040" && p.Year == year2)?.ResultValue;
                    page3.cellNectophil3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040" && p.Year == year3)?.ResultValue;

                    page3.cellLymphocyteRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050")?.ReferenceRange;
                    page3.cellLymphocyte1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050" && p.Year == year1)?.ResultValue;
                    page3.cellLymphocyte2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050" && p.Year == year2)?.ResultValue;
                    page3.cellLymphocyte3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050" && p.Year == year3)?.ResultValue;

                    page3.cellMonocyteRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060")?.ReferenceRange;
                    page3.cellMonocyte1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060" && p.Year == year1)?.ResultValue;
                    page3.cellMonocyte2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060" && p.Year == year2)?.ResultValue;
                    page3.cellMonocyte3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060" && p.Year == year3)?.ResultValue;

                    page3.cellEosinophilRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070")?.ReferenceRange;
                    page3.cellEosinophil1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070" && p.Year == year1)?.ResultValue;
                    page3.cellEosinophil2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070" && p.Year == year2)?.ResultValue;
                    page3.cellEosinophil3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070" && p.Year == year3)?.ResultValue;

                    page3.cellBasophilRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080")?.ReferenceRange;
                    page3.cellBasophil1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080" && p.Year == year1)?.ResultValue;
                    page3.cellBasophil2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080" && p.Year == year2)?.ResultValue;
                    page3.cellBasophil3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080" && p.Year == year3)?.ResultValue;

                    page3.cellPlateletSmearRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427")?.ReferenceRange;
                    page3.cellPlateletSmear1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427" && p.Year == year1)?.ResultValue;
                    page3.cellPlateletSmear2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427" && p.Year == year2)?.ResultValue;
                    page3.cellPlateletSmear3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427" && p.Year == year3)?.ResultValue;

                    page3.cellPlateletsCountRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010")?.ReferenceRange;
                    page3.cellPlateletsCount1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010" && p.Year == year1)?.ResultValue;
                    page3.cellPlateletsCount2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010" && p.Year == year2)?.ResultValue;
                    page3.cellPlateletsCount3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010" && p.Year == year3)?.ResultValue;
                }
                else
                {
                    page3.cellCBCYear1.Text = "ปี" + " " + DateTime.Now.Year;
                    page3.cellCBCYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                    page3.cellCBCYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
                }
            }
            else
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();
                page3.cellCBCYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page3.cellCBCYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                page3.cellCBCYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
            }

        }

        private void GenerateUrinalysis(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> oneYears = labTestSet.Select(p => p.Year).Distinct().ToList();
                if (oneYears.Count == 1)
                {
                    List<DateTime?> repeatYears = labTestSet.Select(p => p.StartDttm).Distinct().ToList();
                    repeatYears.Sort();

                    if (repeatYears.Count > 1)
                    {
                        DateTime? dateMax = DateTime.Now;
                        DateTime? dateMin = repeatYears[0];
                        foreach (DateTime dt in repeatYears)
                        {

                            if (dt > dateMin)
                            {
                                dateMax = dt;
                            }
                        }
                        int yearMax = dateMax.Value.Year;
                        int yearMin = yearMax - 1;

                        labTestSet.Where(w => w.StartDttm == dateMin).ToList().ForEach(i => i.Year = yearMin);
                    }
                }

                int thisYear = DateTime.Now.Year;
                int? compare = thisYear - 2;
                List<int?> Years = labTestSet.Select(p => p.Year).Where(p => p.Value >= compare).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();

                if (countYear != 0)
                {
                    int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                    int? year2 = countYear >= 2 ? (Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1) : year1 - 1;
                    int? year3 = countYear >= 3 ? (Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1) : year2 - 1;
                    page4.cellUAYear1.Text = "ปี" + " " + year1.ToString();
                    page4.cellUAYear2.Text = "ปี" + " " + year2.ToString();
                    page4.cellUAYear3.Text = "ปี" + " " + year3.ToString();

                    page4.cellColorRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080")?.ReferenceRange;
                    page4.cellColor1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year1)?.ResultValue;
                    page4.cellColor2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year2)?.ResultValue;
                    page4.cellColor3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year3)?.ResultValue;

                    page4.cellClarityRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21")?.ReferenceRange;
                    page4.cellClarity1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year1)?.ResultValue;
                    page4.cellClarity2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year2)?.ResultValue;
                    page4.cellClarity3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year3)?.ResultValue;

                    page4.cellSpacGraRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001")?.ReferenceRange;
                    page4.cellSpacGra1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001" && p.Year == year1)?.ResultValue;
                    page4.cellSpacGra2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001" && p.Year == year2)?.ResultValue;
                    page4.cellSpacGra3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001" && p.Year == year3)?.ResultValue;

                    page4.cellPhRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080")?.ReferenceRange;
                    page4.cellPh1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080" && p.Year == year1)?.ResultValue;
                    page4.cellPh2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080" && p.Year == year2)?.ResultValue;
                    page4.cellPh3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080" && p.Year == year3)?.ResultValue;

                    page4.cellProteinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085")?.ReferenceRange;
                    page4.cellProtein1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085" && p.Year == year1)?.ResultValue;
                    page4.cellProtein2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085" && p.Year == year2)?.ResultValue;
                    page4.cellProtein3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085" && p.Year == year3)?.ResultValue;

                    page4.cellGlucoseRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090")?.ReferenceRange;
                    page4.cellGlucose1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090" && p.Year == year1)?.ResultValue;
                    page4.cellGlucose2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090" && p.Year == year2)?.ResultValue;
                    page4.cellGlucose3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090" && p.Year == year3)?.ResultValue;

                    page4.cellKetoneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047")?.ReferenceRange;
                    page4.cellKetone1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047" && p.Year == year1)?.ResultValue;
                    page4.cellKetone2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047" && p.Year == year2)?.ResultValue;
                    page4.cellKetone3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047" && p.Year == year3)?.ResultValue;

                    page4.cellNitritesRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154")?.ReferenceRange;
                    page4.cellNitrites1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154" && p.Year == year1)?.ResultValue;
                    page4.cellNitrites2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154" && p.Year == year2)?.ResultValue;
                    page4.cellNitrites3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154" && p.Year == year3)?.ResultValue;

                    page4.cellBilirubinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151")?.ReferenceRange;
                    page4.cellBilirubin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151" && p.Year == year1)?.ResultValue;
                    page4.cellBilirubin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151" && p.Year == year2)?.ResultValue;
                    page4.cellBilirubin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151" && p.Year == year3)?.ResultValue;

                    page4.cellUrobilinogenRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150")?.ReferenceRange;
                    page4.cellUrobilinogen1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150" && p.Year == year1)?.ResultValue;
                    page4.cellUrobilinogen2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150" && p.Year == year2)?.ResultValue;
                    page4.cellUrobilinogen3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150" && p.Year == year3)?.ResultValue;

                    page4.cellLeukocyteRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153")?.ReferenceRange;
                    page4.cellLeukocyte1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153" && p.Year == year1)?.ResultValue;
                    page4.cellLeukocyte2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153" && p.Year == year2)?.ResultValue;
                    page4.cellLeukocyte3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153" && p.Year == year3)?.ResultValue;

                    page4.cellBloodRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152")?.ReferenceRange;
                    page4.cellBlood1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152" && p.Year == year1)?.ResultValue;
                    page4.cellBlood2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152" && p.Year == year2)?.ResultValue;
                    page4.cellBlood3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152" && p.Year == year3)?.ResultValue;

                    page4.cellErythrocytesRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140")?.ReferenceRange;
                    page4.cellErythrocytes1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140" && p.Year == year1)?.ResultValue;
                    page4.cellErythrocytes2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140" && p.Year == year2)?.ResultValue;
                    page4.cellErythrocytes3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140" && p.Year == year3)?.ResultValue;

                    page4.cellWbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250")?.ReferenceRange;
                    page4.cellWbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250" && p.Year == year1)?.ResultValue;
                    page4.cellWbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250" && p.Year == year2)?.ResultValue;
                    page4.cellWbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250" && p.Year == year3)?.ResultValue;

                    page4.cellRbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260")?.ReferenceRange;
                    page4.cellRbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260" && p.Year == year1)?.ResultValue;
                    page4.cellRbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260" && p.Year == year2)?.ResultValue;
                    page4.cellRbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260" && p.Year == year3)?.ResultValue;

                    page4.cellEpithelialCellsRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270")?.ReferenceRange;
                    page4.cellEpithelialCells1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270" && p.Year == year1)?.ResultValue;
                    page4.cellEpithelialCells2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270" && p.Year == year2)?.ResultValue;
                    page4.cellEpithelialCells3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270" && p.Year == year3)?.ResultValue;

                    page4.cellAmorphousRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14")?.ReferenceRange;
                    page4.cellAmorphous1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14" && p.Year == year1)?.ResultValue;
                    page4.cellAmorphous2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14" && p.Year == year2)?.ResultValue;
                    page4.cellAmorphous3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14" && p.Year == year3)?.ResultValue;

                    page4.cellOtherRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20")?.ReferenceRange;
                    page4.cellOther1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20" && p.Year == year1)?.ResultValue;
                    page4.cellOther2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20" && p.Year == year2)?.ResultValue;
                    page4.cellOther3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20" && p.Year == year3)?.ResultValue;
                }
                else
                {
                    page4.cellUAYear1.Text = "ปี" + " " + DateTime.Now.Year;
                    page4.cellUAYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                    page4.cellUAYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
                }
            }
            else
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();
                page4.cellUAYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page4.cellUAYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                page4.cellUAYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
            }
        }

        private void GenerateRenalFunction(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> oneYears = labTestSet.Select(p => p.Year).Distinct().ToList();
                if (oneYears.Count == 1)
                {
                    List<DateTime?> repeatYears = labTestSet.Select(p => p.StartDttm).Distinct().ToList();
                    repeatYears.Sort();

                    if (repeatYears.Count > 1)
                    {
                        DateTime? dateMax = DateTime.Now;
                        DateTime? dateMin = repeatYears[0];
                        foreach (DateTime dt in repeatYears)
                        {

                            if (dt > dateMin)
                            {
                                dateMax = dt;
                            }
                        }
                        int yearMax = dateMax.Value.Year;
                        int yearMin = yearMax - 1;

                        labTestSet.Where(w => w.StartDttm == dateMin).ToList().ForEach(i => i.Year = yearMin);
                    }
                }

                int thisYear = DateTime.Now.Year;
                int? compare = thisYear - 2;
                List<int?> Years = labTestSet.Select(p => p.Year).Where(p => p.Value >= compare).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();

                if (countYear != 0)
                {
                    int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                    int? year2 = countYear >= 2 ? (Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1) : year1 - 1;
                    int? year3 = countYear >= 3 ? (Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1) : year2 - 1;
                    page5.cellRenalYear1.Text = "ปี" + " " + year1.ToString();
                    page5.cellRenalYear2.Text = "ปี" + " " + year2.ToString();
                    page5.cellRenalYear3.Text = "ปี" + " " + year3.ToString();

                    page5.cellBunRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27")?.ReferenceRange;
                    page5.cellBun1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27" && p.Year == year1)?.ResultValue;
                    page5.cellBun2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27" && p.Year == year2)?.ResultValue;
                    page5.cellBun3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27" && p.Year == year3)?.ResultValue;

                    page5.cellCreatinineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070")?.ReferenceRange;
                    page5.cellCreatinine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070" && p.Year == year1)?.ResultValue;
                    page5.cellCreatinine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070" && p.Year == year2)?.ResultValue;
                    page5.cellCreatinine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070" && p.Year == year3)?.ResultValue;

                    page5.eGFRRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0073")?.ReferenceRange;
                    page5.eGFR1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0073" && p.Year == year1)?.ResultValue;
                    page5.eGFR2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0073" && p.Year == year2)?.ResultValue;
                    page5.eGFR3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0073" && p.Year == year3)?.ResultValue;
                }
                else
                {
                    page5.cellRenalYear1.Text = "ปี" + " " + DateTime.Now.Year;
                    page5.cellRenalYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                    page5.cellRenalYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
                }
            }
            else
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();
                page5.cellRenalYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page5.cellRenalYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                page5.cellRenalYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
            }
        }

        private void GenerateFastingBloodSugar(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> oneYears = labTestSet.Select(p => p.Year).Distinct().ToList();
                if (oneYears.Count == 1)
                {
                    List<DateTime?> repeatYears = labTestSet.Select(p => p.StartDttm).Distinct().ToList();
                    repeatYears.Sort();

                    if (repeatYears.Count > 1)
                    {
                        DateTime? dateMax = DateTime.Now;
                        DateTime? dateMin = repeatYears[0];
                        foreach (DateTime dt in repeatYears)
                        {

                            if (dt > dateMin)
                            {
                                dateMax = dt;
                            }
                        }
                        int yearMax = dateMax.Value.Year;
                        int yearMin = yearMax - 1;

                        labTestSet.Where(w => w.StartDttm == dateMin).ToList().ForEach(i => i.Year = yearMin);
                    }
                }

                int thisYear = DateTime.Now.Year;
                int? compare = thisYear - 2;
                List<int?> Years = labTestSet.Select(p => p.Year).Where(p => p.Value >= compare).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();

                if (countYear != 0)
                {
                    int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                    int? year2 = countYear >= 2 ? (Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1) : year1 - 1;
                    int? year3 = countYear >= 3 ? (Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1) : year2 - 1;
                    page4.cellFbsYear1.Text = "ปี" + " " + year1.ToString();
                    page4.cellFbsYear2.Text = "ปี" + " " + year2.ToString();
                    page4.cellFbsYear3.Text = "ปี" + " " + year3.ToString();

                    page4.cellFbsRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0180")?.ReferenceRange;
                    page4.cellFbs1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0180" && p.Year == year1)?.ResultValue;
                    page4.cellFbs2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0180" && p.Year == year2)?.ResultValue;
                    page4.cellFbs3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0180" && p.Year == year3)?.ResultValue;

                    page4.lbHbA1cRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR7")?.ReferenceRange;
                    page4.lbHbA1c1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR7" && p.Year == year1)?.ResultValue;
                    page4.lbHbA1c2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR7" && p.Year == year2)?.ResultValue;
                    page4.lbHbA1c3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR7" && p.Year == year3)?.ResultValue;
                }
                else
                {
                    page4.cellFbsYear1.Text = "ปี" + " " + DateTime.Now.Year;
                    page4.cellFbsYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                    page4.cellFbsYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
                }
            }
            else
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();
                page4.cellFbsYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page4.cellFbsYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                page4.cellFbsYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
            }
        }

        private void GenerateUricAcid(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> oneYears = labTestSet.Select(p => p.Year).Distinct().ToList();
                if (oneYears.Count == 1)
                {
                    List<DateTime?> repeatYears = labTestSet.Select(p => p.StartDttm).Distinct().ToList();
                    repeatYears.Sort();

                    if (repeatYears.Count > 1)
                    {
                        DateTime? dateMax = DateTime.Now;
                        DateTime? dateMin = repeatYears[0];
                        foreach (DateTime dt in repeatYears)
                        {

                            if (dt > dateMin)
                            {
                                dateMax = dt;
                            }
                        }
                        int yearMax = dateMax.Value.Year;
                        int yearMin = yearMax - 1;

                        labTestSet.Where(w => w.StartDttm == dateMin).ToList().ForEach(i => i.Year = yearMin);
                    }
                }

                int thisYear = DateTime.Now.Year;
                int? compare = thisYear - 2;
                List<int?> Years = labTestSet.Select(p => p.Year).Where(p => p.Value >= compare).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();

                if (countYear != 0)
                {
                    int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                    int? year2 = countYear >= 2 ? (Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1) : year1 - 1;
                    int? year3 = countYear >= 3 ? (Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1) : year2 - 1;
                    page4.cellUricYear1.Text = "ปี" + " " + year1.ToString();
                    page4.cellUricYear2.Text = "ปี" + " " + year2.ToString();
                    page4.cellUricYear3.Text = "ปี" + " " + year3.ToString();

                    page4.cellUricRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320")?.ReferenceRange;
                    page4.cellUric1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320" && p.Year == year1)?.ResultValue;
                    page4.cellUric2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320" && p.Year == year2)?.ResultValue;
                    page4.cellUric3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320" && p.Year == year3)?.ResultValue;
                }
                else
                {
                    page4.cellUricYear1.Text = "ปี" + " " + DateTime.Now.Year;
                    page4.cellUricYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                    page4.cellUricYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
                }
            }
            else
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();
                page4.cellUricYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page4.cellUricYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                page4.cellUricYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
            }
        }

        private void GenerateLipidProfiles(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> oneYears = labTestSet.Select(p => p.Year).Distinct().ToList();
                if (oneYears.Count == 1)
                {
                    List<DateTime?> repeatYears = labTestSet.Select(p => p.StartDttm).Distinct().ToList();
                    repeatYears.Sort();

                    if (repeatYears.Count > 1)
                    {
                        DateTime? dateMax = DateTime.Now;
                        DateTime? dateMin = repeatYears[0];
                        foreach (DateTime dt in repeatYears)
                        {

                            if (dt > dateMin)
                            {
                                dateMax = dt;
                            }
                        }
                        int yearMax = dateMax.Value.Year;
                        int yearMin = yearMax - 1;

                        labTestSet.Where(w => w.StartDttm == dateMin).ToList().ForEach(i => i.Year = yearMin);
                    }
                }

                int thisYear = DateTime.Now.Year;
                int? compare = thisYear - 2;
                List<int?> Years = labTestSet.Select(p => p.Year).Where(p => p.Value >= compare).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();

                if (countYear != 0)
                {
                    int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                    int? year2 = countYear >= 2 ? (Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1) : year1 - 1;
                    int? year3 = countYear >= 3 ? (Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1) : year2 - 1;
                    page3.cellLipidYear1.Text = "ปี" + " " + year1.ToString();
                    page3.cellLipidYear2.Text = "ปี" + " " + year2.ToString();
                    page3.cellLipidYear3.Text = "ปี" + " " + year3.ToString();

                    page3.cellCholesterolRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130")?.ReferenceRange;
                    page3.cellCholesterol1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130" && p.Year == year1)?.ResultValue;
                    page3.cellCholesterol2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130" && p.Year == year2)?.ResultValue;
                    page3.cellCholesterol3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130" && p.Year == year3)?.ResultValue;

                    page3.cellTriglycerideRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140")?.ReferenceRange;
                    page3.cellTriglyceride1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140" && p.Year == year1)?.ResultValue;
                    page3.cellTriglyceride2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140" && p.Year == year2)?.ResultValue;
                    page3.cellTriglyceride3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140" && p.Year == year3)?.ResultValue;

                    page3.cellLdlRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159")?.ReferenceRange;
                    page3.cellLdl1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159" && p.Year == year1)?.ResultValue;
                    page3.cellLdl2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159" && p.Year == year2)?.ResultValue;
                    page3.cellLdl3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159" && p.Year == year3)?.ResultValue;

                    page3.cellHdlRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150")?.ReferenceRange;
                    page3.cellHdl1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150" && p.Year == year1)?.ResultValue;
                    page3.cellHdl2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150" && p.Year == year2)?.ResultValue;
                    page3.cellHdl3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150" && p.Year == year3)?.ResultValue;
                }
                else
                {
                    page3.cellLipidYear1.Text = "ปี" + " " + DateTime.Now.Year;
                    page3.cellLipidYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                    page3.cellLipidYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
                }
            }
            else
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();
                page3.cellLipidYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page3.cellLipidYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                page3.cellLipidYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
            }
        }

        private void GenerateLiverFunction(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> oneYears = labTestSet.Select(p => p.Year).Distinct().ToList();
                if (oneYears.Count == 1)
                {
                    List<DateTime?> repeatYears = labTestSet.Select(p => p.StartDttm).Distinct().ToList();
                    repeatYears.Sort();

                    if (repeatYears.Count > 1)
                    {
                        DateTime? dateMax = DateTime.Now;
                        DateTime? dateMin = repeatYears[0];
                        foreach (DateTime dt in repeatYears)
                        {

                            if (dt > dateMin)
                            {
                                dateMax = dt;
                            }
                        }
                        int yearMax = dateMax.Value.Year;
                        int yearMin = yearMax - 1;

                        labTestSet.Where(w => w.StartDttm == dateMin).ToList().ForEach(i => i.Year = yearMin);
                    }
                }

                int thisYear = DateTime.Now.Year;
                int? compare = thisYear - 2;
                List<int?> Years = labTestSet.Select(p => p.Year).Where(p => p.Value >= compare).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();

                if (countYear != 0)
                {
                    int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                    int? year2 = countYear >= 2 ? (Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1) : year1 - 1;
                    int? year3 = countYear >= 3 ? (Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1) : year2 - 1;
                    page5.cellLiverYear1.Text = "ปี" + " " + year1.ToString();
                    page5.cellLiverYear2.Text = "ปี" + " " + year2.ToString();
                    page5.cellLiverYear3.Text = "ปี" + " " + year3.ToString();

                    page5.cellAstRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50")?.ReferenceRange;
                    page5.cellAst1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50" && p.Year == year1)?.ResultValue;
                    page5.cellAst2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50" && p.Year == year2)?.ResultValue;
                    page5.cellAst3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50" && p.Year == year3)?.ResultValue;

                    page5.cellAltRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51")?.ReferenceRange;
                    page5.cellAlt1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51" && p.Year == year1)?.ResultValue;
                    page5.cellAlt2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51" && p.Year == year2)?.ResultValue;
                    page5.cellAlt3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51" && p.Year == year3)?.ResultValue;

                    page5.cellAlpRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33")?.ReferenceRange;
                    page5.cellAlp1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33" && p.Year == year1)?.ResultValue;
                    page5.cellAlp2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33" && p.Year == year2)?.ResultValue;
                    page5.cellAlp3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33" && p.Year == year3)?.ResultValue;

                    page5.cellTotalBilirubinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48")?.ReferenceRange;
                    page5.cellTotalBilirubin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48" && p.Year == year1)?.ResultValue;
                    page5.cellTotalBilirubin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48" && p.Year == year2)?.ResultValue;
                    page5.cellTotalBilirubin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48" && p.Year == year3)?.ResultValue;

                    page5.cellDirectBilirubinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49")?.ReferenceRange;
                    page5.cellDirectBilirubin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year1)?.ResultValue;
                    page5.cellDirectBilirubin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year2)?.ResultValue;
                    page5.cellDirectBilirubin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year3)?.ResultValue;

                    page5.cellTotalProteinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105")?.ReferenceRange;
                    page5.cellTotalProtein1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105" && p.Year == year1)?.ResultValue;
                    page5.cellTotalProtein2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105" && p.Year == year2)?.ResultValue;
                    page5.cellTotalProtein3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105" && p.Year == year3)?.ResultValue;

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR101") != null)
                    {
                        page5.RowAlbumin.Visible = true;
                        page5.cellAlbuminRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR101")?.ReferenceRange;
                        page5.cellAlbumin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR101" && p.Year == year1)?.ResultValue;
                        page5.cellAlbumin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR101" && p.Year == year2)?.ResultValue;
                        page5.cellAlbumin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR101" && p.Year == year3)?.ResultValue;
                    }

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46") != null)
                    {
                        page5.RowGlobulin.Visible = true;
                        page5.cellGlobulinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46")?.ReferenceRange;
                        page5.cellGlobulin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year1)?.ResultValue;
                        page5.cellGlobulin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year2)?.ResultValue;
                        page5.cellGlobulin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year3)?.ResultValue;
                    }

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR26") != null)
                    {
                        page5.RowGGT.Visible = true;
                        page5.cellGgtRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR26")?.ReferenceRange;
                        page5.cellggt1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR26" && p.Year == year1)?.ResultValue;
                        page5.cellggt2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR26" && p.Year == year2)?.ResultValue;
                        page5.cellggt3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR26" && p.Year == year3)?.ResultValue;
                    }
                }
                else
                {
                    page5.cellLiverYear1.Text = "ปี" + " " + DateTime.Now.Year;
                    page5.cellLiverYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                    page5.cellLiverYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
                }
            }
            else
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();
                page5.cellLiverYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page5.cellLiverYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                page5.cellLiverYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
            }
        }

        private void GenerateImmunology(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> oneYears = labTestSet.Select(p => p.Year).Distinct().ToList();
                if (oneYears.Count == 1)
                {
                    List<DateTime?> repeatYears = labTestSet.Select(p => p.StartDttm).Distinct().ToList();
                    repeatYears.Sort();

                    if (repeatYears.Count > 1)
                    {
                        DateTime? dateMax = DateTime.Now;
                        DateTime? dateMin = repeatYears[0];
                        foreach (DateTime dt in repeatYears)
                        {

                            if (dt > dateMin)
                            {
                                dateMax = dt;
                            }
                        }
                        int yearMax = dateMax.Value.Year;
                        int yearMin = yearMax - 1;

                        labTestSet.Where(w => w.StartDttm == dateMin).ToList().ForEach(i => i.Year = yearMin);
                    }
                }

                int thisYear = DateTime.Now.Year;
                int? compare = thisYear - 2;
                List<int?> Years = labTestSet.Select(p => p.Year).Where(p => p.Value >= compare).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();

                if (countYear != 0)
                {
                    int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                    int? year2 = countYear >= 2 ? (Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1) : year1 - 1;
                    int? year3 = countYear >= 3 ? (Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1) : year2 - 1;
                    page5.cellImmunlogyYear1.Text = "ปี" + " " + year1.ToString();
                    page5.cellImmunlogyYear2.Text = "ปี" + " " + year2.ToString();
                    page5.cellImmunlogyYear3.Text = "ปี" + " " + year3.ToString();

                    page5.cellHbsAgRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35")?.ReferenceRange;
                    page5.cellHbsAg1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35" && p.Year == year1)?.ResultValue;
                    page5.cellHbsAg2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35" && p.Year == year2)?.ResultValue;
                    page5.cellHbsAg3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35" && p.Year == year3)?.ResultValue;

                    page5.cellHbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1255")?.ReferenceRange;
                    page5.cellHbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1255" && p.Year == year1)?.ResultValue;
                    page5.cellHbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1255" && p.Year == year2)?.ResultValue;
                    page5.cellHbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1255" && p.Year == year3)?.ResultValue;

                    page5.cellCoiAgRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34")?.ReferenceRange;
                    page5.cellCoiAg1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34" && p.Year == year1)?.ResultValue;
                    page5.cellCoiAg2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34" && p.Year == year2)?.ResultValue;
                    page5.cellCoiAg3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34" && p.Year == year3)?.ResultValue;

                    page5.cellCoiHbsRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121")?.ReferenceRange;
                    page5.cellCoiHbs1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121" && p.Year == year1)?.ResultValue;
                    page5.cellCoiHbs2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121" && p.Year == year2)?.ResultValue;
                    page5.cellCoiHbs3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121" && p.Year == year3)?.ResultValue;

                    page5.cellAntiHbsRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42")?.ReferenceRange;
                    page5.cellAntiHbs1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42" && p.Year == year1)?.ResultValue;
                    page5.cellAntiHbs2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42" && p.Year == year2)?.ResultValue;
                    page5.cellAntiHbs3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42" && p.Year == year3)?.ResultValue;

                    page5.cellHavIgmRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR192")?.ReferenceRange;
                    page5.cellHavIgm1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR192" && p.Year == year1)?.ResultValue;
                    page5.cellHavIgm2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR192" && p.Year == year2)?.ResultValue;
                    page5.cellHavIgm3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR192" && p.Year == year3)?.ResultValue;

                    page5.cellHavIggRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR190")?.ReferenceRange;
                    page5.cellHavIgg1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR190" && p.Year == year1)?.ResultValue;
                    page5.cellHavIgg2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR190" && p.Year == year2)?.ResultValue;
                    page5.cellHavIgg3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR190" && p.Year == year3)?.ResultValue;

                    page5.cellHCVRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR53")?.ReferenceRange;
                    page5.cellHCV1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR53" && p.Year == year1)?.ResultValue;
                    page5.cellHCV2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR53" && p.Year == year2)?.ResultValue;
                    page5.cellHCV3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR53" && p.Year == year3)?.ResultValue;

                    page5.cellCoiHCVRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR54")?.ReferenceRange;
                    page5.cellCoiHCV1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR54" && p.Year == year1)?.ResultValue;
                    page5.cellCoiHCV2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR54" && p.Year == year2)?.ResultValue;
                    page5.cellCoiHCV3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR54" && p.Year == year3)?.ResultValue;
                }
                else
                {
                    page5.cellImmunlogyYear1.Text = "ปี" + " " + DateTime.Now.Year;
                    page5.cellImmunlogyYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                    page5.cellImmunlogyYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
                }
            }
            else
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();
                page5.cellImmunlogyYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page5.cellImmunlogyYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                page5.cellImmunlogyYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
            }
        }

        private void GenerateStool(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> oneYears = labTestSet.Select(p => p.Year).Distinct().ToList();
                if (oneYears.Count == 1)
                {
                    List<DateTime?> repeatYears = labTestSet.Select(p => p.StartDttm).Distinct().ToList();
                    repeatYears.Sort();

                    if (repeatYears.Count > 1)
                    {
                        DateTime? dateMax = DateTime.Now;
                        DateTime? dateMin = repeatYears[0];
                        foreach (DateTime dt in repeatYears)
                        {

                            if (dt > dateMin)
                            {
                                dateMax = dt;
                            }
                        }
                        int yearMax = dateMax.Value.Year;
                        int yearMin = yearMax - 1;

                        labTestSet.Where(w => w.StartDttm == dateMin).ToList().ForEach(i => i.Year = yearMin);
                    }
                }

                int thisYear = DateTime.Now.Year;
                int? compare = thisYear - 2;
                List<int?> Years = labTestSet.Select(p => p.Year).Where(p => p.Value >= compare).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();

                if (countYear != 0)
                {
                    int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                    int? year2 = countYear >= 2 ? (Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1) : year1 - 1;
                    int? year3 = countYear >= 3 ? (Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1) : year2 - 1;
                    page6.cellStoolYear1.Text = "ปี" + " " + year1.ToString();
                    page6.cellStoolYear2.Text = "ปี" + " " + year2.ToString();
                    page6.cellStoolYear3.Text = "ปี" + " " + year3.ToString();

                    page6.cellStColorRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR69")?.ReferenceRange;
                    page6.cellStColor1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR69" && p.Year == year1)?.ResultValue;
                    page6.cellStColor2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR69" && p.Year == year2)?.ResultValue;
                    page6.cellStColor3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR69" && p.Year == year3)?.ResultValue;

                    page6.cellStappearRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR70")?.ReferenceRange;
                    page6.cellStappear1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR70" && p.Year == year1)?.ResultValue;
                    page6.cellStappear2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR70" && p.Year == year2)?.ResultValue;
                    page6.cellStappear2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR70" && p.Year == year3)?.ResultValue;

                    page6.stoolOvaRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR73")?.ReferenceRange;
                    page6.stoolOva1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR73" && p.Year == year1)?.ResultValue;
                    page6.stoolOva2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR73" && p.Year == year2)?.ResultValue;
                    page6.stoolOva3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR73" && p.Year == year3)?.ResultValue;

                    page6.stoolWbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR71")?.ReferenceRange;
                    page6.stoolWbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR71" && p.Year == year1)?.ResultValue;
                    page6.stoolWbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR71" && p.Year == year2)?.ResultValue;
                    page6.stoolWbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR71" && p.Year == year3)?.ResultValue;

                    page6.stoolRbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR72")?.ReferenceRange;
                    page6.stoolRbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR72" && p.Year == year1)?.ResultValue;
                    page6.stoolRbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR72" && p.Year == year2)?.ResultValue;
                    page6.stoolRbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR72" && p.Year == year3)?.ResultValue;
                }
                else
                {
                    page6.cellStoolYear1.Text = "ปี" + " " + DateTime.Now.Year;
                    page6.cellStoolYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                    page6.cellStoolYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
                }
            }
            else
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();
                page6.cellStoolYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page6.cellStoolYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                page6.cellStoolYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
            }
        }

        private void GenerateStoolCulture(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> oneYears = labTestSet.Select(p => p.Year).Distinct().ToList();
                if (oneYears.Count == 1)
                {
                    List<DateTime?> repeatYears = labTestSet.Select(p => p.StartDttm).Distinct().ToList();
                    repeatYears.Sort();

                    if (repeatYears.Count > 1)
                    {
                        DateTime? dateMax = DateTime.Now;
                        DateTime? dateMin = repeatYears[0];
                        foreach (DateTime dt in repeatYears)
                        {

                            if (dt > dateMin)
                            {
                                dateMax = dt;
                            }
                        }
                        int yearMax = dateMax.Value.Year;
                        int yearMin = yearMax - 1;

                        labTestSet.Where(w => w.StartDttm == dateMin).ToList().ForEach(i => i.Year = yearMin);
                    }
                }

                int thisYear = DateTime.Now.Year;
                int? compare = thisYear - 2;
                List<int?> Years = labTestSet.Select(p => p.Year).Where(p => p.Value >= compare).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();

                if (countYear != 0)
                {
                    int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                    int? year2 = countYear >= 2 ? (Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1) : year1 - 1;
                    int? year3 = countYear >= 3 ? (Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1) : year2 - 1;

                    page7.cellStoolCulterYear1.Text = "ปี" + " " + year1.ToString();
                    page7.cellStoolCulterYear2.Text = "ปี" + " " + year2.ToString();
                    page7.cellStoolCulterYear3.Text = "ปี" + " " + year3.ToString();

                    //page7.stCulter1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR189" && p.Year == year1)?.ResultValue;
                    //page7.stCulter2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR189" && p.Year == year2)?.ResultValue;
                    //page7.stCulter3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR189" && p.Year == year3)?.ResultValue;

                    page7.Salmonella1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1256" && p.Year == year1)?.ResultValue;
                    page7.Salmonella2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1256" && p.Year == year2)?.ResultValue;
                    page7.Salmonella3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1256" && p.Year == year3)?.ResultValue;

                    page7.Shigella1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1257" && p.Year == year1)?.ResultValue;
                    page7.Shigella2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1257" && p.Year == year2)?.ResultValue;
                    page7.Shigella3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1257" && p.Year == year3)?.ResultValue;

                    page7.stChole1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1258" && p.Year == year1)?.ResultValue;
                    page7.stChole2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1258" && p.Year == year2)?.ResultValue;
                    page7.stChole3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1258" && p.Year == year3)?.ResultValue;

                    page7.stPara1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1259" && p.Year == year1)?.ResultValue;
                    page7.stPara2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1259" && p.Year == year2)?.ResultValue;
                    page7.stPara3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1259" && p.Year == year3)?.ResultValue;

                    page7.stCholerar1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1260" && p.Year == year1)?.ResultValue;
                    page7.stCholerar2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1260" && p.Year == year2)?.ResultValue;
                    page7.stCholerar3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1260" && p.Year == year3)?.ResultValue;
                }
                else
                {
                    page7.cellStoolCulterYear1.Text = "ปี" + " " + DateTime.Now.Year;
                    page7.cellStoolCulterYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                    page7.cellStoolCulterYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
                }

            }
            else
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();
                page7.cellStoolCulterYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page7.cellStoolCulterYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                page7.cellStoolCulterYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
            }
        }

        private void GenerateToxicology(IEnumerable<PatientResultComponentModel> labTestSet)
        {

            if (labTestSet != null)
            {
                if (labTestSet != null && labTestSet.Count() > 0)
                {
                    List<int?> oneYears = labTestSet.Select(p => p.Year).Distinct().ToList();
                    if (oneYears.Count == 1)
                    {
                        List<DateTime?> repeatYears = labTestSet.Select(p => p.StartDttm).Distinct().ToList();
                        repeatYears.Sort();

                        if (repeatYears.Count > 1)
                        {
                            DateTime? dateMax = DateTime.Now;
                            DateTime? dateMin = repeatYears[0];
                            foreach (DateTime dt in repeatYears)
                            {

                                if (dt > dateMin)
                                {
                                    dateMax = dt;
                                }
                            }
                            int yearMax = dateMax.Value.Year;
                            int yearMin = yearMax - 1;

                            labTestSet.Where(w => w.StartDttm == dateMin).ToList().ForEach(i => i.Year = yearMin);
                        }
                    }

                    int thisYear = DateTime.Now.Year;
                    int? compare =  thisYear - 2;
                    List<int?> Years = labTestSet.Select(p => p.Year).Where(p => p.Value >= compare).Distinct().ToList();
                    Years.OrderByDescending(p => ((uint?)p));
                    int countYear = Years.Count();

                    if (countYear != 0)
                    {
                        int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                        int? year2 = countYear >= 2 ? (Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1) : year1 - 1;
                        int? year3 = countYear >= 3 ? (Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1) : year2 - 1;
                        page7.cellToxicoYear1.Text = "ปี" + " " + year1.ToString();
                        page7.cellToxicoYear2.Text = "ปี" + " " + year2.ToString();
                        page7.cellToxicoYear3.Text = "ปี" + " " + year3.ToString();


                        #region Aluminium in Urin
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122") != null)
                        {     
                            page7.cellAluminiumRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122")?.ReferenceRange;
                            page7.cellAluminium1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year1)?.ResultValue;
                            page7.cellAluminium2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year2)?.ResultValue;
                            page7.cellAluminium3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year3)?.ResultValue;
                        }

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1319") != null)
                        {
                            page7.cellAluminiumRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1319")?.ReferenceRange;
                            page7.cellAluminium1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1319" && p.Year == year1)?.ResultValue;
                            page7.cellAluminium2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1319" && p.Year == year2)?.ResultValue;
                            page7.cellAluminium3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1319" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Toluene

                        page7.cellTolueneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124")?.ReferenceRange;
                        page7.cellToluene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year1)?.ResultValue;
                        page7.cellToluene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year2)?.ResultValue;
                        page7.cellToluene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year3)?.ResultValue;

                        #endregion

                        #region Xylene

                        page7.cellXyleneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125")?.ReferenceRange;
                        page7.cellXylene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year1)?.ResultValue;
                        page7.cellXylene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year2)?.ResultValue;
                        page7.cellXylene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year3)?.ResultValue;


                        #endregion

                        #region Lead in blood 
                        page7.cellLeadRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75")?.ReferenceRange;
                        page7.cellLead1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year1)?.ResultValue;
                        page7.cellLead2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year2)?.ResultValue;
                        page7.cellLead3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year3)?.ResultValue;

                        #endregion

                        #region Carboxy
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120") != null)
                        {
                            page7.RowCarboxy.Visible = true;
                            page7.cellCarboxyRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120")?.ReferenceRange;
                            page7.cellCarboxy1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year1)?.ResultValue;
                            page7.cellCarboxy2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year2)?.ResultValue;
                            page7.cellCarboxy3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Mek
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127") != null)
                        {
                            page7.RowMEK.Visible = true;
                            page7.cellMekRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127")?.ReferenceRange;
                            page7.cellMek1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year1)?.ResultValue;
                            page7.cellMek2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year2)?.ResultValue;
                            page7.cellMek3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Benzene
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115") != null)
                        {
                            page7.RowBenzene.Visible = true;
                            page7.cellBenzeneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115")?.ReferenceRange;
                            page7.cellBenzene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115" && p.Year == year1)?.ResultValue;
                            page7.cellBenzene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115" && p.Year == year2)?.ResultValue;
                            page7.cellBenzene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115" && p.Year == year3)?.ResultValue;

                        }
                        #endregion

                        #region Methanol
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116") != null)
                        {
                            page7.RowMethanol.Visible = true;
                            page7.cellMethanolRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116")?.ReferenceRange;
                            page7.cellMethanol1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116" && p.Year == year1)?.ResultValue;
                            page7.cellMethanol2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116" && p.Year == year2)?.ResultValue;
                            page7.cellMethanol3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116" && p.Year == year3)?.ResultValue;
                        }

                        #endregion

                        #region Methyrene
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119") != null)
                        {
                            page7.RowMethyrene.Visible = true;
                            page7.cellMethyreneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119")?.ReferenceRange;
                            page7.cellMethyrene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119" && p.Year == year1)?.ResultValue;
                            page7.cellMethyrene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119" && p.Year == year2)?.ResultValue;
                            page7.cellMethyrene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Styrene in Urine
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR195") != null)
                        {
                            page7.RowStyreneUrine.Visible = true;
                            page7.cellStyreneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR195")?.ReferenceRange;
                            page7.cellStyrene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR195" && p.Year == year1)?.ResultValue;
                            page7.cellStyrene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR195" && p.Year == year2)?.ResultValue;
                            page7.cellStyrene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR195" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Hexane
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118") != null)
                        {
                            page7.RowHexane.Visible = true;
                            page7.cellHexaneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118")?.ReferenceRange;
                            page7.cellHexane1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118" && p.Year == year1)?.ResultValue;
                            page7.cellHexane2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118" && p.Year == year2)?.ResultValue;
                            page7.cellHexane3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Isopropanol
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130") != null)
                        {
                            page7.RowIsopropanol.Visible = true;
                            page7.cellIsopropanolRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130")?.ReferenceRange;
                            page7.cellIsopropanol1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year1)?.ResultValue;
                            page7.cellIsopropanol2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year2)?.ResultValue;
                            page7.cellIsopropanol3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Chromium 

                        page7.cellChromiumRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132")?.ReferenceRange;
                        page7.cellChromium1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132" && p.Year == year1)?.ResultValue;
                        page7.cellChromium2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132" && p.Year == year2)?.ResultValue;
                        page7.cellChromium3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132" && p.Year == year3)?.ResultValue;

                        #endregion

                        #region Nickel  in blood
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131") != null)
                        {
                            page7.RowNicinblood.Visible = true;
                            page7.cellNickelRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131")?.ReferenceRange;
                            page7.cellNickel1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131" && p.Year == year1)?.ResultValue;
                            page7.cellNickel2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131" && p.Year == year2)?.ResultValue;
                            page7.cellNickel3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Nickel In Urine
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188") != null)
                        {
                            page7.RowNickelUrine.Visible = true;
                            page7.cellNickelUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188")?.ReferenceRange;
                            page7.cellNickelUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188" && p.Year == year1)?.ResultValue;
                            page7.cellNickelUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188" && p.Year == year2)?.ResultValue;
                            page7.cellNickelUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Acetone 

                        page7.cellAcetoneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117")?.ReferenceRange;
                        page7.cellAcetone1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117" && p.Year == year1)?.ResultValue;
                        page7.cellAcetone2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117" && p.Year == year2)?.ResultValue;
                        page7.cellAcetone3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117" && p.Year == year3)?.ResultValue;

                        #endregion

                        #region Aluminium in bloob
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR194") != null)
                        {
                            page7.RowAluminiumBlood.Visible = true;
                            page7.AluminiumBloodRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR194")?.ReferenceRange;
                            page7.AluminiumBlood1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR194" && p.Year == year1)?.ResultValue;
                            page7.AluminiumBlood2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR194" && p.Year == year2)?.ResultValue;
                            page7.AluminiumBlood3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR194" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Arsenic
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR199") != null)
                        {
                            page7.RowArsenic.Visible = true;
                            page7.ArsenicRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR199")?.ReferenceRange;
                            page7.Arsenic1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR199" && p.Year == year1)?.ResultValue;
                            page7.Arsenic2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR199" && p.Year == year2)?.ResultValue;
                            page7.Arsenic3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR199" && p.Year == year3)?.ResultValue;
                        }

                        #endregion

                        #region Cyclohexanone
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR202") != null)
                        {
                            page7.CyclohexanoneRow.Visible = true;
                            page7.CyclohexanoneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR202")?.ReferenceRange;
                            page7.Cyclohexanone1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR202" && p.Year == year1)?.ResultValue;
                            page7.Cyclohexanone2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR202" && p.Year == year2)?.ResultValue;
                            page7.Cyclohexanone3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR202" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Phenol
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR204") != null)
                        {
                            page7.RowPhenol.Visible = true;
                            page7.PhenolRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR204")?.ReferenceRange;
                            page7.Phenol1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR204" && p.Year == year1)?.ResultValue;
                            page7.Phenol2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR204" && p.Year == year2)?.ResultValue;
                            page7.Phenol3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR204" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region MIBK Urine 

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1232") != null)
                        {
                            page7.RowMibkUrine.Visible = true;
                            page7.cellMibkUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1232")?.ReferenceRange;
                            page7.cellMibkUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1232" && p.Year == year1)?.ResultValue;
                            page7.cellMibkUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1232" && p.Year == year2)?.ResultValue;
                            page7.cellMibkUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1232" && p.Year == year3)?.ResultValue;

                        }
                        #endregion

                        #region Cadmium Urine 

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1233") != null)
                        {
                            page7.RowCadmiumUrine.Visible = true;
                            page7.cellCadmiumUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1233")?.ReferenceRange;
                            page7.cellCadmiumUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1233" && p.Year == year1)?.ResultValue;
                            page7.cellCadmiumUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1233" && p.Year == year2)?.ResultValue;
                            page7.cellCadmiumUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1233" && p.Year == year3)?.ResultValue;

                        }
                        #endregion

                        #region Ethyl benzene in Urine

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1234") != null)
                        {
                            page7.RowEthylbenzeneUrine.Visible = true;
                            page7.cellEthylbenzeneUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1234")?.ReferenceRange;
                            page7.cellEthylbenzeneUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1234" && p.Year == year1)?.ResultValue;
                            page7.cellEthylbenzeneUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1234" && p.Year == year2)?.ResultValue;
                            page7.cellEthylbenzeneUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1234" && p.Year == year3)?.ResultValue;

                        }
                        #endregion

                        #region Mercury Urine

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1235") != null)
                        {
                            page7.RowMercuryUrine.Visible = true;
                            page7.cellMercuryUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1235")?.ReferenceRange;
                            page7.cellMercuryUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1235" && p.Year == year1)?.ResultValue;
                            page7.cellMercuryUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1235" && p.Year == year2)?.ResultValue;
                            page7.cellMercuryUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1235" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Methyrene chloride in Urine

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1236") != null)
                        {
                            page7.RowMethyreneUrine.Visible = true;
                            page7.cellMethyreneUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1236")?.ReferenceRange;
                            page7.cellMethyreneUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1236" && p.Year == year1)?.ResultValue;
                            page7.cellMethyreneUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1236" && p.Year == year2)?.ResultValue;
                            page7.cellMethyreneUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1236" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Benzene (t,t-Muconic acid) in Urine

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1215") != null)
                        {
                            page7.RowBenzenettUrine.Visible = true;
                            page7.cellBenzenettUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1215")?.ReferenceRange;
                            page7.cellBenzenettUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1215" && p.Year == year1)?.ResultValue;
                            page7.cellBenzenettUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1215" && p.Year == year2)?.ResultValue;
                            page7.cellBenzenettUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1215" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Mercury Blood (EDTA)

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1237") != null)
                        {
                            page7.RowMercuryBlood.Visible = true;
                            page7.cellMercuryBloodRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1237")?.ReferenceRange;
                            page7.cellMercuryBlood1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1237" && p.Year == year1)?.ResultValue;
                            page7.cellMercuryBlood2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1237" && p.Year == year2)?.ResultValue;
                            page7.cellMercuryBlood3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1237" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region fluoride  in Urine
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1261") != null)
                        {
                            page7.Rowfluoride.Visible = true;
                            page7.fluorideRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1261")?.ReferenceRange;
                            page7.fluorideY1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1261" && p.Year == year1)?.ResultValue;
                            page7.fluorideY2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1261" && p.Year == year2)?.ResultValue;
                            page7.fluorideY3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1261" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region formadehyde in Urine
                        //if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1237") != null)
                        //{
                        //    page6.RowFormadehyde.Visible = true;
                        //    page6.formadehydeRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1237")?.ReferenceRange;
                        //    page6.formadehydeY1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1237" && p.Year == year1)?.ResultValue;
                        //    page6.formadehydeY2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1237" && p.Year == year2)?.ResultValue;
                        //    page6.formadehydeY3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1237" && p.Year == year3)?.ResultValue;
                        //}
                        #endregion

                        #region 2,5 Hexanedion in urine

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1242") != null)
                        {
                            page7.Row25Hexan.Visible = true;
                            page7.cell25HexanRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1242")?.ReferenceRange;
                            page7.cell25Hexan1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1242" && p.Year == year1)?.ResultValue;
                            page7.cell25Hexan2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1242" && p.Year == year2)?.ResultValue;
                            page7.cell25Hexan3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1242" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Manganese in Blood 

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1270") != null)
                        {
                            page7.RowManganese.Visible = true;
                            page7.ManganeseRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1270")?.ReferenceRange;
                            page7.Manganes1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1270" && p.Year == year1)?.ResultValue;
                            page7.Manganes2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1270" && p.Year == year2)?.ResultValue;
                            page7.Manganes3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1270" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Manganese in Blood 

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1270") != null)
                        {
                            page7.RowManganese.Visible = true;
                            page7.ManganeseRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1270")?.ReferenceRange;
                            page7.Manganes1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1270" && p.Year == year1)?.ResultValue;
                            page7.Manganes2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1270" && p.Year == year2)?.ResultValue;
                            page7.Manganes3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1270" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Zinc zerum

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR141") != null)
                        {
                            page7.RowZinc.Visible = true;
                            page7.ZincRang.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR141")?.ReferenceRange;
                            page7.Zinc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR141" && p.Year == year1)?.ResultValue;
                            page7.Zinc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PPAR141" && p.Year == year2)?.ResultValue;
                            page7.Zinc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR141" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region iron zerum 

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR142") != null)
                        {
                            page7.RowIron.Visible = true;
                            page7.IronRang.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR142")?.ReferenceRange;
                            page7.Iron1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR142" && p.Year == year1)?.ResultValue;
                            page7.Iron2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PPAR142" && p.Year == year2)?.ResultValue;
                            page7.Iron3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR142" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region cadmiun in blood

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1269") != null)
                        {
                            page7.RowCadInb.Visible = true;
                            page7.CadinbRang.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1269")?.ReferenceRange;
                            page7.cadinb1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1269" && p.Year == year1)?.ResultValue;
                            page7.cadinb2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1269" && p.Year == year2)?.ResultValue;
                            page7.cadinb3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1269" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Chromiun in blood

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1268") != null)
                        {
                            page7.RowChroinB.Visible = true;
                            page7.ChroinBRang.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1268")?.ReferenceRange;
                            page7.ChroinB1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1268" && p.Year == year1)?.ResultValue;
                            page7.ChroinB2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1268" && p.Year == year2)?.ResultValue;
                            page7.ChroinB3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1268" && p.Year == year3)?.ResultValue;
                        }


                        #endregion


                        #region Chromiun in blood

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR171") != null)
                        {
                            page7.RowChroinS.Visible = true;
                            page7.ChroinSRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR171")?.ReferenceRange;
                            page7.ChroinS1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR171" && p.Year == year1)?.ResultValue;
                            page7.ChroinS2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR171" && p.Year == year2)?.ResultValue;
                            page7.ChroinS3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR171" && p.Year == year3)?.ResultValue;
                        }

                        #endregion

                        #region Ammo in blood

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1245") != null)
                        {
                            page7.RowAmmo.Visible = true;
                            page7.AmmoRang.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1245")?.ReferenceRange;
                            page7.Ammo1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1245" && p.Year == year1)?.ResultValue;
                            page7.Ammo2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1245" && p.Year == year2)?.ResultValue;
                            page7.Ammo3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1245" && p.Year == year3)?.ResultValue;
                        }

                        #endregion


                        #region Lean in Urin

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1278") != null)
                        {
                            page7.RowLeadInu.Visible = true;
                            page7.RangLeadU.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1278")?.ReferenceRange;
                            page7.LeadinU1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1278" && p.Year == year1)?.ResultValue;
                            page7.LeadinU2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1278" && p.Year == year2)?.ResultValue;
                            page7.LeadinU3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1278" && p.Year == year3)?.ResultValue;
                        }

                        #endregion

                        #region Cholinesterase in blood

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR126") != null)
                        {
                            page7.RowCholinesteraseBlood.Visible = true;
                            page7.CholinesterasebloodRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR126")?.ReferenceRange;
                            page7.Cholinesteraseblood1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR126" && p.Year == year1)?.ResultValue;
                            page7.Cholinesteraseblood2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR126" && p.Year == year2)?.ResultValue;
                            page7.Cholinesteraseblood3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR126" && p.Year == year3)?.ResultValue;
                        }

                        #endregion

                        #region Thinner in urine

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1285") != null)
                        {
                            page7.RowThinnerUrine.Visible = true;
                            page7.ThinnerUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1285")?.ReferenceRange;
                            page7.ThinnerUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1285" && p.Year == year1)?.ResultValue;
                            page7.ThinnerUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1285" && p.Year == year2)?.ResultValue;
                            page7.ThinnerUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1285" && p.Year == year3)?.ResultValue;
                        }

                        #endregion

                        #region Copper in blood

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR139") != null)
                        {
                            page7.RowCopperBlood.Visible = true;
                            page7.CopperbloodRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR139")?.ReferenceRange;
                            page7.CopperBlood1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR139" && p.Year == year1)?.ResultValue;
                            page7.CopperBlood2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PPAR139" && p.Year == year2)?.ResultValue;
                            page7.CopperBlood3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1139" && p.Year == year3)?.ResultValue;
                        }

                        #endregion

                        #region trichloro in Urine

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1300") != null)
                        {
                            page7.RowTrichloroUrine.Visible = true;
                            page7.TrichloroUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1300")?.ReferenceRange;
                            page7.TrichloroUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1300" && p.Year == year1)?.ResultValue;
                            page7.TrichloroUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1300" && p.Year == year2)?.ResultValue;
                            page7.TrichloroUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1300" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Direct Toluene in urine

                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1307") != null)
                        {
                            page7.rowDirectToluene.Visible = true;
                            page7.DirectTolueneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1307")?.ReferenceRange;
                            page7.DirectToluene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1307" && p.Year == year1)?.ResultValue;
                            page7.DirectToluene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1307" && p.Year == year2)?.ResultValue;
                            page7.DirectToluene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1307" && p.Year == year3)?.ResultValue;
                        }
                        #endregion

                        #region Ethanol in blood
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1271") != null)
                        {
                            page7.RowEthanolBlood.Visible = true;
                            page7.RangeEthanolBlood.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1271")?.ReferenceRange;
                            page7.EthanolBlood1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1271" && p.Year == year1)?.ResultValue;
                            page7.EthanolBlood2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1271" && p.Year == year2)?.ResultValue;
                            page7.EthanolBlood3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1271" && p.Year == year3)?.ResultValue;
                        }
                        #endregion
                    }
                    else
                    {
                        page7.cellToxicoYear1.Text = "ปี" + " " + DateTime.Now.Year;
                        page7.cellToxicoYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                        page7.cellToxicoYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
                    }

                    
                }
                else
                {
                    List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                    Years.OrderByDescending(p => ((uint?)p));
                    int countYear = Years.Count();
                    page7.cellToxicoYear1.Text = "ปี" + " " + DateTime.Now.Year;
                    page7.cellToxicoYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                    page7.cellToxicoYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
                }
            }
        }


        private void GenerateTumorMarker(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> oneYears = labTestSet.Select(p => p.Year).Distinct().ToList();
                if (oneYears.Count == 1)
                {
                    List<DateTime?> repeatYears = labTestSet.Select(p => p.StartDttm).Distinct().ToList();
                    repeatYears.Sort();

                    if (repeatYears.Count > 1)
                    {
                        DateTime? dateMax = DateTime.Now;
                        DateTime? dateMin = repeatYears[0];
                        foreach (DateTime dt in repeatYears)
                        {

                            if (dt > dateMin)
                            {
                                dateMax = dt;
                            }
                        }
                        int yearMax = dateMax.Value.Year;
                        int yearMin = yearMax - 1;

                        labTestSet.Where(w => w.StartDttm == dateMin).ToList().ForEach(i => i.Year = yearMin);
                    }
                }

                PatientResultComponentModel CheckGender = labTestSet.FirstOrDefault();
                int thisYear = DateTime.Now.Year;
                int? compare = thisYear - 2;
                List<int?> Years = labTestSet.Select(p => p.Year).Where(p => p.Value >= compare).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();

                if (countYear != 0)
                {
                    int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                    int? year2 = countYear >= 2 ? (Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1) : year1 - 1;
                    int? year3 = countYear >= 3 ? (Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1) : year2 - 1;
                    page6.cellTumorYear1.Text = "ปี" + " " + year1.ToString();
                    page6.cellTumorYear2.Text = "ปี" + " " + year2.ToString();
                    page6.cellTumorYear3.Text = "ปี" + " " + year3.ToString();

                    //cellAfpSIRange

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38") != null)
                    {
                        page6.cellAfpSIRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38")?.ReferenceRange;
                        page6.cellAfpSI1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38" && p.Year == year1)?.ResultValue;
                        page6.cellAfpSI2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38" && p.Year == year2)?.ResultValue;
                        page6.cellAfpSI3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38" && p.Year == year3)?.ResultValue;
                    }

                    //if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186") != null)
                    //{
                    //    page6.cellAfpSIRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186")?.ReferenceRange;
                    //    page6.cellAfpSI1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186" && p.Year == year1)?.ResultValue;
                    //    page6.cellAfpSI2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186" && p.Year == year2)?.ResultValue;
                    //    page6.cellAfpSI3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186" && p.Year == year3)?.ResultValue;
                    //}

                    //AFP โชว์ตัวเลขก่อนตัวอักษร

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39") != null)
                    {
                        page6.cellAfpRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39")?.ReferenceRange;
                        page6.cellAfp1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year1)?.ResultValue;
                        page6.cellAfp2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year2)?.ResultValue;
                        page6.cellAfp3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year3)?.ResultValue;
                    }
                    else
                    {
                        page6.cellAfpRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186")?.ReferenceRange;
                        page6.cellAfp1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186" && p.Year == year1)?.ResultValue;
                        page6.cellAfp2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186" && p.Year == year2)?.ResultValue;
                        page6.cellAfp3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186" && p.Year == year3)?.ResultValue;
                    }


                    page6.cellCa19Range.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114")?.ReferenceRange;
                    page6.cellCa19_1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year1)?.ResultValue;
                    page6.cellCa19_2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year2)?.ResultValue;
                    page6.cellCa19_3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year3)?.ResultValue;

                    if (CheckGender.SEXXXUID == 1)
                    {
                        //PSA โชว์ตัวเลขก่อนตัวอักษร
                        page6.RowCa125.Visible = false;
                        page6.RowCA153.Visible = false;
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4") != null)
                        {
                            page6.cellPsaRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4")?.ReferenceRange;
                            page6.cellPsa1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4" && p.Year == year1)?.ResultValue;
                            page6.cellPsa2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4" && p.Year == year2)?.ResultValue;
                            page6.cellPsa3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4" && p.Year == year3)?.ResultValue;
                        }
                        //else
                        //{
                        //    page6.cellPsaRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187")?.ReferenceRange;
                        //    page6.cellPsa1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187" && p.Year == year1)?.ResultValue;
                        //    page6.cellPsa2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187" && p.Year == year2)?.ResultValue;
                        //    page6.cellPsa3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187" && p.Year == year3)?.ResultValue;
                        //}
                    }


                    if (CheckGender.SEXXXUID == 1)
                    {
                        //PSA In โชว์ตัวเลขก่อนตัวอักษร
                        //if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4") != null)
                        //{
                        //    page6.cellPsaInRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4")?.ReferenceRange;
                        //    page6.cellPsaIn1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4" && p.Year == year1)?.ResultValue;
                        //    page6.cellPsa2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4" && p.Year == year2)?.ResultValue;
                        //    page6.cellPsa3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4" && p.Year == year3)?.ResultValue;
                        //}
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187") != null)
                        {
                            page6.cellPsaInRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187")?.ReferenceRange;
                            page6.cellPsaIn1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187" && p.Year == year1)?.ResultValue;
                            page6.cellPsaIn2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187" && p.Year == year2)?.ResultValue;
                            page6.cellPsaIn3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187" && p.Year == year3)?.ResultValue;
                        }
                    }


                    if (CheckGender.SEXXXUID == 2)
                    {
                        page6.RowPSA.Visible = false;
                        page6.RowPasIn.Visible = false;
                        page6.cellCa125Range.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41")?.ReferenceRange;
                        page6.cellCa125_1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41" && p.Year == year1)?.ResultValue;
                        page6.cellCa125_2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41" && p.Year == year2)?.ResultValue;
                        page6.cellCa125_3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41" && p.Year == year3)?.ResultValue;

                        page6.ca153Range.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR203")?.ReferenceRange;
                        page6.ca153_1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR203" && p.Year == year1)?.ResultValue;
                        page6.ca153_2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR203" && p.Year == year2)?.ResultValue;
                        page6.ca153_3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR203" && p.Year == year3)?.ResultValue;
                    }

                    //CEA โชว์ตัวเลขก่อนตัวอักษร

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR40") != null)
                    {
                        page6.cellCEARange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR40")?.ReferenceRange;
                        page6.cellCEA1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR40" && p.Year == year1)?.ResultValue;
                        page6.cellCEA2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR40" && p.Year == year2)?.ResultValue;
                        page6.cellCEA3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR40" && p.Year == year3)?.ResultValue;
                    }

                    else
                    {
                        if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR185") != null)
                        {
                            page6.cellCEAinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR185")?.ReferenceRange;
                            page6.cellCEAinYear1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR185" && p.Year == year1)?.ResultValue;
                            page6.cellCEAinYear2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR185" && p.Year == year2)?.ResultValue;
                            page6.cellCEAinYear3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR185" && p.Year == year3)?.ResultValue;
                        }
                    }
                }
                else
                {
                    page6.cellTumorYear1.Text = "ปี" + " " + DateTime.Now.Year;
                    page6.cellTumorYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                    page6.cellTumorYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
                }
            }
            else
            {
                
                page6.cellTumorYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page6.cellTumorYear2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                page6.cellTumorYear3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
            }
        }

        private void GenerateOther(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> oneYears = labTestSet.Select(p => p.Year).Distinct().ToList();
                if (oneYears.Count == 1)
                {
                    List<DateTime?> repeatYears = labTestSet.Select(p => p.StartDttm).Distinct().ToList();
                    repeatYears.Sort();

                    if (repeatYears.Count > 1)
                    {
                        DateTime? dateMax = DateTime.Now;
                        DateTime? dateMin = repeatYears[0];
                        foreach (DateTime dt in repeatYears)
                        {

                            if (dt > dateMin)
                            {
                                dateMax = dt;
                            }
                        }
                        int yearMax = dateMax.Value.Year;
                        int yearMin = yearMax - 1;

                        labTestSet.Where(w => w.StartDttm == dateMin).ToList().ForEach(i => i.Year = yearMin);
                    }
                }

                PatientResultComponentModel CheckGender = labTestSet.FirstOrDefault();
                int thisYear = DateTime.Now.Year;
                int? compare = thisYear - 2;
                List<int?> Years = labTestSet.Select(p => p.Year).Where(p => p.Value >= compare).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();

                if (countYear != 0)
                {
                    int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                    int? year2 = countYear >= 2 ? (Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1) : year1 - 1;
                    int? year3 = countYear >= 3 ? (Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1) : year2 - 1;
                    page6.cellOther2Year1.Text = "ปี" + " " + year1.ToString();
                    page6.cellOther2Year2.Text = "ปี" + " " + year2.ToString();
                    page6.cellOther2Year3.Text = "ปี" + " " + year3.ToString();


                    page6.cellAboGroupRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32")?.ReferenceRange;
                    page6.cellAboGroup1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32" && p.Year == year1)?.ResultValue;
                    page6.cellAboGroup2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32" && p.Year == year2)?.ResultValue;
                    page6.cellAboGroup3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32" && p.Year == year3)?.ResultValue;

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR59") != null)
                    {
                        page6.RowVDRL.Visible = true;
                        page6.cellBloodGroupRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR59")?.ReferenceRange;
                        page6.cellBloodGroup1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR59" && p.Year == year1)?.ResultValue;
                        page6.cellBloodGroup2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR59" && p.Year == year2)?.ResultValue;
                        page6.cellBloodGroup3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR59" && p.Year == year3)?.ResultValue;
                    }

                    page6.cellCalciumRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR79")?.ReferenceRange;
                    page6.cellCalcium1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR79" && p.Year == year1)?.ResultValue;
                    page6.cellCalcium2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR79" && p.Year == year2)?.ResultValue;
                    page6.cellCalcium3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR79" && p.Year == year3)?.ResultValue;

                    page6.cellTshRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR24")?.ReferenceRange;
                    page6.cellTsh1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR24" && p.Year == year1)?.ResultValue;
                    page6.cellTsh2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR24" && p.Year == year2)?.ResultValue;
                    page6.cellTsh3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR24" && p.Year == year3)?.ResultValue;


                    page6.T3Range.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR97")?.ReferenceRange;
                    page6.T3_1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR97" && p.Year == year1)?.ResultValue;
                    page6.T3_2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR97" && p.Year == year2)?.ResultValue;
                    page6.T3_3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR97" && p.Year == year3)?.ResultValue;

                    page6.T4Range.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR76")?.ReferenceRange;
                    page6.T4_1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR76" && p.Year == year1)?.ResultValue;
                    page6.T4_2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR76" && p.Year == year2)?.ResultValue;
                    page6.T4_3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR76" && p.Year == year3)?.ResultValue;

                    page6.FreeT3Range.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR22")?.ReferenceRange;
                    page6.FreeT3_1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR22" && p.Year == year1)?.ResultValue;
                    page6.FreeT3_2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR22" && p.Year == year2)?.ResultValue;
                    page6.FreeT3_3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR22" && p.Year == year3)?.ResultValue;

                    page6.FreeT4Range.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR23")?.ReferenceRange;
                    page6.FreeT4_1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR23" && p.Year == year1)?.ResultValue;
                    page6.FreeT4_2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR23" && p.Year == year2)?.ResultValue;
                    page6.FreeT4_3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR23" && p.Year == year3)?.ResultValue;

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1297") != null)
                    {
                        page6.RowHpylori.Visible = true;
                        page6.cellHpyloriRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1297")?.ReferenceRange;
                        page6.cellHpylori1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1297" && p.Year == year1)?.ResultValue;
                        page6.cellHpylori2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1297" && p.Year == year3)?.ResultValue;
                        page6.cellHpylori3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1297" && p.Year == year3)?.ResultValue;
                    }

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR207") != null)
                    {
                        page6.RowPhosphorus.Visible = true;
                        page6.PhosphorusRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR207")?.ReferenceRange;
                        page6.Phosphorus1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR207" && p.Year == year1)?.ResultValue;
                        page6.Phosphorus2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR207" && p.Year == year3)?.ResultValue;
                        page6.Phosphorus3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR207" && p.Year == year3)?.ResultValue;
                    }
                }
                else
                {
                    page6.cellOther2Year1.Text = "ปี" + " " + DateTime.Now.Year;
                    page6.cellOther2Year2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                    page6.cellOther2Year3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
                }
            }
            else
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.OrderByDescending(p => ((uint?)p));
                int countYear = Years.Count();
                page6.cellOther2Year1.Text = "ปี" + " " + DateTime.Now.Year;
                page6.cellOther2Year2.Text = "ปี" + " " + (DateTime.Now.Year - 1);
                page6.cellOther2Year3.Text = "ปี" + " " + (DateTime.Now.Year - 2);
            }
        }

        #endregion

        private void GenerateTimus(IEnumerable<PatientResultComponentModel> TimusResult)
        {
            if (TimusResult != null && TimusResult.Count() > 0)
            {
                page9.lbFarVision.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS19")?.ResultValue;
                page9.lbNearVision.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS20")?.ResultValue;
                page9.lb3DVision.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS21")?.ResultValue;
                page9.lbBalanceEye.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS23")?.ResultValue;
                page9.lbVisionColor.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS22")?.ResultValue;
                page9.lbFieldVision.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS24")?.ResultValue;
            }
        }

        private void GenerateAudio(IEnumerable<PatientResultComponentModel> AudioResult)
        {
            if (AudioResult != null && AudioResult.Count() > 0)
            {
                page9.R1.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO1")?.ResultValue;
                page9.R2.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO2")?.ResultValue;
                page9.R3.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO3")?.ResultValue;
                page9.R4.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO4")?.ResultValue;
                page9.R5.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO5")?.ResultValue;
                page9.R6.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO6")?.ResultValue;
                page9.R7.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO7")?.ResultValue;
                page9.lbAudioRight.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO8")?.ResultValue;

                page9.L1.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO9")?.ResultValue;
                page9.L2.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO10")?.ResultValue;
                page9.L3.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO11")?.ResultValue;
                page9.L4.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO12")?.ResultValue;
                page9.L5.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO13")?.ResultValue;
                page9.L6.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO14")?.ResultValue;
                page9.L7.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO15")?.ResultValue;
                page9.lbAudioLeft.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO16")?.ResultValue;
            }
        }

        private void GenerateSpiro(IEnumerable<PatientResultComponentModel> SpiroResult)
        {
            if (SpiroResult != null && SpiroResult.Count() > 0)
            {
                page9.lbFVCMeasure.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO1")?.ResultValue;
                page9.lbFVCPredic.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO2")?.ResultValue;
                page9.lbFVCPer.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO3")?.ResultValue;
                page9.lbFEV1Measure.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO4")?.ResultValue;
                page9.lbFEV1Predic.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO5")?.ResultValue;
                page9.lbFEV1Per.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO6")?.ResultValue;
                page9.lbFEVMeasure.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO7")?.ResultValue;
                page9.lbFEVPredic.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO8")?.ResultValue;
                page9.lbFEVPer.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO9")?.ResultValue;
            }
        }

        private void GeneratePhysicalExam(IEnumerable<PatientResultComponentModel> PhysicalExamResult)
        {
            if (PhysicalExamResult != null && PhysicalExamResult.Count() > 0)
            {
                page2.lbEye.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM1")?.ResultValue;
                page2.lbEars.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM2")?.ResultValue;
                page2.lbThroat.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM3")?.ResultValue;
                page2.lbNose.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM4")?.ResultValue;
                page2.lbTeeth.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM5")?.ResultValue;
                page2.lbLung.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM8")?.ResultValue;
                page2.lbHeart.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM9")?.ResultValue;
                page2.lbSkin.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM10")?.ResultValue;
                page2.lbThyroid.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM7")?.ResultValue;
                page2.lbLymphNode.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM6")?.ResultValue;
                page2.lbSmoke.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM11")?.ResultValue;
                page2.lbDrugAllergy.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM13")?.ResultValue;
                page2.lbAlcohol.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM12")?.ResultValue;
                page2.lbUnderlying.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM14")?.ResultValue;

                var waterCheck = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM15")?.ResultValue;
                if (waterCheck != null)
                {
                    if (waterCheck == "ไม่งด" || waterCheck == "NO")
                    {
                        page2.WaterN.Checked = true;

                    }
                    else
                    {
                        page2.waterY.Checked = true;
                    }
                }
            }
        }

        private void GeneratePhysicalExamRisk(IEnumerable<PatientResultComponentModel> PhysicalExamRiskResult)
        {
            if (PhysicalExamRiskResult != null && PhysicalExamRiskResult.Count() > 0)
            {

                page10.cellBalance.Text = PhysicalExamRiskResult.FirstOrDefault(p => p.ResultItemCode == "PAR1295")?.ResultValue;
                page10.cellMyofascialTop.Text = PhysicalExamRiskResult.FirstOrDefault(p => p.ResultItemCode == "PAR1301")?.ResultValue;
                page10.cellMyofascialBottom.Text = PhysicalExamRiskResult.FirstOrDefault(p => p.ResultItemCode == "PAR1302")?.ResultValue;
                page10.cellNeckROM.Text = PhysicalExamRiskResult.FirstOrDefault(p => p.ResultItemCode == "PAR1303")?.ResultValue;
                page10.cellRTShoulderROM.Text = PhysicalExamRiskResult.FirstOrDefault(p => p.ResultItemCode == "PAR1304")?.ResultValue;
                page10.cellLTShoulderROM.Text = PhysicalExamRiskResult.FirstOrDefault(p => p.ResultItemCode == "PAR1305")?.ResultValue;
                page10.cellLumbarROM.Text = PhysicalExamRiskResult.FirstOrDefault(p => p.ResultItemCode == "PAR1306")?.ResultValue;

                if (page10.cellMyofascialTop.Text != "" && page10.cellMyofascialBottom.Text != "" && page10.cellNeckROM.Text != "" && page10.cellRTShoulderROM.Text != "" && page10.cellLTShoulderROM.Text != "" && page10.cellLumbarROM.Text != "" )
                {
                    page10.RowRisk1.Visible = true;
                    page10.RowRisk2.Visible = true;

                    if (page10.cellMyofascialTop.Text == "มีความเสี่ยง" || page10.cellMyofascialBottom.Text == "มีความเสี่ยง" ||  page10.cellNeckROM.Text == "ผิดปกติ" || page10.cellRTShoulderROM.Text == "ผิดปกติ" || page10.cellLTShoulderROM.Text == "ผิดปกติ" || page10.cellLumbarROM.Text == "ผิดปกติ" )
                    {
                        page10.RiskRecommed.Text = "โครงสร้างและกล้ามเนื้ออยู่ในเกณฑ์มีความเสี่ยง หากอาการปวดหรืออาการชากระทบกับการดำเนินชีวิตประจำวัน ควรตรวจวินิจฉัยเพิ่มเติมโดยละเอียด และเข้ารับการรักษาที่เหมาะสม ร่วมกับการปรับพฤติกรรม เพื่อลดโอกาสการบาดเจ็บเรื้อรัง";
                    }
                    else
                    {
                        page10.RiskRecommed.Text = "โครงสร้างและกล้ามเนื้ออยู่ในเกณฑ์ปกติ ควรยืดเหยียดกล้ามเนื้อและออกกำลังกายสม่ำเสมออย่างเหมาะสม เพื่อลดความเสี่ยงการบาดเจ็บของโครงสร้างและกล้ามเนื้อ";
                    }
                }
            }
        }

        private void GenerateVisualAcuityTest(IEnumerable<PatientResultComponentModel> VisualAcuityTestResult)
        {
            if (VisualAcuityTestResult != null && VisualAcuityTestResult.Count() > 0)
            {
                page10.lbMyopiaRight.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY1")?.ResultValue;
                page10.lbAstigmaticRight.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY2")?.ResultValue;
                page10.lbViewRight.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY3")?.ResultValue;
                page10.lbHyperopiaRight.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY4")?.ResultValue;
                page10.lbVARight.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY5")?.ResultValue;
                page10.lbMyopiaLeft.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY6")?.ResultValue;
                page10.lbAstigmaticLeft.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY7")?.ResultValue;
                page10.lbViewLeft.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY8")?.ResultValue;
                page10.lbHyperopiaLeft.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY9")?.ResultValue;
                page10.lbVALeft.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY10")?.ResultValue;
                page10.lbBlindColor.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY11")?.ResultValue;
                page10.lbViewResult.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY12")?.ResultValue;
                page10.lbViewRecommend.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY13")?.ResultValue;
            }
        }

        private void GenerateBackStrength(IEnumerable<PatientResultComponentModel> BackStrength)
        {
            if (BackStrength != null && BackStrength.Count() > 0)
            {
                page8.lbBackValue.Text = BackStrength.FirstOrDefault(p => p.ResultItemCode == "MUCS1")?.ResultValue;
                page8.lbBackStrenght.Text = BackStrength.FirstOrDefault(p => p.ResultItemCode == "MUCS2")?.ResultValue;
                page8.lbValueLegStrength.Text = BackStrength.FirstOrDefault(p => p.ResultItemCode == "MUCS5")?.ResultValue;
                page8.lbLegStrength.Text = BackStrength.FirstOrDefault(p => p.ResultItemCode == "MUCS6")?.ResultValue;
                page8.lbValueGripStrength.Text = BackStrength.FirstOrDefault(p => p.ResultItemCode == "MUCS3")?.ResultValue;
                page8.lbGripStrength.Text = BackStrength.FirstOrDefault(p => p.ResultItemCode == "MUCS4")?.ResultValue;
            }
        }

        private void GenerateOccmedGroup(IEnumerable<CheckupGroupResultModel> occmedGroupResult)
        {
            if (occmedGroupResult != null && occmedGroupResult.Count() > 0)
            {
                page9.lbLungResult.Text = occmedGroupResult.FirstOrDefault(p => p.GroupCode == "GPRST33")?.ResultStatus.ToString();
                page9.lbLungRecommend.Text = occmedGroupResult.FirstOrDefault(p => p.GroupCode == "GPRST33")?.Conclusion.ToString();

                string eyeOccmedConclustion = occmedGroupResult.FirstOrDefault(p => p.GroupCode == "GPRST26")?.Conclusion.ToString();
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
                        else if(item != null)
                        {
                            recommand = item.ToString();
                        }


                    }

                    page9.lbVisionOccmedResult.Text = description;
                    page9.lbVisionOccmedRecommend.Text = recommand;
                }


                page9.lbAudioResult.Text = occmedGroupResult.FirstOrDefault(p => p.GroupCode == "GPRST25")?.ResultStatus.ToString();
                page9.lbAudioRecommend.Text = occmedGroupResult.FirstOrDefault(p => p.GroupCode == "GPRST25")?.Conclusion.ToString();

                page8.lbMuscleResult.Text = occmedGroupResult.FirstOrDefault(p => p.GroupCode == "GPRST32")?.Conclusion;
            }
        }


        private void CheckupPage1_AfterPrint(object sender, EventArgs e)
        {
            page2.CreateDocument();
            page3.CreateDocument();
            page4.CreateDocument();
            page5.CreateDocument();
            page6.CreateDocument();
            labA.CreateDocument();
            labB.CreateDocument();
            labC.CreateDocument();
            labD.CreateDocument();
            labE.CreateDocument();
            labF.CreateDocument();
            labG.CreateDocument();
            labH.CreateDocument();
            page7.CreateDocument();
            page8.CreateDocument();
            page9.CreateDocument();
            page10.CreateDocument();
            page12.CreateDocument();
            page11.CreateDocument();
            this.Pages.AddRange(page2.Pages);
            this.Pages.AddRange(page3.Pages);
            this.Pages.AddRange(page4.Pages);
            this.Pages.AddRange(page5.Pages);
            this.Pages.AddRange(page6.Pages);
            this.Pages.AddRange(labA.Pages);
            this.Pages.AddRange(labB.Pages);
            this.Pages.AddRange(labC.Pages);
            this.Pages.AddRange(labD.Pages);
            this.Pages.AddRange(labE.Pages);
            this.Pages.AddRange(labF.Pages);
            this.Pages.AddRange(labG.Pages);
            this.Pages.AddRange(labH.Pages);
            this.Pages.AddRange(page7.Pages);
            this.Pages.AddRange(page8.Pages);
            this.Pages.AddRange(page9.Pages);
            this.Pages.AddRange(page10.Pages);
            this.Pages.AddRange(page11.Pages);
            this.Pages.AddRange(page12.Pages);
   


        }

        public string TranslateXray(string resultValue, string resultStatus, string requestItemName)
        {
            if (dtResultMapping == null)
            {
                dtResultMapping = DataService.Radiology.GetXrayTranslateMapping();
            }

            List<string> listNoMapResult = new List<string>();
            string thairesult = TranslateResult.TranslateResultXray(resultValue, resultStatus, requestItemName, " // ", dtResultMapping, ref listNoMapResult);

            return thairesult;
        }


    }
}
