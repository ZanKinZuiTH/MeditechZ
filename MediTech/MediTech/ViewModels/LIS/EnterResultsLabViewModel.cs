using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
using MediTech.Model;
using System.Windows;
using ShareLibrary;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using MediTech.Views;
using System.IO;
using System.Diagnostics;

namespace MediTech.ViewModels
{
    public class EnterResultsLabViewModel : MediTechViewModelBase
    {

        #region Properties

        private List<LookupReferenceValueModel> _ResultItemRanges;

        public List<LookupReferenceValueModel> ResultItemRanges
        {
            get { return _ResultItemRanges; }
            set { Set(ref _ResultItemRanges, value); }
        }

        private LookupReferenceValueModel _SelectResultItemRange;

        public LookupReferenceValueModel SelectResultItemRange
        {
            get { return _SelectResultItemRange; }
            set { Set(ref _SelectResultItemRange, value); }
        }

        private RequestLabModel _RequestLab;

        public RequestLabModel RequestLab
        {
            get { return _RequestLab; }
            set { Set(ref _RequestLab, value); }
        }

        private List<RequestDetailLabModel> _RequestDetailLabs;

        public List<RequestDetailLabModel> RequestDetailLabs
        {
            get { return _RequestDetailLabs; }
            set { Set(ref _RequestDetailLabs, value); }
        }

        private ResultComponentModel _SelectResultComponent;

        public ResultComponentModel SelectResultComponent
        {
            get { return _SelectResultComponent; }
            set { Set(ref _SelectResultComponent, value); }
        }


        #endregion

        #region Command


        private RelayCommand _GetRangeCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>
        public RelayCommand GetRangeCommand
        {
            get
            {
                return _GetRangeCommand
                    ?? (_GetRangeCommand = new RelayCommand(GetRange));
            }

        }

        private RelayCommand _SaveCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                    ?? (_SaveCommand = new RelayCommand(SaveLabResult));
            }

        }
        private RelayCommand _CloseCommand;

        /// <summary>
        /// Gets the CancelCommand.
        /// </summary>
        public RelayCommand CloseCommand
        {
            get
            {
                return _CloseCommand
                    ?? (_CloseCommand = new RelayCommand(Close));
            }

        }

        private RelayCommand _OpenLabImageCommand;

        /// <summary>
        /// Gets the OpenLabImageCommand.
        /// </summary>
        public RelayCommand OpenLabImageCommand
        {
            get
            {
                return _OpenLabImageCommand
                    ?? (_OpenLabImageCommand = new RelayCommand(OpenLabImage));
            }

        }


        private RelayCommand _ChooseFileCommand;

        /// <summary>
        /// Gets the ChooseFileCommand.
        /// </summary>
        public RelayCommand ChooseFileCommand
        {
            get
            {
                return _ChooseFileCommand
                    ?? (_ChooseFileCommand = new RelayCommand(ChooseFile));
            }

        }


        private RelayCommand _DeleteFileCommand;

        /// <summary>
        /// Gets the DeleteFileCommand.
        /// </summary>
        public RelayCommand DeleteFileCommand
        {
            get
            {
                return _DeleteFileCommand
                    ?? (_DeleteFileCommand = new RelayCommand(DeleteFile));
            }

        }



        #endregion

        #region Method

        public EnterResultsLabViewModel()
        {
            ResultItemRanges = DataService.Technical.GetReferenceValueMany("LABRAM");
            SelectResultItemRange = ResultItemRanges.FirstOrDefault();

        }

        private  void SaveLabResult()
        {
            try
            {
                List<RequestDetailLabModel> revieRequestDetails = new List<RequestDetailLabModel>();
                revieRequestDetails = RequestDetailLabs.Where(p => p.ResultComponents.Count(f => !string.IsNullOrEmpty(f.ResultValue)) > 0).ToList();
                if (revieRequestDetails != null && revieRequestDetails.Count > 0)
                {
                    foreach (var revieRequestDetail in revieRequestDetails)
                    {
                        revieRequestDetail.ResultComponents = new ObservableCollection<ResultComponentModel>
                            (RequestDetailLabs.FirstOrDefault(p => p.RequestDetailUID == revieRequestDetail.RequestDetailUID).ResultComponents.Where(p => p.RequestDetailUID == revieRequestDetail.RequestDetailUID && !string.IsNullOrEmpty(p.ResultValue)).ToList());
                    }
                }


                DataService.Lab.ReviewLabResult(revieRequestDetails, AppUtil.Current.UserID);
                SaveSuccessDialog();
                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        public void AssignModel(RequestLabModel requestLab)
        {
            this.RequestLab = requestLab;
            RequestDetailLabs = DataService.Lab.GetResultLabByRequestUID(RequestLab.RequestUID);
            if (RequestDetailLabs != null && RequestDetailLabs.Count > 0)
            {
                RequestDetailLabs = RequestDetailLabs.Where(p => p.OrderStatus != "Cancelled").OrderBy(p => p.RequestItemName).ToList();
            }
        }
        private void GetRange()
        {
            if (SelectResultItemRange != null)
            {
                var resultItemRange = DataService.Lab.GetResultItemRangeByLABRAMUID(SelectResultItemRange.Key);
                foreach (var item1 in RequestDetailLabs)
                {
                    foreach (var item2 in item1.ResultComponents)
                    {
                        var itemRange = resultItemRange.FirstOrDefault(p => p.ResultItemUID == item2.ResultItemUID && (p.SEXXXUID == 3));
                        if (itemRange != null)
                        {
                            if (item2.ResultValueType == "Numeric")
                            {
                                item2.Low = itemRange.Low;
                                item2.High = itemRange.High;
                            }
                            else if(item2.ResultValueType == "Free Text Field")
                            {
                                item2.ReferenceRange = itemRange.DisplayValue;
                            }
                        }
                        else
                        {
                            var itemRangeGender = resultItemRange.FirstOrDefault(p => p.ResultItemUID == item2.ResultItemUID && (p.SEXXXUID == RequestLab.SEXXXUID));
                            if (itemRangeGender != null)
                            {
                                if (item2.ResultValueType == "Numeric")
                                {
                                    item2.Low = itemRangeGender.Low;
                                    item2.High = itemRangeGender.High;
                                }
                                else if (item2.ResultValueType == "Free Text Field")
                                {
                                    item2.ReferenceRange = itemRangeGender.DisplayValue;
                                }
                            }

                        }

                    }
                }

            }
        }

        private void Close()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }


        private void OpenLabImage()
        {
            try
            {
                //if (SelectResultComponent.ResultComponentUID != 0  
                //    && SelectResultComponent.ResultComponentUID != null
                //    && SelectResultComponent.ImageContent == null)
                //{
                //    var imageData = DataService.Lab.GetResultImageByComponentUID(SelectResultComponent.ResultComponentUID ?? 0);
                //    if (imageData != null)
                //    {
                //        SelectResultComponent.ImageContent = imageData.ImageContent;
                //    }
                //}

                string extension = Path.GetExtension(SelectResultComponent.ResultValue); // "pdf", etc
                string filename = System.IO.Path.GetTempFileName() + extension; // Makes something like "C:\Temp\blah.tmp.pdf"

                File.WriteAllBytes(filename, SelectResultComponent.ImageContent);


                var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = filename;
                process.StartInfo.Verb = "Open";
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                process.EnableRaisingEvents = true;
                process.Exited += delegate
                {
                    System.IO.File.Delete(filename);
                };
                process.Start();

                // Clean up our temporary file...


            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }
        private void ChooseFile()
        {
            if (SelectResultComponent != null)
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.Filter = @"Image files (*.bmp, *.gif, *.jpeg, *.jpg, *.png)|*.bmp; *.gif; *.jpeg; *.jpg; *.png
|PDF (*.pdf)|*.pdf|Html (*.htm,*.html)|*.htm;*.html|Documents (*.doc,*.docx)|*.doc;*.docx";
                openDialog.ShowDialog();
                if (openDialog.FileName.Trim() != "")
                {
                    try
                    {
                        SelectResultComponent.ResultValue = openDialog.SafeFileName.Trim();
                        byte[] fileData = File.ReadAllBytes(openDialog.FileName);
                        SelectResultComponent.ImageContent = fileData;

                    }
                    catch (Exception ex)
                    {
                        ErrorDialog(ex.Message);
                    }
                }
            }
        }

        private void DeleteFile()
        {
            if (SelectResultComponent != null)
            {
                SelectResultComponent.ResultValue = string.Empty;
                SelectResultComponent.ImageContent = null;
            }
        }

        #endregion
    }
}
