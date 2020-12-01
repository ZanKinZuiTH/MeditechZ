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

namespace MediTech.Reports.Operating.Patient.CheckupBook
{
    public partial class CheckupPage1 : DevExpress.XtraReports.UI.XtraReport
    {
        private MediTechDataService _DataService;

        public MediTechDataService DataService
        {
            get { return _DataService ?? (_DataService = new MediTechDataService()); }
        }

        List<XrayTranslateMappingModel> dtResultMapping;

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
        }

        private void CheckupPage1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            long patientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long patientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            int payorDetailUID = int.Parse(this.Parameters["PayorDetailUID"].Value.ToString());
            List<CheckupBookModel> data = DataService.Reports.PrintCheckupBook(patientUID, patientVisitUID);

            if (data != null && data.Count > 0)
            {
                string title = data.FirstOrDefault().Title;

                #region Show HN/Name
                page2.lbHN2.Text = data.FirstOrDefault().PatientID;
                page2.lbName2.Text = data.FirstOrDefault().PatientName;
                page2.lbHN15.Text = data.FirstOrDefault().PatientID;
                page2.lbName15.Text = data.FirstOrDefault().PatientName;

                page3.lbHN3.Text = data.FirstOrDefault().PatientID;
                page3.lbName3.Text = data.FirstOrDefault().PatientName;
                page3.lbHN14.Text = data.FirstOrDefault().PatientID;
                page3.lbName14.Text = data.FirstOrDefault().PatientName;

                page4.lbHN4.Text = data.FirstOrDefault().PatientID;
                page4.lbName4.Text = data.FirstOrDefault().PatientName;
                page4.lbHN13.Text = data.FirstOrDefault().PatientID;
                page4.lbName13.Text = data.FirstOrDefault().PatientName;

                page5.lbHN5.Text = data.FirstOrDefault().PatientID;
                page5.lbName5.Text = data.FirstOrDefault().PatientName;
                page5.lbHN12.Text = data.FirstOrDefault().PatientID;
                page5.lbName12.Text = data.FirstOrDefault().PatientName;

                page6.lbHN6.Text = data.FirstOrDefault().PatientID;
                page6.lbName6.Text = data.FirstOrDefault().PatientName;
                page6.lbHN11.Text = data.FirstOrDefault().PatientID;
                page6.lbName11.Text = data.FirstOrDefault().PatientName;

                page7.lbHN7.Text = data.FirstOrDefault().PatientID;
                page7.lbName7.Text = data.FirstOrDefault().PatientName;
                page7.lbHN10.Text = data.FirstOrDefault().PatientID;
                page7.lbName10.Text = data.FirstOrDefault().PatientName;

                page8.lbHN8.Text = data.FirstOrDefault().PatientID;
                page8.lbName8.Text = data.FirstOrDefault().PatientName;
                page8.lbHN9.Text = data.FirstOrDefault().PatientID;
                page8.lbName9.Text = data.FirstOrDefault().PatientName;

                lbHN16.Text = data.FirstOrDefault().PatientID;
                lbName16.Text = data.FirstOrDefault().PatientName;
                #endregion

                #region Patient information
                lbDateCheckup.Text = data.FirstOrDefault().StartDttm != null ? data.FirstOrDefault().StartDttm.Value.ToString("dd/MM/yyyy") : "";
                lbPatientName.Text = data.FirstOrDefault().PatientName;
                lbHN.Text = data.FirstOrDefault().PatientID;
                lbEmployee.Text = data.FirstOrDefault().EmployeeID;
                lbDepartment.Text = data.FirstOrDefault().Department;
                lbPosition.Text = data.FirstOrDefault().Position;
                lbCompany.Text = data.FirstOrDefault().PayorName;
                lbDateOfBirth.Text = data.FirstOrDefault().BirthDttm != null ? data.FirstOrDefault().BirthDttm.Value.ToString("dd/MM/yyyy") : "";
                lbAge.Text = data.FirstOrDefault().Age != null ? data.FirstOrDefault().Age + " ปี" : "";
                lbGender.Text = data.FirstOrDefault().Gender;

                lbHeight.Text = data.FirstOrDefault().Height != null ? data.FirstOrDefault().Height.ToString() + " cm." : "";
                lbWeight.Text = data.FirstOrDefault().Weight != null ? data.FirstOrDefault().Weight.ToString() + " kg." : "";
                lbBMI.Text = data.FirstOrDefault().BMI != null ? data.FirstOrDefault().BMI.ToString() + " kg/m2" : "";
                lbBP.Text = (data.FirstOrDefault().BPSys != null ? data.FirstOrDefault().BPSys.ToString() : "") + (data.FirstOrDefault().BPDio != null ? "/" + data.FirstOrDefault().BPDio.ToString() : "");
                lbPulse.Text = data.FirstOrDefault().Pulse != null ? data.FirstOrDefault().Pulse.ToString() + " ครั้ง/นาที" : "";
                lbWaist.Text = data.FirstOrDefault().WaistCircumference != null ? data.FirstOrDefault().WaistCircumference.ToString() + " cm." : "";

                if (data.FirstOrDefault().BMI != null)
                {
                    string bmiResult = "";
                    if (data.FirstOrDefault().BMI < 18.5)
                    {
                        bmiResult = "น้ำหนักน้อย";
                    }
                    else if (data.FirstOrDefault().BMI >= 18.5 && data.FirstOrDefault().BMI <= 22.99)
                    {
                        bmiResult = "น้ำหนักปกติ";
                    }
                    else if (data.FirstOrDefault().BMI >= 23 && data.FirstOrDefault().BMI <= 24.99)
                    {
                        bmiResult = "น้ำหนักเกินเกณฑ์";
                    }
                    else if (data.FirstOrDefault().BMI >= 25 && data.FirstOrDefault().BMI <= 29.99)
                    {
                        bmiResult = "โรคอ้วนระดับที่ 1";
                    }
                    else if (data.FirstOrDefault().BMI >= 30)
                    {
                        bmiResult = "โรคอ้วนระดับที่ 2";
                    }
                    lbObesity.Text = bmiResult;

                    if (bmiResult != "น้ำหนักปกติ")
                    {
                        lbObesity.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }
                }

                #endregion

                #region Vision Occmed
                page3.lbFarVision.Text = data.FirstOrDefault().FarPoint != null ? data.FirstOrDefault().FarPoint.ToString() : "";
                if ((page3.lbFarVision.Text != "" && page3.lbFarVision.Text != "ปกติ") && (page3.lbFarVision.Text != "" && page3.lbFarVision.Text != "Normal"))
                {
                    page3.lbFarVision.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
                page3.lbNearVision.Text = data.FirstOrDefault().NearPoint != null ? data.FirstOrDefault().NearPoint.ToString() : "";
                if ((page3.lbNearVision.Text != "" && page3.lbNearVision.Text != "ปกติ") && (page3.lbNearVision.Text != "" && page3.lbNearVision.Text != "Normal"))
                {
                    page3.lbNearVision.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
                page3.lb3DVision.Text = data.FirstOrDefault().Depth != null ? data.FirstOrDefault().Depth.ToString() : "";
                if ((page3.lb3DVision.Text != "" && page3.lb3DVision.Text != "ปกติ") && (page3.lb3DVision.Text != "" && page3.lb3DVision.Text != "Normal"))
                {
                    page3.lb3DVision.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
                page3.lbBalanceEye.Text = data.FirstOrDefault().Muscle != null ? data.FirstOrDefault().Muscle.ToString() : "";
                if ((page3.lbBalanceEye.Text != "" && page3.lbBalanceEye.Text != "ปกติ") && (page3.lbBalanceEye.Text != "" && page3.lbBalanceEye.Text != "Normal"))
                {
                    page3.lbBalanceEye.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
                page3.lbVisionColor.Text = data.FirstOrDefault().Color != null ? data.FirstOrDefault().Color.ToString() : "";
                if ((page3.lbVisionColor.Text != "" && page3.lbVisionColor.Text != "ปกติ") && (page3.lbVisionColor.Text != "" && page3.lbVisionColor.Text != "Normal"))
                {
                    page3.lbVisionColor.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
                page3.lbFieldVision.Text = data.FirstOrDefault().Visualfield != null ? data.FirstOrDefault().Visualfield.ToString() : "";
                if ((page3.lbFieldVision.Text != "" && page3.lbFieldVision.Text != "ปกติ") && (page3.lbFieldVision.Text != "" && page3.lbFieldVision.Text != "Normal"))
                { 
                    page3.lbFieldVision.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
                page3.lbVisionOccmedResult.Text = data.FirstOrDefault().TitmusConclusion != null ? data.FirstOrDefault().TitmusConclusion.ToString() : "";
                if (page3.lbVisionOccmedResult.Text != null && page3.lbVisionOccmedResult.Text.Length > 120)
                {
                    page3.lbVisionOccmedResult.Font = new Font("Angsana New", 9);
                }
                page3.lbVisionOccmedRecommend.Text = data.FirstOrDefault().TitmusRecommend != null ? data.FirstOrDefault().TitmusRecommend.ToString() : "";
                if (page3.lbVisionOccmedRecommend.Text != null && page3.lbVisionOccmedRecommend.Text.Length > 120)
                {
                    page3.lbVisionOccmedRecommend.Font = new Font("Angsana New", 9);
                }

                #endregion

                #region Result Wellness

                var wellnessResult = data.FirstOrDefault().WellnessResult;
                if (wellnessResult != null)
                {
                    string[] locResult = Regex.Split(wellnessResult, "[\r\n]+");
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sb2 = new StringBuilder();
                    int line = 0;
                    foreach (var item in locResult)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            if (line < 14)
                            {
                                sb.AppendLine(item);
                            }
                            else
                            {
                                sb2.AppendLine(item);
                            }
                            line++;
                        }
                    }

                    page2.lbResultWellness.Text = sb.ToString();
                    lbResultWellness2.Text = sb2.ToString();
                }

                #endregion

                #region Lung Funtion 
                page4.lbFVCMeasure.Text = data.FirstOrDefault().FVC != null ? data.FirstOrDefault().FVC.ToString() : "";
                page4.lbFVCPredic.Text = data.FirstOrDefault().FVCPred != null ? data.FirstOrDefault().FVCPred.ToString() : "";
                page4.lbFVCPer.Text = data.FirstOrDefault().FVCPer != null ? data.FirstOrDefault().FVCPer.ToString() + " %" : "";
                page4.lbFEV1Measure.Text = data.FirstOrDefault().FEV1 != null ? data.FirstOrDefault().FEV1.ToString() : "";
                page4.lbFEV1Predic.Text = data.FirstOrDefault().FEV1Pred != null ? data.FirstOrDefault().FEV1Pred.ToString() : "";
                page4.lbFEV1Per.Text = data.FirstOrDefault().FEV1Per != null ? data.FirstOrDefault().FEV1Per.ToString() + " %" : "";
                page4.lbFFVMeasure.Text = data.FirstOrDefault().FEV1FVC != null ? data.FirstOrDefault().FEV1FVC.ToString() + " %" : "";
                page4.lbFFVPredic.Text = data.FirstOrDefault().FEV1FVCPred != null ? data.FirstOrDefault().FEV1FVCPred.ToString() + " %" : "";
                page4.lbFFVPer.Text = data.FirstOrDefault().FEV1FVCPer != null ? data.FirstOrDefault().FEV1FVCPer.ToString() + " %" : "";
                page4.lbLungResult.Text = data.FirstOrDefault().SpiroResult != null ? data.FirstOrDefault().SpiroResult.ToString() : "";
                page4.lbLungRecommend.Text = data.FirstOrDefault().SpiroRecommend != null ? data.FirstOrDefault().SpiroRecommend.ToString() : "";

                #endregion

                #region Audio Test
                page4.lbAudioRight.Text = data.FirstOrDefault().AudioRightResult != null ? data.FirstOrDefault().AudioRightResult.ToString() : "";
                if (page4.lbAudioRight.Text != null && page4.lbAudioRight.Text.Length > 120)
                {
                    page4.lbAudioRight.Font = new Font("Angsana New", 9);
                }
                page4.lbAudioLeft.Text = data.FirstOrDefault().AudioLeftResult != null ? data.FirstOrDefault().AudioLeftResult.ToString() : "";
                if (page4.lbAudioLeft.Text != null && page4.lbAudioLeft.Text.Length > 120)
                {
                    page4.lbAudioLeft.Font = new Font("Angsana New", 9);
                }
                page4.lbAudioResult.Text = data.FirstOrDefault().AudioResult != null ? data.FirstOrDefault().AudioResult.ToString() : "";
                page4.lbAudioRecommend.Text = data.FirstOrDefault().AudioRecommend != null ? data.FirstOrDefault().AudioRecommend.ToString() : "";
                if (page4.lbAudioRecommend.Text != null && page4.lbAudioRecommend.Text.Length > 120)
                {
                    page4.lbAudioRecommend.Font = new Font("Angsana New", 9);
                }

                #endregion

                #region EKG
                page6.lbEKGRecommend.Text = data.FirstOrDefault().EkgConclusion != null ? data.FirstOrDefault().EkgConclusion.ToString() : "";

                #endregion

                #region Vision
                page5.lbAstigmaticRight.Text = data.FirstOrDefault().AstigmaticRight != null ? data.FirstOrDefault().AstigmaticRight.ToString() : "";
                page5.lbAstigmaticLeft.Text = data.FirstOrDefault().AstigmaticLeft != null ? data.FirstOrDefault().AstigmaticLeft.ToString() : "";
                page5.lbMyopiaRight.Text = data.FirstOrDefault().MyopiaRight != null ? data.FirstOrDefault().MyopiaRight.ToString() : "";
                page5.lbMyopiaLeft.Text = data.FirstOrDefault().MyopiaLeft != null ? data.FirstOrDefault().MyopiaLeft.ToString() : "";
                page5.lbViewRight.Text = data.FirstOrDefault().ViewRight != null ? data.FirstOrDefault().ViewRight.ToString() : "";
                page5.lbViewLeft.Text = data.FirstOrDefault().ViewLeft != null ? data.FirstOrDefault().ViewLeft.ToString() : "";
                page5.lbHyperopiaRight.Text = data.FirstOrDefault().HyperopiaRight != null ? data.FirstOrDefault().HyperopiaRight.ToString() : "";
                page5.lbHyperopiaLeft.Text = data.FirstOrDefault().HyperopiaLeft != null ? data.FirstOrDefault().HyperopiaLeft.ToString() : "";
                page5.lbVARight.Text = data.FirstOrDefault().VARight != null ? data.FirstOrDefault().VARight.ToString() : "";
                page5.lbVALeft.Text = data.FirstOrDefault().VALeft != null ? data.FirstOrDefault().VALeft.ToString() : "";
                page5.lbDisease.Text = data.FirstOrDefault().EyeDiseas != null ? data.FirstOrDefault().EyeDiseas.ToString() : "";
                page5.lbBlindColor.Text = data.FirstOrDefault().BlindColor != null ? data.FirstOrDefault().BlindColor.ToString() : "";
                page5.lbViewResult.Text = data.FirstOrDefault().ViewResult != null ? data.FirstOrDefault().ViewResult.ToString() : "";
                page5.lbViewRecommend.Text = data.FirstOrDefault().ViewRecommend != null ? data.FirstOrDefault().ViewRecommend.ToString() : "";

                #endregion

                #region Physical Exam
                page2.lbEye.Text = data.FirstOrDefault().Eyes != null ? data.FirstOrDefault().Eyes.ToString() : "";
                page2.lbEars.Text = data.FirstOrDefault().Ears != null ? data.FirstOrDefault().Ears.ToString() : "";
                page2.lbThroat.Text = data.FirstOrDefault().Throat != null ? data.FirstOrDefault().Throat.ToString() : "";
                page2.lbNose.Text = data.FirstOrDefault().Nose != null ? data.FirstOrDefault().Nose.ToString() : "";
                page2.lbTeeth.Text = data.FirstOrDefault().Teeth != null ? data.FirstOrDefault().Teeth.ToString() : "";
                page2.lbLung.Text = data.FirstOrDefault().Lung != null ? data.FirstOrDefault().Lung.ToString() : "";
                page2.lbHeart.Text = data.FirstOrDefault().Heart != null ? data.FirstOrDefault().Heart.ToString() : "";
                page2.lbSkin.Text = data.FirstOrDefault().Skin != null ? data.FirstOrDefault().Skin.ToString() : "";
                page2.lbThyroid.Text = data.FirstOrDefault().Thyroid != null ? data.FirstOrDefault().Thyroid.ToString() : "";
                page2.lbLymphNode.Text = data.FirstOrDefault().LymphNode != null ? data.FirstOrDefault().LymphNode.ToString() : "";
                page2.lbSmoke.Text = data.FirstOrDefault().Smoke != null ? data.FirstOrDefault().Smoke.ToString() : "";
                page2.lbDrugAllergy.Text = data.FirstOrDefault().DrugAllergy != null ? data.FirstOrDefault().DrugAllergy.ToString() : "";
                page2.lbAlcohol.Text = data.FirstOrDefault().Alcohol != null ? data.FirstOrDefault().Alcohol.ToString() : "";
                page2.lbUnderlying.Text = data.FirstOrDefault().PersonalHistory != null ? data.FirstOrDefault().PersonalHistory.ToString() : "";

                #endregion

                #region Radiology
                if (data.FirstOrDefault(p => !string.IsNullOrEmpty(p.RequestItemName)) != null)
                {

                    if (data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("chest") && p.RequestItemType == "Radiology") != null)
                    {
                        CheckupBookModel chestXray = data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("chest") && p.RequestItemType == "Radiology");
                        if (!string.IsNullOrEmpty(chestXray.RadiologyResultText))
                        {
                            if ((chestXray.Title == "MR.") || (chestXray.Title == "MS.") || (chestXray.Title == "MISS") || (chestXray.Title == "MRS."))
                            {
                                string chestEN = chestXray.RadiologyResultText;
                                string[] ChestResult = chestEN.Split(new string[] { "IMPRESSION", "Impression", "impression" }, StringSplitOptions.None);
                                string ResultChestEn = ChestResult[1].Replace(":", "");
                                ResultChestEn = ResultChestEn.Trim();
                                page6.lbChest.Text = ResultChestEn;
                            }
                            else
                            {
                                string resultChestThai = TranslateXray(chestXray.RadiologyResultText, chestXray.RadiologyResultStatus, chestXray.RequestItemName);
                                if (!string.IsNullOrEmpty(resultChestThai))
                                {
                                    page6.lbChest.Text = resultChestThai;
                                    if (page6.lbChest.Text != null && page6.lbChest.Text.Length > 400)
                                    {
                                        page6.lbChest.Font = new Font("Angsana New", 9);
                                    }
                                }
                                else
                                {
                                    page6.lbChest.Text = "ยังไม่ได้แปลไทย";
                                }
                            }
                        }
                    }
                    else
                    {
                        page6.lbChest.Text = "-";
                    }

                    if (data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("mammo") && p.RequestItemType == "Radiology") != null)
                    {
                        CheckupBookModel mammoGram = data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("mammo") && p.RequestItemType == "Radiology");
                        page6.lbMam.Text = mammoGram.RadiologyResultStatus;
                        if (!string.IsNullOrEmpty(mammoGram.RadiologyResultText))
                        {
                            if ((mammoGram.Title == "MR.") || (mammoGram.Title == "MS.") || (mammoGram.Title == "MISS") || (mammoGram.Title == "MRS."))
                            {
                                string mamEN = mammoGram.RadiologyResultText;
                                string[] MamResult = mamEN.Split(new string[] { "IMPRESSION", "Impression", "impression" }, StringSplitOptions.None);
                                string MamResultEn = MamResult[1].Replace("\n", "").Replace("\r", "").Replace("\r\n", "");
                                MamResultEn = MamResultEn.Trim();
                                page6.lbMam.Text = MamResultEn;
                            }
                            else
                            {
                                string resultChestThai = TranslateXray(mammoGram.RadiologyResultText, mammoGram.RadiologyResultStatus, mammoGram.RequestItemName);
                                if (!string.IsNullOrEmpty(resultChestThai))
                                {
                                    page6.lbMam.Text = resultChestThai;
                                    if (page6.lbMam.Text != null && page6.lbMam.Text.Length > 400)
                                    {
                                        page6.lbMam.Font = new Font("Angsana New", 9);
                                    }
                                }
                                else
                                {
                                    page6.lbMam.Text = "ยังไม่ได้แปลไทย";
                                }
                            }
                        }
                    }
                    else
                    {
                        page6.lbMam.Text = "-";
                    }

                    if (data.FirstOrDefault(p => (p.RequestItemName.ToLower().Contains("ultrasound") || p.RequestItemName.ToLower().Contains("US")) && p.RequestItemType == "Radiology") != null)
                    {
                        CheckupBookModel ultrsound = data.FirstOrDefault(p => (p.RequestItemName.ToLower().Contains("ultrasound") || p.RequestItemName.ToLower().Contains("US")) && p.RequestItemType == "Radiology");
                        page6.lbUlt.Text = ultrsound.RadiologyResultStatus;
                        if (!string.IsNullOrEmpty(ultrsound.RadiologyResultText))
                        {
                            if ((ultrsound.Title == "MR.") || (ultrsound.Title == "MS.") || (ultrsound.Title == "MISS") || (ultrsound.Title == "MRS."))
                            {
                                string UltEN = ultrsound.RadiologyResultText;
                                string[] UltResult = UltEN.Split(new string[] { "IMPRESSION", "Impression", "impression" }, StringSplitOptions.None);
                                string UltResultEn = UltResult[1].Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(":", "");
                                UltResultEn = UltResultEn.Trim();
                                page6.lbUlt.Text = UltResultEn;
                            }
                            else
                            {
                                string resultChestThai = TranslateXray(ultrsound.RadiologyResultText, ultrsound.RadiologyResultStatus, ultrsound.RequestItemName);
                                if (!string.IsNullOrEmpty(resultChestThai))
                                {
                                    page6.lbUlt.Text = resultChestThai;
                                    if (page6.lbUlt.Text != null && page6.lbUlt.Text.Length > 400)
                                    {
                                        page6.lbUlt.Font = new Font("Angsana New", 9);
                                    }
                                }
                                else
                                {
                                    page6.lbUlt.Text = "ยังไม่ได้แปลไทย";
                                }
                            }
                        }
                    }
                    else
                    {
                        page6.lbUlt.Text = "-";
                    }
                }

                #endregion

                if ((title == "MR.") || (title == "MS.") || (title == "MISS") || (title == "MRS."))
                {
                    TitleResultWellness2.Text = "Summary";
                    page2.TitleResultWellness.Text = "Summary";
                    TitleObesity.Text = "BMI Interpretation";
                    lbPulse.Text = data.FirstOrDefault().Pulse != null ? data.FirstOrDefault().Pulse.ToString() + " times/min" : "";

                    if (data.FirstOrDefault().BMI != null)
                    {
                        string bmiResult = "";
                        if (data.FirstOrDefault().BMI < 18.5)
                        {
                            bmiResult = "Less weight";
                        }
                        else if (data.FirstOrDefault().BMI >= 18.5 && data.FirstOrDefault().BMI <= 22.99)
                        {
                            bmiResult = "Normal";
                        }
                        else if (data.FirstOrDefault().BMI >= 23 && data.FirstOrDefault().BMI <= 24.99)
                        {
                            bmiResult = "Overweight";
                        }
                        else if (data.FirstOrDefault().BMI >= 25 && data.FirstOrDefault().BMI <= 29.99)
                        {
                            bmiResult = "Obesity Class 1";
                        }
                        else if (data.FirstOrDefault().BMI >= 30)
                        {
                            bmiResult = "Obesity class 2";
                        }
                        lbObesity.Text = bmiResult;

                        if (bmiResult != "Normal weight")
                        {
                            lbObesity.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }
                    }

                    page3.TitleFarVision.Text = "Far Test";
                    page3.TitleNearVision.Text = "Near Test";
                    page3.Title3DVision.Text = "3D Test";
                    page3.TitleBalanceEye.Text = "Eye Balance";
                    page3.TitleColor.Text = "Color";
                    page3.TitleFieldVision.Text = "Visual Field";
                    page3.TitleVisionOccmedResult.Text = "Summary";
                    page3.TitleVisionOccmedRecommend.Text = "Suggestion";

                    if (page4.lbLungResult.Text == "ปกติ")
                    {
                        page4.lbLungResult.Text = "Normal";
                    }
                    else
                    {
                        page4.lbLungResult.Text = "Abnormal";
                    }

                    page4.TitleAudiogram.Text = "Audiogram";
                    page4.TitleAudioListResult.Text = "Result";
                    page4.TitleAudioRight.Text = "Right ear";
                    page4.TitleAudioLeft.Text = "Left ear";
                    page4.TitleAudioResult.Text = "Summary";
                    page4.TitleAudioRecommend.Text = "Suggestion";

                    if (page4.lbAudioResult.Text == "ปกติ")
                    {
                        page4.lbAudioResult.Text = "Normal";
                    }
                    else if (page4.lbAudioResult.Text == "เฝ้าระวัง")
                    {
                        page4.lbAudioResult.Text = "Mild abnormality";
                    }
                    else
                    {
                        page4.lbAudioResult.Text = "Abnormal";
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
                    page5.TitleDisease.Text = "Eys Disease";
                    page5.TitleBlindColor.Text = "Color Blindness";
                    page5.TitleViewResult.Text = "Result";
                    page5.TitleViewRecommend.Text = "Suggestion";

                }

                List<PatientResultLabModel> labCompare = DataService.Reports.CheckupLabCompare(patientUID, payorDetailUID);
                if (labCompare != null)
                {
                    #region Complete Blood Count

                    IEnumerable<PatientResultLabModel> cbcTestSet = labCompare
                    .Where(p => p.RequestItemName.Contains("CBC"))
                    .OrderBy(p => p.Year);
                    GenerateCompleteBloodCount(cbcTestSet);
                    #endregion

                    #region Urinalysis
                    IEnumerable<PatientResultLabModel> uaTestSet = labCompare
                        .Where(p => p.RequestItemName.Contains("UA"))
                        .OrderBy(p => p.Year);
                    GenerateUrinalysis(uaTestSet);

                    #endregion

                    #region Renal function
                    IEnumerable<PatientResultLabModel> RenalTestSet = labCompare
                        .Where(p => p.RequestItemCode.Contains("LAB212")
                        || p.RequestItemCode.Contains("LAB211"))
                        .OrderBy(p => p.Year);
                    GenerateRenalFunction(RenalTestSet);

                    #endregion

                    #region Fasting Blood Sugar
                    IEnumerable<PatientResultLabModel> FbsTestSet = labCompare
                        .Where(p => p.RequestItemCode.Contains("LAB231"))
                        .OrderBy(p => p.Year);
                    GenerateFastingBloodSugar(FbsTestSet);

                    #endregion

                    #region Uric acid
                    IEnumerable<PatientResultLabModel> UricTestSet = labCompare
                        .Where(p => p.RequestItemCode.Contains("LAB261"))
                        .OrderBy(p => p.Year);
                    GenerateUricAcid(UricTestSet);

                    #endregion

                    #region Lipid Profiles 
                    IEnumerable<PatientResultLabModel> LipidTestSet = labCompare
                        .Where(p => p.RequestItemCode.Contains("LAB241")
                        || p.RequestItemCode.Contains("LAB242")
                        || p.RequestItemCode.Contains("LAB243")
                        || p.RequestItemCode.Contains("LAB244"))
                        .OrderBy(p => p.Year);
                    GenerateLipidProfiles(LipidTestSet);

                    #endregion

                    #region Liver Function
                    IEnumerable<PatientResultLabModel> LiverTestSet = labCompare
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
                    IEnumerable<PatientResultLabModel> ImmunologyTestSet = labCompare
                        .Where(p => p.RequestItemCode.Contains("LAB451")
                        || p.RequestItemCode.Contains("LAB441")
                        || p.RequestItemCode.Contains("LAB512")
                        || p.RequestItemCode.Contains("LAB554"))
                        .OrderBy(p => p.Year);
                    GenerateImmunology(ImmunologyTestSet);
                    #endregion

                    #region Stool Exam
                    IEnumerable<PatientResultLabModel> StoolTestSet = labCompare
                        .Where(p => p.RequestItemName.Contains("Stool Examination"))
                        .OrderBy(p => p.Year);
                    GenerateStool(StoolTestSet);
                    #endregion

                    #region Toxicology
                    IEnumerable<PatientResultLabModel> ToxicoTestSet = labCompare
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
                        || p.RequestItemCode.Contains("LAB558"))
                        .OrderBy(p => p.Year);
                    GenerateToxicology(ToxicoTestSet);
                    #endregion

                    #region Other Lab Teat
                    IEnumerable<PatientResultLabModel> OtherTestSet = labCompare
                        .Where(p => p.RequestItemCode.Contains("LAB281")
                        || p.RequestItemCode.Contains("LAB411")
                        || p.RequestItemCode.Contains("LAB282")
                        || p.RequestItemCode.Contains("LAB284")
                        || p.RequestItemCode.Contains("LAB285")
                        || p.RequestItemCode.Contains("LAB232")
                        || p.RequestItemCode.Contains("LAB251"))
                        .OrderBy(p => p.Year);
                    GenerateOther(OtherTestSet);
                    #endregion
                }

            }
        }

        private void GenerateCompleteBloodCount(IEnumerable<PatientResultLabModel> labTestSet)
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

                string hbAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001" && p.Year == year1)?.IsAbnormal;
                string hbAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001" && p.Year == year2)?.IsAbnormal;
                string hbAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001" && p.Year == year3)?.IsAbnormal;


                if (!string.IsNullOrEmpty(hbAbnormal1))
                {
                    page3.cellHb1.ForeColor = (hbAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page3.cellHb1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(hbAbnormal2))
                {
                    page3.cellHb2.ForeColor = (hbAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page3.cellHb2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(hbAbnormal3))
                {
                    page3.cellHb3.ForeColor = (hbAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page3.cellHb3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page3.cellHctRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020")?.ReferenceRange;
                page3.cellHct1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020" && p.Year == year1)?.ResultValue;
                page3.cellHct2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020" && p.Year == year2)?.ResultValue;
                page3.cellHct3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020" && p.Year == year3)?.ResultValue;

                string hctAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020" && p.Year == year1)?.IsAbnormal;
                string hctAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020" && p.Year == year2)?.IsAbnormal;
                string hctAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(hctAbnormal1))
                {
                    page3.cellHct1.ForeColor = (hctAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page3.cellHct1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(hctAbnormal2))
                {
                    page3.cellHct2.ForeColor = (hctAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page3.cellHct2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(hctAbnormal3))
                {
                    page3.cellHct3.ForeColor = (hctAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page3.cellHct3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }


                page3.cellMcvRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025")?.ReferenceRange;
                page3.cellMcv1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025" && p.Year == year1)?.ResultValue;
                page3.cellMcv2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025" && p.Year == year2)?.ResultValue;
                page3.cellMcv3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025" && p.Year == year3)?.ResultValue;

                string mcvAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025" && p.Year == year1)?.IsAbnormal;
                string mcvAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025" && p.Year == year2)?.IsAbnormal;
                string mcvAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(mcvAbnormal1))
                {
                    page3.cellMcv1.ForeColor = (mcvAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page3.cellMcv1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(mcvAbnormal2))
                {
                    page3.cellMcv2.ForeColor = (mcvAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page3.cellMcv2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(mcvAbnormal3))
                {
                    page3.cellMcv3.ForeColor = (mcvAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page3.cellMcv3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page3.cellMchRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030")?.ReferenceRange;
                page3.cellMch1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030" && p.Year == year1)?.ResultValue;
                page3.cellMch2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030" && p.Year == year2)?.ResultValue;
                page3.cellMch3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030" && p.Year == year3)?.ResultValue;

                string mchAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030" && p.Year == year1)?.IsAbnormal;
                string mchAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030" && p.Year == year2)?.IsAbnormal;
                string mchAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(mchAbnormal1))
                {
                    page3.cellMch1.ForeColor = (mchAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page3.cellMch1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(mchAbnormal2))
                {
                    page3.cellMch2.ForeColor = (mchAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page3.cellMch2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(mchAbnormal3))
                {
                    page3.cellMch3.ForeColor = (mchAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page3.cellMch3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page3.cellMchcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035")?.ReferenceRange;
                page3.cellMchc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035" && p.Year == year1)?.ResultValue;
                page3.cellMchc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035" && p.Year == year2)?.ResultValue;
                page3.cellMchc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035" && p.Year == year3)?.ResultValue;

                string mchcAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035" && p.Year == year1)?.IsAbnormal;
                string mchcAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035" && p.Year == year2)?.IsAbnormal;
                string mchcAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(mchcAbnormal1))
                {
                    page3.cellMchc1.ForeColor = (mchcAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page3.cellMchc1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(mchcAbnormal2))
                {
                    page3.cellMchc2.ForeColor = (mchcAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page3.cellMchc2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(mchcAbnormal3))
                {
                    page3.cellMchc3.ForeColor = (mchcAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page3.cellMchc3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page3.cellRdwRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1")?.ReferenceRange;
                page3.cellRdw1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1" && p.Year == year1)?.ResultValue;
                page3.cellRdw2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1" && p.Year == year2)?.ResultValue;
                page3.cellRdw3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1" && p.Year == year3)?.ResultValue;

                string rdwAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1" && p.Year == year1)?.IsAbnormal;
                string rdwAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1" && p.Year == year2)?.IsAbnormal;
                string rdwAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(rdwAbnormal1))
                {
                    page3.cellRdw1.ForeColor = (rdwAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page3.cellRdw1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(rdwAbnormal2))
                {
                    page3.cellRdw2.ForeColor = (rdwAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page3.cellRdw2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(rdwAbnormal3))
                {
                    page3.cellRdw3.ForeColor = (rdwAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page3.cellRdw3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page3.cellRbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428")?.ReferenceRange;
                page3.cellRbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428" && p.Year == year1)?.ResultValue;
                page3.cellRbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428" && p.Year == year2)?.ResultValue;
                page3.cellRbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428" && p.Year == year3)?.ResultValue;

                string rbcAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428" && p.Year == year1)?.IsAbnormal;
                string rbcAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428" && p.Year == year2)?.IsAbnormal;
                string rbcAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(rbcAbnormal1))
                {
                    page3.cellRbc1.ForeColor = (rbcAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page3.cellRbc1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(rbcAbnormal2))
                {
                    page3.cellRbc2.ForeColor = (rbcAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page3.cellRbc2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(rbcAbnormal3))
                {
                    page3.cellRbc3.ForeColor = (rbcAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page3.cellRbc3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page3.cellRbcMorRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13")?.ReferenceRange;
                page3.cellRbcMor1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13" && p.Year == year1)?.ResultValue;
                page3.cellRbcMor2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13" && p.Year == year2)?.ResultValue;
                page3.cellRbcMor3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13" && p.Year == year3)?.ResultValue;

                string RbcMorAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13" && p.Year == year1)?.IsAbnormal;
                string RbcMorAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13" && p.Year == year2)?.IsAbnormal;
                string RbcMorAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(RbcMorAbnormal1))
                {
                    page3.cellRbcMor1.ForeColor = (RbcMorAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page3.cellRbcMor1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(RbcMorAbnormal2))
                {
                    page3.cellRbcMor2.ForeColor = (RbcMorAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page3.cellRbcMor2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(RbcMorAbnormal3))
                {
                    page3.cellRbcMor3.ForeColor = (RbcMorAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page3.cellRbcMor3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page3.cellWbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006")?.ReferenceRange;
                page3.cellWbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006" && p.Year == year1)?.ResultValue;
                page3.cellWbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006" && p.Year == year2)?.ResultValue;
                page3.cellWbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006" && p.Year == year3)?.ResultValue;

                string wbcAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006" && p.Year == year1)?.IsAbnormal;
                string wbcAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006" && p.Year == year2)?.IsAbnormal;
                string wbcAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(wbcAbnormal1))
                {
                    page3.cellWbc1.ForeColor = (wbcAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page3.cellWbc1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(wbcAbnormal2))
                {
                    page3.cellWbc2.ForeColor = (wbcAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page3.cellWbc2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(wbcAbnormal3))
                {
                    page3.cellWbc3.ForeColor = (wbcAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page3.cellWbc3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page3.cellNectophilRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040")?.ReferenceRange;
                page3.cellNectophil1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040" && p.Year == year1)?.ResultValue;
                page3.cellNectophil2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040" && p.Year == year2)?.ResultValue;
                page3.cellNectophil3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040" && p.Year == year3)?.ResultValue;

                string NectophilAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040" && p.Year == year1)?.IsAbnormal;
                string NectophilAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040" && p.Year == year2)?.IsAbnormal;
                string NectophilAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(NectophilAbnormal1))
                {
                    page3.cellNectophil1.ForeColor = (NectophilAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page3.cellNectophil1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(NectophilAbnormal2))
                {
                    page3.cellNectophil2.ForeColor = (NectophilAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page3.cellNectophil2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(NectophilAbnormal3))
                {
                    page3.cellNectophil3.ForeColor = (NectophilAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page3.cellNectophil3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page3.cellLymphocyteRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050")?.ReferenceRange;
                page3.cellLymphocyte1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050" && p.Year == year1)?.ResultValue;
                page3.cellLymphocyte2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050" && p.Year == year2)?.ResultValue;
                page3.cellLymphocyte3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050" && p.Year == year3)?.ResultValue;

                string LymphocyteAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050" && p.Year == year1)?.IsAbnormal;
                string LymphocyteAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050" && p.Year == year2)?.IsAbnormal;
                string LymphocyteAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(LymphocyteAbnormal1))
                {
                    page3.cellLymphocyte1.ForeColor = (LymphocyteAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page3.cellLymphocyte1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(LymphocyteAbnormal2))
                {
                    page3.cellLymphocyte2.ForeColor = (LymphocyteAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page3.cellLymphocyte2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(LymphocyteAbnormal3))
                {
                    page3.cellLymphocyte3.ForeColor = (LymphocyteAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page3.cellLymphocyte3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page3.cellMonocyteRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060")?.ReferenceRange;
                page3.cellMonocyte1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060" && p.Year == year1)?.ResultValue;
                page3.cellMonocyte2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060" && p.Year == year2)?.ResultValue;
                page3.cellMonocyte3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060" && p.Year == year3)?.ResultValue;

                string MonocyteAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060" && p.Year == year1)?.IsAbnormal;
                string MonocyteAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060" && p.Year == year2)?.IsAbnormal;
                string MonocyteAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(MonocyteAbnormal1))
                {
                    page3.cellMonocyte1.ForeColor = (MonocyteAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page3.cellMonocyte1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(MonocyteAbnormal2))
                {
                    page3.cellMonocyte2.ForeColor = (MonocyteAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page3.cellMonocyte2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(MonocyteAbnormal3))
                {
                    page3.cellMonocyte3.ForeColor = (MonocyteAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page3.cellMonocyte3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page3.cellEosinophilRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070")?.ReferenceRange;
                page3.cellEosinophil1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070" && p.Year == year1)?.ResultValue;
                page3.cellEosinophil2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070" && p.Year == year2)?.ResultValue;
                page3.cellEosinophil3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070" && p.Year == year3)?.ResultValue;

                string EosinophilAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070" && p.Year == year1)?.IsAbnormal;
                string EosinophilAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070" && p.Year == year2)?.IsAbnormal;
                string EosinophilAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(EosinophilAbnormal1))
                {
                    page3.cellEosinophil1.ForeColor = (EosinophilAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page3.cellEosinophil1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(EosinophilAbnormal2))
                {
                    page3.cellEosinophil2.ForeColor = (EosinophilAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page3.cellEosinophil2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(EosinophilAbnormal3))
                {
                    page3.cellEosinophil3.ForeColor = (EosinophilAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page3.cellEosinophil3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page3.cellBasophilRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080")?.ReferenceRange;
                page3.cellBasophil1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080" && p.Year == year1)?.ResultValue;
                page3.cellBasophil2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080" && p.Year == year2)?.ResultValue;
                page3.cellBasophil3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080" && p.Year == year3)?.ResultValue;

                string BasophilAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080" && p.Year == year1)?.IsAbnormal;
                string BasophilAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080" && p.Year == year2)?.IsAbnormal;
                string BasophilAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(BasophilAbnormal1))
                {
                    page3.cellBasophil1.ForeColor = (BasophilAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page3.cellBasophil1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BasophilAbnormal2))
                {
                    page3.cellBasophil2.ForeColor = (BasophilAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page3.cellBasophil2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BasophilAbnormal3))
                {
                    page3.cellBasophil3.ForeColor = (BasophilAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page3.cellBasophil3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page3.cellPlateletSmearRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427")?.ReferenceRange;
                page3.cellPlateletSmear1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427" && p.Year == year1)?.ResultValue;
                page3.cellPlateletSmear2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427" && p.Year == year2)?.ResultValue;
                page3.cellPlateletSmear3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427" && p.Year == year3)?.ResultValue;

                string PlateletSmearAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427" && p.Year == year1)?.IsAbnormal;
                string PlateletSmearAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427" && p.Year == year2)?.IsAbnormal;
                string PlateletSmearAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(PlateletSmearAbnormal1))
                {
                    page3.cellPlateletSmear1.ForeColor = (PlateletSmearAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page3.cellPlateletSmear1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(PlateletSmearAbnormal2))
                {
                    page3.cellPlateletSmear2.ForeColor = (PlateletSmearAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page3.cellPlateletSmear2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(PlateletSmearAbnormal3))
                {
                    page3.cellPlateletSmear3.ForeColor = (PlateletSmearAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page3.cellPlateletSmear3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page3.cellPlateletsCountRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010")?.ReferenceRange;
                page3.cellPlateletsCount1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010" && p.Year == year1)?.ResultValue;
                page3.cellPlateletsCount2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010" && p.Year == year2)?.ResultValue;
                page3.cellPlateletsCount3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010" && p.Year == year3)?.ResultValue;

                string PlateletsCountAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010" && p.Year == year1)?.IsAbnormal;
                string PlateletsCountAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010" && p.Year == year2)?.IsAbnormal;
                string PlateletsCountAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(PlateletsCountAbnormal1))
                {
                    page3.cellPlateletsCount1.ForeColor = (PlateletsCountAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page3.cellPlateletsCount1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(PlateletsCountAbnormal2))
                {
                    page3.cellPlateletsCount2.ForeColor = (PlateletsCountAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page3.cellPlateletsCount2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(PlateletsCountAbnormal3))
                {
                    page3.cellPlateletsCount3.ForeColor = (PlateletsCountAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page3.cellPlateletsCount3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
            }
            else
            {
                page3.cellCBCYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page3.cellCBCYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page3.cellCBCYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }

        }

        private void GenerateUrinalysis(IEnumerable<PatientResultLabModel> labTestSet)
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

                string colorAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year1)?.IsAbnormal;
                string colorAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year2)?.IsAbnormal;
                string colorAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(colorAbnormal1))
                {
                    page4.cellColor1.ForeColor = (colorAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellColor1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(colorAbnormal2))
                {
                    page4.cellColor2.ForeColor = (colorAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellColor2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(colorAbnormal3))
                {
                    page4.cellColor3.ForeColor = (colorAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellColor3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellClarityRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21")?.ReferenceRange;
                page4.cellClarity1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year1)?.ResultValue;
                page4.cellClarity2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year2)?.ResultValue;
                page4.cellClarity3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year3)?.ResultValue;

                string ClarityAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year1)?.IsAbnormal;
                string ClarityAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year2)?.IsAbnormal;
                string ClarityAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(ClarityAbnormal1))
                {
                    page4.cellClarity1.ForeColor = (ClarityAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellClarity1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(ClarityAbnormal2))
                {
                    page4.cellClarity2.ForeColor = (ClarityAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellClarity2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(ClarityAbnormal3))
                {
                    page4.cellClarity3.ForeColor = (ClarityAbnormal3 == "H") ? Color.Red : Color.Blue; ;
                    page4.cellClarity3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellSpacGraRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001")?.ReferenceRange;
                page4.cellSpacGra1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001" && p.Year == year1)?.ResultValue;
                page4.cellSpacGra2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001" && p.Year == year2)?.ResultValue;
                page4.cellSpacGra3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001" && p.Year == year3)?.ResultValue;

                string SpacGraAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001" && p.Year == year1)?.IsAbnormal;
                string SpacGraAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001" && p.Year == year2)?.IsAbnormal;
                string SpacGraAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(SpacGraAbnormal1))
                {
                    page4.cellSpacGra1.ForeColor = (SpacGraAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellSpacGra1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(SpacGraAbnormal2))
                {
                    page4.cellSpacGra2.ForeColor = (SpacGraAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellSpacGra2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(SpacGraAbnormal3))
                {
                    page4.cellSpacGra3.ForeColor = (SpacGraAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellClarity3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellPhRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080")?.ReferenceRange;
                page4.cellPh1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080" && p.Year == year1)?.ResultValue;
                page4.cellPh2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080" && p.Year == year2)?.ResultValue;
                page4.cellPh3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080" && p.Year == year3)?.ResultValue;

                string phAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080" && p.Year == year1)?.IsAbnormal;
                string phAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080" && p.Year == year2)?.IsAbnormal;
                string phAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(phAbnormal1))
                {
                    page4.cellPh1.ForeColor = (phAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellPh1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(phAbnormal2))
                {
                    page4.cellPh2.ForeColor = (phAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellPh2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(phAbnormal3))
                {
                    page4.cellPh3.ForeColor = (phAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellPh3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellProteinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085")?.ReferenceRange;
                page4.cellProtein1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085" && p.Year == year1)?.ResultValue;
                page4.cellProtein2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085" && p.Year == year2)?.ResultValue;
                page4.cellProtein3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085" && p.Year == year3)?.ResultValue;

                string ProteinAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085" && p.Year == year1)?.IsAbnormal;
                string ProteinAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085" && p.Year == year2)?.IsAbnormal;
                string ProteinAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(ProteinAbnormal1))
                {
                    page4.cellProtein1.ForeColor = (ProteinAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellProtein1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(ProteinAbnormal2))
                {
                    page4.cellProtein2.ForeColor = (ProteinAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellProtein2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(ProteinAbnormal3))
                {
                    page4.cellProtein3.ForeColor = (ProteinAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellProtein3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellGlucoseRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090")?.ReferenceRange;
                page4.cellGlucose1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090" && p.Year == year1)?.ResultValue;
                page4.cellGlucose2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090" && p.Year == year2)?.ResultValue;
                page4.cellGlucose3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090" && p.Year == year3)?.ResultValue;

                string GlucoseAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090" && p.Year == year1)?.IsAbnormal;
                string GlucoseAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090" && p.Year == year2)?.IsAbnormal;
                string GlucoseAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(GlucoseAbnormal1))
                {
                    page4.cellGlucose1.ForeColor = (GlucoseAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellGlucose1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(GlucoseAbnormal2))
                {
                    page4.cellGlucose2.ForeColor = (GlucoseAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellGlucose2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(GlucoseAbnormal3))
                {
                    page4.cellGlucose3.ForeColor = (GlucoseAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellGlucose3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellKetoneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047")?.ReferenceRange;
                page4.cellKetone1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047" && p.Year == year1)?.ResultValue;
                page4.cellKetone2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047" && p.Year == year2)?.ResultValue;
                page4.cellKetone3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047" && p.Year == year3)?.ResultValue;

                string KetoneAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047" && p.Year == year1)?.IsAbnormal;
                string KetoneAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047" && p.Year == year2)?.IsAbnormal;
                string KetoneAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(KetoneAbnormal1))
                {
                    page4.cellKetone1.ForeColor = (KetoneAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellKetone1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(KetoneAbnormal2))
                {
                    page4.cellKetone2.ForeColor = (KetoneAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellKetone2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(KetoneAbnormal3))
                {
                    page4.cellKetone3.ForeColor = (KetoneAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellKetone3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellNitritesRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154")?.ReferenceRange;
                page4.cellNitrites1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154" && p.Year == year1)?.ResultValue;
                page4.cellNitrites2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154" && p.Year == year2)?.ResultValue;
                page4.cellNitrites3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154" && p.Year == year3)?.ResultValue;

                string NitritesAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154" && p.Year == year1)?.IsAbnormal;
                string NitritesAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154" && p.Year == year2)?.IsAbnormal;
                string NitritesAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(NitritesAbnormal1))
                {
                    page4.cellNitrites1.ForeColor = (NitritesAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellNitrites1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(NitritesAbnormal2))
                {
                    page4.cellNitrites2.ForeColor = (NitritesAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellNitrites2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(NitritesAbnormal3))
                {
                    page4.cellNitrites3.ForeColor = (NitritesAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellNitrites3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellBilirubinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151")?.ReferenceRange;
                page4.cellBilirubin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151" && p.Year == year1)?.ResultValue;
                page4.cellBilirubin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151" && p.Year == year2)?.ResultValue;
                page4.cellBilirubin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151" && p.Year == year3)?.ResultValue;

                string BilirubinAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151" && p.Year == year1)?.IsAbnormal;
                string BilirubinAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151" && p.Year == year2)?.IsAbnormal;
                string BilirubinAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(BilirubinAbnormal1))
                {
                    page4.cellBilirubin1.ForeColor = (BilirubinAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellBilirubin1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BilirubinAbnormal2))
                {
                    page4.cellBilirubin2.ForeColor = (BilirubinAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellBilirubin2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BilirubinAbnormal3))
                {
                    page4.cellBilirubin3.ForeColor = (BilirubinAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellBilirubin3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellUrobilinogenRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150")?.ReferenceRange;
                page4.cellUrobilinogen1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150" && p.Year == year1)?.ResultValue;
                page4.cellUrobilinogen2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150" && p.Year == year2)?.ResultValue;
                page4.cellUrobilinogen3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150" && p.Year == year3)?.ResultValue;

                string UrobilinogenAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150" && p.Year == year1)?.IsAbnormal;
                string UrobilinogenAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150" && p.Year == year2)?.IsAbnormal;
                string UrobilinogenAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(UrobilinogenAbnormal1))
                {
                    page4.cellUrobilinogen1.ForeColor = (UrobilinogenAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellUrobilinogen1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(UrobilinogenAbnormal2))
                {
                    page4.cellUrobilinogen2.ForeColor = (UrobilinogenAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellUrobilinogen2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(UrobilinogenAbnormal3))
                {
                    page4.cellUrobilinogen3.ForeColor = (UrobilinogenAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellUrobilinogen3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellLeukocyteRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153")?.ReferenceRange;
                page4.cellLeukocyte1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153" && p.Year == year1)?.ResultValue;
                page4.cellLeukocyte2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153" && p.Year == year2)?.ResultValue;
                page4.cellLeukocyte3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153" && p.Year == year3)?.ResultValue;

                string LeukocyteAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153" && p.Year == year1)?.IsAbnormal;
                string LeukocyteAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153" && p.Year == year2)?.IsAbnormal;
                string LeukocyteAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(LeukocyteAbnormal1))
                {
                    page4.cellLeukocyte1.ForeColor = (LeukocyteAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellLeukocyte1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(LeukocyteAbnormal2))
                {
                    page4.cellLeukocyte2.ForeColor = (LeukocyteAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellLeukocyte2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(LeukocyteAbnormal3))
                {
                    page4.cellLeukocyte3.ForeColor = (LeukocyteAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellLeukocyte3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellBloodRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152")?.ReferenceRange;
                page4.cellBlood1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152" && p.Year == year1)?.ResultValue;
                page4.cellBlood2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152" && p.Year == year2)?.ResultValue;
                page4.cellBlood3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152" && p.Year == year3)?.ResultValue;

                string BloodAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152" && p.Year == year1)?.IsAbnormal;
                string BloodAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152" && p.Year == year2)?.IsAbnormal;
                string BloodAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(BloodAbnormal1))
                {
                    page4.cellBlood1.ForeColor = (BloodAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellBlood1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BloodAbnormal2))
                {
                    page4.cellBlood2.ForeColor = (BloodAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellBlood2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BloodAbnormal3))
                {
                    page4.cellBlood3.ForeColor = (BloodAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellBlood3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellErythrocytesRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140")?.ReferenceRange;
                page4.cellErythrocytes1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140" && p.Year == year1)?.ResultValue;
                page4.cellErythrocytes2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140" && p.Year == year2)?.ResultValue;
                page4.cellErythrocytes3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140" && p.Year == year3)?.ResultValue;

                string ErythrocytesAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140" && p.Year == year1)?.IsAbnormal;
                string ErythrocytesAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140" && p.Year == year2)?.IsAbnormal;
                string ErythrocytesAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(ErythrocytesAbnormal1))
                {
                    page4.cellErythrocytes1.ForeColor = (ErythrocytesAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellErythrocytes1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(ErythrocytesAbnormal2))
                {
                    page4.cellErythrocytes2.ForeColor = (ErythrocytesAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellErythrocytes2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(ErythrocytesAbnormal3))
                {
                    page4.cellErythrocytes3.ForeColor = (ErythrocytesAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellErythrocytes3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellWbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250")?.ReferenceRange;
                page4.cellWbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250" && p.Year == year1)?.ResultValue;
                page4.cellWbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250" && p.Year == year2)?.ResultValue;
                page4.cellWbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250" && p.Year == year3)?.ResultValue;

                string WbcAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250" && p.Year == year1)?.IsAbnormal;
                string WbcAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250" && p.Year == year2)?.IsAbnormal;
                string WbcAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(WbcAbnormal1))
                {
                    page4.cellWbc1.ForeColor = (WbcAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellWbc1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(WbcAbnormal2))
                {
                    page4.cellWbc2.ForeColor = (WbcAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellWbc2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(WbcAbnormal3))
                {
                    page4.cellWbc3.ForeColor = (WbcAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellWbc3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellRbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260")?.ReferenceRange;
                page4.cellRbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260" && p.Year == year1)?.ResultValue;
                page4.cellRbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260" && p.Year == year2)?.ResultValue;
                page4.cellRbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260" && p.Year == year3)?.ResultValue;

                string RbcAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260" && p.Year == year1)?.IsAbnormal;
                string RbcAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260" && p.Year == year2)?.IsAbnormal;
                string RbcAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(RbcAbnormal1))
                {
                    page4.cellRbc1.ForeColor = (RbcAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellRbc1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(RbcAbnormal2))
                {
                    page4.cellRbc2.ForeColor = (RbcAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellRbc2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(RbcAbnormal3))
                {
                    page4.cellRbc3.ForeColor = (RbcAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellRbc3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellEpithelialCellsRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270")?.ReferenceRange;
                page4.cellEpithelialCells1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270" && p.Year == year1)?.ResultValue;
                page4.cellEpithelialCells2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270" && p.Year == year2)?.ResultValue;
                page4.cellEpithelialCells3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270" && p.Year == year3)?.ResultValue;

                string EpithelialCellsAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270" && p.Year == year1)?.IsAbnormal;
                string EpithelialCellsAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270" && p.Year == year2)?.IsAbnormal;
                string EpithelialCellsAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(EpithelialCellsAbnormal1))
                {
                    page4.cellEpithelialCells1.ForeColor = (EpithelialCellsAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellEpithelialCells1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(EpithelialCellsAbnormal2))
                {
                    page4.cellEpithelialCells2.ForeColor = (EpithelialCellsAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellEpithelialCells2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(EpithelialCellsAbnormal3))
                {
                    page4.cellEpithelialCells3.ForeColor = (EpithelialCellsAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellEpithelialCells3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellCastsRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR16")?.ReferenceRange;
                page4.cellCasts1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR16" && p.Year == year1)?.ResultValue;
                page4.cellCasts2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR16" && p.Year == year2)?.ResultValue;
                page4.cellCasts3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR16" && p.Year == year3)?.ResultValue;

                string CastsAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR16" && p.Year == year1)?.IsAbnormal;
                string CastsAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR16" && p.Year == year2)?.IsAbnormal;
                string CastsAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR16" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(CastsAbnormal1))
                {
                    page4.cellCasts1.ForeColor = (CastsAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellCasts1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CastsAbnormal2))
                {
                    page4.cellCasts2.ForeColor = (CastsAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellCasts2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CastsAbnormal3))
                {
                    page4.cellCasts3.ForeColor = (CastsAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellCasts3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellBacteriaRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0155")?.ReferenceRange;
                page4.cellBacteria1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0155" && p.Year == year1)?.ResultValue;
                page4.cellBacteria2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0155" && p.Year == year2)?.ResultValue;
                page4.cellBacteria3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0155" && p.Year == year3)?.ResultValue;

                string BacteriaAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0155" && p.Year == year1)?.IsAbnormal;
                string BacteriaAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0155" && p.Year == year2)?.IsAbnormal;
                string BacteriaAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0155" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(BacteriaAbnormal1))
                {
                    page4.cellBacteria1.ForeColor = (BacteriaAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellBacteria1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BacteriaAbnormal2))
                {
                    page4.cellBacteria2.ForeColor = (BacteriaAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellBacteria2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BacteriaAbnormal2))
                {
                    page4.cellBacteria3.ForeColor = (BacteriaAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellBacteria3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellBuddingYeastRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR17")?.ReferenceRange;
                page4.cellBuddingYeast1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR17" && p.Year == year1)?.ResultValue;
                page4.cellBuddingYeast2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR17" && p.Year == year2)?.ResultValue;
                page4.cellBuddingYeast3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR17" && p.Year == year3)?.ResultValue;

                string BuddingYeastAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR17" && p.Year == year1)?.IsAbnormal;
                string BuddingYeastAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR17" && p.Year == year2)?.IsAbnormal;
                string BuddingYeastAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR17" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(BuddingYeastAbnormal1))
                {
                    page4.cellBuddingYeast1.ForeColor = (BuddingYeastAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellBuddingYeast1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BuddingYeastAbnormal2))
                {
                    page4.cellBuddingYeast2.ForeColor = (BuddingYeastAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellBuddingYeast2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BuddingYeastAbnormal3))
                {
                    page4.cellBuddingYeast3.ForeColor = (BuddingYeastAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellBuddingYeast3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellCrystalRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR19")?.ReferenceRange;
                page4.cellCrystal1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR19" && p.Year == year1)?.ResultValue;
                page4.cellCrystal2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR19" && p.Year == year2)?.ResultValue;
                page4.cellCrystal3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR19" && p.Year == year3)?.ResultValue;

                string CrystalAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR19" && p.Year == year1)?.IsAbnormal;
                string CrystalAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR19" && p.Year == year2)?.IsAbnormal;
                string CrystalAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR19" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(CrystalAbnormal1))
                {
                    page4.cellCrystal1.ForeColor = (CrystalAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellCrystal1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CrystalAbnormal2))
                {
                    page4.cellCrystal2.ForeColor = (CrystalAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellCrystal2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CrystalAbnormal3))
                {
                    page4.cellCrystal3.ForeColor = (CrystalAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellCrystal3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellMucousRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR18")?.ReferenceRange;
                page4.cellMucous1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR18" && p.Year == year1)?.ResultValue;
                page4.cellMucous2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR18" && p.Year == year2)?.ResultValue;
                page4.cellMucous3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR18" && p.Year == year3)?.ResultValue;

                string MucousAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR18" && p.Year == year1)?.IsAbnormal;
                string MucousAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR18" && p.Year == year2)?.IsAbnormal;
                string MucousAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR18" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(MucousAbnormal1))
                {
                    page4.cellMucous1.ForeColor = (MucousAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellMucous1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(MucousAbnormal2))
                {
                    page4.cellMucous2.ForeColor = (MucousAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellMucous2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(MucousAbnormal3))
                {
                    page4.cellMucous3.ForeColor = (MucousAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellMucous3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellAmorphousRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14")?.ReferenceRange;
                page4.cellAmorphous1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14" && p.Year == year1)?.ResultValue;
                page4.cellAmorphous2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14" && p.Year == year2)?.ResultValue;
                page4.cellAmorphous3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14" && p.Year == year3)?.ResultValue;

                string AmorphousAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14" && p.Year == year1)?.IsAbnormal;
                string AmorphousAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14" && p.Year == year2)?.IsAbnormal;
                string AmorphousAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(AmorphousAbnormal1))
                {
                    page4.cellAmorphous1.ForeColor = (AmorphousAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellAmorphous1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AmorphousAbnormal2))
                {
                    page4.cellAmorphous2.ForeColor = (AmorphousAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellAmorphous2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AmorphousAbnormal3))
                {
                    page4.cellAmorphous3.ForeColor = (AmorphousAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellAmorphous3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellOtherRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20")?.ReferenceRange;
                page4.cellOther1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20" && p.Year == year1)?.ResultValue;
                page4.cellOther2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20" && p.Year == year2)?.ResultValue;
                page4.cellOther3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20" && p.Year == year3)?.ResultValue;

                string OtherAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20" && p.Year == year1)?.IsAbnormal;
                string OtherAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20" && p.Year == year2)?.IsAbnormal;
                string OtherAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(OtherAbnormal1))
                {
                    page4.cellOther1.ForeColor = (OtherAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellOther1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(OtherAbnormal2))
                {
                    page4.cellOther2.ForeColor = (OtherAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellOther2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(OtherAbnormal3))
                {
                    page4.cellOther3.ForeColor = (OtherAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellOther3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
            }
            else
            {
                page4.cellUAYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page4.cellUAYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page4.cellUAYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }

        private void GenerateRenalFunction(IEnumerable<PatientResultLabModel> labTestSet)
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

                string bunAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27" && p.Year == year1)?.IsAbnormal;
                string bunAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27" && p.Year == year2)?.IsAbnormal;
                string bunAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(bunAbnormal1))
                {
                    page5.cellBun1.ForeColor = (bunAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellBun1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(bunAbnormal2))
                {
                    page5.cellBun2.ForeColor = (bunAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellBun2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(bunAbnormal3))
                {
                    page5.cellBun3.ForeColor = (bunAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellBun3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellCreatinineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070")?.ReferenceRange;
                page5.cellCreatinine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070" && p.Year == year1)?.ResultValue;
                page5.cellCreatinine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070" && p.Year == year2)?.ResultValue;
                page5.cellCreatinine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070" && p.Year == year3)?.ResultValue;

                string CreatinineAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070" && p.Year == year1)?.IsAbnormal;
                string CreatinineAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070" && p.Year == year2)?.IsAbnormal;
                string CreatinineAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(CreatinineAbnormal1))
                {
                    page5.cellCreatinine1.ForeColor = (CreatinineAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellCreatinine1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CreatinineAbnormal2))
                {
                    page5.cellCreatinine2.ForeColor = (CreatinineAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellCreatinine2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CreatinineAbnormal3))
                {
                    page5.cellCreatinine3.ForeColor = (CreatinineAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellCreatinine3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
            }
            else
            {
                page5.cellRenalYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page5.cellRenalYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page5.cellRenalYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }

        private void GenerateFastingBloodSugar(IEnumerable<PatientResultLabModel> labTestSet)
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

                string fbsAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0180" && p.Year == year1)?.IsAbnormal;
                string fbsAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0180" && p.Year == year2)?.IsAbnormal;
                string fbsAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0180" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(fbsAbnormal1))
                {
                    page6.cellFbs1.ForeColor = (fbsAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page6.cellFbs1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(fbsAbnormal2))
                {
                    page6.cellFbs2.ForeColor = (fbsAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page6.cellFbs2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(fbsAbnormal3))
                {
                    page6.cellFbs3.ForeColor = (fbsAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page6.cellFbs3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
            }
            else
            {
                page6.cellFbsYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page6.cellFbsYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page6.cellFbsYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }

        private void GenerateUricAcid(IEnumerable<PatientResultLabModel> labTestSet)
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

                string uricAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320" && p.Year == year1)?.IsAbnormal;
                string uricAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320" && p.Year == year2)?.IsAbnormal;
                string uricAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(uricAbnormal1))
                {
                    page7.cellUric1.ForeColor = (uricAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page7.cellUric1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(uricAbnormal2))
                {
                    page7.cellUric2.ForeColor = (uricAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page7.cellUric2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(uricAbnormal3))
                {
                    page7.cellUric3.ForeColor = (uricAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page7.cellUric3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
            }
            else
            {
                page7.cellUricYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page7.cellUricYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page7.cellUricYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }

        private void GenerateLipidProfiles(IEnumerable<PatientResultLabModel> labTestSet)
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

                string cholesterolAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130" && p.Year == year1)?.IsAbnormal;
                string cholesterolAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130" && p.Year == year2)?.IsAbnormal;
                string cholesterolAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(cholesterolAbnormal1))
                {
                    page6.cellCholesterol1.ForeColor = (cholesterolAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page6.cellCholesterol1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(cholesterolAbnormal2))
                {
                    page6.cellCholesterol2.ForeColor = (cholesterolAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page6.cellCholesterol2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(cholesterolAbnormal3))
                {
                    page6.cellCholesterol3.ForeColor = (cholesterolAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page6.cellCholesterol3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page6.cellTriglycerideRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140")?.ReferenceRange;
                page6.cellTriglyceride1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140" && p.Year == year1)?.ResultValue;
                page6.cellTriglyceride2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140" && p.Year == year2)?.ResultValue;
                page6.cellTriglyceride3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140" && p.Year == year3)?.ResultValue;

                string TriglycerideAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140" && p.Year == year1)?.IsAbnormal;
                string TriglycerideAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140" && p.Year == year2)?.IsAbnormal;
                string TriglycerideAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(TriglycerideAbnormal1))
                {
                    page6.cellTriglyceride1.ForeColor = (TriglycerideAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page6.cellTriglyceride1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(TriglycerideAbnormal2))
                {
                    page6.cellTriglyceride2.ForeColor = (TriglycerideAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page6.cellTriglyceride2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(TriglycerideAbnormal3))
                {
                    page6.cellTriglyceride3.ForeColor = (TriglycerideAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page6.cellTriglyceride3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page6.cellLdlRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159")?.ReferenceRange;
                page6.cellLdl1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159" && p.Year == year1)?.ResultValue;
                page6.cellLdl2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159" && p.Year == year2)?.ResultValue;
                page6.cellLdl3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159" && p.Year == year3)?.ResultValue;

                string ldlAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159" && p.Year == year1)?.IsAbnormal;
                string ldlAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159" && p.Year == year2)?.IsAbnormal;
                string ldlAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(ldlAbnormal1))
                {
                    page6.cellLdl1.ForeColor = (ldlAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page6.cellLdl1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(ldlAbnormal2))
                {
                    page6.cellLdl2.ForeColor = (ldlAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page6.cellLdl2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(ldlAbnormal3))
                {
                    page6.cellLdl3.ForeColor = (ldlAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page6.cellLdl3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page6.cellHdlRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150")?.ReferenceRange;
                page6.cellHdl1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150" && p.Year == year1)?.ResultValue;
                page6.cellHdl2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150" && p.Year == year2)?.ResultValue;
                page6.cellHdl3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150" && p.Year == year3)?.ResultValue;

                string hdlAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150" && p.Year == year1)?.IsAbnormal;
                string hdlAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150" && p.Year == year2)?.IsAbnormal;
                string hdlAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(hdlAbnormal1))
                {
                    page6.cellHdl1.ForeColor = (hdlAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page6.cellHdl1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(hdlAbnormal2))
                {
                    page6.cellHdl2.ForeColor = (hdlAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page6.cellHdl2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(hdlAbnormal3))
                {
                    page6.cellHdl3.ForeColor = (hdlAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page6.cellHdl3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
            }
            else
            {
                page6.cellLipidYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page6.cellLipidYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page6.cellLipidYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }

        private void GenerateLiverFunction(IEnumerable<PatientResultLabModel> labTestSet)
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

                string astAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50" && p.Year == year1)?.IsAbnormal;
                string astAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50" && p.Year == year2)?.IsAbnormal;
                string astAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(astAbnormal1))
                {
                    page5.cellAst1.ForeColor = (astAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellAst1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(astAbnormal2))
                {
                    page5.cellAst2.ForeColor = (astAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellAst2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(astAbnormal3))
                {
                    page5.cellAst3.ForeColor = (astAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellAst3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellAltRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51")?.ReferenceRange;
                page5.cellAlt1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51" && p.Year == year1)?.ResultValue;
                page5.cellAlt2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51" && p.Year == year2)?.ResultValue;
                page5.cellAlt3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51" && p.Year == year3)?.ResultValue;

                string altAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51" && p.Year == year1)?.IsAbnormal;
                string altAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51" && p.Year == year2)?.IsAbnormal;
                string altAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(altAbnormal1))
                {
                    page5.cellAlt1.ForeColor = (altAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellAlt1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(altAbnormal2))
                {
                    page5.cellAlt2.ForeColor = (altAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellAlt2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(altAbnormal3))
                {
                    page5.cellAlt3.ForeColor = (altAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellAlt3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellAlpRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33")?.ReferenceRange;
                page5.cellAlp1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33" && p.Year == year1)?.ResultValue;
                page5.cellAlp2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33" && p.Year == year2)?.ResultValue;
                page5.cellAlp3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33" && p.Year == year3)?.ResultValue;

                string alpAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33" && p.Year == year1)?.IsAbnormal;
                string alpAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33" && p.Year == year2)?.IsAbnormal;
                string alpAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(alpAbnormal1))
                {
                    page5.cellAlp1.ForeColor = (alpAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellAlp1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(alpAbnormal2))
                {
                    page5.cellAlp2.ForeColor = (alpAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellAlp2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(alpAbnormal3))
                {
                    page5.cellAlp3.ForeColor = (alpAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellAlp3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellTotalBilirubinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48")?.ReferenceRange;
                page5.cellTotalBilirubin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48" && p.Year == year1)?.ResultValue;
                page5.cellTotalBilirubin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48" && p.Year == year2)?.ResultValue;
                page5.cellTotalBilirubin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48" && p.Year == year3)?.ResultValue;

                string TotalBilirubinAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48" && p.Year == year1)?.IsAbnormal;
                string TotalBilirubinAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48" && p.Year == year2)?.IsAbnormal;
                string TotalBilirubinAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(TotalBilirubinAbnormal1))
                {
                    page5.cellTotalBilirubin1.ForeColor = (TotalBilirubinAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellTotalBilirubin1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(TotalBilirubinAbnormal2))
                {
                    page5.cellTotalBilirubin2.ForeColor = (TotalBilirubinAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellTotalBilirubin2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(TotalBilirubinAbnormal3))
                {
                    page5.cellTotalBilirubin3.ForeColor = (TotalBilirubinAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellTotalBilirubin3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellDirectBilirubinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49")?.ReferenceRange;
                page5.cellDirectBilirubin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year1)?.ResultValue;
                page5.cellDirectBilirubin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year2)?.ResultValue;
                page5.cellDirectBilirubin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year3)?.ResultValue;

                string DirectBilirubiAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year1)?.IsAbnormal;
                string DirectBilirubiAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year2)?.IsAbnormal;
                string DirectBilirubiAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(DirectBilirubiAbnormal1))
                {
                    page5.cellDirectBilirubin1.ForeColor = (DirectBilirubiAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellDirectBilirubin1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(DirectBilirubiAbnormal2))
                {
                    page5.cellDirectBilirubin2.ForeColor = (DirectBilirubiAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellDirectBilirubin2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(DirectBilirubiAbnormal3))
                {
                    page5.cellDirectBilirubin3.ForeColor = (DirectBilirubiAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellDirectBilirubin3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellTotalProteinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105")?.ReferenceRange;
                page5.cellTotalProtein1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105" && p.Year == year1)?.ResultValue;
                page5.cellTotalProtein2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105" && p.Year == year2)?.ResultValue;
                page5.cellTotalProtein3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105" && p.Year == year3)?.ResultValue;

                string TotalProteinAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105" && p.Year == year1)?.IsAbnormal;
                string TotalProteinAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105" && p.Year == year2)?.IsAbnormal;
                string TotalProteinAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(TotalProteinAbnormal1))
                {
                    page5.cellTotalProtein1.ForeColor = (TotalProteinAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellTotalProtein1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(TotalProteinAbnormal2))
                {
                    page5.cellTotalProtein2.ForeColor = (TotalProteinAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellTotalProtein2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(TotalProteinAbnormal3))
                {
                    page5.cellTotalProtein3.ForeColor = (TotalProteinAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellTotalProtein3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellAlbuminRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49")?.ReferenceRange;
                page5.cellAlbumin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year1)?.ResultValue;
                page5.cellAlbumin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year2)?.ResultValue;
                page5.cellAlbumin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year3)?.ResultValue;

                string AlbuminAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year1)?.IsAbnormal;
                string AlbuminAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year2)?.IsAbnormal;
                string AlbuminAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(AlbuminAbnormal1))
                {
                    page5.cellAlbumin1.ForeColor = (AlbuminAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellAlbumin1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AlbuminAbnormal2))
                {
                    page5.cellAlbumin2.ForeColor = (AlbuminAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellAlbumin2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AlbuminAbnormal3))
                {
                    page5.cellAlbumin3.ForeColor = (AlbuminAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellAlbumin3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellGlobulinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46")?.ReferenceRange;
                page5.cellGlobulin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year1)?.ResultValue;
                page5.cellGlobulin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year2)?.ResultValue;
                page5.cellGlobulin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year3)?.ResultValue;

                string GlobulinAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year1)?.IsAbnormal;
                string GlobulinAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year2)?.IsAbnormal;
                string GlobulinAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(GlobulinAbnormal1))
                {
                    page5.cellGlobulin1.ForeColor = (GlobulinAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellGlobulin1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(GlobulinAbnormal2))
                {
                    page5.cellGlobulin2.ForeColor = (GlobulinAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellGlobulin2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(GlobulinAbnormal3))
                {
                    page5.cellGlobulin3.ForeColor = (GlobulinAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellGlobulin3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
            }
            else
            {
                page5.cellLiverYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page5.cellLiverYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page5.cellLiverYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }

        private void GenerateImmunology(IEnumerable<PatientResultLabModel> labTestSet)
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

                string HbsAgAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35" && p.Year == year1)?.IsAbnormal;
                string HbsAgAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35" && p.Year == year2)?.IsAbnormal;
                string HbsAgAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(HbsAgAbnormal1))
                {
                    page7.cellHbsAg1.ForeColor = (HbsAgAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page7.cellHbsAg1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(HbsAgAbnormal2))
                {
                    page7.cellHbsAg2.ForeColor = (HbsAgAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page7.cellHbsAg2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(HbsAgAbnormal3))
                {
                    page7.cellHbsAg3.ForeColor = (HbsAgAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page7.cellHbsAg3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page7.cellCoiAgRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34")?.ReferenceRange;
                page7.cellCoiAg1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34" && p.Year == year1)?.ResultValue;
                page7.cellCoiAg2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34" && p.Year == year2)?.ResultValue;
                page7.cellCoiAg3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34" && p.Year == year3)?.ResultValue;

                string CoiAgAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34" && p.Year == year1)?.IsAbnormal;
                string CoiAgAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34" && p.Year == year2)?.IsAbnormal;
                string CoiAgAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(CoiAgAbnormal1))
                {
                    page7.cellCoiAg1.ForeColor = (CoiAgAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page7.cellCoiAg1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CoiAgAbnormal2))
                {
                    page7.cellCoiAg2.ForeColor = (CoiAgAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page7.cellCoiAg2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CoiAgAbnormal3))
                {
                    page7.cellCoiAg3.ForeColor = (CoiAgAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page7.cellCoiAg3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page7.cellCoiHbsRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121")?.ReferenceRange;
                page7.cellCoiHbs1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121" && p.Year == year1)?.ResultValue;
                page7.cellCoiHbs2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121" && p.Year == year2)?.ResultValue;
                page7.cellCoiHbs3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121" && p.Year == year3)?.ResultValue;

                string CoiHbsAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121" && p.Year == year1)?.IsAbnormal;
                string CoiHbsAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121" && p.Year == year2)?.IsAbnormal;
                string CoiHbsAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(CoiHbsAbnormal1))
                {
                    page7.cellCoiHbs1.ForeColor = (CoiHbsAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page7.cellCoiHbs1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CoiHbsAbnormal2))
                {
                    page7.cellCoiHbs2.ForeColor = (CoiHbsAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page7.cellCoiHbs2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CoiHbsAbnormal3))
                {
                    page7.cellCoiHbs3.ForeColor = (CoiHbsAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page7.cellCoiHbs3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

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

        private void GenerateStool(IEnumerable<PatientResultLabModel> labTestSet)
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

                page8.cellStColorRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080")?.ReferenceRange;
                page8.cellStColor1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year1)?.ResultValue;
                page8.cellStColor2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year2)?.ResultValue;
                page8.cellStColor3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year3)?.ResultValue;

                string StColorAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year1)?.IsAbnormal;
                string StColorAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year2)?.IsAbnormal;
                string StColorAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(StColorAbnormal1))
                {
                    page8.cellStColor1.ForeColor = (StColorAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page8.cellStColor1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(StColorAbnormal2))
                {
                    page8.cellStColor2.ForeColor = (StColorAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page8.cellStColor2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(StColorAbnormal3))
                {
                    page8.cellStColor3.ForeColor = (StColorAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page8.cellStColor3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page8.cellStappearRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21")?.ReferenceRange;
                page8.cellStappear1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year1)?.ResultValue;
                page8.cellStappear2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year2)?.ResultValue;
                page8.cellStappear2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year3)?.ResultValue;

                string StappearAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year1)?.IsAbnormal;
                string StappearAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year2)?.IsAbnormal;
                string StappearAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(StappearAbnormal1))
                {
                    page8.cellStappear1.ForeColor = (StappearAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page8.cellStappear1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(StappearAbnormal2))
                {
                    page8.cellStappear2.ForeColor = (StappearAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page8.cellStappear2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(StappearAbnormal3))
                {
                    page8.cellStappear3.ForeColor = (StappearAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page8.cellStappear3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
            }
            else
            {
                page8.cellStoolYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page8.cellStoolYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page8.cellStoolYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }

        private void GenerateToxicology(IEnumerable<PatientResultLabModel> labTestSet)
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

                    string AluminiumAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year1)?.IsAbnormal;
                    string AluminiumAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year2)?.IsAbnormal;
                    string AluminiumAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year3)?.IsAbnormal;

                    if (!string.IsNullOrEmpty(AluminiumAbnormal1))
                    {
                        page8.cellAluminium1.ForeColor = (AluminiumAbnormal1 == "H") ? Color.Red : Color.Blue;
                        page8.cellAluminium1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }

                    if (!string.IsNullOrEmpty(AluminiumAbnormal2))
                    {
                        page8.cellAluminium2.ForeColor = (AluminiumAbnormal2 == "H") ? Color.Red : Color.Blue;
                        page8.cellAluminium2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }

                    if (!string.IsNullOrEmpty(AluminiumAbnormal3))
                    {
                        page8.cellAluminium3.ForeColor = (AluminiumAbnormal3 == "H") ? Color.Red : Color.Blue;
                        page8.cellAluminium3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }
                    #endregion

                    #region Toluene

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124") != null)
                    {
                        page8.RowToluene.Visible = true;
                        page8.cellTolueneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124")?.ReferenceRange;
                        page8.cellToluene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year1)?.ResultValue;
                        page8.cellToluene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year2)?.ResultValue;
                        page8.cellToluene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year3)?.ResultValue;

                        string TolueneAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year1)?.IsAbnormal;
                        string TolueneAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year2)?.IsAbnormal;
                        string TolueneAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year3)?.IsAbnormal;

                        if (!string.IsNullOrEmpty(TolueneAbnormal1))
                        {
                            page8.cellToluene1.ForeColor = (TolueneAbnormal1 == "H") ? Color.Red : Color.Blue;
                            page8.cellToluene1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(TolueneAbnormal2))
                        {
                            page8.cellToluene2.ForeColor = (TolueneAbnormal2 == "H") ? Color.Red : Color.Blue;
                            page8.cellToluene2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(TolueneAbnormal3))
                        {
                            page8.cellToluene3.ForeColor = (TolueneAbnormal3 == "H") ? Color.Red : Color.Blue;
                            page8.cellToluene3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

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

                        string XyleneAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year1)?.IsAbnormal;
                        string XyleneAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year2)?.IsAbnormal;
                        string XyleneAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year3)?.IsAbnormal;

                        if (!string.IsNullOrEmpty(XyleneAbnormal1))
                        {
                            page8.cellXylene1.ForeColor = (XyleneAbnormal1 == "H") ? Color.Red : Color.Blue;
                            page8.cellXylene1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(XyleneAbnormal2))
                        {
                            page8.cellXylene2.ForeColor = (XyleneAbnormal2 == "H") ? Color.Red : Color.Blue;
                            page8.cellXylene2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(XyleneAbnormal3))
                        {
                            page8.cellXylene3.ForeColor = (XyleneAbnormal3 == "H") ? Color.Red : Color.Blue;
                            page8.cellXylene3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        page8.cellLeadRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75")?.ReferenceRange;
                        if (page8.cellLeadRange.Text != null && page8.cellLeadRange.Text.Length > 20)
                        {
                            page8.cellLeadRange.Font = new Font("Angsana New", 7);
                        }
                    }

                    #endregion

                    #region Lead in blood (Show all)
                    page8.cellLead1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year1)?.ResultValue;
                    page8.cellLead2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year2)?.ResultValue;
                    page8.cellLead3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year3)?.ResultValue;

                    string LeadAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year1)?.IsAbnormal;
                    string LeadAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year2)?.IsAbnormal;
                    string LeadAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year3)?.IsAbnormal;

                    if (!string.IsNullOrEmpty(LeadAbnormal1))
                    {
                        page8.cellLead1.ForeColor = (LeadAbnormal1 == "H") ? Color.Red : Color.Blue;
                        page8.cellLead1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }

                    if (!string.IsNullOrEmpty(LeadAbnormal2))
                    {
                        page8.cellLead2.ForeColor = (LeadAbnormal2 == "H") ? Color.Red : Color.Blue;
                        page8.cellLead2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }

                    if (!string.IsNullOrEmpty(LeadAbnormal3))
                    {
                        page8.cellLead3.ForeColor = (LeadAbnormal3 == "H") ? Color.Red : Color.Blue;
                        page8.cellLead3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }
                    #endregion

                    #region Carboxy

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120") != null)
                    {
                        page8.RowCarboxy.Visible = true;
                        page8.cellCarboxyRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120")?.ReferenceRange;
                        page8.cellCarboxy1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year1)?.ResultValue;
                        page8.cellCarboxy2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year2)?.ResultValue;
                        page8.cellCarboxy3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year3)?.ResultValue;

                        string CarboxyAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year1)?.IsAbnormal;
                        string CarboxyAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year2)?.IsAbnormal;
                        string CarboxyAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year3)?.IsAbnormal;

                        if (!string.IsNullOrEmpty(CarboxyAbnormal1))
                        {
                            page8.cellCarboxy1.ForeColor = (CarboxyAbnormal1 == "H") ? Color.Red : Color.Blue;
                            page8.cellCarboxy1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(CarboxyAbnormal2))
                        {
                            page8.cellCarboxy2.ForeColor = (CarboxyAbnormal2 == "H") ? Color.Red : Color.Blue;
                            page8.cellCarboxy2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(CarboxyAbnormal3))
                        {
                            page8.cellCarboxy3.ForeColor = (CarboxyAbnormal3 == "H") ? Color.Red : Color.Blue;
                            page8.cellCarboxy3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }
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

                        string MekAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year1)?.IsAbnormal;
                        string MekAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year2)?.IsAbnormal;
                        string MekAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year3)?.IsAbnormal;

                        if (!string.IsNullOrEmpty(MekAbnormal1))
                        {
                            page8.cellMek1.ForeColor = (MekAbnormal1 == "H") ? Color.Red : Color.Blue;
                            page8.cellMek1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(MekAbnormal2))
                        {
                            page8.cellMek2.ForeColor = (MekAbnormal2 == "H") ? Color.Red : Color.Blue;
                            page8.cellMek2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(MekAbnormal3))
                        {
                            page8.cellMek3.ForeColor = (MekAbnormal3 == "H") ? Color.Red : Color.Blue;
                            page8.cellMek3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }
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

                        string BenzeneAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115" && p.Year == year1)?.IsAbnormal;
                        string BenzeneAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115" && p.Year == year2)?.IsAbnormal;
                        string BenzeneAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115" && p.Year == year3)?.IsAbnormal;

                        if (!string.IsNullOrEmpty(BenzeneAbnormal1))
                        {
                            page8.cellBenzene1.ForeColor = (BenzeneAbnormal1 == "H") ? Color.Red : Color.Blue;
                            page8.cellBenzene1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(BenzeneAbnormal2))
                        {
                            page8.cellBenzene2.ForeColor = (BenzeneAbnormal2 == "H") ? Color.Red : Color.Blue;
                            page8.cellBenzene2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(BenzeneAbnormal3))
                        {
                            page8.cellBenzene3.ForeColor = (BenzeneAbnormal3 == "H") ? Color.Red : Color.Blue;
                            page8.cellBenzene3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }
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

                        string MethanolAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116" && p.Year == year1)?.IsAbnormal;
                        string MethanolAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116" && p.Year == year2)?.IsAbnormal;
                        string MethanolAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116" && p.Year == year3)?.IsAbnormal;

                        if (!string.IsNullOrEmpty(MethanolAbnormal1))
                        {
                            page8.cellMethanol1.ForeColor = (MethanolAbnormal1 == "H") ? Color.Red : Color.Blue;
                            page8.cellMethanol1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(MethanolAbnormal2))
                        {
                            page8.cellMethanol2.ForeColor = (MethanolAbnormal2 == "H") ? Color.Red : Color.Blue;
                            page8.cellMethanol2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(MethanolAbnormal3))
                        {
                            page8.cellMethanol3.ForeColor = (MethanolAbnormal3 == "H") ? Color.Red : Color.Blue;
                            page8.cellMethanol3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }
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

                        string MethyreneAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119" && p.Year == year1)?.IsAbnormal;
                        string MethyreneAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119" && p.Year == year2)?.IsAbnormal;
                        string MethyreneAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119" && p.Year == year3)?.IsAbnormal;

                        if (!string.IsNullOrEmpty(MethyreneAbnormal1))
                        {
                            page8.cellMethyrene1.ForeColor = (MethyreneAbnormal1 == "H") ? Color.Red : Color.Blue;
                            page8.cellMethyrene1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(MethyreneAbnormal2))
                        {
                            page8.cellMethyrene2.ForeColor = (MethyreneAbnormal2 == "H") ? Color.Red : Color.Blue;
                            page8.cellMethyrene2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(MethyreneAbnormal3))
                        {
                            page8.cellMethyrene3.ForeColor = (MethyreneAbnormal3 == "H") ? Color.Red : Color.Blue;
                            page8.cellMethyrene3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }
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

                        string AcetoneAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117" && p.Year == year1)?.IsAbnormal;
                        string AcetoneAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117" && p.Year == year2)?.IsAbnormal;
                        string AcetoneAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117" && p.Year == year3)?.IsAbnormal;

                        if (!string.IsNullOrEmpty(AcetoneAbnormal1))
                        {
                            page8.cellAcetone1.ForeColor = (AcetoneAbnormal1 == "H") ? Color.Red : Color.Blue;
                            page8.cellAcetone1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(AcetoneAbnormal2))
                        {
                            page8.cellAcetone2.ForeColor = (AcetoneAbnormal2 == "H") ? Color.Red : Color.Blue;
                            page8.cellAcetone2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(AcetoneAbnormal3))
                        {
                            page8.cellAcetone3.ForeColor = (AcetoneAbnormal3 == "H") ? Color.Red : Color.Blue;
                            page8.cellAcetone3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }
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

                        string HexaneAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118" && p.Year == year1)?.IsAbnormal;
                        string HexaneAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118" && p.Year == year2)?.IsAbnormal;
                        string HexaneAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118" && p.Year == year3)?.IsAbnormal;

                        if (!string.IsNullOrEmpty(HexaneAbnormal1))
                        {
                            page8.cellHexane1.ForeColor = (HexaneAbnormal1 == "H") ? Color.Red : Color.Blue;
                            page8.cellHexane1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(HexaneAbnormal2))
                        {
                            page8.cellHexane2.ForeColor = (HexaneAbnormal2 == "H") ? Color.Red : Color.Blue;
                            page8.cellHexane2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(HexaneAbnormal3))
                        {
                            page8.cellHexane3.ForeColor = (HexaneAbnormal3 == "H") ? Color.Red : Color.Blue;
                            page8.cellHexane3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }
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

                        string IsopropanolAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year1)?.IsAbnormal;
                        string IsopropanolAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year2)?.IsAbnormal;
                        string IsopropanolAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year3)?.IsAbnormal;

                        if (!string.IsNullOrEmpty(IsopropanolAbnormal1))
                        {
                            page8.cellIsopropanol1.ForeColor = (IsopropanolAbnormal1 == "H") ? Color.Red : Color.Blue;
                            page8.cellIsopropanol1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(IsopropanolAbnormal2))
                        {
                            page8.cellIsopropanol2.ForeColor = (IsopropanolAbnormal2 == "H") ? Color.Red : Color.Blue;
                            page8.cellIsopropanol2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(IsopropanolAbnormal3))
                        {
                            page8.cellIsopropanol3.ForeColor = (IsopropanolAbnormal3 == "H") ? Color.Red : Color.Blue;
                            page8.cellIsopropanol3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }
                    }
                    #endregion

                    #region Chromium (Show all)
                    page8.cellChromiumRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132")?.ReferenceRange;
                    page8.cellChromium1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132" && p.Year == year1)?.ResultValue;
                    page8.cellChromium2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132" && p.Year == year2)?.ResultValue;
                    page8.cellChromium3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132" && p.Year == year3)?.ResultValue;

                    string ChromiumAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132" && p.Year == year1)?.IsAbnormal;
                    string ChromiumAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132" && p.Year == year2)?.IsAbnormal;
                    string ChromiumAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132" && p.Year == year3)?.IsAbnormal;

                    if (!string.IsNullOrEmpty(ChromiumAbnormal1))
                    {
                        page8.cellChromium1.ForeColor = (ChromiumAbnormal1 == "H") ? Color.Red : Color.Blue;
                        page8.cellChromium1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }

                    if (!string.IsNullOrEmpty(ChromiumAbnormal2))
                    {
                        page8.cellChromium2.ForeColor = (ChromiumAbnormal2 == "H") ? Color.Red : Color.Blue;
                        page8.cellChromium2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }

                    if (!string.IsNullOrEmpty(ChromiumAbnormal3))
                    {
                        page8.cellChromium3.ForeColor = (ChromiumAbnormal3 == "H") ? Color.Red : Color.Blue;
                        page8.cellChromium3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }
                    #endregion

                    #region Nickel (Show all)
                    page8.cellNickelRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131")?.ReferenceRange;
                    page8.cellNickel1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131" && p.Year == year1)?.ResultValue;
                    page8.cellNickel2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131" && p.Year == year2)?.ResultValue;
                    page8.cellNickel3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131" && p.Year == year3)?.ResultValue;

                    string NickelAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131" && p.Year == year1)?.IsAbnormal;
                    string NickelAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131" && p.Year == year2)?.IsAbnormal;
                    string NickelAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131" && p.Year == year3)?.IsAbnormal;

                    if (!string.IsNullOrEmpty(NickelAbnormal1))
                    {
                        page8.cellNickel1.ForeColor = (NickelAbnormal1 == "H") ? Color.Red : Color.Blue;
                        page8.cellNickel1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }

                    if (!string.IsNullOrEmpty(NickelAbnormal2))
                    {
                        page8.cellNickel2.ForeColor = (NickelAbnormal2 == "H") ? Color.Red : Color.Blue;
                        page8.cellNickel2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }

                    if (!string.IsNullOrEmpty(NickelAbnormal3))
                    {
                        page8.cellNickel3.ForeColor = (NickelAbnormal3 == "H") ? Color.Red : Color.Blue;
                        page8.cellNickel3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }
                    #endregion

                    #region Nickel In Urine

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130") != null)
                    {
                        page8.RowNickelUrine.Visible = true;
                        page8.cellNickelUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188")?.ReferenceRange;
                        page8.cellNickelUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188" && p.Year == year1)?.ResultValue;
                        page8.cellNickelUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188" && p.Year == year2)?.ResultValue;
                        page8.cellNickelUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188" && p.Year == year3)?.ResultValue;

                        string NickelUrineAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188" && p.Year == year1)?.IsAbnormal;
                        string NickelUrineAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188" && p.Year == year2)?.IsAbnormal;
                        string NickelUrineAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188" && p.Year == year3)?.IsAbnormal;

                        if (!string.IsNullOrEmpty(NickelUrineAbnormal1))
                        {
                            page8.cellNickelUrine1.ForeColor = (NickelUrineAbnormal1 == "H") ? Color.Red : Color.Blue;
                            page8.cellNickelUrine1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(NickelUrineAbnormal2))
                        {
                            page8.cellNickelUrine2.ForeColor = (NickelUrineAbnormal2 == "H") ? Color.Red : Color.Blue;
                            page8.cellNickelUrine2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        if (!string.IsNullOrEmpty(NickelUrineAbnormal3))
                        {
                            page8.cellNickelUrine3.ForeColor = (NickelUrineAbnormal3 == "H") ? Color.Red : Color.Blue;
                            page8.cellNickelUrine3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }
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
            else
            {

            }
        }

        private void GenerateOther(IEnumerable<PatientResultLabModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                PatientResultLabModel CheckGender = labTestSet.FirstOrDefault();
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;
                page7.cellOtherYear1.Text = "ปี" + " " + year1.ToString();
                page7.cellOtherYear2.Text = "ปี" + " " + year2.ToString();
                page7.cellOtherYear3.Text = "ปี" + " " + year3.ToString();

                page7.cellAfpRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38")?.ReferenceRange;
                page7.cellAfp1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38" && p.Year == year1)?.ResultValue;
                page7.cellAfp2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38" && p.Year == year2)?.ResultValue;
                page7.cellAfp3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38" && p.Year == year3)?.ResultValue;

                string afpAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38" && p.Year == year1)?.IsAbnormal;
                string afpAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38" && p.Year == year2)?.IsAbnormal;
                string afpAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(afpAbnormal1))
                {
                    page7.cellAfp1.ForeColor = (afpAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page7.cellAfp1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(afpAbnormal2))
                {
                    page7.cellAfp2.ForeColor = (afpAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page7.cellAfp2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(afpAbnormal3))
                {
                    page7.cellAfp3.ForeColor = (afpAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page7.cellAfp3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page7.cellAfpConRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39")?.ReferenceRange;
                page7.cellAfpCon1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year1)?.ResultValue;
                page7.cellAfpCon2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year2)?.ResultValue;
                page7.cellAfpCon3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year3)?.ResultValue;

                string AfpConAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year1)?.IsAbnormal;
                string AfpConAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year2)?.IsAbnormal;
                string AfpConAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(AfpConAbnormal1))
                {
                    page7.cellAfpCon1.ForeColor = (AfpConAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page7.cellAfpCon1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AfpConAbnormal2))
                {
                    page7.cellAfpCon2.ForeColor = (AfpConAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page7.cellAfpCon2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AfpConAbnormal3))
                {
                    page7.cellAfpCon3.ForeColor = (AfpConAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page7.cellAfpCon3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }


                page7.cellAFPInterRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186")?.ReferenceRange;
                page7.cellAfpInter1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186" && p.Year == year1)?.ResultValue;
                page7.cellAfpInter2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186" && p.Year == year2)?.ResultValue;
                page7.cellAfpInter3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186" && p.Year == year3)?.ResultValue;

                string AfpInerAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186" && p.Year == year1)?.IsAbnormal;
                string AfpInerAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186" && p.Year == year2)?.IsAbnormal;
                string AfpInerAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(AfpInerAbnormal1))
                {
                    page7.cellAfpInter1.ForeColor = (AfpInerAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page7.cellAfpInter1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AfpInerAbnormal2))
                {
                    page7.cellAfpInter2.ForeColor = (AfpInerAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page7.cellAfpInter2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AfpInerAbnormal3))
                {
                    page7.cellAfpInter3.ForeColor = (AfpInerAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page7.cellAfpInter3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page7.cellAboGroupRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32")?.ReferenceRange;
                page7.cellAboGroup1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32" && p.Year == year1)?.ResultValue;
                page7.cellAboGroup2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32" && p.Year == year2)?.ResultValue;
                page7.cellAboGroup3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32" && p.Year == year3)?.ResultValue;

                string AboGroupAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32" && p.Year == year1)?.IsAbnormal;
                string AboGroupAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32" && p.Year == year2)?.IsAbnormal;
                string AboGroupAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(AboGroupAbnormal1))
                {
                    page7.cellAboGroup1.ForeColor = (AboGroupAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page7.cellAboGroup1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AboGroupAbnormal2))
                {
                    page7.cellAboGroup2.ForeColor = (AboGroupAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page7.cellAboGroup2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AboGroupAbnormal3))
                {
                    page7.cellAboGroup3.ForeColor = (AboGroupAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page7.cellAboGroup3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page7.cellBloodGroupRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR43")?.ReferenceRange;
                page7.cellBloodGroup1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR43" && p.Year == year1)?.ResultValue;
                page7.cellBloodGroup2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR43" && p.Year == year2)?.ResultValue;
                page7.cellBloodGroup3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR43" && p.Year == year3)?.ResultValue;

                string BloodGroupAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR43" && p.Year == year1)?.IsAbnormal;
                string BloodGroupAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR43" && p.Year == year2)?.IsAbnormal;
                string BloodGroupAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR43" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(BloodGroupAbnormal1))
                {
                    page7.cellBloodGroup1.ForeColor = (BloodGroupAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page7.cellBloodGroup1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BloodGroupAbnormal2))
                {
                    page7.cellBloodGroup2.ForeColor = (BloodGroupAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page7.cellBloodGroup2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BloodGroupAbnormal3))
                {
                    page7.cellBloodGroup3.ForeColor = (BloodGroupAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page7.cellBloodGroup3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page7.cellCaRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114")?.ReferenceRange;
                page7.cellCa1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year1)?.ResultValue;
                page7.cellCa2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year2)?.ResultValue;
                page7.cellCa3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year3)?.ResultValue;

                string caAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year1)?.IsAbnormal;
                string caAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year2)?.IsAbnormal;
                string caAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(caAbnormal1))
                {
                    page7.cellCa1.ForeColor = (caAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page7.cellCa1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(caAbnormal2))
                {
                    page7.cellCa2.ForeColor = (caAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page7.cellCa2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(caAbnormal3))
                {
                    page7.cellCa3.ForeColor = (caAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page7.cellCa3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
                if (CheckGender.SEXXXUID == 1)
                {
                    page7.RowCa125.Visible = false;
                    page7.cellPsaRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187")?.ReferenceRange;
                    page7.cellPsa1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187" && p.Year == year1)?.ResultValue;
                    page7.cellPsa2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187" && p.Year == year2)?.ResultValue;
                    page7.cellPsa3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187" && p.Year == year3)?.ResultValue;

                    string psaAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187" && p.Year == year1)?.IsAbnormal;
                    string psaAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187" && p.Year == year2)?.IsAbnormal;
                    string psaAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAPAR187R4" && p.Year == year3)?.IsAbnormal;

                    if (!string.IsNullOrEmpty(psaAbnormal1))
                    {
                        page7.cellPsa1.ForeColor = (psaAbnormal1 == "H") ? Color.Red : Color.Blue;
                        page7.cellPsa1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }

                    if (!string.IsNullOrEmpty(psaAbnormal2))
                    {
                        page7.cellPsa2.ForeColor = (psaAbnormal2 == "H") ? Color.Red : Color.Blue;
                        page7.cellPsa2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }

                    if (!string.IsNullOrEmpty(psaAbnormal3))
                    {
                        page7.cellPsa3.ForeColor = (psaAbnormal3 == "H") ? Color.Red : Color.Blue;
                        page7.cellPsa3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }
                }

                if (CheckGender.SEXXXUID == 2)
                {
                    page7.RowPSA.Visible = false;
                    page7.cellCa125Range.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41")?.ReferenceRange;
                    page7.cellCa125_1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41" && p.Year == year1)?.ResultValue;
                    page7.cellCa125_2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41" && p.Year == year2)?.ResultValue;
                    page7.cellCa125_3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41" && p.Year == year3)?.ResultValue;

                    string Ca125Abnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41" && p.Year == year1)?.IsAbnormal;
                    string Ca125Abnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41" && p.Year == year2)?.IsAbnormal;
                    string Ca125Abnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41" && p.Year == year3)?.IsAbnormal;

                    if (!string.IsNullOrEmpty(Ca125Abnormal1))
                    {
                        page7.cellCa125_1.ForeColor = (Ca125Abnormal1 == "H") ? Color.Red : Color.Blue;
                        page7.cellCa125_1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }

                    if (!string.IsNullOrEmpty(Ca125Abnormal2))
                    {
                        page7.cellCa125_2.ForeColor = (Ca125Abnormal2 == "H") ? Color.Red : Color.Blue;
                        page7.cellCa125_2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }

                    if (!string.IsNullOrEmpty(Ca125Abnormal3))
                    {
                        page7.cellCa125_3.ForeColor = (Ca125Abnormal3 == "H") ? Color.Red : Color.Blue;
                        page7.cellCa125_3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }
                }


                page7.cellHbA1cRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR7")?.ReferenceRange;
                page7.cellHbA1c1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR7" && p.Year == year1)?.ResultValue;
                page7.cellHbA1c2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR7" && p.Year == year2)?.ResultValue;
                page7.cellHbA1c3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR7" && p.Year == year3)?.ResultValue;

                string HbA1cAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR7" && p.Year == year1)?.IsAbnormal;
                string HbA1cAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR7" && p.Year == year2)?.IsAbnormal;
                string HbA1cAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR7" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(HbA1cAbnormal1))
                {
                    page7.cellHbA1c1.ForeColor = (HbA1cAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page7.cellHbA1c1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(HbA1cAbnormal2))
                {
                    page7.cellHbA1c2.ForeColor = (HbA1cAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page7.cellHbA1c2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(HbA1cAbnormal3))
                {
                    page7.cellHbA1c3.ForeColor = (HbA1cAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page7.cellHbA1c3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page7.cellCalciumRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR79")?.ReferenceRange;
                page7.cellCalcium1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR79" && p.Year == year1)?.ResultValue;
                page7.cellCalcium2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR79" && p.Year == year2)?.ResultValue;
                page7.cellCalcium3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR79" && p.Year == year3)?.ResultValue;

                string CalciumAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR79" && p.Year == year1)?.IsAbnormal;
                string CalciumAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR79" && p.Year == year2)?.IsAbnormal;
                string CalciumAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR79" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(CalciumAbnormal1))
                {
                    page7.cellCalcium1.ForeColor = (CalciumAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page7.cellCalcium1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CalciumAbnormal2))
                {
                    page7.cellCalcium2.ForeColor = (CalciumAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page7.cellCalcium2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CalciumAbnormal3))
                {
                    page7.cellCalcium3.ForeColor = (CalciumAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page7.cellCalcium3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
            }
            else
            {
                page7.cellOtherYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page7.cellOtherYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page7.cellOtherYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
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
            string thairesult = TranslateResult.TranslateResultXray(resultValue, resultStatus, requestItemName, ",", dtResultMapping, ref listNoMapResult);

            return thairesult;
        }

    }
}
