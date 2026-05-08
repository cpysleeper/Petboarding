using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CozyPawsPetboarding.Models
{
    public class OwnerDashboardViewModel
    {
        public string OwnerName { get; set; }
        public OwnerDashboardBookingViewModel NextBooking { get; set; }
    }

    public class OwnerDashboardBookingViewModel
    {
        public Guid BookingId { get; set; }
        public string PetName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
    }
}