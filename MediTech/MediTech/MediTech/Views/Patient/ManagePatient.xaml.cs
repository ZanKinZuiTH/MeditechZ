using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class ManagePatient : UserControl
    {
        public static readonly DependencyProperty IsRegisterPatientProperty = DependencyProperty.RegisterAttached("IsRegisterPatientProperty",
            typeof(bool),
            typeof(ManagePatient),
            new PropertyMetadata(false));
        public ManagePatient()
        {
            InitializeComponent();
            this.Loaded += ManagePatient_Loaded;
        }


        void ManagePatient_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsRegisterPatient)
            {
                visitGroup.Visibility = System.Windows.Visibility.Visible;
                btnSavePatient.Visibility = System.Windows.Visibility.Hidden;
                layoutSearchPatient.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                visitGroup.Visibility = System.Windows.Visibility.Hidden;
                lytBithDate.Visibility = System.Windows.Visibility.Collapsed;
                btnSavePatient.Visibility = System.Windows.Visibility.Visible;
                layoutSearchPatient.Visibility = System.Windows.Visibility.Visible;
            }
        }

        public bool IsRegisterPatient
        {
            get { return (bool)GetValue(IsRegisterPatientProperty); }
            set { SetValue(IsRegisterPatientProperty, value); }
        }
    }
}
