using MediTech.Model;
using MediTech.Model.Report;
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
    /// Interaction logic for ReportParameter8.xaml
    /// </summary>
    public partial class ReportParameter8 : UserControl
    {
        public ReportParameter8(ReportsModel reportTempalte)
        {
            InitializeComponent();
            (this.DataContext as ReportParameter8ViewModel).ReportTemplate = reportTempalte;
        }
    }
}


