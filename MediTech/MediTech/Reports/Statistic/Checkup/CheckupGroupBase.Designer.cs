namespace MediTech.Reports.Statistic.Checkup
{
    partial class CheckupGroupBase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckupGroupBase));
            DevExpress.XtraReports.Parameters.StaticListLookUpSettings staticListLookUpSettings1 = new DevExpress.XtraReports.Parameters.StaticListLookUpSettings();
            this.DetailBase = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMarginBase = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMarginBase = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeaderBase = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.lbTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.LogoType = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // DetailBase
            // 
            this.DetailBase.Name = "DetailBase";
            this.DetailBase.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.DetailBase.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMarginBase
            // 
            this.TopMarginBase.HeightF = 0F;
            this.TopMarginBase.Name = "TopMarginBase";
            this.TopMarginBase.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMarginBase.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMarginBase
            // 
            this.BottomMarginBase.HeightF = 30F;
            this.BottomMarginBase.Name = "BottomMarginBase";
            this.BottomMarginBase.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMarginBase.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeaderBase
            // 
            this.PageHeaderBase.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lbTitle,
            this.xrLabel1,
            this.xrPictureBox1});
            this.PageHeaderBase.HeightF = 121F;
            this.PageHeaderBase.Name = "PageHeaderBase";
            // 
            // lbTitle
            // 
            this.lbTitle.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lbTitle.CanShrink = true;
            this.lbTitle.Font = new System.Drawing.Font("Angsana New", 16F, System.Drawing.FontStyle.Bold);
            this.lbTitle.ForeColor = System.Drawing.Color.Blue;
            this.lbTitle.LocationFloat = new DevExpress.Utils.PointFloat(256.6667F, 65.99995F);
            this.lbTitle.Multiline = true;
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbTitle.SizeF = new System.Drawing.SizeF(591.7916F, 29.6667F);
            this.lbTitle.StylePriority.UseBorderColor = false;
            this.lbTitle.StylePriority.UseBorders = false;
            this.lbTitle.StylePriority.UseFont = false;
            this.lbTitle.StylePriority.UseForeColor = false;
            this.lbTitle.StylePriority.UseTextAlignment = false;
            this.lbTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel1.Font = new System.Drawing.Font("Angsana New", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(860.6667F, 10F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(278.3333F, 94.33334F);
            this.xrLabel1.StylePriority.UseBorders = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "BRXG Polyclinic\r\n155/196 หมู่ 2 ต.ทับมา อ.เมือง จ.ระยอง 21000\r\nbrxggroup@brxggrou" +
    "p.com Tel 033-060-399\r\n เลขที่ใบอนุญาต 21110000362";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBox1.ImageSource"));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(26.66667F, 26.66666F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(220F, 68.99999F);
            this.xrPictureBox1.StylePriority.UseBorders = false;
            // 
            // LogoType
            // 
            this.LogoType.Description = "Logo";
            this.LogoType.Name = "LogoType";
            this.LogoType.Type = typeof(int);
            this.LogoType.ValueInfo = "0";
            staticListLookUpSettings1.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(0, "โรงพยาบาลบูรพารักษ์"));
            staticListLookUpSettings1.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(1, "บีอาร์เอ็กซ์จีสหคลินิก"));
            staticListLookUpSettings1.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(2, "BRXG Company"));
            this.LogoType.ValueSourceSettings = staticListLookUpSettings1;
            // 
            // CheckupGroupBase
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.DetailBase,
            this.TopMarginBase,
            this.BottomMarginBase,
            this.PageHeaderBase});
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 30);
            this.PageHeight = 827;
            this.PageWidth = 1169;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.ParameterPanelLayoutItems.AddRange(new DevExpress.XtraReports.Parameters.ParameterPanelLayoutItem[] {
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.LogoType, DevExpress.XtraReports.Parameters.Orientation.Horizontal)});
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.LogoType});
            this.RequestParameters = false;
            this.Version = "22.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion
        private DevExpress.XtraReports.UI.TopMarginBand TopMarginBase;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMarginBase;
        protected DevExpress.XtraReports.UI.DetailBand DetailBase;
        protected DevExpress.XtraReports.UI.XRPictureBox xrPictureBox1;
        public DevExpress.XtraReports.UI.XRLabel lbTitle;
        protected DevExpress.XtraReports.UI.PageHeaderBand PageHeaderBase;
        private DevExpress.XtraReports.Parameters.Parameter LogoType;
        protected DevExpress.XtraReports.UI.XRLabel xrLabel1;
    }
}
