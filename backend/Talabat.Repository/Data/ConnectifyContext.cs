using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Core;
using Talabat.Core.Entities.Core.Donation;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Data
{
	public class ConnectifyContext : DbContext
	{
		public DbSet<Post> Post { get; set; }

		public DbSet<Comment> Comment { get; set; }
		public DbSet<Repost> Repost { get; set; }
		public DbSet<AppUserFriend> friends { get; set; }
		public DbSet<FriendRequest> friendRequests { get; set; }
        public DbSet<BlockList> blockList { get; set; }
		public DbSet<Message> messages { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<Donation> Donations { get; set; }
		public DbSet<ReportedPost> ReportedPost { get; set; }
        public DbSet<FileNames> FileNames { get; set; }

        public ConnectifyContext(DbContextOptions<ConnectifyContext> options) : base(options)
		{

		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// modelBuilder.ApplyConfiguration(new ProductConfig());
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			modelBuilder.Entity<Post>()
				.HasMany(P => P.Comments)
				.WithOne(C => C.Post)
				.HasForeignKey(C => C.PostId)
				.OnDelete(DeleteBehavior.Cascade);
            
			modelBuilder.Entity<Post>()
				.HasMany(P => P.Likes)
				.WithOne(L => L.Post)
				.HasForeignKey(L => L.PostId)
				.OnDelete(DeleteBehavior.Cascade);
			
			modelBuilder.Entity<Post>()
				.HasMany(P => P.FileName)
				.WithOne(F => F.Post)
				.HasForeignKey(C => C.PostId)
				.OnDelete(DeleteBehavior.Cascade);
            
			modelBuilder.Entity<Post>()
				.HasMany(P => P.Reposts)
				.WithOne(R => R.Post)
				.HasForeignKey(R => R.PostId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Comment>()
				.HasMany(C => C.Likes)
				.WithOne(CL => CL.Comment)
				.HasForeignKey(CL => CL.CommentId)
				.OnDelete(DeleteBehavior.Cascade);
			modelBuilder.Entity<Donation>()
				.Property(d => d.Amount)
				.HasColumnType("decimal(18,2)");
			//base.OnModelCreating(modelBuilder);
		}


    }
}