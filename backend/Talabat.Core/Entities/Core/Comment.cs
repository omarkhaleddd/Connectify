using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Connectify.Core.Entities.Core
{
    public class Comment
    {
        public int Id { get; set; }
        public string content { get; set; }
        public int likeCount { get; set; }
        public DateTime DatePosted { get; set; }
        [ForeignKey("post")]
        public int postId { get; set; }
        public Post post { get; set; }
        [ForeignKey("AppUser")]
        public string appUserId { get; set; }
        public AppUser AppUser { get; set; }

    }
}
