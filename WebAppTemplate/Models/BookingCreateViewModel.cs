using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebAppTemplate.Models
{
    public class BookingCreateViewModel
    {
        [Required]
        public Guid PetId { get; set; }

        [Required]
        public DateTime BookingStartTime { get; set; }

        [Required]
        public DateTime BookingEndTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string CheckInMethod { get; set; }

        [Required]
        [MaxLength(50)]
        public string CheckOutMethod { get; set; }

        public string SpecialInstructions { get; set; }

        public IEnumerable<SelectListItem> Pets { get; set; }
    }
}