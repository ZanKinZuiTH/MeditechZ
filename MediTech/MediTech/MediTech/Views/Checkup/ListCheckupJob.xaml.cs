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
    /// Interaction logic for ListCheckupJob.xaml
    /// </summary>
    public partial class ListCheckupJob : UserControl
    {
        public ListCheckupJob()
        {
            InitializeComponent();
            if (this.DataContext is ListCheckupJobViewModel)
            {
                (this.DataContext as ListCheckupJobViewModel).UpdateEvent += ListCheckupJob_UpdateEvent;
            }
        }

        private void ListCheckupJob_UpdateEvent(object sender, EventArgs e)
        {
            grdCheckupJob.RefreshData();
        }
    }
}
