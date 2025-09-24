using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Security.Permissions;
using MediTech.Model;
using System.Diagnostics;
using MediTech.Views;
using System.Collections.ObjectModel;
using System.IO;
using Dicom.Media;
using Dicom;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Windows;
using MediTech.Helpers;

namespace MediTech.ViewModels
{

    public class PACSWorkListViewModel : MediTechViewModelBase
    {

        #region Properties

        public bool IsOpenFromExam { get; set; }

        private List<StudiesModel> _StudiesList;

        public List<StudiesModel> StudiesList
        {
            get { return _StudiesList; }
            set
            {
                Set(ref _StudiesList, value);
                IsSetStudyList = true;
            }
        }

        private StudiesModel _SelectStudies;

        public StudiesModel SelectStudies
        {
            get { return _SelectStudies; }
            set
            {
                Set(ref _SelectStudies, value);
                if (_SelectStudies != null)
                {
                    try
                    {
                        SeriesSource = DataService.PACS.GetSeriesByStudyUID(_SelectStudies.StudyInstanceUID);
                        if (SeriesSource != null)
                        {
                            SelectSeries = SeriesSource.FirstOrDefault();
                        }
                    }
                    catch (Exception)
                    {

                        SeriesSource = null;
                    }

                }
            }
        }

        private List<SeriesModel> _SeriesSource;

        public List<SeriesModel> SeriesSource
        {
            get { return _SeriesSource; }
            set
            {
                Set(ref _SeriesSource, value);
            }
        }

        private SeriesModel _SelectSeries;

        public SeriesModel SelectSeries
        {
            get { return _SelectSeries; }
            set
            {
                Set(ref _SelectSeries, value);
                //if (SelectSeries != null)
                //{
                //    ImageSeries = DataService.PACS.GetImageSeries(SelectSeries.StudyInstanceUID,SelectSeries.SeriesInstanceUID);
                //}
                //else
                //{
                //    ImageSeries = null;
                //}
            }
        }

        private List<ImageSerires> _ImageSeries;

        public List<ImageSerires> ImageSeries
        {
            get { return _ImageSeries; }
            set
            {
                Set(ref _ImageSeries, value);
            }
        }

        private ObservableCollection<InstancesModel> _SelectInstances;

        public ObservableCollection<InstancesModel> SelectInstances
        {
            get { return _SelectInstances ?? (_SelectInstances = new ObservableCollection<InstancesModel>()); }
            set { Set(ref _SelectInstances, value); }
        }


        private SeriesCopyToModel _SelectCDRBuffer;

        public SeriesCopyToModel SelectCDRBuffer
        {
            get { return _SelectCDRBuffer; }
            set { Set(ref _SelectCDRBuffer, value); }
        }

        private InstancesCopyToModel _SelectInstancesCopy;

        public InstancesCopyToModel SelectInstancesCopy
        {
            get { return _SelectInstancesCopy; }
            set { Set(ref _SelectInstancesCopy, value); }
        }



        private List<LookupReferenceValueModel> _ModalityList;

        public List<LookupReferenceValueModel> ModalityList
        {
            get { return _ModalityList; }
            set { _ModalityList = value; }
        }


        private List<object> _SelectModalityList;

        public List<object> SelectModalityList
        {
            get { return _SelectModalityList ?? (_SelectModalityList = new List<object>()); }
            set { Set(ref _SelectModalityList, value); }
        }

        private bool _IsCheckedToDay;

        public bool IsCheckedToDay
        {
            get { return _IsCheckedToDay; }
            set
            {
                Set(ref _IsCheckedToDay, value);
                if (_IsCheckedToDay)
                {
                    IsEnableDateFrom = false;
                    IsEnableDateTo = false;
                    DateFrom = DateTime.Now;
                    DateTo = DateTime.Now;
                }
            }
        }

        private bool _IsCheckedYesterDay;

        public bool IsCheckedYesterDay
        {
            get { return _IsCheckedYesterDay; }
            set
            {
                Set(ref _IsCheckedYesterDay, value);

                if (_IsCheckedYesterDay)
                {
                    IsEnableDateFrom = false;
                    IsEnableDateTo = false;
                    DateFrom = DateTime.Now.AddDays(-1);
                    DateTo = DateTime.Now;
                }

            }
        }

        private bool _IsCheckedPastWeek;

        public bool IsCheckedPastWeek
        {
            get { return _IsCheckedPastWeek; }
            set
            {
                Set(ref _IsCheckedPastWeek, value);
                if (_IsCheckedPastWeek)
                {
                    IsEnableDateFrom = false;
                    IsEnableDateTo = false;
                    DateFrom = DateTime.Now.AddDays(-7);
                    DateTo = DateTime.Now;
                }
            }
        }

        private bool _IsCheckedPeriod;

        public bool IsCheckedPeriod
        {
            get { return _IsCheckedPeriod; }
            set
            {
                Set(ref _IsCheckedPeriod, value);
                if (_IsCheckedPeriod)
                {
                    IsEnableDateFrom = true;
                    IsEnableDateTo = true;
                }

            }
        }

        private bool _IsCheckedNone;

        public bool IsCheckedNone
        {
            get { return _IsCheckedNone; }
            set
            {
                Set(ref _IsCheckedNone, value);
                if (_IsCheckedNone)
                {
                    IsEnableDateFrom = false;
                    IsEnableDateTo = false;
                    DateFrom = null;
                    DateTo = null;
                }
            }
        }


        private bool _IsEnableDateFrom;

        public bool IsEnableDateFrom
        {
            get { return _IsEnableDateFrom; }
            set { Set(ref _IsEnableDateFrom, value); }
        }

        private DateTime? _DateFrom;

        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set { Set(ref _DateFrom, value); }
        }

        private bool _IsEnableDateTo;

        public bool IsEnableDateTo
        {
            get { return _IsEnableDateTo; }
            set { Set(ref _IsEnableDateTo, value); }
        }
        private DateTime? _DateTo;

        public DateTime? DateTo
        {
            get { return _DateTo; }
            set { Set(ref _DateTo, value); }
        }

        private string _Modality;

        public string Modality
        {
            get { return _Modality; }
            set
            {
                Set(ref _Modality, value);


                var defaultModality = ModalityList.FirstOrDefault(p => p.ValueCode == Modality);
                if (defaultModality != null)
                {
                    if (Modality == "MG")
                    {
                        SelectModalityList.Add(2891);
                        SelectModalityList.Add(2895);
                    }
                    else if (Modality == "DX")
                    {
                        SelectModalityList.Add(2889);
                        SelectModalityList.Add(2886);
                    }
                    else
                    {
                        SelectModalityList.Add(defaultModality.Key);
                    }

                }
                else
                {
                    SelectModalityList.Clear();
                }
                OnUpdateEvent();
            }
        }

        private string _Gender;

        public string Gender
        {
            get { return _Gender; }
            set { Set(ref _Gender, value); }
        }

        private string _PatientID;

        public string PatientID
        {
            get { return _PatientID; }
            set { Set(ref _PatientID, value); }
        }

        private string _PatientName;

        public string PatientName
        {
            get { return _PatientName; }
            set { Set(ref _PatientName, value); }
        }

        private string _AccessionNumber;

        public string AccessionNumber
        {
            get { return _AccessionNumber; }
            set { Set(ref _AccessionNumber, value); }
        }

        private string _StudyID;

        public string StudyID
        {
            get { return _StudyID; }
            set { Set(ref _StudyID, value); }
        }

        private string _StudyDescription;

        public string StudyDescription
        {
            get { return _StudyDescription; }
            set { Set(ref _StudyDescription, value); }
        }


        private string _Bodypart;

        public string Bodypart
        {
            get { return _Bodypart; }
            set { Set(ref _Bodypart, value); }
        }


        private ObservableCollection<SeriesCopyToModel> _CDRBufferList;

        public ObservableCollection<SeriesCopyToModel> CDRBufferList
        {
            get { return _CDRBufferList ?? (_CDRBufferList = new ObservableCollection<SeriesCopyToModel>()); }
            set
            {
                Set(ref _CDRBufferList, value);
            }
        }

        private bool _IsEnabledMicroDiom = true;

        public bool IsEnabledMicroDiom
        {
            get { return _IsEnabledMicroDiom; }
            set { Set(ref _IsEnabledMicroDiom, value); }
        }


        private bool _IsEnabledCDBuffer = true;

        public bool IsEnabledCDBuffer
        {
            get { return _IsEnabledCDBuffer; }
            set { Set(ref _IsEnabledCDBuffer, value); }
        }

        private Visibility _IsVisibilityProgressBar = Visibility.Collapsed;

        public Visibility IsVisibilityProgressBar
        {
            get { return _IsVisibilityProgressBar; }
            set
            {
                Set(ref _IsVisibilityProgressBar, value);
                if (_IsVisibilityProgressBar == Visibility.Collapsed)
                {
                    PACSWorkList view = (this.View as PACSWorkList);
                    view.SetProgressBarValue(0);
                }
            }
        }


        private bool _IsSetStudyList = false;

        public bool IsSetStudyList
        {
            get { return _IsSetStudyList; }
            set { _IsSetStudyList = value; }
        }


        #endregion

        #region Command

        private RelayCommand _ClearFieldCommand;

        /// <summary>
        /// Gets the ClearFieldCommand.
        /// </summary>
        public RelayCommand ClearFieldCommand
        {
            get
            {
                return _ClearFieldCommand
                    ?? (_ClearFieldCommand = new RelayCommand(ClearField));
            }
        }

        private RelayCommand _SearchCommand;

        /// <summary>
        /// Gets the SearchCommand.
        /// </summary>
        public RelayCommand SearchCommand
        {
            get
            {
                return _SearchCommand
                    ?? (_SearchCommand = new RelayCommand(Search));
            }
        }

        private RelayCommand _EditStudyCommand;

        /// <summary>
        /// Gets the EditStudyCommand.
        /// </summary>
        public RelayCommand EditStudyCommand
        {
            get
            {
                return _EditStudyCommand
                    ?? (_EditStudyCommand = new RelayCommand(EditStudyData));
            }
        }


        private RelayCommand _MicroDicomCommand;

        /// <summary>
        /// Gets the MicroDicomCommand.
        /// </summary>
        public RelayCommand MicroDicomCommand
        {
            get
            {
                return _MicroDicomCommand
                    ?? (_MicroDicomCommand = new RelayCommand(MicroDicom));
            }
        }

        private RelayCommand _OPENPACSViewCommand;

        /// <summary>
        /// Gets the OPENPACSViewCommand.
        /// </summary>
        public RelayCommand OPENPACSViewCommand
        {
            get
            {
                return _OPENPACSViewCommand
                    ?? (_OPENPACSViewCommand = new RelayCommand(OPENPACSView));
            }
        }


        private RelayCommand _CDBufferCommand;

        /// <summary>
        /// Gets the CDBufferCommand.
        /// </summary>
        public RelayCommand CDBufferCommand
        {
            get
            {
                return _CDBufferCommand
                    ?? (_CDBufferCommand = new RelayCommand(CDBuffer));
            }
        }

        private RelayCommand _DeleteCDBufferCommand;

        public RelayCommand DeleteCDBufferCommand
        {
            get
            {
                return _DeleteCDBufferCommand
                    ?? (_DeleteCDBufferCommand = new RelayCommand(DeleteCDBuffer));
            }
        }

        private RelayCommand _DeleteInstanceCommand;

        public RelayCommand DeleteInstanceCommand
        {
            get
            {
                return _DeleteInstanceCommand
                    ?? (_DeleteInstanceCommand = new RelayCommand(DeleteInstance));
            }
        }


        private RelayCommand _ViewerOnCDCommand;

        /// <summary>
        /// Gets the ViewerOnCDCommand.
        /// </summary>
        public RelayCommand ViewerOnCDCommand
        {
            get
            {
                return _ViewerOnCDCommand
                    ?? (_ViewerOnCDCommand = new RelayCommand(ViewerOnCD));
            }
        }


        private RelayCommand _ClearBufferCommand;

        /// <summary>
        /// Gets the ClearBufferCommand.
        /// </summary>
        public RelayCommand ClearBufferCommand
        {
            get
            {
                return _ClearBufferCommand
                    ?? (_ClearBufferCommand = new RelayCommand(ClearBuffer));
            }
        }



        #endregion

        #region Method

        List<InstancesCopyToModel> instances;
        public PACSWorkListViewModel()
        {
            IsCheckedToDay = true;
            var refValue = DataService.Technical.GetReferenceValueList("RIMTYP");
            ModalityList = refValue.Where(p => p.DomainCode == "RIMTYP").ToList();
            //Search();
        }

        public override void OnLoaded()
        {
            if (!IsSetStudyList)
            {
                if (!IsOpenFromExam)
                {
                    Search();
                }
                else
                {
                    OpenFromExam();
                }
            }
            else
            {
                if (StudiesList != null && StudiesList.Count == 1)
                {
                    OPENPACSViewFromStudy();
                    this.CloseViewDialog(ActionDialog.Cancel);
                }
            }

        }
        private void Search()
        {
            string modalityList = "";
            //foreach (var item in SelectModality)
            //{
            //    if (modality == "")
            //    {
            //        modality = item.ToString();
            //    }
            //    else
            //    {
            //        modality = modality + "," + item.ToString();
            //    }
            //}
            if (SelectModalityList != null)
            {
                foreach (object modality in SelectModalityList)
                {
                    if (modalityList == "")
                    {
                        modalityList = "'" + ModalityList.FirstOrDefault(p => p.Key.ToString() == modality.ToString()).ValueCode.ToString() + "'";
                    }
                    else
                    {
                        modalityList += "," + "'" + ModalityList.FirstOrDefault(p => p.Key.ToString() == modality.ToString()).ValueCode.ToString() + "'";
                    }
                }
            }
            StudiesList = DataService.PACS.SearchPACSWorkList(DateFrom, DateTo, modalityList, Gender, PatientID, PatientName, AccessionNumber, StudyID, StudyDescription, Bodypart);
            SeriesSource = null;
        }

        public void OpenFromExam()
        {
            Search();
            if (StudiesList != null && StudiesList.Count == 1)
            {
                OPENPACSViewFromStudy();
                this.CloseViewDialog(ActionDialog.Cancel);
            }
        }
        private void ClearField()
        {
            IsCheckedToDay = true;
            Modality = "";
            Gender = "";
            PatientID = "";
            PatientName = "";
            AccessionNumber = "";
            StudyID = "";
            StudyDescription = "";
            Bodypart = "";
        }

        private void OPENPACSView()
        {

            if (SelectSeries != null)
            {
                try
                {
                    string url = PACSHelper.GetPACSViewerSeriesUrl(SelectSeries.SeriesInstanceUID);
                    PACSHelper.OpenPACSViewer(url);
                }
                catch (Exception er)
                {

                    ErrorDialog(er.Message);

                }

            }
        }

        private void OPENPACSViewFromStudy()
        {
            if (StudiesList != null)
            {
                try
                {
                    string url = PACSHelper.GetPACSViewerStudyUrl(StudiesList.FirstOrDefault().StudyInstanceUID);
                    PACSHelper.OpenPACSViewer(url);
                }
                catch (Exception er)
                {

                    ErrorDialog(er.Message);
                }

            }
        }


        private async void CDBuffer()
        {
            try
            {
                //PACSWorkList view = (PACSWorkList)(this.View);
                //view.dockManager.DockController.Restore(view.pnlCDBuffer);
                //view.dockManager.DockController.Activate(view.pnlCDBuffer);
                IsEnabledCDBuffer = false;
                IsVisibilityProgressBar = Visibility.Visible;
                if (SelectStudies != null)
                {
                    PatientInformationModel patientInfo = DataService.PatientIdentity.GetPatientByHN(SelectStudies.PatientID);
                    if (SelectSeries != null)
                    {
                        if (CDRBufferList != null)
                        {
                            if (CDRBufferList.Any(p => p.PatientID == SelectStudies.PatientID && p.SeriesInstanceUID == SelectSeries.SeriesInstanceUID))
                            {
                                WarningDialog("มีรายการที่เลือกแล้ว โปรดตรวจสอบ");
                                return;
                            };

                        }

                        await GetDicomByteArray(SelectSeries.Instances);

                        if (instances != null && instances.Count > 0)
                        {
                            SeriesCopyToModel CDRBuffer = new SeriesCopyToModel();

                            if (patientInfo != null)
                            {
                                CDRBuffer.PatientName = patientInfo.FirstName + " " + patientInfo.LastName;
                                CDRBuffer.PatientID = patientInfo.PatientID;
                                CDRBuffer.OtherID = patientInfo.EmployeeID;
                            }
                            else
                            {
                                CDRBuffer.PatientName = SelectStudies.PatientName;
                                CDRBuffer.PatientID = SelectStudies.PatientID;
                            }

                            CDRBuffer.StudyID = SelectStudies.StudyID;
                            CDRBuffer.StudyDate = SelectStudies.StudyDate;
                            CDRBuffer.Modality = SelectSeries.Modality;
                            CDRBuffer.SeriesDate = SelectSeries.SeriesDate;
                            CDRBuffer.SeriesDescription = SelectSeries.SeriesDescription;
                            CDRBuffer.SeriesInstanceUID = SelectSeries.SeriesInstanceUID;
                            CDRBuffer.SeriesNumber = SelectSeries.SeriesNumber;
                            CDRBuffer.SeriesTime = SelectSeries.SeriesTime;
                            CDRBuffer.Instances = instances;
                            //CDRBuffer.DicomFiles = new List<byte[]>();
                            //foreach (var file in dicomFiles)
                            //{
                            //    CDRBuffer.DicomFiles.Add(file);
                            //}
                            CDRBuffer.No = (CDRBufferList != null && CDRBufferList.Count() > 0) ? CDRBufferList.Max(p => p.No) + 1 : 1;
                            CDRBufferList.Add(CDRBuffer);
                        }

                    }
                    else if (SelectInstances != null)
                    {
                        if (CDRBufferList != null)
                        {
                            InstancesModel selectInstance = SelectInstances.FirstOrDefault();

                            foreach (var item in SelectInstances)
                            {
                                if (CDRBufferList.Any(p => p.PatientID == item.PatientID
    && p.SeriesInstanceUID == item.SeriesInstanceUID
    && p.Instances.Any(s => s.SOPInstanceUID == item.SOPInstanceUID)))
                                {
                                    WarningDialog("มีรายการ Instance No " + item.InstanceNumber + " อยู่แล้ว โปรดตรวจสอบ");
                                    return;
                                };
                            }

                            await GetDicomByteArray(SelectInstances.ToList());

                            if (instances != null && instances.Count > 0)
                            {
                                SeriesCopyToModel cdrBufferExites = CDRBufferList.FirstOrDefault(p => p.PatientID == selectInstance.PatientID
                            && p.SeriesInstanceUID == selectInstance.SeriesInstanceUID);
                                if (cdrBufferExites != null)
                                {
                                    cdrBufferExites.Instances.AddRange(instances);
                                }
                                else
                                {
                                    SeriesCopyToModel CDRBuffer = new SeriesCopyToModel();

                                    if (patientInfo != null)
                                    {
                                        CDRBuffer.PatientName = patientInfo.FirstName + " " + patientInfo.LastName;
                                        CDRBuffer.PatientID = patientInfo.PatientID;
                                        CDRBuffer.OtherID = patientInfo.EmployeeID;
                                    }
                                    else
                                    {
                                        CDRBuffer.PatientName = SelectStudies.PatientName;
                                        CDRBuffer.PatientID = SelectStudies.PatientID;
                                    }

                                    CDRBuffer.StudyID = SelectStudies.StudyID;
                                    CDRBuffer.StudyDate = SelectStudies.StudyDate;
                                    CDRBuffer.Modality = selectInstance.Modality;
                                    CDRBuffer.SeriesDate = selectInstance.SeriesDate;
                                    CDRBuffer.SeriesDescription = selectInstance.SeriesDescription;
                                    CDRBuffer.SeriesInstanceUID = selectInstance.SeriesInstanceUID;
                                    CDRBuffer.SeriesNumber = selectInstance.SeriesNumber;
                                    CDRBuffer.SeriesTime = selectInstance.SeriesTime;
                                    CDRBuffer.Instances = instances;
                                    CDRBuffer.No = (CDRBufferList != null && CDRBufferList.Count() > 0) ? CDRBufferList.Max(p => p.No) + 1 : 1;
                                    CDRBufferList.Add(CDRBuffer);
                                }

                            }
                        }
                    }
                    PACSWorkList view = (this.View as PACSWorkList);
                    view.grdCDBuffer.RefreshData();
                }

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
            finally
            {
                IsEnabledCDBuffer = true;
                IsVisibilityProgressBar = Visibility.Collapsed;
            }

        }

        private async void MicroDicom()
        {
            try
            {
                IsEnabledMicroDiom = false;
                IsVisibilityProgressBar = Visibility.Visible;
                string pacsViwerPath = System.Configuration.ConfigurationManager.AppSettings["PACSViwer"].ToString();
                string tempPath = Path.GetTempPath();
                var dicomDir = new DicomDirectory();
                string studyPath = string.Empty;
                string seriesPath = string.Empty;

                if (SelectStudies != null)
                {
                    PatientInformationModel patientInfo = DataService.PatientIdentity.GetPatientByHN(SelectStudies.PatientID);
                    if (SelectSeries != null)
                    {
                        studyPath = tempPath + SelectStudies.StudyInstanceUID;
                        seriesPath = studyPath + "\\" + SelectSeries.SeriesInstanceUID;
                        await GetDicomByteArray(SelectSeries.Instances);
                    }
                    else if (SelectInstances != null)
                    {
                        studyPath = tempPath + SelectInstances.FirstOrDefault().StudyInstanceUID;
                        seriesPath = studyPath + "\\" + SelectInstances.FirstOrDefault().SeriesInstanceUID;
                        await GetDicomByteArray(SelectInstances.ToList());
                    }

                    if (instances != null && instances.Count > 0)
                    {

                        if (Directory.Exists(studyPath))
                        {
                            Directory.Delete(studyPath, true);
                        }

                        Directory.CreateDirectory(studyPath);
                        Directory.CreateDirectory(seriesPath);
                        foreach (var item in instances.ToList())
                        {

                            MemoryStream ms = new MemoryStream(item.DicomFiles);
                            var dicomFile = Dicom.DicomFile.Open(ms);

                            //#if !DEBUG
                            if (patientInfo != null)
                            {
                                dicomFile.Dataset.AddOrUpdate(DicomTag.SpecificCharacterSet, Encoding.UTF8, "ISO_IR 192");
                                dicomFile.Dataset.AddOrUpdate(DicomTag.PatientName, Encoding.UTF8, patientInfo.FirstName + " " + patientInfo.LastName);
                                dicomFile.Dataset.AddOrUpdate(DicomTag.PatientID, Encoding.UTF8, !string.IsNullOrEmpty(patientInfo.EmployeeID) ? patientInfo.EmployeeID : patientInfo.PatientID);
                            }

                            //#endif


                            dicomFile.Save(seriesPath + "\\" + item.SOPInstanceUID);
                            instances.Remove(item);
                            MemoryManagement.FlushMemory();
                        }



                        System.Diagnostics.Process execute = new System.Diagnostics.Process();
                        if (IntPtr.Size == 8)
                            execute.StartInfo.FileName = pacsViwerPath + "//" + "x64" + "//" + "mDicom.exe";
                        else
                            execute.StartInfo.FileName = pacsViwerPath + "//" + "Win32" + "//" + "mDicom.exe";

                        execute.StartInfo.Arguments = "\"" + studyPath + "\""; ;

                        execute.EnableRaisingEvents = true;

                        execute.Exited += (sender, e) =>
                        {
                            if (Directory.Exists(studyPath))
                            {
                                Directory.Delete(studyPath, true);
                            }
                        };

                        execute.Start();


                    }
                }

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
            finally
            {
                IsEnabledMicroDiom = true;
                IsVisibilityProgressBar = Visibility.Collapsed;
            }
        }

        private void EditStudyData()
        {
            if (SelectStudies != null)
            {
                EditStudy pageView = new EditStudy(SelectStudies);
                var viewModel = (EditStudyViewModel)LaunchViewDialogNonPermiss(pageView, true, false);
                if (viewModel.ResultDialog == ActionDialog.Save)
                {
                    Search();
                }
            }

        }

        private async Task GetDicomByteArray(List<InstancesModel> instancesFilms)
        {

            string BaseAddress = System.Configuration.ConfigurationManager.AppSettings["PACSAddress"];
            instances = new List<InstancesCopyToModel>();
            PACSWorkList view = (this.View as PACSWorkList);
            view.SetProgressBarLimits(0, instancesFilms.Count);

            using (var client = new HttpClient())
            {
                var serializer = new JsonSerializer();
                var header = new MediaTypeWithQualityHeaderValue("application/json");

                client.DefaultRequestHeaders.Accept.Add(header);
                client.BaseAddress = new Uri(BaseAddress);
                string requestApi = string.Format("Api/PACS/GetDicomFilesInstancesList");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestApi);
                var json = JsonConvert.SerializeObject(instancesFilms);
                StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content = stringContent;
                await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ContinueWith(
                    (response) =>
                    {

                        using (var stream = response.Result.Content.ReadAsStreamAsync().Result)
                        using (var sr = new StreamReader(stream))
                        using (var jr = new JsonTextReader(sr))
                        {
                            int loopCount = 0;
                            while (jr.Read())
                            {
                                // Don't worry about commas.
                                // JSON reader will handle them for us.
                                if (jr.TokenType != JsonToken.StartArray && jr.TokenType != JsonToken.EndArray)
                                {
                                    loopCount++;
                                    instances.Add(serializer.Deserialize<InstancesCopyToModel>(jr));
                                }

                                view.SetProgressBarValue(loopCount);
                            }
                        }

                    });

            }


        }

        private void ClearBuffer()
        {
            CDRBufferList = null;
        }

        private void ViewerOnCD()
        {
            BurnImage burnImage = new BurnImage();
            (burnImage.DataContext as BurnImageViewModel).BufferList = CDRBufferList;
            LaunchViewDialogNonPermiss(burnImage, true);
        }

        private void DeleteCDBuffer()
        {
            if (SelectCDRBuffer != null)
            {
                CDRBufferList.Remove(SelectCDRBuffer);
            }

        }

        private void DeleteInstance()
        {
            if (SelectInstancesCopy != null)
            {
                foreach (var item in CDRBufferList)
                {
                    if (item.Instances.Count > 1)
                    {
                        item.Instances.Remove(SelectInstancesCopy);
                        PACSWorkList view = (this.View as PACSWorkList);
                        view.grdCDBuffer.RefreshData();
                    }

                }

                SelectInstancesCopy = null;
            }
        }
        #endregion

    }



}
