using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Core;
using Talabat.Core.Specifications;

namespace Connectify.Core.Specifications
{
    internal class PostWithCommentSpecs : BaseSpecifications<Post>
    {
        public PostWithCommentSpecs() :base() 
        {
            Includes.Add(P => P.Comments);
        }
    }
}
