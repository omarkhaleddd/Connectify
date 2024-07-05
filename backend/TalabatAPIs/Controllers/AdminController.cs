using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Controllers;
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
    public class AdminController : APIBaseController
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _manager;
        private readonly IGenericRepository<Post> _repositoryPost;
        private readonly IGenericRepository<ReportedPost> _repositoryReportedPost;

        public AdminController(IMapper mapper, UserManager<AppUser> userManager, IGenericRepository<Post> genericRepository, IGenericRepository<ReportedPost> genericRepository1)
        {
            _mapper = mapper;
            _manager = userManager;
            _repositoryPost = genericRepository;
            _repositoryReportedPost = genericRepository1;
        }
        [Authorize]
        [HttpGet()]
        public async Task<ActionResult> GetReports()
        {
            var myUser = await _manager.GetUserAddressAsync(User);
            if (myUser is null)
            {
                return Unauthorized(new ApiResponse(401));
            }

            var spec = new ReportedPostWithPostSpecs();
            var ReportedPosts = await _repositoryReportedPost.GetAllWithSpecAsync(spec);
            if (ReportedPosts == null)
            {
                return BadRequest("No Reported Posts");
            }
            var mappedReportedPosts = new List<ReportedPostDto>();
            mappedReportedPosts = _mapper.Map<List<ReportedPost>, List<ReportedPostDto>>(ReportedPosts.ToList());

            return Ok(mappedReportedPosts);
        }

        [Authorize]
        [HttpPut("dismiss-report/{id}")]
        public async Task<ActionResult> DismissReport(int id)
        {
            var myUser = await _manager.GetUserAddressAsync(User);
            if (myUser is null)
            {
                return Unauthorized(new ApiResponse(401));
            }

            var spec = new BaseSpecifications<ReportedPost>(u => u.Id == id);
            var currReportedPost = await _repositoryReportedPost.GetEntityWithSpecAsync(spec);
            if (currReportedPost is null)
            {
                return NotFound();
            }
            var postSpec = new BaseSpecifications<Post>(u => u.Id == currReportedPost.PostId);
            var currPost = await _repositoryPost.GetEntityWithSpecAsync(postSpec);
            if (currPost is null)
            {
                return NotFound();
            }
            currReportedPost.Status = "dismiss";
            _repositoryReportedPost.Update(currReportedPost);
            _repositoryReportedPost.SaveChanges();
            currPost.ReportCount = 0;
            _repositoryPost.Update(currPost);
            _repositoryPost.SaveChanges();
            return Ok("Report Dismissed");
        }
        [Authorize]
        [HttpPut("resolve-report/{id}")]
        public async Task<ActionResult> ResolveReport(int id)
        {
            var myUser = await _manager.GetUserAddressAsync(User);
            if (myUser is null)
            {
                return Unauthorized(new ApiResponse(401));
            }

            var spec = new BaseSpecifications<ReportedPost>(u => u.Id == id);
            var currReportedPost = await _repositoryReportedPost.GetEntityWithSpecAsync(spec);
            if (currReportedPost is null)
            {
                return NotFound();
            }
            var postSpec = new BaseSpecifications<Post>(u => u.Id == currReportedPost.PostId);
            var currPost = await _repositoryPost.GetEntityWithSpecAsync(postSpec);
            if (currPost is null)
            {
                return NotFound();
            }
            currReportedPost.Status = "resolve";
            _repositoryReportedPost.Update(currReportedPost);
            _repositoryReportedPost.SaveChanges();
            _repositoryPost.Delete(currPost);
            _repositoryPost.SaveChanges();
            return Ok("Report Resolved");

        }
        [Authorize]
        [HttpDelete("delete-user/{id}")]
        public async Task<ActionResult> DeleteUser(string Id)
        {
            var myUser = await _manager.GetUserAddressAsync(User);
            if (myUser is null)
            {
                return Unauthorized(new ApiResponse(401));
            }
            var user = await _manager.GetUserByIdAsync(Id);
            if (user is null)
                return NotFound(new ApiResponse(404));
            var result = await _manager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));
            return Ok();
        }

    }
}
