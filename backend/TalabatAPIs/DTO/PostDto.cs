using Talabat.Core.Entities.Core;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.DTO
{
    public class PostDto
    {
        public int Id { get; set; }
        public string content { get; set; }
        public DateTime DatePosted { get; set; }
        public string AuthorId { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string? AuthorImage { get; set; }
        public int LikeCount { get; set; }
        public ICollection<PostLikesDto>? Likes { get; set; } = new HashSet<PostLikesDto>();
        public ICollection<CommentDto>? Comments { get; set; } = new HashSet<CommentDto>();
        public ICollection<string> mentions { get; set; } = new HashSet<string>();
        public ICollection<FileNameDto>? UploadedFileNames { get; set; } = new HashSet<FileNameDto>(); 
        public List<IFormFile>? UploadedFiles { get; set; }
        public int Privacy { get; set; }
    }
}
