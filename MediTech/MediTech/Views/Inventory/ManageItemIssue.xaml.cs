using DevExpress.Xpf.Grid;
using MediTech.Models;
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
    /// Interaction logic for ManageItemIssue.xaml
    /// </summary>
    public partial class ManageItemIssue : UserControl
    {
        public ManageItemIssue()
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
                    System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("คุณต้องการลบข้อมูลที่เลือก ใช้หรือไม่ ?", "Question", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        view.DeleteRow(view.FocusedRowHandle);
                        (this.DataContext as ManageItemIssueViewModel).CellChangeValue(null);
                    }


                }
            }
        }
        private void view_ValidateRow(object sender, DevExpress.Xpf.Grid.GridRowValidationEventArgs e)
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
            //if (newItem.UnitPrice == null || newItem.UnitPrice <= 0)
            //{
            //    e.IsValid = false;
            //    MessageBox.Show("กรุณาใส่ราคาส่งออก", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    e.Handled = true;
            //    return;
            //}
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

        void view_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }

    }
}
