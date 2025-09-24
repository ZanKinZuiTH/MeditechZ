using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediTech.Model;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using MediTech.DataService;
using MediTech.Views;

namespace MediTech.ViewModels
{
    public class ApprovePOViewModel : MediTechViewModelBase
    {

        private List<LookupReferenceValueModel> _POStatus;

        public List<LookupReferenceValueModel> POStatus
        {
            get { return _POStatus; }
            set { _POStatus = value; }
        }

        private LookupReferenceValueModel _SelectPOStatus;

        public LookupReferenceValueModel SelectPOStatus
        {
            get { return _SelectPOStatus; }
            set { _SelectPOStatus = value; }
        }



        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }


        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                    ?? (_SaveCommand = new RelayCommand(Save));
            }

        }

        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get
            {
                return _CancelCommand
                    ?? (_CancelCommand = new RelayCommand(Cancel));
            }

        }


        public ApprovePOViewModel()
        {
            var data = DataService.Technical.GetReferenceValueMany("POSTS");
            if (data != null)
            {
                POStatus = data.Where(p => p.ValueCode == "NOAPP" || p.ValueCode == "APPRV").ToList();
            }
        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        private void Save()
        {
            if (SelectPOStatus == null)
            {
                System.Windows.Forms.MessageBox.Show("กรุณาเลือกสถานะ", "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            CloseViewDialog(ActionDialog.Save);
        }
    }
}
