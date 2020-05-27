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
    /// Interaction logic for DispenseDrugs.xaml
    /// </summary>
    public partial class CashierWorklist : UserControl
    {
        public CashierWorklist()
        {
            InitializeComponent();
            gvTotalCash.PreviewKeyDown += gvTotalCash_PreviewKeyDown;
            if (this.DataContext is CashierWorklistViewModel)
            {
                (this.DataContext as CashierWorklistViewModel).UpdateEvent += DispenseDrugs_UpdateEvent;
            }
        }



        void gvTotalCash_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (gvTotalCash.FocusedRowHandle >= 0)
                {

                    System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("คุณต้องการลบข้อมูลที่เลือก ใช้หรือไม่ ?", "Question", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        gvTotalCash.DeleteRow(gvTotalCash.FocusedRowHandle);
                    }


                }
            }
        }

        void DispenseDrugs_UpdateEvent(object sender, EventArgs e)
        {
            gTotalCash.RefreshData();
            gDrugDetail.RefreshData();
            gvTotalCash.BestFitColumn(colDescription);
            gvDrugDetail.BestFitColumn(colDrugname);
        }
    }
}
