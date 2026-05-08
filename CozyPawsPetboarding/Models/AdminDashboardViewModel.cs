using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CozyPawsPetboarding.Models
{
    public class AdminDashboardViewModel
    {
        public List<EmployeeItemViewModel> Employees { get; set; }
    }

    public class EmployeeItemViewModel
    {
        public Guid EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class EditEmployeeViewModel
    {
        public Guid EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }


}