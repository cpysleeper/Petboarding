using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebAppTemplate.Models.DataModel
{
    public class PetOwner
    {
        [Key]
        public Guid PetOwnerId { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(50)]
        public string State { get; set; }

        [MaxLength(20)]
        public string PostalCode { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        public List<Pet> Pets { get; set; }
        public List<ContactUsSubmission> ContactUsSubmissions { get; set; }

        public PetOwner()
        {
            PetOwnerId = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
    }
}