using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;
using Talabat.Core.Specifications;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Talabat.APIs.Hubs;
using Talabat.Core;
using Talabat.Core.Entities.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Hubs.Interfaces;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications;
using Talabat.Service.Services;
namespace Talabat.Service.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _manager;

        public AdminService(IUnitOfWork unitOfWork, UserManager<AppUser> manager)
        {
            _unitOfWork = unitOfWork;
            _manager = manager;
        }

        public async void DeleteUser(AppUser user)
        {
            var result = await _manager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new InvalidOperationException("Couldn't Delete User");
        }

        public async Task<bool> DismissResolveReport(int id, int flag)
        {
            var spec = new BaseSpecifications<ReportedPost>(u => u.Id == id);
            var currReportedPost = await _unitOfWork.Repository<ReportedPost>().GetEntityWithSpecAsync(spec);
            if (currReportedPost is null)
                throw new InvalidOperationException("Reported Post Not Found");

            var postSpec = new BaseSpecifications<Post>(u => u.Id == currReportedPost.PostId);
            var currPost = await _unitOfWork.Repository<Post>().GetEntityWithSpecAsync(postSpec);

            if (currPost is null)
                throw new InvalidOperationException("Post Not Found");

            if (flag == 1)
            {
                currReportedPost.Status = "dismissed";
                _unitOfWork.Repository<ReportedPost>().Update(currReportedPost);
                _unitOfWork.Repository<ReportedPost>().SaveChanges();
                currPost.ReportCount = 0;
                _unitOfWork.Repository<Post>().Update(currPost);
                _unitOfWork.Repository<Post>().SaveChanges();
                return true ;
            }
            else
            {
                currReportedPost.Status = "resolved";
                _unitOfWork.Repository<ReportedPost>().Update(currReportedPost);
                _unitOfWork.Repository<ReportedPost>().SaveChanges();
                return false ;
            }
        }

        public async Task<IEnumerable<Post>> getPostsAdmin(string? id)
        {
            IEnumerable<Post> posts;
            if (id is not null){
                var spec = new PostWithCommentSpecs(id);
                posts = await _unitOfWork.Repository<Post>().GetAllWithSpecAsync(spec);
            }
            else { 
                var spec = new PostWithCommentSpecs();
                posts = await _unitOfWork.Repository<Post>().GetAllWithSpecAsync(spec);
            }
            if (posts == null)
                throw new InvalidOperationException("No Posts Found");
            return posts;
        }

        public async Task<IEnumerable<ReportedPost>> GetReports()
        {
            var spec = new ReportedPostWithPostSpecs();
            var ReportedPosts = await _unitOfWork.Repository<ReportedPost>().GetAllWithSpecAsync(spec);
            if (ReportedPosts == null)
                throw new InvalidOperationException("No Reported Posts Found");
            return ReportedPosts;
        }
    }
}
