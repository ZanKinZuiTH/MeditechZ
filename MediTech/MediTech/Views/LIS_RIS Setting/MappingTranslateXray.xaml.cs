using DevExpress.Xpf.Grid;
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

namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for MappingTranslateXray.xaml
    /// </summary>
    public partial class MappingTranslateXray : UserControl
    {
        public MappingTranslateXray()
        {
            InitializeComponent();
            gvMappingXray.ValidateRow += gvMappingXray_ValidateRow;
            gvMappingXray.InvalidRowException += gvMappingXray_InvalidRowException;
        }

        void gvMappingXray_InvalidRowException(object sender, DevExpress.Xpf.Grid.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;

        }

        void gvMappingXray_ValidateRow(object sender, DevExpress.Xpf.Grid.GridRowValidationEventArgs e)
        {
            if (e.Row == null) return;
            XrayTranslateMappingModel newItem = (XrayTranslateMappingModel)e.Row;
            if (String.IsNullOrEmpty(newItem.EngResult))
            {
                e.IsValid = false;
                MessageBox.Show("กรุณาใส่ข้อมูลภาษาอังกฤษ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                e.Handled = true;
                return;
            }
            if (newItem.Type == null)
            {
                e.IsValid = false;
                MessageBox.Show("กรุณาเลือกประเภท", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                e.Handled = true;
                return;
            }
            if (newItem.IsKeyword == false)
            {
                if (string.IsNullOrEmpty(newItem.ThaiResult))
                {
                    e.IsValid = false;
                    MessageBox.Show("กรุณาใส่ข้อมูลภาษาไทย", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    e.Handled = true;
                    return;
                }
            }
        }

    }
}
