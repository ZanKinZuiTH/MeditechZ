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
    /// Interaction logic for ManagePayorDetail.xaml
    /// </summary>
    public partial class ManagePayorDetail : UserControl
    {
        public ManagePayorDetail()
        {
            InitializeComponent();
            if (this.DataContext is ManagePayorDetailViewModel)
            {
                (this.DataContext as ManagePayorDetailViewModel).UpdateEvent += ManagePayorDetail_UpdateEvent;
            }
        }

        void ManagePayorDetail_UpdateEvent(object sender, EventArgs e)
        {
            grdPayorAgreement.RefreshData();
        }

        public ManagePayorDetail(bool PageDetail)
        {
            InitializeComponent();
            if (this.DataContext is ManagePayorDetailViewModel)
            {
                (this.DataContext as ManagePayorDetailViewModel).ObjectWindow(PageDetail);
            }
        }
    }
}
