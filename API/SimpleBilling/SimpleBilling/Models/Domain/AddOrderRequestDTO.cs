namespace SimpleBilling.Models.Domain
{
    public class AddOrderRequestDTO
    {
        public DateTime OrderDate { get; set; } // Represents the order date

        // A collection of order details
        public List<OrderDetailDTO> OrderDetails { get; set; } = new List<OrderDetailDTO>();
    }

    public class OrderDetailDTO
    {
        public Guid CategoryId { get; set; } // ID of the category
        public string CategoryName { get; set; } // Name of the category
        public Guid ItemId { get; set; } // ID of the item
        public string ItemName { get; set; } // Name of the item
        public decimal Price { get; set; } // Price of the item
        public int Quantity { get; set; } // Quantity of the item
    }

}
