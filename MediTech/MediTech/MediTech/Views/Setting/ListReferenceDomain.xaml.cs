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
    /// Interaction logic for ReferenceDomain.xaml
    /// </summary>
    public partial class ListReferenceDomain : UserControl
    {
        public ListReferenceDomain()
        {
            InitializeComponent();
            if (this.DataContext != null)
            {
                if (this.DataContext is ListReferenceDomainViewModel)
                {
                    (this.DataContext as ListReferenceDomainViewModel).UpdateEvent += ReferenceDomain_UpdateEvent;
                }

            }
        }

        void ReferenceDomain_UpdateEvent(object sender, EventArgs e)
        {
            grdReferenceDomain.RefreshData();
        }
    }
}
