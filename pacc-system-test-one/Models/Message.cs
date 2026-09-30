namespace pacc_system_test_one.Models
{
    public class Message
    {
        public string MessageId { get; set; } = "";

        public string UserId { get; set; } = "";

        public string UserName { get; set; } = "";

        public string Title { get; set; } = "";

        public string Content { get; set; } = "";

        public DateTime Date { get; set; }

        public bool IsRead { get; set; }
    }
}