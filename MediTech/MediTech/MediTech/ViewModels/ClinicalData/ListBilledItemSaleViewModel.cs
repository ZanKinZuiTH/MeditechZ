using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ListBilledItemSaleViewModel : MediTechViewModelBase
    {
        #region Properties

        public List<HealthOrganisationModel> Organisations { get; set; }
        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set
            {
                Set(ref _SelectOrganisation, value);
            }
        }

        private DateTime? _DateFrom;

        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set { _DateFrom = value; }
        }

        private DateTime? _DateTo;

        public DateTime? DateTo
        {
            get { return _DateTo; }
            set { _DateTo = value; }
        }


        #endregion

        #region Command

        private RelayCommand _PrintBillCommand;
        public RelayCommand PrintBillCommand
        {
            get { return _PrintBillCommand ?? (_PrintBillCommand = new RelayCommand(PrintBill)); }
        }

        private RelayCommand _CreateSaleItemCommand;
        public RelayCommand CreateSaleItemCommand
        {
            get { return _CreateSaleItemCommand ?? (_CreateSaleItemCommand = new RelayCommand(CreateSaleItem)); }
        }

        private RelayCommand _EditSaleCommand;
        public RelayCommand EditSaleCommand
        {
            get { return _EditSaleCommand ?? (_EditSaleCommand = new RelayCommand(EditSale)); }
        }

        private RelayCommand _CancelBillCommand;
        public RelayCommand CancelBillCommand
        {
            get { return _CancelBillCommand ?? (_CancelBillCommand = new RelayCommand(CancelBill)); }
        }

        #endregion

        #region Method
        public ListBilledItemSaleViewModel()
        {
            Organisations = GetHealthOrganisationIsRoleStock();
            DateTime now = DateTime.Now;
            DateFrom = now;
            DateTo = now;
        }
                                                                                                                                                                                                                                                                                          
        private void CreateSaleItem()
        {
            ManageBilledItemSale manageSale = new ManageBilledItemSale();
            ChangeViewPermission(manageSale,this);
        }

        private void EditSale()
        {
            ManageBilledItemSaleViewModel manageSale = new ManageBilledItemSaleViewModel();
            ChangeViewPermission(manageSale, this);
        }
        private void PrintBill()
        {

        }

        private void CancelBill()
        {

        }
        #endregion
    }
}
