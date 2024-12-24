using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SimpleBilling.Models.Domain
{
    public class OrderDetail
    {
        public OrderDetail(Guid itemId, string itemName, Guid categoryId, string categoryName, decimal quantity, decimal price, Guid orderId)
        {
            Id = Guid.NewGuid(); // Automatically generate a new unique ID
            ItemId = itemId;
            ItemName = itemName;
            CategoryId = categoryId;
            CategoryName = categoryName;
            Quantity = quantity;
            Price = price;
            OrderId = orderId;
            // Initialize navigation properties
            Item = new Item();       // Replace with default initialization if applicable
            Category = new Category(); // Replace with default initialization if applicable
            Order = new Order();     // Replace with default initialization if applicable
        }
        [Display(Name = "Order Detail Id")]
        public Guid Id { get; set; }


        [Display(Name = "Item Id")]
        public Guid ItemId { get; set; }
        public virtual Item Item{ get; set; } 

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "Category Id")]
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        [Display(Name = "Quantity")]
        [Precision(18, 2)]
        public decimal Quantity { get; set; }

        [Display(Name = "Price")]
        [Precision(18, 2)]
        public decimal Price { get; set; }

        // Optional: Navigation property to the associated Order
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

    }
}
