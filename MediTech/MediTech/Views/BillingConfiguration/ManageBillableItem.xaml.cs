using DevExpress.Xpf.Editors;
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
using DevExpress.Data.Filtering;
using MediTech.Model;
using System.Collections.ObjectModel;

namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for ManageBillableItem.xaml
    /// </summary>
    public partial class ManageBillableItem : UserControl
    {
        MediTechViewModelBase viewModel;
        public ManageBillableItem()
        {
            InitializeComponent();
            cmbOrganisation.PreviewKeyDown += ComboBoxEdit_PreviewKeyDown;
            if (this.DataContext is ManageBillableItemViewModel)
            {
                viewModel = (this.DataContext as ManageBillableItemViewModel);
                viewModel.UpdateEvent += ManageBillableItem_UpdateEvent;
            }
        }

        void ManageBillableItem_UpdateEvent(object sender, EventArgs e)
        {
            grdBillDetail.ItemsSource = (viewModel as ManageBillableItemViewModel).BillableItemDetail.Where(p => p.StatusFlag == "A");
            grdBillDetail.RefreshData();
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
