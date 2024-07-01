using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Core;
namespace Talabat.Core.Services
{
    public class UploadService : IUploadService
    {
        private readonly string _uploadFolder= "wwwroot/Images/Posts";

        public async Task<string> UploadFileAsync(IFormFile file, string fileName)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine(_uploadFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return fileName;
            }
            return null;
        }

        public async Task<IEnumerable<string>> UploadFilesAsync(IFormFileCollection files)
        {
            var uploadedFileNames = new List<string>();
            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    await UploadFileAsync(file, fileName); // Call the single file upload method
                    uploadedFileNames.Add(fileName);
                }
            }
            return uploadedFileNames;
        }
    }
}
