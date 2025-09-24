//
// FileItem.cs
//
// by Eric Haddan
//
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using IMAPI2.Interop;

namespace IMAPI2.MediaItem
{
    /// <summary>
    /// 
    /// </summary>
    public class FileItem : IMediaItem
    {
        private const Int64 SectorSize = 2048;

        private readonly Int64 mFileLength;

        public FileItem(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("The file added to FileItem was not found!", path);
            }

            filePath = path;

            var fileInfo = new FileInfo(filePath);
            displayName = fileInfo.Name;
            mFileLength = fileInfo.Length;

        }

        private bool isSelected;

        /// <summary>
        /// Gets/sets whether this customer is selected in the UI.
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value == isSelected)
                    return;

                isSelected = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public Int64 SizeOnDisc
        {
            get
            {
                if (mFileLength > 0)
                {
                    return ((mFileLength/SectorSize) + 1)*SectorSize;
                }

                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Path
        {
            get { return filePath; }
        }

        private readonly string filePath;



        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return displayName;
        }

        private readonly string displayName;

        public bool AddToFileSystem(IFsiDirectoryItem rootItem)
        {
            IStream stream = null;

            try
            {
                Win32.SHCreateStreamOnFile(filePath, Win32.STGM_READ | Win32.STGM_SHARE_DENY_WRITE, ref stream);

                if (stream != null)
                {
                    rootItem.AddFile(displayName, stream);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error adding file",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (stream != null)
                {
                    Marshal.FinalReleaseComObject(stream);
                }
            }

            return false;
        }
    }
}
