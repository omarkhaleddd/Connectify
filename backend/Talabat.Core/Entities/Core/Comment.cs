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
        public Post post { get; set; }
        public AppUser AppUser { get; set; }

    }
}
