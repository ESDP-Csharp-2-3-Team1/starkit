using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Starkit.Services
{
    public class CreateFile
    {
        public async Task FileCreate(string fileName, string filePath, IFormFile file)
        {
            var stream = new FileStream(Path.Combine(filePath,fileName),FileMode.Create);
            await file.CopyToAsync(stream);
        }
    }
}