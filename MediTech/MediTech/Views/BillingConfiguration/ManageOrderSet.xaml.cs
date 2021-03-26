using DevExpress.Data.Filtering;
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
using System.Collections.ObjectModel;

namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for ManageOrderSet.xaml
    /// </summary>
    public partial class ManageOrderSet : UserControl
    {
        MediTechViewModelBase viewModel;
        public ManageOrderSet()
        {
            InitializeComponent();
            if (this.DataContext is ManageOrderSetViewModel)
            {
                viewModel = (this.DataContext as ManageOrderSetViewModel);
                viewModel.UpdateEvent += ManageOrderSet_UpdateEvent;
            }
        }

        void ManageOrderSet_UpdateEvent(object sender, EventArgs e)
        {
            grdOrderSetBill.ItemsSource = (viewModel as ManageOrderSetViewModel).OrderSetBillableItems.Where(p => p.StatusFlag == "A");
            grdOrderSetBill.RefreshData();
        }

    }
}
