
using System.ComponentModel.DataAnnotations;

namespace Deliverable_2_StudentBookExchangeMVC.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Condition { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsAvailable { get; set; } = true;

        public string? SellerId { get; set; }
    }
}
