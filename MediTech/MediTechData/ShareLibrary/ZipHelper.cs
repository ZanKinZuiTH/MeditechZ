using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareLibrary
{
    public class ZipHelper
    {
        public static void CreateZip(string outputFilePath, string inputFilePath, params string[] inputFilePaths)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AddFile(inputFilePath);
                foreach (string path in inputFilePaths)
                {
                    zip.AddFile(path);
                }

                zip.Save(outputFilePath);
            }
        }

        public static void ExtractZip(string zipFilePath, string outputDir)
        {
            using (ZipFile zip = new ZipFile(zipFilePath))
            {
                //zip.ExtractAll(outputDir, ExtractExistingFileAction.OverwriteSilently);
                foreach (ZipEntry e in zip)
                {
                    try
                    {
                        e.Extract(outputDir, ExtractExistingFileAction.OverwriteSilently);
                    }
                    catch (Exception ex)
                    {
                        string s = ex.Message;
                    }
                }
            }
        }
    }
}
