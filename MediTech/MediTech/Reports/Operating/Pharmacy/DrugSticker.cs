using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Model;
using System.Collections.Generic;
using MediTech.DataService;
using MediTech.Model.Report;
using System.Windows.Media.Imaging;
using System.IO;
using System.Linq;

namespace MediTech.Reports.Operating.Pharmacy
{
    public partial class DrugSticker : DevExpress.XtraReports.UI.XtraReport
    {
        public DrugSticker()
        {
            InitializeComponent();
            this.BeforePrint += DrugSticker_BeforePrint;
        }

        void DrugSticker_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long prescriptionItemUID = long.Parse(this.Parameters["PrescriptionItemUID"].Value.ToString());
            List<DrugStickerModel> drugSticker = (new PharmacyService()).PrintStrickerDrug(prescriptionItemUID);
            if (drugSticker != null)
            {
                foreach (var item in drugSticker)
                {
                    if (item.NumericValue == null || item.NumericValue == 0)
                    {
                        item.DrugLable = item.DrugLable + " " + item.Dosage + " " + item.ItemUnit;
                    }
                }
            }
            this.DataSource = drugSticker;

            var OrganisationBRXG = (new MasterDataService()).GetHealthOrganisationByUID(17);
            if (drugSticker != null && drugSticker.Count > 0)
            {
                if (!string.IsNullOrEmpty(OrganisationUID.ToString()) && OrganisationUID != 0)
                {
                    var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                    lbFooterOrganisation.Text = Organisation.Description != null ? Organisation.Description?.ToString() : "";
                    string mobile1 = Organisation.MobileNo != null ? "Tel. " + Organisation.MobileNo.ToString() : "";

                    lbAddress.Text = Organisation.Address != null ? Organisation.Address?.ToString() + " "+ mobile1 : "";

                    if (Organisation.LogoImage != null)
                    {
                        MemoryStream ms = new MemoryStream(Organisation.LogoImage);
                        logo.Image = Image.FromStream(ms);
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                        logo.Image = Image.FromStream(ms);
                    }

                }
                else
                {
                    var OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(AppUtil.Current.OwnerOrganisationUID);
                    lbFooterOrganisation.Text = OrganisationDefault.Description?.ToString();
                    string mobile2 = OrganisationDefault.MobileNo != null ? "Tel. " + OrganisationDefault.MobileNo.ToString() : "";

                    lbAddress.Text = OrganisationDefault.Address?.ToString() + " " + mobile2;
                    if (OrganisationDefault.LogoImage != null)
                    {
                        MemoryStream ms = new MemoryStream(OrganisationDefault.LogoImage);
                        logo.Image = Image.FromStream(ms);
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                        logo.Image = Image.FromStream(ms);
                    }
                }
                //string healthOrganisationCode = drugSticker.FirstOrDefault().OrganisationCode;
                //if (healthOrganisationCode.ToUpper().Contains("BRXG"))
                //{
                //    Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoBRXG.png", UriKind.Absolute);
                //    BitmapImage imageSource = new BitmapImage(uri);
                //    using (MemoryStream outStream = new MemoryStream())
                //    {
                //        BitmapEncoder enc = new BmpBitmapEncoder();
                //        enc.Frames.Add(BitmapFrame.Create(imageSource));
                //        enc.Save(outStream);
                //        this.logo.Image = System.Drawing.Image.FromStream(outStream);
                //    }
                //}
                //if (!String.IsNullOrEmpty(OrganisationUID.ToString()))
                //{
                //    var Organisation = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                //    if (Organisation != null)
                //    {
                //        lbFooterOrganisation.Text = Organisation.Description?.ToString();
                //        lbAddress.Text = Organisation.Address?.ToString();
                //    }
                //}
                //else if (healthOrganisationCode.ToUpper().Contains("DRC"))
                //{
                //    Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoDRC.png", UriKind.Absolute);
                //    BitmapImage imageSource = new BitmapImage(uri);
                //    using (MemoryStream outStream = new MemoryStream())
                //    {
                //        BitmapEncoder enc = new BmpBitmapEncoder();
                //        enc.Frames.Add(BitmapFrame.Create(imageSource));
                //        enc.Save(outStream);
                //        this.logo.Image = System.Drawing.Image.FromStream(outStream);
                //    }
                //}
            }

            if (OrganisationUID == 24)
            {
                logo.Visible = false;
            }
        }

    }
}
