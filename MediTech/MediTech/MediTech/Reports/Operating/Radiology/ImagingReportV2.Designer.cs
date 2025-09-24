namespace MediTech.Reports.Operating.Radiology
{
    partial class ImagingReportV2
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
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.lblPatientName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCheckupDate = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.NumberCode = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.CheckupLocation = new DevExpress.XtraReports.Parameters.Parameter();
            this.lblTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.lblDoctor = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.ResultUID = new DevExpress.XtraReports.Parameters.Parameter();
            this.CheckupDate = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAge = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.HeightF = 110.7916F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
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
            this.BottomMargin.HeightF = 3F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.lblAge,
            this.lblPatientName,
            this.lblCheckupDate,
            this.xrLabel12,
            this.xrLabel11,
            this.xrLabel10,
            this.xrLabel9,
            this.xrLabel8,
            this.lblTitle,
            this.xrLabel1,
            this.xrLabel7});
            this.PageHeader.HeightF = 349.6251F;
            this.PageHeader.Name = "PageHeader";
            // 
            // lblPatientName
            // 
            this.lblPatientName.Font = new System.Drawing.Font("Angsana New", 20F, System.Drawing.FontStyle.Bold);
            this.lblPatientName.LocationFloat = new DevExpress.Utils.PointFloat(542.1458F, 276.2917F);
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblPatientName.SizeF = new System.Drawing.SizeF(256.1042F, 33.41669F);
            this.lblPatientName.StylePriority.UseFont = false;
            this.lblPatientName.Text = "xrLabel7";
            // 
            // lblCheckupDate
            // 
            this.lblCheckupDate.Font = new System.Drawing.Font("Angsana New", 20F, System.Drawing.FontStyle.Bold);
            this.lblCheckupDate.LocationFloat = new DevExpress.Utils.PointFloat(542.1458F, 242.875F);
            this.lblCheckupDate.Name = "lblCheckupDate";
            this.lblCheckupDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblCheckupDate.SizeF = new System.Drawing.SizeF(256.1042F, 33.41669F);
            this.lblCheckupDate.StylePriority.UseFont = false;
            this.lblCheckupDate.Text = "xrLabel7";
            // 
            // xrLabel12
            // 
            this.xrLabel12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.NumberCode, "Text", "")});
            this.xrLabel12.Font = new System.Drawing.Font("Angsana New", 20F, System.Drawing.FontStyle.Bold);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(150.9584F, 276.2917F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(275.5624F, 33.41669F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.Text = "xrLabel7";
            // 
            // NumberCode
            // 
            this.NumberCode.Description = "Parameter1";
            this.NumberCode.Name = "NumberCode";
            this.NumberCode.Visible = false;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Font = new System.Drawing.Font("Angsana New", 20F, System.Drawing.FontStyle.Bold);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(426.5208F, 276.2917F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(115.625F, 33.41669F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.Text = "ชื่อ-สกุล";
            // 
            // xrLabel10
            // 
            this.xrLabel10.Font = new System.Drawing.Font("Angsana New", 20F, System.Drawing.FontStyle.Bold);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(35.33338F, 276.2917F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(115.625F, 33.41669F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.Text = "รหัสโครงการ   ";
            // 
            // xrLabel9
            // 
            this.xrLabel9.Font = new System.Drawing.Font("Angsana New", 20F, System.Drawing.FontStyle.Bold);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(426.5208F, 242.875F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(115.625F, 33.41669F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.Text = "วันที่ตรวจ    ";
            // 
            // xrLabel8
            // 
            this.xrLabel8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.CheckupLocation, "Text", "")});
            this.xrLabel8.Font = new System.Drawing.Font("Angsana New", 20F, System.Drawing.FontStyle.Bold);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(150.9584F, 242.875F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(275.5624F, 33.41669F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.Text = "xrLabel7";
            // 
            // CheckupLocation
            // 
            this.CheckupLocation.Name = "CheckupLocation";
            this.CheckupLocation.Visible = false;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Angsana New", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.LocationFloat = new DevExpress.Utils.PointFloat(134.2917F, 149.6666F);
            this.lblTitle.Multiline = true;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTitle.SizeF = new System.Drawing.SizeF(498.9583F, 80.12502F);
            this.lblTitle.StylePriority.UseFont = false;
            this.lblTitle.StylePriority.UseTextAlignment = false;
            this.lblTitle.Text = "รายงานผลการตรวจเอกซเรย์ปอด\r\nโครงการตรวจสุขภาพ และเฝ้าระวังสุขภาพเชิงรุก";
            this.lblTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(112.5F, 57.08331F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(582.2916F, 92.58334F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "กลุ่มงาน รังสีวิทยา  โรงพยาบาลเฉลิมพระเกียรติ\r\nสมเด็จพระเทพรัตนราชสุดาฯสยามบรมราช" +
    "กุมารี ระยอง";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Angsana New", 20F, System.Drawing.FontStyle.Bold);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(35.33338F, 242.875F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(115.625F, 33.41669F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.Text = "สถานที่ตรวจ      ";
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblDoctor,
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel2});
            this.PageFooter.HeightF = 157.2917F;
            this.PageFooter.Name = "PageFooter";
            // 
            // lblDoctor
            // 
            this.lblDoctor.Font = new System.Drawing.Font("Angsana New", 20F, System.Drawing.FontStyle.Bold);
            this.lblDoctor.LocationFloat = new DevExpress.Utils.PointFloat(479.0834F, 7.12504F);
            this.lblDoctor.Multiline = true;
            this.lblDoctor.Name = "lblDoctor";
            this.lblDoctor.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblDoctor.SizeF = new System.Drawing.SizeF(292.7083F, 32.20832F);
            this.lblDoctor.StylePriority.UseFont = false;
            this.lblDoctor.StylePriority.UseTextAlignment = false;
            this.lblDoctor.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Font = new System.Drawing.Font("Angsana New", 20F, System.Drawing.FontStyle.Bold);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(390.6876F, 7.12504F);
            this.xrLabel5.Multiline = true;
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(88.39581F, 32.20832F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "รังสีแพทย์";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Angsana New", 20F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(79.22923F, 92.9791F);
            this.xrLabel4.Multiline = true;
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(692.5624F, 32.20831F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "โทร 0-3868-4444 ต่อ 1125-6     โทรสาร 0- 3868-7340 ";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Angsana New", 16F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(79.22923F, 60.77081F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(692.5624F, 32.20831F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "กลุ่มงานรังสีวิทยา โรงพยาบาลเฉลิมพระเกียรติ สมเด็จพระเทพรัตนราชสุดาฯสยามบรมราชกุม" +
    "ารี ระยอง";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // ResultUID
            // 
            this.ResultUID.Name = "ResultUID";
            this.ResultUID.Visible = false;
            // 
            // CheckupDate
            // 
            this.CheckupDate.Name = "CheckupDate";
            this.CheckupDate.Type = typeof(System.DateTime);
            this.CheckupDate.Visible = false;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Angsana New", 20F, System.Drawing.FontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(426.5208F, 309.7084F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(115.625F, 33.41669F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = "อายุ";
            // 
            // lblAge
            // 
            this.lblAge.Font = new System.Drawing.Font("Angsana New", 20F, System.Drawing.FontStyle.Bold);
            this.lblAge.LocationFloat = new DevExpress.Utils.PointFloat(542.1459F, 309.7084F);
            this.lblAge.Name = "lblAge";
            this.lblAge.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblAge.SizeF = new System.Drawing.SizeF(256.1042F, 33.41669F);
            this.lblAge.StylePriority.UseFont = false;
            this.lblAge.Text = "xrLabel7";
            // 
            // ImagingReportV2
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.PageFooter});
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 3);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.ResultUID,
            this.NumberCode,
            this.CheckupLocation,
            this.CheckupDate});
            this.Version = "17.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel lblDoctor;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.Parameters.Parameter ResultUID;
        private DevExpress.XtraReports.Parameters.Parameter NumberCode;
        private DevExpress.XtraReports.UI.XRLabel lblPatientName;
        private DevExpress.XtraReports.UI.XRLabel lblCheckupDate;
        private DevExpress.XtraReports.UI.XRLabel xrLabel12;
        private DevExpress.XtraReports.Parameters.Parameter CheckupLocation;
        private DevExpress.XtraReports.Parameters.Parameter CheckupDate;
        internal DevExpress.XtraReports.UI.XRLabel lblTitle;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        public DevExpress.XtraReports.UI.XRLabel lblAge;
    }
}
