using DevExpress.Xpf.Editors;
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
    /// Interaction logic for DoctorRoom.xaml
    /// </summary>
    public partial class DoctorRoom : UserControl
    {
        public DoctorRoom()
        {
            InitializeComponent();
            cmbDoctor.PreviewKeyDown += ComboBoxEdit_PreviewKeyDown;
            cmbStatus.PreviewKeyDown += ComboBoxEdit_PreviewKeyDown;
            (electronicMedicalView.DataContext as EMRViewViewModel).View = electronicMedicalView;
        }

        private void ComboBoxEdit_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete && e.Key != Key.Back)
                return;

            ComboBoxEdit edit = (ComboBoxEdit)sender;
            if (!edit.AllowNullInput || edit.SelectionLength != edit.Text.Length)
                return;

            edit.EditValue = null;
            e.Handled = true;
        }
    }
}
