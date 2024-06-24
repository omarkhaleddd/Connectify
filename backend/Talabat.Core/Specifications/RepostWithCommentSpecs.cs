using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Core;
using Talabat.Core.Specifications;

namespace Talabat.Core.Specifications
{
	public class RepostWithCommentSpecs : BaseSpecifications<Repost>
	{
		public RepostWithCommentSpecs() : base()
		{
			Includes.Add(P => P.Comments);
			Includes.Add(P => P.Likes);
			Includes.Add(P => P.Post);
			
		}
		public RepostWithCommentSpecs(int postId) : base(P => P.Id == postId)
		{
			Includes.Add(P => P.Comments);
			Includes.Add(P => P.Likes);
			Includes.Add(P => P.Post);
		}
		public RepostWithCommentSpecs(string authorId) : base(P => P.AuthorId == authorId)
		{
			Includes.Add(P => P.Comments);
			Includes.Add(P => P.Likes);
			Includes.Add(P => P.Post);
		}
	}
}
