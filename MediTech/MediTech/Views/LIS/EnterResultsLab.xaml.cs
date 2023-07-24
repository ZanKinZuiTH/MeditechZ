using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraRichEdit.Import.OpenXml;
using MediTech.Model;
using MediTech.Reports.Operating.Checkup;
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
    /// Interaction logic for EnterLabResults.xaml
    /// </summary>
    public partial class EnterResultsLab : UserControl
    {
        public EnterResultsLab()
        {
            InitializeComponent();

        }


        private void ChangeVal(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeChangedEventArgs e)
        {
            var type = e.ChangeType;
            var data = e.Node.Content as ResultComponentModel;
            var nodeChangeType = DevExpress.Data.TreeList.NodeChangeType.Content;
            if (data != null)
            {
                if (data.ResultItemCode == "C0070")
                {
                    if (data.ResultValue != null && type == nodeChangeType)
                    {

                        var Oldresult = (this.DataContext as EnterResultsLabViewModel).RequestDetailLabs.FirstOrDefault(p => p.RequestItemCode == "LAB212");
                        var status = Oldresult.OrderStatus;


                        if (status != "Reviewed")
                        {
                            if (data.ResultValue != "")
                            {
                                double Scr = (data.ResultValue != "") ? double.Parse(data.ResultValue) : 0;
                                (this.DataContext as EnterResultsLabViewModel).CalculateEGFR(Scr);
                            }
                        }

                        if (status == "Reviewed")
                       {
                            (this.DataContext as EnterResultsLabViewModel).CalculateEGFR(0);
                            var valThai2 = Oldresult.ResultComponents.Where(p => p.ResultItemCode == "C0073").FirstOrDefault().ResultValue;
                            var valAfrica2 = Oldresult.ResultComponents.Where(p => p.ResultItemCode == "C0074").FirstOrDefault().ResultValue;
                            var Non2 = Oldresult.ResultComponents.Where(p => p.ResultItemCode == "C0075").FirstOrDefault().ResultValue;

                            if (valThai2 == null && valAfrica2 == null && Non2 == null)
                            {
                                double Scr = (data.ResultValue != "") ? double.Parse(data.ResultValue) : 0;
                                (this.DataContext as EnterResultsLabViewModel).CalculateEGFR(Scr);
                            }
                        }
                    }

                }
            }
        }
    }
}
