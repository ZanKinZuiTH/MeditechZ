using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.PivotGrid.Internal;
using MediTech.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for BillSettlementIP.xaml
    /// </summary>
    public partial class BillSettlementIP : UserControl
    {
        BillSettlementIPViewModel viewModel;
        public BillSettlementIP()
        {
            InitializeComponent();
            if (this.DataContext is BillSettlementIPViewModel)
            {
                viewModel = (this.DataContext as BillSettlementIPViewModel);
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Calculator calculator = new Calculator() { ShowBorder = false, Width = 220 };
            //FloatingContainer container = FloatingContainer.ShowDialog(calculator, this, new Size(1, 1), new FloatingContainerParameters() { Title = "Calculator" });
            //container.SizeToContent = SizeToContent.WidthAndHeight;
            //container.ContainerStartupLocation = WindowStartupLocation.CenterOwner;
            //calculator.Focus();
            Process.Start("calc");
        }
        public void Collapse()
        {
            pivotGrid.CollapseAllRows();
        }
        public void Expand()
        {
            pivotGrid.ExpandAllRows();
        }

        private void imgAllocate_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;

            if (bt != null && bt.CommandParameter != null)
            {
                if (((CellsAreaItem)((Button)sender).DataContext).Item.IsGrandTotalAppearance)
                {
                    viewModel.callAllocatedMergeReceipt(long.Parse(bt.CommandParameter.ToString()));
                }
                else
                {
                    viewModel.CallAllocatedBillableItem(long.Parse(bt.CommandParameter.ToString()));
                }

            }
        }
    }
}
