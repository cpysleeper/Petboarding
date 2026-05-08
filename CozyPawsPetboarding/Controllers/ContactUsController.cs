using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CozyPawsPetboarding.Models;
using CozyPawsPetboarding.Models.DataModel;
using DbContext = CozyPawsPetboarding.Models.DataModel.ApplicationDbContext;

namespace CozyPawsPetboarding.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly DbContext _db = new DbContext();

        // GET: ContactUs
        public ActionResult Index()
        {
            return View(new ContactUsSubmissionVM());
        }

        // POST: ContactUs
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ContactUsSubmissionVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var submission = new ContactUsSubmission
            {
                FullName = model.FullName,
                Email = model.Email,
                Phone = model.Phone,
                Subject = model.Subject,
                Message = model.Message,
                Status = "New",
                SubmittedAt = DateTime.UtcNow
            };

            _db.ContactUsSubmissions.Add(submission);
            _db.SaveChanges();

            return RedirectToAction("ContactUsSuccessful");
        }

        public ActionResult ContactUsSuccessful()
        {
            return View();
        }
    }
} 