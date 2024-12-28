using Microsoft.EntityFrameworkCore;
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
        public Guid CategoryId { get; set; }

        [Display(Name = "Unit")]
        public string Unit { get; set; }

        [Display(Name = "Unit Price")]
        [Precision(18, 2)]
        public decimal Price {get; set; }
    }
}
