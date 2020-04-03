using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Models;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediTech.ViewModels
{
    public class ManageVendorDetailViewModel : MediTechViewModelBase
    {
        #region Variable
        VendorDetailModel model;
        #endregion


        #region Properties


        private string _CompanyName;

        public string CompanyName
        {
            get { return _CompanyName; }
            set { Set(ref _CompanyName, value); }
        }

        private string _ContactPerson;

        public string ContactPerson
        {
            get { return _ContactPerson; }
            set { Set(ref _ContactPerson, value); }
        }

        private string _MobileNo;

        public string MobileNo
        {
            get { return _MobileNo; }
            set { Set(ref _MobileNo, value); }
        }

        private string _FaxNo;

        public string FaxNo
        {
            get { return _FaxNo; }
            set { Set(ref _FaxNo, value); }
        }


        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { Set(ref _Email, value); }
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

        private string _TINNo;

        public string TINNo
        {
            get { return _TINNo; }
            set { Set(ref _TINNo, value); }
        }


        public List<LookupReferenceValueModel> VendorTypes { get; set; }
        private LookupReferenceValueModel _SelectVendorType;

        public LookupReferenceValueModel SelectVendorType
        {
            get { return _SelectVendorType; }
            set { Set(ref _SelectVendorType , value); }
        }


        private string _Address;

        public string Address
        {
            get { return _Address; }
            set { Set(ref _Address, value); }
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

        #endregion

        #region Command

        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(SaveVendorDeail)); }
        }



        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }




        #endregion

        #region Method

        public ManageVendorDetailViewModel()
        {
            VendorTypes = DataService.Technical.GetReferenceValueMany("MNFTP");
            ProvinceSource = DataService.Technical.GetProvince();
        }

        private void SaveVendorDeail()
        {
            try
            {
                if (String.IsNullOrEmpty(CompanyName))
                {
                    WarningDialog("กรุณาใส่ชื่อบริษัท");
                    return;
                }
                if (SelectVendorType == null)
                {
                    WarningDialog("กรุณาใส่ประเภท");
                    return;
                }

                AssingPropertiesToModel();
                DataService.Purchaseing.ManageVendorDetail(model, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ListVendors pageListVendor = new ListVendors();
                ChangeViewPermission(pageListVendor);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void Cancel()
        {
            ListVendors pageListVendor = new ListVendors();
            ChangeViewPermission(pageListVendor);
        }
        public void AssignModelData(VendorDetailModel modelData)
        {
            model = modelData;
            AssingModelTOProperties();
        }

        public void AssingModelTOProperties()
        {
            SuppressZipCodeEvent = true;
            CompanyName = model.CompanyName;
            MobileNo = model.MobileNo;
            FaxNo = model.FaxNo;
            Email = model.Email;
            ContactPerson = model.ContactPerson;
            ActiveFrom = model.ActiveFrom;
            ActiveTo = model.ActiveTo;
            Address = model.Address;
            TINNo = model.TINNo;
            if (VendorTypes != null)
                SelectVendorType = VendorTypes.FirstOrDefault(p => p.Key == model.MNFTPUID);

            if (ProvinceSource != null)
                SelectedProvince = ProvinceSource.FirstOrDefault(p => p.Key == model.ProvinceUID);

            if (AmphurSource != null)
                SelectedAmphur = AmphurSource.FirstOrDefault(p => p.Key == model.AmphurUID);

            if (DistrictSource != null)
                SelectedDistrict = DistrictSource.FirstOrDefault(p => p.Key == model.DistrictUID);
            ZipCode = model.ZipCode;
        }

        public void AssingPropertiesToModel()
        {
            if (model == null)
            {
                model = new VendorDetailModel();
            }

            model.CompanyName = CompanyName;
            model.MobileNo = MobileNo;
            model.FaxNo = FaxNo;
            model.Email = Email;
            model.ContactPerson = ContactPerson;
            model.ActiveFrom = ActiveFrom;
            model.ActiveTo = ActiveTo;
            model.Address = Address;
            model.TINNo = TINNo;
            model.MNFTPUID = SelectVendorType != null ? SelectVendorType.Key : 0;
            model.ProvinceUID = SelectedProvince != null ? SelectedProvince.Key : (int?)null;
            model.AmphurUID = SelectedAmphur != null ? SelectedAmphur.Key : (int?)null;
            model.DistrictUID = SelectedDistrict != null ? SelectedDistrict.Key : (int?)null;
            model.ZipCode = ZipCode;
        }

        #endregion
    }
}
