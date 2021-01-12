using System;
using System.ComponentModel.DataAnnotations;
using ems.Models;

namespace ems.ViewModels
{
    public class NoticeViewModel
    {
        public int Id { get; set; }

        public DateTime? CreatedDate { get; set; }


        [Required(ErrorMessage = "{0} is Required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        public string Description { get; set; }

        public string OwnerName { get; set; }
        
        

        internal Notice ToModel()
        {
            return new Notice
            {
                Id = Id,
                CreatedDate = CreatedDate,
                Title = Title,
                Description = Description
            };
        }
    }
}