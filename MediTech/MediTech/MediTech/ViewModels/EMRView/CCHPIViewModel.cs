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

namespace MediTech.ViewModels
{
    public class CCHPIViewModel : MediTechViewModelBase
    {
        #region Properties

        private ObservableCollection<CCHPIMasterModel> _CCHPIMaster;

        public ObservableCollection<CCHPIMasterModel> CCHPIMaster
        {
            get { return _CCHPIMaster; }
            set { Set(ref _CCHPIMaster, value); }
        }

        private CCHPIMasterModel _SelectCCHPIMaster;

        public CCHPIMasterModel SelectCCHPIMaster
        {
            get { return _SelectCCHPIMaster; }
            set { Set(ref _SelectCCHPIMaster, value); }
        }


        private int? _Period;

        public int? Period
        {
            get { return _Period; }
            set { Set(ref _Period, value); }
        }

        private List<LookupReferenceValueModel> _UnitMeasure;

        public List<LookupReferenceValueModel> UnitMeasure
        {
            get { return _UnitMeasure; }
            set { Set(ref _UnitMeasure, value); }
        }

        private LookupReferenceValueModel _SelectUnitMeasure;

        public LookupReferenceValueModel SelectUnitMeasure
        {
            get { return _SelectUnitMeasure; }
            set { Set(ref _SelectUnitMeasure, value); }
        }



        private string _Presentillness;

        public string Presentillness
        {
            get { return _Presentillness; }
            set { Set(ref _Presentillness, value); }
        }

        private PatientVisitModel _SelectPatientVisit;

        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set
            {
                Set(ref _SelectPatientVisit, value);
                //if (SelectPatientVisit != null)
                //{
                //    model = DataService.PatientHistory.GetCCHPIByVisit(SelectPatientVisit.PatientVisitUID);
                //    AssignModelToProperties();
                //}

            }
        }

        #endregion

        #region Command

        private RelayCommand<string> _AddToFavoruiteCommand;

        public RelayCommand<string> AddToFavoruiteCommand
        {
            get { return _AddToFavoruiteCommand ?? (_AddToFavoruiteCommand = new RelayCommand<string>(AddToFavoruite)); }
        }

        private RelayCommand<string> _TemplateSettingCommand;

        public RelayCommand<string> TemplateSettingCommand
        {
            get { return _TemplateSettingCommand ?? (_TemplateSettingCommand = new RelayCommand<string>(TemplateSetting)); }
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

        private CCHPIModel _Model;

        public CCHPIModel Model
        {
            get { return _Model; }
            set
            {
                _Model = value;
            }
        }


        public void AssingPatientVisit(PatientVisitModel visitModel, CCHPIModel model = null)
        {
            SelectPatientVisit = visitModel;
            this.Model = model;
            AssignModelToProperties();
        }
        public CCHPIViewModel()
        {
            UnitMeasure = DataService.Technical.GetReferenceValueMany("AGUOM");
            CCHPIMaster = new ObservableCollection<CCHPIMasterModel>(DataService.PatientHistory.GetCCHPIMaster("CC"));
        }

        public void AddToFavoruite(string type)
        {
            try
            {
                if (type == "CC")
                {
                    if (SelectCCHPIMaster == null || string.IsNullOrEmpty(SelectCCHPIMaster?.Name))
                    {
                        WarningDialog("กรุณาใส่ Chief Complaint");
                        return;
                    }
                    if (CCHPIMaster.FirstOrDefault(p => p.CCHPIMasterUID != 0 && p.Name == SelectCCHPIMaster.Name) != null)
                    {
                        WarningDialog("ข้อมูลนี้มีอยู่แล้วในระบบ");
                        return;
                    }
                    CCHPIMasterModel cchpiMaster = new CCHPIMasterModel();
                    cchpiMaster.Name = SelectCCHPIMaster.Name;
                    cchpiMaster.Description = SelectCCHPIMaster.Description;
                    cchpiMaster.Type = "CC";
                    DataService.PatientHistory.ManageCCHPIMaster(cchpiMaster, AppUtil.Current.UserID);

                    SaveSuccessDialog();

                    string tempCC = SelectCCHPIMaster.Name;
                    CCHPIMaster = new ObservableCollection<CCHPIMasterModel>(DataService.PatientHistory.GetCCHPIMaster("CC"));
                    SelectCCHPIMaster = CCHPIMaster.FirstOrDefault(p => p.Name == tempCC);
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        public void TemplateSetting(string typeTemplate)
        {
            CustomTemplate pageview = new CustomTemplate(typeTemplate, Presentillness);
            CustomTemplateViewModel result = (CustomTemplateViewModel)LaunchViewDialogNonPermiss(pageview, true);
            if (result.ResultDialog == ActionDialog.Save)
            {
                if (typeTemplate == "CC")
                {
                    if (result.ActionType == "AddTemplate")
                    {
                        CCHPIMaster = new ObservableCollection<CCHPIMasterModel>(DataService.PatientHistory.GetCCHPIMaster("CC"));
                    }
                    else if (result.ActionType == "UseTemplate")
                    {
                        SelectCCHPIMaster = CCHPIMaster.FirstOrDefault(p => p.CCHPIMasterUID == result.SelectCCHPIMaster.CCHPIMasterUID);
                    }
                }
                else if (typeTemplate == "PI")
                {
                    if (result.ActionType == "UseTemplate")
                    {
                        Presentillness = result.SelectCCHPIMaster.Description;
                    }

                }
            }

        }

        public void Save()
        {
            try
            {
                AssignPropertiesModel();
                DataService.PatientHistory.ManageCCHPI(Model, AppUtil.Current.UserID);
                SaveSuccessDialog();
                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        public void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        public void AssignPropertiesModel()
        {
            if (Model == null)
            {
                Model = new CCHPIModel();
                Model.PatientUID = SelectPatientVisit.PatientUID;
                Model.PatientVisitUID = SelectPatientVisit.PatientVisitUID;
            }

            Model.Complaint = SelectCCHPIMaster != null ? SelectCCHPIMaster.Name?.Trim() : null;
            Model.CCHPIMasterUID = SelectCCHPIMaster != null  ? SelectCCHPIMaster.CCHPIMasterUID : (int?)null;
            Model.Presentillness = Presentillness?.Trim();
            Model.Period = Period;
            Model.CCHPIMasterUID = SelectCCHPIMaster != null ? SelectCCHPIMaster.CCHPIMasterUID : (int?)null;
            Model.AGUOMUID = SelectUnitMeasure != null ? SelectUnitMeasure.Key : (int?)null;

        }
        public void AssignModelToProperties()
        {
            if (Model != null)
            {
                if (Model.CCHPIMasterUID == null || Model.CCHPIMasterUID == 0)
                {
                    CCHPIMaster.Add(new CCHPIMasterModel() { Name = Model.Complaint, Description = Model.Complaint });
                    SelectCCHPIMaster = CCHPIMaster.FirstOrDefault(p => p.Name == Model.Complaint);
                }
                else
                {
                    SelectCCHPIMaster = CCHPIMaster.FirstOrDefault(p => p.CCHPIMasterUID == Model.CCHPIMasterUID);
                }

                Presentillness = Model.Presentillness;
                Period = Model.Period;
                SelectUnitMeasure = UnitMeasure != null ? UnitMeasure.FirstOrDefault(p => p.Key == Model.AGUOMUID) : null;
            }
        }

        #endregion
    }
}
