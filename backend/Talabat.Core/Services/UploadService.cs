using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities.Core;
namespace Talabat.Core.Services
{
    public class UploadService : IUploadService
    {
        private readonly string _uploadFolder= "wwwroot/Images/Posts";
        private IHostingEnvironment _hostingEnvironment;
        public UploadService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string DeleteFile(string fileName, string folder)
        {
            string path = _hostingEnvironment.WebRootPath + $"\\Images\\{folder}";
            var filePath = Path.Combine(path, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return $"File {fileName} successfully deleted.";
            }
            else
            {
                return $"File {fileName} not found.";
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file,string folder)
        {
            if (file != null && file.Length > 0)
            {
                string path = _hostingEnvironment.WebRootPath + $"\\Images\\{folder}";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(path, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return fileName;
            }
            return null;
        }

        public async Task<IEnumerable<string>> UploadFilesAsync(List<IFormFile>  files , string folder)
        {
            var uploadedFileNames = new List<string>();
            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    // Call the single file upload method
                    uploadedFileNames.Add(await UploadFileAsync(file, folder));
                }
            }
            return uploadedFileNames;
        }

    }
}
