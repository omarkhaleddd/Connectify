using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Core;
using Talabat.Core.Specifications;

namespace Talabat.Core.Specifications
{
	public class PostWithAuthor : BaseSpecifications<Post>
	{
		public PostWithAuthor() : base()
		{
			Includes.Add(P => P.AuthorId);
		}
		public PostWithAuthor(int postId) : base(p => p.Id == postId)
		{
			Includes.Add(P => P.AuthorId);
        }
    }
}
