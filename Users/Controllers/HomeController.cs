using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Users.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.Users;
using Models.ViewModels;
using Repository.News_Data;
using Models.Enums;
using Models.Classes;
using Microsoft.AspNetCore.Hosting;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System;
using Repository.Heritage;
using DataLibrary.Context;
using Microsoft.Extensions.Configuration;
using Repository.Utility;

namespace Users.Controllers
{

    public class HomeController : Controller
    {
        int CurrentPage = 1;
        int PAGESIZE = 2;

        public ImageFormat imageFormat = ImageFormat.Png;
        IHostingEnvironment env;

        private UserManager<AppUser> userManager;
        private IVideoRepository videoRepository;
        private IHeritageRepository heritageRepository;

        private readonly UserManager<AppUser> _userManager;//
        private readonly RoleManager<IdentityRole> _roleManager;//
        private IConfiguration Configuration { get; }

        public HomeController(UserManager<AppUser> userMgr, IVideoRepository videoRep, IHostingEnvironment env_, IHeritageRepository heritageRepository_,
                                  RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            userManager = userMgr;
            videoRepository = videoRep;
            env = env_;
            heritageRepository = heritageRepository_;

            _userManager = userMgr;//
            _roleManager = roleManager;//
            Configuration = configuration;//
        }

        //[Authorize]
        public IActionResult Index()
        {
            //CreateAdminAccount1(_userManager, _roleManager, Configuration).Wait();
            try
            {
              CreateAdminAccount1(_userManager, _roleManager, Configuration).Wait();
            }
            catch (Exception ex)
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                string methodName = st.GetFrame(0).GetMethod().Name;

                string Msg = methodName + " ### " + ex.Message + " ### public void Configure(IApplicationBuilder app)";
                if (ex.InnerException != null)
                    Msg += ex.InnerException.Message;

                Helper helper = new Helper(env);
                helper.WriteToLog(Msg);
            }

            return View("MainPage");
        }

        public IActionResult MainPage(LoginModel model)
        {
            ViewBag.returnUrl = model?.returnUrl;

            return View(nameof(MainPage));
        }

        [Authorize]
        public IActionResult SecurePage() => View(nameof(SecurePage), new SecurePageModel { desc = "aaaaa", Id = 1 });

        [Authorize]
        public IActionResult SecurePage1() => View(nameof(SecurePage1), new SecurePageModel { desc = "aaaaa", Id = 1 });

        [Authorize]
        public IActionResult GetCaruselImages() => View();

        //[Authorize(Roles = "Users")]
        [Authorize(Policy = "DCUsers")]
        public IActionResult OtherAction() => View("Index", GetData(nameof(OtherAction)));

        [Authorize(Policy = "NotBob")]
        public IActionResult NotBob() => View("Index", GetData(nameof(NotBob)));

        private Dictionary<string, object> GetData(string actionName) =>
            new Dictionary<string, object>
            {
                ["Action"] = actionName,
                ["User"] = HttpContext.User.Identity.Name,
                ["Authenticated"] = HttpContext.User.Identity.IsAuthenticated,
                ["Auth Type"] = HttpContext.User.Identity.AuthenticationType,
                ["In Users Role"] = HttpContext.User.IsInRole("Users"),
                ["City"] = CurrentUser.Result.City,
                ["Qualification"] = CurrentUser.Result.Qualifications
            };

        [Authorize]
        public async Task<IActionResult> UserProps()
        {
            return View(await CurrentUser);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UserProps(
                [Required]Cities city,
                [Required]QualificationLevels qualifications)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await CurrentUser;
                user.City = city;
                user.Qualifications = qualifications;
                await userManager.UpdateAsync(user);
                return RedirectToAction("Index");
            }
            return View(await CurrentUser);
        }

        private Task<AppUser> CurrentUser =>
            userManager.FindByNameAsync(HttpContext.User.Identity.Name);

        public IActionResult PaginationPage()
        {
            //return View(videoRepository.GetNewsList(2, NewsTypes.All, 1));
            return View();
        }

        [HttpPost]
        public JsonResult LoadPagination(int page, int rowStart, int pagesize, string sortByColumn, string sortByOrder)
        {
            NewsSearchDataResponse newsSearchDataResponse = new NewsSearchDataResponse();
            NewsDataPaginationViewModels res = videoRepository.GetNewsList(2, NewsTypes.All, page, pagesize, sortByColumn, sortByOrder);
            List<NewsSearchData> searchDataList = new List<NewsSearchData>();

            if (res != null && res.NewsDatasList.Count > 0)
            {
                foreach (var item in res.NewsDatasList)
                {
                    NewsSearchData data = new NewsSearchData();
                    data.Description = item.description;
                    data.Name = item.lName;
                    data.Path = item.Path;

                    searchDataList.Add(data);
                }

                newsSearchDataResponse.news = searchDataList;
                newsSearchDataResponse.errorMessage = "";
                newsSearchDataResponse.totalRow = res.PagingInfo.TotalItems;
            }
            else
            {
                newsSearchDataResponse.errorMessage = "There was some error in the server!";
            }

            return Json(newsSearchDataResponse);
        }

        public IActionResult PaginationPage_a(NewsDataPagination_a_ViewModels model, int currentPage, string sortByColumn, string sortByOrder)
        {
            int currentPage_ = currentPage == 0 ? CurrentPage : currentPage;
            NewsSearchDataResponse newsSearchDataResponse = new NewsSearchDataResponse();
            //NewsDataPaginationViewModels res = videoRepository.GetNewsList(2, NewsTypes.All, page, pagesize, sortByColumn, sortByOrder);
            NewsDataPagination_a_ViewModels res = videoRepository.GetNewsList_a(2, NewsTypes.All, currentPage_, PAGESIZE, sortByColumn, sortByOrder);
            List<NewsSearchData> searchDataList = new List<NewsSearchData>();

            if (res != null && res.NewsDatasList.Count > 0)
            {
                foreach (var item in res.NewsDatasList)
                {
                    NewsSearchData data = new NewsSearchData();
                    data.Description = item.description;
                    data.Name = item.lName;
                    data.Path = item.Path;

                    searchDataList.Add(data);
                }

                newsSearchDataResponse.news = searchDataList;
                newsSearchDataResponse.errorMessage = "";
                newsSearchDataResponse.totalRow = res.paging_A_Info?.totalItems ?? 0;
            }
            else
            {
                newsSearchDataResponse.errorMessage = "There was some error in the server!";
            }

            //return Json(newsSearchDataResponse);
            return View("PaginationPage_a", res);
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
