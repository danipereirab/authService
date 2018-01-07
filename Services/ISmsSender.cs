using System.Threading.Tasks;

namespace AuthService.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
