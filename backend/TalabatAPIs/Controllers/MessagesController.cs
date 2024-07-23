using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
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
    public class MessagesController : APIBaseController
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Message> _repositoryMessage;
        private readonly UserManager<AppUser> _manager;
        public MessagesController(IMapper mapper, UserManager<AppUser> manager, IGenericRepository<Message> genericRepository)
        {
            _manager = manager;
            _mapper = mapper;
            _repositoryMessage = genericRepository;
        }
        [Authorize]
        [HttpGet("getMessages/{id}")]
        public async Task<ActionResult<MessageDto>> GetMessages(string id)
        {
            var myUser = await _manager.GetUserAddressAsync(User);
            if (myUser is null)
            {
                return Unauthorized(new ApiResponse(401));
            }
            var spec = new BaseSpecifications<Message>(m => (m.senderId == id && m.recieverId == myUser.Id) || (m.senderId == myUser.Id && m.recieverId == id));
            var messages = await _repositoryMessage.GetAllWithSpecAsync(spec);
            if (messages == null)
            {
                return BadRequest("No Messages");
            }

            var mappedMessage = _mapper.Map<List<Message>, List<MessageDto>>(messages.ToList());
            return Ok(mappedMessage);
        }
        [Authorize]
        [HttpGet("getGroupMessages/{groupName}")]
        public async Task<ActionResult<MessageDto>> GetGrpMessages(string groupName)
        {
            var myUser = await _manager.GetUserAddressAsync(User);
            if (myUser is null)
            {
                return Unauthorized(new ApiResponse(401));
            }
            var spec = new BaseSpecifications<Message>(m => m.groupName == groupName);
            var messages = await _repositoryMessage.GetAllWithSpecAsync(spec);
            if (messages == null)
            {
                return BadRequest("No Messages");
            }

            var mappedMessage = _mapper.Map<List<Message>, List<MessageDto>>(messages.ToList());
            return Ok(mappedMessage);
        }
    }
    }
