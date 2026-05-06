using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppTemplate.Models
{
    public class PetListViewModel
    {
        public List<PetItemViewModel> Pets { get; set; }
    }

    public class PetItemViewModel
    {
        public Guid PetId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public decimal Weight { get; set; }
    }
    public class PetDetailsViewModel
    {
        public Guid PetId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public decimal Weight { get; set; }
        public string Color { get; set; }
        public string Notes { get; set; }

        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }
        public string OwnerPhone { get; set; }

        public int EmergencyContactCount { get; set; }
        public int FeedingPlanCount { get; set; }
        public int MedicationCount { get; set; }
    }
}