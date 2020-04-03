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

            if (drugSticker != null && drugSticker.Count > 0)
            {
                string healthOrganisationCode = drugSticker.FirstOrDefault().OrganisationCode;
                if (healthOrganisationCode.ToUpper().Contains("BRXG"))
                {
                    Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoBRXG.png", UriKind.Absolute);
                    BitmapImage imageSource = new BitmapImage(uri);
                    using (MemoryStream outStream = new MemoryStream())
                    {
                        BitmapEncoder enc = new BmpBitmapEncoder();
                        enc.Frames.Add(BitmapFrame.Create(imageSource));
                        enc.Save(outStream);
                        this.logo.Image = System.Drawing.Image.FromStream(outStream);
                    }
                }
                else if (healthOrganisationCode.ToUpper().Contains("DRC"))
                {
                    Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoDRC.png", UriKind.Absolute);
                    BitmapImage imageSource = new BitmapImage(uri);
                    using (MemoryStream outStream = new MemoryStream())
                    {
                        BitmapEncoder enc = new BmpBitmapEncoder();
                        enc.Frames.Add(BitmapFrame.Create(imageSource));
                        enc.Save(outStream);
                        this.logo.Image = System.Drawing.Image.FromStream(outStream);
                    }
                }
            }
            else
            {
                Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/LogoBRXG.png", UriKind.Absolute);
                BitmapImage imageSource = new BitmapImage(uri);
                using (MemoryStream outStream = new MemoryStream())
                {
                    BitmapEncoder enc = new BmpBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(imageSource));
                    enc.Save(outStream);
                    this.logo.Image = System.Drawing.Image.FromStream(outStream);
                }
            }

        }

    }
}
