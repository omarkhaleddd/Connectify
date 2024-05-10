using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Core;
using Talabat.Core.Specifications;

namespace Talabat.Core.Specifications
{
    public class PostWithCommentSpecs : BaseSpecifications<Comment>
    {
        public PostWithCommentSpecs() :base() 
        {
        }
        public PostWithCommentSpecs(int postId): base(C => C.PostId == postId)
        {
        }
    }
}
