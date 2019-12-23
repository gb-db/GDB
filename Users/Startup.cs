using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Users.Models;
using Users.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Repository.Common;


using Microsoft.AspNetCore.Authorization;
using System;
using Models.Users;
using DataLibrary.Context;
using Repository.News_Data;
using Repository.Heritage;
using Repository.Utility;
using Microsoft.AspNetCore.Hosting;

namespace Users
{

    public class Startup
    {
        public IConfiguration Configuration { get; }
        IHostingEnvironment env;
        public Startup(IConfiguration configuration, IHostingEnvironment env_)
        {
            Configuration = configuration;
            env = env_;
        }

        //public IServiceProvider ServiceProvider { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IVideoRepository, VideoRepository>();
            services.AddTransient<IHeritageRepository, HeritageRepository>();

            services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordValidator>();
            services.AddTransient<IUserValidator<AppUser>, CustomUserValidator>();
            services.AddSingleton<IClaimsTransformation, LocationClaimsProvider>();
            services.AddTransient<IAuthorizationHandler, BlockUsersHandler>();
            services.AddTransient<IAuthorizationHandler, DocumentAuthorizationHandler>();

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("DCUsers", policy =>
                {
                    policy.RequireRole("Users");
                    policy.RequireClaim(ClaimTypes.StateOrProvince, "DC");
                });
                opts.AddPolicy("NotBob", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddRequirements(new BlockUsersRequirement("Bob"));
                });
                opts.AddPolicy("AuthorsAndEditors", policy =>
                {
                    policy.AddRequirements(new DocumentAuthorizationRequirement
                    {
                        AllowAuthors = true,
                        AllowEditors = true
                    });
                });
            });

            services.AddAuthentication().AddGoogle(opts =>
            {
                opts.ClientId = "<enter client id here>";
                opts.ClientSecret = "<enter client secret here>";
            });

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(
                    Configuration["Data:SportStoreIdentity:ConnectionString"],b => b.MigrationsAssembly("Users")));

            services.AddIdentity<AppUser, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStatusCodePages();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();

            //try
            //{
            //    AppIdentityDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
            //    string methodName = st.GetFrame(0).GetMethod().Name;

            //    string Msg = methodName + " ### " + ex.Message + " ### public void Configure(IApplicationBuilder app)";
            //    if (ex.InnerException != null)
            //        Msg += ex.InnerException.Message;

            //    Helper helper = new Helper(env);
            //    helper.WriteToLog(Msg);
            //}
        }
    }
}
