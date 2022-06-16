using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech
{
    public class AppUtil
    {
        private int _loginID;

        public int LoginID
        {
            get { return _loginID; }
            set { _loginID = value; }
        }

        private string _loginName;

        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = value; }
        }

        private int _userID;

        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private int _RoleUID;
        public int RoleUID
        {
            get { return _RoleUID; }
            set { _RoleUID = value; }
        }

        private string _RoleName;

        public string RoleName
        {
            get { return _RoleName; }
            set { _RoleName = value; }
        }


        private System.Nullable<bool> _IsAdmin;

        public System.Nullable<bool> IsAdmin
        {
            get { return _IsAdmin; }
            set { _IsAdmin = value; }
        }

        private System.Nullable<bool> _IsDoctor;

        public System.Nullable<bool> IsDoctor
        {
            get { return _IsDoctor; }
            set { _IsDoctor = value; }
        }

        private System.Nullable<bool> _IsRadiologist;

        public System.Nullable<bool> IsRadiologist
        {
            get { return _IsRadiologist; }
            set { _IsRadiologist = value; }
        }

        private System.Nullable<bool> _IsAdminRadiologist;

        public System.Nullable<bool> IsAdminRadiologist
        {
            get { return _IsAdminRadiologist; }
            set { _IsAdminRadiologist = value; }
        }

        private System.Nullable<bool> _IsAdminRadread;

        public System.Nullable<bool> IsAdminRadread
        {
            get { return _IsAdminRadread; }
            set { _IsAdminRadread = value; }
        }

        private System.Nullable<bool> _IsRDUStaff;

        public System.Nullable<bool> IsRDUStaff
        {
            get { return _IsRDUStaff; }
            set { _IsRDUStaff = value; }
        }

        private int _OwnerOrganisationUID;

        public int OwnerOrganisationUID
        {
            get { return _OwnerOrganisationUID; }
            set { _OwnerOrganisationUID = value; }
        }

        private int _LocationUID;

        public int LocationUID
        {
            get { return _LocationUID; }
            set { _LocationUID = value; }
        }

        private List<CareproviderOrganisationModel> _CareproviderOrganisations;

        public List<CareproviderOrganisationModel> CareproviderOrganisations
        {
            get { return _CareproviderOrganisations; }
            set { _CareproviderOrganisations = value; }
        }

        private List<CareproviderLocationModel> _CareproviderLocations;

        public List<CareproviderLocationModel> CareproviderLocations
        {
            get { return _CareproviderLocations; }
            set { _CareproviderLocations = value; }
        }

        private string _ApplicationVersion;

        public string ApplicationVersion
        {
            get { return _ApplicationVersion; }
            set { _ApplicationVersion = value; }
        }

        private string _ApplicationStatus;

        public string ApplicationStaus
        {
            get { return _ApplicationStatus; }
            set { _ApplicationStatus = value; }
        }

        private string _ApplicationId;

        public string ApplicationId
        {
            get { return _ApplicationId; }
            set { _ApplicationId = value; }
        }


        private List<PageViewModel> _PageViewPermission;

        public List<PageViewModel> PageViewPermission
        {
            get { return _PageViewPermission; }
            set { _PageViewPermission = value; }
        }

        private static AppUtil _Current;
        public static AppUtil Current
        {
            get
            {
                return _Current ?? (_Current = new AppUtil());
            }
            internal set { _Current = value; }
        }
    }
}
