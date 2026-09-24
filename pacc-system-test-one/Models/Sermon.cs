using System;
using System.Collections.Generic;
using System.Text;

namespace pacc_system_test_one.Models
{
    public class Sermon
    {
        public int SermonId { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; } = "";

        public string Content { get; set; } = "";

        public DateTime Date { get; set; }

        // Navigation/display information
        public string PastorName { get; set; } = "";

        public string ImageUrl { get; set; } = "";

        public string Duration { get; set; } = "";
    }
}
