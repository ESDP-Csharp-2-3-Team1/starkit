using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Starkit.Services
{
    public class UploadService
    {
        public async Task Upload(string path, string fileName, IFormFile file)
        {
            using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
            await file.CopyToAsync(stream);
        }
    }
}