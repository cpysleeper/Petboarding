using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CozyPawsPetboarding.Models
{
    public class EmergencyContactListViewModel
    {
        public Guid PetId { get; set; }
        public List<EmergencyContactItemViewModel> Contacts { get; set; }
    }

    public class EmergencyContactItemViewModel
    {
        public Guid PetEmergencyContactId { get; set; }
        public Guid PetId { get; set; }
        public string FullName { get; set; }
        public string RelationshipToOwner { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public class EmergencyContactEditViewModel
    {
        public Guid PetEmergencyContactId { get; set; }
        public Guid PetId { get; set; }
        public string FullName { get; set; }
        public string RelationshipToOwner { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
    }
}