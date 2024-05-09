using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using Talabat.APIs.Controllers;
using Talabat.APIs.DTO;
using Talabat.APIs.Errors;
using Talabat.APIs.Exstentions;
using Talabat.Core.Entities.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications;
using Talabat.Repository;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : APIBaseController
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _manager;
        private readonly IGenericRepository<Post> _repositoryPost;
        private readonly IGenericRepository<Comment> _repositoryComment;

        public PostController(IMapper mapper, UserManager<AppUser> manager, IGenericRepository<Post> genericRepository , IGenericRepository<Comment> genericRepository2)
        {
            _mapper = mapper;
            _manager = manager;
            _repositoryPost = genericRepository;
            _repositoryComment = genericRepository2;
        }
        //Get Post by UserId
        //Get Posts
        [Authorize]
        [HttpGet("")]
        public async Task<ActionResult<PostDto>> GetPosts()
        {
            var spec = new BaseSpecifications<Post>();
            var Posts = await _repositoryPost.GetAllWithSpecAsync(spec);
            if(Posts == null)
            {
                return BadRequest("No posts");
            }
            else
                return Ok(Posts);
        }
        //Get Post by id
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetPost(int id)
        {
            //get by id in specs
            var spec = new BaseSpecifications<Post>(P=>P.Id == id);
            var post = await _repositoryPost.GetEntityWithSpecAsync(spec);
            if (post == null)
            {
                return BadRequest("No posts");
            }
            else
                return Ok(post);
        }
        //Post Request
        [Authorize]
        [HttpPost("")]
        public async Task<ActionResult<PostDto>> CreatePost(PostDto newPost) 
        {
            var user = await _manager.GetUserAddressAsync(User);
            if (user is null)
                return Unauthorized(new ApiResponse(401));

            var post = _mapper.Map<PostDto, Post>(newPost);
            post.AuthorId = user.Id;
            post.Comments = null;
            var result = _repositoryPost.Add(post);
            _repositoryPost.SaveChanges();
            if (!result.IsCompletedSuccessfully)
                return BadRequest(new ApiResponse(400));

            return Ok(post);

        }
        //Put Request 
        [Authorize]
		[HttpPut("{id}")]
		public async Task<ActionResult<PostDto>> UpdatePost(PostDto newPost,int id)
		{
			var user = await _manager.GetUserAddressAsync(User);
            var oldPost = await _repositoryPost.GetEntityWithSpecAsync(new BaseSpecifications<Post>(P => P.Id == id));
			if (user is null)
				return Unauthorized(new ApiResponse(401));

			if (oldPost is null)
				return BadRequest(new ApiResponse(404));

			var post = _mapper.Map<PostDto, Post>(newPost);
            post = oldPost;
			_repositoryPost.Update(post);
			_repositoryPost.SaveChanges();

            return Ok(post);
		}
        //Delete Post
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<PostDto>> DeletePost(int id)
        {
            //get by id in specs
            var spec = new BaseSpecifications<Post>(P => P.Id == id);
            var post = await _repositoryPost.GetEntityWithSpecAsync(spec);
            if (post == null)
            {
                return BadRequest("No posts");
            }
            else
            {
                if (post.Comments != null)
                {
                    //for loop on comments to remove
                    foreach (Comment comment in post.Comments)
                    {
                        _repositoryComment.Delete(comment);
                    }
                }
                _repositoryPost.Delete(post);
                _repositoryPost.SaveChanges();
            }
            return Ok("Post deleted");
        }

        //Put Request 
        [Authorize]
        [HttpPut("AddComment")]
        public async Task<ActionResult<PostDto>> AddComment(CommentDto newComment)
        {
            var user = await _manager.GetUserAddressAsync(User);
            var oldPost = await _repositoryPost.GetEntityWithSpecAsync(new BaseSpecifications<Post>(P => P.Id == newComment.PostId));
            if (user is null)
                return Unauthorized(new ApiResponse(401));
            if (oldPost is null)
                return BadRequest(new ApiResponse(404));
            var post = oldPost;
            var mappedComment = _mapper.Map<CommentDto, Comment>(newComment);
            mappedComment.AuthorId = user.Id;   
            post.Comments.Add(mappedComment);
            _repositoryPost.Update(post);
            _repositoryPost.SaveChanges();

            return Ok(post);
        }
        //Put Request 
        //[Authorize]
        //[HttpPut("EditComment/{id}")]
        //public async Task<ActionResult<PostDto>> EditComment(CommentDto newComment ,int id)
        //{
        //    var user = await _manager.GetUserAddressAsync(User);
        //    var oldComment = await _repositoryComment.GetEntityWithSpecAsync(new BaseSpecifications<Comment>(C => C.Id == id));
        //    if (user is null)
        //        return Unauthorized(new ApiResponse(401));
        //    if (oldComment is null)
        //        return BadRequest(new ApiResponse(404));

        //    //for retrieving post to update it
        //    var oldPost = await _repositoryPost.GetEntityWithSpecAsync(new BaseSpecifications<Post>(P => P.Id == newComment.PostId));
        //    var post = oldPost;
        //    //mapping the new comment to add authorId
        //    var mappedComment = _mapper.Map<CommentDto, Comment>(newComment);
        //    mappedComment.AuthorId = user.Id;
        //    mappedComment = oldComment;
        //    //deleting old comment
        //    var spec = new BaseSpecifications<Comment>(C => C.Id == mappedComment.Id);
        //    var deletedComment = await _repositoryComment.GetEntityWithSpecAsync(spec);
        //    _repositoryComment.Delete(deletedComment);
        //    //update post
        //    post.Comments.Add(mappedComment);
        //    _repositoryPost.Update(post);
        //    _repositoryPost.SaveChanges();

        //    return Ok(post);
        //}
        //get posts with comments
        [Authorize]
        [HttpGet("GetPostsWithComments")]
        public async Task<ActionResult<PostDto>> GetPostsWithComments()
        {
            var spec = new PostWithCommentSpecs();
            var Posts = await _repositoryPost.GetAllWithSpecAsync(spec);
            if (Posts == null)
            {
                return BadRequest("No posts");
            }
            else
                return Ok(Posts);
        }

        //Get specific post with comments by post id
        [Authorize]
        [HttpGet("GetPostByIdWithComments/{id}")]
        public async Task<ActionResult<PostDto>> GetPostByIdWithComments(int id)
        {
            //get by id in specs
            var spec = new PostWithCommentSpecs(id);
            var post = await _repositoryPost.GetEntityWithSpecAsync(spec);
            if (post == null)
            {
                return BadRequest(new ApiResponse(404));
            }
            else
                return Ok(post);
        }
        //Get specific post with comments by AuthorId
        //[Authorize]
        //[HttpGet("GetPostAuthorByIdWithComments/{id}")]
        //public async Task<ActionResult<PostDto>> GetPostAuthorByIdWithComments(int id)
        //{
        //    //get by id in specs
        //    var spec = new PostWithCommentSpecs(id);
        // i want to get the posts of the user with id so the specs
        //    var post = await _repositoryPost.GetEntityWithSpecAsync(spec);
        //    if (post == null)
        //    {
        //        return BadRequest(new ApiResponse(404));
        //    }
        //    else
        //        return Ok(post);
        //}
        //Delete Post
        [Authorize]
        [HttpDelete("DeleteComment/{id}")]
        public async Task<ActionResult<PostDto>> DeleteComment(int id)
        {
            var spec = new BaseSpecifications<Comment>(P => P.Id == id);
            var comment = await _repositoryComment.GetEntityWithSpecAsync(spec);
            if (comment == null)
            {
                return BadRequest("No Comment");
            }
            var postSpec = new BaseSpecifications<Post>(P => P.Id == comment.PostId);
            var post = await _repositoryPost.GetEntityWithSpecAsync(postSpec);
            post.Comments.Remove(comment);
            _repositoryPost.Update(post);
            _repositoryPost.SaveChanges();
            _repositoryComment.Delete(comment);
            _repositoryComment.SaveChanges();
            return Ok("Comment deleted");
        }
    }
}
