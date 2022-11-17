using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
    public class Helps
    {
        public Helps()
        {
            CreatedOn = DateTime.Now.ToUniversalTime();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string English { get; set; }
        public bool IsDeleted { get; set; }
        public string Arabic { get; set; }
    }
}
