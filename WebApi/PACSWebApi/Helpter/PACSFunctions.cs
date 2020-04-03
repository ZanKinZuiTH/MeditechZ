using Dicom;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Web;

namespace PACSWebApi.Helpter
{
    public static class PACSFunctions
    {
        public  static void SaveDicomFile(string studyInstanceUID, string seriesInstanceUID, string sopInstanceUID, DicomFile dicomFile)
        {
            SafeTokenHandle safeTokenHandle;
            try
            {
                string userName = System.Configuration.ConfigurationManager.AppSettings["UserName"].ToString();
                string domainName = System.Configuration.ConfigurationManager.AppSettings["DomainName"].ToString();
                string passWord = System.Configuration.ConfigurationManager.AppSettings["Password"].ToString();
                string dicomPath = System.Configuration.ConfigurationManager.AppSettings["DICOMPath"].ToString();
                bool returnValue = LogonUser(userName, domainName, passWord, 2, 0, out safeTokenHandle);

                if (returnValue == true)
                {
                    WindowsIdentity newId = new WindowsIdentity(safeTokenHandle.DangerousGetHandle());
                    using (WindowsImpersonationContext impersonatedUser = newId.Impersonate())
                    {
                        string studyPath = dicomPath + studyInstanceUID;
                        string seriesPath = studyPath + "\\" + seriesInstanceUID;

                        if (!Directory.Exists(studyPath))
                        {
                            Directory.CreateDirectory(studyPath);
                        }

                        if (!Directory.Exists(seriesPath))
                        {
                            Directory.CreateDirectory(seriesPath);
                        }

                        dicomFile.Save(seriesPath + "\\" + sopInstanceUID + ".dcm");
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