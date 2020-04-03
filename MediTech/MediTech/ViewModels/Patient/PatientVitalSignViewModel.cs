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
using System.Windows.Forms;
using System.Windows;
namespace MediTech.ViewModels
{
    public class PatientVitalSignViewModel : MediTechViewModelBase
    {
        #region Properties

        public int SelectTabIndex { get; set; }

        private bool _IsEditRecent;

        public bool IsEditRecent
        {
            get { return _IsEditRecent; }
            set { Set(ref _IsEditRecent, value); }
        }


        private PatientVisitModel _SelectPatientVisit;

        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set { Set(ref _SelectPatientVisit, value); }
        }

        private List<PatientVitalSignModel> _RecentVitals;

        public List<PatientVitalSignModel> RecentVitals
        {
            get { return _RecentVitals; }
            set { Set(ref _RecentVitals, value); }
        }

        private PatientVitalSignModel _SelectRecentVital;

        public PatientVitalSignModel SelectRecentVital
        {
            get { return _SelectRecentVital; }
            set
            {

                Set(ref _SelectRecentVital, value);
                if (SelectRecentVital != null)
                {
                    IsEditRecent = true;
                    AssingModel(SelectRecentVital);
                }
                else
                {
                    IsEditRecent = false;
                }
            }
        }


        private double? _Height;

        public double? Height
        {
            get { return _Height; }
            set { Set(ref _Height, value); }
        }

        private double? _Weight;

        public double? Weight
        {
            get { return _Weight; }
            set { Set(ref _Weight, value); }
        }

        private string _BMI;

        public string BMI
        {
            get { return _BMI; }
            set
            {
                Set(ref _BMI, value);
            }
        }


        private string _BSA;

        public string BSA
        {
            get { return _BSA; }
            set
            {
                Set(ref _BSA, value);
            }
        }

        private double? _Tempe;

        public double? Tempe
        {
            get { return _Tempe; }
            set { Set(ref _Tempe, value); }
        }

        private double? _Pluse;

        public double? Pluse
        {
            get { return _Pluse; }
            set { Set(ref _Pluse, value); }
        }

        private double? _RespiratoryRate;

        public double? RespiratoryRate
        {
            get { return _RespiratoryRate; }
            set { Set(ref _RespiratoryRate, value); }
        }


        private double? _SBP;

        public double? SBP
        {
            get { return _SBP; }
            set { Set(ref _SBP, value); }
        }
        private double? _DBP;

        public double? DBP
        {
            get { return _DBP; }
            set { Set(ref _DBP, value); }
        }



        private double? _HeightRe;

        public double? HeightRe
        {
            get { return _HeightRe; }
            set { Set(ref _HeightRe, value); }
        }


        private double? _WeightRe;

        public double? WeightRe
        {
            get { return _WeightRe; }
            set { Set(ref _WeightRe, value); }
        }

        private string _BMIRe;

        public string BMIRe
        {
            get { return _BMIRe; }
            set { Set(ref _BMIRe, value); }
        }

        private string _BSARe;

        public string BSARe
        {
            get { return _BSARe; }
            set { Set(ref _BSARe, value); }
        }

        private double? _TempRe;

        public double? TempRe
        {
            get { return _TempRe; }
            set { Set(ref _TempRe, value); }
        }

        private double? _PluseRe;

        public double? PluseRe
        {
            get { return _PluseRe; }
            set { Set(ref _PluseRe, value); }
        }


        private double? _RssRateRe;

        public double? RssRateRe
        {
            get { return _RssRateRe; }
            set { Set(ref _RssRateRe, value); }
        }

        private double? _SBPRe;

        public double? SBPRe
        {
            get { return _SBPRe; }
            set { Set(ref _SBPRe, value); }
        }

        private double? _DBPRe;

        public double? DBPRe
        {
            get { return _DBPRe; }
            set { Set(ref _DBPRe, value); }
        }

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

        private RelayCommand _DeleteVitalSignCommand;

        public RelayCommand DeleteVitalSignCommand
        {
            get { return _DeleteVitalSignCommand ?? (_DeleteVitalSignCommand = new RelayCommand(DeleteVitalSign)); }
        }
        

        #endregion

        #region Method

        PatientVitalSignModel model;
        public PatientVitalSignViewModel()
        {

        }

        public void Save()
        {
            try
            {
                AssingPropertiesToModel();
                DataService.PatientHistory.ManagePatientVitalSign(model, AppUtil.Current.UserID);
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

        public void DeleteVitalSign()
        {
            if (SelectRecentVital != null)
            {
                DialogResult result = DeleteDialog();
                if (result == DialogResult.Yes)
                {
                    DataService.PatientHistory.DeletePatientVitalSign(SelectRecentVital.PatientVitalSignUID, AppUtil.Current.UserID);

                    WeightRe = null;
                    HeightRe = null;
                    BMIRe = null;
                    BSARe = null;
                    TempRe = null;
                    PluseRe = null;
                    RssRateRe = null;
                    SBPRe = null;
                    DBPRe = null;

                    RecentVitals.Remove(SelectRecentVital);
                    OnUpdateEvent();
                }
            }
        }

        public void AssingPatientVisit(PatientVisitModel visitModel)
        {
            SelectPatientVisit = visitModel;
            RecentVitals = DataService.PatientHistory.GetPatientVitalSingByVisitUID(SelectPatientVisit.PatientVisitUID);
        }
        public void AssingModel(PatientVitalSignModel modelData)
        {
            model = modelData;
            AssingModelToProperties();
        }

        public void AssingModelToProperties()
        {
            WeightRe = model.Weight;
            HeightRe = model.Height;
            BMIRe = model.BMIValue.ToString();
            BSARe = model.BSAValue.ToString();
            TempRe = model.Temprature;
            PluseRe = model.Pulse;
            RssRateRe = model.RespiratoryRate;
            SBPRe = model.BPSys;
            DBPRe = model.BPDio;
        }

        public void AssingPropertiesToModel()
        {
            if (SelectTabIndex == 0)
            {
                model = new PatientVitalSignModel();
                model.PatientUID = SelectPatientVisit.PatientUID;
                model.PatientVisitUID = SelectPatientVisit.PatientVisitUID;
                model.Weight = Weight;
                model.Height = Height;
                model.BMIValue = Convert.ToDouble(BMI);
                model.BSAValue = Convert.ToDouble(BSA);
                model.Temprature = Tempe;
                model.Pulse = Pluse;
                model.RespiratoryRate = RespiratoryRate;
                model.BPSys = SBP;
                model.BPDio = DBP;
                model.RecordedDttm = DateTime.Now;
            }
            else if (SelectTabIndex == 1)
            {
                if (SelectRecentVital != null)
                {
                    model.Weight = WeightRe;
                    model.Height = HeightRe;
                    model.BMIValue = Convert.ToDouble(BMIRe);
                    model.BSAValue = Convert.ToDouble(BSARe);
                    model.Temprature = TempRe;
                    model.Pulse = PluseRe;
                    model.RespiratoryRate = RssRateRe;
                    model.BPSys = SBPRe;
                    model.BPDio = DBPRe;
                }

            }
        }

        #endregion
    }
}
