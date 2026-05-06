using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppTemplate.Models.DataModel
{
    public class Employee
    {
        [Key]
        public Guid EmployeeId { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Role { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<BookingStatusHistory> BookingStatusHistories { get; set; }

        public Employee()
        {
            EmployeeId = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
    }
}