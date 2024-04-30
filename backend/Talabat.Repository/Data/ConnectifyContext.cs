﻿using Connectify.Core.Entities.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Data
{
	public class ConnectifyContext : DbContext
	{
		public DbSet<Post> Post { get; set; }

		public DbSet<Comment> Comment { get; set; }

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

			//base.OnModelCreating(modelBuilder);
		}


	}
}