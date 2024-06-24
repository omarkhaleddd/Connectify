using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTO;
using Talabat.APIs.Errors;
using Talabat.APIs.Exstentions;
using Talabat.Core.Entities.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : APIBaseController
    {

        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _manager;
        private readonly IGenericRepository<Notification> _repositoryNotification;
        public NotificationController(IMapper mapper, UserManager<AppUser> manager, IGenericRepository<Notification> genericRepository)
        {
            _mapper = mapper;
            _manager = manager;
            _repositoryNotification = genericRepository;
        }
        [Authorize]
        [HttpGet("Notifications")]
        public async Task<ActionResult<Notification>> GetNotications()
        {
            var myUser = await _manager.GetUserAddressAsync(User);
            if (myUser is null)
            {
                return Unauthorized(new ApiResponse(401));
            }
            var spec = new BaseSpecifications<Notification>(n => n.userId == myUser.Id);
            var notifications = await _repositoryNotification.GetAllWithSpecAsync(spec);
            if(notifications is null)
            {
                return NotFound();
            }
            var mappedNotifications = _mapper.Map < List<Notification>, List< NotificationDto > >(notifications.ToList());
            return Ok(mappedNotifications);
        }
    }
}



