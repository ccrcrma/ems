using System;
using System.ComponentModel.DataAnnotations;
using ems.Areas.Identity.Models;
using ems.ViewModels;

namespace ems.Models
{
    public class Leave
    {
        public enum LeaveType
        {
            [Display(Name = "Annual Leave")]
            AnnualLeave = 1,
            [Display(Name = "Sick Leave")]
            SickLeave,
            [Display(Name = "Parental Leave")]
            ParentalLeave,

            [Display(Name = "Other Leave")]
            OtherLeave
        }
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public LeaveType Type { get; set; }
        public string Description { get; set; }

        public string OwnerId { get; set; }

        public ApplicationUser Owner { get; set; }
        internal LeaveViewModel ToViewModel()
        {
            return new LeaveViewModel
            {
                Id = Id,
                From = From,
                To = To,
                Description = Description,
                Leave = Type,
                Reply = Reply,
                OwnerId = Owner?.Id,
                OwnerName = Owner?.Name
            };
        }

        public Reply Reply { get; set; }

    }
}