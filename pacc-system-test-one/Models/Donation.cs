namespace pacc_system_test_one.Models
{
    public class Donation
    {
        public string DonationId { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public string DonorName { get; set; } = string.Empty;

        public string GivingType { get; set; } = "General";

        public string PaymentMethod { get; set; } = "Cash";
    }
}