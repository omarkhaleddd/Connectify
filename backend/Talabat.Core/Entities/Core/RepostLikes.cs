using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Core
{
	public class RepostLikes : BaseEntity
	{
		public RepostLikes()
		{
			IsDeleted = false;
			InsertDate = DateTime.Now;
			UpdateDate = DateTime.Now;
			DeleteDate = null;
		}
		public string userId { get; set; }
		public string userName { get; set; }
		[ForeignKey("Post")]
		public int RepostId { get; set; }
		public Repost Repost { get; set; }

	}
}
