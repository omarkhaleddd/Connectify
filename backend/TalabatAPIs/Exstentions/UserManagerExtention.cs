using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Exstentions
{
    public static class UserManagerExtention
    {

        public static async Task<AppUser?> GetUserAddressAsync( this UserManager<AppUser> userManager, ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user =await userManager.Users.Where(U => U.Email == email).Include(a => a.Address).FirstOrDefaultAsync();
            return user;

        }
    }
}
