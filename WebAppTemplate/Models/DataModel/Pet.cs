using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace WebAppTemplate.Models.DataModel
{
    public class Pet
    {
        [Key]
        public Guid PetId { get; set; }

        public Guid PetOwnerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Species { get; set; }

        [MaxLength(100)]
        public string Breed { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int Age { get; set; }

        [MaxLength(20)]
        public string Sex { get; set; }

        public decimal Weight { get; set; }

        [MaxLength(50)]
        public string Color { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public PetOwner PetOwner { get; set; }

        public List<PetMedication> Medications { get; set; }
        public List<PetEmergencyContact> EmergencyContacts { get; set; }
        public List<PetFeedingPlan> FeedingPlans { get; set; }
        public List<Booking> Bookings { get; set; }

        public Pet()
        {
            PetId = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
    }
}