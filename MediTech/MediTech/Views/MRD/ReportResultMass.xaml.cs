using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for ReportResultBulk.xaml
    /// </summary>
    public partial class ReportResultMass : UserControl
    {
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);

        private UpdateProgressBarDelegate _updatePbDelegate;
        public ReportResultMass()
        {
            InitializeComponent();
            gcPatientXray.CustomColumnDisplayText += GcPatientXray_CustomColumnDisplayText;
            _updatePbDelegate = new UpdateProgressBarDelegate(progressBar1.SetValue);
        }

        private void GcPatientXray_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.Name == "colNoSequnce" && e.Column.Visible == true)
            {
                e.DisplayText = (e.RowHandle + 1).ToString();
                if (e.Row != null)
                {
                    (e.Row as Model.Report.PatientResultRadiology).No = (e.RowHandle + 1).ToString();
                }
            }

        }

        public void SetProgressBarValue(double value)
        {
            Dispatcher.Invoke(_updatePbDelegate,
                    System.Windows.Threading.DispatcherPriority.Background,
                    new object[] { ProgressBar.ValueProperty, value });

        }

        #region SetProgressBarValues()
        public void SetProgressBarLimits(int minValue, int maxValue)
        {
            progressBar1.Minimum = minValue;
            progressBar1.Maximum = maxValue;
        }

        GridColumn colPostion;
        GridColumn colDepartment;
        GridColumn colCompany;
        GridColumn colEmpID;
        GridColumn colCheckupDate;
        public void AddColumnMobile()
        {
            RemoveColumnMobile();

            colPostion = new GridColumn();
            colPostion.Name = "colPostion";
            colPostion.Header = "ตำแหน่ง";
            colPostion.FieldName = "Position";
            colPostion.ReadOnly = true;

            colDepartment = new GridColumn();
            colDepartment.Name = "colDepartment";
            colDepartment.Header = "แผนก";
            colDepartment.FieldName = "Department";
            colDepartment.ReadOnly = true;

            colCompany = new GridColumn();
            colCompany.Name = "colCompany";
            colCompany.Header = "บริษัท";
            colCompany.FieldName = "Company";
            colCompany.ReadOnly = true;

            colEmpID = new GridColumn();
            colEmpID.Name = "colEmpID";
            colEmpID.Header = "รหัสพนักงาน";
            colEmpID.FieldName = "EmployeeID";
            colEmpID.ReadOnly = true;

            colCheckupDate = new GridColumn();
            colCheckupDate.Name = "colCheckupDate";
            colCheckupDate.Header = "วันที่ตรวจ";
            colCheckupDate.FieldName = "CheckupDttm";
            colCheckupDate.ReadOnly = true;

            gcPatientXray.Columns.Add(colEmpID);
            gcPatientXray.Columns.Add(colPostion);
            gcPatientXray.Columns.Add(colDepartment);
            gcPatientXray.Columns.Add(colCompany);
            gcPatientXray.Columns.Add(colCheckupDate);

            colNoExcel.Visible = true;
            colNoExcel.VisibleIndex = 0;

            colNoSequnce.Visible = false;
        }

        public void RemoveColumnMobile()
        {
            gcPatientXray.Columns.Remove(colEmpID);
            gcPatientXray.Columns.Remove(colPostion);
            gcPatientXray.Columns.Remove(colDepartment);
            gcPatientXray.Columns.Remove(colCompany);

            colNoExcel.Visible = false;
            colNoSequnce.Visible = true;
            colNoSequnce.VisibleIndex = 0;

        }
        #endregion
    }
}
