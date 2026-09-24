using System;
using System.Collections.Generic;
using System.Text;

namespace pacc_system_test_one.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; } = "";

        public string Content { get; set; } = "";

        public DateTime Date { get; set; }

        public string UserName { get; set; } = "";

        public bool IsRead { get; set; }
    }
}
