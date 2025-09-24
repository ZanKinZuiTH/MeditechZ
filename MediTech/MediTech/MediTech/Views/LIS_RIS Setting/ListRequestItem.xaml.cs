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
    /// Interaction logic for ListRequestItem.xaml
    /// </summary>
    public partial class ListRequestItem : UserControl
    {
        public ListRequestItem()
        {
            InitializeComponent();
            if (this.DataContext != null)
            {
                if (this.DataContext is ListRequestItemViewModel)
                {
                    (this.DataContext as ListRequestItemViewModel).UpdateEvent += ListRequestItemViewModel_UpdateEvent;
                }

            }
        }

        void ListRequestItemViewModel_UpdateEvent(object sender, EventArgs e)
        {
            grdRequestItem.RefreshData();
        }
    }
}
