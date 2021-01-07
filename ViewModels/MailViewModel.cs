using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ems.ViewModels
{
    public class MailViewModel
    {
        public enum ReceiverType
        {
            All = 1,
            Staff,
            Department
        }
        [Required]
        public string Body { get; set; }

        [Required]
        public string Subject { get; set; }
        public List<IFormFile> Files { get; set; }

        [Display(Name = "Select")]
        [Required]
        public ReceiverType Receiver { get; set; }

        public int? ResourceId { get; set; }
        public List<SelectListItem> Departments { get; set; }

        public List<SelectListItem> Users { get; set; }



    }
}