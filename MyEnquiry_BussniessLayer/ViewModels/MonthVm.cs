using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.ViewModels
{
    public class MonthVm
    {
        public string NameBank { get; set; }
        public List<DuiesBank> Jan { get; set; }
        public List<DuiesBank> Feb { get; set; }
        public List<DuiesBank> MArc { get; set; }
        public List<DuiesBank> April { get; set; }
        public List<DuiesBank> May { get; set; }
        public List<DuiesBank> Jun { get; set; }
        public List<DuiesBank> Jul { get; set; }
        public List<DuiesBank> Agu { get; set; }
        public List<DuiesBank> Sept { get; set; }
        public List<DuiesBank> oct { get; set; }
        public List<DuiesBank> Nuv { get; set; }
        public List<DuiesBank> Dec { get; set; }
    }
}
