using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppTemplate.Models
{
    public class BookingListViewModel
    {
        public List<BookingItemViewModel> Bookings { get; set; }
    }

    public class BookingItemViewModel
    {
        public Guid BookingId { get; set; }
        public string PetName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public string UpdatedByEmployeeName { get; set; }
        public DateTime? StatusUpdatedAt { get; set; }
    }

    public class BookingCancelRequestViewModel
    {
        public Guid BookingId { get; set; }

        public string PetName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Cancellation reason is required.")]
        [StringLength(500, ErrorMessage = "Reason cannot be more than 500 characters.")]
        public string CancellationReason { get; set; }
    }
}