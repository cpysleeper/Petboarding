using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CozyPawsPetboarding.Models.DataModel
{
    public class ContactUsSubmission
    {
        [Key]
        public Guid ContactUsSubmissionId { get; set; }

        public Guid? PetOwnerId { get; set; }

        [MaxLength(150)]
        public string FullName { get; set; }

        [MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(200)]
        public string Subject { get; set; }

        public string Message { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        public DateTime SubmittedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }

        public PetOwner PetOwner { get; set; }

        public ContactUsSubmission()
        {
            ContactUsSubmissionId = Guid.NewGuid();
            SubmittedAt = DateTime.UtcNow;
        }
    }
}