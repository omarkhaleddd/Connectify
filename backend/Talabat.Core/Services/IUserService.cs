using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Services
{
    public interface IUserService
    {
        Task<AppUser> GetUserAsync(string userId);
        Task<bool> IsUserBlockedAsync(string userId, string authorId);
        Task<List<string>> GetFriendsListAsync(string userId);
    }
}
