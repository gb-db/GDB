using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.News;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Repository.Common;
using Models.Classes;
using Models.ViewModels;
using Repository.News_Data;

namespace Users.Controllers
{
    public class NewsController : Controller
    {
        private IHostingEnvironment _environment;
        private IUserRepository userRepository;
        private IVideoRepository videoRepository;

        public NewsController(IUserRepository repository, IHostingEnvironment env, IVideoRepository videoRep)
        {
            userRepository = repository;
            _environment = env;
            videoRepository = videoRep;
        }

        public ViewResult UploadNews()
        {
            //NewsDataViewModels newsData = new NewsDataViewModels();
            //newsData.message = "1";
            //return View("NewsUpload",newsData);
            return View("NewsUpload");
        }

        [HttpPost("NewsUpload")]
        public IActionResult UploadNews(ICollection<IFormFile> files, int User_Id, NewsDataViewModels newsData)
        {
            //long size = files.Sum(f => f.Length);
            string mainFolder = _environment.ContentRootPath;
            int n = 0;
            string msg = "";
            bool success = false;
            foreach (var file in files)
            {
                string filePath = "";

                string[] videos = new string[] { "mp4", "mp3" };
                string[] images = new string[] { "jpg", "png" };
                string fileName =  file?.FileName?.Replace(" ", "_").Replace("-", "_");
                if (file?.FileName != null)
                {
                    int ii = fileName.LastIndexOf("\\");
                    if (ii != -1)
                    {
                        fileName = fileName.Substring(ii + 1);
                    }
                }

                int i = file.FileName.LastIndexOf(".");
                if (i == -1)
                {
                    continue;
                }
                string ext = file.FileName.Substring(i + 1);


                if (videos.Contains(ext))
                {
                    filePath = _environment.WebRootPath + @"/VideoFiles/Videos/" + fileName;
                }
                if (images.Contains(ext))
                {
                    filePath = _environment.WebRootPath + @"\VideoFiles\Images\" + fileName;
                }

                if (file.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        try
                        {
                            file.CopyToAsync(stream);
                            bool res = videoRepository.AddVideo(newsData, filePath);
                            n += 1;
                        }
                        catch (Exception ex)
                        {
                            msg += " There was problem to save file :  " + file?.FileName;
                        }
                    }
                }
            }

            if (n > 0)
            {
                msg += " Files saved successfully are: " + n.ToString();
                success = true;
                return Json(new { success = success, message = msg });
            }
            else {
            newsData.message = "There was not selected file!";
            //return Ok(new { count = files.Count, size, filePath });
            return View("NewsUpload", newsData);
            }

        }

        public JsonResult SearechUsers(string term)
        {
            if (!string.IsNullOrWhiteSpace(term))
            {
                List<LabelValue> list = userRepository.GetUserList(term.Trim());

                if (list != null && list.Count > 0)
                {

                    return Json(list);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public IActionResult GetMyViewComponent(int Hidd,int user_id)
        {

            return ViewComponent("TableWithPagination", new Tuple<int,int>(Hidd,user_id));
        }
    }
}
