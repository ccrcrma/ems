using System.Threading.Tasks;
using ems.Models;

namespace ems.Services
{
    public interface IMailService
    {
        Task SendMailAsync(MailRequest mailRequest);
    }
}