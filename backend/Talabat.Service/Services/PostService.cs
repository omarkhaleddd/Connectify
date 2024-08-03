using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities.Core;
using Talabat.Core.Services;
using Talabat.Core.Specifications;

namespace Talabat.Service.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFriendService _friendService;

        public PostService(IUnitOfWork unitOfWork, IFriendService friendService)
        {
            _unitOfWork = unitOfWork;
            _friendService = friendService;
        }

        public async Task<bool> CheckBlockStatus(string authorId)
        {
            var blockSpec = new BaseSpecifications<BlockList>(u => u.BlockedId == authorId && u.UserId == authorId);
            var isBlocked = await _unitOfWork.Repository<BlockList>().GetEntityWithSpecAsync(blockSpec);

            if (isBlocked is not null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IReadOnlyList<Post>> GetPostsByAuthorIdAsync(string authorId)
        {

            var spec = new PostWithCommentSpecs(authorId);
            var posts = await _unitOfWork.Repository<Post>().GetAllWithSpecAsync(spec);

            return posts;
        }

        public bool CheckPrivatePrivacy(Post post)
        {
            if (post.Privacy == 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CheckOnlyFriendsPrivacy(string authorId, Post post)
        {
            var friendsList = await _friendService.GetFriendsByUserIdAsync(authorId);
            bool found = friendsList.Contains(authorId);
            if (post.Privacy == 2 || found || post.AuthorId == authorId) 
            {
                return true;
            }
            return false;
        }

        public Task<List<Post>> GetPublicPostsByAuthorIdAsync(string authorId)
        {
            throw new NotImplementedException();
        }
    }
}
