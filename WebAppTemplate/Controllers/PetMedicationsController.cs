using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppTemplate.Models;
using WebAppTemplate.Models.DataModel;

namespace WebAppTemplate.Controllers
{
    public class PetMedicationsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(Guid? petId)
        {
            if (petId == null || petId == Guid.Empty)
            {
                return RedirectToAction("Index", "Pets");
            }

            var vm = new MedicationListViewModel
            {
                PetId = petId.Value,
                Medications = db.PetMedications
                    .Where(m => m.PetId == petId.Value)
                    .Select(m => new MedicationItemViewModel
                    {
                        PetMedicationId = m.PetMedicationId,
                        PetId = m.PetId,
                        MedicationName = m.MedicationName,
                        Dosage = m.Dosage,
                        Frequency = m.Frequency
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

            return View(new MedicationEditViewModel { PetId = petId.Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MedicationEditViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.MedicationName))
            {
                ModelState.AddModelError("MedicationName", "Required.");
            }

            if (vm.StartDate == default(DateTime))
            {
                ModelState.AddModelError("StartDate", "Start date is required.");
            }

            if (vm.EndDate < vm.StartDate)
            {
                ModelState.AddModelError("EndDate", "End date must be after start date.");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var med = new PetMedication
            {
                PetMedicationId = Guid.NewGuid(),
                PetId = vm.PetId,
                MedicationName = vm.MedicationName,
                Dosage = vm.Dosage,
                Frequency = vm.Frequency,
                Instructions = vm.Instructions,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            db.PetMedications.Add(med);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Medication added successfully.";

            return RedirectToAction("Index", new { petId = vm.PetId });
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return RedirectToAction("Index", "Pets");
            }

            var med = db.PetMedications.Find(id.Value);

            if (med == null)
            {
                return HttpNotFound();
            }

            var vm = new MedicationEditViewModel
            {
                PetMedicationId = med.PetMedicationId,
                PetId = med.PetId,
                MedicationName = med.MedicationName,
                Dosage = med.Dosage,
                Frequency = med.Frequency,
                Instructions = med.Instructions,
                StartDate = med.StartDate,
                EndDate = med.EndDate
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MedicationEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var med = db.PetMedications.Find(vm.PetMedicationId);

            if (med == null)
            {
                return HttpNotFound();
            }

            med.MedicationName = vm.MedicationName;
            med.Dosage = vm.Dosage;
            med.Frequency = vm.Frequency;
            med.Instructions = vm.Instructions;
            med.StartDate = vm.StartDate;
            med.EndDate = vm.EndDate;

            db.SaveChanges();

            TempData["SuccessMessage"] = "Updated successfully.";

            return RedirectToAction("Index", new { petId = vm.PetId });
        }

        public ActionResult Delete(Guid id)
        {
            var med = db.PetMedications.Find(id);

            if (med == null)
            {
                return HttpNotFound();
            }

            var petId = med.PetId;

            db.PetMedications.Remove(med);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Deleted successfully.";

            return RedirectToAction("Index", new { petId });
        }
    }
}