using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Core;
using Talabat.Core.Specifications;

namespace Talabat.Core.Specifications
{
    public class PostWithCommentSpecs : BaseSpecifications<Post>
    {
        public PostWithCommentSpecs() :base() 
        {
            Includes.Add(P=>P.Comments);
            Includes.Add(P => P.Likes);
        }
        public PostWithCommentSpecs(int postId): base(P => P.Id == postId)
        {
			Includes.Add(P => P.Comments);
            Includes.Add(P => P.Likes);
        }
        public PostWithCommentSpecs(string authorId) : base(P => P.AuthorId == authorId)
        {
            Includes.Add(P => P.Comments);
            Includes.Add(P => P.Likes);
        }
    }
}
