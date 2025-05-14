using System.ComponentModel.DataAnnotations;

namespace TechnicalTestVDI.Models
{
    public class DiscountTransaction
    {
        [Key]
        public string TransactionId { get; set; }
        public string CustomerType { get; set; }
        public int PointReward { get; set; }
        public decimal TotalPurchase { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPay { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
