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
    public partial class CheckupCSR : DevExpress.XtraReports.UI.XtraReport
    {

        private MediTechDataService _DataService;

        public MediTechDataService DataService
        {
            get { return _DataService ?? (_DataService = new MediTechDataService()); }
        }
        List<XrayTranslateMappingModel> dtResultMapping;
        public string PreviewWellness { get; set; }

        public CheckupCSR()
        {
            InitializeComponent();
            BeforePrint += CheckupCSR_BeforePrint;
          
        }

        private void CheckupCSR_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            long patientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long patientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            int payorDetailUID = int.Parse(this.Parameters["PayorDetailUID"].Value.ToString());
            PatientWellnessModel data = DataService.Reports.PrintWellnessBook(patientUID, patientVisitUID, payorDetailUID);


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

                   lbResultWellness.Text = sb.ToString();
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
            }
        }
    }
}
