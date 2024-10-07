namespace SimpleBilling.Models
{
    public class AddItemRequestDTO
    {
        public string Name{ get; set; }
        public int CategoryId { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
    }
}
