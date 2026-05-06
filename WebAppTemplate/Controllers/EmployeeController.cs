using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebAppTemplate.Models;
using WebAppTemplate.Models.DataModel;

namespace WebAppTemplate.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var vm = new EmployeeDashboardViewModel
            {
                Bookings = db.Bookings
                    .Include(b => b.Pet)
                    .Include(b => b.Pet.PetOwner)
                    .OrderByDescending(b => b.BookingStartTime)
                    .Select(b => new EmployeeBookingItemViewModel
                    {
                        BookingId = b.BookingId,
                        PetName = b.Pet.Name,
                        OwnerEmail = b.Pet.PetOwner.Email,
                        StartTime = b.BookingStartTime,
                        EndTime = b.BookingEndTime,
                        Status = b.Status
                    })
                    .ToList(),

                ContactMessages = db.ContactUsSubmissions
                    .OrderByDescending(c => c.SubmittedAt)
                    .Select(c => new ContactUsItemViewModel
                    {
                        ContactUsSubmissionId = c.ContactUsSubmissionId,
                        FullName = c.FullName,
                        Email = c.Email,
                        Subject = c.Subject,
                        Status = c.Status,
                        SubmittedAt = c.SubmittedAt
                    })
                    .ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateBookingStatus(UpdateBookingStatusViewModel vm)
        {
            if (vm.BookingId == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(vm.Status))
            {
                TempData["ErrorMessage"] = "Status is required.";
                return RedirectToAction("Index");
            }

            var booking = db.Bookings.Find(vm.BookingId);

            if (booking == null)
            {
                return HttpNotFound();
            }

            string email = User.Identity.GetUserName();

            var employee = db.Employees.FirstOrDefault(e => e.Email == email);

            if (employee == null)
            {
                return HttpNotFound();
            }

            booking.Status = vm.Status;
            booking.UpdatedAt = DateTime.UtcNow;

            var history = new BookingStatusHistory
            {
                BookingId = booking.BookingId,
                EmployeeId = employee.EmployeeId,
                Status = vm.Status,
                StatusAt = DateTime.UtcNow,
                Notes = vm.Notes
            };

            db.BookingStatusHistories.Add(history);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Booking status updated successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkContactResolved(Guid id)
        {
            var message = db.ContactUsSubmissions.Find(id);

            if (message == null)
            {
                return HttpNotFound();
            }

            message.Status = "Resolved";
            message.ResolvedAt = DateTime.UtcNow;

            db.SaveChanges();

            TempData["SuccessMessage"] = "Contact message marked as resolved.";
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