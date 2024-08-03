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
    public class FriendService : IFriendService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FriendService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<string>> GetFriendsByUserIdAsync(string userId)
        {
            var specFriendUserId = new BaseSpecifications<AppUserFriend>(u => u.UserId == userId);
            var specFriendFriendId = new BaseSpecifications<AppUserFriend>(u => u.FriendId == userId);
            var friendsByUserId = await _unitOfWork.Repository<AppUserFriend>().GetAllWithSpecAsync(specFriendUserId);
            var friendsByFriendId = await _unitOfWork.Repository<AppUserFriend>().GetAllWithSpecAsync(specFriendFriendId);
            var friendsList = new List<string>();
            foreach (var friendByUserId in friendsByUserId)
            {
                friendsList.Add(friendByUserId.FriendId);
            }

            foreach (var friendByFriendId in friendsByFriendId)
            {

                friendsList.Add(friendByFriendId.UserId);
            }

            return friendsList;
        }
    }
}
