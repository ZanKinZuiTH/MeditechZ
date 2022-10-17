using MediTech.ViewModels;
using MediTech.ViewModels.Doctor;
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
    /// Interaction logic for PatientList.xaml
    /// </summary>
    public partial class PatientListForDoctor : UserControl
    {
        public PatientListForDoctor()
        {
            InitializeComponent();
            (this.DataContext as PatientListForDoctorViewModel).UpdateEvent += PatientListForDoctor_UpdateEvent;
        }

        private void PatientListForDoctor_UpdateEvent(object sender, EventArgs e)
        {
            grdVisitList.RefreshData();
        }
    }
}
