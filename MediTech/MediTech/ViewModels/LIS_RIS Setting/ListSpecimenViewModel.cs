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
    public class ListSpecimenViewModel : MediTechViewModelBase
    {
        #region Properties

        private string _SpecimenNameCode;

        public string SpecimenNameCode
        {
            get { return _SpecimenNameCode; }
            set { Set(ref _SpecimenNameCode, value); }
        }

        private List<SpecimenModel> _ListSpecimans;

        public List<SpecimenModel> ListSpecimans
        {
            get { return _ListSpecimans; }
            set { Set(ref _ListSpecimans, value); }
        }


        private SpecimenModel _SelectSpecimen;

        public SpecimenModel SelectSpecimen
        {
            get { return _SelectSpecimen; }
            set { Set(ref _SelectSpecimen, value); }
        }


        #endregion

        #region Command


        private RelayCommand _SearchCommnad;

        public RelayCommand SearchCommnad
        {
            get { return _SearchCommnad ?? (_SearchCommnad = new RelayCommand(SearchSpecimen)); }
        }

        private RelayCommand _AddCommand;

        public RelayCommand AddCommand
        {
            get { return _AddCommand ?? (_AddCommand = new RelayCommand(AddSpecimen)); }
        }


        private RelayCommand _EditCommand;

        public RelayCommand EditCommand
        {
            get { return _EditCommand ?? (_EditCommand = new RelayCommand(EditSpecimen)); }
        }


        private RelayCommand _DeleteCommand;

        public RelayCommand DeleteCommand
        {
            get { return _DeleteCommand ?? (_DeleteCommand = new RelayCommand(DeleteSpecimen)); }
        }


        #endregion

        #region Method

        public ListSpecimenViewModel()
        {
            SearchSpecimen();
        }

        void SearchSpecimen()
        {
            ListSpecimans = DataService.MasterData.SearhcSpecimen(SpecimenNameCode);
        }

        void AddSpecimen()
        {
            ManageSpecimen manageSpecimen = new ManageSpecimen();
            ChangeViewPermission(manageSpecimen);
        }



        void EditSpecimen()
        {
            if (SelectSpecimen != null)
            {
                ManageSpecimen manageSpecimen = new ManageSpecimen();
                (manageSpecimen.DataContext as ManageSpecimenViewModel).EditData(SelectSpecimen.SpecimenUID);
                ChangeViewPermission(manageSpecimen);
            }

        }

        void DeleteSpecimen()
        {
            try
            {
                if (SelectSpecimen != null)
                {
                    MessageBoxResult result = QuestionDialog("คุณต้องการลบข้อมูล ใช้หรื่อไม่ ?");
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.MasterData.DeleteSpecimen(SelectSpecimen.SpecimenUID, AppUtil.Current.UserID);
                        ListSpecimans.Remove(SelectSpecimen);
                        (this.View as ListSpecimen).grdSpecimen.RefreshData();
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
