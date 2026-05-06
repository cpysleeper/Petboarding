using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebAppTemplate.Models;
using WebAppTemplate.Models.DataModel;

namespace WebAppTemplate.Controllers
{
    [Authorize(Roles = "PetOwner")]
    public class OwnerDashboardController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            string userEmail = User.Identity.GetUserName();

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            var petOwner = db.PetOwners
                .Include(po => po.Pets.Select(p => p.Bookings))
                .FirstOrDefault(po => po.Email == userEmail);

            var viewModel = new OwnerDashboardViewModel
            {
                OwnerName = "Pet Owner",
                NextBooking = null
            };

            if (petOwner != null)
            {
                viewModel.OwnerName = petOwner.FirstName;

                viewModel.NextBooking = petOwner.Pets
                    .SelectMany(p => p.Bookings.Select(b => new OwnerDashboardBookingViewModel
                    {
                        BookingId = b.BookingId,
                        PetName = p.Name,
                        StartTime = b.BookingStartTime,
                        EndTime = b.BookingEndTime,
                        Status = b.Status
                    }))
                    .Where(b => b.StartTime >= DateTime.Now)
                    .OrderBy(b => b.StartTime)
                    .FirstOrDefault();
            }
            return View(viewModel);
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