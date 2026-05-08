using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CozyPawsPetboarding.Models
{
    public class EmployeeDashboardViewModel
    {
        public List<EmployeeBookingItemViewModel> Bookings { get; set; }
        public List<ContactUsItemViewModel> ContactMessages { get; set; }
    }

    public class EmployeeBookingItemViewModel
    {
        public Guid BookingId { get; set; }
        public string PetName { get; set; }
        public string OwnerEmail { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
    }

    public class UpdateBookingStatusViewModel
    {
        public Guid BookingId { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }

    public class ContactUsItemViewModel
    {
        public Guid ContactUsSubmissionId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}