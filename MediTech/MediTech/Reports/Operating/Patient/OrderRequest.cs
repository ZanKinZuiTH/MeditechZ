using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Operating.Patient
{
    public partial class OrderRequest : DevExpress.XtraReports.UI.XtraReport
    {
        public OrderRequest()
        {
            InitializeComponent();
            BeforePrint += OrderRequest_BeforePrint;
        }

        private void OrderRequest_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            long PatientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var dataSource = (new ReportsService()).PrintOrderRequestCard(PatientUID, PatientVisitUID);

            this.DataSource = dataSource;
          
        }
    }
}
