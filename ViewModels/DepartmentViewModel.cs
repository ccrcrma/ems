using System.ComponentModel.DataAnnotations;
using ems.Models;

namespace ems.ViewModels
{
    public class DepartmentViewModel
    {
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "{0} must be between {1} and {2} characters")]

        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        public string Description { get; set; }

        public int Id { get; set; }



        public Department ToModel()
        {
            return new Department
            {
                Name = Name,
                Description = Description,
                Id = Id
            };
        }

    }
}