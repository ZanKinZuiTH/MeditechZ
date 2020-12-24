using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class CheckupSummaryModel
    {
        public string GroupName { get; set; }
        public int NormalCount { get; set; }
        public double NormalPercent { get; set; }
        public int AbnormalCount { get; set; }
        public double AbnormalPercent { get; set; }
        public int AttentionCount { get; set; }
        public double AttentionPercent { get; set; }
        public int CheckinCount { get; set; }
        public int NonCheckinCount { get; set; }
        public int TotalCount { get; set; }
        public int GPRSTUID { get; set; }
    }
}
