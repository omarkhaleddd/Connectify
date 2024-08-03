using Connectify.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.APIs.Hubs;
using Talabat.APIs.MiddleWares;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Talabat.APIs.Exstentions;
using Talabat.Core.Entities.Identity;
using Talabat.Service.Services;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure logging
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<ConnectifyContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        builder.Services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
        });

        builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
        {
            Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
        });

        builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
        {
            var ConnectionString = builder.Configuration.GetConnectionString("RedisConnection");
            return ConnectionMultiplexer.Connect(ConnectionString);
        });

        builder.Services.AddSingleton<RedisCacheService>();
        builder.Services.AddTransient<IUploadService, UploadService>();
        builder.Services.AddTransient<IEmailService, EmailService>();
        builder.Services.AddScoped<ICommentService, CommentService>();

        builder.Services.AddAutoMapper(x => x.AddProfile<MappingProfiles>());
        builder.Services.AddApplicationService();
        builder.Services.AddIdentityServices(builder.Configuration);

        // Add CORS policy
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins("http://localhost:4200") // Update with your Angular application's URL
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        var app = builder.Build();

        // Log server names
        LogServerNames(app.Services.GetRequiredService<IConfiguration>(), app.Services.GetRequiredService<ILogger<Program>>());

        #region Update Database

        var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();

        try
        {
            var dbContext = services.GetRequiredService<ConnectifyContext>();
            await dbContext.Database.MigrateAsync();

            var identityDbContext = services.GetRequiredService<AppIdentityDbContext>();
            await identityDbContext.Database.MigrateAsync();

            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await AppIdentityDbContextSeed.SeedUsersAsync(userManager, roleManager);
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "An error occurred during migrations.");
        }

        #endregion

        app.UseRouting();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseCors(); // Enable CORS for all origins, headers, and methods
            app.UseMiddleware<ExceptionsMiddleWare>();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseStaticFiles();
        app.UseStatusCodePagesWithRedirects("/errors/{0}");
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapHub<ChatHub>("/chatHub");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<AccountNotificationHub>("/notification");
            endpoints.MapHub<VideoCallHub>("/videocallhub");
        });

        app.MapControllers();
        app.Run();
    }

    private static void LogServerNames(IConfiguration configuration, ILogger logger)
    {
        var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");
        var identityConnectionString = configuration.GetConnectionString("IdentityConnection");

        var defaultConnectionBuilder = new SqlConnectionStringBuilder(defaultConnectionString);
        var identityConnectionBuilder = new SqlConnectionStringBuilder(identityConnectionString);

        var defaultServerName = defaultConnectionBuilder.DataSource;
        var identityServerName = identityConnectionBuilder.DataSource;

        logger.LogInformation("Using SQL Server for DefaultConnection: {ServerName}", defaultServerName);
        logger.LogInformation("Using SQL Server for IdentityConnection: {ServerName}", identityServerName);
    }
}
