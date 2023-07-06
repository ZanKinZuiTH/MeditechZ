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
    public class ReportParameter7ViewModel : MediTechViewModelBase
    {
        #region Properites

        public ReportsModel ReportTemplate { get; set; }

        private List<CareproviderModel> _Doctors;

        public List<CareproviderModel> Doctors
        {
            get { return _Doctors; }
            set { _Doctors = value; }
        }

        private CareproviderModel _SelectDoctor;

        public CareproviderModel SelectDoctor
        {
            get { return _SelectDoctor; }
            set { Set(ref _SelectDoctor, value); }
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

        public ReportParameter7ViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            Doctors = DataService.UserManage.GetCareproviderDoctor();
            //hardcode สำหรับหมอตั้ง
            if ((AppUtil.Current.IsDoctor ?? false) && AppUtil.Current.UserID != 2)
            {
                SelectDoctor = Doctors.FirstOrDefault(p => p.CareproviderUID == AppUtil.Current.UserID);
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
                        item.Value = (SelectDoctor != null ) ? SelectDoctor.CareproviderUID : (object)null;
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
