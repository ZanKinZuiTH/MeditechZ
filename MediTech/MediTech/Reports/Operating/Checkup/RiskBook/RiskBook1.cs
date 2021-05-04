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
using MediTech.Model.Report;

namespace MediTech.Reports.Operating.Checkup.RiskBook
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
        RiskBook3 page3 = new RiskBook3();
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
            PatientRiskBookModel data = DataService.Reports.PrintRiskBook(patientUID, patientVisitUID, payorDetailUID);

            if (data.PatientInfomation != null)
            {
                var patient = data.PatientInfomation;
                var groupResult = data.GroupResult;

                lbRunNo.Text = patient.PatientID;
                lbName.Text = patient.PatientName;
                lbCompany.Text = patient.PayorName;
                lbBranch.Text = patient.CompanyName;
                lbDeparment.Text = patient.Department;
                lbEmployeeID.Text = patient.EmployeeID;
                lbDateVisit.Text = patient.StartDttm != null ? patient.StartDttm.Value.AddYears(543).ToString("dd/MM/yyyy") : "";

                page2.lbNameP2.Text = patient.PatientName;
                page2.lbBrithDateP2.Text = patient.BirthDttm != null ? patient.BirthDttm.Value.AddYears(543).ToString("dd/MM/yyyy") : "";
                page2.lbIDCard.Text = patient.NationalID;
                page2.lbGenderP2.Text = patient.Gender;

                page4.lbBP.Text = (patient.BPSys != null ? patient.BPSys.ToString() : "") + (patient.BPDio != null ? "/" + patient.BPDio.ToString() : "");
                page4.lbPulse.Text = patient.Pulse != null ? patient.Pulse.ToString() + " ครั้ง/นาที" : "";
                page4.lbHeight.Text = patient.Height != null ? patient.Height.ToString() + " cm." : "";
                page4.lbWeight.Text = patient.Weight != null ? patient.Weight.ToString() + " kg." : "";
                page4.lbBMI.Text = patient.BMI != null ? patient.BMI.ToString() + " kg/m2" : "";


                page3.PtName.Text = patient.PatientName;
                page4.dateStart.Text = patient.StartDttm != null ? patient.StartDttm.Value.AddYears(543).ToString("dd/MM/yyyy") : "";

                #region Result Wellness

                page4.lbResult.Text = patient.WellnessResult;
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
                var radilogy = data.Radiology;
                if (radilogy.FirstOrDefault(p => !string.IsNullOrEmpty(p.RequestItemName)) != null)
                {

                    if (radilogy.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("chest")) != null)
                    {
                        ResultRadiologyModel chestXray = radilogy.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("chest"));

                        if (!string.IsNullOrEmpty(chestXray.PlainText))
                        {
                            string resultChestThai = TranslateXray(chestXray.PlainText, chestXray.ResultStatus, chestXray.RequestItemName);
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

                    if (radilogy.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("mammo")) != null)
                    {
                        ResultRadiologyModel mammoGram = radilogy.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("mammo"));
                        page4.lbMam.Text = mammoGram.ResultStatus;
                        if (!string.IsNullOrEmpty(mammoGram.PlainText))
                        {
                            string resultChestThai = TranslateXray(mammoGram.PlainText, mammoGram.ResultStatus, mammoGram.RequestItemName);
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

                    if (radilogy.FirstOrDefault(p => (p.RequestItemName.ToLower().Contains("ultrasound") || p.RequestItemName.ToLower().Contains("US"))) != null)
                    {
                        ResultRadiologyModel ultrsound = radilogy.FirstOrDefault(p => (p.RequestItemName.ToLower().Contains("ultrasound") || p.RequestItemName.ToLower().Contains("US")));
                        page3.lbUlt.Text = ultrsound.ResultStatus;
                        if (!string.IsNullOrEmpty(ultrsound.PlainText))
                        {
                            string resultChestThai = TranslateXray(ultrsound.PlainText, ultrsound.ResultStatus, ultrsound.RequestItemName);
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

                page4.lbEKGRecommend.Text = groupResult.FirstOrDefault(p => p.GroupCode == "GPRST23")?.Conclusion;

                var WorkHistorys = data.WorkHistorys;
                if (WorkHistorys != null)
                {
                    if (data.WorkHistorys != null && data.WorkHistorys.Count > 0)
                    {
                        for (int i = 0; i < data.WorkHistorys.Count; i++)
                        {
                            xrTable3.Rows[i + 1].Cells[0].Text = data.WorkHistorys[i].CompanyName;
                            xrTable3.Rows[i + 1].Cells[1].Text = data.WorkHistorys[i].Business;
                            xrTable3.Rows[i + 1].Cells[2].Text = data.WorkHistorys[i].Description;
                            xrTable3.Rows[i + 1].Cells[3].Text = data.WorkHistorys[i].Timeperiod;
                            xrTable3.Rows[i + 1].Cells[4].Text = data.WorkHistorys[i].Riskfactor;
                            xrTable3.Rows[i + 1].Cells[5].Text = data.WorkHistorys[i].Equipment;
                        }
                    }
                }

                
                    if (data.InjuryDetails != null && data.InjuryDetails.Count > 0)
                    {
                        for (int i = 0; i < data.InjuryDetails.Count; i++)
                        {
                            page3.xrTable1.Rows[i + 3].Cells[0].Text = data.InjuryDetails[i].OccuredDate != null ? (data.InjuryDetails[i].OccuredDate?.Year + 543).ToString() : "";
                            page3.xrTable1.Rows[i + 3].Cells[1].Text = data.InjuryDetails[i].BodyLocation != null ? data.InjuryDetails[i].BodyLocation.ToString() : "";
                            page3.xrTable1.Rows[i + 3].Cells[2].Text = data.InjuryDetails[i].InjuryDetail != null ? data.InjuryDetails[i].InjuryDetail.ToString() : "";
                            switch (data.InjuryDetails[i].InjuryServerity)
                            {
                                case "ทุพพลภาพ":
                                    page3.xrTable1.Rows[i + 3].Cells[3].Text = "/";
                                    break;
                                case "สูยเสียอวัยวะบางส่วน":
                                    page3.xrTable1.Rows[i + 3].Cells[4].Text = "/";
                                    break;
                                case "หยุดงานไม่เกิน 3 วัน":
                                    page3.xrTable1.Rows[i + 3].Cells[5].Text = "/";
                                    break;
                                case "หยุดงานเกิน 3 วัน":
                                    page3.xrTable1.Rows[i + 3].Cells[6].Text = "/";
                                    break;
                                default:
                                    break;
                            }

                        }
                    }
                

                var PatientAddresss = data.PatientAddresses;
                if (PatientAddresss != null)
                {
                    page2.DefaultLine1.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 401)?.Line1;
                    page2.DefaultLine2.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 401)?.Line2;
                    page2.DefaultLine3.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 401)?.Line3;
                    page2.DefaultLine4.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 401)?.Line4;
                    page2.DefaultDistrict.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 401)?.DistrictName;
                    page2.DefaultAmphur.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 401)?.AmphurName;
                    page2.DefaultProvince.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 401)?.ProvinceName;
                    page2.DefaultZipCode.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 401)?.ZipCode;
                    page2.DefaultPhone.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 401)?.Phone;

                    page2.ContartLine1.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 4256)?.Line1;
                    page2.ContartLine2.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 4256)?.Line2;
                    page2.ContartLine3.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 4256)?.Line3;
                    page2.ContartLine4.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 4256)?.Line4;
                    page2.ContartDistrict.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 4256)?.DistrictName;
                    page2.ContactAmphur.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 4256)?.AmphurName;
                    page2.ContartProvince.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 4256)?.ProvinceName;
                    page2.ContartZipCode.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 4256)?.ZipCode;
                    page2.ContartPhone.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 4256)?.Phone;

                    page2.OfficeLine1.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 405)?.Line1;
                    page2.OfficeLine2.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 405)?.Line2;
                    page2.OfficeLine3.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 405)?.Line3;
                    page2.OfficeLine4.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 405)?.Line4;
                    page2.OfficeDistrict.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 405)?.DistrictName;
                    page2.OfficeAmphur.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 405)?.AmphurName;
                    page2.OfficeProvince.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 405)?.ProvinceName;
                    page2.OfficeZipCode.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 405)?.ZipCode;
                    page2.OfficePhone.Text = PatientAddresss.FirstOrDefault(p => p.ADTYPUID == 405)?.Phone;
                }
                var MedicalHistory = data.MedicalHistory;
                if (MedicalHistory != null)
                {
                    if (MedicalHistory.PastMedicalHistorys != null && MedicalHistory.PastMedicalHistorys.Count > 0)
                    {
                        foreach (var medical in MedicalHistory.PastMedicalHistorys)
                        {
                            page2.lbPastMedical1.Text = medical.MedicalName;
                            page2.lbPastMedicalDttm1.Text = medical.MedicalDttm != null ? (medical.MedicalDttm.Value.Year + 543).ToString() : "";
                        }
                    }
                    if (!string.IsNullOrEmpty(MedicalHistory.ChronicDisease))
                    {
                        if (MedicalHistory.ChronicDisease.Trim() == "ไม่มี")
                        {
                            page2.CheckChronicNo.Checked = true;
                        }
                        else
                        {
                            page2.CheckChronicYes.Checked = true;
                            page2.ChronicDisease.Text = MedicalHistory.ChronicDisease;
                        }

                    }

                    if (!string.IsNullOrEmpty(MedicalHistory.SurgicalDetail))
                    {
                        if (MedicalHistory.SurgicalDetail.Trim() == "ไม่เคย")
                        {
                            page2.CheckSurgicalNo.Checked = true;
                        }
                        else
                        {
                            page2.CheckSurgicalYes.Checked = true;
                            page2.SurgicalDetail.Text = MedicalHistory.SurgicalDetail;
                        }
              
                    }


                    if (!string.IsNullOrEmpty(MedicalHistory.ImmunizationDetail))
                    {
                        if (MedicalHistory.ImmunizationDetail.Trim() == "ไม่เคย")
                        {
                            page2.CheckImmunizationNo.Checked = true;
                        }
                        else
                        {
                            page2.CheckImmunizationYes.Checked = true;
                            page2.ImmunizationDetail.Text = MedicalHistory.ImmunizationDetail;
                        }

                    }


                    if (!string.IsNullOrEmpty(MedicalHistory.Familyhistory))
                    {
                        if (MedicalHistory.Familyhistory.Trim() == "ไม่มี")
                        {
                            page2.CheckFamilyhistoryNo.Checked = true;
                        }
                        else
                        {
                            page2.CheckFamilyhistoryY.Checked = true;
                            page2.Familyhistory.Text = MedicalHistory.Familyhistory;
                        }

                    }


                    if (!string.IsNullOrEmpty(MedicalHistory.LongTemMedication))
                    {
                        if (MedicalHistory.LongTemMedication.Trim() == "ไม่มี")
                        {
                            page3.CheckLongTemMedicationNo.Checked = true;
                        }
                        else
                        {
                            page3.CheckLongTemMedicationYes.Checked = true;
                            page3.LongTemMedication.Text = MedicalHistory.LongTemMedication;
                        }

                    }


                    if (!string.IsNullOrEmpty(MedicalHistory.AllergyDescription))
                    {
                        if (MedicalHistory.AllergyDescription.Trim() == "ไม่มี")
                        {
                            page3.CheckAllergyDescriptionNo.Checked = true;
                        }
                        else
                        {
                            page3.CheckAllergyDescriptionYes.Checked = true;
                            page3.AllergyDescription.Text = MedicalHistory.AllergyDescription;
                        }

                    }


                    if (!string.IsNullOrEmpty(MedicalHistory.Narcotic))
                    {
                        if (MedicalHistory.Narcotic.Trim() == "ไม่เคย")
                        {
                            page3.CheckNarcoticNo.Checked = true;
                        }
                        else
                        {
                            page3.CheckNarcoticYes.Checked = true;
                            page3.Narcotic.Text = MedicalHistory.Narcotic;
                        }

                    }

                    if (!String.IsNullOrEmpty(MedicalHistory.Smoke))
                    {
                        if (MedicalHistory.Smoke?.Trim() == "ไม่เคย")
                        {
                            page3.CheckSmokeNo.Checked = true;
                        }
                        else if (MedicalHistory.Smoke?.Trim() == "เคยแต่เลิกแล้ว")
                        {
                            page3.CheckSmokeUsed.Checked = true;

                        }
                        else if (MedicalHistory.Smoke?.Trim() != "")
                        {
                            page3.CheckSmokeDay.Checked = true;
                            page3.Smoke.Text = MedicalHistory.Smoke;
                        }

                        page3.SmokePeriodYear.Text = MedicalHistory.SmokePeriodYear;
                        page3.SmokePeriodMonth.Text = MedicalHistory.SmokePeriodMonth;
                        page3.BFQuitSmoke.Text = MedicalHistory.BFQuitSmoke;
                    }

                    if (!string.IsNullOrEmpty(MedicalHistory.Alcohol))
                    {
                        if (MedicalHistory.Alcohol?.Trim() == "ไม่เคย")
                        {
                            page3.CheckAlcoholNo.Checked = true;
                        }
                        else if (MedicalHistory.Alcohol?.Trim() == "ดื่มน้อยกว่า 1 ครั้งต่อสัปดาห์")
                        {
                            page3.CheckAlcoholLess1Week.Checked = true;
                        }
                        else if (MedicalHistory.Alcohol?.Trim() == "ดื่ม 1 ครั้ง/สัปดาห์")
                        {
                            page3.CheckAlcohol1Week.Checked = true;
                        }
                        else if (MedicalHistory.Alcohol?.Trim() == "ดื่ม 2-3 ครั้ง/สัปดาห์")
                        {
                            page3.CheckAlcohol2Week.Checked = true;
                        }
                        else if (MedicalHistory.Alcohol?.Trim() == "ดื่มมากกว่า 3 ครั้ง/สัปดาห์")
                        {
                            page3.CheckAlcoholMore3Week.Checked = true;
                        }
                        else if (MedicalHistory.Alcohol?.Trim() == "เคยแต่เลิกแล้ว")
                        {
                            page3.CheckAlcohoLast.Checked = true;
                        }
                        page3.AlcohoPeriodYear.Text = MedicalHistory.AlcohoPeriodYear;
                        page3.AlcohoPeriodMonth.Text = MedicalHistory.AlcohoPeriodMonth;
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

                }

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
                        || p.RequestItemCode.Contains("LAB211"))
                        .OrderBy(p => p.Year);
                    GenerateRenalFunction(RenalTestSet);

                    #endregion

                    #region Immunology and Virology
                    IEnumerable<PatientResultComponentModel> ImmunologyTestSet = labCompare
                        .Where(p => p.RequestItemCode.Contains("LAB451")
                        || p.RequestItemCode.Contains("LAB441")
                        || p.RequestItemCode.Contains("LAB452")
                        || p.RequestItemCode.Contains("LAB512") //เชื้อไวรัสตับอักเสบเอ
                        || p.RequestItemCode.Contains("LAB554")) //ถูมิไวรัสตับอักเสบเอ
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
                        .Where(p => p.RequestItemCode.Contains("LAB508") //Aluminium in Urine
                        || p.RequestItemCode.Contains("LAB517") //Chromium in urine
                        || p.RequestItemCode.Contains("LAB516") //Nickel in blood
                        || p.RequestItemCode.Contains("LAB314") //Toluene (Urine)
                        || p.RequestItemCode.Contains("LAB319") //Xylene in Urine
                        || p.RequestItemCode.Contains("LAB414") //Lead (Blood)
                        || p.RequestItemCode.Contains("LAB510") //Carboxyhemoglobin in Blood
                        || p.RequestItemCode.Contains("LAB477") //Methyl Ethyl Ketone(MEK) Urine
                        || p.RequestItemCode.Contains("LAB315") //Benzene (Urine)
                        || p.RequestItemCode.Contains("LAB317") //Methanol (Urine)
                        || p.RequestItemCode.Contains("LAB325") //Methyrene chloride in Blood
                        || p.RequestItemCode.Contains("LAB323") //Acetone in Urine
                        || p.RequestItemCode.Contains("LAB324") //Hexane in Urine
                        || p.RequestItemCode.Contains("LAB519") //Isopropyl in Urine
                        || p.RequestItemCode.Contains("LAB558") //Styrene in Urine 
                        || p.RequestItemCode.Contains("LAB518") //Aluminium in Blood
                        || p.RequestItemCode.Contains("LAB560") //Aluminium in Blood
                        || p.RequestItemCode.Contains("LAB561") //Arsenic
                        || p.RequestItemCode.Contains("LAB562") //Cyclohexanone
                        || p.RequestItemCode.Contains("LAB316") //Phenol
                        || p.RequestItemCode.Contains("LAB570") //MIBK
                        || p.RequestItemCode.Contains("LAB571") //Cadmium in Urine
                        || p.RequestItemCode.Contains("LAB572") //Ethylbenzene
                        || p.RequestItemCode.Contains("LAB489") //Mercury
                        || p.RequestItemCode.Contains("LAB573") //Methyrene chloride in Urine
                        || p.RequestItemCode.Contains("LAB568") //Benzene (t,t-Muconic acid) in Urine
                        || p.RequestItemCode.Contains("LAB488")) //Mercury in blood
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
                int? year2 = Years.ElementAtOrDefault(1) != null ? Years[1] : year1 + 1;
                int? year3 = Years.ElementAtOrDefault(2) != null ? Years[2] : year2 + 1;
                page3.UAyear1.Text = "ปี" + " " + (year1 + 543).ToString();
                page3.UAyear2.Text = "ปี" + " " + (year2 + 543).ToString();
                page3.UAyear3.Text = "ปี" + " " + (year3 + 543).ToString();

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
                page3.HbYear1.Text = "ปี" + " " + (year1 + 543).ToString();
                page3.HbYear2.Text = "ปี" + " " + (year2 + 543).ToString();
                page3.HbYear3.Text = "ปี" + " " + (year3 + 543).ToString();

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

                page3.AntiHAVRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR192")?.ReferenceRange;
                page3.AntiHAV1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR192" && p.Year == year1)?.ResultValue;
                page3.AntiHAV2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR192" && p.Year == year2)?.ResultValue;
                page3.AntiHAV3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR192" && p.Year == year3)?.ResultValue;

                page3.HavIgGRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR190")?.ReferenceRange;
                page3.HavIgG1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR190" && p.Year == year1)?.ResultValue;
                page3.HavIgG2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR190" && p.Year == year2)?.ResultValue;
                page3.HavIgG3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR190" && p.Year == year3)?.ResultValue;

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
                if (page4.cellFBS1.Text == "ปฏิเสธการเจาะเลือดส่งตรวจ")
                {
                    page4.cellFBS1.Text = "ปฏิเสธเจาะเลือด";
                }
                page4.cellFBS2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0180" && p.Year == year2)?.ResultValue;
                page4.cellFBS3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0180" && p.Year == year3)?.ResultValue;

                page4.cellCholRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130")?.ReferenceRange;
                page4.cellChol1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130" && p.Year == year1)?.ResultValue;
                if (page4.cellChol1.Text == "ปฏิเสธการเจาะเลือดส่งตรวจ")
                {
                    page4.cellChol1.Text = "ปฏิเสธเจาะเลือด";
                }
                page4.cellChol2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130" && p.Year == year2)?.ResultValue;
                page4.cellChol3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130" && p.Year == year3)?.ResultValue;

                page4.cellTGRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140")?.ReferenceRange;
                page4.cellTG1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140" && p.Year == year1)?.ResultValue;
                if (page4.cellTG1.Text == "ปฏิเสธการเจาะเลือดส่งตรวจ")
                {
                    page4.cellTG1.Text = "ปฏิเสธเจาะเลือด";
                }
                page4.cellTG2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140" && p.Year == year2)?.ResultValue;
                page4.cellTG3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140" && p.Year == year3)?.ResultValue;

                page4.cellHDLRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150")?.ReferenceRange;
                page4.cellHDL1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150" && p.Year == year1)?.ResultValue;
                if (page4.cellHDL1.Text == "ปฏิเสธการเจาะเลือดส่งตรวจ")
                {
                    page4.cellHDL1.Text = "ปฏิเสธเจาะเลือด";
                }
                page4.cellHDL2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150" && p.Year == year2)?.ResultValue;
                page4.cellHDL3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150" && p.Year == year3)?.ResultValue;

                page4.cellLDLRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159")?.ReferenceRange;
                page4.cellLDL1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159" && p.Year == year1)?.ResultValue;
                if (page4.cellLDL1.Text == "ปฏิเสธการเจาะเลือดส่งตรวจ")
                {
                    page4.cellLDL1.Text = "ปฏิเสธเจาะเลือด";
                }
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
                page3.KidneyYear1.Text = "ปี" + " " + (year1 + 543).ToString();
                page3.KidneyYear2.Text = "ปี" + " " + (year2 + 543).ToString();
                page3.KidneyYear3.Text = "ปี" + " " + (year3 + 543).ToString();

                page3.cellBunRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27")?.ReferenceRange;
                page3.cellBun1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27" && p.Year == year1)?.ResultValue;
                if (page3.cellBun1.Text == "ปฏิเสธการเจาะเลือดส่งตรวจ")
                {
                    page3.cellBun1.Text = "ปฏิเสธเจาะเลือด";
                }
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
                page4.LiverYear1.Text = "ปี" + " " + (year1 + 543).ToString();
                page4.LiverYear2.Text = "ปี" + " " + (year2 + 543).ToString();
                page4.LiverYear3.Text = "ปี" + " " + (year3 + 543).ToString();

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
                if (page4.cellSgot1.Text == "ปฏิเสธการเจาะเลือดส่งตรวจ")
                {
                    page4.cellSgot1.Text = "ปฏิเสธเจาะเลือด";
                }
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
            if (labTestSet != null)
            {
                page3.RowNickelBlood.Visible = false;
                page3.RowNickelUrine.Visible = false;
                page3.RowXylene.Visible = false;
                page3.RowCarboxy.Visible = false;
                page3.RowMEK.Visible = false;
                page3.RowAluminiumUrine.Visible = false;
                page3.RowAluminiumBlood.Visible = false;
                page3.RowPhenol.Visible = false;
                page3.RowIsopropanol.Visible = false;

                page3.RowMibkUrine.Visible = false;
                page3.RowCadmiumUrine.Visible = false;
                page3.RowEthylbenzeneUrine.Visible = false;
                page3.RowMercuryUrine.Visible = false;
                page3.RowMethyreneUrine.Visible = false;
                page3.RowBenzenettUrine.Visible = false;
                page3.RowMercuryBlood.Visible = false;

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

                    #region Aluminium Urine
                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122") != null)
                    {
                        page3.RowAluminiumUrine.Visible = true;
                        page3.cellAluminiumRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122")?.ReferenceRange;
                        page3.cellAluminium1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year1)?.ResultValue;
                        page3.cellAluminium2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year2)?.ResultValue;
                        page3.cellAluminium3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR122" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region Toluene Urine
                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124") != null)
                    {
                        page3.RowToluene.Visible = true;
                        page3.cellTolueneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124")?.ReferenceRange;
                        page3.cellToluene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year1)?.ResultValue;
                        page3.cellToluene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year2)?.ResultValue;
                        page3.cellToluene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR124" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region Xylene Urine
                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125") != null)
                    {
                        page3.RowXylene.Visible = true;
                        page3.cellXyleneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125")?.ReferenceRange;
                        page3.cellXylene1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year1)?.ResultValue;
                        page3.cellXylene2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year2)?.ResultValue;
                        page3.cellXylene3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR125" && p.Year == year3)?.ResultValue;
                    }
                    #endregion


                    page3.cellLeadRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75")?.ReferenceRange;
                    page3.cellLead1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year1)?.ResultValue;
                    page3.cellLead2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year2)?.ResultValue;
                    page3.cellLead3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR75" && p.Year == year3)?.ResultValue;

                    #region carboxy
                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120") != null)
                    {
                        page3.RowCarboxy.Visible = true;
                        page3.cellCarboxyRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120")?.ReferenceRange;
                        page3.cellCarboxy1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year1)?.ResultValue;
                        page3.cellCarboxy2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year2)?.ResultValue;
                        page3.cellCarboxy3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR120" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region MEK
                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127") != null)
                    {
                        page3.RowMEK.Visible = true;
                        page3.cellMekRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127")?.ReferenceRange;
                        page3.cellMek1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year1)?.ResultValue;
                        page3.cellMek2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year2)?.ResultValue;
                        page3.cellMek3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR127" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

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

                    #region NickelBlood
                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130") != null)
                    {
                        page3.RowIsopropanol.Visible = true;
                        page3.cellIsopropanolRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130")?.ReferenceRange;
                        page3.cellIsopropanol1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year1)?.ResultValue;
                        page3.cellIsopropanol2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year2)?.ResultValue;
                        page3.cellIsopropanol3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR130" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    page3.cellChromiumRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132")?.ReferenceRange;
                    page3.cellChromium1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132" && p.Year == year1)?.ResultValue;
                    page3.cellChromium2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132" && p.Year == year2)?.ResultValue;
                    page3.cellChromium3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR132" && p.Year == year3)?.ResultValue;

                    #region NickelBlood
                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131") != null)
                    {
                        page3.RowNickelBlood.Visible = true;
                        page3.cellNickelRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131")?.ReferenceRange;
                        page3.cellNickel1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131" && p.Year == year1)?.ResultValue;
                        page3.cellNickel2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131" && p.Year == year2)?.ResultValue;
                        page3.cellNickel3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR131" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region NickelUrine
                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188") != null)
                    {
                        page3.RowNickelUrine.Visible = true;
                        page3.cellNickelUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188")?.ReferenceRange;
                        page3.cellNickelUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188" && p.Year == year1)?.ResultValue;
                        page3.cellNickelUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188" && p.Year == year2)?.ResultValue;
                        page3.cellNickelUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR188" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    page3.StyreneUrineRange.Text = labTestSet.FirstOrDefault(p => p.RequestItemCode == "PAR195")?.ReferenceRange;
                    page3.StyreneUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR195" && p.Year == year1)?.ResultValue;
                    page3.StyreneUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR195" && p.Year == year2)?.ResultValue;
                    page3.StyreneUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR195" && p.Year == year3)?.ResultValue;

                    #region Aluminium Blood
                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR194") != null)
                    {
                        page3.RowAluminiumBlood.Visible = true;
                        page3.AluminiumBloodRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR194")?.ReferenceRange;
                        page3.AluminiumBlood1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR194" && p.Year == year1)?.ResultValue;
                        page3.AluminiumBlood2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR194" && p.Year == year2)?.ResultValue;
                        page3.AluminiumBlood3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR194" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region Arsenic 
                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR199") != null)
                    {
                        page3.RowArsenicUrine.Visible = true;
                        page3.ArsenicRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR199")?.ReferenceRange;
                        page3.Arsenic1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR199" && p.Year == year1)?.ResultValue;
                        page3.Arsenic2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR199" && p.Year == year2)?.ResultValue;
                        page3.Arsenic3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR199" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region Cyclohexanone
                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR202") != null)
                    {
                        page3.RowCyclohexanone.Visible = true;
                        page3.CyclohexanoneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR202")?.ReferenceRange;
                        page3.Cyclohexanone1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR202" && p.Year == year1)?.ResultValue;
                        page3.Cyclohexanone2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR202" && p.Year == year2)?.ResultValue;
                        page3.Cyclohexanone3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR202" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region Phenol
                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR204") != null)
                    {
                        page3.RowPhenol.Visible = true;
                        page3.PhenolRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR204")?.ReferenceRange;
                        page3.Phenol1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR204" && p.Year == year1)?.ResultValue;
                        page3.Phenol2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR204" && p.Year == year2)?.ResultValue;
                        page3.Phenol3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR204" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region MIBK Urine 

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1232") != null)
                    {
                        page3.RowMibkUrine.Visible = true;
                        page3.cellMibkUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1232")?.ReferenceRange;
                        page3.cellMibkUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1232" && p.Year == year1)?.ResultValue;
                        page3.cellMibkUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1232" && p.Year == year2)?.ResultValue;
                        page3.cellMibkUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1232" && p.Year == year3)?.ResultValue;

                    }
                    #endregion

                    #region Cadmium Urine 

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1233") != null)
                    {
                        page3.RowCadmiumUrine.Visible = true;
                        page3.cellCadmiumUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1233")?.ReferenceRange;
                        page3.cellCadmiumUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1233" && p.Year == year1)?.ResultValue;
                        page3.cellCadmiumUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1233" && p.Year == year2)?.ResultValue;
                        page3.cellCadmiumUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1233" && p.Year == year3)?.ResultValue;

                    }
                    #endregion

                    #region Ethyl benzene in Urine

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1234") != null)
                    {
                        page3.RowEthylbenzeneUrine.Visible = true;
                        page3.cellEthylbenzeneUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1234")?.ReferenceRange;
                        page3.cellEthylbenzeneUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1234" && p.Year == year1)?.ResultValue;
                        page3.cellEthylbenzeneUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1234" && p.Year == year2)?.ResultValue;
                        page3.cellEthylbenzeneUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1234" && p.Year == year3)?.ResultValue;

                    }
                    #endregion

                    #region Mercury Urine

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1235") != null)
                    {
                        page3.RowMercuryUrine.Visible = true;
                        page3.cellMercuryUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1235")?.ReferenceRange;
                        page3.cellMercuryUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1235" && p.Year == year1)?.ResultValue;
                        page3.cellMercuryUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1235" && p.Year == year2)?.ResultValue;
                        page3.cellMercuryUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1235" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region Methyrene chloride in Urine

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1236") != null)
                    {
                        page3.RowMethyreneUrine.Visible = true;
                        page3.cellMethyreneUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1236")?.ReferenceRange;
                        page3.cellMethyreneUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1236" && p.Year == year1)?.ResultValue;
                        page3.cellMethyreneUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1236" && p.Year == year2)?.ResultValue;
                        page3.cellMethyreneUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1236" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region Benzene (t,t-Muconic acid) in Urine

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1215") != null)
                    {
                        page3.RowBenzenettUrine.Visible = true;
                        page3.cellBenzenettUrineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1215")?.ReferenceRange;
                        page3.cellBenzenettUrine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1215" && p.Year == year1)?.ResultValue;
                        page3.cellBenzenettUrine2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1215" && p.Year == year2)?.ResultValue;
                        page3.cellBenzenettUrine3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1215" && p.Year == year3)?.ResultValue;
                    }
                    #endregion

                    #region Mercury Blood

                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1237") != null)
                    {
                        page3.RowMercuryBlood.Visible = true;
                        page3.cellMercuryBloodRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1237")?.ReferenceRange;
                        page3.cellMercuryBlood1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1237" && p.Year == year1)?.ResultValue;
                        page3.cellMercuryBlood2.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1237" && p.Year == year2)?.ResultValue;
                        page3.cellMercuryBlood3.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1237" && p.Year == year3)?.ResultValue;
                    }
                    #endregion
                }
            }
        }

        private void GeneratePhysicalExam(IEnumerable<PatientResultComponentModel> PhysicalExamResult)
        {
            if (PhysicalExamResult != null && PhysicalExamResult.Count() > 0)
            {
                page4.lbEye.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM1")?.ResultValue;
                page4.lbEar.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM2")?.ResultValue;
                page4.lbThroat.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM3")?.ResultValue;
                page4.lbNose.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM4")?.ResultValue;
                page4.lbTeeth.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM5")?.ResultValue;
                page4.lbLung.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM8")?.ResultValue;
                page4.lbHeart.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM9")?.ResultValue;
                page4.lbSkin.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM10")?.ResultValue;
                page4.lbThyroid.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM7")?.ResultValue;
                page4.lbLymphNode.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM6")?.ResultValue;
                page4.lbSmoke.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM11")?.ResultValue;
                page4.lbDrugAllergy.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM13")?.ResultValue;
                page4.lbAlcohol.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM12")?.ResultValue;
                page4.lbPastHistory.Text = PhysicalExamResult.FirstOrDefault(p => p.ResultItemCode == "PEXAM14")?.ResultValue;
            }
        }

        private void GenerateTimus(IEnumerable<PatientResultComponentModel> TimusResult)
        {
            if (TimusResult != null && TimusResult.Count() > 0)
            {
                page3.lbFarVision1.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS19")?.ResultValue;
                page3.lbNearVision1.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS20")?.ResultValue;
                page3.lb3DVision1.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS21")?.ResultValue;
                page3.lbBalanceEye1.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS23")?.ResultValue;
                page3.lbVisionColor1.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS22")?.ResultValue;
                page3.lbFieldVision1.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS24")?.ResultValue;
            }
        }

        private void GenerateAudio(IEnumerable<PatientResultComponentModel> AudioResult)
        {
            if (AudioResult != null && AudioResult.Count() > 0)
            {

                page4.R5001.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO1")?.ResultValue;
                page4.R10001.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO2")?.ResultValue;
                page4.R20001.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO3")?.ResultValue;
                page4.R30001.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO4")?.ResultValue;
                page4.R40001.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO5")?.ResultValue;
                page4.R60001.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO6")?.ResultValue;
                page4.R80001.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO7")?.ResultValue;

                page4.L5001.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO9")?.ResultValue;
                page4.L10001.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO10")?.ResultValue;
                page4.L20001.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO11")?.ResultValue;
                page4.L30001.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO12")?.ResultValue;
                page4.L40001.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO13")?.ResultValue;
                page4.L60001.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO14")?.ResultValue;
                page4.L80001.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO15")?.ResultValue;
            }
        }

        private void GenerateSpiro(IEnumerable<PatientResultComponentModel> SpiroResult)
        {
            if (SpiroResult != null && SpiroResult.Count() > 0)
            {
                page4.lbFVCPer1.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO3")?.ResultValue;
                page4.lbFEV1Per1.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO6")?.ResultValue;
                page4.lbFEVPer1.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO9")?.ResultValue;
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
            string thairesult = TranslateResult.TranslateResultXray(resultValue, resultStatus, requestItemName, " ", dtResultMapping, ref listNoMapResult);

            return thairesult;
        }

    }
}
