using System;
using System.Linq;
using System.Web.Mvc;
using WebAppTemplate.Models;
using WebAppTemplate.Models.DataModel;

namespace WebAppTemplate.Controllers
{
    [Authorize]
    public class PetEmergencyContactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(Guid? petId)
        {
            if (petId == null || petId == Guid.Empty)
            {
                return RedirectToAction("Index", "Pets");
            }

            bool petExists = db.Pets.Any(p => p.PetId == petId.Value);

            if (!petExists)
            {
                return HttpNotFound();
            }

            var vm = new EmergencyContactListViewModel
            {
                PetId = petId.Value,
                Contacts = db.PetEmergencyContacts
                    .Where(c => c.PetId == petId.Value)
                    .Select(c => new EmergencyContactItemViewModel
                    {
                        PetEmergencyContactId = c.PetEmergencyContactId,
                        PetId = c.PetId,
                        FullName = c.FullName,
                        RelationshipToOwner = c.RelationshipToOwner,
                        Phone = c.Phone,
                        Email = c.Email
                    })
                    .ToList()
            };

            return View(vm);
        }

        public ActionResult Create(Guid? petId)
        {
            if (petId == null || petId == Guid.Empty)
            {
                return RedirectToAction("Index", "Pets");
            }

            var vm = new EmergencyContactEditViewModel
            {
                PetId = petId.Value
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmergencyContactEditViewModel vm)
        {
            if (vm.PetId == Guid.Empty)
            {
                ModelState.AddModelError("", "Invalid pet.");
            }

            if (string.IsNullOrWhiteSpace(vm.FullName))
            {
                ModelState.AddModelError("FullName", "Full name is required.");
            }
            else if (vm.FullName.Length > 150)
            {
                ModelState.AddModelError("FullName", "Full name cannot be more than 150 characters.");
            }

            if (string.IsNullOrWhiteSpace(vm.Phone))
            {
                ModelState.AddModelError("Phone", "Phone number is required.");
            }
            else if (vm.Phone.Length > 20)
            {
                ModelState.AddModelError("Phone", "Phone number cannot be more than 20 characters.");
            }

            if (!string.IsNullOrWhiteSpace(vm.Email))
            {
                if (vm.Email.Length > 200)
                {
                    ModelState.AddModelError("Email", "Email cannot be more than 200 characters.");
                }
                else if (!System.Text.RegularExpressions.Regex.IsMatch(vm.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    ModelState.AddModelError("Email", "Invalid email format.");
                }
            }

            if (!string.IsNullOrWhiteSpace(vm.RelationshipToOwner) && vm.RelationshipToOwner.Length > 100)
            {
                ModelState.AddModelError("RelationshipToOwner", "Relationship cannot be more than 100 characters.");
            }

            if (!string.IsNullOrWhiteSpace(vm.Notes) && vm.Notes.Length > 500)
            {
                ModelState.AddModelError("Notes", "Notes cannot be more than 500 characters.");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var contact = new PetEmergencyContact
            {
                PetEmergencyContactId = Guid.NewGuid(),
                PetId = vm.PetId,
                FullName = vm.FullName.Trim(),
                RelationshipToOwner = string.IsNullOrWhiteSpace(vm.RelationshipToOwner) ? null : vm.RelationshipToOwner.Trim(),
                Phone = vm.Phone.Trim(),
                Email = string.IsNullOrWhiteSpace(vm.Email) ? null : vm.Email.Trim(),
                Notes = string.IsNullOrWhiteSpace(vm.Notes) ? null : vm.Notes.Trim()
            };

            db.PetEmergencyContacts.Add(contact);
            db.SaveChanges();

            return RedirectToAction("Index", new { petId = vm.PetId });
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return RedirectToAction("Index", "Pets");
            }

            var contact = db.PetEmergencyContacts.Find(id.Value);

            if (contact == null)
            {
                return HttpNotFound();
            }

            var vm = new EmergencyContactEditViewModel
            {
                PetEmergencyContactId = contact.PetEmergencyContactId,
                PetId = contact.PetId,
                FullName = contact.FullName,
                RelationshipToOwner = contact.RelationshipToOwner,
                Phone = contact.Phone,
                Email = contact.Email,
                Notes = contact.Notes
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmergencyContactEditViewModel vm)
        {
            if (vm.PetEmergencyContactId == Guid.Empty)
            {
                ModelState.AddModelError("", "Invalid emergency contact.");
            }

            if (vm.PetId == Guid.Empty)
            {
                ModelState.AddModelError("", "Invalid pet.");
            }

            if (string.IsNullOrWhiteSpace(vm.FullName))
            {
                ModelState.AddModelError("FullName", "Full name is required.");
            }
            else if (vm.FullName.Length > 150)
            {
                ModelState.AddModelError("FullName", "Full name cannot be more than 150 characters.");
            }

            if (!string.IsNullOrWhiteSpace(vm.RelationshipToOwner) && vm.RelationshipToOwner.Length > 100)
            {
                ModelState.AddModelError("RelationshipToOwner", "Relationship cannot be more than 100 characters.");
            }

            if (string.IsNullOrWhiteSpace(vm.Phone))
            {
                ModelState.AddModelError("Phone", "Phone number is required.");
            }
            else if (vm.Phone.Length > 20)
            {
                ModelState.AddModelError("Phone", "Phone number cannot be more than 20 characters.");
            }

            if (!string.IsNullOrWhiteSpace(vm.Email))
            {
                if (vm.Email.Length > 200)
                {
                    ModelState.AddModelError("Email", "Email cannot be more than 200 characters.");
                }
                else if (!System.Text.RegularExpressions.Regex.IsMatch(vm.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    ModelState.AddModelError("Email", "Please enter a valid email address.");
                }
            }

            if (!string.IsNullOrWhiteSpace(vm.Notes) && vm.Notes.Length > 500)
            {
                ModelState.AddModelError("Notes", "Notes cannot be more than 500 characters.");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var contact = db.PetEmergencyContacts.Find(vm.PetEmergencyContactId);

            if (contact == null)
            {
                return HttpNotFound();
            }

            contact.FullName = vm.FullName.Trim();
            contact.RelationshipToOwner = string.IsNullOrWhiteSpace(vm.RelationshipToOwner) ? null : vm.RelationshipToOwner.Trim();
            contact.Phone = vm.Phone.Trim();
            contact.Email = string.IsNullOrWhiteSpace(vm.Email) ? null : vm.Email.Trim();
            contact.Notes = string.IsNullOrWhiteSpace(vm.Notes) ? null : vm.Notes.Trim();

            db.SaveChanges();

            return RedirectToAction("Index", new { petId = vm.PetId });
        }

        public ActionResult Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return HttpNotFound();
            }

            var contact = db.PetEmergencyContacts.Find(id);

            if (contact == null)
            {
                return HttpNotFound();
            }

            Guid petId = contact.PetId;

            db.PetEmergencyContacts.Remove(contact);
            db.SaveChanges();

            return RedirectToAction("Index", new { petId = petId });
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