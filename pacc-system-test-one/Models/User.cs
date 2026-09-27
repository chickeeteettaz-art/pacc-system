namespace pacc_system_test_one.Models
{
    public class User
    {
        public string UserId { get; set; } = "";

        public string FirebaseUid { get; set; } = "";

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string EmailAddress { get; set; } = "";

        public string UserType { get; set; } = "User";

        public string FullName =>
            $"{FirstName} {LastName}".Trim();

        public string Initials
        {
            get
            {
                var first = string.IsNullOrWhiteSpace(FirstName)
                    ? ""
                    : FirstName.Substring(0, 1);

                var last = string.IsNullOrWhiteSpace(LastName)
                    ? ""
                    : LastName.Substring(0, 1);

                return $"{first}{last}".ToUpper();
            }
        }
    }
}