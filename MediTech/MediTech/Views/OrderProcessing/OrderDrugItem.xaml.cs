using DevExpress.Xpf.Editors;
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
    /// Interaction logic for OrderDrug.xaml
    /// </summary>
    public partial class OrderDrugItem : UserControl
    {
        public OrderDrugItem(BillableItemModel billablItem, int ownerOrganisationUID, int encounterTypeUID, DateTime? startDttm = null)
        {
            InitializeComponent();
            if (this.DataContext is OrderDrugItemViewModel)
            {
                DateTime now = DateTime.Now;
                (this.DataContext as OrderDrugItemViewModel).OwnerOrgansitaionUID = ownerOrganisationUID;
                (this.DataContext as OrderDrugItemViewModel).EncounterTypeUID = encounterTypeUID;

                (this.DataContext as OrderDrugItemViewModel).BillableItem = billablItem;
                (this.DataContext as OrderDrugItemViewModel).StartDate = startDttm != null ? startDttm.Value.Date : now.Date;
                (this.DataContext as OrderDrugItemViewModel).StartTime = startDttm != null ? startDttm.Value : now;
                (this.DataContext as OrderDrugItemViewModel).BindingFromBillableItem();
            }
        }






        public OrderDrugItem(PatientOrderDetailModel orderDetail)
        {
            InitializeComponent();
            if (this.DataContext is OrderDrugItemViewModel)
            {
                (this.DataContext as OrderDrugItemViewModel).PatientOrderDetail = orderDetail;
                (this.DataContext as OrderDrugItemViewModel).BindingFromPatientOrderDetail();
            }
        }

    }
}
