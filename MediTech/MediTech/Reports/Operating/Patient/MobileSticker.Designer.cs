namespace MediTech.Reports.Operating.Patient
{
    partial class MobileSticker
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
            DevExpress.XtraPrinting.BarCode.Code128Generator code128Generator1 = new DevExpress.XtraPrinting.BarCode.Code128Generator();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.lblNo = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrBarCode1 = new DevExpress.XtraReports.UI.XRBarCode();
            this.HN = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.PatientName = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.No = new DevExpress.XtraReports.Parameters.Parameter();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.Age = new DevExpress.XtraReports.Parameters.Parameter();
            this.BirthDttm = new DevExpress.XtraReports.Parameters.Parameter();
            this.CheckUp = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblNo,
            this.xrLabel6,
            this.xrBarCode1,
            this.xrLabel2,
            this.xrLabel1,
            this.xrLabel4,
            this.xrLabel5});
            this.Detail.HeightF = 95F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 2, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblNo
            // 
            this.lblNo.CanGrow = false;
            this.lblNo.Font = new System.Drawing.Font("Times New Roman", 16F, System.Drawing.FontStyle.Bold);
            this.lblNo.LocationFloat = new DevExpress.Utils.PointFloat(10.37037F, 0F);
            this.lblNo.Name = "lblNo";
            this.lblNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
            this.lblNo.SizeF = new System.Drawing.SizeF(52.67386F, 21.5116F);
            this.lblNo.StylePriority.UseFont = false;
            this.lblNo.StylePriority.UsePadding = false;
            this.lblNo.StylePriority.UseTextAlignment = false;
            this.lblNo.Text = " [Parameters.No]";
            this.lblNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.lblNo.WordWrap = false;
            // 
            // xrLabel6
            // 
            this.xrLabel6.CanGrow = false;
            this.xrLabel6.Font = new System.Drawing.Font("Angsana New", 11F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(58.97015F, 0F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(132.3632F, 17.43752F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UsePadding = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "[Parameters.CheckUp]";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrLabel6.WordWrap = false;
            // 
            // xrBarCode1
            // 
            this.xrBarCode1.Alignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrBarCode1.BorderColor = System.Drawing.Color.White;
            this.xrBarCode1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.HN, "Text", "")});
            this.xrBarCode1.LocationFloat = new DevExpress.Utils.PointFloat(10F, 56.07822F);
            this.xrBarCode1.Module = 1F;
            this.xrBarCode1.Name = "xrBarCode1";
            this.xrBarCode1.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrBarCode1.ShowText = false;
            this.xrBarCode1.SizeF = new System.Drawing.SizeF(180F, 30.6255F);
            this.xrBarCode1.StylePriority.UseBackColor = false;
            this.xrBarCode1.StylePriority.UseBorderColor = false;
            this.xrBarCode1.StylePriority.UseBorderWidth = false;
            this.xrBarCode1.StylePriority.UseFont = false;
            this.xrBarCode1.StylePriority.UseTextAlignment = false;
            code128Generator1.CharacterSet = DevExpress.XtraPrinting.BarCode.Code128Charset.CharsetAuto;
            this.xrBarCode1.Symbology = code128Generator1;
            this.xrBarCode1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // HN
            // 
            this.HN.Description = "HN";
            this.HN.Name = "HN";
            // 
            // xrLabel2
            // 
            this.xrLabel2.CanGrow = false;
            this.xrLabel2.Font = new System.Drawing.Font("Angsana New", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(155.9167F, 37.68289F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(35.41667F, 15.33335F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UsePadding = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "Age: [Parameters.Age]";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrLabel2.WordWrap = false;
            // 
            // xrLabel1
            // 
            this.xrLabel1.CanGrow = false;
            this.xrLabel1.Font = new System.Drawing.Font("Angsana New", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(104.3931F, 22.06715F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(86.94028F, 16.54167F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "Birth Date: [Parameters.BirthDttm]";
            this.xrLabel1.WordWrap = false;
            // 
            // xrLabel4
            // 
            this.xrLabel4.CanGrow = false;
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.PatientName, "Text", "")});
            this.xrLabel4.Font = new System.Drawing.Font("Angsana New", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(10.18519F, 34.90514F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(145.7315F, 19.40741F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.WordWrap = false;
            // 
            // PatientName
            // 
            this.PatientName.Description = "PatientName";
            this.PatientName.Name = "PatientName";
            // 
            // xrLabel5
            // 
            this.xrLabel5.CanGrow = false;
            this.xrLabel5.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(10.18519F, 17.43752F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(86.46512F, 20.24537F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "HN: [Parameters.HN]";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel5.WordWrap = false;
            // 
            // No
            // 
            this.No.Description = "No";
            this.No.Name = "No";
            this.No.Type = typeof(int);
            this.No.ValueInfo = "0";
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
            // Age
            // 
            this.Age.Description = "Age";
            this.Age.Name = "Age";
            // 
            // BirthDttm
            // 
            this.BirthDttm.Description = "BirthDttm";
            this.BirthDttm.Name = "BirthDttm";
            // 
            // CheckUp
            // 
            this.CheckUp.Description = "CheckUp";
            this.CheckUp.Name = "CheckUp";
            this.CheckUp.Visible = false;
            // 
            // MobileSticker
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.BorderWidth = 0F;
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageHeight = 95;
            this.PageWidth = 200;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.PatientName,
            this.Age,
            this.HN,
            this.No,
            this.BirthDttm,
            this.CheckUp});
            this.Version = "17.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.Parameters.Parameter PatientName;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.Parameters.Parameter Age;
        private DevExpress.XtraReports.Parameters.Parameter HN;
        private DevExpress.XtraReports.Parameters.Parameter No;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.Parameters.Parameter BirthDttm;
        private DevExpress.XtraReports.Parameters.Parameter CheckUp;
        internal DevExpress.XtraReports.UI.XRBarCode xrBarCode1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        public DevExpress.XtraReports.UI.XRLabel lblNo;
        internal DevExpress.XtraReports.UI.DetailBand Detail;
    }
}
