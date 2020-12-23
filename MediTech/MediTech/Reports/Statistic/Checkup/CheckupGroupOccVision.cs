using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace MediTech.Reports.Statistic.Checkup
{
    public partial class CheckupGroupOccVision : MediTech.Reports.Statistic.Checkup.CheckupGroupBase
    {
        public CheckupGroupOccVision()
        {
            InitializeComponent();
            tableOccVision.BeforePrint += TableOccVision_BeforePrint;
        }


        private void TableOccVision_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string eyeOccmedConclustion = xrTableCell30.Text;
            if (!string.IsNullOrEmpty(eyeOccmedConclustion))
            {
                string[] results = eyeOccmedConclustion.Split(',');
                string note = "";
                if (results.Length >=3 )
                {
                    note  = results[0] + ", " + results[1];
                }

                string description = "";
                string recommand = "";
                foreach (var item in results)
                {
                    if (item.Contains("ควร"))
                    {
                        int index = item.IndexOf("ควร");
                        description += string.IsNullOrEmpty(description) ? item.Substring(0, index).Trim() : " " + item.Substring(0, index).Trim();
                        recommand = item.Substring(index).Trim();

                    }

                }

                xrTableCell20.Text = description;
                xrTableCell30.Text = recommand;
                xrTableCell29.Text = note;
            }
        }
    }
}
