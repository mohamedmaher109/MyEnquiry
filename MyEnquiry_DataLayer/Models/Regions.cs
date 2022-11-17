using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
    public class Regions
    {
        public int Id { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public bool Active { get; set; }

        public bool Deleted { get; set; }
        public int? CitiesId { get; set; }
        [ForeignKey("CitiesId")]
        public Cities Cities { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //public int CityId { get; set; }
        //[ForeignKey("CityId")]
        //public Cities City { get; set; }
    }
}
