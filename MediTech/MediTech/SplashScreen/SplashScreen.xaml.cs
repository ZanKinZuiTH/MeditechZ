using DevExpress.Xpf.Core;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MediTech
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window, ISplashScreen
    {
        Storyboard Storyboard { get { return (Storyboard)FindResource("storyboard"); } }
        public SplashScreen()
        {
            InitializeComponent();
        }


        public void CloseSplashScreen()
        {
            this.Close();
            //if (Storyboard != null)
            //{
            //    Storyboard.Completed += OnAnimationCompleted;
            //    Storyboard.Begin();
            //}
        }


        void OnAnimationCompleted(object sender, EventArgs e)
        {
            Storyboard.Completed -= OnAnimationCompleted;
            this.Close();
        }


        public void Progress(double value)
        {
            progressBar.Value = value;
        }

        public void SetProgressState(bool isIndeterminate)
        {

        }
    }
}
