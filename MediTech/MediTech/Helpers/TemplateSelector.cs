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
                        else if (dataItem.ResultItemCode == "TIMUS19" || dataItem.ResultItemCode == "TIMUS20" || dataItem.ResultItemCode == "TIMUS21"
                            || dataItem.ResultItemCode == "TIMUS22" || dataItem.ResultItemCode == "TIMUS23" || dataItem.ResultItemCode == "TIMUS24")
                        {
                            dataTemplate = (DataTemplate)((FrameworkElement)container).FindResource("textEditEditor");
                        }
                    }

                }
            }
            return dataTemplate;
        }
    }

    public class EditorTemplateAudio : DataTemplateSelector
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
                        if (dataItem.ResultItemName == "แปลผลหูขวา" || dataItem.ResultItemName == "แปลผลหูซ้าย"
                            || dataItem.ResultItemName == "สรุปผลหูขวา" || dataItem.ResultItemName == "สรุปผลหูซ้าย"
                            )
                        {
                            dataTemplate = (DataTemplate)((FrameworkElement)container).FindResource("textEditEditor");
                        }

                    }

                }
            }
            return dataTemplate;
        }
    }

    public class EditorTemplateFitnessTest : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate dataTemplate = null;
            if (item != null)
            {

                dataTemplate = (DataTemplate)((FrameworkElement)container).FindResource("textEditEditor");
                if (item is GridCellData)
                {
                    GridCellData data = (GridCellData)item;
                    if (data.RowData.Row is ResultComponentModel)
                    {
                        var dataItem = data.RowData.Row as ResultComponentModel;
                        if (dataItem.ResultItemCode == "PAR1221"   || dataItem.ResultItemCode == "PAR1225")
                        {
                            dataTemplate = (DataTemplate)((FrameworkElement)container).FindResource("ListBoxCheckEdit");
                        }
                        else if (dataItem.ResultItemCode == "PAR1223" || dataItem.ResultItemCode == "PAR1227" || dataItem.ResultItemCode == "PAR1228")
                        {
                            dataTemplate = (DataTemplate)((FrameworkElement)container).FindResource("RadioListBoxEdit");
                        }
                        else if (dataItem.ResultItemCode == "PAR1222" || dataItem.ResultItemCode == "PAR1224" || dataItem.ResultItemCode == "PAR1226"
                            || dataItem.ResultItemCode == "PAR1229" || dataItem.ResultItemCode == "PAR1230")
                        {
                            dataTemplate = (DataTemplate)((FrameworkElement)container).FindResource("textEditEditor");
                        }
                    }

                }
            }
            return dataTemplate;
        }
    }
}
