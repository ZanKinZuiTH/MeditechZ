using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Helpers
{
    namespace RDNIDWRAPPER
    {
        internal static partial class DefineConstants
        {
            public const int NID_SUCCESS = 0;
            public const int NID_INTERNAL_ERROR = -1;
            public const int NID_INVALID_LICENSE = -2;
            public const int NID_READER_NOT_FOUND = -3;
            public const int NID_CONNECTION_ERROR = -4;
            public const int NID_GET_PHOTO_ERROR = -5;
            public const int NID_GET_DATA_ERROR = -6;
            public const int NID_INVALID_CARD = -7;
            public const int NID_UNKNOWN_CARD_VERSION = -8;
            public const int NID_DISCONNECTION_ERROR = -9;
            public const int NID_INIT_ERROR = -10;
            public const int NID_SUPPORTED_READER_NOT_FOUND = -11;
        }

        class RDNID
        {

            const string _RDNIDLib_DLL_ = "RDNIDLibD.DLL";

            [DllImport(_RDNIDLib_DLL_,
                    EntryPoint = "OpenNIDLib", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern Int32 OpenNIDLib(byte[] _szReaders);

            [DllImport(_RDNIDLib_DLL_)]
            public static extern Int32 CloseNIDLib();

            [DllImport(_RDNIDLib_DLL_)]
            public static extern Int32 LoadNIDData();

            [DllImport(_RDNIDLib_DLL_,
                    EntryPoint = "getReaderListRD", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern Int32 getReaderListRD([Out] byte[] _szReaders, int size);

            [DllImport(_RDNIDLib_DLL_,
                    EntryPoint = "selectReaderRD", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr selectReaderRD(byte[] _szReaders);

            [DllImport(_RDNIDLib_DLL_,
                    EntryPoint = "deselectReaderRD", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern Int32 deselectReaderRD(IntPtr obj);

            [DllImport(_RDNIDLib_DLL_,
                    EntryPoint = "disconnectCardRD", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern Int32 disconnectCardRD(IntPtr obj);

            [DllImport(_RDNIDLib_DLL_,
                    EntryPoint = "isCardInsertRD", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern Int32 isCardInsertRD(IntPtr obj);

            [DllImport(_RDNIDLib_DLL_,
                    EntryPoint = "getNIDNumberRD", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern Int32 getNIDNumberRD(IntPtr obj, [Out] byte[] strID);

            [DllImport(_RDNIDLib_DLL_,
                    EntryPoint = "getNIDTextRD", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern Int32 getNIDTextRD(IntPtr obj, [Out] byte[] strData, int sizeData);

            [DllImport(_RDNIDLib_DLL_,
                    EntryPoint = "getNIDPhotoRD", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern Int32 getNIDPhotoRD(IntPtr obj, [Out] byte[] strData, out int sizeData);


            [DllImport(_RDNIDLib_DLL_,
                    EntryPoint = "getSoftwareInfoRD", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern Int32 getSoftwareInfoRD([Out] byte[] strData);


            [DllImport(_RDNIDLib_DLL_,
                    EntryPoint = "getDLDInfoRD", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern Int32 getDLDInfoRD([Out] byte[] strData);


            [DllImport(_RDNIDLib_DLL_,
                    EntryPoint = "chkReaderLicenseRD", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern Int32 chkReaderLicenseRD(byte[] _szReaders);

            [DllImport(_RDNIDLib_DLL_,
                    EntryPoint = "connectCardRD", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern Int32 connectCardRD(IntPtr obj);

            [DllImport(_RDNIDLib_DLL_,
                    EntryPoint = "updateLicenseFileRD", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern Int32 updateLicenseFileRD(byte[] targetPath);

            [DllImport(_RDNIDLib_DLL_,
                    EntryPoint = "getAtrDD", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern Int32 getAtrDD(IntPtr obj, [Out] byte[] strData, int sizeData);

            [DllImport(_RDNIDLib_DLL_,
                    EntryPoint = "getRidDD", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern Int32 getRidDD(IntPtr obj, [Out] byte[] strData);

            [DllImport(_RDNIDLib_DLL_,
                    EntryPoint = "openNIDLibRD", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern Int32 openNIDLibRD(byte[] _szReaders);

            [DllImport(_RDNIDLib_DLL_)]
            public static extern Int32 closeNIDLibRD();

            [DllImport(_RDNIDLib_DLL_,
                    EntryPoint = "getLicenseInfoRD", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern Int32 getLicenseInfoRD([Out] byte[] strData);

        }
    }
}
