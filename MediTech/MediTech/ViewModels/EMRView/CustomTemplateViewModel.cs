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

namespace MediTech.ViewModels
{
    public class CustomTemplateViewModel : MediTechViewModelBase
    {
        #region Properties

        public string ActionType { get; set; }
        public string TypeTemplate { get; set; }

        private List<LookupItemModel> _Types;

        public List<LookupItemModel> Types
        {
            get { return _Types; }
            set { Set(ref _Types, value); }
        }

        private LookupItemModel _SelectType;

        public LookupItemModel SelectType
        {
            get { return _SelectType; }
            set
            {
                Set(ref _SelectType, value);
                if (SelectType != null)
                {
                    switch (SelectType.Display)
                    {
                        case "CC":
                            TitleLabel = "กรอกคำที่ใช้บ่อย";
                            break;
                        case "PE":
                        case "PI":
                            TitleLabel = "Tempalte Name";
                            break;
                    }
                }
            }
        }

        private string _TitleLabel;

        public string TitleLabel
        {
            get { return _TitleLabel; }
            set { Set(ref _TitleLabel, value); }
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { Set(ref _Name, value); }
        }

        private List<CCHPIMasterModel> _CCHPIMasters;

        public List<CCHPIMasterModel> CCHPIMasters
        {
            get { return _CCHPIMasters; }
            set { Set(ref _CCHPIMasters, value); }
        }

        private CCHPIMasterModel _SelectCCHPIMaster;

        public CCHPIMasterModel SelectCCHPIMaster
        {
            get { return _SelectCCHPIMaster; }
            set { Set(ref _SelectCCHPIMaster, value); }
        }

        public string TemplateValue { get; set; }
        #endregion

        #region Command

        private RelayCommand _AddCommand;

        public RelayCommand AddCommand
        {
            get { return _AddCommand ?? (_AddCommand = new RelayCommand(AddTemplate)); }
        }



        private RelayCommand _DeleteCommand;

        public RelayCommand DeleteCommand
        {
            get { return _DeleteCommand ?? (_DeleteCommand = new RelayCommand(DeleteTemplate)); }
        }

        private RelayCommand _AcceptCommand;

        public RelayCommand AcceptCommand
        {
            get { return _AcceptCommand ?? (_AcceptCommand = new RelayCommand(AcceptTemplate)); }
        }


        #endregion

        #region Method

        public CustomTemplateViewModel(string typeTemplate,string templateValue = "")
        {
            Types = new List<LookupItemModel>();
            Types.Add(new LookupItemModel { Key = 1, Display = "CC" });
            Types.Add(new LookupItemModel { Key = 2, Display = "PI" });
            Types.Add(new LookupItemModel { Key = 3, Display = "PE" });

            SelectType = Types.FirstOrDefault(p => p.Display == typeTemplate);
            this.TypeTemplate = typeTemplate;
            this.TemplateValue = templateValue;
            LoadDataMaster();

        }

        void LoadDataMaster()
        {
            if (SelectType != null)
            {
                CCHPIMasters = new List<CCHPIMasterModel>();
                if (SelectType.Display == "CC" || SelectType.Display == "PI")
                {
                    CCHPIMasters = DataService.PatientHistory.GetCCHPIMaster(SelectType.Display);
                }
                else
                {
                    var peTemplate = DataService.PatientHistory.GetPhysicalExamTemplate();
                    foreach (var item in peTemplate)
                    {
                        CCHPIMasterModel newItem = new CCHPIMasterModel();
                        newItem.CCHPIMasterUID = item.PhysicalExamTemplateUID;
                        newItem.Name = item.Name;
                        newItem.Description = item.TemplateValue;
                        CCHPIMasters.Add(newItem);
                    }
                }
            }
        }

        private void AddTemplate()
        {
            try
            {
                CCHPIMasterModel cchpiMaster = new CCHPIMasterModel();

                    switch (SelectType.Display)
                    {
                        case "CC":
                            if (string.IsNullOrEmpty(Name))
                            {
                                WarningDialog("กรอกคำที่ใช้บ่อย");
                                return;
                            }

                            cchpiMaster.Name = Name;
                            cchpiMaster.Description = Name;
                            cchpiMaster.Type = "CC";
                            DataService.PatientHistory.ManageCCHPIMaster(cchpiMaster, AppUtil.Current.UserID);
                            break;
                        case "PI":
                            if (string.IsNullOrEmpty(Name))
                            {
                                WarningDialog("Tempalte Name");
                                return;
                            }

                            cchpiMaster.Name = Name;
                            cchpiMaster.Description = TemplateValue;
                            cchpiMaster.Type = "PI";
                            DataService.PatientHistory.ManageCCHPIMaster(cchpiMaster, AppUtil.Current.UserID);
                            break;
                        case "PE":
                            if (string.IsNullOrEmpty(Name))
                            {
                                WarningDialog("Tempalte Name");
                                return;
                            }
                            PhysicalExamTemplateModel peTemplate = new PhysicalExamTemplateModel();
                            peTemplate.Name = Name;
                            peTemplate.TemplateValue = TemplateValue;
                            DataService.PatientHistory.ManagePhysicalExamTemplate(peTemplate, AppUtil.Current.UserID);
                            break;
                    }

                    SaveSuccessDialog();
                    ActionType = "AddTemplate";
                    CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }
        private void DeleteTemplate()
        {
            try
            {
                if (SelectCCHPIMaster != null)
                {
                    MessageBoxResult result = QuestionDialog("คุณต้องการลบข้อมูลนี้ ใช่หรือไม่");
                    if (result == MessageBoxResult.Yes)
                    {
                        if (SelectType.Display == "CC" || SelectType.Display == "PI")
                        {
                            DataService.PatientHistory.DeleteCCHPIMaster(SelectCCHPIMaster.CCHPIMasterUID, AppUtil.Current.UserID);
                        }
                        else if(SelectType.Display == "PE")
                        {
                            DataService.PatientHistory.DeletePhysicalExamTemplate(SelectCCHPIMaster.CCHPIMasterUID, AppUtil.Current.UserID);
                        }

                        DeleteSuccessDialog();
                        LoadDataMaster();
                    }

                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void AcceptTemplate()
        {
            if (SelectCCHPIMaster != null)
            {
                ActionType = "UseTemplate";
                CloseViewDialog(ActionDialog.Save);
            }
        }
        #endregion
    }
}
