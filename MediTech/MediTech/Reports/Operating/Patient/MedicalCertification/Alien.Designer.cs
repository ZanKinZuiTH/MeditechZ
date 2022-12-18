using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Windows.Controls;
using DevExpress.XtraPrinting;
using MediTech.DataService;
using MediTech.Model.Report;
using MediTech.Reports.Operating.Patient.MedicalCertification;
using MediTech.Model;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using System.IO;


namespace MediTech.Reports.Operating.Patient
{
    partial class Alien
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Alien));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel77 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel76 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel102 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel75 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel74 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel73 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel72 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel71 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel70 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel69 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel68 = new DevExpress.XtraReports.UI.XRLabel();
            this.imgGoodButSyphilis = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgGoodButElephantiasis = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgGoodButLeprosy = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgGoodButTuberculosis = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgNotGood = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgGoodBut = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgGoodhealth = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgPregnant3 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgPregnant2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgPregnant1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgNarcotic3 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgSyphilis3 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgElephantiasis3 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgLeprosy3 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgTuberculosis3 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgAlcohol2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgNarcotic2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgSyphilis2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgElephantiasis2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgAlcohol1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgNarcotic1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgSyphilis1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgElephantiasis1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgLeprosy2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgTuberculosis2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.imgLeprosy1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel81 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel79 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel78 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel103 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel67 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel61 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel62 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel63 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel64 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel65 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel54 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel55 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel56 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel57 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel58 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel59 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel60 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel53 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel52 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel51 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel50 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel49 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel48 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel47 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel46 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel45 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel44 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel43 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel42 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel41 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel38 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel35 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.imgTuberculosis1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrLabel66 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.OrganisationUID = new DevExpress.XtraReports.Parameters.Parameter();
            this.PatientVisitUID = new DevExpress.XtraReports.Parameters.Parameter();
            this.PatientUID = new DevExpress.XtraReports.Parameters.Parameter();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel77,
            this.xrLabel76,
            this.xrLabel102,
            this.xrLabel75,
            this.xrLabel74,
            this.xrLabel73,
            this.xrLabel72,
            this.xrLabel71,
            this.xrLabel70,
            this.xrLabel69,
            this.xrLabel68,
            this.imgGoodButSyphilis,
            this.imgGoodButElephantiasis,
            this.imgGoodButLeprosy,
            this.imgGoodButTuberculosis,
            this.imgNotGood,
            this.imgGoodBut,
            this.imgGoodhealth,
            this.imgPregnant3,
            this.imgPregnant2,
            this.imgPregnant1,
            this.imgNarcotic3,
            this.imgSyphilis3,
            this.imgElephantiasis3,
            this.imgLeprosy3,
            this.imgTuberculosis3,
            this.imgAlcohol2,
            this.imgNarcotic2,
            this.imgSyphilis2,
            this.imgElephantiasis2,
            this.imgAlcohol1,
            this.imgNarcotic1,
            this.imgSyphilis1,
            this.imgElephantiasis1,
            this.imgLeprosy2,
            this.imgTuberculosis2,
            this.imgLeprosy1,
            this.xrLabel81,
            this.xrLabel79,
            this.xrLabel78,
            this.xrLabel15,
            this.xrLabel8,
            this.xrLabel103,
            this.xrLabel67,
            this.xrLabel61,
            this.xrLabel62,
            this.xrLabel63,
            this.xrLabel64,
            this.xrLabel65,
            this.xrLabel54,
            this.xrLabel55,
            this.xrLabel56,
            this.xrLabel57,
            this.xrLabel58,
            this.xrLabel59,
            this.xrLabel60,
            this.xrLabel53,
            this.xrLabel52,
            this.xrLabel51,
            this.xrLabel50,
            this.xrLabel49,
            this.xrLabel48,
            this.xrLabel47,
            this.xrLabel46,
            this.xrLabel45,
            this.xrLabel44,
            this.xrLabel43,
            this.xrLabel42,
            this.xrLabel40,
            this.xrLabel41,
            this.xrLabel39,
            this.xrLabel38,
            this.xrLabel37,
            this.xrLabel36,
            this.xrLabel34,
            this.xrLabel35,
            this.xrLabel33,
            this.xrLabel32,
            this.xrLabel31,
            this.xrLabel27,
            this.xrLabel28,
            this.xrLabel29,
            this.xrLabel30,
            this.xrLabel26,
            this.xrLabel25,
            this.xrLabel24,
            this.xrLabel23,
            this.xrLabel22,
            this.xrLabel21,
            this.xrLabel20,
            this.xrLabel19,
            this.xrLabel17,
            this.xrLabel16,
            this.xrLabel14,
            this.xrLabel11,
            this.xrLabel10,
            this.xrLabel6,
            this.xrLabel13,
            this.xrLabel12,
            this.xrLabel9,
            this.xrLabel7,
            this.xrLabel5,
            this.xrLabel4,
            this.imgTuberculosis1,
            this.xrLabel18});
            this.Detail.HeightF = 932.1667F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.Detail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.MedicalCertification_BeforePrint);
            // 
            // xrLabel77
            // 
            this.xrLabel77.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Nationality]")});
            this.xrLabel77.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel77.LocationFloat = new DevExpress.Utils.PointFloat(378.2664F, 100.6419F);
            this.xrLabel77.Multiline = true;
            this.xrLabel77.Name = "xrLabel77";
            this.xrLabel77.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel77.SizeF = new System.Drawing.SizeF(372.9005F, 23.28115F);
            this.xrLabel77.StylePriority.UseFont = false;
            this.xrLabel77.StylePriority.UsePadding = false;
            this.xrLabel77.StylePriority.UseTextAlignment = false;
            this.xrLabel77.Text = "xrLabel69";
            this.xrLabel77.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel76
            // 
            this.xrLabel76.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CompanyName]")});
            this.xrLabel76.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel76.LocationFloat = new DevExpress.Utils.PointFloat(145.4167F, 197.1875F);
            this.xrLabel76.Multiline = true;
            this.xrLabel76.Name = "xrLabel76";
            this.xrLabel76.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel76.SizeF = new System.Drawing.SizeF(269.5833F, 22.89584F);
            this.xrLabel76.StylePriority.UseFont = false;
            this.xrLabel76.Text = "xrLabel76";
            // 
            // xrLabel102
            // 
            this.xrLabel102.CanGrow = false;
            this.xrLabel102.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel102.LocationFloat = new DevExpress.Utils.PointFloat(492.3333F, 291.9243F);
            this.xrLabel102.Name = "xrLabel102";
            this.xrLabel102.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel102.SizeF = new System.Drawing.SizeF(258.8336F, 26.56064F);
            this.xrLabel102.StylePriority.UseFont = false;
            this.xrLabel102.Text = "โรงพยาบาลบูรพารักษ์ ( Burapharux Hospital )";
            this.xrLabel102.WordWrap = false;
            // 
            // xrLabel75
            // 
            this.xrLabel75.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PassportID]")});
            this.xrLabel75.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel75.InteractiveSorting.TargetBand = this.Detail;
            this.xrLabel75.LocationFloat = new DevExpress.Utils.PointFloat(156.4375F, 102.0625F);
            this.xrLabel75.Multiline = true;
            this.xrLabel75.Name = "xrLabel75";
            this.xrLabel75.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel75.SizeF = new System.Drawing.SizeF(89.97917F, 23.72919F);
            this.xrLabel75.StylePriority.UseFont = false;
            // 
            // xrLabel74
            // 
            this.xrLabel74.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[BPDio]")});
            this.xrLabel74.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel74.LocationFloat = new DevExpress.Utils.PointFloat(518.6205F, 394.8333F);
            this.xrLabel74.Multiline = true;
            this.xrLabel74.Name = "xrLabel74";
            this.xrLabel74.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel74.SizeF = new System.Drawing.SizeF(29.02454F, 23F);
            this.xrLabel74.StylePriority.UseFont = false;
            this.xrLabel74.StylePriority.UseTextAlignment = false;
            this.xrLabel74.Text = "xrLabel74";
            this.xrLabel74.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel73
            // 
            this.xrLabel73.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel73.LocationFloat = new DevExpress.Utils.PointFloat(510.7991F, 394.8333F);
            this.xrLabel73.Multiline = true;
            this.xrLabel73.Name = "xrLabel73";
            this.xrLabel73.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel73.SizeF = new System.Drawing.SizeF(8.071289F, 23F);
            this.xrLabel73.StylePriority.UseFont = false;
            this.xrLabel73.StylePriority.UseTextAlignment = false;
            this.xrLabel73.Text = "/";
            this.xrLabel73.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel72
            // 
            this.xrLabel72.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[BPSys]")});
            this.xrLabel72.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel72.LocationFloat = new DevExpress.Utils.PointFloat(482.917F, 394.8333F);
            this.xrLabel72.Multiline = true;
            this.xrLabel72.Name = "xrLabel72";
            this.xrLabel72.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel72.SizeF = new System.Drawing.SizeF(27.88211F, 23F);
            this.xrLabel72.StylePriority.UseFont = false;
            this.xrLabel72.StylePriority.UseTextAlignment = false;
            this.xrLabel72.Text = "xrLabel72";
            this.xrLabel72.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel71
            // 
            this.xrLabel71.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DoctorLicenseNo]")});
            this.xrLabel71.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel71.LocationFloat = new DevExpress.Utils.PointFloat(261.8333F, 292.0833F);
            this.xrLabel71.Multiline = true;
            this.xrLabel71.Name = "xrLabel71";
            this.xrLabel71.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel71.SizeF = new System.Drawing.SizeF(116.7632F, 30.08337F);
            this.xrLabel71.StylePriority.UseFont = false;
            this.xrLabel71.Text = "xrLabel71";
            // 
            // xrLabel70
            // 
            this.xrLabel70.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Doctor]")});
            this.xrLabel70.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel70.LocationFloat = new DevExpress.Utils.PointFloat(145.4167F, 264.8797F);
            this.xrLabel70.Multiline = true;
            this.xrLabel70.Name = "xrLabel70";
            this.xrLabel70.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel70.SizeF = new System.Drawing.SizeF(349.3335F, 28.13892F);
            this.xrLabel70.StylePriority.UseFont = false;
            this.xrLabel70.Text = "xrLabel70";
            // 
            // xrLabel69
            // 
            this.xrLabel69.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DateOfBirth]")});
            this.xrLabel69.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel69.LocationFloat = new DevExpress.Utils.PointFloat(145.4167F, 124.2005F);
            this.xrLabel69.Multiline = true;
            this.xrLabel69.Name = "xrLabel69";
            this.xrLabel69.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel69.SizeF = new System.Drawing.SizeF(101F, 24.57073F);
            this.xrLabel69.StylePriority.UseFont = false;
            this.xrLabel69.StylePriority.UsePadding = false;
            this.xrLabel69.StylePriority.UseTextAlignment = false;
            this.xrLabel69.Text = "xrLabel69";
            this.xrLabel69.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrLabel69.TextFormatString = "{0:dd/MM/yyyy}";
            // 
            // xrLabel68
            // 
            this.xrLabel68.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[IDCard]")});
            this.xrLabel68.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel68.LocationFloat = new DevExpress.Utils.PointFloat(160.1459F, 77.60418F);
            this.xrLabel68.Multiline = true;
            this.xrLabel68.Name = "xrLabel68";
            this.xrLabel68.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel68.SizeF = new System.Drawing.SizeF(209.6271F, 23F);
            this.xrLabel68.StylePriority.UseFont = false;
            this.xrLabel68.Text = "xrLabel68";
            // 
            // imgGoodButSyphilis
            // 
            this.imgGoodButSyphilis.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgGoodButSyphilis.ImageSource"));
            this.imgGoodButSyphilis.LocationFloat = new DevExpress.Utils.PointFloat(680.0833F, 704.5833F);
            this.imgGoodButSyphilis.Name = "imgGoodButSyphilis";
            this.imgGoodButSyphilis.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgGoodButSyphilis.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgGoodButSyphilis.StylePriority.UseBorderColor = false;
            // 
            // imgGoodButElephantiasis
            // 
            this.imgGoodButElephantiasis.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgGoodButElephantiasis.ImageSource"));
            this.imgGoodButElephantiasis.LocationFloat = new DevExpress.Utils.PointFloat(539F, 704.5833F);
            this.imgGoodButElephantiasis.Name = "imgGoodButElephantiasis";
            this.imgGoodButElephantiasis.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgGoodButElephantiasis.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgGoodButElephantiasis.StylePriority.UseBorderColor = false;
            // 
            // imgGoodButLeprosy
            // 
            this.imgGoodButLeprosy.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgGoodButLeprosy.ImageSource"));
            this.imgGoodButLeprosy.LocationFloat = new DevExpress.Utils.PointFloat(414.9999F, 704.5833F);
            this.imgGoodButLeprosy.Name = "imgGoodButLeprosy";
            this.imgGoodButLeprosy.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgGoodButLeprosy.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgGoodButLeprosy.StylePriority.UseBorderColor = false;
            // 
            // imgGoodButTuberculosis
            // 
            this.imgGoodButTuberculosis.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgGoodButTuberculosis.ImageSource"));
            this.imgGoodButTuberculosis.LocationFloat = new DevExpress.Utils.PointFloat(258.4167F, 704.5834F);
            this.imgGoodButTuberculosis.Name = "imgGoodButTuberculosis";
            this.imgGoodButTuberculosis.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgGoodButTuberculosis.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgGoodButTuberculosis.StylePriority.UseBorderColor = false;
            // 
            // imgNotGood
            // 
            this.imgNotGood.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgNotGood.ImageSource"));
            this.imgNotGood.LocationFloat = new DevExpress.Utils.PointFloat(35.66663F, 728.5834F);
            this.imgNotGood.Name = "imgNotGood";
            this.imgNotGood.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgNotGood.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgNotGood.StylePriority.UseBorderColor = false;
            // 
            // imgGoodBut
            // 
            this.imgGoodBut.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgGoodBut.ImageSource"));
            this.imgGoodBut.LocationFloat = new DevExpress.Utils.PointFloat(35.66663F, 682.5834F);
            this.imgGoodBut.Name = "imgGoodBut";
            this.imgGoodBut.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgGoodBut.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgGoodBut.StylePriority.UseBorderColor = false;
            // 
            // imgGoodhealth
            // 
            this.imgGoodhealth.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgGoodhealth.ImageSource"));
            this.imgGoodhealth.LocationFloat = new DevExpress.Utils.PointFloat(35.66663F, 658.5834F);
            this.imgGoodhealth.Name = "imgGoodhealth";
            this.imgGoodhealth.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgGoodhealth.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgGoodhealth.StylePriority.UseBorderColor = false;
            // 
            // imgPregnant3
            // 
            this.imgPregnant3.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgPregnant3.ImageSource"));
            this.imgPregnant3.LocationFloat = new DevExpress.Utils.PointFloat(745.0834F, 589.7499F);
            this.imgPregnant3.Name = "imgPregnant3";
            this.imgPregnant3.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgPregnant3.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgPregnant3.StylePriority.UseBorderColor = false;
            // 
            // imgPregnant2
            // 
            this.imgPregnant2.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgPregnant2.ImageSource"));
            this.imgPregnant2.LocationFloat = new DevExpress.Utils.PointFloat(494.7501F, 589.7499F);
            this.imgPregnant2.Name = "imgPregnant2";
            this.imgPregnant2.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgPregnant2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgPregnant2.StylePriority.UseBorderColor = false;
            // 
            // imgPregnant1
            // 
            this.imgPregnant1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgPregnant1.ImageSource"));
            this.imgPregnant1.LocationFloat = new DevExpress.Utils.PointFloat(313.5F, 589.7499F);
            this.imgPregnant1.Name = "imgPregnant1";
            this.imgPregnant1.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgPregnant1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgPregnant1.StylePriority.UseBorderColor = false;
            // 
            // imgNarcotic3
            // 
            this.imgNarcotic3.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgNarcotic3.ImageSource"));
            this.imgNarcotic3.LocationFloat = new DevExpress.Utils.PointFloat(745.0834F, 540.75F);
            this.imgNarcotic3.Name = "imgNarcotic3";
            this.imgNarcotic3.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgNarcotic3.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgNarcotic3.StylePriority.UseBorderColor = false;
            // 
            // imgSyphilis3
            // 
            this.imgSyphilis3.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgSyphilis3.ImageSource"));
            this.imgSyphilis3.LocationFloat = new DevExpress.Utils.PointFloat(745.0834F, 517.75F);
            this.imgSyphilis3.Name = "imgSyphilis3";
            this.imgSyphilis3.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgSyphilis3.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgSyphilis3.StylePriority.UseBorderColor = false;
            // 
            // imgElephantiasis3
            // 
            this.imgElephantiasis3.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgElephantiasis3.ImageSource"));
            this.imgElephantiasis3.LocationFloat = new DevExpress.Utils.PointFloat(745.0834F, 494.75F);
            this.imgElephantiasis3.Name = "imgElephantiasis3";
            this.imgElephantiasis3.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgElephantiasis3.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgElephantiasis3.StylePriority.UseBorderColor = false;
            // 
            // imgLeprosy3
            // 
            this.imgLeprosy3.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgLeprosy3.ImageSource"));
            this.imgLeprosy3.LocationFloat = new DevExpress.Utils.PointFloat(745.0834F, 471.75F);
            this.imgLeprosy3.Name = "imgLeprosy3";
            this.imgLeprosy3.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgLeprosy3.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgLeprosy3.StylePriority.UseBorderColor = false;
            // 
            // imgTuberculosis3
            // 
            this.imgTuberculosis3.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgTuberculosis3.ImageSource"));
            this.imgTuberculosis3.LocationFloat = new DevExpress.Utils.PointFloat(745.0834F, 449.75F);
            this.imgTuberculosis3.Name = "imgTuberculosis3";
            this.imgTuberculosis3.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgTuberculosis3.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgTuberculosis3.StylePriority.UseBorderColor = false;
            // 
            // imgAlcohol2
            // 
            this.imgAlcohol2.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgAlcohol2.ImageSource"));
            this.imgAlcohol2.LocationFloat = new DevExpress.Utils.PointFloat(494.7501F, 563.75F);
            this.imgAlcohol2.Name = "imgAlcohol2";
            this.imgAlcohol2.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgAlcohol2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgAlcohol2.StylePriority.UseBorderColor = false;
            // 
            // imgNarcotic2
            // 
            this.imgNarcotic2.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgNarcotic2.ImageSource"));
            this.imgNarcotic2.LocationFloat = new DevExpress.Utils.PointFloat(494.7501F, 540.75F);
            this.imgNarcotic2.Name = "imgNarcotic2";
            this.imgNarcotic2.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgNarcotic2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgNarcotic2.StylePriority.UseBorderColor = false;
            // 
            // imgSyphilis2
            // 
            this.imgSyphilis2.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgSyphilis2.ImageSource"));
            this.imgSyphilis2.LocationFloat = new DevExpress.Utils.PointFloat(494.7501F, 517.7501F);
            this.imgSyphilis2.Name = "imgSyphilis2";
            this.imgSyphilis2.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgSyphilis2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgSyphilis2.StylePriority.UseBorderColor = false;
            // 
            // imgElephantiasis2
            // 
            this.imgElephantiasis2.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgElephantiasis2.ImageSource"));
            this.imgElephantiasis2.LocationFloat = new DevExpress.Utils.PointFloat(494.7501F, 494.75F);
            this.imgElephantiasis2.Name = "imgElephantiasis2";
            this.imgElephantiasis2.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgElephantiasis2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgElephantiasis2.StylePriority.UseBorderColor = false;
            // 
            // imgAlcohol1
            // 
            this.imgAlcohol1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgAlcohol1.ImageSource"));
            this.imgAlcohol1.LocationFloat = new DevExpress.Utils.PointFloat(313.5F, 564.7499F);
            this.imgAlcohol1.Name = "imgAlcohol1";
            this.imgAlcohol1.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgAlcohol1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgAlcohol1.StylePriority.UseBorderColor = false;
            // 
            // imgNarcotic1
            // 
            this.imgNarcotic1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgNarcotic1.ImageSource"));
            this.imgNarcotic1.LocationFloat = new DevExpress.Utils.PointFloat(313.5F, 541.75F);
            this.imgNarcotic1.Name = "imgNarcotic1";
            this.imgNarcotic1.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgNarcotic1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgNarcotic1.StylePriority.UseBorderColor = false;
            // 
            // imgSyphilis1
            // 
            this.imgSyphilis1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgSyphilis1.ImageSource"));
            this.imgSyphilis1.LocationFloat = new DevExpress.Utils.PointFloat(313.5F, 518.75F);
            this.imgSyphilis1.Name = "imgSyphilis1";
            this.imgSyphilis1.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgSyphilis1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgSyphilis1.StylePriority.UseBorderColor = false;
            // 
            // imgElephantiasis1
            // 
            this.imgElephantiasis1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgElephantiasis1.ImageSource"));
            this.imgElephantiasis1.LocationFloat = new DevExpress.Utils.PointFloat(313.5F, 495.75F);
            this.imgElephantiasis1.Name = "imgElephantiasis1";
            this.imgElephantiasis1.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgElephantiasis1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgElephantiasis1.StylePriority.UseBorderColor = false;
            // 
            // imgLeprosy2
            // 
            this.imgLeprosy2.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgLeprosy2.ImageSource"));
            this.imgLeprosy2.LocationFloat = new DevExpress.Utils.PointFloat(494.7501F, 471.75F);
            this.imgLeprosy2.Name = "imgLeprosy2";
            this.imgLeprosy2.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgLeprosy2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgLeprosy2.StylePriority.UseBorderColor = false;
            // 
            // imgTuberculosis2
            // 
            this.imgTuberculosis2.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgTuberculosis2.ImageSource"));
            this.imgTuberculosis2.LocationFloat = new DevExpress.Utils.PointFloat(494.7501F, 449.75F);
            this.imgTuberculosis2.Name = "imgTuberculosis2";
            this.imgTuberculosis2.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgTuberculosis2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgTuberculosis2.StylePriority.UseBorderColor = false;
            // 
            // imgLeprosy1
            // 
            this.imgLeprosy1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgLeprosy1.ImageSource"));
            this.imgLeprosy1.LocationFloat = new DevExpress.Utils.PointFloat(313.5F, 472.75F);
            this.imgLeprosy1.Name = "imgLeprosy1";
            this.imgLeprosy1.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgLeprosy1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgLeprosy1.StylePriority.UseBorderColor = false;
            // 
            // xrLabel81
            // 
            this.xrLabel81.CanGrow = false;
            this.xrLabel81.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Pulse]")});
            this.xrLabel81.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel81.LocationFloat = new DevExpress.Utils.PointFloat(642.3956F, 397.3333F);
            this.xrLabel81.Name = "xrLabel81";
            this.xrLabel81.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel81.SizeF = new System.Drawing.SizeF(37.68768F, 20.5F);
            this.xrLabel81.StylePriority.UseFont = false;
            this.xrLabel81.StylePriority.UseTextAlignment = false;
            this.xrLabel81.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrLabel81.WordWrap = false;
            // 
            // xrLabel79
            // 
            this.xrLabel79.CanGrow = false;
            this.xrLabel79.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SkinColor]")});
            this.xrLabel79.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel79.LocationFloat = new DevExpress.Utils.PointFloat(307.0833F, 397.3333F);
            this.xrLabel79.Name = "xrLabel79";
            this.xrLabel79.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel79.SizeF = new System.Drawing.SizeF(84.33331F, 20.5F);
            this.xrLabel79.StylePriority.UseFont = false;
            this.xrLabel79.StylePriority.UseTextAlignment = false;
            this.xrLabel79.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrLabel79.WordWrap = false;
            // 
            // xrLabel78
            // 
            this.xrLabel78.CanGrow = false;
            this.xrLabel78.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Weight]")});
            this.xrLabel78.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel78.LocationFloat = new DevExpress.Utils.PointFloat(195.8089F, 397.3333F);
            this.xrLabel78.Name = "xrLabel78";
            this.xrLabel78.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel78.SizeF = new System.Drawing.SizeF(50.60783F, 20.5F);
            this.xrLabel78.StylePriority.UseFont = false;
            this.xrLabel78.StylePriority.UseTextAlignment = false;
            this.xrLabel78.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrLabel78.WordWrap = false;
            // 
            // xrLabel15
            // 
            this.xrLabel15.CanGrow = false;
            this.xrLabel15.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Height]")});
            this.xrLabel15.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(60.16668F, 397.3333F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(55.99999F, 20.5F);
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrLabel15.WordWrap = false;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(264.7499F, 589.7499F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(48.75006F, 23F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "ไม่ตรวจ";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel103
            // 
            this.xrLabel103.CanGrow = false;
            this.xrLabel103.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel103.LocationFloat = new DevExpress.Utils.PointFloat(76.41664F, 317.9167F);
            this.xrLabel103.Name = "xrLabel103";
            this.xrLabel103.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel103.SizeF = new System.Drawing.SizeF(632.1667F, 20.08334F);
            this.xrLabel103.StylePriority.UseFont = false;
            this.xrLabel103.Text = "99/99 หมู่ 2 ต.หนองบัว อ.บ้านค่าย จ.ระยอง 21120 โทรศัพท์ 038-032432";
            this.xrLabel103.WordWrap = false;
            // 
            // xrLabel67
            // 
            this.xrLabel67.CanGrow = false;
            this.xrLabel67.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PatientName]")});
            this.xrLabel67.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel67.LocationFloat = new DevExpress.Utils.PointFloat(112.6667F, 28.99999F);
            this.xrLabel67.Name = "xrLabel67";
            this.xrLabel67.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel67.SizeF = new System.Drawing.SizeF(495.0001F, 25F);
            this.xrLabel67.StylePriority.UseFont = false;
            this.xrLabel67.WordWrap = false;
            // 
            // xrLabel61
            // 
            this.xrLabel61.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel61.LocationFloat = new DevExpress.Utils.PointFloat(570.0001F, 541.75F);
            this.xrLabel61.Name = "xrLabel61";
            this.xrLabel61.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel61.SizeF = new System.Drawing.SizeF(175.0833F, 23F);
            this.xrLabel61.StylePriority.UseFont = false;
            this.xrLabel61.StylePriority.UseTextAlignment = false;
            this.xrLabel61.Text = "ให้ตรวจยืนยัน";
            this.xrLabel61.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel62
            // 
            this.xrLabel62.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel62.LocationFloat = new DevExpress.Utils.PointFloat(570.0001F, 518.75F);
            this.xrLabel62.Name = "xrLabel62";
            this.xrLabel62.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel62.SizeF = new System.Drawing.SizeF(175.0833F, 23F);
            this.xrLabel62.StylePriority.UseFont = false;
            this.xrLabel62.StylePriority.UseTextAlignment = false;
            this.xrLabel62.Text = "ระยะที่ 3";
            this.xrLabel62.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel63
            // 
            this.xrLabel63.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel63.LocationFloat = new DevExpress.Utils.PointFloat(570.0001F, 495.75F);
            this.xrLabel63.Name = "xrLabel63";
            this.xrLabel63.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel63.SizeF = new System.Drawing.SizeF(175.0833F, 22.99997F);
            this.xrLabel63.StylePriority.UseFont = false;
            this.xrLabel63.StylePriority.UseTextAlignment = false;
            this.xrLabel63.Text = "อาการเป็นที่รังเกียจ";
            this.xrLabel63.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel64
            // 
            this.xrLabel64.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel64.LocationFloat = new DevExpress.Utils.PointFloat(570.0001F, 472.75F);
            this.xrLabel64.Name = "xrLabel64";
            this.xrLabel64.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel64.SizeF = new System.Drawing.SizeF(175.0833F, 23F);
            this.xrLabel64.StylePriority.UseFont = false;
            this.xrLabel64.StylePriority.UseTextAlignment = false;
            this.xrLabel64.Text = "ระยะติดต่อ/อาการเป็นที่รังเกียจ";
            this.xrLabel64.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel65
            // 
            this.xrLabel65.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel65.LocationFloat = new DevExpress.Utils.PointFloat(570.0001F, 449.75F);
            this.xrLabel65.Name = "xrLabel65";
            this.xrLabel65.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel65.SizeF = new System.Drawing.SizeF(175.0833F, 23F);
            this.xrLabel65.StylePriority.UseFont = false;
            this.xrLabel65.StylePriority.UseTextAlignment = false;
            this.xrLabel65.Text = "ระยะอันตราย";
            this.xrLabel65.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel54
            // 
            this.xrLabel54.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel54.LocationFloat = new DevExpress.Utils.PointFloat(401.6667F, 448.75F);
            this.xrLabel54.Name = "xrLabel54";
            this.xrLabel54.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel54.SizeF = new System.Drawing.SizeF(93.0834F, 22.99997F);
            this.xrLabel54.StylePriority.UseFont = false;
            this.xrLabel54.StylePriority.UseTextAlignment = false;
            this.xrLabel54.Text = "ผิดปกติ/ให้รักษา";
            this.xrLabel54.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel55
            // 
            this.xrLabel55.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel55.LocationFloat = new DevExpress.Utils.PointFloat(401.6667F, 471.75F);
            this.xrLabel55.Name = "xrLabel55";
            this.xrLabel55.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel55.SizeF = new System.Drawing.SizeF(93.08344F, 23.00003F);
            this.xrLabel55.StylePriority.UseFont = false;
            this.xrLabel55.StylePriority.UseTextAlignment = false;
            this.xrLabel55.Text = "ผิดปกติ/ให้รักษา";
            this.xrLabel55.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel56
            // 
            this.xrLabel56.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel56.LocationFloat = new DevExpress.Utils.PointFloat(401.6667F, 494.75F);
            this.xrLabel56.Name = "xrLabel56";
            this.xrLabel56.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel56.SizeF = new System.Drawing.SizeF(93.08347F, 23F);
            this.xrLabel56.StylePriority.UseFont = false;
            this.xrLabel56.StylePriority.UseTextAlignment = false;
            this.xrLabel56.Text = "ผิดปกติ/ให้รักษา";
            this.xrLabel56.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel57
            // 
            this.xrLabel57.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel57.LocationFloat = new DevExpress.Utils.PointFloat(401.6667F, 517.75F);
            this.xrLabel57.Name = "xrLabel57";
            this.xrLabel57.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel57.SizeF = new System.Drawing.SizeF(93.0834F, 23F);
            this.xrLabel57.StylePriority.UseFont = false;
            this.xrLabel57.StylePriority.UseTextAlignment = false;
            this.xrLabel57.Text = "ผิดปกติ/ให้รักษา";
            this.xrLabel57.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel58
            // 
            this.xrLabel58.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel58.LocationFloat = new DevExpress.Utils.PointFloat(401.6667F, 540.75F);
            this.xrLabel58.Name = "xrLabel58";
            this.xrLabel58.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel58.SizeF = new System.Drawing.SizeF(93.08344F, 23F);
            this.xrLabel58.StylePriority.UseFont = false;
            this.xrLabel58.StylePriority.UseTextAlignment = false;
            this.xrLabel58.Text = "ผิดปกติ/ให้รักษา";
            this.xrLabel58.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel59
            // 
            this.xrLabel59.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel59.LocationFloat = new DevExpress.Utils.PointFloat(401.6667F, 563.75F);
            this.xrLabel59.Name = "xrLabel59";
            this.xrLabel59.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel59.SizeF = new System.Drawing.SizeF(93.0834F, 23F);
            this.xrLabel59.StylePriority.UseFont = false;
            this.xrLabel59.StylePriority.UseTextAlignment = false;
            this.xrLabel59.Text = "ปรากฏอาการ";
            this.xrLabel59.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel60
            // 
            this.xrLabel60.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel60.LocationFloat = new DevExpress.Utils.PointFloat(684.0833F, 588.7499F);
            this.xrLabel60.Name = "xrLabel60";
            this.xrLabel60.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel60.SizeF = new System.Drawing.SizeF(61.00012F, 23F);
            this.xrLabel60.StylePriority.UseFont = false;
            this.xrLabel60.StylePriority.UseTextAlignment = false;
            this.xrLabel60.Text = "ตั้งครรภ์";
            this.xrLabel60.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel53
            // 
            this.xrLabel53.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel53.LocationFloat = new DevExpress.Utils.PointFloat(431.5001F, 589.7499F);
            this.xrLabel53.Name = "xrLabel53";
            this.xrLabel53.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel53.SizeF = new System.Drawing.SizeF(63.25002F, 23F);
            this.xrLabel53.StylePriority.UseFont = false;
            this.xrLabel53.StylePriority.UseTextAlignment = false;
            this.xrLabel53.Text = "ไม่ตั้งครรภ์";
            this.xrLabel53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel52
            // 
            this.xrLabel52.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel52.LocationFloat = new DevExpress.Utils.PointFloat(280.4166F, 564.75F);
            this.xrLabel52.Name = "xrLabel52";
            this.xrLabel52.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel52.SizeF = new System.Drawing.SizeF(33.08337F, 23F);
            this.xrLabel52.StylePriority.UseFont = false;
            this.xrLabel52.StylePriority.UseTextAlignment = false;
            this.xrLabel52.Text = "ปกติ";
            this.xrLabel52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel51
            // 
            this.xrLabel51.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel51.LocationFloat = new DevExpress.Utils.PointFloat(280.4166F, 541.75F);
            this.xrLabel51.Name = "xrLabel51";
            this.xrLabel51.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel51.SizeF = new System.Drawing.SizeF(33.08337F, 23F);
            this.xrLabel51.StylePriority.UseFont = false;
            this.xrLabel51.StylePriority.UseTextAlignment = false;
            this.xrLabel51.Text = "ปกติ";
            this.xrLabel51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel50
            // 
            this.xrLabel50.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel50.LocationFloat = new DevExpress.Utils.PointFloat(280.4166F, 518.75F);
            this.xrLabel50.Name = "xrLabel50";
            this.xrLabel50.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel50.SizeF = new System.Drawing.SizeF(33.08337F, 23F);
            this.xrLabel50.StylePriority.UseFont = false;
            this.xrLabel50.StylePriority.UseTextAlignment = false;
            this.xrLabel50.Text = "ปกติ";
            this.xrLabel50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel49
            // 
            this.xrLabel49.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel49.LocationFloat = new DevExpress.Utils.PointFloat(281.4166F, 495.75F);
            this.xrLabel49.Name = "xrLabel49";
            this.xrLabel49.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel49.SizeF = new System.Drawing.SizeF(32.0834F, 22.99997F);
            this.xrLabel49.StylePriority.UseFont = false;
            this.xrLabel49.StylePriority.UseTextAlignment = false;
            this.xrLabel49.Text = "ปกติ";
            this.xrLabel49.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel48
            // 
            this.xrLabel48.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel48.LocationFloat = new DevExpress.Utils.PointFloat(280.4166F, 472.75F);
            this.xrLabel48.Name = "xrLabel48";
            this.xrLabel48.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel48.SizeF = new System.Drawing.SizeF(33.08337F, 23F);
            this.xrLabel48.StylePriority.UseFont = false;
            this.xrLabel48.StylePriority.UseTextAlignment = false;
            this.xrLabel48.Text = "ปกติ";
            this.xrLabel48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel47
            // 
            this.xrLabel47.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel47.LocationFloat = new DevExpress.Utils.PointFloat(280.4166F, 449.75F);
            this.xrLabel47.Name = "xrLabel47";
            this.xrLabel47.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel47.SizeF = new System.Drawing.SizeF(33.08337F, 23F);
            this.xrLabel47.StylePriority.UseFont = false;
            this.xrLabel47.StylePriority.UseTextAlignment = false;
            this.xrLabel47.Text = "ปกติ";
            this.xrLabel47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel46
            // 
            this.xrLabel46.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel46.LocationFloat = new DevExpress.Utils.PointFloat(172.2499F, 856.5001F);
            this.xrLabel46.Name = "xrLabel46";
            this.xrLabel46.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel46.SizeF = new System.Drawing.SizeF(598.8333F, 22.99994F);
            this.xrLabel46.StylePriority.UseFont = false;
            this.xrLabel46.StylePriority.UseTextAlignment = false;
            this.xrLabel46.Text = "(หมายเหตุ ใบรับรองแพทย์ฉบับนี้มีอายุ 60 วันนับวันที่ตรวจร่างกาย ยกเว้น กรณีใช้สำห" +
    "รับประกันสุขภาพมีอายุ 1 ปี)";
            this.xrLabel46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel45
            // 
            this.xrLabel45.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel45.LocationFloat = new DevExpress.Utils.PointFloat(259.4166F, 833.5001F);
            this.xrLabel45.Name = "xrLabel45";
            this.xrLabel45.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel45.SizeF = new System.Drawing.SizeF(444.1667F, 23F);
            this.xrLabel45.StylePriority.UseFont = false;
            this.xrLabel45.StylePriority.UseTextAlignment = false;
            this.xrLabel45.Text = "(................................................................................" +
    "..............................)ให้ประทับตรา";
            this.xrLabel45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel44
            // 
            this.xrLabel44.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel44.LocationFloat = new DevExpress.Utils.PointFloat(443.3334F, 807.0833F);
            this.xrLabel44.Name = "xrLabel44";
            this.xrLabel44.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel44.SizeF = new System.Drawing.SizeF(74.99991F, 22.99994F);
            this.xrLabel44.StylePriority.UseFont = false;
            this.xrLabel44.StylePriority.UseTextAlignment = false;
            this.xrLabel44.Text = "แพทย์ผู้ตรวจ";
            this.xrLabel44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel43
            // 
            this.xrLabel43.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel43.LocationFloat = new DevExpress.Utils.PointFloat(60.16668F, 776.5834F);
            this.xrLabel43.Name = "xrLabel43";
            this.xrLabel43.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel43.SizeF = new System.Drawing.SizeF(643.4167F, 22.99994F);
            this.xrLabel43.StylePriority.UseFont = false;
            this.xrLabel43.StylePriority.UseTextAlignment = false;
            this.xrLabel43.Text = "3.2 เป็นโรคไม่อนุญาตให้ทำงาน และไม่ให้ประกันสุขภาพ (ตามประกาศกระทรวงสาธารณสุข)";
            this.xrLabel43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel42
            // 
            this.xrLabel42.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel42.LocationFloat = new DevExpress.Utils.PointFloat(60.16668F, 753.5834F);
            this.xrLabel42.Name = "xrLabel42";
            this.xrLabel42.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel42.SizeF = new System.Drawing.SizeF(643.4167F, 23F);
            this.xrLabel42.StylePriority.UseFont = false;
            this.xrLabel42.StylePriority.UseTextAlignment = false;
            this.xrLabel42.Text = "3.1 ร่างกายทุพพลภาพจนไม่สามารถประกอบการหาเลี้ยงชีพได้ / จิตฟั่นเฟือนไม่สมประกอบ";
            this.xrLabel42.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel40
            // 
            this.xrLabel40.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(60.1667F, 728.5834F);
            this.xrLabel40.Name = "xrLabel40";
            this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel40.SizeF = new System.Drawing.SizeF(181.5F, 23F);
            this.xrLabel40.StylePriority.UseFont = false;
            this.xrLabel40.StylePriority.UseTextAlignment = false;
            this.xrLabel40.Text = "ไม่ผ่านการตรวจสุขภาพเนื่องจาก";
            this.xrLabel40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel41
            // 
            this.xrLabel41.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel41.LocationFloat = new DevExpress.Utils.PointFloat(9.999949F, 728.5834F);
            this.xrLabel41.Name = "xrLabel41";
            this.xrLabel41.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel41.SizeF = new System.Drawing.SizeF(21.66667F, 23F);
            this.xrLabel41.StylePriority.UseFont = false;
            this.xrLabel41.StylePriority.UseTextAlignment = false;
            this.xrLabel41.Text = "3.) ";
            this.xrLabel41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel39
            // 
            this.xrLabel39.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(612.5833F, 704.5834F);
            this.xrLabel39.Name = "xrLabel39";
            this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel39.SizeF = new System.Drawing.SizeF(67.5F, 23F);
            this.xrLabel39.StylePriority.UseFont = false;
            this.xrLabel39.StylePriority.UseTextAlignment = false;
            this.xrLabel39.Text = "โรคซิฟิลิส";
            this.xrLabel39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel38
            // 
            this.xrLabel38.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel38.LocationFloat = new DevExpress.Utils.PointFloat(475.6667F, 704.5834F);
            this.xrLabel38.Name = "xrLabel38";
            this.xrLabel38.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel38.SizeF = new System.Drawing.SizeF(63.33334F, 23F);
            this.xrLabel38.StylePriority.UseFont = false;
            this.xrLabel38.StylePriority.UseTextAlignment = false;
            this.xrLabel38.Text = "โรคเท้าช้าง";
            this.xrLabel38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel37
            // 
            this.xrLabel37.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(353.3333F, 704.5834F);
            this.xrLabel37.Name = "xrLabel37";
            this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel37.SizeF = new System.Drawing.SizeF(61.66666F, 23F);
            this.xrLabel37.StylePriority.UseFont = false;
            this.xrLabel37.StylePriority.UseTextAlignment = false;
            this.xrLabel37.Text = "โรคเรื้อน";
            this.xrLabel37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel36
            // 
            this.xrLabel36.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(211.75F, 704.5834F);
            this.xrLabel36.Name = "xrLabel36";
            this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel36.SizeF = new System.Drawing.SizeF(46.66667F, 23F);
            this.xrLabel36.StylePriority.UseFont = false;
            this.xrLabel36.StylePriority.UseTextAlignment = false;
            this.xrLabel36.Text = "วัณโรค";
            this.xrLabel36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel34
            // 
            this.xrLabel34.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(9.999949F, 681.5834F);
            this.xrLabel34.Name = "xrLabel34";
            this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel34.SizeF = new System.Drawing.SizeF(21.66667F, 23F);
            this.xrLabel34.StylePriority.UseFont = false;
            this.xrLabel34.StylePriority.UseTextAlignment = false;
            this.xrLabel34.Text = "2.) ";
            this.xrLabel34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel35
            // 
            this.xrLabel35.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(60.16668F, 681.5834F);
            this.xrLabel35.Name = "xrLabel35";
            this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel35.SizeF = new System.Drawing.SizeF(341.5F, 23F);
            this.xrLabel35.StylePriority.UseFont = false;
            this.xrLabel35.StylePriority.UseTextAlignment = false;
            this.xrLabel35.Text = "ผ่านการตรวจสุขภาพ แต่ต้องให้การรักษา ควบคุม ติดตามต่อเนื่อง";
            this.xrLabel35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel33
            // 
            this.xrLabel33.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(60.16668F, 658.5834F);
            this.xrLabel33.Name = "xrLabel33";
            this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel33.SizeF = new System.Drawing.SizeF(137.5F, 23F);
            this.xrLabel33.StylePriority.UseFont = false;
            this.xrLabel33.StylePriority.UseTextAlignment = false;
            this.xrLabel33.Text = "สุขภาพสมบูรณ์ดี";
            this.xrLabel33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel32
            // 
            this.xrLabel32.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(10F, 658.5834F);
            this.xrLabel32.Name = "xrLabel32";
            this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel32.SizeF = new System.Drawing.SizeF(21.66667F, 23F);
            this.xrLabel32.StylePriority.UseFont = false;
            this.xrLabel32.StylePriority.UseTextAlignment = false;
            this.xrLabel32.Text = "1.) ";
            this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel31
            // 
            this.xrLabel31.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(335F, 640.3334F);
            this.xrLabel31.Name = "xrLabel31";
            this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel31.SizeF = new System.Drawing.SizeF(136.6667F, 23F);
            this.xrLabel31.StylePriority.UseFont = false;
            this.xrLabel31.StylePriority.UseTextAlignment = false;
            this.xrLabel31.Text = "สรุปผลการตรวจ";
            this.xrLabel31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel27
            // 
            this.xrLabel27.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(10F, 541.75F);
            this.xrLabel27.Name = "xrLabel27";
            this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel27.SizeF = new System.Drawing.SizeF(137.5F, 23F);
            this.xrLabel27.StylePriority.UseFont = false;
            this.xrLabel27.StylePriority.UseTextAlignment = false;
            this.xrLabel27.Text = "ผลการตรวจสารเสพติด";
            this.xrLabel27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel28
            // 
            this.xrLabel28.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(10F, 564.7499F);
            this.xrLabel28.Name = "xrLabel28";
            this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel28.SizeF = new System.Drawing.SizeF(219.1667F, 23.00006F);
            this.xrLabel28.StylePriority.UseFont = false;
            this.xrLabel28.StylePriority.UseTextAlignment = false;
            this.xrLabel28.Text = "ผลการตรวจอาการของโรคพิษสุราเรื้อรัง";
            this.xrLabel28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel29
            // 
            this.xrLabel29.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(9.999949F, 587.7501F);
            this.xrLabel29.Name = "xrLabel29";
            this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel29.SizeF = new System.Drawing.SizeF(137.5F, 23F);
            this.xrLabel29.StylePriority.UseFont = false;
            this.xrLabel29.StylePriority.UseTextAlignment = false;
            this.xrLabel29.Text = "ผลการตรวจตั้งครรภ์";
            this.xrLabel29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel30
            // 
            this.xrLabel30.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(10F, 613.25F);
            this.xrLabel30.Name = "xrLabel30";
            this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel30.SizeF = new System.Drawing.SizeF(766.0833F, 23F);
            this.xrLabel30.StylePriority.UseFont = false;
            this.xrLabel30.StylePriority.UseTextAlignment = false;
            this.xrLabel30.Text = resources.GetString("xrLabel30.Text");
            this.xrLabel30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel26
            // 
            this.xrLabel26.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(10F, 518.75F);
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel26.SizeF = new System.Drawing.SizeF(137.5F, 23F);
            this.xrLabel26.StylePriority.UseFont = false;
            this.xrLabel26.StylePriority.UseTextAlignment = false;
            this.xrLabel26.Text = "ผลการตรวจโรคซิฟิลิส";
            this.xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel25
            // 
            this.xrLabel25.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(10F, 495.75F);
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(137.5F, 23F);
            this.xrLabel25.StylePriority.UseFont = false;
            this.xrLabel25.StylePriority.UseTextAlignment = false;
            this.xrLabel25.Text = "ผลการตรวจโรคเท้าช้าง";
            this.xrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel24
            // 
            this.xrLabel24.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(10F, 472.75F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(137.5F, 23F);
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.StylePriority.UseTextAlignment = false;
            this.xrLabel24.Text = "ผลการตรวจโรคเรื้อน";
            this.xrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel23
            // 
            this.xrLabel23.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(10F, 449.75F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(137.5F, 23F);
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.StylePriority.UseTextAlignment = false;
            this.xrLabel23.Text = "ผลการตรวจวัณโรค";
            this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel22
            // 
            this.xrLabel22.CanGrow = false;
            this.xrLabel22.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(10F, 423.3333F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(766.0833F, 23.00003F);
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.StylePriority.UseTextAlignment = false;
            this.xrLabel22.Text = resources.GetString("xrLabel22.Text");
            this.xrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel22.WordWrap = false;
            // 
            // xrLabel21
            // 
            this.xrLabel21.CanGrow = false;
            this.xrLabel21.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(10F, 400.3333F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(766.0833F, 23.00003F);
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.StylePriority.UseTextAlignment = false;
            this.xrLabel21.Text = "ส่วนสูง......................ซม. น้ำหนัก.....................กก. สีผิว..........." +
    ".................. ความดันโลหิต ........................มม.ปรอท ชีพจร..........." +
    ".....ครั้ง/นาที";
            this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel21.WordWrap = false;
            // 
            // xrLabel20
            // 
            this.xrLabel20.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(335F, 370.3333F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(136.6667F, 23F);
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.StylePriority.UseTextAlignment = false;
            this.xrLabel20.Text = "ผลการตรวจสุขภาพ";
            this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel19
            // 
            this.xrLabel19.CanGrow = false;
            this.xrLabel19.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(37.66663F, 322.0833F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(733.4166F, 25.5F);
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.StylePriority.UseTextAlignment = false;
            this.xrLabel19.Text = resources.GetString("xrLabel19.Text");
            this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel19.WordWrap = false;
            // 
            // xrLabel17
            // 
            this.xrLabel17.CanGrow = false;
            this.xrLabel17.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(37.6667F, 272.0001F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(733.4166F, 23F);
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.StylePriority.UseTextAlignment = false;
            this.xrLabel17.Text = resources.GetString("xrLabel17.Text");
            this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel17.WordWrap = false;
            // 
            // xrLabel16
            // 
            this.xrLabel16.CanGrow = false;
            this.xrLabel16.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(22.66668F, 249F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(585.0001F, 23F);
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.Text = "๓. ข้อมูลแพทย์ผู้ตรวจ";
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel16.WordWrap = false;
            // 
            // xrLabel14
            // 
            this.xrLabel14.CanGrow = false;
            this.xrLabel14.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(37.66671F, 226F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(733.4166F, 23F);
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = resources.GetString("xrLabel14.Text");
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel14.WordWrap = false;
            // 
            // xrLabel11
            // 
            this.xrLabel11.CanGrow = false;
            this.xrLabel11.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(37.66671F, 202F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(733.4166F, 23F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "ชื่อ-สกุล (นายจ้าง).............................................................." +
    "...............................สถานประกอบการ...................................." +
    "...................................";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel11.WordWrap = false;
            // 
            // xrLabel10
            // 
            this.xrLabel10.CanGrow = false;
            this.xrLabel10.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(22.66668F, 178F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(585.0001F, 23F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "๒. ข้อมูลนายจ้าง/สถานประกอบการ";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel10.WordWrap = false;
            // 
            // xrLabel6
            // 
            this.xrLabel6.CanGrow = false;
            this.xrLabel6.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(37.6667F, 155F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(733.4166F, 23F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = resources.GetString("xrLabel6.Text");
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel6.WordWrap = false;
            // 
            // xrLabel13
            // 
            this.xrLabel13.CanGrow = false;
            this.xrLabel13.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(37.66671F, 131F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(733.4166F, 23.00002F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = resources.GetString("xrLabel13.Text");
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel13.WordWrap = false;
            // 
            // xrLabel12
            // 
            this.xrLabel12.CanGrow = false;
            this.xrLabel12.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(37.66666F, 107F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(733.4166F, 22.99999F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.Text = resources.GetString("xrLabel12.Text");
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel12.WordWrap = false;
            // 
            // xrLabel9
            // 
            this.xrLabel9.CanGrow = false;
            this.xrLabel9.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(37.6667F, 83.00003F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(733.4166F, 23F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = resources.GetString("xrLabel9.Text");
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel9.WordWrap = false;
            // 
            // xrLabel7
            // 
            this.xrLabel7.CanGrow = false;
            this.xrLabel7.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(37.66664F, 59F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(733.4166F, 23.00001F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = resources.GetString("xrLabel7.Text");
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel7.WordWrap = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.CanGrow = false;
            this.xrLabel5.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(37.66666F, 35F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(733.4166F, 23F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = resources.GetString("xrLabel5.Text");
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel5.WordWrap = false;
            // 
            // xrLabel4
            // 
            this.xrLabel4.CanGrow = false;
            this.xrLabel4.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(22.66668F, 10F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(585.0001F, 23F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "๑. รายละเอียดประวัติส่วนตัวของผู้รับการตรวจสุขภาพ";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel4.WordWrap = false;
            // 
            // imgTuberculosis1
            // 
            this.imgTuberculosis1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("imgTuberculosis1.ImageSource"));
            this.imgTuberculosis1.LocationFloat = new DevExpress.Utils.PointFloat(313.5F, 449.75F);
            this.imgTuberculosis1.Name = "imgTuberculosis1";
            this.imgTuberculosis1.SizeF = new System.Drawing.SizeF(21F, 23F);
            this.imgTuberculosis1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.imgTuberculosis1.StylePriority.UseBorderColor = false;
            // 
            // xrLabel18
            // 
            this.xrLabel18.CanGrow = false;
            this.xrLabel18.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(37.6667F, 296F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(748.4167F, 23F);
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.StylePriority.UseTextAlignment = false;
            this.xrLabel18.Text = "ใบอนุญาตประกอบวิชาชีพเวชกรรมเลขที่..........................................สถานพ" +
    "ยาบาลชื่อ......................................................................." +
    "................";
            this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel18.WordWrap = false;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 10F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 10F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel66,
            this.xrLabel3,
            this.xrLabel2,
            this.xrPictureBox1,
            this.xrLabel1});
            this.PageHeader.HeightF = 109.1667F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrLabel66
            // 
            this.xrLabel66.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[strVisitData]")});
            this.xrLabel66.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel66.LocationFloat = new DevExpress.Utils.PointFloat(557.2292F, 83.04166F);
            this.xrLabel66.Multiline = true;
            this.xrLabel66.Name = "xrLabel66";
            this.xrLabel66.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel66.SizeF = new System.Drawing.SizeF(186.4167F, 23F);
            this.xrLabel66.StylePriority.UseFont = false;
            this.xrLabel66.Text = "xrLabel66";
            this.xrLabel66.TextFormatString = "{0:dd/MM/yyyy}";
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Angsana New", 14F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(492.3333F, 86.16666F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(278.7499F, 23F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "วันที่ตรวจ ...............................................................";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel2
            // 
            this.xrLabel2.CanGrow = false;
            this.xrLabel2.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(291.9444F, 58.59818F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(226.3889F, 23F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "ตรวจสุขภาพคนต่างด้าว/แรงงานต่างด้าว";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrLabel2.WordWrap = false;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.BorderWidth = 0F;
            this.xrPictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBox1.ImageSource"));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(594.738F, 0F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(207.262F, 58.59818F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.xrPictureBox1.StylePriority.UseBorderWidth = false;
            // 
            // xrLabel1
            // 
            this.xrLabel1.CanGrow = false;
            this.xrLabel1.Font = new System.Drawing.Font("Angsana New", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(353.3333F, 35.59817F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "ใบรับรองแพทย์";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrLabel1.WordWrap = false;
            // 
            // OrganisationUID
            // 
            this.OrganisationUID.Description = "OrganisationUID";
            this.OrganisationUID.Name = "OrganisationUID";
            this.OrganisationUID.Type = typeof(long);
            this.OrganisationUID.ValueInfo = "0";
            this.OrganisationUID.Visible = false;
            // 
            // PatientVisitUID
            // 
            this.PatientVisitUID.Description = "PatientVisitUID";
            this.PatientVisitUID.Name = "PatientVisitUID";
            this.PatientVisitUID.Type = typeof(long);
            this.PatientVisitUID.ValueInfo = "0";
            this.PatientVisitUID.Visible = false;
            // 
            // PatientUID
            // 
            this.PatientUID.Description = "Parameter1";
            this.PatientUID.Name = "PatientUID";
            this.PatientUID.Type = typeof(long);
            this.PatientUID.ValueInfo = "0";
            this.PatientUID.Visible = false;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(MediTech.Model.Report.MedicalCertificateModel);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // Alien
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataMember = "Detail";
            this.DataSource = this.objectDataSource1;
            this.Margins = new System.Drawing.Printing.Margins(10, 10, 10, 10);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.ParameterPanelLayoutItems.AddRange(new DevExpress.XtraReports.Parameters.ParameterPanelLayoutItem[] {
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.OrganisationUID, DevExpress.XtraReports.Parameters.Orientation.Horizontal),
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.PatientVisitUID, DevExpress.XtraReports.Parameters.Orientation.Horizontal),
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.PatientUID, DevExpress.XtraReports.Parameters.Orientation.Horizontal)});
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.OrganisationUID,
            this.PatientVisitUID,
            this.PatientUID});
            this.Version = "22.1";
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel12;
        private DevExpress.XtraReports.UI.XRLabel xrLabel13;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel xrLabel14;
        private DevExpress.XtraReports.UI.XRLabel xrLabel16;
        private DevExpress.XtraReports.UI.XRLabel xrLabel17;
        private DevExpress.XtraReports.UI.XRLabel xrLabel19;
        private DevExpress.XtraReports.UI.XRLabel xrLabel18;
        private DevExpress.XtraReports.UI.XRLabel xrLabel20;
        private DevExpress.XtraReports.UI.XRLabel xrLabel21;
        private DevExpress.XtraReports.UI.XRLabel xrLabel22;
        private DevExpress.XtraReports.UI.XRLabel xrLabel24;
        private DevExpress.XtraReports.UI.XRLabel xrLabel23;
        private DevExpress.XtraReports.UI.XRLabel xrLabel31;
        private DevExpress.XtraReports.UI.XRLabel xrLabel27;
        private DevExpress.XtraReports.UI.XRLabel xrLabel28;
        private DevExpress.XtraReports.UI.XRLabel xrLabel29;
        private DevExpress.XtraReports.UI.XRLabel xrLabel30;
        private DevExpress.XtraReports.UI.XRLabel xrLabel26;
        private DevExpress.XtraReports.UI.XRLabel xrLabel25;
        private DevExpress.XtraReports.UI.XRLabel xrLabel32;
        private DevExpress.XtraReports.UI.XRLabel xrLabel33;
        private DevExpress.XtraReports.UI.XRLabel xrLabel34;
        private DevExpress.XtraReports.UI.XRLabel xrLabel35;
        private DevExpress.XtraReports.UI.XRLabel xrLabel36;
        private DevExpress.XtraReports.UI.XRLabel xrLabel37;
        private DevExpress.XtraReports.UI.XRLabel xrLabel39;
        private DevExpress.XtraReports.UI.XRLabel xrLabel38;
        private DevExpress.XtraReports.UI.XRLabel xrLabel40;
        private DevExpress.XtraReports.UI.XRLabel xrLabel41;
        private DevExpress.XtraReports.UI.XRLabel xrLabel42;
        private DevExpress.XtraReports.UI.XRLabel xrLabel43;
        private DevExpress.XtraReports.UI.XRLabel xrLabel46;
        private DevExpress.XtraReports.UI.XRLabel xrLabel45;
        private DevExpress.XtraReports.UI.XRLabel xrLabel44;
        private DevExpress.XtraReports.UI.XRLabel xrLabel47;
        private DevExpress.XtraReports.UI.XRLabel xrLabel53;
        private DevExpress.XtraReports.UI.XRLabel xrLabel52;
        private DevExpress.XtraReports.UI.XRLabel xrLabel51;
        private DevExpress.XtraReports.UI.XRLabel xrLabel50;
        private DevExpress.XtraReports.UI.XRLabel xrLabel49;
        private DevExpress.XtraReports.UI.XRLabel xrLabel48;
        private DevExpress.XtraReports.UI.XRLabel xrLabel54;
        private DevExpress.XtraReports.UI.XRLabel xrLabel55;
        private DevExpress.XtraReports.UI.XRLabel xrLabel56;
        private DevExpress.XtraReports.UI.XRLabel xrLabel57;
        private DevExpress.XtraReports.UI.XRLabel xrLabel58;
        private DevExpress.XtraReports.UI.XRLabel xrLabel59;
        private DevExpress.XtraReports.UI.XRLabel xrLabel60;
        private DevExpress.XtraReports.UI.XRLabel xrLabel61;
        private DevExpress.XtraReports.UI.XRLabel xrLabel62;
        private DevExpress.XtraReports.UI.XRLabel xrLabel63;
        private DevExpress.XtraReports.UI.XRLabel xrLabel64;
        private DevExpress.XtraReports.UI.XRLabel xrLabel65;
        private DevExpress.XtraReports.UI.XRLabel xrLabel67;
        private DevExpress.XtraReports.UI.XRLabel xrLabel103;
        private DevExpress.XtraReports.UI.XRLabel xrLabel102;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel15;
        private DevExpress.XtraReports.UI.XRLabel xrLabel78;
        private DevExpress.XtraReports.UI.XRLabel xrLabel79;
        private DevExpress.XtraReports.UI.XRLabel xrLabel81;
        private DevExpress.XtraReports.UI.XRPictureBox imgTuberculosis1;
        private DevExpress.XtraReports.UI.XRPictureBox imgLeprosy2;
        private DevExpress.XtraReports.UI.XRPictureBox imgTuberculosis2;
        private DevExpress.XtraReports.UI.XRPictureBox imgLeprosy1;
        private DevExpress.XtraReports.UI.XRPictureBox imgAlcohol1;
        private DevExpress.XtraReports.UI.XRPictureBox imgNarcotic1;
        private DevExpress.XtraReports.UI.XRPictureBox imgSyphilis1;
        private DevExpress.XtraReports.UI.XRPictureBox imgElephantiasis1;
        private DevExpress.XtraReports.UI.XRPictureBox imgAlcohol2;
        private DevExpress.XtraReports.UI.XRPictureBox imgNarcotic2;
        private DevExpress.XtraReports.UI.XRPictureBox imgSyphilis2;
        private DevExpress.XtraReports.UI.XRPictureBox imgElephantiasis2;
        private DevExpress.XtraReports.UI.XRPictureBox imgNarcotic3;
        private DevExpress.XtraReports.UI.XRPictureBox imgSyphilis3;
        private DevExpress.XtraReports.UI.XRPictureBox imgElephantiasis3;
        private DevExpress.XtraReports.UI.XRPictureBox imgLeprosy3;
        private DevExpress.XtraReports.UI.XRPictureBox imgTuberculosis3;
        private DevExpress.XtraReports.UI.XRPictureBox imgPregnant3;
        private DevExpress.XtraReports.UI.XRPictureBox imgPregnant2;
        private DevExpress.XtraReports.UI.XRPictureBox imgPregnant1;
        private DevExpress.XtraReports.UI.XRPictureBox imgGoodBut;
        private DevExpress.XtraReports.UI.XRPictureBox imgGoodhealth;
        private DevExpress.XtraReports.UI.XRPictureBox imgNotGood;
        private DevExpress.XtraReports.UI.XRPictureBox imgGoodButSyphilis;
        private DevExpress.XtraReports.UI.XRPictureBox imgGoodButElephantiasis;
        private DevExpress.XtraReports.UI.XRPictureBox imgGoodButLeprosy;
        private DevExpress.XtraReports.UI.XRPictureBox imgGoodButTuberculosis;
        private Parameter OrganisationUID;
        private Parameter PatientVisitUID;
        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
        private XRLabel xrLabel70;
        private XRLabel xrLabel69;
        private XRLabel xrLabel68;
        private XRLabel xrLabel66;
        private Parameter PatientUID;
        private XRLabel xrLabel71;
        private XRLabel xrLabel73;
        private XRLabel xrLabel72;
        private XRLabel xrLabel74;
        public XRLabel xrLabel75;
        private XRLabel xrLabel76;
        private XRLabel xrLabel77;
    }
}
