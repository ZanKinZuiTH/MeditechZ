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
    /// Interaction logic for ManageUserOrganisation.xaml
    /// </summary>
    public partial class ManageUserOrganisation : UserControl
    {
        public ManageUserOrganisation()
        {
            InitializeComponent();
            if(this.DataContext is ManageUserOrganisationViewModel)
            {
                (this.DataContext as ManageUserOrganisationViewModel).UpdateEvent += ManageUserOrganisation_UpdateEvent; ;
            }
        }

        private void ManageUserOrganisation_UpdateEvent(object sender, EventArgs e)
        {
            grdUsersLocation.RefreshData();
        }
    }
}
