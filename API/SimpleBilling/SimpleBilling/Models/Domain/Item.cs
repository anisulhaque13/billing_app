using System.ComponentModel.DataAnnotations;

namespace SimpleBilling.Models.Domain
{
    public class Item
    {
        [Display(Name = "Itam Id")]
        public Guid Id { get; set; }

        [Display(Name = "Item Name")]
        public string Name { get; set; }


        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }

        [Display(Name = "Unit")]
        public string Unit { get; set; }

        [Display(Name = "Unit Price")]
        public decimal Price {get; set; }
    }
}
