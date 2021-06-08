using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class ManagePayorDetailViewModel : MediTechViewModelBase
    {
        #region Properties

        private bool _PageDialog;
        public bool PageDialog
        {
            get { return _PageDialog; }
            set { Set(ref _PageDialog, value); }
        }

        private string _Code;

        public string Code
        {
            get { return _Code; }
            set { Set(ref _Code, value); }
        }

        private string _PayorName;

        public string PayorName
        {
            get { return _PayorName; }
            set { Set(ref _PayorName, value); }
        }

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { Set(ref _Description, value); }
        }

        private string _ContactPersonName;

        public string ContactPersonName
        {
            get { return _ContactPersonName; }
            set { Set(ref _ContactPersonName, value); }
        }


        private string _Address1;

        public string Address1
        {
            get { return _Address1; }
            set { Set(ref _Address1, value); }
        }

        private string _Address2;

        public string Address2
        {
            get { return _Address2; }
            set { Set(ref _Address2, value); }
        }

        private string _TINNo;

        public string TINNo
        {
            get { return _TINNo; }
            set { Set(ref _TINNo, value); }
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


        private string _Comment;

        public string Comment
        {
            get { return _Comment; }
            set { Set(ref _Comment, value); }
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

        private string _FaxNumber;

        public string FaxNumber
        {
            get { return _FaxNumber; }
            set { Set(ref _FaxNumber, value); }
        }

        private string _PhoneNumber;

        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set { Set(ref _PhoneNumber, value); }
        }


        private string _MobileNumber;

        public string MobileNumber
        {
            get { return _MobileNumber; }
            set { Set(ref _MobileNumber, value); }
        }

        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { Set(ref _Email, value); }
        }

        private List<LookupReferenceValueModel> _CreditTerm;

        public List<LookupReferenceValueModel> CreditTerm
        {
            get { return _CreditTerm; }
            set { Set(ref _CreditTerm, value); }
        }

        private List<LookupReferenceValueModel> _PayorCategory;

        public List<LookupReferenceValueModel> PayorCategory
        {
            get { return _PayorCategory; }
            set { Set(ref _PayorCategory, value); }
        }

        private LookupReferenceValueModel _SelectPayorCategory;

        public LookupReferenceValueModel SelectPayorCategory
        {
            get { return _SelectPayorCategory; }
            set { Set(ref _SelectPayorCategory, value); }
        }

        private LookupReferenceValueModel _SelectPayorCredit;

        public LookupReferenceValueModel SelectPayorCredit
        {
            get { return _SelectPayorCredit; }
            set { Set(ref _SelectPayorCredit, value); }
        }


        private string _AgreementName;

        public string AgreementName
        {
            get { return _AgreementName; }
            set { Set(ref _AgreementName, value); }
        }
        private List<LookupReferenceValueModel> _BillType;

        public List<LookupReferenceValueModel> BillType
        {
            get { return _BillType; }
            set { Set(ref _BillType, value); }
        }

        private LookupReferenceValueModel _SelectBillType;

        public LookupReferenceValueModel SelectBillType
        {
            get { return _SelectBillType; }
            set { Set(ref _SelectBillType, value); }
        }

        private LookupReferenceValueModel _SelectAgreementCredit;

        public LookupReferenceValueModel SelectAgreementCredit
        {
            get { return _SelectAgreementCredit; }
            set { Set(ref _SelectAgreementCredit, value); }
        }

        private DateTime? _ActiveFrom2;

        public DateTime? ActiveFrom2
        {
            get { return _ActiveFrom2; }
            set { Set(ref _ActiveFrom2, value); }
        }

        private DateTime? _ActiveTo2;

        public DateTime? ActiveTo2
        {
            get { return _ActiveTo2; }
            set { Set(ref _ActiveTo2, value); }
        }

        private List<PayorAgreementModel> _PayorAgreements;

        public List<PayorAgreementModel> PayorAgreements
        {
            get
            {
                return _PayorAgreements
                    ?? (_PayorAgreements = new List<PayorAgreementModel>());
            }
            set { Set(ref _PayorAgreements, value); }
        }


        private PayorAgreementModel _SelectPayorAgreement;

        public PayorAgreementModel SelectPayorAgreement
        {
            get { return _SelectPayorAgreement; }
            set
            {
                Set(ref _SelectPayorAgreement, value);
                if (_SelectPayorAgreement != null)
                {
                    AgreementName = SelectPayorAgreement.Name;
                    SelectBillType = BillType.FirstOrDefault(p => p.Key == SelectPayorAgreement.PBTYPUID);
                    SelectAgreementCredit = CreditTerm.FirstOrDefault(p => p.Key == SelectPayorAgreement.PAYTRMUID);
                    ActiveFrom2 = SelectPayorAgreement.ActiveFrom;
                    ActiveTo2 = SelectPayorAgreement.ActiveTo;
                }
            }
        }


        private int _SelectTabIndex;

        public int SelectTabIndex
        {
            get { return _SelectTabIndex; }
            set { Set(ref _SelectTabIndex, value); }
        }

        private bool? _IsGenerateBill;

        public bool? IsGenerateBill
        {
            get { return _IsGenerateBill; }
            set
            {
                Set(ref _IsGenerateBill, value);
                if (IsGenerateBill ?? false)
                {
                    IsEnableIDFormat = Visibility.Visible;
                }
                else
                {
                    IsEnableIDFormat = Visibility.Collapsed;
                }
            }
        }

        private Visibility _IsEnableIDFormat;

        public Visibility IsEnableIDFormat
        {
            get { return _IsEnableIDFormat; }
            set { Set(ref _IsEnableIDFormat, value); }
        }

        private string _IDFormat;

        public string IDFormat
        {
            get { return _IDFormat; }
            set { Set(ref _IDFormat, value); }
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
        #endregion

        #region Command

        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                    ?? (_SaveCommand = new RelayCommand(SavePayor));
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


        private RelayCommand _AddCommand;

        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand
                    ?? (_AddCommand = new RelayCommand(AddAgreement));
            }
        }

        private RelayCommand _EditCommand;

        public RelayCommand EditCommand
        {
            get
            {
                return _EditCommand
                    ?? (_EditCommand = new RelayCommand(EditAgreement));
            }
        }

        private RelayCommand _ClearCommand;

        public RelayCommand ClearCommand
        {
            get
            {
                return _ClearCommand
                    ?? (_ClearCommand = new RelayCommand(ClearAgreement));
            }
        }

        private RelayCommand _DeleteCommand;

        public RelayCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand
                    ?? (_DeleteCommand = new RelayCommand(DeleteAgreement));
            }
        }

        #endregion

        #region Method

        PayorDetailModel payorDetailModel;

        public ManagePayorDetailViewModel()
        {
            var refValue = DataService.Technical.GetReferenceValueList("PAYTRM,PYRACAT,PBTYP");
            ProvinceSource = DataService.Technical.GetProvince();
            CreditTerm = refValue.Where(p => p.DomainCode == "PAYTRM").ToList();
            PayorCategory = refValue.Where(p => p.DomainCode == "PYRACAT").ToList();
            BillType = refValue.Where(p => p.DomainCode == "PBTYP").ToList();

            DateTime now = DateTime.Now;
            ActiveFrom = now;
            ActiveFrom2 = now;

            IsGenerateBill = false;
            IDLength = 4;
            IDFormat = "BL[YYYY][MM][Number]";
            IDNumberValue = 1;
        }
        private void SavePayor()
        {
            try
            {
                if (string.IsNullOrEmpty(Code))
                {
                    WarningDialog("กรุณาระบุ Code");
                    return;
                }

                if (string.IsNullOrEmpty(PayorName))
                {
                    WarningDialog("กรุณาระบุ ชื่อ");
                    return;
                }

                if (PayorAgreements == null || PayorAgreements.Count() <= 0)
                {
                    WarningDialog("กรุณาใส่ข้อตกลง");
                    SelectTabIndex = 1;
                    return;
                }

                if (IsGenerateBill ?? false)
                {
                    if (string.IsNullOrEmpty(IDFormat))
                    {
                        WarningDialog("กรุณาใส่ IDFormat");
                        return;
                    }

                    if (IDLength == null || IDLength == 0)
                    {
                        WarningDialog("กรุณาใส่ IDLength");
                        return;
                    }

                    if (IDNumberValue == null || IDNumberValue == 0)
                    {
                        WarningDialog("กรุณาใส่ NumberValue");
                        return;
                    }
                }

                AssingPropertiesToModel();
                DataService.MasterData.ManagePayorDetail(payorDetailModel, AppUtil.Current.UserID);
                SaveSuccessDialog();
                
                
                if(PageDialog)
                {
                    CloseViewDialog(ActionDialog.Cancel);
                }
                else
                {
                    ListPayorDetail listView = new ListPayorDetail();
                    ChangeViewPermission(listView);
                }

            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }

        private void Cancel()
        {
            if (PageDialog)
            {
                CloseViewDialog(ActionDialog.Cancel);
            }
            else
            {
                ListPayorDetail listView = new ListPayorDetail();
                ChangeViewPermission(listView);
            }
        }

        private void AddAgreement()
        {
            if (string.IsNullOrEmpty(AgreementName))
            {
                WarningDialog("กรุณาระบุ ข้อตกลง");
                return;
            }

            if (SelectBillType == null)
            {
                WarningDialog("กรุณาเลือก Bill Type");
                return;
            }

            PayorAgreementModel payAgree = new PayorAgreementModel();

            payAgree.Name = AgreementName;
            payAgree.PBTYPUID = SelectBillType.Key;
            payAgree.PayorBillType = SelectBillType.Display;
            payAgree.PAYTRMUID = SelectAgreementCredit != null ? SelectAgreementCredit.Key : (int?)null;
            payAgree.PaymentTerms = SelectAgreementCredit != null ? SelectAgreementCredit.Display : null;
            payAgree.ActiveFrom = ActiveFrom2;
            payAgree.ActiveTo = ActiveTo2;

            PayorAgreements.Add(payAgree);
            OnUpdateEvent();
            ClearAgreement();
        }

        private void EditAgreement()
        {
            if (SelectPayorAgreement != null)
            {
                SelectPayorAgreement.Name = AgreementName;
                SelectPayorAgreement.PBTYPUID = SelectBillType.Key;
                SelectPayorAgreement.PayorBillType = SelectBillType.Display;
                SelectPayorAgreement.PAYTRMUID = SelectAgreementCredit != null ? SelectAgreementCredit.Key : (int?)null;
                SelectPayorAgreement.PaymentTerms = SelectAgreementCredit != null ? SelectAgreementCredit.Display : null;
                SelectPayorAgreement.ActiveFrom = ActiveFrom2;
                SelectPayorAgreement.ActiveTo = ActiveTo2;

                SelectPayorAgreement.MWhen = DateTime.Now;
                OnUpdateEvent();
                ClearAgreement();
            }
        }
        private void DeleteAgreement()
        {
            if (SelectPayorAgreement != null)
            {
                PayorAgreements.Remove(SelectPayorAgreement);
                OnUpdateEvent();
                ClearAgreement();
            }
        }

        private void ClearAgreement()
        {
            AgreementName = string.Empty;
            SelectBillType = null;
            SelectAgreementCredit = null;
            ActiveFrom2 = DateTime.Now;
            ActiveTo = null;
            SelectPayorAgreement = null;
        }

        public void AssingModel(int payorDetailUID)
        {
            payorDetailModel = DataService.MasterData.GetPayorDetailByUID(payorDetailUID);
            AssingModelToProperties(payorDetailModel);
        }
        public void AssingPropertiesToModel()
        {
            if (payorDetailModel == null)
            {
                payorDetailModel = new PayorDetailModel();
            }

            payorDetailModel.Code = Code;
            payorDetailModel.PayorName = PayorName;
            payorDetailModel.Description = Description;
            payorDetailModel.ContactPersonName = ContactPersonName;
            payorDetailModel.Address1 = Address1;
            payorDetailModel.Address2 = Address2;
            payorDetailModel.TINNo = TINNo;
            payorDetailModel.ActiveFrom = ActiveFrom;
            payorDetailModel.ActiveTo = ActiveTo;
            payorDetailModel.PAYTRMUID = SelectPayorCredit != null ? SelectPayorCredit.Key : (int?)null;
            payorDetailModel.MobileNumber = MobileNumber;
            payorDetailModel.PhoneNumber = PhoneNumber;
            payorDetailModel.FaxNumber = FaxNumber;
            payorDetailModel.Email = Email;
            payorDetailModel.Comment = Comment;
            payorDetailModel.PYRACATUID = SelectPayorCategory != null ? SelectPayorCategory.Key : (int?)null;
            payorDetailModel.ActiveFrom = ActiveFrom;
            payorDetailModel.ActiveTo = ActiveTo;

            payorDetailModel.ProvinceUID = SelectedProvince != null ? SelectedProvince.Key : (int?)null;
            payorDetailModel.AmphurUID = SelectedAmphur != null ? SelectedAmphur.Key : (int?)null;
            payorDetailModel.DistrictUID = SelectedDistrict != null ? SelectedDistrict.Key : (int?)null;
            payorDetailModel.ZipCode = ZipCode;

            payorDetailModel.PayorAgrrements = PayorAgreements;

            payorDetailModel.IsGenerateBillNumber = IsGenerateBill;
            payorDetailModel.IDFormat = IDFormat;
            payorDetailModel.IDLength = IDLength;
            payorDetailModel.NumberValue = IDNumberValue;

        }

        public void AssingModelToProperties(PayorDetailModel model)
        {

            Code = model.Code;
            PayorName = model.PayorName;
            Description = model.Description;
            ContactPersonName = model.ContactPersonName;
            Address1 = model.Address1;
            Address2 = model.Address2;
            TINNo = model.TINNo;
            ActiveFrom = model.ActiveFrom;
            ActiveTo = model.ActiveTo;
            SelectPayorCredit = CreditTerm.FirstOrDefault(p => p.Key == model.PAYTRMUID);
            MobileNumber = model.MobileNumber;
            PhoneNumber = model.PhoneNumber;
            FaxNumber = model.FaxNumber;
            Email = model.Email;
            Comment = model.Comment;
            SelectPayorCategory = PayorCategory.FirstOrDefault(p => p.Key == model.PYRACATUID);
            ActiveFrom = model.ActiveFrom;
            ActiveTo = model.ActiveTo;


            SuppressZipCodeEvent = true;
            if (ProvinceSource != null)
                SelectedProvince = ProvinceSource.FirstOrDefault(p => p.Key == payorDetailModel.ProvinceUID);

            if (AmphurSource != null)
                SelectedAmphur = AmphurSource.FirstOrDefault(p => p.Key == payorDetailModel.AmphurUID);

            if (DistrictSource != null)
                SelectedDistrict = DistrictSource.FirstOrDefault(p => p.Key == payorDetailModel.DistrictUID);
            ZipCode = payorDetailModel.ZipCode;

            PayorAgreements = model.PayorAgrrements;

            IsGenerateBill = model.IsGenerateBillNumber;
            IDFormat = model.IDFormat;
            IDLength = model.IDLength;
            IDNumberValue = model.NumberValue;

        }

        public void ObjectWindow(bool pageWin)
        {
            PageDialog = pageWin;
        }
        #endregion
    }
}
