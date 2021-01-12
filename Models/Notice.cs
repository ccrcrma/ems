using System;
using ems.Areas.Identity.Models;
using ems.ViewModels;

namespace ems.Models
{
    public class Notice
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        internal NoticeViewModel ToViewModel()
        {
            return new NoticeViewModel
            {
                Id = Id,
                Title = Title,
                Description = Description,
                CreatedDate = CreatedDate,
                OwnerName = Owner?.Name
            };
        }
    }
}