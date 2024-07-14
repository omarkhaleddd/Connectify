using AutoMapper;
using Connectify.Core.Services;
using Connetify.APIs.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.IO.MemoryMappedFiles;
using System.Security.Claims;
using System.Text.Json;
using Talabat.APIs.Controllers;
using Talabat.APIs.DTO;
using Talabat.APIs.Errors;
using Talabat.APIs.Exstentions;
using Talabat.APIs.Hubs;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Hubs.Interfaces;
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
		private readonly IHubContext<MentionNotification, INotificationHub> _mentionNotification;
		private readonly RedisCacheService _cacheService;
        private readonly IUploadService _uploadService;
        private readonly IUnitOfWork _unitOfWork;


        public PostController(IMapper mapper, UserManager<AppUser> manager,IUnitOfWork unitOfWork, RedisCacheService cacheService, IUploadService uploadService)
		{
			_mapper = mapper;
			_manager = manager;
			_cacheService = cacheService;
            _uploadService = uploadService;
            _unitOfWork = unitOfWork;
        }


        //Get Posts by UserId
        [Authorize]
        [HttpGet("GetPostByAuthorId/{authorId}")]
        public async Task<ActionResult<PostDto>> GetPostByAuthorId(string authorId)
        {
            var user = await _manager.GetUserByIdAsync(authorId);
            if (user == null)
            {
                return NotFound($"User not found");
            }
            //check if the this user added me in the blockList
            var blockSpec = new BaseSpecifications<BlockList>(u => u.BlockedId == user.Id && u.UserId == authorId);
            var isBlocked = await _unitOfWork.Repository<BlockList>().GetEntityWithSpecAsync(blockSpec);
            if (isBlocked is not null)
            {
                return BadRequest("You are Blocked");
            }
            //get by id in specs
            var spec = new PostWithCommentSpecs(authorId);
            var posts = await _unitOfWork.Repository<Post>().GetAllWithSpecAsync(spec);
            
            if (posts == null)
            {
                return BadRequest("No posts");
            }
            var postDtos = new List<PostDto>();
            foreach (var post in posts)
            {
                if (post.ReportCount == 10)
                {
                    continue;
                }
                var comments = _mapper.Map<ICollection<Comment>, ICollection<CommentDto>>(post.Comments);
                var PostLikes = _mapper.Map<ICollection<PostLikes>, ICollection<PostLikesDto>>(post.Likes);

                var postDto = new PostDto
                {
                    Id = post.Id,
                    content = post.content,
                    Likes = PostLikes,
                    LikeCount = post.Likes.Count(),
                    DatePosted = post.DatePosted,
                    Comments = comments,
                    AuthorId = user.Id,
                    AuthorName = user.DisplayName
                };

                postDtos.Add(postDto);
            }

            return Ok(postDtos);
        }

        //Get Posts of my Friends
        [Authorize]
        [HttpGet("")]
        public async Task<ActionResult<PostDto>> GetPosts()
        {

			var myUser = await _manager.GetUserAddressAsync(User);
			if (myUser is null)
			{
				return Unauthorized(new ApiResponse(401));
			}

			var postDtos = new List<PostDto>();
			var repostDtos = new List<RepostDto>();
			var resultDtos = new List<object>();
			var cachedResultDtos = new List<object>();

			var cachedPostsKey = $"posts - {myUser.Id}";
			var cachedRepostsKey = $"reposts - {myUser.Id}";

			// Try to get the posts and reposts from the cache
			var cachedPosts = await _cacheService.GetStringAsync(cachedPostsKey);
			var cachedReposts = await _cacheService.GetStringAsync(cachedRepostsKey);

			if (cachedPosts != null || cachedReposts != null)
			{
				var cachedPostDtos = JsonConvert.DeserializeObject<List<PostDto>>(cachedPosts);
				var cachedRepostDtos = JsonConvert.DeserializeObject<List<RepostDto>>(cachedReposts);

				cachedResultDtos.AddRange(cachedPostDtos);
				cachedResultDtos.AddRange(cachedRepostDtos);

				return Ok(cachedResultDtos);
			}

			
            var currUser = await _manager.GetUserAddressAsync(User);
            var specFriendUserId = new BaseSpecifications<AppUserFriend>(u => u.UserId == currUser.Id);
            var specFriendFriendId = new BaseSpecifications<AppUserFriend>(u => u.FriendId == currUser.Id);
            var friendsByUserId = await _unitOfWork.Repository<AppUserFriend>().GetAllWithSpecAsync(specFriendUserId);
            var friendsByFriendId = await _unitOfWork.Repository<AppUserFriend>().GetAllWithSpecAsync(specFriendFriendId);
            //b3d kda hangeb el posts ely el author id bta3hom 3aks ba3d
            var postList = new List<Post>();
			var repostList = new List<Repost>();


			foreach (var friendByUserId in friendsByUserId)
			{
				var postSpec = new PostWithCommentSpecs(friendByUserId.FriendId);
				var posts = await _unitOfWork.Repository<Post>().GetAllWithSpecAsync(postSpec);
				postList.AddRange(posts);

				var repostSpec = new RepostWithCommentSpecs(friendByUserId.FriendId);
				var reposts = await _unitOfWork.Repository<Repost>().GetAllWithSpecAsync(repostSpec);
				repostList.AddRange(reposts);
			}

			foreach (var friendByFriendId in friendsByFriendId)
			{
				var postSpec = new PostWithCommentSpecs(friendByFriendId.UserId);
				var posts = await _unitOfWork.Repository<Post>().GetAllWithSpecAsync(postSpec);
				postList.AddRange(posts);

				var repostSpec = new RepostWithCommentSpecs(friendByFriendId.UserId);
				var reposts = await _unitOfWork.Repository<Repost>().GetAllWithSpecAsync(repostSpec);
				repostList.AddRange(reposts);
			}
			if (!postList.Any() && !repostList.Any())
			{
				return NotFound("No posts or reposts found");
			}

			foreach (var post in postList)
			{
				var user = await _manager.GetUserByIdAsync(post.AuthorId);
				if (user == null)
				{
					return NotFound($"User not found for post with ID: {post.Id}");
				}
                if(post.ReportCount == 10)
                {
                    continue;
                }
				var comments = _mapper.Map<ICollection<Comment>, ICollection<CommentDto>>(post.Comments);
				var postLikes = _mapper.Map<ICollection<PostLikes>, ICollection<PostLikesDto>>(post.Likes);
                var PostImages = _mapper.Map<ICollection<FileNames>, ICollection<FileNameDto>>(post.FileName);

                var postDto = new PostDto
				{
					Id = post.Id,
					content = post.content,
					Likes = postLikes,
					LikeCount = post.Likes.Count(),
					DatePosted = post.DatePosted,
					Comments = comments,
					AuthorId = user.Id,
					AuthorName = user.DisplayName,
                    UploadedFileNames = PostImages

                };

				postDtos.Add(postDto);
			}
			foreach (var repost in repostList)
			{
				var user = await _manager.GetUserByIdAsync(repost.AuthorId);
				if (user == null)
				{
					return NotFound($"User not found for repost with ID: {repost.Id}");
				}

				var comments = _mapper.Map<ICollection<Comment>, ICollection<CommentDto>>(repost.Comments);
				var repostLikes = _mapper.Map<ICollection<RepostLikes>, ICollection<RepostLikesDto>>(repost.Likes);
                var postsOfReposts = _mapper.Map<Post, PostDto>(repost.Post);
                var repostDto = new RepostDto
                {
                    Id = repost.Id,
                    content = repost.content,
                    Likes = repostLikes,
                    LikeCount = repost.Likes.Count(),
                    DatePosted = repost.DatePosted,
                    Comments = comments,
                    AuthorId = user.Id,
                    AuthorName = user.DisplayName,
                    PostId = repost.PostId,
                    Post = postsOfReposts
				};

				repostDtos.Add(repostDto);
			}



			// Cache the result
			await _cacheService.SetStringAsync(cachedPostsKey, JsonConvert.SerializeObject(postDtos));
			await _cacheService.SetStringAsync(cachedRepostsKey, JsonConvert.SerializeObject(repostDtos));

			return Ok(resultDtos);
		}




		//Get Post by id
		[Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetPost(int id)
        {
            var myUser = await _manager.GetUserAddressAsync(User);
            if (myUser is null)
            {
                return Unauthorized(new ApiResponse(401));
            }
            //get by id in specs
            var spec = new PostWithCommentSpecs(id);
            var post = await _unitOfWork.Repository<Post>().GetEntityWithSpecAsync(spec);
            var blockSpec = new BaseSpecifications<BlockList>(u => u.BlockedId == myUser.Id && u.UserId == post.AuthorId);
            var isBlocked = await _unitOfWork.Repository<BlockList>().GetEntityWithSpecAsync(blockSpec);
            if (isBlocked is not null)
            {
                return BadRequest("You are Blocked");
            }
            if (post == null)
            {
                return BadRequest("No posts");
            }
            if (post.ReportCount == 10)
            {
                return NotFound();
            }
            var author = await _manager.GetUserByIdAsync(post.AuthorId);
            if (author == null)
            {
                return NotFound("User not found");
            }

            var comments = _mapper.Map<ICollection<Comment>, ICollection<CommentDto>>(post.Comments);
            var PostLikes = _mapper.Map<ICollection<PostLikes>, ICollection<PostLikesDto>>(post.Likes);
            var PostImages = _mapper.Map<ICollection<FileNames>, ICollection<FileNameDto>>(post.FileName);
            var postDto = new PostDto
            {
                Id = post.Id,
                content = post.content,
                LikeCount = post.Likes.Count(),
                Likes = PostLikes,
                DatePosted = post.DatePosted,
                Comments = comments,
                AuthorId = author.Id,
                AuthorName = author.DisplayName,
                UploadedFileNames = PostImages
            };

            return Ok(postDto);
        }

        //Post Request
        [Authorize]
        [HttpPost("")]
        public async Task<ActionResult<PostDto>> CreatePost([FromForm]PostDto newPost)
        {
            var user = await _manager.GetUserAddressAsync(User);
            if (user is null)
                return Unauthorized(new ApiResponse(401));

            string uploadedFileName = null;

            
            var post = _mapper.Map<PostDto, Post>(newPost);
            if (post is null)
            {
                return BadRequest(new ApiResponse(400));
            }
            post.AuthorId = user.Id;
            post.Comments = null;

            var result = _unitOfWork.Repository<Post>().Add(post);
            _unitOfWork.Repository<Post>().SaveChanges();
            if (!result.IsCompletedSuccessfully)
                return BadRequest(new ApiResponse(400));
            var mappedFileNames = new List<FileNames>();
            if (newPost.UploadedFiles != null && newPost.UploadedFiles.Count > 1) // Handle multiple file upload
            {
                var uploadedFileNames = await _uploadService.UploadFilesAsync(newPost.UploadedFiles,"Posts");
                foreach (var file in uploadedFileNames) {
                    mappedFileNames.Add(new FileNames { FileName = file, PostId = post.Id, Post = post });
                }
            }
            foreach (var file in mappedFileNames)
            {
            var resultFile = _unitOfWork.Repository<FileNames>().Add(file);
            _unitOfWork.Repository<FileNames>().SaveChanges();
             if (!resultFile.IsCompletedSuccessfully)
                   return BadRequest(new ApiResponse(400));
            }
            if (newPost.mentions is not null)
			{
				foreach (var mention in newPost.mentions)
				{
					var mentionedUser = _manager.Users.Where(u => u.UserName == mention).FirstOrDefault();


					var newNotification = new NotificationDto
					{
						content = $"The User {user.UserName} mentioned you in a post ",
						userId = mentionedUser.Id,
						type = "Friend Mention",
					};

					var mappedNotification = _mapper.Map<NotificationDto, Notification>(newNotification);

					// Check if _mentionNotification is not null before sending the notification
					if (_mentionNotification != null)
					{
						// send notification to the user 
						await _mentionNotification.Clients.All.SendNotification(mappedNotification);
					}

					// Check if _repositoryNotification is not null before adding the notification
					if (_unitOfWork.Repository<Notification>() != null)
					{
						// save notification in db
						await _unitOfWork.Repository<Notification>().Add(mappedNotification);
                        _unitOfWork.Repository<Notification>().SaveChanges();
					}
				}
			}


			return Ok(post);

        }
        [Authorize]
        [HttpPut("report-post/{id}")]
        public async Task<ActionResult<PostDto>> ReportPost(int id)
        {
            var user = await _manager.GetUserAddressAsync(User);
            var post = await _unitOfWork.Repository<Post>().GetEntityWithSpecAsync(new BaseSpecifications<Post>(P => P.Id == id));
            if (user is null)
                return Unauthorized(new ApiResponse(401));
            if (post is null)
                return BadRequest(new ApiResponse(404));
            post.ReportCount++;
            _unitOfWork.Repository<Post>().Update(post);
            _unitOfWork.Repository<Post>().SaveChanges();
            return Ok("post reported");
        }

        //Put Request 
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<PostDto>> UpdatePost(PostDto newPost, int id)
        {
            var user = await _manager.GetUserAddressAsync(User);
            var oldPost = await _unitOfWork.Repository<Post>().GetEntityWithSpecAsync(new BaseSpecifications<Post>(P => P.Id == id));
            if (user is null)
                return Unauthorized(new ApiResponse(401));

            if (oldPost is null)
                return BadRequest(new ApiResponse(404));


            if (oldPost.AuthorId != user.Id)
                return Unauthorized(new ApiResponse(401));

            oldPost.content = newPost.content;
            _unitOfWork.Repository<Post>().Update(oldPost);
            _unitOfWork.Repository<Post>().SaveChanges();

            return Ok(oldPost);
        }

        //Delete Post
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<PostDto>> DeletePost(int id)
        {
            //get by id in specs
            var spec = new BaseSpecifications<Post>(P => P.Id == id);
            var post = await _unitOfWork.Repository<Post>().GetEntityWithSpecAsync(spec);
            var user = await _manager.GetUserAddressAsync(User);

            if (user is null)
                return Unauthorized(new ApiResponse(401));

            if (post == null)
                return BadRequest($"No post found with that id : {id}");

            if (post.AuthorId != user.Id)
                return Unauthorized(new ApiResponse(401));

            if (post.Comments != null)
            {
                foreach (Comment comment in post.Comments)
                {
                    _unitOfWork.Repository<Comment>().Delete(comment);
                }
            }
            _unitOfWork.Repository<Post>().Delete(post);
            _unitOfWork.Repository<Post>().SaveChanges();

            return Ok("Post deleted");
        }
        
        //Like or Dislike Post
        [Authorize]
        [HttpPut("LikePost/{PostId}")]
        public async Task<ActionResult<PostDto>> LikePost(int PostId)
        {
            var user = await _manager.GetUserAddressAsync(User);
            var oldPost = await _unitOfWork.Repository<Post>().GetEntityWithSpecAsync(new BaseSpecifications<Post>(P => P.Id == PostId));
            if (user is null)
                return Unauthorized(new ApiResponse(401));

            if (oldPost is null)
                return BadRequest(new ApiResponse(404));

            var spec = new BaseSpecifications<PostLikes>(P => P.PostId == PostId);
            var postLikes = await _unitOfWork.Repository<PostLikes>().GetAllWithSpecAsync(spec);


            var isLiked = postLikes.Where(L => L.userId == user.Id).FirstOrDefault();

            if (isLiked is null)
            {
                var newPostLikes = new PostLikesDto { userId= user.Id, PostId=  PostId, userName= user.DisplayName };
                var mappedPostLikes = _mapper.Map<PostLikesDto, PostLikes > (newPostLikes);
                await _unitOfWork.Repository<PostLikes>().Add(mappedPostLikes);
                _unitOfWork.Repository<PostLikes>().SaveChanges();
                // hena ba3ml retreive ll post b3d el update
                var specLikes = new PostWithCommentSpecs(PostId);
                var post = await _unitOfWork.Repository<Post>().GetEntityWithSpecAsync(specLikes);
                var retPostLikes = _mapper.Map<ICollection<PostLikes>, ICollection<PostLikesDto>>(post.Likes);
                var response = new { message = "Dislike", likeCount = retPostLikes.Count(), likes = retPostLikes };
                
                return Ok(System.Text.Json.JsonSerializer.Serialize(response));
            }
            else
            {
                _unitOfWork.Repository<PostLikes>().Delete(isLiked);
                _unitOfWork.Repository<PostLikes>().SaveChanges();
                // hena ba3ml retreive ll post b3d el update w 3ayz avalidate hena la2no ka by3ml remove fa momken yrg3lo zero 
                var specLikes = new PostWithCommentSpecs(PostId);
                var post = await _unitOfWork.Repository<Post>().GetEntityWithSpecAsync(specLikes);
                var retPostLikes = _mapper.Map<ICollection<PostLikes>, ICollection<PostLikesDto>>(post?.Likes);

                var response = new { message = "Like", likeCount = retPostLikes.Count(), likes = retPostLikes };

				return Ok(System.Text.Json.JsonSerializer.Serialize(response));
            }

        }

		//Post Request
		[Authorize]
		[HttpPost("Repost")]
		public async Task<ActionResult<RepostDto>> CreateRepost(RepostDto newRepost)
		{
			var user = await _manager.GetUserAddressAsync(User);
			if (user is null)
				return Unauthorized(new ApiResponse(401));


			var repost = _mapper.Map<RepostDto, Repost>(newRepost);
			repost.AuthorId = user.Id;
            repost.AuthorName = user.DisplayName;
			repost.Comments = null;
			repost.DatePosted = DateTime.Now;
            repost.Post = null;

			if (repost is null)
			{
				return BadRequest(new ApiResponse(400));
			}
			var result = _unitOfWork.Repository<Repost>().Add(repost);
            _unitOfWork.Repository<Repost>().SaveChanges();
			if (!result.IsCompletedSuccessfully)
				return BadRequest(new ApiResponse(400));

			var repostDto = _mapper.Map<RepostDto>(repost);
			return Ok(repost);

		}
	}
}