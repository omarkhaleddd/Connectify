using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using System.Text.Json;
using Talabat.APIs.DTO;
using Talabat.APIs.Errors;
using Talabat.APIs.Exstentions;
using Talabat.Core.Entities.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : APIBaseController
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _manager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IGenericRepository<Post> _repositoryPost;
        private readonly IGenericRepository<AppUserFriend> _repositoryFriend;

        public AccountsController(IMapper mapper, UserManager<AppUser> manager, IGenericRepository<Post> genericRepository,IGenericRepository<AppUserFriend> genericRepository1, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            _mapper = mapper;
            _manager = manager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _repositoryPost = genericRepository;
            _repositoryFriend = genericRepository1;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {

            if (checkDuplicateEmail(model.Email).Result.Value)
                return BadRequest(new ApiResponse(400, "Email ALready Exist"));

            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber
            };
            var result = await _manager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            var returnedUser = new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _manager)

            };
            return Ok(returnedUser);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _manager.FindByEmailAsync(model.Email);
            if (user is null) return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _manager)
            });

        }
        [Authorize]
		[HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _manager.FindByEmailAsync(Email);
            var ReturnedUser = new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _manager)
            };
            return Ok(ReturnedUser);
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {

            var user = await _manager.GetUserAddressAsync(User);
            if (user == null) return NotFound("user null");
            var mappedUser = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(mappedUser);

        }

		[Authorize]
		[HttpPost("Address")]
		public async Task<ActionResult<AddressDto>> CreateAddress(AddressDto newAddress)
		{
			var user = await _manager.GetUserAddressAsync(User); 
			if (user is null)
				return Unauthorized(new ApiResponse(401));

			var address = _mapper.Map<AddressDto, Address>(newAddress);
			user.Address = address;

			var result = await _manager.UpdateAsync(user); 

			if (!result.Succeeded)
				return BadRequest(new ApiResponse(400));

			//var createdAddressDto = _mapper.Map<Address, AddressDto>(user.Address);
			return Ok(newAddress);
		}

		[Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto updatedAddress)
        {
           
            var user = await _manager.GetUserAddressAsync(User);
            if (user is null)
                return Unauthorized(new ApiResponse(401));
            var address =_mapper.Map<AddressDto, Address>(updatedAddress);
            address.Id = user.Address.Id;
            user.Address = address;
            var result =await _manager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(updatedAddress);
        }

		[Authorize]
		[HttpPut("UpdateProfile")]
		public async Task<ActionResult<UserDto>> UpdateProfile(UserDto updateUser)
		{
            //example of how to use get user main async
            var userId = await _manager.GetUserAddressAsync(User);
            
			var user = await _manager.GetUserAddressAsync(User);
            if (user is null)
                return Unauthorized(new ApiResponse(401, $" ggg {user}"));
            var appuser = _mapper.Map<UserDto, AppUser>(updateUser);
            user.DisplayName = updateUser.DisplayName;
            user.Id = updateUser.Id;
			var result = await _manager.UpdateAsync(user);
			if (!result.Succeeded) return BadRequest(new ApiResponse(400));
			return Ok(updateUser);
		}
		[HttpGet("EmailExist")]
        public async Task<ActionResult<bool>> checkDuplicateEmail(string email)
        {
            var user =await _manager.FindByEmailAsync(email);
            if (user is null) return false;
            else return true;
        }

        [HttpGet("Search/{DisplayName}")]
        public async Task<ActionResult<ICollection<UserDto>>> GetUsersWithNames(string DisplayName)
        {
            var UserStartingWithPrefix = await _manager.Users
                .Where(N => N.DisplayName.StartsWith(DisplayName))
                .ToListAsync();
            if(UserStartingWithPrefix.Count == 0)
            {
                return NotFound("User not found");
            }
            var mappedUsers = _mapper.Map<List<AppUser>, List<AppUserDto>>(UserStartingWithPrefix);
            return Ok(mappedUsers);
        }
        [HttpGet("User/{id}")]
        public async Task<ActionResult<UserDto>> GetCurrentUser(string id)
        {
            var user = await _manager.GetUserByIdAsync(id);
            if (user is null) 
                return NotFound(new ApiResponse(404));
            var spec = new PostWithCommentSpecs(id);
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
            var ReturnedUser = new
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                posts = postDtos
            };
            return Ok(ReturnedUser);
        }

        //GetFriends
        [Authorize]
        [HttpGet("Friends")]
        public async Task<ActionResult<FriendDto>> GetFriends()
        {
            var user = await _manager.GetUserAddressAsync(User);
            var spec = new BaseSpecifications<AppUserFriend>(u => u.UserId == user.Id );
            var friends = await _repositoryFriend.GetAllWithSpecAsync(spec);
            if (friends == null || !friends.Any())
            {
                return NotFound("No Friends found");
            }
            var mappedFriends = _mapper.Map<List<AppUserFriend>, List<FriendDto>>(friends.ToList());
            return Ok(mappedFriends);
        }
        //AddFriend
        [Authorize]
        [HttpPost("AddFriend/{id}")]
        public async Task<ActionResult<AppUserFriend>> AddFriend(string id)
        {
            var user = await _manager.GetUserAddressAsync(User);
            if(id == user.Id)
            {
                return BadRequest();
            }
            var spec = new BaseSpecifications<AppUserFriend>(u => u.UserId == user.Id);
            var friends = await _repositoryFriend.GetAllWithSpecAsync(spec);
            var isFriend = friends.Where(f => f.FriendId == id).FirstOrDefault();
            if(isFriend is null)
            {
                var newFriend = new FriendDto { UserId = user.Id , FriendId = id };
                var mappedNewFriend = _mapper.Map<FriendDto,AppUserFriend>(newFriend);
                await _repositoryFriend.Add(mappedNewFriend);
                _repositoryFriend.SaveChanges();
                var updatedFriends = await _repositoryFriend.GetAllWithSpecAsync(spec);
                var result = new { message = "Friend Added" , Friends = updatedFriends };
                return Ok(JsonSerializer.Serialize(result));
            }
            else
            {
                _repositoryFriend.Delete(isFriend);
                _repositoryFriend.SaveChanges();
                var updatedFriends = await _repositoryFriend.GetAllWithSpecAsync(spec);
                var result = new { message = "Friend Deleted", Friends = updatedFriends };
                return Ok(JsonSerializer.Serialize(result));
            }
        }
    }
}
