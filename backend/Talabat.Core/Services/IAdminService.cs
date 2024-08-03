using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Core;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Services
{
    public interface IAdminService
    {
        public Task<bool> DismissResolveReport(int id,int flag);
        public Task<IEnumerable<ReportedPost>> GetReports();
        public void DeleteUser(AppUser user);
        public Task<IEnumerable<Post>> getPostsAdmin(string? id);
    }
}
