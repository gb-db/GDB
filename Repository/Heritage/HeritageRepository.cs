using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using Models.Classes;
using Models.Enums;
using DataLibrary.Context;
using Models.ViewModels;
using Newtonsoft.Json;
using Models.Heritage;
using System.Transactions;
using Microsoft.AspNetCore.Hosting;
using Repository.Utility;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace Repository.Heritage
{
    public interface IHeritageRepository
    {
        void ModifyImage(string path, int width, int height, ImageFormat imageFormat);
        HerRepData ModifyImageStrict(string path, int width, int height, string folder, ImageFormat imageFormat_, ImageModificationsType imageModificationsType);

        #region DB Access Methods
        RetHerUpdate InsertHerInformation(HeritageViewModel heritageViewModel);
        List<LabelValue> GetAutocomplateList(string term, string className, string allTextBoxIds);
        List<ItemInfo> GetAutocomplateListMain(string term, string className, string allTextBoxIds);
        int InsertNewCountry(string name);
        int InsertNewProvince(string name, int countryId);
        int InsertNewCity(string name, int countryId, int provinceId);
        int InsertNewChurch(string name, int countryId, int provinceId, int cityId);
        HeritageViewModel GetHerEditModelByLevels(CardIds cardIds);
        HeritageViewModel ConvertDateTimes(HeritageViewModel heritageViewModel);
        Models.Classes.Heritage GetHeritageById(int user_id, int herUserId);
        RetSimple DeleteHeritageLineFrom(DeleteData deleteData);
        RetSimple UpdateDBWithPath(UploadPhotoViewModel herRepData);
        RetSimple UpdateDBWithImageInfo(UploadPhotoViewModel uploadData);
        CardIds GetLevelsAsIntList(string id);
        bool DeleteTempFolderFiles(string path);
        RetSimple SaveCommentToDB(s_tr textarea);
        List<InfoItem> GetImageCommentListByIds(string card_id);
        RetSimple DeleteImageComment(s_tr deleteData, string folderPath);
        RetSimple RemovePhoto(string card_id, string folderPath, string emptyPhoto);
        AppSettings GetAppSettings(IConfiguration _config);
        s_strings GetRootImageNamesByIds(string card_id);
        #endregion
    }

    public class HeritageRepository : IHeritageRepository
    {
        AppIdentityDbContext context = null;
        private IHostingEnvironment _environment;
        public ImageFormat imageFormat = ImageFormat.Png;

        public HeritageRepository(AppIdentityDbContext cx, IHostingEnvironment env)
        {
            context = cx;
            _environment = env;
        }

        //ModifyImage(path, 40, 55, imageFormat)                         
        public void ModifyImage(string path, int width, int height, ImageFormat imageFormat_)
        {
            imageFormat_ = imageFormat;
            FileStream fs = null;
            try
            {
                using (fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                using (Stream fileNew = System.IO.File.Create(ChangeFileNameExt(path, width, height, imageFormat_.ToString())))
                {
                    Image image = Image.FromStream(fs);
                    Size sizeOld = image.Size;
                    Image imageNew = resizeImage(image, new Size() { Width = width, Height = height }, sizeOld);
                    Stream outputStream = ConvertImage(imageNew, imageFormat_);

                    outputStream.CopyTo(fileNew);
                    int i = 0;
                }
            }
            catch (Exception ex)
            {
                int i = 0;
            }
        }

        public HerRepData ModifyImageStrict(string path, int width, int height, string folder, ImageFormat imageFormat_, ImageModificationsType imageModificationsType)
        {
            imageFormat_ = imageFormat;
            HerRepData herRepData = new HerRepData();
            FileStream fs = null;
            try
            {
                string retPath = ChangeFileNameExtStrict(path, width, height, folder, imageFormat_.ToString());
                using (fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                using (Stream fileNew = System.IO.File.Create(retPath))
                {
                    Bitmap bitmap = new Bitmap(fs);
                    //PropertyItem pi = bitmap.PropertyItems.Select(x => x).FirstOrDefault(x => x.Id == 0x0112);
                    Rotate(bitmap);
                    Image image = bitmap;
                    Size sizeOld = image.Size;
                    herRepData = resizeImageStrict(image, new Size() { Width = width, Height = height }, sizeOld, imageModificationsType);
                    Stream outputStream = ConvertImage(herRepData.image, imageFormat_);
                    outputStream.CopyTo(fileNew);
                    herRepData.path = retPath;

                    return herRepData;
                }
            }
            catch (Exception ex)
            {
                herRepData.message = "There was problem to modify image.";

                return herRepData;
            }
        }

        public RetHerUpdate InsertHerInformation(HeritageViewModel heritageViewModel)
        {
            RetHerUpdate retSimple = new RetHerUpdate { retId = 0, message = "", success = false };
            her heritage = JsonConvert.DeserializeObject<her>(heritageViewModel.data_information);

            RetHerClasses retHerClasses = CheckHerExistance(heritage);

            //if (heritage?.parent_level == 0 && heritage?.parent_number == 0 && heritage?.level == 0 && heritage?.number == 0)
            if (retHerClasses.heratigeUser == null)
            {
                #region retHerClasses.heratigeUser == null
                HeratigeUser heratigeUser = new HeratigeUser();
                HeratigeData heratigeData = new HeratigeData();
                HeratigePermission heratigePermission = new HeratigePermission();
                string her_birthDate = "";
                string her_endDate = "";

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    heratigeUser.user_id = heritage.user_id;
                    heratigeUser.lName = heritageViewModel.lName;
                    heratigeUser.fName = heritageViewModel.fName;
                    //heratigeUser.UserCode = heritageViewModel.PWD;
                    heratigeUser.isActive = true;

                    context.Add<HeratigeUser>(heratigeUser);

                    try
                    {
                        context.SaveChanges();
                        int heratigeUserId = heratigeUser.HeratigeUserId;

                        if (heratigeUserId != 0)
                        {
                            heratigeData.HeratigeUserId = heratigeUserId;
                            heratigeData.user_id = heritage.user_id;
                            heratigeData.parent_level = heritage?.parent_level ?? default(int);
                            heratigeData.parent_number = heritage?.parent_number ?? default(int);
                            heratigeData.level = heritage?.level ?? default(int);
                            heratigeData.number = heritage?.number ?? default(int);
                            heratigeData.DOB = heritageViewModel.DOB;
                            her_birthDate = heritageViewModel.DOB != null ? heritageViewModel.DOB.Value.Year.ToString() : null;
                            heratigeData.PWD = heritageViewModel.PWD;
                            her_endDate = heritageViewModel.PWD != null ? heritageViewModel.PWD.Value.Year.ToString() : null;
                            Country Country_ = context.Country.Where(t => t.CountryId == heritageViewModel.countryId).Select(t => t).FirstOrDefault();
                            if (Country_ != null)
                            {
                                heratigeData.country = context.Country.Where(t => t.CountryId == heritageViewModel.countryId).Select(t => t).FirstOrDefault();
                            }

                            Province Province_ = context.Province.Where(t => t.provinceId == heritageViewModel.provinceId).Select(t => t).FirstOrDefault();
                            if (Province_ != null)
                            {
                                heratigeData.province = context.Province.Where(t => t.provinceId == heritageViewModel.provinceId).Select(t => t).FirstOrDefault();
                            }

                            City City_ = context.City.Where(t => t.cityId == heritageViewModel.cityId).Select(t => t).FirstOrDefault();
                            if (City_ != null)
                            {
                                heratigeData.city = context.City.Where(t => t.cityId == heritageViewModel.cityId).Select(t => t).FirstOrDefault();
                            }

                            Church Church_ = context.Church.Where(t => t.churchId == heritageViewModel.churchId).Select(t => t).FirstOrDefault();
                            if (Church_ != null)
                            {
                                heratigeData.church = context.Church.Where(t => t.churchId == heritageViewModel.churchId).Select(t => t).FirstOrDefault();
                            }
                            heratigeData.fName = heritageViewModel.fName;
                            heratigeData.lName = heritageViewModel.lName;
                            heratigeData.fNameParentF = heritageViewModel.fNameParentF;
                            heratigeData.lNameParentF = heritageViewModel.lNameParentF;
                            heratigeData.fNameParentM = heritageViewModel.fNameParentM;
                            heratigeData.lNameParentM = heritageViewModel.lNameParentM;
                            heratigeData.information = heritageViewModel.information;
                            heratigeData.ImagePath = heritage?.her_src;
                            heratigeData.pattern = heritage?.pattern;
                            heratigeData.isActive = true;
                            heratigeData.tt_width = 55;
                            heratigeData.tt_height = 40;


                            context.Add<HeratigeData>(heratigeData);
                            //context.SaveChanges();

                            heratigePermission.HeratigeUserId = heratigeUserId;
                            heratigePermission.isReadable = true;
                            heratigePermission.isWritable = true;
                            heratigePermission.permit = true;
                            heratigePermission.dateTime = DateTime.Now;
                            heratigePermission.user_id = heritage.user_id;

                            context.Add<HeratigePermission>(heratigePermission);
                            context.SaveChanges();

                        }

                        scope.Complete();

                        retSimple.success = true;
                        retSimple.heratigeUser_Id = heratigeUserId.ToString();
                        retSimple.her_fn = heritageViewModel.fName;
                        retSimple.her_ln = heritageViewModel.lName;
                        retSimple.her_birthDate = her_birthDate;
                        retSimple.her_endDate = her_endDate;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                        string methodName = st.GetFrame(0).GetMethod().Name;

                        string Msg = methodName + " ### " + ex.Message + " ### RetSimple InsertHerInformation(HeritageViewModel heritageViewModel)";
                        if (ex.InnerException != null)
                            Msg += ex.InnerException.Message;

                        Helper helper = new Helper(_environment);
                        helper.WriteToLog(Msg);

                        retSimple.message = "There was problem to insert information( heratigeData, heratigeUser ).";
                    }
                }
                #endregion
            }
            else
            {
                #region retHerClasses.HeratigeData == null
                if (retHerClasses.HeratigeData == null)
                {
                    HeratigeData heratigeData = new HeratigeData();
                    string her_birthDate = "";
                    string her_endDate = "";

                    int heratigeUserId = retHerClasses.heratigeUser.HeratigeUserId;

                    if (heratigeUserId != 0)
                    {
                        heratigeData.HeratigeUserId = heratigeUserId;
                        heratigeData.user_id = heritage.user_id;
                        heratigeData.parent_level = heritage?.parent_level ?? default(int);
                        heratigeData.parent_number = heritage?.parent_number ?? default(int);
                        heratigeData.level = heritage?.level ?? default(int);
                        heratigeData.number = heritage?.number ?? default(int);
                        if (heritageViewModel.DOB != null)
                        {
                            heratigeData.DOB = heritageViewModel.DOB;
                            DateTime dt = heritageViewModel.DOB ?? default(DateTime);
                            her_birthDate = dt.Year.ToString();
                        }
                        if (heritageViewModel.PWD != null)
                        {
                            heratigeData.PWD = heritageViewModel.PWD;
                            DateTime dt = heritageViewModel.PWD ?? default(DateTime);
                            her_endDate = dt.Year.ToString();
                        }
                        //
                        heratigeData.CountryId = heritageViewModel.countryId;
                        //heratigeData.province = context.Province.Where(t => t.provinceId == heritageViewModel.provinceId).Select(t => t).FirstOrDefault();
                        //heratigeData.city = context.City.Where(t => t.cityId == heritageViewModel.cityId).Select(t => t).FirstOrDefault();
                        //heratigeData.church = context.Church.Where(t => t.churchId == heritageViewModel.churchId).Select(t => t).FirstOrDefault();

                        if (heritageViewModel.provinceId != 0)
                        {
                            heratigeData.provinceId = heritageViewModel.provinceId;
                        }
                        if (heritageViewModel.cityId != 0)
                        {
                            heratigeData.cityId = heritageViewModel.cityId;
                        }
                        if (heritageViewModel.churchId != 0)
                        {
                            heratigeData.churchId = heritageViewModel.churchId;
                        }

                        if (heritageViewModel.countryBPId != 0)
                        {
                            heratigeData.CountryBPId = heritageViewModel.countryBPId;
                        }
                        if (heritageViewModel.provinceBPId != 0)
                        {
                            heratigeData.provinceBPId = heritageViewModel.provinceBPId;
                        }
                        if (heritageViewModel.cityBPId != 0)
                        {
                            heratigeData.cityBPId = heritageViewModel.cityBPId;
                        }

                        heratigeData.fName = heritageViewModel.fName;
                        heratigeData.lName = heritageViewModel.lName;
                        heratigeData.fNameParentF = heritageViewModel.fNameParentF;
                        heratigeData.lNameParentF = heritageViewModel.lNameParentF;
                        heratigeData.fNameParentM = heritageViewModel.fNameParentM;
                        heratigeData.lNameParentM = heritageViewModel.lNameParentM;
                        heratigeData.information = heritageViewModel.information;
                        heratigeData.ImagePath = heritage?.her_src;
                        heratigeData.pattern = heritage?.pattern;
                        heratigeData.isActive = true;
                        heratigeData.tt_width = 55;
                        heratigeData.tt_height = 40;


                        context.Add<HeratigeData>(heratigeData);
                        try
                        {
                            context.SaveChanges();

                            retSimple.success = true;
                            retSimple.heratigeUser_Id = heratigeUserId.ToString();
                            retSimple.her_fn = heritageViewModel.fName;
                            retSimple.her_ln = heritageViewModel.lName;
                            retSimple.her_birthDate = her_birthDate;
                            retSimple.her_endDate = her_endDate;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                            string methodName = st.GetFrame(0).GetMethod().Name;

                            string Msg = methodName + " ### " + ex.Message + " ### RetSimple InsertHerInformation(HeritageViewModel heritageViewModel)    else";
                            if (ex.InnerException != null)
                                Msg += ex.InnerException.Message;

                            Helper helper = new Helper(_environment);
                            helper.WriteToLog(Msg);

                            retSimple.message = "There was problem to insert information( heratigeData ).";
                        }
                    }
                    #endregion
                }
                else//then it is an update of heritage
                {
                    HeratigeData heratigeData = retHerClasses.HeratigeData;
                    string her_birthDate = "";
                    string her_endDate = "";

                    int heratigeUserId = retHerClasses.heratigeUser.HeratigeUserId;

                    if (heratigeUserId != 0)
                    {
                        heratigeData.HeratigeUserId = heratigeUserId;
                        heratigeData.user_id = heritage.user_id;
                        heratigeData.parent_level = heritage?.parent_level ?? default(int);
                        heratigeData.parent_number = heritage?.parent_number ?? default(int);
                        heratigeData.level = heritage?.level ?? default(int);
                        heratigeData.number = heritage?.number ?? default(int);
                        if (heritageViewModel.DOB != null)
                        {
                            heratigeData.DOB = heritageViewModel.DOB;
                            DateTime dt = heritageViewModel.DOB ?? default(DateTime);
                            her_birthDate = dt.Year.ToString();
                        }
                        if (heritageViewModel.PWD != null)
                        {
                            heratigeData.PWD = heritageViewModel.PWD;
                            DateTime dt = heritageViewModel.PWD ?? default(DateTime);
                            her_endDate = dt.Year.ToString();
                        }
                        //
                        heratigeData.CountryId = heritageViewModel.countryId;
                        //heratigeData.province = context.Province.Where(t => t.provinceId == heritageViewModel.provinceId).Select(t => t).FirstOrDefault();
                        //heratigeData.city = context.City.Where(t => t.cityId == heritageViewModel.cityId).Select(t => t).FirstOrDefault();
                        //heratigeData.church = context.Church.Where(t => t.churchId == heritageViewModel.churchId).Select(t => t).FirstOrDefault();
                        if (heritageViewModel.provinceId != 0)
                        {
                            heratigeData.provinceId = heritageViewModel.provinceId;
                        }
                        if (heritageViewModel.cityId != 0)
                        {
                            heratigeData.cityId = heritageViewModel.cityId;
                        }
                        if (heritageViewModel.churchId != 0)
                        {
                            heratigeData.churchId = heritageViewModel.churchId;
                        }

                        if (heritageViewModel.countryBPId != 0)
                        {
                            heratigeData.CountryBPId = heritageViewModel.countryBPId;
                        }
                        if (heritageViewModel.provinceBPId != 0)
                        {
                            heratigeData.provinceBPId = heritageViewModel.provinceBPId;
                        }
                        if (heritageViewModel.cityBPId != 0)
                        {
                            heratigeData.cityBPId = heritageViewModel.cityBPId;
                        }


                        heratigeData.fName = heritageViewModel.fName;
                        heratigeData.lName = heritageViewModel.lName;
                        heratigeData.fNameParentF = heritageViewModel.fNameParentF;
                        heratigeData.lNameParentF = heritageViewModel.lNameParentF;
                        heratigeData.fNameParentM = heritageViewModel.fNameParentM;
                        heratigeData.lNameParentM = heritageViewModel.lNameParentM;
                        heratigeData.information = heritageViewModel.information;
                        heratigeData.ImagePath = heritage?.her_src;
                        heratigeData.pattern = heritage?.pattern;
                        heratigeData.isActive = true;


                        //context.Add<HeratigeData>(heratigeData);
                        try
                        {
                            context.SaveChanges();

                            retSimple.success = true;
                            retSimple.heratigeUser_Id = heratigeUserId.ToString();
                            retSimple.her_fn = heritageViewModel.fName;
                            retSimple.her_ln = heritageViewModel.lName;
                            retSimple.her_birthDate = her_birthDate;
                            retSimple.her_endDate = her_endDate;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                            string methodName = st.GetFrame(0).GetMethod().Name;

                            string Msg = methodName + " ### " + ex.Message + " ### RetSimple InsertHerInformation(HeritageViewModel heritageViewModel)    else";
                            if (ex.InnerException != null)
                                Msg += ex.InnerException.Message;

                            Helper helper = new Helper(_environment);
                            helper.WriteToLog(Msg);

                            retSimple.message = "There was problem to insert information( heratigeData ).";
                        }
                    }
                }
            }

            return retSimple;
        }

        public List<LabelValue> GetAutocomplateList(string term, string className, string allTextBoxIds)
        {
            AllTextBoxIds allIds = GetAllTextBoxIds(allTextBoxIds);
            term = term.Trim().ToLower();
            List<LabelValue> list = null;
            switch (className)
            {
                case "country":
                case "countryBP":
                    list = context.Country.Where(t => t.Name.ToLower().StartsWith(term)).
                        Select(tt => new LabelValue { label = tt.Name, value = tt.CountryId.ToString() }).ToList();
                    break;
                case "province":
                    list = context.Province.Where(t => t.Name.StartsWith(term) && t.country.CountryId == allIds.countryId).
                        Select(tt => new LabelValue { label = tt.Name, value = tt.provinceId.ToString() }).ToList();
                    break;
                case "provinceBP":
                    list = context.Province.Where(t => t.Name.StartsWith(term) && t.country.CountryId == allIds.countryBPId).
                        Select(tt => new LabelValue { label = tt.Name, value = tt.provinceId.ToString() }).ToList();
                    break;
                case "city":
                    list = context.City.Where(t => t.Name.StartsWith(term) && t.country.CountryId == allIds.countryId && t.province.provinceId == allIds.provinceId).
                        Select(tt => new LabelValue { label = tt.Name, value = tt.cityId.ToString() }).ToList();
                    break;
                case "cityBP":
                    list = context.City.Where(t => t.Name.StartsWith(term) && t.country.CountryId == allIds.countryBPId && t.province.provinceId == allIds.provinceBPId).
                        Select(tt => new LabelValue { label = tt.Name, value = tt.cityId.ToString() }).ToList();
                    break;
                case "church":
                    list = context.Church.Where(t => t.Name.StartsWith(term) && t.country.CountryId == allIds.countryId && t.province.provinceId == allIds.provinceId && t.city.cityId == allIds.cityId)
                        .Select(tt => new LabelValue { label = tt.Name, value = tt.churchId.ToString() }).ToList();
                    break;
                case "userSearch":
                    list = (from u in context.HeratigeUsers
                               join hu in context.HeratigeDatas on u.HeratigeUserId equals hu.HeratigeUserId
                               where (hu.lName ?? "" + ", " + hu.fName ?? "").StartsWith(term)
                               select  new LabelValue {
                                   label = (hu.lName ?? "" + ", " + hu.fName ?? ""),
                                   value = hu.HeratigeUserId.ToString()
                               }).ToList();
                    //.Where(t => (t.lName ?? "" + ", " + t.fName ?? "").StartsWith(term)).Select(tt => new LabelValue { label = (tt.lName ?? "" + ", " + tt.fName ?? ""), value = tt.HeratigeUserId.ToString() }).ToList();

                    //list = context.HeratigeUsers.Where(t => (t.lName??"" + ", " + t.fName??"").StartsWith(term)).Select(tt => new LabelValue { label = (tt.lName??"" + ", " + tt.fName??""), value = tt.HeratigeUserId.ToString() }).ToList();
                    break;
                default:
                    break;
            }

            return list;
        }

        public List<ItemInfo> GetAutocomplateListMain(string term, string className, string allTextBoxIds)
        {
            //countryId_provinceId_cityId_churchId_countryBPId_provinceBPId_cityBPId
            AllTextBoxIds allIds = GetAllTextBoxIds(allTextBoxIds);
            term = term.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(term))
            {
                return null;
            }
            List<string> termList = term.Split(new string[] { "," }, StringSplitOptions.None).ToList();
            string country = termList[0];
            string province = termList.Count > 1 ? (termList[1]).Trim() : "";
            string city = termList.Count > 2 ? (termList[2]).Trim() : "";
            string lName = (termList.Count > 3 ? (termList[3]).Trim() : "");// + ", " + (termList.Count > 4 ? (termList[4]).Trim() : "");
            string fName = (termList.Count > 4 ? (termList[4]).Trim() : "");
            string names = lName + (fName != "" ? ", " + fName : "");

            List<ItemInfo> list = null;
            switch (className)
            {
                case "mainSearch":
                    if (province != "")
                    {
                        if (city != "")
                        {
                            if (names.Trim() != ",")
                            {
                                #region Country-province-city-names
                                list = (from u in context.HeratigeDatas
                                        join cnt in context.Country on u.CountryId equals cnt.CountryId
                                        join pr in context.Province on u.provinceId equals pr.provinceId
                                        join ci in context.City on u.cityId equals ci.cityId
                                        where (cnt.Name).StartsWith(country, StringComparison.InvariantCultureIgnoreCase) &&
                                        u.parent_level == 0 && u.parent_number == 0 && u.level == 0 && u.number == 0
                                        && pr.Name.StartsWith(province, StringComparison.InvariantCultureIgnoreCase)
                                        && ci.Name.StartsWith(city, StringComparison.InvariantCultureIgnoreCase)
                                        && ((u.lName ?? "") + ", " + (u.fName ?? "")).StartsWith(names, StringComparison.InvariantCultureIgnoreCase)
                                        select new ItemInfo
                                        {
                                            country = cnt.Name,
                                            flagImgPath = cnt.imgPath,
                                            province = pr.Name ?? "-",
                                            city = ci.Name ?? "-",
                                            countryBP = context.Country.Where(t => t.CountryId == u.CountryBPId).Select(tt => tt.Name).FirstOrDefault() ?? "-",
                                            provinceBP = context.Province.Where(t => t.provinceId == u.provinceBPId).Select(tt => tt.Name).FirstOrDefault() ?? "-",
                                            flagImgPathBP = context.Country.Where(t => t.CountryId == u.CountryBPId).Select(tt => tt.imgPath).FirstOrDefault() ?? "-",
                                            cityBP = context.City.Where(t => t.cityId == u.cityBPId).Select(tt => tt.Name).FirstOrDefault() ?? "-",
                                            heritageIds = "" + u.user_id.ToString() + "_" + u.HeratigeUserId.ToString(),
                                            isOpend = context.HeratigePermissions.Where(t => t.HeratigeUserId == u.HeratigeUserId && t.isRoot == true).Select(tt=> tt.permit).FirstOrDefault(),//permit is for any
                                            isEditable = context.HeratigePermissions.Where(t => t.HeratigeUserId == u.HeratigeUserId && t.user_id == allIds.user_id).Select(tt => tt.isWritable).FirstOrDefault(),
                                            LN_FN = (u.lName ?? "-") + " " + (u.fName ?? "-")
                                        }).ToList();
                                #endregion
                            }
                            else
                            {
                                #region Country-province-city
                                list = (from u in context.HeratigeDatas
                                        join cnt in context.Country on u.CountryId equals cnt.CountryId
                                        join pr in context.Province on u.provinceId equals pr.provinceId
                                        join ci in context.City on u.cityId equals ci.cityId
                                        where (cnt.Name).StartsWith(country, StringComparison.InvariantCultureIgnoreCase)
                                        && pr.Name.StartsWith(province, StringComparison.InvariantCultureIgnoreCase)
                                        && ci.Name.StartsWith(city, StringComparison.InvariantCultureIgnoreCase) &&
                                        u.parent_level == 0 && u.parent_number == 0 && u.level == 0 && u.number == 0
                                        select new ItemInfo
                                        {
                                            country = cnt.Name,
                                            flagImgPath = cnt.imgPath,
                                            province = pr.Name ?? "-",
                                            city = ci.Name ?? "-",
                                            countryBP = context.Country.Where(t => t.CountryId == u.CountryBPId).Select(tt => tt.Name).FirstOrDefault() ?? "-",
                                            flagImgPathBP = context.Country.Where(t => t.CountryId == u.CountryBPId).Select(tt => tt.imgPath).FirstOrDefault() ?? "-",
                                            provinceBP = context.Province.Where(t => t.provinceId == u.provinceBPId).Select(tt => tt.Name).FirstOrDefault() ?? "-",
                                            cityBP = context.City.Where(t => t.cityId == u.cityBPId).Select(tt => tt.Name).FirstOrDefault() ?? "-",
                                            heritageIds = "" + u.user_id.ToString() + "_" + u.HeratigeUserId.ToString(),
                                            isOpend = context.HeratigePermissions.Where(t => t.HeratigeUserId == u.HeratigeUserId && t.isRoot == true).Select(tt => tt.permit).FirstOrDefault(),//permit is for any
                                            isEditable = context.HeratigePermissions.Where(t => t.HeratigeUserId == u.HeratigeUserId && t.user_id == allIds.user_id).Select(tt => tt.isWritable).FirstOrDefault(),
                                            LN_FN = (u.lName ?? "-") + " " + (u.fName ?? "-")
                                        }).ToList();
                                #endregion
                            }
                        }
                        else
                        {
                            #region Country-province
                            list = (from u in context.HeratigeDatas
                                    join cnt in context.Country on u.CountryId equals cnt.CountryId
                                    join pr in context.Province on u.provinceId equals pr.provinceId
                                    join ci in context.City on u.cityId equals ci.cityId
                                    where (cnt.Name).StartsWith(country, StringComparison.InvariantCultureIgnoreCase)
                                            && pr.Name.StartsWith(province, StringComparison.InvariantCultureIgnoreCase) &&
                                        u.parent_level == 0 && u.parent_number == 0 && u.level == 0 && u.number == 0
                                    select new ItemInfo
                                    {
                                        country = cnt.Name,
                                        flagImgPath = cnt.imgPath,
                                        province = pr.Name ?? "-",
                                        city = ci.Name ?? "-",
                                        countryBP = context.Country.Where(t => t.CountryId == u.CountryBPId).Select(tt => tt.Name).FirstOrDefault() ?? "-",
                                        flagImgPathBP = context.Country.Where(t => t.CountryId == u.CountryBPId).Select(tt => tt.imgPath).FirstOrDefault() ?? "-",
                                        provinceBP = context.Province.Where(t => t.provinceId == u.provinceBPId).Select(tt => tt.Name).FirstOrDefault() ?? "-",
                                        cityBP = context.City.Where(t => t.cityId == u.cityBPId).Select(tt => tt.Name).FirstOrDefault() ?? "-",
                                        heritageIds = "" + u.user_id.ToString() + "_" + u.HeratigeUserId.ToString(),
                                        isOpend = context.HeratigePermissions.Where(t => t.HeratigeUserId == u.HeratigeUserId && t.isRoot == true).Select(tt => tt.permit).FirstOrDefault(),//permit is for any
                                        isEditable = context.HeratigePermissions.Where(t => t.HeratigeUserId == u.HeratigeUserId && t.user_id == allIds.user_id).Select(tt => tt.isWritable).FirstOrDefault(),
                                        LN_FN = (u.lName ?? "-") + " " + (u.fName ?? "-")
                                    }).ToList();
                            #endregion

                        }
                    }
                    else
                    {
                        #region Country
                        list = (from u in context.HeratigeDatas
                                join cnt in context.Country on u.CountryId equals cnt.CountryId
                                join pr in context.Province on u.provinceId equals pr.provinceId
                                join ci in context.City on u.cityId equals ci.cityId
                                where cnt.Name.StartsWith(country, StringComparison.InvariantCultureIgnoreCase) &&
                                        u.parent_level == 0 && u.parent_number == 0 && u.level == 0 && u.number == 0
                                select new ItemInfo
                                {
                                    country = cnt.Name,
                                    flagImgPath = cnt.imgPath,
                                    province = pr.Name ?? "-",
                                    city = ci.Name ?? "-",
                                    countryBP = context.Country.Where(t => t.CountryId == u.CountryBPId).Select(tt => tt.Name).FirstOrDefault() ?? "-",
                                    flagImgPathBP = context.Country.Where(t => t.CountryId == u.CountryBPId).Select(tt => tt.imgPath).FirstOrDefault() ?? "-",
                                    provinceBP = context.Province.Where(t => t.provinceId == u.provinceBPId).Select(tt => tt.Name).FirstOrDefault() ?? "-",
                                    cityBP = context.City.Where(t => t.cityId == u.cityBPId).Select(tt => tt.Name).FirstOrDefault() ?? "-",
                                    heritageIds = "" + u.user_id.ToString() + "_" + u.HeratigeUserId.ToString(),
                                    isOpend = context.HeratigePermissions.Where(t => t.HeratigeUserId == u.HeratigeUserId && t.isRoot == true).Select(tt => tt.permit).FirstOrDefault(),//permit is for any
                                    isEditable = context.HeratigePermissions.Where(t => t.HeratigeUserId == allIds.heratigeUser_Id && t.user_id == allIds.user_id).Select(tt => tt.isWritable).FirstOrDefault(),
                                    LN_FN = (u.lName ?? "-") + " " + (u.fName ?? "-")
                                }).ToList();
                        #endregion
                    }
                    break;
                default:
                    break;
            }

            return list;
        }

        public HeritageViewModel GetHerEditModelByLevels(CardIds cardIds)
        {//userId(0)_heratigeUser_Id(1)_parentLevel(2)_parentNumber(3)_level(4)_number(5)
            HeratigeUser heratigeUser = null;
            HeratigeData heratigeData = null;
            HeritageViewModel heritageViewModel = new HeritageViewModel();

            if (cardIds.heratigeUser_Id > 0)
            {
                heratigeUser = context.HeratigeUsers.Where(t => t.HeratigeUserId == cardIds.heratigeUser_Id).FirstOrDefault();
            }

            if (heratigeUser != null)
            {
                heratigeData = context.HeratigeDatas.Where(t => t.user_id == heratigeUser.user_id &&
                   t.HeratigeUserId == heratigeUser.HeratigeUserId &&
                   t.parent_level == cardIds.parentLevel && t.parent_number == cardIds.parentNumber &&
                   t.level == cardIds.level && t.number == cardIds.number
                   ).FirstOrDefault();

                if (heratigeData != null)
                {
                    heritageViewModel.fName = heratigeData.fName;
                    heritageViewModel.lName = heratigeData.lName;
                    heritageViewModel.DOB_str = heratigeData.DOB == null ? null : heratigeData.DOB.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    heritageViewModel.PWD_str = heratigeData.PWD == null ? null : heratigeData.PWD.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //heritageViewModel.country = heratigeData.country.Name;
                    if (heratigeData.CountryId != 0)
                    {
                        heritageViewModel.country = context.Country.Where(t => t.CountryId == heratigeData.CountryId).Select(tt => tt.Name).FirstOrDefault();
                    }

                    heritageViewModel.countryId = heratigeData.CountryId;
                    if (heratigeData.provinceId != null && heratigeData.provinceId != 0)
                    {
                        heritageViewModel.province = context.Province.Where(t => t.provinceId == heratigeData.provinceId).Select(tt => tt.Name).FirstOrDefault();
                    }
                    heritageViewModel.provinceId = heratigeData.provinceId ?? 0;

                    if (heratigeData.cityId != null && heratigeData.cityId != 0)
                    {
                        heritageViewModel.city = context.City.Where(t => t.cityId == heratigeData.cityId).Select(tt => tt.Name).FirstOrDefault();
                    }

                    heritageViewModel.cityId = heratigeData.cityId ?? 0;

                    if (heratigeData.churchId != null && heratigeData.churchId != 0)
                    {
                        heritageViewModel.church = context.Church.Where(t => t.churchId == heratigeData.churchId).Select(tt => tt.Name).FirstOrDefault();
                    }

                    heritageViewModel.churchId = heratigeData.churchId ?? 0;


                    if (heratigeData.CountryBPId != null && heratigeData.CountryBPId != 0)
                    {
                        heritageViewModel.countryBP = context.Country.Where(t => t.CountryId == heratigeData.CountryBPId).Select(tt => tt.Name).FirstOrDefault();
                    }
                    heritageViewModel.countryBPId = heratigeData.CountryBPId ?? 0;

                    if (heratigeData.provinceBPId != null && heratigeData.provinceBPId != 0)
                    {
                        heritageViewModel.provinceBP = context.Province.Where(t => t.provinceId == heratigeData.provinceBPId).Select(tt => tt.Name).FirstOrDefault();
                    }
                    heritageViewModel.provinceBPId = heratigeData.provinceBPId ?? 0;

                    if (heratigeData.cityBPId != null && heratigeData.cityBPId != 0)
                    {
                        heritageViewModel.cityBP = context.City.Where(t => t.cityId == heratigeData.cityBPId).Select(tt => tt.Name).FirstOrDefault();
                    }
                    heritageViewModel.cityBPId = heratigeData.cityBPId ?? 0;



                    heritageViewModel.lNameParentF = heratigeData.lNameParentF;
                    heritageViewModel.fNameParentF = heratigeData.fNameParentF;
                    heritageViewModel.lNameParentM = heratigeData.lNameParentM;
                    heritageViewModel.fNameParentM = heratigeData.fNameParentM;
                    heritageViewModel.information = heratigeData.information;
                }

            }


            return heritageViewModel;
        }

        public Models.Classes.Heritage GetHeritageById(int user_id, int herUserId)
        {
            Models.Classes.Heritage heritageRet = new Models.Classes.Heritage();
            heritageRet.success = false;

            List<her> listHer = (from u in context.HeratigeUsers
                                 join d in context.HeratigeDatas on u.HeratigeUserId equals d.HeratigeUserId
                                 where u.HeratigeUserId == herUserId && u.user_id == user_id &&
                                          d.isActive == true && u.isActive == true
                                 select new her
                                 {
                                     user_id = d.user_id,
                                     heratigeUser_Id = d.HeratigeUserId,
                                     birthDate = d.DOB,
                                     endDate = d.PWD,
                                     her_fn = d.fName,
                                     her_ln = d.lName,
                                     her_src = d.ImagePath,
                                     id = d.Id.ToString(),
                                     level = d.level,
                                     number = d.number,
                                     isClosed = d.isClosed,
                                     isEditable = d.isEditable,
                                     parent_level = d.parent_level,
                                     parent_number = d.parent_number,
                                     pattern = d.pattern,
                                     tt_width = d.tt_width,
                                     tt_height = d.tt_height,
                                     flagImgPath = context.Country.Where(t => t.CountryId == d.CountryId).Select(tt => tt.imgPath).FirstOrDefault() ?? "",
                                     flagImgPathBP = context.Country.Where(t => t.CountryId == d.CountryBPId).Select(tt => tt.imgPath).FirstOrDefault() ?? "",
                                     countryName = context.Country.Where(t => t.CountryId == d.CountryId).Select(tt => tt.Name).FirstOrDefault() ?? "",
                                     countryNameBP = context.Country.Where(t => t.CountryId == d.CountryBPId).Select(tt => tt.Name).FirstOrDefault() ?? ""

                                 }).ToList();

            if (listHer != null && listHer.Count > 0)
            {
                her Her_0 = listHer.Where(d => d.level == 0 && d.number == 0 && d.parent_level == 0 && d.parent_number == 0).FirstOrDefault();
                if (Her_0 != null)
                {
                    List<her> retList = new List<her>();
                    listHer = GetUpdatedHerList(listHer);

                    listHer.Remove(Her_0);
                    retList.Add(Her_0);
                    retList.AddRange(listHer);

                    heritageRet.hers = retList;
                    List<List<her>> listArr = GetArrList(retList);
                    heritageRet.listArr = listArr;
                    heritageRet.success = true;
                }
                else
                {
                    heritageRet.message = "Main Heritage with supplied Id does not exists in db.";
                }
            }
            else
            {
                heritageRet.message = "Heritage line with supplied User Id does not exists in db.";
            }

            return heritageRet;
        }

        public RetSimple UpdateDBWithPath(UploadPhotoViewModel herRepData)
        {
            RetSimple retVal = new RetSimple() { message = "" };
            //userId_heratigeUser_Id_parentLevel_parentNumber_level_number                            
            CardIds cardIds = GetLevelsAsIntList(herRepData.heritageIds);
            int user_Id = cardIds.user_Id;
            int heratigeUser_Id = cardIds.heratigeUser_Id;
            int parentLevel = cardIds.parentLevel;
            int parentNumber = cardIds.parentNumber;
            int level = cardIds.level;
            int number = cardIds.number;

            HeratigeUser heratigeUser = context.HeratigeUsers.Where(t => t.HeratigeUserId == heratigeUser_Id && t.isActive == true).FirstOrDefault();
            if (heratigeUser == null)
            {
                retVal.message = " Heritage User does not exists in DB.";
                return retVal;
            }

            HeratigeData heratigeData = context.HeratigeDatas.Where(t => t.user_id == user_Id && t.HeratigeUserId == heratigeUser_Id && t.parent_level == parentLevel &&
                                        t.parent_number == parentNumber && t.level == level && t.number == number).FirstOrDefault();

            if (heratigeData != null)
            {
                heratigeData.ImagePath = herRepData.path;
                heratigeData.tt_width = herRepData.width;
                heratigeData.tt_height = herRepData.height;

                try
                {
                    context.SaveChanges();
                    retVal.number = heratigeData.Id;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                    string methodName = st.GetFrame(0).GetMethod().Name;

                    string Msg = methodName + " ### " + ex.Message + " ### RetSimple UpdateDBWithPath(UploadPhotoViewModel herRepData)";
                    if (ex.InnerException != null)
                        Msg += ex.InnerException.Message;

                    Helper helper = new Helper(_environment);
                    helper.WriteToLog(Msg);

                    retVal.message = "There was problem to update image path.";
                }
            }
            else
            {
                retVal.message = "There was problem to update photo path.";
            }



            return retVal;
        }

        public RetSimple UpdateDBWithImageInfo(UploadPhotoViewModel herImgData)
        {
            RetSimple retVal = new RetSimple() { message = "" };
            //userId_heratigeUser_Id_parentLevel_parentNumber_level_number                            
            CardIds cardIds = GetLevelsAsIntList(herImgData.heritageIds);
            int user_Id = cardIds.user_Id;
            int heratigeUser_Id = cardIds.heratigeUser_Id;
            int parentLevel = cardIds.parentLevel;
            int parentNumber = cardIds.parentNumber;
            int level = cardIds.level;
            int number = cardIds.number;

            HeratigeUser heratigeUser = context.HeratigeUsers.Where(t => t.HeratigeUserId == heratigeUser_Id &&
                                           t.isActive == true).FirstOrDefault();
            if (heratigeUser == null)
            {
                retVal.message = " Heritage User does not exists in DB.";
                return retVal;
            }

            HerInfoSrc herInfoSrc = context.HerInfoSrcs.Where(t => t.user_id == user_Id && t.HeratigeUserId == heratigeUser_Id && t.parent_level == parentLevel &&
                                        t.parent_number == parentNumber && t.level == level && t.number == number).FirstOrDefault();

            if (herInfoSrc != null)
            {
                #region MyRegion
                Info_Src info_Src = new Info_Src();
                info_Src.src = herImgData.path;
                info_Src.tt_width = herImgData.width;
                info_Src.tt_height = herImgData.height;
                info_Src.isActive = true;
                info_Src.HerInfoSrcId = herInfoSrc.HerInfoSrcId;

                context.Entry<Info_Src>(info_Src).State = EntityState.Added;

                try
                {
                    context.SaveChanges();
                    retVal.Id = info_Src.Id;
                    retVal.success = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                    string methodName = st.GetFrame(0).GetMethod().Name;

                    string Msg = methodName + " ### " + ex.Message + " ### RetSimple UpdateDBWithPath(UploadPhotoViewModel herRepData)";
                    if (ex.InnerException != null)
                        Msg += ex.InnerException.Message;

                    Helper helper = new Helper(_environment);
                    helper.WriteToLog(Msg);

                    retVal.message = "There was problem to update photo path.";
                }
                #endregion
            }
            else
            {
                #region MyRegion
                HerInfoSrc heratigeImage = new HerInfoSrc();
                heratigeImage.HeratigeUserId = heratigeUser.HeratigeUserId;
                heratigeImage.user_id = user_Id;
                heratigeImage.parent_level = parentLevel;
                heratigeImage.parent_number = parentNumber;
                heratigeImage.level = level;
                heratigeImage.number = number;
                heratigeImage.isActive = true;

                context.Entry<HerInfoSrc>(heratigeImage).State = EntityState.Added;

                try
                {
                    context.SaveChanges();

                    if (heratigeImage.HerInfoSrcId > 0)
                    {
                        Info_Src info_Src = new Info_Src();
                        info_Src.src = herImgData.path;
                        info_Src.tt_width = herImgData.width;
                        info_Src.tt_height = herImgData.height;
                        info_Src.isActive = true;
                        info_Src.HerInfoSrcId = heratigeImage.HerInfoSrcId;

                        context.Entry<Info_Src>(info_Src).State = EntityState.Added;
                        context.SaveChanges();

                        retVal.number = info_Src.Id;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                    string methodName = st.GetFrame(0).GetMethod().Name;

                    string Msg = methodName + " ### " + ex.Message + " ### RetSimple UpdateDBWithPath(UploadPhotoViewModel herRepData)   else";
                    if (ex.InnerException != null)
                        Msg += ex.InnerException.Message;

                    Helper helper = new Helper(_environment);
                    helper.WriteToLog(Msg);

                    retVal.message = "There was problem to update image path.";
                }
                #endregion
            }

            return retVal;
        }

        public RetSimple SaveCommentToDB(s_tr textarea)
        {
            RetSimple retVal = new RetSimple() { message = "" };
            //userId_heratigeUser_Id_parentLevel_parentNumber_level_number                            
            CardIds cardIds = GetLevelsAsIntList(textarea.heritageIds);
            int user_Id = cardIds.user_Id;
            int heratigeUser_Id = cardIds.heratigeUser_Id;
            int parentLevel = cardIds.parentLevel;
            int parentNumber = cardIds.parentNumber;
            int level = cardIds.level;
            int number = cardIds.number;

            HeratigeUser heratigeUser = context.HeratigeUsers.Where(t => t.HeratigeUserId == heratigeUser_Id).FirstOrDefault();
            if (heratigeUser == null)
            {
                retVal.message = " Heritage User does not exists in DB.";
                return retVal;
            }

            HerInfoSrc herInfoSrc = context.HerInfoSrcs.Where(t => t.user_id == user_Id && t.HeratigeUserId == heratigeUser_Id && t.parent_level == parentLevel &&
                                        t.parent_number == parentNumber && t.level == level && t.number == number).FirstOrDefault();

            if (herInfoSrc != null)
            {
                #region MyRegion
                Info_Src info_Src = new Info_Src();
                //info_Src.src = herImgData.path;
                //info_Src.tt_width = herImgData.width;
                //info_Src.tt_height = herImgData.height;
                info_Src.comment = textarea.str;
                info_Src.isActive = true;
                info_Src.HerInfoSrcId = herInfoSrc.HerInfoSrcId;

                context.Entry<Info_Src>(info_Src).State = EntityState.Added;

                try
                {
                    context.SaveChanges();
                    retVal.Id = info_Src.Id;
                    retVal.card_ids = textarea.heritageIds;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                    string methodName = st.GetFrame(0).GetMethod().Name;

                    string Msg = methodName + " ### " + ex.Message + " ### RetSimple SaveCommentToDB(s_tr textarea)";
                    if (ex.InnerException != null)
                        Msg += ex.InnerException.Message;

                    Helper helper = new Helper(_environment);
                    helper.WriteToLog(Msg);

                    retVal.message = "There was problem to update comment path.";
                }
                #endregion
            }
            else
            {
                #region MyRegion
                HerInfoSrc heratigeImage = new HerInfoSrc();
                heratigeImage.HeratigeUserId = heratigeUser.HeratigeUserId;
                heratigeImage.user_id = user_Id;
                heratigeImage.parent_level = parentLevel;
                heratigeImage.parent_number = parentNumber;
                heratigeImage.level = level;
                heratigeImage.number = number;
                heratigeImage.isActive = true;

                context.Entry<HerInfoSrc>(heratigeImage).State = EntityState.Added;

                try
                {
                    context.SaveChanges();

                    if (heratigeImage.HerInfoSrcId > 0)
                    {
                        Info_Src info_Src = new Info_Src();
                        info_Src.comment = textarea.str;
                        info_Src.isActive = true;
                        info_Src.HerInfoSrcId = heratigeImage.HerInfoSrcId;

                        context.Entry<Info_Src>(info_Src).State = EntityState.Added;
                        context.SaveChanges();

                        retVal.number = info_Src.Id;
                        retVal.card_ids = textarea.heritageIds;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                    string methodName = st.GetFrame(0).GetMethod().Name;

                    string Msg = methodName + " ### " + ex.Message + " ### RetSimple SaveCommentToDB(s_tr textarea)   else";
                    if (ex.InnerException != null)
                        Msg += ex.InnerException.Message;

                    Helper helper = new Helper(_environment);
                    helper.WriteToLog(Msg);

                    retVal.message = "There was problem to update comment path.";
                }
                #endregion
            }

            return retVal;
        }

        public List<InfoItem> GetImageCommentListByIds(string card_id)
        {
            List<InfoItem> retList = new List<InfoItem>();
            //userId_heratigeUser_Id_parentLevel_parentNumber_level_number                            
            CardIds cardIds = GetLevelsAsIntList(card_id);
            int user_Id = cardIds.user_Id;
            int heratigeUser_Id = cardIds.heratigeUser_Id;
            int parentLevel = cardIds.parentLevel;
            int parentNumber = cardIds.parentNumber;
            int level = cardIds.level;
            int number = cardIds.number;

            HeratigeUser heratigeUser = context.HeratigeUsers.Where(t => t.HeratigeUserId == heratigeUser_Id).FirstOrDefault();
            if (heratigeUser == null)
            {
                return retList;
            }

            HerInfoSrc herInfoSrc = context.HerInfoSrcs.Where(t => t.user_id == user_Id && t.HeratigeUserId == heratigeUser_Id && t.parent_level == parentLevel &&
                        t.parent_number == parentNumber && t.level == level && t.number == number && t.isActive == true).FirstOrDefault();
            if (herInfoSrc == null)
            {
                return retList;
            }

            List<Info_Src> info_Srcs = context.Info_Srcs.Where(t => t.HerInfoSrcId == herInfoSrc.HerInfoSrcId
                                                                    && t.isActive == true).OrderBy(t => t.Id).ToList();


            if (info_Srcs != null && info_Srcs.Count > 0)
            {
                foreach (var item in info_Srcs)
                {
                    InfoItem infoItem = new InfoItem();
                    if (!string.IsNullOrWhiteSpace(item.src))
                    {
                        infoItem.type = "image";
                        infoItem.path = item.src;
                        infoItem.width = item.tt_width;
                        infoItem.height = item.tt_height;
                        infoItem.order_number = item.Id;
                        infoItem.card_ids = card_id;
                    }
                    else if (!string.IsNullOrWhiteSpace(item.comment))
                    {
                        infoItem.type = "comment";
                        infoItem.comment = item.comment;
                        infoItem.order_number = item.Id;
                        infoItem.card_ids = card_id;
                    }

                    retList.Add(infoItem);
                }
            }

            return retList;
        }

        public s_strings GetRootImageNamesByIds(string card_id)
        {
            //userId_heratigeUser_Id_parentLevel_parentNumber_level_number                            
            CardIds cardIds = GetLevelsAsIntList(card_id);
            int user_Id = cardIds.user_Id;
            int heratigeUser_Id = cardIds.heratigeUser_Id;
            int parentLevel = cardIds.parentLevel;
            int parentNumber = cardIds.parentNumber;
            int level = cardIds.level;
            int number = cardIds.number;

            s_strings Her = (from u in context.HeratigeUsers
                             join d in context.HeratigeDatas on u.HeratigeUserId equals d.HeratigeUserId
                             where u.HeratigeUserId == heratigeUser_Id && u.user_id == user_Id &&
                                      d.parent_level == parentLevel && d.parent_number == parentNumber &&
                                      d.level == level && d.number == number
                             select new s_strings
                             {
                                 str1 = d.ImagePath,
                                 str2 = d.lName + " " + d.fName
                             }).FirstOrDefault();

            return Her;
        }
        #region Delete Records

        public RetSimple DeleteHeritageLineFrom(DeleteData deleteData)
        {
            RetSimple retSimple = new RetSimple();
            List<her> listHer = (from u in context.HeratigeUsers
                                 join d in context.HeratigeDatas on u.HeratigeUserId equals d.HeratigeUserId
                                 where u.HeratigeUserId == deleteData.heratigeUser_Id && u.user_id == deleteData.user_id &&
                                            d.level >= deleteData.level && d.pattern.StartsWith(deleteData.pattern) &&
                                          d.isActive == true && u.isActive == true
                                 select new her
                                 {
                                     intId = d.Id,
                                     user_id = d.user_id,
                                     heratigeUser_Id = d.HeratigeUserId,
                                     birthDate = d.DOB,
                                     endDate = d.PWD,
                                     her_fn = d.fName,
                                     her_ln = d.lName,
                                     her_src = d.ImagePath,
                                     id = d.Id.ToString(),
                                     level = d.level,
                                     number = d.number,
                                     //last_child = d.last_child,
                                     parent_level = d.parent_level,
                                     parent_number = d.parent_number,
                                     pattern = d.pattern
                                 }).ToList();

            List<int> intList = listHer.Select(t => t.intId).ToList();
            List<HeratigeData> forUpdateList = context.HeratigeDatas.Where(t => intList.Contains(t.Id)).ToList();
            //forUpdateList.ForEach(t => t.isActive = false);
            if (forUpdateList != null && forUpdateList.Count > 0)
            {
                context.HeratigeDatas.RemoveRange(forUpdateList); 
            }

            if (deleteData.level == 0 && deleteData.number == 0 && deleteData.parent_level == 0 && deleteData.parent_number == 0)
            {
                List<HeratigePermission> heratigePermissions = (from u in context.HeratigeUsers
                                                                join up in context.HeratigePermissions on u.HeratigeUserId equals up.HeratigeUserId
                                                                where u.HeratigeUserId == deleteData.heratigeUser_Id
                                                                select up).ToList();
                if (heratigePermissions != null && heratigePermissions.Count > 0)
                {
                    context.HeratigePermissions.RemoveRange(heratigePermissions);
                } 
            }

            int retInt = 0;

            try
            {
                retInt = context.SaveChanges();
                retSimple.retId = retInt;
                retSimple.success = true;
                retSimple.message = "" + retInt.ToString() + " decedants where deleted.";
            }
            catch (Exception ex)
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                string methodName = st.GetFrame(0).GetMethod().Name;

                string Msg = methodName + " ### " + ex.Message + " ### RetSimple DeleteHeritageLineFrom(DeleteData deleteData)";
                if (ex.InnerException != null)
                    Msg += ex.InnerException.Message;

                Helper helper = new Helper(_environment);
                helper.WriteToLog(Msg);

                retSimple.success = false;
                retSimple.message = "There was problem to insert information( heratigeData, heratigeUser ).";
            }

            return new RetSimple { number = retInt, success = true, message = "" + retInt.ToString() + " decedants where deleted." };
        }

        public bool DeleteTempFolderFiles(string path)
        {
            bool retVal = false;

            System.IO.DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                try
                {
                    file.Delete();
                    retVal = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                    string methodName = st.GetFrame(0).GetMethod().Name;

                    string Msg = methodName + " ### " + ex.Message + " ### bool DeleteTempFolderFiles(string path)";
                    if (ex.InnerException != null)
                        Msg += ex.InnerException.Message;

                    Helper helper = new Helper(_environment);
                    helper.WriteToLog(Msg);
                }
            }

            return retVal;
        }

        public RetSimple DeleteImageComment(s_tr deleteData_, string folderPath)
        {
            //userId_heratigeUser_Id_parentLevel_parentNumber_level_number                            
            CardIds cardIds = GetLevelsAsIntList(deleteData_.heritageIds);
            int user_Id = cardIds.user_Id;
            int heratigeUser_Id = cardIds.heratigeUser_Id;
            int parentLevel = cardIds.parentLevel;
            int parentNumber = cardIds.parentNumber;
            int level = cardIds.level;
            int number = cardIds.number;

            int order_number = 0;
            int.TryParse(deleteData_?.str, out order_number);

            RetSimple retSimple = new RetSimple();
            Info_Src info_Src = (from u in context.HeratigeUsers
                                 join hi in context.HerInfoSrcs on u.HeratigeUserId equals hi.HeratigeUserId
                                 join s in context.Info_Srcs on hi.HerInfoSrcId equals s.HerInfoSrcId
                                 where u.HeratigeUserId == heratigeUser_Id && u.user_id == user_Id &&
                                                hi.parent_level == parentLevel && hi.parent_number >= parentNumber &&
                                                hi.level == level && hi.number >= number && s.Id == order_number
                                 select s).FirstOrDefault();

            if (info_Src == null)
            {
                return new RetSimple { number = 0, success = false, message = "There was problem to delete item." };
            }

            int retInt = 0;
            context.Entry<Info_Src>(info_Src).State = EntityState.Deleted;

            try
            {
                retInt = context.SaveChanges();
                retSimple.retId = order_number;
                retSimple.success = true;
                retSimple.message = "" + order_number.ToString() + " item was deleted.";

                if (!string.IsNullOrWhiteSpace(info_Src.src))
                {
                    string path = folderPath + info_Src.src;
                    if (File.Exists(path))
                    {
                        try
                        {
                            File.Delete(path);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                            string methodName = st.GetFrame(0).GetMethod().Name;

                            string Msg = methodName + " ### " + ex.Message + " ### RetSimple DeleteHeritageLineFrom(DeleteData deleteData) Fail to delete FILE !!!!!";
                            if (ex.InnerException != null)
                                Msg += ex.InnerException.Message;

                            Helper helper = new Helper(_environment);
                            helper.WriteToLog(Msg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                string methodName = st.GetFrame(0).GetMethod().Name;

                string Msg = methodName + " ### " + ex.Message + " ### RetSimple DeleteImageComment(s_tr deleteData_, string folderPath)";
                if (ex.InnerException != null)
                    Msg += ex.InnerException.Message;

                Helper helper = new Helper(_environment);
                helper.WriteToLog(Msg);

                retSimple.success = false;
                retSimple.message = "There was problem to delete Item.";
            }

            return new RetSimple { number = retSimple.retId, success = true, message = "" + retSimple.retId.ToString() + " item was deleted." };
        }

        public RetSimple RemovePhoto(string card_id, string folderPath, string emptyPhoto)
        {
            //userId_heratigeUser_Id_parentLevel_parentNumber_level_number                            
            CardIds cardIds = GetLevelsAsIntList(card_id);
            int user_Id = cardIds.user_Id;
            int heratigeUser_Id = cardIds.heratigeUser_Id;
            int parentLevel = cardIds.parentLevel;
            int parentNumber = cardIds.parentNumber;
            int level = cardIds.level;
            int number = cardIds.number;

            RetSimple retSimple = new RetSimple();
            HeratigeData heratigeData = (from u in context.HeratigeUsers
                                         join hd in context.HeratigeDatas on u.HeratigeUserId equals hd.HeratigeUserId
                                         where u.HeratigeUserId == heratigeUser_Id && u.user_id == user_Id &&
                                                        hd.parent_level == parentLevel && hd.parent_number >= parentNumber &&
                                                        hd.level == level && hd.number >= number
                                         select hd).FirstOrDefault();

            if (heratigeData == null)
            {
                return new RetSimple { number = 0, success = false, message = "There was problem to remove photo." };
            }

            int retInt = 0;
            string ImagePath = heratigeData.ImagePath ?? "";
            heratigeData.ImagePath = emptyPhoto;

            try
            {
                retInt = context.SaveChanges();
                retSimple.success = true;
                retSimple.message = "Photo was removed.";

                if (!string.IsNullOrWhiteSpace(heratigeData.ImagePath))
                {
                    string path = folderPath + ImagePath;
                    if (File.Exists(path))
                    {
                        try
                        {
                            File.Delete(path);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                            string methodName = st.GetFrame(0).GetMethod().Name;

                            string Msg = methodName + " ### " + ex.Message + " ### RetSimple RemovePhoto(string card_id, string folderPath, string emptyPhoto) Fail to delete FILE !!!!!";
                            if (ex.InnerException != null)
                                Msg += ex.InnerException.Message;

                            Helper helper = new Helper(_environment);
                            helper.WriteToLog(Msg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                string methodName = st.GetFrame(0).GetMethod().Name;

                string Msg = methodName + " ### " + ex.Message + " ### RetSimple RemovePhoto(string card_id, string folderPath, string emptyPhoto)";
                if (ex.InnerException != null)
                    Msg += ex.InnerException.Message;

                Helper helper = new Helper(_environment);
                helper.WriteToLog(Msg);

                retSimple.success = false;
                retSimple.message = "There was problem to remove photo.";
            }

            return new RetSimple { number = retSimple.retId, success = true, message = "Photo was removed." };
        }
        #endregion


        #region Insert new objects
        public int InsertNewCountry(string name)
        {
            int retId = 0;

            Country country = new Country();
            country.Name = name;
            country.isActive = true;
            country.ShortName = name?.Substring(0, 3)?.ToUpper();

            //throw new Exception("Hey");

            context.Add<Country>(country);
            try
            {
                context.SaveChanges();
                retId = country.CountryId;
            }
            catch (Exception ex)
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                string methodName = st.GetFrame(0).GetMethod().Name;

                string Msg = methodName + " ### " + ex.Message + " ### InsertNewCountry(string name)";
                if (ex.InnerException != null)
                    Msg += ex.InnerException.Message;

                Helper helper = new Helper(_environment);
                helper.WriteToLog(Msg);
            }

            return retId;
        }

        public int InsertNewProvince(string name, int countryId)
        {
            int retId = 0;

            Province province = new Province();
            province.Name = name;
            province.isActive = true;
            province.country = context.Country.Where(t => t.CountryId == countryId).Select(t => t).FirstOrDefault();
            province.ShortName = name?.Substring(0, 3)?.ToUpper();

            //throw new Exception("Hey");

            context.Add<Province>(province);
            try
            {
                context.SaveChanges();
                retId = province.provinceId;
            }
            catch (Exception ex)
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                string methodName = st.GetFrame(0).GetMethod().Name;

                string Msg = methodName + " ### " + ex.Message + " ### InsertNewProvince(string name, int countryId)";
                if (ex.InnerException != null)
                    Msg += ex.InnerException.Message;

                Helper helper = new Helper(_environment);
                helper.WriteToLog(Msg);
            }

            return retId;
        }

        public int InsertNewCity(string name, int countryId, int provinceId)
        {
            int retId = 0;

            City city = new City();
            city.Name = name;
            city.isActive = true;
            city.country = context.Country.Where(t => t.CountryId == countryId).Select(t => t).FirstOrDefault();
            if (provinceId > 0)
            {
                city.province = context.Province.Where(t => t.provinceId == provinceId).Select(t => t).FirstOrDefault();
            }
            city.ShortName = name?.Substring(0, 3)?.ToUpper();

            //throw new Exception("Hey");

            context.Add<City>(city);
            try
            {
                context.SaveChanges();
                retId = city.cityId;
            }
            catch (Exception ex)
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                string methodName = st.GetFrame(0).GetMethod().Name;

                string Msg = methodName + " ### " + ex.Message + " ### InsertNewCountry(string name)";
                if (ex.InnerException != null)
                    Msg += ex.InnerException.Message;

                Helper helper = new Helper(_environment);
                helper.WriteToLog(Msg);
            }

            return retId;
        }

        public int InsertNewChurch(string name, int countryId, int provinceId, int cityId)
        {
            int retId = 0;

            Church church = new Church();
            church.Name = name;
            church.isActive = true;
            church.country = context.Country.Where(t => t.CountryId == countryId).Select(t => t).FirstOrDefault();
            if (provinceId > 0)
            {
                church.province = context.Province.Where(t => t.provinceId == provinceId).Select(t => t).FirstOrDefault();
            }
            if (cityId > 0)
            {
                church.city = context.City.Where(t => t.cityId == cityId).Select(t => t).FirstOrDefault();
            }
            church.ShortName = name?.Substring(0, 3)?.ToUpper();

            //throw new Exception("Hey");

            context.Add<Church>(church);
            try
            {
                context.SaveChanges();
                retId = church.churchId;
            }
            catch (Exception ex)
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                string methodName = st.GetFrame(0).GetMethod().Name;

                string Msg = methodName + " ### " + ex.Message + " ### InsertNewChurch(string name, int countryId, int provinceId, int cityId)";
                if (ex.InnerException != null)
                    Msg += ex.InnerException.Message;

                Helper helper = new Helper(_environment);
                helper.WriteToLog(Msg);
            }

            return retId;
        }
        #endregion

        #region Miscellanouse
        public HeritageViewModel ConvertDateTimes(HeritageViewModel heritageViewModel)
        {
            heritageViewModel.DOB = ConvertDataTime(heritageViewModel.DOB_str);

            heritageViewModel.PWD = ConvertDataTime(heritageViewModel.PWD_str);

            return heritageViewModel;
        }

        public CardIds GetLevelsAsIntList(string id)
        {
            CardIds cardIds = new CardIds();
            //List<int> levelsList = new List<int>();
            List<string> levelsList_ = id.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            int int_ = 0;
            int.TryParse(levelsList_[0], out int_);
            cardIds.user_Id = int_;

            int_ = 0;
            int.TryParse(levelsList_[1], out int_);
            cardIds.heratigeUser_Id = int_;

            int_ = 0;
            int.TryParse(levelsList_[2], out int_);
            cardIds.parentLevel = int_;

            int_ = 0;
            int.TryParse(levelsList_[3], out int_);
            cardIds.parentNumber = int_;

            int_ = 0;
            int.TryParse(levelsList_[4], out int_);
            cardIds.level = int_;

            int_ = 0;
            int.TryParse(levelsList_[5], out int_);
            cardIds.number = int_;



            //int user_Id = levelsList[0];
            //int heratigeUser_Id = levelsList[1];
            //int parentLevel = levelsList[2];
            //int parentNumber = levelsList[3];
            //int level = levelsList[4];
            //int number = levelsList[5];


            //foreach (var item in levelsList_)
            //{
            //    int int_ = 0;
            //    int.TryParse(item, out int_);
            //    levelsList.Add(int_);
            //}

            //return levelsList;
            return cardIds;
        }


        public AppSettings GetAppSettings(IConfiguration _config)
        {
            AppSettings retVal = new AppSettings();
            _config.GetSection("Data:AppSettings").Bind(retVal);

            return retVal;
        }
        #endregion


        #region Private Methods
        void Rotate(Bitmap bmp)
        {
            PropertyItem pi = bmp.PropertyItems.Select(x => x)
                                               .FirstOrDefault(x => x.Id == 0x0112);
            if (pi == null) return;

            byte o = pi.Value[0];

            if (o == 2) bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            if (o == 3) bmp.RotateFlip(RotateFlipType.RotateNoneFlipXY);
            if (o == 4) bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            if (o == 5) bmp.RotateFlip(RotateFlipType.Rotate90FlipX);
            if (o == 6) bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            if (o == 7) bmp.RotateFlip(RotateFlipType.Rotate90FlipY);
            if (o == 8) bmp.RotateFlip(RotateFlipType.Rotate90FlipXY);
        }

        private HerRepData resizeImageStrict(Image imgToResize, Size size, Size sizeOld, ImageModificationsType imageModificationsType)
        {
            if (imageModificationsType == ImageModificationsType.WithGivenWidth)
            {
                return resizeImageWithGivenWidth(imgToResize, size, sizeOld);
            }
            else
            {
                return resizeImageInDivCenter(imgToResize, size, sizeOld);
            }
        }

        private HerRepData resizeImageWithGivenWidth(Image imgToResize, Size size, Size sizeOld)
        {
            HerRepData herRepData = new HerRepData();
            Size newSize = new Size { Height = 0, Width = 0 };
            decimal ratioOld = (decimal)((decimal)sizeOld.Height / (decimal)sizeOld.Width);
            newSize.Width = size.Width;
            newSize.Height = (int)(((decimal)(size.Width) / (decimal)(sizeOld.Width)) * sizeOld.Height);
            herRepData.size = newSize;
            herRepData.image = (Image)(new Bitmap(imgToResize, newSize));

            return herRepData;
        }

        private HerRepData resizeImageInDivCenter(Image imgToResize, Size size, Size sizeOld)
        {
            HerRepData herRepData = new HerRepData();
            Size newSize = new Size { Height = 0, Width = 0 };
            Size newSizeTooltip = new Size { Height = 0, Width = 0 };
            decimal ratioOld = (decimal)((decimal)sizeOld.Height / (decimal)sizeOld.Width);
            decimal ratio = (decimal)((decimal)size.Height / (decimal)size.Width);

            if (ratio > ratioOld)
            {
                newSize.Width = size.Width;
                newSize.Height = (int)(((decimal)(size.Width) / (decimal)(sizeOld.Width)) * sizeOld.Height);
            }
            else if (ratio < ratioOld)
            {
                newSize.Height = size.Height;
                newSize.Width = (int)(((decimal)(size.Height) / (decimal)(sizeOld.Height)) * sizeOld.Width);
            }
            else
            {
                herRepData.image = imgToResize;
                herRepData.size = size;

                return herRepData;
            }

            newSizeTooltip.Width = size.Width * 5;
            newSizeTooltip.Height = newSize.Height * 5;
            herRepData.size = newSize;
            herRepData.image = (Image)(new Bitmap(imgToResize, newSizeTooltip));

            return herRepData;
        }


        private Image resizeImage(Image imgToResize, Size size, Size sizeOld)
        {
            //int width = (int)(((decimal)(sizeOld.Width) / (decimal)(sizeOld.Height)) * size.Height);
            //size.Width = width;
            return (Image)(new Bitmap(imgToResize, size));
        }

        private Stream ConvertImage(Image image, ImageFormat format)
        {
            //var image = Image.FromStream(originalStream);

            var stream = new MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }

        private Stream ConvertImage1(Stream originalStream, ImageFormat format)
        {
            var image = Image.FromStream(originalStream);

            var stream = new MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }

        private string ChangeFileNameExt(string path, int width, int height, string ext)
        {
            string fn = Path.GetFileNameWithoutExtension(path);
            string newStr = path.Replace(fn, fn + "_" + width.ToString() + "_" + height.ToString());
            newStr = newStr.Replace(@"\Temp", "");
            string retStr = Path.ChangeExtension(newStr, "." + ext);

            return retStr;
        }

        private string ChangeFileNameExtStrict(string path, int width, int height, string folder, string ext)
        {
            string fn = Path.GetFileNameWithoutExtension(path);
            string newStr = path.Replace(fn, fn + "_" + width.ToString() + "_" + height.ToString());
            newStr = newStr.Replace(@"\Temp", @"\Photos\" + folder);
            Directory.CreateDirectory(Path.GetDirectoryName(newStr));
            string retStr = Path.ChangeExtension(newStr, "." + ext);

            return retStr;
        }

        private RetHerClasses CheckHerExistance(her heritage)
        {
            RetHerClasses retVal = new RetHerClasses();
            retVal.HeratigeData = (from u in context.HeratigeUsers
                                   join d in context.HeratigeDatas on u.HeratigeUserId equals d.HeratigeUserId
                                   where u.user_id == heritage.user_id && u.HeratigeUserId == heritage.heratigeUser_Id && d.level == heritage.level
                                          && d.number == heritage.number && d.parent_number == heritage.parent_number && d.parent_level == heritage.parent_level
                                   select d).FirstOrDefault();
            retVal.heratigeUser = context.HeratigeUsers.Where(t => t.HeratigeUserId == heritage.heratigeUser_Id && t.user_id == heritage.user_id).FirstOrDefault();

            return retVal;
        }

        private DateTime? ConvertDataTime(string dOB_str)
        {
            DateTime? dt = null;
            try
            {
                DateTime dateTime = DateTime.ParseExact(dOB_str, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dt = dateTime;
            }
            catch (Exception ex)
            {
            }

            return dt;
        }

        private List<List<her>> GetArrList(List<her> list)
        {
            List<List<her>> retList = new List<List<her>>();
            List<int> levelList = list.Select(t => t.level).Distinct().ToList();
            her her_0 = list.Where(d => d.level == 0 && d.number == 0 && d.parent_level == 0 &&
                                    d.parent_number == 0).FirstOrDefault();
            retList.Add(new List<her> { her_0 });
            list.Remove(her_0);

            if (levelList != null && levelList.Count > 0)
            {
                levelList.Remove(0);
                foreach (var iLevel in levelList)
                {
                    List<her> listItem = list.Where(t => t.level == iLevel).ToList();
                    retList.Add(listItem.OrderBy(t => t.number).ToList());
                }
            }

            return retList;
        }

        private List<her> GetUpdatedHerList(List<her> listHer)
        {
            List<her> retList = new List<her>();
            foreach (var item in listHer)
            {
                string herStr = JsonConvert.SerializeObject(item);
                item.herString = herStr;

                item.her_birthDate = item.birthDate == null ? null : item.birthDate.Value.Year.ToString();
                item.her_endDate = item.endDate == null ? null : item.endDate.Value.Year.ToString();
                item.birthDate = null;
                item.endDate = null;
                if (!string.IsNullOrWhiteSpace(item.flagImgPath))
                {
                    item.countryName = "Country person is living: " + item.countryName;
                }
                else
                {
                    item.countryName = "";
                }

                if (!string.IsNullOrWhiteSpace(item.flagImgPathBP))
                {
                    item.countryNameBP = "County person was born: " + item.countryNameBP;
                }
                else
                {
                    item.countryNameBP = "";
                }

                retList.Add(item);
            }


            return retList;
        }

        private AllTextBoxIds GetAllTextBoxIds(string allTextBoxIds)
        {
            //countryId_provinceId_cityId_curchId_countryBPId_provinceBPId_cityBPId_userId_heratigeUserId
            AllTextBoxIds all = new AllTextBoxIds();
            List<string> idList = allTextBoxIds.Split(new string[] { @"_" }, StringSplitOptions.None).ToList();

            int int_ = 0;
            int.TryParse(idList[0], out int_);
            all.countryId = int_;

            int.TryParse(idList[1], out int_);
            all.provinceId = int_;

            int.TryParse(idList[2], out int_);
            all.cityId = int_;

            int.TryParse(idList[3], out int_);
            all.churchId = int_;

            int.TryParse(idList[4], out int_);
            all.countryBPId = int_;

            int.TryParse(idList[5], out int_);
            all.provinceBPId = int_;

            int.TryParse(idList[6], out int_);
            all.cityBPId = int_;

            int.TryParse(idList[7], out int_);
            all.user_id = int_;

            int.TryParse(idList[8], out int_);
            all.heratigeUser_Id = int_;

            return all;

        }
        #endregion





    }
}
