using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
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
    public class InsuranceCompanyViewModel : MediTechViewModelBase
    {
        #region Properties

        private string _InsranceCompanyCode;
        public string InsranceCompanyCode
        {
            get { return _InsranceCompanyCode; }
            set { Set(ref _InsranceCompanyCode, value); }
        }

        private string _InsranceCompanyName;
        public string InsranceCompanyName
        {
            get { return _InsranceCompanyName; }
            set { Set(ref _InsranceCompanyName, value); }
        }

        private List<InsuranceCompanyModel> _InsranceCompanySource;
        public List<InsuranceCompanyModel> InsranceCompanySource
        {
            get { return _InsranceCompanySource; }
            set { Set(ref _InsranceCompanySource, value); }
        }

        private InsuranceCompanyModel _SelectInsranceCompany;
        public InsuranceCompanyModel SelectInsranceCompany
        {
            get { return _SelectInsranceCompany; }
            set { Set(ref _SelectInsranceCompany, value); 
            if(SelectInsranceCompany != null)
                {
                    IsSelectInsuranceCompany = true;

                    //SelectPayorNameByINCO = PayorNameByINCO.FirstOrDefault(p => p.InsuranceCompanyUID == SelectInsranceCompany.InsuranceCompanyUID);
                    SelectedName = SelectInsranceCompany.CompanyName;
                    PayorDetails = DataService.Billing.SearchPayorDetailByINCO(PayorDetailCode, SelectInsranceCompany.InsuranceCompanyUID);

                    PayorAgreement = DataService.Billing.SearchPayorAgreementByINCO("", SelectInsranceCompany.InsuranceCompanyUID);
                    InsurancePlan = DataService.Billing.SearchInsurancePlaneByINCO(SelectInsranceCompany.InsuranceCompanyUID);
                   
                }
                else
                {
                    IsSelectInsuranceCompany = false;
                }
            }
        }

        private bool _IsSelectInsuranceCompany;
        public bool IsSelectInsuranceCompany
        {
            get { return _IsSelectInsuranceCompany; }
            set { Set(ref _IsSelectInsuranceCompany, value); }
        }

        private string _SelectedName;
        public string SelectedName
        {
            get { return _SelectedName; }
            set { Set(ref _SelectedName, value); }
        }

        private int _SelectTabIndex;
        public int SelectTabIndex
        {
            get { return _SelectTabIndex; }
            set { Set(ref _SelectTabIndex, value); }
        }


        private string _PayorDetailCode;
        public string PayorDetailCode
        {
            get { return _PayorDetailCode; }
            set { Set(ref _PayorDetailCode, value); }
        }

        private List<InsuranceCompanyModel> _PayorNameByINCO;
        public List<InsuranceCompanyModel> PayorNameByINCO
        {
            get { return _PayorNameByINCO; }
            set { Set(ref _PayorNameByINCO, value); }
        }

        private InsuranceCompanyModel _SelectPayorNameByINCO;
        public InsuranceCompanyModel SelectPayorNameByINCO
        {
            get { return _SelectPayorNameByINCO; }
            set { Set(ref _SelectPayorNameByINCO, value); }
        }

        private List<PayorDetailModel> _PayorDetails;
        public List<PayorDetailModel> PayorDetails
        {
            get { return _PayorDetails; }
            set { Set(ref _PayorDetails, value); }
        }

        private PayorDetailModel _SelectPayorDetail;
        public PayorDetailModel SelectPayorDetail
        {
            get { return _SelectPayorDetail; }
            set { Set(ref _SelectPayorDetail, value); }
        }

        private List<PayorAgreementModel> _PayorAgreement;
        public List<PayorAgreementModel> PayorAgreement
        {
            get { return _PayorAgreement; }
            set { Set(ref _PayorAgreement, value); }
        }

        private PayorAgreementModel _SelectPayorAgreement;
        public PayorAgreementModel SelectPayorAgreement
        {
            get { return _SelectPayorAgreement; }
            set { Set(ref _SelectPayorAgreement, value); }
        }


        private List<InsurancePlanModel> _InsurancePlan;
        public List<InsurancePlanModel> InsurancePlan
        {
            get { return _InsurancePlan; }
            set { Set(ref _InsurancePlan, value); }
        }

        private InsurancePlanModel _SelectInsurancePlan;
        public InsurancePlanModel SelectInsurancePlan
        {
            get { return _SelectInsurancePlan; }
            set { Set(ref _SelectInsurancePlan, value); }
        }

        #endregion

        #region Command
        private RelayCommand _SearchInsranceCompanyCommand;
        public RelayCommand SearchInsranceCompanyCommand
        {
            get
            {
                return _SearchInsranceCompanyCommand ?? (_SearchInsranceCompanyCommand = new RelayCommand(SearchInsranceCompany));
            }
        }

        private RelayCommand _AddINCOCommand;
        public RelayCommand AddINCOCommand
        {
            get
            {
                return _AddINCOCommand ?? (_AddINCOCommand = new RelayCommand(AddINCO));
            }
        }

        private RelayCommand _ModifyINCOCommand;
        public RelayCommand ModifyINCOCommand
        {
            get
            {
                return _ModifyINCOCommand ?? (_ModifyINCOCommand = new RelayCommand(ModifyINCO));
            }
        }

        private RelayCommand _DeleteINCOCommand;
        public RelayCommand DeleteINCOCommand
        {
            get
            {
                return _DeleteINCOCommand ?? (_DeleteINCOCommand = new RelayCommand(DeleteINCO));
            }
        }

        private RelayCommand _SearchPayorDetailCommand;
        public RelayCommand SearchPayorDetailCommand
        {
            get
            {
                return _SearchPayorDetailCommand ?? (_SearchPayorDetailCommand = new RelayCommand(SearchInsranceCompany));
            }
        }

        private RelayCommand _AddCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand ?? (_AddCommand = new RelayCommand(Add));
            }
        }

        private RelayCommand _ModifyCommand;
        public RelayCommand ModifyCommand
        {
            get
            {
                return _ModifyCommand ?? (_ModifyCommand = new RelayCommand(Modify));
            }
        }

        private RelayCommand _DeleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand ?? (_DeleteCommand = new RelayCommand(Delete));
            }
        }

        #endregion

        #region Method
        public InsuranceCompanyViewModel()
        {
            InsranceCompanySource = DataService.Billing.GetInsuranceCompanyAll();
            PayorNameByINCO = DataService.Billing.GetInsuranceCompanyAll();
        }

        private void SearchInsranceCompany()
        {
            if (!string.IsNullOrEmpty(InsranceCompanyCode) || !string.IsNullOrEmpty(InsranceCompanyName))
            {
                InsranceCompanySource = DataService.Billing.SearchInsuranceCompany(InsranceCompanyCode, InsranceCompanyName);
            }
        }

        private void AddINCO()
        {
            ManageInsuranceCompany pageview = new ManageInsuranceCompany();
            ManageInsuranceCompanyViewModel result = (ManageInsuranceCompanyViewModel)LaunchViewDialog(pageview, "MNINCO", true);
            if (result != null && result.ResultDialog == ActionDialog.Save)
            {
                SaveSuccessDialog();
                InsranceCompanySource = DataService.Billing.SearchInsuranceCompany(InsranceCompanyCode, InsranceCompanyName);
                SelectInsranceCompany = InsranceCompanySource.FirstOrDefault(p => p.Code == result.Code);
                SelectPayorDetail = null;
                SelectPayorAgreement = null;
                SelectInsurancePlan = null;
            }

        }

        private void ModifyINCO()
        {
            if (SelectInsranceCompany == null)
            {
                WarningDialog("กรุณาเลือก Insrance Company");
                return;
            }
                ManageInsuranceCompany mangaINCO = new ManageInsuranceCompany();
                (mangaINCO.DataContext as ManageInsuranceCompanyViewModel).AssingModel(SelectInsranceCompany.InsuranceCompanyUID);
                ManageInsuranceCompanyViewModel result = (ManageInsuranceCompanyViewModel)LaunchViewDialog(mangaINCO, "MNINCO", true);

                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    InsranceCompanySource = DataService.Billing.SearchInsuranceCompany(InsranceCompanyCode, InsranceCompanyName);
                    SelectInsranceCompany = InsranceCompanySource.FirstOrDefault(p => p.Code == result.Code);
                    SelectPayorDetail = null;
                    SelectPayorAgreement = null;
                    SelectInsurancePlan = null;
                }
            
        }

        private void DeleteINCO()
        {
            if (SelectInsranceCompany != null)
            {
                try
                {
                    MessageBoxResult result = DeleteDialog();
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.Billing.DeleteInsuranceCompany(SelectInsranceCompany.InsuranceCompanyUID, AppUtil.Current.UserID);
                        DeleteSuccessDialog();
                        InsranceCompanySource.Remove(SelectInsranceCompany);
                        InsranceCompanySource = DataService.Billing.SearchInsuranceCompany(InsranceCompanyCode, InsranceCompanyName);

                    }
                }
                catch (Exception er)
                {

                    ErrorDialog(er.Message);
                }
            }
        }

        private void Add()
        {
            if(SelectTabIndex == 0)
            {
                if (SelectInsranceCompany != null)
                {
                    ManagePayorDetail page = new ManagePayorDetail();
                    (page.DataContext as ManagePayorDetailViewModel).AssingModel(SelectInsranceCompany.InsuranceCompanyUID, null);
                    ManagePayorDetailViewModel result = (ManagePayorDetailViewModel)LaunchViewDialog(page, "PAYMN", true);

                    if (result != null && result.ResultDialog == ActionDialog.Save)
                    {
                        SaveSuccessDialog();
                        PayorDetails = DataService.Billing.SearchPayorDetailByINCO("", SelectInsranceCompany.InsuranceCompanyUID);
                        SelectPayorDetail = null;
                    }
                }
            }

            if (SelectTabIndex == 1)
            {
                if (SelectInsranceCompany != null)
                {
                    ManagePayorAgreement page = new ManagePayorAgreement();
                    (page.DataContext as ManagePayorAgreementViewModel).AssignModel(SelectInsranceCompany.InsuranceCompanyUID,null);
                    ManagePayorAgreementViewModel result = (ManagePayorAgreementViewModel)LaunchViewDialog(page, "MNPYAGM", false, true);

                    if (result != null && result.ResultDialog == ActionDialog.Save)
                    {
                        SaveSuccessDialog();
                        PayorAgreement = DataService.Billing.SearchPayorAgreementByINCO("", SelectInsranceCompany.InsuranceCompanyUID);

                    }

                }
            }

            if (SelectTabIndex == 2)
            {
                if (SelectInsranceCompany != null)
                {
                    ManageInsurancePlan page = new ManageInsurancePlan();
                    (page.DataContext as ManageInsurancePlanViewModel).AssignModel(SelectInsranceCompany.InsuranceCompanyUID,null);
                    ManageInsurancePlanViewModel result = (ManageInsurancePlanViewModel)LaunchViewDialog(page, "MNINPLN", true);

                    if (result != null && result.ResultDialog == ActionDialog.Save)
                    {
                        SaveSuccessDialog();
                        InsurancePlan = DataService.Billing.SearchInsurancePlaneByINCO(SelectInsranceCompany.InsuranceCompanyUID);
                    }
                }
            }
        }

        private void Modify()
        {
            if (SelectTabIndex == 0)
            {
                if (SelectPayorDetail == null)
                {
                    WarningDialog("กรุณาเลือก Payor Detail");
                    return;
                }
                    ManagePayorDetail page = new ManagePayorDetail();
                    (page.DataContext as ManagePayorDetailViewModel).AssingModel(SelectInsranceCompany.InsuranceCompanyUID, SelectPayorDetail);
                    ManagePayorDetailViewModel result = (ManagePayorDetailViewModel)LaunchViewDialog(page, "PAYMN", true);

                    if (result != null && result.ResultDialog == ActionDialog.Save)
                    {
                        SaveSuccessDialog();
                        PayorDetails = DataService.Billing.SearchPayorDetailByINCO("", SelectInsranceCompany.InsuranceCompanyUID);
                    }
                
            }

            if (SelectTabIndex == 1)
            {
                if (SelectPayorAgreement == null)
                {
                    WarningDialog("กรุณาเลือก Payor Agreement");
                    return;
                }
                    ManagePayorAgreement page = new ManagePayorAgreement();
                    (page.DataContext as ManagePayorAgreementViewModel).AssignModel(SelectInsranceCompany.InsuranceCompanyUID, SelectPayorAgreement);
                    ManagePayorAgreementViewModel result = (ManagePayorAgreementViewModel)LaunchViewDialog(page, "MNPYAGM", false,true);

                    if (result != null && result.ResultDialog == ActionDialog.Save)
                    {
                        SaveSuccessDialog();
                        PayorAgreement = DataService.Billing.SearchPayorAgreementByINCO("", SelectInsranceCompany.InsuranceCompanyUID);
                    }
                
            }

            if (SelectTabIndex == 2)
            {
                if(SelectInsurancePlan == null)
                {
                    WarningDialog("กรุณาเลือก Insurance Plan");
                    return;
                }
                ManageInsurancePlan page = new ManageInsurancePlan();
                    (page.DataContext as ManageInsurancePlanViewModel).AssignModel(SelectInsranceCompany.InsuranceCompanyUID, SelectInsurancePlan);
                    ManageInsurancePlanViewModel result = (ManageInsurancePlanViewModel)LaunchViewDialog(page, "MNINPLN", true);

                    if (result != null && result.ResultDialog == ActionDialog.Save)
                    {
                        SaveSuccessDialog();
                        InsurancePlan = DataService.Billing.SearchInsurancePlaneByINCO(SelectInsranceCompany.InsuranceCompanyUID);
                    }

                

            }
        }

        private void Delete()
        {
            if (SelectTabIndex == 0)
            {
                if (SelectPayorDetail != null)
                {
                    var agreement = DataService.Billing.CheckInsurancePlan(SelectPayorDetail.PayorDetailUID, null);
                    if (agreement != null)
                    {
                        WarningDialog("Office ถูกใช้งานอยู่กรุณาตรวจสอบข้อมูลก่อนทำการลบ");
                        return;
                    }
                    try
                    {
                        MessageBoxResult result = DeleteDialog();
                        if (result == MessageBoxResult.Yes)
                        {
                            DataService.Billing.DeletePayorOfficeDetail(SelectPayorDetail.PayorDetailUID, AppUtil.Current.UserID);
                            DeleteSuccessDialog();
                            PayorDetails.Remove(SelectPayorDetail);
                            PayorDetails = DataService.Billing.SearchPayorDetailByINCO(PayorDetailCode, SelectInsranceCompany.InsuranceCompanyUID);

                        }
                    }
                    catch (Exception er)
                    {

                        ErrorDialog(er.Message);
                    }
                }
            }

            if (SelectTabIndex == 1)
            {
                if (SelectPayorAgreement != null)
                {
                    var agreement = DataService.Billing.CheckInsurancePlan(null,SelectPayorAgreement.PayorAgreementUID);
                        if(agreement != null)
                        {
                            WarningDialog("Agreement ถูกใช้งานอยู่ กรุณาตรวจสอบข้อมูลก่อนทำการลบ");
                            return;
                        }

                    try
                    {
                        MessageBoxResult result = DeleteDialog();
                        if (result == MessageBoxResult.Yes)
                        {
                            DataService.Billing.DeletePayorAgreement(SelectPayorAgreement.PayorAgreementUID, AppUtil.Current.UserID);
                            DeleteSuccessDialog();
                            PayorAgreement.Remove(SelectPayorAgreement);
                            PayorAgreement = DataService.Billing.SearchPayorAgreementByINCO("", SelectInsranceCompany.InsuranceCompanyUID);

                        }
                    }
                    catch (Exception er)
                    {

                        ErrorDialog(er.Message);
                    }
                   
                }
            }

            if (SelectTabIndex == 2)
            {
                if (SelectInsurancePlan != null)
                {
                    try
                    {
                        MessageBoxResult result = DeleteDialog();
                        if (result == MessageBoxResult.Yes)
                        {
                            DataService.Billing.DeleteInsurancePlan(SelectInsurancePlan.InsurancePlanUID, AppUtil.Current.UserID);
                            DeleteSuccessDialog();
                            InsurancePlan.Remove(SelectInsurancePlan);
                            InsurancePlan = DataService.Billing.SearchInsurancePlaneByINCO(SelectInsranceCompany.InsuranceCompanyUID);
                        }
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
