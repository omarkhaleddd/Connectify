using AutoMapper;
using Connetify.APIs.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
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
        private readonly IGenericRepository<FriendRequest> _repositoryFriendRequest;
        private readonly IGenericRepository<BlockList> _repositoryBlock;
        private readonly IGenericRepository<AppUserFriend> _repositoryFriend;


        public AccountsController(IMapper mapper, UserManager<AppUser> manager, IGenericRepository<Post> genericRepository, IGenericRepository<AppUserFriend> genericRepository1, IGenericRepository<FriendRequest> genericRepository2,IGenericRepository<BlockList> genericRepository3, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            _mapper = mapper;
            _manager = manager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _repositoryPost = genericRepository;
            _repositoryFriend = genericRepository1;
            _repositoryFriendRequest = genericRepository2;
            _repositoryBlock = genericRepository3;
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
            var address = _mapper.Map<AddressDto, Address>(updatedAddress);
            address.Id = user.Address.Id;
            user.Address = address;
            var result = await _manager.UpdateAsync(user);
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
            var user = await _manager.FindByEmailAsync(email);
            if (user is null) return false;
            else return true;
        }

        [HttpGet("Search/{DisplayName}")]
        public async Task<ActionResult<ICollection<UserDto>>> Search(string DisplayName)
        {
            var user = await _manager.GetUserAddressAsync(User);
            if (user is null)
            {
                return Unauthorized(new ApiResponse(401));
            }
            //check if the this user added me in the blockList
            var blockSpec = new BaseSpecifications<BlockList>(u => u.BlockedId == user.Id);
            var blockedMe = await _repositoryBlock.GetAllWithSpecAsync(blockSpec);
            var UserStartingWithPrefix = await _manager.Users
                .Where(N => N.DisplayName.StartsWith(DisplayName))
                .ToListAsync();
            if (UserStartingWithPrefix.Count == 0)
            {
                return NotFound("User not found");
            }
            foreach (var oneBlockedMe in blockedMe)
            {
                var isBlocked = UserStartingWithPrefix.Find(u => u.Id == oneBlockedMe.UserId);
                if (isBlocked is not null)
                {
                    UserStartingWithPrefix.Remove(isBlocked);
                }
            }
            var isMe = UserStartingWithPrefix.Find(u => u.Id == user.Id);
            if (isMe is not null)
            {
                UserStartingWithPrefix.Remove(isMe);
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
            //check if the this user added me in the blockList
            var blockSpec = new BaseSpecifications<BlockList>(u => u.BlockedId == user.Id && u.UserId == id);
            var isBlocked = await _repositoryBlock.GetEntityWithSpecAsync(blockSpec);
            if(isBlocked is not null)
            {
                return BadRequest("You are Blocked");
            }
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
            if (user is null)
            {
                return Unauthorized(new ApiResponse(401));
            }
            var spec = new BaseSpecifications<AppUserFriend>(u => u.UserId == user.Id || u.FriendId == user.Id);
            var friends = await _repositoryFriend.GetAllWithSpecAsync(spec);
            if (friends == null || !friends.Any())
            {
                return NotFound("No Friends found");
            }
            var mappedFriends = _mapper.Map<List<AppUserFriend>, List<FriendDto>>(friends.ToList());
            return Ok(mappedFriends);
        }
        //Show BlockList 
        [Authorize]
        [HttpGet("BlockList")]
        public async Task<ActionResult<AppUserFriend>> GetBlockList()
        {
            var user = await _manager.GetUserAddressAsync(User);
            if (user is null)
            {
                return Unauthorized(new ApiResponse(401));
            }
            var spec = new BaseSpecifications<BlockList>(u => u.UserId == user.Id);
            var blocks = await _repositoryBlock.GetAllWithSpecAsync(spec);
            if(blocks is null)
            {
                return NotFound();
            }
            var blockList = _mapper.Map< List < BlockList> ,List<BlockListDto>>(blocks.ToList());
            return Ok(blockList);
        }
        //Block / UnBlock User
        [Authorize]
        [HttpPost("Block/{id}")]
        public async Task<ActionResult<AppUserFriend>> Block(string id)
        {
            var user = await _manager.GetUserAddressAsync(User);
            if (user is null)
            {
                return Unauthorized(new ApiResponse(401));
            }
            if (id == user.Id)
            {
                return BadRequest();
            }

            var baseSpec = new BaseSpecifications<BlockList>();
            var spec = new BaseSpecifications<BlockList>(u => u.BlockedId == user.Id && u.UserId == id);
            var isBlocked = await _repositoryBlock.GetEntityWithSpecAsync(spec);
            if (isBlocked is not null)
            {
                return BadRequest("This user already blocked you");
            }
            var specBlock = new BaseSpecifications<BlockList>(u => u.BlockedId == id && u.UserId == user.Id);
            var alreadyBlocked = await _repositoryBlock.GetEntityWithSpecAsync(specBlock);
            if(alreadyBlocked is null)
            {
                var blockedUser = new BlockListDto { UserId = user.Id, BlockedId = id };
                var mappedUser = _mapper.Map<BlockListDto, BlockList>(blockedUser);
                await _repositoryBlock.Add(mappedUser);
                _repositoryBlock.SaveChanges();
                var friendSpec = new BaseSpecifications<AppUserFriend>(u => (u.FriendId == user.Id && u.UserId == id) ||(u.UserId == user.Id && u.FriendId == id));
                var isFriend = await _repositoryFriend.GetEntityWithSpecAsync(friendSpec);
                if (isFriend is not null)
                {
                    _repositoryFriend.Delete(isFriend);
                    _repositoryFriend.SaveChanges();
                }
                var friendReqSpec = new BaseSpecifications<FriendRequest>(u => (u.SenderId == user.Id && u.Recieverid == id) || (u.Recieverid == user.Id && u.SenderId == id));
                var isFriendReq = await _repositoryFriendRequest.GetEntityWithSpecAsync(friendReqSpec);
                if (isFriendReq is not null)
                {
                    _repositoryFriendRequest.Delete(isFriendReq);
                    _repositoryFriendRequest.SaveChanges();
                }
                var updatedBlockList = await _repositoryBlock.GetAllWithSpecAsync(baseSpec);
                var sentResult = new { message = "unBlock", BlockList = updatedBlockList };
                return Ok(JsonSerializer.Serialize(sentResult));
            }
            else
            {
                _repositoryBlock.Delete(alreadyBlocked);
                _repositoryBlock.SaveChanges();
                var updatedBlockList = await _repositoryBlock.GetAllWithSpecAsync(baseSpec);
                var sentResult = new { message = "block", BlockList = updatedBlockList };
                return Ok(JsonSerializer.Serialize(sentResult));
            }

        }
        //CheckFriendRequest
        [Authorize]
        [HttpGet("CheckBlocked/{id}")]
        public async Task<ActionResult<AppUserFriend>> CheckBlocked(string id)
        {
            var user = await _manager.GetUserAddressAsync(User);
            if (user is null)
            {
                return Unauthorized(new ApiResponse(401));
            }
            if (id == user.Id)
            {
                return BadRequest();
            }

            var blockedUserReqSpec = new BaseSpecifications<BlockList>(u => u.UserId == user.Id && u.BlockedId == id);
            var blockedUserRequest = await _repositoryBlock.GetEntityWithSpecAsync(blockedUserReqSpec);

            if (blockedUserRequest is not null)
            {
                var result = new { message = true };
                return Ok(JsonSerializer.Serialize(result));
            }

            var sentResult = new { message = false };
            return Ok(JsonSerializer.Serialize(sentResult));
        }
        //SendFriendRequest
        [Authorize]
        [HttpPost("SendFriendRequest/{id}")]
        public async Task<ActionResult<AppUserFriend>> SendFriendRequest(string id)
        {
            var user = await _manager.GetUserAddressAsync(User);
            if (user is null)
            {
                return Unauthorized(new ApiResponse(401));
            }
            if (id == user.Id)
            {
                return BadRequest();
            }
            //check if the this user added me in the blockList
            var blockSpec = new BaseSpecifications<BlockList>(u => u.BlockedId == user.Id && u.UserId == id);
            var isBlocked = await _repositoryBlock.GetEntityWithSpecAsync(blockSpec);
            if (isBlocked is not null)
            {
                return BadRequest("You are Blocked");
            }

            var spec = new BaseSpecifications<AppUserFriend>(u => u.UserId == user.Id || u.FriendId == user.Id);
            var friends = await _repositoryFriend.GetAllWithSpecAsync(spec);
            var isFriend = friends.Where(f => f.FriendId == id || f.UserId == id).FirstOrDefault();

            var receivedFriendReqSpec = new BaseSpecifications<FriendRequest>(u => u.Recieverid == user.Id && u.SenderId == id);
            var receivedFriendRequest = await _repositoryFriendRequest.GetEntityWithSpecAsync(receivedFriendReqSpec);

            var sentFriendReqSpec = new BaseSpecifications<FriendRequest>(u => u.SenderId == user.Id && u.Recieverid == id);
            var sentFriendRequest = await _repositoryFriendRequest.GetEntityWithSpecAsync(sentFriendReqSpec);



            if (sentFriendRequest is not null)
            {
                _repositoryFriendRequest.Delete(sentFriendRequest);
                _repositoryFriendRequest.SaveChanges();
                var updatedFriendRequests = await _repositoryFriendRequest.GetAllWithSpecAsync(sentFriendReqSpec);
                var result = new { message = "Removed", FriendRequests = updatedFriendRequests };
                return Ok(JsonSerializer.Serialize(result));
            }

			if (receivedFriendRequest is not null)
			{
				var result = new { message = "Received" };
				return Ok(JsonSerializer.Serialize(result));
			}

            if (isFriend is not null)
            {
				var result = new { message = "Friend" };
				return Ok(JsonSerializer.Serialize(result));
			}


            var newFriendRequest = new FriendRequestDto { SenderId = user.Id, Recieverid = id };
            var mappedNewFriendRequest = _mapper.Map<FriendRequestDto, FriendRequest>(newFriendRequest);
            await _repositoryFriendRequest.Add(mappedNewFriendRequest);
            _repositoryFriendRequest.SaveChanges();
            var updatedFriendRequestsAdd = await _repositoryFriendRequest.GetAllWithSpecAsync(sentFriendReqSpec);
            var sentResult = new { message = "Sent", FriendRequests = updatedFriendRequestsAdd };
            return Ok(JsonSerializer.Serialize(sentResult));
        
       
    }
        //CheckFriendRequest
		[Authorize]
		[HttpGet("CheckFriendRequestFromUser/{id}")]
		public async Task<ActionResult<AppUserFriend>> CheckFriendRequestFromUser(string id)
		{
			var user = await _manager.GetUserAddressAsync(User);
            if (user is null)
            {
                return Unauthorized(new ApiResponse(401));
            }
            if (id == user.Id)
			{
				return BadRequest();
			}

			var receivedFriendReqSpec = new BaseSpecifications<FriendRequest>(u => u.Recieverid == user.Id && u.SenderId == id);
			var receivedFriendRequest = await _repositoryFriendRequest.GetEntityWithSpecAsync(receivedFriendReqSpec);

			if (receivedFriendRequest is not null)
			{
				var result = new { message = true };
				return Ok(JsonSerializer.Serialize(result));
			}

			var sentResult = new { message = false };
			return Ok(JsonSerializer.Serialize(sentResult));
		}

		//Accept/Reject-Friend-Req
		[Authorize]
		[HttpPost("FriendState/{id}")]
		public async Task<ActionResult<FriendDto>> FriendState(string id, [FromBody] StateDto stateDto)
		{
			var user = await _manager.GetUserAddressAsync(User);
			var spec = new BaseSpecifications<FriendRequest>(u => u.Recieverid == user.Id);
			var requests = await _repositoryFriendRequest.GetAllWithSpecAsync(spec);
			var requestById = requests.Where(u => u.SenderId == id).FirstOrDefault();
			if (requestById is null)
			{
				return NotFound(404);
			}
			if (stateDto.State == 0) // reject
			{
				_repositoryFriendRequest.Delete(requestById);
				_repositoryFriendRequest.SaveChanges();
				var result = new { message = "Rejected" };
				return Ok(JsonSerializer.Serialize(result));
			}
			else if (stateDto.State == 1)
			{// accept
				_repositoryFriendRequest.Delete(requestById);
				_repositoryFriendRequest.SaveChanges();
                var Friend = await _manager.GetUserByIdAsync(id);
                var friend = new AppUserFriend { UserId = requestById.SenderId,UserName = user.DisplayName , FriendId = requestById.Recieverid, FriendName = Friend.DisplayName };

				await _repositoryFriend.Add(friend);
				_repositoryFriend.SaveChanges();
				var result = new { message = "Accepted" };
				return Ok(JsonSerializer.Serialize(result));
			}
			else
			{
				return BadRequest();
			}
		}
		[Authorize]
		[HttpGet("FriendRequestsRecieved")]
		public async Task<ActionResult<IEnumerable<FriendRequest>>> FriendRequestsRecieved()
		{
			var user = await _manager.GetUserAddressAsync(User);
			var spec = new BaseSpecifications<FriendRequest>(u => u.Recieverid == user.Id);
			var requests = await _repositoryFriendRequest.GetAllWithSpecAsync(spec);

			if (requests.Any())
			{
				return Ok(requests);
			}
			else
			{
				return NotFound("No Requests Received found");
			}
		}
		[Authorize]
		[HttpGet("FriendRequestsSent")]
		public async Task<ActionResult<IEnumerable<FriendRequest>>> FriendRequestsSent()
		{
			var user = await _manager.GetUserAddressAsync(User);
			var spec = new BaseSpecifications<FriendRequest>(u => u.SenderId == user.Id);
			var requests = await _repositoryFriendRequest.GetAllWithSpecAsync(spec);

			if (requests.Any())
			{
				return Ok(requests);
			}
			else
			{
				return NotFound("No Requests Sent found");
			}
		}

	}
}
