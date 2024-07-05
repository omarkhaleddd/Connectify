using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Talabat.APIs.DTO;
using Talabat.APIs.Errors;
using Talabat.APIs.Exstentions;
using Talabat.APIs.Hubs;
using Talabat.Core.Entities.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Hubs.Interfaces;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : APIBaseController
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _manager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHubContext<AccountNotificationHub, INotificationHub> _accountNotification;
        private readonly ITokenService _tokenService;
        private readonly IGenericRepository<Post> _repositoryPost;

        public AdminController(IMapper mapper, UserManager<AppUser> manager, IGenericRepository<Post> genericRepository, IGenericRepository<AppUserFriend> genericRepository1, IGenericRepository<FriendRequest> genericRepository2, IGenericRepository<BlockList> genericRepository3, IGenericRepository<Notification> genericRepository4, SignInManager<AppUser> signInManager, ITokenService tokenService, IHubContext<AccountNotificationHub, INotificationHub> accountNotification, IEmailService emailService)
        {
            _mapper = mapper;
            _manager = manager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _repositoryPost = genericRepository;
            _accountNotification = accountNotification;
        }


        [HttpGet("Users")]
        public async Task<ActionResult<UserDto>> GetUsers()
        {
            var users = await _manager.GetAllUsersAsync();
            if (users is null)
                return NotFound(new ApiResponse(404));
            
            return Ok(users);
        }

        [HttpGet("GetPostsByUserId/{userId}")]
        public async Task<ActionResult<PostDto>> GetPostsByAuthorId(string userId)
        {
            var user = await _manager.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User not found");
            }
           
            //get by id in specs
            var spec = new PostWithCommentSpecs(userId);
            var posts = await _repositoryPost.GetAllWithSpecAsync(spec);

            if (posts == null)
            {
                return BadRequest("No posts");
            }
            var postDtos = new List<PostDto>();
            foreach (var post in posts)
            {
                var comments = _mapper.Map<ICollection<Comment>, ICollection<CommentDto>>(post.Comments);
                var PostLikes = _mapper.Map<ICollection<PostLikes>, ICollection<PostLikesDto>>(post.Likes);

                var postDto = new PostDto
                {
                    Id = post.Id,
                    content = post.content,
                    Likes = PostLikes,
                    LikeCount = post.Likes.Count,
                    DatePosted = post.DatePosted,
                    Comments = comments,
                    AuthorId = user.Id,
                    AuthorName = user.DisplayName
                };

                postDtos.Add(postDto);
            }

            return Ok(postDtos);
        }


    }
}
