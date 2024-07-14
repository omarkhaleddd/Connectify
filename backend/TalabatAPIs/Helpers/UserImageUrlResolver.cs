using AutoMapper;
using AutoMapper.Execution;
using Talabat.APIs.DTO;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Helpers
{
    public class UserImageUrlResolver : IValueResolver<AppUser, AppUserDto, string>
    {
        private readonly IConfiguration _configuration;

        public UserImageUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(AppUser source, AppUserDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ProfileImageUrl))
                return $"{_configuration["ApiBaseUrl"]}{source.ProfileImageUrl}";
            return string.Empty;

        }
    }
}
