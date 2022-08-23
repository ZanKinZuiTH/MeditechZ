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
    /// Interaction logic for CreateDispenseReturn.xaml
    /// </summary>
    public partial class CreateDispenseReturn : UserControl
    {
        public CreateDispenseReturn()
        {
            InitializeComponent();
            view.ValidateRow += view_ValidateRow;
            view.InvalidRowException += view_InvalidRowException;
        }

        private void view_ValidateRow(object sender, DevExpress.Xpf.Grid.GridRowValidationEventArgs e)
        {
            if (e.Row == null) return;
            DispenseReturnModel newItem = (DispenseReturnModel)e.Row;
            if (newItem.ReturnQty > (newItem.DispensedQty - newItem.PreviousReturnQty))
            {
                e.IsValid = false;
                MessageBox.Show("จำนวนคืนมากกว่าจำนวน Dispensed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
                return;
            }
        }

        void view_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
    }
}
