using DevExpress.Data.Filtering;
using DevExpress.Xpf.Grid.LookUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.CustomControl
{
    public class MyLookUpEditStrategy : LookUpEditStrategy
    {
        public MyLookUpEditStrategy(LookUpEdit editor)
            : base(editor)
        {
        }

        public override void UpdateDisplayFilter()
        {
            Editor.Dispatcher.BeginInvoke(new Action(() => ItemsProvider.SetDisplayFilterCriteria(CreateDisplayFilterCriteria(AutoSearchText), this.CurrentDataViewHandle)));

        }
        protected CriteriaOperator CreateDisplayFilterCriteria(string text)
        {
            MyLookUpEdit lookUp = (MyLookUpEdit)Editor;
            GroupOperator groupOperator = new GroupOperator() { OperatorType = GroupOperatorType.Or };
            foreach (ColumnItem col in lookUp.ColumnsToFilter)
            {
                List<CriteriaOperator> list = new List<CriteriaOperator>();
                list.Add(new OperandProperty() { PropertyName = col.FieldName });
                list.Add(new OperandValue() { Value = text });
                groupOperator.Operands.Add(new FunctionOperator(FunctionOperatorType.Contains, list));
            }
            return groupOperator;
        }
    }
}
