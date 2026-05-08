using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CozyPawsPetboarding.Models.DataModel
{
    public class BookingStatusHistory
    {
        [Key]
        public Guid BookingStatusHistoryId { get; set; }

        public Guid BookingId { get; set; }
        public Guid EmployeeId { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        public DateTime StatusAt { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public Booking Booking { get; set; }
        public Employee Employee { get; set; }

        public BookingStatusHistory()
        {
            BookingStatusHistoryId = Guid.NewGuid();
        }
    }
}