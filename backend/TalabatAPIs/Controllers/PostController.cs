using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("post/{id}")]
        public async Task<ActionResult<PostDto>> GetPost()
        {
            //get by id in specs
            var spec = new BaseSpecifications<Post>();
            var Posts = await _repositoryPost.GetEntityWithSpecAsync(spec);
            if (Posts == null)
            {
                return BadRequest("No posts");
            }
            else
                return Ok(Posts);
        }
        //Post Request
        [Authorize]
        [HttpPost("create-post")]
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
        //Delete Post
        [Authorize]
        [HttpDelete("delete-post/{id}")]
        public async Task<ActionResult<PostDto>> DeletePost()
        {
            //get by id in specs
            var spec = new BaseSpecifications<Post>();
            var post = await _repositoryPost.GetEntityWithSpecAsync(spec);
            if (post == null)
            {
                return BadRequest("No posts");
            }
            else
            {
                if(post.Comments != null) { 
                //for loop on comments to remove
                    foreach(Comment comment in post.Comments)
                    {
                        _repositoryComment.Delete(comment);
                    }
                }
                _repositoryPost.Delete(post);
                _repositoryPost.SaveChanges();
            }
            return Ok("Post deleted");
        }
    }
}
