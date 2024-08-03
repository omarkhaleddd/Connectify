using AutoMapper;
using Connectify.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Talabat.APIs.Controllers;
using Talabat.APIs.DTO;
using Talabat.APIs.Errors;
using Talabat.APIs.Exstentions;
using Talabat.Core;
using Talabat.Core.Entities.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : APIBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommentService _commentService;
        private readonly UserManager<AppUser> _manager;
        private readonly RedisCacheService _cacheService;
        private readonly IMapper _mapper;


        public CommentController(IUnitOfWork unitOfWork, ICommentService commentService,UserManager<AppUser> manager, IMapper autoMapper, RedisCacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _commentService = commentService;
            _mapper = autoMapper;
            _manager = manager;
            _cacheService = cacheService;
        }
        //Get Comments by PostId
        [Authorize]
        [HttpGet("GetCommentsByPostId/{id}")]
        public async Task<ActionResult<CommentDto>> GetCommentsByPostId(int id)
        {
            var user = await _manager.GetUserAddressAsync(User);
            if (user is null)
                return Unauthorized(new ApiResponse(401));
            try { 
                var currComments = _commentService.GetComments(id).Result;
                var mappedComments = _mapper.Map<IReadOnlyList<Comment>, List<CommentDto>>(currComments);
                return Ok(mappedComments);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        //Post Request 
        [Authorize]
        [HttpPost("AddComment")]
        public async Task<ActionResult<CommentDto>> AddComment(CommentDto newComment)
        {
            var user = await _manager.GetUserAddressAsync(User);
            if (user is null)
                return Unauthorized(new ApiResponse(401));
            var comment = _mapper.Map<CommentDto, Comment>(newComment);
            comment.AuthorId = user.Id;
            comment.AuthorName = user.DisplayName;
            try
            {
                var Result = _commentService.AddComment(comment).Result;
                var mappedComment = _mapper.Map<Comment,CommentDto>(Result);
                mappedComment.AuthorImage = user.ProfileImageUrl;
                var cachedPostsKey = $"posts - {user.Id}";
                var cachedRepostsKey = $"reposts - {user.Id}";

                if (await _cacheService.KeyExistsAsync(cachedPostsKey) && await _cacheService.KeyExistsAsync(cachedPostsKey))
                {
                    // Delete cached posts
                    await _cacheService.RemoveAsync(cachedPostsKey);

                    // Delete cached reposts
                    await _cacheService.RemoveAsync(cachedRepostsKey);

                }

                return Ok(mappedComment);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        //Put Request 
        [Authorize]
        [HttpPut("UpdateComment")]
        public async Task<ActionResult<CommentDto>> UpdateComment(CommentDto newComment)
        {
            var user = await _manager.GetUserAddressAsync(User);
            if (user is null)
                return Unauthorized(new ApiResponse(401));
            var comment = _mapper.Map<CommentDto , Comment>(newComment);
            comment.AuthorName = user.DisplayName;
            comment.AuthorId = user.Id;
            try
            {
                var Result = _commentService.UpdateComment(comment, user.Id).Result;
                var mappedResult = _mapper.Map<Comment,CommentDto>(Result);
                mappedResult.AuthorImage = user.ProfileImageUrl;
                var cachedPostsKey = $"posts - {user.Id}";
                var cachedRepostsKey = $"reposts - {user.Id}";

                if (await _cacheService.KeyExistsAsync(cachedPostsKey) && await _cacheService.KeyExistsAsync(cachedPostsKey))
                {
                    // Delete cached posts
                    await _cacheService.RemoveAsync(cachedPostsKey);

                    // Delete cached reposts
                    await _cacheService.RemoveAsync(cachedRepostsKey);

                }
                return Ok(mappedResult);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        //Delete Post
        [Authorize]
        [HttpDelete("DeleteComment/{id}")]
        public async Task<ActionResult<PostDto>> DeleteComment(int id)
        {
            var user = await _manager.GetUserAddressAsync(User);
            if (user is null)
                return Unauthorized(new ApiResponse(401));
            var comment = await _unitOfWork.Repository<Comment>().GetEntityWithSpecAsync(new BaseSpecifications<Comment>(C => C.Id == id));
            if (comment is null || comment.AuthorId.Equals(user.Id))
                return BadRequest(new ApiResponse(404));

            _unitOfWork.Repository<Comment>().Delete(comment);
            _unitOfWork.Repository<Comment>().SaveChanges();

            var cachedPostsKey = $"posts - {user.Id}";
            var cachedRepostsKey = $"reposts - {user.Id}";

            if (await _cacheService.KeyExistsAsync(cachedPostsKey) && await _cacheService.KeyExistsAsync(cachedPostsKey))
            {
                // Delete cached posts
                await _cacheService.RemoveAsync(cachedPostsKey);

                // Delete cached reposts
                await _cacheService.RemoveAsync(cachedRepostsKey);

            }

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
