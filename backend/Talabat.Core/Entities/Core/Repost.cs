using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Entities.Core
{
	public class Repost : BaseEntity
	{

		public Repost()
		{
			DatePosted = DateTime.Now;
			IsDeleted = false;
			InsertDate = DateTime.Now;
			UpdateDate = DateTime.Now;
			DeleteDate = null;
			Likes = null;
		}

		public string content { get; set; }
		public DateTime DatePosted { get; set; }
		public string AuthorId { get; set; }
		public string AuthorName { get; set; }
		public ICollection<RepostLikes>? Likes { get; set; } = new HashSet<RepostLikes>();
		public ICollection<Comment>? Comments { get; set; } = new HashSet<Comment>();

		[ForeignKey("Post")]
		public int PostId { get; set; }
		public Post Post { get; set; }
	}
}
