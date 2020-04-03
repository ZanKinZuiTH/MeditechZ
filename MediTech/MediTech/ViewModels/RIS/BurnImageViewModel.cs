using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Dicom.Media;
using Dicom;
using IMAPI2.MediaItem;
using System.Windows.Media;
using IMAPI2.Interop;
using System.Runtime.InteropServices;
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using MediTech.Model;
using System.Windows.Data;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;

namespace MediTech.ViewModels
{
    public class BurnImageViewModel : MediTechViewModelBase
    {
        #region Properties

        private IDiscRecorder2 _CurrentRecorder;

        public IDiscRecorder2 CurrentRecorder
        {
            get { return _CurrentRecorder; }
            set { _CurrentRecorder = value; }
        }

        private string _BurnStatusMsg;

        public string BurnStatusMsg
        {
            get { return _BurnStatusMsg; }
            set { Set(ref _BurnStatusMsg, value); }
        }

        private ObservableCollection<IMediaItem> _MediaItems;

        public ObservableCollection<IMediaItem> MediaItems
        {
            get { return _MediaItems ?? (_MediaItems = new ObservableCollection<IMediaItem>()); }
            set { _MediaItems = value; }
        }


        private SolidColorBrush _CapacityProgressBrush;

        public SolidColorBrush CapacityProgressBrush
        {
            get { return _CapacityProgressBrush; }
            set { Set(ref _CapacityProgressBrush, value); }
        }

        private string _TotalSize;
        public string TotalSize
        {
            get { return _TotalSize; }
            set { Set(ref _TotalSize, value); }
        }
        private string _TotalMediaSize;

        public string TotalMediaSize
        {
            get { return _TotalMediaSize; }
            set { Set(ref _TotalMediaSize, value); }
        }

        private string _VolumeLabel;

        public string VolumeLabel
        {
            get { return _VolumeLabel; }
            set { Set(ref _VolumeLabel, value); }
        }

        private string _MediaType;

        public string MediaType
        {
            get { return _MediaType; }
            set { Set(ref _MediaType, value); }
        }

        private string _SupportedMedia;

        public string SupportedMedia
        {
            get { return _SupportedMedia; }
            set
            {
                Set(ref _SupportedMedia, value);
                if (!string.IsNullOrEmpty(SupportedMedia))
                {
                    SuportMediaVisibility = Visibility.Visible;
                }
                else
                {
                    SuportMediaVisibility = Visibility.Hidden;
                }
            }
        }


        private int _CapacityProgressValue;

        public int CapacityProgressValue
        {
            get { return _CapacityProgressValue; }
            set { Set(ref _CapacityProgressValue, value); }
        }

        private int _BurnProgressValue;

        public int BurnProgressValue
        {
            get { return _BurnProgressValue; }
            set { Set(ref _BurnProgressValue, value); }
        }

        private ObservableCollection<SeriesCopyToModel> _BufferList;

        public ObservableCollection<SeriesCopyToModel> BufferList
        {
            get { return _BufferList; }
            set { Set(ref _BufferList, value); }
        }

        private ObservableCollection<MsftDiscRecorder2> _DiscRecorders;

        public ObservableCollection<MsftDiscRecorder2> DiscRecorders
        {
            get { return _DiscRecorders; }
            set { Set(ref _DiscRecorders, value); }
        }



        private ObservableCollection<string> _VerificationTypes;

        public ObservableCollection<string> VerificationTypes
        {
            get { return _VerificationTypes; }
            set { Set(ref _VerificationTypes, value); }
        }

        private bool _IsEnableBurn = false;

        public bool IsEnableBurn
        {
            get { return _IsEnableBurn; }
            set { Set(ref _IsEnableBurn, value); }
        }

        private Visibility _SuportMediaVisibility = Visibility.Hidden;

        public Visibility SuportMediaVisibility
        {
            get { return _SuportMediaVisibility; }
            set { Set(ref _SuportMediaVisibility, value); }
        }

        private string _ContentBurn = "Burn";

        public string ContentBurn
        {
            get { return _ContentBurn; }
            set { Set(ref _ContentBurn, value); }
        }

        private bool _ShouldEject = true;

        public bool ShouldEject
        {
            get { return _ShouldEject; }
            set { _ShouldEject = value; }
        }

        #endregion

        #region Command




        private RelayCommand _ViewerOnCDCommand;

        /// <summary>
        /// Gets the CDBufferCommand.
        /// </summary>
        public RelayCommand ViewerOnCDCommand
        {
            get
            {
                return _ViewerOnCDCommand
                    ?? (_ViewerOnCDCommand = new RelayCommand(ViewerOnCD));
            }
        }

        private RelayCommand _DetectMediaCommand;

        public RelayCommand DetectMediaCommand
        {
            get
            {
                return _DetectMediaCommand
                    ?? (_DetectMediaCommand = new RelayCommand(DetectMedia));
            }
        }

        private RelayCommand _BurnCDCommand;

        public RelayCommand BurnCDCommand
        {
            get
            {
                return _BurnCDCommand
                    ?? (_BurnCDCommand = new RelayCommand(BurnCD));
            }
        }

        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get
            {
                return _CancelCommand
                    ?? (_CancelCommand = new RelayCommand(Cancel));
            }
        }

        #endregion

        #region Variable

        Int64 totalDiscSize;
        private bool isBurning;
        private BackgroundWorker backgroundBurnWorker;
        private readonly BurnData burnData = new BurnData();
        private const string ClientName = "BurnMedia";
        private IMAPI_BURN_VERIFICATION_LEVEL verificationLevel =
    IMAPI_BURN_VERIFICATION_LEVEL.IMAPI_BURN_VERIFICATION_QUICK;

        private ICollectionView discRecordersView;
        private ICollectionView verificationTypesView;

        string tempMedia = Path.GetTempPath() + "\\MeditechMedia";
        string dicomPath = Path.GetTempPath() + "\\MeditechMedia" + "\\IMAGEDICOM";
        #endregion

        #region Method

        public BurnImageViewModel()
        {
            try
            {
                var now = DateTime.Now;
                VolumeLabel = now.Year + "_" + now.Month + "_" + now.Day;
                BurnStatusMsg = string.Empty;

                VerificationTypes = new ObservableCollection<string> { "None", "Quick", "Full" };
                verificationTypesView = CollectionViewSource.GetDefaultView(VerificationTypes);
                verificationTypesView.CurrentChanged += VerificationTypesViewCurrentChanged;

                backgroundBurnWorker = new BackgroundWorker
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };
                backgroundBurnWorker.DoWork += BackgroundBurnWorkerDoWork;
                backgroundBurnWorker.ProgressChanged += BackgroundBurnWorkerProgressChanged;
                backgroundBurnWorker.RunWorkerCompleted += BackgroundBurnWorkerRunWorkerCompleted;

                AddRecordingDevices();
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        public override void OnLoaded()
        {
            try
            {
                var view = (this.View as Views.BurnImage);
                if (view.devicesComboBox.ItemsSource != null)
                    view.devicesComboBox.SelectedIndex = 0;

                view.comboBoxVerification.SelectedIndex = 1;

                DetectMedia();
                ViewerOnCD();
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }


        }

        private void ViewerOnCD()
        {
            try
            {
                if (BufferList != null && BufferList.Count > 0)
                {
                    string pacsViwerPath = string.Empty;
                    if (System.Configuration.ConfigurationManager.AppSettings["PACSViwer"] != null)
                    {
                        pacsViwerPath = System.Configuration.ConfigurationManager.AppSettings["PACSViwer"].ToString();
                    }

                    if (string.IsNullOrEmpty(pacsViwerPath))
                    {
                        WarningDialog("Setting Config PACSViwer");
                        return;
                    }

                    if (!Directory.Exists(pacsViwerPath))
                    {
                        WarningDialog("ไม่พบ PACS Viwer สำหรับ CD/DVD Version กรุณาตรวจสอบ");
                        return;
                    }

                    var dirInfo = new DirectoryInfo(pacsViwerPath);


                    foreach (var folders in dirInfo.GetDirectories())
                    {
                        MediaItems.Add(new DirectoryItem(folders.FullName));
                    }

                    foreach (var files in dirInfo.GetFiles())
                    {
                        MediaItems.Add(new FileItem(files.FullName));
                    }

                    //foreach (var file in dirInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly))
                    //{
                    //    MediaItems.Add(new FileItem(file.FullName));
                    //}


                    int i = 0;
                    var dicomDirPath = Path.Combine(tempMedia, "DICOMDIR");
                    var dicomDir = new DicomDirectory();
                    if (!Directory.Exists(tempMedia))
                    {
                        Directory.CreateDirectory(tempMedia);
                    }

                    DirectoryInfo diTempMedia = new DirectoryInfo(tempMedia);
                    foreach (FileInfo file in diTempMedia.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in diTempMedia.GetDirectories())
                    {
                        dir.Delete(true);
                    }

                    foreach (var item in BufferList)
                    {
                        if (!Directory.Exists(dicomPath))
                        {
                            Directory.CreateDirectory(dicomPath);
                        }
                        foreach (var instance in item.Instances.OrderBy(p => p.InstanceNumber))
                        {

                            MemoryStream ms = new MemoryStream(instance.DicomFiles);
                            var dicomFile = Dicom.DicomFile.Open(ms);
                            dicomFile.Dataset.AddOrUpdate(DicomTag.SpecificCharacterSet, "ISO_IR 192");
                            dicomFile.Dataset.AddOrUpdate(DicomTag.PatientName, Encoding.UTF8, item.No.ToString().PadLeft(4, '0') + " " + item.PatientName);
                            dicomFile.Dataset.AddOrUpdate(DicomTag.PatientID, Encoding.UTF8, !string.IsNullOrEmpty(item.OtherID) ? item.OtherID : item.PatientID);


                            if (dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.MediaStorageSOPInstanceUID, null) == null)
                            {
                                DicomDataset dataset = dicomFile.Dataset;
                                string sopInstanceUID = dicomFile.Dataset.GetSingleValue<string>(DicomTag.SOPInstanceUID);
                                dataset.Add(DicomTag.MediaStorageSOPInstanceUID, sopInstanceUID);

                                DicomFile dicomfile2 = new DicomFile(dataset);
                                dicomfile2.Save(dicomPath + "\\0000000" + i.ToString());

                                dicomDir.AddFile(dicomfile2, String.Format(@"IMAGEDICOM\{0}", "0000000" + i.ToString()));
                                i++;
                                continue;
                            }

                            dicomFile.Save(dicomPath + "\\0000000" + i.ToString());

                            dicomDir.AddFile(dicomFile, String.Format(@"IMAGEDICOM\{0}", "0000000" + i.ToString()));
                            i++;
                        }

                    }
                    dicomDir.Save(dicomDirPath);


                    foreach (var folders in diTempMedia.GetDirectories())
                    {
                        MediaItems.Add(new DirectoryItem(folders.FullName));
                    }

                    foreach (var files in diTempMedia.GetFiles())
                    {
                        MediaItems.Add(new FileItem(files.FullName));
                    }

                    UpdateCapacity();
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }


        }



        private void UpdateCapacity()
        {
            //
            // Get the text for the Max Size
            //
            if (totalDiscSize == 0)
            {
                TotalSize = "0MB";
                return;
            }

            TotalSize = totalDiscSize < 1000000000 ?
                string.Format("{0}MB", totalDiscSize / 1000000) :
                string.Format("{0:F2}GB", totalDiscSize / 1000000000.0);

            //
            // Calculate the size of the files
            //
            Int64 totalMediaSize = MediaItems.Sum(mediaItem => mediaItem.SizeOnDisc);
            TotalMediaSize = totalMediaSize < 1000000000 ?
                string.Format("{0}MB", totalMediaSize / 1000000) :
                string.Format("{0:F2}GB", totalMediaSize / 1000000000.0);
            if (totalMediaSize == 0)
            {
                CapacityProgressValue = 0;
                CapacityProgressBrush = Brushes.Yellow;
            }
            else
            {
                var percent = (int)((totalMediaSize * 100) / totalDiscSize);
                if (percent > 100)
                {
                    CapacityProgressValue = 100;
                    CapacityProgressBrush = Brushes.Red;
                    IsEnableBurn = false;
                }
                else
                {
                    CapacityProgressValue = percent;
                    CapacityProgressBrush = Brushes.Yellow;
                }
            }
        }


        private void DetectMedia()
        {
            if (CurrentRecorder != null)
            {

                MsftFileSystemImage fileSystemImage = null;
                MsftDiscFormat2Data discFormatData = null;

                try
                {
                    //
                    // Create and initialize the IDiscFormat2Data
                    //

                    discFormatData = new MsftDiscFormat2Data();
                    if (!discFormatData.IsCurrentMediaSupported(CurrentRecorder))
                    {
                        MediaType = "Media not supported";
                        totalDiscSize = 0;
                        IsEnableBurn = false;
                        return;
                    }
                    else
                    {
                        //
                        // Get the media type in the recorder
                        //
                        discFormatData.Recorder = CurrentRecorder;
                        IMAPI_MEDIA_PHYSICAL_TYPE currentPhysicalMediaType = discFormatData.CurrentPhysicalMediaType;
                        MediaType = GetMediaTypeString(currentPhysicalMediaType);

                        //
                        // Create a file system and select the media type
                        //
                        fileSystemImage = new MsftFileSystemImage();
                        fileSystemImage.ChooseImageDefaultsForMediaType(currentPhysicalMediaType);

                        //
                        // See if there are other recorded sessions on the disc
                        //
                        if (!discFormatData.MediaHeuristicallyBlank)
                        {
                            fileSystemImage.MultisessionInterfaces = discFormatData.MultisessionInterfaces;
                            fileSystemImage.ImportFileSystem();
                        }

                        Int64 freeMediaBlocks = fileSystemImage.FreeMediaBlocks;
                        totalDiscSize = 2048 * freeMediaBlocks;

                        IsEnableBurn = true;
                    }
                }
                catch (System.Runtime.InteropServices.COMException exception)
                {
                    ErrorDialog(exception.Message);
                }
                finally
                {
                    if (discFormatData != null)
                    {
                        Marshal.ReleaseComObject(discFormatData);
                    }

                    if (fileSystemImage != null)
                    {
                        Marshal.ReleaseComObject(fileSystemImage);
                    }
                }


                UpdateCapacity();
            }
        }

        private void BurnCD()
        {
            //if (MediaItems.Count < 1 )
            //{
            //    return;
            //}

            if (ContentBurn == "Finish")
            {
                CloseViewDialog(ActionDialog.Save);
                return;
            }

            if (!isBurning)
            {
                isBurning = true;
                IsEnableBurn = !isBurning;

                burnData.uniqueRecorderId = CurrentRecorder.ActiveDiscRecorder;
                backgroundBurnWorker.RunWorkerAsync(burnData);
            }
        }

        private void Cancel()
        {
            if (isBurning)
            {
                backgroundBurnWorker.CancelAsync();
            }
            else
            {
                CloseViewDialog(ActionDialog.Cancel);
            }
        }

        void VerificationTypesViewCurrentChanged(object sender, EventArgs e)
        {
            verificationLevel = (IMAPI_BURN_VERIFICATION_LEVEL)verificationTypesView.CurrentPosition;
        }

        void OnDiscRecordersViewCurrentChanged(object sender, EventArgs e)
        {
            CurrentRecorder = discRecordersView.CurrentItem as IDiscRecorder2;

            if (CurrentRecorder != null)
            {
                SupportedMedia = string.Empty;

                //
                // Verify recorder is supported
                //
                IDiscFormat2Data discFormatData = null;
                try
                {
                    discFormatData = new MsftDiscFormat2Data();
                    if (!discFormatData.IsRecorderSupported(CurrentRecorder))
                    {
                        SupportedMedia = "Recorder not supported: " + ClientName;
                        return;
                    }

                    var supportedMediaTypes = new StringBuilder();

                    foreach (IMAPI_PROFILE_TYPE profileType in CurrentRecorder.SupportedProfiles)
                    {
                        string profileName = GetProfileTypeString(profileType);

                        if (string.IsNullOrEmpty(profileName))
                            continue;

                        if (supportedMediaTypes.Length > 0)
                            supportedMediaTypes.Append(", ");
                        supportedMediaTypes.Append(profileName);
                    }

                    SupportedMedia = supportedMediaTypes.ToString();
                }
                catch (COMException)
                {
                    SupportedMedia = "Error getting supported types";
                }
                finally
                {
                    if (discFormatData != null)
                    {
                        Marshal.ReleaseComObject(discFormatData);
                    }

                }

                DetectMedia();
            }
        }

        private void BackgroundBurnWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            MsftDiscRecorder2 discRecorder = null;
            MsftDiscFormat2Data discFormatData = null;

            try
            {
                //
                // Create and initialize the IDiscRecorder2 object
                //
                discRecorder = new MsftDiscRecorder2();
                var data = (BurnData)e.Argument;
                discRecorder.InitializeDiscRecorder(data.uniqueRecorderId);

                //
                // Create and initialize the IDiscFormat2Data
                //
                discFormatData = new MsftDiscFormat2Data
                {
                    Recorder = discRecorder,
                    ClientName = ClientName,
                    ForceMediaToBeClosed = true
                };

                //
                // Set the verification level
                //
                var burnVerification = (IBurnVerification)discFormatData;
                burnVerification.BurnVerificationLevel = verificationLevel;

                //
                // Check if media is blank, (for RW media)
                //
                object[] multisessionInterfaces = null;
                if (!discFormatData.MediaHeuristicallyBlank)
                {
                    multisessionInterfaces = discFormatData.MultisessionInterfaces;
                }

                //
                // Create the file system
                //
                IStream fileSystem;
                if (!CreateMediaFileSystem(discRecorder, multisessionInterfaces, out fileSystem))
                {
                    e.Result = -1;
                    return;
                }

                //
                // add the Update event handler
                //
                discFormatData.Update += DiscFormatDataUpdate;


                //
                // Write the data here
                //
                try
                {
                    discFormatData.Write(fileSystem);
                    e.Result = 0;
                }
                catch (COMException ex)
                {
                    e.Result = ex.ErrorCode;

                    ErrorDialog(ex.Message);

                }
                finally
                {
                    if (fileSystem != null)
                    {
                        Marshal.FinalReleaseComObject(fileSystem);
                    }
                }

                //
                // remove the Update event handler
                //
                discFormatData.Update -= DiscFormatDataUpdate;

                if (ShouldEject)
                {
                    discRecorder.EjectMedia();
                }

            }
            catch (COMException exception)
            {

                e.Result = exception.ErrorCode;


                ErrorDialog(exception.Message);
            }
            finally
            {
                if (discRecorder != null)
                {
                    Marshal.ReleaseComObject(discRecorder);
                }

                if (discFormatData != null)
                {
                    Marshal.ReleaseComObject(discFormatData);
                }
            }
        }




        /// <summary>
        /// Event receives notification from the Burn thread of an event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackgroundBurnWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //int percent = e.ProgressPercentage;
            var data = (BurnData)e.UserState;

            if (data.task == BURN_MEDIA_TASK.BURN_MEDIA_TASK_FILE_SYSTEM)
            {
                BurnStatusMsg = data.statusMessage;
            }
            else if (data.task == BURN_MEDIA_TASK.BURN_MEDIA_TASK_WRITING)
            {
                switch (data.currentAction)
                {
                    case IMAPI_FORMAT2_DATA_WRITE_ACTION.IMAPI_FORMAT2_DATA_WRITE_ACTION_VALIDATING_MEDIA:
                        BurnStatusMsg = "Validating current media...";
                        break;

                    case IMAPI_FORMAT2_DATA_WRITE_ACTION.IMAPI_FORMAT2_DATA_WRITE_ACTION_FORMATTING_MEDIA:
                        BurnStatusMsg = "Formatting media...";
                        break;

                    case IMAPI_FORMAT2_DATA_WRITE_ACTION.IMAPI_FORMAT2_DATA_WRITE_ACTION_INITIALIZING_HARDWARE:
                        BurnStatusMsg = "Initialising hardware...";
                        break;

                    case IMAPI_FORMAT2_DATA_WRITE_ACTION.IMAPI_FORMAT2_DATA_WRITE_ACTION_CALIBRATING_POWER:
                        BurnStatusMsg = "Optimising laser intensity...";
                        break;

                    case IMAPI_FORMAT2_DATA_WRITE_ACTION.IMAPI_FORMAT2_DATA_WRITE_ACTION_WRITING_DATA:
                        long writtenSectors = data.lastWrittenLba - data.startLba;

                        if (writtenSectors > 0 && data.sectorCount > 0)
                        {
                            var percent = (int)((100 * writtenSectors) / data.sectorCount);
                            BurnStatusMsg = string.Format("Progress: {0}%", percent);
                            BurnProgressValue = percent;
                        }
                        else
                        {
                            BurnStatusMsg = "Progress 0%";
                            BurnProgressValue = 0;
                        }
                        break;

                    case IMAPI_FORMAT2_DATA_WRITE_ACTION.IMAPI_FORMAT2_DATA_WRITE_ACTION_FINALIZATION:
                        BurnStatusMsg = "Finalising writing...";
                        break;

                    case IMAPI_FORMAT2_DATA_WRITE_ACTION.IMAPI_FORMAT2_DATA_WRITE_ACTION_COMPLETED:
                        BurnStatusMsg = "Completed!";
                        break;

                    case IMAPI_FORMAT2_DATA_WRITE_ACTION.IMAPI_FORMAT2_DATA_WRITE_ACTION_VERIFYING:
                        BurnStatusMsg = "Verifying";
                        break;
                }
            }
        }


        private void BackgroundBurnWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BurnStatusMsg = (int)e.Result == 0 ? "Finished burning disc" : "Error Burning Disc!";
            BurnProgressValue = 0;

            isBurning = false;
            IsEnableBurn = true;

            ContentBurn = "Finish";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="progress"></param>
        void DiscFormatDataUpdate([In, MarshalAs(UnmanagedType.IDispatch)] object sender, [In, MarshalAs(UnmanagedType.IDispatch)] object progress)
        {
            //
            // Check if we've cancelled
            //
            if (backgroundBurnWorker.CancellationPending)
            {
                var format2Data = (IDiscFormat2Data)sender;
                format2Data.CancelWrite();
                return;
            }

            var eventArgs = (IDiscFormat2DataEventArgs)progress;

            burnData.task = BURN_MEDIA_TASK.BURN_MEDIA_TASK_WRITING;

            // IDiscFormat2DataEventArgs Interface
            burnData.elapsedTime = eventArgs.ElapsedTime;
            burnData.remainingTime = eventArgs.RemainingTime;
            burnData.totalTime = eventArgs.TotalTime;

            // IWriteEngine2EventArgs Interface
            burnData.currentAction = eventArgs.CurrentAction;
            burnData.startLba = eventArgs.StartLba;
            burnData.sectorCount = eventArgs.SectorCount;
            burnData.lastReadLba = eventArgs.LastReadLba;
            burnData.lastWrittenLba = eventArgs.LastWrittenLba;
            burnData.totalSystemBuffer = eventArgs.TotalSystemBuffer;
            burnData.usedSystemBuffer = eventArgs.UsedSystemBuffer;
            burnData.freeSystemBuffer = eventArgs.FreeSystemBuffer;

            //
            // Report back to the UI
            //
            backgroundBurnWorker.ReportProgress(0, burnData);
        }


        private static string GetMediaTypeString(IMAPI_MEDIA_PHYSICAL_TYPE mediaType)
        {
            switch (mediaType)
            {
                default:
                    return "Unknown Media Type";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_CDROM:
                    return "CD-ROM";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_CDR:
                    return "CD-R";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_CDRW:
                    return "CD-RW";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDROM:
                    return "DVD ROM";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDRAM:
                    return "DVD-RAM";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDPLUSR:
                    return "DVD+R";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDPLUSRW:
                    return "DVD+RW";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDPLUSR_DUALLAYER:
                    return "DVD+R Dual Layer";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDDASHR:
                    return "DVD-R";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDDASHRW:
                    return "DVD-RW";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDDASHR_DUALLAYER:
                    return "DVD-R Dual Layer";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DISK:
                    return "random-access writes";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDPLUSRW_DUALLAYER:
                    return "DVD+RW DL";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_HDDVDROM:
                    return "HD DVD-ROM";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_HDDVDR:
                    return "HD DVD-R";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_HDDVDRAM:
                    return "HD DVD-RAM";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_BDROM:
                    return "Blu-ray DVD (BD-ROM)";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_BDR:
                    return "Blu-ray media";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_BDRE:
                    return "Blu-ray Rewritable media";
            }
        }

        /// <summary>
        /// converts an IMAPI_PROFILE_TYPE to it's string
        /// </summary>
        /// <param name="profileType"></param>
        /// <returns></returns>
        static string GetProfileTypeString(IMAPI_PROFILE_TYPE profileType)
        {
            switch (profileType)
            {
                default:
                    return string.Empty;

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_CD_RECORDABLE:
                    return "CD-R";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_CD_REWRITABLE:
                    return "CD-RW";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVDROM:
                    return "DVD ROM";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_DASH_RECORDABLE:
                    return "DVD-R";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_RAM:
                    return "DVD-RAM";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_PLUS_R:
                    return "DVD+R";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_PLUS_RW:
                    return "DVD+RW";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_PLUS_R_DUAL:
                    return "DVD+R Dual Layer";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_DASH_REWRITABLE:
                    return "DVD-RW";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_DASH_RW_SEQUENTIAL:
                    return "DVD-RW Sequential";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_DASH_R_DUAL_SEQUENTIAL:
                    return "DVD-R DL Sequential";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_DASH_R_DUAL_LAYER_JUMP:
                    return "DVD-R Dual Layer";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_PLUS_RW_DUAL:
                    return "DVD+RW DL";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_HD_DVD_ROM:
                    return "HD DVD-ROM";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_HD_DVD_RECORDABLE:
                    return "HD DVD-R";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_HD_DVD_RAM:
                    return "HD DVD-RAM";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_BD_ROM:
                    return "Blu-ray DVD (BD-ROM)";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_BD_R_SEQUENTIAL:
                    return "Blu-ray media Sequential";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_BD_R_RANDOM_RECORDING:
                    return "Blu-ray media";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_BD_REWRITABLE:
                    return "Blu-ray Rewritable media";
            }
        }

        private void AddRecordingDevices()
        {
            DiscRecorders = new ObservableCollection<MsftDiscRecorder2>();
            discRecordersView = CollectionViewSource.GetDefaultView(DiscRecorders);
            discRecordersView.CurrentChanged += OnDiscRecordersViewCurrentChanged;

            //
            // Determine the current recording devices
            //
            MsftDiscMaster2 discMaster = null;
            try
            {
                discMaster = new MsftDiscMaster2();

                if (!discMaster.IsSupportedEnvironment)
                    return;

                foreach (string uniqueRecorderId in discMaster)
                {
                    var discRecorder2 = new MsftDiscRecorder2();
                    discRecorder2.InitializeDiscRecorder(uniqueRecorderId);

                    DiscRecorders.Add(discRecorder2);
                }
                //// Dirty code
                //if (devicesComboBox.Items.Count > 0)
                //{
                //    devicesComboBox.SelectedIndex = 0;
                //}
            }
            catch (COMException ex)
            {
                ErrorDialog(string.Format("Error:{0} - Please install IMAPI2", ex.ErrorCode));
                return;
            }
            finally
            {
                if (discMaster != null)
                {
                    Marshal.ReleaseComObject(discMaster);
                }
            }


            UpdateCapacity();
        }


        #region File System Process

        private bool CreateMediaFileSystem(IDiscRecorder2 discRecorder, object[] multisessionInterfaces, out IStream dataStream)
        {
            MsftFileSystemImage fileSystemImage = null;
            try
            {
                fileSystemImage = new MsftFileSystemImage();
                fileSystemImage.ChooseImageDefaults(discRecorder);
                fileSystemImage.FileSystemsToCreate =
                    FsiFileSystems.FsiFileSystemJoliet | FsiFileSystems.FsiFileSystemISO9660;
                fileSystemImage.VolumeName = VolumeLabel;

                //if (!textBoxLabel.Dispatcher.CheckAccess())
                //{
                //    textBoxLabel.Dispatcher.Invoke(
                //      System.Windows.Threading.DispatcherPriority.Normal, new Action(() => fileSystemImage.VolumeName = textBoxLabel.Text));
                //}
                //else
                //{
                //    fileSystemImage.VolumeName = textBoxLabel.Text;
                //}



                fileSystemImage.Update += FileSystemImageUpdate;

                //
                // If multisessions, then import previous sessions
                //
                if (multisessionInterfaces != null)
                {
                    fileSystemImage.MultisessionInterfaces = multisessionInterfaces;
                    fileSystemImage.ImportFileSystem();
                }

                //
                // Get the image root
                //
                IFsiDirectoryItem rootItem = fileSystemImage.Root;

                //
                // Add Files and Directories to File System Image
                //


                foreach (IMediaItem mediaItem in MediaItems)
                {
                    //
                    // Check if we've cancelled
                    //
                    if (backgroundBurnWorker.CancellationPending)
                    {
                        break;
                    }

                    //
                    // Add to File System
                    //
                    mediaItem.AddToFileSystem(rootItem);
                }

                fileSystemImage.Update -= FileSystemImageUpdate;

                //
                // did we cancel?
                //
                if (backgroundBurnWorker.CancellationPending)
                {
                    dataStream = null;
                    return false;
                }

                dataStream = fileSystemImage.CreateResultImage().ImageStream;
            }
            catch (COMException exception)
            {

                ErrorDialog(exception.Message);
                dataStream = null;
                return false;
            }
            finally
            {
                if (fileSystemImage != null)
                {
                    Marshal.ReleaseComObject(fileSystemImage);
                }
            }

            return true;
        }

        /// <summary>
        /// Event Handler for File System Progress Updates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="currentFile"></param>
        /// <param name="copiedSectors"></param>
        /// <param name="totalSectors"></param>
        void FileSystemImageUpdate([In, MarshalAs(UnmanagedType.IDispatch)] object sender,
            [In, MarshalAs(UnmanagedType.BStr)]string currentFile, [In] int copiedSectors, [In] int totalSectors)
        {
            var percentProgress = 0;
            if (copiedSectors > 0 && totalSectors > 0)
            {
                percentProgress = (copiedSectors * 100) / totalSectors;
            }

            if (!string.IsNullOrEmpty(currentFile))
            {
                var fileInfo = new FileInfo(currentFile);
                burnData.statusMessage = "Adding \"" + fileInfo.Name + "\" to image...";

                //
                // report back to the ui
                //
                burnData.task = BURN_MEDIA_TASK.BURN_MEDIA_TASK_FILE_SYSTEM;
                backgroundBurnWorker.ReportProgress(percentProgress, burnData);
            }

        }
        #endregion

        #endregion
    }
}
