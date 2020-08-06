using System.Threading.Tasks;

namespace Starkit.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
    }
}