using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    public class EventsView
    {
        public class EventsViewModel
        {
            public int Id { get; set; }

            public string Title { get; set; }

            public DateTime Date { get; set; }

            public TimeSpan? Duration { get; set; }

            public string TypeId { get; set; }

            public List<GuestBooking> Booking { get; set; }

            public List<StaffBooking> StaffBooking { get; set; }

            public string TypeValue { get; set; }

            public string VenueCode { get; set; }
        }
    }
}
