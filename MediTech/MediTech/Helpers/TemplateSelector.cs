using DevExpress.Xpf.Grid;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MediTech.Helpers
{
    public class EditorTemplatePulmonary : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate dataTemplate = null;
            if (item != null)
            {
                dataTemplate = (DataTemplate)((FrameworkElement)container).FindResource("textblockEditor");
                if (item is GridCellData)
                {
                    GridCellData data = (GridCellData)item;
                    if (data.RowData.Row is ResultComponentModel)
                    {
                        var dataItem = data.RowData.Row as ResultComponentModel;
                        if (dataItem.IsMandatory == "Y")
                        {
                            dataTemplate = (DataTemplate)((FrameworkElement)container).FindResource("textEditEditor");
                        }
                    }

                }
            }
            return dataTemplate;
        }
    }

    public class EditorTemplateOccuVisionTest : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate dataTemplate = null;
            if (item != null)
            {

                dataTemplate = (DataTemplate)((FrameworkElement)container).FindResource("ComboBoxEdit");
                if (item is GridCellData)
                {
                    GridCellData data = (GridCellData)item;
                    if (data.RowData.Row is ResultComponentModel)
                    {
                        var dataItem = data.RowData.Row as ResultComponentModel;
                        if (dataItem.ResultItemCode == "TIMUS1" || dataItem.ResultItemCode == "TIMUS2" || dataItem.ResultItemCode == "TIMUS3"
                            || dataItem.ResultItemCode == "TIMUS4")
                        {
                            dataTemplate = (DataTemplate)((FrameworkElement)container).FindResource("ListBoxRadioButtonEdit");
                        }
                        else if (dataItem.ResultItemCode == "TIMUS9" || dataItem.ResultItemCode == "TIMUS16" || dataItem.ResultItemCode == "TIMUS17")
                        {
                            dataTemplate = (DataTemplate)((FrameworkElement)container).FindResource("ListBoxCheckEdit");
                        }

                    }

                }
            }
            return dataTemplate;
        }
    }
}
