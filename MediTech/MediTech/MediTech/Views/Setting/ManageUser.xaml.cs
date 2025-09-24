using DevExpress.Data.Filtering;
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
    /// Interaction logic for UserManage.xaml
    /// </summary>
    public partial class ManageUser : UserControl
    {
        public ManageUser()
        {
            InitializeComponent();
            if (this.DataContext is ManageUserViewModel)
            {
                (this.DataContext as ManageUserViewModel).UpdateEvent += ManageUser_UpdateEvent;
            }
        }

        void ManageUser_UpdateEvent(object sender, EventArgs e)
        {
            grdRoleProfile.RefreshData();
        }
    }
}
