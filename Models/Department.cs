using ems.ViewModels;

namespace ems.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DepartmentViewModel ToViewModel()
        {
            return new DepartmentViewModel
            {
                Id = Id,
                Name = Name,
                Description = Description
            };
        }

    }
}