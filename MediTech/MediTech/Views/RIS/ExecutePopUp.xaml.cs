using MediTech.Model;
using MediTech.ViewModels;
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
    /// Interaction logic for ExecutePopUp.xaml
    /// </summary>
    public partial class ExecutePopUp : UserControl
    {
        public ExecutePopUp()
        {
            InitializeComponent();
            if (this.DataContext is ExecutePopUpViewModel)
            {
                (this.DataContext as ExecutePopUpViewModel).UpdateEvent += ExecutePopUp_UpdateEvent;
            }
        }

        public ExecutePopUp(List<RequestListModel> requestList)
        {
            InitializeComponent();
            if (this.DataContext is ExecutePopUpViewModel)
            {
                ExecutePopUpViewModel viewModel = (this.DataContext as ExecutePopUpViewModel);
                viewModel.UpdateEvent += ExecutePopUp_UpdateEvent;
                viewModel.ExecuteList = new List<RequestListModel>();
                foreach (var item in requestList)
                {
                    RequestListModel newItem = new RequestListModel();
                    newItem.PatientUID = item.PatientUID;
                    newItem.PatientVisitUID = item.PatientVisitUID;
                    newItem.RequestUID = item.RequestUID;
                    newItem.RequestDetailUID = item.RequestDetailUID;
                    newItem.ResultUID = item.ResultUID;
                    newItem.PatientID = item.PatientID;
                    newItem.PatientName = item.PatientName;
                    newItem.RequestNumber = item.RequestNumber;
                    newItem.AccessionNumber = item.AccessionNumber;
                    newItem.RequestedDttm = item.RequestedDttm;
                    newItem.ArriveTime = item.ArriveTime;
                    newItem.PreparedDttm = item.PreparedDttm;
                    newItem.ResultedDttm = item.ResultedDttm;
                    newItem.ProcessingNote = item.ProcessingNote;
                    newItem.OrderStatus = item.OrderStatus;
                    newItem.PriorityStatus = item.PriorityStatus;
                    newItem.ResultStatus = item.ResultStatus;
                    newItem.RequestUserName = item.RequestUserName;
                    newItem.RequestItemCode = item.RequestItemCode;
                    newItem.RequestItemName = item.RequestItemName;
                    newItem.Modality = item.Modality;
                    newItem.RadiologistUID = item.RadiologistUID;
                    newItem.ExecuteByUID = item.ExecuteByUID;
                    newItem.DoctorName = item.DoctorName;
                    newItem.OrganisationName = item.OrganisationName;
                    newItem.Comments = item.Comments;
                    viewModel.ExecuteList.Add(newItem);
                }
            }
        }

        void ExecutePopUp_UpdateEvent(object sender, EventArgs e)
        {
            grdExecuteList.RefreshData();
        }
    }
}
