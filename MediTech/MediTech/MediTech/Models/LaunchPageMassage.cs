using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MediTech.Models
{
    public class LaunchPageMassage
    {
        public string TitleName { get; set; }
        public object PageObject { get; set; }
        public MediTech.Model.PageViewModel PageView { get; set; }
        public object BackwardView { get; set; }
    }
}
