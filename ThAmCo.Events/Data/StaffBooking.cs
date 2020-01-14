using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Data
{
    public class StaffBooking
    {
        [Display(Name = "Staff Name")]
        public int StaffId { get; set; }

        public StaffInfo StaffInfo { get; set; }

        [Display(Name = "Event Name")]
        public int EventId { get; set; }

        public EventsViewModel Event { get; set; }
    }
}