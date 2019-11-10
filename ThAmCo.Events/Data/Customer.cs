using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Data
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required, Display(Name = "Forname")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Email Address"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Guest Bookings")]
        public List<GuestBooking> Bookings { get; set; }
    }
}
