using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Model;
using System.Collections.Generic;
using MediTech.DataService;
using System.Linq;
using DevExpress.DataProcessing;
using DevExpress.Xpf.CodeView;
using DevExpress.XtraCharts;

namespace MediTech.Reports.Operating.Checkup
{
    public partial class AudiogramGraph : DevExpress.XtraReports.UI.XtraReport
    {

        public AudiogramGraph()
        {
            InitializeComponent();
            BeforePrint += AudiogramGraph_BeforePrint;
            
        }

        void AudiogramGraph_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            long PatientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var dataAudio = (new ReportsService()).AudiogramResult(PatientUID, PatientVisitUID); 
            if (dataAudio != null && dataAudio.Count > 0)
            {
                lbPatientName.Text = dataAudio.FirstOrDefault().PatientName;
                lbAge.Text = dataAudio.FirstOrDefault().Age;
                lbGender.Text = dataAudio.FirstOrDefault().Gender;
                lbBirthDttm.Text = dataAudio.FirstOrDefault().BirthDttmString;
                lbWeight.Text = dataAudio.FirstOrDefault().Weight.ToString();
                lbHeight.Text = dataAudio.FirstOrDefault().Height.ToString();
                foreach (Series series in audioChartLine.Series)
                {
                    if (series.Name == "ขวา")
                    {
                        foreach (var item in dataAudio.Where(p => p.ResultItemName.EndsWith("R")).OrderBy(p => int.Parse(p.ResultItemName.Replace("R", "").Trim())))
                        {
                            SeriesPoint seriesPoint1 = new SeriesPoint(int.Parse(item.ResultItemName.Replace("R","").Trim()),item.ResultValue);
                            series.Points.Add(seriesPoint1);
                        }
                    }
                    else if(series.Name == "ซ้าย")
                    {
                        foreach (var item in dataAudio.Where(p => p.ResultItemName.EndsWith("L")).OrderBy(p => int.Parse(p.ResultItemName.Replace("L", "").Trim())))
                        {
                            SeriesPoint seriesPoint1 = new SeriesPoint(int.Parse(item.ResultItemName.Replace("L", "").Trim()), item.ResultValue);
                            series.Points.Add(seriesPoint1);
                        }
                    }
                }
            //    DevExpress.XtraCharts.SeriesPoint seriesPoint1 = new DevExpress.XtraCharts.SeriesPoint(0D, new object[] {
            //((object)(50D))});
                
            //    series1.Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] {
            //seriesPoint1});

            }
        }
    }
}
