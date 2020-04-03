using DevExpress.Xpf.LayoutControl;
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
    /// Interaction logic for PastMedicalHistory.xaml
    /// </summary>
    public partial class PastMedicalHistory : UserControl
    {
        public PastMedicalHistory()
        {
            InitializeComponent();
            checkShowAsGroupBoxes.Checked += ShowAs_Checked;
            checkShowAsTabs.Checked += ShowAs_Checked;
        }

        private void ShowAs_Checked(object sender, RoutedEventArgs e)
        {
            if (groupContainer == null)
                return;
            LayoutGroupView containerView, childView;
            if (sender == checkShowAsGroupBoxes)
            {
                containerView = LayoutGroupView.GroupBox;
                childView = LayoutGroupView.GroupBox;
            }
            else
                if (sender == checkShowAsTabs)
                {
                    containerView = LayoutGroupView.Tabs;
                    childView = LayoutGroupView.Group;
                }
                else
                    return;
            groupContainer.View = containerView;
            foreach (FrameworkElement child in groupContainer.GetLogicalChildren(false))
                if (child is LayoutGroup)
                    ((LayoutGroup)child).View = childView;
        }
    }
}
