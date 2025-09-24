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
    /// Interaction logic for Store.xaml
    /// </summary>
    public partial class ListStores : UserControl
    {
        public ListStores()
        {
            InitializeComponent();
            if (this.DataContext is ListStoresViewModel)
            {
                (this.DataContext as ListStoresViewModel).UpdateEvent += ListStores_UpdateEvent;
            }
        }

        void ListStores_UpdateEvent(object sender, EventArgs e)
        {
            grdStore.RefreshData();
        }
    }
}
