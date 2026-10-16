
namespace pacc_system_test_one.Models
{
    public class PrayerRequest
    {
        public string RequestId { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public DateTime Date { get; set; } = DateTime.Now;

        public bool Confidential { get; set; }
    }
}
