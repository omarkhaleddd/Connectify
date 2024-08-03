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
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<Comment>> GetComments(int id)
        {
            var currPost = await _unitOfWork.Repository<Post>().GetEntityWithSpecAsync(new BaseSpecifications<Post>(P => P.Id == id));
            if (currPost is null)
                throw new InvalidOperationException("Invalid Post");
            var currComments = await _unitOfWork.Repository<Comment>().GetAllWithSpecAsync(new BaseSpecifications<Comment>(C => C.PostId == id));
            if (currComments is null)
                throw new InvalidOperationException("Comments Not Found");
            return currComments;
        }
        public async Task<Comment> AddComment(Comment comment)
        {
            comment.Post = await _unitOfWork.Repository<Post>().GetEntityWithSpecAsync(new BaseSpecifications<Post>(P => P.Id == comment.PostId));
            if (comment.Post is null)
                throw new InvalidOperationException("Post Not Found");
            var result = _unitOfWork.Repository<Comment>().Add(comment);
            if (!result.IsCompletedSuccessfully)
                throw new InvalidOperationException("Comments Not Found");
            _unitOfWork.Repository<Comment>().SaveChanges();
            return comment;
        }


        public async Task<Comment> UpdateComment(Comment comment, string id)
        {
            var oldPost = await _unitOfWork.Repository<Post>().GetEntityWithSpecAsync(new BaseSpecifications<Post>(P => P.Id == comment.PostId));
            var oldComment = await _unitOfWork.Repository<Comment>().GetEntityWithSpecAsync(new BaseSpecifications<Comment>(C => C.Id == comment.Id));
            if (oldPost is null)
                throw new InvalidOperationException("Post Not Found");
            if (oldComment is null)
                throw new InvalidOperationException("Comment Not Found");
            if (!oldComment.AuthorId.Equals(id))
            {
                throw new InvalidOperationException("NonAuthorized Function");
            }
            oldComment.content = comment.content;

            _unitOfWork.Repository<Comment>().Update(oldComment);
            _unitOfWork.Repository<Comment>().SaveChanges();
            return comment;
        }
    }
}
