using Connetify.APIs.DTO;
using Talabat.APIs.DTO;
using Talabat.Core.Entities.Core;

namespace Talabat.APIs.DTO
{
	public class RepostDto
	{
		private PostDto post;

		public int Id { get; set; }
		public string content { get; set; }
		public DateTime DatePosted { get; set; }
		public string AuthorId { get; set; } = string.Empty;
		public string AuthorName { get; set; } = string.Empty;
		public int LikeCount { get; set; }
		public ICollection<RepostLikesDto>? Likes { get; set; } = new HashSet<RepostLikesDto>();
		public ICollection<CommentDto>? Comments { get; set; } = new HashSet<CommentDto>();
		public int PostId { get; set; }
		public PostDto? Post { get => post; set => post = value; }
	}
}
