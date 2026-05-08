using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CozyPawsPetboarding.Models
{
    public class FeedingPlanItemViewModel
    {
        public Guid PetFeedingPlanId { get; set; }
        public Guid PetId { get; set; }
        public int FeedingsPerDay { get; set; }
        public string AmountPerFeeding { get; set; }
        public string FoodBrand { get; set; }
    }

    public class FeedingPlanListViewModel
    {
        public Guid PetId { get; set; }
        public List<FeedingPlanItemViewModel> Plans { get; set; }
    }

    public class FeedingPlanEditViewModel
    {
        public Guid PetFeedingPlanId { get; set; }
        public Guid PetId { get; set; }

        public int FeedingsPerDay { get; set; }
        public string AmountPerFeeding { get; set; }
        public string FoodBrand { get; set; }
        public string FeedingInstructions { get; set; }
        public bool HasSpecialDiet { get; set; }
    }
}