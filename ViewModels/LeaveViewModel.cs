using System;
using System.ComponentModel.DataAnnotations;
using ems.Models;
using static ems.Models.Leave;

namespace ems.ViewModels
{
    public class LeaveViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "From Date")]
        public DateTime From { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "To Date")]
        public DateTime To { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        public string Description { get; set; }

        public LeaveType Leave { get; set; }

        public Reply Reply;

        public Leave ToModel()
        {
            return new Leave
            {
                From = From,
                To = To,
                Description = Description,
                Type = Leave,
                Id = Id
            };
        }

    }
}