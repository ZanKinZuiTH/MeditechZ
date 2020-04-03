using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid.LookUp;
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
    /// Interaction logic for ListItemReceive.xaml
    /// </summary>
    public partial class ListItemReceive : UserControl
    {
        public ListItemReceive()
        {
            InitializeComponent();
            lkeOrganisationFrom.PreviewKeyDown += ComboBoxEdit_PreviewKeyDown;
            lkeOrganisationTo.PreviewKeyDown += ComboBoxEdit_PreviewKeyDown;
            if (this.DataContext is ListItemReceiveViewModel)
            {
                (this.DataContext as ListItemReceiveViewModel).UpdateEvent += ListItemReceive_UpdateEvent;
            }
        }


        void ListItemReceive_UpdateEvent(object sender, EventArgs e)
        {
            grdItemReceive.RefreshData();
        }

        private void ComboBoxEdit_PreviewKeyDown(object sender, KeyEventArgs e)
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
