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
    /// Interaction logic for OrderGroupReceipt.xaml
    /// </summary>
    public partial class OrderGroupReceipt : UserControl
    {
        public OrderGroupReceipt()
        {
            InitializeComponent();
        }
        public OrderGroupReceipt(OrderSetModel orderSet, int? ownerOrganisationUID, string typeOrder)
        {
            InitializeComponent();
            if (this.DataContext is OrderGroupReceiptViewModel)
            {
                (this.DataContext as OrderGroupReceiptViewModel).OwnerOrgansitaion = ownerOrganisationUID;
                (this.DataContext as OrderGroupReceiptViewModel).OrderSet = orderSet;
                (this.DataContext as OrderGroupReceiptViewModel).TypeOrder = typeOrder;
                (this.DataContext as OrderGroupReceiptViewModel).BindingFromOrderset();
            }
        }
        public OrderGroupReceipt(BillableItemModel billablItem, int? ownerOrganisationUID, string typeOrder)
        {
            InitializeComponent();
            if (this.DataContext is OrderGroupReceiptViewModel)
            {
                (this.DataContext as OrderGroupReceiptViewModel).OwnerOrgansitaion = ownerOrganisationUID;
                (this.DataContext as OrderGroupReceiptViewModel).BillableItem = billablItem;
                (this.DataContext as OrderGroupReceiptViewModel).TypeOrder = typeOrder;
                (this.DataContext as OrderGroupReceiptViewModel).BindingFromBillableItem();
            }
        }

    }
}
