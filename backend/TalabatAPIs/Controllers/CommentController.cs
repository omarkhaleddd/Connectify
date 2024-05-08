using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _manager;
        private readonly IGenericRepository<Post> _repositoryPost;
        private readonly IGenericRepository<Comment> _repositoryComment;

        public CommentController(IMapper mapper, UserManager<AppUser> manager, IGenericRepository<Post> genericRepository, IGenericRepository<Comment> genericRepository2)
        {
            _mapper = mapper;
            _manager = manager;
            _repositoryPost = genericRepository;
            _repositoryComment = genericRepository2;
        }
        [Authorize]
        [HttpGet("")]
        public async Task<ActionResult<PostDto>> GetPosts()
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
        //Add Comment
        //Delete Comment
        //Update Comment

    }
}
