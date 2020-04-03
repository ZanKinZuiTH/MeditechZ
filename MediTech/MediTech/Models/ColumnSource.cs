using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Models
{
    public class Column
    {
        // Specifies the name of a data source field to which the column is bound. 
        public string FieldName { get; set; }
        // Specifies the type of an in-place editor used to edit column values. 
        public string Header { get; set; }
    }
}
