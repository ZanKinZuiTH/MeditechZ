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
    /// Interaction logic for ListRadiologistTemplate.xaml
    /// </summary>
    public partial class ListRadiologistTemplate : UserControl
    {
        public ListRadiologistTemplate()
        {
            InitializeComponent();
            if (this.DataContext is ListRadiologistTemplateViewModel)
            {
                (this.DataContext as ListRadiologistTemplateViewModel).UpdateEvent += ListRadiologistTemplate_UpdateEvent;
            }
        }

        void ListRadiologistTemplate_UpdateEvent(object sender, EventArgs e)
        {
            grdResultTemplate.RefreshData();
        }
    }
}
