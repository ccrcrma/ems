using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ems.Areas.Identity.Models;
using ems.Util;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ems.Areas.Identity.ViewModels
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(20, ErrorMessage = "{0} must be between {2} and {1}", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(300, ErrorMessage = "{0} must be between {2} and {1}", MinimumLength = 5)]
        public string Description { get; set; }

        public string Id { get; set; }
        public List<Checkbox> Users { get; set; } = new List<Checkbox>();

        public void CreateCheckBoxes(List<SelectListItem> allCheckBoxes)
        {
            foreach (var role in allCheckBoxes)
            {
                Users.Add(new Checkbox
                {
                    Text = role.Text,
                    Value = role.Value,
                    Selected = false
                });
            }
        }

        public void SetCheckBoxes(string[] selectedCheckBoxes)
        {
            foreach (var user in Users)
            {
                if (selectedCheckBoxes.Contains(user.Value))
                {
                    user.Selected = true;
                    continue;
                }
            }
        }
        public ApplicationRole ToModel(ApplicationRole role = null)
        {
            if (role == null)
                role = new ApplicationRole();

            role.Name = Name;
            role.NormalizedName = Name.ToUpper();
            role.Description = Description;
            return role;
        }
    }
}