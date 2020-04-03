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
using MediTech.ViewModels;

namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for ListPayorDetail.xaml
    /// </summary>
    public partial class ListPayorDetail : UserControl
    {
        public ListPayorDetail()
        {
            InitializeComponent();
            if (this.DataContext != null)
            {
                if (this.DataContext is ListPayorDetailViewModel)
                {
                    (this.DataContext as ListPayorDetailViewModel).UpdateEvent += ListPayorDetail_UpdateEvent;
                }

            }
        }

        void ListPayorDetail_UpdateEvent(object sender, EventArgs e)
        {
            grdPayorDetail.RefreshData();
        }
    }
}
