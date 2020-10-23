using DevExpress.Xpf.NavBar;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MediTech.ViewModels
{
    public class EnterCheckupResultViewModel : MediTechViewModelBase
    {

        #region Properties
        private NavBarItem _SelectedItemNavbar;

        public NavBarItem SelectedItemNavbar
        {
            get { return _SelectedItemNavbar; }
            set
            {
                Set(ref _SelectedItemNavbar, value);
                if (_SelectedItemNavbar != null)
                {
                    SetDocument(_SelectedItemNavbar.Name);
                }
            }
        }

        private object _DocumentCotent;

        public object DocumentCotent
        {
            get { return _DocumentCotent; }
            set { _DocumentCotent = value; }
        }

        #endregion



        #region Method
        PatientVitalSign patientVital = new PatientVitalSign();
        Examination examination = new Examination();
        Audiogram audiogram = new Audiogram();
        Spirometry spirometry = new Spirometry();
        Timus timus = new Timus();
        EKG ekg = new EKG();
        void SetDocument(string name)
        {
            switch (name)
            {
                case "vitalsign":
                    patientVital.Banner.Visibility = System.Windows.Visibility.Collapsed;
                    patientVital.btnSave.Visibility = System.Windows.Visibility.Collapsed;
                    patientVital.btnCancel.Visibility = System.Windows.Visibility.Collapsed;
                    ((patientVital as UserControl).DataContext as MediTechViewModelBase).View = patientVital;
                    (this.View as EnterCheckupResult).documentFrame.Navigate(patientVital);
                    break;
                case "examination":
                    ((examination as UserControl).DataContext as MediTechViewModelBase).View = examination;
                    (this.View as EnterCheckupResult).documentFrame.Navigate(examination);
                    break;
                case "audiogram":
                    ((audiogram as UserControl).DataContext as MediTechViewModelBase).View = audiogram;
                    (this.View as EnterCheckupResult).documentFrame.Navigate(audiogram);
                    break;
                case "spirometry":
                    ((spirometry as UserControl).DataContext as MediTechViewModelBase).View = spirometry;
                    (this.View as EnterCheckupResult).documentFrame.Navigate(spirometry);
                    break;
                case "titmus":
                    ((timus as UserControl).DataContext as MediTechViewModelBase).View = timus;
                    (this.View as EnterCheckupResult).documentFrame.Navigate(timus);
                    break;
                case "ekg":
                    ((ekg as UserControl).DataContext as MediTechViewModelBase).View = ekg;
                    (this.View as EnterCheckupResult).documentFrame.Navigate(ekg);
                    break;
            }



    
        }

        #endregion

    }
}
