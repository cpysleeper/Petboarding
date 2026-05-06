using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Configuration;
using WebAppTemplate.Models;
using WebAppTemplate.Models.DataModel;

[assembly: OwinStartupAttribute(typeof(WebAppTemplate.Startup))]
namespace WebAppTemplate
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            // Ensure roles and initial admin user exist
            CreateRolesAndAdminUser(app);
        }

        private void CreateRolesAndAdminUser(IAppBuilder app)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var roleStore = new RoleStore<IdentityRole>(context);
                    var roleManager = new RoleManager<IdentityRole>(roleStore);
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);

                    // Ensure required roles
                    string[] roles = new[] { "Admin", "Employee", "PetOwner" };
                    foreach (var role in roles)
                    {
                        if (!roleManager.RoleExists(role))
                        {
                            roleManager.Create(new IdentityRole(role));
                        }
                    }

                    // Create initial admin user if configured
                    var adminEmail = ConfigurationManager.AppSettings["AdminUserEmail"];
                    var adminPassword = ConfigurationManager.AppSettings["AdminUserPassword"];
                    if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
                    {
                        var adminUser = userManager.FindByName(adminEmail);
                        if (adminUser == null)
                        {
                            adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                            var createResult = userManager.Create(adminUser, adminPassword);
                            if (createResult.Succeeded)
                            {
                                userManager.AddToRole(adminUser.Id, "Admin");
                            }
                        }
                        else
                        {
                            if (!userManager.IsInRole(adminUser.Id, "Admin"))
                            {
                                userManager.AddToRole(adminUser.Id, "Admin");
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"CreateRolesAndAdminUser failed: {ex}");
            }
        }
    }
}
