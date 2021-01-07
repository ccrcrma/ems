using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace ems.Models
{
    public class MailRequest
    {
        public string[] ToEmails { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
        
    }
}