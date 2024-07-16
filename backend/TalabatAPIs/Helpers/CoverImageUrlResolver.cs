using AutoMapper;
using AutoMapper.Execution;
using Talabat.APIs.DTO;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Helpers
{
    public class CoverImageUrlResolver : IValueResolver<AppUser, AppUserDto, string?>
    {
        private readonly IConfiguration _configuration;

        public CoverImageUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(AppUser source, AppUserDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.CoverImageUrl))
                return $"{_configuration["ApiBaseUrl"]}\\Images\\Users\\{source.CoverImageUrl}";
            return string.Empty;

        }
    }
}
