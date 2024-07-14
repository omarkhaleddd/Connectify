using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.APIs.DTO;
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

		public static async Task<AppUser?> GetUserByIdAsync(this UserManager<AppUser> userManager , string id)
		{
			var user = await userManager.Users.Where(U => U.Id == id).FirstOrDefaultAsync();
			return user;
		}

        public static async Task<IEnumerable<AppUser>> GetAllUsersAsync(this UserManager<AppUser> userManager)
        {
            return await userManager.Users
                                 .Select(user => new AppUser
                                 {
                                     Id = user.Id,
                                     DisplayName = user.DisplayName,
                                     ProfileImageUrl = user.ProfileImageUrl,
                                    
                                 })
                                 .ToListAsync();
        }
    }
}
