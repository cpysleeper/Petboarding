using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppTemplate.Models
{
    public class MedicationItemViewModel
    {
        public Guid PetMedicationId { get; set; }
        public Guid PetId { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
    }

    public class MedicationListViewModel
    {
        public Guid PetId { get; set; }
        public List<MedicationItemViewModel> Medications { get; set; }
    }

    public class MedicationEditViewModel
    {
        public Guid PetMedicationId { get; set; }
        public Guid PetId { get; set; }

        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Instructions { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}