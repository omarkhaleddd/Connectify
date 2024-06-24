using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Talabat.APIs.Controllers;
using Talabat.APIs.DTO;
using Talabat.APIs.Errors;
using Talabat.APIs.Exstentions;
using Talabat.Core.Entities.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : APIBaseController
    {
        private readonly IGenericRepository<Comment> _repositoryComment;
        private readonly IGenericRepository<Post> _repositoryPost;
        private readonly UserManager<AppUser> _manager;
        private readonly IMapper _mapper;
		private readonly IGenericRepository<PostLikes> _repositoryPostLikes;

		public CommentController(IGenericRepository<Comment> genericRepository, IGenericRepository<Post> genericRepositoryPost, UserManager<AppUser> manager, IMapper autoMapper, IGenericRepository<PostLikes> repositoryPostLikes) { 
            _repositoryComment = genericRepository;
            _repositoryPost = genericRepositoryPost;
            _mapper = autoMapper;
            _manager = manager;
			_repositoryPostLikes = repositoryPostLikes;
		}
        //Get Comments by PostId
        [Authorize]
        [HttpGet("GetCommentsByPostId/{id}")]
        public async Task<ActionResult<CommentDto>> GetCommentsByPostId(int id)
        {
            var user = await _manager.GetUserAddressAsync(User);
            var currPost = await _repositoryPost.GetEntityWithSpecAsync(new BaseSpecifications<Post>(P => P.Id == id));
            var currComments = await _repositoryComment.GetAllWithSpecAsync(new BaseSpecifications<Comment>(C => C.PostId == id));
            if (user is null)
                return Unauthorized(new ApiResponse(401));
            if (currPost is null)
                return BadRequest(new ApiResponse(404));
            if (currComments is null)
                return BadRequest(new ApiResponse(404));
            var mappedComments = _mapper.Map<IReadOnlyList<Comment>, List<CommentDto>>(currComments);
            return Ok(mappedComments);
        }
        //Post Request 
        [Authorize]
        [HttpPost("AddComment")]
        public async Task<ActionResult<CommentDto>> AddComment(CommentDto newComment)
        {
            var user = await _manager.GetUserAddressAsync(User);
            var oldPost = await _repositoryPost.GetEntityWithSpecAsync(new BaseSpecifications<Post>(P => P.Id == newComment.PostId));
            if (user is null)
                return Unauthorized(new ApiResponse(401));
            if (oldPost is null)
                return BadRequest(new ApiResponse(404));
            newComment.AuthorId = user.Id;
            var mappedComment = _mapper.Map<CommentDto, Comment>(newComment);
            mappedComment.AuthorId = user.Id;
            mappedComment.AuthorName = user.DisplayName;
            var result = _repositoryComment.Add(mappedComment);
            if (!result.IsCompletedSuccessfully)
                return BadRequest(new ApiResponse(400));
            _repositoryComment.SaveChanges();
            return Ok(newComment);
        }
        //Put Request 
        [Authorize]
        [HttpPut("UpdateComment")]
        public async Task<ActionResult<CommentDto>> UpdateComment(CommentDto newComment)
        {
            var user = await _manager.GetUserAddressAsync(User);
            var oldPost = await _repositoryPost.GetEntityWithSpecAsync(new BaseSpecifications<Post>(P => P.Id == newComment.PostId));
            var oldComment = await _repositoryComment.GetEntityWithSpecAsync(new BaseSpecifications<Comment>(C => C.Id == newComment.Id));
            if (user is null)
                return Unauthorized(new ApiResponse(401));
            if (oldPost is null)
                return BadRequest(new ApiResponse(404));
            if (oldComment is null)
                return BadRequest(new ApiResponse(404));
            if (!newComment.AuthorId.Equals(user.Id))
            {
                return Unauthorized(new ApiResponse(401));
            }
            newComment.AuthorId = user.Id;
			oldComment.content = newComment.content;

            _repositoryComment.Update(oldComment);
            _repositoryComment.SaveChanges();
            return Ok(newComment);
        }
        //Delete Post
        [Authorize]
        [HttpDelete("DeleteComment/{id}")]
        public async Task<ActionResult<PostDto>> DeleteComment(int id)
        {
            var user = await _manager.GetUserAddressAsync(User);
            var comment = await _repositoryComment.GetEntityWithSpecAsync(new BaseSpecifications<Comment>(C => C.Id == id));
            if (user is null)
                return Unauthorized(new ApiResponse(401));
            if (comment is null)
                return BadRequest(new ApiResponse(404));
            if (comment.AuthorId.Equals(user.Id))
            {
                return Unauthorized(new ApiResponse(401));
            }
            _repositoryComment.Delete(comment);
            _repositoryComment.SaveChanges();
            return Ok("Comment Deleted");
        }

		//[HttpPut("LikePost/{PostId}")]
		//public async Task<ActionResult<PostDto>> LikePost(int CommentId)
		//{
		//	var user = await _manager.GetUserAddressAsync(User);
		//	var comment = await _repositoryComment.GetEntityWithSpecAsync(new BaseSpecifications<Comment>(C => C.Id == CommentId));
		//	if (user is null)
		//		return Unauthorized(new ApiResponse(401));

		//	if (comment is null)
		//		return BadRequest(new ApiResponse(404));

		//	var spec = new BaseSpecifications<PostLikes>(C => C.Id == CommentId);
		//	var postLikes = await _repositoryPostLikes.GetAllWithSpecAsync(spec);


		//	var isLiked = postLikes.Where(L => L.userId == user.Id).FirstOrDefault();

		//	if (isLiked is null)
		//	{
		//		var newPostLikes = new Comment { userId = user.Id, PostId = CommentId, userName = user.DisplayName };
		//		var mappedPostLikes = _mapper.Map<PostLikesDto, PostLikes>(newPostLikes);
		//		await _repositoryPostLikes.Add(mappedPostLikes);
		//		_repositoryPostLikes.SaveChanges();
		//		// hena ba3ml retreive ll post b3d el update
		//		var specLikes = new PostWithCommentSpecs(PostId);
		//		var post = await _repositoryPost.GetEntityWithSpecAsync(specLikes);
		//		var retPostLikes = _mapper.Map<ICollection<PostLikes>, ICollection<PostLikesDto>>(post.Likes);
		//		var response = new { message = "Dislike", likeCount = retPostLikes.Count, likes = retPostLikes };

		//		return Ok(JsonSerializer.Serialize(response));
		//	}
		//	else
		//	{
		//		_repositoryPostLikes.Delete(isLiked);
		//		_repositoryPostLikes.SaveChanges();
		//		// hena ba3ml retreive ll post b3d el update w 3ayz avalidate hena la2no ka by3ml remove fa momken yrg3lo zero 
		//		var specLikes = new PostWithCommentSpecs(PostId);
		//		var post = await _repositoryPost.GetEntityWithSpecAsync(specLikes);
		//		var retPostLikes = _mapper.Map<ICollection<PostLikes>, ICollection<PostLikesDto>>(post?.Likes);

		//		var response = new { message = "Like", likeCount = retPostLikes.Count, likes = retPostLikes };

		//		return Ok(JsonSerializer.Serialize(response));
		//	}

		//}

	}
}
