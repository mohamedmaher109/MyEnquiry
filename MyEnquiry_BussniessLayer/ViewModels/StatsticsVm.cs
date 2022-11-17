using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.ViewModels
{
    public class StatsticsVm
    {
        public string NameBank { get; set; }
        public string NameCompany { get; set; }
        public int NumberOfCasesDone { get; set; }
        public int NumberOfCasesWaiting { get; set; }
    }
}
