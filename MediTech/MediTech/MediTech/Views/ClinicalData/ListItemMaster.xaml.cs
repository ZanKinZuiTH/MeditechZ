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
    /// Interaction logic for ItemMaster.xaml
    /// </summary>
    public partial class ListItemMaster : UserControl
    {
        public ListItemMaster()
        {
            InitializeComponent();
            if (this.DataContext is ListItemMasterViewModel)
            {
                (this.DataContext as ListItemMasterViewModel).UpdateEvent += ListItemMaster_UpdateEvent;   
            }
        }

        void ListItemMaster_UpdateEvent(object sender, EventArgs e)
        {
            grdItemMaster.RefreshData();
        }
    }
}
