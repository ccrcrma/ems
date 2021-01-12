using System;

namespace ems.ViewModels
{
    public class IndexViewModel
    {
        public int DepartmentCount { get; set; }
        public int LeaveCount { get; set; }
        public int NoticeCount { get; set; }
        public int UserCount { get; set; }

        public class PersonalDetail
        {
            public string Email { get; set; }
            public string Address { get; set; }
            public string MobileNumber { get; set; }
            public string Designation { get; set; }
            public DateTime StartDate { get; set; }
            public string Department { get; set; }
            public string[] Roles { get; set; }
            
        }
        public PersonalDetail IndividualInfo { get; set; }
    }
}