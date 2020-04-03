using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MediTech.Model;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using MediTech.DataService;
using MediTech.Views;
using System.Windows.Forms;
using System.Windows.Media;

namespace MediTech.ViewModels
{
    public class PatientAllergyViewModel : MediTechViewModelBase
    {

        #region Properties

        private PatientVisitModel _SelectPatientVisit;

        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set { Set(ref _SelectPatientVisit, value); }
        }

        private List<DrugGenericModel> _GenericSearchSource;

        public List<DrugGenericModel> GenericSearchSource
        {
            get { return _GenericSearchSource; }
            set { Set(ref _GenericSearchSource, value); }
        }

        private DrugGenericModel _SelectGenericSearch;

        public DrugGenericModel SelectGenericSearch
        {
            get { return _SelectGenericSearch; }
            set { Set(ref _SelectGenericSearch, value); }
        }

        private List<ItemMasterModel> _DrugSearchSource;

        public List<ItemMasterModel> DrugSearchSource
        {
            get { return _DrugSearchSource; }
            set { Set(ref _DrugSearchSource, value); }
        }

        private ItemMasterModel _SelectDrugSearch;

        public ItemMasterModel SelectDrugSearch
        {
            get { return _SelectDrugSearch; }
            set { Set(ref _SelectDrugSearch, value); }
        }

        public bool SuppressGenericEvent { get; set; }

        private string _SearchGenericCriteria;

        public string SearchGenericCriteria
        {
            get { return _SearchGenericCriteria; }
            set
            {
                Set(ref _SearchGenericCriteria, value);
                if (!SuppressGenericEvent)
                {
                    if (!string.IsNullOrEmpty(_SearchGenericCriteria) && _SearchGenericCriteria.Length >= 3)
                    {
                        GenericSearchSource = DataService.Pharmacy.GetDrugGenericCriteria(SearchGenericCriteria);
                    }
                    else
                    {
                        GenericSearchSource = null;
                    }
                }

            }
        }

        public bool SuppressDrugEvent { get; set; }

        private string _SearchDrugCriteria;

        public string SearchDrugCriteria
        {
            get { return _SearchDrugCriteria; }
            set
            {
                Set(ref _SearchDrugCriteria, value);
                if (!SuppressDrugEvent)
                {
                    if (!string.IsNullOrEmpty(_SearchDrugCriteria) && _SearchDrugCriteria.Length >= 3)
                    {
                        DrugSearchSource = DataService.Pharmacy.GetDrugCriteria(SearchDrugCriteria);
                    }
                    else
                    {
                        DrugSearchSource = null;
                    }
                }

            }
        }

        private string _FreeText;

        public string FreeText
        {
            get { return _FreeText; }
            set { Set(ref _FreeText, value); }
        }



        public string IdentifyingType { get; set; }

        private List<LookupReferenceValueModel> _AllergyClass;

        public List<LookupReferenceValueModel> AllergyClass
        {
            get { return _AllergyClass; }
            set { Set(ref _AllergyClass, value); }
        }


        private Visibility _VisibilityGeneric = Visibility.Collapsed;

        public Visibility VisibilityGeneric
        {
            get { return _VisibilityGeneric; }
            set { Set(ref _VisibilityGeneric, value); }
        }


        private Visibility _VisibilityDrug = Visibility.Collapsed;

        public Visibility VisibilityDrug
        {
            get { return _VisibilityDrug; }
            set { Set(ref _VisibilityDrug, value); }
        }

        private Visibility _VisibilityOther = Visibility.Collapsed;

        public Visibility VisibilityOther
        {
            get { return _VisibilityOther; }
            set { Set(ref _VisibilityOther, value); }
        }

        private LookupReferenceValueModel _SelectAllergyClass;

        public LookupReferenceValueModel SelectAllergyClass
        {
            get { return _SelectAllergyClass; }
            set
            {
                Set(ref _SelectAllergyClass, value);
                if (_SelectAllergyClass != null)
                {
                    if (SelectAllergyClass.Display == "ยา")
                    {
                        VisibilityDrug = Visibility.Visible;
                        VisibilityGeneric = Visibility.Visible;
                        VisibilityOther = Visibility.Visible;
                    }
                    else
                    {
                        VisibilityDrug = Visibility.Collapsed;
                        VisibilityGeneric = Visibility.Collapsed;
                        VisibilityOther = Visibility.Visible;
                    }
                }
                else
                {
                    VisibilityDrug = Visibility.Collapsed;
                    VisibilityGeneric = Visibility.Collapsed;
                    VisibilityOther = Visibility.Collapsed;
                }
            }
        }

        private List<LookupReferenceValueModel> _Severity;

        public List<LookupReferenceValueModel> Severity
        {
            get { return _Severity; }
            set { Set(ref _Severity, value); }
        }

        private LookupReferenceValueModel _SelectSeverity;

        public LookupReferenceValueModel SelectSeverity
        {
            get { return _SelectSeverity; }
            set { Set(ref _SelectSeverity, value); }
        }

        private List<LookupReferenceValueModel> _Accuracy;

        public List<LookupReferenceValueModel> Accuracy
        {
            get { return _Accuracy; }
            set { Set(ref _Accuracy, value); }
        }


        private LookupReferenceValueModel _SelectAccuracy;

        public LookupReferenceValueModel SelectAccuracy
        {
            get { return _SelectAccuracy; }
            set { Set(ref _SelectAccuracy, value); }
        }

        private string _AllergyDescription;

        public string AllergyDescription
        {
            get { return _AllergyDescription; }
            set { Set(ref _AllergyDescription, value); }
        }


        private bool _IsEnableDrugGeneric = false;

        public bool IsEnableDrugGeneric
        {
            get { return _IsEnableDrugGeneric; }
            set { Set(ref _IsEnableDrugGeneric, value); }
        }


        private bool _IsEnableDrug = false;

        public bool IsEnableDrug
        {
            get { return _IsEnableDrug; }
            set { Set(ref _IsEnableDrug, value); }
        }

        private bool _IsEnableOther = false;

        public bool IsEnableOther
        {
            get { return _IsEnableOther; }
            set
            {
                Set(ref _IsEnableOther, value);
                if (_IsEnableOther)
                {
                    Background = Brushes.White;
                }
                else
                {
                    Background = Brushes.Gainsboro;
                }
            }
        }

        private Brush _Background = Brushes.Gainsboro;

        public Brush Background
        {
            get { return _Background; }
            set { Set(ref _Background, value); }
        }

        private bool _IsCheckGeneric;

        public bool IsCheckGeneric
        {
            get { return _IsCheckGeneric; }
            set { Set(ref _IsCheckGeneric, value); }
        }

        private bool _IsCheckDrug;

        public bool IsCheckDrug
        {
            get { return _IsCheckDrug; }
            set { Set(ref _IsCheckDrug, value); }
        }

        private bool _IsCheckOther;

        public bool IsCheckOther
        {
            get { return _IsCheckOther; }
            set { Set(ref _IsCheckOther, value); }
        }


        private ObservableCollection<PatientAllergyModel> _PatientAllergyList;

        public ObservableCollection<PatientAllergyModel> PatientAllergyList
        {
            get { return _PatientAllergyList; }
            set { Set(ref _PatientAllergyList, value); }
        }

        private PatientAllergyModel _SelectPatientAllergy;

        public PatientAllergyModel SelectPatientAllergy
        {
            get { return _SelectPatientAllergy; }
            set
            {
                Set(ref _SelectPatientAllergy, value);
                if (_SelectPatientAllergy != null)
                {
                    SelectAllergyClass = AllergyClass.FirstOrDefault(p => p.Key == SelectPatientAllergy.ALRCLUID);
                    SelectAccuracy = Accuracy.FirstOrDefault(p => p.Key == SelectPatientAllergy.CERNTUID);
                    SelectSeverity = Severity.FirstOrDefault(p => p.Key == SelectPatientAllergy.SEVRTUID);
                    AllergyDescription = SelectPatientAllergy.AllergyDescription;

                    SelectType(SelectPatientAllergy.IdentifyingType);
                    switch (SelectPatientAllergy.IdentifyingType)
                    {
                        case "ชื่อยาสามัญ":

                            SearchGenericCriteria = SelectPatientAllergy.AllergicTo;

                            SelectGenericSearch = new DrugGenericModel { DrugGenericUID = SelectPatientAllergy.IdentifyingUID.Value,Name = SelectPatientAllergy.AllergicTo };
                            SearchDrugCriteria = "";
                            FreeText = "";
                            break;
                        case "ยา":

                            SearchGenericCriteria = "";
                            SearchDrugCriteria = SelectPatientAllergy.AllergicTo;
                            SelectDrugSearch = new ItemMasterModel { ItemMasterUID = SelectPatientAllergy.IdentifyingUID.Value, Name = SelectPatientAllergy.AllergicTo };
                            FreeText = "";
                            break;
                        case "อื่นๆ":

                            SearchGenericCriteria = "";
                            SearchDrugCriteria = "";
                            FreeText = SelectPatientAllergy.AllergicTo;
                            break;
                    }
                }
            }
        }


        #endregion

        #region Command

        private RelayCommand _AddCommand;

        public RelayCommand AddCommand
        {
            get { return _AddCommand ?? (_AddCommand = new RelayCommand(Add)); }
        }

        private RelayCommand _EditCommand;

        public RelayCommand EditCommand
        {
            get { return _EditCommand ?? (_EditCommand = new RelayCommand(Edit)); }
        }


        private RelayCommand _DeleteCommand;

        public RelayCommand DeleteCommand
        {
            get { return _DeleteCommand ?? (_DeleteCommand = new RelayCommand(Delete)); }
        }

        private RelayCommand<object> _SelectTypeCommand;

        public RelayCommand<object> SelectTypeCommand
        {
            get { return _SelectTypeCommand ?? (_SelectTypeCommand = new RelayCommand<object>(SelectType)); }
        }


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



        #endregion

        #region Method

        #region Varibale

        List<PatientAllergyModel> model;

        #endregion

        public PatientAllergyViewModel()
        {
            var refData = DataService.Technical.GetReferenceValueList("ALRCL,SEVRT,CERNT");
            AllergyClass = refData.Where(p => p.DomainCode == "ALRCL").ToList();
            Severity = refData.Where(p => p.DomainCode == "SEVRT").ToList();
            Accuracy = refData.Where(p => p.DomainCode == "CERNT").ToList();
            SelectAllergyClass = AllergyClass.FirstOrDefault();

        }

        public bool Validate()
        {
            if (SelectAllergyClass == null)
            {
                WarningDialog("กรุณาเลือกประเภท");
                return false;
            }
            if (IsCheckDrug == false && IsCheckGeneric == false && IsCheckOther == false)
            {
                WarningDialog("กรุณาระบุสิ่งที่แพ้");
                return false;
            }
            if (SelectSeverity == null)
            {
                WarningDialog("กรุณาเลือกความรุนแรง");
                return false;
            }

            if (IsCheckGeneric)
            {
                if (string.IsNullOrEmpty(SearchGenericCriteria))
                {
                    WarningDialog("กรุณาระบุชื่อยาสามัญ");
                    return false;
                }
                if (SelectGenericSearch == null)
                {
                    WarningDialog("กรุณาเลือกชื่อสามัญ ให้ตรงกับในระบบ");
                    return false;
                }
            }

            if (IsCheckDrug)
            {
                if (string.IsNullOrEmpty(SearchDrugCriteria))
                {
                    WarningDialog("กรุณาระบุชื่อยา");
                    return false;
                }
                if (SelectDrugSearch == null)
                {
                    WarningDialog("กรุณาเลือกชื่อยา ให้ตรงกับในระบบ");
                    return false;
                }
            }

            if (IsCheckOther)
            {
                if (string.IsNullOrEmpty(FreeText))
                {
                    WarningDialog("กรุณาระบุสิ่งที่แพ้");
                    return false;
                }
            }


            return true;
        }
        private void Add()
        {
            if (SelectPatientVisit != null)
            {
                if (!Validate())
                    return;

                PatientAllergyModel patAllergy = new PatientAllergyModel();
                patAllergy.PatientUID = SelectPatientVisit.PatientUID;
                patAllergy.PatientVisitUID = SelectPatientVisit.PatientVisitUID;
                patAllergy.ALRCLUID = SelectAllergyClass.Key;
                patAllergy.AllergyClass = SelectAllergyClass.Display;
                patAllergy.AllergyDescription = AllergyDescription;

                if (SelectSeverity != null)
                {
                    patAllergy.SEVRTUID = SelectSeverity.Key;
                    patAllergy.Severity = SelectSeverity.Display;
                }

                if (SelectAccuracy != null)
                {
                    patAllergy.CERNTUID = SelectAccuracy.Key;
                    patAllergy.Certanity = SelectAccuracy.Display;
                }

                if (IsCheckDrug)
                {
                    if (SelectDrugSearch != null)
                        patAllergy.IdentifyingUID = SelectDrugSearch.ItemMasterUID;

                    patAllergy.IdentifyingType = IdentifyingType;
                    patAllergy.AllergicTo = SelectDrugSearch.Name;
                }
                else if (IsCheckGeneric)
                {
                    if (SelectGenericSearch != null)
                        patAllergy.IdentifyingUID = SelectGenericSearch.DrugGenericUID;

                    patAllergy.IdentifyingType = IdentifyingType;
                    patAllergy.AllergicTo = SelectGenericSearch.Name;
                }
                else if (IsCheckOther)
                {
                    patAllergy.IdentifyingUID = null;
                    patAllergy.IdentifyingType = IdentifyingType;
                    patAllergy.AllergicTo = FreeText;
                }

                patAllergy.RecordByName = AppUtil.Current.UserName;

                PatientAllergyList.Add(patAllergy);

                ClearData();
            }


        }

        private void Edit()
        {
            if (SelectPatientVisit != null)
            {
                if (SelectPatientAllergy != null)
                {
                    if (!Validate())
                        return;

                    SelectPatientAllergy.PatientUID = SelectPatientVisit.PatientUID;
                    SelectPatientAllergy.PatientVisitUID = SelectPatientVisit.PatientVisitUID;
                    SelectPatientAllergy.ALRCLUID = SelectAllergyClass.Key;
                    SelectPatientAllergy.AllergyClass = SelectAllergyClass.Display;
                    SelectPatientAllergy.AllergyDescription = AllergyDescription;

                    if (SelectSeverity != null)
                    {
                        SelectPatientAllergy.SEVRTUID = SelectSeverity.Key;
                        SelectPatientAllergy.Severity = SelectSeverity.Display;
                    }

                    if (SelectAccuracy != null)
                    {
                        SelectPatientAllergy.CERNTUID = SelectAccuracy.Key;
                        SelectPatientAllergy.Certanity = SelectAccuracy.Display;
                    }

                    if (IsCheckDrug)
                    {
                        if (SelectDrugSearch != null)
                            SelectPatientAllergy.IdentifyingUID = SelectDrugSearch.ItemMasterUID;

                        SelectPatientAllergy.IdentifyingType = IdentifyingType;
                        SelectPatientAllergy.AllergicTo = SelectDrugSearch.Name;
                    }
                    else if (IsCheckGeneric)
                    {
                        if (SelectGenericSearch != null)
                            SelectPatientAllergy.IdentifyingUID = SelectGenericSearch.DrugGenericUID;

                        SelectPatientAllergy.IdentifyingType = IdentifyingType;
                        SelectPatientAllergy.AllergicTo = SelectGenericSearch.Name;
                    }
                    else if (IsCheckOther)
                    {
                        SelectPatientAllergy.IdentifyingUID = null;
                        SelectPatientAllergy.IdentifyingType = IdentifyingType;
                        SelectPatientAllergy.AllergicTo = FreeText;
                    }

                    SelectPatientAllergy.RecordByName = AppUtil.Current.UserName;

                    OnUpdateEvent();

                    ClearData();
                }

            }

        }
        private void Delete()
        {
            if (SelectPatientAllergy != null)
            {
                PatientAllergyList.Remove(SelectPatientAllergy);
            }
        }

        private void ClearData()
        {
            SelectAllergyClass = null;
            SelectSeverity = null;
            SelectAccuracy = null;
            SelectDrugSearch = null;
            SelectGenericSearch = null;
            AllergyDescription = string.Empty;

            SelectType("");

            SelectPatientAllergy = null;
        }

        private void Save()
        {
            try
            {
                if (SelectPatientVisit != null)
                {
                    AssignPropertiesToModel();
                    long patientUID = SelectPatientVisit.PatientUID;
                    DataService.PatientIdentity.ManagePatientAllergy(patientUID, model, AppUtil.Current.UserID);
                    CloseViewDialog(ActionDialog.Save);
                }

            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }

        private void SelectType(object content)
        {
            IdentifyingType = content.ToString();
            switch (IdentifyingType)
            {
                case "ชื่อยาสามัญ":
                    IsEnableDrug = false;
                    IsEnableOther = false;
                    IsEnableDrugGeneric = true;

                    IsCheckDrug = false;
                    IsCheckOther = false;
                    IsCheckGeneric = true;

                    break;
                case "ยา":
                    IsEnableDrug = true;
                    IsEnableOther = false;
                    IsEnableDrugGeneric = false;

                    IsCheckDrug = true;
                    IsCheckOther = false;
                    IsCheckGeneric = false;
                    break;
                case "อื่นๆ":
                    IsEnableDrug = false;
                    IsEnableOther = true;
                    IsEnableDrugGeneric = false;

                    IsCheckDrug = false;
                    IsCheckOther = true;
                    IsCheckGeneric = false;
                    break;
                default:

                    IsEnableDrug = false;
                    IsEnableOther = false;
                    IsEnableDrugGeneric = false;

                    IsCheckDrug = false;
                    IsCheckOther = false;
                    IsCheckGeneric = false;

                    break;
            }

            SearchGenericCriteria = "";
            SearchDrugCriteria = "";
            FreeText = "";
        }
        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        public void AssingPatientVisit(PatientVisitModel visitModel)
        {
            SelectPatientVisit = visitModel;
            var data = DataService.PatientIdentity.GetPatientAllergyByPatientUID(SelectPatientVisit.PatientUID);
            AssignModel(data);
        }
        public void AssignModel(List<PatientAllergyModel> model)
        {
            this.model = model;
            AssignModelToProperties();
        }

        public void AssignModelToProperties()
        {
            PatientAllergyList = new ObservableCollection<PatientAllergyModel>();
            foreach (var item in model)
            {
                PatientAllergyList.Add(item);
            }


        }
        public void AssignPropertiesToModel()
        {
            model = new List<PatientAllergyModel>();
            foreach (var item in PatientAllergyList)
            {
                model.Add(item);
            }
        }

        #endregion


    }
}
