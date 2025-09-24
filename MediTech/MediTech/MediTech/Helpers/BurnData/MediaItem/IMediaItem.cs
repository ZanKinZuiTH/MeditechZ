//
// IMediaItem.cs
//
// by Eric Haddan
//
using System;
using System.Windows.Media.Imaging;
using IMAPI2.Interop;


namespace IMAPI2.MediaItem
{
    public interface IMediaItem
    {
        /// <summary>
        /// Returns the full path of the file or directory
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Returns the size of the file or directory to the next largest sector
        /// </summary>
        Int64 SizeOnDisc { get; }


        // Adds the file or directory to the directory item, usually the root.
        bool AddToFileSystem(IFsiDirectoryItem rootItem);

    }
}
