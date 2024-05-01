using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Controllers;
using Talabat.APIs.DTO;
using Talabat.APIs.Errors;
using Talabat.APIs.Exstentions;
using Talabat.Core.Entities.Core;
using Talabat.Core.Entities.Identity;
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
        private readonly GenericRepository<Post> _repository;
        public PostController(IMapper mapper, UserManager<AppUser> manager, GenericRepository<Post> genericRepository)
        {
            _mapper = mapper;
            _manager = manager;
            _repository = genericRepository;
        }
        //Get Posts
        [Authorize]
        [HttpGet("")]
        public async Task<ActionResult<PostDto>> GetPosts()
        {
            var spec = new BaseSpecifications<Post>();
            var Posts = await _repository.GetAllWithSpecAsync(spec);
            if(Posts == null)
            {
                return BadRequest("No posts");
            }
            else
                return Ok(Posts);

        }
        //Get Post by id
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
            var result = _repository.Add(post);

            if (!result.IsCompletedSuccessfully)
                return BadRequest(new ApiResponse(400));

            return Ok(post);

        }
        //Put Request 
        //Delete Post
        //Get Post by UserId
    }
}
