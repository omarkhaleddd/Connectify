using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Talabat.APIs.DTO;
using Talabat.APIs.Errors;
using Talabat.APIs.Exstentions;
using Talabat.APIs.Hubs;
using Talabat.Core;
using Talabat.Core.Entities.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Hubs.Interfaces;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications;
using Talabat.Service.Services;


namespace Talabat.APIs.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : APIBaseController
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHubContext<AccountNotificationHub, INotificationHub> _accountNotification;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _manager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AdminService _adminService;

        public AdminController(IMapper mapper,IUnitOfWork unitOfWork, AdminService adminService,UserManager<AppUser> manager, SignInManager<AppUser> signInManager, ITokenService tokenService, IHubContext<AccountNotificationHub, INotificationHub> accountNotification, IEmailService emailService)
        {
            _mapper = mapper;
            _manager = manager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _accountNotification = accountNotification;
            _unitOfWork = unitOfWork;
            _adminService = adminService;
        }

        [HttpGet("home/{daysGrowth}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Home( int daysGrowth)
        {
            var spec = new BaseSpecifications<Post>();
            var Posts = await _unitOfWork.Repository<Post>().GetAllWithSpecAsync(spec);
            var users = await _manager.GetAllUsersAsync();
            var postGraph = Posts
                .GroupBy(p => p.InsertDate.ToString("MMMM-yyyy"))
                .Select(g => new
                {
                    DateCreated = g.Key,
                    PostCount = g.Count()
                })
                .ToList();
            var usersGraph = users
                .Where(u => u.DateCreated.HasValue)
                  .GroupBy(p => p.DateCreated.HasValue ? p.DateCreated.Value.ToString("MMMM-yyyy") : "No Date") 
                .Select(g => new
                {
                    DateCreated = g.Key,
                    usersCount = g.Count()
                })
                .ToList();
            var postGraphX = new List<string>();
            var postGraphY = new List<int>();
            foreach (var post in postGraph) {
                postGraphX.Add(post.DateCreated);
                postGraphY.Add(post.PostCount);
            }
            var userGraphX = new List<string>();
            var userGraphY = new List<int>();
            foreach (var user in usersGraph)
            {
                if(user.DateCreated is null)
                {
                    continue;
                }
                userGraphX.Add(user.DateCreated);
                userGraphY.Add(user.usersCount);
            }

            var DaysAgo = DateTime.Now.AddDays(-daysGrowth).Date;
            var countPosts = Posts.Count(x=> x.InsertDate.Date <= DaysAgo);
            var countUsers = users.Count(x => x.DateCreated.HasValue ? x.DateCreated.Value.Date <= DaysAgo : false);
            var totalUserCount = users.Count();
            var totalPostCount = Posts.Count();
            double postGrowth;
            double usersGrowth;

            if (countPosts == 0)
            {
                postGrowth = totalPostCount == 0 ? 0 : 100; 
            }
            else
                postGrowth = Math.Round(((double)(totalPostCount - countPosts) / countPosts) * 100 , 2);
            if (countUsers == 0)
            {
                usersGrowth = totalUserCount == 0 ? 0 : 100;
            }
            else
                usersGrowth = Math.Round(((double)(totalUserCount - countUsers) / countUsers) * 100, 2);

            var HomeResult = new
            {
                postCount = totalPostCount,
                postGrowth,
                userCount = totalUserCount,
                usersGrowth,
                PostGraph = new { GraphX = postGraphX , GraphY = postGraphY },
                usersGraph = new { GraphX = userGraphX, GraphY = userGraphY },

            };
            return Ok(HomeResult);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<ReportedPostDto>>> GetReports()
        {
            var ReportedPosts =  await _adminService.GetReports();   
            var mappedReportedPosts = _mapper.Map<List<ReportedPost>, List<ReportedPostDto>>(ReportedPosts.ToList());

            return Ok(mappedReportedPosts);
        }

        
        [HttpPut("action-report/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> DismissResolveReport(int id,[FromBody] FlagDto flag)
        {
            try
            {
              var result = await _adminService.DismissResolveReport(id, flag.number);
              return result ? Ok("Post Report Dismissed"): Ok("Post Report Resolved");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("delete-user/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(string Id)
        {
            var user = await _manager.GetUserByIdAsync(Id);
            if (user is null)
                return NotFound("User Not Found");
            try {
                _adminService.DeleteUser(user);
                return Ok();
            }
            catch(Exception e) {
            return BadRequest(e.Message);   
            }

        }
        [HttpGet("Users")]
        //[Authorize(Roles ="Admin")]
        public async Task<ActionResult<AppUserDto>> GetUsers()
        {
            var users = await _manager.GetAllUsersAsync();
            var mappedUsers = _mapper.Map<List<AppUser>,List<AppUserDto>>(users.ToList());
            if (users is null)
                return NotFound(new ApiResponse(404));
            
            return Ok(mappedUsers);
        }

        [HttpGet("GetPostsByUserId/{userId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPostsByAuthorId(string userId)
        {
            var user = await _manager.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound($"User not found");
            var posts = _adminService.getPostsAdmin(user.Id).Result;
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
                    AuthorName = user.DisplayName,
                    AuthorImage = user.ProfileImageUrl

                };

                postDtos.Add(postDto);
            }

            return Ok(postDtos);
        }
        [HttpGet("get-all-posts")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPostsAdmin()
        {
            try
            {
                var posts = _adminService.getPostsAdmin(null).Result;
                var postDtos = new List<PostDto>();
                foreach (var post in posts)
                {
                    var user = await _manager.GetUserByIdAsync(post.AuthorId);
                    string defaultValue = "null";

                    var comments = _mapper.Map<ICollection<Comment>, ICollection<CommentDto>>(post.Comments);
                    var PostLikes = _mapper.Map<ICollection<PostLikes>, ICollection<PostLikesDto>>(post.Likes);
                    var PostImages = _mapper.Map<ICollection<FileNames>, ICollection<FileNameDto>>(post.FileName);
                    var postDto = new PostDto
                    {
                        Id = post.Id,
                        content = post.content,
                        Likes = PostLikes,
                        LikeCount = post.Likes.Count,
                        DatePosted = post.DatePosted,
                        Comments = comments,
                        AuthorId = user?.Id ?? defaultValue,
                        AuthorName = user?.DisplayName ?? defaultValue,
                        UploadedFileNames = PostImages
                    };

                    postDtos.Add(postDto);
                }
                return Ok(postDtos);
            }
            catch (Exception ex)
            {
               return BadRequest(ex.Message);
            }
        }
    }
}
