using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Helpers
{
    public static class PACSHelper
    {
        static string webPACSViewer = System.Configuration.ConfigurationManager.AppSettings["WebPACSViewer"].ToString();
        public static string GetPACSViewerStudyUrl(string studyInstanceUID)
        {
            string PACSViwerURL = string.Format(webPACSViewer + "id=admin&password=password&studyinstanceuid={0}", studyInstanceUID);
            return PACSViwerURL;
        }

        public static string GetPACSViewerSeriesUrl(string seriesInstanceUID)
        {
            string PACSViwerURL = string.Format(webPACSViewer + "id=admin&password=password&seriesinstanceuid={0}", seriesInstanceUID);
            return PACSViwerURL;
        }


        public static string GetPACSViewerPatientUrl(string patientID)
        {
            string PACSViwerURL = string.Format(webPACSViewer + "id=admin&password=password&patientid={0}", patientID);
            return PACSViwerURL;
        }

        public static void OpenPACSViewer(string url)
        {
            foreach (var processBrowser in Process.GetProcesses().Where(p => p.MainWindowTitle.Contains("SonicDICOM") && p.MainWindowTitle.Contains("Viewer")))
            {
                processBrowser.CloseMainWindow();
            }

            try
            {
                Process.Start("chrome.exe", url + " --new-window");
            }
            catch (Exception)
            {

                Process.Start("iexplore.exe", url);
            }
        }
    }
}
