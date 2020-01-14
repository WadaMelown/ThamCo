using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Data
{
    public class GuestBooking
    {
        [Display(Name = "Customer Name")]
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        [Display(Name = "Event Name")]
        public int EventId { get; set; }

        public EventsViewModel Event { get; set; }

        public bool Attended { get; set; }
    }
}
