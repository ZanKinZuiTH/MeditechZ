using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ManageCheckupJob.xaml
    /// </summary>
    public partial class ManageCheckupJob : UserControl
    {
        public ManageCheckupJob()
        {
            InitializeComponent();
            gvJobTask.CellValueChanged += GvJobTask_CellValueChanged;
        }

        private void GvJobTask_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            if (e.Column.Equals(colDisplayOrder))
            {
                var dataTaskList = (grdJobTask.ItemsSource as ObservableCollection<CheckupJobTaskModel>);
                if (dataTaskList != null)
                {
                    int oldValue = int.Parse(e.OldValue.ToString());
                    int value = int.Parse(e.Value.ToString());
                    foreach (var task in dataTaskList)
                    {

                        if (!task.Equals(e.Row) && (task.DisplayOrder < oldValue))
                        {
                            int number = (task.DisplayOrder ?? 0) - value;
                            if ((number == 1 && value != 0) || number == 0)
                            {
                                value = (++task.DisplayOrder ?? 0);
                            }
                        }
                        else if (!task.Equals(e.Row) && (task.DisplayOrder > oldValue))
                        {
                            int number = (task.DisplayOrder ?? 0) - value;
                            if (number == 0)
                            {
                                value = (++task.DisplayOrder ?? 0);
                            }
                        }
                    }


                }
                grdJobTask.ItemsSource = new ObservableCollection<CheckupJobTaskModel>(dataTaskList?.OrderBy(p => p.DisplayOrder));
                grdJobTask.RefreshData();
                grdJobTask.RefreshRow(e.RowHandle);
            }
        }

    }
}
