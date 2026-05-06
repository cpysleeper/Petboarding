using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppTemplate.Models;
using WebAppTemplate.Models.DataModel;
using System.Data.Entity;

namespace WebAppTemplate.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Bookings
        public ActionResult Index()
        {
            string userEmail = User.Identity.GetUserName();

            var petOwner = db.PetOwners
                .Include(po => po.Pets.Select(p => p.Bookings))
                .FirstOrDefault(po => po.Email == userEmail);

            var viewModel = new BookingListViewModel
            {
                Bookings = new List<BookingItemViewModel>()
            };

            if (petOwner != null && petOwner.Pets != null)
            {
                viewModel.Bookings = petOwner.Pets
                    .SelectMany(p => p.Bookings
                    .Where(b => b.Status != "Cancelled")
                    .Select(b => new BookingItemViewModel
                    {
                        BookingId = b.BookingId,
                        PetName = p.Name,
                        StartTime = b.BookingStartTime,
                        EndTime = b.BookingEndTime,
                        Status = b.Status,

                        UpdatedByEmployeeName = db.BookingStatusHistories
                            .Where(h => h.BookingId == b.BookingId)
                            .OrderByDescending(h => h.StatusAt)
                            .Select(h => h.Employee.FirstName + " " + h.Employee.LastName)
                            .FirstOrDefault(),

                        StatusUpdatedAt = db.BookingStatusHistories
                            .Where(h => h.BookingId == b.BookingId)
                            .OrderByDescending(h => h.StatusAt)
                            .Select(h => (DateTime?)h.StatusAt)
                            .FirstOrDefault()
                     }))
                    .OrderByDescending(b => b.StartTime).ToList();
            }
            return View(viewModel);
        }

        public ActionResult Details(Guid id)
        {
            string userEmail = User.Identity.GetUserName();

            var booking = db.Bookings
                .Include(b => b.Pet)
                .Include(b => b.Pet.PetOwner)
                .FirstOrDefault(b =>
                    b.BookingId == id &&
                    b.Pet.PetOwner.Email == userEmail &&
                    b.Status != "Cancelled");

            if (booking == null)
            {
                return HttpNotFound();
            }

            return View(booking);
        }

        [Authorize]
        public ActionResult RequestCancel(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            string userEmail = User.Identity.GetUserName();

            var booking = db.Bookings
                .Include(b => b.Pet)
                .Include(b => b.Pet.PetOwner)
                .FirstOrDefault(b => b.BookingId == id.Value &&
                                     b.Pet.PetOwner.Email == userEmail);

            if (booking == null)
            {
                return HttpNotFound();
            }

            var vm = new BookingCancelRequestViewModel
            {
                BookingId = booking.BookingId,
                PetName = booking.Pet.Name,
                StartTime = booking.BookingStartTime,
                EndTime = booking.BookingEndTime
            };

            return View(vm);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult RequestCancel(BookingCancelRequestViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.CancellationReason))
            {
                ModelState.AddModelError("CancellationReason", "Cancellation reason is required.");
            }
            else if (vm.CancellationReason.Length > 500)
            {
                ModelState.AddModelError("CancellationReason", "Reason cannot be more than 500 characters.");
            }

            string userEmail = User.Identity.GetUserName();

            var booking = db.Bookings
                .Include(b => b.Pet)
                .Include(b => b.Pet.PetOwner)
                .FirstOrDefault(b => b.BookingId == vm.BookingId &&
                                     b.Pet.PetOwner.Email == userEmail);

            if (booking == null)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                vm.PetName = booking.Pet.Name;
                vm.StartTime = booking.BookingStartTime;
                vm.EndTime = booking.BookingEndTime;
                return View(vm);
            }

            booking.Status = "Cancellation Requested";
            booking.SpecialInstructions = booking.SpecialInstructions + "\nCancellation Reason: " + vm.CancellationReason.Trim();
            booking.UpdatedAt = DateTime.UtcNow;

            db.SaveChanges();

            TempData["SuccessMessage"] = "Cancellation request submitted.";

            return RedirectToAction("Index");
        }

        // Example:
        // /Bookings/Add?PetId=GUID&BookingStartTime=2026-04-10&BookingEndTime=2026-04-12
        public ActionResult Create()
        {
            string userEmail = User.Identity.GetUserName();

            var petOwner = db.PetOwners
                .Include(po => po.Pets)
                .FirstOrDefault(po => po.Email == userEmail);

            var model = new BookingCreateViewModel
            {
                BookingStartTime = DateTime.Now,
                BookingEndTime = DateTime.Now.AddDays(1),
                Pets = petOwner == null
                    ? new List<SelectListItem>()
                    : petOwner.Pets.Select(p => new SelectListItem
                    {
                        Value = p.PetId.ToString(),
                        Text = p.Name
                    }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookingCreateViewModel model)
        {
            string userEmail = User.Identity.GetUserName();

            var petOwner = db.PetOwners
                .Include(po => po.Pets)
                .FirstOrDefault(po => po.Email == userEmail);

            if (petOwner == null)
            {
                return HttpNotFound();
            }

            bool petBelongsToUser = petOwner.Pets.Any(p => p.PetId == model.PetId);

            if (!petBelongsToUser)
            {
                ModelState.AddModelError("PetId", "Invalid pet selected.");
            }

            if (model.BookingStartTime >= model.BookingEndTime)
            {
                ModelState.AddModelError("", "Booking start time must be before booking end time.");
            }

            if (!ModelState.IsValid)
            {
                model.Pets = petOwner.Pets.Select(p => new SelectListItem
                {
                    Value = p.PetId.ToString(),
                    Text = p.Name
                }).ToList();

                return View(model);
            }

            var booking = new Booking
            {
                PetId = model.PetId,
                BookingStartTime = model.BookingStartTime,
                BookingEndTime = model.BookingEndTime,
                CheckInMethod = model.CheckInMethod,
                CheckOutMethod = model.CheckOutMethod,
                SpecialInstructions = model.SpecialInstructions,
                Status = "Pending",
                UpdatedAt = DateTime.UtcNow
            };

            db.Bookings.Add(booking);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // Example:
        // /Bookings/Remove?id=GUID
        public ActionResult Remove(Guid id)
        {
            var booking = db.Bookings.FirstOrDefault(b => b.BookingId == id);

            if (booking == null)
            {
                return Content("Failed to remove booking: booking not found.");
            }

            db.Bookings.Remove(booking);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Content("Failed to remove booking: " + ex.Message);
            }

            return Content("Removed successfully.");
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