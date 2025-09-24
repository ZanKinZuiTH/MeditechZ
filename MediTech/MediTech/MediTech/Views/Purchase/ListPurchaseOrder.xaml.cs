using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid.LookUp;
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
    /// Interaction logic for ListPurchaseOrder.xaml
    /// </summary>
    public partial class ListPurchaseOrder : UserControl
    {
        public ListPurchaseOrder()
        {
            InitializeComponent();
            lkeOrganisation.PreviewKeyDown += LookUpEdit_PreviewKeyDown;
            cmbStore.PreviewKeyDown += ComboBoxEdit_PreviewKeyDown;
            lkeVendor.PreviewKeyDown += LookUpEdit_PreviewKeyDown;
            cmbPOStatus.PreviewKeyDown += ComboBoxEdit_PreviewKeyDown;

            if (this.DataContext is ListPurchaseOrderViewModel)
            {
                (this.DataContext as ListPurchaseOrderViewModel).UpdateEvent += ListPurchaseOrder_UpdateEvent;
            }
        }

        void ListPurchaseOrder_UpdateEvent(object sender, EventArgs e)
        {
            grdPurchaseOrder.RefreshData();
        }

        private void LookUpEdit_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete && e.Key != Key.Back)
                return;

            LookUpEdit edit = (LookUpEdit)sender;
            if (!edit.AllowNullInput)
                return;

            edit.EditValue = null;
        }

        private void ComboBoxEdit_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete && e.Key != Key.Back)
                return;

            ComboBoxEdit edit = (ComboBoxEdit)sender;
            if (!edit.AllowNullInput || edit.SelectionLength != edit.Text.Length)
                return;

            edit.EditValue = null;
            e.Handled = true;
        }
    }
}
