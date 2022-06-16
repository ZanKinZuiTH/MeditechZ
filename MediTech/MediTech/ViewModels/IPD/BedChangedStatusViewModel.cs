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

    public  class BedChangedStatusViewModel : MediTechViewModelBase
    {

        #region property

        private DateTime? _ActiveToDate;
        public DateTime? ActiveToDate
        {
            get { return _ActiveToDate; }
            set
            {
                Set(ref _ActiveToDate, value);

            }
        }
        

        private string _BedHeader;

        public string BedHeader
        {
            get { return _BedHeader; }
            set { Set(ref _BedHeader, value); }
        }

        private List<LookupReferenceValueModel> _StatusBed;

        public List<LookupReferenceValueModel> StatusBed
        {
            get { return _StatusBed; }
            set { Set(ref _StatusBed, value); }
        }

        private LookupReferenceValueModel _SelectStatusBed;

        public LookupReferenceValueModel SelectStatusBed
        {
            get { return _SelectStatusBed; }
            set { Set(ref _SelectStatusBed, value); }
        }


        private LocationModel _Bed;

        public LocationModel Bed
        {
            get { return _Bed; }
            set { Set(ref _Bed, value); }
        }


        private List<LocationModel> _ListBedStatus;

        public List<LocationModel> ListBedStatus
        {
            get { return _ListBedStatus; }
            set { Set(ref _ListBedStatus, value); }
        }


        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set { Set(ref _Comment, value); }
        }






        #endregion

        #region commend

        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                    ?? (_SaveCommand = new RelayCommand(Save));
            }

        }

        #endregion



        #region Method


        private void Save()
        {
            try
            {
                LocationModel locamodel = new LocationModel();
                locamodel.LocationUID = Bed.LocationUID;
                locamodel.LCTSTUID = SelectStatusBed.Key;
                locamodel.Comment = Comment;
                locamodel.ActiveTo = ActiveToDate;

                DataService.IPDService.ChangeBedStatus(locamodel, AppUtil.Current.UserID);

                SaveSuccessDialog();
                CloseViewDialog(ActionDialog.Save);

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }



        public BedChangedStatusViewModel()
        {
           StatusBed = DataService.Technical.GetReferenceValueList("LCTST");

        }

        public void SendingBed(LocationModel SelectBed)
        {
            BedHeader = SelectBed.Name +": "+SelectBed.Description;
            List<LocationModel> beddata = new List<LocationModel>();
            Bed = DataService.Technical.GetLocationByUID(SelectBed.LocationUID);
            beddata.Add(Bed);
            ListBedStatus = beddata;
            // LocationModel bedaddsign = DataService.Technical.GetLocationByUID(SelectBed.LocationUID);
            //ListBedStatus.Add(bedaddsign);

        }




        #endregion


    }
}
