namespace MediTech.Reports.Statistic.Registration
{
    partial class VisitTimesStatistic
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrChart1 = new DevExpress.XtraReports.UI.XRChart();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.lblReportHeader = new DevExpress.XtraReports.UI.XRLabel();
            this.DateTo = new DevExpress.XtraReports.Parameters.Parameter();
            this.DateFrom = new DevExpress.XtraReports.Parameters.Parameter();
            this.OrganisationList = new DevExpress.XtraReports.Parameters.Parameter();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.VISTYUID = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseTextAlignment = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrChart1
            // 
            this.xrChart1.BorderColor = System.Drawing.Color.Black;
            this.xrChart1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrChart1.Legend.Name = "Default Legend";
            this.xrChart1.LocationFloat = new DevExpress.Utils.PointFloat(303.125F, 10.49999F);
            this.xrChart1.Name = "xrChart1";
            this.xrChart1.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.xrChart1.SizeF = new System.Drawing.SizeF(787.5001F, 423.3334F);
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblReportHeader});
            this.ReportHeader.HeightF = 81.25F;
            this.ReportHeader.Name = "ReportHeader";
            this.ReportHeader.StylePriority.UseBorders = false;
            // 
            // lblReportHeader
            // 
            this.lblReportHeader.Font = new System.Drawing.Font("Angsana New", 18F, System.Drawing.FontStyle.Bold);
            this.lblReportHeader.LocationFloat = new DevExpress.Utils.PointFloat(487.5F, 29.54165F);
            this.lblReportHeader.Name = "lblReportHeader";
            this.lblReportHeader.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblReportHeader.SizeF = new System.Drawing.SizeF(387.5F, 29.25F);
            this.lblReportHeader.StylePriority.UseFont = false;
            this.lblReportHeader.StylePriority.UseTextAlignment = false;
            this.lblReportHeader.Text = "รายงานสถิติช่วงเวลาผู้เข้ารับบริการ ";
            this.lblReportHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // DateTo
            // 
            this.DateTo.Name = "DateTo";
            this.DateTo.Type = typeof(System.DateTime);
            this.DateTo.Visible = false;
            // 
            // DateFrom
            // 
            this.DateFrom.Name = "DateFrom";
            this.DateFrom.Type = typeof(System.DateTime);
            this.DateFrom.Visible = false;
            // 
            // OrganisationList
            // 
            this.OrganisationList.Name = "OrganisationList";
            this.OrganisationList.Visible = false;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrChart1});
            this.PageHeader.HeightF = 433.8334F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Angsana New", 15F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 23.12501F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(282.2917F, 29.25F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "ตารางแสดงช่วงเวลาที่ผู้ป่วยเข้ามารับบริการ";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // VISTYUID
            // 
            this.VISTYUID.Description = "Parameter1";
            this.VISTYUID.Name = "VISTYUID";
            this.VISTYUID.Type = typeof(int);
            this.VISTYUID.ValueInfo = "0";
            this.VISTYUID.Visible = false;
            // 
            // VisitTimesStatistic
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageHeader});
            this.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageHeight = 827;
            this.PageWidth = 1169;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.OrganisationList,
            this.DateFrom,
            this.DateTo,
            this.VISTYUID});
            this.Version = "17.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrChart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.Parameters.Parameter OrganisationList;
        private DevExpress.XtraReports.Parameters.Parameter DateFrom;
        private DevExpress.XtraReports.Parameters.Parameter DateTo;
        private DevExpress.XtraReports.UI.XRLabel lblReportHeader;
        private DevExpress.XtraReports.UI.XRChart xrChart1;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.Parameters.Parameter VISTYUID;
    }
}
