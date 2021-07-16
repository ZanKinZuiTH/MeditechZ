using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ManageUserViewModel : MediTechViewModelBase
    {
        #region Properties

        private int _SelectTabIndex;

        public int SelectTabIndex
        {
            get { return _SelectTabIndex; }
            set { Set(ref _SelectTabIndex, value); }
        }


        private string _Code;

        public string Code
        {
            get { return _Code; }
            set { Set(ref _Code, value); }
        }

        public List<LookupReferenceValueModel> Titles { get; set; }
        private LookupReferenceValueModel _SelectTitle;

        public LookupReferenceValueModel SelectTitle
        {
            get { return _SelectTitle; }
            set { Set(ref _SelectTitle, value); }
        }


        private string _FirstName;

        public string FirstName
        {
            get { return _FirstName; }
            set { Set(ref _FirstName, value); }
        }

        private string _MiddleName;

        public string MiddleName
        {
            get { return _MiddleName; }
            set { Set(ref _MiddleName, value); }
        }

        private string _LastName;

        public string LastName
        {
            get { return _LastName; }
            set { Set(ref _LastName, value); }
        }

        private string _EnglishName;

        public string EnglishName
        {
            get { return _EnglishName; }
            set { Set(ref _EnglishName, value); }
        }

        public List<LookupReferenceValueModel> Genders { get; set; }
        private LookupReferenceValueModel _SelectGender;

        public LookupReferenceValueModel SelectGender
        {
            get { return _SelectGender; }
            set { Set(ref _SelectGender, value); }
        }

        private DateTime? _ActiveFrom;

        public DateTime? ActiveFrom
        {
            get { return _ActiveFrom; }
            set { Set(ref _ActiveFrom, value); }
        }

        private DateTime? _ActiveTo;

        public DateTime? ActiveTo
        {
            get { return _ActiveTo; }
            set { Set(ref _ActiveTo, value); }
        }

        private bool _IsDoctor;

        public bool IsDoctor
        {
            get { return _IsDoctor; }
            set { Set(ref _IsDoctor, value); }
        }

        private bool _IsRadiologist;

        public bool IsRadiologist
        {
            get { return _IsRadiologist; }
            set {
                Set(ref _IsRadiologist, value);
                if (_IsRadiologist)
                {
                    IsEnableAdminRadiologist = true;
                }
                else
                {
                    IsEnableAdminRadiologist = false;
                    IsAdminRadiologist = false;
                }
            }
        }


        private bool _IsAdminRadiologist;

        public bool IsAdminRadiologist
        {
            get { return _IsAdminRadiologist; }
            set { Set(ref _IsAdminRadiologist, value); }
        }

        private bool _IsEnableAdminRadiologist;

        public bool IsEnableAdminRadiologist
        {
            get { return _IsEnableAdminRadiologist; }
            set { Set(ref _IsEnableAdminRadiologist, value); }
        }



        private bool _IsAdminRadread;

        public bool IsAdminRadread
        {
            get { return _IsAdminRadread; }
            set { Set(ref _IsAdminRadread, value); }
        }

        private bool _IsRDUStaff;

        public bool IsRDUStaff
        {
            get { return _IsRDUStaff; }
            set { Set(ref _IsRDUStaff, value); }
        }

        private string _LicenseNo;

        public string LicenseNo
        {
            get { return _LicenseNo; }
            set { Set(ref _LicenseNo, value); }
        }

        private DateTime? _LicenseIssueDttm;

        public DateTime? LicenseIssueDttm
        {
            get { return _LicenseIssueDttm; }
            set { Set(ref _LicenseIssueDttm, value); }
        }

        private DateTime? _LicenseExpiryDttm;

        public DateTime? LicenseExpiryDttm
        {
            get { return _LicenseExpiryDttm; }
            set { Set(ref _LicenseExpiryDttm, value); }
        }

        private string _Tel;

        public string Tel
        {
            get { return _Tel; }
            set { Set(ref _Tel, value); }
        }

        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { Set(ref _Email, value); }
        }

        private string _LineID;

        public string LineID
        {
            get { return _LineID; }
            set { Set(ref _LineID, value); }
        }

        private string _LoginName;

        public string LoginName
        {
            get { return _LoginName; }
            set { Set(ref _LoginName, value); }
        }

        public List<LookupItemModel> Roles { get; set; }
        private LookupItemModel _SelectRole;

        public LookupItemModel SelectRole
        {
            get { return _SelectRole; }
            set { Set(ref _SelectRole, value); }
        }

        private List<RoleProfileModel> _RoleProfiles;

        public List<RoleProfileModel> RoleProfiles
        {
            get { return _RoleProfiles; }
            set { Set(ref _RoleProfiles, value); }
        }

        private RoleProfileModel _SelectRoleProfile;

        public RoleProfileModel SelectRoleProfile
        {
            get { return _SelectRoleProfile; }
            set { Set(ref _SelectRoleProfile, value); }
        }


        private DateTime? _LastPasswordModifiredDTTM;

        public DateTime? LastPasswordModifiredDTTM
        {
            get { return _LastPasswordModifiredDTTM; }
            set { Set(ref _LastPasswordModifiredDTTM, value); }
        }

        private DateTime? _LoginActiveFrom;

        public DateTime? LoginActiveFrom
        {
            get { return _LoginActiveFrom; }
            set { Set(ref _LoginActiveFrom, value); }
        }


        private DateTime? _LoginActiveTo;

        public DateTime? LoginActiveTo
        {
            get { return _LoginActiveTo; }
            set { Set(ref _LoginActiveTo, value); }
        }

        private DateTime? _LastAccessedDttm;

        public DateTime? LastAccessedDttm
        {
            get { return _LastAccessedDttm; }
            set { Set(ref _LastAccessedDttm, value); }
        }


        #endregion

        #region Command


        private RelayCommand _InsertCommand;

        public RelayCommand InsertCommand
        {
            get
            {
                return _InsertCommand
                    ?? (_InsertCommand = new RelayCommand(InsertRoleProfile));
            }
        }

        private RelayCommand _UpdateCommand;

        public RelayCommand UpdateCommand
        {
            get
            {
                return _UpdateCommand
                    ?? (_UpdateCommand = new RelayCommand(UpdateRoleProfile));
            }
        }

        private RelayCommand _DeleteCommand;

        public RelayCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand
                    ?? (_DeleteCommand = new RelayCommand(DeleteRoleProfile));
            }
        }

        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                    ?? (_SaveCommand = new RelayCommand(Save));
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

        #region variable

        CareproviderModel modelCareprovider;

        #endregion

        #region Method

        public ManageUserViewModel()
        {
            RoleProfiles = new List<RoleProfileModel>();
            var refData = DataService.Technical.GetReferenceValueList("TITLE,SEXXX");
            Titles = refData.Where(p => p.DomainCode == "TITLE").ToList();
            Genders = refData.Where(p => p.DomainCode == "SEXXX").ToList();

            Roles = DataService.RoleManage.GetLookUpRoleAll();
            
        }

        void InsertRoleProfile()
        {
            if (ValidateRoleProfile())
            {
                return;
            }

            if (RoleProfiles != null)
            {
                if (RoleProfiles.Count(p => p.RoleUID == SelectRole.Key) > 0)
                {
                    return;
                }
            }
            RoleProfileModel newRole = new RoleProfileModel();
            newRole.RoleUID = SelectRole.Key.Value;
            newRole.RoleName = SelectRole.Display;
            RoleProfiles.Add(newRole);
            OnUpdateEvent();
        }

        void UpdateRoleProfile()
        {
            if (ValidateRoleProfile())
            {
                return;
            }
            if (RoleProfiles != null)
            {
                if (RoleProfiles.Count(p => p.RoleUID == SelectRole.Key) > 0)
                {
                    return;
                }
            }
            if (SelectRoleProfile != null)
            {
                SelectRoleProfile.RoleUID = SelectRole.Key.Value;
                SelectRoleProfile.RoleName = SelectRole.Display;
                SelectRoleProfile.MWhen = DateTime.Now;
                OnUpdateEvent();
            }
        }

        void DeleteRoleProfile()
        {
            if (SelectRoleProfile != null)
            {
                RoleProfiles.Remove(SelectRoleProfile);
                OnUpdateEvent();
            }
        }

        private void Save()
        {
            try
            {
                if (ValidateCareProviderDetail())
                {
                    SelectTabIndex = 0;
                    return;
                }
                if ((!string.IsNullOrEmpty(LoginName) && SelectRole == null) || (!string.IsNullOrEmpty(LoginName)))
                {
                    if (ValidateLoginDetail())
                    {
                        SelectTabIndex = 1;
                        return;
                    }
                }
                AssingPropertiesToModel();
                DataService.UserManage.ManageCareProvider(modelCareprovider, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ListUsers userView = new ListUsers();
                ChangeViewPermission(userView);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void Cancel()
        {
            ListUsers usersPage = new ListUsers();
            ChangeViewPermission(usersPage);
        }


        public void AssingModel(int careProviderUID)
        {
            var modelData = DataService.UserManage.GetCareproviderByUID(careProviderUID);
            modelCareprovider = modelData;
            AssignModelToProperties();
        }

        public void AssignModelToProperties()
        {
            Code = modelCareprovider.Code;
            SelectTitle = Titles.FirstOrDefault(p => p.Key == modelCareprovider.TITLEUID);
            FirstName = modelCareprovider.FirstName;
            MiddleName = modelCareprovider.MiddleName;
            LastName = modelCareprovider.LastName;
            SelectGender = Genders.FirstOrDefault(p => p.Key == modelCareprovider.SEXXXUID);
            EnglishName = modelCareprovider.EnglishName;
            ActiveFrom = modelCareprovider.ActiveFrom;
            ActiveTo = modelCareprovider.ActiveTo;
            IsDoctor = modelCareprovider.IsDoctor;
            IsRadiologist = modelCareprovider.IsRadiologist;
            IsAdminRadiologist = modelCareprovider.IsAdminRadiologist;
            IsRDUStaff = modelCareprovider.IsRDUStaff;
            IsAdminRadread = modelCareprovider.IsAdminRadread;
            LicenseNo = modelCareprovider.LicenseNo;
            LicenseIssueDttm = modelCareprovider.LicenseIssueDttm;
            LicenseExpiryDttm = modelCareprovider.LicenseExpiryDttm;
            Tel = modelCareprovider.Tel;
            Email = modelCareprovider.Email;
            LineID = modelCareprovider.LineID;
            if (modelCareprovider.loginModel != null)
            {
                LoginName = modelCareprovider.loginModel.LoginName;
                SelectRole = Roles.FirstOrDefault(p => p.Key == modelCareprovider.loginModel.RoleUID);
                LastAccessedDttm = modelCareprovider.loginModel.LastAccessedDttm;
                LoginActiveFrom = modelCareprovider.loginModel.ActiveFrom;
                LoginActiveTo = modelCareprovider.loginModel.ActiveTo;
                LastPasswordModifiredDTTM = modelCareprovider.loginModel.LastPasswordModifiredDTTM;
                RoleProfiles = modelCareprovider.loginModel.RoleProfiles;

            }
        }

        public void AssingPropertiesToModel()
        {
            if (modelCareprovider == null)
            {
                modelCareprovider = new CareproviderModel();

            }

            if (modelCareprovider.loginModel == null)
            {
                modelCareprovider.loginModel = new LoginModel();
            }

            modelCareprovider.Code = Code;
            modelCareprovider.TITLEUID = SelectTitle != null ? SelectTitle.Key : (int?)null;
            modelCareprovider.FirstName = FirstName;
            modelCareprovider.MiddleName = MiddleName;
            modelCareprovider.LastName = LastName;
            modelCareprovider.SEXXXUID = SelectGender != null ? SelectGender.Key : (int?)null;
            modelCareprovider.EnglishName = EnglishName;
            modelCareprovider.ActiveFrom = ActiveFrom;
            modelCareprovider.ActiveTo = ActiveTo;
            modelCareprovider.IsDoctor = IsDoctor;
            modelCareprovider.IsRadiologist = IsRadiologist;
            modelCareprovider.IsAdminRadiologist = IsAdminRadiologist;
            modelCareprovider.IsRDUStaff = IsRDUStaff;
            modelCareprovider.IsAdminRadread = IsAdminRadread;
            modelCareprovider.LicenseNo = LicenseNo;
            modelCareprovider.LicenseIssueDttm = LicenseIssueDttm;
            modelCareprovider.LicenseExpiryDttm = LicenseExpiryDttm;
            modelCareprovider.Tel = Tel;
            modelCareprovider.Email = Email;
            modelCareprovider.LineID = LineID;
            modelCareprovider.loginModel.LoginName = LoginName;
            modelCareprovider.loginModel.ActiveFrom = LoginActiveFrom;
            modelCareprovider.loginModel.ActiveTo = LoginActiveTo;
            modelCareprovider.loginModel.RoleUID = SelectRole != null ? SelectRole.Key.Value : 0;

            modelCareprovider.loginModel.RoleProfiles = RoleProfiles;

        }

        bool ValidateCareProviderDetail()
        {
            if (string.IsNullOrEmpty(Code))
            {
                WarningDialog("กรุณาใส่ รหัสผู้ใช้งาน");
                return true;
            }

            if (string.IsNullOrEmpty(FirstName))
            {
                WarningDialog("กรุณาใส่ ชื่อผู้ใช้งาน");
                return true;
            }

            return false;
        }
        bool ValidateLoginDetail()
        {
            if (string.IsNullOrEmpty(LoginName))
            {
                WarningDialog("กรุณาใส่ LoginName");
                return true;
            }

            if (RoleProfiles == null || RoleProfiles.Count(p => p.StatusFlag != "D") <= 0)
            {
                WarningDialog("กรุณาระบุสิทธิ์ อย่างน้อย 1 สิทธิ์");
                return true;
            }

            return false;
        }

        bool ValidateRoleProfile()
        {
            if (SelectRole == null)
            {
                WarningDialog("กรุณาเลือก สิทธิ์");
                return true;
            }

            return false;
        }

        #endregion
    }
}
