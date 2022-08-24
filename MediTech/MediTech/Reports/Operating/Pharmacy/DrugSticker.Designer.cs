namespace MediTech.Reports.Operating.Pharmacy
{
    partial class DrugSticker
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrugSticker));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPrescriptionNumber = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbFooterOrganisation = new DevExpress.XtraReports.UI.XRLabel();
            this.lbAddress = new DevExpress.XtraReports.UI.XRLabel();
            this.logo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PrescriptionItemUID = new DevExpress.XtraReports.Parameters.Parameter();
            this.ExpiryDate = new DevExpress.XtraReports.Parameters.Parameter();
            this.OrganisationUID = new DevExpress.XtraReports.Parameters.Parameter();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.formattingRule1 = new DevExpress.XtraReports.UI.FormattingRule();
            this.DrugLabelF = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel6,
            this.xrLabel13,
            this.xrLabel11,
            this.xrLabel8,
            this.xrLabel7,
            this.lblPrescriptionNumber,
            this.xrLabel2,
            this.xrLabel3,
            this.xrLabel1,
            this.xrLabel4,
            this.xrLabel5,
            this.lbFooterOrganisation,
            this.lbAddress,
            this.logo});
            this.Detail.HeightF = 175.7077F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel6
            // 
            this.xrLabel6.CanGrow = false;
            this.xrLabel6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "DrugLabelF")});
            this.xrLabel6.Font = new System.Drawing.Font("Angsana New", 12F);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(9F, 73.04642F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(320F, 17.99998F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UsePadding = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel13
            // 
            this.xrLabel13.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel13.CanGrow = false;
            this.xrLabel13.Font = new System.Drawing.Font("Angsana New", 11F);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(182.4625F, 29.9552F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(147.7042F, 15F);
            this.xrLabel13.StylePriority.UseBorders = false;
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UsePadding = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = "Expiry : [Parameters.ExpiryDate!dd/MM/yyyy]";
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel11
            // 
            this.xrLabel11.CanGrow = false;
            this.xrLabel11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "LocalInstructionText")});
            this.xrLabel11.Font = new System.Drawing.Font("Angsana New", 12F);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(8.999902F, 116.1358F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(321.4167F, 18F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UsePadding = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel8.CanGrow = false;
            this.xrLabel8.Font = new System.Drawing.Font("Angsana New", 11F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(181.7124F, 14.95516F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(148.4542F, 15F);
            this.xrLabel8.StylePriority.UseBorders = false;
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UsePadding = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "(DOB:[DateofBirth!dd/MM/yyyy])";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel7.CanGrow = false;
            this.xrLabel7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PrescribedDttm", "{0:dd/MM/yyyy HH:mm}")});
            this.xrLabel7.Font = new System.Drawing.Font("Angsana New", 11F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(240.0456F, 0.1496253F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(90.12103F, 15F);
            this.xrLabel7.StylePriority.UseBorders = false;
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UsePadding = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblPrescriptionNumber
            // 
            this.lblPrescriptionNumber.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblPrescriptionNumber.CanGrow = false;
            this.lblPrescriptionNumber.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PrescriptionNumber")});
            this.lblPrescriptionNumber.Font = new System.Drawing.Font("Angsana New", 11F);
            this.lblPrescriptionNumber.LocationFloat = new DevExpress.Utils.PointFloat(130.1286F, 0.1496177F);
            this.lblPrescriptionNumber.Name = "lblPrescriptionNumber";
            this.lblPrescriptionNumber.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblPrescriptionNumber.SizeF = new System.Drawing.SizeF(105.4583F, 15F);
            this.lblPrescriptionNumber.StylePriority.UseBorders = false;
            this.lblPrescriptionNumber.StylePriority.UseFont = false;
            this.lblPrescriptionNumber.StylePriority.UsePadding = false;
            this.lblPrescriptionNumber.StylePriority.UseTextAlignment = false;
            this.lblPrescriptionNumber.Text = "lblPrescriptionNumber";
            this.lblPrescriptionNumber.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel2
            // 
            this.xrLabel2.CanGrow = false;
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FrequencyDefinition")});
            this.xrLabel2.Font = new System.Drawing.Font("Angsana New", 12F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(8.750012F, 87.59412F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(321.4166F, 18F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UsePadding = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel3
            // 
            this.xrLabel3.CanGrow = false;
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PatientInstruction")});
            this.xrLabel3.Font = new System.Drawing.Font("Angsana New", 12F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(9.000002F, 101.9691F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(321.4166F, 18F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UsePadding = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.CanGrow = false;
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PatientName")});
            this.xrLabel1.Font = new System.Drawing.Font("Angsana New", 12F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(8.750012F, 42.26072F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(321.6666F, 18F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UsePadding = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel4
            // 
            this.xrLabel4.CanGrow = false;
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "DrugName")});
            this.xrLabel4.Font = new System.Drawing.Font("Angsana New", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(8.750012F, 57.5107F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(321.6666F, 18F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UsePadding = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "[BirthDate]";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel5
            // 
            this.xrLabel5.CanGrow = false;
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "DoctorName")});
            this.xrLabel5.Font = new System.Drawing.Font("Angsana New", 12F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(9.000002F, 129.1357F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(172.5366F, 18F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UsePadding = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lbFooterOrganisation
            // 
            this.lbFooterOrganisation.CanGrow = false;
            this.lbFooterOrganisation.Font = new System.Drawing.Font("Angsana New", 12F);
            this.lbFooterOrganisation.LocationFloat = new DevExpress.Utils.PointFloat(9.000003F, 141.886F);
            this.lbFooterOrganisation.Name = "lbFooterOrganisation";
            this.lbFooterOrganisation.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.lbFooterOrganisation.SizeF = new System.Drawing.SizeF(321.4166F, 18.82166F);
            this.lbFooterOrganisation.StylePriority.UseFont = false;
            this.lbFooterOrganisation.StylePriority.UsePadding = false;
            this.lbFooterOrganisation.StylePriority.UseTextAlignment = false;
            this.lbFooterOrganisation.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lbAddress
            // 
            this.lbAddress.CanGrow = false;
            this.lbAddress.Font = new System.Drawing.Font("Angsana New", 12F);
            this.lbAddress.LocationFloat = new DevExpress.Utils.PointFloat(9F, 159.7077F);
            this.lbAddress.Multiline = true;
            this.lbAddress.Name = "lbAddress";
            this.lbAddress.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.lbAddress.SizeF = new System.Drawing.SizeF(321.4166F, 15F);
            this.lbAddress.StylePriority.UseFont = false;
            this.lbAddress.StylePriority.UsePadding = false;
            this.lbAddress.StylePriority.UseTextAlignment = false;
            this.lbAddress.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // logo
            // 
            this.logo.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("logo.ImageSource"));
            this.logo.LocationFloat = new DevExpress.Utils.PointFloat(9.000002F, 0F);
            this.logo.Name = "logo";
            this.logo.SizeF = new System.Drawing.SizeF(105.279F, 42.26072F);
            this.logo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
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
            this.BottomMargin.HeightF = 1.017151F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PrescriptionItemUID
            // 
            this.PrescriptionItemUID.Description = "PrescriptionItemUID";
            this.PrescriptionItemUID.Name = "PrescriptionItemUID";
            this.PrescriptionItemUID.Type = typeof(long);
            this.PrescriptionItemUID.ValueInfo = "0";
            this.PrescriptionItemUID.Visible = false;
            // 
            // ExpiryDate
            // 
            this.ExpiryDate.Description = "วันหมดอายุ";
            this.ExpiryDate.Name = "ExpiryDate";
            this.ExpiryDate.Type = typeof(System.DateTime);
            this.ExpiryDate.Visible = false;
            // 
            // OrganisationUID
            // 
            this.OrganisationUID.Name = "OrganisationUID";
            this.OrganisationUID.Type = typeof(long);
            this.OrganisationUID.ValueInfo = "0";
            this.OrganisationUID.Visible = false;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(MediTech.Model.Report.DrugStickerModel);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // formattingRule1
            // 
            this.formattingRule1.Condition = "IsNull([Quantity])";
            this.formattingRule1.Formatting.Visible = DevExpress.Utils.DefaultBoolean.False;
            this.formattingRule1.Name = "formattingRule1";
            // 
            // DrugLabelF
            // 
            this.DrugLabelF.Expression = "[DrugLable] + \' #\' + [Quantity]";
            this.DrugLabelF.Name = "DrugLabelF";
            // 
            // DrugSticker
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.DrugLabelF});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataSource = this.objectDataSource1;
            this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
            this.formattingRule1});
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 1);
            this.PageHeight = 175;
            this.PageWidth = 340;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.PrescriptionItemUID,
            this.ExpiryDate,
            this.OrganisationUID});
            this.Version = "20.2";
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRLabel lbFooterOrganisation;
        private DevExpress.XtraReports.UI.XRLabel lbAddress;
        private DevExpress.XtraReports.UI.XRPictureBox logo;
        private DevExpress.XtraReports.Parameters.Parameter PrescriptionItemUID;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel lblPrescriptionNumber;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel xrLabel13;
        private DevExpress.XtraReports.Parameters.Parameter ExpiryDate;
        private DevExpress.XtraReports.Parameters.Parameter OrganisationUID;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.FormattingRule formattingRule1;
        private DevExpress.XtraReports.UI.CalculatedField DrugLabelF;
    }
}
