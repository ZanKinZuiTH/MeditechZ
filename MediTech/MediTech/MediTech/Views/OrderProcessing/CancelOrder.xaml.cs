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
namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for CancelOrder.xaml
    /// </summary>
    public partial class CancelOrder : UserControl
    {
        public CancelOrder(List<PatientOrderDetailModel> ListOrderCancel)
        {
            InitializeComponent();
            if (this.DataContext is CancelOrderViewModel)
            {
                (this.DataContext as CancelOrderViewModel).ListOrderCancel = ListOrderCancel;
            }
        }
    }
}
