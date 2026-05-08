using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace CozyPawsPetboarding.Models.DataModel
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<PetOwner> PetOwners { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetMedication> PetMedications { get; set; }
        public DbSet<PetEmergencyContact> PetEmergencyContacts { get; set; }
        public DbSet<PetFeedingPlan> PetFeedingPlans { get; set; }
        public DbSet<ContactUsSubmission> ContactUsSubmissions { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingStatusHistory> BookingStatusHistories { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}