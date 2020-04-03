using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Operating.Patient
{
    public partial class AppointmentCard : DevExpress.XtraReports.UI.XtraReport
    {
        public AppointmentCard()
        {
            InitializeComponent();
            this.BeforePrint += AppointmentCard_BeforePrint;
        }

        private void AppointmentCard_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int bookUID = int.Parse(this.Parameters["BookUID"].Value.ToString());
            var dataSource = (new ReportsService()).PrintPatientBooking(bookUID);
      
         this.DataSource = dataSource;
      
         }
    }
}
