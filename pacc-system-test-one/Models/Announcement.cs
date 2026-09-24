using System;
using System.Collections.Generic;
using System.Text;

namespace pacc_system_test_one.Models
{
    public class Announcement
    {
        public int AnnouncementId { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public string Location { get; set; } = "";

        public DateTime Date { get; set; }
    }
}
