using MediTech.DataService;
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
    /// Interaction logic for ReviewRISResult.xaml
    /// </summary>
    public partial class ReviewRISResult : UserControl
    {
        public ReviewRISResult()
        {
            InitializeComponent();
            this.Loaded += ReviewRISResult_Loaded;
            this.Unloaded += ReviewRISResult_Unloaded;
            if (this.DataContext is ReviewRISResultViewModel)
            {
                var viewModel = (this.DataContext as ReviewRISResultViewModel);
                viewModel.Document = richEdit.Document;
                viewModel.UpdateEvent += ReviewRISResult_UpdateEvent;
            }

        }

        void ReviewRISResult_UpdateEvent(object sender, EventArgs e)
        {
            grdTemplate.RefreshData();
        }


        void ReviewRISResult_Loaded(object sender, RoutedEventArgs e)
        {
            ReviewRISResultViewModel viewModel = (this.DataContext as ReviewRISResultViewModel);
            if (viewModel.PatientRequest.Result == null
&& !viewModel.PatientRequest.RequestItemName.Contains("chest")
&& viewModel.PatientRequest.PreviousResult != null
&& viewModel.PatientRequest.PreviousResult.Count() > 0)
            {
                PreviousResult previousResultLast = null;
                if (viewModel.PatientRequest.RequestItemName.ToLower().Contains("mammo"))
                {
                    previousResultLast = viewModel.PatientRequest.PreviousResult.OrderByDescending(p => p.ResultEnteredDttm)
                        .FirstOrDefault(p => p.TestName.ToLower().Contains("mammo"));
                }
                else if (viewModel.PatientRequest.RequestItemName.ToLower().Contains("ultrasound"))
                {
                    previousResultLast = viewModel.PatientRequest.PreviousResult.OrderByDescending(p => p.ResultEnteredDttm)
                         .FirstOrDefault(p => p.TestName.ToLower().Contains("ultrasound"));
                }

                if (previousResultLast != null)
                {
                    ResultRadiologyModel oldResult = viewModel.DataService.Radiology.GetResultRadiologyByResultUID(previousResultLast.ResultUID);
                    viewModel.Document.HtmlText = oldResult.Value;

                    //เปลี่ยน Backgroud ถ้าเป็นคนละ Order กัน
                    if ((viewModel.PatientRequest.RequestItemName.ToLower().Contains("whole") && previousResultLast.TestName.ToLower().Contains("whole"))
    || (viewModel.PatientRequest.RequestItemName.ToLower().Contains("upper") && previousResultLast.TestName.ToLower().Contains("upper"))
    || (viewModel.PatientRequest.RequestItemName.ToLower().Contains("lower") && previousResultLast.TestName.ToLower().Contains("lower"))
    || (viewModel.PatientRequest.RequestItemName.ToLower().Contains("mammo") && previousResultLast.TestName.ToLower().Contains("mammo")))
                    {
                        richEdit.ActiveView.BackColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        richEdit.ActiveView.BackColor = System.Drawing.Color.Yellow;
                    }
                }
            }


            richEdit.Document.Unit = DevExpress.Office.DocumentUnit.Centimeter;
            richEdit.Document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            richEdit.Document.Sections[0].Page.Height = 20f;
            richEdit.Document.Sections[0].Margins.Left = 0.5f;
            richEdit.Document.Sections[0].Margins.Top = 0.5f;
            richEdit.Document.Sections[0].Margins.Bottom = 0.5f;

        }

        void ReviewRISResult_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is ReviewRISResultViewModel)
            {
                long requestDetailUID = (this.DataContext as ReviewRISResultViewModel).PatientRequest.RequestDetailUID;
                (new RadiologyService()).UnReviewingReqeustDetail(requestDetailUID, AppUtil.Current.UserID);
            }
        }
    }
}
