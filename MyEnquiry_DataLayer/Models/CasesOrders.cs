using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
    public class CasesOrders
    {
        public CasesOrders()
        {
            this.OrderFiles = new HashSet<OrderFiles>();
            this.OrderReview = new HashSet<OrderReview>();
        }
        public int Id { get; set; }

        public int Status { get; set; }//1 Pending 2 Accepted 3 Rejected


        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
        public int CaseId { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [ForeignKey("CaseId")]
        public Cases Cases { get; set; }

        public HashSet<OrderFiles> OrderFiles { get; set; }
        public HashSet<OrderReview> OrderReview { get; set; }
    }
}
