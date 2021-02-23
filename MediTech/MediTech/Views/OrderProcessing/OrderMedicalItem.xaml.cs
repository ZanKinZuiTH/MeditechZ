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
    /// Interaction logic for OrderMedicalItem.xaml
    /// </summary>
    public partial class OrderMedicalItem : UserControl
    {


        public OrderMedicalItem(BillableItemModel billablItem, int ownerOrganisationUID)
        {
            InitializeComponent();
            if (this.DataContext is OrderMedicalItemViewModel)
            {
                (this.DataContext as OrderMedicalItemViewModel).OwnerOrgansitaionUID = ownerOrganisationUID;
                (this.DataContext as OrderMedicalItemViewModel).BillableItem = billablItem;
                (this.DataContext as OrderMedicalItemViewModel).BindingFromBillableItem();
            }
        }

        public OrderMedicalItem(PatientOrderDetailModel orderDetail)
        {
            InitializeComponent();
            if (this.DataContext is OrderMedicalItemViewModel)
            {
                (this.DataContext as OrderMedicalItemViewModel).PatientOrderDetail = orderDetail;
                (this.DataContext as OrderMedicalItemViewModel).BindingFromPatientOrderDetail();
            }
        }
    }
}
