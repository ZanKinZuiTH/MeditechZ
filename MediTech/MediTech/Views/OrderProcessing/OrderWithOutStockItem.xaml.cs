using MediTech.Model;
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
    /// Interaction logic for OrderLab.xaml
    /// </summary>
    public partial class OrderWithOutStockItem : UserControl
    {
        public OrderWithOutStockItem(BillableItemModel billablItem, int ownerOrganisationUID)
        {
            InitializeComponent();
            if (this.DataContext is OrderWithOutStockItemViewModel)
            {
                (this.DataContext as OrderWithOutStockItemViewModel).OwnerOrgansitaionUID = ownerOrganisationUID;
                (this.DataContext as OrderWithOutStockItemViewModel).BillableItem = billablItem;
                (this.DataContext as OrderWithOutStockItemViewModel).BindingFromBillableItem();
            }
        }

        public OrderWithOutStockItem(PatientOrderDetailModel orderDetail)
        {
            InitializeComponent();
            if (this.DataContext is OrderWithOutStockItemViewModel)
            {
                (this.DataContext as OrderWithOutStockItemViewModel).PatientOrderDetail = orderDetail;
                (this.DataContext as OrderWithOutStockItemViewModel).BindingFromPatientOrderDetail();
            }
        }

    }
}
