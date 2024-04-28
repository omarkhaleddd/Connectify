using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Connectify.Core.Entities.Core
{
    public class Post
    {
        public int Id { get; set; }
        public string content { get; set; }
        public int likeCount { get; set; }
        public DateTime DatePosted { get; set; }
        [ForeignKey("AppUser")]
        public string appUserId { get; set; }

        public AppUser AppUser { get; set; }

        public ICollection<Comment> comments { get; set; } = new HashSet<Comment>();
    }
}
