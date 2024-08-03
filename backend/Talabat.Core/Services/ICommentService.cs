using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Core;

namespace Talabat.Core.Services
{
    public interface ICommentService
    {
        public Task<IReadOnlyList<Comment>> GetComments(int id);
        public Task<Comment> UpdateComment(Comment comment, string id);
        public Task<Comment> AddComment(Comment comment);
    }
}
