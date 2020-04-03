using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using MediTech.Helpers;
using MediTech.Model;
using MediTech.DataService;
using System.Linq;

namespace MediTech.Reports.Operating.Patient.Checkup_Book
{
    public partial class BookPage1 : DevExpress.XtraReports.UI.XtraReport
    {

        private MediTechDataService _DataService;

        public MediTechDataService DataService
        {
            get { return _DataService ?? (_DataService = new MediTechDataService()); }
        }

        List<XrayTranslateMappingModel> dtResultMapping;

        BookPage2 bookPage2 = new BookPage2();
        BookPage3 bookPage3 = new BookPage3();
        BookPage4 bookPage4 = new BookPage4();
        public BookPage1()
        {
            InitializeComponent();
            BeforePrint += BookPage1_BeforePrint;
            AfterPrint += BookPage1_AfterPrint;
        }

        private void BookPage1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            long patientuID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long patientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            List<CheckupBookModel> data = DataService.Reports.PrintCheckupBook(patientuID, patientVisitUID);

            if (data != null && data.Count > 0)
            {
                lblSummeryResult.Text = data.FirstOrDefault().WellnessResult;
                if (lblSummeryResult.Text != null && lblSummeryResult.Text.Length > 1700)
                {
                    lblSummeryResult.Font = new Font("Angsana New", 9);
                }
                else if (lblSummeryResult.Text != null && lblSummeryResult.Text.Length > 1350)
                {
                    lblSummeryResult.Font = new Font("Angsana New", 10);
                }
                lblCheckupDate.Text = data.FirstOrDefault().StartDttm != null ? data.FirstOrDefault().StartDttm.Value.ToString("dd/MM/yyyy") : "";
                lblPatientName.Text = data.FirstOrDefault().PatientName;
                lblHN.Text = data.FirstOrDefault().PatientID;
                lblNationalID.Text = data.FirstOrDefault().NationalID;
                lblBirthDttm.Text = data.FirstOrDefault().BirthDttm != null ? data.FirstOrDefault().BirthDttm.Value.ToString("dd/MM/yyyy") : "";
                lblAge.Text = data.FirstOrDefault().Age != null ? data.FirstOrDefault().Age + " ปี" : "";
                lblHeight.Text = data.FirstOrDefault().Height != null ? data.FirstOrDefault().Height.ToString() + " cm." : "";
                lblWeight.Text = data.FirstOrDefault().Weight != null ? data.FirstOrDefault().Weight.ToString() + " kg." : "";
                lblBMI.Text = data.FirstOrDefault().BMI != null ? data.FirstOrDefault().BMI.ToString() + " kg/m2" : "";
                lblBP.Text = (data.FirstOrDefault().BPSys != null ? data.FirstOrDefault().BPSys.ToString() : "") + (data.FirstOrDefault().BPDio != null ? "/" + data.FirstOrDefault().BPDio.ToString() : "");
                if (lblBP.Text != "")
                {
                    lblBP.Text += " mm.Hg";
                }
                lblPulse.Text = data.FirstOrDefault().Pulse != null ? data.FirstOrDefault().Pulse.ToString() + " ครั้ง/นาที" : "";

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
                    lblBMIResult.Text = bmiResult;

                    if (bmiResult != "น้ำหนักปกติ")
                    {
                        lblBMIResult.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }
                }

                if (data.FirstOrDefault().BPSys != null && data.FirstOrDefault().BPDio != null)
                {
                    string bpResult = "ปกติ";
                    if ((data.FirstOrDefault().BPSys >= 140)
                        || (data.FirstOrDefault().BPDio >= 90))
                    {
                        bpResult = "ผิดปกติ";
                    }
                    lblBPResult.Text = bpResult;

                    if (bpResult == "ผิดปกติ")
                    {
                        lblBPResult.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }
                }

                if (data.FirstOrDefault().Pulse != null)
                {
                    string pulseResult = "ปกติ";
                    if (data.FirstOrDefault().Pulse > 100 || data.FirstOrDefault().Pulse < 60)
                    {
                        pulseResult = "ผิดปกติ";
                    }
                    lblPulseResult.Text = pulseResult;

                    if (pulseResult == "ผิดปกติ")
                    {
                        lblPulseResult.Font = new Font("Angsana New", 11, FontStyle.Bold);
                    }
                }

                bookPage2.lblEyes.Text = data.FirstOrDefault().Eyes != null ? data.FirstOrDefault().Eyes.ToString() : "";
                bookPage2.lblEars.Text = data.FirstOrDefault().Ears != null ? data.FirstOrDefault().Ears.ToString() : "";
                bookPage2.lblThroat.Text = data.FirstOrDefault().Throat != null ? data.FirstOrDefault().Throat.ToString() : "";
                bookPage2.lblNose.Text = data.FirstOrDefault().Nose != null ? data.FirstOrDefault().Nose.ToString() : "";
                bookPage2.lblTeeth.Text = data.FirstOrDefault().Teeth != null ? data.FirstOrDefault().Teeth.ToString() : "";
                bookPage2.lblLung.Text = data.FirstOrDefault().Lung != null ? data.FirstOrDefault().Lung.ToString() : "";
                bookPage2.lblHeart.Text = data.FirstOrDefault().Heart != null ? data.FirstOrDefault().Heart.ToString() : "";
                bookPage2.lblSkin.Text = data.FirstOrDefault().Skin != null ? data.FirstOrDefault().Skin.ToString() : "";
                bookPage2.lblThyroid.Text = data.FirstOrDefault().Thyroid != null ? data.FirstOrDefault().Thyroid.ToString() : "";
                bookPage2.lblLymphNode.Text = data.FirstOrDefault().LymphNode != null ? data.FirstOrDefault().LymphNode.ToString() : "";
                bookPage2.lblSmoke.Text = data.FirstOrDefault().Smoke != null ? data.FirstOrDefault().Smoke.ToString() : "";
                bookPage2.lblDrugAllergy.Text = data.FirstOrDefault().DrugAllergy != null ? data.FirstOrDefault().DrugAllergy.ToString() : "";
                bookPage2.lblAlcohol.Text = data.FirstOrDefault().Alcohol != null ? data.FirstOrDefault().Alcohol.ToString() : "";
                bookPage2.lblMedicalUnderLing.Text = data.FirstOrDefault().PersonalHistory != null ? data.FirstOrDefault().PersonalHistory.ToString() : "";




                bookPage2.lblEKGConclusion.Text = data.FirstOrDefault().EkgConclusion != null ? data.FirstOrDefault().EkgConclusion.ToString() : "";

                if (data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("chest") && p.RequestItemType == "Radiology") != null)
                {
                    CheckupBookModel chestXray = data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("chest") && p.RequestItemType == "Radiology");
                    if (!string.IsNullOrEmpty(chestXray.RadiologyResultText))
                    {
                        string resultChestThai = TranslateXray(chestXray.RadiologyResultText, chestXray.RadiologyResultStatus);
                        if (!string.IsNullOrEmpty(resultChestThai))
                        {
                            bookPage2.lblChest.Text = resultChestThai;
                        }
                        else
                        {
                            bookPage2.lblMamo.Text = "ยังไม่ได้แปลไทย";
                        }
                    }

                }

                if (data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("mammo") && p.RequestItemType == "Radiology") != null)
                {
                    CheckupBookModel mammoGram = data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("mammo") && p.RequestItemType == "Radiology");
                    bookPage2.lblMamo.Text = mammoGram.RadiologyResultStatus;
                    if (!string.IsNullOrEmpty(mammoGram.RadiologyResultText))
                    {
                        string resultChestThai = TranslateXray(mammoGram.RadiologyResultText, mammoGram.RadiologyResultStatus);
                        if (!string.IsNullOrEmpty(resultChestThai))
                        {
                            bookPage2.lblMamo.Text = resultChestThai;
                        }
                        else
                        {
                            bookPage2.lblMamo.Text = "ยังไม่ได้แปลไทย";
                        }
                    }
                }

                if (data.FirstOrDefault(p => (p.RequestItemName.ToLower().Contains("ultrasound") || p.RequestItemName.ToLower().Contains("US")) && p.RequestItemType == "Radiology") != null)
                {
                    CheckupBookModel ultrsound = data.FirstOrDefault(p => (p.RequestItemName.ToLower().Contains("ultrasound") || p.RequestItemName.ToLower().Contains("US")) && p.RequestItemType == "Radiology");
                    bookPage2.lblUltrasound.Text = ultrsound.RadiologyResultStatus;
                    if (!string.IsNullOrEmpty(ultrsound.RadiologyResultText))
                    {
                        string resultChestThai = TranslateXray(ultrsound.RadiologyResultText, ultrsound.RadiologyResultStatus);
                        if (!string.IsNullOrEmpty(resultChestThai))
                        {
                            bookPage2.lblUltrasound.Text = resultChestThai;
                        }
                        else
                        {
                            bookPage2.lblUltrasound.Text = "ยังไม่ได้แปลไทย";
                        }
                    }


                }

                if (data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("ABO Group") && p.RequestItemType == "Lab") != null)
                {
                    CheckupBookModel bloodGroup = data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("ABO Group") && p.RequestItemType == "Lab");
                    lblBloodGroup.Text = bloodGroup.ResultValue;
                }

                if (data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("Blood Group Rh") && p.RequestItemType == "Lab") != null)
                {
                    CheckupBookModel bloodGroupRh = data.FirstOrDefault(p => p.RequestItemName.ToLower().Contains("Blood Group Rh") && p.RequestItemType == "Lab");
                    if (lblBloodGroup.Text == "")
                    {
                        lblBloodGroup.Text = "RH :" + bloodGroupRh.ResultValue;
                    }
                    else
                    {
                        lblBloodGroup.Text = lblBloodGroup.Text + ", RH :" + bloodGroupRh.ResultValue;
                    }

                }

                if (data.FirstOrDefault(p => p.RequestItemName.Contains("CBC") && p.RequestItemType == "Lab") != null)
                {
                    List<CheckupBookModel> cbcTestSet = data.Where(p => p.RequestItemName.Contains("CBC") && p.RequestItemType == "Lab").ToList();
                    bookPage3.tableCBC.BeginInit();
                    foreach (var cbcResult in cbcTestSet)
                    {

                        XRTableRow row = new XRTableRow();
                        XRTableCell cell0 = new XRTableCell();
                        XRTableCell cell1 = new XRTableCell();
                        XRTableCell cell2 = new XRTableCell();
                        cell0.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;

                        cell0.Text = cbcResult.ResultItemName;
                        cell1.Text = cbcResult.ReferenceRange;
                        cell2.Text = cbcResult.ResultValue;
                        if (!string.IsNullOrEmpty(cbcResult.IsAbnormal))
                        {
                            cell2.ForeColor = Color.Red;
                            cell2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        row.Cells.AddRange(new XRTableCell[] { cell0, cell1, cell2 });


                        bookPage3.tableCBC.Rows.Add(row);
                        cell0.CanGrow = true;
                        cell1.CanGrow = true;
                        cell2.CanGrow = true;
                        cell0.Multiline = true;
                        cell1.Multiline = true;
                        cell2.Multiline = true;

                        if ((cbcResult.ReferenceRange != null && cbcResult.ReferenceRange.Length > 40) || (cbcResult.ResultValue != null && cbcResult.ResultValue.Length > 40))
                        {
                            row.HeightF = 47f;
                            if (cbcResult.ReferenceRange.Length > 40)
                            {
                                cell1.Font = new Font("Angsana New", 8);
                            }
                            if (cbcResult.ResultValue.Length > 40)
                            {
                                cell2.Font = new Font("Angsana New", 8);
                            }
                        }
                        else
                        {
                            row.HeightF = 20f;
                        }
                    }

                    float rowHeight = 0;
                    foreach (XRTableRow row in bookPage3.tableCBC.Rows)
                    {
                        rowHeight += row.HeightF;
                    }
                    bookPage3.tableCBC.HeightF = rowHeight;
                    bookPage3.tableCBC.EndInit();
                }
                else
                {
                    bookPage3.tableCBC.Visible = false;
                }

                bookPage3.lblFVC.Text = data.FirstOrDefault().FVC != null ? data.FirstOrDefault().FVC.ToString() : "";
                bookPage3.lblFvcPred.Text = data.FirstOrDefault().FVCPred != null ? data.FirstOrDefault().FVCPred.ToString() : "";
                bookPage3.lblFVCPer.Text = data.FirstOrDefault().FVCPer != null ? data.FirstOrDefault().FVCPer.ToString() + " %" : "";
                bookPage3.lblFEV1.Text = data.FirstOrDefault().FEV1 != null ? data.FirstOrDefault().FEV1.ToString() : "";
                bookPage3.lblFEV1Pred.Text = data.FirstOrDefault().FEV1Pred != null ? data.FirstOrDefault().FEV1Pred.ToString() : "";
                bookPage3.lblFEV1Per.Text = data.FirstOrDefault().FEV1Per != null ? data.FirstOrDefault().FEV1Per.ToString() + " %" : "";
                bookPage3.lblFEV1FVC.Text = data.FirstOrDefault().FEV1FVC != null ? data.FirstOrDefault().FEV1FVC.ToString() + " %" : "";
                bookPage3.lblFEV1FVCPred.Text = data.FirstOrDefault().FEV1FVCPred != null ? data.FirstOrDefault().FEV1FVCPred.ToString() + " %" : "";
                bookPage3.lblFEV1FVCPer.Text = data.FirstOrDefault().FEV1FVCPer != null ? data.FirstOrDefault().FEV1FVCPer.ToString() + " %" : "";
                bookPage3.lblSpiroResult.Text = data.FirstOrDefault().SpiroResult != null ? data.FirstOrDefault().SpiroResult.ToString() : "";
                bookPage3.lblSpiroRecommend.Text = data.FirstOrDefault().SpiroRecommend != null ? data.FirstOrDefault().SpiroRecommend.ToString() : "";

                if (bookPage3.lblSpiroResult.Text != "" && bookPage3.lblSpiroResult.Text != "ปกติ")
                {
                    bookPage3.lblSpiroResult.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                if (bookPage3.lblSpiroRecommend.Text != null && bookPage3.lblSpiroRecommend.Text.Length > 120)
                {
                    bookPage3.lblSpiroRecommend.Font = new Font("Angsana New", 9);
                }

                bookPage3.lblFarPoint.Text = data.FirstOrDefault().FarPoint != null ? data.FirstOrDefault().FarPoint.ToString() : "";
                if (bookPage3.lblFarPoint.Text != "" && bookPage3.lblFarPoint.Text != "ปกติ")
                {
                    bookPage3.lblFarPoint.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                bookPage3.lblNearPoint.Text = data.FirstOrDefault().NearPoint != null ? data.FirstOrDefault().NearPoint.ToString() : "";
                if (bookPage3.lblNearPoint.Text != "" && bookPage3.lblNearPoint.Text != "ปกติ")
                {
                    bookPage3.lblNearPoint.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                bookPage3.lblDepth.Text = data.FirstOrDefault().Depth != null ? data.FirstOrDefault().Depth.ToString() : "";
                if (bookPage3.lblDepth.Text != "" && bookPage3.lblDepth.Text != "ปกติ")
                {
                    bookPage3.lblDepth.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
                bookPage3.lblMuscle.Text = data.FirstOrDefault().Muscle != null ? data.FirstOrDefault().Muscle.ToString() : "";
                if (bookPage3.lblMuscle.Text != "" && bookPage3.lblMuscle.Text != "ปกติ")
                {
                    bookPage3.lblMuscle.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
                bookPage3.lblColor.Text = data.FirstOrDefault().Color != null ? data.FirstOrDefault().Color.ToString() : "";
                if (bookPage3.lblColor.Text != "" && bookPage3.lblColor.Text != "ปกติ")
                {
                    bookPage3.lblColor.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
                bookPage3.lblVisualfield.Text = data.FirstOrDefault().Visualfield != null ? data.FirstOrDefault().Visualfield.ToString() : "";
                if (bookPage3.lblVisualfield.Text != "" && bookPage3.lblVisualfield.Text != "ปกติ")
                {
                    bookPage3.lblVisualfield.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }
                bookPage3.lblEyesResult.Text = data.FirstOrDefault().TitmusConclusion != null ? data.FirstOrDefault().TitmusConclusion.ToString() : "";
                bookPage3.lblEyesRecommend.Text = data.FirstOrDefault().TitmusRecommend != null ? data.FirstOrDefault().TitmusRecommend.ToString() : "";

                if (bookPage3.lblEyesResult.Text != null && bookPage3.lblEyesResult.Text.Length > 120)
                {
                    bookPage3.lblEyesResult.Font = new Font("Angsana New", 9);
                }

                if (bookPage3.lblEyesRecommend.Text != null && bookPage3.lblEyesRecommend.Text.Length > 120)
                {
                    bookPage3.lblEyesRecommend.Font = new Font("Angsana New", 9);
                }

                bookPage3.lblEarsRight.Text = data.FirstOrDefault().AudioRightResult != null ? data.FirstOrDefault().AudioRightResult.ToString() : "";
                bookPage3.lblEarsLeft.Text = data.FirstOrDefault().AudioLeftResult != null ? data.FirstOrDefault().AudioLeftResult.ToString() : "";
                bookPage3.lblEarsResult.Text = data.FirstOrDefault().AudioResult != null ? data.FirstOrDefault().AudioResult.ToString() : "";
                if (bookPage3.lblEarsResult.Text != "" && bookPage3.lblEarsResult.Text != "ปกติ")
                {
                    bookPage3.lblEarsResult.Font = new Font("Angsana New", 11, FontStyle.Bold);
                }

                bookPage3.lblEarsRecommend.Text = data.FirstOrDefault().AudioRecommend != null ? data.FirstOrDefault().AudioRecommend.ToString() : "";

                if (bookPage3.lblEarsRight.Text != null && bookPage3.lblEarsRight.Text.Length > 120)
                {
                    bookPage3.lblEarsRight.Font = new Font("Angsana New", 9);
                }

                if (bookPage3.lblEarsLeft.Text != null && bookPage3.lblEarsLeft.Text.Length > 120)
                {
                    bookPage3.lblEarsLeft.Font = new Font("Angsana New", 9);
                }

                if (bookPage3.lblEarsRecommend.Text != null && bookPage3.lblEarsRecommend.Text.Length > 120)
                {
                    bookPage3.lblEarsRecommend.Font = new Font("Angsana New", 9);
                }


                if (data.FirstOrDefault(p => (p.RequestItemName.Contains("UA") || p.RequestItemName.Contains("Urine Analysis")) && p.RequestItemType == "Lab") != null)
                {
                    List<CheckupBookModel> urineTestSet = data.Where(p => (p.RequestItemName.Contains("UA") || p.RequestItemName.Contains("Urine Analysis")) && p.RequestItemType == "Lab").ToList();
                    bookPage4.tableUA.BeginInit();
                    foreach (var urineResult in urineTestSet)
                    {

                        XRTableRow row = new XRTableRow();
                        XRTableCell cell0 = new XRTableCell();
                        XRTableCell cell1 = new XRTableCell();
                        XRTableCell cell2 = new XRTableCell();

                        row.CanGrow = true;
                        cell0.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;

                        cell0.Text = urineResult.ResultItemName;
                        cell1.Text = urineResult.ReferenceRange;
                        cell2.Text = urineResult.ResultValue;
                        if (!string.IsNullOrEmpty(urineResult.IsAbnormal))
                        {
                            cell2.ForeColor = Color.Red;
                            cell2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        row.Cells.AddRange(new XRTableCell[] { cell0, cell1, cell2 });


                        bookPage4.tableUA.Rows.Add(row);
                        cell0.CanGrow = false;
                        cell1.CanGrow = false;
                        cell2.CanGrow = false;

                        cell0.Multiline = true;
                        cell1.Multiline = true;
                        cell2.Multiline = true;


                        if ((urineResult.ReferenceRange != null && urineResult.ReferenceRange.Length > 40) || (urineResult.ResultValue != null && urineResult.ResultValue.Length > 40))
                        {
                            row.HeightF = 47f;
                            if (urineResult.ReferenceRange.Length > 40)
                            {
                                cell1.Font = new Font("Angsana New", 8);
                            }
                            if (urineResult.ResultValue.Length > 40)
                            {
                                cell2.Font = new Font("Angsana New", 8);
                            }
                        }
                        else
                        {
                            row.HeightF = 20f;
                        }
                    }
                    float rowHeight = 0;
                    foreach (XRTableRow row in bookPage4.tableUA.Rows)
                    {
                        rowHeight += row.HeightF;
                    }
                    bookPage4.tableUA.HeightF = rowHeight;
                    bookPage4.tableUA.EndInit();
                }
                else
                {
                    bookPage4.tableUA.Visible = false;
                }

                if (data.FirstOrDefault(p => (!p.RequestItemName.Contains("UA") && !p.RequestItemName.Contains("Urine Analysis")
                && !p.RequestItemName.Contains("CBC") && !p.RequestItemName.ToLower().Contains("ABO Group") && !p.RequestItemName.ToLower().Contains("Blood Group Rh")
                && !p.RequestItemName.Contains("Stool")
                && p.Catagory != "Immunology")
                && p.RequestItemType == "Lab") != null)
                {
                    List<CheckupBookModel> labbortoryList = data.Where(p => (!p.RequestItemName.Contains("UA") && !p.RequestItemName.Contains("Urine Analysis")
                && !p.RequestItemName.Contains("CBC") && !p.RequestItemName.ToLower().Contains("ABO Group") && !p.RequestItemName.ToLower().Contains("Blood Group Rh")
                && !p.RequestItemName.Contains("Stool")
                && p.Catagory != "Immunology")
                && p.RequestItemType == "Lab").ToList();
                    bookPage4.tableLabora.BeginInit();
                    foreach (var labbortory in labbortoryList)
                    {

                        XRTableRow row = new XRTableRow();
                        XRTableCell cell0 = new XRTableCell();
                        XRTableCell cell1 = new XRTableCell();
                        XRTableCell cell2 = new XRTableCell();
                        cell0.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;

                        cell0.Text = labbortory.ResultItemName;
                        cell1.Text = labbortory.ReferenceRange;
                        cell2.Text = labbortory.ResultValue;
                        if (!string.IsNullOrEmpty(labbortory.IsAbnormal))
                        {
                            cell2.ForeColor = Color.Red;
                            cell2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        row.Cells.AddRange(new XRTableCell[] { cell0, cell1, cell2 });


                        bookPage4.tableLabora.Rows.Add(row);
                        cell0.CanGrow = false;
                        cell1.CanGrow = false;
                        cell2.CanGrow = false;

                        cell0.Multiline = true;
                        cell1.Multiline = true;
                        cell2.Multiline = true;


                        if ((labbortory.ReferenceRange != null && labbortory.ReferenceRange.Length > 40) || (labbortory.ResultValue != null && labbortory.ResultValue.Length > 40))
                        {
                            row.HeightF = 47f;
                            if (labbortory.ReferenceRange.Length > 40)
                            {
                                cell1.Font = new Font("Angsana New", 8);
                            }
                            if (labbortory.ResultValue.Length > 40)
                            {
                                cell2.Font = new Font("Angsana New", 8);
                            }
                        }
                        else
                        {
                            row.HeightF = 20f;
                        }
                    }
                    float rowHeight = 0;
                    foreach (XRTableRow row in bookPage4.tableLabora.Rows)
                    {
                        rowHeight += row.HeightF;
                    }
                    bookPage4.tableLabora.HeightF = rowHeight;
                    bookPage4.tableLabora.EndInit();
                }
                else
                {
                    bookPage4.tableLabora.Visible = false;
                }

                if (data.FirstOrDefault(p => (p.RequestItemName.Contains("Stool")) && p.RequestItemType == "Lab") != null)
                {
                    List<CheckupBookModel> stoolExamList = data.Where(p => p.RequestItemName.Contains("Stool") && p.RequestItemType == "Lab").ToList();
                    bookPage4.tableStool.BeginInit();
                    foreach (var stool in stoolExamList)
                    {

                        XRTableRow row = new XRTableRow();
                        XRTableCell cell0 = new XRTableCell();
                        XRTableCell cell1 = new XRTableCell();
                        XRTableCell cell2 = new XRTableCell();
                        cell0.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;

                        cell0.Text = stool.ResultItemName;
                        cell1.Text = stool.ReferenceRange;
                        cell2.Text = stool.ResultValue;
                        if (!string.IsNullOrEmpty(stool.IsAbnormal))
                        {
                            cell2.ForeColor = Color.Red;
                            cell2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        row.Cells.AddRange(new XRTableCell[] { cell0, cell1, cell2 });


                        bookPage4.tableStool.Rows.Add(row);
                        cell0.CanGrow = false;
                        cell1.CanGrow = false;
                        cell2.CanGrow = false;

                        cell0.Multiline = true;
                        cell1.Multiline = true;
                        cell2.Multiline = true;


                        if ((stool.ReferenceRange != null && stool.ReferenceRange.Length > 40) || (stool.ResultValue != null && stool.ResultValue.Length > 40))
                        {
                            row.HeightF = 47f;
                            if (stool.ReferenceRange.Length > 40)
                            {
                                cell1.Font = new Font("Angsana New", 8);
                            }
                            if (stool.ResultValue.Length > 40)
                            {
                                cell2.Font = new Font("Angsana New", 8);
                            }
                        }
                        else
                        {
                            row.HeightF = 20f;
                        }
                    }
                    float rowHeight = 0;
                    foreach (XRTableRow row in bookPage4.tableStool.Rows)
                    {
                        rowHeight += row.HeightF;
                    }
                    bookPage4.tableStool.HeightF = rowHeight;
                    bookPage4.tableStool.EndInit();
                }
                else
                {
                    bookPage4.tableStool.Visible = false;
                }

                if (data.FirstOrDefault(p => (p.Catagory == "Immunology") && p.RequestItemType == "Lab") != null)
                {
                    List<CheckupBookModel> ImmunologyList = data.Where(p => p.Catagory == "Immunology" && p.RequestItemType == "Lab").ToList();
                    bookPage4.tableImmun.BeginInit();
                    foreach (var immuno in ImmunologyList)
                    {

                        XRTableRow row = new XRTableRow();
                        XRTableCell cell0 = new XRTableCell();
                        XRTableCell cell1 = new XRTableCell();
                        XRTableCell cell2 = new XRTableCell();
                        cell0.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;

                        cell0.Text = immuno.ResultItemName;
                        cell1.Text = immuno.ReferenceRange;
                        cell2.Text = immuno.ResultValue;
                        if (!string.IsNullOrEmpty(immuno.IsAbnormal))
                        {
                            cell2.ForeColor = Color.Red;
                            cell2.Font = new Font("Angsana New", 11, FontStyle.Bold);
                        }

                        row.Cells.AddRange(new XRTableCell[] { cell0, cell1, cell2 });


                        bookPage4.tableImmun.Rows.Add(row);
                        cell0.CanGrow = false;
                        cell1.CanGrow = false;
                        cell2.CanGrow = false;

                        cell0.Multiline = true;
                        cell1.Multiline = true;
                        cell2.Multiline = true;


                        if ((immuno.ReferenceRange != null && immuno.ReferenceRange.Length > 40) || (immuno.ResultValue != null && immuno.ResultValue.Length > 40))
                        {
                            row.HeightF = 47f;
                            if (immuno.ReferenceRange.Length > 40)
                            {
                                cell1.Font = new Font("Angsana New", 8);
                            }
                            if (immuno.ResultValue.Length > 40)
                            {
                                cell2.Font = new Font("Angsana New", 8);
                            }
                        }
                        else
                        {
                            row.HeightF = 20f;
                        }
                    }
                    float rowHeight = 0;
                    foreach (XRTableRow row in bookPage4.tableImmun.Rows)
                    {
                        rowHeight += row.HeightF;
                    }
                    bookPage4.tableImmun.HeightF = rowHeight;
                    bookPage4.tableImmun.EndInit();
                }
                else
                {
                    bookPage4.tableImmun.Visible = false;
                }
            }
        }

        private void BookPage1_AfterPrint(object sender, EventArgs e)
        {
            bookPage2.CreateDocument();
            bookPage3.CreateDocument();
            bookPage4.CreateDocument();
            this.Pages.AddRange(bookPage2.Pages);
            this.Pages.AddRange(bookPage3.Pages);
            this.Pages.AddRange(bookPage4.Pages);
        }


        public string TranslateXray(string resultValue, string resultStatus)
        {
            if (dtResultMapping == null)
            {
                dtResultMapping = DataService.Radiology.GetXrayTranslateMapping();
            }

            List<string> listNoMapResult = new List<string>();
            string thairesult = TranslateResult.TranslateResultXray(resultValue, resultStatus, dtResultMapping, ref listNoMapResult);

            return thairesult;
        }
    }
}
