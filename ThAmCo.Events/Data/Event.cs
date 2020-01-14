using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Data
{
    public class EventsViewModel
    {
        public int Id { get; set; }

        [Required, Display(Name = "Event Name")]
        public string Title { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? Duration { get; set; }

        [Required, Display(Name = "Event Type"), MaxLength(3), MinLength(3)]
        public string TypeId { get; set; }

        [Display(Name = "Guest Bookings")]
        public List<GuestBooking> Bookings { get; set; }

        [Display(Name = "Staff Bookings")]
        public List<StaffBooking> StaffBookings { get; set; }

        [Display(Name = "Venue Name")]
        public string VenueCode { get; set; }

    }
}
