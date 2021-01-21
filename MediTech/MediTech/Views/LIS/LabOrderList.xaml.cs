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
    /// Interaction logic for ResultsEntry.xaml
    /// </summary>
    public partial class LabOrderList : UserControl
    {
        public LabOrderList()
        {
            InitializeComponent();
            (this.DataContext as LabOrderListViewModel).UpdateEvent += LabOrderList_UpdateEvent;
        }

        private void LabOrderList_UpdateEvent(object sender, EventArgs e)
        {
            grdRequestItemList.RefreshData();
        }
    }
}
