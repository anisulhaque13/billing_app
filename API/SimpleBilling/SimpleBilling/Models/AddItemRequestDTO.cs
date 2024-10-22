namespace SimpleBilling.Models
{
    public class AddItemRequestDTO
    {
        public string Name{ get; set; }
        public Guid CategoryId { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
    }
}
