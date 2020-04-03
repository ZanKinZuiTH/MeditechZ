using DevExpress.Xpf.Grid.LookUp;
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
    /// Interaction logic for ListStockWorkList.xaml
    /// </summary>
    public partial class ListStockWorkList : UserControl
    {
        public ListStockWorkList()
        {
            InitializeComponent();
            lkeOrganisation.PreviewKeyDown += lkeOrganisation_PreviewKeyDown;
        }

        void lkeOrganisation_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete && e.Key != Key.Back)
                return;

            LookUpEdit edit = (LookUpEdit)sender;
            if (!edit.AllowNullInput)
                return;

            edit.EditValue = null;
        }
    }
}
