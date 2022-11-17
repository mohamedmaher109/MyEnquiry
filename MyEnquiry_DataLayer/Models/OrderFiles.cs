using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
    public class OrderFiles
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public int Type { get; set; }//1 for word 2 for img
        public int FromType { get; set; }//1 From Represintastive 2 From Reviwer 

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public CasesOrders Order { get; set; }
    }
}
