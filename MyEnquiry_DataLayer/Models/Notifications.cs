using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
    public class Notifications
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string FromId { get; set; }

        public string ToId { get; set; }



        public int OrderId { get; set; }

        public bool IsDeleted { get; set; }

        public bool Status { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("ToId")]
         public virtual ApplicationUser To { get; set; }


        [ForeignKey("FromId")]
         public virtual ApplicationUser From { get; set; }
    }
}
