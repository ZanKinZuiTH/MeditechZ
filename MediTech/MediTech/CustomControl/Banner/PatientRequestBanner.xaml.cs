using MediTech.Model;
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

namespace MediTech.CustomControl.Banner
{
    /// <summary>
    /// Interaction logic for PatientRequestBanner.xaml
    /// </summary>
    public partial class PatientRequestBanner : UserControl
    {
        public PatientRequestBanner()
        {
            InitializeComponent();
        }


        public void SetPatientBanner(RequestListModel request)
        {
            tePatientName.Text = request.PatientName;
            tePatientID.Text = request.PatientID;
            lbBirthDate.Content = request.BirthDateString;
            lbPatientAge.Content = request.PatientAge;
            lbGender.Content = request.Gender;
            teReferenceNo.Text = request.RequestNumber;
            lbVisitID.Content = request.VisitID;
            teAddress.Text = request.PatientAddress;
            tePayorName.Text = request.PayorName;
            lbWeight.Content = request.Weight;
            lbHeight.Content = request.Height;
            lbBMI.Content = CalculateBMI(request.Weight.ToString(),request.Height.ToString());
        }

        string CalculateBMI(string h, string w)
        {
            string ret = string.Empty;
            try
            {
                
                ret = String.Format("{0:F2}", (float.Parse(w) / (float.Parse(h) / 100 * float.Parse(h) / 100)));

            }
            catch (Exception)
            {
                return ret;
            }


            return ret;
        }




    }
}
