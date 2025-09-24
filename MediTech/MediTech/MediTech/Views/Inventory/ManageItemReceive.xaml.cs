using DevExpress.Xpf.Grid;
using MediTech.Models;
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
    /// Interaction logic for ManageItemReceive.xaml
    /// </summary>
    public partial class ManageItemReceive : UserControl
    {
        public ManageItemReceive()
        {
            InitializeComponent();
            view.PreviewKeyDown += view_PreviewKeyDown;
            view.ValidateRow += view_ValidateRow;
            view.InvalidRowException += view_InvalidRowException;
        }

        void view_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (view.FocusedRowHandle >= 0)
                {
                    System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("คุณต้องการลบข้อมูลที่เลือก ใช่หรือไม่ ?", "Question", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        view.DeleteRow(view.FocusedRowHandle);
                    }
                }
            }
        }
        private void view_ValidateRow(object sender, DevExpress.Xpf.Grid.GridRowValidationEventArgs e)
        {
            if (e.Row == null) return;
            ItemMasterList newItem = (ItemMasterList)e.Row;
            if (newItem.ItemMasterUID == 0)
            {
                e.IsValid = false;
                MessageBox.Show("กรุณาเลือกรายการสินค้า", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
                return;
            }
            if (newItem.Quantity == null | newItem.Quantity <= 0)
            {
                e.IsValid = false;
                MessageBox.Show("กรุณาใส่จำนวนที่รับเข้า", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
