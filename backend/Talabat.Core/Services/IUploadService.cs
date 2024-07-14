using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Core;

namespace Talabat.Core.Services
{
    public interface IUploadService
    {
        public Task<string> UploadFileAsync(IFormFile file, string fileName , string folder);
        public Task<IEnumerable<string>> UploadFilesAsync(List<IFormFile> files, string folder);

    }
}
