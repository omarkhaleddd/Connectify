using AutoMapper;
using Connetify.APIs.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Stripe;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
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
    public class AccountsController : APIBaseController
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _manager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHubContext<AccountNotificationHub, INotificationHub> _accountNotification;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUploadService _uploadService;

		public AccountsController(IMapper mapper,IUploadService uploadService, UserManager<AppUser> manager,IUnitOfWork unitOfWork, IGenericRepository<Post> genericRepository, IGenericRepository<AppUserFriend> genericRepository1, IGenericRepository<FriendRequest> genericRepository2, IGenericRepository<BlockList> genericRepository3, IGenericRepository<Notification> genericRepository4, SignInManager<AppUser> signInManager, ITokenService tokenService, IHubContext<AccountNotificationHub, INotificationHub> accountNotification, IEmailService emailService)
		{
			_mapper = mapper;
			_manager = manager;
			_signInManager = signInManager;
			_tokenService = tokenService;
            _unitOfWork = unitOfWork;
			_accountNotification = accountNotification;
			_emailService = emailService;
            _uploadService = uploadService;
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
                PhoneNumber = model.PhoneNumber,
                ProfileImageUrl = "defaultProfileImage.jpg",
                CoverImageUrl = "defaultCoverImage.jpg"
            };
            var result = await _manager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            var returnedUser = new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _manager)

            };
			_emailService.SendEmail(user.Email, "User Regitered");
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
            var mappedUser = _mapper.Map<Core.Entities.Identity.Address, AddressDto>(user.Address);
            return Ok(mappedUser);

        }

        [Authorize]
        [HttpPost("Address")]
        public async Task<ActionResult<AddressDto>> CreateAddress(AddressDto newAddress)
        {
            var user = await _manager.GetUserAddressAsync(User);
            if (user is null)
                return Unauthorized(new ApiResponse(401));

            var address = _mapper.Map<AddressDto, Core.Entities.Identity.Address>(newAddress);
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
            var address = _mapper.Map<AddressDto, Core.Entities.Identity.Address>(updatedAddress);
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
        [Authorize]
        [HttpDelete("DeleteCoverImage")]
        public async Task<ActionResult<UserDto>> DeleteCoverImage()
        {
            //example of how to use get user main async
            var userId = await _manager.GetUserAddressAsync(User);

            var user = await _manager.GetUserAddressAsync(User);
            if (user is null)
                return Unauthorized(new ApiResponse(401, $" ggg {user}"));
            if (user.CoverImageUrl != null)
            {
                if (user.ProfileImageUrl != "defaultCoverImage.jpg")
                    _uploadService.DeleteFile(user.CoverImageUrl, "Users");
            }
            user.CoverImageUrl = "defaultCoverImage.jpg";
            var result = await _manager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(user);
        }
        [Authorize]
        [HttpDelete("DeleteProfileImage")]
        public async Task<ActionResult<UserDto>> DeleteProfileImage()
        {
            //example of how to use get user main async
            var userId = await _manager.GetUserAddressAsync(User);

            var user = await _manager.GetUserAddressAsync(User);
            if (user is null)
                return Unauthorized(new ApiResponse(401, $" ggg {user}"));
            if (user.ProfileImageUrl != null)
            {
                if (user.ProfileImageUrl != "defaultProfileImage.jpg")
                    _uploadService.DeleteFile(user.ProfileImageUrl, "Users");
            }
            user.ProfileImageUrl = "defaultProfileImage.jpg";
            var result = await _manager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(user);
        }
        [Authorize]
        [HttpPatch("UpdateCoverImage")]
        public async Task<ActionResult<UserDto>> UpdateCoverImage([FromForm]IFormFile CoverImage)
        {
            //example of how to use get user main async
            var userId = await _manager.GetUserAddressAsync(User);

            var user = await _manager.GetUserAddressAsync(User);
            if (user is null)
                return Unauthorized(new ApiResponse(401, $" ggg {user}"));
            if(user.CoverImageUrl != null)
            {
                if (user.ProfileImageUrl != "defaultCoverImage.jpg")
                    _uploadService.DeleteFile(user.CoverImageUrl, "Users");
            }
            if (user.CoverImageUrl != "defaultCoverImage.jpg")
                user.CoverImageUrl = await _uploadService.UploadFileAsync(CoverImage, "Users");
            var result = await _manager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(user);
        }
        [Authorize]
        [HttpPatch("UpdateProfileImage")]
        public async Task<ActionResult<UserDto>> UpdateProfileImage([FromForm] IFormFile ProfileImage)
        {          
            //example of how to use get user main async
            var userId = await _manager.GetUserAddressAsync(User);

            var user = await _manager.GetUserAddressAsync(User);
            if (user is null)
                return Unauthorized(new ApiResponse(401, $" ggg {user}"));
            if (user.ProfileImageUrl != null)
            {
                if(user.ProfileImageUrl != "defaultProfileImage.jpg")   
                     _uploadService.DeleteFile(user.ProfileImageUrl, "Users");
            }
            if (user.ProfileImageUrl != "defaultProfileImage.jpg")
                user.ProfileImageUrl = await _uploadService.UploadFileAsync(ProfileImage, "Users");
            var result = await _manager.UpdateAsync(user);
            
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(user);
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
            var blockedMe = await _unitOfWork.Repository<BlockList>().GetAllWithSpecAsync(blockSpec);
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
            var isBlocked = await _unitOfWork.Repository<BlockList>().GetEntityWithSpecAsync(blockSpec);
            if(isBlocked is not null)
            {
                return BadRequest("You are Blocked");
            }
            var spec = new PostWithCommentSpecs(id);
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
            var friendsRaw = await _unitOfWork.Repository<AppUserFriend>().GetAllWithSpecAsync(spec);
            if (friendsRaw == null || !friendsRaw.Any())
            {
                return NotFound("No Friends found");
            }
            var friends = new List<FriendDto>();
            foreach(var friend in friendsRaw)
            {
                var currFriend = new FriendDto
                {
                    FriendId = "",
                    FriendName = ""
                };
                if (user.Id == friend.UserId)
                {
                    currFriend.FriendId = friend.FriendId;
                    currFriend.FriendName = friend.FriendName;
                }
                else
                {
                    currFriend.FriendId = friend.UserId;
                    currFriend.FriendName = friend.UserName;
                }
                friends.Add(currFriend);
            }
            return Ok(friends);
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
            var blocks = await _unitOfWork.Repository<BlockList>().GetAllWithSpecAsync(spec);
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
            var isBlocked = await _unitOfWork.Repository<BlockList>().GetEntityWithSpecAsync(spec);
            if (isBlocked is not null)
            {
                return BadRequest("This user already blocked you");
            }
            var specBlock = new BaseSpecifications<BlockList>(u => u.BlockedId == id && u.UserId == user.Id);
            var alreadyBlocked = await _unitOfWork.Repository<BlockList>().GetEntityWithSpecAsync(specBlock);
            if(alreadyBlocked is null)
            {
                var blockedUser = new BlockListDto { UserId = user.Id, BlockedId = id };
                var mappedUser = _mapper.Map<BlockListDto, BlockList>(blockedUser);
                await _unitOfWork.Repository<BlockList>().Add(mappedUser);
                _unitOfWork.Repository<BlockList>().SaveChanges();
                var friendSpec = new BaseSpecifications<AppUserFriend>(u => (u.FriendId == user.Id && u.UserId == id) ||(u.UserId == user.Id && u.FriendId == id));
                var isFriend = await _unitOfWork.Repository<AppUserFriend>().GetEntityWithSpecAsync(friendSpec);
                if (isFriend is not null)
                {
                    _unitOfWork.Repository<AppUserFriend>().Delete(isFriend);
                    _unitOfWork.Repository<AppUserFriend>().SaveChanges();
                }
                var friendReqSpec = new BaseSpecifications<FriendRequest>(u => (u.SenderId == user.Id && u.Recieverid == id) || (u.Recieverid == user.Id && u.SenderId == id));
                var isFriendReq = await _unitOfWork.Repository<FriendRequest>().GetEntityWithSpecAsync(friendReqSpec);
                if (isFriendReq is not null)
                {
                    _unitOfWork.Repository<FriendRequest>().Delete(isFriendReq);
                    _unitOfWork.Repository<FriendRequest>().SaveChanges();
                }
                var updatedBlockList = await _unitOfWork.Repository<BlockList>().GetAllWithSpecAsync(baseSpec);
                var sentResult = new { message = "unBlock", BlockList = updatedBlockList };
                return Ok(JsonSerializer.Serialize(sentResult));
            }
            else
            {
                _unitOfWork.Repository<BlockList>().Delete(alreadyBlocked);
                _unitOfWork.Repository<BlockList>().SaveChanges();
                var updatedBlockList = await _unitOfWork.Repository<BlockList>().GetAllWithSpecAsync(baseSpec);
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
            var blockedUserRequest = await _unitOfWork.Repository<BlockList>().GetEntityWithSpecAsync(blockedUserReqSpec);

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
                return BadRequest("You are sending friend request to yourself");
            }
            //check if the this user added me in the blockList
            var blockSpec = new BaseSpecifications<BlockList>(u => u.BlockedId == user.Id && u.UserId == id);
            var isBlocked = await _unitOfWork.Repository<BlockList>().GetEntityWithSpecAsync(blockSpec);
            if (isBlocked is not null)
            {
                return BadRequest("You are Blocked");
            }

            var spec = new BaseSpecifications<AppUserFriend>(u => u.UserId == user.Id || u.FriendId == user.Id);
            var friends = await _unitOfWork.Repository<AppUserFriend>().GetAllWithSpecAsync(spec);
            var isFriend = friends.Where(f => f.FriendId == id || f.UserId == id).FirstOrDefault();

            var receivedFriendReqSpec = new BaseSpecifications<FriendRequest>(u => u.Recieverid == user.Id && u.SenderId == id);
            var receivedFriendRequest = await _unitOfWork.Repository<FriendRequest>().GetEntityWithSpecAsync(receivedFriendReqSpec);

            var sentFriendReqSpec = new BaseSpecifications<FriendRequest>(u => u.SenderId == user.Id && u.Recieverid == id);
            var sentFriendRequest = await _unitOfWork.Repository<FriendRequest>().GetEntityWithSpecAsync(sentFriendReqSpec);



            if (sentFriendRequest is not null)
            {
                _unitOfWork.Repository<FriendRequest>().Delete(sentFriendRequest);
                _unitOfWork.Repository<FriendRequest>().SaveChanges();
                var updatedFriendRequests = await _unitOfWork.Repository<FriendRequest>().GetAllWithSpecAsync(sentFriendReqSpec);
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
            await _unitOfWork.Repository<FriendRequest>().Add(mappedNewFriendRequest);
            _unitOfWork.Repository<FriendRequest>().SaveChanges();
            var updatedFriendRequestsAdd = await _unitOfWork.Repository<FriendRequest>().GetAllWithSpecAsync(sentFriendReqSpec);
            var sentResult = new { message = "Sent", FriendRequests = updatedFriendRequestsAdd };

			var newNotification = new NotificationDto
            {
				content = $"The User {user.UserName} sent you a friend request.",
				userId = id,
				type = "Friend Request",
			};

            var mappedNotification = _mapper.Map<NotificationDto, Notification>(newNotification);

            // Check if _accountNotification is not null before sending the notification
            if (_unitOfWork.Repository<Notification>() != null)
			{
				// send notification to the user 
				await _accountNotification.Clients.All.SendNotification(mappedNotification);
			}

			// Check if _repositoryNotification is not null before adding the notification
			if (_unitOfWork.Repository<Notification>() != null)
			{
				// save notification in db
				await _unitOfWork.Repository<Notification>().Add(mappedNotification);
                _unitOfWork.Repository<Notification>().SaveChanges();
			}


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
			var receivedFriendRequest = await _unitOfWork.Repository<FriendRequest>().GetEntityWithSpecAsync(receivedFriendReqSpec);

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
			var requests = await _unitOfWork.Repository<FriendRequest>().GetAllWithSpecAsync(spec);
			var requestById = requests.Where(u => u.SenderId == id).FirstOrDefault();
			if (requestById is null)
			{
				return NotFound(404);
			}
			if (stateDto.State == 0) // reject
			{
				_unitOfWork.Repository<FriendRequest>().Delete(requestById);
				_unitOfWork.Repository<FriendRequest>().SaveChanges();
				var result = new { message = "Rejected" };
				return Ok(JsonSerializer.Serialize(result));
			}
			else if (stateDto.State == 1)
			{// accept
				_unitOfWork.Repository<FriendRequest>().Delete(requestById);
				_unitOfWork.Repository<FriendRequest>().SaveChanges();
                var Friend = await _manager.GetUserByIdAsync(id);
                var friend = new AppUserFriend { UserId = requestById.SenderId,UserName = user.DisplayName , FriendId = requestById.Recieverid, FriendName = Friend.DisplayName };

				await _unitOfWork.Repository<AppUserFriend>().Add(friend);
                _unitOfWork.Repository<AppUserFriend>().SaveChanges();
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
			var requests = await _unitOfWork.Repository<FriendRequest>().GetAllWithSpecAsync(spec);

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
			var requests = await _unitOfWork.Repository<FriendRequest>().GetAllWithSpecAsync(spec);

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
