using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Lot
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Мінімальна ціна має бути більшою або дорівнювати 1.")]
        public decimal MinPrice { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Ціна викупу має бути більшою або дорівнювати 1.")]
        public decimal BuyoutPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Початкова ставка має бути невід'ємним значенням.")]
        public decimal CurrentBid { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<Bid> Bids { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
    }
}
