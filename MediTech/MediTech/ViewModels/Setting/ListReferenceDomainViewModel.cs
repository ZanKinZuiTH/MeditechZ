using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Models;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class ListReferenceDomainViewModel : MediTechViewModelBase
    {

        #region Properties

        public string DomainCode { get; set; }
        public string DomainDescription { get; set; }
        public string ValueDescription { get; set; }

        private List<ReferenceDomainModel> _ReferenceDomainSource;

        public List<ReferenceDomainModel> ReferenceDomainSource
        {
            get { return _ReferenceDomainSource; }
            set { Set(ref _ReferenceDomainSource, value); }
        }


        private ReferenceDomainModel _SelectReferenceDomain;

        public ReferenceDomainModel SelectReferenceDomain
        {
            get { return _SelectReferenceDomain; }
            set { Set(ref _SelectReferenceDomain, value); }
        }


        #endregion

        #region Command

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(SearchDomain)); }
        }

        private RelayCommand _AddCommand;
        public RelayCommand AddCommand
        {
            get { return _AddCommand ?? (_AddCommand = new RelayCommand(AddDomain)); }
        }

        private RelayCommand _EditCommand;
        public RelayCommand EditCommand
        {
            get { return _EditCommand ?? (_EditCommand = new RelayCommand(EditDomain)); }
        }

        private RelayCommand _DeleteCommand;
        public RelayCommand DeleteCommand
        {
            get { return _DeleteCommand ?? (_DeleteCommand = new RelayCommand(DeleteDomain)); }
        }

        #endregion

        #region Variable

        #endregion

        #region Method

        public ListReferenceDomainViewModel()
        {
            
        }

        private void SearchDomain()
        {
            List<ReferenceDomainModel> domainData = DataService.Technical.SearchReferenceDomain(DomainCode, DomainDescription, ValueDescription);
            ReferenceDomainSource = domainData;
        }

        private void AddDomain()
        {
            ManageReferenceDomain page = new ManageReferenceDomain();
            ChangeViewPermission(page);
        }

        private void EditDomain()
        {
            if (_SelectReferenceDomain != null)
            {
                ManageReferenceDomain page = new ManageReferenceDomain();
                var modelData = DataService.Technical.GetReferenceDomainByUID(_SelectReferenceDomain.UID);
                page.AssignModel(modelData);
                ChangeViewPermission(page);
            }
        }

        private void DeleteDomain()
        {
            if (_SelectReferenceDomain != null)
            {
                MessageBoxResult result = DeleteDialog();
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        DataService.Technical.DeleteRefereceDomain(_SelectReferenceDomain.UID, AppUtil.Current.UserID);
                        DeleteSuccessDialog();
                        ReferenceDomainSource.Remove(_SelectReferenceDomain);
                        OnUpdateEvent();
                        
                    }
                    catch (Exception er)
                    {

                        ErrorDialog(er.Message);
                    }

                }
            }
        }

        #endregion
    }
}
