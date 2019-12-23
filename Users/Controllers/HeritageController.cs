using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using Repository.Common;
using Repository.Heritage;
using System.Drawing.Imaging;
using System.Drawing;
using Newtonsoft.Json;
using Models.Classes;
using Models.Enums;
using Microsoft.Extensions.Configuration;
using Models.Users;
using Microsoft.AspNetCore.Identity;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860  

namespace Users.Controllers
{
    public class HeritageController : Controller
    {
        public ImageFormat imageFormat = ImageFormat.Png;

        private IHostingEnvironment _environment;
        private IUserRepository userRepository;
        private IHeritageRepository heritageRepository;
        private IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;

        public HeritageController(IUserRepository repository, IHostingEnvironment env,
                                        IHeritageRepository heritageRep, IConfiguration config, UserManager<AppUser> userMgr)
        {
            userRepository = repository;
            _environment = env;
            heritageRepository = heritageRep;
            _config = config;
            _userManager = userMgr;
        }

        public IActionResult Heritage()
        {
            ViewBag.IsNew = "true";

            AppUser appUser = _userManager.GetUserAsync(User).Result;
            //ViewBag.user_id = 2;
            IList<string> list = null;
            if (appUser != null)
            {
                list = _userManager.GetRolesAsync(appUser)?.Result;
                ViewBag.user_id = appUser.user_id;
            }
            ViewBag.heratigeUser_Id = 0;


            return View("Heritage");
            //return View("JS");
        }

        #region Create & Edit Hetitage Person
        // GET: /<controller>/
        public IActionResult HerEdit(string id)
        {
            AppSettings appSettings = heritageRepository.GetAppSettings(_config);

            CardIds cardIds = heritageRepository.GetLevelsAsIntList(id);
            HeritageViewModel model = heritageRepository.GetHerEditModelByLevels(cardIds);

            //model.user_id = 2;
            //model.lName = "albert";
            return View("HerEdit", model);
        }

        [HttpPost]
        public IActionResult HerEdit(HeritageViewModel heritageViewModel)
        {
            HeritageViewModel retVal = CheckHerEdit(heritageViewModel);
            if (retVal.message != "")
            {
                heritageViewModel.message = retVal.message;
                return View("HerEdit", heritageViewModel);
            }

            //string mainFolder = _environment.ContentRootPath;
            //int n = 0;
            //string msg = "";
            //bool success = false;
            RetHerUpdate retHerUpdate = null;

            if (heritageViewModel != null)
            {
                HeritageViewModel heritageViewModel_ = heritageRepository.ConvertDateTimes(heritageViewModel);

                retHerUpdate = heritageRepository.InsertHerInformation(heritageViewModel);
            }

            if (retHerUpdate.success)
            {
                retHerUpdate.message = " Information is inserted successfully.";
                return Json(retHerUpdate);
            }
            else
            {
                heritageViewModel.message = retHerUpdate.message;
                return View("HerEdit", heritageViewModel);
            }

        }

        public HeritageViewModel CheckHerEdit(HeritageViewModel heritageViewModel)
        {
            heritageViewModel.message = "";
            if (heritageViewModel.countryId == 0)
            {
                heritageViewModel.message = "Country must be selected before to save.";
            }
            else
            {
                if (heritageViewModel.cityId != 0)
                {
                    if (heritageViewModel.provinceId == 0)
                    {
                        heritageViewModel.message = "Province must be selected before selecting city.";
                    }
                }
                else
                {
                    if (heritageViewModel.cityId == 0 && heritageViewModel.churchId != 0)
                    {
                        heritageViewModel.message = "City must be selected before selecting church.";
                    }
                }
            }

            if (heritageViewModel.message == "")
            {
                if (heritageViewModel.countryBPId == 0)
                {
                    if (heritageViewModel.provinceBPId != 0)
                    {
                        heritageViewModel.message = "Country(Birth Place) must be selected before selecting province.";
                    }
                    else
                    {
                        if (heritageViewModel.cityBPId != 0)
                        {
                            heritageViewModel.message = "Province(Birth Place) must be selected before selecting city.";
                        }
                    }
                }
                else
                {
                    if (heritageViewModel.cityBPId != 0)
                    {
                        if (heritageViewModel.provinceBPId == 0)
                        {
                            heritageViewModel.message = "Province(Birth Place) must be selected before selecting city.";
                        }
                    }
                }
            }

            return heritageViewModel;
        }
        #endregion


        #region Upload Photo
        public ViewResult HerUploadImage()
        {
            return View("HerUploadImage");
        }

        //uploading and saving photo
        [HttpPost("HerUploadImage")]
        public IActionResult HerUploadImage(ICollection<IFormFile> files, int User_Id, UploadPhotoViewModel uploadData)
        {
            AppSettings appSettings = heritageRepository.GetAppSettings(_config);
            int n = 0;
            string msg = "";
            bool success = false;
            string filePath = "";

            IFormFile file = (files.ToList())[0];
            string fileName = file?.FileName?.Replace(" ", "_").Replace("-", "_");
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
                uploadData.message = "It is not correct file name!!";
                return View("HerUploadImage", uploadData);
            }
            fileName = (uploadData?.heritageIds ?? "") + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString()+ Path.GetExtension(file.FileName);
            //string user_name = "63263cc2-119b-4ec5-9f4a-b33a30ecb74b";//?????????????????????????????????????????????????????????????
            string folderPath = _environment.WebRootPath + @"\Images\Heritage\Temp\";
            filePath = folderPath + fileName;
            //filePath = _environment.WebRootPath + @"\Images\Heritage\Temp\" + fileName;
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                try
                {
                    file.CopyTo(stream);
                    n += 1;


                }
                catch (Exception ex)
                {
                    msg += " There was problem to save file :  " + file?.FileName;
                }
            }

            HerRepData herRepData = heritageRepository.ModifyImageStrict(filePath, appSettings.PHOTO_WIDTH, appSettings.PHOTO_HEIGHT, appSettings.userIdStr, imageFormat, ImageModificationsType.InDivCenter);
            string PATH = herRepData?.path;
            herRepData.path = herRepData?.path?.Substring(herRepData.path.IndexOf("Images") - 1);

            if (!string.IsNullOrWhiteSpace(herRepData?.path))
            {
                bool retVal = heritageRepository.DeleteTempFolderFiles(folderPath);
            }

            if (n > 0 && herRepData.message == "")
            {
                uploadData.height = herRepData?.size.Height ?? 0;
                uploadData.width = herRepData?.size.Width ?? 0;
                uploadData.path = herRepData.path.ToLower();
                RetSimple retSimple = heritageRepository.UpdateDBWithPath(uploadData);
                if (retSimple.message != "")
                {
                    uploadData.message = retSimple.message;
                    return View("HerUploadImage", uploadData);
                }

                msg += " File saved successfully.";
                success = true;
                return Json(new
                {
                    success = success,
                    message = msg,
                    path = herRepData.path.ToLower(),
                    height = herRepData?.size.Height,
                    width = herRepData?.size.Width
                });
            }
            else
            {
                uploadData.message = "There was not selected file! \n " + herRepData.message;
                //return Ok(new { count = files.Count, size, filePath });
                return View("HerUploadImage", uploadData);
            }

        }
        #endregion

        #region Upload Information
        public ViewResult GetHerData_Image(string card_id)
        {
            UploadPhotoViewModel uploadPhotoViewModel = new UploadPhotoViewModel();
            s_strings s_Strings = heritageRepository.GetRootImageNamesByIds(card_id);
            uploadPhotoViewModel.rootHerImg = s_Strings?.str1;
            uploadPhotoViewModel.rootHerName = s_Strings?.str2;
            List<InfoItem> InfoItems = heritageRepository.GetImageCommentListByIds(card_id);
            uploadPhotoViewModel.infoItemsStr = JsonConvert.SerializeObject(InfoItems);

            return View("HerUploadImage_Data", uploadPhotoViewModel);
        }

        //Sending image in to db
        [HttpPost("PostHerImage")]
        public IActionResult PostHerImage(ICollection<IFormFile> files, int User_Id, UploadPhotoViewModel uploadData, string img_comment)
        {
            AppSettings appSettings = heritageRepository.GetAppSettings(_config);
            int n = 0;
            string msg = "";
            bool success = false;
            string filePath = "";

            if (files.Count == 0)
            {
                uploadData.message = "Select file for upload!";
                return View("HerUploadImage", uploadData);
            }

            IFormFile file = (files.ToList())[0];
            string fileName = file?.FileName?.Replace(" ", "_").Replace("-", "_");
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
                uploadData.message = "It is not correct file name!!";
                return View("HerUploadImage", uploadData);
            }
            //string user_name = "63263cc2-119b-4ec5-9f4a-b33a30ecb74b";//????????????   after when authentication
            //string user_name = userIdStr;//????????????   after when authentication
            //filePath = _environment.WebRootPath + @"\Images\Heritage\Temp\" + fileName;

            string folderPath = _environment.WebRootPath + @"\Images\Heritage\Temp\";
            filePath = folderPath + uploadData?.heritageIds + fileName;

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                try
                {
                    file.CopyTo(stream);
                    n += 1;
                }
                catch (Exception ex)
                {
                    msg += " There was problem to save file :  " + file?.FileName;
                }
            }
            HerRepData herRepData = heritageRepository.ModifyImageStrict(filePath, appSettings.IMAGE_WIDTH, appSettings.IMAGE_HEIGHT, appSettings.userIdStr, imageFormat, ImageModificationsType.WithGivenWidth);
            string PATH = herRepData?.path;
            herRepData.path = herRepData?.path?.Substring(herRepData.path.IndexOf("Images") - 1);

            if (!string.IsNullOrWhiteSpace(herRepData?.path))
            {
                bool retVal = heritageRepository.DeleteTempFolderFiles(folderPath);
            }

            if (n > 0 && herRepData.message == "")
            {
                uploadData.height = herRepData?.size.Height ?? 0;
                uploadData.width = herRepData?.size.Width ?? 0;
                uploadData.path = herRepData.path.ToLower();
                RetSimple retSimple = heritageRepository.UpdateDBWithImageInfo(uploadData);
                if (retSimple.message != "")
                {
                    uploadData.message = retSimple.message;
                    return View("HerUploadImage_Data", uploadData);
                }

                msg += " File saved successfully.";
                success = true;
                return Json(new
                {
                    success = success,
                    message = msg,
                    path = herRepData.path.ToLower(),
                    height = herRepData?.size.Height,
                    width = herRepData?.size.Width,
                    img_comment = img_comment,
                    order_number = retSimple.Id,
                    card_ids = uploadData.heritageIds
                });
            }
            else
            {
                uploadData.message = "There was not selected file! \n " + herRepData.message;
                //return Ok(new { count = files.Count, size, filePath });
                return View("HerUploadImage_Data", uploadData);
            }

        }

        //[HttpPost]//No any referencies -----> to Api
        //public IActionResult PostHerData(ICollection<IFormFile> files, int User_Id, NewsDataViewModels newsData, string img_comment, string hidd, string inp, [FromBody]string comment)
        //{
        //    //long size = files.Sum(f => f.Length);
        //    string mainFolder = _environment.ContentRootPath;
        //    int n = 0;
        //    string msg = "";
        //    bool success = false;

        //    IFormFile file = (files.ToList())[0];
        //    string filePath = "";

        //    string fileName = file?.FileName?.Replace(" ", "_").Replace("-", "_");
        //    if (file?.FileName != null)
        //    {
        //        int ii = fileName.LastIndexOf("\\");
        //        if (ii != -1)
        //        {
        //            fileName = fileName.Substring(ii + 1);
        //        }
        //    }

        //    int i = file.FileName.LastIndexOf(".");
        //    if (i == -1)
        //    {
        //        newsData.message = "It is not correct file name!!";
        //        //return Ok(new { count = files.Count, size, filePath });
        //        return View("HerUploadImage", newsData);
        //    }
        //    string ext = file.FileName.Substring(i + 1);


        //    //if (videos.Contains(ext))40---55
        //    //{
        //    string path1 = _environment.ContentRootPath + @"\wwwroot\Images\Heritage\63263cc2-119b-4ec5-9f4a-b33a30ecb74b.jpg";
        //    filePath = _environment.WebRootPath + @"\Images\Heritage\Temp\" + fileName;
        //    //}
        //    //if (images.Contains(ext))
        //    //{
        //    //    filePath = _environment.WebRootPath + @"\VideoFiles\Images\" + fileName;
        //    //}

        //    //if (file.Length > 0)
        //    //{
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        try
        //        {
        //            file.CopyTo(stream);
        //            //bool res = videoRepository.AddVideo(newsData, filePath);
        //            //string pathImg1 = heritageRepository.ModifyImageStrickt(filePath, 40, 55, "63263cc2-119b-4ec5-9f4a-b33a30ecb74b", imageFormat);
        //            //stream.Position = 0;
        //            n += 1;


        //        }
        //        catch (Exception ex)
        //        {
        //            msg += " There was problem to save file :  " + file?.FileName;
        //        }
        //    }
        //    //}
        //    //}
        //    HerRepData herRepData = heritageRepository.ModifyImageStrict(filePath, PHOTO_WIDTH, PHOTO_HEIGHT, "63263cc2-119b-4ec5-9f4a-b33a30ecb74b", imageFormat, ImageModificationsType.InDivCenter);
        //    herRepData.path = herRepData.path.Substring(herRepData.path.IndexOf("Images") - 1);

        //    if (n > 0)
        //    {
        //        msg += " File saved successfully.";
        //        success = true;
        //        return Json(new
        //        {
        //            success = success,
        //            message = msg,
        //            path = herRepData.path.ToLower(),
        //            height = herRepData?.size.Height,
        //            width = herRepData?.size.Width
        //        });
        //    }
        //    else
        //    {
        //        newsData.message = "There was not selected file!";
        //        //return Ok(new { count = files.Count, size, filePath });
        //        return View("HerUploadImage", newsData);
        //    }

        //}
        #endregion

        #region Permissions
        public IActionResult AskHerPermission(string ids, bool isEditable)
        {
            AppSettings appSettings = heritageRepository.GetAppSettings(_config);

            UploadPhotoViewModel model = new UploadPhotoViewModel();
            //CardIds cardIds = heritageRepository.GetLevelsAsIntList(id);
            //HeritageViewModel model = heritageRepository.GetHerEditModelByLevels(cardIds);

            //model.user_id = 2;
            //model.lName = "albert";
            return View("AskHerPermission", model);
        }

        public IActionResult SetHerPermission()
        {
            AppSettings appSettings = heritageRepository.GetAppSettings(_config);

            ItemInfoViewModel model_ = new ItemInfoViewModel();
            HerItem herItem = new HerItem() {LN_FN="Grigor Grigoryan",country="Armenia",province="Ararat",city="Artashat",isEditable=false,isOpend=true };
            List<HerItem> herItems = new List<HerItem>() { herItem };
            model_.herItems = herItems;
            model_.message = "AAAAAAAAAAAAAAAAAAAAAAAAAA";
            //CardIds cardIds = heritageRepository.GetLevelsAsIntList(id);
            //HeritageViewModel model = heritageRepository.GetHerEditModelByLevels(cardIds);

            //model.user_id = 2;
            //model.lName = "albert";
            return View("SetHerPermission", model_);
        }

        [HttpPost]
        public IActionResult SetHerPermission(ItemInfoViewModel model)
        {
            AppSettings appSettings = heritageRepository.GetAppSettings(_config);

            ItemInfoViewModel model_ = new ItemInfoViewModel();
            HerItem herItem = new HerItem() { LN_FN = "Grigor Grigoryan", country = "Armenia", province = "Ararat", city = "Artashat", isEditable = false, isOpend = true };
            List<HerItem> herItems = new List<HerItem>() { herItem };
            model_.herItems = herItems;
            //CardIds cardIds = heritageRepository.GetLevelsAsIntList(id);
            //HeritageViewModel model = heritageRepository.GetHerEditModelByLevels(cardIds);

            //model.user_id = 2;
            //model.lName = "albert";
            return View("SetHerPermission", model_);
        }
        #endregion
    }


}
