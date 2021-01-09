using System.ComponentModel.DataAnnotations;

namespace ems.Areas.Identity.Types
{
    public enum Designation
    {
        [Display(Name = "Software Engineer")]
        SoftwareEngineer = 1,
        Supervisor,
        Receptionist
    }
}