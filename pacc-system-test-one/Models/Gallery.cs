namespace pacc_system_test_one.Models
{
    public class Gallery
    {
        public string ImageId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}