using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Helpers;
using MediTech.Model;
using MediTech.Models;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace MediTech.ViewModels
{
    public class ManageOrganisationViewModel : MediTechViewModelBase
    {
        #region Variable

        HealthOrganisationModel model;

        #endregion

        #region Properties

        private string _Code;

        public string Code
        {
            get { return _Code; }
            set { Set(ref _Code, value); }
        }

        private bool _IsStock;

        public bool IsStock
        {
            get { return _IsStock; }
            set { Set(ref _IsStock, value); }
        }


        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { Set(ref _Name, value); }
        }

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { Set(ref _Description, value); }
        }

        private string _MobileNo;

        public string MobileNo
        {
            get { return _MobileNo; }
            set { Set(ref _MobileNo, value); }
        }

        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { Set(ref _Email, value); }
        }


        private string _FaxNo;

        public string FaxNo
        {
            get { return _FaxNo; }
            set { Set(ref _FaxNo, value); }
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

        private List<LookupReferenceValueModel> _HealthOrganisationTypes;

        public List<LookupReferenceValueModel> HealthOrganisationTypes
        {
            get { return _HealthOrganisationTypes; }
            set { Set(ref _HealthOrganisationTypes, value); }
        }

        private LookupReferenceValueModel _SelectHealthOrganisationType;

        public LookupReferenceValueModel SelectHealthOrganisationType
        {
            get { return _SelectHealthOrganisationType; }
            set { Set(ref _SelectHealthOrganisationType, value); }
        }

        private BitmapImage _LogoImage;
        public BitmapImage LogoImage
        {
            get
            {
                return _LogoImage;
            }
            set
            {
                Set(ref _LogoImage, value);
            }
        }

        private string _TINNo;

        public string TINNo
        {
            get { return _TINNo; }
            set { Set(ref _TINNo, value); }
        }

        private string _LicenseNo;

        public string LicenseNo
        {
            get { return _LicenseNo; }
            set { Set(ref _LicenseNo, value); }
        }


        private string _Address;

        public string Address
        {
            get { return _Address; }
            set { Set(ref _Address, value); }
        }

        private string _Address2;

        public string Address2
        {
            get { return _Address2; }
            set { Set(ref _Address2, value); }
        }

        public bool SuppressZipCodeEvent { get; set; }

        private List<PostalCode> _ZipCodeSource;

        public List<PostalCode> ZipCodeSource
        {
            get { return _ZipCodeSource; }
            set { Set(ref _ZipCodeSource, value); }
        }

        private PostalCode _SelectedZipCode;

        public PostalCode SelectedZipCode
        {
            get { return _SelectedZipCode; }
            set
            {
                _SelectedZipCode = value;
                if (_SelectedZipCode != null)
                {
                    SelectedProvince = ProvinceSource.FirstOrDefault(p => p.Key == _SelectedZipCode.ProvinceUID);
                    SelectedAmphur = AmphurSource.FirstOrDefault(p => p.Key == _SelectedZipCode.AmphurUID);
                    SelectedDistrict = DistrictSource.FirstOrDefault(p => p.Key == _SelectedZipCode.DistrictUID);
                }
            }
        }


        private string _ZipCode;

        public string ZipCode
        {
            get { return _ZipCode; }
            set
            {
                Set(ref _ZipCode, value);
                if (!SuppressZipCodeEvent)
                {
                    if (!string.IsNullOrEmpty(_ZipCode) && _ZipCode.Length == 5)
                    {

                        ZipCodeSource = DataService.Technical.GetPostalCode(_ZipCode);

                    }
                    else
                    {
                        ZipCodeSource = null;
                    }
                }
                SuppressZipCodeEvent = false;
            }
        }

        public List<LookupItemModel> ProvinceSource { get; set; }
        private LookupItemModel _SelectedProvince;

        public LookupItemModel SelectedProvince
        {
            get { return _SelectedProvince; }
            set
            {
                Set(ref _SelectedProvince, value);
                if (_SelectedProvince != null)
                {
                    AmphurSource = DataService.Technical.GetAmphurByPronvince(_SelectedProvince.Key);
                    DistrictSource = null;
                    ZipCode = string.Empty;
                }
            }
        }


        private List<LookupItemModel> _AmphurSource;

        public List<LookupItemModel> AmphurSource
        {
            get { return _AmphurSource; }
            set { Set(ref _AmphurSource, value); }
        }


        private LookupItemModel _SelectedAmphur;

        public LookupItemModel SelectedAmphur
        {
            get { return _SelectedAmphur; }
            set
            {
                Set(ref _SelectedAmphur, value);
                if (_SelectedAmphur != null)
                {
                    DistrictSource = DataService.Technical.GetDistrictByAmphur(_SelectedAmphur.Key);
                    ZipCode = string.Empty;
                }
            }
        }

        private List<LookupReferenceValueModel> _DistrictSource;

        public List<LookupReferenceValueModel> DistrictSource
        {
            get { return _DistrictSource; }
            set { Set(ref _DistrictSource, value); }
        }
        private LookupReferenceValueModel _SelectedDistrict;

        public LookupReferenceValueModel SelectedDistrict
        {
            get { return _SelectedDistrict; }
            set
            {
                Set(ref _SelectedDistrict, value);
                if (_SelectedDistrict != null)
                {
                    SuppressZipCodeEvent = true;
                    ZipCode = _SelectedDistrict.ValueCode;
                }
            }
        }


        private string _IDFormat;

        public string IDFormat
        {
            get { return _IDFormat; }
            set { Set(ref _IDFormat, value); }
        }

        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set { Set(ref _Comment, value); }
        }

        private int? _IDLength;

        public int? IDLength
        {
            get { return _IDLength; }
            set { Set(ref _IDLength, value); }
        }

        private int? _IDNumberValue;

        public int? IDNumberValue
        {
            get { return _IDNumberValue; }
            set { Set(ref _IDNumberValue, value); }
        }

        private List<LookupReferenceValueModel> _BillTypes;

        public List<LookupReferenceValueModel> BillTypes
        {
            get { return _BillTypes; }
            set { Set(ref _BillTypes, value); }
        }

        private LookupReferenceValueModel _SelectBillType;

        public LookupReferenceValueModel SelectBillType
        {
            get { return _SelectBillType; }
            set { Set(ref _SelectBillType, value); }
        }

        private DateTime? _IDActiveFrom;

        public DateTime? IDActiveFrom
        {
            get { return _IDActiveFrom; }
            set { Set(ref _IDActiveFrom, value); }
        }

        private DateTime? _IDActiveTo;

        public DateTime? IDActiveTo
        {
            get { return _IDActiveTo; }
            set { Set(ref _IDActiveTo, value); }
        }


        private List<HealthOrganisationIDModel> _HealthOrganisationIDs;

        public List<HealthOrganisationIDModel> HealthOrganisationIDs
        {
            get { return _HealthOrganisationIDs ?? (_HealthOrganisationIDs = new List<HealthOrganisationIDModel>()); }
            set { Set(ref _HealthOrganisationIDs, value); }
        }

        private HealthOrganisationIDModel _SelectHealthOrganID;

        public HealthOrganisationIDModel SelectHealthOrganID
        {
            get { return _SelectHealthOrganID; }
            set {
                Set(ref _SelectHealthOrganID, value);
                if (_SelectHealthOrganID != null)
                {
                    IDFormat = SelectHealthOrganID.IDFormat;
                    SelectBillType = BillTypes.FirstOrDefault(p => p.Key == SelectHealthOrganID.BLTYPUID);
                    IDLength = SelectHealthOrganID.IDLength;
                    IDNumberValue = SelectHealthOrganID.NumberValue;
                    IDActiveFrom = SelectHealthOrganID.ActiveFrom;
                    IDActiveTo = SelectHealthOrganID.ActiveTo;
                }
            }
        }

        #endregion

        #region Command

        private RelayCommand _AddIdentifierCommand;
        public RelayCommand AddIdentifierCommand
        {
            get { return _AddIdentifierCommand ?? (_AddIdentifierCommand = new RelayCommand(AddIdentifier)); }
        }

        private RelayCommand _UpdateIdentifierCommand;
        public RelayCommand UpdateIdentifierCommand
        {
            get { return _UpdateIdentifierCommand ?? (_UpdateIdentifierCommand = new RelayCommand(UpdateIdentifier)); }
        }

        private RelayCommand _DeleteIdentifierCommand;
        public RelayCommand DeleteIdentifierCommand
        {
            get { return _DeleteIdentifierCommand ?? (_DeleteIdentifierCommand = new RelayCommand(DeleteIdentifier)); }
        }

        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(SaveOrganisation)); }
        }

        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }
        
        private RelayCommand _UploadCommand;
        public RelayCommand UploadCommand
        {
            get { return _UploadCommand ?? (_UploadCommand = new RelayCommand(Upload)); }
        }
        #endregion

        #region Method
        public ManageOrganisationViewModel()
        {
            var refValues = DataService.Technical.GetReferenceValueList("HOTYP,BLTYP");
            HealthOrganisationTypes = refValues.Where(p => p.DomainCode == "HOTYP").ToList();
            BillTypes = refValues.Where(p => p.DomainCode == "BLTYP").ToList();
            ProvinceSource = DataService.Technical.GetProvince();
            IDActiveFrom = DateTime.Now;
        }

        private void AddIdentifier()
        {
            if (string.IsNullOrEmpty(IDFormat))
            {
                WarningDialog("กรุณาระบุ IDFormat");
                return;
            }


            if (SelectBillType == null)
            {
                WarningDialog("กรุณาเลือกประเภท Bill");
                return;
            }

            if (IDLength == null || IDLength == 0)
            {
                WarningDialog("กรุณาระบุ Length");
                return;
            }

            if (IDNumberValue == null || IDNumberValue == 0)
            {
                WarningDialog("กรุณาระบุ NumberValue");
                return;
            }

            if (HealthOrganisationIDs != null
                && HealthOrganisationIDs.Count(p => p.BLTYPUID == SelectBillType.Key) > 0
                )
            {
                WarningDialog("ประเภทของ Bill ซ้ำ กรุณาตรวจสอบ");
                return;
            }
            HealthOrganisationIDModel newHealthOrgnID = new HealthOrganisationIDModel();
            newHealthOrgnID.IDFormat = IDFormat;
            newHealthOrgnID.BLTYPUID = SelectBillType.Key;
            newHealthOrgnID.BillType = SelectBillType.Display;
            newHealthOrgnID.IDLength = IDLength;
            newHealthOrgnID.NumberValue = IDNumberValue;
            newHealthOrgnID.ActiveFrom = IDActiveFrom;
            newHealthOrgnID.ActiveTo = IDActiveTo;
            HealthOrganisationIDs.Add(newHealthOrgnID);
            OnUpdateEvent();
        }

        private void UpdateIdentifier()
        {
            if (string.IsNullOrEmpty(IDFormat))
            {
                WarningDialog("กรุณาระบุ IDFormat");
                return;
            }


            if (SelectBillType == null)
            {
                WarningDialog("กรุณาเลือกประเภท Bill");
                return;
            }

            if (IDLength == null || IDLength == 0)
            {
                WarningDialog("กรุณาระบุ Length");
                return;
            }

            if (IDNumberValue == null || IDNumberValue == 0)
            {
                WarningDialog("กรุณาระบุ NumberValue");
                return;
            }
            if (HealthOrganisationIDs != null 
                && HealthOrganisationIDs.Count(p => !p.Equals(SelectHealthOrganID) 
                && p.BLTYPUID == SelectBillType.Key) > 0)
            {
                WarningDialog("ประเภทของ Bill ซ้ำ กรุณาตรวจสอบ");
                return;
            }

            if (SelectHealthOrganID != null)
            {
                SelectHealthOrganID.IDFormat = IDFormat;
                SelectHealthOrganID.BLTYPUID = SelectBillType.Key;
                SelectHealthOrganID.BillType = SelectBillType.Display;
                SelectHealthOrganID.IDLength = IDLength;
                SelectHealthOrganID.NumberValue = IDNumberValue;
                SelectHealthOrganID.ActiveFrom = IDActiveFrom;
                SelectHealthOrganID.ActiveTo = IDActiveTo;
                SelectHealthOrganID.MWhen = DateTime.Now;
                OnUpdateEvent();
            }
        }

        private void DeleteIdentifier()
        {
            if (SelectHealthOrganID != null)
            {
                HealthOrganisationIDs.Remove(SelectHealthOrganID);
                OnUpdateEvent();
            }
        }

        private void SaveOrganisation()
        {

            try
            {
                if (String.IsNullOrEmpty(Code))
                {
                    WarningDialog("กรุณาใส่รหัส");
                    return;
                }
                if (string.IsNullOrEmpty(Name))
                {
                    WarningDialog("กรุณาใส่ชื่อสถานประกอบการ");
                    return;
                }

                if (SelectHealthOrganisationType == null)
                {
                    WarningDialog("กรุณาเลือกประเภท");
                    return;
                }


                AssingPropertiesToModel();
                DataService.MasterData.ManageHealthOrganisation(model, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ListOrganisation pageListOrgan = new ListOrganisation();
                ChangeViewPermission(pageListOrgan);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void Cancel()
        {
            ListOrganisation pageListOrgan = new ListOrganisation();
            ChangeViewPermission(pageListOrgan);
        }

        private void Upload()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "(*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            if (op.ShowDialog() == DialogResult.OK)
            {
                LogoImage = new BitmapImage(new Uri(op.FileName));
            }

        }

        public void AssignModelData(HealthOrganisationModel modelData)
        {
            model = modelData;
            AssingModelToProperties();
        }

        public void AssingModelToProperties()
        {
            SuppressZipCodeEvent = true;
            Code = model.Code;
            Name = model.Name;
            MobileNo = model.MobileNo;
            FaxNo = model.FaxNo;
            Description = model.Description;
            ActiveFrom = model.ActiveFrom;
            ActiveTo = model.ActiveTo;
            Address = model.Address;
            Address2 = model.Address2;
            TINNo = model.TINNo;
            LicenseNo = model.LicenseNo;
            Email = model.Email;
            IsStock = model.IsStock == "Y" ? true : false;

            if (HealthOrganisationTypes != null)
                SelectHealthOrganisationType = HealthOrganisationTypes.FirstOrDefault(p => p.Key == model.HOTYPUID);

            if (ProvinceSource != null)
                SelectedProvince = ProvinceSource.FirstOrDefault(p => p.Key == model.ProvinceUID);
            if (AmphurSource != null)
                SelectedAmphur = AmphurSource.FirstOrDefault(p => p.Key == model.AmphurUID);
            if (DistrictSource != null)
                SelectedDistrict = DistrictSource.FirstOrDefault(p => p.Key == model.DistrictUID);
            ZipCode = model.ZipCode;

            if (model.LogoImage != null)
            {
                if (model.LogoImage.Length > 0)
                {
                    LogoImage = ImageHelpers.ConvertByteToBitmap(model.LogoImage);
                }

            }
            else
            {
                LogoImage = null;
            }

            IDFormat = model.IDFormat;
            IDLength = model.IDLength;
            IDNumberValue = model.NumberValue;
            Comment = model.Comment;

            HealthOrganisationIDs = model.HealthOrganisationIDs;
        }

        public void AssingPropertiesToModel()
        {
            if (model == null)
            {
                model = new HealthOrganisationModel();
            }

            model.Code = Code;
            model.Name = Name;
            model.MobileNo = MobileNo;
            model.FaxNo = FaxNo;
            model.Description = Description;
            model.ActiveFrom = ActiveFrom;
            model.ActiveTo = ActiveTo;
            model.Address = Address;
            model.Address2 = Address2;
            model.HOTYPUID = SelectHealthOrganisationType.Key;
            model.IsStock = IsStock ? "Y" : "N";
            model.TINNo = TINNo;
            model.LicenseNo = LicenseNo;
            model.ProvinceUID = SelectedProvince != null ? SelectedProvince.Key : (int?)null;
            model.AmphurUID = SelectedProvince != null ? SelectedAmphur.Key : (int?)null;
            model.DistrictUID = SelectedProvince != null ? SelectedDistrict.Key : (int?)null;
            model.ZipCode = ZipCode;
            model.Email = Email;
            model.IDFormat = IDFormat;
            model.IDLength = IDLength;
            model.NumberValue = IDNumberValue;

            if (LogoImage != null)
            {
                byte[] patImage = ImageHelpers.ResizeImage(ImageHelpers.ConvertBitmapToByte(LogoImage), 600, 400, true);
                model.LogoImage = patImage;
            }
            else
            {
                model.LogoImage = null;
            }

            model.HealthOrganisationIDs = HealthOrganisationIDs;
            model.Comment = Comment;
        }
        #endregion
    }
}
