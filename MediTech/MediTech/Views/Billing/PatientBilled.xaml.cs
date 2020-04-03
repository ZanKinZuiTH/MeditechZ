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
    /// Interaction logic for PatientBilled.xaml
    /// </summary>
    public partial class PatientBilled : UserControl
    {
        public PatientBilled()
        {
            InitializeComponent();
            if (this.DataContext is PatientBilledViewModel)
            {
                (this.DataContext as PatientBilledViewModel).UpdateEvent += PatientBilledViewModel_UpdateEvent;
            }
        }

        void PatientBilledViewModel_UpdateEvent(object sender, EventArgs e)
        {
            grdPatBill.RefreshData();
        }
    }
}
