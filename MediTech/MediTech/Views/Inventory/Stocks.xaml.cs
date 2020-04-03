using DevExpress.Xpf.Editors;
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
    /// Interaction logic for Stocks.xaml
    /// </summary>
    public partial class Stocks : UserControl
    {
        public Stocks()
        {
            InitializeComponent();
            lkeOrganisationBalance.PreviewKeyDown += LookUpEdit_PreviewKeyDown;
            lkeOrganisationMovement.PreviewKeyDown += LookUpEdit_PreviewKeyDown;
            lkeOrganisationOnHand.PreviewKeyDown += LookUpEdit_PreviewKeyDown;
            cmbStoreBalance.PreviewKeyDown += ComboBoxEdit_PreviewKeyDown;
            cmbStoreMovement.PreviewKeyDown += ComboBoxEdit_PreviewKeyDown;
            cmbStoreOnHand.PreviewKeyDown += ComboBoxEdit_PreviewKeyDown;
            cmbItemType.PreviewKeyDown += ComboBoxEdit_PreviewKeyDown;
            cmbTranType.PreviewKeyDown += ComboBoxEdit_PreviewKeyDown;

        }

        private void LookUpEdit_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete && e.Key != Key.Back)
                return;

            LookUpEdit edit = (LookUpEdit)sender;
            if (!edit.AllowNullInput)
                return;

            edit.EditValue = null;
        }
        private void ComboBoxEdit_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete && e.Key != Key.Back)
                return;

            ComboBoxEdit edit = (ComboBoxEdit)sender;
            if (!edit.AllowNullInput)
                return;

            edit.EditValue = null;
        }
    }
}
