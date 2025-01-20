using System.Collections.Generic;
using System;

namespace PromoCodeFactory.WebHost.Models
{
    public class UpdateEmployeeRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int AppliedPromocodesCount { get; set; }
    }
}
