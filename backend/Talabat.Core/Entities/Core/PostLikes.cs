using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Core
{
    public class PostLikes : BaseEntity
    {
        public PostLikes() 
        {
            IsDeleted = false;
            InsertDate = DateTime.Now;
            UpdateDate = DateTime.Now;
            DeleteDate = null;
        }
        public string userId { get; set; }
		public string userName { get; set; }
		[ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }

    }
}
