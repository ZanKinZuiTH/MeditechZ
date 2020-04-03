using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Globalization;
using MediTech.Model.Report;
using MediTech.DataService;
using System.Collections.Generic;
using MediTech.Model;
using System.Linq;

namespace MediTech.Reports.Operating.Radiology
{
    public partial class ImagingReportV2 : DevExpress.XtraReports.UI.XtraReport
    {
        public ImagingReportV2()
        {
            InitializeComponent();
            this.BeforePrint += ImagingReportV2_BeforePrint;
        }

        private void ImagingReportV2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            CultureInfo culture = new CultureInfo("th-TH");
            PatientResultRadiology dataReport = (new ReportsService()).GetPatientResultRadiology(Convert.ToInt64(this.Parameters["ResultUID"].Value));
            if (dataReport != null)
            {
                DateTime checkUpdate = DateTime.MinValue;
                this.lblPatientName.Text = dataReport.PatientName;

                if (this.Parameters["CheckupDate"].Value != null)
                {
                    checkUpdate = DateTime.Parse(this.Parameters["CheckupDate"].Value.ToString());
                }

                this.lblCheckupDate.Text = checkUpdate.ToString("dd MMM yyyy", culture);
                this.lblDoctor.Text = dataReport.Doctor;

                if (dataReport.ResultStatus == "Normal")
                {
                    XRCheckBox xrCheckBox = new XRCheckBox();
                    xrCheckBox.LocationFloat = new DevExpress.Utils.PointFloat(35.33F, 30.00001F);
                    xrCheckBox.Name = "xrCheckBox1";
                    xrCheckBox.SizeF = new System.Drawing.SizeF(18.75F, 23F);
                    xrCheckBox.StylePriority.UseFont = false;
                    xrCheckBox.Checked = true;
                    Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrCheckBox });

                    XRLabel xrlabel = new XRLabel();
                    xrlabel.Font = new System.Drawing.Font("Angsana New", 20F, System.Drawing.FontStyle.Bold);
                    xrlabel.LocationFloat = new DevExpress.Utils.PointFloat(56.08F, 20.00001F);
                    xrlabel.SizeF = new System.Drawing.SizeF(742F, 80F);
                    xrlabel.Text = "Negative";
                    xrlabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                    Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrlabel });
                }
                else
                {
                    List<XrayTranslateMappingModel> dtResultMapping = (new RadiologyService()).GetXrayTranslateMapping();


                    List<string> plainTextList;
                    if (dataReport.ResultValue.ToUpper().Contains("IMPRESSION") || dataReport.ResultValue.ToUpper().Contains("CONCLUSION"))
                    {
                        int startIndex;
                        if (dataReport.ResultValue.ToUpper().Contains("IMPRESSION"))
                        {
                            startIndex = dataReport.ResultValue.ToUpper().IndexOf("IMPRESSION");
                        }
                        else
                        {
                            startIndex = dataReport.ResultValue.ToUpper().IndexOf("CONCLUSION");
                        }
                        int endIndex = dataReport.ResultValue.Length - startIndex;
                        string subString = dataReport.ResultValue.Substring(startIndex, endIndex);
                        //plainTextList = subString.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Replace(".", "").Replace("-", "").Trim()).ToList();
                        plainTextList = subString.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p)).ToList();
                    }
                    else
                    {
                        //plainTextList = xrayResultText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Replace(".", "").Replace("-", "").Trim()).ToList();
                        plainTextList = dataReport.ResultValue.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p)).ToList();
                    }

                    float lastYPositon = 0;
                    foreach (string textResult in plainTextList)
                    {

                        string formatText = textResult;

                        if (formatText.StartsWith("-"))
                        {
                            formatText = formatText.Remove(0, 1);
                        }

                        if (formatText.StartsWith("."))
                        {
                            formatText = formatText.Remove(0, 1);
                        }

                        if (formatText.EndsWith("."))
                        {
                            formatText = formatText.Remove(formatText.Length - 1, 1);
                        }

                        if (formatText.EndsWith(":"))
                        {
                            formatText = formatText.Remove(formatText.IndexOf(":"), 1);
                        }

                        if (formatText.EndsWith("\\"))
                        {
                            formatText = formatText.Remove(formatText.IndexOf("\\"), 1);
                        }

                        formatText = formatText.Trim();
                        XrayTranslateMappingModel row = dtResultMapping.FirstOrDefault(p => p.EngResult.ToString().ToLower() == formatText.ToLower());
                        if (row != null && row.IsKeyword == false && row.ThaiResult != "ปกติ")
                        {
                            if (Detail.Controls.Count == 0)
                            {
                                lastYPositon = 30.00001F;
                            }
                            else
                            {
                                lastYPositon = lastYPositon + 100.00001F;
                            }

                            XRCheckBox xrCheckBox = new XRCheckBox();
                            xrCheckBox.LocationFloat = new DevExpress.Utils.PointFloat(35.33F, lastYPositon);
                            xrCheckBox.Name = "xrCheckBox1";
                            xrCheckBox.SizeF = new System.Drawing.SizeF(18.75F, 23F);
                            xrCheckBox.StylePriority.UseFont = false;
                            xrCheckBox.Checked = true;
                            Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrCheckBox });

                            XRLabel xrlabel = new XRLabel();
                            xrlabel.Font = new System.Drawing.Font("Angsana New", 20F, System.Drawing.FontStyle.Bold);
                            xrlabel.LocationFloat = new DevExpress.Utils.PointFloat(56.08F, lastYPositon - 10);
                            xrlabel.SizeF = new System.Drawing.SizeF(742F, 80F);
                            xrlabel.Text = formatText;
                            xrlabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                            Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrlabel });
                        }
                        else if (row == null)
                        {
                            if (Detail.Controls.Count == 0)
                            {
                                lastYPositon = 30.00001F;
                            }
                            else
                            {
                                lastYPositon = lastYPositon + 100.00001F;
                            }

                            XRCheckBox xrCheckBox = new XRCheckBox();
                            xrCheckBox.LocationFloat = new DevExpress.Utils.PointFloat(35.33F, lastYPositon);
                            xrCheckBox.Name = "xrCheckBox1";
                            xrCheckBox.SizeF = new System.Drawing.SizeF(18.75F, 23F);
                            xrCheckBox.StylePriority.UseFont = false;
                            xrCheckBox.Checked = true;
                            Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrCheckBox });

                            XRLabel xrlabel = new XRLabel();
                            xrlabel.Font = new System.Drawing.Font("Angsana New", 20F, System.Drawing.FontStyle.Bold);
                            xrlabel.LocationFloat = new DevExpress.Utils.PointFloat(56.08F, lastYPositon - 10);
                            xrlabel.SizeF = new System.Drawing.SizeF(742F, 80F);
                            xrlabel.Text = formatText;
                            xrlabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                            Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrlabel });
                        }
                    }
                }


                #region Optional Condition

                //List<XrayTranslateMappingModel> dtResultMapping = (new RadiologyService()).GetXrayTranslateMapping();


                //List<string> plainTextList;
                //if (dataReport.ResultValue.ToUpper().Contains("IMPRESSION") || dataReport.ResultValue.ToUpper().Contains("CONCLUSION"))
                //{
                //    int startIndex;
                //    if (dataReport.ResultValue.ToUpper().Contains("IMPRESSION"))
                //    {
                //        startIndex = dataReport.ResultValue.ToUpper().IndexOf("IMPRESSION");
                //    }
                //    else
                //    {
                //        startIndex = dataReport.ResultValue.ToUpper().IndexOf("CONCLUSION");
                //    }
                //    int endIndex = dataReport.ResultValue.Length - startIndex;
                //    string subString = dataReport.ResultValue.Substring(startIndex, endIndex);
                //    //plainTextList = subString.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Replace(".", "").Replace("-", "").Trim()).ToList();
                //    plainTextList = subString.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p)).ToList();
                //}
                //else
                //{
                //    //plainTextList = xrayResultText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Replace(".", "").Replace("-", "").Trim()).ToList();
                //    plainTextList = dataReport.ResultValue.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p)).ToList();
                //}

                //float lastYPositon = 0;
                //foreach (string textResult in plainTextList)
                //{

                //    string formatText = textResult;

                //    if (formatText.StartsWith("-"))
                //    {
                //        formatText = formatText.Remove(0, 1);
                //    }

                //    if (formatText.StartsWith("."))
                //    {
                //        formatText = formatText.Remove(0, 1);
                //    }

                //    if (formatText.EndsWith("."))
                //    {
                //        formatText = formatText.Remove(formatText.Length - 1, 1);
                //    }

                //    if (formatText.EndsWith(":"))
                //    {
                //        formatText = formatText.Remove(formatText.IndexOf(":"), 1);
                //    }

                //    if (formatText.EndsWith("\\"))
                //    {
                //        formatText = formatText.Remove(formatText.IndexOf("\\"), 1);
                //    }

                //    formatText = formatText.Trim();
                //    XrayTranslateMappingModel row = dtResultMapping.FirstOrDefault(p => p.EngResult.ToString().ToLower() == formatText.ToLower());
                //    if (row != null && row.IsKeyword == false && row.ThaiResult != "ปกติ")
                //    {
                //        if (Detail.Controls.Count == 0)
                //        {
                //            lastYPositon = 30.00001F;
                //        }
                //        else
                //        {
                //            lastYPositon = lastYPositon + 100.00001F;
                //        }

                //        XRCheckBox xrCheckBox = new XRCheckBox();
                //        xrCheckBox.LocationFloat = new DevExpress.Utils.PointFloat(35.33F, lastYPositon);
                //        xrCheckBox.Name = "xrCheckBox1";
                //        xrCheckBox.SizeF = new System.Drawing.SizeF(18.75F, 23F);
                //        xrCheckBox.StylePriority.UseFont = false;
                //        xrCheckBox.Checked = true;
                //        Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrCheckBox });

                //        XRLabel xrlabel = new XRLabel();
                //        xrlabel.Font = new System.Drawing.Font("Angsana New", 20F, System.Drawing.FontStyle.Bold);
                //        xrlabel.LocationFloat = new DevExpress.Utils.PointFloat(56.08F, lastYPositon - 10);
                //        xrlabel.SizeF = new System.Drawing.SizeF(742F, 80F);
                //        xrlabel.Text = formatText;
                //        xrlabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                //        Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrlabel });
                //    }
                //    else if (row == null)
                //    {
                //        if (Detail.Controls.Count == 0)
                //        {
                //            lastYPositon = 30.00001F;
                //        }
                //        else
                //        {
                //            lastYPositon = lastYPositon + 100.00001F;
                //        }

                //        XRCheckBox xrCheckBox = new XRCheckBox();
                //        xrCheckBox.LocationFloat = new DevExpress.Utils.PointFloat(35.33F, lastYPositon);
                //        xrCheckBox.Name = "xrCheckBox1";
                //        xrCheckBox.SizeF = new System.Drawing.SizeF(18.75F, 23F);
                //        xrCheckBox.StylePriority.UseFont = false;
                //        xrCheckBox.Checked = true;
                //        Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrCheckBox });

                //        XRLabel xrlabel = new XRLabel();
                //        xrlabel.Font = new System.Drawing.Font("Angsana New", 20F, System.Drawing.FontStyle.Bold);
                //        xrlabel.LocationFloat = new DevExpress.Utils.PointFloat(56.08F, lastYPositon - 10);
                //        xrlabel.SizeF = new System.Drawing.SizeF(742F, 80F);
                //        xrlabel.Text = formatText;
                //        xrlabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                //        Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrlabel });
                //    }
                //}

                //if (Detail.Controls.Count <= 0)
                //{
                //    XRCheckBox xrCheckBox = new XRCheckBox();
                //    xrCheckBox.LocationFloat = new DevExpress.Utils.PointFloat(35.33F, 30.00001F);
                //    xrCheckBox.Name = "xrCheckBox1";
                //    xrCheckBox.SizeF = new System.Drawing.SizeF(18.75F, 23F);
                //    xrCheckBox.StylePriority.UseFont = false;
                //    xrCheckBox.Checked = true;
                //    Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrCheckBox });

                //    XRLabel xrlabel = new XRLabel();
                //    xrlabel.Font = new System.Drawing.Font("Angsana New", 20F, System.Drawing.FontStyle.Bold);
                //    xrlabel.LocationFloat = new DevExpress.Utils.PointFloat(56.08F, 20.00001F);
                //    xrlabel.SizeF = new System.Drawing.SizeF(742F, 80F);
                //    xrlabel.Text = "Negative";
                //    xrlabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                //    Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrlabel });
                //}

                #endregion
            }
        }
    }
}
