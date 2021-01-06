using System;

namespace ems.Models
{
    public class Notice
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        
    }
}