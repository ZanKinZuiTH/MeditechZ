using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using MediTech.Model;
using System.Collections.Generic;
using System.Linq;
using MediTech.Helpers;

namespace MediTech.Reports.Operating.Patient.RiskBook
{
    public partial class RiskBook1 : DevExpress.XtraReports.UI.XtraReport
    {
        private MediTechDataService _DataService;

        public MediTechDataService DataService
        {
            get { return _DataService ?? (_DataService = new MediTechDataService()); }
        }

        List<XrayTranslateMappingModel> dtResultMapping;

        RiskBook2 page2 = new RiskBook2();
        PatientName page3 = new PatientName();
        RiskBook4 page4 = new RiskBook4();

        public RiskBook1()
        {
            InitializeComponent();
            BeforePrint += Riskbook1_BeforePrint;
            AfterPrint += Riskbook1_AfterPrint;
        }
        private void Riskbook1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            long patientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long patientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            int payorDetailUID = int.Parse(this.Parameters["PayorDetailUID"].Value.ToString());
            List<CheckupBookModel> data = DataService.Reports.PrintCheckupBook(patientUID, payorDetailUID);

            if (data != null && data.Count > 0)
            {
                lbRunNo.Text = data.FirstOrDefault().PatientID;
                lbName.Text = data.FirstOrDefault().PatientName;
                lbCompany.Text = data.FirstOrDefault().PayorName;
                lbDeparment.Text = data.FirstOrDefault().Department;
                lbEmployeeID.Text = data.FirstOrDefault().EmployeeID;
                lbDateVisit.Text = data.FirstOrDefault().StartDttm != null ? data.FirstOrDefault().StartDttm.Value.AddYears(543).ToString("dd/MM/yyyy") : "";

                page2.lbNameP2.Text = data.FirstOrDefault().PatientName;
                page2.lbBrithDateP2.Text = data.FirstOrDefault().BirthDttm != null ? data.FirstOrDefault().BirthDttm.Value.AddYears(543).ToString("dd/MM/yyyy") : "";
                page2.lbIDCard.Text = data.FirstOrDefault().NationalID;
                page2.lbGenderP2.Text = data.FirstOrDefault().Gender;

                page4.lbBP.Text = (data.FirstOrDefault().BPSys != null ? data.FirstOrDefault().BPSys.ToString() : "") + (data.FirstOrDefault().BPDio != null ? "/" + data.FirstOrDefault().BPDio.ToString() : "");
                page4.lbPulse.Text = data.FirstOrDefault().Pulse != null ? data.FirstOrDefault().Pulse.ToString() + " ครั้ง/นาที" : "";
                page4.lbHeight.Text = data.FirstOrDefault().Height != null ? data.FirstOrDefault().Height.ToString() + " cm." : "";
                page4.lbWeight.Text = data.FirstOrDefault().Weight != null ? data.FirstOrDefault().Weight.ToString() + " kg." : "";
                page4.lbBMI.Text = data.FirstOrDefault().BMI != null ? data.FirstOrDefault().BMI.ToString() + " kg/m2" : "";

                page4.lbEye.Text = data.FirstOrDefault().Eyes != null ? data.FirstOrDefault().Eyes.ToString() : "";
                page4.lbEar.Text = data.FirstOrDefault().Ears != null ? data.FirstOrDefault().Ears.ToString() : "";
                page4.lbThroat.Text = data.FirstOrDefault().Throat != null ? data.FirstOrDefault().Throat.ToString() : "";
                page4.lbNose.Text = data.FirstOrDefault().Nose != null ? data.FirstOrDefault().Nose.ToString() : "";
                page4.lbTeeth.Text = data.FirstOrDefault().Teeth != null ? data.FirstOrDefault().Teeth.ToString() : "";
                page4.lbLung.Text = data.FirstOrDefault().Lung != null ? data.FirstOrDefault().Lung.ToString() : "";
                page4.lbHeart.Text = data.FirstOrDefault().Heart != null ? data.FirstOrDefault().Heart.ToString() : "";
                page4.lbSkin.Text = data.FirstOrDefault().Skin != null ? data.FirstOrDefault().Skin.ToString() : "";
                page4.lbThyroid.Text = data.FirstOrDefault().Thyroid != null ? data.FirstOrDefault().Thyroid.ToString() : "";
                page4.lbSmoke.Text = data.FirstOrDefault().Smoke != null ? data.FirstOrDefault().Smoke.ToString() : "";
                page4.lbDrugAllergy.Text = data.FirstOrDefault().DrugAllergy != null ? data.FirstOrDefault().DrugAllergy.ToString() : "";
                page4.lbAlcohol.Text = data.FirstOrDefault().Alcohol != null ? data.FirstOrDefault().Alcohol.ToString() : "";
                page4.lbPastHistory.Text = data.FirstOrDefault().PersonalHistory != null ? data.FirstOrDefault().PersonalHistory.ToString() : "";
                page4.lbLymphNode.Text = data.FirstOrDefault().LymphNode != null ? data.FirstOrDefault().LymphNode.ToString() : "";

                page3.PtName.Text = data.FirstOrDefault().PatientName;
                page4.dateStart.Text = data.FirstOrDefault().StartDttm != null ? data.FirstOrDefault().StartDttm.Value.AddYears(543).ToString("dd/MM/yyyy") : "";

                #region Lung occmed

                page4.lbFVCPer1.Text = data.FirstOrDefault().FVCPer != null ? data.FirstOrDefault().FVCPer.ToString() + " %" : "";
                page4.lbFEV1Per1.Text = data.FirstOrDefault().FEV1Per != null ? data.FirstOrDefault().FEV1Per.ToString() + " %" : "";
                page4.lbFEVPer1.Text = data.FirstOrDefault().FEV1FVCPer != null ? data.FirstOrDefault().FEV1FVCPer.ToString() + " %" : "";


                #endregion

                #region vision occmed


                page3.lbFarVision1.Text = data.FirstOrDefault().FarPoint != null ? data.FirstOrDefault().FarPoint.ToString() : "";

                page3.lbNearVision1.Text = data.FirstOrDefault().NearPoint != null ? data.FirstOrDefault().NearPoint.ToString() : "";

                page3.lb3DVision1.Text = data.FirstOrDefault().Depth != null ? data.FirstOrDefault().Depth.ToString() : "";

                page3.lbBalanceEye1.Text = data.FirstOrDefault().Muscle != null ? data.FirstOrDefault().Muscle.ToString() : "";

                page3.lbVisionColor1.Text = data.FirstOrDefault().Color != null ? data.FirstOrDefault().Color.ToString() : "";

                page3.lbFieldVision1.Text = data.FirstOrDefault().Visualfield != null ? data.FirstOrDefault().Visualfield.ToString() : "";



                #endregion

                #region Audio Test
                page4.R5001.Text = data.FirstOrDefault().R500Hz != null ? data.FirstOrDefault().R500Hz.ToString() : "";
                page4.R10001.Text = data.FirstOrDefault().R1000Hz != null ? data.FirstOrDefault().R1000Hz.ToString() : "";
                page4.R20001.Text = data.FirstOrDefault().R2000Hz != null ? data.FirstOrDefault().R2000Hz.ToString() : "";
                page4.R30001.Text = data.FirstOrDefault().R3000Hz != null ? data.FirstOrDefault().R3000Hz.ToString() : "";
                page4.R40001.Text = data.FirstOrDefault().R4000Hz != null ? data.FirstOrDefault().R4000Hz.ToString() : "";
                page4.R60001.Text = data.FirstOrDefault().R6000Hz != null ? data.FirstOrDefault().R6000Hz.ToString() : "";
                page4.R80001.Text = data.FirstOrDefault().R8000Hz != null ? data.FirstOrDefault().R8000Hz.ToString() : "";

                page4.L5001.Text = data.FirstOrDefault().L500Hz != null ? data.FirstOrDefault().L500Hz.ToString() : "";
                page4.L10001.Text = data.FirstOrDefault().L1000Hz != null ? data.FirstOrDefault().L1000Hz.ToString() : "";
                page4.L20001.Text = data.FirstOrDefault().L2000Hz != null ? data.FirstOrDefault().L2000Hz.ToString() : "";
                page4.L30001.Text = data.FirstOrDefault().L3000Hz != null ? data.FirstOrDefault().L3000Hz.ToString() : "";
                page4.L40001.Text = data.FirstOrDefault().L4000Hz != null ? data.FirstOrDefault().L4000Hz.ToString() : "";
                page4.L60001.Text = data.FirstOrDefault().L6000Hz != null ? data.FirstOrDefault().L6000Hz.ToString() : "";
                page4.L80001.Text = data.FirstOrDefault().L8000Hz != null ? data.FirstOrDefault().L8000Hz.ToString() : "";

                #endregion

                #region Result Wellness

                page4.lbResult.Text = data.FirstOrDefault().WellnessResult;
                if (page4.lbResult.Text != null && page4.lbResult.Text.Length > 2000)
                {
                    page4.lbResult.Font = new Font("Angsana New", 8);
                }
                else if (page4.lbResult.Text != null && page4.lbResult.Text.Length > 1670)
                {
                    page4.lbResult.Font = new Font("Angsana New", 9);
                }
                else if (page4.lbResult.Text != null && page4.lbResult.Text.Length > 1350)
                {
                    page4.lbResult.Font = new Font("Angsana New", 10);
                }
                #endregion

                #region radiology
                if (data.FirstOrDefault(p => !string.IsNullOrEmpty(p.RequestItemName)) != null)
                {

                    if (data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("chest") && p.RequestItemType == "Radiology") != null)
                    {
                        CheckupBookModel chestXray = data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("chest") && p.RequestItemType == "Radiology");
                        if (!string.IsNullOrEmpty(chestXray.RadiologyResultText))
                        {
                            string resultChestThai = TranslateXray(chestXray.RadiologyResultText, chestXray.RadiologyResultStatus, chestXray.RequestItemName);
                            if (!string.IsNullOrEmpty(resultChestThai))
                            {
                                page4.lbChest.Text = resultChestThai;
                            }
                            else
                            {
                                page4.lbChest.Text = "ยังไม่ได้แปลไทย";
                            }
                        }

                    }
                    else
                    {
                        page4.lbChest.Text = "-";
                    }

                    if (data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("mammo") && p.RequestItemType == "Radiology") != null)
                    {
                        CheckupBookModel mammoGram = data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("mammo") && p.RequestItemType == "Radiology");
                        page4.lbMam.Text = mammoGram.RadiologyResultStatus;
                        if (!string.IsNullOrEmpty(mammoGram.RadiologyResultText))
                        {
                            string resultChestThai = TranslateXray(mammoGram.RadiologyResultText, mammoGram.RadiologyResultStatus, mammoGram.RequestItemName);
                            if (!string.IsNullOrEmpty(resultChestThai))
                            {
                                page4.lbMam.Text = resultChestThai;
                            }
                            else
                            {
                                page4.lbMam.Text = "ยังไม่ได้แปลไทย";
                            }
                        }
                    }
                    else
                    {
                        page4.lbMam.Text = "-";
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
                }
                #endregion

                page4.lbEKGRecommend.Text = data.FirstOrDefault().EkgConclusion != null ? data.FirstOrDefault().EkgConclusion.ToString() : "";

                List<PatientResultComponentModel> labCompare = DataService.Reports.CheckupLabCompare(patientUID, payorDetailUID);
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
                        || p.RequestItemCode.Contains("LAB211"))
                        .OrderBy(p => p.Year);
                    GenerateRenalFunction(RenalTestSet);

                    #endregion

                    #region Immunology and Virology
                    IEnumerable<PatientResultComponentModel> ImmunologyTestSet = labCompare
                        .Where(p => p.RequestItemCode.Contains("LAB451")
                        || p.RequestItemCode.Contains("LAB441")
                        || p.RequestItemCode.Contains("LAB452"))
                        .OrderBy(p => p.Year);
                    GenerateImmunology(ImmunologyTestSet);
                    #endregion

                    #region Sugar
                    IEnumerable<PatientResultComponentModel> SugarTestSet = labCompare
                        .Where(p => p.RequestItemCode.Contains("LAB231")
                        || p.RequestItemCode.Contains("LAB241")
                        || p.RequestItemCode.Contains("LAB242")
                        || p.RequestItemCode.Contains("LAB243")
                        || p.RequestItemCode.Contains("LAB244")
                        || p.RequestItemCode.Contains("LAB261"))
                        .OrderBy(p => p.Year);
                    GenerateSugar(SugarTestSet);
                    #endregion

                    #region Liver Function
                    IEnumerable<PatientResultComponentModel> LiverTestSet = labCompare
                        .Where(p => p.RequestItemCode.Contains("LAB474")
                        || p.RequestItemCode.Contains("LAB475")
                        || p.RequestItemCode.Contains("LAB223")
                        || p.RequestItemCode.Contains("LAB221")
                        || p.RequestItemCode.Contains("LAB222")
                        || p.RequestItemCode.Contains("LAB225")
                        || p.RequestItemCode.Contains("LAB226")
                        || p.RequestItemCode.Contains("LAB503"))
                        .OrderBy(p => p.Year);
                    GenerateLiverFunction(LiverTestSet);
                    #endregion

                    #region Tumor Marker
                    IEnumerable<PatientResultComponentModel> TumorMarkerTestSet = labCompare
                        .Where(p => p.RequestItemCode.Contains("LAB281")
                        || p.RequestItemCode.Contains("LAB283")
                        || p.RequestItemCode.Contains("LAB284")
                        || p.RequestItemCode.Contains("LAB285")
                        || p.RequestItemCode.Contains("LAB282"))
                        .OrderBy(p => p.Year);
                    GenerateTumorMarkerTestSet(TumorMarkerTestSet);
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
                        || p.RequestItemCode.Contains("LAB315")
                        || p.RequestItemCode.Contains("LAB317")
                        || p.RequestItemCode.Contains("LAB325")
                        || p.RequestItemCode.Contains("LAB323")
                        || p.RequestItemCode.Contains("LAB324")
                        || p.RequestItemCode.Contains("LAB519")
                        || p.RequestItemCode.Contains("LAB558")
                        || p.RequestItemCode.Contains("LAB518"))
                        .OrderBy(p => p.Year);
                    GenerateToxicology(ToxicoTestSet);
                    #endregion
                }
            }
        }

        private void GenerateCompleteBloodCount(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;
                page3.cellYear1.Text = "ปี" + " " + (year1 + 543).ToString();
                page3.cellYear2.Text = "ปี" + " " + (year2 + 543).ToString();
                page3.cellYear3.Text = "ปี" + " " + (year3 + 543).ToString();

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

                page3.cellWbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006")?.ReferenceRange;
                page3.cellWbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006" && p.Year == year1)?.ResultValue;
                page3.cellWbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006" && p.Year == year2)?.ResultValue;
                page3.cellWbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006" && p.Year == year3)?.ResultValue;

                page3.cellPmnRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040")?.ReferenceRange;
                page3.cellPmn1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040" && p.Year == year1)?.ResultValue;
                page3.cellPmn2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040" && p.Year == year2)?.ResultValue;
                page3.cellPmn3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040" && p.Year == year3)?.ResultValue;

                page3.cellLRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050")?.ReferenceRange;
                page3.cellL1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050" && p.Year == year1)?.ResultValue;
                page3.cellL2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050" && p.Year == year2)?.ResultValue;
                page3.cellL3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050" && p.Year == year3)?.ResultValue;

                page3.cellMRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060")?.ReferenceRange;
                page3.cellM1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060" && p.Year == year1)?.ResultValue;
                page3.cellM2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060" && p.Year == year2)?.ResultValue;
                page3.cellM3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060" && p.Year == year3)?.ResultValue;

                page3.cellEoRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070")?.ReferenceRange;
                page3.cellEo1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070" && p.Year == year1)?.ResultValue;
                page3.cellEo2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070" && p.Year == year2)?.ResultValue;
                page3.cellEo3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070" && p.Year == year3)?.ResultValue;

                page3.cellBaRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080")?.ReferenceRange;
                page3.cellBa1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080" && p.Year == year1)?.ResultValue;
                page3.cellBa2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080" && p.Year == year2)?.ResultValue;
                page3.cellBa3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080" && p.Year == year3)?.ResultValue;

                page3.cellPltRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010")?.ReferenceRange;
                page3.cellPlt1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010" && p.Year == year1)?.ResultValue;
                page3.cellPlt2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010" && p.Year == year2)?.ResultValue;
                page3.cellPlt3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010" && p.Year == year3)?.ResultValue;

                page3.cellRbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428")?.ReferenceRange;
                page3.cellRbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428" && p.Year == year1)?.ResultValue;
                page3.cellRbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428" && p.Year == year2)?.ResultValue;
                page3.cellRbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428" && p.Year == year3)?.ResultValue;
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
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2;

                page3.cellcolorRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080")?.ReferenceRange;
                page3.cellcolor1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year1)?.ResultValue;
                page3.cellcolor2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year2)?.ResultValue;
                page3.cellcolor3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year3)?.ResultValue;

                page3.cellClarityRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21")?.ReferenceRange;
                page3.cellClarity1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year1)?.ResultValue;
                page3.cellClarity2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year2)?.ResultValue;
                page3.cellClarity3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year3)?.ResultValue;

                page3.cellSpecificityRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001")?.ReferenceRange;
                page3.cellSpecificity1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001" && p.Year == year1)?.ResultValue;
                page3.cellSpecificity2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001" && p.Year == year2)?.ResultValue;
                page3.cellSpecificity3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001" && p.Year == year3)?.ResultValue;

                page3.cellKetoneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047")?.ReferenceRange;
                page3.cellKetone1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047" && p.Year == year1)?.ResultValue;
                page3.cellKetone2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047" && p.Year == year2)?.ResultValue;
                page3.cellKetone3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047" && p.Year == year3)?.ResultValue;

                page3.cellSugarRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090")?.ReferenceRange;
                page3.cellSugar1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090" && p.Year == year1)?.ResultValue;
                page3.cellSugar2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090" && p.Year == year2)?.ResultValue;
                page3.cellSugar3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090" && p.Year == year3)?.ResultValue;

                page3.cellProteinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085")?.ReferenceRange;
                page3.cellProtein1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085" && p.Year == year1)?.ResultValue;
                page3.cellProtein2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085" && p.Year == year2)?.ResultValue;
                page3.cellProtein3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085" && p.Year == year3)?.ResultValue;

                page3.cellPhRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080")?.ReferenceRange;
                page3.cellPh1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080" && p.Year == year1)?.ResultValue;
                page3.cellPh2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080" && p.Year == year2)?.ResultValue;
                page3.cellPh3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080" && p.Year == year3)?.ResultValue;

                page3.cellUARbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260")?.ReferenceRange;
                page3.cellUARbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260" && p.Year == year1)?.ResultValue;
                page3.cellUARbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260" && p.Year == year2)?.ResultValue;
                page3.cellUARbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260" && p.Year == year3)?.ResultValue;

                page3.cellUAWbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250")?.ReferenceRange;
                page3.cellUAWbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250" && p.Year == year1)?.ResultValue;
                page3.cellUAWbc2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250" && p.Year == year2)?.ResultValue;
                page3.cellUAWbc3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250" && p.Year == year3)?.ResultValue;

                page3.cellEpiRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270")?.ReferenceRange;
                page3.cellEpi1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270" && p.Year == year1)?.ResultValue;
                page3.cellEpi2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270" && p.Year == year2)?.ResultValue;
                page3.cellEpi3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270" && p.Year == year3)?.ResultValue;

                page3.cellBloodRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152")?.ReferenceRange;
                page3.cellBlood1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152" && p.Year == year1)?.ResultValue;
                page3.cellBlood2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152" && p.Year == year2)?.ResultValue;
                page3.cellBlood3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152" && p.Year == year3)?.ResultValue;
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

                page3.cellHBsAgRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35")?.ReferenceRange;
                page3.cellHBsAg1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35" && p.Year == year1)?.ResultValue;
                page3.cellHBsAg2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35" && p.Year == year2)?.ResultValue;
                page3.cellHBsAg3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35" && p.Year == year3)?.ResultValue;

                page3.cellHBsAbRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42")?.ReferenceRange;
                page3.cellHBsAb1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42" && p.Year == year1)?.ResultValue;
                page3.cellHBsAb2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42" && p.Year == year2)?.ResultValue;
                page3.cellHBsAb3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42" && p.Year == year3)?.ResultValue;

                page3.cellAntiHCVRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR53")?.ReferenceRange;
                page3.cellAntiHCV1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR53" && p.Year == year1)?.ResultValue;
                page3.cellAntiHCV2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR53" && p.Year == year2)?.ResultValue;
                page3.cellAntiHCV3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR53" && p.Year == year3)?.ResultValue;


            }
        }

        private void GenerateSugar(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;

                page4.cellSugarYear1.Text = "ปี" + " " + (year1 + 543).ToString();
                page4.cellSugarYear2.Text = "ปี" + " " + (year2 + 543).ToString();
                page4.cellSugarYear3.Text = "ปี" + " " + (year3 + 543).ToString();

                page4.cellFBSRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0180")?.ReferenceRange;
                page4.cellFBS1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0180" && p.Year == year1)?.ResultValue;
                page4.cellFBS2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0180" && p.Year == year2)?.ResultValue;
                page4.cellFBS3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0180" && p.Year == year3)?.ResultValue;

                page4.cellCholRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130")?.ReferenceRange;
                page4.cellChol1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130" && p.Year == year1)?.ResultValue;
                page4.cellChol2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130" && p.Year == year2)?.ResultValue;
                page4.cellChol3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130" && p.Year == year3)?.ResultValue;

                page4.cellCholRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130")?.ReferenceRange;
                page4.cellChol1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130" && p.Year == year1)?.ResultValue;
                page4.cellChol2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130" && p.Year == year2)?.ResultValue;
                page4.cellChol3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130" && p.Year == year3)?.ResultValue;

                page4.cellTGRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140")?.ReferenceRange;
                page4.cellTG1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140" && p.Year == year1)?.ResultValue;
                page4.cellTG2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140" && p.Year == year2)?.ResultValue;
                page4.cellTG3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140" && p.Year == year3)?.ResultValue;

                page4.cellHDLRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150")?.ReferenceRange;
                page4.cellHDL1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150" && p.Year == year1)?.ResultValue;
                page4.cellHDL2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150" && p.Year == year2)?.ResultValue;
                page4.cellHDL3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150" && p.Year == year3)?.ResultValue;

                page4.cellLDLRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159")?.ReferenceRange;
                page4.cellLDL1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159" && p.Year == year1)?.ResultValue;
                page4.cellLDL2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159" && p.Year == year2)?.ResultValue;
                page4.cellLDL3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159" && p.Year == year3)?.ResultValue;

                page4.cellUricRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320")?.ReferenceRange;
                page4.cellUric1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320" && p.Year == year1)?.ResultValue;
                page4.cellUric2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320" && p.Year == year2)?.ResultValue;
                page4.cellUric3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320" && p.Year == year3)?.ResultValue;
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

                page3.cellBunRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27")?.ReferenceRange;
                page3.cellBun1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27" && p.Year == year1)?.ResultValue;
                page3.cellBun2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27" && p.Year == year2)?.ResultValue;
                page3.cellBun3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27" && p.Year == year3)?.ResultValue;

                page3.cellCrRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070")?.ReferenceRange;
                page3.cellCr1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070" && p.Year == year1)?.ResultValue;
                page3.cellCr2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070" && p.Year == year2)?.ResultValue;
                page3.cellCr3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070" && p.Year == year3)?.ResultValue;
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

                page4.cellBilirubinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48")?.ReferenceRange;
                page4.cellBilirubin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48" && p.Year == year1)?.ResultValue;
                page4.cellBilirubin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48" && p.Year == year2)?.ResultValue;
                page4.cellBilirubin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48" && p.Year == year3)?.ResultValue;

                page4.cellDirectRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49")?.ReferenceRange;
                page4.cellDirect1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year1)?.ResultValue;
                page4.cellDirect2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year2)?.ResultValue;
                page4.cellDirect3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year3)?.ResultValue;

                page4.cellAlkRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33")?.ReferenceRange;
                page4.cellAlk1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33" && p.Year == year1)?.ResultValue;
                page4.cellAlk2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33" && p.Year == year2)?.ResultValue;
                page4.cellAlk3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33" && p.Year == year3)?.ResultValue;

                page4.cellSgotRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50")?.ReferenceRange;
                page4.cellSgot1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50" && p.Year == year1)?.ResultValue;
                page4.cellSgot2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50" && p.Year == year2)?.ResultValue;
                page4.cellSgot3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50" && p.Year == year3)?.ResultValue;

                page4.cellSgptRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51")?.ReferenceRange;
                page4.cellSgpt1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51" && p.Year == year1)?.ResultValue;
                page4.cellSgpt2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51" && p.Year == year2)?.ResultValue;
                page4.cellSgpt3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51" && p.Year == year3)?.ResultValue;

                page4.cellAlbuminRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49")?.ReferenceRange;
                page4.cellAlbumin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year1)?.ResultValue;
                page4.cellAlbumin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year2)?.ResultValue;
                page4.cellAlbumin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year3)?.ResultValue;

                page4.cellGlobulinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46")?.ReferenceRange;
                page4.cellGlobulin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year1)?.ResultValue;
                page4.cellGlobulin2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year2)?.ResultValue;
                page4.cellGlobulin3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year3)?.ResultValue;

                page4.cellProteinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105")?.ReferenceRange;
                page4.cellProtein1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105" && p.Year == year1)?.ResultValue;
                page4.cellProtein2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105" && p.Year == year2)?.ResultValue;
                page4.cellProtein3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105" && p.Year == year3)?.ResultValue;

            }
        }

        private void GenerateTumorMarkerTestSet(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;

                page4.cellTumorYear1.Text = "ปี" + " " + (year1 + 543).ToString();
                page4.cellTumorYear2.Text = "ปี" + " " + (year2 + 543).ToString();
                page4.cellTumorYear3.Text = "ปี" + " " + (year3 + 543).ToString();

                page4.cellAFPRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39")?.ReferenceRange;
                page4.cellAFP1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year1)?.ResultValue;
                page4.cellAFP2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year2)?.ResultValue;
                page4.cellAFP3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year3)?.ResultValue;

                page4.cellCEARange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR40")?.ReferenceRange;
                page4.cellCEA1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR40" && p.Year == year1)?.ResultValue;
                page4.cellCEA2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR40" && p.Year == year2)?.ResultValue;
                page4.cellCEA3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR40" && p.Year == year3)?.ResultValue;

                page4.cellPSARange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4")?.ReferenceRange;
                page4.cellPSA1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4" && p.Year == year1)?.ResultValue;
                page4.cellPSA2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4" && p.Year == year2)?.ResultValue;
                page4.cellPSA3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4" && p.Year == year3)?.ResultValue;

                page4.cellCARange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41")?.ReferenceRange;
                page4.cellCA1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41" && p.Year == year1)?.ResultValue;
                page4.cellCA2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41" && p.Year == year2)?.ResultValue;
                page4.cellCA3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41" && p.Year == year3)?.ResultValue;

                page4.cellCA19Range.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114")?.ReferenceRange;
                page4.cellCA191.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year1)?.ResultValue;
                page4.cellCA192.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year2)?.ResultValue;
                page4.cellCA193.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year3)?.ResultValue;

                page4.cellCA19Range.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114")?.ReferenceRange;
                page4.cellCA191.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year1)?.ResultValue;
                page4.cellCA192.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year2)?.ResultValue;
                page4.cellCA193.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR114" && p.Year == year3)?.ResultValue;
            }
        }

        private void GenerateToxicology(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().ToList();
                Years.Sort();
                int countYear = Years.Count();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;

                page3.cellToxicoYear1.Text = "ปี" + " " + (year1 + 543).ToString();
                page3.cellToxicoYear2.Text = "ปี" + " " + (year2 + 543).ToString();
                page3.cellToxicoYear3.Text = "ปี" + " " + (year3 + 543).ToString();

                page3.cellAluminiumRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122")?.ReferenceRange;
                page3.cellAluminium1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year1)?.ResultValue;
                page3.cellAluminium2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year2)?.ResultValue;
                page3.cellAluminium3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year3)?.ResultValue;

                page3.cellTolueneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124")?.ReferenceRange;
                page3.cellToluene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year1)?.ResultValue;
                page3.cellToluene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year2)?.ResultValue;
                page3.cellToluene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year3)?.ResultValue;

                page3.cellXyleneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125")?.ReferenceRange;
                page3.cellXylene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year1)?.ResultValue;
                page3.cellXylene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year2)?.ResultValue;
                page3.cellXylene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year3)?.ResultValue;

                page3.cellLeadRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75")?.ReferenceRange;
                page3.cellLead1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year1)?.ResultValue;
                page3.cellLead2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year2)?.ResultValue;
                page3.cellLead3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year3)?.ResultValue;

                page3.cellCarboxyRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120")?.ReferenceRange;
                page3.cellCarboxy1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year1)?.ResultValue;
                page3.cellCarboxy2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year2)?.ResultValue;
                page3.cellCarboxy3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year3)?.ResultValue;

                page3.cellMekRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127")?.ReferenceRange;
                page3.cellMek1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year1)?.ResultValue;
                page3.cellMek2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year2)?.ResultValue;
                page3.cellMek3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year3)?.ResultValue;


                page3.cellBenzeneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115")?.ReferenceRange;
                page3.cellBenzene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115" && p.Year == year1)?.ResultValue;
                page3.cellBenzene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115" && p.Year == year2)?.ResultValue;
                page3.cellBenzene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR115" && p.Year == year3)?.ResultValue;


                page3.cellMethanolRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116")?.ReferenceRange;
                page3.cellMethanol1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116" && p.Year == year1)?.ResultValue;
                page3.cellMethanol2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116" && p.Year == year2)?.ResultValue;
                page3.cellMethanol3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR116" && p.Year == year3)?.ResultValue;

                page3.cellMethyreneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119")?.ReferenceRange;
                page3.cellMethyrene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119" && p.Year == year1)?.ResultValue;
                page3.cellMethyrene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119" && p.Year == year2)?.ResultValue;
                page3.cellMethyrene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR119" && p.Year == year3)?.ResultValue;

                page3.cellAcetoneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117")?.ReferenceRange;
                page3.cellAcetone1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117" && p.Year == year1)?.ResultValue;
                page3.cellAcetone2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117" && p.Year == year2)?.ResultValue;
                page3.cellAcetone3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR117" && p.Year == year3)?.ResultValue;

                page3.cellHexaneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118")?.ReferenceRange;
                page3.cellHexane1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118" && p.Year == year1)?.ResultValue;
                page3.cellHexane2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118" && p.Year == year2)?.ResultValue;
                page3.cellHexane3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR118" && p.Year == year3)?.ResultValue;

                page3.cellIsopropanolRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130")?.ReferenceRange;
                page3.cellIsopropanol1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year1)?.ResultValue;
                page3.cellIsopropanol2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year2)?.ResultValue;
                page3.cellIsopropanol3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year3)?.ResultValue;

                page3.cellChromiumRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132")?.ReferenceRange;
                page3.cellChromium1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132" && p.Year == year1)?.ResultValue;
                page3.cellChromium2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132" && p.Year == year2)?.ResultValue;
                page3.cellChromium3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132" && p.Year == year3)?.ResultValue;

                page3.cellNickelRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131")?.ReferenceRange;
                page3.cellNickel1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131" && p.Year == year1)?.ResultValue;
                page3.cellNickel2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131" && p.Year == year2)?.ResultValue;
                page3.cellNickel3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131" && p.Year == year3)?.ResultValue;

                page3.cellNickelUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188")?.ReferenceRange;
                page3.cellNickelUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188" && p.Year == year1)?.ResultValue;
                page3.cellNickelUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188" && p.Year == year2)?.ResultValue;
                page3.cellNickelUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188" && p.Year == year3)?.ResultValue;

                page3.StyreneUrineRange.Text = labTestSet.FirstOrDefault(p => p.RequestItemCode == "PAR195")?.ReferenceRange;
                page3.StyreneUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR195" && p.Year == year1)?.ResultValue;
                page3.StyreneUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR195" && p.Year == year2)?.ResultValue;
                page3.StyreneUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR195" && p.Year == year3)?.ResultValue;

            }
        }
        private void Riskbook1_AfterPrint(object sender, EventArgs e)
        {
            page2.CreateDocument();
            page3.CreateDocument();
            page4.CreateDocument();
            this.Pages.AddRange(page2.Pages);
            this.Pages.AddRange(page3.Pages);
            this.Pages.AddRange(page4.Pages);
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
