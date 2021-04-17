using MediTech.ViewModels;
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
    /// Interaction logic for CheckupRule.xaml
    /// </summary>
    public partial class VerifyChekupResult : UserControl
    {
        public VerifyChekupResult()
        {
            InitializeComponent();
            gvResult.CustomCellAppearance += GvResult_CustomCellAppearance;
            txtWellness.LostFocus += TxtWellness_LostFocus;
        }



        private void GvResult_CustomCellAppearance(object sender, DevExpress.Xpf.Grid.CustomCellAppearanceEventArgs e)
        {
            try
            {
                if (e.Column != null && e.Column.FieldName == "ResultValue1" && e.Property == TextBlock.ForegroundProperty)
                {
                    var cellValue = gcResult.GetCellValue(e.RowHandle, e.Column.FieldName);
                    if (cellValue != null && cellValue.ToString() != "")
                    {
                        string[] values = cellValue.ToString().Split(' ');
                        if (values != null && values.Count() > 1)
                        {
                            string IsAbnormal = "";
                            if (values?[1] == "R" && values.Count() == 3)
                            {
                                IsAbnormal = values?[2];
                            }
                            else
                            {
                                IsAbnormal = values?[1];
                            }
                            if (IsAbnormal == "H")
                            {
                                e.Result = new SolidColorBrush(Colors.Red);
                                e.Handled = true;
                            }
                            else if (IsAbnormal == "L")
                            {
                                e.Result = new SolidColorBrush(Colors.Blue);
                                e.Handled = true;
                            }
                        }
                    }

                }

            }
            catch (Exception)
            {

            }
        }

        private void TxtWellness_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is VerifyChekupResultViewModel)
            {
                var viewModel = (this.DataContext as VerifyChekupResultViewModel);
                string[] locResult = txtWellness.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                viewModel.ListGroupResult = locResult.ToList();
            }
        }
    }
}
