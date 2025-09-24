using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using System.Linq;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows;
using MediTech.Model;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using MediTech.Model.Report;
using DevExpress.Xpf.Editors.Helpers;
using System.Globalization;

namespace MediTech.Reports.Operating.Checkup 
{
    public partial class LabResultAmphe : DevExpress.XtraReports.UI.XtraReport
    {
        List<HealthOrganisationModel> Organisations = new List<HealthOrganisationModel>();
        public bool PrintAuto = false;
        public LabResultAmphe()
        {
            InitializeComponent();
            Organisations = (new MasterDataService()).GetHealthOrganisation();
            StaticListLookUpSettings lookupSettings = new StaticListLookUpSettings();
            foreach (var item in Organisations)
            {
                lookupSettings.LookUpValues.Add(new LookUpValue(item.HealthOrganisationUID, item.Name));
            }

            this.LogoType.LookUpSettings = lookupSettings;
            this.BeforePrint += LabResultReport_BeforePrint;
        }

        private void LabResultReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());
                int logoHead = int.Parse(this.Parameters["logoHead"].Value.ToString());
                int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
                long patientVisitUID = Convert.ToInt64(this.Parameters["PatientVisitUID"].Value.ToString());
                string requestNumber = this.Parameters["RequestNumber"].Value.ToString();
                var labResults = new List<PatientLabResult>();
                var permission = new ViewModels.MediTechViewModelBase().RoleIsConfidential();
                DateTime dateFrom = DateTime.ParseExact(this.Parameters["StartDate"].Value.ToString(), "dd/MM/yyyy hh:mm:ss", new CultureInfo("en-US"));
                DateTime dateTo = DateTime.ParseExact(this.Parameters["EndDate"].Value.ToString(), "dd/MM/yyyy hh:mm:ss", new CultureInfo("en-US"));

                var lab = (new LabDataService()).GetResultLabGroupRequestNumberByVisit(patientVisitUID, dateFrom, dateTo);
                foreach (var labResult in lab)
                {
                    var datalab = labResult.RequestDetailLabs.Where(p => p.RequestItemName.Contains("Amphetamine")).FirstOrDefault();
                    if(datalab != null)
                    {
                        var data = (new ReportsService()).GetLabResultByRequestNumber(patientVisitUID, labResult.LabNumber);
                        labResults = data.Where(p => p.ResultItemName.Contains("Amphetamine")).ToList();
                    }
                }

                var OrganisationBRXG = (new MasterDataService()).GetHealthOrganisationByUID(17);

                if (logoType == 0)
                {
                    //if(logoHead == 17)
                    //{
                    //    var SelectHead = (new MasterDataService()).GetHealthOrganisationByUID(logoHead);
                    //    if (SelectHead != null)
                    //    {
                    //        lbLicenseNo.Text = SelectHead.Description?.ToString();
                    //        //lbLicenseNo.Text = SelectOrganisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + SelectOrganisation.LicenseNo.ToString() : "";
                    //        if (SelectHead.LicenseNo != null)
                    //        {
                    //            lbLicenseNo.Text = lbLicenseNo.Text + "\r\nใบอนุญาตเลขที่ " + SelectHead.LicenseNo.ToString();
                    //        }
                    //        string mobile1 = SelectHead.MobileNo != null ? "โทรศัพท์ " + SelectHead.MobileNo.ToString() : "";
                    //        string email = SelectHead.Email != null ? "e-mail:" + SelectHead.Email.ToString() : "";

                    //        xrLabel19.Text = SelectHead.Address?.ToString() + " " + mobile1;
                    //        MemoryStream ms = new MemoryStream(SelectHead.LogoImage);
                    //        logo.Image = Image.FromStream(ms);
                    //    }
                       
                    //}
                    if(logoHead == 30 || logoHead == 17)
                    {
                        var SelectHead = (new MasterDataService()).GetHealthOrganisationByUID(logoHead);
                        if (SelectHead != null)
                        {
                            lbLicenseNo.Text = SelectHead.Description?.ToString();
                            //lbLicenseNo.Text = SelectOrganisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + SelectOrganisation.LicenseNo.ToString() : "";
                            if (SelectHead.LicenseNo != null)
                            {
                                lbLicenseNo.Text = lbLicenseNo.Text + "\r\nใบอนุญาตเลขที่ " + SelectHead.LicenseNo.ToString();
                            }
                            string mobile1 = SelectHead.MobileNo != null ? "โทรศัพท์ " + SelectHead.MobileNo.ToString() : "";
                            string email = SelectHead.Email != null ? "e-mail:" + SelectHead.Email.ToString() : "";

                            xrLabel19.Text = SelectHead.Address?.ToString() + " " + mobile1;
                            MemoryStream ms = new MemoryStream(SelectHead.LogoImage);
                            logo.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        var OrganisationDefault = (new MasterDataService()).GetHealthOrganisationByUID(OrganisationUID);
                        lbLicenseNo.Text = OrganisationDefault.Description?.ToString();
                        if (OrganisationDefault.LicenseNo != null)
                        {
                            lbLicenseNo.Text = lbLicenseNo.Text + "\r\nใบอนุญาตเลขที่ " + OrganisationDefault.LicenseNo.ToString();
                        }
                        //lbLicenseNo.Text = OrganisationDefault.LicenseNo != null ? "ใบอนุญาตเลขที่ " + OrganisationDefault.LicenseNo.ToString() : "";

                        string mobile1 = OrganisationDefault.MobileNo != null ? "โทรศัพท์ " + OrganisationDefault.MobileNo.ToString() : "";
                        string email = OrganisationDefault.Email != null ? "e-mail:" + OrganisationDefault.Email.ToString() : "";

                        xrLabel19.Text = OrganisationDefault.Address?.ToString() + " " + mobile1;

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
                    
                }
                else
                {
                    var SelectOrganisation = (new MasterDataService()).GetHealthOrganisationByUID(logoType);
                    if (SelectOrganisation != null)
                    {
                        lbLicenseNo.Text = SelectOrganisation.Description?.ToString();
                        //lbLicenseNo.Text = SelectOrganisation.LicenseNo != null ? "ใบอนุญาตเลขที่ " + SelectOrganisation.LicenseNo.ToString() : "";
                        if (SelectOrganisation.LicenseNo != null)
                        {
                            lbLicenseNo.Text = lbLicenseNo.Text + "\r\nใบอนุญาตเลขที่ " + SelectOrganisation.LicenseNo.ToString();
                        }
                        string mobile1 = SelectOrganisation.MobileNo != null ? "โทรศัพท์ " + SelectOrganisation.MobileNo.ToString() : "";
                        string email = SelectOrganisation.Email != null ? "e-mail:" + SelectOrganisation.Email.ToString() : "";

                        xrLabel19.Text = SelectOrganisation.Address?.ToString() + " " + mobile1;
                    }

                    if (SelectOrganisation.LogoImage != null)
                    {
                        MemoryStream ms = new MemoryStream(SelectOrganisation.LogoImage);
                        logo.Image = Image.FromStream(ms);
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream(OrganisationBRXG.LogoImage);
                        logo.Image = Image.FromStream(ms);
                    }
                }

                if (labResults.FirstOrDefault(p => p.IsConfidential == "Y") != null)
                {
                    if (permission != true)
                    {
                        foreach (var item in labResults.ToList())
                        {
                            if (item.IsConfidential == "Y")
                            {
                                labResults.Remove(item);
                                
                            }
                        }
                    }
                }

                if (labResults != null && labResults.Count > 0)
                {
                    var labResultNoImage = labResults.Where(p => p.ResultValueType != "Image");
                    if (labResultNoImage != null && labResultNoImage.Count() > 0)
                    {
                        this.DataSource = labResultNoImage;
                    }

                    //if (PrintAuto == false)
                    //{
                    var resultImageType = labResults.Where(p => p.ResultValueType == "Image");
                    if (resultImageType != null && resultImageType.Count() > 0)
                    {
                        LabDataService labDataServ = new LabDataService();
                        foreach (var item in resultImageType)
                        {
                            var imageData = labDataServ.GetResultImageByComponentUID(item.ResultComponentUID);
                            if (imageData != null)
                            {
                                string extension = Path.GetExtension(item.ResultValue); // "pdf", etc
                                string filename = System.IO.Path.GetTempFileName() + extension; // Makes something like "C:\Temp\blah.tmp.pdf"

                                File.WriteAllBytes(filename, imageData.ImageContent);


                                var process = new System.Diagnostics.Process();
                                process.StartInfo.FileName = filename;
                                process.StartInfo.Verb = "Open";
                                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                                process.EnableRaisingEvents = true;
                                process.Exited += delegate
                                {
                                    try
                                    {
                                        System.IO.File.Delete(filename);
                                    }
                                    catch (IOException)
                                    {
                                        //file is currently locked
                                    }

                                };
                                process.Start();
                            }

                        }
                        //}
                    }


                    if (labResultNoImage == null || labResultNoImage.Count() <= 0)
                    {
                        e.Cancel = true;
                        this.ClosePreview();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
