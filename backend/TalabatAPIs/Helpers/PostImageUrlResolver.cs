using AutoMapper;
using AutoMapper.Execution;
using Talabat.APIs.DTO;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Core;

namespace Talabat.APIs.Helpers
{
    public class PostImageUrlResolver : IValueResolver<FileNames, FileNameDto, string>
    {
        private readonly IConfiguration _configuration;

        public PostImageUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(FileNames source, FileNameDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.FileName))

                return $"{_configuration["ApiBaseUrl"]}{source.FileName}";
            return string.Empty;

        }

    }
}
