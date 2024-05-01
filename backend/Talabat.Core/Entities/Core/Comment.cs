using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Entities.Core
{
    public class Comment : BaseEntity
    {
        public string content { get; set; }
        public int likeCount { get; set; }
        public DateTime DatePosted { get; set; } 
		public string AuthorId { get; set; }
		[ForeignKey("post")]
        public int PostId { get; set; }
        public Post Post { get; set; }
       

    }
}
