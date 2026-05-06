using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppTemplate.Models
{
    public class PetCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Species { get; set; }
        public string Breed { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public int Age { get; set; }
        public string Sex { get; set; }
        public decimal Weight { get; set; }
        public string Color { get; set; }
        public string Notes { get; set; }
    }
}