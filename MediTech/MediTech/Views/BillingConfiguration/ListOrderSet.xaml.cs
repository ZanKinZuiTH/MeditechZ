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
    /// Interaction logic for ListOrderSet.xaml
    /// </summary>
    public partial class ListOrderSet : UserControl
    {
        public ListOrderSet()
        {
            InitializeComponent();
            if (this.DataContext != null)
            {
                if (this.DataContext is ListOrderSetViewModel)
                {
                    (this.DataContext as ListOrderSetViewModel).UpdateEvent += OrderSets_UpdateEvent;
                }

            }
        }

        private void OrderSets_UpdateEvent(object sender, EventArgs e)
        {
            grdOrderSet.RefreshData();
            //gvOrderSet.BestFitColumns();
        }
    }
}
