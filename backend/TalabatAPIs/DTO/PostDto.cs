using Talabat.Core.Entities.Core;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.DTO
{
    public class PostDto
    {
        public int Id { get; set; }
        public string content { get; set; }
        public int likeCount { get; set; }
        public DateTime DatePosted { get; set; }
        public string AuthorId { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public ICollection<CommentDto>? Comments { get; set; } = new HashSet<CommentDto>();

    }
}
