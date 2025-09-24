using DevExpress.Xpf.Grid;
using MediTech.Model;
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
    /// Interaction logic for DisposeStock.xaml
    /// </summary>
    public partial class DisposeStock : UserControl
    {
        public DisposeStock()
        {
            InitializeComponent();
            view.ValidateRow += view_ValidateRow;
            view.InvalidRowException += view_InvalidRowException;
        }

        private void view_ValidateRow(object sender, DevExpress.Xpf.Grid.GridRowValidationEventArgs e)
        {
            if (e.Row == null) return;
            StockModel newItem = (StockModel)e.Row;

            if (newItem.DisposeQty > newItem.Quantity)
            {
                e.IsValid = false;
                MessageBox.Show("จำนวนที่จะทำลายทิ้งมากว่าจำนวนในคลัง โปรดตรวจสอบ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
                return;
            }

            grid.RefreshData();
        }

        void view_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
    }
}
