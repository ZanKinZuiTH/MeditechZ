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
    /// Interaction logic for CreateOrderItem.xaml
    /// </summary>
    public partial class PatientOrderEntry : UserControl
    {
        public PatientOrderEntry()
        {
            InitializeComponent();
            if (this.DataContext is PatientOrderEntryViewModel)
            {
                (this.DataContext as PatientOrderEntryViewModel).UpdateEvent += OrderDetail_UpdateEvent;
            }
        }

        void OrderDetail_UpdateEvent(object sender, EventArgs e)
        {
            grdOrderDetail.RefreshData();
            gvOrderDetail.BestFitColumn(colItemName);
        }

    }
}
