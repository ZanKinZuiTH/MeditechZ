using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;

namespace MediTech.Reports.Operating.Patient.WellnessBook
{
    public partial class WellnessBookBack : DevExpress.XtraReports.UI.XtraReport
    {
        public WellnessBookBack()
        {
            InitializeComponent();
            xrRichText1.BeforePrint += XrRichText1_BeforePrint;
        }

        private void XrRichText1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRRichText richText = sender as XRRichText;

            RichEditDocumentServer docServer = new RichEditDocumentServer();
            docServer.Text = richText.Text;
            docServer.Document.DefaultParagraphProperties.LineSpacingType = ParagraphLineSpacing.Multiple;
            docServer.Document.DefaultParagraphProperties.LineSpacingMultiplier = 1.3f;

            richText.Text = docServer.RtfText;
        }
    }
}
