using Talabat.Core.Entities.Core;

namespace Talabat.APIs.DTO
{
    public class PostDto
    {
        public string content { get; set; }
        public int likeCount { get; set; }
        public DateTime DatePosted { get; set; }
        public ICollection<Comment>? Comments { get; set; } = new HashSet<Comment>();

    }
}
