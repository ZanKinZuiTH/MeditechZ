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
    /// Interaction logic for ManageOrganisation.xaml
    /// </summary>
    public partial class ManageOrganisation : UserControl
    {
        public ManageOrganisation()
        {
            InitializeComponent();
            if (this.DataContext is ManageOrganisationViewModel)
            {
                (this.DataContext as ManageOrganisationViewModel).UpdateEvent += ManageOrganisation_UpdateEvent; ;
            }
        }

        private void ManageOrganisation_UpdateEvent(object sender, EventArgs e)
        {
            grdHealthID.RefreshData();
        }
    }
}
