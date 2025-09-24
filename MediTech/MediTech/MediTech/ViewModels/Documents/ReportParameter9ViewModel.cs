using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Model.Report;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MediTech.ViewModels
{
    public class ReportParameter9ViewModel : MediTechViewModelBase
    {
        #region Properites

        public ReportsModel ReportTemplate { get; set; }

        private List<CareproviderModel> _Careproviders;

        public List<CareproviderModel> Careproviders
        {
            get { return _Careproviders; }
            set { _Careproviders = value; }
        }

        private CareproviderModel _SelectCareprovider;

        public CareproviderModel SelectCareprovider
        {
            get { return _SelectCareprovider; }
            set { Set(ref _SelectCareprovider, value); }
        }

        private DateTime? _DateFrom;

        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set { Set(ref _DateFrom, value); }
        }


        private DateTime? _DateTo;

        public DateTime? DateTo
        {
            get { return _DateTo; }
            set { Set(ref _DateTo, value); }
        }

        private bool _IsEnabledDoctor;

        public bool IsEnabledDoctor
        {
            get { return _IsEnabledDoctor; }
            set { Set(ref _IsEnabledDoctor, value); }
        }

        #endregion

        #region Command

        private RelayCommand _PrintCommand;
        public RelayCommand PrintCommand
        {
            get
            {
                return  _PrintCommand
                    ?? ( _PrintCommand = new RelayCommand(Print));
            }
        }

        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return  _CancelCommand
                    ?? ( _CancelCommand = new RelayCommand(Cancel));
            }
        }
        #endregion

        #region Method

        public ReportParameter9ViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            Careproviders = DataService.UserManage.GetCareproviderAll();
            if (AppUtil.Current.IsDoctor ?? false)
            {
                SelectCareprovider = Careproviders.FirstOrDefault(p => p.CareproviderUID == AppUtil.Current.UserID);
                IsEnabledDoctor = false;
            }
            else
            {
                IsEnabledDoctor = true;
            }
        }

        void Print()
        {
            var myReport = Activator.CreateInstance(Type.GetType(ReportTemplate.NamespaceName));
            XtraReport report = (XtraReport)myReport;

            foreach (var item in report.Parameters)
            {
                switch (item.Name)
                {
                    case "DateFrom":
                        item.Value = DateFrom;
                        break;
                    case "DateTo":
                        item.Value = DateTo;
                        break;
                    case "CareproviderUID":
                        item.Value = (SelectCareprovider != null ) ? SelectCareprovider.CareproviderUID : (object)null;
                        break;
                }
            }

            ReportPrintTool printTool = new ReportPrintTool(report);
            report.ShowPrintMarginsWarning = false;
            printTool.ShowPreviewDialog();

        }

        void Cancel()
        {
            ChangeViewPermission(this.BackwardView);
        }

        #endregion

    }
}
