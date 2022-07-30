using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class IPFillsDetailViewModel : MediTechViewModelBase
    {
        #region Properties

        private string _FillID;
        public string FillID
        {
            get { return _FillID; }
            set { Set(ref _FillID, value); }
        }

        private DateTime _FillDate;
        public DateTime FillDate
        {
            get { return _FillDate; }
            set { Set(ref _FillDate, value); }
        }

        private string _FilledBy;
        public string FilledBy
        {
            get { return _FilledBy; }
            set { Set(ref _FilledBy, value); }
        }

        private ObservableCollection<IPFillDetailModel> _FillDetail;
        public ObservableCollection<IPFillDetailModel> FillDetail
        {
            get { return _FillDetail; }
            set { Set(ref _FillDetail, value); }
        }

        private IPFillDetailModel _SelectFillDetail;
        public IPFillDetailModel SelectFillDetail
        {
            get { return _SelectFillDetail; }
            set { Set(ref _SelectFillDetail, value); }
        }
        #endregion

        #region Command
        private RelayCommand _PrintAllCommand;
        public RelayCommand PrintAllCommand
        {
            get { return _PrintAllCommand ?? (_PrintAllCommand = new RelayCommand(PrintAll)); }
        }

        private RelayCommand _PrintStickerAllCommand;
        public RelayCommand PrintStickerAllCommand
        {
            get { return _PrintStickerAllCommand ?? (_PrintStickerAllCommand = new RelayCommand(PrintStickerAll)); }
        }

        private RelayCommand _PrintCommand;
        public RelayCommand PrintCommand
        {
            get { return _PrintCommand ?? (_PrintCommand = new RelayCommand(Print)); }
        }

        private RelayCommand _PrintStickerCommand;
        public RelayCommand PrintStickerCommand
        {
            get { return _PrintStickerCommand ?? (_PrintStickerCommand = new RelayCommand(PrintSticker)); }
        }

        private RelayCommand _CloseCommand;
        public RelayCommand CloseCommand
        {
            get { return _CloseCommand ?? (_CloseCommand = new RelayCommand(Close)); }
        }
        #endregion

        #region Method

        public void AssignModel(IPFillProcessModel model)
        {
            FillID = model.FillID;
            FillDate = model.FillDttm;
            FilledBy = model.FillByUser;

            List<IPFillDetailModel> data = DataService.Inventory.GetIPFillDetail(model.IPFillProcessUID);
            FillDetail = new ObservableCollection<IPFillDetailModel>(data);
        }

        private void Print()
        {

        }
        private void PrintAll()
        {

        }

        private void PrintSticker()
        {

        }
        private void PrintStickerAll ()
        {

        }


        private void Close()
        {
            IPFills iPFills = new IPFills();
            ChangeViewPermission(iPFills);
        }

        #endregion
    }
}
