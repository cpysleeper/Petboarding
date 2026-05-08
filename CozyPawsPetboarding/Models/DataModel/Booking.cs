using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CozyPawsPetboarding.Models.DataModel
{
    public class Booking
    {
        [Key]
        public Guid BookingId { get; set; }

        public Guid PetId { get; set; }

        public DateTime BookingStartTime { get; set; }
        public DateTime BookingEndTime { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        [MaxLength(50)]
        public string CheckInMethod { get; set; }

        [MaxLength(50)]
        public string CheckOutMethod { get; set; }

        public DateTime? CanceledAt { get; set; }

        [MaxLength(300)]
        public string CancellationReason { get; set; }

        public string SpecialInstructions { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Pet Pet { get; set; }

        public List<BookingStatusHistory> StatusHistory { get; set; }

        public Booking()
        {
            BookingId = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
    }
}