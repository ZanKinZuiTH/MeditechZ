using GalaSoft.MvvmLight.Command;
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
    public class ManageTestParameterViewModel : MediTechViewModelBase
    {
        #region Properties

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

        private List<LookupReferenceValueModel> _ParameterType;

        public List<LookupReferenceValueModel> ParameterType
        {
            get { return _ParameterType; }
            set
            {
                Set(ref _ParameterType, value);
            }
        }

        private List<LookupReferenceValueModel> _UnitofMeasure;

        public List<LookupReferenceValueModel> UnitofMeasure
        {
            get { return _UnitofMeasure; }
            set
            {
                Set(ref _UnitofMeasure, value);
            }
        }

        private LookupReferenceValueModel _SelectParameterType;

        public LookupReferenceValueModel SelectParameterType
        {
            get { return _SelectParameterType; }
            set
            {
                Set(ref _SelectParameterType, value);
                if (_SelectParameterType != null)
                {
                    if (_SelectParameterType.Display == "Numeric" || _SelectParameterType.Display == "Free Text Field")
                    {
                        VisibilityValueRange = Visibility.Visible;
                    }
                    else
                    {
                        VisibilityValueRange = Visibility.Hidden;
                    }
                }
            }
        }

        private LookupReferenceValueModel _SelectUnitofMeasure;

        public LookupReferenceValueModel SelectUnitofMeasure
        {
            get { return _SelectUnitofMeasure; }
            set
            {
                Set(ref _SelectUnitofMeasure, value);
            }
        }

        private DateTime? _EffectiveFrom;

        public DateTime? EffectiveFrom
        {
            get { return _EffectiveFrom; }
            set { Set(ref _EffectiveFrom, value); }
        }


        private DateTime? _EffectiveTo;

        public DateTime? EffectiveTo
        {
            get { return _EffectiveTo; }
            set { Set(ref _EffectiveTo, value); }
        }


        private bool __IsCumulative;

        public bool IsCumulative
        {
            get { return __IsCumulative; }
            set { Set(ref __IsCumulative, value); }
        }


        private bool _IsEnabledEdit = true;

        public bool IsEnabledEdit
        {
            get { return _IsEnabledEdit; }
            set { Set(ref _IsEnabledEdit, value); }
        }

        private bool IsGenerateCode = false;

        public bool _IsGenerateCode
        {
            get { return _IsGenerateCode; }
            set { _IsGenerateCode = value; }
        }

        private Visibility _VisibilityValueRange = Visibility.Hidden;

        public Visibility VisibilityValueRange
        {
            get { return _VisibilityValueRange; }
            set { Set(ref _VisibilityValueRange, value); }
        }

        private int _SelectTabIndex;

        public int SelectTabIndex
        {
            get { return _SelectTabIndex; }
            set { Set(ref _SelectTabIndex, value); }
        }


        #region ResultItemRange

        private List<ResultItemRangeModel> _ListResultItemRange;

        public List<ResultItemRangeModel> ListResultItemRange
        {
            get { return _ListResultItemRange; }
            set { Set(ref _ListResultItemRange, value); }
        }

        private ResultItemRangeModel _SelectResultItemRange;

        public ResultItemRangeModel SelectResultItemRange
        {
            get { return _SelectResultItemRange; }
            set
            {
                Set(ref _SelectResultItemRange, value);
                if (SelectResultItemRange != null)
                {
                    BidingResultItemRange();

                }

            }
        }


        private List<LookupReferenceValueModel> _LabRangeMasters;

        public List<LookupReferenceValueModel> LabRangeMasters
        {
            get { return _LabRangeMasters; }
            set
            {
                Set(ref _LabRangeMasters, value);
            }
        }

        private List<LookupReferenceValueModel> _Genders;

        public List<LookupReferenceValueModel> Genders
        {
            get { return _Genders; }
            set
            {
                Set(ref _Genders, value);
            }
        }

        private LookupReferenceValueModel _SelectLabRangeMasters;

        public LookupReferenceValueModel SelectLabRangeMasters
        {
            get { return _SelectLabRangeMasters; }
            set
            {
                Set(ref _SelectLabRangeMasters, value);
            }
        }

        private LookupReferenceValueModel _SelectGender;

        public LookupReferenceValueModel SelectGender
        {
            get { return _SelectGender; }
            set
            {
                Set(ref _SelectGender, value);
            }
        }

        private string _DisplayValue;

        public string DisplayValue
        {
            get { return _DisplayValue; }
            set { Set(ref _DisplayValue, value); }
        }


        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { Set(ref _Comments, value); }
        }


        private double? _Low;

        public double? Low
        {
            get { return _Low; }
            set { Set(ref _Low, value); }
        }


        private double? _High;

        public double? High
        {
            get { return _High; }
            set { Set(ref _High, value); }
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


        private RelayCommand _GenerateCodeCommand;
        public RelayCommand GenerateCodeCommand
        {
            get { return _GenerateCodeCommand ?? (_GenerateCodeCommand = new RelayCommand(GenerateCode)); }
        }

        #region ResultItemRange
        private RelayCommand _AddRangeCommand;
        public RelayCommand AddRangeCommand
        {
            get { return _AddRangeCommand ?? (_AddRangeCommand = new RelayCommand(AddRange)); }
        }
        private RelayCommand _UpdateRangeCommand;
        public RelayCommand UpdateRangeCommand
        {
            get { return _UpdateRangeCommand ?? (_UpdateRangeCommand = new RelayCommand(UpdateRange)); }
        }

        private RelayCommand _DeleteRangeCommand;
        public RelayCommand DeleteRangeCommand
        {
            get { return _DeleteRangeCommand ?? (_DeleteRangeCommand = new RelayCommand(DeleteRange)); }
        }
        #endregion

        #endregion


        #region Method

        ResultItemModel Datamodel;

        public ManageTestParameterViewModel()
        {
            var referenceValue = DataService.Technical.GetReferenceValueList("RVTYP,RIUOM,SEXXX,LABRAM");
            ParameterType = referenceValue.Where(p => p.DomainCode == "RVTYP").ToList();
            UnitofMeasure = referenceValue.Where(p => p.DomainCode == "RIUOM").OrderBy(p => p.Display).ToList();

            LabRangeMasters = referenceValue.Where(p => p.DomainCode == "LABRAM").ToList();
            Genders = referenceValue.Where(p => p.DomainCode == "SEXXX").ToList();
        }


        void AddRange()
        {
            if (ValidateResultItemRange())
            {
                return;
            }

            if (ListResultItemRange != null && ListResultItemRange.Any(p => p.LABRAMUID == SelectLabRangeMasters.Key && p.SEXXXUID == SelectGender.Key))
            {
                WarningDialog("มีข้อมูล Range " + SelectLabRangeMasters.Display + " อยู่แล้ว");
                return;
            }
            ResultItemRangeModel resulItemRangeModel = new ResultItemRangeModel();
            resulItemRangeModel.Comments = Comments;
            resulItemRangeModel.DisplayValue = DisplayValue;
            resulItemRangeModel.Low = Low;
            resulItemRangeModel.High = High;
            resulItemRangeModel.LABRAMUID = SelectLabRangeMasters.Key;
            resulItemRangeModel.LabRangeMaster = SelectLabRangeMasters.Display;
            resulItemRangeModel.SEXXXUID = SelectGender.Key;
            resulItemRangeModel.Gender = SelectGender.Display;
            if (ListResultItemRange == null)
            {
                ListResultItemRange = new List<ResultItemRangeModel>();
            }
            ListResultItemRange.Add(resulItemRangeModel);

            (this.View as ManageTestParameter).grdResultItemRange.RefreshData();
        }
        void UpdateRange()
        {
            if (SelectResultItemRange == null)
            {
                WarningDialog("กรุณาเลือก Range ที่ต้องการแก้ไข");
                return;
            }
            if (ValidateResultItemRange())
            {
                return;
            }
            if (ListResultItemRange != null
                && ListResultItemRange.Where( p => !p.Equals(SelectResultItemRange)).Any(p => p.LABRAMUID == SelectLabRangeMasters.Key && p.SEXXXUID == SelectGender.Key))
            {
                WarningDialog("มีข้อมูล Range " + SelectLabRangeMasters.Display + " อยู่แล้ว");
                return;
            }
            if (SelectResultItemRange != null)
            {
                SelectResultItemRange.Comments = Comments;
                SelectResultItemRange.DisplayValue = DisplayValue;
                SelectResultItemRange.Low = Low;
                SelectResultItemRange.High = High;
                SelectResultItemRange.LABRAMUID = SelectLabRangeMasters.Key;
                SelectResultItemRange.LabRangeMaster = SelectLabRangeMasters.Display;
                SelectResultItemRange.SEXXXUID = SelectGender.Key;
                SelectResultItemRange.Gender = SelectGender.Display;

                (this.View as ManageTestParameter).grdResultItemRange.RefreshData();
            }
        }

        void DeleteRange()
        {
            if (SelectResultItemRange != null)
            {
                MessageBoxResult result = QuestionDialog("คุณต้องการลบ ใช้ หรือ่ไม่");
                if (result == MessageBoxResult.Yes)
                {
                    ListResultItemRange.Remove(SelectResultItemRange);
                    (this.View as ManageTestParameter).grdResultItemRange.RefreshData();
                }
            }
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
                if (string.IsNullOrEmpty(Description))
                {
                    WarningDialog("กรุณาใส่ Print As");
                    return;
                }
                if (SelectParameterType == null)
                {
                    WarningDialog("กรุณาเลือก ประเภท");
                    return;
                }

                if (Datamodel == null)
                {
                    var dupicateCode = DataService.MasterData.GetResultItemByCode(Code);
                    if (dupicateCode != null)
                    {
                        WarningDialog("Code ซ้ำ โปรดตรวจสอบ");
                        return;
                    }
                }

                if (SelectTabIndex == 1)
                {
                    if (ValidateResultItemRange())
                    {
                        return;
                    }
                }

                AssignPropertiesToModel();
                DataService.MasterData.ManageResultItem(Datamodel, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ListTestParameter pageList = new ListTestParameter();
                ChangeViewPermission(pageList);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        bool ValidateResultItemRange()
        {
            if (SelectLabRangeMasters == null)
            {
                WarningDialog("กรุณาเลือก Lab ange Master");
                return true;
            }
            if (SelectGender == null)
            {
                WarningDialog("กรุณาเลือก Gender");
                return true;
            }
            //if (Low == null)
            //{
            //    WarningDialog("กรุณาระบุ Low");
            //    return true;
            //}
            //if (High == null)
            //{
            //    WarningDialog("กรุณาระบุ High");
            //    return true;
            //}
            return false;

        }

        private void Cancel()
        {
            ListTestParameter pageList = new ListTestParameter();
            ChangeViewPermission(pageList);
        }

        private void GenerateCode()
        {
            try
            {
                if (IsGenerateCode == false && IsEnabledEdit)
                {
                    string ItemCode = DataService.MasterData.GenerateResultItemCode();
                    Code = ItemCode;
                    IsGenerateCode = true;
                }

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        public void EditData(int resultItemUID)
        {
            Datamodel = DataService.MasterData.GetResultItemUID(resultItemUID);
            BidingData();
        }

        private void BidingResultItemRange()
        {
            SelectLabRangeMasters = LabRangeMasters.FirstOrDefault(p => p.Key == SelectResultItemRange.LABRAMUID);
            SelectGender = Genders.FirstOrDefault(p => p.Key == SelectResultItemRange.SEXXXUID);
            DisplayValue = SelectResultItemRange.DisplayValue;
            Comments = SelectResultItemRange.Comments;
            Low = SelectResultItemRange.Low;
            High = SelectResultItemRange.High;

        }

        private void BidingData()
        {
            Code = Datamodel.Code;
            Name = Datamodel.DisplyName;
            Description = Datamodel.Description;
            SelectParameterType = ParameterType.FirstOrDefault(p => p.Key == Datamodel.RVTYPUID);
            SelectUnitofMeasure = UnitofMeasure.FirstOrDefault(p => p.Key == Datamodel.UnitofMeasure);
            EffectiveFrom = Datamodel.EffectiveFrom;
            EffectiveTo = Datamodel.EffectiveTo;
            IsCumulative = Datamodel.IsCumulative == "Y" ? true : false;

            ListResultItemRange = Datamodel.ResultItemRanges;

            IsEnabledEdit = false;
        }
        private void AssignPropertiesToModel()
        {
            if (Datamodel == null)
            {
                Datamodel = new ResultItemModel();
            }

            Datamodel.Code = Code;
            Datamodel.DisplyName = Name;
            Datamodel.Description = Description;
            Datamodel.RVTYPUID = SelectParameterType.Key;
            Datamodel.UnitofMeasure = SelectUnitofMeasure != null ? SelectUnitofMeasure.Key : (int?)null;
            Datamodel.IsCumulative = IsCumulative ? "Y" : null;
            Datamodel.EffectiveFrom = EffectiveFrom;
            Datamodel.EffectiveTo = EffectiveTo;

            if (SelectParameterType.Display == "Numeric" || SelectParameterType.Display == "Free Text Field")
            {
                if (ListResultItemRange != null)
                {
                    Datamodel.ResultItemRanges = new List<ResultItemRangeModel>();
                    Datamodel.ResultItemRanges.AddRange(ListResultItemRange);
                }

            }
        }

        #endregion
    }
}
