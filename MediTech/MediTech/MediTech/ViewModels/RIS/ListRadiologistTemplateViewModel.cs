using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediTech.ViewModels
{
    public class ListRadiologistTemplateViewModel : MediTechViewModelBase
    {

        #region Properties

        private List<ResultRadiologyTemplateModel> _ResultTemplate;

        public List<ResultRadiologyTemplateModel> ResultTemplate
        {
            get { return _ResultTemplate; }
            set { _ResultTemplate = value; }
        }

        private ResultRadiologyTemplateModel _SelectResultTemplate;

        public ResultRadiologyTemplateModel SelectResultTemplate
        {
            get { return _SelectResultTemplate; }
            set { _SelectResultTemplate = value; }
        }

        #endregion

        #region Command

        private RelayCommand _AddCommand;

        /// <summary>
        /// Gets the AddCommand.
        /// </summary>
        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand
                    ?? (_AddCommand = new RelayCommand(AddTemplate));
            }
        }

        private RelayCommand _EditCommand;

        /// <summary>
        /// Gets the EditCommand.
        /// </summary>
        public RelayCommand EditCommand
        {
            get
            {
                return _EditCommand
                    ?? (_EditCommand = new RelayCommand(EditTemplate));
            }
        }

        private RelayCommand _DeleteCommand;

        /// <summary>
        /// Gets the EditCommand.
        /// </summary>
        public RelayCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand
                    ?? (_DeleteCommand = new RelayCommand(DeleteTemplate));
            }
        }



        #endregion

        #region Method
        public ListRadiologistTemplateViewModel()
        {
            ResultTemplate = DataService.Radiology.GetResultRadiologyTemplateByDoctor(AppUtil.Current.UserID);
        }
        private void AddTemplate()
        {
            ManageRadiologistTemplate manageView = new ManageRadiologistTemplate();
            ChangeViewPermission(manageView);
        }


        private void EditTemplate()
        {
            if (SelectResultTemplate != null)
            {
                ManageRadiologistTemplate manageView = new ManageRadiologistTemplate();
                (manageView.DataContext as ManageRadiologistTemplateViewModel).AssingModel(SelectResultTemplate.ResultRadiologyTemplateUID);
                ChangeViewPermission(manageView);
            }
        }
        private void DeleteTemplate()
        {
            if (SelectResultTemplate != null)
            {
                MessageBoxResult result = QuestionDialog("ต้องการลบ Template " + SelectResultTemplate.Name + " ใช่หรือไม่ ?");
                if (result == MessageBoxResult.Yes)
                {
                    DataService.Radiology.DeleteResultRadiologyTemplate(SelectResultTemplate.ResultRadiologyTemplateUID, AppUtil.Current.UserID);
                    DeleteSuccessDialog();
                    ResultTemplate.Remove(SelectResultTemplate);
                    SelectResultTemplate = null;
                    OnUpdateEvent();
                }
            }
        }

        #endregion
    }
}
