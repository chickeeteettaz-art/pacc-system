namespace pacc_system_test_one.Models
{
    public class PrayerRequest
    {
        public string RequestId { get; set; } = "";

        public string UserId { get; set; } = "";

        public string Title { get; set; } = "";

        public string Content { get; set; } = "";

        public DateTime Date { get; set; }

        public string UserName { get; set; } = "";

        public string Status { get; set; } = "Pending";
    }
}