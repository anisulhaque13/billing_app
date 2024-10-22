namespace SimpleBilling.Models
{
    public class ItemResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } // Ensure this is included
        public string Unit { get; set; }
        public decimal Price { get; set; }
    }
}
