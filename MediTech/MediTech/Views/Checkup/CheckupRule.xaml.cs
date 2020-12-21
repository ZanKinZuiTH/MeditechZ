using DevExpress.Xpf.Grid;
using MediTech.Model;
using MediTech.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class CheckupRule : UserControl
    {
        public CheckupRule()
        {
            InitializeComponent();
            gvTextMaster.ValidateRow += GvTextMaster_ValidateRow;
            gvTextMaster.InvalidRowException += GvTextMaster_InvalidRowException;
        }


        private void GvTextMaster_InvalidRowException(object sender, DevExpress.Xpf.Grid.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }

        private void GvTextMaster_ValidateRow(object sender, DevExpress.Xpf.Grid.GridRowValidationEventArgs e)
        {
            if (e.Row == null) return;
            CheckupTextMasterModel newItem = (CheckupTextMasterModel)e.Row;
            var checupTextDataSource = (gcTextMaster.ItemsSource as ObservableCollection<CheckupTextMasterModel>);
            if (checupTextDataSource != null && checupTextDataSource.Any(p => p.ThaiWord == newItem.ThaiWord && !p.Equals(newItem)))
            {
                e.IsValid = false;
                MessageBox.Show("มีข้อความนี้อยู่แล้ว", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                e.Handled = true;
                return;
            }
        }
    }
}
