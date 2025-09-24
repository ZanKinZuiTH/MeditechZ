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

namespace MediTech.Reports.Operating.Checkup
{
    public partial class RiskMobileCheckup : DevExpress.XtraReports.UI.XtraReport
    {

        private MediTechDataService _DataService;

        public MediTechDataService DataService
        {
            get { return _DataService ?? (_DataService = new MediTechDataService()); }
        }
        public string PreviewWellness { get; set; }
        RiskMobileCheckup2 RiskPage2 = new RiskMobileCheckup2();

        public RiskMobileCheckup()
        {
            InitializeComponent();
            BeforePrint += RiskMobileCheckup_BeforePrint;
            AfterPrint += RiskMobileCheckup_AfterPrint;

        }

        private void RiskMobileCheckup_AfterPrint(object sender, EventArgs e)
        {
            RiskPage2.CreateDocument();
            this.Pages.AddRange(RiskPage2.Pages);
        }

        private void RiskMobileCheckup_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            long patientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long patientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            int payorDetailUID = int.Parse(this.Parameters["PayorDetailUID"].Value.ToString());
            PatientRiskBookModel dataRisk = DataService.Reports.PrintRiskBook(patientUID, patientVisitUID, payorDetailUID);
            if (dataRisk.PatientInfomation != null)
            {
                var patient = dataRisk.PatientInfomation;
                var groupResult = dataRisk.GroupResult;
                #region Patient information
                lbDateCheckup.Text = patient.StartDttm != null ? patient.StartDttm.Value.ToString("dd/MM/yyyy") : "";
                lbPatientName.Text = patient.PatientName;
                lbCardID.Text = patient.NationalID;
                //lblicense.Text = patient.
                lbEmployee.Text = patient.EmployeeID;
               // lbCareProvider.Text = patient.CareProviderName;
                lbHeight.Text = patient.Height != null ? patient.Height.ToString() : "";
                lbWeight.Text = patient.Weight != null ? patient.Weight.ToString() : "";
                lbBMI.Text = patient.BMI != null ? patient.BMI.ToString() + " kg/m2" : "";
                lbBP.Text = (patient.BPSys != null ? patient.BPSys.ToString() : "") + (patient.BPDio != null ? "/" + patient.BPDio.ToString() : "");
                lbPulse.Text = patient.Pulse != null ? patient.Pulse.ToString() : "";
                //lbWaist.Text = patient.WaistCircumference != null ? patient.WaistCircumference.ToString() : "";

                #endregion

                #region PE
                //var pe = groupResult.FirstOrDefault(p => p.GroupCode == "GPRST1");
                //if (pe!= null)
                //{
                //if (pe.ResultStatus == "ปกติ" )
                //{
                //    cbPE1.Checked = true;
                //}
                //else
                //{
                //    cbPE2.Checked = true;
                //}
                //}
                //lbResultPE.Text = groupResult.FirstOrDefault(p => p.GroupCode == "GPRST1")?.Conclusion;
                #endregion

                #region lab
                cblab1.Checked = true;
                var test = groupResult.FirstOrDefault(p => p.GroupCode == "GPRST22")?.ResultStatus;
                if (groupResult.FirstOrDefault(p => p.GroupCode == "GPRST18")?.ResultStatus != "ปกติ" && groupResult.FirstOrDefault(p => p.GroupCode == "GPRST18")?.ResultStatus != null)
                {
                    cbLipid.Checked = true;
                    cblab1.Checked = false;
                    cblab2.Checked = true;
                }
                if (groupResult.FirstOrDefault(p => p.GroupCode == "GPRST12")?.ResultStatus != "ปกติ" && groupResult.FirstOrDefault(p => p.GroupCode == "GPRST12")?.ResultStatus != null)
                {
                    cbRenal.Checked = true;
                    cblab1.Checked = false;
                    cblab2.Checked = true;
                }
                if (groupResult.FirstOrDefault(p => p.GroupCode == "GPRST10")?.ResultStatus != "ปกติ" && groupResult.FirstOrDefault(p => p.GroupCode == "GPRST10")?.ResultStatus != null)
                {
                    cbLiver.Checked = true;
                    cblab1.Checked = false;
                    cblab2.Checked = true;
                }
                if (groupResult.FirstOrDefault(p => p.GroupCode == "GPRST14")?.ResultStatus != "ปกติ" && groupResult.FirstOrDefault(p => p.GroupCode == "GPRST14")?.ResultStatus != null)
                {
                    cbSugar.Checked = true;
                    cblab1.Checked = false;
                    cblab2.Checked = true;
                }
                if (groupResult.FirstOrDefault(p => p.GroupCode == "GPRST15")?.ResultStatus != "ปกติ" && groupResult.FirstOrDefault(p => p.GroupCode == "GPRST15")?.ResultStatus != null)
                {
                    cbUrin.Checked = true;
                    cblab1.Checked = false;
                    cblab2.Checked = true;
                }
                if (groupResult.FirstOrDefault(p => p.GroupCode == "GPRST22")?.ResultStatus != "ปกติ" && groupResult.FirstOrDefault(p => p.GroupCode == "GPRST22")?.ResultStatus != null)
                {
                    cbUric.Checked = true;
                    cblab1.Checked = false;
                    cblab2.Checked = true;
                }
                if (groupResult.FirstOrDefault(p => p.GroupCode == "GPRST7")?.ResultStatus != "ปกติ" && groupResult.FirstOrDefault(p => p.GroupCode == "GPRST7")?.ResultStatus != null)
                {
                    cbRedblood.Checked = true;
                    cblab1.Checked = false;
                    cblab2.Checked = true;
                }

                #endregion

                var occmed = dataRisk.MobileResult;
                if (occmed != null)
                {
                    IEnumerable<PatientResultComponentModel> timusResult = occmed
                        .Where(p => p.RequestItemCode.Contains("TIMUS"));
                    GenerateTimus(timusResult);

                }
                var resultAddio = groupResult.FirstOrDefault(p => p.GroupCode == "GPRST25");
                if (resultAddio != null )
                {
                    if (resultAddio.RABSTSUID == 2883)
                    {
                        cbAD1.Checked = true;
                        cbadnormal.Checked = true;

                    }
                    else
                    {
                        if (resultAddio.RABSTSUID == 2885)
                        {
                            cbAd2.Checked = true;
                            cbAdLow.Checked = true;

                        }
                        else if (resultAddio.RABSTSUID == 2882)
                        {
                            cbAd2.Checked = true;
                            cbAdHigh.Checked = true;
                        }

                    }
                   
                }
                #region toxi

                string toxiline = "";

                toxiline += GetwordingToxi(groupResult, "GPRST27");
                toxiline += GetwordingToxi(groupResult, "GPRST28");
                toxiline += GetwordingToxi(groupResult, "GPRST29");
                toxiline += GetwordingToxi(groupResult, "GPRST30");
                toxiline += GetwordingToxi(groupResult, "GPRST31");
                toxiline += GetwordingToxi(groupResult, "GPRST35");
                toxiline += GetwordingToxi(groupResult, "GPRST40");
                toxiline += GetwordingToxi(groupResult, "GPRST41");
                toxiline += GetwordingToxi(groupResult, "GPRST44");
                toxiline += GetwordingToxi(groupResult, "GPRST45");
                toxiline += GetwordingToxi(groupResult, "GPRST46");
                toxiline += GetwordingToxi(groupResult, "GPRST47");

                toxiline += GetwordingToxi(groupResult, "GPRST50");
                toxiline += GetwordingToxi(groupResult, "GPRST51");
                toxiline += GetwordingToxi(groupResult, "GPRST52");
                toxiline += GetwordingToxi(groupResult, "GPRST63");
                toxiline += GetwordingToxi(groupResult, "GPRST64");
                toxiline += GetwordingToxi(groupResult, "GPRST65");
                toxiline += GetwordingToxi(groupResult, "GPRST66");
                toxiline += GetwordingToxi(groupResult, "GPRST73");
                toxiline += GetwordingToxi(groupResult, "GPRST74");
                toxiline += GetwordingToxi(groupResult, "GPRST75");
                toxiline += GetwordingToxi(groupResult, "GPRST76");
                toxiline += GetwordingToxi(groupResult, "GPRST77");

                toxiline += GetwordingToxi(groupResult, "GPRST80");
                toxiline += GetwordingToxi(groupResult, "GPRST81");
                toxiline += GetwordingToxi(groupResult, "GPRST82");
                toxiline += GetwordingToxi(groupResult, "GPRST83");
                toxiline += GetwordingToxi(groupResult, "GPRST84");
                toxiline += GetwordingToxi(groupResult, "GPRST85");
                toxiline += GetwordingToxi(groupResult, "GPRST86");
                toxiline += GetwordingToxi(groupResult, "GPRST87");
                toxiline += GetwordingToxi(groupResult, "GPRST88");
                lbToxi.Text = toxiline;

                if (toxiline.Trim().ToString() != "")
                {
                    cbToxi1.Checked = false;
                    cbToxi2.Checked = true;
                }
                else
                {
                    cbToxi1.Checked = true;
                }
               
              


                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST27")?.ResultStatus != null ? ":" + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST27").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST28")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST28").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST29")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST29").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST30")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST30").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST31")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST31").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST35")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST35").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST40")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST40").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST41")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST41").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST44")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST44").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST45")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST45").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST46")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST46").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST47")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST47").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST50")?.ResultStatus!=  null ? " "+ groupResult.FirstOrDefault(p => p.GroupCode == "GPRST50").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST51")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST51").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST52")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST52").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST63")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST63").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST64")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST64").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST65")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST65").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST66")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST66").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST73")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST73").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST74")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST74").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST75")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST75").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST76")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST76").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST77")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST77").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST80")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST80").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST81")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST81").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST82")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST82").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST83")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST83").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST84")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST84").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST85")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST85").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST86")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST86").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST87")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST87").Conclusion : "";
                //toxiline += groupResult.FirstOrDefault(p => p.GroupCode == "GPRST88")?.ResultStatus != null ? " " + groupResult.FirstOrDefault(p => p.GroupCode == "GPRST88").Conclusion : "";

                #endregion


            }

        }

        private void GenerateTimus(IEnumerable<PatientResultComponentModel> TimusResult)
        {
            if (TimusResult != null && TimusResult.Count() > 0)
            {
                cbOcc1.Checked = true;

                if (TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS20")?.ResultValue != "ปกติ" && TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS20")?.ResultValue != null)
                {
                    cbNearVision1.Checked = true;
                    cbOcc2.Checked = true;
                    cbOcc1.Checked = false;
                }

                if (TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS21")?.ResultValue != "ปกติ" && TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS21")?.ResultValue != null)
                {
                    cb3DVision1.Checked = true;
                    cbOcc2.Checked = true;
                    cbOcc1.Checked = false;
                }

                if (TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS19")?.ResultValue != "ปกติ" && TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS19")?.ResultValue != null)
                {
                    cbFarVision1.Checked = true;
                    cbOcc2.Checked = true;
                    cbOcc1.Checked = false;
                }

                if (TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS22")?.ResultValue != "ปกติ" && TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS22")?.ResultValue != null)
                {
                    cbVisionColor1.Checked = true;
                    cbOcc2.Checked = true;
                    cbOcc1.Checked = false;
                }


                if (TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS23")?.ResultValue != "ปกติ" && TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS23")?.ResultValue != null)
                {
                    cbBalanceEye1.Checked = true;
                    cbOcc2.Checked = true;
                    cbOcc1.Checked = false;
                }


                if (TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS24")?.ResultValue != "ปกติ" && TimusResult.FirstOrDefault(p => p.ResultItemCode == "TIMUS24")?.ResultValue != null)
                {
                    cbFieldVision1.Checked = true;
                    cbOcc2.Checked = true;
                    cbOcc1.Checked = false;
                }

            }
        }

        private string GetwordingToxi(List<CheckupGroupResultModel> models,string code)
        {
            string result = "";
            var currydata = models.FirstOrDefault(p => p.GroupCode == code);

            if (currydata != null)
            {
                if (currydata.RABSTSUID != 2883)
                {
                    result = currydata.ItemNameResult +":" + currydata.Conclusion;
                }
            }

            return result;
        }

       




    }
}
