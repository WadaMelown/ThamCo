using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Data
{
    public class Event
    {
        public int Id { get; set; }

        [Required, Display(Name = "Event Name")]
        public string Title { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? Duration { get; set; }

        [Required, Display(Name = "Event Type")]
        public string TypeId { get; set; }

        [Display(Name = "Guest Bookings")]
        public List<GuestBooking> Bookings { get; set; }

        [Display(Name = "Staff Bookings")]
        public List<StaffBooking> StaffBookings { get; set; }

        [Display(Name = "Venue Name")]
        public string VenueCode { get; set; }

        public string id { get; set; }

        public string title { get; set; }
    }
}
