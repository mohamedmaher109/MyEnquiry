using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public string userId { get; set; }

        [ForeignKey("userId")]
         public virtual ApplicationUser user { get; set; }
    }
}
