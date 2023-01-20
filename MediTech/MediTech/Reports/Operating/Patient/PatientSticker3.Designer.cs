namespace MediTech.Reports.Operating.Patient
{
    partial class PatientSticker3
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
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.CompanyName = new DevExpress.XtraReports.Parameters.Parameter();
            this.Age = new DevExpress.XtraReports.Parameters.Parameter();
            this.PatientName = new DevExpress.XtraReports.Parameters.Parameter();
            this.HN = new DevExpress.XtraReports.Parameters.Parameter();
            this.Department = new DevExpress.XtraReports.Parameters.Parameter();
            this.EmployeeID = new DevExpress.XtraReports.Parameters.Parameter();
            this.BirthDttm = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel6,
            this.xrLabel2,
            this.xrLabel1});
            this.Detail.Name = "Detail";
            // 
            // CompanyName
            // 
            this.CompanyName.Description = "CompanyName";
            this.CompanyName.Name = "CompanyName";
            this.CompanyName.Visible = false;
            // 
            // Age
            // 
            this.Age.Description = "Age";
            this.Age.Name = "Age";
            // 
            // PatientName
            // 
            this.PatientName.Description = "PatientName";
            this.PatientName.Name = "PatientName";
            this.PatientName.Visible = false;
            // 
            // HN
            // 
            this.HN.Description = "HN";
            this.HN.Name = "HN";
            this.HN.Visible = false;
            // 
            // Department
            // 
            this.Department.Description = "Department";
            this.Department.Name = "Department";
            this.Department.Visible = false;
            // 
            // EmployeeID
            // 
            this.EmployeeID.Description = "EmployeeID";
            this.EmployeeID.Name = "EmployeeID";
            this.EmployeeID.Visible = false;
            // 
            // BirthDttm
            // 
            this.BirthDttm.Description = "BirthDttm";
            this.BirthDttm.Name = "BirthDttm";
            this.BirthDttm.Visible = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.CanGrow = false;
            this.xrLabel5.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 0F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(135.5164F, 21.74814F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = "HN: [Parameters.HN]";
            this.xrLabel5.WordWrap = false;
            // 
            // xrLabel4
            // 
            this.xrLabel4.CanGrow = false;
            this.xrLabel4.Font = new System.Drawing.Font("Angsana New", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(12.3512F, 20.08929F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(179.4712F, 23F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "[Parameters.PatientName]";
            this.xrLabel4.WordWrap = false;
            // 
            // xrLabel6
            // 
            this.xrLabel6.CanGrow = false;
            this.xrLabel6.Font = new System.Drawing.Font("Angsana New", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(12.64881F, 52.52977F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(179.4712F, 21.33333F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = "EmpID. [Parameters.EmployeeID] Dep. [Parameters.Department]";
            this.xrLabel6.WordWrap = false;
            // 
            // xrLabel2
            // 
            this.xrLabel2.CanGrow = false;
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.CompanyName, "Text", "")});
            this.xrLabel2.Font = new System.Drawing.Font("Angsana New", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(12.64881F, 67.26191F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(179.2605F, 17.16667F);
            this.xrLabel2.StylePriority.UseBorderWidth = false;
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.WordWrap = false;
            // 
            // xrLabel1
            // 
            this.xrLabel1.CanGrow = false;
            this.xrLabel1.Font = new System.Drawing.Font("Angsana New", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 37.5F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(180F, 18F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "[Parameters.BirthDttm] Age [Parameters.Age] ";
            this.xrLabel1.WordWrap = false;
            // 
            // PatientSticker3
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail});
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageHeight = 95;
            this.PageWidth = 200;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.ParameterPanelLayoutItems.AddRange(new DevExpress.XtraReports.Parameters.ParameterPanelLayoutItem[] {
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.Age, DevExpress.XtraReports.Parameters.Orientation.Horizontal),
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.PatientName, DevExpress.XtraReports.Parameters.Orientation.Horizontal),
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.HN, DevExpress.XtraReports.Parameters.Orientation.Horizontal),
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.Department, DevExpress.XtraReports.Parameters.Orientation.Horizontal),
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.EmployeeID, DevExpress.XtraReports.Parameters.Orientation.Horizontal),
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.CompanyName, DevExpress.XtraReports.Parameters.Orientation.Horizontal),
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.BirthDttm, DevExpress.XtraReports.Parameters.Orientation.Horizontal)});
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.Age,
            this.PatientName,
            this.HN,
            this.Department,
            this.EmployeeID,
            this.CompanyName,
            this.BirthDttm});
            this.SnappingMode = DevExpress.XtraReports.UI.SnappingMode.SnapToGrid;
            this.Version = "22.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.Parameters.Parameter Age;
        private DevExpress.XtraReports.Parameters.Parameter PatientName;
        private DevExpress.XtraReports.Parameters.Parameter HN;
        private DevExpress.XtraReports.Parameters.Parameter Department;
        private DevExpress.XtraReports.Parameters.Parameter EmployeeID;
        private DevExpress.XtraReports.Parameters.Parameter CompanyName;
        private DevExpress.XtraReports.Parameters.Parameter BirthDttm;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
    }
}
