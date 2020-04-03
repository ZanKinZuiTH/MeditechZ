using DevExpress.Xpf.Editors;
using MediTech.Model;
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
    /// Interaction logic for CCHPI.xaml
    /// </summary>
    public partial class CCHPI : UserControl
    {
        public CCHPI()
        {
            InitializeComponent();
            cmbCCHPIMaster.ProcessNewValue += cmbCCHPIMaster_ProcessNewValue;
        }

        void cmbCCHPIMaster_ProcessNewValue(DependencyObject sender, ProcessNewValueEventArgs e)
        {
            if (e.DisplayText == null || (e.DisplayText as string) == "")
                return;
            ComboBoxEdit editor = sender as ComboBoxEdit;
            string NewItemName = e.DisplayText;
            ObservableCollection<CCHPIMasterModel> items = editor.ItemsSource as ObservableCollection<CCHPIMasterModel>;
            bool shouldAdd = true;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Name == NewItemName)
                {
                    shouldAdd = false;
                    break;
                }
            }
            if (shouldAdd)
                items.Add(new CCHPIMasterModel() { Name = NewItemName,Description = NewItemName });
        }



    }
}
