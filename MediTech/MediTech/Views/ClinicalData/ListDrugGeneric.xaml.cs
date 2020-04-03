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
    /// Interaction logic for DrugGeneric.xaml
    /// </summary>
    public partial class ListDrugGeneric : UserControl
    {
        public ListDrugGeneric()
        {
            InitializeComponent();
            if (this.DataContext != null)
            {
                if (this.DataContext is ListDrugGenericViewModel)
                {
                    (this.DataContext as ListDrugGenericViewModel).UpdateEvent += DrugGenerics_UpdateEvent;
                }

            }
        }

        private void DrugGenerics_UpdateEvent(object sender, EventArgs e)
        {
            grdDrugGeneric.RefreshData();
        }
    }
}
