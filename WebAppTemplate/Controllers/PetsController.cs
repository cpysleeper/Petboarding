using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebAppTemplate.Models;
using WebAppTemplate.Models.DataModel;

namespace WebAppTemplate.Controllers
{
    public class PetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pets
        [Authorize]
        public ActionResult Index()
        {
            string userEmail = User.Identity.GetUserName();

            var petOwner = db.PetOwners
                .Include(po => po.Pets)
                .FirstOrDefault(po => po.Email == userEmail);

            var viewModel = new PetListViewModel
            {
                Pets = new List<PetItemViewModel>()
            };

            if (petOwner != null && petOwner.Pets != null)
            {
                viewModel.Pets = petOwner.Pets
                    .Select(p => new PetItemViewModel
                    {
                        PetId = p.PetId,
                        Name = p.Name,
                        Species = p.Species,
                        Breed = p.Breed,
                        Age = p.Age,
                        Weight = p.Weight
                    })
                    .ToList();
            }

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View(new PetCreateViewModel());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PetCreateViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Name))
            {
                ModelState.AddModelError("Name", "Pet name is required.");
            }

            if (string.IsNullOrWhiteSpace(vm.Species))
            {
                ModelState.AddModelError("Species", "Species is required.");
            }

            if (string.IsNullOrWhiteSpace(vm.Sex))
            {
                ModelState.AddModelError("Sex", "Sex is required.");
            }

            if (vm.DateOfBirth == default(DateTime))
            {
                ModelState.AddModelError("DateOfBirth", "Date of birth is required.");
            }
            else if (vm.DateOfBirth < new DateTime(1900, 1, 1))
            {
                ModelState.AddModelError("DateOfBirth", "Date of birth is not realistic.");
            }
            else if (vm.DateOfBirth > DateTime.Today)
            {
                ModelState.AddModelError("DateOfBirth", "Date of birth cannot be in the future.");
            }

            if (vm.Age < 0 || vm.Age > 100)
            {
                ModelState.AddModelError("Age", "Age must be between 0 and 100.");
            }

            if (vm.Weight <= 0 || vm.Weight > 200)
            {
                ModelState.AddModelError("Weight", "Weight must be greater than 0 and less than 200.");
            }

            if (!string.IsNullOrWhiteSpace(vm.Name) && vm.Name.Length > 100)
            {
                ModelState.AddModelError("Name", "Pet name cannot be more than 100 characters.");
            }

            if (!string.IsNullOrWhiteSpace(vm.Breed) && vm.Breed.Length > 100)
            {
                ModelState.AddModelError("Breed", "Breed cannot be more than 100 characters.");
            }

            if (!string.IsNullOrWhiteSpace(vm.Color) && vm.Color.Length > 50)
            {
                ModelState.AddModelError("Color", "Color cannot be more than 50 characters.");
            }

            if (!string.IsNullOrWhiteSpace(vm.Notes) && vm.Notes.Length > 500)
            {
                ModelState.AddModelError("Notes", "Notes cannot be more than 500 characters.");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            string userEmail = User.Identity.GetUserName();

            var petOwner = db.PetOwners.FirstOrDefault(po => po.Email == userEmail);

            if (petOwner == null)
            {
                ModelState.AddModelError("", "No pet owner profile found for this user.");
                return View(vm);
            }

            var pet = new Pet
            {
                PetOwnerId = petOwner.PetOwnerId,
                Name = vm.Name.Trim(),
                Species = vm.Species.Trim(),
                Breed = string.IsNullOrWhiteSpace(vm.Breed) ? null : vm.Breed.Trim(),
                DateOfBirth = vm.DateOfBirth,
                Age = vm.Age,
                Sex = vm.Sex.Trim(),
                Weight = vm.Weight,
                Color = string.IsNullOrWhiteSpace(vm.Color) ? null : vm.Color.Trim(),
                Notes = string.IsNullOrWhiteSpace(vm.Notes) ? null : vm.Notes.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            db.Pets.Add(pet);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return RedirectToAction("Index", "Pets");
            }

            var pet = db.Pets
                .Include(p => p.PetOwner)
                .Include(p => p.EmergencyContacts)
                .Include(p => p.FeedingPlans)
                .Include(p => p.Medications)
                .FirstOrDefault(p => p.PetId == id.Value);

            if (pet == null)
            {
                return HttpNotFound();
            }

            var viewModel = new PetDetailsViewModel
            {
                PetId = pet.PetId,
                Name = pet.Name,
                Species = pet.Species,
                Breed = pet.Breed,
                Age = pet.Age,
                Sex = pet.Sex,
                Weight = pet.Weight,
                Color = pet.Color,
                Notes = pet.Notes,

                OwnerName = pet.PetOwner == null ? "" : pet.PetOwner.FirstName,
                OwnerEmail = pet.PetOwner == null ? "" : pet.PetOwner.Email,
                OwnerPhone = pet.PetOwner == null ? "" : pet.PetOwner.Phone,

                EmergencyContactCount = pet.EmergencyContacts == null ? 0 : pet.EmergencyContacts.Count,
                FeedingPlanCount = pet.FeedingPlans == null ? 0 : pet.FeedingPlans.Count,
                MedicationCount = pet.Medications == null ? 0 : pet.Medications.Count
            };

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Remove(Guid id)
        {
            var pet = db.Pets
                .Include(p => p.Bookings)
                .Include(p => p.Medications)
                .Include(p => p.EmergencyContacts)
                .Include(p => p.FeedingPlans)
                .FirstOrDefault(p => p.PetId == id);

            if (pet == null)
            {
                return RedirectToAction("Index");
            }

            if (pet.Bookings != null && pet.Bookings.Any())
            {
                db.Bookings.RemoveRange(pet.Bookings);
            }

            if (pet.Medications != null && pet.Medications.Any())
            {
                db.PetMedications.RemoveRange(pet.Medications);
            }

            if (pet.EmergencyContacts != null && pet.EmergencyContacts.Any())
            {
                db.PetEmergencyContacts.RemoveRange(pet.EmergencyContacts);
            }

            if (pet.FeedingPlans != null && pet.FeedingPlans.Any())
            {
                db.PetFeedingPlans.RemoveRange(pet.FeedingPlans);
            }

            db.Pets.Remove(pet);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Pet and related information deleted successfully.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}