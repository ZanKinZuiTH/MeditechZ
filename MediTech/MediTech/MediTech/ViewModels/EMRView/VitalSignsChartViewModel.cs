using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Model.Report;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class VitalSignsChartViewModel : MediTechViewModelBase
    {
        #region Properties
        public long PatientUID { get; set; }

        private DateTime _DateFrom;

        public DateTime DateFrom
        {
            get { return _DateFrom; }
            set
            {
                Set(ref _DateFrom, value);
            }
        }

        private DateTime _DateTo;

        public DateTime DateTo
        {
            get { return _DateTo; ; }
            set
            {
                Set(ref _DateTo, value);
            }
        }

        private List<LookupReferenceValueModel> _VitalSignList;

        public List<LookupReferenceValueModel> VitalSignList
        {
            get { return _VitalSignList; }
            set { Set(ref _VitalSignList, value); }
        }

        private List<object> _SelectVitalSignList;

        public List<object> SelectVitalSignList
        {
            get { return _SelectVitalSignList ?? (_SelectVitalSignList = new List<object>()); }
            set { Set(ref _SelectVitalSignList, value); }
        }

        private ObservableCollection<ChartStatisticModel> _DataStatistic;

        public ObservableCollection<ChartStatisticModel> DataStatistic
        {
            get { return _DataStatistic; }
            set
            {
                Set(ref _DataStatistic, value);
            }
        }

        #endregion

        #region Command

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(SearchVitalSigns)); }
        }


        private RelayCommand _ClearCommand;

        public RelayCommand ClearCommand
        {
            get { return _ClearCommand ?? (_ClearCommand = new RelayCommand(Clear)); }
        }
        #endregion

        #region Method

        public VitalSignsChartViewModel()
        {
            VitalSignList = new List<LookupReferenceValueModel>();
            VitalSignList.Add(new LookupReferenceValueModel { Display = "Height" });
            VitalSignList.Add(new LookupReferenceValueModel { Display = "Weight" });
            VitalSignList.Add(new LookupReferenceValueModel { Display = "BMI" });
            VitalSignList.Add(new LookupReferenceValueModel { Display = "BSA" });
            VitalSignList.Add(new LookupReferenceValueModel { Display = "Temprature" });
            VitalSignList.Add(new LookupReferenceValueModel { Display = "Pulse" });
            VitalSignList.Add(new LookupReferenceValueModel { Display = "Respiratory Rate" });
            VitalSignList.Add(new LookupReferenceValueModel { Display = "Systolic BP" });
            VitalSignList.Add(new LookupReferenceValueModel { Display = "Diastolic BP" });
            VitalSignList.Add(new LookupReferenceValueModel { Display = "Oxygen Sat" });
            VitalSignList.Add(new LookupReferenceValueModel { Display = "Waist Circumference" });
            foreach (var item in VitalSignList)
            {
                SelectVitalSignList.Add(item.Display);
            }

            //DateFrom = 
        }

        public override void OnLoaded()
        {
            Clear();
            SearchVitalSigns();
        }

        void SearchVitalSigns()
        {
            List<PatientVitalSignModel> dataSource = DataService.PatientHistory.SearchPatientVitalSign(PatientUID,DateFrom, DateTo);
            GenerateDataStatistic(dataSource);
        }
        void Clear()
        {
            DateFrom = DateTime.Now.AddDays(-30);
            DateTo = DateTime.Now;
        }

        void GenerateDataStatistic(List<PatientVitalSignModel> data)
        {
            if (data != null)
            {
                DataStatistic = new ObservableCollection<ChartStatisticModel>();
                foreach (var item in data.OrderBy(p => p.RecordedDttm))
                {
                    if (item.Height != null && _SelectVitalSignList.Contains("Height"))
                    {
                        DataStatistic.Add(new ChartStatisticModel() { DisplayName = "Height", Argument = item.RecordedDttm?.ToString("dd/MM/yyyy HH:mm"), Value = item.Height.Value });
                    }
                    if (item.Weight != null && _SelectVitalSignList.Contains("Weight"))
                    {
                        DataStatistic.Add(new ChartStatisticModel() { DisplayName = "Weight", Argument = item.RecordedDttm?.ToString("dd/MM/yyyy HH:mm"), Value = item.Weight.Value });
                    }
                    if (item.BMIValue != null && _SelectVitalSignList.Contains("BMI"))
                    {
                        DataStatistic.Add(new ChartStatisticModel() { DisplayName = "BMI", Argument = item.RecordedDttm?.ToString("dd/MM/yyyy HH:mm"), Value = item.BMIValue.Value });
                    }
                    if (item.BSAValue != null && _SelectVitalSignList.Contains("BSA"))
                    {
                        DataStatistic.Add(new ChartStatisticModel() { DisplayName = "BSA", Argument = item.RecordedDttm?.ToString("dd/MM/yyyy HH:mm"), Value = item.BSAValue.Value });
                    }
                    if (item.Temprature != null && _SelectVitalSignList.Contains("Temprature"))
                    {
                        DataStatistic.Add(new ChartStatisticModel() { DisplayName = "Temprature", Argument = item.RecordedDttm?.ToString("dd/MM/yyyy HH:mm"), Value = item.Temprature.Value });
                    }
                    if (item.Pulse != null && _SelectVitalSignList.Contains("Pulse"))
                    {
                        DataStatistic.Add(new ChartStatisticModel() { DisplayName = "Pulse", Argument = item.RecordedDttm?.ToString("dd/MM/yyyy HH:mm"), Value = item.Pulse.Value });
                    }
                    if (item.RespiratoryRate != null && _SelectVitalSignList.Contains("Respiratory Rate"))
                    {
                        DataStatistic.Add(new ChartStatisticModel() { DisplayName = "Respiratory Rate", Argument = item.RecordedDttm?.ToString("dd/MM/yyyy HH:mm"), Value = item.RespiratoryRate.Value });
                    }
                    if (item.BPSys != null && _SelectVitalSignList.Contains("Systolic BP"))
                    {
                        DataStatistic.Add(new ChartStatisticModel() { DisplayName = "Systolic BP", Argument = item.RecordedDttm?.ToString("dd/MM/yyyy HH:mm"), Value = item.BPSys.Value });
                    }
                    if (item.BPDio != null && _SelectVitalSignList.Contains("Diastolic BP"))
                    {
                        DataStatistic.Add(new ChartStatisticModel() { DisplayName = "Diastolic BP", Argument = item.RecordedDttm?.ToString("dd/MM/yyyy HH:mm"), Value = item.BPDio.Value });
                    }
                    if (item.OxygenSat != null && _SelectVitalSignList.Contains("Oxygen Sat"))
                    {
                        DataStatistic.Add(new ChartStatisticModel() { DisplayName = "Oxygen Sat", Argument = item.RecordedDttm?.ToString("dd/MM/yyyy HH:mm"), Value = item.OxygenSat.Value });
                    }
                    if (item.WaistCircumference != null && _SelectVitalSignList.Contains("Waist Circumference"))
                    {
                        DataStatistic.Add(new ChartStatisticModel() { DisplayName = "Waist Circumference", Argument = item.RecordedDttm?.ToString("dd/MM/yyyy HH:mm"), Value = item.WaistCircumference.Value });
                    }
                }
            }
        }

        #endregion
    }
}
