using MediTech.Interface;
using MediTech.Model;
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
    /// Interaction logic for PatientDiagnosis.xaml
    /// </summary>
    public partial class PatientDiagnosis : UserControl
    {
        
        public PatientDiagnosis()
        {
            InitializeComponent();
            if (this.DataContext is PatientDiagnosisViewModel)
            {
                (this.DataContext as PatientDiagnosisViewModel).UpdateEvent += PatientDiagnosisViewModel_UpdateEvent;
            }
        }

        public void RefershData()
        {
            
        }

        void PatientDiagnosisViewModel_UpdateEvent(object sender, EventArgs e)
        {
            grdPatientProblem.RefreshData();
        }

    }
}
