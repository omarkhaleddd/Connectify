using System.ComponentModel.DataAnnotations.Schema;
using Talabat.Core.Entities.Core;

namespace Talabat.APIs.DTO
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string content { get; set; }
        public DateTime DatePosted { get; set; }
        public string AuthorId { get; set; } = string.Empty;
		public string AuthorName { get; set; } = string.Empty;
		public int LikeCount { get; set; }
		public ICollection<CommentLikesDto>? Likes { get; set; } = new HashSet<CommentLikesDto>();
		public int PostId { get; set; }

    }
}
