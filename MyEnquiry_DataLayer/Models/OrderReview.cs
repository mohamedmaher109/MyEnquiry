using System.ComponentModel.DataAnnotations.Schema;


namespace MyEnquiry_DataLayer.Models
{
    public class OrderReview
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public int Type { get; set; } //1 for Represitative 2 for reviewer

        public int Rate { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public CasesOrders Order { get; set; }
    }
}
