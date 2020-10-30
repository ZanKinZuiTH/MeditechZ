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
    public class ManageRequestItemViewModel : MediTechViewModelBase
    {

        #region Properties

        private Visibility _ImageTypeVisibility = Visibility.Collapsed;

        public Visibility ImageTypeVisibility
        {
            get { return _ImageTypeVisibility; }
            set { Set(ref _ImageTypeVisibility, value); }
        }

        private string _Code;

        public string Code
        {
            get { return _Code; }
            set { Set(ref _Code, value); }
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

        private List<LookupReferenceValueModel> _Category;

        public List<LookupReferenceValueModel> Category
        {
            get { return _Category; }
            set { Set(ref _Category, value); }
        }

        private LookupReferenceValueModel _SelectCategory;

        public LookupReferenceValueModel SelectCategory
        {
            get { return _SelectCategory; }
            set { Set(ref _SelectCategory, value); }
        }

        private List<LookupReferenceValueModel> _TestType;

        public List<LookupReferenceValueModel> TestType
        {
            get { return _TestType; }
            set { Set(ref _TestType, value); }
        }

        private LookupReferenceValueModel _SelectTestType;

        public LookupReferenceValueModel SelectTestType
        {
            get { return _SelectTestType; }
            set
            {
                Set(ref _SelectTestType, value);
                if (SelectTestType != null && SelectTestType.Display == "RADIOLOGY")
                {
                    ImageTypeVisibility = Visibility.Visible;
                }
                else
                {
                    ImageTypeVisibility = Visibility.Collapsed;
                }
            }
        }

        private List<LookupReferenceValueModel> _ImageType;

        public List<LookupReferenceValueModel> ImageType
        {
            get { return _ImageType; }
            set { Set(ref _ImageType, value); }
        }

        private LookupReferenceValueModel _SelectImageType;

        public LookupReferenceValueModel SelectImageType
        {
            get { return _SelectImageType; }
            set { Set(ref _SelectImageType, value); }
        }

        #region Parameters

        private string _ParameterName;

        public string ParameterName
        {
            get { return _ParameterName; }
            set { Set(ref _ParameterName, value); }
        }

        private int _PrintOrder;

        public int PrintOrder
        {
            get { return _PrintOrder; }
            set { Set(ref _PrintOrder, value); }
        }

        private bool _IsCheckMandatory;

        public bool IsCheckMandatory
        {
            get { return _IsCheckMandatory; }
            set { Set(ref _IsCheckMandatory, value); }
        }

        private List<RequestResultLinkModel> _RequestResultLinks;

        public List<RequestResultLinkModel> RequestResultLinks
        {
            get { return _RequestResultLinks; }
            set { Set(ref _RequestResultLinks, value); }
        }

        private RequestResultLinkModel _SelectRequestResultLink;

        public RequestResultLinkModel SelectRequestResultLink
        {
            get { return _SelectRequestResultLink; }
            set
            {
                Set(ref _SelectRequestResultLink, value);
                if (SelectRequestResultLink != null)
                {
                    PrintOrder = SelectRequestResultLink.PrintOrder ?? 0;
                    IsCheckMandatory = SelectRequestResultLink.IsMandatory == "Y" ? true : false;

                    if (SelectedParameterSearch == null)
                        SelectedParameterSearch = new ResultItemModel();

                    SelectedParameterSearch.ResultItemUID = SelectRequestResultLink.ResultItemUID;
                    SelectedParameterSearch.DisplyName = SelectRequestResultLink.ResultItemName;
                    SelectedParameterSearch.Description = SelectRequestResultLink.ResultItemName;
                    SelectedParameterSearch.UOM = SelectRequestResultLink.Unit;
                    ParameterName = SelectRequestResultLink.ResultItemName;
                }
                else
                {
                    SelectedParameterSearch = null;
                    PrintOrder = 0;
                    IsCheckMandatory = false;
                    ParameterName = string.Empty;
                }

            }
        }
        #endregion

        #region Specimen

        private string _SpecimenName;

        public string SpecimenName
        {
            get { return _SpecimenName; }
            set { Set(ref _SpecimenName, value); }
        }

        private string _CollectionMethod;

        public string CollectionMethod
        {
            get { return _CollectionMethod; }
            set { Set(ref _CollectionMethod, value); }
        }

        private string _SpecimenType;

        public string SpecimenType
        {
            get { return _SpecimenType; }
            set { Set(ref _SpecimenType, value); }
        }

        private string _CollecitonSite;

        public string CollecitonSite
        {
            get { return _CollecitonSite; }
            set { Set(ref _CollecitonSite, value); }
        }


        private string _VolumnetoCollected;

        public string VolumnetoCollected
        {
            get { return _VolumnetoCollected; }
            set { Set(ref _VolumnetoCollected, value); }
        }

        private string _CollectionRoute;

        public string CollectionRoute
        {
            get { return _CollectionRoute; }
            set { Set(ref _CollectionRoute, value); }
        }

        private bool _SpecimenIsDefault;

        public bool SpecimenIsDefault
        {
            get { return _SpecimenIsDefault; }
            set { Set(ref _SpecimenIsDefault, value); }
        }


        private List<RequestItemSpecimenModel> _RequestItemSpecimen;

        public List<RequestItemSpecimenModel> RequestItemSpecimen
        {
            get { return _RequestItemSpecimen; }
            set { Set(ref _RequestItemSpecimen, value); }
        }

        private RequestItemSpecimenModel _SelectRequestItemSpecimen;

        public RequestItemSpecimenModel SelectRequestItemSpecimen
        {
            get { return _SelectRequestItemSpecimen; }
            set { Set(ref _SelectRequestItemSpecimen, value); }
        }


        #endregion


        #region GroupResult
        private List<LookupReferenceValueModel> _RequestItemGropResult;

        public List<LookupReferenceValueModel> RequestItemGropResult
        {
            get { return _RequestItemGropResult; }
            set { Set(ref _RequestItemGropResult, value); }
        }

        private LookupReferenceValueModel _SelectGroupResult;

        public LookupReferenceValueModel SelectGroupResult
        {
            get { return _SelectGroupResult; }
            set { Set(ref _SelectGroupResult, value); }
        }

        private List<RequestItemGroupResultModel> _RequestItemGroupResults;

        public List<RequestItemGroupResultModel> RequestItemGroupResults
        {
            get { return _RequestItemGroupResults; }
            set { Set(ref _RequestItemGroupResults, value); }
        }

        private RequestItemGroupResultModel _SelectRequestItemGroupResult;

        public RequestItemGroupResultModel SelectRequestItemGroupResult
        {
            get { return _SelectRequestItemGroupResult; }
            set { Set(ref _SelectRequestItemGroupResult, value); }
        }

        private int _PrintOrderGroupResult;

        public int PrintOrderGroupResult
        {
            get { return _PrintOrderGroupResult; }
            set { Set(ref _PrintOrderGroupResult, value); }
        }

        #endregion

        #endregion

        #region Command

        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save)); }
        }



        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }


        #region Parameters


        #region ParameterSearch

        private string _SearchParameterCriteria;

        public string SearchParameterCriteria
        {
            get { return _SearchParameterCriteria; }
            set
            {
                Set(ref _SearchParameterCriteria, value);
                if (!string.IsNullOrEmpty(_SearchParameterCriteria) && _SearchParameterCriteria.Length >= 3)
                {
                    ParametersSource = DataService.MasterData.SearchResultItem(_SearchParameterCriteria, null);

                }
                else
                {
                    ParametersSource = null;
                }
            }
        }


        private List<ResultItemModel> _ParametersSource;

        public List<ResultItemModel> ParametersSource
        {
            get { return _ParametersSource; }
            set { Set(ref _ParametersSource, value); }
        }

        private ResultItemModel _SelectedParameterSearch;

        public ResultItemModel SelectedParameterSearch
        {
            get { return _SelectedParameterSearch; }
            set
            {
                _SelectedParameterSearch = value;
                if (SelectedParameterSearch != null)
                {
                    ParameterName = _SelectedParameterSearch.DisplyName;
                    SearchParameterCriteria = string.Empty;
                }
            }
        }

        #endregion

        private RelayCommand _AddParameterCommand;
        public RelayCommand AddParameterCommand
        {
            get { return _AddParameterCommand ?? (_AddParameterCommand = new RelayCommand(AddParameter)); }
        }


        private RelayCommand _UpdateParameterCommand;
        public RelayCommand UpdateParameterCommand
        {
            get { return _UpdateParameterCommand ?? (_UpdateParameterCommand = new RelayCommand(UpdateParameter)); }
        }

        private RelayCommand _DeleteParameterCommand;
        public RelayCommand DeleteParameterCommand
        {
            get { return _DeleteParameterCommand ?? (_DeleteParameterCommand = new RelayCommand(DeleteParameter)); }
        }

        #endregion

        #region Specimen

        private string _SearchSpecimenCriteria;

        public string SearchSpecimenCriteria
        {
            get { return _SearchSpecimenCriteria; }
            set
            {
                Set(ref _SearchSpecimenCriteria, value);
                if (!string.IsNullOrEmpty(_SearchSpecimenCriteria) && _SearchSpecimenCriteria.Length >= 3)
                {
                    SpecimenSource = DataService.MasterData.SearhcSpecimen(_SearchSpecimenCriteria);

                }
                else
                {
                    SpecimenSource = null;
                }
            }
        }


        private List<SpecimenModel> _SpecimenSource;

        public List<SpecimenModel> SpecimenSource
        {
            get { return _SpecimenSource; }
            set { Set(ref _SpecimenSource, value); }
        }

        private SpecimenModel _SelectSpecimen;

        public SpecimenModel SelectSpecimen
        {
            get { return _SelectSpecimen; }
            set
            {
                _SelectSpecimen = value;
                if (SelectSpecimen != null)
                {
                    SearchSpecimenCriteria = string.Empty;
                    SpecimenName = SelectSpecimen.Name;
                    CollectionMethod = SelectSpecimen.CollectionMethod;
                    SpecimenType = SelectSpecimen.SpecimenType;
                    CollecitonSite = SelectSpecimen.CollectionSite;
                    CollectionRoute = SelectSpecimen.CollectionRoute;
                    VolumnetoCollected = SelectSpecimen.VolumeCollected != null ? SelectSpecimen.VolumeCollected.ToString() : "";
                    CollectionMethod = SelectSpecimen.CollectionMethod;
                }
            }
        }

        private RelayCommand _AddSpecimenCommand;
        public RelayCommand AddSpecimenCommand
        {
            get { return _AddSpecimenCommand ?? (_AddSpecimenCommand = new RelayCommand(AddSpecimen)); }
        }



        private RelayCommand _DeleteSpecimenCommand;
        public RelayCommand DeleteSpecimenCommand
        {
            get { return _DeleteSpecimenCommand ?? (_DeleteSpecimenCommand = new RelayCommand(DeleteSpecimen)); }
        }

        #endregion

        #region GroupResult

        private RelayCommand _AddGroupResultCommand;
        public RelayCommand AddGroupResultCommand
        {
            get { return _AddGroupResultCommand ?? (_AddGroupResultCommand = new RelayCommand(AddGroupResult)); }
        }

        private RelayCommand _DeleteGroupResultCommand;
        public RelayCommand DeleteGroupResultCommand
        {
            get { return _DeleteGroupResultCommand ?? (_DeleteGroupResultCommand = new RelayCommand(DeleteGroupResult)); }
        }

        #endregion

        #endregion


        #region Method

        RequestItemModel model;

        public ManageRequestItemViewModel()
        {
            var refData = DataService.Technical.GetReferenceValueList("TSTTP,RIMTYP,PRTGP,GPRST");
            TestType = refData.Where(p => p.DomainCode == "TSTTP").ToList();
            ImageType = refData.Where(p => p.DomainCode == "RIMTYP").ToList();
            Category = refData.Where(p => p.DomainCode == "PRTGP").ToList();
            RequestItemGropResult = refData.Where(p => p.DomainCode == "GPRST").ToList();
            ActiveFrom = DateTime.Now;
        }

        public void AssignModel(RequestItemModel model)
        {
            this.model = DataService.MasterData.GetRequestItemByUID(model.RequestItemUID);
            AssignModelToProperties();
        }


        private void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(Code))
                {
                    WarningDialog("กรุณาใส่ รหัส");
                    return;
                }
                if (string.IsNullOrEmpty(Name))
                {
                    WarningDialog("กรุณาใส่ ชื่อ");
                    return;
                }
                if (SelectTestType == null)
                {
                    WarningDialog("กรุณาเลือก ประเภท");
                    return;
                }

                if (SelectCategory == null)
                {
                    WarningDialog("กรุณาเลือก หมวดหมู่");
                    return;
                }


                AssingPropertiesToModel();



                if (model.RequestItemUID == 0)
                {
                    var dupicateCode = DataService.MasterData.GetRequestItemByCode(Code);
                    if (dupicateCode != null)
                    {
                        WarningDialog("Code ซ้ำ โปรดตรวจสอบ");
                        return;
                    }
                }

                DataService.MasterData.ManageRequestItem(model, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ListRequestItem pageList = new ListRequestItem();
                ChangeViewPermission(pageList);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void Cancel()
        {
            ListRequestItem pageList = new ListRequestItem();
            ChangeViewPermission(pageList);
        }

        bool ValidateRequestResultLink()
        {
            if (SelectedParameterSearch == null)
            {
                WarningDialog("กรุณาเลือก Parameter");
                return true;
            }
            if (String.IsNullOrEmpty(ParameterName))
            {
                WarningDialog("กรุณาระยุ ParameterName");
                return true;
            }
            return false;

        }

        void AddParameter()
        {
            if (ValidateRequestResultLink())
            {
                return;
            }

            if (RequestResultLinks != null && RequestResultLinks.Any(p => p.ResultItemUID == SelectedParameterSearch.ResultItemUID))
            {
                WarningDialog("มีข้อมูล Parameter " + SelectedParameterSearch.DisplyName + " อยู่แล้ว");
                return;
            }
            RequestResultLinkModel resultItemLink = new RequestResultLinkModel();
            resultItemLink.IsMandatory = IsCheckMandatory == true ? "Y" : "N";
            resultItemLink.PrintOrder = PrintOrder;
            resultItemLink.ResultItemUID = SelectedParameterSearch.ResultItemUID;
            resultItemLink.ResultItemName = ParameterName;
            resultItemLink.Unit = SelectedParameterSearch.UOM;
            if (RequestResultLinks == null)
            {
                RequestResultLinks = new List<RequestResultLinkModel>();
            }
            RequestResultLinks.Add(resultItemLink);

            (this.View as ManageRequestItem).grdResultItem.RefreshData();
        }

        void UpdateParameter()
        {
            if (SelectRequestResultLink == null)
            {
                WarningDialog("กรุณาเลือกข้อมูลที่ต้องการแก้ไข");
                return;
            }

            if (SelectedParameterSearch != null)
            {
                if (RequestResultLinks != null
    && RequestResultLinks.Where(p => !p.Equals(SelectRequestResultLink)).Any(p => p.ResultItemUID == SelectedParameterSearch.ResultItemUID))
                {
                    WarningDialog("มีข้อมูล Parameter " + SelectedParameterSearch.DisplyName + " อยู่แล้ว");
                    return;
                }
            }

            if (SelectRequestResultLink != null)
            {

                SelectRequestResultLink.ResultItemUID = SelectedParameterSearch.ResultItemUID;
                SelectRequestResultLink.Unit = SelectedParameterSearch.UOM;
                SelectRequestResultLink.IsMandatory = IsCheckMandatory == true ? "Y" : "N";
                SelectRequestResultLink.PrintOrder = PrintOrder;
                SelectRequestResultLink.ResultItemName = ParameterName;
                SelectRequestResultLink.MWhen = DateTime.Now;

                (this.View as ManageRequestItem).grdResultItem.RefreshData();
            }
        }

        void DeleteParameter()
        {
            if (SelectRequestResultLink != null)
            {
                MessageBoxResult result = QuestionDialog("คุณต้องการลบข้อมูลนี้ ใช้ หรือ่ไม่");
                if (result == MessageBoxResult.Yes)
                {
                    RequestResultLinks.Remove(SelectRequestResultLink);
                    (this.View as ManageRequestItem).grdResultItem.RefreshData();
                }
            }
        }

        void AddSpecimen()
        {
            if (SelectSpecimen != null)
            {
                if (RequestItemSpecimen != null && RequestItemSpecimen.Any(p=> p.SpecimenUID == SelectSpecimen.SpecimenUID))
                {
                    WarningDialog("มีข้อมูล Specimen " + SelectSpecimen.Name + " อยู่แล้ว");
                    return;
                }

                if (RequestItemSpecimen != null && RequestItemSpecimen.Any(p => p.IsDefault == "Y" && SpecimenIsDefault))
                {
                    WarningDialog("มี Specimen Default อยู่แล้ว");
                    return;
                }

                RequestItemSpecimenModel newSpecimen = new RequestItemSpecimenModel();

                newSpecimen.SpecimenName = SelectSpecimen.Name;
                newSpecimen.SpecimenUID = SelectSpecimen.SpecimenUID;
                newSpecimen.IsDefault = SpecimenIsDefault == true ? "Y" : "N";
                newSpecimen.VolumeCollected = SelectSpecimen.VolumeCollected != null ? SelectSpecimen.VolumeCollected : (double?)null;
                newSpecimen.SpecimenType = SelectSpecimen.SpecimenType;
                newSpecimen.CollectionSite = SelectSpecimen.CollectionSite;
                newSpecimen.CollectionRoute = SelectSpecimen.CollectionRoute;
                newSpecimen.CollectionMethod = SelectSpecimen.CollectionMethod;
                if (RequestItemSpecimen == null)
                {
                    RequestItemSpecimen = new List<RequestItemSpecimenModel>();
                }

                RequestItemSpecimen.Add(newSpecimen);
                (this.View as ManageRequestItem).grdSpecimen.RefreshData();
                SelectSpecimen = null;
                ClearSpecimenDetail();
            }
        }

        void DeleteSpecimen()
        {
            if (SelectRequestItemSpecimen != null)
            {
                MessageBoxResult result = QuestionDialog("คุณต้องการลบ ใช้ หรือ่ไม่");
                if (result == MessageBoxResult.Yes)
                {
                    RequestItemSpecimen.Remove(SelectRequestItemSpecimen);
                    (this.View as ManageRequestItem).grdSpecimen.RefreshData();
                }
            }
        }

        void ClearSpecimenDetail()
        {
            SpecimenName = string.Empty;
            CollectionMethod = string.Empty;
            SpecimenType = string.Empty;
            CollecitonSite = string.Empty;
            VolumnetoCollected = string.Empty;
            CollectionRoute = string.Empty;
        }

        void AddGroupResult()
        {
            if (RequestItemGroupResults == null)
                RequestItemGroupResults = new List<RequestItemGroupResultModel>();

            if(SelectGroupResult == null)
            {
                WarningDialog("กรุณาเลือก Group Result");
                return;
            }

            if(RequestItemGroupResults.Any(p => p.GPRSTUID == SelectGroupResult.Key))
            {
                WarningDialog("ข้อมูลซ้ำกรุณาตรวจสอบ");
                return;
            }

            RequestItemGroupResultModel newGroupResult = new RequestItemGroupResultModel();
            newGroupResult.GroupResultName = SelectGroupResult.Display;
            newGroupResult.GPRSTUID = SelectGroupResult.Key;
            newGroupResult.PrintOrder = PrintOrderGroupResult;

            RequestItemGroupResults.Add(newGroupResult);
            (this.View as ManageRequestItem).grdGroupResult.RefreshData();
        }

        void DeleteGroupResult()
        {
            if (SelectRequestItemGroupResult != null)
            {
                MessageBoxResult result = QuestionDialog("คุณต้องการลบข้อมูลนี้ ใช้ หรือ่ไม่");
                if (result == MessageBoxResult.Yes)
                {
                    RequestItemGroupResults.Remove(SelectRequestItemGroupResult);
                    (this.View as ManageRequestItem).grdGroupResult.RefreshData();
                }

            }
        }

        private void AssignModelToProperties()
        {
            Code = model.Code;
            Name = model.ItemName;
            Description = model.Description;
            SelectCategory = Category.FirstOrDefault(p => p.Key == model.PRTGPUID);
            SelectTestType = TestType.FirstOrDefault(p => p.Key == model.TSTTPUID);
            SelectImageType = ImageType.FirstOrDefault(p => p.Key == model.RIMTYPUID);
            ActiveFrom = model.EffectiveFrom;
            ActiveTo = model.EffectiveTo;
            RequestResultLinks = model.RequestResultLinks;
            RequestItemSpecimen = model.RequestItemSpecimens;
            RequestItemGroupResults = model.RequestItemGroupResults;
        }
        private void AssingPropertiesToModel()
        {
            if (model == null)
            {
                model = new RequestItemModel();
            }

            model.Code = Code;
            model.ItemName = Name;
            model.Description = Description;
            model.PRTGPUID = SelectCategory.Key;
            model.TSTTPUID = SelectTestType.Key;

            if (ImageTypeVisibility == Visibility.Visible)
            {
                model.RIMTYPUID = SelectImageType != null ? SelectImageType.Key : (int?)null;
            }
            model.EffectiveFrom = ActiveFrom;
            model.EffectiveTo = ActiveTo;
            model.RequestResultLinks = RequestResultLinks;
            model.RequestItemSpecimens = RequestItemSpecimen;
            model.RequestItemGroupResults = RequestItemGroupResults;
        }


        #endregion
    }
}
