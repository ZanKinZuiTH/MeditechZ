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
    public class ListTestParameterViewModel : MediTechViewModelBase
    {

        #region Properties

        private string _TestParameterName;

        public string TestParameterName
        {
            get { return _TestParameterName; }
            set { Set(ref _TestParameterName, value); }
        }

        private List<LookupReferenceValueModel> _ResultType;

        public List<LookupReferenceValueModel> ResultType
        {
            get { return _ResultType; }
            set { Set(ref _ResultType, value); }
        }

        private LookupReferenceValueModel _SelectResultType;

        public LookupReferenceValueModel SelectResultType
        {
            get { return _SelectResultType; }
            set { Set(ref _SelectResultType, value); }
        }


        private List<ResultItemModel> _ListTestParameters;

        public List<ResultItemModel> ListTestParameters
        {
            get { return _ListTestParameters; }
            set { Set(ref _ListTestParameters, value); }
        }

        private ResultItemModel _SelectTestParameter;

        public ResultItemModel SelectTestParameter
        {
            get { return _SelectTestParameter; }
            set { Set(ref _SelectTestParameter, value); }
        }

        #endregion


        #region Command

        private RelayCommand _AddCommand;
        public RelayCommand AddCommand
        {
            get { return _AddCommand ?? (_AddCommand = new RelayCommand(AddTestParameter)); }
        }

        private RelayCommand _EditCommand;
        public RelayCommand EditCommand
        {
            get { return _EditCommand ?? (_EditCommand = new RelayCommand(EditTestParameter)); }
        }


        private RelayCommand _DeleteCommand;
        public RelayCommand DeleteCommand
        {
            get { return _DeleteCommand ?? (_DeleteCommand = new RelayCommand(DeleteTestParameter)); }
        }

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(SearchTestParameter)); }
        }


        #endregion


        #region Method

        public ListTestParameterViewModel()
        {
            ResultType = DataService.Technical.GetReferenceValueMany("RVTYP");
        }

        void SearchTestParameter()
        {
            int? resultTypeUID = null;
            if (SelectResultType != null)
            {
                resultTypeUID = SelectResultType.Key;
            }
            ListTestParameters = DataService.MasterData.SearchResultItem(TestParameterName, resultTypeUID);
        }

        void AddTestParameter()
        {
            ManageTestParameter manageTest = new ManageTestParameter();
            ChangeViewPermission(manageTest);
        }

        void EditTestParameter()
        {
            if (SelectTestParameter != null)
            {
                ManageTestParameter manageTest = new ManageTestParameter();
                (manageTest.DataContext as ManageTestParameterViewModel).EditData(SelectTestParameter.ResultItemUID);
                ChangeViewPermission(manageTest);
            }
        }

        void DeleteTestParameter()
        {
            try
            {
                if (SelectTestParameter != null)
                {
                    MessageBoxResult result = QuestionDialog("คุณต้องการลบข้อมูล ใช้หรื่อไม่ ?");
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.MasterData.DeleteResultItem(SelectTestParameter.ResultItemUID, AppUtil.Current.UserID);
                        ListTestParameters.Remove(SelectTestParameter);
                        (this.View as ListTestParameter).grdTestParameter.RefreshData();
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }


        }

        #endregion
    }
}
