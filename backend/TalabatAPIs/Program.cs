using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Exstentions;
using Talabat.APIs.Helpers;
using Talabat.APIs.MiddleWares;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

public class Program
{
    public static async Task Main(string [] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<ConnectifyContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
        {
            Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
        });

        builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
        {
        var ConnectionSting = builder.Configuration.GetConnectionString("RedisConnection");
            return ConnectionMultiplexer.Connect(ConnectionSting);

        });
      
        builder.Services.AddApplicationService();
        builder.Services.AddIdentityServices(builder.Configuration);
        using var app = builder.Build();
        #region Update Database


        //  StoreContext context=new StoreContext()
        var Scopped = app.Services.CreateScope();
        var services = Scopped.ServiceProvider;
        var LoggerFactory=services.GetService<ILoggerFactory>();

        try
        {
            
            var dbContext = services.GetRequiredService<ConnectifyContext>();
            await dbContext.Database.MigrateAsync();

            var IdentityDbCOntext =services.GetRequiredService<AppIdentityDbContext>();
            await IdentityDbCOntext.Database.MigrateAsync();
          await  ConnectifyContextSeed.SeedAsync(dbContext);
        }
        catch (Exception ex)
        {
            var logger = LoggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "error while Migrations");

        }
        #endregion


        #region DataSeed
       
        #endregion
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMiddleware<ExceptionsMiddleWare>();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseStaticFiles();
        app.UseStatusCodePagesWithRedirects("/errors/{0}");

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();

    }
}
