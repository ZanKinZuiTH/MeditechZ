using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
namespace MediTech.ViewModels
{
    public class MappingTranslateXrayViewModel : MediTechViewModelBase
    {

        #region Properites

        private ObservableCollection<XrayTranslateMappingModel> _MappingXrays;

        public ObservableCollection<XrayTranslateMappingModel> MappingXrays
        {
            get { return _MappingXrays ; }
            set { Set(ref _MappingXrays , value); }
        }

        private XrayTranslateMappingModel _SelectMappingXray;

        public XrayTranslateMappingModel SelectMappingXray
        {
            get { return _SelectMappingXray; }
            set { _SelectMappingXray = value; }
        }


        private List<LookupItemModel> _TranslateTypes;

        public List<LookupItemModel> TranslateTypes
        {
            get { return _TranslateTypes; }
            set { Set(ref _TranslateTypes , value); }
        }


        #endregion

        #region Command

        private RelayCommand<DevExpress.Xpf.Grid.RowEventArgs> _RowUpdatedCommand;

        /// <summary>
        /// Gets the RowUpdatedCommand.
        /// </summary>
        public RelayCommand<DevExpress.Xpf.Grid.RowEventArgs> RowUpdatedCommand
        {
            get
            {
                return _RowUpdatedCommand
                    ?? (_RowUpdatedCommand = new RelayCommand<DevExpress.Xpf.Grid.RowEventArgs>(RowUpdated));
            }
        }


        private RelayCommand _DeleteCommand;

        /// <summary>
        /// Gets the DeleteCommand.
        /// </summary>
        public RelayCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand
                    ?? (_DeleteCommand = new RelayCommand(DeleteXrayMapping));
            }
        }

        private void ExecuteDeleteCommand()
        {

        }
        #endregion

        #region Method

        public MappingTranslateXrayViewModel()
        {
            MappingXrays = new ObservableCollection<XrayTranslateMappingModel>(DataService.Radiology.GetXrayTranslateMapping());


            TranslateTypes = new List<LookupItemModel>();
            TranslateTypes.Add(new LookupItemModel { Key = 1, Display = "Chest" });
            TranslateTypes.Add(new LookupItemModel { Key = 2, Display = "Ultrasound" });
            TranslateTypes.Add(new LookupItemModel { Key = 3, Display = "Mammo" });
        }

        private void RowUpdated(DevExpress.Xpf.Grid.RowEventArgs e)
        {
            try
            {
                if (e.Row is XrayTranslateMappingModel)
                {
                    int userUID = AppUtil.Current.UserID;
                    XrayTranslateMappingModel rowItem = (XrayTranslateMappingModel)e.Row;
                    rowItem.CUser = userUID;
                    rowItem.MUser = userUID;
                    DataService.Radiology.SaveXrayTranslateMapping(rowItem);
                    SaveSuccessDialog();
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void DeleteXrayMapping()
        {
            try
            {
                if (SelectMappingXray != null)
                {
                    MessageBoxResult result =  DeleteDialog();
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.Radiology.DeleteXrayTranslateMapping(SelectMappingXray.XrayTranslateMappingUID, AppUtil.Current.UserID);
                        DeleteSuccessDialog();
                        MappingXrays.Remove(SelectMappingXray);
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
