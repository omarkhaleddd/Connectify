using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Core;

namespace Talabat.Core.Services
{
    public interface IPostService
    {
        Task<bool> CheckBlockStatus(string authorId);
        Task<IReadOnlyList<Post>> GetPostsByAuthorIdAsync(string authorId);
        bool CheckPrivatePrivacy(Post post);
        Task<bool> CheckOnlyFriendsPrivacy(string authorId,Post post);
        Task<List<Post>> GetPublicPostsByAuthorIdAsync(string authorId);

    }
}
