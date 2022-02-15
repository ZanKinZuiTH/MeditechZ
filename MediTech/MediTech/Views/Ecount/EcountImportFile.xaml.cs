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
using MediTech.Models;
using DevExpress.Xpf.Grid;

namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for EcountImportFile.xaml
    /// </summary>
    public partial class EcountImportFile : UserControl
    {

        public EcountImportFile()
        {
            InitializeComponent();
            view.PreviewKeyDown += View_PreviewKeyDown;
            view.ValidateRow += View_ValidateRow;
            view.InvalidRowException += View_InvalidRowException;
            if (this.DataContext is EcountImportFile)
            {
                (this.DataContext as ManageGRNViewModel).UpdateEvent += ManageGRNViewModel_UpdateEvent;
            }


        }

        private void View_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void View_ValidateRow(object sender, GridRowValidationEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void View_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        void ManageGRNViewModel_UpdateEvent(object sender, EventArgs e)
        {
            grdPurchaseItemList.RefreshData();
        }




    }  
}

