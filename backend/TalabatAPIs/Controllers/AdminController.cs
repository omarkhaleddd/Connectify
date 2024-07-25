using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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


namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class AdminController : APIBaseController
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHubContext<AccountNotificationHub, INotificationHub> _accountNotification;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _manager;
        private readonly IUnitOfWork _unitOfWork;

        
        public AdminController(IMapper mapper,IUnitOfWork unitOfWork, UserManager<AppUser> manager, SignInManager<AppUser> signInManager, ITokenService tokenService, IHubContext<AccountNotificationHub, INotificationHub> accountNotification, IEmailService emailService)
        {
            _mapper = mapper;
            _manager = manager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _accountNotification = accountNotification;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("home/{daysGrowth}")]
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
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<ReportedPostDto>>> GetReports()
        {
            

            var spec = new ReportedPostWithPostSpecs();
            var ReportedPosts = await _unitOfWork.Repository<ReportedPost>().GetAllWithSpecAsync(spec);
            if (ReportedPosts == null)
            {
                return BadRequest("No Reported Posts");
            }
            var mappedReportedPosts = new List<ReportedPostDto>();
            mappedReportedPosts = _mapper.Map<List<ReportedPost>, List<ReportedPostDto>>(ReportedPosts.ToList());

            return Ok(mappedReportedPosts);
        }

        
        [HttpPut("action-report/{id}")]
        public async Task<ActionResult> DismissResolveReport(int id,[FromBody] FlagDto flag)
        {
            var spec = new BaseSpecifications<ReportedPost>(u => u.Id == id);
            var currReportedPost = await _unitOfWork.Repository<ReportedPost>().GetEntityWithSpecAsync(spec);
            if (currReportedPost is null)
            {
                return NotFound();
            }
            var postSpec = new BaseSpecifications<Post>(u => u.Id == currReportedPost.PostId);
            var currPost = await _unitOfWork.Repository<Post>().GetEntityWithSpecAsync(postSpec);
            if (currPost is null)
            {
                return NotFound();
            }
            if (flag.number == 1)
            {
                currReportedPost.Status = "dismissed";
                _unitOfWork.Repository<ReportedPost>().Update(currReportedPost);
                _unitOfWork.Repository<ReportedPost>().SaveChanges();
                currPost.ReportCount = 0;
                _unitOfWork.Repository<Post>().Update(currPost);
                _unitOfWork.Repository<Post>().SaveChanges();
                return Ok("Report Dismissed");
            }
            else
            {
                currReportedPost.Status = "resolved";
                _unitOfWork.Repository<ReportedPost>().Update(currReportedPost);
                _unitOfWork.Repository<ReportedPost>().SaveChanges();
                return Ok("Report Resolved");
            }

        }

        [HttpDelete("delete-user/{id}")]
        public async Task<ActionResult> DeleteUser(string Id)
        {
            
            var user = await _manager.GetUserByIdAsync(Id);
            if (user is null)
                return NotFound(new ApiResponse(404));
            var result = await _manager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));
            return Ok();
        }
        [HttpGet("Users")]
        public async Task<ActionResult<AppUserDto>> GetUsers()
        {
            var users = await _manager.GetAllUsersAsync();
            var mappedUsers = _mapper.Map<List<AppUser>,List<AppUserDto>>(users.ToList());
            if (users is null)
                return NotFound(new ApiResponse(404));
            
            return Ok(mappedUsers);
        }

        [HttpGet("GetPostsByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPostsByAuthorId(string userId)
        {
            var user = await _manager.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User not found");
            }
           
            //get by id in specs
            var spec = new PostWithCommentSpecs(userId);
            var posts = await _unitOfWork.Repository<Post>().GetAllWithSpecAsync(spec);

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
                    AuthorName = user.DisplayName,
                    AuthorImage = user.ProfileImageUrl

                };

                postDtos.Add(postDto);
            }

            return Ok(postDtos);
        }
        [HttpGet("get-all-posts")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPostsAdmin()
        {
            var spec = new PostWithCommentSpecs();
            var posts = await _unitOfWork.Repository<Post>().GetAllWithSpecAsync(spec);

            if (posts == null)
            {
                return BadRequest("No posts");
            }
            var postDtos = new List<PostDto>();
            foreach (var post in posts)
            {
                var user = await _manager.GetUserByIdAsync(post.AuthorId);
                string defaultValue = "null";
                //if (user == null)
                //{
                //    return NotFound($"User not found for post with ID: {post.Id}");
                //}
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
                    AuthorId = user?.Id?? defaultValue,
                    AuthorName = user?.DisplayName??defaultValue,
                    UploadedFileNames = PostImages
                };

                postDtos.Add(postDto);
            }

            return Ok(postDtos);
        }
    }
}
