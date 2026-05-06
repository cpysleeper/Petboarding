using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppTemplate.Models.DataModel
{
    public class PetEmergencyContact
    {
        [Key]
        public Guid PetEmergencyContactId { get; set; }

        public Guid PetId { get; set; }

        [MaxLength(150)]
        public string FullName { get; set; }

        [MaxLength(100)]
        public string RelationshipToOwner { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public Pet Pet { get; set; }

        public PetEmergencyContact()
        {
            PetEmergencyContactId = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
    }
}