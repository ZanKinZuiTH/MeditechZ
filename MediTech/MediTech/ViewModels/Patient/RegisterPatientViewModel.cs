using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.Helpers;
using MediTech.Helpers.RDNIDWRAPPER;
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
    public class RegisterPatientViewModel : MediTechViewModelBase
    {

        #region Properties

        private PatientInformationModel _SelectedPatient;

        public PatientInformationModel SelectedPatient
        {
            get { return _SelectedPatient; }
            set { _SelectedPatient = value; RaisePropertyChanged("SelectedPatient"); }
        }

        private BookingModel _SelectedBooking;

        public BookingModel SelectedBooking
        {
            get { return _SelectedBooking; }
            set { _SelectedBooking = value; RaisePropertyChanged("SelectedBooking"); }
        }

        private bool _IsManageRegister;

        public bool IsManageRegister
        {
            get { return _IsManageRegister; }
            set { _IsManageRegister = value; RaisePropertyChanged("IsManageRegister"); }
        }

        private int _SelectPageIndex;

        public int SelectPageIndex
        {
            get { return _SelectPageIndex; }
            set { _SelectPageIndex = value; RaisePropertyChanged("SelectPageIndex"); }
        }

        #endregion

        #region Command

        private RelayCommand _ReadCardCommand;

        public RelayCommand ReadCardCommand
        {
            get { return _ReadCardCommand ?? (_ReadCardCommand = new RelayCommand(ReadCard)); }
        }


        private RelayCommand _SkipCommand;

        public RelayCommand SkipCommand
        {
            get { return _SkipCommand ?? (_SkipCommand = new RelayCommand(SkipPage)); }
        }

        private RelayCommand _NextCommand;

        public RelayCommand NextCommand
        {
            get { return _NextCommand ?? (_NextCommand = new RelayCommand(NextPage)); }
        }


        private RelayCommand _BackCommand;

        public RelayCommand BackCommand
        {
            get { return _BackCommand ?? (_BackCommand = new RelayCommand(BackTabPage)); }
        }


        #endregion

        #region Method

        public RegisterPatientViewModel()
        {
        
        }

        private void ReadCard()
        {
            try
            {
                //MediTech.Helpers.RDNIDWRAPPER.RDNID mRDNIDWRAPPER = new MediTech.Helpers.RDNIDWRAPPER.RDNID();
                string StartupPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

                string fileName = StartupPath + "\\RDNIDLib.DLD";
                if (System.IO.File.Exists(fileName) == false)
                {
                    ErrorDialog("RDNIDLib.DLD not found");
                    return;
                }

                byte[] _lic = String2Byte(fileName);

                int nres = 0;
                nres = RDNID.openNIDLibRD(_lic);

                string s = ListCardReader();
                if (string.IsNullOrEmpty(s))
                {
                    ErrorDialog("เครื่องอ่านบัตรไม่พร้อมใช้งานโปรดตรวจสอบ");
                    return;
                }
                String[] readlist = s.Split(';');
                String strTerminal = readlist[0];

                IntPtr obj = selectReader(strTerminal);


                Int32 nInsertCard = 0;

                nInsertCard = RDNID.connectCardRD(obj);
                if (nInsertCard != 0)
                {
                    String m;
                    m = String.Format(" error no {0} ", nInsertCard);
                    ErrorDialog(m);

                    RDNID.disconnectCardRD(obj);
                    RDNID.deselectReaderRD(obj);
                    return;
                }

                //BindData();
                byte[] id = new byte[30];
                int res = RDNID.getNIDNumberRD(obj, id);
                if (res != DefineConstants.NID_SUCCESS)
                {
                    ErrorDialog("ไมสามารถอ่าน IDNumber ได้");
                    return;
                }
                String NIDNum = aByteToString(id);


                byte[] data = new byte[1024];
                res = RDNID.getNIDTextRD(obj, data, data.Length);
                if (res != DefineConstants.NID_SUCCESS)
                {
                    ErrorDialog("ไม่สามารถอ่านข้อมูลในบัตรได้");
                    return;
                }

                String NIDData = aByteToString(data);

                if (NIDData == "")
                {
                    ErrorDialog("Read Text error");
                    return;
                }

                string[] fields = NIDData.Split('#');

                byte[] NIDPicture = new byte[1024 * 5];
                int imgsize = NIDPicture.Length;
                res = RDNID.getNIDPhotoRD(obj, NIDPicture, out imgsize);
                if (res != DefineConstants.NID_SUCCESS)
                {
                    ErrorDialog("ไม่สามารถอ่านข้อมูลภาพได้");
                    return;
                }

                byte[] byteImage = NIDPicture;

                RDNID.disconnectCardRD(obj);
                RDNID.deselectReaderRD(obj);

                RegisterPatient registerPage = (this.View as RegisterPatient);
                ManagePatient managePatient = (registerPage.ManagePage.Content as ManagePatient);
                SearchPatient searchPatient = (registerPage.SearchPage.Content as SearchPatient);
                ManagePatientViewModel managePatientViewModel = (managePatient.DataContext as ManagePatientViewModel);

                string firstName = fields[(int)NID_FIELD.NAME_T].Trim();
                string middleName = fields[(int)NID_FIELD.MIDNAME_T].Trim();
                string lastName = fields[(int)NID_FIELD.SURNAME_T].Trim();
                string nationalID = NIDNum;
                string gender;
                string province = fields[(int)NID_FIELD.PROVINCE].Trim().Replace("จังหวัด", "");
                string amphur = fields[(int)NID_FIELD.AMPHOE].Trim().Replace("อำเภอ", ""); ;
                string district = fields[(int)NID_FIELD.TUMBON].Trim().Replace("ตำบล", ""); ;
                string title = fields[(int)NID_FIELD.TITLE_T].Trim();

                if (title == "น.ส.")
                {
                    title = "นางสาว";
                }

                if (fields[(int)NID_FIELD.GENDER] == "1")
                {
                    gender = "ชาย";
                }
                else
                {
                    gender = "หญิง";
                }

                DateTime? birthDttm = _yyyymmdd_(fields[(int)NID_FIELD.BIRTH_DATE]);

                PatientInformationModel patientData = new PatientInformationModel();

                patientData.FirstName = firstName;
                patientData.LastName = lastName;
                patientData.MiddelName = middleName;

                if (gender == "ชาย")
                {
                    patientData.SEXXXUID = 1;
                }
                else if (gender == "หญิง")
                {
                    patientData.SEXXXUID = 2;
                }

                var selectTitle = managePatientViewModel.TitleSource.FirstOrDefault(p => p.Display == title);
                if (selectTitle != null)
                {
                    patientData.TITLEUID = selectTitle.Key;
                }

                patientData.NationalID = nationalID;
                patientData.Line1 = fields[(int)NID_FIELD.HOME_NO].Trim();
                patientData.Line2 = fields[(int)NID_FIELD.MOO].Trim();
                patientData.Line3 = (fields[(int)NID_FIELD.TROK].Trim() + " " + fields[(int)NID_FIELD.SOI].Trim() + " " + fields[(int)NID_FIELD.ROAD].Trim()).Trim();
                var selectedProvince = managePatientViewModel.ProvinceSource.FirstOrDefault(p => p.Display == province);
                managePatientViewModel.SelectedProvince = selectedProvince;
                var selectedAmphur = managePatientViewModel.AmphurSource.FirstOrDefault(p => p.Display == amphur);
                managePatientViewModel.SelectedAmphur = selectedAmphur;
                var selectedDistrict = managePatientViewModel.DistrictSource.FirstOrDefault(p => p.Display == district);
                managePatientViewModel.SelectedDistrict = selectedDistrict;




                if (selectedProvince != null)
                {
                    patientData.ProvinceUID = selectedProvince.Key;
                }

                if (selectedAmphur != null)
                {
                    patientData.AmphurUID = selectedAmphur.Key;
                }

                if (selectedDistrict != null)
                {
                    patientData.DistrictUID = selectedDistrict.Key;
                    patientData.ZipCode = selectedDistrict.ValueCode;
                }

                //patientData.SEXXXUID
                patientData.BirthDttm = birthDttm;
                patientData.PatientImage = byteImage;

                if (SelectPageIndex == 0)
                {
                    SearchPatientViewModel searchPatViewModel = (searchPatient.DataContext as SearchPatientViewModel);
                    searchPatViewModel.SearchPatient("", firstName, lastName, middleName, "", birthDttm, "", null, gender);

                    if (searchPatViewModel.PatientSource == null || searchPatViewModel.PatientSource.Count <= 0)
                    {
                        OpenPage(PageRegister.Manage, patientData);
                        IsManageRegister = true;
                    }
                }
                else if (SelectPageIndex == 1)
                {
                    managePatientViewModel.FirstName = firstName;
                    managePatientViewModel.LastName = lastName;
                    managePatientViewModel.SelectedTitle = managePatientViewModel.TitleSource.FirstOrDefault(p => p.Key == patientData.TITLEUID);
                    managePatientViewModel.SelectedGender = managePatientViewModel.GenderSource.FirstOrDefault(p => p.Key == patientData.SEXXXUID);
                    managePatientViewModel.NatinonalID = nationalID;

                    managePatientViewModel.BirthDate = patientData.BirthDttm;
                    if (managePatientViewModel.BirthDate != null)
                    {
                        if (managePatientViewModel.CheckBuddhist == true)
                        {
                            managePatientViewModel.BirthDate = managePatientViewModel.BirthDate.Value.AddYears(543);
                        }
                        else
                        {
                            managePatientViewModel.BirthDate = managePatientViewModel.BirthDate.Value.AddYears(-543);
                        }
                    }
                    managePatientViewModel.Line1 = patientData.Line1;
                    managePatientViewModel.Line2 = patientData.Line2;
                    managePatientViewModel.Line3 = patientData.Line3;
                    managePatientViewModel.SuppressZipCodeEvent = true;
                    managePatientViewModel.ZipCode = patientData.ZipCode;
                    managePatientViewModel.PatientImage = ImageHelpers.ConvertByteToBitmap(patientData.PatientImage);
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
           
        }

        public PatientInformationModel AssingPatientData(PatientInformationModel patientData)
        {

            return patientData;
        }

        public void SkipPage()
        {
            OpenPage(PageRegister.Manage, null);
            IsManageRegister = true;
        }

        public void NextPage()
        {
            if (SelectedPatient == null)
            {
                WarningDialog("กรุณาเลือกผู้ป่วย");
                return;
            }
            if (SelectedBooking != null)
            {
                DialogResult result = QuestionDialog("ผู้ป่วยมีนัด คุณต้องการดึงนัดมาลงทะเบียน หรือไม่?");
                if (result == DialogResult.Yes)
                {
                    OpenPage(PageRegister.Manage, SelectedPatient, SelectedBooking);
                }
                else
                {
                    OpenPage(PageRegister.Manage, SelectedPatient);
                }
            }
            else
            {
                OpenPage(PageRegister.Manage, SelectedPatient);
            }

            IsManageRegister = true;
        }

        public void BackTabPage()
        {
            if (BackwardView == null)
            {
                OpenPage(PageRegister.Search, null);
                IsManageRegister = false;
            }
            else
            {
                ChangeViewPermission(BackwardView);
            }


        }

        public void OpenPage(PageRegister page,PatientInformationModel patientData,BookingModel booking = null)
        {
            if (page == PageRegister.Search)
            {
                SelectPageIndex = 0;
            }
            else if (page == PageRegister.Manage)
            {
                RegisterPatient registerPage = (this.View as RegisterPatient);
                ManagePatient managePatient = (registerPage.ManagePage.Content as ManagePatient);
                if (managePatient.DataContext is ManagePatientViewModel)
                {
                    ManagePatientViewModel managePatViewModel = (managePatient.DataContext as ManagePatientViewModel);
                    managePatViewModel.ClearPropertiesControl();
                    managePatViewModel.Booking = booking;

                    if (patientData != null)
                    {
                        managePatViewModel.AssingModel(patientData);
                    }
                    else
                    {
                        SearchPatient searchPatient = (registerPage.SearchPage.Content as SearchPatient);
                        managePatViewModel.AssingDataFromSkip(new PatientInformationModel()
                        {
                            FirstName = searchPatient.txtFirstName.Text,
                            LastName = searchPatient.txtLastName.Text,
                            NickName = searchPatient.txtNickName.Text,
                            NationalID = searchPatient.txtNationalID.Text,
                            SEXXXUID = searchPatient.cmbGender.EditValue != null ? (int?)searchPatient.cmbGender.EditValue : null
                        });
                    }


                }
                SelectPageIndex = 1;
            }
        }

        #endregion


        #region NIDCard

        enum NID_FIELD
        {
            NID_Number,   //1234567890123#

            TITLE_T,    //Thai title#
            NAME_T,     //Thai name#
            MIDNAME_T,  //Thai mid name#
            SURNAME_T,  //Thai surname#

            TITLE_E,    //Eng title#
            NAME_E,     //Eng name#
            MIDNAME_E,  //Eng mid name#
            SURNAME_E,  //Eng surname#

            HOME_NO,    //12/34#
            MOO,        //10#
            TROK,       //ตรอกxxx#
            SOI,        //ซอยxxx#
            ROAD,       //ถนนxxx#
            TUMBON,     //ตำบลxxx#
            AMPHOE,     //อำเภอxxx#
            PROVINCE,   //จังหวัดxxx#

            GENDER,     //1#			//1=male,2=female

            BIRTH_DATE, //25200131#	    //YYYYMMDD 
            ISSUE_PLACE,//xxxxxxx#      //
            ISSUE_DATE, //25580131#     //YYYYMMDD 
            EXPIRY_DATE,//25680130      //YYYYMMDD 
            ISSUE_NUM,  //12345678901234 //14-Char
            END
        };

        DateTime _yyyymmdd_(String d)
        {
            DateTime birthDate;
            string _yyyy = d.Substring(0, 4);
            string _mm = d.Substring(4, 2);
            string _dd = d.Substring(6, 2);



            birthDate = Convert.ToDateTime(_dd + "/" + _mm + "/" + _yyyy);
            birthDate = birthDate.AddYears(-543);
            return birthDate;
        }
        public IntPtr selectReader(String reader)
        {
            IntPtr mCard = (IntPtr)0;
            byte[] _reader = String2Byte(reader);
            IntPtr res = (IntPtr)RDNID.selectReaderRD(_reader);
            if ((Int64)res > 0)
                mCard = (IntPtr)res;
            return mCard;
        }

        private string ListCardReader()
        {
            byte[] szReaders = new byte[1024 * 2];
            int size = szReaders.Length;
            int numreader = RDNID.getReaderListRD(szReaders, size);
            if (numreader <= 0)
                return "";
            String s = aByteToString(szReaders);
            return s;
        }

        static string aByteToString(byte[] b)
        {
            Encoding ut = Encoding.GetEncoding(874); // 874 for Thai langauge
            int i;
            for (i = 0; b[i] != 0; i++) ;

            string s = ut.GetString(b);
            s = s.Substring(0, i);
            return s;
        }

        static byte[] String2Byte(string s)
        {
            // Create two different encodings.
            Encoding ascii = Encoding.GetEncoding(874);
            Encoding unicode = Encoding.Unicode;

            // Convert the string into a byte array.
            byte[] unicodeBytes = unicode.GetBytes(s);

            // Perform the conversion from one encoding to the other.
            byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

            return asciiBytes;
        }

        #endregion
    }
}
