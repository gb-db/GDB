using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.News;
using Models.Users;
using System;
using System.Threading.Tasks;
using Models.Heritage;

namespace DataLibrary.Context
{

    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {

        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options) { }        

        public DbSet<Code> Codes { get; set; }
        public DbSet<NewsData> NewsData { get; set; }
        public DbSet<HeratigeData> HeratigeDatas { get; set; }
        public DbSet<HeratigeUser> HeratigeUsers { get; set; }
        public DbSet<HerInfoSrc> HerInfoSrcs { get; set; }
        public DbSet<Info_Src> Info_Srcs { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Province> Province { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Church> Church { get; set; }
        public DbSet<HeratigePermission> HeratigePermissions { get; set; }


        public static async Task CreateAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
        {

            UserManager<AppUser> userManager =
                serviceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string username = configuration["Data:AdminUser:Name"];
            string email = configuration["Data:AdminUser:Email"];
            string password = configuration["Data:AdminUser:Password"];
            string role = configuration["Data:AdminUser:Role"];
            string user_id_str = configuration["Data:AdminUser:user_id"];
            int user_id = 1;
            int.TryParse(user_id_str, out user_id); 

            if (await userManager.FindByNameAsync(username) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                AppUser user = new AppUser
                {
                    UserName = username,
                    Email = email,
                    user_id= user_id
                };

                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }




        public static async Task CreateAdminAccount1(UserManager<AppUser> userManager_, RoleManager<IdentityRole> roleManager_, IConfiguration configuration)
        {

            UserManager<AppUser> userManager = userManager_;
            RoleManager<IdentityRole> roleManager = roleManager_;

            string username = configuration["Data:AdminUser:Name"];
            string email = configuration["Data:AdminUser:Email"];
            string password = configuration["Data:AdminUser:Password"];
            string role = configuration["Data:AdminUser:Role"];
            string user_id_str = configuration["Data:AdminUser:user_id"];
            int user_id = 1;
            int.TryParse(user_id_str, out user_id);

            if (await userManager.FindByNameAsync(username) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                AppUser user = new AppUser
                {
                    UserName = username,
                    Email = email
                    //,
                    //user_id = user_id
                };

                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
