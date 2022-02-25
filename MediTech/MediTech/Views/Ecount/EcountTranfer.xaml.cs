using DevExpress.Xpf.Grid;
using MediTech.Models;
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
    /// Interaction logic for EcountTranfer.xaml
    /// </summary>
    public partial class EcountTranfer : UserControl
    {
        public EcountTranfer()
        {
            InitializeComponent();
            view.PreviewKeyDown += View_PreviewKeyDown;
            view.ValidateRow += View_ValidateRow;
            view.InvalidRowException += View_InvalidRowException;
        }

        private void View_InvalidRowException(object sender, DevExpress.Xpf.Grid.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }

        private void View_ValidateRow(object sender, DevExpress.Xpf.Grid.GridRowValidationEventArgs e)
        {
            if (e.Row == null) return;
            ItemMasterList newItem = (ItemMasterList)e.Row;
            var rowAlredy = ((ObservableCollection<ItemMasterList>)grdIssueItemList.ItemsSource).Count(p => p.StockUID == newItem.StockUID);

            if (rowAlredy >= 2)
            {
                e.IsValid = false;
                MessageBox.Show("เลือกรายการในคลังซ้ำกัน โปรดตรวจสอบ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
                return;
            }

            if (newItem.ItemMasterUID == 0)
            {
                e.IsValid = false;
                MessageBox.Show("กรุณาเลือกรายการสินค้า", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
                return;
            }
            if (newItem.Quantity == null || newItem.Quantity <= 0)
            {
                e.IsValid = false;
                MessageBox.Show("กรุณาใส่จำนวนส่งออก", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
                return;
            }
            if (newItem.Quantity > newItem.BatchQuantity)
            {
                e.IsValid = false;
                MessageBox.Show("จำนวนส่งออกมากว่าจำนวนในคลัง โปรดตรวจสอบ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
                return;
            }

            newItem.ShowBatchQuantity = newItem.BatchQuantity - (newItem.Quantity ?? 0);
            grdIssueItemList.RefreshData();
        }

        private void View_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (view.FocusedRowHandle >= 0)
                {
                    System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("คุณต้องการลบข้อมูลที่เลือก ใช้หรือไม่ ?", "Question", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        view.DeleteRow(view.FocusedRowHandle);
                    }


                }
            }
        }
    }
}
