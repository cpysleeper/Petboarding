using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebAppTemplate.Models.DataModel;

namespace WebAppTemplate.Controllers
{
    [Authorize]
    public class MyAccountController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            string userEmail = User.Identity.GetUserName();

            var petOwner = db.PetOwners
                .FirstOrDefault(p => p.Email == userEmail);

            return View(petOwner);
        }
    }
}