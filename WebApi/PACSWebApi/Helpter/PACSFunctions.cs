using Dicom;
using JIUN.DICOM3.IO;
using JIUN.DICOM3.Util;
using JIUN.DSP;
using JIUN.DSP.Entities;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Web;
using JIUN.DSP.Net;

namespace PACSWebApi.Helpter
{
    public static class PACSFunctions
    {
        public  static void SaveDicomFile(string studyPath, string seriesPath, string instancePath, DicomFile dicomFile,out JIUN.DSP.Entities.Instance instancesSonic)
        {
            SafeTokenHandle safeTokenHandle;
            try
            {
                string userName = System.Configuration.ConfigurationManager.AppSettings["UserName"].ToString();
                string domainName = System.Configuration.ConfigurationManager.AppSettings["DomainName"].ToString();
                string passWord = System.Configuration.ConfigurationManager.AppSettings["Password"].ToString();
                bool returnValue = LogonUser(userName, domainName, passWord, 2, 0, out safeTokenHandle);
                instancesSonic = null;
                //if (!Directory.Exists(studyPath))
                //{
                //    Directory.CreateDirectory(studyPath);
                //}

                //if (!Directory.Exists(seriesPath))
                //{
                //    Directory.CreateDirectory(seriesPath);
                //}

                //dicomFile.Save(instancePath);
                if (returnValue == true)
                {
                    WindowsIdentity newId = new WindowsIdentity(safeTokenHandle.DangerousGetHandle());
                    using (WindowsImpersonationContext impersonatedUser = newId.Impersonate())
                    {

                        if (!Directory.Exists(studyPath))
                        {
                            Directory.CreateDirectory(studyPath);
                        }

                        if (!Directory.Exists(seriesPath))
                        {
                            Directory.CreateDirectory(seriesPath);
                        }

                        dicomFile.Save(instancePath);

                        using (FileStream input = new FileStream(instancePath, FileMode.Open, FileAccess.Read))
                        {
                            instancesSonic = PACSFunctions.CreateInstance(input);
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        public static Instance CreateInstance(Stream input)
        {
            using (ElementReader elementReader = new ElementReader(input, true))
            {
                if (input.Length < 132)
                {
                    throw new NotDICOMFileException(NotDICOMFileReason.PreambleNotFound);
                }
                input.Position = 128L;
                if ((input.ReadByte() << 24) + (input.ReadByte() << 16) + (input.ReadByte() << 8) + input.ReadByte() != 1145652045)
                {
                    throw new NotDICOMFileException(NotDICOMFileReason.PrefixNotFound);
                }
                Instance instance = new Instance();
                string[] array = new string[] { "ISO 2022 IR 6" };
                if (array == null || array.Length == 0)
                {
                    array = new string[1]
                    {
                "ISO 2022 IR 6"
                    };
                }
                long length = input.Length;
                while (length > input.Position)
                {
                    JIUN.DICOM3.Element element = elementReader.Read();
                    switch (element.Tag)
                    {
                        case JIUN.DICOM3.Tag.TRANSFER_SYNTAX_UID:
                            instance.TransferSyntaxUID = DataReader.ReadString(element, Array.Empty<string>());
                            break;
                        case JIUN.DICOM3.Tag.SPECIFIC_CHARACTER_SET:
                            {
                                string[] array2 = DataReader.ReadStringArray(element, Array.Empty<string>());
                                if (array2 != null && array2.Length != 0)
                                {
                                    if (array2.Length != 1)
                                    {
                                        array = array2;
                                    }
                                    else if (array2[0] != "ISO_IR 6" && array2[0] != "ISO 2022 IR 6" && array2[0] != "ISO_IR 100" && array2[0] != "ISO 2022 IR 100")
                                    {
                                        array = array2;
                                    }
                                }
                                break;
                            }
                        case JIUN.DICOM3.Tag.IMAGE_TYPE:
                            instance.ImageType = DataReader.ReadString(element, Array.Empty<string>());
                            break;
                        case JIUN.DICOM3.Tag.COLUMNS:
                            instance.Width = DataReader.ReadInt32(element);
                            break;
                        case JIUN.DICOM3.Tag.ROWS:
                            instance.Height = DataReader.ReadInt32(element);
                            break;
                        case JIUN.DICOM3.Tag.WINDOW_WIDTH:
                            instance.WindowWidth = DataReader.ReadString(element, Array.Empty<string>());
                            break;
                        case JIUN.DICOM3.Tag.WINDOW_CENTER:
                            instance.WindowCenter = DataReader.ReadString(element, Array.Empty<string>());
                            break;
                        case JIUN.DICOM3.Tag.LOSSY_IMAGE_COMPRESSION:
                            instance.LossyImageCompression = DataReader.ReadString(element, Array.Empty<string>());
                            break;
                        case JIUN.DICOM3.Tag.PIXEL_DATA:
                            instance.PixelData = (int)(element.Data as JIUN.DICOM3.StreamData).Posision;
                            break;
                    }
                }
                instance.SpecificCharacterSet = array.ToSpecificCharacterSetsValue();
                instance.FileSize = input.Length;
                return instance;
            }
        }
    }

    public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeTokenHandle()
            : base(true)
        {
        }

        [DllImport("kernel32.dll")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr handle);

        protected override bool ReleaseHandle()
        {
            return CloseHandle(handle);
        }
    }
}