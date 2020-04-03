using DevExpress.Xpf.Editors;
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
    /// Interaction logic for PatientVitalSign.xaml
    /// </summary>
    public partial class PatientVitalSign : UserControl
    {
        double maxTemp = 37.5;
        double minTemp = 36.5;

        double maxPluse = 100;
        double minPluse = 60;

        double maxResRate = 22;
        double minResRate = 17;

        double maxSBP = 130;
        double minSBP = 90;

        double maxDBP = 85;
        double minDBP = 60;
        public PatientVitalSign()
        {
            InitializeComponent();
            txtWeight.EditValueChanged += txtWeight_EditValueChanged;
            txtHeight.EditValueChanged += txtHeight_EditValueChanged;
            txtTempe.EditValueChanged += txtTempe_EditValueChanged;
            txtPluse.EditValueChanged += txtPluse_EditValueChanged;
            txtRespiratoryRate.EditValueChanged += txtRespiratoryRate_EditValueChanged;
            txtSBP.EditValueChanged += txtSBP_EditValueChanged;
            txtDBP.EditValueChanged += txtDBP_EditValueChanged;
            txtHeightRe.EditValueChanged += txtHeightRe_EditValueChanged;
            txtWeightRe.EditValueChanged += txtWeightRe_EditValueChanged;
            txtTempeRe.EditValueChanged += txtTempeRe_EditValueChanged;
            txtPluseRe.EditValueChanged += txtPluseRe_EditValueChanged;
            txtRespiratoryRateRe.EditValueChanged += txtRespiratoryRateRe_EditValueChanged;
            txtSBPRe.EditValueChanged += txtSBPRe_EditValueChanged;
            txtDBPRe.EditValueChanged += txtDBPRe_EditValueChanged;

            if (this.DataContext is PatientVitalSignViewModel)
            {
                (this.DataContext as PatientVitalSignViewModel).UpdateEvent += PatientVitalSign_UpdateEvent;
            }
        }

        private void PatientVitalSign_UpdateEvent(object sender, EventArgs e)
        {
            gcRecentVital.RefreshData();
        }

        string CalculateBMI(string h, string w)
        {
            string ret = string.Empty;
            try
            {
                //Weight/(Height/100*Height/100)
                //ret = (float.Parse(w) / (float.Parse(h) / 100 * float.Parse(h) / 100)).ToString();
                ret = String.Format("{0:F2}", (float.Parse(w) / (float.Parse(h) / 100 * float.Parse(h) / 100)));



            }
            catch (Exception)
            {
                return ret;
            }


            return ret;
        }

        string CalculateBSA(string h, string w)
        {
            string ret = string.Empty;
            try
            {
                //(Weight^0.425) * (Height^0.725)* 0.007184
                //ret = (Math.Pow(Double.Parse(w), 0.425) * Math.Pow(Double.Parse(h), 0.725) * 0.007184).ToString();
                ret = String.Format("{0:F2}", (Math.Pow(Double.Parse(w), 0.425) * Math.Pow(Double.Parse(h), 0.725) * 0.007184));
            }
            catch (Exception)
            {
                return ret;
            }


            return ret;
        }

        void AdjustStatus(TextBlock name, TextEdit value, TextBlock unit, TextBlock rage, TextBlock status, string rageValue)
        {
            Brush DefaultColor = txtHeight.Foreground;
            if (rageValue == "H" || rageValue == "L")
            {
                name.Foreground = Brushes.Red;
                value.Foreground = Brushes.Red;
                unit.Foreground = Brushes.Red;
                rage.Foreground = Brushes.Red;
                status.Foreground = Brushes.Red;
            }
            else
            {
                name.Foreground = DefaultColor;
                value.Foreground = DefaultColor;
                unit.Foreground = DefaultColor;
                rage.Foreground = DefaultColor;
                status.Foreground = DefaultColor;
            }
            status.Text = rageValue;
        }

        private void txtWeight_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (txtWeight.Text.Trim() != "" && txtHeight.Text.Trim() != "")
            {
                string BMI = CalculateBMI(txtHeight.Text.Trim(), txtWeight.Text.Trim());
                string BSA = CalculateBSA(txtHeight.Text.Trim(), txtWeight.Text.Trim());
                txtBMI.Text = BMI;
                txtBSA.Text = BSA;
            }
        }

        private void txtHeight_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (txtWeight.Text.Trim() != "" && txtHeight.Text.Trim() != "")
            {
                string BMI = CalculateBMI(txtHeight.Text.Trim(), txtWeight.Text.Trim());
                string BSA = CalculateBSA(txtHeight.Text.Trim(), txtWeight.Text.Trim());
                txtBMI.Text = BMI;
                txtBSA.Text = BSA;
            }
        }

        private void txtTempe_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            double value;
            if (txtTempe.EditValue != null && txtTempe.EditValue.ToString() != "")
            {
                value = Convert.ToDouble(txtTempe.EditValue);
                if (value > maxTemp)
                {
                    AdjustStatus(lblTempe, txtTempe, lblTempUnit, lblTempRage, lblTempStatus, "H");
                }
                else if (value < minTemp)
                {
                    AdjustStatus(lblTempe, txtTempe, lblTempUnit, lblTempRage, lblTempStatus, "L");
                }
                else
                {
                    AdjustStatus(lblTempe, txtTempe, lblTempUnit, lblTempRage, lblTempStatus, "");
                }
            }
            else
            {
                AdjustStatus(lblTempe, txtTempe, lblTempUnit, lblTempRage, lblTempStatus, "");
            }

        }

        private void txtPluse_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            double value;

            if (txtPluse.EditValue != null && txtPluse.EditValue.ToString() != "")
            {
                value = Convert.ToDouble(txtPluse.EditValue);
                if (value > maxPluse)
                {
                    AdjustStatus(lblPluse, txtPluse, lblPluseUnit, lblPluseRage, lblPluseStatus, "H");
                }
                else if (value < minPluse)
                {
                    AdjustStatus(lblPluse, txtPluse, lblPluseUnit, lblPluseRage, lblPluseStatus, "L");
                }
                else
                {
                    AdjustStatus(lblPluse, txtPluse, lblPluseUnit, lblPluseRage, lblPluseStatus, "");
                }
            }
            else
            {
                AdjustStatus(lblPluse, txtPluse, lblPluseUnit, lblPluseRage, lblPluseStatus, "");
            }


        }

        private void txtRespiratoryRate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            double value;
            if (txtRespiratoryRate.EditValue != null && txtRespiratoryRate.EditValue.ToString() != "")
            {
                value = Convert.ToDouble(txtRespiratoryRate.EditValue);
                if (value > maxResRate)
                {
                    AdjustStatus(lblRssRate, txtRespiratoryRate, lblRssRateUnit, lblRssRateRage, lblRssRateStatus, "H");
                }
                else if (value < minResRate)
                {
                    AdjustStatus(lblRssRate, txtRespiratoryRate, lblRssRateUnit, lblRssRateRage, lblRssRateStatus, "L");
                }
                else
                {
                    AdjustStatus(lblRssRate, txtRespiratoryRate, lblRssRateUnit, lblRssRateRage, lblRssRateStatus, "");
                }
            }
            else
            {
                AdjustStatus(lblRssRate, txtRespiratoryRate, lblRssRateUnit, lblRssRateRage, lblRssRateStatus, "");
            }

        }

        private void txtSBP_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            double value;
            if (txtSBP.EditValue != null && txtSBP.EditValue.ToString() != "")
            {
                value = Convert.ToDouble(txtSBP.EditValue);
                if (value > maxSBP)
                {
                    AdjustStatus(lblSBP, txtSBP, lblSBPUnit, lblSBPRage, lblSBPStatus, "H");
                }
                else if (value < minSBP)
                {
                    AdjustStatus(lblSBP, txtSBP, lblSBPUnit, lblSBPRage, lblSBPStatus, "L");
                }
                else
                {
                    AdjustStatus(lblSBP, txtSBP, lblSBPUnit, lblSBPRage, lblSBPStatus, "");
                }
            }
            else
            {
                AdjustStatus(lblSBP, txtSBP, lblSBPUnit, lblSBPRage, lblSBPStatus, "");
            }
        }

        private void txtDBP_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            double value;
            if (txtDBP.EditValue != null && txtDBP.EditValue.ToString() != "")
            {
                value = Convert.ToDouble(txtDBP.EditValue);
                if (value > maxDBP)
                {
                    AdjustStatus(lblDBP, txtDBP, lblDBPUnit, lblDBPRage, lblDBPStatus, "H");
                }
                else if (value < minDBP)
                {
                    AdjustStatus(lblDBP, txtDBP, lblDBPUnit, lblDBPRage, lblDBPStatus, "L");
                }
                else
                {
                    AdjustStatus(lblDBP, txtDBP, lblDBPUnit, lblDBPRage, lblDBPStatus, "");
                }
            }
            else
            {
                AdjustStatus(lblDBP, txtDBP, lblDBPUnit, lblDBPRage, lblDBPStatus, "");
            }


        }

        private void txtHeightRe_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (txtWeightRe.Text.Trim() != "" && txtHeightRe.Text.Trim() != "")
            {
                string BMI = CalculateBMI(txtHeightRe.Text.Trim(), txtWeightRe.Text.Trim());
                string BSA = CalculateBSA(txtHeightRe.Text.Trim(), txtWeightRe.Text.Trim());
                txtBMIRe.Text = BMI;
                txtBSARe.Text = BSA;
            }
        }

        private void txtWeightRe_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (txtWeightRe.Text.Trim() != "" && txtHeightRe.Text.Trim() != "")
            {
                string BMI = CalculateBMI(txtHeightRe.Text.Trim(), txtWeightRe.Text.Trim());
                string BSA = CalculateBSA(txtHeightRe.Text.Trim(), txtWeightRe.Text.Trim());
                txtBMIRe.Text = BMI;
                txtBSARe.Text = BSA;
            }
        }

        private void txtTempeRe_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            double value;
            if (txtTempeRe.EditValue != null && txtTempeRe.EditValue.ToString() != "")
            {
                value = Convert.ToDouble(txtTempeRe.EditValue);
                if (value > maxTemp)
                {
                    AdjustStatus(lblTempRe, txtTempeRe, lblTempReUnit, lblTempRage, lblTempReStatus, "H");
                }
                else if (value < minTemp)
                {
                    AdjustStatus(lblTempRe, txtTempeRe, lblTempReUnit, lblTempRage, lblTempReStatus, "L");
                }
                else
                {
                    AdjustStatus(lblTempRe, txtTempeRe, lblTempReUnit, lblTempRage, lblTempReStatus, "");
                }

            }
            else
            {
                AdjustStatus(lblTempRe, txtTempeRe, lblTempReUnit, lblTempRage, lblTempReStatus, "");
            }

        }

        private void txtPluseRe_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            double value;
            if (txtPluseRe.EditValue != null && txtPluseRe.EditValue.ToString() != "")
            {
                value = Convert.ToDouble(txtPluseRe.EditValue);
                if (value > maxPluse)
                {
                    AdjustStatus(lblPluseRe, txtPluseRe, lblPluseReUnit, lblPluseReRage, lblPluseReStatus, "H");
                }
                else if (value < minPluse)
                {
                    AdjustStatus(lblPluseRe, txtPluseRe, lblPluseReUnit, lblPluseReRage, lblPluseReStatus, "L");
                }
                else
                {
                    AdjustStatus(lblPluseRe, txtPluseRe, lblPluseReUnit, lblPluseReRage, lblPluseReStatus, "");
                }
            }
            else
            {
                AdjustStatus(lblPluseRe, txtPluseRe, lblPluseReUnit, lblPluseReRage, lblPluseReStatus, "");
            }

        }

        private void txtRespiratoryRateRe_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            double value;
            if (txtRespiratoryRateRe.EditValue != null && txtRespiratoryRateRe.EditValue.ToString() != "")
            {
                value = Convert.ToDouble(txtRespiratoryRateRe.EditValue);
                if (value > maxResRate)
                {
                    AdjustStatus(lblRssRateRe, txtRespiratoryRateRe, lblRssRateReUnit, lblRssRateReRage, lblRssRateReStatus, "H");
                }
                else if (value < minResRate)
                {
                    AdjustStatus(lblRssRateRe, txtRespiratoryRateRe, lblRssRateReUnit, lblRssRateReRage, lblRssRateReStatus, "L");
                }
                else
                {
                    AdjustStatus(lblRssRateRe, txtRespiratoryRateRe, lblRssRateReUnit, lblRssRateReRage, lblRssRateReStatus, "");
                }
            }
            else
            {
                AdjustStatus(lblRssRateRe, txtRespiratoryRateRe, lblRssRateReUnit, lblRssRateReRage, lblRssRateReStatus, "");
            }


        }

        private void txtSBPRe_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            double value;
            if (txtSBPRe.EditValue != null && txtSBPRe.EditValue.ToString() != "")
            {
                value = Convert.ToDouble(txtSBPRe.EditValue);
                if (value > maxSBP)
                {
                    AdjustStatus(lblSBPRe, txtSBPRe, lblSBPReUnit, lblSBPReRage, lblSBPReStatus, "H");
                }
                else if (value < minSBP)
                {
                    AdjustStatus(lblSBPRe, txtSBPRe, lblSBPReUnit, lblSBPReRage, lblSBPReStatus, "L");
                }
                else
                {
                    AdjustStatus(lblSBPRe, txtSBPRe, lblSBPReUnit, lblSBPReRage, lblSBPReStatus, "");
                }
            }
            else
            {
                AdjustStatus(lblSBPRe, txtSBPRe, lblSBPReUnit, lblSBPReRage, lblSBPReStatus, "");
            }

        }

        private void txtDBPRe_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            double value;
            if (txtSBPRe.EditValue != null && txtSBPRe.EditValue.ToString() != "")
            {
                value = Convert.ToDouble(txtDBPRe.EditValue);
                if (value > maxDBP)
                {
                    AdjustStatus(lblDBPRe, txtDBPRe, lblDBPReUnit, lblDBPReRage, lblDBPReStatus, "H");
                }
                else if (value < minDBP)
                {
                    AdjustStatus(lblDBPRe, txtDBPRe, lblDBPReUnit, lblDBPReRage, lblDBPReStatus, "L");
                }
                else
                {
                    AdjustStatus(lblDBPRe, txtDBPRe, lblDBPReUnit, lblDBPReRage, lblDBPReStatus, "");
                }
            }
            else
            {
                AdjustStatus(lblDBPRe, txtDBPRe, lblDBPReUnit, lblDBPReRage, lblDBPReStatus, "");
            }
        }
    }
}
