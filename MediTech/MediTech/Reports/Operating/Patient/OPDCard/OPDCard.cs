using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Model;
using System.Collections.Generic;
using MediTech.DataService;
using MediTech.Model.Report;
using System.Linq;
using System.Windows.Media.Imaging;
using System.IO;

namespace MediTech.Reports.Operating.Patient
{
    public partial class OPDCard : DevExpress.XtraReports.UI.XtraReport
    {
        List<OPDCardModel> listData;
        public OPDCard()
        {
            InitializeComponent();
            this.BeforePrint += OPDCard_BeforePrint;
            xrSubreport1.BeforePrint += xrSubreport1_BeforePrint;
            xrSubreport2.BeforePrint += xrSubreport2_BeforePrint;
        }


        void OPDCard_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            listData = new List<OPDCardModel>();
            long patientUID = long.Parse(this.Parameters["PatientUID"].Value.ToString());
            long patientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            listData = (new ReportsService()).PrintOPDCard(patientUID, patientVisitUID);
            this.DataSource = listData;

            if (listData != null && listData.Count > 0)
            {
                string healthOrganisationCode = listData.FirstOrDefault().OrganisationCode;
                if (healthOrganisationCode.ToUpper().Contains("BRXG"))
                {
                    Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoBRXG.png", UriKind.Absolute);
                    BitmapImage imageSource = new BitmapImage(uri);
                    using (MemoryStream outStream = new MemoryStream())
                    {
                        BitmapEncoder enc = new BmpBitmapEncoder();
                        enc.Frames.Add(BitmapFrame.Create(imageSource));
                        enc.Save(outStream);
                        this.logo.Image = System.Drawing.Image.FromStream(outStream);
                        this.lblOrganisationName.Text = "บีอาร์เอ็กซ์จีสหคลินิค (BRXG Polyclinic)";
                    }
                }
                else if (healthOrganisationCode.ToUpper().Contains("DRC"))
                {
                    Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoDRC.png", UriKind.Absolute);
                    BitmapImage imageSource = new BitmapImage(uri);
                    using (MemoryStream outStream = new MemoryStream())
                    {
                        BitmapEncoder enc = new BmpBitmapEncoder();
                        enc.Frames.Add(BitmapFrame.Create(imageSource));
                        enc.Save(outStream);
                        this.logo.Image = System.Drawing.Image.FromStream(outStream);
                        this.lblOrganisationName.Text = "ดี อาร์ ซี คลินิกการพยาบาลและการผดุงครรภ์ เลขที่ 125/17 หมู่ 1 ต.สำนักทอง อ.เมื่อง จ.ระยอง 21000 โทร. 098-746-3622";
                    }
                }
            }
            else
            {
                Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoBRXG.png", UriKind.Absolute);
                BitmapImage imageSource = new BitmapImage(uri);
                using (MemoryStream outStream = new MemoryStream())
                {
                    BitmapEncoder enc = new BmpBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(imageSource));
                    enc.Save(outStream);
                    Image image = System.Drawing.Image.FromStream(outStream);
                    this.lblOrganisationName.Text = "บีอาร์เอ็กซ์จีสหคลินิค (BRXG Polyclinic)";
                }
            }
        }
        void xrSubreport2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (listData != null)
            {
                foreach (var item in listData)
                {
                    ((XRSubreport)sender).ReportSource.DataSource = item.PatientDrugDetail;
                }

            }
        }

        void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (listData != null)
            {
                foreach (var item in listData)
                {
                    ((XRSubreport)sender).ReportSource.DataSource = item.PateintProblem;
                }

            }

        }
    }
}
