using DevExpress.Xpf.Controls;
using GalaSoft.MvvmLight;
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
    /// Interaction logic for RegisterPatient.xaml
    /// </summary>
    public partial class RegisterPatient : UserControl
    {
        public RegisterPatient()
        {

            InitializeComponent();
        }




        //void RegisterPatient_Loaded(object sender, RoutedEventArgs e)
        //{
        //    var contentFooter = wizardRegister.FooterTemplate.LoadContent();
        //    if (contentFooter != null)
        //    {
        //        WizardDialogFooter dialogFooter = contentFooter as WizardDialogFooter;
        //        WizardButton skipButton = dialogFooter.FindName("btnSkip") as WizardButton;
        //        skipButton.Visibility = System.Windows.Visibility.Hidden;
        //        dialogFooter.UpdateLayout();
        //        wizardRegister.UpdateLayout();

        //    }
        //}





    }
}
