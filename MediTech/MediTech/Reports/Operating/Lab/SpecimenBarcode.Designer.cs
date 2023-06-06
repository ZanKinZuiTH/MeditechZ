namespace MediTech.Reports.Operating.Lab
{
    partial class SpecimenBarcode
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
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.CollectionDttm = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.SpecimenName = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.RequestNumber = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrBarCode1 = new DevExpress.XtraReports.UI.XRBarCode();
            this.PatientID = new DevExpress.XtraReports.Parameters.Parameter();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.BirthDate = new DevExpress.XtraReports.Parameters.Parameter();
            this.PatientName = new DevExpress.XtraReports.Parameters.Parameter();
            this.Age = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.xrLabel1,
            this.xrLabel2,
            this.xrLabel3,
            this.xrBarCode1});
            this.Detail.HeightF = 70.33329F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel4
            // 
            this.xrLabel4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "?CollectionDttm")});
            this.xrLabel4.Font = new System.Drawing.Font("Angsana New", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(9.999999F, 56.00003F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(161.25F, 13.49994F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // CollectionDttm
            // 
            this.CollectionDttm.Description = "CollectionDttm";
            this.CollectionDttm.Name = "CollectionDttm";
            this.CollectionDttm.Visible = false;
            // 
            // xrLabel1
            // 
            this.xrLabel1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "?SpecimenName")});
            this.xrLabel1.Font = new System.Drawing.Font("Angsana New", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(9.999999F, 42.83334F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(161.25F, 13.49994F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // SpecimenName
            // 
            this.SpecimenName.Description = "Parameter1";
            this.SpecimenName.Name = "SpecimenName";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Angle = 90F;
            this.xrLabel2.CanGrow = false;
            this.xrLabel2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "?RequestNumber")});
            this.xrLabel2.Font = new System.Drawing.Font("Angsana New", 9F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(175.3333F, 1.666667F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(18.33331F, 67.83331F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // RequestNumber
            // 
            this.RequestNumber.Description = "RequestNumber";
            this.RequestNumber.Name = "RequestNumber";
            this.RequestNumber.Visible = false;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Angsana New", 10F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(9.999999F, 27.49999F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(161.25F, 16.09261F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "[Parameters.PatientName] Age: [Parameters.Age]";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrBarCode1
            // 
            this.xrBarCode1.AutoModule = true;
            this.xrBarCode1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "?RequestNumber")});
            this.xrBarCode1.LocationFloat = new DevExpress.Utils.PointFloat(9.999999F, 1F);
            this.xrBarCode1.Module = 1F;
            this.xrBarCode1.Name = "xrBarCode1";
            this.xrBarCode1.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrBarCode1.SizeF = new System.Drawing.SizeF(161.25F, 25.5F);
            this.xrBarCode1.StylePriority.UseTextAlignment = false;
            this.xrBarCode1.Symbology = code128Generator1;
            // 
            // PatientID
            // 
            this.PatientID.Description = "PatientID";
            this.PatientID.Name = "PatientID";
            this.PatientID.Visible = false;
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
            this.BottomMargin.HeightF = 1.333364F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BirthDate
            // 
            this.BirthDate.Description = "BirthDate";
            this.BirthDate.Name = "BirthDate";
            this.BirthDate.Visible = false;
            // 
            // PatientName
            // 
            this.PatientName.Description = "PatientName";
            this.PatientName.Name = "PatientName";
            this.PatientName.Visible = false;
            // 
            // Age
            // 
            this.Age.Description = "Parameter1";
            this.Age.Name = "Age";
            // 
            // SpecimenBarcode
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 1);
            this.PageHeight = 85;
            this.PageWidth = 200;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.RequestNumber,
            this.PatientID,
            this.BirthDate,
            this.PatientName,
            this.SpecimenName,
            this.CollectionDttm,
            this.Age});
            this.Version = "22.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.Parameters.Parameter RequestNumber;
        private DevExpress.XtraReports.Parameters.Parameter PatientID;
        private DevExpress.XtraReports.Parameters.Parameter BirthDate;
        private DevExpress.XtraReports.Parameters.Parameter PatientName;
        private DevExpress.XtraReports.Parameters.Parameter SpecimenName;
        private DevExpress.XtraReports.Parameters.Parameter CollectionDttm;
        private DevExpress.XtraReports.Parameters.Parameter Age;
        private DevExpress.XtraReports.UI.XRBarCode xrBarCode1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
    }
}
