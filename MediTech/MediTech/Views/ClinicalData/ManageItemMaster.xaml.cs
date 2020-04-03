using DevExpress.Data.Filtering;
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
using MediTech.ViewModels;

namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for ManageItemMaster.xaml
    /// </summary>
    public partial class ManageItemMaster : UserControl
    {
        public ManageItemMaster()
        {
            InitializeComponent();
            viewConvertUnit.ValidateRow += view_ValidateRow;
            viewConvertUnit.InvalidRowException += view_InvalidRowException;
            viewVendor.ValidateRow += viewVendor_ValidateRow;
            viewVendor.InvalidRowException += viewVendor_InvalidRowException;

            if (this.DataContext is ManageItemMasterViewModel)
            {
                (this.DataContext as ManageItemMasterViewModel).UpdateEvent += ManageItemMaster_UpdateEvent;
            }
        }

        void ManageItemMaster_UpdateEvent(object sender, EventArgs e)
        {
            grdConvertUOM.RefreshData();
            grdItemVendorDetail.RefreshData();
        }



        void viewVendor_ValidateRow(object sender, GridRowValidationEventArgs e)
        {
            if (e.Row == null) return;
            ItemVendorDetailModel newItem = (ItemVendorDetailModel)e.Row;
            if (newItem.VendorDetailUID == 0)
            {
                e.IsValid = false;
                MessageBox.Show("กรุณาเลือกผู้จัดจำหน่าย", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
                return;
            }
            if (newItem.ItemAmount <= 0)
            {
                e.IsValid = false;
                MessageBox.Show("กรุณาใส่ราคาต่อหน่วย", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
                return;
            }
        }

        void viewVendor_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
        private void view_ValidateRow(object sender, DevExpress.Xpf.Grid.GridRowValidationEventArgs e)
        {
            if (e.Row == null) return;
            ItemUOMConversionModel newItem = (ItemUOMConversionModel)e.Row;
            if (newItem.ConversionUOMUID == 0)
            {
                e.IsValid = false;
                MessageBox.Show("กรุณาเลือกหน่วย", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
                return;
            }
            if (newItem.ConversionValue <= 0)
            {
                e.IsValid = false;
                MessageBox.Show("กรุณาใส่ค่า", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
