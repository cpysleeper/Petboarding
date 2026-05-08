using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CozyPawsPetboarding.Models;
using CozyPawsPetboarding.Models.DataModel;

namespace CozyPawsPetboarding.Controllers
{
    public class PetFeedingPlansController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(Guid? petId)
        {
            if (petId == null || petId == Guid.Empty)
            {
                return RedirectToAction("Index", "Pets");
            }

            var vm = new FeedingPlanListViewModel
            {
                PetId = petId.Value,
                Plans = db.PetFeedingPlans
                    .Where(p => p.PetId == petId.Value)
                    .Select(p => new FeedingPlanItemViewModel
                    {
                        PetFeedingPlanId = p.PetFeedingPlanId,
                        PetId = p.PetId,
                        FeedingsPerDay = p.FeedingsPerDay,
                        AmountPerFeeding = p.AmountPerFeeding,
                        FoodBrand = p.FoodBrand
                    }).ToList()
            };

            return View(vm);
        }

        public ActionResult Create(Guid? petId)
        {
            if (petId == null || petId == Guid.Empty)
            {
                return RedirectToAction("Index", "Pets");
            }

            return View(new FeedingPlanEditViewModel { PetId = petId.Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FeedingPlanEditViewModel vm)
        {
            if (vm.FeedingsPerDay <= 0)
            {
                ModelState.AddModelError("FeedingsPerDay", "Must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(vm.AmountPerFeeding))
            {
                ModelState.AddModelError("AmountPerFeeding", "Required.");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var plan = new PetFeedingPlan
            {
                PetFeedingPlanId = Guid.NewGuid(),
                PetId = vm.PetId,
                FeedingsPerDay = vm.FeedingsPerDay,
                AmountPerFeeding = vm.AmountPerFeeding,
                FoodBrand = vm.FoodBrand,
                FeedingInstructions = vm.FeedingInstructions,
                HasSpecialDiet = vm.HasSpecialDiet,
                UpdatedAt = DateTime.UtcNow
            };

            db.PetFeedingPlans.Add(plan);
            db.SaveChanges();

            return RedirectToAction("Index", new { petId = vm.PetId });
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return RedirectToAction("Index", "Pets");
            }

            var plan = db.PetFeedingPlans.Find(id.Value);

            if (plan == null)
            {
                return HttpNotFound();
            }

            var vm = new FeedingPlanEditViewModel
            {
                PetFeedingPlanId = plan.PetFeedingPlanId,
                PetId = plan.PetId,
                FeedingsPerDay = plan.FeedingsPerDay,
                AmountPerFeeding = plan.AmountPerFeeding,
                FoodBrand = plan.FoodBrand,
                FeedingInstructions = plan.FeedingInstructions,
                HasSpecialDiet = plan.HasSpecialDiet
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FeedingPlanEditViewModel vm)
        {
            if (vm.FeedingsPerDay <= 0)
            {
                ModelState.AddModelError("FeedingsPerDay", "Must be greater than 0.");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var plan = db.PetFeedingPlans.Find(vm.PetFeedingPlanId);

            if (plan == null)
            {
                return HttpNotFound();
            }

            plan.FeedingsPerDay = vm.FeedingsPerDay;
            plan.AmountPerFeeding = vm.AmountPerFeeding;
            plan.FoodBrand = vm.FoodBrand;
            plan.FeedingInstructions = vm.FeedingInstructions;
            plan.HasSpecialDiet = vm.HasSpecialDiet;
            plan.UpdatedAt = DateTime.UtcNow;

            db.SaveChanges();

            return RedirectToAction("Index", new { petId = vm.PetId });
        }

        public ActionResult Delete(Guid id)
        {
            var plan = db.PetFeedingPlans.Find(id);

            if (plan == null)
            {
                return HttpNotFound();
            }

            var petId = plan.PetId;

            db.PetFeedingPlans.Remove(plan);
            db.SaveChanges();

            return RedirectToAction("Index", new { petId });
        }
    }
}