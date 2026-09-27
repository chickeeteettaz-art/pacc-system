using System;
using System.Collections.Generic;
using System.Text;

namespace pacc_system_test_one.Models
{
    public class Announcement
    {
        public string AnnouncementId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime AnnouncementDate { get; set; } = DateTime.Now;

        public string Location { get; set; } = string.Empty;

        public string Category { get; set; } = "General";

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
