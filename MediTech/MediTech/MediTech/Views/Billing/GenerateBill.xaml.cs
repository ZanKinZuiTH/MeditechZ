using DevExpress.Xpf.Grid;
using MediTech.Model;
using MediTech.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for SettleBill.xaml
    /// </summary>
    public partial class GenerateBill : UserControl
    {
        public GenerateBill()
        {
            InitializeComponent();
        }


        private void view_NodeExpanding(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeAllowEventArgs e)
        {
            TreeListNode node = e.Node;
            if (node.Tag == null || (bool)node.Tag == false)
            {
                InitChild(node);
                node.Tag = true;
            }
        }

        private void InitChild(TreeListNode node)
        {
            GenerateBillViewModel viewModel = (GenerateBillViewModel)this.DataContext;
            AllocatedPatBillableItemsAccountResultModel updateItem;
            gridList.BeginDataUpdate();
            AllocatedPatBillableItemsAccountResultModel item = node.Content as AllocatedPatBillableItemsAccountResultModel;
            if (item != null)
            {
                List<AllocatedPatBillableItemsSubAccountResultModel> ChildItems;

                updateItem = item;
                if (updateItem.AllocatedPatBillableItemsSubGroups != null && updateItem.AllocatedPatBillableItemsSubGroups.Count() > 0)
                {
                    ChildItems = new List<AllocatedPatBillableItemsSubAccountResultModel>(updateItem.AllocatedPatBillableItemsSubGroups.ToArray());
                }
                else
                {
                    ChildItems = viewModel.LoadPatientBillableItemsSubGroups(item.AccountUID ?? 0, item.PatientVisitPayorUID ?? 0, item.IsPackage);
                    updateItem.AllocatedPatBillableItemsSubGroups = new ObservableCollection<AllocatedPatBillableItemsSubAccountResultModel>(ChildItems.ToArray());
                }
                if (ChildItems != null && ChildItems.Count() > 0)
                {
                    item.AllocatedPatBillableItemsSubGroups = new ObservableCollection<AllocatedPatBillableItemsSubAccountResultModel>(ChildItems.ToArray());
                }

            }
            else
            {
                AllocatedPatBillableItemsSubAccountResultModel subGroup = node.Content as AllocatedPatBillableItemsSubAccountResultModel;
                if (subGroup != null)
                {
                    List<AllocatedPatBillableItemsResultModel> ChildItems;
                    if (subGroup.AllocatedPatBillableItems != null && subGroup.AllocatedPatBillableItems.Count() > 0)
                    {
                        ChildItems = new List<AllocatedPatBillableItemsResultModel>(subGroup.AllocatedPatBillableItems.ToArray());
                    }
                    else
                    {
                        ChildItems = viewModel.LoadPatientBillableItems(subGroup.PatientVisitPayorUID ?? 0, subGroup.AccountUID ?? 0, subGroup.SubAccountUID ?? 0, subGroup.CareProviderUID ?? 0, subGroup.IsPackage);
 
                        subGroup.AllocatedPatBillableItems = new ObservableCollection<AllocatedPatBillableItemsResultModel>(ChildItems.ToArray());
                    }
                    if (ChildItems != null && ChildItems.Count() > 0)
                    {
                        subGroup.AllocatedPatBillableItems = new ObservableCollection<AllocatedPatBillableItemsResultModel>(ChildItems.ToArray());
                    }
                }
            }
            gridList.EndDataUpdate();
        }
    }
}
