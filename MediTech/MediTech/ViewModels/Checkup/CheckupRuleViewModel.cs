using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediTech.ViewModels
{
    public class CheckupRuleViewModel : MediTechViewModelBase
    {
        #region Properties

        private List<LookupReferenceValueModel> _GroupResults;

        public List<LookupReferenceValueModel> GroupResults
        {
            get { return _GroupResults; }
            set { Set(ref _GroupResults, value); }
        }

        private LookupReferenceValueModel _SelectGroupResult;

        public LookupReferenceValueModel SelectGroupResult
        {
            get { return _SelectGroupResult; }
            set
            {
                Set(ref _SelectGroupResult, value);
                if (_SelectGroupResult != null)
                {
                    GetChekcupRule();
                }
                else
                {
                    CheckupRules = null;
                }
            }
        }

        private List<LookupReferenceValueModel> _Genders;

        public List<LookupReferenceValueModel> Genders
        {
            get { return _Genders; }
            set { Set(ref _Genders, value); }
        }

        private LookupReferenceValueModel _SelectGender;

        public LookupReferenceValueModel SelectGender
        {
            get { return _SelectGender; }
            set { Set(ref _SelectGender, value); }
        }


        private List<LookupReferenceValueModel> _ResultStatus;

        public List<LookupReferenceValueModel> ResultStatus
        {
            get { return _ResultStatus; }
            set { Set(ref _ResultStatus, value); }
        }

        private LookupReferenceValueModel _SelectResultStatus;

        public LookupReferenceValueModel SelectResultStatus
        {
            get { return _SelectResultStatus; }
            set { Set(ref _SelectResultStatus, value); }
        }

        private List<ResultItemModel> _ResultItems;

        public List<ResultItemModel> ResultItems
        {
            get { return _ResultItems; }
            set { Set(ref _ResultItems, value); }
        }

        private ResultItemModel _SelectResultItem;

        public ResultItemModel SelectResultItem
        {
            get { return _SelectResultItem; }
            set { Set(ref _SelectResultItem, value); }
        }

        private string _RuleName;

        public string RuleName
        {
            get { return _RuleName; }
            set { Set(ref _RuleName, value); }
        }

        private int? _AgeFrom;

        public int? AgeFrom
        {
            get { return _AgeFrom; }
            set { Set(ref _AgeFrom, value); }
        }

        private int? _AgeTo;

        public int? AgeTo
        {
            get { return _AgeTo; }
            set { Set(ref _AgeTo, value); }
        }


        private List<CheckupRuleModel> _CheckupRules;

        public List<CheckupRuleModel> CheckupRules
        {
            get { return _CheckupRules; }
            set {
                Set(ref _CheckupRules, value);
                if (_CheckupRules == null || _CheckupRules.Count <= 0)
                {
                    CheckupRuleItems = null;
                    CheckupDescriptions = null;
                    CheckupRecommends = null;
                }
            }
        }

        private CheckupRuleModel _SelectCheckupRule;

        public CheckupRuleModel SelectCheckupRule
        {
            get { return _SelectCheckupRule; }
            set
            {
                Set(ref _SelectCheckupRule, value);
                if (_SelectCheckupRule != null)
                {
                    GetCheckupRuleItem();
                    GetCheckupDescription();
                    GetCheckupRecommend();

                    RuleName = SelectCheckupRule.Name;
                    SelectGender = Genders.FirstOrDefault(p => p.Key == SelectCheckupRule.SEXXXUID);
                    AgeFrom = SelectCheckupRule.AgeFrom;
                    AgeTo = SelectCheckupRule.AgeTo;
                    SelectResultStatus = ResultStatus.FirstOrDefault(p => p.Key == SelectCheckupRule.RABSTSUID);

                }
                else
                {
                    CheckupRuleItems = null;
                    CheckupDescriptions = null;
                    CheckupRecommends = null;
                }
            }
        }

        private double? _ValueLow;

        public double? ValueLow
        {
            get { return _ValueLow; }
            set
            {
                Set(ref _ValueLow, value);
                if (_ValueLow != null)
                {
                    TextualValue = null;
                    NotEqual = null;
                    NonCheckup = null;
                }
            }
        }

        private double? _ValueHigh;

        public double? ValueHigh
        {
            get { return _ValueHigh; }
            set
            {
                Set(ref _ValueHigh, value);
                if (_ValueHigh != null)
                {
                    TextualValue = null;
                    NotEqual = null;
                    NonCheckup = null;
                }
            }
        }


        private string _TextualValue;

        public string TextualValue
        {
            get { return _TextualValue; }
            set
            {
                Set(ref _TextualValue, value);
                if (!string.IsNullOrEmpty(_TextualValue))
                {
                    ValueLow = null;
                    ValueHigh = null;
                    NonCheckup = null;
                }

            }
        }

        private bool _OperatorAnd = true;

        public bool OperatorAnd
        {
            get { return _OperatorAnd; }
            set { Set(ref _OperatorAnd, value); }
        }

        private bool _OperatorOr;

        public bool OperatorOr
        {
            get { return _OperatorOr; }
            set { Set(ref _OperatorOr, value); }
        }

        private bool? _NonCheckup;

        public bool? NonCheckup
        {
            get { return _NonCheckup; }
            set { Set(ref _NonCheckup, value);
                if (_NonCheckup != null)
                {
                    ValueLow = null;
                    ValueHigh = null;
                    TextualValue = null;
                    NotEqual = null;
                }
            }
        }

        private bool? _NotEqual;

        public bool? NotEqual
        {
            get { return _NotEqual; }
            set {
                Set(ref _NotEqual, value);
                if (_NotEqual != null && _NotEqual.Value != false)
                {
                    ValueLow = null;
                    ValueHigh = null;
                    NonCheckup = null;
                }
            }
        }



        private List<CheckupRuleItemModel> _CheckupRuleItems;

        public List<CheckupRuleItemModel> CheckupRuleItems
        {
            get { return _CheckupRuleItems; }
            set { Set(ref _CheckupRuleItems, value); }
        }

        private CheckupRuleItemModel _SelectCheckupRuleItem;

        public CheckupRuleItemModel SelectCheckupRuleItem
        {
            get { return _SelectCheckupRuleItem; }
            set {
                Set(ref _SelectCheckupRuleItem, value);
                if (SelectCheckupRuleItem != null)
                {
                    SelectResultItem = ResultItems.FirstOrDefault(p => p.ResultItemUID == SelectCheckupRuleItem.ResultItemUID);
                    ValueLow = SelectCheckupRuleItem.Low;
                    ValueHigh = SelectCheckupRuleItem.Hight;
                    TextualValue = SelectCheckupRuleItem.Text;
                    NotEqual = SelectCheckupRuleItem.NotEqual;
                    NonCheckup = SelectCheckupRuleItem.NonCheckup;
                    if (SelectCheckupRuleItem.Operator == "And")
                    {
                        OperatorAnd = true;
                    }
                    else if(SelectCheckupRuleItem.Operator == "Or")
                    {
                        OperatorOr = true;
                    }

                }
            }
        }

        private ObservableCollection<CheckupTextMasterModel> _CheckupTextMasters;

        public ObservableCollection<CheckupTextMasterModel> CheckupTextMasters
        {
            get { return _CheckupTextMasters; }
            set { Set(ref _CheckupTextMasters, value); }
        }

        private CheckupTextMasterModel _SelectCheckupTextMaster;

        public CheckupTextMasterModel SelectCheckupTextMaster
        {
            get { return _SelectCheckupTextMaster; }
            set { Set(ref _SelectCheckupTextMaster, value); }
        }

        private ObservableCollection<CheckupRuleDescriptionModel> _CheckupDescriptions;

        public ObservableCollection<CheckupRuleDescriptionModel> CheckupDescriptions
        {
            get { return _CheckupDescriptions; }
            set { Set(ref _CheckupDescriptions, value); }
        }

        private CheckupRuleDescriptionModel _SelectCheckupDescription;

        public CheckupRuleDescriptionModel SelectCheckupDescription
        {
            get { return _SelectCheckupDescription; }
            set { Set(ref _SelectCheckupDescription, value); }
        }

        private ObservableCollection<CheckupRuleRecommendModel> _CheckupRecommends;

        public ObservableCollection<CheckupRuleRecommendModel> CheckupRecommends
        {
            get { return _CheckupRecommends; }
            set { Set(ref _CheckupRecommends, value); }
        }

        private CheckupRuleRecommendModel _SelectCheckupRecommend;

        public CheckupRuleRecommendModel SelectCheckupRecommend
        {
            get { return _SelectCheckupRecommend; }
            set { Set(ref _SelectCheckupRecommend, value); }
        }
        #endregion

        #region Command

        private RelayCommand _AddRuleCommmand;

        public RelayCommand AddRuleCommmand
        {
            get
            {
                return _AddRuleCommmand
                    ?? (_AddRuleCommmand = new RelayCommand(AddRule));
            }
        }

        private RelayCommand _EditRuleCommand;

        public RelayCommand EditRuleCommand
        {
            get
            {
                return _EditRuleCommand
                    ?? (_EditRuleCommand = new RelayCommand(EditRule));
            }
        }

        private RelayCommand _DeleteRuleCommand;

        public RelayCommand DeleteRuleCommand
        {
            get
            {
                return _DeleteRuleCommand
                    ?? (_DeleteRuleCommand = new RelayCommand(DeleteRule));
            }
        }

        private RelayCommand _AddRuleItemCommmand;

        public RelayCommand AddRuleItemCommmand
        {
            get
            {
                return _AddRuleItemCommmand
                    ?? (_AddRuleItemCommmand = new RelayCommand(AddRuleItem));
            }
        }

        private RelayCommand _EditRuleItemCommmand;

        public RelayCommand EditRuleItemCommmand
        {
            get
            {
                return _EditRuleItemCommmand
                    ?? (_EditRuleItemCommmand = new RelayCommand(EditRuleItem));
            }
        }


        private RelayCommand _DeleteRuleItemCommand;

        public RelayCommand DeleteRuleItemCommand
        {
            get
            {
                return _DeleteRuleItemCommand
                    ?? (_DeleteRuleItemCommand = new RelayCommand(DeleteRuleItem));
            }
        }


        private RelayCommand _CopyRuleCommmand;

        public RelayCommand CopyRuleCommmand
        {
            get
            {
                return _CopyRuleCommmand
                    ?? (_CopyRuleCommmand = new RelayCommand(CopyRuleItem));
            }
        }
        private RelayCommand<DevExpress.Xpf.Grid.RowEventArgs> _RowTextMasterUpdatedCommand;

        /// <summary>
        /// Gets the RowUpdatedCommand.
        /// </summary>
        public RelayCommand<DevExpress.Xpf.Grid.RowEventArgs> RowTextMasterUpdatedCommand
        {
            get
            {
                return _RowTextMasterUpdatedCommand
                    ?? (_RowTextMasterUpdatedCommand = new RelayCommand<DevExpress.Xpf.Grid.RowEventArgs>(RowTextMasterUpdated));
            }
        }

        private RelayCommand _DeleteTextMasterCommand;

        public RelayCommand DeleteTextMasterCommand
        {
            get
            {
                return _DeleteTextMasterCommand
                    ?? (_DeleteTextMasterCommand = new RelayCommand(DeleteCheckupTextMaster));
            }
        }


        private RelayCommand _AddDescriptionCommand;

        public RelayCommand AddDescriptionCommand
        {
            get
            {
                return _AddDescriptionCommand
                    ?? (_AddDescriptionCommand = new RelayCommand(AddDescription));
            }
        }


        private RelayCommand _AddRecommendCommand;

        public RelayCommand AddRecommendCommand
        {
            get
            {
                return _AddRecommendCommand
                    ?? (_AddRecommendCommand = new RelayCommand(AddRecommend));
            }
        }

        private RelayCommand _DeletetDescriptionCommand;

        public RelayCommand DeletetDescriptionCommand
        {
            get
            {
                return _DeletetDescriptionCommand
                    ?? (_DeletetDescriptionCommand = new RelayCommand(DeletetDescription));
            }
        }

        private RelayCommand _DeletetRecommendCommand;

        public RelayCommand DeletetRecommendCommand
        {
            get
            {
                return _DeletetRecommendCommand
                    ?? (_DeletetRecommendCommand = new RelayCommand(DeletetRecommend));
            }
        }


        #endregion

        #region Method

        public CheckupRuleViewModel()
        {
            CheckupTextMasters = new ObservableCollection<CheckupTextMasterModel>(DataService.Checkup.GetCheckupTextMaster());
            var refValueList = DataService.Technical.GetReferenceValueList("GPRST,SEXXX,RABSTS");
            GroupResults = refValueList.Where(p => p.DomainCode == "GPRST").ToList();
            Genders = refValueList.Where(p => p.DomainCode == "SEXXX").ToList();
            ResultStatus = refValueList.Where(p => p.DomainCode == "RABSTS").ToList();
            ResultItems = DataService.MasterData.GetResultItems();

        }

        public override void OnLoaded()
        {
            (this.View as CheckupRule).gvGroupResult.BestFitColumns();
        }

        void GetChekcupRule()
        {
            CheckupRules = DataService.Checkup.GetCheckupRuleByGroup(SelectGroupResult.Key.Value);
        }

        void GetCheckupRuleItem()
        {
            CheckupRuleItems = DataService.Checkup.GetCheckupRuleItemByRuleUID(SelectCheckupRule.CheckupRuleUID);
        }

        void GetCheckupDescription()
        {
            CheckupDescriptions = new ObservableCollection<CheckupRuleDescriptionModel>(DataService.Checkup.GetCheckupRuleDescriptionByRuleUID(SelectCheckupRule.CheckupRuleUID));
        }

        void GetCheckupRecommend()
        {
            CheckupRecommends = new ObservableCollection<CheckupRuleRecommendModel>(DataService.Checkup.GetCheckupRuleRecommendModelByRuleUID(SelectCheckupRule.CheckupRuleUID));
        }

        void AddRule()
        {
            try
            {
                if (string.IsNullOrEmpty(RuleName))
                {
                    WarningDialog("กรุณาระบุ ชื่อ");
                    return;
                }
                if (SelectGender == null)
                {
                    WarningDialog("กรุณาระบุ เพศ");
                    return;
                }
                if (SelectResultStatus == null)
                {
                    WarningDialog("กรุณาระบุ สถานะ");
                    return;
                }

                CheckupRuleModel newRule = new CheckupRuleModel();
                newRule.Name = RuleName;
                newRule.RABSTSUID = SelectResultStatus.Key.Value;
                newRule.SEXXXUID = SelectGender.Key;
                newRule.GPRSTUID = SelectGroupResult.Key.Value;
                newRule.AgeFrom = AgeFrom;
                newRule.AgeTo = AgeTo;
                DataService.Checkup.SaveCheckupRule(newRule, AppUtil.Current.UserID);
                GetChekcupRule();
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        void EditRule()
        {
            try
            {
                if (string.IsNullOrEmpty(RuleName))
                {
                    WarningDialog("กรุณาระบุ ชื่อ");
                    return;
                }

                if (SelectGender == null)
                {
                    WarningDialog("กรุณาระบุ เพศ");
                    return;
                }
                if (SelectResultStatus == null)
                {
                    WarningDialog("กรุณาระบุ สถานะ");
                    return;
                }

                if (SelectCheckupRule != null)
                {
                    CheckupRuleModel newRule = new CheckupRuleModel();
                    newRule.CheckupRuleUID = SelectCheckupRule.CheckupRuleUID;
                    newRule.Name = RuleName;
                    newRule.RABSTSUID = SelectResultStatus.Key.Value;
                    newRule.SEXXXUID = SelectGender.Key;
                    newRule.GPRSTUID = SelectGroupResult.Key.Value;
                    newRule.AgeFrom = AgeFrom;
                    newRule.AgeTo = AgeTo;
                    DataService.Checkup.SaveCheckupRule(newRule, AppUtil.Current.UserID);
                    GetChekcupRule();
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        void DeleteRule()
        {
            try
            {
                if (SelectCheckupRule != null)
                {
                    MessageBoxResult diagResult = DeleteDialog();
                    if (diagResult == MessageBoxResult.Yes)
                    {
                        DataService.Checkup.DeleteCheckupRule(SelectCheckupRule.CheckupRuleUID, AppUtil.Current.UserID);
                        GetChekcupRule();
                    }
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        void AddRuleItem()
        {
            try
            {
                if (SelectCheckupRule == null)
                {
                    WarningDialog("กรุณาเลือก Rule ที่จะเพิ่มสูตร");
                    return;
                }
                if (SelectResultItem == null)
                {
                    WarningDialog("กรุณาระบุ รายการ");
                    return;
                }
                if (string.IsNullOrEmpty(TextualValue) && ValueLow == null && ValueHigh == null && (NonCheckup ?? false) == false)
                {
                    WarningDialog("กรุณาระบุ ตัวเลข ข้อความ หรือ ไม่ได้ตรวจ อย่างใดอย่างหนึ่ง");
                    return;
                }

                CheckupRuleItemModel newRuleItem = new CheckupRuleItemModel();
                newRuleItem.CheckupRuleUID = SelectCheckupRule.CheckupRuleUID;
                newRuleItem.ResultItemUID = SelectResultItem.ResultItemUID;
                newRuleItem.ResultItemName = SelectResultItem.DisplyName;
                newRuleItem.Low = ValueLow;
                newRuleItem.Hight = ValueHigh;
                newRuleItem.Text = TextualValue;
                newRuleItem.NotEqual = NotEqual;
                newRuleItem.NonCheckup = NonCheckup;
                string Operator = "";
                if (OperatorAnd)
                {
                    Operator = "And";
                }
                else if(OperatorOr)
                {
                    Operator = "Or";
                }
                newRuleItem.Operator = Operator;
                DataService.Checkup.SaveCheckupRuleItem(newRuleItem, AppUtil.Current.UserID);
                GetCheckupRuleItem();
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        void EditRuleItem()
        {
            try
            {
                if (SelectCheckupRuleItem != null)
                {
                    if (SelectCheckupRule == null)
                    {
                        WarningDialog("กรุณาเลือก Rule ที่จะเพิ่มสูตร");
                        return;
                    }
                    if (SelectResultItem == null)
                    {
                        WarningDialog("กรุณาระบุ รายการ");
                        return;
                    }
                    if (string.IsNullOrEmpty(TextualValue) && ValueLow == null && ValueHigh == null && (NonCheckup ?? false) == false)
                    {
                        WarningDialog("กรุณาระบุ ตัวเลข ข้อความ หรือ ไม่ได้ตรวจ อย่างใดอย่างหนึ่ง");
                        return;
                    }

                    CheckupRuleItemModel newRuleItem = new CheckupRuleItemModel();
                    newRuleItem.CheckupRuleItemUID = SelectCheckupRuleItem.CheckupRuleItemUID;
                    newRuleItem.CheckupRuleUID = SelectCheckupRule.CheckupRuleUID;
                    newRuleItem.ResultItemUID = SelectResultItem.ResultItemUID;
                    newRuleItem.ResultItemName = SelectResultItem.DisplyName;
                    newRuleItem.Low = ValueLow;
                    newRuleItem.Hight = ValueHigh;
                    newRuleItem.Text = TextualValue;
                    newRuleItem.NotEqual = NotEqual;
                    newRuleItem.NonCheckup = NonCheckup;
                    string Operator = "";
                    if (OperatorAnd)
                    {
                        Operator = "And";
                    }
                    else if (OperatorOr)
                    {
                        Operator = "Or";
                    }
                    newRuleItem.Operator = Operator;
                    DataService.Checkup.SaveCheckupRuleItem(newRuleItem, AppUtil.Current.UserID);
                    GetCheckupRuleItem();
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }
        void DeleteRuleItem()
        {
            try
            {
                if (SelectCheckupRuleItem != null)
                {
                    MessageBoxResult diagResult = DeleteDialog();
                    if (diagResult == MessageBoxResult.Yes)
                    {
                        DataService.Checkup.DeleteCheckupRuleItem(SelectCheckupRuleItem.CheckupRuleItemUID, AppUtil.Current.UserID);
                        GetCheckupRuleItem();
                    }
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        void CopyRuleItem()
        {
            if (SelectCheckupRule != null)
            {
                DataService.Checkup.CopyCheckupRule(SelectCheckupRule, AppUtil.Current.UserID);
                GetChekcupRule();
                //(this.View as CheckupRule).grdCheckupRule.RefreshData();
                SelectCheckupRule = CheckupRules[CheckupRules.Count - 1];
            }
        }

        private void RowTextMasterUpdated(DevExpress.Xpf.Grid.RowEventArgs e)
        {
            try
            {
                if (e.Row is CheckupTextMasterModel)
                {
                    int userUID = AppUtil.Current.UserID;
                    CheckupTextMasterModel rowItem = (CheckupTextMasterModel)e.Row;
                    rowItem.CUser = userUID;
                    rowItem.MUser = userUID;
                    var returnData = DataService.Checkup.SaveCheckupTextMaster(rowItem);
                    (e.Row as CheckupTextMasterModel).CheckupTextMasterUID = returnData.CheckupTextMasterUID;
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void DeleteCheckupTextMaster()
        {
            try
            {
                if (SelectCheckupTextMaster != null)
                {
                    MessageBoxResult result = DeleteDialog();
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.Checkup.DeleteCheckupTextMaster(SelectCheckupTextMaster.CheckupTextMasterUID, AppUtil.Current.UserID);
                        CheckupTextMasters.Remove(SelectCheckupTextMaster);
                    }

                }

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }


        void AddDescription()
        {
            try
            {
                if (SelectCheckupTextMaster == null)
                {
                    WarningDialog("กรุณาเลือก ข้อความที่จะเพิ่ม");
                    return;
                }

                if (CheckupDescriptions != null && CheckupDescriptions.Any(p => p.CheckupTextMasterUID == SelectCheckupTextMaster.CheckupTextMasterUID))
                {
                    WarningDialog("มีข้อความนี้อยู่แล้ว");
                    return;
                }



                CheckupRuleDescriptionModel newRuleDescription = new CheckupRuleDescriptionModel();
                newRuleDescription.CheckupRuleUID = SelectCheckupRule.CheckupRuleUID;
                newRuleDescription.CheckupTextMasterUID = SelectCheckupTextMaster.CheckupTextMasterUID;
                DataService.Checkup.AddCheckupRuleDescription(newRuleDescription, AppUtil.Current.UserID);
                GetCheckupDescription();
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        void AddRecommend()
        {
            try
            {
                if (SelectCheckupTextMaster == null)
                {
                    WarningDialog("กรุณาเลือก คำที่จะเพิ่ม");
                    return;
                }

                if (CheckupRecommends != null && CheckupRecommends.Any(p => p.CheckupTextMasterUID == SelectCheckupTextMaster.CheckupTextMasterUID))
                {
                    WarningDialog("มีข้อความนี้อยู่แล้ว");
                    return;
                }

                CheckupRuleRecommendModel newRuleRecommend = new CheckupRuleRecommendModel();
                newRuleRecommend.CheckupRuleUID = SelectCheckupRule.CheckupRuleUID;
                newRuleRecommend.CheckupTextMasterUID = SelectCheckupTextMaster.CheckupTextMasterUID;
                DataService.Checkup.AddCheckupRuleRecommend(newRuleRecommend, AppUtil.Current.UserID);
                GetCheckupRecommend();
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        void DeletetDescription()
        {
            try
            {
                if (SelectCheckupDescription != null)
                {
                    MessageBoxResult result = DeleteDialog();
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.Checkup.DeleteCheckupRuleDescription(SelectCheckupDescription.CheckupRuleDescriptionUID, AppUtil.Current.UserID);
                        CheckupDescriptions.Remove(SelectCheckupDescription);
                    }

                }

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        void DeletetRecommend()
        {
            try
            {
                if (SelectCheckupRecommend != null)
                {
                    MessageBoxResult result = DeleteDialog();
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.Checkup.DeleteCheckupRuleRecommend(SelectCheckupRecommend.CheckupRuleRecommendUID, AppUtil.Current.UserID);
                        CheckupRecommends.Remove(SelectCheckupRecommend);
                    }

                }

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }
        #endregion
    }
}
