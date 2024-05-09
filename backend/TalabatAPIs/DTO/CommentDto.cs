using System.ComponentModel.DataAnnotations.Schema;
using Talabat.Core.Entities.Core;

namespace Talabat.APIs.DTO
{
    public class CommentDto
    {
        public string content { get; set; }
        public int likeCount { get; set; }
        public DateTime DatePosted { get; set; }
        public int PostId { get; set; }

    }
}
