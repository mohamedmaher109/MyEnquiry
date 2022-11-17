using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
   public class UserWallet
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public int CaseId { get; set; }
        public bool Paid { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [ForeignKey("CaseId")]
        public Cases Case { get; set; }
    }
}
