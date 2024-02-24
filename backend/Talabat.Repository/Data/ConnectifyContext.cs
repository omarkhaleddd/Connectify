using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;


namespace Talabat.Repository.Data
{
    public class ConnectifyContext:DbContext
    {
      
        public ConnectifyContext(DbContextOptions<ConnectifyContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // modelBuilder.ApplyConfiguration(new ProductConfig());
           modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //base.OnModelCreating(modelBuilder);
        }
      

    }
}
