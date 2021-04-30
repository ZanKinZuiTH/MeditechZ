using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using MediTech.Model;
using System.Collections.Generic;
using MediTech.Helpers;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using DevExpress.XtraPrinting;
using MediTech.Model.Report;

namespace MediTech.Reports.Operating.Checkup.CheckupBookReport
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


        public CheckupPage1()
        {
            InitializeComponent();
            BeforePrint += CheckupPage1_BeforePrint;
            AfterPrint += CheckupPage1_AfterPrint;

            lbResultWellness2.BeforePrint += LbResultWellness2_BeforePrint;
        }

        private void LbResultWellness2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel label = page2.lbResultWellness;
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
                var result1 = page2.lbResultWellness.Lines.Where(p => !string.IsNullOrEmpty(p)).ToList();
                var result2 = lbResultWellness2.Lines.Where(p => !string.IsNullOrEmpty(p)).ToList();
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

                page2.lbResultWellness.Lines = result1.ToArray();
                lbResultWellness2.Lines = result2.ToArray();
                LbResultWellness2_BeforePrint(null, null);
            }
        }



        private void CheckupPage1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            long patientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long patientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            int payorDetailUID = int.Parse(this.Parameters["PayorDetailUID"].Value.ToString());
            PatientWellnessModel data = DataService.Reports.PrintWellnessBook(patientUID, patientVisitUID, payorDetailUID);

            if (data.PatientInfomation != null)
            {
                var patient = data.PatientInfomation;
                var groupResult = data.GroupResult;

                #region Show HN/Name
                page2.lbHN2.Text = patient.PatientID;
                page2.lbName2.Text = patient.PatientName;
                page2.lbHN15.Text = patient.PatientID;
                page2.lbName15.Text = patient.PatientName;

                page3.lbHN3.Text = patient.PatientID;
                page3.lbName3.Text = patient.PatientName;
                page3.lbHN14.Text = patient.PatientID;
                page3.lbName14.Text = patient.PatientName;

                page4.lbHN4.Text = patient.PatientID;
                page4.lbName4.Text = patient.PatientName;
                page4.lbHN13.Text = patient.PatientID;
                page4.lbName13.Text = patient.PatientName;

                page5.lbHN5.Text = patient.PatientID;
                page5.lbName5.Text = patient.PatientName;
                page5.lbHN12.Text = patient.PatientID;
                page5.lbName12.Text = patient.PatientName;

                page6.lbHN6.Text = patient.PatientID;
                page6.lbName6.Text = patient.PatientName;
                page6.lbHN11.Text = patient.PatientID;
                page6.lbName11.Text = patient.PatientName;

                page7.lbHN7.Text = patient.PatientID;
                page7.lbName7.Text = patient.PatientName;
                page7.lbHN10.Text = patient.PatientID;
                page7.lbName10.Text = patient.PatientName;

                page8.lbHN8.Text = patient.PatientID;
                page8.lbName8.Text = patient.PatientName;
                page8.lbHN9.Text = patient.PatientID;
                page8.lbName9.Text = patient.PatientName;

                lbHN16.Text = patient.PatientID;
                lbName16.Text = patient.PatientName;
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
                lbAge.Text = patient.Age != null ? patient.Age + " ปี" : "";
                lbGender.Text = patient.Gender;

                lbHeight.Text = patient.Height != null ? patient.Height.ToString() + " cm." : "";
                lbWeight.Text = patient.Weight != null ? patient.Weight.ToString() + " kg." : "";
                lbBMI.Text = patient.BMI != null ? patient.BMI.ToString() + " kg/m2" : "";
                lbBP.Text = (patient.BPSys != null ? patient.BPSys.ToString() : "") + (patient.BPDio != null ? "/" + patient.BPDio.ToString() : "");
                lbPulse.Text = patient.Pulse != null ? patient.Pulse.ToString() + " ครั้ง/นาที" : "";
                lbWaist.Text = patient.WaistCircumference != null ? patient.WaistCircumference.ToString() + " cm." : "";

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

                    page2.lbResultWellness.Text = sb.ToString();
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
                                lbObesity.Font = new Font("Angsana New", 11, FontStyle.Bold);
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
                    page6.lbChest.Text = chestPA;
                }
                else
                {
                    page6.lbChest.Text = "-";
                }

                if (!string.IsNullOrEmpty(mammogram))
                {
                    page6.lbMam.Text = mammogram;
                }
                else
                {
                    page6.lbMam.Text = "-";
                }

                if (!string.IsNullOrEmpty(ultrasound))
                {
                    page6.lbUlt.Text = ultrasound;
                }
                else
                {
                    page6.lbUlt.Text = "-";
                }


                if (!string.IsNullOrEmpty(ultrasound_thyroid))
                {
                    page6.lbThyroid.Text = ultrasound_thyroid;
                }
                else
                {
                    page6.lbThyroid.Text = "-";
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
                                        page6.lbChest.Text = "Normal";
                                    }
                                    else
                                    {
                                        page6.lbChest.Text = ResultChestEn;
                                    }
                                }
                                else if (ChestResult[0].ToLower().Contains("old fracture right clavicle"))
                                {
                                    page6.lbChest.Text = "Calcification in aorta Old fracture of right clavicle";
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
                                string MamResultEn = MamResult[1].Replace("\n", "").Replace("\r", "").Replace("\r\n", "");
                                MamResultEn = MamResultEn.Trim();
                                page6.lbMam.Text = MamResultEn;
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
                                string UltResultEn = UltResult[1].Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(":", "");
                                UltResultEn = UltResultEn.Trim();
                                page6.lbUlt.Text = UltResultEn;
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

                page6.lbEKGRecommend.Text = groupResult.FirstOrDefault(p => p.GroupCode == "GPRST23")?.Conclusion;
                //page4.lbFarVision.Text = dataFirstOrDefault(p => p.ResultItemCode == "TIMUS19")?.ResultValue;        

                var labCompare = data.LabCompare;
                if (labCompare != null)
                {
                    #region Complete Blood Count

                    IEnumerable<PatientResultComponentModel> cbcTestSet = labCompare
                    .Where(p => p.RequestItemName.Contains("CBC"))
                    .OrderBy(p => p.Year);
                    GenerateCompleteBloodCount(cbcTestSet);
                    #endregion

                    #region Urinalysis
                    IEnumerable<PatientResultComponentModel> uaTestSet = labCompare
                        .Where(p => p.RequestItemName.Contains("UA"))
                        .OrderBy(p => p.Year);
                    GenerateUrinalysis(uaTestSet);

                    #endregion

                    #region Renal function
                    IEnumerable<PatientResultComponentModel> RenalTestSet = labCompare
                        .Where(p => p.RequestItemCode.Contains("LAB212")
                        || p.RequestItemCode.Contains("LAB211")
                        || p.RequestItemCode.Contains("LAB213"))
                        .OrderBy(p => p.Year);
                    GenerateRenalFunction(RenalTestSet);

                    #endregion

                    #region Fasting Blood Sugar
                    IEnumerable<PatientResultComponentModel> FbsTestSet = labCompare
                        .Where(p => p.RequestItemCode.Contains("LAB231")
                        || p.RequestItemCode.Contains("LAB232"))
                        .OrderBy(p => p.Year);
                    GenerateFastingBloodSugar(FbsTestSet);

                    #endregion

                    #region Uric acid
                    IEnumerable<PatientResultComponentModel> UricTestSet = labCompare
                        .Where(p => p.RequestItemCode.Contains("LAB261"))
                        .OrderBy(p => p.Year);
                    GenerateUricAcid(UricTestSet);

                    #endregion

                    #region Lipid Profiles 
                    IEnumerable<PatientResultComponentModel> LipidTestSet = labCompare
                        .Where(p => p.RequestItemCode.Contains("LAB241")
                        || p.RequestItemCode.Contains("LAB242")
                        || p.RequestItemCode.Contains("LAB243")
                        || p.RequestItemCode.Contains("LAB244"))
                        .OrderBy(p => p.Year);
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
                        || p.RequestItemCode.Contains("LAB226"))
                        .OrderBy(p => p.Year);
                    GenerateLiverFunction(LiverTestSet);
                    #endregion

                    #region Immunology and Virology
                    IEnumerable<PatientResultComponentModel> ImmunologyTestSet = labCompare
                        .Where(p => p.RequestItemCode.Contains("LAB451")
                        || p.RequestItemCode.Contains("LAB441")
                        || p.RequestItemCode.Contains("LAB512")
                        || p.RequestItemCode.Contains("LAB554"))
                        .OrderBy(p => p.Year);
                    GenerateImmunology(ImmunologyTestSet);
                    #endregion

                    #region Stool Exam
                    IEnumerable<PatientResultComponentModel> StoolTestSet = labCompare
                        .Where(p => p.RequestItemName.Contains("Stool Examination"))
                        .OrderBy(p => p.Year);
                    GenerateStool(StoolTestSet);
                    #endregion

                    #region Stool Culture
                    IEnumerable<PatientResultComponentModel> StoolCultureTestSet = labCompare
                        .Where(p => p.RequestItemCode.Contains("LAB322"))
                        .OrderBy(p => p.Year);
                    GenerateStoolCulture(StoolCultureTestSet);
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
                        || p.RequestItemCode.Contains("LAB570") //MIBK in  Urine
                        || p.RequestItemCode.Contains("LAB571") //Cadmium in Urine
                        || p.RequestItemCode.Contains("LAB572") //Ethyl benzene in urine
                        || p.RequestItemCode.Contains("LAB489") //Mercury in Urine
                        || p.RequestItemCode.Contains("LAB573") //Methyrene chloride in Urine
                        || p.RequestItemCode.Contains("LAB568")) //Benzene (t,t-Muconic acid) in Urine
                        .OrderBy(p => p.Year);
                    GenerateToxicology(ToxicoTestSet);
                    #endregion

                    #region tumor marker
                    IEnumerable<PatientResultComponentModel> TumorMarker = labCompare
                        .Where(p => p.RequestItemCode.Contains("LAB281") //afp
                        || p.RequestItemCode.Contains("LAB282") //ca19-9
                        || p.RequestItemCode.Contains("LAB283") //cea
                        || p.RequestItemCode.Contains("LAB284") //psa
                        || p.RequestItemCode.Contains("LAB285") //ca125
                        || p.RequestItemCode.Contains("LAB286")) //ca153
                        .OrderBy(p => p.Year);
                    GenerateTumorMarker(TumorMarker);
                    #endregion

                    #region Other Lab Teat
                    IEnumerable<PatientResultComponentModel> OtherTestSet = labCompare
                        .Where(p => p.RequestItemCode.Contains("LAB411") //blood 
                        || p.RequestItemCode.Contains("LAB251") //calcium
                        || p.RequestItemCode.Contains("LAB271") //tsh
                        || p.RequestItemCode.Contains("LAB272") //T3
                        || p.RequestItemCode.Contains("LAB273") //T4
                        || p.RequestItemCode.Contains("LAB274") //FreeT3
                        || p.RequestItemCode.Contains("LAB275")) //FreeT4
                        .OrderBy(p => p.Year);
                    GenerateOther(OtherTestSet);
                    #endregion

                    #region muscle
                    IEnumerable<PatientResultComponentModel> commentMuscle = labCompare
                       .Where(p => p.RequestItemCode.Contains("MUSCLEBA")
                       || p.RequestItemCode.Contains("MUSCLEGR")
                       || p.RequestItemCode.Contains("MUSCLELEG"));
                    GenerateCommentMuscle(commentMuscle);
                    #endregion
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
                    TitleResultWellness2.Text = "Summary";
                    page2.TitleResultWellness.Text = "Summary";
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

                    page4.TitlePulmonaryResult.Text = "Summary";
                    page4.TitlePulmonaryRecommend.Text = "Suggestion";

                    if (page4.lbLungResult.Text == "ปกติ")
                    {
                        page4.lbLungResult.Text = "Normal";
                        page4.lbLungRecommend.Text = "Annualy check up";
                    }
                    if (page4.lbLungResult.Text == "ผิดปกติ")
                    {
                        page4.lbLungResult.Text = "Abnormal";
                    }

                    page4.TitleFarVision.Text = "Far Test";
                    page4.TitleNearVision.Text = "Near Test";
                    page4.Title3DVision.Text = "3D Test";
                    page4.TitleBalanceEye.Text = "Eye Balance";
                    page4.TitleColor.Text = "Color";
                    page4.TitleFieldVision.Text = "Visual Field";
                    page4.TitleVisionOccmedResult.Text = "Summary";
                    page4.TitleVisionOccmedRecommend.Text = "Suggestion";

                    if (page4.lbVisionOccmedResult.Text == "ปกติ")
                    {
                        page4.lbVisionOccmedResult.Text = "Normal";
                    }
                    if (page4.lbVisionOccmedResult.Text == "ผิดปกติ")
                    {
                        page4.lbVisionOccmedResult.Text = "Abnormal";
                    }

                    page5.TitleAudiogram.Text = "Audiogram";
                    page5.TitleAudioListResult.Text = "Result";
                    page5.TitleAudioRight.Text = "Right ear";
                    page5.TitleAudioLeft.Text = "Left ear";
                    page5.TitleAudioResult.Text = "Summary";
                    page5.TitleAudioRecommend.Text = "Suggestion";

                    if (page5.lbAudioLeft.Text == "ไม่พบความผิดปกติ")
                    {
                        page5.lbAudioLeft.Text = "Normal";
                    }
                    if (page5.lbAudioRight.Text == "ไม่พบความผิดปกติ")
                    {
                        page5.lbAudioRight.Text = "Normal";
                    }
                    if (page5.lbAudioResult.Text == "ปกติ")
                    {
                        page5.lbAudioResult.Text = "Normal";
                        page5.lbAudioRecommend.Text = "Annualy check up";
                    }
                    if (page5.lbAudioResult.Text == "เฝ้าระวัง")
                    {
                        page5.lbAudioResult.Text = "Mild abnormality";
                    }
                    if (page5.lbAudioResult.Text == "ผิดปกตื")
                    {
                        page5.lbAudioResult.Text = "Abnormal";
                    }

                    page5.TitleMyopiaRight.Text = "Shortsighted Rt.";
                    page5.TitleMyopiaLeft.Text = "Shortsighted Lt.";
                    page5.TitleAstigmaticRight.Text = "Astigmatic Rt.";
                    page5.TitleAstigmaticLeft.Text = "Astigmatic Lt.";
                    page5.TitleViewRight.Text = "Degree Rt.";
                    page5.TitleViewLeft.Text = "Degree Lt.";
                    page5.TitleHyperopiaRight.Text = "Longsighted Rt.";
                    page5.TitleHyperopiaLeft.Text = "Longsighted Lt.";
                    page5.TitleVARight.Text = "VA Rt.";
                    page5.TitleVALeft.Text = "VA Lt.";
                    page5.TitleBlindColor.Text = "Color Blindness";
                    page5.TitleViewResult.Text = "Result";
                    page5.TitleViewRecommend.Text = "Suggestion";


                }
            }
        }

        #region Result Lab 
        private void GenerateCompleteBloodCount(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                page3.cellHbRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001")?.ReferenceRange;
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;
                page3.cellCBCYear1.Text = "ปี" + " " + year1.ToString();
                page3.cellCBCYear2.Text = "ปี" + " " + year2.ToString();
                page3.cellCBCYear3.Text = "ปี" + " " + year3.ToString();

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
                page3.cellCBCYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page3.cellCBCYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }

        }

        private void GenerateUrinalysis(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;
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

                page4.cellCastsRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR16")?.ReferenceRange;
                page4.cellCasts1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR16" && p.Year == year1)?.ResultValue;
                page4.cellCasts2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR16" && p.Year == year2)?.ResultValue;
                page4.cellCasts3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR16" && p.Year == year3)?.ResultValue;

                page4.cellBacteriaRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0155")?.ReferenceRange;
                page4.cellBacteria1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0155" && p.Year == year1)?.ResultValue;
                page4.cellBacteria2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0155" && p.Year == year2)?.ResultValue;
                page4.cellBacteria3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0155" && p.Year == year3)?.ResultValue;

                page4.cellBuddingYeastRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR17")?.ReferenceRange;
                page4.cellBuddingYeast1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR17" && p.Year == year1)?.ResultValue;
                page4.cellBuddingYeast2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR17" && p.Year == year2)?.ResultValue;
                page4.cellBuddingYeast3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR17" && p.Year == year3)?.ResultValue;

                page4.cellCrystalRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR19")?.ReferenceRange;
                page4.cellCrystal1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR19" && p.Year == year1)?.ResultValue;
                page4.cellCrystal2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR19" && p.Year == year2)?.ResultValue;
                page4.cellCrystal3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR19" && p.Year == year3)?.ResultValue;

                page4.cellMucousRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR18")?.ReferenceRange;
                page4.cellMucous1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR18" && p.Year == year1)?.ResultValue;
                page4.cellMucous2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR18" && p.Year == year2)?.ResultValue;
                page4.cellMucous3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR18" && p.Year == year3)?.ResultValue;

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
                page4.cellUAYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page4.cellUAYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }

        private void GenerateRenalFunction(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;
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
                page5.cellRenalYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page5.cellRenalYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }

        private void GenerateFastingBloodSugar(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;
                page6.cellFbsYear1.Text = "ปี" + " " + year1.ToString();
                page6.cellFbsYear2.Text = "ปี" + " " + year2.ToString();
                page6.cellFbsYear3.Text = "ปี" + " " + year3.ToString();

                page6.cellFbsRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0180")?.ReferenceRange;
                page6.cellFbs1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0180" && p.Year == year1)?.ResultValue;
                page6.cellFbs2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0180" && p.Year == year2)?.ResultValue;
                page6.cellFbs3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0180" && p.Year == year3)?.ResultValue;

                page6.cellHbA1cRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR7")?.ReferenceRange;
                page6.cellHbA1c1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR7" && p.Year == year1)?.ResultValue;
                page6.cellHbA1c2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR7" && p.Year == year2)?.ResultValue;
                page6.cellHbA1c3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR7" && p.Year == year3)?.ResultValue;

            }
            else
            {
                page6.cellFbsYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page6.cellFbsYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page6.cellFbsYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }

        private void GenerateUricAcid(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;
                page7.cellUricYear1.Text = "ปี" + " " + year1.ToString();
                page7.cellUricYear2.Text = "ปี" + " " + year2.ToString();
                page7.cellUricYear3.Text = "ปี" + " " + year3.ToString();

                page7.cellUricRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320")?.ReferenceRange;
                page7.cellUric1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320" && p.Year == year1)?.ResultValue;
                page7.cellUric2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320" && p.Year == year2)?.ResultValue;
                page7.cellUric3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320" && p.Year == year3)?.ResultValue;

            }
            else
            {
                page7.cellUricYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page7.cellUricYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page7.cellUricYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }

        private void GenerateLipidProfiles(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;
                page6.cellLipidYear1.Text = "ปี" + " " + year1.ToString();
                page6.cellLipidYear2.Text = "ปี" + " " + year2.ToString();
                page6.cellLipidYear3.Text = "ปี" + " " + year3.ToString();

                page6.cellCholesterolRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130")?.ReferenceRange;
                page6.cellCholesterol1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130" && p.Year == year1)?.ResultValue;
                page6.cellCholesterol2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130" && p.Year == year2)?.ResultValue;
                page6.cellCholesterol3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130" && p.Year == year3)?.ResultValue;

                page6.cellTriglycerideRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140")?.ReferenceRange;
                page6.cellTriglyceride1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140" && p.Year == year1)?.ResultValue;
                page6.cellTriglyceride2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140" && p.Year == year2)?.ResultValue;
                page6.cellTriglyceride3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140" && p.Year == year3)?.ResultValue;

                page6.cellLdlRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159")?.ReferenceRange;
                page6.cellLdl1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159" && p.Year == year1)?.ResultValue;
                page6.cellLdl2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159" && p.Year == year2)?.ResultValue;
                page6.cellLdl3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159" && p.Year == year3)?.ResultValue;

                page6.cellHdlRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150")?.ReferenceRange;
                page6.cellHdl1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150" && p.Year == year1)?.ResultValue;
                page6.cellHdl2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150" && p.Year == year2)?.ResultValue;
                page6.cellHdl3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150" && p.Year == year3)?.ResultValue;

            }
            else
            {
                page6.cellLipidYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page6.cellLipidYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page6.cellLipidYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }

        private void GenerateLiverFunction(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;
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

                page5.cellAlbuminRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR101")?.ReferenceRange;
                page5.cellAlbumin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR101" && p.Year == year1)?.ResultValue;
                page5.cellAlbumin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR101" && p.Year == year2)?.ResultValue;
                page5.cellAlbumin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR101" && p.Year == year3)?.ResultValue;

                page5.cellGlobulinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46")?.ReferenceRange;
                page5.cellGlobulin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year1)?.ResultValue;
                page5.cellGlobulin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year2)?.ResultValue;
                page5.cellGlobulin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year3)?.ResultValue;

            }
            else
            {
                page5.cellLiverYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page5.cellLiverYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page5.cellLiverYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }

        private void GenerateImmunology(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;
                page7.cellImmunlogyYear1.Text = "ปี" + " " + year1.ToString();
                page7.cellImmunlogyYear2.Text = "ปี" + " " + year2.ToString();
                page7.cellImmunlogyYear3.Text = "ปี" + " " + year3.ToString();

                page7.cellHbsAgRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35")?.ReferenceRange;
                page7.cellHbsAg1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35" && p.Year == year1)?.ResultValue;
                page7.cellHbsAg2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35" && p.Year == year2)?.ResultValue;
                page7.cellHbsAg3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35" && p.Year == year3)?.ResultValue;

                page7.cellCoiAgRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34")?.ReferenceRange;
                page7.cellCoiAg1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34" && p.Year == year1)?.ResultValue;
                page7.cellCoiAg2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34" && p.Year == year2)?.ResultValue;
                page7.cellCoiAg3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34" && p.Year == year3)?.ResultValue;

                page7.cellCoiHbsRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121")?.ReferenceRange;
                page7.cellCoiHbs1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121" && p.Year == year1)?.ResultValue;
                page7.cellCoiHbs2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121" && p.Year == year2)?.ResultValue;
                page7.cellCoiHbs3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121" && p.Year == year3)?.ResultValue;

                page7.cellAntiHbsRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42")?.ReferenceRange;
                page7.cellAntiHbs1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42" && p.Year == year1)?.ResultValue;
                page7.cellAntiHbs2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42" && p.Year == year2)?.ResultValue;
                page7.cellAntiHbs3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42" && p.Year == year3)?.ResultValue;

                page7.cellHavIgmRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR192")?.ReferenceRange;
                page7.cellHavIgm1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR192" && p.Year == year1)?.ResultValue;
                page7.cellHavIgm2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR192" && p.Year == year2)?.ResultValue;
                page7.cellHavIgm3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR192" && p.Year == year3)?.ResultValue;

                page7.cellHavIggRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR190")?.ReferenceRange;
                page7.cellHavIgg1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR190" && p.Year == year1)?.ResultValue;
                page7.cellHavIgg2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR190" && p.Year == year2)?.ResultValue;
                page7.cellHavIgg3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR190" && p.Year == year3)?.ResultValue;
            }
            else
            {
                page7.cellImmunlogyYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page7.cellImmunlogyYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page7.cellImmunlogyYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }

        private void GenerateStool(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;
                page8.cellStoolYear1.Text = "ปี" + " " + year1.ToString();
                page8.cellStoolYear2.Text = "ปี" + " " + year2.ToString();
                page8.cellStoolYear3.Text = "ปี" + " " + year3.ToString();

                page8.cellStColorRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR69")?.ReferenceRange;
                page8.cellStColor1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR69" && p.Year == year1)?.ResultValue;
                page8.cellStColor2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR69" && p.Year == year2)?.ResultValue;
                page8.cellStColor3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR69" && p.Year == year3)?.ResultValue;


                page8.cellStappearRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21")?.ReferenceRange;
                page8.cellStappear1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year1)?.ResultValue;
                page8.cellStappear2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year2)?.ResultValue;
                page8.cellStappear2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year3)?.ResultValue;


                page8.stoolOvaRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR73")?.ReferenceRange;
                page8.stoolOva1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR73" && p.Year == year1)?.ResultValue;
                page8.stoolOva2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR73" && p.Year == year2)?.ResultValue;
                page8.stoolOva3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR73" && p.Year == year3)?.ResultValue;


                page8.stoolWbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR71")?.ReferenceRange;
                page8.stoolWbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR71" && p.Year == year1)?.ResultValue;
                page8.stoolWbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR71" && p.Year == year2)?.ResultValue;
                page8.stoolWbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR71" && p.Year == year3)?.ResultValue;


                page8.stoolRbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR72")?.ReferenceRange;
                page8.stoolRbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR72" && p.Year == year1)?.ResultValue;
                page8.stoolRbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR72" && p.Year == year2)?.ResultValue;
                page8.stoolRbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR72" && p.Year == year3)?.ResultValue;
            }
            else
            {
                page8.cellStoolYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page8.cellStoolYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page8.cellStoolYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }

        private void GenerateStoolCulture(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;

                page8.StoolCultureYear1.Text = "ปี" + " " + year1.ToString();
                page8.StoolCultureYear2.Text = "ปี" + " " + year2.ToString();
                page8.StoolCultureYear3.Text = "ปี" + " " + year3.ToString();

                page8.cellStColorRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR189")?.ReferenceRange;
                page8.cellStoolCulter1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR189" && p.Year == year1)?.ResultValue;
                page8.cellStoolCulter2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR189" && p.Year == year2)?.ResultValue;
                page8.cellStoolCulter3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR189" && p.Year == year3)?.ResultValue;
            }
            else
            {
                page8.StoolCultureYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page8.StoolCultureYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page8.StoolCultureYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }

        private void GenerateToxicology(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null)
            {
                page8.RowToluene.Visible = false;
                page8.RowXylene.Visible = false;
                page8.RowCarboxy.Visible = false;
                page8.RowMEK.Visible = false;
                page8.RowBenzene.Visible = false;
                page8.RowMethanol.Visible = false;
                page8.RowMethyrene.Visible = false;
                page8.RowAcetone.Visible = false;
                page8.RowHexane.Visible = false;
                page8.RowIsopropanol.Visible = false;
                page8.RowStyreneUrine.Visible = false;
                page8.RowAluminiumBlood.Visible = false;
                page8.RowArsenic.Visible = false;
                page8.CyclohexanoneRow.Visible = false;
                page8.RowPhenol.Visible = false;
                page8.RowStyreneBlood.Visible = false;
                page8.RowMethyreneUrine.Visible = false;

                page8.RowMibkUrine.Visible = false;
                page8.RowCadmiumUrine.Visible = false;
                page8.RowEthylbenzeneUrine.Visible = false;
                page8.RowMercuryUrine.Visible = false;

                page8.RowMethyreneUrine.Visible = false;
                page8.RowBenzenettUrine.Visible = false;

                if (labTestSet != null && labTestSet.Count() > 0)
                {
                    List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                    Years.Sort();
                    int countYear = Years.Count();
                    int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                    int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                    int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;
                    page8.cellToxicoYear1.Text = "ปี" + " " + year1.ToString();
                    page8.cellToxicoYear2.Text = "ปี" + " " + year2.ToString();
                    page8.cellToxicoYear3.Text = "ปี" + " " + year3.ToString();


                    #region Aluminium (Show all)

                    page8.cellAluminiumRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122")?.ReferenceRange;
                    page8.cellAluminium1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year1)?.ResultValue;
                    page8.cellAluminium2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year2)?.ResultValue;
                    page8.cellAluminium3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year3)?.ResultValue;

                    #endregion

                    #region Toluene

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124") != null)
                    {
                        page8.RowToluene.Visible = true;
                        page8.cellTolueneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124")?.ReferenceRange;
                        page8.cellToluene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year1)?.ResultValue;
                        page8.cellToluene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year2)?.ResultValue;
                        page8.cellToluene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region Xylene

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125") != null)
                    {
                        page8.RowXylene.Visible = true;
                        page8.cellXyleneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125")?.ReferenceRange;
                        page8.cellXylene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year1)?.ResultValue;
                        page8.cellXylene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year2)?.ResultValue;
                        page8.cellXylene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year3)?.ResultValue;
                    }

                    #endregion

                    #region Lead in blood (Show all)
                    page8.cellLeadRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75")?.ReferenceRange;
                    page8.cellLead1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year1)?.ResultValue;
                    page8.cellLead2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year2)?.ResultValue;
                    page8.cellLead3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year3)?.ResultValue;

                    #endregion

                    #region Carboxy

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120") != null)
                    {
                        page8.RowCarboxy.Visible = true;
                        page8.cellCarboxyRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120")?.ReferenceRange;
                        page8.cellCarboxy1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year1)?.ResultValue;
                        page8.cellCarboxy2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year2)?.ResultValue;
                        page8.cellCarboxy3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region Mek

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127") != null)
                    {
                        page8.RowMEK.Visible = true;
                        page8.cellMekRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127")?.ReferenceRange;
                        page8.cellMek1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year1)?.ResultValue;
                        page8.cellMek2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year2)?.ResultValue;
                        page8.cellMek3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region Benzene

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115") != null)
                    {
                        page8.RowBenzene.Visible = true;
                        page8.cellBenzeneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115")?.ReferenceRange;
                        page8.cellBenzene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115" && p.Year == year1)?.ResultValue;
                        page8.cellBenzene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115" && p.Year == year2)?.ResultValue;
                        page8.cellBenzene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115" && p.Year == year3)?.ResultValue;
                    }

                    #endregion

                    #region Methanol

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116") != null)
                    {
                        page8.RowMethanol.Visible = true;
                        page8.cellMethanolRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116")?.ReferenceRange;
                        page8.cellMethanol1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116" && p.Year == year1)?.ResultValue;
                        page8.cellMethanol2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116" && p.Year == year2)?.ResultValue;
                        page8.cellMethanol3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116" && p.Year == year3)?.ResultValue;
                    }

                    #endregion

                    #region Methyrene

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119") != null)
                    {
                        page8.RowMethyrene.Visible = true;
                        page8.cellMethyreneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119")?.ReferenceRange;
                        page8.cellMethyrene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119" && p.Year == year1)?.ResultValue;
                        page8.cellMethyrene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119" && p.Year == year2)?.ResultValue;
                        page8.cellMethyrene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region Styrene in Urine

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR195") != null)
                    {
                        page8.RowStyreneUrine.Visible = true;
                        page8.cellStyreneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR195")?.ReferenceRange;
                        page8.cellStyrene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR195" && p.Year == year1)?.ResultValue;
                        page8.cellStyrene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR195" && p.Year == year2)?.ResultValue;
                        page8.cellStyrene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR195" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region Hexane

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118") != null)
                    {
                        page8.RowHexane.Visible = true;
                        page8.cellHexaneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118")?.ReferenceRange;
                        page8.cellHexane1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118" && p.Year == year1)?.ResultValue;
                        page8.cellHexane2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118" && p.Year == year2)?.ResultValue;
                        page8.cellHexane3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region Isopropanol

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130") != null)
                    {
                        page8.RowIsopropanol.Visible = true;
                        page8.cellIsopropanolRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130")?.ReferenceRange;
                        page8.cellIsopropanol1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year1)?.ResultValue;
                        page8.cellIsopropanol2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year2)?.ResultValue;
                        page8.cellIsopropanol3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region Chromium (Show all)
                    page8.cellChromiumRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132")?.ReferenceRange;
                    page8.cellChromium1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132" && p.Year == year1)?.ResultValue;
                    page8.cellChromium2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132" && p.Year == year2)?.ResultValue;
                    page8.cellChromium3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132" && p.Year == year3)?.ResultValue;

                    #endregion

                    #region Nickel (Show all)
                    page8.cellNickelRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131")?.ReferenceRange;
                    page8.cellNickel1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131" && p.Year == year1)?.ResultValue;
                    page8.cellNickel2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131" && p.Year == year2)?.ResultValue;
                    page8.cellNickel3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131" && p.Year == year3)?.ResultValue;

                    #endregion

                    #region Nickel In Urine

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188") != null)
                    {
                        page8.RowNickelUrine.Visible = true;
                        page8.cellNickelUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188")?.ReferenceRange;
                        page8.cellNickelUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188" && p.Year == year1)?.ResultValue;
                        page8.cellNickelUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188" && p.Year == year2)?.ResultValue;
                        page8.cellNickelUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region Acetone 

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117") != null)
                    {
                        page8.RowAcetone.Visible = true;
                        page8.cellAcetoneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117")?.ReferenceRange;
                        page8.cellAcetone1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117" && p.Year == year1)?.ResultValue;
                        page8.cellAcetone2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117" && p.Year == year2)?.ResultValue;
                        page8.cellAcetone3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117" && p.Year == year3)?.ResultValue;
                    }
                    #endregion               

                    #region Aluminium in bloob
                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR194") != null)
                    {
                        page8.RowAluminiumBlood.Visible = true;
                        page8.AluminiumBloodRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR194")?.ReferenceRange;
                        page8.AluminiumBlood1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR194" && p.Year == year1)?.ResultValue;
                        page8.AluminiumBlood2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR194" && p.Year == year2)?.ResultValue;
                        page8.AluminiumBlood3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR194" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region Arsenic

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR199") != null)
                    {
                        page8.RowArsenic.Visible = true;
                        page8.ArsenicRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR199")?.ReferenceRange;
                        page8.Arsenic1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR199" && p.Year == year1)?.ResultValue;
                        page8.Arsenic2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR199" && p.Year == year2)?.ResultValue;
                        page8.Arsenic3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR199" && p.Year == year3)?.ResultValue;
                    }

                    #endregion

                    #region Cyclohexanone

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR202") != null)
                    {
                        page8.CyclohexanoneRow.Visible = true;
                        page8.CyclohexanoneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR202")?.ReferenceRange;
                        page8.Cyclohexanone1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR202" && p.Year == year1)?.ResultValue;
                        page8.Cyclohexanone2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR202" && p.Year == year2)?.ResultValue;
                        page8.Cyclohexanone3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR202" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region Phenol

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR204") != null)
                    {
                        page8.RowPhenol.Visible = true;
                        page8.PhenolRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR204")?.ReferenceRange;
                        page8.Phenol1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR204" && p.Year == year1)?.ResultValue;
                        page8.Phenol2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR204" && p.Year == year2)?.ResultValue;
                        page8.Phenol3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR204" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region MIBK Urine 

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1232") != null)
                    {
                        page8.RowMibkUrine.Visible = true;
                        page8.cellMibkUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1232")?.ReferenceRange;
                        page8.cellMibkUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1232" && p.Year == year1)?.ResultValue;
                        page8.cellMibkUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1232" && p.Year == year2)?.ResultValue;
                        page8.cellMibkUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1232" && p.Year == year3)?.ResultValue;

                    }
                    #endregion

                    #region Cadmium Urine 

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1233") != null)
                    {
                        page8.RowCadmiumUrine.Visible = true;
                        page8.cellCadmiumUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1233")?.ReferenceRange;
                        page8.cellCadmiumUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1233" && p.Year == year1)?.ResultValue;
                        page8.cellCadmiumUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1233" && p.Year == year2)?.ResultValue;
                        page8.cellCadmiumUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1233" && p.Year == year3)?.ResultValue;

                    }
                    #endregion

                    #region Ethyl benzene in Urine

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1234") != null)
                    {
                        page8.RowEthylbenzeneUrine.Visible = true;
                        page8.cellEthylbenzeneUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1234")?.ReferenceRange;
                        page8.cellEthylbenzeneUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1234" && p.Year == year1)?.ResultValue;
                        page8.cellEthylbenzeneUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1234" && p.Year == year2)?.ResultValue;
                        page8.cellEthylbenzeneUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1234" && p.Year == year3)?.ResultValue;
                        
                    }
                    #endregion

                    #region Mercury Urine

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1235") != null)
                    {
                        page8.RowMercuryUrine.Visible = true;
                        page8.cellMercuryUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1235")?.ReferenceRange;
                        page8.cellMercuryUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1235" && p.Year == year1)?.ResultValue;
                        page8.cellMercuryUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1235" && p.Year == year2)?.ResultValue;
                        page8.cellMercuryUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1235" && p.Year == year3)?.ResultValue;                        
                    }
                    #endregion

                    #region Methyrene chloride in Urine

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1236") != null)
                    {
                        page8.RowMethyreneUrine.Visible = true;
                        page8.cellMethyreneUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1236")?.ReferenceRange;
                        page8.cellMethyreneUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1236" && p.Year == year1)?.ResultValue;
                        page8.cellMethyreneUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1236" && p.Year == year2)?.ResultValue;
                        page8.cellMethyreneUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1236" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region Benzene (t,t-Muconic acid) in Urine

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1215") != null)
                    {
                        page8.RowBenzenettUrine.Visible = true;
                        page8.cellBenzenettUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1215")?.ReferenceRange;
                        page8.cellMethyreneUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1215" && p.Year == year1)?.ResultValue;
                        page8.cellBenzenettUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1215" && p.Year == year2)?.ResultValue;
                        page8.cellBenzenettUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1215" && p.Year == year3)?.ResultValue;
                    }
                    #endregion
                }
                else
                {
                    page8.cellToxicoYear1.Text = "ปี" + " " + DateTime.Now.Year;
                    page8.cellToxicoYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                    page8.cellToxicoYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
                }
            }
        }

        private void GenerateTumorMarker(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                PatientResultComponentModel CheckGender = labTestSet.FirstOrDefault();
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;
                page7.cellOtherYear1.Text = "ปี" + " " + year1.ToString();
                page7.cellOtherYear2.Text = "ปี" + " " + year2.ToString();
                page7.cellOtherYear3.Text = "ปี" + " " + year3.ToString();

                page7.cellAfpSIRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38")?.ReferenceRange;
                page7.cellAfpSI1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38" && p.Year == year1)?.ResultValue;
                page7.cellAfpSI2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38" && p.Year == year2)?.ResultValue;
                page7.cellAfpSI3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38" && p.Year == year3)?.ResultValue;

                //AFP โชว์ตัวเลขก่อนตัวอักษร

                if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39") != null)
                {
                    page7.cellAfpRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39")?.ReferenceRange;
                    page7.cellAfp1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year1)?.ResultValue;
                    page7.cellAfp2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year2)?.ResultValue;
                    page7.cellAfp3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year3)?.ResultValue;
                }
                else
                {
                    page7.cellAfpRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186")?.ReferenceRange;
                    page7.cellAfp1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186" && p.Year == year1)?.ResultValue;
                    page7.cellAfp2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186" && p.Year == year2)?.ResultValue;
                    page7.cellAfp3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186" && p.Year == year3)?.ResultValue;
                }


                page7.cellCaRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114")?.ReferenceRange;
                page7.cellCa1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year1)?.ResultValue;
                page7.cellCa2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year2)?.ResultValue;
                page7.cellCa3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year3)?.ResultValue;

                if (CheckGender.SEXXXUID == 1)
                {
                    //PSA โชว์ตัวเลขก่อนตัวอักษร
                    page7.RowCa125.Visible = false;
                    page7.RowCA153.Visible = false;
                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4") != null)
                    {
                        page7.cellPsaRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4")?.ReferenceRange;
                        page7.cellPsa1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4" && p.Year == year1)?.ResultValue;
                        page7.cellPsa2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4" && p.Year == year2)?.ResultValue;
                        page7.cellPsa3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4" && p.Year == year3)?.ResultValue;
                    }
                    else
                    {
                        page7.cellPsaRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187")?.ReferenceRange;
                        page7.cellPsa1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187" && p.Year == year1)?.ResultValue;
                        page7.cellPsa2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187" && p.Year == year2)?.ResultValue;
                        page7.cellPsa3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187" && p.Year == year3)?.ResultValue;
                    }

                }

                if (CheckGender.SEXXXUID == 2)
                {
                    page7.RowPSA.Visible = false;
                    page7.cellCa125Range.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41")?.ReferenceRange;
                    page7.cellCa125_1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41" && p.Year == year1)?.ResultValue;
                    page7.cellCa125_2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41" && p.Year == year2)?.ResultValue;
                    page7.cellCa125_3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41" && p.Year == year3)?.ResultValue;

                    page7.ca153Range.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR203")?.ReferenceRange;
                    page7.ca153_1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR203" && p.Year == year1)?.ResultValue;
                    page7.ca153_2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR203" && p.Year == year2)?.ResultValue;
                    page7.ca153_3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR203" && p.Year == year3)?.ResultValue;
                }


                //CEA โชว์ตัวเลขก่อนตัวอักษร
                if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR40") != null)
                {
                    page7.cellCEARange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR40")?.ReferenceRange;
                    page7.cellCEA1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR40" && p.Year == year1)?.ResultValue;
                    page7.cellCEA2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR40" && p.Year == year2)?.ResultValue;
                    page7.cellCEA3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR40" && p.Year == year3)?.ResultValue;
                }
                else
                {
                    page7.cellCEARange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR185")?.ReferenceRange;
                    page7.cellCEA1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR185" && p.Year == year1)?.ResultValue;
                    page7.cellCEA2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR185" && p.Year == year2)?.ResultValue;
                    page7.cellCEA3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR185" && p.Year == year3)?.ResultValue;
                }

            }
            else
            {
                page7.cellOtherYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page7.cellOtherYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page7.cellOtherYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }


        private void GenerateOther(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                PatientResultComponentModel CheckGender = labTestSet.FirstOrDefault();
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;
                page7.cellOther2Year1.Text = "ปี" + " " + year1.ToString();
                page7.cellOther2Year2.Text = "ปี" + " " + year2.ToString();
                page7.cellOther2Year3.Text = "ปี" + " " + year3.ToString();


                page7.cellAboGroupRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32")?.ReferenceRange;
                page7.cellAboGroup1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32" && p.Year == year1)?.ResultValue;
                page7.cellAboGroup2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32" && p.Year == year2)?.ResultValue;
                page7.cellAboGroup3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32" && p.Year == year3)?.ResultValue;

                page7.cellBloodGroupRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR43")?.ReferenceRange;
                page7.cellBloodGroup1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR43" && p.Year == year1)?.ResultValue;
                page7.cellBloodGroup2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR43" && p.Year == year2)?.ResultValue;
                page7.cellBloodGroup3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR43" && p.Year == year3)?.ResultValue;


                page7.cellCalciumRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR79")?.ReferenceRange;
                page7.cellCalcium1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR79" && p.Year == year1)?.ResultValue;
                page7.cellCalcium2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR79" && p.Year == year2)?.ResultValue;
                page7.cellCalcium3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR79" && p.Year == year3)?.ResultValue;

                page7.cellTshRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR24")?.ReferenceRange;
                page7.cellTsh1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR24" && p.Year == year1)?.ResultValue;
                page7.cellTsh2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR24" && p.Year == year2)?.ResultValue;
                page7.cellTsh3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR24" && p.Year == year3)?.ResultValue;


                page7.T3Range.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR97")?.ReferenceRange;
                page7.T3_1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR97" && p.Year == year1)?.ResultValue;
                page7.T3_2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR97" && p.Year == year2)?.ResultValue;
                page7.T3_3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR97" && p.Year == year3)?.ResultValue;

                page7.T4Range.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR76")?.ReferenceRange;
                page7.T4_1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR76" && p.Year == year1)?.ResultValue;
                page7.T4_2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR76" && p.Year == year2)?.ResultValue;
                page7.T4_3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR76" && p.Year == year3)?.ResultValue;

                page7.FreeT3Range.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR22")?.ReferenceRange;
                page7.FreeT3_1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR22" && p.Year == year1)?.ResultValue;
                page7.FreeT3_2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR22" && p.Year == year2)?.ResultValue;
                page7.FreeT3_3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR22" && p.Year == year3)?.ResultValue;

                page7.FreeT4Range.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR23")?.ReferenceRange;
                page7.FreeT4_1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR23" && p.Year == year1)?.ResultValue;
                page7.FreeT4_2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR23" && p.Year == year2)?.ResultValue;
                page7.FreeT4_3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR23" && p.Year == year3)?.ResultValue;
            }
            else
            {
                page7.cellOther2Year1.Text = "ปี" + " " + DateTime.Now.Year;
                page7.cellOther2Year2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page7.cellOther2Year3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }
        #endregion


        private void GenerateTimus(IEnumerable<PatientResultComponentModel> TimusResult)
        {
            if (TimusResult != null && TimusResult.Count() > 0)
            {
                page4.lbFarVision.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS19")?.ResultValue;
                page4.lbNearVision.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS20")?.ResultValue;
                page4.lb3DVision.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS21")?.ResultValue;
                page4.lbBalanceEye.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS23")?.ResultValue;
                page4.lbVisionColor.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS22")?.ResultValue;
                page4.lbFieldVision.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS24")?.ResultValue;
            }
        }
        private void GenerateAudio(IEnumerable<PatientResultComponentModel> AudioResult)
        {
            if (AudioResult != null && AudioResult.Count() > 0)
            {
                page5.lbAudioRight.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO8")?.ResultValue;
                page5.lbAudioLeft.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO16")?.ResultValue;
            }
        }
        private void GenerateSpiro(IEnumerable<PatientResultComponentModel> SpiroResult)
        {
            if (SpiroResult != null && SpiroResult.Count() > 0)
            {
                page4.lbFVCMeasure.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO1")?.ResultValue;
                page4.lbFVCPredic.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO2")?.ResultValue;
                page4.lbFVCPer.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO3")?.ResultValue;
                page4.lbFEV1Measure.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO4")?.ResultValue;
                page4.lbFEV1Predic.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO5")?.ResultValue;
                page4.lbFEV1Per.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO6")?.ResultValue;
                page4.lbFEVMeasure.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO7")?.ResultValue;
                page4.lbFEVPredic.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO8")?.ResultValue;
                page4.lbFEVPer.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO9")?.ResultValue;
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
            }
        }

        private void GenerateVisualAcuityTest(IEnumerable<PatientResultComponentModel> VisualAcuityTestResult)
        {
            if (VisualAcuityTestResult != null && VisualAcuityTestResult.Count() > 0)
            {
                page5.lbMyopiaRight.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY1")?.ResultValue;
                page5.lbAstigmaticRight.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY2")?.ResultValue;
                page5.lbViewRight.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY3")?.ResultValue;
                page5.lbHyperopiaRight.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY4")?.ResultValue;
                page5.lbVARight.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY5")?.ResultValue;
                page5.lbMyopiaLeft.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY6")?.ResultValue;
                page5.lbAstigmaticLeft.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY7")?.ResultValue;
                page5.lbViewLeft.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY8")?.ResultValue;
                page5.lbHyperopiaLeft.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY9")?.ResultValue;
                page5.lbVALeft.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY10")?.ResultValue;
                page5.lbBlindColor.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY11")?.ResultValue;
                page5.lbViewResult.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY12")?.ResultValue;
                page5.lbViewRecommend.Text = VisualAcuityTestResult.FirstOrDefault(p => p.ResultItemCode == "VISAY13")?.ResultValue;
            }
        }

        private void GenerateBackStrength(IEnumerable<PatientResultComponentModel> BackStrength)
        {
            if (BackStrength != null && BackStrength.Count() > 0)
            {
                page3.lbBackValue.Text = BackStrength.FirstOrDefault(p => p.ResultItemCode == "MUCS1")?.ResultValue;
                page3.lbBackStrenght.Text = BackStrength.FirstOrDefault(p => p.ResultItemCode == "MUCS2")?.ResultValue;
                page3.lbValueLegStrength.Text = BackStrength.FirstOrDefault(p => p.ResultItemCode == "MUCS5")?.ResultValue;
                page3.lbLegStrength.Text = BackStrength.FirstOrDefault(p => p.ResultItemCode == "MUCS6")?.ResultValue;
                page3.lbValueGripStrength.Text = BackStrength.FirstOrDefault(p => p.ResultItemCode == "MUCS3")?.ResultValue;
                page3.lbGripStrength.Text = BackStrength.FirstOrDefault(p => p.ResultItemCode == "MUCS4")?.ResultValue;
            }

        }
        private void GenerateOccmedGroup(IEnumerable<CheckupGroupResultModel> occmedGroupResult)
        {
            if (occmedGroupResult != null && occmedGroupResult.Count() > 0)
            {
                page4.lbLungResult.Text = occmedGroupResult.FirstOrDefault(p => p.GroupCode == "GPRST33")?.ResultStatus.ToString();
                page4.lbLungRecommend.Text = occmedGroupResult.FirstOrDefault(p => p.GroupCode == "GPRST33")?.Conclusion.ToString();

                string eyeOccmedConclustion = occmedGroupResult.FirstOrDefault(p => p.GroupCode == "GPRST26")?.Conclusion.ToString();
                if (!string.IsNullOrEmpty(eyeOccmedConclustion))
                {
                    string[] results = eyeOccmedConclustion.Split(',');

                    string description = "";
                    string recommand = "";
                    foreach (var item in results)
                    {
                        if (item.Contains("ควร"))
                        {
                            int index = item.IndexOf("ควร");
                            description += string.IsNullOrEmpty(description) ? item.Substring(0, index).Trim() : " " + item.Substring(0, index).Trim();
                            recommand = item.Substring(index).Trim();

                        }

                    }

                    page4.lbVisionOccmedResult.Text = description;
                    page4.lbVisionOccmedRecommend.Text = recommand;
                }


                page5.lbAudioResult.Text = occmedGroupResult.FirstOrDefault(p => p.GroupCode == "GPRST25")?.ResultStatus.ToString();
                page5.lbAudioRecommend.Text = occmedGroupResult.FirstOrDefault(p => p.GroupCode == "GPRST25")?.Conclusion.ToString();

                page3.lbMuscleResult.Text = occmedGroupResult.FirstOrDefault(p => p.GroupCode == "GPRST32")?.Conclusion;
            }
        }
        private void GenerateCommentMuscle(IEnumerable<PatientResultComponentModel> CommentMuscle)
        {
            //if(CommentMuscle.Count() > 2 ) 
            //{
            //    page3.lbNoteMuscle.Text = CommentMuscle.FirstOrDefault(p => p.ResultItemCode == "MUSCLEBA")?.Comments.ToString() + ","
            //                             + CommentMuscle.FirstOrDefault(p => p.ResultItemCode == "MUSCLELEG")?.Comments.ToString() + ","
            //                             + CommentMuscle.FirstOrDefault(p => p.ResultItemCode == "MUSCLEGR")?.Comments.ToString();
            //}
            //else
            //{
            //    page3.lbNoteMuscle.Text = CommentMuscle.FirstOrDefault(p => p.ResultItemCode == "MUSCLEBA")?.Comments.ToString();
            //    page3.lbNoteMuscle.Text = CommentMuscle.FirstOrDefault(p => p.ResultItemCode == "MUSCLELEG")?.Comments.ToString();
            //    page3.lbNoteMuscle.Text = CommentMuscle.FirstOrDefault(p => p.ResultItemCode == "MUSCLEGR")?.Comments.ToString();
            //}
        }


        private void CheckupPage1_AfterPrint(object sender, EventArgs e)
        {
            page2.CreateDocument();
            page3.CreateDocument();
            page4.CreateDocument();
            page5.CreateDocument();
            page6.CreateDocument();
            page7.CreateDocument();
            page8.CreateDocument();
            this.Pages.AddRange(page2.Pages);
            this.Pages.AddRange(page3.Pages);
            this.Pages.AddRange(page4.Pages);
            this.Pages.AddRange(page5.Pages);
            this.Pages.AddRange(page6.Pages);
            this.Pages.AddRange(page7.Pages);
            this.Pages.AddRange(page8.Pages);
        }

        public string TranslateXray(string resultValue, string resultStatus, string requestItemName)
        {
            if (dtResultMapping == null)
            {
                dtResultMapping = DataService.Radiology.GetXrayTranslateMapping();
            }

            List<string> listNoMapResult = new List<string>();
            string thairesult = TranslateResult.TranslateResultXray(resultValue, resultStatus, requestItemName, " ", dtResultMapping, ref listNoMapResult);

            return thairesult;
        }

    }

}
