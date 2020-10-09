using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Model;
using MediTech.DataService;
using System.Collections.Generic;
using System.Linq;
using MediTech.Helpers;

namespace MediTech.Reports.Operating.Patient.CheckupBookReport
{
    public partial class CheckupBook1 : DevExpress.XtraReports.UI.XtraReport
    {
        private MediTechDataService _DataService;

        public MediTechDataService DataService
        {
            get { return _DataService ?? (_DataService = new MediTechDataService()); }
        }

        List<XrayTranslateMappingModel> dtResultMapping;

        CheckupBook2 page2 = new CheckupBook2();
        CheckupBook3 page3 = new CheckupBook3();
        CheckupBook4 page4 = new CheckupBook4();
        CheckupBook5 page5 = new CheckupBook5();
        CheckupBook6 page6 = new CheckupBook6();

        public CheckupBook1()
        {
            InitializeComponent();
            BeforePrint += BookPage1_BeforePrint;
            AfterPrint += BookPage1_AfterPrint;
        }

        private void BookPage1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            long patientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long patientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            int payorDetailUID = int.Parse(this.Parameters["PayorDetailUID"].Value.ToString());
            List<CheckupBookModel> data = DataService.Reports.PrintCheckupBook(patientUID, patientVisitUID);
            
            if (data != null && data.Count > 0)
            {
                #region Show HN/Name
                page2.lbHN2.Text = data.FirstOrDefault().PatientID;
                page2.lbName2.Text = data.FirstOrDefault().PatientName;
                page2.lbHN11.Text = data.FirstOrDefault().PatientID;
                page2.lbName11.Text = data.FirstOrDefault().PatientName;

                page3.lbHN3.Text = data.FirstOrDefault().PatientID;
                page3.lbName3.Text = data.FirstOrDefault().PatientName;
                page3.lbHN10.Text = data.FirstOrDefault().PatientID;
                page3.lbName10.Text = data.FirstOrDefault().PatientName;

                page4.lbHN4.Text = data.FirstOrDefault().PatientID;
                page4.lbName4.Text = data.FirstOrDefault().PatientName;
                page4.lbHN9.Text = data.FirstOrDefault().PatientID;
                page4.lbName9.Text = data.FirstOrDefault().PatientName;

                page5.lbHN5.Text = data.FirstOrDefault().PatientID;
                page5.lbName5.Text = data.FirstOrDefault().PatientName;
                page5.lbHN8.Text = data.FirstOrDefault().PatientID;
                page5.lbName8.Text = data.FirstOrDefault().PatientName;

                page6.lbHN6.Text = data.FirstOrDefault().PatientID;
                page6.lbName6.Text = data.FirstOrDefault().PatientName;
                page6.lbHN7.Text = data.FirstOrDefault().PatientID;
                page6.lbName7.Text = data.FirstOrDefault().PatientName;

                lbHN12.Text = data.FirstOrDefault().PatientID;
                lbName12.Text = data.FirstOrDefault().PatientName;
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
                lbFarVision.Text = data.FirstOrDefault().FarPoint != null ? data.FirstOrDefault().FarPoint.ToString() : "";
                if (lbFarVision.Text != "" && lbFarVision.Text != "ปกติ")
                {
                    lbFarVision.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
                lbNearVision.Text = data.FirstOrDefault().NearPoint != null ? data.FirstOrDefault().NearPoint.ToString() : "";
                if (lbNearVision.Text != "" && lbNearVision.Text != "ปกติ")
                {
                    lbNearVision.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
                lb3DVision.Text = data.FirstOrDefault().Depth != null ? data.FirstOrDefault().Depth.ToString() : "";
                if (lb3DVision.Text != "" && lb3DVision.Text != "ปกติ")
                {
                    lb3DVision.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
                lbBalanceEye.Text = data.FirstOrDefault().Muscle != null ? data.FirstOrDefault().Muscle.ToString() : "";
                if (lbBalanceEye.Text != "" && lbBalanceEye.Text != "ปกติ")
                {
                    lbBalanceEye.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
                lbVisionColor.Text = data.FirstOrDefault().Color != null ? data.FirstOrDefault().Color.ToString() : "";
                if (lbVisionColor.Text != "" && lbVisionColor.Text != "ปกติ")
                {
                    lbVisionColor.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
                lbFieldVision.Text = data.FirstOrDefault().Visualfield != null ? data.FirstOrDefault().Visualfield.ToString() : "";
                if (lbFieldVision.Text != "" && lbFieldVision.Text != "ปกติ")
                {
                    lbFieldVision.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
                lbVisionOccmedResult.Text = data.FirstOrDefault().TitmusConclusion != null ? data.FirstOrDefault().TitmusConclusion.ToString() : "";
                if (lbVisionOccmedResult.Text != null && lbVisionOccmedResult.Text.Length > 120)
                {
                    lbVisionOccmedResult.Font = new Font("Angsana New", 9);
                }
                lbVisionOccmedRecommend.Text = data.FirstOrDefault().TitmusRecommend != null ? data.FirstOrDefault().TitmusRecommend.ToString() : "";
                if (lbVisionOccmedRecommend.Text != null && lbVisionOccmedRecommend.Text.Length > 120)
                {
                    lbVisionOccmedRecommend.Font = new Font("Angsana New", 9);
                }

                #endregion

                #region Result Wellness
                page2.lbResultWellness.Text = data.FirstOrDefault().WellnessResult;
                if (page2.lbResultWellness.Text != null && page2.lbResultWellness.Text.Length > 2000)
                {
                    page2.lbResultWellness.Font = new Font("Angsana New", 8);
                }
                else if (page2.lbResultWellness.Text != null && page2.lbResultWellness.Text.Length > 1670)
                {
                    page2.lbResultWellness.Font = new Font("Angsana New", 9);
                }
                else if(page2.lbResultWellness.Text != null && page2.lbResultWellness.Text.Length > 1350)
                {
                    page2.lbResultWellness.Font = new Font("Angsana New", 10);
                }

                #endregion

                #region Lung Funtion 
                lbFVCMeasure.Text = data.FirstOrDefault().FVC != null ? data.FirstOrDefault().FVC.ToString() : "";
                lbFVCPredic.Text = data.FirstOrDefault().FVCPred != null ? data.FirstOrDefault().FVCPred.ToString() : "";
                lbFVCPer.Text = data.FirstOrDefault().FVCPer != null ? data.FirstOrDefault().FVCPer.ToString() + " %" : "";
                lbFEV1Measure.Text = data.FirstOrDefault().FEV1 != null ? data.FirstOrDefault().FEV1.ToString() : "";
                lbFEV1Predic.Text = data.FirstOrDefault().FEV1Pred != null ? data.FirstOrDefault().FEV1Pred.ToString() : "";
                lbFEV1Per.Text = data.FirstOrDefault().FEV1Per != null ? data.FirstOrDefault().FEV1Per.ToString() + " %" : "";
                lbFFVMeasure.Text = data.FirstOrDefault().FEV1FVC != null ? data.FirstOrDefault().FEV1FVC.ToString() + " %" : "";
                lbFFVPredic.Text = data.FirstOrDefault().FEV1FVCPred != null ? data.FirstOrDefault().FEV1FVCPred.ToString() + " %" : "";
                lbFFVPer.Text = data.FirstOrDefault().FEV1FVCPer != null ? data.FirstOrDefault().FEV1FVCPer.ToString() + " %" : "";
                lbLungResult.Text = data.FirstOrDefault().SpiroResult != null ? data.FirstOrDefault().SpiroResult.ToString() : "";
                lbLungRecommend.Text = data.FirstOrDefault().SpiroRecommend != null ? data.FirstOrDefault().SpiroRecommend.ToString() : "";

                #endregion

                #region Audio Test
                page2.lbAudioRight.Text = data.FirstOrDefault().AudioRightResult != null ? data.FirstOrDefault().AudioRightResult.ToString() : "";
                if (page2.lbAudioRight.Text != null && page2.lbAudioRight.Text.Length > 120)
                {
                    page2.lbAudioRight.Font = new Font("Angsana New", 9);
                }
                page2.lbAudioLeft.Text = data.FirstOrDefault().AudioLeftResult != null ? data.FirstOrDefault().AudioLeftResult.ToString() : "";
                if (page2.lbAudioLeft.Text != null && page2.lbAudioLeft.Text.Length > 120)
                {
                    page2.lbAudioLeft.Font = new Font("Angsana New", 9);
                }
                page2.lbAudioResult.Text = data.FirstOrDefault().AudioResult != null ? data.FirstOrDefault().AudioResult.ToString() : "";
                if (page2.lbAudioResult.Text != "" && page2.lbAudioResult.Text != "ปกติ")
                {
                    page2.lbAudioResult.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
                page2.lbAudioRecommend.Text = data.FirstOrDefault().AudioRecommend != null ? data.FirstOrDefault().AudioRecommend.ToString() : "";
                if (page2.lbAudioRecommend.Text != null && page2.lbAudioRecommend.Text.Length > 120)
                {
                    page2.lbAudioRecommend.Font = new Font("Angsana New", 9);
                }

                #endregion

                #region EKG
                page3.lbEKGRecommend.Text = data.FirstOrDefault().EkgConclusion != null ? data.FirstOrDefault().EkgConclusion.ToString() : "";

                #endregion

                #region Vision
                page2.lbAstigmaticRight.Text = data.FirstOrDefault().AstigmaticRight != null ? data.FirstOrDefault().AstigmaticRight.ToString() : "";
                page2.lbAstigmaticLeft.Text = data.FirstOrDefault().AstigmaticLeft != null ? data.FirstOrDefault().AstigmaticLeft.ToString() : "";
                page2.lbMyopiaRight.Text = data.FirstOrDefault().MyopiaRight != null ? data.FirstOrDefault().MyopiaRight.ToString() : "";
                page2.lbMyopiaLeft.Text = data.FirstOrDefault().MyopiaLeft != null ? data.FirstOrDefault().MyopiaLeft.ToString() : "";
                page2.lbViewRight.Text = data.FirstOrDefault().ViewRight != null ? data.FirstOrDefault().ViewRight.ToString() : "";
                page2.lbViewLeft.Text = data.FirstOrDefault().ViewLeft != null ? data.FirstOrDefault().ViewLeft.ToString() : "";
                page2.lbHyperopiaRight.Text = data.FirstOrDefault().HyperopiaRight != null ? data.FirstOrDefault().HyperopiaRight.ToString() : "";
                page2.lbHyperopiaLeft.Text = data.FirstOrDefault().HyperopiaLeft != null ? data.FirstOrDefault().HyperopiaLeft.ToString() : "";
                page2.lbVARight.Text = data.FirstOrDefault().VARight != null ? data.FirstOrDefault().VARight.ToString() : "";
                page2.lbVALeft.Text = data.FirstOrDefault().VALeft != null ? data.FirstOrDefault().VALeft.ToString() : "";
                page2.lbDisease.Text = data.FirstOrDefault().EyeDiseas != null ? data.FirstOrDefault().EyeDiseas.ToString() : "";
                page2.lbBlindColor.Text = data.FirstOrDefault().BlindColor != null ? data.FirstOrDefault().BlindColor.ToString() : "";
                page2.lbViewResult.Text = data.FirstOrDefault().ViewResult != null ? data.FirstOrDefault().ViewResult.ToString() : "";
                page2.lbViewRecommend.Text = data.FirstOrDefault().ViewRecommend != null ? data.FirstOrDefault().ViewRecommend.ToString() : "";

                #endregion

                #region Physical Exam
                page3.lbEye.Text = data.FirstOrDefault().Eyes != null ? data.FirstOrDefault().Eyes.ToString() : "";
                page3.lbEars.Text = data.FirstOrDefault().Ears != null ? data.FirstOrDefault().Ears.ToString() : "";
                page3.lbThroat.Text = data.FirstOrDefault().Throat != null ? data.FirstOrDefault().Throat.ToString() : "";
                page3.lbNose.Text = data.FirstOrDefault().Nose != null ? data.FirstOrDefault().Nose.ToString() : "";
                page3.lbTeeth.Text = data.FirstOrDefault().Teeth != null ? data.FirstOrDefault().Teeth.ToString() : "";
                page3.lbLung.Text = data.FirstOrDefault().Lung != null ? data.FirstOrDefault().Lung.ToString() : "";
                page3.lbHeart.Text = data.FirstOrDefault().Heart != null ? data.FirstOrDefault().Heart.ToString() : "";
                page3.lbSkin.Text = data.FirstOrDefault().Skin != null ? data.FirstOrDefault().Skin.ToString() : "";
                page3.lbThyroid.Text = data.FirstOrDefault().Thyroid != null ? data.FirstOrDefault().Thyroid.ToString() : "";
                page3.lbLymphNode.Text = data.FirstOrDefault().LymphNode != null ? data.FirstOrDefault().LymphNode.ToString() : "";
                page3.lbSmoke.Text = data.FirstOrDefault().Smoke != null ? data.FirstOrDefault().Smoke.ToString() : "";
                page3.lbDrugAllergy.Text = data.FirstOrDefault().DrugAllergy != null ? data.FirstOrDefault().DrugAllergy.ToString() : "";
                page3.lbAlcohol.Text = data.FirstOrDefault().Alcohol != null ? data.FirstOrDefault().Alcohol.ToString() : "";
                page3.lbUnderlying.Text = data.FirstOrDefault().PersonalHistory != null ? data.FirstOrDefault().PersonalHistory.ToString() : "";

                #endregion

                #region Radiology
                if (data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("chest") && p.RequestItemType == "Radiology") != null)
                {
                    CheckupBookModel chestXray = data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("chest") && p.RequestItemType == "Radiology");
                    if (!string.IsNullOrEmpty(chestXray.RadiologyResultText))
                    {
                        string resultChestThai = TranslateXray(chestXray.RadiologyResultText, chestXray.RadiologyResultStatus, chestXray.RequestItemName);
                        if (!string.IsNullOrEmpty(resultChestThai))
                        {
                            page3.lbChest.Text = resultChestThai;
                        }
                        else
                        {
                            page3.lbChest.Text = "ยังไม่ได้แปลไทย";
                        }
                    }

                }
                else
                {
                    page3.lbChest.Text = "-";
                }

                if (data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("mammo") && p.RequestItemType == "Radiology") != null)
                {
                    CheckupBookModel mammoGram = data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("mammo") && p.RequestItemType == "Radiology");
                    page3.lbMam.Text = mammoGram.RadiologyResultStatus;
                    if (!string.IsNullOrEmpty(mammoGram.RadiologyResultText))
                    {
                        string resultChestThai = TranslateXray(mammoGram.RadiologyResultText, mammoGram.RadiologyResultStatus, mammoGram.RequestItemName);
                        if (!string.IsNullOrEmpty(resultChestThai))
                        {
                            page3.lbMam.Text = resultChestThai;
                        }
                        else
                        {
                            page3.lbMam.Text = "ยังไม่ได้แปลไทย";
                        }
                    }
                }
                else
                {
                    page3.lbMam.Text = "-";
                }

                if (data.FirstOrDefault(p => (p.RequestItemName.ToLower().Contains("ultrasound") || p.RequestItemName.ToLower().Contains("US")) && p.RequestItemType == "Radiology") != null)
                {
                    CheckupBookModel ultrsound = data.FirstOrDefault(p => (p.RequestItemName.ToLower().Contains("ultrasound") || p.RequestItemName.ToLower().Contains("US")) && p.RequestItemType == "Radiology");
                    page3.lbUlt.Text = ultrsound.RadiologyResultStatus;
                    if (!string.IsNullOrEmpty(ultrsound.RadiologyResultText))
                    {
                        string resultChestThai = TranslateXray(ultrsound.RadiologyResultText, ultrsound.RadiologyResultStatus, ultrsound.RequestItemName);
                        if (!string.IsNullOrEmpty(resultChestThai))
                        {
                            page3.lbUlt.Text = resultChestThai;
                        }
                        else
                        {
                            page3.lbUlt.Text = "ยังไม่ได้แปลไทย";
                        }
                    }
                }
                else
                {
                    page3.lbUlt.Text = "-";
                }

                #endregion

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
                        .Where(p => p.RequestItemName.Contains("Cr") || p.RequestItemName.Contains("BUN"))
                        .OrderBy(p => p.Year);
                    GenerateRenalFunction(RenalTestSet);

                    #endregion

                    #region Fasting Blood Sugar
                    IEnumerable<PatientResultLabModel> FbsTestSet = labCompare
                        .Where(p => p.RequestItemName.Contains("FBS"))
                        .OrderBy(p => p.Year);
                    GenerateFastingBloodSugar(FbsTestSet);

                    #endregion

                    #region Uric acid
                    IEnumerable<PatientResultLabModel> UricTestSet = labCompare
                        .Where(p => p.RequestItemName.Contains("Uric acid"))
                        .OrderBy(p => p.Year);
                    GenerateUricAcid(UricTestSet);

                    #endregion

                    #region Lipid Profiles 
                    IEnumerable<PatientResultLabModel> LipidTestSet = labCompare
                        .Where(p => p.RequestItemName.Contains("CHOL")
                        || p.RequestItemName.Contains("TG")
                        || p.RequestItemName.Contains("HDL-Cholesterol")
                        || p.RequestItemName.Contains("LDL-Cholesterol"))
                        .OrderBy(p => p.Year);
                    GenerateLipidProfiles(LipidTestSet);

                    #endregion

                    #region Liver Function
                    IEnumerable<PatientResultLabModel> LiverTestSet = labCompare
                        .Where(p => p.RequestItemName.Contains("AST (SGOT)")
                        || p.RequestItemName.Contains("ALT (SGPT)")
                        || p.RequestItemName.Contains("ALP")
                        || p.RequestItemName.Contains("Total Billirubin")
                        || p.RequestItemName.Contains("Direct Billirubin")
                        || p.RequestItemName.Contains("Total Protein in blood")
                        || p.RequestItemName.Contains("Alb")
                        || p.RequestItemName.Contains("Glob"))
                        .OrderBy(p => p.Year);
                    GenerateLiverFunction(LiverTestSet);
                    #endregion

                    #region Immunology and Virology
                    IEnumerable<PatientResultLabModel> ImmunologyTestSet = labCompare
                        .Where(p => p.RequestItemName.Contains("Anti HBs") || p.RequestItemName.Contains("HBsAg"))
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
                        .Where(p => p.RequestItemName.Contains("Aluminium in Urine") 
                        || p.RequestItemName.Contains("Toluene (Urine)")
                        || p.RequestItemName.Contains("Xylene in Urine")
                        || p.RequestItemName.Contains("Lead (Blood)")
                        || p.RequestItemName.Contains("Carboxyhemoglobin in Blood")
                        || p.RequestItemName.Contains("Methyl Ethyl Ketone(MEK) Urine")
                        || p.RequestItemName.Contains("Benzene (Urine)")
                        || p.RequestItemName.Contains("Methanol (Urine)")
                        || p.RequestItemName.Contains("Methyrene chloride in Blood")
                        || p.RequestItemName.Contains("Acetone in Urine")
                        || p.RequestItemName.Contains("Hexane in Urine")
                        || p.RequestItemName.Contains("Isopropyl in Urine"))
                        .OrderBy(p => p.Year);
                    GenerateToxicology(ToxicoTestSet);
                    #endregion

                    #region Other Lab Teat
                    IEnumerable<PatientResultLabModel> OtherTestSet = labCompare
                        .Where(p => p.RequestItemName.Contains("AFP")
                        || p.RequestItemName.Contains("Blood Group ABO")
                        || p.RequestItemName.Contains("CA 19-9")
                        || p.RequestItemName.Contains("PSA"))
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
                page4.cellHbRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001")?.ReferenceRange;
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;
                page4.cellCBCYear1.Text = "ปี" + " " + year1.ToString();
                page4.cellCBCYear2.Text = "ปี" + " " + year2.ToString();
                page4.cellCBCYear3.Text = "ปี" + " " + year3.ToString();
                
                page4.cellHb1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001" && p.Year == year1)?.ResultValue;
                page4.cellHb2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001" && p.Year == year2)?.ResultValue;
                page4.cellHb3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001" && p.Year == year3)?.ResultValue;

                string hbAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001" && p.Year == year1)?.IsAbnormal;
                string hbAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001" && p.Year == year2)?.IsAbnormal;
                string hbAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001" && p.Year == year3)?.IsAbnormal;
                

                if (!string.IsNullOrEmpty(hbAbnormal1))
                {
                    page4.cellHb1.ForeColor = (hbAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellHb1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(hbAbnormal2))
                {
                    page4.cellHb2.ForeColor = (hbAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellHb2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(hbAbnormal3))
                {
                    page4.cellHb3.ForeColor = (hbAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellHb3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellHctRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020")?.ReferenceRange;
                page4.cellHct1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020" && p.Year == year1)?.ResultValue;
                page4.cellHct2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020" && p.Year == year2)?.ResultValue;
                page4.cellHct3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020" && p.Year == year3)?.ResultValue;

                string hctAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020" && p.Year == year1)?.IsAbnormal;
                string hctAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020" && p.Year == year2)?.IsAbnormal;
                string hctAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(hctAbnormal1))
                {
                    page4.cellHct1.ForeColor = (hctAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellHct1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(hctAbnormal2))
                {
                    page4.cellHct2.ForeColor = (hctAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellHct2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(hctAbnormal3))
                {
                    page4.cellHct3.ForeColor = (hctAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellHct3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }


                page4.cellMcvRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025")?.ReferenceRange;
                page4.cellMcv1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025" && p.Year == year1)?.ResultValue;
                page4.cellMcv2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025" && p.Year == year2)?.ResultValue;
                page4.cellMcv3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025" && p.Year == year3)?.ResultValue;

                string mcvAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025" && p.Year == year1)?.IsAbnormal;
                string mcvAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025" && p.Year == year2)?.IsAbnormal;
                string mcvAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(mcvAbnormal1))
                {
                    page4.cellMcv1.ForeColor = (mcvAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellMcv1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(mcvAbnormal2))
                {
                    page4.cellMcv2.ForeColor = (mcvAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellMcv2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(mcvAbnormal3))
                {
                    page4.cellMcv3.ForeColor = (mcvAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellMcv3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellMchRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030")?.ReferenceRange;
                page4.cellMch1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030" && p.Year == year1)?.ResultValue;
                page4.cellMch2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030" && p.Year == year2)?.ResultValue;
                page4.cellMch3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030" && p.Year == year3)?.ResultValue;

                string mchAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030" && p.Year == year1)?.IsAbnormal;
                string mchAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030" && p.Year == year2)?.IsAbnormal;
                string mchAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(mchAbnormal1))
                {
                    page4.cellMch1.ForeColor = (mchAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellMch1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(mchAbnormal2))
                {
                    page4.cellMch2.ForeColor = (mchAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellMch2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(mchAbnormal3))
                {
                    page4.cellMch3.ForeColor = (mchAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellMch3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellMchcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035")?.ReferenceRange;
                page4.cellMchc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035" && p.Year == year1)?.ResultValue;
                page4.cellMchc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035" && p.Year == year2)?.ResultValue;
                page4.cellMchc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035" && p.Year == year3)?.ResultValue;

                string mchcAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035" && p.Year == year1)?.IsAbnormal;
                string mchcAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035" && p.Year == year2)?.IsAbnormal;
                string mchcAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(mchcAbnormal1))
                {
                    page4.cellMchc1.ForeColor = (mchcAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellMchc1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(mchcAbnormal2))
                {
                    page4.cellMchc2.ForeColor = (mchcAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellMchc2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(mchcAbnormal3))
                {
                    page4.cellMchc3.ForeColor = (mchcAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellMchc3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellRdwRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1")?.ReferenceRange;
                page4.cellRdw1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1" && p.Year == year1)?.ResultValue;
                page4.cellRdw2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1" && p.Year == year2)?.ResultValue;
                page4.cellRdw3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1" && p.Year == year3)?.ResultValue;

                string rdwAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1" && p.Year == year1)?.IsAbnormal;
                string rdwAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1" && p.Year == year2)?.IsAbnormal;
                string rdwAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(rdwAbnormal1))
                {
                    page4.cellRdw1.ForeColor = (rdwAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellRdw1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(rdwAbnormal2))
                {
                    page4.cellRdw2.ForeColor = (rdwAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellRdw2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(rdwAbnormal3))
                {
                    page4.cellRdw3.ForeColor = (rdwAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellRdw3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellRbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428")?.ReferenceRange;
                page4.cellRbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428" && p.Year == year1)?.ResultValue;
                page4.cellRbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428" && p.Year == year2)?.ResultValue;
                page4.cellRbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428" && p.Year == year3)?.ResultValue;

                string rbcAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428" && p.Year == year1)?.IsAbnormal;
                string rbcAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428" && p.Year == year2)?.IsAbnormal;
                string rbcAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(rbcAbnormal1))
                {
                    page4.cellRbc1.ForeColor = (rbcAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellRbc1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(rbcAbnormal2))
                {
                    page4.cellRbc2.ForeColor = (rbcAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellRbc2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(rbcAbnormal3))
                {
                    page4.cellRbc3.ForeColor = (rbcAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellRbc3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellRbcMorRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13")?.ReferenceRange;
                page4.cellRbcMor1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13" && p.Year == year1)?.ResultValue;
                page4.cellRbcMor2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13" && p.Year == year2)?.ResultValue;
                page4.cellRbcMor3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13" && p.Year == year3)?.ResultValue;

                string RbcMorAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13" && p.Year == year1)?.IsAbnormal;
                string RbcMorAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13" && p.Year == year2)?.IsAbnormal;
                string RbcMorAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(RbcMorAbnormal1))
                {
                    page4.cellRbcMor1.ForeColor = (RbcMorAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellRbcMor1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(RbcMorAbnormal2))
                {
                    page4.cellRbcMor2.ForeColor = (RbcMorAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellRbcMor2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(RbcMorAbnormal3))
                {
                    page4.cellRbcMor3.ForeColor = (RbcMorAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellRbcMor3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellWbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006")?.ReferenceRange;
                page4.cellWbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006" && p.Year == year1)?.ResultValue;
                page4.cellWbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006" && p.Year == year2)?.ResultValue;
                page4.cellWbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006" && p.Year == year3)?.ResultValue;

                string wbcAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006" && p.Year == year1)?.IsAbnormal;
                string wbcAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006" && p.Year == year2)?.IsAbnormal;
                string wbcAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(wbcAbnormal1))
                {
                    page4.cellWbc1.ForeColor = (wbcAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellWbc1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(wbcAbnormal2))
                {
                    page4.cellWbc2.ForeColor = (wbcAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellWbc2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(wbcAbnormal3))
                {
                    page4.cellWbc3.ForeColor = (wbcAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellWbc3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellNectophilRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040")?.ReferenceRange;
                page4.cellNectophil1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040" && p.Year == year1)?.ResultValue;
                page4.cellNectophil2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040" && p.Year == year2)?.ResultValue;
                page4.cellNectophil3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040" && p.Year == year3)?.ResultValue;

                string NectophilAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040" && p.Year == year1)?.IsAbnormal;
                string NectophilAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040" && p.Year == year2)?.IsAbnormal;
                string NectophilAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(NectophilAbnormal1))
                {
                    page4.cellNectophil1.ForeColor = (NectophilAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellNectophil1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(NectophilAbnormal2))
                {
                    page4.cellNectophil2.ForeColor = (NectophilAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellNectophil2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(NectophilAbnormal3))
                {
                    page4.cellNectophil3.ForeColor = (NectophilAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellNectophil3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellLymphocyteRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050")?.ReferenceRange;
                page4.cellLymphocyte1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050" && p.Year == year1)?.ResultValue;
                page4.cellLymphocyte2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050" && p.Year == year2)?.ResultValue;
                page4.cellLymphocyte3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050" && p.Year == year3)?.ResultValue;

                string LymphocyteAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050" && p.Year == year1)?.IsAbnormal;
                string LymphocyteAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050" && p.Year == year2)?.IsAbnormal;
                string LymphocyteAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(LymphocyteAbnormal1))
                {
                    page4.cellLymphocyte1.ForeColor = (LymphocyteAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellLymphocyte1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(LymphocyteAbnormal2))
                {
                    page4.cellLymphocyte2.ForeColor = (LymphocyteAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellLymphocyte2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(LymphocyteAbnormal3))
                {
                    page4.cellLymphocyte3.ForeColor = (LymphocyteAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellLymphocyte3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellMonocyteRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060")?.ReferenceRange;
                page4.cellMonocyte1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060" && p.Year == year1)?.ResultValue;
                page4.cellMonocyte2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060" && p.Year == year2)?.ResultValue;
                page4.cellMonocyte3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060" && p.Year == year3)?.ResultValue;

                string MonocyteAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060" && p.Year == year1)?.IsAbnormal;
                string MonocyteAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060" && p.Year == year2)?.IsAbnormal;
                string MonocyteAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(MonocyteAbnormal1))
                {
                    page4.cellMonocyte1.ForeColor = (MonocyteAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellMonocyte1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(MonocyteAbnormal2))
                {
                    page4.cellMonocyte2.ForeColor = (MonocyteAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellMonocyte2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(MonocyteAbnormal3))
                {
                    page4.cellMonocyte3.ForeColor = (MonocyteAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellMonocyte3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellEosinophilRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070")?.ReferenceRange;
                page4.cellEosinophil1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070" && p.Year == year1)?.ResultValue;
                page4.cellEosinophil2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070" && p.Year == year2)?.ResultValue;
                page4.cellEosinophil3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070" && p.Year == year3)?.ResultValue;

                string EosinophilAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070" && p.Year == year1)?.IsAbnormal;
                string EosinophilAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070" && p.Year == year2)?.IsAbnormal;
                string EosinophilAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(EosinophilAbnormal1))
                {
                    page4.cellEosinophil1.ForeColor = (EosinophilAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellEosinophil1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(EosinophilAbnormal2))
                {
                    page4.cellEosinophil2.ForeColor = (EosinophilAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellEosinophil2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(EosinophilAbnormal3))
                {
                    page4.cellEosinophil3.ForeColor = (EosinophilAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellEosinophil3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellBasophilRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080")?.ReferenceRange;
                page4.cellBasophil1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080" && p.Year == year1)?.ResultValue;
                page4.cellBasophil2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080" && p.Year == year2)?.ResultValue;
                page4.cellBasophil3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080" && p.Year == year3)?.ResultValue;

                string BasophilAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080" && p.Year == year1)?.IsAbnormal;
                string BasophilAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080" && p.Year == year2)?.IsAbnormal;
                string BasophilAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(BasophilAbnormal1))
                {
                    page4.cellBasophil1.ForeColor = (BasophilAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellBasophil1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BasophilAbnormal2))
                {
                    page4.cellBasophil2.ForeColor = (BasophilAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellBasophil2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BasophilAbnormal3))
                {
                    page4.cellBasophil3.ForeColor = (BasophilAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellBasophil3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellPlateletSmearRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427")?.ReferenceRange;
                page4.cellPlateletSmear1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427" && p.Year == year1)?.ResultValue;
                page4.cellPlateletSmear2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427" && p.Year == year2)?.ResultValue;
                page4.cellPlateletSmear3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427" && p.Year == year3)?.ResultValue;

                string PlateletSmearAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427" && p.Year == year1)?.IsAbnormal;
                string PlateletSmearAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427" && p.Year == year2)?.IsAbnormal;
                string PlateletSmearAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(PlateletSmearAbnormal1))
                {
                    page4.cellPlateletSmear1.ForeColor = (PlateletSmearAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellPlateletSmear1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(PlateletSmearAbnormal2))
                {
                    page4.cellPlateletSmear2.ForeColor = (PlateletSmearAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellPlateletSmear2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(PlateletSmearAbnormal3))
                {
                    page4.cellPlateletSmear3.ForeColor = (PlateletSmearAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellPlateletSmear3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellPlateletsCountRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010")?.ReferenceRange;
                page4.cellPlateletsCount1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010" && p.Year == year1)?.ResultValue;
                page4.cellPlateletsCount2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010" && p.Year == year2)?.ResultValue;
                page4.cellPlateletsCount3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010" && p.Year == year3)?.ResultValue;

                string PlateletsCountAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010" && p.Year == year1)?.IsAbnormal;
                string PlateletsCountAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010" && p.Year == year2)?.IsAbnormal;
                string PlateletsCountAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(PlateletsCountAbnormal1))
                {
                    page4.cellPlateletsCount1.ForeColor = (PlateletsCountAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellPlateletsCount1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(PlateletsCountAbnormal2))
                {
                    page4.cellPlateletsCount2.ForeColor = (PlateletsCountAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellPlateletsCount2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(PlateletsCountAbnormal3))
                {
                    page4.cellPlateletsCount3.ForeColor = (PlateletsCountAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellPlateletsCount3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
            }
            else
            {
                page4.cellCBCYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page4.cellCBCYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page4.cellCBCYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }

        }

        private void GenerateUrinalysis(IEnumerable<PatientResultLabModel> labTestSet)
        {
            if(labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;
                page5.cellUAYear1.Text = "ปี" + " " + year1.ToString();
                page5.cellUAYear2.Text = "ปี" + " " + year2.ToString();
                page5.cellUAYear3.Text = "ปี" + " " + year3.ToString();

                page5.cellColorRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080")?.ReferenceRange;
                page5.cellColor1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year1)?.ResultValue;
                page5.cellColor2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year2)?.ResultValue;
                page5.cellColor3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year3)?.ResultValue;

                string colorAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year1)?.IsAbnormal;
                string colorAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year2)?.IsAbnormal;
                string colorAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year3)?.IsAbnormal;
                
                if (!string.IsNullOrEmpty(colorAbnormal1))
                {
                    page5.cellColor1.ForeColor = (colorAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellColor1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(colorAbnormal2))
                {
                    page5.cellColor2.ForeColor = (colorAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellColor2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(colorAbnormal3))
                {
                    page5.cellColor3.ForeColor = (colorAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellColor3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellClarityRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21")?.ReferenceRange;
                page5.cellClarity1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year1)?.ResultValue;
                page5.cellClarity2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year2)?.ResultValue;
                page5.cellClarity3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year3)?.ResultValue;

                string ClarityAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year1)?.IsAbnormal;
                string ClarityAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year2)?.IsAbnormal;
                string ClarityAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year3)?.IsAbnormal;
                
                if (!string.IsNullOrEmpty(ClarityAbnormal1))
                {
                    page5.cellClarity1.ForeColor = (ClarityAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellClarity1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(ClarityAbnormal2))
                {
                    page5.cellClarity2.ForeColor = (ClarityAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellClarity2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(ClarityAbnormal3))
                {
                    page5.cellClarity3.ForeColor = (ClarityAbnormal3 == "H") ? Color.Red : Color.Blue; ;
                    page5.cellClarity3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellSpacGraRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001")?.ReferenceRange;
                page5.cellSpacGra1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001" && p.Year == year1)?.ResultValue;
                page5.cellSpacGra2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001" && p.Year == year2)?.ResultValue;
                page5.cellSpacGra3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001" && p.Year == year3)?.ResultValue;

                string SpacGraAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001" && p.Year == year1)?.IsAbnormal;
                string SpacGraAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001" && p.Year == year2)?.IsAbnormal;
                string SpacGraAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001" && p.Year == year3)?.IsAbnormal;
                
                if (!string.IsNullOrEmpty(SpacGraAbnormal1))
                {
                    page5.cellSpacGra1.ForeColor = (SpacGraAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellSpacGra1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(SpacGraAbnormal2))
                {
                    page5.cellSpacGra2.ForeColor = (SpacGraAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellSpacGra2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(SpacGraAbnormal3))
                {
                    page5.cellSpacGra3.ForeColor = (SpacGraAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellClarity3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellPhRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080")?.ReferenceRange;
                page5.cellPh1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080" && p.Year == year1)?.ResultValue;
                page5.cellPh2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080" && p.Year == year2)?.ResultValue;
                page5.cellPh3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080" && p.Year == year3)?.ResultValue;

                string phAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080" && p.Year == year1)?.IsAbnormal;
                string phAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080" && p.Year == year2)?.IsAbnormal;
                string phAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080" && p.Year == year3)?.IsAbnormal;
                
                if (!string.IsNullOrEmpty(phAbnormal1))
                {
                    page5.cellPh1.ForeColor = (phAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellPh1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(phAbnormal2))
                {
                    page5.cellPh2.ForeColor = (phAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellPh2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(phAbnormal3))
                {
                    page5.cellPh3.ForeColor = (phAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellPh3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellProteinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085")?.ReferenceRange;
                page5.cellProtein1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085" && p.Year == year1)?.ResultValue;
                page5.cellProtein2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085" && p.Year == year2)?.ResultValue;
                page5.cellProtein3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085" && p.Year == year3)?.ResultValue;

                string ProteinAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085" && p.Year == year1)?.IsAbnormal;
                string ProteinAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085" && p.Year == year2)?.IsAbnormal;
                string ProteinAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085" && p.Year == year3)?.IsAbnormal;
                
                if (!string.IsNullOrEmpty(ProteinAbnormal1))
                {
                    page5.cellProtein1.ForeColor = (ProteinAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellProtein1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(ProteinAbnormal2))
                {
                    page5.cellProtein2.ForeColor = (ProteinAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellProtein2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(ProteinAbnormal3))
                {
                    page5.cellProtein3.ForeColor = (ProteinAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellProtein3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellGlucoseRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090")?.ReferenceRange;
                page5.cellGlucose1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090" && p.Year == year1)?.ResultValue;
                page5.cellGlucose2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090" && p.Year == year2)?.ResultValue;
                page5.cellGlucose3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090" && p.Year == year3)?.ResultValue;

                string GlucoseAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090" && p.Year == year1)?.IsAbnormal;
                string GlucoseAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090" && p.Year == year2)?.IsAbnormal;
                string GlucoseAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(GlucoseAbnormal1))
                {
                    page5.cellGlucose1.ForeColor = (GlucoseAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellGlucose1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(GlucoseAbnormal2))
                {
                    page5.cellGlucose2.ForeColor = (GlucoseAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellGlucose2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(GlucoseAbnormal3))
                {
                    page5.cellGlucose3.ForeColor = (GlucoseAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellGlucose3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellKetoneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047")?.ReferenceRange;
                page5.cellKetone1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047" && p.Year == year1)?.ResultValue;
                page5.cellKetone2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047" && p.Year == year2)?.ResultValue;
                page5.cellKetone3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047" && p.Year == year3)?.ResultValue;

                string KetoneAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047" && p.Year == year1)?.IsAbnormal;
                string KetoneAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047" && p.Year == year2)?.IsAbnormal;
                string KetoneAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(KetoneAbnormal1))
                {
                    page5.cellKetone1.ForeColor = (KetoneAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellKetone1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(KetoneAbnormal2))
                {
                    page5.cellKetone2.ForeColor = (KetoneAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellKetone2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(KetoneAbnormal3))
                {
                    page5.cellKetone3.ForeColor = (KetoneAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellKetone3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellNitritesRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154")?.ReferenceRange;
                page5.cellNitrites1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154" && p.Year == year1)?.ResultValue;
                page5.cellNitrites2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154" && p.Year == year2)?.ResultValue;
                page5.cellNitrites3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154" && p.Year == year3)?.ResultValue;

                string NitritesAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154" && p.Year == year1)?.IsAbnormal;
                string NitritesAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154" && p.Year == year2)?.IsAbnormal;
                string NitritesAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(NitritesAbnormal1))
                {
                    page5.cellNitrites1.ForeColor = (NitritesAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellNitrites1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(NitritesAbnormal2))
                {
                    page5.cellNitrites2.ForeColor = (NitritesAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellNitrites2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(NitritesAbnormal3))
                {
                    page5.cellNitrites3.ForeColor = (NitritesAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellNitrites3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellBilirubinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151")?.ReferenceRange;
                page5.cellBilirubin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151" && p.Year == year1)?.ResultValue;
                page5.cellBilirubin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151" && p.Year == year2)?.ResultValue;
                page5.cellBilirubin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151" && p.Year == year3)?.ResultValue;

                string BilirubinAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151" && p.Year == year1)?.IsAbnormal;
                string BilirubinAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151" && p.Year == year2)?.IsAbnormal;
                string BilirubinAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(BilirubinAbnormal1))
                {
                    page5.cellBilirubin1.ForeColor = (BilirubinAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellBilirubin1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BilirubinAbnormal2))
                {
                    page5.cellBilirubin2.ForeColor = (BilirubinAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellBilirubin2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BilirubinAbnormal3))
                {
                    page5.cellBilirubin3.ForeColor = (BilirubinAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellBilirubin3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellUrobilinogenRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150")?.ReferenceRange;
                page5.cellUrobilinogen1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150" && p.Year == year1)?.ResultValue;
                page5.cellUrobilinogen2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150" && p.Year == year2)?.ResultValue;
                page5.cellUrobilinogen3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150" && p.Year == year3)?.ResultValue;

                string UrobilinogenAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150" && p.Year == year1)?.IsAbnormal;
                string UrobilinogenAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150" && p.Year == year2)?.IsAbnormal;
                string UrobilinogenAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(UrobilinogenAbnormal1))
                {
                    page5.cellUrobilinogen1.ForeColor = (UrobilinogenAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellUrobilinogen1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(UrobilinogenAbnormal2))
                {
                    page5.cellUrobilinogen2.ForeColor = (UrobilinogenAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellUrobilinogen2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(UrobilinogenAbnormal3))
                {
                    page5.cellUrobilinogen3.ForeColor = (UrobilinogenAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellUrobilinogen3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellLeukocyteRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153")?.ReferenceRange;
                page5.cellLeukocyte1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153" && p.Year == year1)?.ResultValue;
                page5.cellLeukocyte2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153" && p.Year == year2)?.ResultValue;
                page5.cellLeukocyte3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153" && p.Year == year3)?.ResultValue;

                string LeukocyteAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153" && p.Year == year1)?.IsAbnormal;
                string LeukocyteAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153" && p.Year == year2)?.IsAbnormal;
                string LeukocyteAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(LeukocyteAbnormal1))
                {
                    page5.cellLeukocyte1.ForeColor = (LeukocyteAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellLeukocyte1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(LeukocyteAbnormal2))
                {
                    page5.cellLeukocyte2.ForeColor = (LeukocyteAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellLeukocyte2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(LeukocyteAbnormal3))
                {
                    page5.cellLeukocyte3.ForeColor = (LeukocyteAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellLeukocyte3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellBloodRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152")?.ReferenceRange;
                page5.cellBlood1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152" && p.Year == year1)?.ResultValue;
                page5.cellBlood2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152" && p.Year == year2)?.ResultValue;
                page5.cellBlood3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152" && p.Year == year3)?.ResultValue;

                string BloodAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152" && p.Year == year1)?.IsAbnormal;
                string BloodAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152" && p.Year == year2)?.IsAbnormal;
                string BloodAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(BloodAbnormal1))
                {
                    page5.cellBlood1.ForeColor = (BloodAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellBlood1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BloodAbnormal2))
                {
                    page5.cellBlood2.ForeColor = (BloodAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellBlood2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BloodAbnormal3))
                {
                    page5.cellBlood3.ForeColor = (BloodAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellBlood3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellErythrocytesRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140")?.ReferenceRange;
                page5.cellErythrocytes1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140" && p.Year == year1)?.ResultValue;
                page5.cellErythrocytes2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140" && p.Year == year2)?.ResultValue;
                page5.cellErythrocytes3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140" && p.Year == year3)?.ResultValue;

                string ErythrocytesAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140" && p.Year == year1)?.IsAbnormal;
                string ErythrocytesAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140" && p.Year == year2)?.IsAbnormal;
                string ErythrocytesAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(ErythrocytesAbnormal1))
                {
                    page5.cellErythrocytes1.ForeColor = (ErythrocytesAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellErythrocytes1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(ErythrocytesAbnormal2))
                {
                    page5.cellErythrocytes2.ForeColor = (ErythrocytesAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellErythrocytes2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(ErythrocytesAbnormal3))
                {
                    page5.cellErythrocytes3.ForeColor = (ErythrocytesAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellErythrocytes3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellWbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250")?.ReferenceRange;
                page5.cellWbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250" && p.Year == year1)?.ResultValue;
                page5.cellWbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250" && p.Year == year2)?.ResultValue;
                page5.cellWbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250" && p.Year == year3)?.ResultValue;

                string WbcAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250" && p.Year == year1)?.IsAbnormal;
                string WbcAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250" && p.Year == year2)?.IsAbnormal;
                string WbcAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(WbcAbnormal1))
                {
                    page5.cellWbc1.ForeColor = (WbcAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellWbc1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(WbcAbnormal2))
                {
                    page5.cellWbc2.ForeColor = (WbcAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellWbc2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(WbcAbnormal3))
                {
                    page5.cellWbc3.ForeColor = (WbcAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellWbc3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellRbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260")?.ReferenceRange;
                page5.cellRbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260" && p.Year == year1)?.ResultValue;
                page5.cellRbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260" && p.Year == year2)?.ResultValue;
                page5.cellRbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260" && p.Year == year3)?.ResultValue;

                string RbcAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260" && p.Year == year1)?.IsAbnormal;
                string RbcAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260" && p.Year == year2)?.IsAbnormal;
                string RbcAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(RbcAbnormal1))
                {
                    page5.cellRbc1.ForeColor = (RbcAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellRbc1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(RbcAbnormal2))
                {
                    page5.cellRbc2.ForeColor = (RbcAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellRbc2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(RbcAbnormal3))
                {
                    page5.cellRbc3.ForeColor = (RbcAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellRbc3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellEpithelialCellsRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270")?.ReferenceRange;
                page5.cellEpithelialCells1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270" && p.Year == year1)?.ResultValue;
                page5.cellEpithelialCells2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270" && p.Year == year2)?.ResultValue;
                page5.cellEpithelialCells3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270" && p.Year == year3)?.ResultValue;

                string EpithelialCellsAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270" && p.Year == year1)?.IsAbnormal;
                string EpithelialCellsAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270" && p.Year == year2)?.IsAbnormal;
                string EpithelialCellsAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(EpithelialCellsAbnormal1))
                {
                    page5.cellEpithelialCells1.ForeColor = (EpithelialCellsAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellEpithelialCells1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(EpithelialCellsAbnormal2))
                {
                    page5.cellEpithelialCells2.ForeColor = (EpithelialCellsAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellEpithelialCells2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(EpithelialCellsAbnormal3))
                {
                    page5.cellEpithelialCells3.ForeColor = (EpithelialCellsAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellEpithelialCells3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellCastsRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR16")?.ReferenceRange;
                page5.cellCasts1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR16" && p.Year == year1)?.ResultValue;
                page5.cellCasts2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR16" && p.Year == year2)?.ResultValue;
                page5.cellCasts3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR16" && p.Year == year3)?.ResultValue;

                string CastsAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR16" && p.Year == year1)?.IsAbnormal;
                string CastsAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR16" && p.Year == year2)?.IsAbnormal;
                string CastsAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR16" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(CastsAbnormal1))
                {
                    page5.cellCasts1.ForeColor = (CastsAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellCasts1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CastsAbnormal2))
                {
                    page5.cellCasts2.ForeColor = (CastsAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellCasts2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CastsAbnormal3))
                {
                    page5.cellCasts3.ForeColor = (CastsAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellCasts3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellBacteriaRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0155")?.ReferenceRange;
                page5.cellBacteria1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0155" && p.Year == year1)?.ResultValue;
                page5.cellBacteria2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0155" && p.Year == year2)?.ResultValue;
                page5.cellBacteria3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0155" && p.Year == year3)?.ResultValue;

                string BacteriaAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0155" && p.Year == year1)?.IsAbnormal;
                string BacteriaAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0155" && p.Year == year2)?.IsAbnormal;
                string BacteriaAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0155" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(BacteriaAbnormal1))
                {
                    page5.cellBacteria1.ForeColor = (BacteriaAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellBacteria1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BacteriaAbnormal2))
                {
                    page5.cellBacteria2.ForeColor = (BacteriaAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellBacteria2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BacteriaAbnormal2))
                {
                    page5.cellBacteria3.ForeColor = (BacteriaAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellBacteria3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellBuddingYeastRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR17")?.ReferenceRange;
                page5.cellBuddingYeast1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR17" && p.Year == year1)?.ResultValue;
                page5.cellBuddingYeast2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR17" && p.Year == year2)?.ResultValue;
                page5.cellBuddingYeast3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR17" && p.Year == year3)?.ResultValue;

                string BuddingYeastAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR17" && p.Year == year1)?.IsAbnormal;
                string BuddingYeastAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR17" && p.Year == year2)?.IsAbnormal;
                string BuddingYeastAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR17" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(BuddingYeastAbnormal1))
                {
                    page5.cellBuddingYeast1.ForeColor = (BuddingYeastAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellBuddingYeast1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BuddingYeastAbnormal2))
                {
                    page5.cellBuddingYeast2.ForeColor = (BuddingYeastAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellBuddingYeast2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BuddingYeastAbnormal3))
                {
                    page5.cellBuddingYeast3.ForeColor = (BuddingYeastAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellBuddingYeast3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellCrystalRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR19")?.ReferenceRange;
                page5.cellCrystal1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR19" && p.Year == year1)?.ResultValue;
                page5.cellCrystal2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR19" && p.Year == year2)?.ResultValue;
                page5.cellCrystal3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR19" && p.Year == year3)?.ResultValue;

                string CrystalAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR19" && p.Year == year1)?.IsAbnormal;
                string CrystalAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR19" && p.Year == year2)?.IsAbnormal;
                string CrystalAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR19" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(CrystalAbnormal1))
                {
                    page5.cellCrystal1.ForeColor = (CrystalAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellCrystal1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CrystalAbnormal2))
                {
                    page5.cellCrystal2.ForeColor = (CrystalAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellCrystal2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CrystalAbnormal3))
                {
                    page5.cellCrystal3.ForeColor = (CrystalAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellCrystal3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellMucousRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR18")?.ReferenceRange;
                page5.cellMucous1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR18" && p.Year == year1)?.ResultValue;
                page5.cellMucous2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR18" && p.Year == year2)?.ResultValue;
                page5.cellMucous3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR18" && p.Year == year3)?.ResultValue;

                string MucousAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR18" && p.Year == year1)?.IsAbnormal;
                string MucousAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR18" && p.Year == year2)?.IsAbnormal;
                string MucousAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR18" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(MucousAbnormal1))
                {
                    page5.cellMucous1.ForeColor = (MucousAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellMucous1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(MucousAbnormal2))
                {
                    page5.cellMucous2.ForeColor = (MucousAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellMucous2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(MucousAbnormal3))
                {
                    page5.cellMucous3.ForeColor = (MucousAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellMucous3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellAmorphousRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14")?.ReferenceRange;
                page5.cellAmorphous1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14" && p.Year == year1)?.ResultValue;
                page5.cellAmorphous2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14" && p.Year == year2)?.ResultValue;
                page5.cellAmorphous3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14" && p.Year == year3)?.ResultValue;

                string AmorphousAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14" && p.Year == year1)?.IsAbnormal;
                string AmorphousAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14" && p.Year == year2)?.IsAbnormal;
                string AmorphousAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(AmorphousAbnormal1))
                {
                    page5.cellAmorphous1.ForeColor = (AmorphousAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellAmorphous1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AmorphousAbnormal2))
                {
                    page5.cellAmorphous2.ForeColor = (AmorphousAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellAmorphous2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AmorphousAbnormal3))
                {
                    page5.cellAmorphous3.ForeColor = (AmorphousAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellAmorphous3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellOtherRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20")?.ReferenceRange;
                page5.cellOther1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20" && p.Year == year1)?.ResultValue;
                page5.cellOther2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20" && p.Year == year2)?.ResultValue;
                page5.cellOther3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20" && p.Year == year3)?.ResultValue;

                string OtherAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20" && p.Year == year1)?.IsAbnormal;
                string OtherAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20" && p.Year == year2)?.IsAbnormal;
                string OtherAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(OtherAbnormal1))
                {
                    page5.cellOther1.ForeColor = (OtherAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellOther1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(OtherAbnormal2))
                {
                    page5.cellOther2.ForeColor = (OtherAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellOther2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(OtherAbnormal3))
                {
                    page5.cellOther3.ForeColor = (OtherAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellOther3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
            }
            else
            {
                page5.cellUAYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page5.cellUAYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page5.cellUAYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
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
                page6.cellRenalYear1.Text = "ปี" + " " + year1.ToString();
                page6.cellRenalYear2.Text = "ปี" + " " + year2.ToString();
                page6.cellRenalYear3.Text = "ปี" + " " + year3.ToString();

                page6.cellBunRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27")?.ReferenceRange;
                page6.cellBun1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27" && p.Year == year1)?.ResultValue;
                page6.cellBun2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27" && p.Year == year2)?.ResultValue;
                page6.cellBun3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27" && p.Year == year3)?.ResultValue;

                string bunAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27" && p.Year == year1)?.IsAbnormal;
                string bunAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27" && p.Year == year2)?.IsAbnormal;
                string bunAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(bunAbnormal1))
                {
                    page6.cellBun1.ForeColor = (bunAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page6.cellBun1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(bunAbnormal2))
                {
                    page6.cellBun2.ForeColor = (bunAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page6.cellBun2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(bunAbnormal3))
                {
                    page6.cellBun3.ForeColor = (bunAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page6.cellBun3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page6.cellCreatinineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070")?.ReferenceRange;
                page6.cellCreatinine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070" && p.Year == year1)?.ResultValue;
                page6.cellCreatinine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070" && p.Year == year2)?.ResultValue;
                page6.cellCreatinine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070" && p.Year == year3)?.ResultValue;

                string CreatinineAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070" && p.Year == year1)?.IsAbnormal;
                string CreatinineAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070" && p.Year == year2)?.IsAbnormal;
                string CreatinineAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(CreatinineAbnormal1))
                {
                    page6.cellCreatinine1.ForeColor = (CreatinineAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page6.cellCreatinine1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CreatinineAbnormal2))
                {
                    page6.cellCreatinine2.ForeColor = (CreatinineAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page6.cellCreatinine2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CreatinineAbnormal3))
                {
                    page6.cellCreatinine3.ForeColor = (CreatinineAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page6.cellCreatinine3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
            }
            else
            {
                page6.cellRenalYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page6.cellRenalYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page6.cellRenalYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
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
                page6.cellUricYear1.Text = "ปี" + " " + year1.ToString();
                page6.cellUricYear2.Text = "ปี" + " " + year2.ToString();
                page6.cellUricYear3.Text = "ปี" + " " + year3.ToString();

                page6.cellUricRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320")?.ReferenceRange;
                page6.cellUric1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320" && p.Year == year1)?.ResultValue;
                page6.cellUric2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320" && p.Year == year2)?.ResultValue;
                page6.cellUric3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320" && p.Year == year3)?.ResultValue;

                string uricAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320" && p.Year == year1)?.IsAbnormal;
                string uricAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320" && p.Year == year2)?.IsAbnormal;
                string uricAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(uricAbnormal1))
                {
                    page6.cellUric1.ForeColor = (uricAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page6.cellUric1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(uricAbnormal2))
                {
                    page6.cellUric2.ForeColor = (uricAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page6.cellUric2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(uricAbnormal3))
                {
                    page6.cellUric3.ForeColor = (uricAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page6.cellUric3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
            }
            else
            {
                page6.cellUricYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page6.cellUricYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page6.cellUricYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
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
                page6.cellLiverYear1.Text = "ปี" + " " + year1.ToString();
                page6.cellLiverYear2.Text = "ปี" + " " + year2.ToString();
                page6.cellLiverYear3.Text = "ปี" + " " + year3.ToString();

                page6.cellAstRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50")?.ReferenceRange;
                page6.cellAst1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50" && p.Year == year1)?.ResultValue;
                page6.cellAst2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50" && p.Year == year2)?.ResultValue;
                page6.cellAst3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50" && p.Year == year3)?.ResultValue;

                string astAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50" && p.Year == year1)?.IsAbnormal;
                string astAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50" && p.Year == year2)?.IsAbnormal;
                string astAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(astAbnormal1))
                {
                    page6.cellAst1.ForeColor = (astAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page6.cellAst1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(astAbnormal2))
                {
                    page6.cellAst2.ForeColor = (astAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page6.cellAst2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(astAbnormal3))
                {
                    page6.cellAst3.ForeColor = (astAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page6.cellAst3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page6.cellAltRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51")?.ReferenceRange;
                page6.cellAlt1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51" && p.Year == year1)?.ResultValue;
                page6.cellAlt2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51" && p.Year == year2)?.ResultValue;
                page6.cellAlt3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51" && p.Year == year3)?.ResultValue;

                string altAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51" && p.Year == year1)?.IsAbnormal;
                string altAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51" && p.Year == year2)?.IsAbnormal;
                string altAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(altAbnormal1))
                {
                    page6.cellAlt1.ForeColor = (altAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page6.cellAlt1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(altAbnormal2))
                {
                    page6.cellAlt2.ForeColor = (altAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page6.cellAlt2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(altAbnormal3))
                {
                    page6.cellAlt3.ForeColor = (altAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page6.cellAlt3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page6.cellAlpRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33")?.ReferenceRange;
                page6.cellAlp1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33" && p.Year == year1)?.ResultValue;
                page6.cellAlp2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33" && p.Year == year2)?.ResultValue;
                page6.cellAlp3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33" && p.Year == year3)?.ResultValue;

                string alpAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33" && p.Year == year1)?.IsAbnormal;
                string alpAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33" && p.Year == year2)?.IsAbnormal;
                string alpAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(alpAbnormal1))
                {
                    page6.cellAlp1.ForeColor = (alpAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page6.cellAlp1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(alpAbnormal2))
                {
                    page6.cellAlp2.ForeColor = (alpAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page6.cellAlp2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(alpAbnormal3))
                {
                    page6.cellAlp3.ForeColor = (alpAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page6.cellAlp3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page6.cellTotalBilirubinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48")?.ReferenceRange;
                page6.cellTotalBilirubin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48" && p.Year == year1)?.ResultValue;
                page6.cellTotalBilirubin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48" && p.Year == year2)?.ResultValue;
                page6.cellTotalBilirubin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48" && p.Year == year3)?.ResultValue;

                string TotalBilirubinAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48" && p.Year == year1)?.IsAbnormal;
                string TotalBilirubinAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48" && p.Year == year2)?.IsAbnormal;
                string TotalBilirubinAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(TotalBilirubinAbnormal1))
                {
                    page6.cellTotalBilirubin1.ForeColor = (TotalBilirubinAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page6.cellTotalBilirubin1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(TotalBilirubinAbnormal2))
                {
                    page6.cellTotalBilirubin2.ForeColor = (TotalBilirubinAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page6.cellTotalBilirubin2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(TotalBilirubinAbnormal3))
                {
                    page6.cellTotalBilirubin3.ForeColor = (TotalBilirubinAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page6.cellTotalBilirubin3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page6.cellDirectBilirubinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49")?.ReferenceRange;
                page6.cellDirectBilirubin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year1)?.ResultValue;
                page6.cellDirectBilirubin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year2)?.ResultValue;
                page6.cellDirectBilirubin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year3)?.ResultValue;

                string DirectBilirubiAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year1)?.IsAbnormal;
                string DirectBilirubiAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year2)?.IsAbnormal;
                string DirectBilirubiAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(DirectBilirubiAbnormal1))
                {
                    page6.cellDirectBilirubin1.ForeColor = (DirectBilirubiAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page6.cellDirectBilirubin1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(DirectBilirubiAbnormal2))
                {
                    page6.cellDirectBilirubin2.ForeColor = (DirectBilirubiAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page6.cellDirectBilirubin2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(DirectBilirubiAbnormal3))
                {
                    page6.cellDirectBilirubin3.ForeColor = (DirectBilirubiAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page6.cellDirectBilirubin3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page6.cellTotalProteinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105")?.ReferenceRange;
                page6.cellTotalProtein1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105" && p.Year == year1)?.ResultValue;
                page6.cellTotalProtein2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105" && p.Year == year2)?.ResultValue;
                page6.cellTotalProtein3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105" && p.Year == year3)?.ResultValue;

                string TotalProteinAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105" && p.Year == year1)?.IsAbnormal;
                string TotalProteinAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105" && p.Year == year2)?.IsAbnormal;
                string TotalProteinAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(TotalProteinAbnormal1))
                {
                    page6.cellTotalProtein1.ForeColor = (TotalProteinAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page6.cellTotalProtein1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(TotalProteinAbnormal2))
                {
                    page6.cellTotalProtein2.ForeColor = (TotalProteinAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page6.cellTotalProtein2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(TotalProteinAbnormal3))
                {
                    page6.cellTotalProtein3.ForeColor = (TotalProteinAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page6.cellTotalProtein3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page6.cellAlbuminRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49")?.ReferenceRange;
                page6.cellAlbumin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year1)?.ResultValue;
                page6.cellAlbumin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year2)?.ResultValue;
                page6.cellAlbumin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year3)?.ResultValue;

                string AlbuminAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year1)?.IsAbnormal;
                string AlbuminAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year2)?.IsAbnormal;
                string AlbuminAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(AlbuminAbnormal1))
                {
                    page6.cellAlbumin1.ForeColor = (AlbuminAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page6.cellAlbumin1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AlbuminAbnormal2))
                {
                    page6.cellAlbumin2.ForeColor = (AlbuminAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page6.cellAlbumin2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AlbuminAbnormal3))
                {
                    page6.cellAlbumin3.ForeColor = (AlbuminAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page6.cellAlbumin3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page6.cellGlobulinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46")?.ReferenceRange;
                page6.cellGlobulin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year1)?.ResultValue;
                page6.cellGlobulin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year2)?.ResultValue;
                page6.cellGlobulin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year3)?.ResultValue;

                string GlobulinAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year1)?.IsAbnormal;
                string GlobulinAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year2)?.IsAbnormal;
                string GlobulinAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(GlobulinAbnormal1))
                {
                    page6.cellGlobulin1.ForeColor = (GlobulinAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page6.cellGlobulin1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(GlobulinAbnormal2))
                {
                    page6.cellGlobulin2.ForeColor = (GlobulinAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page6.cellGlobulin2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(GlobulinAbnormal3))
                {
                    page6.cellGlobulin3.ForeColor = (GlobulinAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page6.cellGlobulin3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
            }
            else
            {
                page6.cellLiverYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page6.cellLiverYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page6.cellLiverYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
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
                page5.cellImmunlogyYear1.Text = "ปี" + " " + year1.ToString();
                page5.cellImmunlogyYear2.Text = "ปี" + " " + year2.ToString();
                page5.cellImmunlogyYear3.Text = "ปี" + " " + year3.ToString();

                page5.cellHbsAgRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35")?.ReferenceRange;
                page5.cellHbsAg1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35" && p.Year == year1)?.ResultValue;
                page5.cellHbsAg2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35" && p.Year == year2)?.ResultValue;
                page5.cellHbsAg3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35" && p.Year == year3)?.ResultValue;

                string HbsAgAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35" && p.Year == year1)?.IsAbnormal;
                string HbsAgAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35" && p.Year == year2)?.IsAbnormal;
                string HbsAgAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(HbsAgAbnormal1))
                {
                    page5.cellHbsAg1.ForeColor = (HbsAgAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellHbsAg1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(HbsAgAbnormal2))
                {
                    page5.cellHbsAg2.ForeColor = (HbsAgAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellHbsAg2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(HbsAgAbnormal3))
                {
                    page5.cellHbsAg3.ForeColor = (HbsAgAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellHbsAg3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellCoiAgRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34")?.ReferenceRange;
                page5.cellCoiAg1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34" && p.Year == year1)?.ResultValue;
                page5.cellCoiAg2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34" && p.Year == year2)?.ResultValue;
                page5.cellCoiAg3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34" && p.Year == year3)?.ResultValue;

                string CoiAgAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34" && p.Year == year1)?.IsAbnormal;
                string CoiAgAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34" && p.Year == year2)?.IsAbnormal;
                string CoiAgAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(CoiAgAbnormal1))
                {
                    page5.cellCoiAg1.ForeColor = (CoiAgAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellCoiAg1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CoiAgAbnormal2))
                {
                    page5.cellCoiAg2.ForeColor = (CoiAgAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellCoiAg2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CoiAgAbnormal3))
                {
                    page5.cellCoiAg3.ForeColor = (CoiAgAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellCoiAg3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellCoiHbsRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121")?.ReferenceRange;
                page5.cellCoiHbs1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121" && p.Year == year1)?.ResultValue;
                page5.cellCoiHbs2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121" && p.Year == year2)?.ResultValue;
                page5.cellCoiHbs3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121" && p.Year == year3)?.ResultValue;

                string CoiHbsAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121" && p.Year == year1)?.IsAbnormal;
                string CoiHbsAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121" && p.Year == year2)?.IsAbnormal;
                string CoiHbsAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(CoiHbsAbnormal1))
                {
                    page5.cellCoiHbs1.ForeColor = (CoiHbsAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellCoiHbs1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CoiHbsAbnormal2))
                {
                    page5.cellCoiHbs2.ForeColor = (CoiHbsAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellCoiHbs2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CoiHbsAbnormal3))
                {
                    page5.cellCoiHbs3.ForeColor = (CoiHbsAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellCoiHbs3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellAntiHbsRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42")?.ReferenceRange;
                page5.cellAntiHbs1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42" && p.Year == year1)?.ResultValue;
                page5.cellAntiHbs2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42" && p.Year == year2)?.ResultValue;
                page5.cellAntiHbs3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42" && p.Year == year3)?.ResultValue;

                string AntiHbsAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42" && p.Year == year1)?.IsAbnormal;
                string AntiHbsAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42" && p.Year == year2)?.IsAbnormal;
                string AntiHbsAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(AntiHbsAbnormal1))
                {
                    page5.cellAntiHbs1.ForeColor = (AntiHbsAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellAntiHbs1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AntiHbsAbnormal2))
                {
                    page5.cellAntiHbs2.ForeColor = (AntiHbsAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellAntiHbs2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AntiHbsAbnormal3))
                {
                    page5.cellAntiHbs3.ForeColor = (AntiHbsAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellAntiHbs3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
            }
            else
            {
                page5.cellImmunlogyYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page5.cellImmunlogyYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page5.cellImmunlogyYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }

        private void GenerateToxicology(IEnumerable<PatientResultLabModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;
                page4.cellToxicoYear1.Text = "ปี" + " " + year1.ToString();
                page4.cellToxicoYear2.Text = "ปี" + " " + year2.ToString();
                page4.cellToxicoYear3.Text = "ปี" + " " + year3.ToString();

                page4.cellAluminiumRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122")?.ReferenceRange;
                page4.cellAluminium1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year1)?.ResultValue;
                page4.cellAluminium2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year2)?.ResultValue;
                page4.cellAluminium3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year3)?.ResultValue;

                string AluminiumAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year1)?.IsAbnormal;
                string AluminiumAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year2)?.IsAbnormal;
                string AluminiumAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(AluminiumAbnormal1))
                {
                    page4.cellAluminium1.ForeColor = (AluminiumAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellAluminium1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AluminiumAbnormal2))
                {
                    page4.cellAluminium2.ForeColor = (AluminiumAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellAluminium2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AluminiumAbnormal3))
                {
                    page4.cellAluminium3.ForeColor = (AluminiumAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellAluminium3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellTolueneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124")?.ReferenceRange;
                page4.cellToluene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year1)?.ResultValue;
                page4.cellToluene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year2)?.ResultValue;
                page4.cellToluene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year3)?.ResultValue;

                string TolueneAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year1)?.IsAbnormal;
                string TolueneAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year2)?.IsAbnormal;
                string TolueneAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(TolueneAbnormal1))
                {
                    page4.cellToluene1.ForeColor = (TolueneAbnormal1 == "H") ? Color.Red : Color.Blue; 
                    page4.cellToluene1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(TolueneAbnormal2))
                {
                    page4.cellToluene2.ForeColor = (TolueneAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellToluene2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(TolueneAbnormal3))
                {
                    page4.cellToluene3.ForeColor = (TolueneAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellToluene3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellXyleneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125")?.ReferenceRange;
                page4.cellXylene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year1)?.ResultValue;
                page4.cellXylene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year2)?.ResultValue;
                page4.cellXylene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year3)?.ResultValue;

                string XyleneAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year1)?.IsAbnormal;
                string XyleneAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year2)?.IsAbnormal;
                string XyleneAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(XyleneAbnormal1))
                {
                    page4.cellXylene1.ForeColor = (XyleneAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellXylene1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(XyleneAbnormal2))
                {
                    page4.cellXylene2.ForeColor = (XyleneAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellXylene2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(XyleneAbnormal3))
                {
                    page4.cellXylene3.ForeColor = (XyleneAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellXylene3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellLeadRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75")?.ReferenceRange;
                if (page4.cellLeadRange.Text != null && page4.cellLeadRange.Text.Length > 20)
                {
                    page4.cellLeadRange.Font = new Font("Angsana New", 7);
                }

                page4.cellLead1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year1)?.ResultValue;
                page4.cellLead2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year2)?.ResultValue;
                page4.cellLead3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year3)?.ResultValue;

                string LeadAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year1)?.IsAbnormal;
                string LeadAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year2)?.IsAbnormal;
                string LeadAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(LeadAbnormal1))
                {
                    page4.cellLead1.ForeColor = (LeadAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellLead1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(LeadAbnormal2))
                {
                    page4.cellLead2.ForeColor = (LeadAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellLead2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(LeadAbnormal3))
                {
                    page4.cellLead3.ForeColor = (LeadAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellLead3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellCarboxyRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120")?.ReferenceRange;
                page4.cellCarboxy1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year1)?.ResultValue;
                page4.cellCarboxy2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year2)?.ResultValue;
                page4.cellCarboxy3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year3)?.ResultValue;

                string CarboxyAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year1)?.IsAbnormal;
                string CarboxyAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year2)?.IsAbnormal;
                string CarboxyAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(CarboxyAbnormal1))
                {
                    page4.cellCarboxy1.ForeColor = (CarboxyAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellCarboxy1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CarboxyAbnormal2))
                {
                    page4.cellCarboxy2.ForeColor = (CarboxyAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellCarboxy2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(CarboxyAbnormal3))
                {
                    page4.cellCarboxy3.ForeColor = (CarboxyAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellCarboxy3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellMekRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127")?.ReferenceRange;
                page4.cellMek1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year1)?.ResultValue;
                page4.cellMek2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year2)?.ResultValue;
                page4.cellMek3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year3)?.ResultValue;

                string MekAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year1)?.IsAbnormal;
                string MekAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year2)?.IsAbnormal;
                string MekAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(MekAbnormal1))
                {
                    page4.cellMek1.ForeColor = (MekAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellMek1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(MekAbnormal2))
                {
                    page4.cellMek2.ForeColor = (MekAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellMek2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(MekAbnormal3))
                {
                    page4.cellMek3.ForeColor = (MekAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellMek3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellBenzeneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115")?.ReferenceRange;
                page4.cellBenzene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115" && p.Year == year1)?.ResultValue;
                page4.cellBenzene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115" && p.Year == year2)?.ResultValue;
                page4.cellBenzene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115" && p.Year == year3)?.ResultValue;

                string BenzeneAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115" && p.Year == year1)?.IsAbnormal;
                string BenzeneAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115" && p.Year == year2)?.IsAbnormal;
                string BenzeneAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(BenzeneAbnormal1))
                {
                    page4.cellBenzene1.ForeColor = (BenzeneAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellBenzene1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BenzeneAbnormal2))
                {
                    page4.cellBenzene2.ForeColor = (BenzeneAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellBenzene2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BenzeneAbnormal3))
                {
                    page4.cellBenzene3.ForeColor = (BenzeneAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellBenzene3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellMethanolRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116")?.ReferenceRange;
                page4.cellMethanol1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116" && p.Year == year1)?.ResultValue;
                page4.cellMethanol2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116" && p.Year == year2)?.ResultValue;
                page4.cellMethanol3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116" && p.Year == year3)?.ResultValue;

                string MethanolAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116" && p.Year == year1)?.IsAbnormal;
                string MethanolAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116" && p.Year == year2)?.IsAbnormal;
                string MethanolAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(MethanolAbnormal1))
                {
                    page4.cellMethanol1.ForeColor = (MethanolAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellMethanol1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(MethanolAbnormal2))
                {
                    page4.cellMethanol2.ForeColor = (MethanolAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellMethanol2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(MethanolAbnormal3))
                {
                    page4.cellMethanol3.ForeColor = (MethanolAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellMethanol3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellMethyreneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119")?.ReferenceRange;
                page4.cellMethyrene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119" && p.Year == year1)?.ResultValue;
                page4.cellMethyrene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119" && p.Year == year2)?.ResultValue;
                page4.cellMethyrene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119" && p.Year == year3)?.ResultValue;

                string MethyreneAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119" && p.Year == year1)?.IsAbnormal;
                string MethyreneAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119" && p.Year == year2)?.IsAbnormal;
                string MethyreneAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(MethyreneAbnormal1))
                {
                    page4.cellMethyrene1.ForeColor = (MethyreneAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellMethyrene1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(MethyreneAbnormal2))
                {
                    page4.cellMethyrene2.ForeColor = (MethyreneAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellMethyrene2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(MethyreneAbnormal3))
                {
                    page4.cellMethyrene3.ForeColor = (MethyreneAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellMethyrene3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellAcetoneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117")?.ReferenceRange;
                page4.cellAcetone1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117" && p.Year == year1)?.ResultValue;
                page4.cellAcetone2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117" && p.Year == year2)?.ResultValue;
                page4.cellAcetone3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117" && p.Year == year3)?.ResultValue;

                string AcetoneAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117" && p.Year == year1)?.IsAbnormal;
                string AcetoneAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117" && p.Year == year2)?.IsAbnormal;
                string AcetoneAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(AcetoneAbnormal1))
                {
                    page4.cellAcetone1.ForeColor = (AcetoneAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellAcetone1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AcetoneAbnormal2))
                {
                    page4.cellAcetone2.ForeColor = (AcetoneAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellAcetone2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AcetoneAbnormal3))
                {
                    page4.cellAcetone3.ForeColor = (AcetoneAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellAcetone3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellHexaneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118")?.ReferenceRange;
                page4.cellHexane1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118" && p.Year == year1)?.ResultValue;
                page4.cellHexane2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118" && p.Year == year2)?.ResultValue;
                page4.cellHexane3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118" && p.Year == year3)?.ResultValue;

                string HexaneAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118" && p.Year == year1)?.IsAbnormal;
                string HexaneAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118" && p.Year == year2)?.IsAbnormal;
                string HexaneAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(HexaneAbnormal1))
                {
                    page4.cellHexane1.ForeColor = (HexaneAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellHexane1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(HexaneAbnormal2))
                {
                    page4.cellHexane2.ForeColor = (HexaneAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellHexane2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(HexaneAbnormal3))
                {
                    page4.cellHexane3.ForeColor = (HexaneAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellHexane3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellIsopropanolRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130")?.ReferenceRange;
                page4.cellIsopropanol1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year1)?.ResultValue;
                page4.cellIsopropanol2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year2)?.ResultValue;
                page4.cellIsopropanol3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year3)?.ResultValue;

                string IsopropanolAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year1)?.IsAbnormal;
                string IsopropanolAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year2)?.IsAbnormal;
                string IsopropanolAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(IsopropanolAbnormal1))
                {
                    page4.cellIsopropanol1.ForeColor = (IsopropanolAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellIsopropanol1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(IsopropanolAbnormal2))
                {
                    page4.cellIsopropanol2.ForeColor = (IsopropanolAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellIsopropanol2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(IsopropanolAbnormal3))
                {
                    page4.cellIsopropanol3.ForeColor = (IsopropanolAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellIsopropanol3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
            }
            else
            {
                page4.cellToxicoYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page4.cellToxicoYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page4.cellToxicoYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
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
                page5.cellStoolYear1.Text = "ปี" + " " + year1.ToString();
                page5.cellStoolYear2.Text = "ปี" + " " + year2.ToString();
                page5.cellStoolYear3.Text = "ปี" + " " + year3.ToString();

                page5.cellStColorRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080")?.ReferenceRange;
                page5.cellStColor1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year1)?.ResultValue;
                page5.cellStColor2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year2)?.ResultValue;
                page5.cellStColor3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year3)?.ResultValue;

                string StColorAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year1)?.IsAbnormal;
                string StColorAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year2)?.IsAbnormal;
                string StColorAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(StColorAbnormal1))
                {
                    page5.cellStColor1.ForeColor = (StColorAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellStColor1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(StColorAbnormal2))
                {
                    page5.cellStColor2.ForeColor = (StColorAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellStColor2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(StColorAbnormal3))
                {
                    page5.cellStColor3.ForeColor = (StColorAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellStColor3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page5.cellStappearRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21")?.ReferenceRange;
                page5.cellStappear1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year1)?.ResultValue;
                page5.cellStappear2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year2)?.ResultValue;
                page5.cellStappear2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year3)?.ResultValue;

                string StappearAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year1)?.IsAbnormal;
                string StappearAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year2)?.IsAbnormal;
                string StappearAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(StappearAbnormal1))
                {
                    page5.cellStappear1.ForeColor = (StappearAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page5.cellStappear1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(StappearAbnormal2))
                {
                    page5.cellStappear2.ForeColor = (StappearAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page5.cellStappear2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(StappearAbnormal3))
                {
                    page5.cellStappear3.ForeColor = (StappearAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page5.cellStappear3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
            }
            else
            {
                page5.cellStoolYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page5.cellStoolYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page5.cellStoolYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }

        private void GenerateOther(IEnumerable<PatientResultLabModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {

                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;
                page4.cellOtherYear1.Text = "ปี" + " " + year1.ToString();
                page4.cellOtherYear2.Text = "ปี" + " " + year2.ToString();
                page4.cellOtherYear3.Text = "ปี" + " " + year3.ToString();

                page4.cellAfpRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38")?.ReferenceRange;
                page4.cellAfp1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38" && p.Year == year1)?.ResultValue;
                page4.cellAfp2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38" && p.Year == year2)?.ResultValue;
                page4.cellAfp3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38" && p.Year == year3)?.ResultValue;

                string afpAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38" && p.Year == year1)?.IsAbnormal;
                string afpAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38" && p.Year == year2)?.IsAbnormal;
                string afpAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(afpAbnormal1))
                {
                    page4.cellAfp1.ForeColor = (afpAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellAfp1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(afpAbnormal2))
                {
                    page4.cellAfp2.ForeColor = (afpAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellAfp2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(afpAbnormal3))
                {
                    page4.cellAfp3.ForeColor = (afpAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellAfp3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellAfpConRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39")?.ReferenceRange;
                page4.cellAfpCon1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year1)?.ResultValue;
                page4.cellAfpCon2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year2)?.ResultValue;
                page4.cellAfpCon3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year3)?.ResultValue;

                string AfpConAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year1)?.IsAbnormal;
                string AfpConAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year2)?.IsAbnormal;
                string AfpConAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(AfpConAbnormal1))
                {
                    page4.cellAfpCon1.ForeColor = (AfpConAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellAfpCon1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AfpConAbnormal2))
                {
                    page4.cellAfpCon2.ForeColor = (AfpConAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellAfpCon2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AfpConAbnormal3))
                {
                    page4.cellAfpCon3.ForeColor = (AfpConAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellAfpCon3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellAboGroupRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32")?.ReferenceRange;
                page4.cellAboGroup1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32" && p.Year == year1)?.ResultValue;
                page4.cellAboGroup2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32" && p.Year == year2)?.ResultValue;
                page4.cellAboGroup3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32" && p.Year == year3)?.ResultValue;

                string AboGroupAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32" && p.Year == year1)?.IsAbnormal;
                string AboGroupAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32" && p.Year == year2)?.IsAbnormal;
                string AboGroupAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR32" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(AboGroupAbnormal1))
                {
                    page4.cellAboGroup1.ForeColor = (AboGroupAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellAboGroup1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AboGroupAbnormal2))
                {
                    page4.cellAboGroup2.ForeColor = (AboGroupAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellAboGroup2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(AboGroupAbnormal3))
                {
                    page4.cellAboGroup3.ForeColor = (AboGroupAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellAboGroup3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellBloodGroupRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR43")?.ReferenceRange;
                page4.cellBloodGroup1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR43" && p.Year == year1)?.ResultValue;
                page4.cellBloodGroup2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR43" && p.Year == year2)?.ResultValue;
                page4.cellBloodGroup3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR43" && p.Year == year3)?.ResultValue;

                string BloodGroupAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR43" && p.Year == year1)?.IsAbnormal;
                string BloodGroupAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR43" && p.Year == year2)?.IsAbnormal;
                string BloodGroupAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR43" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(BloodGroupAbnormal1))
                {
                    page4.cellBloodGroup1.ForeColor = (BloodGroupAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellBloodGroup1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BloodGroupAbnormal2))
                {
                    page4.cellBloodGroup2.ForeColor = (BloodGroupAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellBloodGroup2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(BloodGroupAbnormal3))
                {
                    page4.cellBloodGroup3.ForeColor = (BloodGroupAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellBloodGroup3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellCaRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114")?.ReferenceRange;
                page4.cellCa1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year1)?.ResultValue;
                page4.cellCa2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year2)?.ResultValue;
                page4.cellCa3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year3)?.ResultValue;

                string caAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year1)?.IsAbnormal;
                string caAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year2)?.IsAbnormal;
                string caAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(caAbnormal1))
                {
                    page4.cellCa1.ForeColor = (caAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellCa1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(caAbnormal2))
                {
                    page4.cellCa2.ForeColor = (caAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellCa2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(caAbnormal3))
                {
                    page4.cellCa3.ForeColor = (caAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellCa3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                page4.cellPsaRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4")?.ReferenceRange;
                page4.cellPsa1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4" && p.Year == year1)?.ResultValue;
                page4.cellPsa2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4" && p.Year == year2)?.ResultValue;
                page4.cellPsa3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4" && p.Year == year3)?.ResultValue;

                string psaAbnormal1 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4" && p.Year == year1)?.IsAbnormal;
                string psaAbnormal2 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4" && p.Year == year2)?.IsAbnormal;
                string psaAbnormal3 = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4" && p.Year == year3)?.IsAbnormal;

                if (!string.IsNullOrEmpty(psaAbnormal1))
                {
                    page4.cellPsa1.ForeColor = (psaAbnormal1 == "H") ? Color.Red : Color.Blue;
                    page4.cellPsa1.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(psaAbnormal2))
                {
                    page4.cellPsa2.ForeColor = (psaAbnormal2 == "H") ? Color.Red : Color.Blue;
                    page4.cellPsa2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(psaAbnormal3))
                {
                    page4.cellPsa3.ForeColor = (psaAbnormal3 == "H") ? Color.Red : Color.Blue;
                    page4.cellPsa3.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
            }
            else
            {
                page4.cellOtherYear1.Text = "ปี" + " " + DateTime.Now.Year;
                page4.cellOtherYear2.Text = "ปี" + " " + (DateTime.Now.Year + 1);
                page4.cellOtherYear3.Text = "ปี" + " " + (DateTime.Now.Year + 2);
            }
        }


            private void BookPage1_AfterPrint(object sender, EventArgs e)
        {
            page2.CreateDocument();
            page3.CreateDocument();
            page4.CreateDocument();
            page5.CreateDocument();
            page6.CreateDocument();
            this.Pages.AddRange(page2.Pages);
            this.Pages.AddRange(page3.Pages);
            this.Pages.AddRange(page4.Pages);
            this.Pages.AddRange(page5.Pages);
            this.Pages.AddRange(page6.Pages);
        }

        public string TranslateXray(string resultValue, string resultStatus, string requestItemName)
        {
            if (dtResultMapping == null)
            {
                dtResultMapping = DataService.Radiology.GetXrayTranslateMapping();
            }

            List<string> listNoMapResult = new List<string>();
            string thairesult = TranslateResult.TranslateResultXray(resultValue, resultStatus, requestItemName,",", dtResultMapping, ref listNoMapResult);

            return thairesult;
        }
    }
}

