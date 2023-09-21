using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Model.Report;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Linq;

namespace MediTech.Reports.Operating.Checkup
{
    public partial class MedReport1 : DevExpress.XtraReports.UI.XtraReport
    {
        private MediTechDataService _DataService;
        public MediTechDataService DataService
        {
            get { return _DataService ?? (_DataService = new MediTechDataService()); }
        }

        MedReport2 page2 = new MedReport2();
        public string PreviewWellness { get; set; }
        public MedReport1()
        {
            InitializeComponent();
            BeforePrint += Page1_BeforePrint;
            AfterPrint += Page2_AfterPrint;
            page2.lbResultWellness.BeforePrint += LbResultWellness2_BeforePrint;
        }

        private void LbResultWellness2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel label = page2.lbResultWellness;
            string text = label.Text;

            this.PrintingSystem.Graph.Font = label.Font;

            float labelWidth = label.WidthF;
            float textHeight = 0;

            switch (this.ReportUnit)
            {
                case DevExpress.XtraReports.UI.ReportUnit.HundredthsOfAnInch:
                    labelWidth = GraphicsUnitConverter.Convert(labelWidth, GraphicsUnit.Inch, GraphicsUnit.Document) / 97;
                    break;
                case DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter:
                    labelWidth = GraphicsUnitConverter.Convert(labelWidth, GraphicsUnit.Millimeter, GraphicsUnit.Document) / 10;
                    break;
            }

            SizeF size = this.PrintingSystem.Graph.MeasureString(text, (int)labelWidth);


            switch (this.ReportUnit)
            {
                case DevExpress.XtraReports.UI.ReportUnit.HundredthsOfAnInch:
                    textHeight = GraphicsUnitConverter.Convert(size.Height, GraphicsUnit.Document, GraphicsUnit.Inch) * 97;
                    break;
                case DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter:
                    textHeight = GraphicsUnitConverter.Convert(size.Height, GraphicsUnit.Document, GraphicsUnit.Millimeter) * 10;
                    break;
            }

            if (textHeight > label.HeightF)
            {
                var newSize = label.Font.Size - 0.5f;
                page2.lbResultWellness.Font = new System.Drawing.Font(page2.lbResultWellness.Font.Name, newSize);
                LbResultWellness2_BeforePrint(null, null);
            }
        }

        private void Page1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
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
                    xrPictureBox1.Image = Image.FromStream(ms);
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
                    this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(559.5F, 10F);
                    this.xrPictureBox1.Image = System.Drawing.Image.FromStream(outStream);
                }
                this.xrPictureBox1.SizeF = new System.Drawing.SizeF(205.4585F, 64.16669F);
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
                lbAge.Text = patient.Age != null ? patient.Age : "";
                lbGender.Text = patient.Gender;

                lbHeight.Text = patient.Height != null ? patient.Height.ToString() : "";
                lbWeight.Text = patient.Weight != null ? patient.Weight.ToString() : "";
                lbBMI.Text = patient.BMI != null ? patient.BMI.ToString() : "";
                lbBP.Text = (patient.BPSys != null ? patient.BPSys.ToString() : "") + (patient.BPDio != null ? "/" + patient.BPDio.ToString() : "");
                lbPulse.Text = patient.Pulse != null ? patient.Pulse.ToString() : "";
                lbWaist.Text = patient.WaistCircumference != null ? patient.WaistCircumference.ToString() : "";
                lbHip.Text = patient.HipCircumference != null ? patient.HipCircumference.ToString() : "";

                page2.hn2.Text = "HN : " + patient.PatientID;
                page2.Name2.Text = patient.PatientName;
                #endregion

                rowCholinesteraseBlood.Visible = false;

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
                    page2.lbResultWellness.Text = sb.ToString();


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

                #region Chest
                string chestPA = groupResult.FirstOrDefault(p => p.GroupCode == "GPRST4")?.Conclusion;

                if (!string.IsNullOrEmpty(chestPA))
                {
                    lbChest.Text = chestPA;
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
                                        lbChest.Text = "Negative";
                                    }
                                    else
                                    {
                                        lbChest.Text = ResultChestEn;
                                    }
                                }
                                else if (ChestResult[0].ToLower().Contains("old fracture right clavicle"))
                                {
                                    lbChest.Text = "Calcification in aorta Old fracture of right clavicle";
                                }
                            }
                        }
                    }
                }

                #endregion

                #region EKG

                lbEKGRecommend.Text = groupResult.FirstOrDefault(p => p.GroupCode == "GPRST23")?.Conclusion;

                #endregion


                IOrderedEnumerable<PatientResultComponentModel> labCompare;
                if (data.LabCompare != null)
                {
                    labCompare = data.LabCompare.OrderByDescending(p => p.Year);

                    if (labCompare != null)
                    {
                        #region Complete Blood Count

                        IEnumerable<PatientResultComponentModel> cbcTestSet = labCompare
                        .Where(p => p.RequestItemName.Contains("CBC")
                        || p.RequestItemCode.Contains("LAB597"))
                        .OrderByDescending(p => p.Year);
                        GenerateCompleteBloodCount(cbcTestSet);
                        #endregion

                        #region Urinalysis
                        IEnumerable<PatientResultComponentModel> uaTestSet = labCompare
                            .Where(p => p.RequestItemName.Contains("UA"))
                            .OrderByDescending(p => p.Year);
                        GenerateUrinalysis(uaTestSet);
                        #endregion

                        #region Chemical
                        IEnumerable<PatientResultComponentModel> ChemicalTestSet = labCompare
                            .Where(p => p.RequestItemCode.Contains("LAB231")
                            || p.RequestItemCode.Contains("LAB232")
                            || p.RequestItemCode.Contains("LAB212")
                            || p.RequestItemCode.Contains("LAB211")
                            || p.RequestItemCode.Contains("LAB213")
                            || p.RequestItemCode.Contains("LAB241")
                            || p.RequestItemCode.Contains("LAB242")
                            || p.RequestItemCode.Contains("LAB243")
                            || p.RequestItemCode.Contains("LAB244")
                            || p.RequestItemCode.Contains("LAB261"))
                             .OrderByDescending(p => p.Year);
                        GenerateChemical(ChemicalTestSet);
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
                            ).OrderByDescending(p => p.Year);
                        GenerateLiverFunction(LiverTestSet);
                        #endregion

                        #region other
                        IEnumerable<PatientResultComponentModel> OtherTestSet = labCompare
                            .Where(p => p.RequestItemCode.Contains("LAB318") //Amphetamine (Urine)
                            || p.RequestItemCode.Contains("LAB483") //Amphetamine
                            || p.RequestItemCode.Contains("LAB490") //Amphetamine (strip)
                            || p.RequestItemCode.Contains("LAB441") //hbs ag
                            || p.RequestItemCode.Contains("LAB451") //anti hbs-ag
                            || p.RequestItemCode.Contains("LAB595") //Hepatitis B surface Antigen (CMIA)
                            || p.RequestItemCode.Contains("LAB596") //Hepatitis B surface Antibody (CMIA)
                            || p.RequestItemCode.Contains("LAB281") //afp
                            || p.RequestItemCode.Contains("LAB282") //ca19-9
                            || p.RequestItemCode.Contains("LAB283") //cea
                            || p.RequestItemCode.Contains("LAB284") //psa
                            || p.RequestItemCode.Contains("LAB285") //ca125
                            || p.RequestItemCode.Contains("LAB286") //ca153
                            || p.RequestItemCode.Contains("LAB511") //Cholinesterase in blood
                            ).OrderByDescending(p => p.Year);
                        GenerateOtherFunction(OtherTestSet);
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

                    IEnumerable<PatientResultComponentModel> BackStrength = occmed
                            .Where(p => p.RequestItemCode.Contains("MUSCLEBA")
                            || p.RequestItemCode.Contains("MUSCLEGR")
                            || p.RequestItemCode.Contains("MUSCLELEG"));
                    GenerateBackStrength(BackStrength);
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
            }
        }


        #region Lab Result
        private void GenerateCompleteBloodCount(IEnumerable<PatientResultComponentModel> labTestSet)
        {

            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().OrderByDescending(x => x.Value).ToList();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;

                cellHbRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001")?.ReferenceRange;
                cellHb1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0001" && p.Year == year1)?.ResultValue;

                cellHctRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020")?.ReferenceRange;
                cellHct1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0020" && p.Year == year1)?.ResultValue;

                cellMcvRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025")?.ReferenceRange;
                cellMcv1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0025" && p.Year == year1)?.ResultValue;

                cellMchRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030")?.ReferenceRange;
                cellMch1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0030" && p.Year == year1)?.ResultValue;

                cellMchcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035")?.ReferenceRange;
                cellMchc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0035" && p.Year == year1)?.ResultValue;

                cellRdwRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1")?.ReferenceRange;
                cellRdw1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR1" && p.Year == year1)?.ResultValue;


                cellRbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428")?.ReferenceRange;
                cellRbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0428" && p.Year == year1)?.ResultValue;

                cellRbcMorRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13")?.ReferenceRange;
                cellRbcMor1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR13" && p.Year == year1)?.ResultValue;

                cellWbcRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006")?.ReferenceRange;
                cellWbc1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0006" && p.Year == year1)?.ResultValue;

                cellNectophilRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040")?.ReferenceRange;
                cellNectophil1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0040" && p.Year == year1)?.ResultValue;

                cellLymphocyteRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050")?.ReferenceRange;
                cellLymphocyte1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0050" && p.Year == year1)?.ResultValue;

                cellMonocyteRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060")?.ReferenceRange;
                cellMonocyte1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0060" && p.Year == year1)?.ResultValue;

                cellEosinophilRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070")?.ReferenceRange;
                cellEosinophil1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0070" && p.Year == year1)?.ResultValue;

                cellBasophilRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080")?.ReferenceRange;
                cellBasophil1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0080" && p.Year == year1)?.ResultValue;

                //cellPlateletSmearRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427")?.ReferenceRange;
                //cellPlateletSmear1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0427" && p.Year == year1)?.ResultValue;

                cellPlateletsCountRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010")?.ReferenceRange;
                cellPlateletsCount1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "A0010" && p.Year == year1)?.ResultValue;
            }
        }
        private void GenerateUrinalysis(IEnumerable<PatientResultComponentModel> labTestSet)
        {

            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().OrderByDescending(x => x.Value).ToList();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;

                 cellColorRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080")?.ReferenceRange;
                 cellColor1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0080" && p.Year == year1)?.ResultValue;

                 cellAppearanceRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21")?.ReferenceRange;
                 cellAppearance1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR21" && p.Year == year1)?.ResultValue;

                 cellSpacGraRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001")?.ReferenceRange;
                 cellSpacGra1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0001" && p.Year == year1)?.ResultValue;

                 cellPhRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080")?.ReferenceRange;
                 cellPh1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0080" && p.Year == year1)?.ResultValue;

                 cellProteinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085")?.ReferenceRange;
                 cellProtein1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0085" && p.Year == year1)?.ResultValue;

                 cellGlucoseRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090")?.ReferenceRange;
                 cellGlucose1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0090" && p.Year == year1)?.ResultValue;

                 cellKetoneRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047")?.ReferenceRange;
                 cellKetone1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0047" && p.Year == year1)?.ResultValue;

                 cellNitritesRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154")?.ReferenceRange;
                 cellNitrites1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0154" && p.Year == year1)?.ResultValue;

                 cellBilirubinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151")?.ReferenceRange;
                 cellBilirubin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0151" && p.Year == year1)?.ResultValue;

                 cellUrobilinogenRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150")?.ReferenceRange;
                 cellUrobilinogen1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0150" && p.Year == year1)?.ResultValue;

                 cellLeukocyteRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153")?.ReferenceRange;
                 cellLeukocyte1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0153" && p.Year == year1)?.ResultValue;

                 cellBloodRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152")?.ReferenceRange;
                 cellBlood1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0152" && p.Year == year1)?.ResultValue;

                 //cellErythrocytesRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140")?.ReferenceRange;
                 //cellErythrocytes1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "E0140" && p.Year == year1)?.ResultValue;

                 cellWbcUARange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250")?.ReferenceRange;
                 cellWbcUA1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0250" && p.Year == year1)?.ResultValue;

                 cellRbcUARange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260")?.ReferenceRange;
                 cellRbcUA1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0260" && p.Year == year1)?.ResultValue;

                 cellEpithelialCellsRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270")?.ReferenceRange;
                 cellEpithelialCells1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "D0270" && p.Year == year1)?.ResultValue;

                 //cellAmorphousRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14")?.ReferenceRange;
                 //cellAmorphous1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR14" && p.Year == year1)?.ResultValue;

                 //cellOtherRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20")?.ReferenceRange;
                 //cellOther1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR20" && p.Year == year1)?.ResultValue;

              
            }
        }
        private void GenerateChemical(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().OrderByDescending(x => x.Value).ToList();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;

                 cellFbsRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0180")?.ReferenceRange;
                 cellFbs1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0180" && p.Year == year1)?.ResultValue;

                 cellHbA1cRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR7")?.ReferenceRange;
                 cellHbA1c1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR7" && p.Year == year1)?.ResultValue;

                 cellBunRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27")?.ReferenceRange;
                 cellBun1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR27" && p.Year == year1)?.ResultValue;

                 cellCreatinineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070")?.ReferenceRange;
                 cellCreatinine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0070" && p.Year == year1)?.ResultValue;

                 cellCholesterolRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130")?.ReferenceRange;
                 cellCholesterol1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0130" && p.Year == year1)?.ResultValue;

                 cellTriglycerideRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140")?.ReferenceRange;
                 cellTriglyceride1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0140" && p.Year == year1)?.ResultValue;

                 cellLdlRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159")?.ReferenceRange;
                 cellLdl1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0159" && p.Year == year1)?.ResultValue;

                 cellHdlRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150")?.ReferenceRange;
                 cellHdl1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0150" && p.Year == year1)?.ResultValue;

                 cellUricRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320")?.ReferenceRange;
                 cellUric1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "C0320" && p.Year == year1)?.ResultValue;

            }
        }

        private void GenerateLiverFunction(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().OrderByDescending(x => x.Value).ToList();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;

                cellSGOTRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50")?.ReferenceRange;
                cellSGOT1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR50" && p.Year == year1)?.ResultValue;

                cellSGPTRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51")?.ReferenceRange;
                cellSGPT1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR51" && p.Year == year1)?.ResultValue;

                cellAlpRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33")?.ReferenceRange;
                cellAlp1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR33" && p.Year == year1)?.ResultValue;

                cellTotalBilirubinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48")?.ReferenceRange;
                cellTotalBilirubin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR48" && p.Year == year1)?.ResultValue;

                cellDirectBilirubinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49")?.ReferenceRange;
                cellDirectBilirubin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR49" && p.Year == year1)?.ResultValue;

                cellTotalProteinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105")?.ReferenceRange;
                cellTotalProtein1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR105" && p.Year == year1)?.ResultValue;

               
                cellAlbuminRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR101")?.ReferenceRange;
                cellAlbumin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR101" && p.Year == year1)?.ResultValue;
                

                cellGlobulinRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46")?.ReferenceRange;
                cellGlobulin1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR46" && p.Year == year1)?.ResultValue;
                

                cellGgtRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR26")?.ReferenceRange;
                cellGgt1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR26" && p.Year == year1)?.ResultValue;
                
            }
        }

        private void GenerateOtherFunction(IEnumerable<PatientResultComponentModel> labTestSet)
        {
            if (labTestSet != null && labTestSet.Count() > 0)
            {
                PatientResultComponentModel CheckGender = labTestSet.FirstOrDefault();
                List<int?> Years = labTestSet.Select(p => p.Year).Distinct().OrderByDescending(x => x.Value).ToList();
                int? year1 = Years.ElementAtOrDefault(0) != null ? Years[0] : DateTime.Now.Year;

                cellAmphetamineRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR12")?.ReferenceRange;
                cellAmphetamine1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR12" && p.Year == year1)?.ResultValue;

                cellHBsAgRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35")?.ReferenceRange; 
                cellHBsAgRange1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR35" && p.Year == year1)?.ResultValue;

                cellCoiHBsAgRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34")?.ReferenceRange;
                cellCoiHBsAg1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR34" && p.Year == year1)?.ResultValue;

                cellCoiAntiHbsRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121")?.ReferenceRange;
                cellCoiAntiHbs1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR121" && p.Year == year1)?.ResultValue;

                cellAntiHbsRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42")?.ReferenceRange;
                cellAntiHbs1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR42" && p.Year == year1)?.ResultValue;


                if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38") != null)
                {
                    cellAfpRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38")?.ReferenceRange;
                    cellAfp1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR38" && p.Year == year1)?.ResultValue;
                }
                else
                {
                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39") != null)
                    {
                        cellAfpRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39")?.ReferenceRange;
                        cellAfp1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR39" && p.Year == year1)?.ResultValue;
                    }
                    else
                    {
                        cellAfpRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186")?.ReferenceRange;
                        cellAfp1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR186" && p.Year == year1)?.ResultValue;
                    }
                }

                if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR40") != null)
                {
                    cellCEARange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR40")?.ReferenceRange;
                    cellCEA1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR40" && p.Year == year1)?.ResultValue;
                }

                else
                {
                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR185") != null)
                    {
                        cellCEARange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR185")?.ReferenceRange;
                        cellCEA1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR185" && p.Year == year1)?.ResultValue;
                    }
                }

                if (CheckGender.SEXXXUID == 1)
                {
                    //PSA โชว์ตัวเลขก่อนตัวอักษร
                    if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4") != null)
                    {
                        cellPsaRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4")?.ReferenceRange;
                        cellPsa1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR4" && p.Year == year1)?.ResultValue;
                    }
                    else
                    {
                        cellPsaRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187")?.ReferenceRange;
                        cellPsa1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR187" && p.Year == year1)?.ResultValue;
                    }

                }

                if (CheckGender.SEXXXUID == 2)
                {
                    cellCa125Range.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41")?.ReferenceRange;
                    cellCa125_1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR41" && p.Year == year1)?.ResultValue;

                    ca153Range.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR203")?.ReferenceRange;
                    ca153_1.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR203" && p.Year == year1)?.ResultValue;
                }

                if (labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR126") != null)
                {
                    rowCholinesteraseBlood.Visible = true;
                    CholinesterasebloodRange.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR126")?.ReferenceRange;
                    Cholinesteraseblood.Text = labTestSet.FirstOrDefault(p => p.ResultItemCode == "PAR126" && p.Year == year1)?.ResultValue;
                }
            }
        }
        #endregion

        #region Occmed Result

        private void GenerateSpiro(IEnumerable<PatientResultComponentModel> SpiroResult)
        {
            if (SpiroResult != null && SpiroResult.Count() > 0)
            {
                page2.lbFVCMeasure.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO1")?.ResultValue;
                page2.lbFVCPredic.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO2")?.ResultValue;
                page2.lbFVCPer.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO3")?.ResultValue;
                page2.lbFEV1Measure.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO4")?.ResultValue;
                page2.lbFEV1Predic.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO5")?.ResultValue;
                page2.lbFEV1Per.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO6")?.ResultValue;
                page2.lbFEVMeasure.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO7")?.ResultValue;
                page2.lbFEVPredic.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO8")?.ResultValue;
                page2.lbFEVPer.Text = SpiroResult.FirstOrDefault(p => p.ResultItemCode == "SPIRO9")?.ResultValue;
            }
        }

        private void GenerateTimus(IEnumerable<PatientResultComponentModel> TimusResult)
        {
            if (TimusResult != null && TimusResult.Count() > 0)
            {
                page2.lbFarVision.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS19")?.ResultValue;
                page2.lbNearVision.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS20")?.ResultValue;
                page2.lb3DVision.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS21")?.ResultValue;
                page2.lbBalanceEye.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS23")?.ResultValue;
                page2.lbVisionColor.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS22")?.ResultValue;
                page2.lbFieldVision.Text = TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS24")?.ResultValue;
            }
        }

        private void GenerateAudio(IEnumerable<PatientResultComponentModel> AudioResult)
        {
            if (AudioResult != null && AudioResult.Count() > 0)
            {
                page2.R1.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO1")?.ResultValue;
                page2.R2.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO2")?.ResultValue;
                page2.R3.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO3")?.ResultValue;
                page2.R4.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO4")?.ResultValue;
                page2.R5.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO5")?.ResultValue;
                page2.R6.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO6")?.ResultValue;
                page2.R7.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO7")?.ResultValue;
                page2.lbAudioRight.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO8")?.ResultValue;

                page2.L1.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO9")?.ResultValue;
                page2.L2.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO10")?.ResultValue;
                page2.L3.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO11")?.ResultValue;
                page2.L4.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO12")?.ResultValue;
                page2.L5.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO13")?.ResultValue;
                page2.L6.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO14")?.ResultValue;
                page2.L7.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO15")?.ResultValue;
                page2.lbAudioLeft.Text = AudioResult.FirstOrDefault(p => p.ResultItemCode == "AUDIO16")?.ResultValue;
            }
        }

        private void GenerateBackStrength(IEnumerable<PatientResultComponentModel> BackStrength)
        {
            if (BackStrength != null && BackStrength.Count() > 0)
            {
                page2.lbBackValue.Text = BackStrength.FirstOrDefault(p => p.ResultItemCode == "MUCS1")?.ResultValue;
                page2.lbBackStrenght.Text = BackStrength.FirstOrDefault(p => p.ResultItemCode == "MUCS2")?.ResultValue;
                page2.lbValueLegStrength.Text = BackStrength.FirstOrDefault(p => p.ResultItemCode == "MUCS5")?.ResultValue;
                page2.lbLegStrength.Text = BackStrength.FirstOrDefault(p => p.ResultItemCode == "MUCS6")?.ResultValue;
                page2.lbValueGripStrength.Text = BackStrength.FirstOrDefault(p => p.ResultItemCode == "MUCS3")?.ResultValue;
                page2.lbGripStrength.Text = BackStrength.FirstOrDefault(p => p.ResultItemCode == "MUCS4")?.ResultValue;
            }
        }

        private void GenerateOccmedGroup(IEnumerable<CheckupGroupResultModel> occmedGroupResult)
        {
            if (occmedGroupResult != null && occmedGroupResult.Count() > 0)
            {
                page2.lbLungResult.Text = occmedGroupResult.FirstOrDefault(p => p.GroupCode == "GPRST33")?.Conclusion.ToString();
               

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
                        else if (item != null)
                        {
                            recommand = item.ToString();
                        }
                    }

                    page2.lbVisionOccmedResult.Text = description + " " + recommand;
                    //page2.lbVisionOccmedRecommend.Text = recommand;
                }

                //page2.lbAudioResult.Text = occmedGroupResult.FirstOrDefault(p => p.GroupCode == "GPRST25")?.ResultStatus.ToString();
                //page2.lbAudioRecommend.Text = occmedGroupResult.FirstOrDefault(p => p.GroupCode == "GPRST25")?.Conclusion.ToString();

                page2.lbMuscleResult.Text = occmedGroupResult.FirstOrDefault(p => p.GroupCode == "GPRST32")?.Conclusion;
            }
        }

        #endregion

        private void Page2_AfterPrint(object sender, EventArgs e)
        {
            page2.CreateDocument();
            this.Pages.AddRange(page2.Pages);
        }
    }
}
