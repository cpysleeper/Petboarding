using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using CozyPawsPetboarding.Models;
using CozyPawsPetboarding.Models.DataModel;

namespace CozyPawsPetboarding.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                var vm = new AdminDashboardViewModel
                {
                    Employees = db.Employees
                        .OrderByDescending(e => e.CreatedAt)
                        .Select(e => new EmployeeItemViewModel
                        {
                            EmployeeId = e.EmployeeId,
                            FullName = e.FirstName + " " + e.LastName,
                            Email = e.Email,
                            Role = e.Role,
                            IsActive = e.IsActive,
                            CreatedAt = e.CreatedAt
                        })
                        .ToList()
                };

                return View(vm);
            }
        }

        // GET: Admin/CreateEmployee
        public ActionResult CreateEmployee()
        {
            return View();
        }

        // POST: Admin/CreateEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateEmployee(CreateEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Create employee record
            Employee employee = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Role = "Employee",
                IsActive = true
            };

            try
            {
                using (var db = new ApplicationDbContext())
                {
                    db.Employees.Add(employee);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Failed to create Employee record: {ex}");
                ModelState.AddModelError("", "Failed to create employee record.");
                return View(model);
            }

            // Create identity user for employee
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                // roll back employee creation if desired (left as-is for audit). Inform admin.
                System.Diagnostics.Trace.TraceWarning($"Failed to create identity user for employee {model.Email}: {string.Join(", ", result.Errors)}");
                ModelState.AddModelError("", "Failed to create user account for employee: " + string.Join("; ", result.Errors));
                return View(model);
            }

            try
            {
                // associate employee id with the user
                user.EmployeeId = employee.EmployeeId;
                await userManager.UpdateAsync(user);

                // ensure Employee role exists and assign
                using (var roleContext = new ApplicationDbContext())
                using (var roleStore = new RoleStore<IdentityRole>(roleContext))
                using (var roleManager = new RoleManager<IdentityRole>(roleStore))
                {
                    const string employeeRole = "Employee";
                    if (!roleManager.RoleExists(employeeRole))
                    {
                        var roleCreateResult = roleManager.Create(new IdentityRole(employeeRole));
                        if (!roleCreateResult.Succeeded)
                        {
                            System.Diagnostics.Trace.TraceWarning($"Failed to create role '{employeeRole}': {string.Join(", ", roleCreateResult.Errors)}");
                        }
                    }

                    var addToRoleResult = await userManager.AddToRoleAsync(user.Id, employeeRole);
                    if (!addToRoleResult.Succeeded)
                    {
                        System.Diagnostics.Trace.TraceWarning($"Failed to add user {user.Id} to role '{employeeRole}': {string.Join(", ", addToRoleResult.Errors)}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Failed to link user to employee or assign role: {ex}");
                ModelState.AddModelError("", "Employee created but failed to finalize user association.");
                return View(model);
            }

            return RedirectToAction("CreateEmployeeConfirmation");
        }

        [HttpGet]
        public ActionResult CreateEmployeeConfirmation()
        {
            return View();
        }

        public ActionResult EditEmployee(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            using (var db = new ApplicationDbContext())
            {
                var emp = db.Employees.Find(id.Value);

                if (emp == null)
                {
                    return HttpNotFound();
                }

                var vm = new EditEmployeeViewModel
                {
                    EmployeeId = emp.EmployeeId,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    Email = emp.Email,
                    Role = emp.Role,
                    IsActive = emp.IsActive
                };

                return View(vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployee(EditEmployeeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            using (var db = new ApplicationDbContext())
            {
                var emp = db.Employees.Find(vm.EmployeeId);

                if (emp == null)
                {
                    return HttpNotFound();
                }

                emp.FirstName = vm.FirstName;
                emp.LastName = vm.LastName;
                emp.Email = vm.Email;
                emp.Role = vm.Role;
                emp.IsActive = vm.IsActive;

                db.SaveChanges();
            }

            TempData["SuccessMessage"] = "Employee updated successfully.";

            return RedirectToAction("Index");
        }

        public ActionResult DeleteEmployee(Guid id)
        {
            using (var db = new ApplicationDbContext())
            {
                var emp = db.Employees.Find(id);

                if (emp == null)
                {
                    return HttpNotFound();
                }

                var user = db.Users.FirstOrDefault(u => u.EmployeeId == id);

                if (user != null)
                {
                    user.EmployeeId = null;
                    db.Users.Remove(user);
                }

                db.Employees.Remove(emp);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Employee deleted successfully.";
            }

            return RedirectToAction("Index");
        }
    }
}