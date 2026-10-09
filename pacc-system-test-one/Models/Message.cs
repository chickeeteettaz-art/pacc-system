namespace pacc_system_test_one.Models
{
    public class MobileMessage
    {
        // Firestore document ID used for updates and deletion.
        public string DocumentId { get; set; } = string.Empty;

        public string MessageId { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Reason { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Now;

        public bool IsRead { get; set; }
    }
}