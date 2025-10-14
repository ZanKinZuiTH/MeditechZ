using MediTech.Model;
using MediTech.ViewModels;
using System.Windows.Controls;

namespace MediTech.Views
{
    public partial class EditStudyDetails : UserControl
    {
        public EditStudyDetails(StudiesModel study)
        {
            InitializeComponent();
            if (this.DataContext is EditStudyDetailsViewModel)
            {
                (this.DataContext as EditStudyDetailsViewModel).SelectedStudy = study;
            }
        }
    }
}


