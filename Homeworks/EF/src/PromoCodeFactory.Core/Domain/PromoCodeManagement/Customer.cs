using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class Customer
        : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; }

        /// <summary>
        /// One To Many. Customer <-* PromoCodes.
        /// </summary>
        public IList<PromoCode> PromoCodes { get; set; }

        /// <summary>
        /// Many to many. Customers *-* Preferences.
        /// </summary>
        public IList<CustomerPreference> CustomerPreferences { get; set; }
    }
}