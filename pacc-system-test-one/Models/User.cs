using System;
using System.Collections.Generic;
using System.Text;

namespace pacc_system_test_one.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string EmailAddress { get; set; } = "";

        public string UserType { get; set; } = "User";

        public string FullName => $"{FirstName} {LastName}";
    }
}
