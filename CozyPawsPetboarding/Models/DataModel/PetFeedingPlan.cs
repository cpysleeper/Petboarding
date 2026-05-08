using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CozyPawsPetboarding.Models.DataModel
{
    public class PetFeedingPlan
    {
        [Key]
        public Guid PetFeedingPlanId { get; set; }

        public Guid PetId { get; set; }

        public int FeedingsPerDay { get; set; }

        [MaxLength(100)]
        public string AmountPerFeeding { get; set; }

        [MaxLength(150)]
        public string FoodBrand { get; set; }

        [MaxLength(500)]
        public string FeedingInstructions { get; set; }

        public bool HasSpecialDiet { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Pet Pet { get; set; }

        public PetFeedingPlan()
        {
            PetFeedingPlanId = Guid.NewGuid();
        }
    }
}