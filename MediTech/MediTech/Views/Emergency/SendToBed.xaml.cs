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
using System.Windows.Shapes;

namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for SendToBed.xaml
    /// </summary>
    public partial class SendToBed : Window
    {
        public ActionDialog ResultDialog = ActionDialog.Cancel;

        public SendToBed()
        {
            InitializeComponent();
        }
        void PatientStatus_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
