using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
    public class Cities
    {
        public Cities()
        {
            //this.Regions = new HashSet<Regions>();
        }
        public int Id { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public bool Active { get; set; }

        public bool Deleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //public HashSet<Regions> Regions { get; set; }
    }
}
