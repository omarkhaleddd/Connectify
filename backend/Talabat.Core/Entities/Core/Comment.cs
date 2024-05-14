using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Entities.Core
{
    public class Comment : BaseEntity
    {
        public Comment()
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
		[ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }
        public ICollection<CommentLikes>? Likes { get; set; } = new HashSet<CommentLikes>();

    }
}
