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
        public int LikeCount { get; set; }
        public ICollection<PostLikesDto>? Likes { get; set; } = new HashSet<PostLikesDto>();
        public ICollection<CommentDto>? Comments { get; set; } = new HashSet<CommentDto>();
        public ICollection<string> mentions { get; set; } = new HashSet<string>();
        public string? FileName { get; set; } // For storing single uploaded filename
        public List<FileNameDto>? UploadedFileNames { get; set; } // For storing multiple uploaded filenames
        public IFormFile? UploadedFile { get; set; }
        public IFormFileCollection? UploadedFiles { get; set; }

    }
}
