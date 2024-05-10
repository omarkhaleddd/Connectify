using System.ComponentModel.DataAnnotations.Schema;
using Talabat.Core.Entities.Core;

namespace Talabat.APIs.DTO
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string content { get; set; }
        public int likeCount { get; set; }
        public DateTime DatePosted { get; set; }
        public string AuthorId { get; set; } = string.Empty;
        public int PostId { get; set; }

    }
}
