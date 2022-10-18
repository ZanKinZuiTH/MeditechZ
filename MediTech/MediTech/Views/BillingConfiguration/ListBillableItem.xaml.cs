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
    /// Interaction logic for BillableItem.xaml
    /// </summary>
    public partial class ListBillableItem : UserControl
    {
        public ListBillableItem()
        {
            InitializeComponent();
            if (this.DataContext != null)
            {
                if (this.DataContext is ListBillableItemViewModel)
                {
                    (this.DataContext as ListBillableItemViewModel).UpdateEvent += BillableItem_UpdateEvent;
                }

            }
        }

        private void BillableItem_UpdateEvent(object sender, EventArgs e)
        {
            grdBillableItem.RefreshData();
            gvBillableItem.BestFitColumns();
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (true)
            {

            }
        }
    }
}

