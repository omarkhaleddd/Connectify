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
        private readonly IGenericRepository<PostLikes> _repositoryPostLikes;

        public PostController(IMapper mapper, UserManager<AppUser> manager, IGenericRepository<Post> genericRepository, IGenericRepository<Comment> genericRepository2 , IGenericRepository<PostLikes> genericRepository3)
        {
            _mapper = mapper;
            _manager = manager;
            _repositoryPost = genericRepository;
            _repositoryComment = genericRepository2;
            _repositoryPostLikes = genericRepository3;

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
            //get by id in specs
            var spec = new PostWithCommentSpecs(authorId);
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
                    DatePosted = post.DatePosted,
                    Comments = comments,
                    AuthorId = user.Id,
                    AuthorName = user.DisplayName
                };

                postDtos.Add(postDto);
            }

            return Ok(postDtos);
        }

        //Get Posts
        [Authorize]
        [HttpGet("")]
        public async Task<ActionResult<PostDto>> GetPosts()
        {
            var spec = new PostWithCommentSpecs();
            var posts = await _repositoryPost.GetAllWithSpecAsync(spec);
            if (posts == null || !posts.Any())
            {
                return NotFound("No posts found");
            }

            var postDtos = new List<PostDto>();
            foreach (var post in posts)
            {
                var user = await _manager.GetUserByIdAsync(post.AuthorId);
                if (user == null)
                {
                    return NotFound($"User not found for post with ID: {post.Id}");
                }

                var comments = _mapper.Map<ICollection<Comment>, ICollection<CommentDto>>(post.Comments);
                var PostLikes = _mapper.Map<ICollection<PostLikes>, ICollection<PostLikesDto>>(post.Likes);

                var postDto = new PostDto
                {
                    Id = post.Id,
                    content = post.content,
                    Likes = PostLikes,
                    DatePosted = post.DatePosted,
                    Comments = comments,
                    AuthorId = user.Id,
                    AuthorName = user.DisplayName
                };

                postDtos.Add(postDto);
            }

            return Ok(postDtos);
        }



        //Get Post by id
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetPost(int id)
        {
            //get by id in specs
            var spec = new PostWithCommentSpecs(id);
            var post = await _repositoryPost.GetEntityWithSpecAsync(spec);
            if (post == null)
            {
                return BadRequest("No posts");
            }

            var author = await _manager.GetUserByIdAsync(post.AuthorId);
            if (author == null)
            {
                return NotFound("User not found");
            }

            var comments = _mapper.Map<ICollection<Comment>, ICollection<CommentDto>>(post.Comments);
            var PostLikes = _mapper.Map<ICollection<PostLikes>, ICollection<PostLikesDto>>(post.Likes);

            var postDto = new PostDto
            {
                Id = post.Id,
                content = post.content,
                Likes = PostLikes,
                DatePosted = post.DatePosted,
                Comments = comments,
                AuthorId = author.Id,
                AuthorName = author.DisplayName
            };

            return Ok(postDto);
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
            if (post is null)
            {
                return BadRequest(new ApiResponse(400));
            }
            var result = _repositoryPost.Add(post);
            _repositoryPost.SaveChanges();
            if (!result.IsCompletedSuccessfully)
                return BadRequest(new ApiResponse(400));

            return Ok(post);

        }

        //Put Request 
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<PostDto>> UpdatePost(PostDto newPost, int id)
        {
            var user = await _manager.GetUserAddressAsync(User);
            var oldPost = await _repositoryPost.GetEntityWithSpecAsync(new BaseSpecifications<Post>(P => P.Id == id));
            if (user is null)
                return Unauthorized(new ApiResponse(401));

            if (oldPost is null)
                return BadRequest(new ApiResponse(404));


            if (oldPost.AuthorId != user.Id)
                return Unauthorized(new ApiResponse(401));

            oldPost.content = newPost.content;
            _repositoryPost.Update(oldPost);
            _repositoryPost.SaveChanges();

            return Ok(oldPost);
        }

        //Delete Post
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<PostDto>> DeletePost(int id)
        {
            //get by id in specs
            var spec = new BaseSpecifications<Post>(P => P.Id == id);
            var post = await _repositoryPost.GetEntityWithSpecAsync(spec);
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
                    _repositoryComment.Delete(comment);
                }
            }
            _repositoryPost.Delete(post);
            _repositoryPost.SaveChanges();

            return Ok("Post deleted");
        }
        //Like Post
        [Authorize]
        [HttpPut("LikePost/{PostId}")]
        public async Task<ActionResult<PostDto>> LikePost(int PostId)
        {
            var user = await _manager.GetUserAddressAsync(User);
            var oldPost = await _repositoryPost.GetEntityWithSpecAsync(new BaseSpecifications<Post>(P => P.Id == PostId));
            if (user is null)
                return Unauthorized(new ApiResponse(401));

            if (oldPost is null)
                return BadRequest(new ApiResponse(404));

            var spec = new BaseSpecifications<PostLikes>(P => P.PostId == PostId);
            var postLikes = await _repositoryPostLikes.GetAllWithSpecAsync(spec);


            var isLiked = postLikes.Where(L => L.userId == user.Id).FirstOrDefault();

            if (isLiked is null)
            {
                var newPostLikes = new PostLikesDto { userId= user.Id, PostId=  PostId };
                var mappedPostLikes = _mapper.Map<PostLikesDto, PostLikes > (newPostLikes);
                await _repositoryPostLikes.Add(mappedPostLikes);
                _repositoryPostLikes.SaveChanges();
                return Ok(true);
            }
            return Ok(false);
        }
    }
}