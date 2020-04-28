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
    /// Interaction logic for CheckupReport.xaml
    /// </summary>
    public partial class CheckupReport : UserControl
    {
        public CheckupReport()
        {
            InitializeComponent();
        }

        private void PivotData_CustomCellAppearance(object sender, DevExpress.Xpf.PivotGrid.PivotCustomCellAppearanceEventArgs e)
        {
            if (e.DataField.FieldName == "ResultValue")
            {
                if (e.Value != null)
                {
                    string[] values = e.Value.ToString().Split(' ');
                    string IsAbnormal = values?[1];
                    if (IsAbnormal == "H")
                    {
                        e.Foreground = Brushes.Red;
                    }
                    else if(IsAbnormal == "L")
                    {
                        e.Foreground = Brushes.Blue;
                    }
                }

            }
        }
    }
}
