using System;
using System.Collections.Generic;
using System.Text;

namespace pacc_system_test_one.Models
{
    public class Donation
    {
        public int DonationId { get; set; }

        public int UserId { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public string DonorName { get; set; } = "";

        public string GivingType { get; set; } = "";

        public string PaymentMethod { get; set; } = "";
    }
}
