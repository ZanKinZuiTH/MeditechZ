//
// DirectoryItem.cs
//
// by Eric Haddan
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using IMAPI2.Interop;
namespace IMAPI2.MediaItem
{
    /// <summary>
    /// 
    /// </summary>
    public class DirectoryItem : IMediaItem
    {
        private List<IMediaItem> mediaItems = new List<IMediaItem>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directoryPath"></param>
        public DirectoryItem(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new FileNotFoundException("The directory added to DirectoryItem was not found!", directoryPath);
            }

            m_directoryPath = directoryPath;
            FileInfo fileInfo = new FileInfo(m_directoryPath);
            displayName = fileInfo.Name;

            //
            // Get all the files in the directory
            //
            string[] files = Directory.GetFiles(m_directoryPath);
            foreach (string file in files)
            {
                mediaItems.Add(new FileItem(file));
            }

            //
            // Get all the subdirectories
            //
            string[] directories = Directory.GetDirectories(m_directoryPath);
            foreach (string directory in directories)
            {
                mediaItems.Add(new DirectoryItem(directory));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Path
        {
            get
            {
                return m_directoryPath;
            }
        }
        private string m_directoryPath;

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return displayName;
        }
        private string displayName;

        /// <summary>
        /// 
        /// </summary>
        public Int64 SizeOnDisc
        {
            get
            {
                Int64 totalSize = 0;
                foreach (IMediaItem mediaItem in mediaItems)
                {
                    totalSize += mediaItem.SizeOnDisc;
                }
                return totalSize;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootItem"></param>
        /// <returns></returns>
        public bool AddToFileSystem(IFsiDirectoryItem rootItem)
        {
            try
            {
                rootItem.AddTree(m_directoryPath, true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error adding folder",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
