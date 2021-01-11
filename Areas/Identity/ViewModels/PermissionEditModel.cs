using System.Collections.Generic;
using ems.Util;

namespace ems.Areas.Identity.ViewModels
{
    public class PermissionEditModel
    {
        public class ResourceAction
        {
            public const string Prefix = "Permissions";
            public ResourceAction(string resourceName)
            {
                Create.Value = $"{Prefix}.{resourceName}.Create";
                View.Value = $"{Prefix}.{resourceName}.View";
                Edit.Value = $"{Prefix}.{resourceName}.Edit";
                Delete.Value = $"{Prefix}.{resourceName}.Delete";
                List.Value = $"{Prefix}.{resourceName}.List";
            }
            public Checkbox Create { get; set; } = new Checkbox();
            public Checkbox View { get; set; } = new Checkbox();
            public Checkbox Edit { get; set; } = new Checkbox();
            public Checkbox Delete { get; set; } = new Checkbox();
            public Checkbox List { get; set; } = new Checkbox();

            public List<Checkbox> ReturnSelectedActions()
            {
                var selectedCheckboxes = new List<Checkbox>();
                if (Create.IsSelected) selectedCheckboxes.Add(Create);
                if (View.IsSelected) selectedCheckboxes.Add(View);
                if (Edit.IsSelected) selectedCheckboxes.Add(Edit);
                if (Delete.IsSelected) selectedCheckboxes.Add(Delete);
                if (List.IsSelected) selectedCheckboxes.Add(List);
                return selectedCheckboxes;
            }
        }

        public ResourceAction Department { get; set; } = new ResourceAction(nameof(Department));
        public ResourceAction Role { get; set; } = new ResourceAction(nameof(Role));
        public ResourceAction Permission { get; set; } = new ResourceAction(nameof(Permission));
        public ResourceAction User { get; set; } = new ResourceAction(nameof(User));
        public ResourceAction Notice { get; set; } = new ResourceAction(nameof(Notice));
        public ResourceAction Leave { get; set; } = new ResourceAction(nameof(Leave));
        public Checkbox Mail { get; set; }


    }
}