using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Starkit.Models;

namespace Starkit.Services
{
    public interface IRecaptchaService
    {
        Task<RecaptchaResponse> Validate(IFormCollection form);
    }
}