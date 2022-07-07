using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
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
    /// Interaction logic for BillSettlementOP.xaml
    /// </summary>
    public partial class BillSettlementOP : UserControl
    {
        public BillSettlementOP()
        {
            InitializeComponent();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Calculator calculator = new Calculator() { ShowBorder = false, Width = 220 };
            FloatingContainer container = FloatingContainer.ShowDialog(calculator, this, new Size(1, 1), new FloatingContainerParameters() { Title = "Calculator" });
            container.SizeToContent = SizeToContent.WidthAndHeight;
            container.ContainerStartupLocation = WindowStartupLocation.CenterOwner;
            calculator.Focus();
        }

        public void Collapse()
        {
            pivotGrid.CollapseAllRows();
        }
        public void Expand()
        {
            pivotGrid.ExpandAllRows();
        }
    }
}
