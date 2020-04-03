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
using MediTech.ViewModels;
namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for PhysicalExam.xaml
    /// </summary>
    public partial class PhysicalExam : UserControl
    {
        public PhysicalExam()
        {
            InitializeComponent();
            if (this.DataContext is PhysicalExamViewModel)
            {
                (this.DataContext as PhysicalExamViewModel).Document = richEdit.Document;
            }
        }
    }
}
