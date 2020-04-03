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
    /// Interaction logic for ListItemRequest.xaml
    /// </summary>
    public partial class ListItemRequest : UserControl
    {
        public ListItemRequest()
        {
            InitializeComponent();
            lkeOrganisation.PreviewKeyDown += LookUpEdit_PreviewKeyDown;
            lkeOrganisationTo.PreviewKeyDown += LookUpEdit_PreviewKeyDown;
            cmbPriority.PreviewKeyDown += ComboBoxEdit_PreviewKeyDown;
            cmbRequestStatus.PreviewKeyDown += ComboBoxEdit_PreviewKeyDown;
            if (this.DataContext is ListItemRequestViewModel)
            {
                (this.DataContext as ListItemRequestViewModel).UpdateEvent += ListItemRequest_UpdateEvent;
            }
        }

        void ListItemRequest_UpdateEvent(object sender, EventArgs e)
        {
            grdItemRequest.RefreshData();
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
