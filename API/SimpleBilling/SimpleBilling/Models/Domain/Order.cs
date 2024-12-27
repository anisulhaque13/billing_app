using System.ComponentModel.DataAnnotations;

namespace SimpleBilling.Models.Domain
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }

        [Display(Name = "Order Id")]
        public Guid Id { get; set; }

        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
