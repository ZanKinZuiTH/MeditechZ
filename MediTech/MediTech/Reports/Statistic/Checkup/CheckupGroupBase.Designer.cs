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
            this.DetailBase = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMarginBase = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMarginBase = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeaderBase = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.lbTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // DetailBase
            // 
            this.DetailBase.HeightF = 100F;
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
            this.PageHeaderBase.HeightF = 96.00002F;
            this.PageHeaderBase.Name = "PageHeaderBase";
            // 
            // lbTitle
            // 
            this.lbTitle.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lbTitle.CanShrink = true;
            this.lbTitle.Font = new System.Drawing.Font("EucrosiaUPC", 16F, System.Drawing.FontStyle.Bold);
            this.lbTitle.ForeColor = System.Drawing.Color.Blue;
            this.lbTitle.LocationFloat = new DevExpress.Utils.PointFloat(320.3751F, 59.66665F);
            this.lbTitle.Multiline = true;
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbTitle.SizeF = new System.Drawing.SizeF(455.5832F, 29.66669F);
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
            this.xrLabel1.Font = new System.Drawing.Font("EucrosiaUPC", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(860.6667F, 11.66667F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(278.3333F, 84.33334F);
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
            this.xrPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox1.Image")));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(26.66667F, 10F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(220F, 68.99999F);
            this.xrPictureBox1.StylePriority.UseBorders = false;
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
            this.Version = "17.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion
        private DevExpress.XtraReports.UI.TopMarginBand TopMarginBase;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMarginBase;
        protected DevExpress.XtraReports.UI.DetailBand DetailBase;
        protected DevExpress.XtraReports.UI.XRPictureBox xrPictureBox1;
        protected DevExpress.XtraReports.UI.XRLabel xrLabel1;
        public DevExpress.XtraReports.UI.XRLabel lbTitle;
        protected DevExpress.XtraReports.UI.PageHeaderBand PageHeaderBase;
    }
}
