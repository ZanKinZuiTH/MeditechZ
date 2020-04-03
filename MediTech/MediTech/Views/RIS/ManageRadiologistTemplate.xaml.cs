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
    /// Interaction logic for ManageRadiologistTemplate.xaml
    /// </summary>
    public partial class ManageRadiologistTemplate : UserControl
    {
        public ManageRadiologistTemplate()
        {
            InitializeComponent();
            this.Loaded += ManageRadiologistTemplate_Loaded;
            if (this.DataContext is ManageRadiologistTemplateViewModel)
            {
                var viewModel = (this.DataContext as ManageRadiologistTemplateViewModel);
                viewModel.Document = richEdit.Document;
            }
        }

        void ManageRadiologistTemplate_Loaded(object sender, RoutedEventArgs e)
        {
            richEdit.Document.Unit = DevExpress.Office.DocumentUnit.Centimeter;
            richEdit.Document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            richEdit.Document.Sections[0].Page.Height = 20f;
            richEdit.Document.Sections[0].Margins.Left = 0.5f;
            richEdit.Document.Sections[0].Margins.Top = 0.5f;
            richEdit.Document.Sections[0].Margins.Bottom = 0.5f;
        }
    }
}
