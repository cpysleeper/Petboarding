using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppTemplate.Models.DataModel
{
    public class PetMedication
    {
        [Key]
        public Guid PetMedicationId { get; set; }

        public Guid PetId { get; set; }

        [MaxLength(200)]
        public string MedicationName { get; set; }

        [MaxLength(100)]
        public string Dosage { get; set; }

        [MaxLength(100)]
        public string Frequency { get; set; }

        [MaxLength(500)]
        public string Instructions { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public Pet Pet { get; set; }

        public PetMedication()
        {
            PetMedicationId = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
    }
}