using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Data
{
    public class StaffInfo
    {
        public int Id { get; set; }

        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required, DataType(DataType.EmailAddress), Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Staff Bookings")]
        public List<StaffBooking> staffBookings{get; set;}

        public string fullName() {

            return FirstName + Surname;
        }

    }
}
