using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Core;

namespace Talabat.Core.Services
{
    public interface IFriendService
    {
        Task<List<string>> GetFriendsByUserIdAsync(string userId);
    }
}
