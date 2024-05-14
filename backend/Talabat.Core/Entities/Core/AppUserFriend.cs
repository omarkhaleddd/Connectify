using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Entities.Core
{
    public class AppUserFriend : BaseEntity
    {
        public string UserId { get; set; }
        public string FriendId { get; set; }
    }
}
