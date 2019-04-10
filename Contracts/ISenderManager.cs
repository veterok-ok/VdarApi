using System.Threading.Tasks;

namespace Contracts
{
    public interface ISenderManager
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendSMSAsync(string phone, string subject, string message);
    }
}
