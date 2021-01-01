using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Model;
using System.Collections.Generic;
using MediTech.DataService;
using DevExpress.DataProcessing;
using DevExpress.Xpf.CodeView;

namespace MediTech.Reports.Statistic.Checkup
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
            int PayorDetailUID = int.Parse(this.Parameters["PayorDetailUID"].Value.ToString());
            var DataSource = (new ReportsService()).AudiogramResult(PatientUID, PatientVisitUID, PayorDetailUID);
            if (DataSource != null)
            {
                List<PatientResultComponentModel> AudidoLeft = new List<PatientResultComponentModel>();
                List<PatientResultComponentModel> AudidoRight = new List<PatientResultComponentModel>();
                foreach(var item in DataSource)
                {
                    if (item.ResultItemName.EndsWith("R"))
                    {
                        AudidoRight.Add(item);
                    }
                    if (item.ResultItemName.EndsWith("L"))
                    {
                        AudidoLeft.Add(item);
                    }
                }

            }
        }
    }
}
