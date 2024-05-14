using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Core;
using Talabat.Core.Entities;

namespace Connectify.Core.Entities.Core
{
	public class CommentLikes : BaseEntity
	{
		public CommentLikes()
		{
			IsDeleted = false;
			InsertDate = DateTime.Now;
			UpdateDate = DateTime.Now;
			DeleteDate = null;
		}
		public string userId { get; set; }
		public string userName { get; set; }
		[ForeignKey("Comment")]
		public int CommentId { get; set; }
		public Comment Post { get; set; }

	}
}
